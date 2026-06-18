Option Strict Off
Option Explicit On
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date:
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a ACTCommissionPayments.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' *****************************************************************
    ' Added to replace global variables
    ' Username.
    Private m_sUsername As String

    ' Password.
    Private m_sPassword As String

    ' User ID
    Private m_iUserID As Short

    ' Calling Application
    Private m_sCallingAppName As String

    ' Source ID
    Private m_iSourceID As Short

    ' Language ID
    Private m_iLanguageID As Short

    ' Currency ID
    Private m_iCurrencyID As Short

    ' LogLevel
    Private m_iLogLevel As Short
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    'RiskData object
    Private m_oDataSet As cGISDataSetControl.Application

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_lError As Integer
    Private m_oCurrencyConvert As Object

    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            PMProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Short, ByRef iSourceID As Short, ByRef iLanguageID As Short, ByRef iCurrencyID As Short, ByRef iLogLevel As Short, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer
        Dim cGISDataSetControl As Object

        Dim sValue As String

        Try

            Initialise = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            'sj 16/12/2002 - start
            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = PMTransactionTypeGeneric

            'sj 16/12/2002 - end

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Initialise = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_oDataSet = New cGISDataSetControl.Application

            ' Currency Convert
            m_lReturn = GetBusinessObject(m_oCurrencyConvert, "bACTCurrencyConvert.Form")
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", Initialise, Failed to create instance of bACTCurrencyConvert.Form")
            End If

            Exit Function

        Catch ex As Exception

            Initialise = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function

        End Try
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
            Me.disposedValue = True
            If disposing Then
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub




    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Try

            BeginTrans = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLBeginTrans

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                BeginTrans = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            BeginTrans = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Try



            CommitTrans = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLCommitTrans

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CommitTrans = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception
            CommitTrans = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Try

            RollbackTrans = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLRollbackTrans

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RollbackTrans = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function

        Catch ex As Exception

            RollbackTrans = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

            Exit Function
        End Try
    End Function


    Private Sub Class_Initialize_Renamed()


        ' Class Initialise

        Exit Sub


    End Sub
    Public Sub New()
        MyBase.New()
        Class_Initialize_Renamed()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ' ***************************************************************** '
    '
    ' Name: CreateBusinessObject
    '
    ' Description:
    '
    ' History: 16/02/2001 RWH - Created.
    '
    ' ***************************************************************** '
    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer



        CreateBusinessObject = gPMConstants.PMEReturnCode.PMTrue


        CreateBusinessObject = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Short) As Integer

        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As Integer

        Try



            AddInputParameter = gPMConstants.PMEReturnCode.PMTrue

            ' Add Parameter to database object

            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, " Failed to add parameter name:" & v_sName & ", values :" & v_vValue & ", type:" & v_iType, gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=AddInputParameter, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: PrepareAgentSummary (Public)
    '
    ' Description: search Commission Payments on the basis of selected Criteria
    '
    '****************************************************************** '
    Public Function PrepareAgentSummary(ByVal v_dTransDateFrom As Date, ByVal v_dTransDateTo As Date, ByVal v_iCurrencyID As Short, ByVal v_lProductID As Integer, ByVal v_lCompanyId As Integer, ByVal v_lUserId As Integer, ByVal v_iOnlyAuthorityLimit As Short, ByVal v_sAgentId As String, ByRef r_sSessionGUID As String, ByRef r_vResultArray(,) As Object) As Integer


        Const kMethodName As String = "PrepareAgentSummary"
        Try
            PrepareAgentSummary = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_date_from", vValue:=v_dTransDateFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter transaction_date_from")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_date_to", vValue:=v_dTransDateTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter transaction_date_to")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=v_iCurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter currency_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter product_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=v_lCompanyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter company_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=v_lUserId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter user_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="only_within_authority_limit", vValue:=v_iOnlyAuthorityLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter only_within_authority_limit")
                Exit Function
            End If

            v_sAgentId = "(" & v_sAgentId & ")"


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Agents", vValue:=v_sAgentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter AgentsAccountId")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="session_guid", vValue:=r_sSessionGUID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter session_guid")
                Exit Function
            End If

            'Execute SQL Statement

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACPrepareAgentSummaryCodeSQL, sSQLName:=ACPrepareAgentSummaryCodeName, bStoredProcedure:=ACPrepareAgentSummary, vResultArray:=r_vResultArray)

            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Exit Function
            End If


            r_sSessionGUID = ToSafeString(m_oDatabase.Parameters.Item("session_guid").Value)


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PrepareAgentSummary, v_sUsername:=m_sUsername, excep:=ex)
        Finally


        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: PrepareAgentSummary (Public)
    '
    ' Description: search Commission Payments on the basis of selected Criteria
    '
    '****************************************************************** '
    Public Function PrepareAgentSummaryForAllocatedTrans(ByVal v_dTransDateFrom As Date, ByVal v_dTransDateTo As Date, ByVal v_iCurrencyID As Short, ByVal v_lProductID As Integer, ByVal v_lCompanyId As Integer, ByVal v_lUserId As Integer, ByVal v_iOnlyAuthorityLimit As Short, ByVal v_sAgentId As String, ByRef r_sSessionGUID As String, ByRef r_vResultArray(,) As Object) As Integer


        Const kMethodName As String = "PrepareAgentSummary"
        Try
            PrepareAgentSummaryForAllocatedTrans = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="allocation_date_from", vValue:=v_dTransDateFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter transaction_date_from")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="allocation_date_to", vValue:=v_dTransDateTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter transaction_date_to")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=v_iCurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter currency_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter product_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=v_lCompanyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter company_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=v_lUserId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter user_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="only_within_authority_limit", vValue:=v_iOnlyAuthorityLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter only_within_authority_limit")
                Exit Function
            End If

            v_sAgentId = "(" & v_sAgentId & ")"


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Agents", vValue:=v_sAgentId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter AgentsAccountId")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="session_guid", vValue:=r_sSessionGUID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter session_guid")
                Exit Function
            End If

            'Execute SQL Statement

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACPrepareAgentSummaryAllocatedTransCodeSQL, sSQLName:=ACPrepareAgentSummaryAllocatedTransCodeName, bStoredProcedure:=ACPrepareAgentSummary, vResultArray:=r_vResultArray)

            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Exit Function
            End If


            r_sSessionGUID = ToSafeString(m_oDatabase.Parameters.Item("session_guid").Value)


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=PrepareAgentSummaryForAllocatedTrans, v_sUsername:=m_sUsername, excep:=ex)
        Finally


        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: MarkCommissionPayments (Public)
    '
    ' Description: Mark Commission Payments
    '
    '****************************************************************** '
    Public Function MarkCommissionPayments(ByVal v_lUserId As Integer, ByVal v_sSession_Guid As String, ByVal v_vSelectedAccounts As Object, ByVal v_dStatementDate As Date, ByRef r_lBatchId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Const kMethodName As String = "MarkCommissionPayments"
        Dim iCount As Integer
        Dim bUpdFlag As Boolean

        Try


            MarkCommissionPayments = gPMConstants.PMEReturnCode.PMTrue

            ' 1st Step is to Creare Commission Payments Batch
            ' 2nd Step is to Mark the Commission Payments In Batch with Batch ID retrived from STEP 1

            ' STEP - 1
            If r_lBatchId = 0 Then
                'Clear the Database Parameters Collection

                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=v_lUserId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter user_id")
                    Exit Function
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=r_lBatchId, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter batch_id")
                    Exit Function
                End If

                'Execute SQL Statement

                m_lError = m_oDatabase.SQLSelect(sSQL:=ACCreateCommPaymentsBatchSQL, sSQLName:=ACCreateCommPaymentsBatchName, bStoredProcedure:=ACCreateCommPaymentsBatch, vResultArray:=r_vResultArray)

                If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                    'Raise Error
                    RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                    Exit Function
                End If


                r_lBatchId = ToSafeLong(m_oDatabase.Parameters.Item("batch_id").Value)

            End If

            ' STEP - 2
            '   Loop through all the selected v_vSelectedAccounts
            For iCount = 0 To UBound(v_vSelectedAccounts)
                'Clear the Database Parameters Collection

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="account_Id", vValue:=v_vSelectedAccounts(iCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter account_Id")
                    Exit Function
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="session_guid", vValue:=v_sSession_Guid, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter session_guid")
                    Exit Function
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=Trim(CStr(r_lBatchId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter batch_id")
                    Exit Function
                End If

                If iCount = 0 Then
                    bUpdFlag = 1
                Else
                    bUpdFlag = 0
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="bUpdFlag", vValue:=ToSafeBoolean(bUpdFlag), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter bUpdFlag")
                    Exit Function
                End If

                'Execute SQL Statement

                m_lError = m_oDatabase.SQLAction(sSQL:=ACMarkCommissionPaymentsInBatchSQL, sSQLName:=ACMarkCommissionPaymentsInBatchName, bStoredProcedure:=ACMarkCommissionPaymentsInBatch)

                If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                    'Raise Error
                    RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                    Exit Function
                End If
            Next

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=MarkCommissionPayments, v_sUsername:=m_sUsername, excep:=ex)
        Finally

        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: GenerateCommissionPayments (Public)
    '
    ' Description: Call Generate Commission to Generate Cash List Item and Allocate them to the Commission Credits.
    '               Any outstanding currency differences will be written off to the standard write off accounts
    '
    '****************************************************************** '

    Public Function GenerateCommissionPayments(ByVal v_lBatchID As Integer, ByVal v_dStatementDate As Date) As Integer

        Const kMethodName As String = "GenerateCommissionPayments"
        Dim vResultarray(,) As Object
        Try


            GenerateCommissionPayments = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=v_lBatchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter batch_id")
                Exit Function
            End If

            'Execute SQL Statement

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetChosenCommissionPaymentsSQL, sSQLName:=ACGetChosenCommissionPaymentsName, bStoredProcedure:=ACGetChosenCommissionPayments, vResultArray:=vResultarray)

            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Exit Function
            End If

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GenerateCommissionPayments, v_sUsername:=m_sUsername, excep:=ex)
        Finally


        End Try
        Exit Function
    End Function

    ' *******************************************************************************
    ' PUBLIC METHODS
    ' *******************************************************************************

    ' Format a value as currency via the currency convert object.

    Public Function FormatCurrency_Renamed(ByVal v_lCurrencyID As Integer, ByVal v_cCurrencyAmount As Decimal, Optional ByVal v_dtConversionDate As Date = #12:00:00 AM#) As String

        Dim sFormattedCurrency As String

        Try

            ' Pass through to format object

            FormatCurrency_Renamed = m_oCurrencyConvert.FormatCurrency(vCurrencyID:=v_lCurrencyID, vCurrencyAmount:=v_cCurrencyAmount, vFormattedCurrency:=sFormattedCurrency, vConversionDate:=v_dtConversionDate)

            ' Return the formatted currency
            FormatCurrency_Renamed = sFormattedCurrency

            Exit Function

        Catch ex As Exception
            ' Log Error Message (and return empty as result)
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FormatCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FormatCurrency", vErrNo:=Err.Number, vErrDesc:=Err.Description, excep:=ex)

        End Try
    End Function

    ' *******************************************************************************
    ' PRIVATE METHODS
    ' *******************************************************************************
    ' Macro function for loading business objects

    Private Function GetBusinessObject(ByRef r_oBusiness As Object, ByVal v_sName As String, Optional ByVal v_bForceRefresh As Boolean = False) As Integer


        GetBusinessObject = gPMConstants.PMEReturnCode.PMTrue

        ' Check if object is already set or we are forcing
        If (r_oBusiness Is Nothing) Or v_bForceRefresh Then
            ' Create new instance
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oBusiness, v_sClassName:=v_sName, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel)

            ' Check return code
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                GetBusinessObject = gPMConstants.PMEReturnCode.PMFalse
            End If

        End If



    End Function

    '****************************************************************** '
    ' Name: GetDocumentsForAccountBatch (Public)
    '
    ' Description:
    '
    '****************************************************************** '
    Public Function GetDocumentsForAccountBatch(ByVal v_lAccountId As Integer, ByVal v_lBatchID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Const kMethodName As String = "GetDocumentsForAccountBatch"
        Try

            GetDocumentsForAccountBatch = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=v_lAccountId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter account_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=v_lBatchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter account_id")
                Exit Function
            End If

            'Execute SQL Statement

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentsForAccountBatchSQL, sSQLName:=ACGetDocumentsForAccountBatchName, bStoredProcedure:=ACGetDocumentsForAccountBatch, vResultArray:=r_vResultArray)

            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Exit Function
            End If

        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetDocumentsForAccountBatch, v_sUsername:=m_sUsername, excep:=ex)
        Finally


        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: GetAgentDetailsforPayments (Public)
    '
    ' Description: Get Agent Details for the Payments
    '
    '****************************************************************** '
    Public Function GetAgentDetailsforPayments(ByVal v_lAccountId As Integer, ByVal v_lSourceID As Integer, ByRef r_vResultArray(,) As Object) As Integer


        Const kMethodName As String = "GetAgentDetailsforPayments"
        Try

            GetAgentDetailsforPayments = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=v_lAccountId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter account_id")
                Exit Function
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_lSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter account_id")
                Exit Function
            End If

            'Execute SQL Statement

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetAgentDetailsforPaymentsSQL, sSQLName:=ACGetAgentDetailsforPaymentsName, bStoredProcedure:=ACGetAgentDetailsforPayments, vResultArray:=r_vResultArray)

            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Exit Function
            End If

        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetAgentDetailsforPayments, v_sUsername:=m_sUsername, excep:=ex)
        Finally

        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: RemoveCommissionPaymentsBatch (Public)
    '
    ' Description:
    '
    '****************************************************************** '
    Public Function RemoveCommissionPaymentsBatch(ByVal v_lBatchID As Integer) As Integer


        Const kMethodName As String = "RemoveCommissionPaymentsBatch"
        Try


            RemoveCommissionPaymentsBatch = gPMConstants.PMEReturnCode.PMTrue

            If ToSafeLong(v_lBatchID) > 0 Then

                'Clear the Database Parameters Collection

                m_oDatabase.Parameters.Clear()


                m_lReturn = m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=v_lBatchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    RaiseError(kMethodName, "Failed to Add parameter batch_id")
                    Exit Function
                End If

                'Execute SQL Statement

                m_lError = m_oDatabase.SQLAction(sSQL:=ACRemoveCommissionPaymentsBatchSQL, sSQLName:=ACRemoveCommissionPaymentsBatchName, bStoredProcedure:=ACRemoveCommissionPaymentsBatch)

                If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                    'Raise Error
                    RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                    Exit Function
                End If
            End If
        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RemoveCommissionPaymentsBatch, v_sUsername:=m_sUsername, excep:=ex)
        Finally

        End Try
        Exit Function
    End Function

    '****************************************************************** '
    ' Name: GetShortNameForParty (Public)
    '
    ' Description: GetShortNameForParty
    '
    '****************************************************************** '
    Public Function GetShortNameForParty(ByVal v_lPartyCnt As Integer, ByRef r_sPartyShortName As String) As Integer

        Dim vResultarray(,) As Object

        Const kMethodName As String = "GetShortNameForParty"
        Try


            GetShortNameForParty = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="partycnt", vValue:=v_lPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter party_cnt")
            End If


            'Execute SQL Statement

            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetPartyShortnameSQL, sSQLName:=ACGetPartyShortnameName, bStoredProcedure:=ACGetPartyShortname, vResultArray:=vResultarray)

            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                'Raise Error
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If

            If IsArray(vResultarray) Then
                r_sPartyShortName = Trim(ToSafeString(vResultarray(0, 0)))
            End If
        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetShortNameForParty, v_sUsername:=m_sUsername, excep:=ex)
        Finally

        End Try
        Exit Function
    End Function


    Public Function SaveSelection(ByVal v_vCriteriaFields As Object) As Integer

        Const kMethodName As String = "SaveSelection"
        Dim lWorkTaskID As Integer
        Dim lTaskGroupID As Integer
        Dim vResultarray As Object
        Dim oWrkTaskInstance As bPMWrkTaskInstance.TaskControl
        Try


            SaveSelection = gPMConstants.PMEReturnCode.PMTrue
            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl
            m_lReturn = oWrkTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "bPMWrkTaskInstance.TaskControl GetInstance failed.")
            End If

            'GET THE TASK ID
            'Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_code", vValue:="COMMPAY", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter task_code")
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="task_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter task_id")
            End If
            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetWorkTaskIDSQL, sSQLName:=ACGetWorkTaskIDName, bStoredProcedure:=ACGetWorkTaskIDStored)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If


            lWorkTaskID = ToSafeLong(m_oDatabase.Parameters.Item("task_id").Value)

            'ACGetWorkTaskGroupIDSQL
            m_lReturn = GetTaskGroupID(v_lWorkTaskID:=lWorkTaskID, r_lTaskGroupID:=lTaskGroupID)


            'Create the WorkManager Task

            m_lReturn = oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=lTaskGroupID, v_lPMWrkTaskID:=lWorkTaskID, v_sCustomer:="System", v_dtTaskDueDate:=Today, v_sDescription:="Saved Commission Payments Criteria for " & m_sUsername, v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=0, r_lPMWrkTaskInstanceCnt:=0, v_iUserID:=m_iUserID, v_vKeyArray:=v_vCriteriaFields, v_lPMUserGroupID:=1)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "bPMWrkTaskInstance.TaskControl GetInstance failed.")
            End If

        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SaveSelection, v_sUsername:=m_sUsername, excep:=ex)
        Finally

            'oWrkTaskInstance.Terminate()
            oWrkTaskInstance.Dispose()

            oWrkTaskInstance = Nothing

        End Try
        Exit Function
    End Function


    Private Function GetTaskGroupID(ByVal v_lWorkTaskID As Integer, ByRef r_lTaskGroupID As Integer) As Integer

        Dim vResultarray(,) As Object
        Const kMethodName As String = "GetTaskGroupID"


        GetTaskGroupID = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=v_lWorkTaskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError(kMethodName, "Failed to Add parameter WorkTaskID")
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Group_code", vValue:="SLACS", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError(kMethodName, "Failed to Add parameter Group_code")
        End If
        'Execute SQL Statement

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetWorkTaskGroupIDSQL, sSQLName:=ACGetWorkTaskGroupIDName, bStoredProcedure:=ACGetWorkTaskGroupIDStored, vResultArray:=vResultarray)
        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
        End If
        If IsArray(vResultarray) Then
            r_lTaskGroupID = ToSafeLong(vResultarray(0, 0))
        End If
        If Not IsArray(vResultarray) Or r_lTaskGroupID = 0 Then

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pmwrk_task_id", vValue:=v_lWorkTaskID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to Add parameter WorkTaskID")
            End If
            'Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetWorkTaskGroupIDSQL, sSQLName:=ACGetWorkTaskGroupIDName, bStoredProcedure:=ACGetWorkTaskGroupIDStored, vResultArray:=vResultarray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
            End If
            If IsArray(vResultarray) Then
                r_lTaskGroupID = ToSafeLong(vResultarray(0, 0))
            End If

        End If

        Exit Function
    End Function
End Class
