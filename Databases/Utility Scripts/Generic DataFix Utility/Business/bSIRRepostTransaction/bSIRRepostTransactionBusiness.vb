Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Text
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public Class Business
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 19/11/2003
    '
    ' Description: Repost to orion
    '
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"
	
	' PUBLIC Data Members (Begin)
	' PUBLIC Data Members (End)
	
	
	' PRIVATE Data Members (Begin)
	
	' Database Class (Private)
	Private m_oDatabase As dPMDAO.Database
	
	' Close Database Flag (Private)
	Private m_bCloseDatabase As Boolean
	
	' Error Code (Private)
	Private m_lReturn As Integer
	
	' Process Mode Properties
	Private m_iTask As Integer
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_sTransactionType As String = ""
	Private m_dtEffectiveDate As Date
	Private m_bAnyFailed As Boolean
	
	
	Private m_oPMBTransactions As Object
	Private m_oReinsurance As Object
	Private m_oChangeClaimStatus As Object
	Private m_oFindClaim As Object
	Private m_oLock As Object
	Private m_oClaimTrans As Object
	
	'claim peril array position
	Const ACClaimPerilPolicyID As Integer = 0
	Const ACClaimPerilPolicyNumber As Integer = 1
	Const ACClaimPerilClaimNumber As Integer = 2
	Const ACClaimPerilClaimPerilID As Integer = 3
	Const ACClaimPerilClassOfBusinessID As Integer = 4
	Const ACClaimPerilClassOfBusinessCode As Integer = 5
	Const ACClaimPerilTotalThisRevision As Integer = 6
	Const ACClaimPerilTotalThisPayment As Integer = 7
	Const ACClaimPerilPartyCnt As Integer = 8
	
	
	' Username.
	Private m_sUsername As String = ""
	
	' Password.
	Private m_sPassword As String = ""
	
	' User ID
	Public m_iUserID As Integer
	
	' Calling Application
	Public m_sCallingAppName As String = ""
	' Source ID
	Public m_iSourceID As Integer
	' Language ID
	Public m_iLanguageID As Integer
	' Currency ID
	Public m_iCurrencyID As Integer
	' LogLevel
	Public m_iLogLevel As Integer

    Public Const PMAllRecords As Integer = -1
	' PRIVATE Data Members (End)

    Public Const kTaxTypeArrayPosCode As Integer = 1
    Public Const kTaxTypeArrayPosTaxAmount As Integer = 2
    Public Const kClaimDetailPostClaimsTaxes As Integer = 11

    Private m_lInsuranceFileCnt As Integer
    Private m_bIsCloned As Boolean
    Private m_sNextOrionDocRef As String = ""
    Private m_bIsPT As Boolean
    Private m_sMessage As String = ""
    Public Const kSystemOptionRoundOffAccount As Integer = 5080
    Public Const kSystemOptionCreditControlEnabled As Integer = 5001
    Public Const kSystemOptionChaseCycleEnabled As Integer = 5096
    Private Const ACLockName As String = "CashDepositAccount"
    Private m_bByPassLocking As Boolean
    Private m_nClonedReversalDocumentID As Integer
    Private m_dtTransferDate As Date
    Private m_lClonedInsuranceFileCnt As Long
    Private m_bReverseCloned As Boolean
    Private m_bBackDateMTA As Boolean
    Private m_bIsSDDTransaction As Boolean
    Public m_sDocumentRef As String
    Private m_IsClonedReversal As Integer
    Private m_sTransactionTypeCode As String
    Private m_lTransactionTypeID As Integer
    Private m_lClaimID As Integer

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property
    Public Property IsCloned() As Boolean
        Get
            Return m_bIsCloned
        End Get
        Set(value As Boolean)
            m_bIsCloned = value
        End Set
    End Property
    Public WriteOnly Property NextOrionDocRef() As String
        Set(ByVal Value As String)
            m_sNextOrionDocRef = Value
        End Set
    End Property

    Public Property IsPT() As Boolean
        Get
            Return m_bIsPT
        End Get
        Set(value As Boolean)
            m_bIsPT = value
        End Set
    End Property
    Public Property Message() As String
        Get
            Return m_sMessage
        End Get
        Set(ByVal Value As String)
            m_sMessage = Value
        End Set
    End Property

    Public Property ByPassLocking() As Boolean
        Get
            Return m_bByPassLocking
        End Get
        Set(ByVal Value As Boolean)
            m_bByPassLocking = Value
        End Set
    End Property
    Public Property TransferDate() As DateTime
        Get
            Return m_dtTransferDate
        End Get
        Set(value As DateTime)
            m_dtTransferDate = value
        End Set
    End Property

    Public Property ClonedInsuranceFileCnt() As Integer
        Get
            Return m_lClonedInsuranceFileCnt
        End Get
        Set(value As Integer)
            m_lClonedInsuranceFileCnt = value
        End Set
    End Property

    Public Property ReverseCloned() As Boolean
        Get
            Return m_bReverseCloned
        End Get
        Set(value As Boolean)
            m_bReverseCloned = value
        End Set
    End Property

    Public Property BackDateMTA() As Boolean
        Get
            Return m_bBackDateMTA
        End Get
        Set(value As Boolean)
            m_bBackDateMTA = value
        End Set
    End Property

    Public Property IsSDDTransaction() As Boolean
        Get
            Return m_bIsSDDTransaction
        End Get
        Set(value As Boolean)
            m_bIsSDDTransaction = value
        End Set
    End Property

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

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

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oPMBTransactions, v_sClassName:="bPMBTransactions.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bPMBTransactions.Automated", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the process modes

            m_lReturn = m_oPMBTransactions.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Terminate() As Integer

        Dim result As Integer = 0
        Static bTerminated As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If we have already Terminated then exit

            If Not (m_oPMBTransactions Is Nothing) Then

                m_lReturn = m_oPMBTransactions.Terminate()
                m_oPMBTransactions = Nothing
            End If

            If Not (m_oReinsurance Is Nothing) Then

                m_lReturn = m_oReinsurance.Terminate()
                m_oReinsurance = Nothing
            End If

            If Not (m_oFindClaim Is Nothing) Then

                m_lReturn = m_oFindClaim.Terminate()
                m_oFindClaim = Nothing
            End If

            If Not (m_oChangeClaimStatus Is Nothing) Then

                m_lReturn = m_oChangeClaimStatus.Terminate()
                m_oChangeClaimStatus = Nothing
            End If

            If Not (m_oLock Is Nothing) Then

                m_lReturn = m_oLock.Terminate()
                m_oLock = Nothing
            End If

            If Not (m_oClaimTrans Is Nothing) Then

                m_lReturn = m_oClaimTrans.Terminate()
                m_oClaimTrans = Nothing
            End If

            If bTerminated Then
                Return result
            Else
                bTerminated = True
            End If

            ' If this class opened the database, close it
            If m_bCloseDatabase Then
                ' Close the Database
                m_lReturn = m_oDatabase.CloseDatabase()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Release reference to PM Data Access Object
            m_oDatabase = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Terminate", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
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

                m_lProcessMode = CInt(vProcessMode)
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
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()

        Try

            ' Class Terminate

            ' Call Terminate Method in case Calling Object
            ' has forgotten to.
            m_lReturn = Terminate()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Terminate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Terminate", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    ' Name : SendToOrion (Private)
    '
    ' Desc : post data to orion
    '        'p' pending, 's' sent 'f' failed 'c' completed
    ' Hist : 19/11/2003 Created - Tinny
    ' ***************************************************************** '
    Public Function SendToOrion(ByVal v_lTransactionExportFolderCnt As Integer, Optional ByRef r_lDocumentId As Integer = 0) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Call the function to Send the Transaction to Orion

            m_lReturn = m_oPMBTransactions.SendToOrion(v_lTransactionExportFolderCnt, r_lDocumentId:=r_lDocumentId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SendToOrion Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="SendToOrion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name : UpdateExportStatus (Private)
    '
    ' Desc : update transaction export folder
    '
    ' Hist : 19/11/2003 Created - Tinny
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateExportStatus) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateExportStatus(ByVal v_lTransExportFolderCnt As Integer, ByVal v_sStatus As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="accounts_export_status", vValue:=v_sStatus, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransExportFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdTransExportFolderSQL, sSQLName:=ACUpdTransExportFolderName, bStoredProcedure:=ACUpdTransExportFolderStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    'Return result
    'End If
    '
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateExportStatus Failed ", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateExportStatus", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    '*********************************************************************************************
    ' Name : GetFailedTransaction
    '
    ' Desc : get a list of transaction in transaction export folder with status = 'f'
    '
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function GetFailedTransaction(ByRef r_vResult(,) As Object, Optional ByVal v_lExcludeOtherDoc As Integer = 1) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.QueryTimeout = 7200

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ExcludeOtherDoc", vValue:=CStr(v_lExcludeOtherDoc), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACGetFailedTransactionsSQL, sSQLName:=ACGetFailedTransactionsName, bStoredProcedure:=ACGetFailedTransactionsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResult)

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFailedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : DeleteTransactionExport
    '
    ' Desc : delete from transaction export detail and folder for this insurance file cnt
    '
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function DeleteTransactionExport(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=IIf(v_sDocumentRef = "", DBNull.Value, v_sDocumentRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return m_oDatabase.SQLAction(sSQL:=ACDeleteTransactionExportSQL, sSQLName:=ACDeleteTransactionExportName, bStoredProcedure:=ACDeleteTransactionExportStored)

    End Function

    '*********************************************************************************************
    ' Name : DeleteStatsFolder
    '
    ' Desc : delete stats folder for this insurance file cnt
    '        this will also delete stats detail and transaction export folder and detail
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function DeleteStatsFolder(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=IIf(v_sDocumentRef = "", DBNull.Value, v_sDocumentRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_oDatabase.SQLAction(sSQL:=ACDeleteStatsFolderSQL, sSQLName:=ACDeleteStatsFolderName, bStoredProcedure:=ACDeleteStatsFolderStored)

    End Function

    '*********************************************************************************************
    ' Name : DeleteStatsDetail
    '
    ' Desc : delete stats detail for this insurance file cnt
    '        this will also delete transaction export folder and detail
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function DeleteStatsDetail(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=IIf(v_sDocumentRef = "", DBNull.Value, v_sDocumentRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_oDatabase.SQLAction(sSQL:=ACDeleteStatsDetailSQL, sSQLName:=ACDeleteStatsDetailName, bStoredProcedure:=ACDeleteStatsDetailStored)

    End Function

    '*********************************************************************************************
    ' Name : CreateStatsDetail
    '
    ' Desc : create stats details
    '        stored procedure will delete stats folder when failed to add stats detail
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function CreateStatsDetail(ByVal v_lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsDetailsSQL, sSQLName:=ACAddStatsDetailsName, bStoredProcedure:=ACAddStatsDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsDetail", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : GetStatsFolderCnt
    '
    ' Desc : get stats folder cnt for this insurance file cnt
    '
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function GetStatsFolderCnt(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sDocumentRef As String, ByRef r_lStatsFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetStatsFolderSQL, sSQLName:=ACGetStatsFolderName, bStoredProcedure:=ACGetStatsFolderStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Dim auxVar As Object = vResultArray(0, 0)


            r_lStatsFolderCnt = IIf(Convert.IsDBNull(auxVar) Or IsNothing(auxVar), 0, CInt(vResultArray(0, 0)))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatsFolderCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatsFolderCnt", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : CreateExportFolder
    '
    ' Desc : create transaction export folder
    '
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function CreateExportFolder(ByVal v_lStatsFolderCnt As Integer, ByRef r_lExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(r_lExportFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportFolderSQL, sSQLName:=ACAddExportFolderName, bStoredProcedure:=ACAddExportFolderStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateExportFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportFolder", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : CreateExportDetail
    '
    ' Desc : create transaction export details
    '        stored proc will delete transaction export folder when failed to add export details
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function CreateExportDetail(ByVal v_lStatsFolderCnt As Integer, ByVal v_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportDetailsSQL, sSQLName:=ACAddExportDetailsName, bStoredProcedure:=ACAddExportDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateExportDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateExportDetail", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : GetTransactionExportFolderCnt
    '
    ' Desc : get transaction export folder cnt for this insurance file cnt
    '
    ' Hist : Thinh Nguyen 19/11/2003 - Created
    '*********************************************************************************************
    Public Function GetTransactionExportFolderCnt(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransExportFolderSQL, sSQLName:=ACGetTransExportFolderName, bStoredProcedure:=ACGetStatsFolderStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Dim auxVar As Object = vResultArray(0, 0)


            r_lTransactionExportFolderCnt = IIf(Convert.IsDBNull(auxVar) Or IsNothing(auxVar), 0, CInt(vResultArray(0, 0)))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionExportFolderCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionExportFolderCnt", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : IsDocumentInAccount
    '
    ' Desc : check to see if this document ref is in account
    '
    ' Hist : Thinh Nguyen 03/12/2003 - Created
    '*********************************************************************************************
    Public Function IsDocumentInAccount(ByVal v_sDocumentRef As String, ByRef r_lStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentIDSQL, sSQLName:=ACGetDocumentIDName, bStoredProcedure:=ACGetDocumentIDStored, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                r_lStatus = gPMConstants.PMEReturnCode.PMNotFound
            Else
                r_lStatus = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsDocumentInAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsDocumentInAccount", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    '*********************************************************************************************
    ' Name : GetAllPolicyVersion
    '
    ' Desc : get all versions of policy
    '
    ' Hist : Thinh Nguyen 04/12/2003 - Created
    '*********************************************************************************************
    Public Function GetAllPolicyVersion(ByVal v_sInsuranceRef As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceRef", vValue:=v_sInsuranceRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyVersionSQL, sSQLName:=ACGetPolicyVersionName, bStoredProcedure:=ACGetPolicyVersionStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    Public Function ExecuteSql(ByVal v_sSQL As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=v_sSQL, _
                                                  sSQLName:="SuppliedSql", _
                                                  bStoredProcedure:=False, _
                                                  vResultArray:=r_vResultArray, _
                                                  lNumberRecords:=-1)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExecuteSql Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ExecuteSql", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try

    End Function

    '*********************************************************************************************
    ' Name : IsPolicyVersionInAccount
    '
    ' Desc : check to see if this version of policy is in account ie document is created
    '
    ' Hist : Thinh Nguyen 04/12/2003 - Created
    '*********************************************************************************************
    Public Function IsPolicyVersionInAccount(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDocSQL, sSQLName:=ACGetPolicyDocName, bStoredProcedure:=ACGetPolicyDocStored, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then
                r_sDocumentRef = gPMFunctions.ToSafeString(vResultArray(0, 0))
            Else
                r_sDocumentRef = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsPolicyVersionInAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsPolicyVersionInAccount", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : DeleteDocument
    '
    ' Desc : delete this document and all its allocation from account
    '
    ' Hist : Thinh Nguyen 04/12/2003 - Created
    '*********************************************************************************************
    Public Function DeleteDocument(ByVal v_sDocumentRef As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocRef", vValue:=v_sDocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLAction(sSQL:=ACDelDocumentSQL, sSQLName:=ACDelDocumentName, bStoredProcedure:=ACDelDocumentStored)

        Catch excep As System.Exception



            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : DeleteDocumentAllocation
    '
    ' Desc : delete all allocations for this document from account
    '
    ' Hist : Thinh Nguyen 04/12/2003 - Created
    '*********************************************************************************************
    Public Function DeleteDocumentAllocation(ByVal v_sDocumentRef As String) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocRef", vValue:=v_sDocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLAction(sSQL:=ACDelDocAllocationSQL, sSQLName:=ACDelDocAllocationName, bStoredProcedure:=ACDelDocAllocationStored)

        Catch excep As System.Exception



            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteDocumentAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteDocumentAllocation", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : GetPolicyVersionDocument
    '
    ' Desc : get all documents for this policy version
    '
    ' Hist : Thinh Nguyen 19/12/2003 - Created
    '*********************************************************************************************
    Public Function GetPolicyVersionDocument(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vDocumentRef(,) As Object) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyDocumentSQL, sSQLName:=ACGetPolicyDocumentName, bStoredProcedure:=ACGetPolicyDocumentStored, vResultArray:=r_vDocumentRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyVersionDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyVersionDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    '*********************************************************************************************
    ' Name : DeletePolicyVersion
    '
    ' Desc : delete this version of policy and all its dependancies
    '
    ' Hist : Thinh Nguyen 19/12/2003 - Created
    '*********************************************************************************************
    Public Function DeletePolicyVersion(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLAction(sSQL:=ACDelPolicyVersionSQL, sSQLName:=ACDelPolicyVersionName, bStoredProcedure:=ACDelPolicyVersionStored)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeletePolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    ' Name : GetClosedClaimWithNoPosting
    '
    ' Desc : get all closed claims where the last transaction is not a CLA or CLP
    '        or last transaction is a CLA or CLP but the transaction amount <> this_revision or amount <> this_payment
    '
    ' Hist : Thinh Nguyen 08/03/2005 - Created
    '*********************************************************************************************
    Public Function GetClosedClaimWithNoPosting(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()
            m_oDatabase.QueryTimeout = 7200

            Return m_oDatabase.SQLSelect(sSQL:=ACGetClosedClaimWithNoPostingSQL, sSQLName:=ACGetClosedClaimWithNoPostingName, bStoredProcedure:=ACGetClosedClaimWithNoPostingStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClosedClaimWithNoPosting() Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClosedClaimWithNoPosting()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    '*********************************************************************************************
    'reprocess and post the amount in this_revision or this_payment.
    'it will only be one or the other
    ' if this payment or revision is passed in then we use it rather than values from table
    'Note: there will only be one peril per transaction
    ' We should only redo reinsurance for 100% retained model
    '*********************************************************************************************
    Public Function ReprocessClaim(ByRef r_sMessage As String, Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_lClaimID As Integer = 0, Optional ByVal v_cThisRevision As Decimal = 0, Optional ByVal v_cThisPayment As Decimal = 0, Optional ByVal v_lOriginalReserveID As Integer = 0, Optional ByVal v_lPaymentID As Integer = 0, Optional ByVal v_sTransactionType As String = "") As Integer

        Dim result As Integer = 0
        Dim lWorkClaimID As Integer
        Dim sLockBy As String = ""
        Dim vResultArray As String = ""
        Dim vClaimPeril As Object
        Dim sTransactionType As String = ""
        Dim bWorkClaim As Boolean
        Dim cBalance As Decimal
        Dim lValid, lBand As Integer
        Dim bAdjustRILine As Boolean

        Const ACEventClaChange As Integer = 6

        On Error GoTo Catch_Renamed

        bAdjustRILine = False
        bWorkClaim = False
        r_sMessage = ""

        '*********************** create required objects (start)*****************************************
        If m_oReinsurance Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bCLMReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create an instance of bCLMReinsurance.Form"

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
            End If
        End If

        If m_oChangeClaimStatus Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oChangeClaimStatus, v_sClassName:="bCLMChangeClaimStatus.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create an instance of bCLMChangeClaimStatus.Business"

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
            End If
        End If

        If m_oFindClaim Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindClaim, v_sClassName:="bCLMFindClaim.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create an instance of bCLMFindClaim.Business"

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
            End If
        End If

        If m_oClaimTrans Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oClaimTrans, v_sClassName:="bControlTransClaims.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create an instance of bControlTransClaims.Automated"

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
            End If
        End If
        '*********************** create required objects (end)*******************************************

        'must have either claim number or claim ID
        If v_sClaimNumber = "" And v_lClaimID = 0 Then
            r_sMessage = "Must provide either claim number or claim ID"
            GoTo Finally_Renamed
        End If

        If v_lClaimID = 0 Then
            result = GetValueFromTable(v_sTableName:="Claim", v_vReturnColumn:="claim_id", v_sKeyColumn:="claim_number", v_sKeyValue:=v_sClaimNumber, v_lDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=vResultArray)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                v_lClaimID = CInt(vResultArray) 'This will only be an array if we expect multiple columns back
            Else
                r_sMessage = "Failed to get claim ID for claim number " & v_sClaimNumber
                GoTo Finally_Renamed
            End If
        Else
            If v_sClaimNumber = "" Then
                result = GetValueFromTable(v_sTableName:="Claim", v_vReturnColumn:="claim_number", v_sKeyColumn:="claim_id", v_sKeyValue:=CStr(v_lClaimID), v_lDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    v_sClaimNumber = vResultArray.Trim() 'This will only be an array if we expect multiple columns back
                Else
                    r_sMessage = "Failed to get claim number for claim ID " & v_lClaimID
                    GoTo Finally_Renamed
                End If
            End If
        End If

        'lock this claim to stop other user from processing it (this will be unlock in Change Claim Status routine)
        result = LockKey(v_sKeyName:="claim_id", v_lKeyValue:=v_lClaimID, v_lUserID:=m_iUserID, r_sLockedBy:=sLockBy)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then

            If sLockBy = "ERROR" Then
                r_sMessage = "Failed to lock claim (ClaimID = " & v_lClaimID & ")"
            Else
                r_sMessage = "ClaimID = " & v_lClaimID & " is being locked by " & sLockBy
            End If

            GoTo Finally_Renamed
        Else
            sLockBy = "Locked"
        End If

        'copy claim to work table

        result = m_oFindClaim.CopyClaimToWork(v_lClaimID:=v_lClaimID, r_lWorkClaimId:=lWorkClaimID)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to copy claim to work tables ClaimID = " & v_lClaimID & " to work tables"
            GoTo Finally_Renamed
        End If
        bWorkClaim = True

        'set work and real claim ids ready for posting

        m_oChangeClaimStatus.WorkClaimID = lWorkClaimID

        m_oChangeClaimStatus.ClaimId = v_lClaimID

        'get ri_band for this reserve
        result = GetReserveBand(v_lOriginalReserveID, lBand, r_sMessage)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        'get claim balance (initial reserve + revised reserve - paid to date)
        result = GetClaimBalance(v_lWorkClaimID:=lWorkClaimID, r_cBalance:=cBalance, r_sMessage:=r_sMessage)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        If cBalance < 0 Then
            r_sMessage = "Claim balance is below zero " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        'update work_reserve.this_revision and work_reserve.this_payment with values from reserve or from supplied values
        result = UpdateWorkReserveThisRevisionPayment(v_lWorkClaimID:=lWorkClaimID, r_sMessage:=r_sMessage, v_cThisRevision:=v_cThisRevision, v_cThisPayment:=v_cThisPayment, v_lOriginalReserveID:=v_lOriginalReserveID)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        'update work_payment.original_payment_id = null for payment_id provided so that stats is created with correct currency_id
        If v_lPaymentID <> 0 Then
            'check to see if we have original_payment_id in work_payment table
            If IsColumn("Work_Payment", "original_payment_id", r_sMessage) Then
                result = UpdateWorkPayment(v_lWorkClaimID:=lWorkClaimID, v_lOriginalPaymentID:=v_lPaymentID, r_sMessage:=r_sMessage)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    GoTo Finally_Renamed
                End If
            Else
                If r_sMessage <> "" Then
                    GoTo Finally_Renamed
                End If
            End If
        End If

        'get claim peril details so we can work out whether its a claim payment or claim adjustment
        'Note: there should only be one peril ie user can only process one peril per transaction
        'this call is needed here because we can come in with a different mode, ie payment_id is not passed in and yet its a payment transaction
        result = GetClaimPerilDetail(v_lWorkClaimID:=lWorkClaimID, r_vResultArray:=vClaimPeril, r_sMessage:=r_sMessage, v_cThisRevision:=v_cThisRevision, v_cThisPayment:=v_cThisPayment, v_lPaymentID:=v_lPaymentID, v_lOriginalReserveID:=v_lOriginalReserveID)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = r_sMessage & " Claim Number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        If Not Information.IsArray(vClaimPeril) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
            r_sMessage = "No claim peril details found for Claim Number " & v_sClaimNumber
            GoTo Finally_Renamed
        Else
            If v_sTransactionType <> "" Then
                sTransactionType = v_sTransactionType
            Else

                If CDec(vClaimPeril(ACClaimPerilTotalThisPayment, 0)) <> 0 Then
                    sTransactionType = "C_CP"
                Else
                    sTransactionType = "C_CR"
                End If
            End If
        End If

        'NOTE: this claim is balance with all the transactions that have gone through
        'we are only reposting what is already there.
        'reinsurance won't get recalculate if the reserve total and band total are the same
        'and the NET portion will be incorrect if band total goes below zero
        'since this is a repost we'll botch it so that we'll always have enough reserve
        result = UpdateRIBand(v_lWorkClaimID:=lWorkClaimID, v_lRIBand:=lBand, v_cReserveAmount:=v_cThisRevision, v_cPaymentAmount:=v_cThisPayment, r_sMessage:=r_sMessage)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        'update (real) claim_risk_ri_arrangement_line
        'we need to do this because m_oReinsurance.CalculateRI copies values over to work when calculation is done
        result = UpdateClaimRIArrangmentLine(v_lClaimID:=v_lClaimID, v_lBandID:=lBand, v_cReserveAmount:=v_cThisRevision, v_cPaymentAmount:=v_cThisPayment, r_sMessage:=r_sMessage)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If
        bAdjustRILine = True

        'set transaction type

        result = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=sTransactionType)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to SetProcessModes for bCLMReinsurance.Form"
            GoTo Finally_Renamed
        End If


        result = m_oChangeClaimStatus.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=sTransactionType)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to SetProcessModes for bCLMChangeClaimStatus.Business"
            GoTo Finally_Renamed
        End If


        result = m_oClaimTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=sTransactionType)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to SetProcessModes for bControlTransClaims.Automated"
            GoTo Finally_Renamed
        End If

        'create stats

        result = CreateWorkStats(v_lWorkClaimID:=lWorkClaimID, v_vClaimPeril:=vClaimPeril, r_sMessage:=r_sMessage, v_sTransactionType:=sTransactionType)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        'this is the stored procedure spu_copy_reinsurance_details_to_work_claim used in m_oReinsurance.CalculateRI
        'sort out reinsurance

        m_oReinsurance.ClaimId = lWorkClaimID

        result = m_oReinsurance.CalculateRI

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to do reinsurance for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If


        result = m_oReinsurance.GetDetails
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to get reinsurance for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If


        result = m_oReinsurance.ValidateBands(r_lValid:=lValid, r_lBand:=lBand)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to validate reinsurance bands for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        If lValid <> 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse
            r_sMessage = "Reinsurance band " & lBand & " was not 100% allocated for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        'ReprocessClaim = m_oChangeClaimStatus.CopyWorkToClaim()

        'If ReprocessClaim <> PMTrue Then
        'r_sMessage = "Failed to copy work to claim for claim number " & v_sClaimNumber
        'GoTo Finally
        'End If

        '***************** TO DO? if CLP need to CheckUserPaymentAuthority() *****************


        result = m_oChangeClaimStatus.UpdateInsuranceFileSystem()

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to update insurance file system for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        'this is the proc used to work out NET values spu_add_claims_stats_details_reins

        result = m_oChangeClaimStatus.RaiseTransactions(False)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to raise transactions for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If


        result = m_oChangeClaimStatus.SetPaymentReferred(v_lClaimID, 0)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to set payment referred for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If


        result = m_oChangeClaimStatus.CreateEvent(ACEventClaChange, "ReprocessClaim() - repost failed transaction")

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to create event for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError
        r_sMessage = "ReprocessClaim() Failed - " & Information.Err().Description

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReprocessClaim() Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If bAdjustRILine Then
            'after botching the reserve/payment on claim_ri_arrangement_line we better put it back
            result = UpdateClaimRIArrangmentLine(v_lClaimID:=v_lClaimID, v_lBandID:=lBand, v_cReserveAmount:=(v_cThisRevision * -1), v_cPaymentAmount:=(v_cThisPayment * -1), r_sMessage:=r_sMessage)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Failed to retored Claim_RI_Arrangement_Line to its original state for claim " & v_sClaimNumber
            End If
        End If

        If bWorkClaim Then

            m_lReturn = m_oChangeClaimStatus.DeleteWorkClaim()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = r_sMessage & Strings.Chr(13) & Strings.Chr(10) & "Failed to delete work claim for claim number " & v_sClaimNumber
            End If
        End If

        If sLockBy = "Locked" Then
            m_lReturn = UnLockKey(v_sKeyName:="claim_id", v_lKeyValue:=v_lClaimID, v_lUserID:=m_iUserID)
        End If

        Return result
    End Function

    '************************************************************************************
    'create work stats folder and work stats details for this claim per per claim peril id
    'Note : all the IDs refer to in this function are work IDs
    '************************************************************************************
    Private Function CreateWorkStats(ByVal v_lWorkClaimID As Integer, ByVal v_vClaimPeril(,) As Object, ByRef r_sMessage As String, Optional ByVal v_sTransactionType As String = "") As Integer

        Dim result As Integer = 0
        Dim lStatsFolderCnt As Integer
        Dim sCreditAccountCode As String = ""
        Dim lTransactionTypeID As Integer
        Dim sTransactionTypeCode As String = ""
        Dim lDocumentTypeID As Integer
        Dim sDebitCredit As String = ""
        Dim lCreditAccountID, lDebitAccountID As Integer
        Dim cTransactionAmount As Decimal
        Dim vResultArray, sClaimSuspenseOption As String

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        'do we have any peril
        If Not Information.IsArray(v_vClaimPeril) Then
            GoTo Finally_Renamed
        End If



        m_oClaimTrans.InsuranceFileCnt = CInt(v_vClaimPeril(ACClaimPerilPolicyID, 0))

        'loop through and create stats detail for each claim peril
        For lCount As Integer = 0 To v_vClaimPeril.GetUpperBound(1)
            'we will only either have value for TotalThisPayment or TotalThisRevision depending on last transaction on this claim

            If CDec(v_vClaimPeril(ACClaimPerilTotalThisPayment, 0)) <> 0 Then 'claim payment

                'amount to repost

                cTransactionAmount = CDec(v_vClaimPeril(ACClaimPerilTotalThisPayment, 0))

                'check to see if claim suspense option is set
                result = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_iOptionNumber:=2002, r_sOptionValue:=sClaimSuspenseOption)

                ' ---- GetSystemOption() return PMFalse if value is not found.
                '            If CreateWorkStats <> PMTrue Then
                '                r_sMessage = "Failed to get system option value = 2002 (Claim Suspense)"
                '                GoTo Finally
                '            End If

                If gPMFunctions.ToSafeLong(sClaimSuspenseOption) = 0 Then
                    lCreditAccountID = 0

                    sCreditAccountCode = "CLMSUS" & CStr(v_vClaimPeril(ACClaimPerilClassOfBusinessCode, lCount)).Trim()
                Else
                    'do we have party cnt

                    If CInt(v_vClaimPeril(ACClaimPerilPartyCnt, lCount)) <> 0 Then

                        result = GetValueFromTable(v_sTableName:="Party", v_vReturnColumn:="shortname", v_sKeyColumn:="party_cnt", v_sKeyValue:=CStr(v_vClaimPeril(ACClaimPerilPartyCnt, lCount)), v_lDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)
                        If result <> gPMConstants.PMEReturnCode.PMTrue Then

                            r_sMessage = "Failed to get party short name for party_cnt = " & CStr(v_vClaimPeril(ACClaimPerilPartyCnt, lCount))
                            GoTo Finally_Renamed
                        End If

                        'get back party shortname
                        sCreditAccountCode = vResultArray 'This will only be an array if we expect multiple columns back



                        result = m_oClaimTrans.GetAccountID(r_lAccountID:=lCreditAccountID, v_lPartyCnt:=CInt(v_vClaimPeril(ACClaimPerilPartyCnt, lCount)))

                        If result <> gPMConstants.PMEReturnCode.PMTrue Then

                            r_sMessage = "Failed to get account_id for party_cnt = " & CStr(v_vClaimPeril(ACClaimPerilPartyCnt, lCount))
                            GoTo Finally_Renamed
                        End If
                    Else
                        lCreditAccountID = 0
                        sCreditAccountCode = "CLMPAYABLE"
                    End If
                End If

                lDebitAccountID = 0
                lTransactionTypeID = 27
                sTransactionTypeCode = "C_CP"
                lDocumentTypeID = 28
                sDebitCredit = "C"


                m_oClaimTrans.DocumentComment = "Payment for claim number " & CStr(v_vClaimPeril(ACClaimPerilClaimNumber, 0))
            Else
                'claim adjustment

                'amount to repost

                cTransactionAmount = CDec(v_vClaimPeril(ACClaimPerilTotalThisRevision, lCount))

                lDebitAccountID = 0
                lCreditAccountID = 0
                lTransactionTypeID = 28
                sTransactionTypeCode = "C_CR"
                lDocumentTypeID = 35
                sDebitCredit = "D"



                m_oClaimTrans.DocumentComment = "Reserve for claim number " & CStr(v_vClaimPeril(ACClaimPerilClaimNumber, 0))

                sCreditAccountCode = "CLMRES" & CStr(v_vClaimPeril(ACClaimPerilClassOfBusinessCode, lCount)).Trim()
            End If

            If v_sTransactionType <> "" Then
                sTransactionTypeCode = v_sTransactionType
            End If

            If cTransactionAmount <> 0 Then

                m_oClaimTrans.DebitAccountID = lDebitAccountID

                m_oClaimTrans.CreditAccountID = lCreditAccountID

                m_oClaimTrans.TransactionTypeID = lTransactionTypeID

                m_oClaimTrans.TransactionTypeCode = sTransactionTypeCode

                m_oClaimTrans.DocumentTypeID = lDocumentTypeID

                m_oClaimTrans.ClaimId = v_lWorkClaimID

                m_oClaimTrans.DebitCredit = sDebitCredit


                m_oClaimTrans.PerilID = CInt(v_vClaimPeril(ACClaimPerilClaimPerilID, lCount))

                m_oClaimTrans.TransactionAmount = cTransactionAmount

                'create stats folder once for each claim
                If lCount = 0 Then

                    result = m_oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=sTransactionTypeCode)

                    If result <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to create stats folder for claim number " & CStr(v_vClaimPeril(ACClaimPerilClaimNumber, 0))
                        GoTo Finally_Renamed
                    End If
                End If





                result = m_oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=CInt(v_vClaimPeril(ACClaimPerilClassOfBusinessID, lCount)), v_sClassOfBusCode:=CStr(v_vClaimPeril(ACClaimPerilClassOfBusinessCode, lCount)).Trim(), v_lRIPartyCnt:=CInt(v_vClaimPeril(ACClaimPerilPartyCnt, lCount)), v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

                If result <> gPMConstants.PMEReturnCode.PMTrue Then

                    r_sMessage = "Failed to create stats detail for claim number " & CStr(v_vClaimPeril(ACClaimPerilClaimNumber, 0))
                    GoTo Finally_Renamed
                End If
            End If
        Next lCount

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError
        r_sMessage = "CreateWorkStats() Failed - " & Information.Err().Description

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReprocessClaim() Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
        Resume
        Return result
    End Function

    '************************************************************************************
    ' get claim peril details
    '************************************************************************************
    Private Function GetClaimPerilDetail(ByVal v_lWorkClaimID As Integer, ByRef r_vResultArray(,) As Object, ByRef r_sMessage As String, Optional ByVal v_cThisRevision As Decimal = 0, Optional ByVal v_cThisPayment As Decimal = 0, Optional ByVal v_lPaymentID As Integer = 0, Optional ByVal v_lOriginalReserveID As Integer = 0) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add (WorkClaimID) parameter"
            GoTo Finally_Renamed
        End If

        If v_cThisRevision <> 0 Or v_cThisPayment <> 0 Then
            If (v_cThisPayment <> 0 And v_lPaymentID = 0) Or v_lOriginalReserveID = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "Must provide PaymentID and or OriginalReserveID"
                GoTo Finally_Renamed
            End If

            result = m_oDatabase.Parameters.Add(sName:="ThisRevision", vValue:=CStr(v_cThisRevision), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add (ThisRevision) parameter"
                GoTo Finally_Renamed
            End If

            result = m_oDatabase.Parameters.Add(sName:="ThisPayment", vValue:=CStr(v_cThisPayment), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add (ThisPayment) parameter"
                GoTo Finally_Renamed
            End If

            result = m_oDatabase.Parameters.Add(sName:="PaymentID", vValue:=CStr(v_lPaymentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add (PaymentID) parameter"
                GoTo Finally_Renamed
            End If

            result = m_oDatabase.Parameters.Add(sName:="OriginalReserveID", vValue:=CStr(v_lOriginalReserveID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add (OriginalReserveID) parameter"
                GoTo Finally_Renamed
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimPerilDetail2SQL, sSQLName:=ACGetClaimPerilDetail2Name, bStoredProcedure:=ACGetClaimPerilDetail2Stored, vResultArray:=r_vResultArray)

        Else

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimPerilDetailSQL, sSQLName:=ACGetClaimPerilDetailName, bStoredProcedure:=ACGetClaimPerilDetailStored, vResultArray:=r_vResultArray)
        End If

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to get claim peril details"
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        result = gPMConstants.PMEReturnCode.PMError

        r_sMessage = "Failed to get claim peril details"

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimPerilDetail() Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPerilDetail()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    '************************************************************************************
    'create a lock for specified key and value
    '************************************************************************************
    Public Function LockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer, ByRef r_sLockedBy As String) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        If m_oLock Is Nothing Then
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oLock, v_sClassName:="bPMLock.User", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sLockedBy = "ERROR"

                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bPMLock.User", vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessThisRevision", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

                GoTo Finally_Renamed
            End If
        End If


        result = m_oLock.LockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID, sCurrentlyLockedBy:=r_sLockedBy)

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

        GoTo Finally_Renamed

Finally_Renamed:

        Return result

    End Function

    '************************************************************************************
    'unlock specified key
    '************************************************************************************
    Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed


        result = m_oLock.UnLockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID)

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="UnLockKey()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        Return result

    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lDataType = PMLong, PMString etc
    '
    '****************************************************************************
    Public Function GetValueFromTable(ByVal v_sTableName As String, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As String, ByVal v_sKeyValue As String, ByVal v_lDataType As Integer, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray As Object

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        sSQL = New StringBuilder("SELECT DISTINCT ")

        If Information.IsArray(v_vReturnColumn) Then

            For Each v_vReturnColumn_item As Object In v_vReturnColumn

                sSQL.Append(CStr(v_vReturnColumn_item) & ",")
            Next v_vReturnColumn_item

            'get rid of last comma
            sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

        Else
            sSQL.Append(CStr(v_vReturnColumn))
        End If

        sSQL.Append(Strings.Chr(13) & Strings.Chr(10) & "FROM " & v_sTableName & Strings.Chr(13) & Strings.Chr(10))

        If v_sKeyColumn <> "" Then
            sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

            m_oDatabase.Parameters.Clear()

            Select Case v_lDataType
                Case gPMConstants.PMEDataType.PMString
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)
                Case gPMConstants.PMEDataType.PMLong
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)
                Case gPMConstants.PMEDataType.PMInteger
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CInt(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)

                Case gPMConstants.PMEDataType.PMDouble
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDbl(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)

                Case gPMConstants.PMEDataType.PMDate
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=DateTimeHelper.ToString(CDate(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)

                Case gPMConstants.PMEDataType.PMBoolean
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CBool(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)

                Case gPMConstants.PMEDataType.PMCurrency
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=CStr(CDec(v_sKeyValue)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_lDataType)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GoTo Finally_Renamed
            End If
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Finally_Renamed
        End If

        'are we returning an array or a single value?
        If Information.IsArray(vResultArray) Then
            If Information.IsArray(v_vReturnColumn) Then

                r_vResult = vResultArray
            Else

                r_vResult = vResultArray(0, 0)
            End If
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        Return result

    End Function

    '***********************************************************************************************************
    'update the work reserve table with values from this_revision or this_payment from reserve table
    'and adjust the revision or the paid_to_date as though we are going through the road map
    'if this_revision or this_payment are passed in then use them
    '***********************************************************************************************************
    Private Function UpdateWorkReserveThisRevisionPayment(ByVal v_lWorkClaimID As Integer, ByRef r_sMessage As String, Optional ByVal v_cThisRevision As Decimal = 0, Optional ByVal v_cThisPayment As Decimal = 0, Optional ByVal v_lOriginalReserveID As Integer = 0) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add (WorkClaimID) parameter to m_oDatabase"
            GoTo Finally_Renamed
        End If

        If v_cThisRevision <> 0 Or v_cThisPayment <> 0 Then
            If v_lOriginalReserveID <> 0 Then
                'update work this revision and this payment with passed in values
                result = m_oDatabase.Parameters.Add(sName:="ThisRevision", vValue:=CStr(v_cThisRevision), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to add (ThisRevision) parameter to m_oDatabase"
                    GoTo Finally_Renamed
                End If

                result = m_oDatabase.Parameters.Add(sName:="ThisPayment", vValue:=CStr(v_cThisPayment), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to add (ThisPayment) parameter to m_oDatabase"
                    GoTo Finally_Renamed
                End If

                result = m_oDatabase.Parameters.Add(sName:="OriginalReserveID", vValue:=CStr(v_lOriginalReserveID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to add (OriginalReserveID) parameter to m_oDatabase"
                    GoTo Finally_Renamed
                End If

                result = m_oDatabase.SQLAction(sSQL:=ACUpdateWorkReserve2SQL, sSQLName:=ACUpdateWorkReserve2Name, bStoredProcedure:=ACUpdateWorkReserve2Stored)

            Else
                result = gPMConstants.PMEReturnCode.PMFalse
                r_sMessage = "OriginalReserveID = 0"
                GoTo Finally_Renamed
            End If
        Else
            'update work this revision and work this payment with values from original this revision and this payment
            result = m_oDatabase.SQLAction(sSQL:=ACUpdateWorkReserveSQL, sSQLName:=ACUpdateWorkReserveName, bStoredProcedure:=ACUpdateWorkReserveStored)
        End If

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to update work_reserve table"
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed
Catch_Renamed:

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update work reserve with values from reserve table", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateWorkReserveThisRevisionPayment()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    '***********************************************************************************************************
    'Get all failed claim transactions including those without transaction export folder
    '***********************************************************************************************************
    Public Function GetFailedClaimTransaction(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.SQLSelect(sSQL:=ACGetFailedClaimTransactionSQL, sSQLName:=ACGetFailedClaimTransactionName, bStoredProcedure:=ACGetFailedClaimTransactionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)


        GoTo Finally_Renamed

Catch_Renamed:
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get failed claim transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedClaimTransaction()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' get imbalanced closed claims ie stats_detail.this_premium_home do not sum to zero
    '***********************************************************************************************************
    Public Function GetImbalanceClosedClaim(ByRef r_vResultArray(,) As Object, Optional ByVal v_sClaimNumber As String = "") As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.QueryTimeout = 7200

        m_oDatabase.Parameters.Clear()


        result = m_oDatabase.Parameters.Add(sName:="ClaimNumber", vValue:=IIf(v_sClaimNumber = "", DBNull.Value, v_sClaimNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACGetImbalanceClosedClaimSQL, sSQLName:=ACGetImbalanceClosedClaimName, bStoredProcedure:=ACGetImbalanceClosedClaimStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

        GoTo Finally_Renamed

Catch_Renamed:
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get imbalance closed claims", vApp:=ACApp, vClass:=ACClass, vMethod:="GetImbalanceClosedClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' get reserve details
    '***********************************************************************************************************
    Public Function GetReserveDetail(ByVal v_sClaimNumber As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lNoneZeroReserve As Integer = 0) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="ClaimNumber", vValue:=v_sClaimNumber, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        If v_lNoneZeroReserve = 1 Then
            result = m_oDatabase.SQLSelect(sSQL:=ACGetNoneZeroReserveDetailSQL, sSQLName:=ACGetNoneZeroReserveDetailName, bStoredProcedure:=ACGetNoneZeroReserveDetailStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

        Else
            result = m_oDatabase.SQLSelect(sSQL:=ACGetReserveDetailSQL, sSQLName:=ACGetReserveDetailName, bStoredProcedure:=ACGetReserveDetailStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get reserve details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetail()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' get payment details
    '***********************************************************************************************************
    Public Function GetPaymentDetail(ByVal v_sClaimNumber As String, ByRef r_vResultArray(,) As Object, Optional ByVal v_lUniquePaymentPartyCode As Integer = 0) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="ClaimNumber", vValue:=v_sClaimNumber, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        If v_lUniquePaymentPartyCode = 0 Then
            result = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentDetailSQL, sSQLName:=ACGetPaymentDetailName, bStoredProcedure:=ACGetPaymentDetailStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
        Else
            result = m_oDatabase.SQLSelect(sSQL:=ACGetUniquePaymentPartyCodeSQL, sSQLName:=ACGetUniquePaymentPartyCodeName, bStoredProcedure:=ACGetUniquePaymentPartyCodeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

        End If

        GoTo Finally_Renamed

Catch_Renamed:
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get payment details", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetail()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' get claim peril
    '***********************************************************************************************************
    Public Function GetClaimPeril(ByVal v_lClaimID As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimPerilSQL, sSQLName:=ACGetClaimPerilName, bStoredProcedure:=ACGetClaimPerilStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

        GoTo Finally_Renamed

Catch_Renamed:
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get claim peril", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPeril()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function


    '***********************************************************************************************************
    ' update work_payment.original_payment_id = null so stats is created with correct currency_code
    '***********************************************************************************************************
    Public Function UpdateWorkPayment(ByVal v_lWorkClaimID As Integer, ByVal v_lOriginalPaymentID As Integer, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        result = m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add (WorkClaimID) parameter to m_oDatabase"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.Parameters.Add(sName:="OriginalPaymentID", vValue:=CStr(v_lOriginalPaymentID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add (OriginalPaymentID) parameter to m_oDatabase"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLAction(sSQL:=ACUpdateWorkPaymentSQL, sSQLName:=ACUpdateWorkPaymentName, bStoredProcedure:=ACUpdateWorkPaymentStored)
        GoTo Finally_Renamed


        r_sMessage = Information.Err().Description

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set work_payment.original_payment_id = null", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateWorkPayment()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' return True if field exist in table
    '***********************************************************************************************************
    Private Function IsColumn(ByVal v_sTable As String, ByVal v_sColumn As String, ByRef r_sMessage As String) As Boolean

        Dim result As Boolean = False
        Dim vResultArray(,) As Object

        On Error GoTo Catch_Renamed
        result = True
        r_sMessage = ""

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add("TableName", v_sTable, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add TableName param"
            GoTo Finally_Renamed
        End If

        If m_oDatabase.Parameters.Add("ColumnName", v_sColumn, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add ColumnName param"
            GoTo Finally_Renamed
        End If

        If m_oDatabase.SQLSelect(sSQL:=ACIsColumnSQL, sSQLName:=ACIsColumnName, bStoredProcedure:=ACIsColumnStored, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to execute SQL statement to check for column name - " & v_sColumn & " in table - " & v_sTable
            GoTo Finally_Renamed
        End If

        result = Information.IsArray(vResultArray)

        GoTo Finally_Renamed

Catch_Renamed:
        result = False

        r_sMessage = Information.Err().Description

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if column - " & v_sColumn & " exist in table - " & v_sTable, vApp:=ACApp, vClass:=ACClass, vMethod:="IsColumn()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' get risk details for this policy version
    '***********************************************************************************************************
    Public Function GetRiskDetail(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACGetRiskDetailSQL, sSQLName:=ACGetRiskDetailName, bStoredProcedure:=ACGetRiskDetailStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=False)

        GoTo Finally_Renamed

Catch_Renamed:

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get risk details for policy_id = " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetail()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '***********************************************************************************************************
    ' get transaction export for this policy version
    '***********************************************************************************************************
    Public Function GetTransactionExport(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionExportSQL, sSQLName:=ACGetTransactionExportName, bStoredProcedure:=ACGetTransactionExportStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=False)

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get transaction exoprt for policy_id = " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionExport()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    '*****************************************************************************
    ' add RI to policy at risk level
    ' Note : this will only work if RI model supplied is 100% retained
    '*****************************************************************************
    Public Function AddRIToPolicy(ByVal v_sRIModelCode As String, ByVal v_sInsuranceRef As String, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()


        result = m_oDatabase.Parameters.Add(sName:="InsuranceRef", vValue:=IIf(v_sInsuranceRef = "", DBNull.Value, v_sInsuranceRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add param InsuranceRef"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.Parameters.Add(sName:="RIModelCode", vValue:=v_sRIModelCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add param RIModelCode"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.Parameters.Add(sName:="Message", vValue:=r_sMessage, idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add param Message"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLAction(sSQL:=ACAddRIToPolicySQL, sSQLName:=ACAddRIToPolicyName, bStoredProcedure:=ACAddRIToPolicyStored)

        r_sMessage = m_oDatabase.Parameters.Item("Message").Value

        GoTo Finally_Renamed

Catch_Renamed:
        If r_sMessage = "" Then
            r_sMessage = Information.Err().Description
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="AddRIToPolicy()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    '******************************************************************************
    ' change status for this policy version
    '******************************************************************************
    Public Function SetStatusPolicyVersion(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFileStatusID As Integer, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed
        r_sMessage = ""

        m_oDatabase.Parameters.Clear()


        result = m_oDatabase.Parameters.Add(sName:="InsuranceFileStatusID", vValue:=IIf(v_lInsuranceFileStatusID = 0, DBNull.Value, CStr(v_lInsuranceFileStatusID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add param InsuranceFileStatusID"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to add param InsuranceFileStatusID"
            GoTo Finally_Renamed
        End If

        result = m_oDatabase.SQLAction(sSQL:=ACUpdPolicyStatusSQL, sSQLName:=ACUpdPolicyStatusName, bStoredProcedure:=ACUpdPolicyStatusStored)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            r_sMessage = "Failed to update policy status for policy_id (" & v_lInsuranceFileCnt & ")"
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        If r_sMessage = "" Then
            r_sMessage = Information.Err().Description
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatusPolicyVersion()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '******************************************************************************
    ' get all data from insurance_file_status
    '******************************************************************************
    Public Function PopulatePolicyStatus(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed

        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyStatusSQL, sSQLName:=ACGetPolicyStatusName, bStoredProcedure:=ACGetPolicyStatusStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)
        GoTo Finally_Renamed

Catch_Renamed:

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed PopulatePolicyStatus", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulatePolicyStatus()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '******************************************************************************
    ' delete claim and all associated postings including stats
    '******************************************************************************
    Public Function DeleteClaim(Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_lClaimID As Integer = 0) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimNumber", vValue:=v_sClaimNumber, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add parameter (ClaimNumber)"
            GoTo Catch_Renamed
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add parameter (ClaimID)"
            GoTo Catch_Renamed
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelClaimSQL, sSQLName:=ACDelClaimName, bStoredProcedure:=ACDelClaimStored)

        GoTo Finally_Renamed

Catch_Renamed:
        result = gPMConstants.PMEReturnCode.PMError

        If sMessage = "" Then
            sMessage = "Failed delete claim"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result
    End Function

    '******************************************************************************
    ' get claim postings
    '******************************************************************************
    Public Function GetClaimPosting(ByVal v_sClaimNumber As String, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimNumber", vValue:=v_sClaimNumber, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add parameter claim number"
            GoTo Catch_Renamed
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimPostingSQL, sSQLName:=ACGetClaimPostingName, bStoredProcedure:=ACGetClaimPostingStored, vResultArray:=r_vResultArray)
        GoTo Finally_Renamed

Catch_Renamed:
        result = gPMConstants.PMEReturnCode.PMError

        If sMessage = "" Then
            sMessage = "Failed delete claim"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimPosting()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '******************************************************************************
    ' get claim balance
    ' balance = initial reserve + revised reserve - paid to date
    '******************************************************************************
    Public Function GetClaimBalance(ByVal v_lWorkClaimID As Integer, ByRef r_cBalance As Decimal, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""
        Dim vResultArray(,) As Object

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue
        sMessage = ""

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param WorkClaimID "
            GoTo Catch_Renamed
        End If

        If m_oDatabase.SQLSelect(sSQL:=ACGetClaimBalanceSQL, sSQLName:=ACGetClaimBalanceName, bStoredProcedure:=ACGetClaimBalanceStored, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to retrieve claim balance"
            GoTo Catch_Renamed
        End If

        If Not Information.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        Else

            r_cBalance = CDec(vResultArray(0, 0))
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        result = gPMConstants.PMEReturnCode.PMError

        If sMessage = "" Then
            sMessage = "Failed to get claim balance"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimBalance()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        If Not False Then
            r_sMessage = sMessage
        End If

        Return result
    End Function

    '******************************************************************************
    ' change document date and period_id for document
    '******************************************************************************
    Public Function ChangeDateAndPeriodID(ByVal v_sDocumentRef As String, ByVal v_dDocumentDate As Date, ByVal v_lPeriodID As Integer, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param DocumentRef"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="DocDate", vValue:=DateTimeHelper.ToString(v_dDocumentDate), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param DocDate"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="PeriodID", vValue:=CStr(v_lPeriodID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param PeriodID"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.SQLAction(sSQL:=ACUpdateDocDatePeriodSQL, sSQLName:=ACUpdateDocDatePeriodName, bStoredProcedure:=ACUpdateDocDatePeriodStored) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to change document date and period_id"
            GoTo Catch_Renamed
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        result = gPMConstants.PMEReturnCode.PMError

        If sMessage = "" Then
            sMessage = "Failed to adjust work reserve"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeDateAndPeriodID()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        If Not False Then
            r_sMessage = sMessage
        End If

        Return result

    End Function

    '******************************************************************************
    ' copy reinsurance form policy to claim
    '******************************************************************************
    Public Function CopyRIToClaim(Optional ByVal v_sClaimNumber As String = "", Optional ByVal v_lClaimID As Integer = 0, Optional ByRef r_sMessage As String = "") As Integer
        Dim result As Integer = 0
        Dim sMessage As String = ""
        Dim vResultArray, sLockBy As String
        Dim lWorkClaimID As Integer
        Dim bWorkClaim As Boolean

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue
        bWorkClaim = False

        If m_oChangeClaimStatus Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oChangeClaimStatus, v_sClassName:="bCLMChangeClaimStatus.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create an instance of bCLMChangeClaimStatus.Business"

                GoTo Catch_Renamed
            End If
        End If

        If m_oReinsurance Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bCLMReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bCLMReinsurance.Form"

                GoTo Catch_Renamed
            End If
        End If

        If m_oFindClaim Is Nothing Then
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindClaim, v_sClassName:="bCLMFindClaim.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create an instance of bCLMFindClaim.Business"

                GoTo Catch_Renamed
            End If
        End If

        'must have either claim number or claim ID
        If v_sClaimNumber = "" And v_lClaimID = 0 Then
            sMessage = "Must provide either claim number or claim ID"
            GoTo Finally_Renamed
        End If

        If v_lClaimID = 0 Then
            result = GetValueFromTable(v_sTableName:="Claim", v_vReturnColumn:="claim_id", v_sKeyColumn:="claim_number", v_sKeyValue:=v_sClaimNumber, v_lDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=vResultArray)

            If result = gPMConstants.PMEReturnCode.PMTrue Then
                v_lClaimID = CInt(vResultArray) 'This will only be an array if we expect multiple columns back
            Else
                sMessage = "Failed to get claim ID for claim number " & v_sClaimNumber
                GoTo Finally_Renamed
            End If
        Else
            If v_sClaimNumber = "" Then
                result = GetValueFromTable(v_sTableName:="Claim", v_vReturnColumn:="claim_number", v_sKeyColumn:="claim_id", v_sKeyValue:=CStr(v_lClaimID), v_lDataType:=gPMConstants.PMEDataType.PMLong, r_vResult:=vResultArray)
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    v_sClaimNumber = vResultArray.Trim() 'This will only be an array if we expect multiple columns back
                Else
                    sMessage = "Failed to get claim number for claim ID " & v_lClaimID
                    GoTo Finally_Renamed
                End If
            End If
        End If

        'lock this claim to stop other user from processing it (this will be unlock in Change Claim Status routine)
        result = LockKey(v_sKeyName:="claim_id", v_lKeyValue:=v_lClaimID, v_lUserID:=m_iUserID, r_sLockedBy:=sLockBy)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then

            If sLockBy = "ERROR" Then
                sMessage = "Failed to lock claim (ClaimID = " & v_lClaimID & ")"
            Else
                sMessage = "ClaimID = " & v_lClaimID & " is being locked by " & sLockBy
            End If

            GoTo Finally_Renamed
        Else
            sLockBy = "Locked"
        End If

        'copy claim to work table

        result = m_oFindClaim.CopyClaimToWork(v_lClaimID:=v_lClaimID, r_lWorkClaimId:=lWorkClaimID)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to copy claim to work tables ClaimID = " & v_lClaimID & " to work tables"
            GoTo Finally_Renamed
        End If
        bWorkClaim = True


        m_oChangeClaimStatus.WorkClaimID = lWorkClaimID

        'this_revision = total reserve and this_payment total payment
        If UpdateThisRevisionPayment(lWorkClaimID, sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If

        'set original claim id = null to force reinsurance copy from policy
        result = ResetOrigClaimID(lWorkClaimID)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to reset original claim id to force copy of reinsurance from policy"
            GoTo Finally_Renamed
        End If

        'copy reinsurance from policy

        m_oReinsurance.ClaimId = lWorkClaimID

        result = m_oReinsurance.CalculateRI

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to do reinsurance for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        'put original claim id back
        If UpdateOriginalClaimID(lWorkClaimID, v_lClaimID, sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
            GoTo Finally_Renamed
        End If


        result = m_oChangeClaimStatus.CopyWorkToClaim()

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to copy work to claim for claim number " & v_sClaimNumber
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed

Catch_Renamed:
        result = gPMConstants.PMEReturnCode.PMError

        If sMessage = "" Then
            sMessage = "Failed to copy reinsurance from policy to claim"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRIToClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If bWorkClaim Then

            m_lReturn = m_oChangeClaimStatus.DeleteWorkClaim()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = sMessage & Strings.Chr(13) & Strings.Chr(10) & "Failed to delete work claim for claim number " & v_sClaimNumber
            End If
        End If

        If sLockBy = "Locked" Then
            m_lReturn = UnLockKey(v_sKeyName:="claim_id", v_lKeyValue:=v_lClaimID, v_lUserID:=m_iUserID)
        End If

        If Not False Then
            r_sMessage = sMessage
        End If

        Return result
    End Function

    '******************************************************************************
    ' reset original claim id to force reinsurance copy from policy
    '******************************************************************************
    Private Function ResetOrigClaimID(ByVal v_lWorkClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add param ClaimID"
                Throw New Exception()
            End If

            If m_oDatabase.SQLAction(sSQL:=ACUpdateResetOrigClaimIDSQL, sSQLName:=ACUpdateResetOrigClaimIDName, bStoredProcedure:=ACUpdateResetOrigClaimIDStored) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed reset original claim id"
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception



            If sMessage <> "" Then
                sMessage = "Failed to update work claim table and reset original claim id"
            End If

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ResetOrigClaimID()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)



            Return result
        End Try
    End Function

    '******************************************************************************
    ' get all claims with no reinsurance
    '******************************************************************************
    Public Function GetNoRIClaim(ByRef r_vResultArray(,) As Object, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.SQLSelect(sSQL:=ACGetNoRIClaimSQL, sSQLName:=ACGetNoRIClaimName, bStoredProcedure:=ACGetNoRIClaimStored, vResultArray:=r_vResultArray)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to retrieve claims with no reinsurance"
                Return result
            End If

            If Not Information.IsArray(r_vResultArray) Then
                sMessage = "No data found"
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            If sMessage <> "" Then
                sMessage = "Failed to get claims with no reinsurance"
            End If

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetNoRIClaim()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)



            Return result
        End Try
    End Function

    '******************************************************************************
    ' Update original claim id
    '******************************************************************************
    Private Function UpdateOriginalClaimID(ByVal v_lWorkClaimID As Integer, ByVal v_lOriginalClaimID As Integer, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param WorkClaimID"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lOriginalClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ClaimID"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.SQLAction(sSQL:=ACUpdateOrigClaimIDSQL, sSQLName:=ACUpdateOrigClaimIDName, bStoredProcedure:=ACUpdateOrigClaimIDStored) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to update original claim id to work claim"
            GoTo Catch_Renamed
        End If


        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        If sMessage <> "" Then
            sMessage = "Failed to update original claim id"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateOriginalClaimID()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If Not False Then
            r_sMessage = sMessage
        End If
        Return result
    End Function


    '******************************************************************************
    ' update work this_revision and this_payment
    ' with total reserve and total payment so when we copy reinsurance over from policy
    ' the ri_band reserve and payment is set correctly
    '******************************************************************************
    Private Function UpdateThisRevisionPayment(ByVal v_lWorkClaimID As Integer, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param WorkClaimID"
            GoTo Finally_Renamed
        End If

        If m_oDatabase.SQLAction(sSQL:=ACUpdateThisReservePaymentSQL, sSQLName:=ACUpdateThisReservePaymentName, bStoredProcedure:=ACUpdateThisReservePaymentStored) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to set this_revision and this_payment"
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed
Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        If sMessage <> "" Then
            sMessage = "Failed to update this_reserve and this_payment"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateThisRevisionPayment()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If Not False Then
            r_sMessage = sMessage
        End If

        Return result
    End Function

    '******************************************************************************
    ' adjust ri_band reserve/payment so that the repost of reserve/payment won't cause
    ' ri_band <> 100%
    '******************************************************************************
    Private Function UpdateRIBand(ByVal v_lWorkClaimID As Integer, ByVal v_lRIBand As Integer, ByVal v_cReserveAmount As Decimal, ByVal v_cPaymentAmount As Decimal, Optional ByRef r_sMessage As String = "") As Integer
        Dim result As Integer = 0
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="ReserveAmount", vValue:=CStr(v_cReserveAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ReserveAmount"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="PaymentAmount", vValue:=CStr(v_cPaymentAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ReserveAmount"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="WorkClaimID", vValue:=CStr(v_lWorkClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ReserveAmount"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="RIBand", vValue:=CStr(v_lRIBand), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ReserveAmount"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.SQLAction(sSQL:=ACUpdateRIBandReservePaymentSQL, sSQLName:=ACUpdateRIBandReservePaymentName, bStoredProcedure:=ACUpdateRIBandReservePaymentStored) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Fail to adjust ri_band reserve/payment"
            GoTo Catch_Renamed
        End If

        GoTo Finally_Renamed
Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        If sMessage <> "" Then
            sMessage = "Failed to update ri_band reserve/payment"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRIBand()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If Not False Then
            r_sMessage = sMessage
        End If

        Return result
    End Function

    '******************************************************************************
    'update arrangement line total (real table)
    '******************************************************************************
    Private Function UpdateClaimRIArrangmentLine(ByVal v_lClaimID As Integer, ByVal v_lBandID As Integer, ByVal v_cReserveAmount As Decimal, ByVal v_cPaymentAmount As Decimal, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ClaimID"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="RIBand", vValue:=CStr(v_lBandID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param RIBand"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="ReserveAmount", vValue:=CStr(v_cReserveAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ReserveAmount"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.Parameters.Add(sName:="PaymentAmount", vValue:=CStr(v_cPaymentAmount), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param PaymentAmount"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.SQLAction(sSQL:=ACUpdateClaimRIArrangementLineSQL, sSQLName:=ACUpdateClaimRIArrangementLineName, bStoredProcedure:=ACUpdateClaimRIArrangementLineStored) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to update Claim_RI_Arrangement_Line"
            GoTo Catch_Renamed
        End If

        GoTo Finally_Renamed
Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        If sMessage <> "" Then
            sMessage = "Failed to adjust Claim_RI_Arrangement_Line"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimRIArrangmentLine()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If Not False Then
            r_sMessage = sMessage
        End If

        Return result
    End Function

    '******************************************************************************
    ' Get band_id for this reserve
    '******************************************************************************
    Private Function GetReserveBand(ByVal v_lOriginalReserveID As Integer, ByRef r_lBandID As Integer, Optional ByRef r_sMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object
        Dim sMessage As String = ""

        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="ReserveID", vValue:=CStr(v_lOriginalReserveID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add param ReserveID"
            GoTo Catch_Renamed
        End If

        If m_oDatabase.SQLSelect(sSQL:=ACGetBandIDForReserveSQL, sSQLName:=ACGetBandIDForReserveName, bStoredProcedure:=ACGetBandIDForReserveStored, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to get band_id for reserve_id " & v_lOriginalReserveID
            GoTo Catch_Renamed
        End If

        If Information.IsArray(vResultArray) Then

            r_lBandID = CInt(vResultArray(0, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
            sMessage = "Band_ID cannot be found"
            GoTo Finally_Renamed
        End If

        GoTo Finally_Renamed
Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError

        If sMessage <> "" Then
            sMessage = "Failed to retrieve band_id for reserve"
        End If

        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveBand()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:

        If Not False Then
            r_sMessage = sMessage
        End If

        Return result
    End Function


    Public Function CreateReverseStats(ByVal v_nInsuranceFileCnt As Integer, ByRef r_nStatsFolderCnt As Integer, Optional ByRef v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        Dim IsClonedReverse As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Left(v_sDocumentRef, 3) = "SDD" Or Left(v_sDocumentRef, 3) = "SDR" Then
                IsClonedReverse = 1
            Else
                IsClonedReverse = 0
            End If


            m_lReturn = CreateReverseStatsFolder(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, r_nStatsFolderCnt:=r_nStatsFolderCnt, v_nIsCloneReverse:=IsClonedReverse, v_sDocumentRef:=v_sDocumentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", CreateReverseStats, CreateReverseStatsFolder Failed")
            End If

            m_lReturn = CreateReverseStatsDetails(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, v_nStatsFolderCnt:=r_nStatsFolderCnt, v_sDocumentRef:=v_sDocumentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", CreateReverseStats, CreateReverseStatsDetails Failed")
            End If

            Return m_lReturn

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStats", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Private Function CreateReverseStatsFolder(ByVal v_nInsuranceFileCnt As Integer, ByRef r_nStatsFolderCnt As Integer, Optional v_nIsCloneReverse As Integer = 0, Optional ByVal v_sDocumentRef As String = "") As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        Dim sNextOrionDocRef As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetNextOrionDocRef(r_sDocumentRef:=sNextOrionDocRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", CreateReverseStatsFolder, GetNextOrionDocRef Failed")
            End If


            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="next_orion_doc_ref", vValue:=sNextOrionDocRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                m_lReturn = .Parameters.Add(sName:="is_cloned_reverse", vValue:=CStr(v_nIsCloneReverse), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                m_lReturn = .Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End With

            ' Execute Add Stats Folder SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsFolderRevesrseSQL, sSQLName:=ACAddStatsFolderRevesrseName, bStoredProcedure:=ACAddStatsFolderRevesrseStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Cnt of the record inserted
            r_nStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value
            If r_nStatsFolderCnt < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateReverseStatsFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsFolder", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Private Function CreateReverseStatsDetails(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nStatsFolderCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            With m_oDatabase


                If v_sDocumentRef <> "" Then
                    m_lReturn = .Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    m_lReturn = .Parameters.Add(sName:="StatsFolderCnt", vValue:=v_nStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Copy_Stats_for_Reversal_By_DocumentRef", sSQLName:=ACAddStatsDetailsReverseName, bStoredProcedure:=ACAddStatsDetailsReverseStored, lRecordsAffected:=lRecordsAffected)
                Else
                    m_lReturn = .Parameters.Add(sName:="nNewStatsFolderCnt", vValue:=v_nStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .Parameters.Add(sName:="nInsuranceFileCnt", vValue:=v_nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsDetailsReverseSQL, sSQLName:=ACAddStatsDetailsReverseName, bStoredProcedure:=ACAddStatsDetailsReverseStored, lRecordsAffected:=lRecordsAffected)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Private Function GetNextOrionDocRef(ByRef r_sDocumentRef As String) As Integer

        Dim result As Integer = 0
        Dim sRange As String = "SND"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oACTAutoNumber As Object
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oACTAutoNumber, v_sClassName:="bACTAutoNumber.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetNextOrionRef", "Initilizing the componenet bACTAutoNumber.Business failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            'm_lReturn = oACTAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:="SND", v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=r_sDocumentRef)

            m_lReturn = oACTAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=sRange, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=r_sDocumentRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetNextOrionRef", "Method GenerateDocumentReferenceNumber failed", gPMConstants.PMELogLevel.PMLogError)
                Return result
            End If

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextOrionDocRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextOrionDocRef", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Function ReverseDocument(ByVal v_sDocumentRef As String) As Integer

        Dim nDocumentID As Integer
        Dim aoResultArray(,) As Object
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sAppName As String = "bAllocationPost"

        Try

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentDetailsSQL, _
                                                sSQLName:=ACGetDocumentDetailsName, _
                                                bStoredProcedure:=ACGetDocumentDetailsStored, _
                                                vResultArray:=aoResultArray, _
                                                bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not IsArray(aoResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nDocumentID = CLng(aoResultArray(0, 0))

            Dim oDocumentReversal As Object

            result = gPMComponentServices.CreateBusinessObject(r_oObject:=oDocumentReversal, _
                                                                v_sClassName:="bACTDocumentReversal.Business", _
                                                                v_sCallingAppName:=sAppName, _
                                                                v_sUsername:=m_sUsername$, _
                                                                v_sPassword:=m_sPassword$, _
                                                                v_iUserID:=m_iUserID%, _
                                                                v_iSourceID:=m_iSourceID%, _
                                                                v_iLanguageID:=m_iLanguageID%, _
                                                                v_iCurrencyID:=m_iCurrencyID%, _
                                                                v_iLogLevel:=m_iLogLevel%, _
                                                                v_oDatabase:=m_oDatabase)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception

            End If

            oDocumentReversal.DocumentId = nDocumentID
            result = oDocumentReversal.Start()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            oDocumentReversal.Dispose()
            oDocumentReversal = Nothing

            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextOrionDocRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try

    End Function

    ''' <summary>
    ''' RIRefresh
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="sTransactionType"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RIRefresh(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String) As Integer

        Dim lCount As Long
        Dim m_vIsRI2007 As Object
        Dim lRiskCnt As Long
        Dim aoRiskArray(,) As Object
        Dim oReinsurance As Object
        Dim lValidRIBand, lReinsBand As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            Dim temp_oReinsurance As Object
            nResult = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=m_vIsRI2007)

            If m_vIsRI2007 <> "1" Then
                'Create bSIRReinsurance.Form object
                nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=temp_oReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            Else
                nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=temp_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If
            oReinsurance = temp_oReinsurance
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RIRefresh", "Failed to instance of Reinsurance", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            With m_oDatabase
                nResult = .Parameters.Add(sName:="InsuranceFileCnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                nResult = .SQLSelect(sSQL:=ACGetRiskCntForInsuranceFileCntSQL,
                                       sSQLName:=ACGetRiskCntForInsuranceFileCntName,
                                       bStoredProcedure:=ACGetRiskCntForInsuranceFileCntStored,
                                       vResultArray:=aoRiskArray)
            End With
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RIRefresh", "Failed to get risk cnt for insurance file cnt", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            If IsArray(aoRiskArray) Then
                For lCount = 0 To UBound(aoRiskArray, 2)

                    nResult = SetProcessModes(vTask:=1, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
                    oReinsurance.InsuranceFileCnt = CLng(nInsuranceFileCnt)
                    oReinsurance.RiskId = CLng(aoRiskArray(0, lCount))
                    'If oReinsurance.CalculateRI() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If CalculateRI(CLng(nInsuranceFileCnt), CLng(aoRiskArray(0, lCount))) <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed", gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If

                    nResult = oReinsurance.GetDetails()
                    If nResult = 811 Or nResult = 1 Then
                        nResult = 1
                    Else
                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed", gPMConstants.PMELogLevel.PMLogError)
                            Return nResult
                        End If
                    End If
                    ' Save new reinsurance details

                    If oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed", gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If

                    ' Do we have valid reinsurance bands ie adds up to 100%

                    If oReinsurance.ValidateBands(r_lValid:=lValidRIBand, r_lBand:=lReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed to validate RI for Risk cnt" & lRiskCnt, gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If

                    If lValidRIBand <> 0 Then
                        gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed to validate RI for Risk cnt" & lRiskCnt, gPMConstants.PMELogLevel.PMLogError)
                        Return nResult
                    End If
                Next lCount
            End If
            oReinsurance.Dispose()
            oReinsurance = Nothing
            Return nResult

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RIRefresh Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RIRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try

    End Function

    ''' <summary>
    ''' RIRefresh for single risk
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="sTransactionType"></param>
    ''' <param name="nRiskCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RIRefreshForSingleRisk(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String, ByVal nRiskCnt As Integer) As Integer

        'Dim lCount As Long
        Dim m_vIsRI2007 As Object
        'Dim lRiskCnt As Long
        'Dim aoRiskArray(,) As Object
        Dim oReinsurance As Object
        Dim lValidRIBand, lReinsBand As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try

            Dim temp_oReinsurance As Object
            nResult = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=m_vIsRI2007)

            If m_vIsRI2007 <> "1" Then
                'Create bSIRReinsurance.Form object
                nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=temp_oReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            Else
                nResult = gPMComponentServices.CreateBusinessObject(r_oObject:=temp_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If
            oReinsurance = temp_oReinsurance
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RIRefresh", "Failed to instance of Reinsurance", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            'With m_oDatabase
            '    nResult = .Parameters.Add(sName:="InsuranceFileCnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            '    nResult = .SQLSelect(sSQL:=ACGetRiskCntForInsuranceFileCntSQL,
            '                           sSQLName:=ACGetRiskCntForInsuranceFileCntName,
            '                           bStoredProcedure:=ACGetRiskCntForInsuranceFileCntStored,
            '                           vResultArray:=aoRiskArray)
            'End With
            'If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError("RIRefresh", "Failed to get risk cnt for insurance file cnt", gPMConstants.PMELogLevel.PMLogError)
            '    Return nResult
            'End If

            'If IsArray(aoRiskArray) Then
            'For lCount = 0 To UBound(aoRiskArray, 2)

            nResult = SetProcessModes(vTask:=1, vTransactionType:=sTransactionType, vEffectiveDate:=DateTime.Now)
            oReinsurance.InsuranceFileCnt = CLng(nInsuranceFileCnt)
            oReinsurance.RiskId = CLng(nRiskCnt)
            'If oReinsurance.CalculateRI() <> gPMConstants.PMEReturnCode.PMTrue Then
            If CalculateRI(CLng(nInsuranceFileCnt), CLng(nRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            nResult = oReinsurance.GetDetails()
            If nResult = 811 Or nResult = 1 Then
                nResult = 1
            Else
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed", gPMConstants.PMELogLevel.PMLogError)
                    Return nResult
                End If
            End If
            ' Save new reinsurance details

            If oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed", gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            ' Do we have valid reinsurance bands ie adds up to 100%

            If oReinsurance.ValidateBands(r_lValid:=lValidRIBand, r_lBand:=lReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed to validate RI for Risk cnt" & nRiskCnt, gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If

            If lValidRIBand <> 0 Then
                gPMFunctions.RaiseError("RIRefresh", "Method RIRefresh failed to validate RI for Risk cnt" & nRiskCnt, gPMConstants.PMELogLevel.PMLogError)
                Return nResult
            End If
            'Next lCount
            'End If
            oReinsurance.Dispose()
            oReinsurance = Nothing
            Return nResult

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RIRefresh Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RIRefresh", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return nResult

        End Try

    End Function

    Public Function UpdateRiskStatus(ByVal v_lRiskCnt As Integer, ByVal v_sRiskStatusCode As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.Parameters.Add(sName:="risk_status_code", vValue:=v_sRiskStatusCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            result = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskStatusSQL, sSQLName:=ACUpdateRiskStatusName, bStoredProcedure:=ACUpdateRiskStatusStored)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTaxCalc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try

    End Function

    Public Function UpdateTaxCalc(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            result = m_oDatabase.SQLAction(sSQL:="spu_Update_TaxCalculation_DataFix", sSQLName:="UpdateTaxCalculation", bStoredProcedure:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTaxCalc Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTaxCalc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try

    End Function

    Public Function UpdateFileType(ByVal nInsuranceFileCnt As Integer, ByVal nFileTypeId As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insuranceFilecnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.Parameters.Add(sName:="insuranceFileTypeId", vValue:=nFileTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.SQLAction(sSQL:="spu_update_insurance_file_type_datafix", sSQLName:="UpdateInsuranceFileType", bStoredProcedure:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFileType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFileType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try

    End Function

    Public Function CreateTransExportReverse(ByVal v_lStatsFolderCnt As Integer, ByVal v_lTransactionExportFolderCnt As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Dim bMissingExistingTransaction As Boolean = False
        Dim aoRiskArray(,) As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            result = m_oDatabase.SQLSelect(sSQL:=ACGetTrasanctionExportDetailSQL,
                                   sSQLName:=ACGetTrasanctionExportDetailName,
                                   bStoredProcedure:=ACGetTrasanctionExportDetailStored,
                                   vResultArray:=aoRiskArray)

            With m_oDatabase
                .Parameters.Clear()
                If v_sDocumentRef <> "" AndAlso IsArray(aoRiskArray) Then
                    m_lReturn = .Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = .Parameters.Add(sName:="transactiontExportFolderCnt", vValue:=v_lTransactionExportFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = .SQLAction(sSQL:=ACAddTransExportDetailsReverseSQL, sSQLName:=ACAddTransExportDetailsReverseName, bStoredProcedure:=ACAddTransExportDetailsReverseStored, lRecordsAffected:=lRecordsAffected)
                Else
                    m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = .SQLAction(sSQL:=ACAddExportDetailsSQL, sSQLName:=ACAddExportDetailsName, bStoredProcedure:=ACAddExportDetailsStored)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Function ValidateDocumentRef(ByVal sDocumentRef As String, ByRef vResultArray As Object) As Long


        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            result = m_oDatabase.SQLSelect( _
          sSQL:="SELECT transdetail_id FROM TransDetail td JOIN document d on td.document_id=d.document_id where d.document_ref='" & sDocumentRef & "'", _
          sSQLName:="SELECTDocumentID", _
          bStoredProcedure:=False, _
          lNumberRecords:=0, _
          vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateDocumentRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try

    End Function

    
    Public Function ValidateRiskForOriginalLine(ByVal nRiskCnt As Integer, ByVal nInsuranceFileCnt As Integer, ByRef nOriginalLegExist As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        Dim vResultArray(,) As Object

        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.SQLSelect(sSQL:="spu_ValidateRisk_DataFix", sSQLName:="ValidateRiskForOriginalLine", bStoredProcedure:=True, vResultArray:=vResultArray)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Information.IsArray(vResultArray) Then
                nOriginalLegExist = CInt(vResultArray(0, 0))
            End If


            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateRiskForOriginalLine Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateRiskForOriginalLine", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try

    End Function


    Public Function InsertRatingSectionAndPeril(ByVal nRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            result = m_oDatabase.SQLAction(sSQL:="spu_InsertRatingSectionPeril_DataFix", sSQLName:="InsertRatingSectionAndPeril", bStoredProcedure:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsertRatingSectionAndPeril Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="InsertRatingSectionAndPeril", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try

    End Function

    Public Function ReverseAllocation(ByVal lTransDetailId As Long) As Long
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()
            result = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=lTransDetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.SQLAction(sSQL:="spu_reverse_SRP", sSQLName:="CancelSRP", bStoredProcedure:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result
        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try

    End Function


    Public Function ProcessClaimTransactions(ByVal v_lClaimId As Long, ByVal v_sDocumentRef As String, ByVal v_sRefNumber As String, Optional ByVal v_bRIRefresh As Boolean = False, Optional ByVal v_bRePost As Boolean = False) As Long

        ProcessClaimTransactions = gPMConstants.PMEReturnCode.PMTrue
        Dim sTransactionCode As String = ""
        Dim bIsOnlyRIRefresh As Boolean

        m_lReturn = GetTransactionTypeCode(v_sDocumentRef:=v_sDocumentRef, r_Transaction_Type_Code:=sTransactionCode)

        If Left(v_sDocumentRef, 3) = "" And v_bRIRefresh Then
            bIsOnlyRIRefresh = True
        ElseIf Left(v_sDocumentRef, 3) = "" Then
            Return 0
            Exit Function
        End If

        ' Call BeginTrans()
        m_lReturn = m_oDatabase.SQLBeginTrans
        If Not bIsOnlyRIRefresh Then
            m_lReturn = ReverseClaimTransactionsbyDocumentRef(v_lClaimId, v_sDocumentRef, sTransactionCode, v_sRefNumber)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ProcessClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                ' Call RollbackTrans()
                m_lReturn = m_oDatabase.SQLRollbackTrans
                Exit Function
            End If
        End If

        If v_bRIRefresh Then
            m_lReturn = RecalculateRI(v_lClaimId)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                m_lReturn = m_oDatabase.SQLRollbackTrans
                Exit Function
            End If
        End If

        If v_bRePost Then
            If Not bIsOnlyRIRefresh Then
                If Left(v_sDocumentRef, 3) = "CLD" Or Left(v_sDocumentRef, 3) = "CLC" Then
                    m_lReturn = RePostClonedClaimTransaction(v_lClaimId:=v_lClaimId, v_sDocumentRef:=v_sDocumentRef, v_sRefNumber:=v_sRefNumber, v_sTransactionType:=sTransactionCode)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ProcessClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                    ' Call RollbackTrans()
                    m_lReturn = m_oDatabase.SQLRollbackTrans
                    Exit Function
                End If
            End If
        End If

        ' Call CommitTrans()
        m_lReturn = m_oDatabase.SQLCommitTrans
    End Function

    Private Function ReverseClaimTransactionsbyDocumentRef(v_lClaimId As Long, v_sDocumentRef As String, ByVal v_sTransactionTypeCode As String, ByVal v_sRefNumber As String) As Long

        Const kMethodName As String = "ProcessTransactions"

        Dim lTransactionExportFolderCnt As Long
        Dim lDepositExportFolderCnt As Long
        Dim iPaymentOption As Integer
        Dim iOriginalPaymentOption As Integer
        Dim lInsuranceFolderCnt As Long             ' insurnace_folder_cnt
        Dim sAuthAccountsTransOptionVal As String
        Dim m_lReturn As Long
        Dim sOptionValue As String
        Dim lReturn As Long
        Dim lStatsFolderCnt As Long
        Dim lDocumentID As Long
        Dim vTransactions As Object
        Dim lCount As Integer
        Dim sSQL As String
        Dim result As Integer = 0
        Try
            ReverseClaimTransactionsbyDocumentRef = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn& = CreateClaimStatsReverse(lStatsFolderCnt, v_lClaimId, v_sDocumentRef, v_sTransactionTypeCode)
            If (m_lReturn& <> gPMConstants.PMEReturnCode.PMTrue) Then
                ReverseClaimTransactionsbyDocumentRef = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = AddDataFixUtilityLog(v_sPMNumber:=v_sRefNumber, v_sCreatedBy:="DataFixUtility", v_ClaimId:=v_lClaimId, _
             v_sOldDocumentRef:=v_sDocumentRef, v_sNewDocumentid:=lDocumentID, v_bIsReversal:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReverseClaimTransactionsbyDocumentRef = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("Document_Id", lDocumentID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction("spu_ACT_Add_TransDetailEx", "Add Collection Buckets", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReverseClaimTransactionsbyDocumentRef = m_lReturn
            End If

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseClaimTransactionsbyDocumentRef", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseAllocation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result       
        End Try
    End Function

    Private Function CreateClaimStatsReverse(lStatsFolderCnt As Long, lClaimId As Long, sDocumentRef As String, sTransactionTypeCode As String) As Long

        Dim lNewStatsFolderCnt As Long
        Dim vIdent As Object
        Dim m_lReturn As Long
        Dim sNextOrionDocRef As String
        Dim lRecordsAffected As Long
        Dim IsClonedReverse As Boolean
        Dim sSQL As String
        Try

            CreateClaimStatsReverse = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateClaimStatsFolder(lStatsFolderCnt, lClaimId, sTransactionTypeCode)
            If (m_lReturn& <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateClaimStatsReverse = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            lNewStatsFolderCnt = lStatsFolderCnt

            m_lReturn = ReverseClaimStatsDetail(sDocumentRef, lNewStatsFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CreateClaimStatsReverse = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            m_lReturn = ReverseClaimTransactions(v_lClaimId:=lClaimId, v_lStatsFolderCnt:=lNewStatsFolderCnt, m_sTransactionCode:=sTransactionTypeCode, sDocumentRef:=sDocumentRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CreateClaimStatsReverse = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            CreateClaimStatsReverse = m_lReturn

            Exit Function

        Catch excep As Exception
            CreateClaimStatsReverse = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateClaimStatsReverse", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateClaimStatsReverse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return m_lReturn
        End Try


    End Function

    Private Function CreateClaimStatsFolder( _
    ByRef lStatsFolderCnt As Long, lClaimId As Long, sTransactionTypeCode As String) As Long

        Dim lRecordsAffected As Long
        Dim vntResult As Object
        Dim m_lReturn As Long

        Try

            CreateClaimStatsFolder = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", _
                                                      vValue:=0, _
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, _
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="transaction_type_code", _
                                                      vValue:=sTransactionTypeCode, _
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                    iDataType:=gPMConstants.PMEDataType.PMString)


                m_lReturn = .Parameters.Add(sName:="user_id", _
                                                      vValue:=m_iUserID, _
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                    iDataType:=gPMConstants.PMEDataType.PMInteger)


                m_lReturn = .Parameters.Add(sName:="claim_id", _
                                                      vValue:=lClaimId, _
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)

            End With

            ' Execute Add Stats Folder SQL Statement
            m_lReturn = m_oDatabase.SQLAction( _
                sSQL:="spu_clm_add_stats_folder", _
                sSQLName:="AddStatsFolderClaims", _
                bStoredProcedure:=True, _
                lRecordsAffected:=lRecordsAffected)


            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateClaimStatsFolder = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Get the Cnt of the record inserted
            lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value
            If (lStatsFolderCnt < 1) Then
                CreateClaimStatsFolder = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            Exit Function

        Catch excep As Exception
            CreateClaimStatsFolder = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateClaimStatsFolder", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateClaimStatsReverse", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return m_lReturn
        End Try


    End Function


    Public Function ReverseClaimStatsDetail(sDocumentRef As String, ByVal lNewStatsFolderCnt As Long) As Long
        ' Const kMethodName As String = "ReverseClaimStatsDetail"

        Dim m_lReturn As Long


        Try

            ReverseClaimStatsDetail = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add( _
                    sName:="OldDocument_ref", _
                    vValue:=sDocumentRef, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReverseClaimStatsDetail = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add( _
                    sName:="New_Stats_Folder_Cnt", _
                    vValue:=lNewStatsFolderCnt, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReverseClaimStatsDetail = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLAction( _
                                    sSQL:="spu_Copy_Stats_Reversal_Claim_By_Document_Ref", _
                                    sSQLName:="spu_Copy_Stats_Reversal_Claim_By_Document_Ref", _
                                    bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ReverseClaimStatsDetail = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch excep As Exception
            ReverseClaimStatsDetail = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseClaimStatsDetail", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseClaimStatsDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return m_lReturn
        End Try

    End Function


    Public Function ReverseClaimTransactions(ByVal v_lClaimId As Long, ByVal v_lStatsFolderCnt As Long, ByVal m_sTransactionCode As String, ByVal sDocumentRef As String) As Long

        Const kMethodName As String = "ReverseTransactions"

        Dim oObject As Object
        Dim vArray As Object
        Dim bStatsSuppressed As Boolean
        Dim r_lDocumentId As Long

        Dim oAllocationManual As Object
        Dim lRow As Long
        Dim lTransDetailId As Long
        Dim vCreditTransactions As Object

        Dim vKeyArray(1, 3) As Object
        Dim lRecordsAffected As Long
        Dim lDocumentID As Long
        Dim lCount As Long
        Dim vTrans(0) As Object
        Dim sSQL As String
        Dim vTransactions As Object
        Dim m_lReturn As Long
        Dim lTransactionTypeId As Long
        Dim IsClonedReversal As Integer
        Dim lcloned As Integer

        Try

            ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, _
                                                                v_sClassName:="bControlTransClaims.Automated", _
                                                                v_sCallingAppName:=ACApp, _
                                                                v_sUsername:=m_sUsername, _
                                                                v_sPassword:=m_sPassword, _
                                                                v_iUserID:=m_iUserID, _
                                                                v_iSourceID:=m_iSourceID, _
                                                                v_iLanguageID:=m_iLanguageID, _
                                                                v_iCurrencyID:=m_iCurrencyID, _
                                                                v_iLogLevel:=m_iLogLevel, _
                                                                v_oDatabase:=m_oDatabase)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalsegPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            oObject.ClaimId = v_lClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", _
                                                   vValue:=m_sTransactionCode, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL, _
                                              sSQLName:=ACGetTransactionTypeIDName, _
                                              bStoredProcedure:=True, _
                                              lNumberRecords:=gPMConstants.PMAllRecords, _
                                              vResultArray:=vArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(vArray) Then
                oObject.TransactionTypeId = vArray(0, 0)
                lTransactionTypeId = vArray(0, 0)
            Else
                ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            oObject.TransactionTypeCode = m_sTransactionCode
            oObject.IsClonedReversal = 1

            If Left(sDocumentRef, 3) = "CLC" Then
                IsClonedReversal = 1
                lcloned = 0
                oObject.IsClonedReversal = IsClonedReversal
            ElseIf Left(sDocumentRef, 3) = "CLD" Then
                lcloned = 1
                IsClonedReversal = 1
                'oObject.IsClonedReversal=IsClonedReversal
            Else
                IsClonedReversal = 0
                lcloned = 0
                oObject.IsClonedReversal = IsClonedReversal
            End If

            ' finalise the stats folders details and determine whether
            ' the transactions should be suppressed
            m_lReturn = FinaliseStats(v_lStatsFolderCnt, v_lClaimId, lTransactionTypeId, m_sTransactionCode, lcloned, IsClonedReversal, bStatsSuppressed, sDocumentRef)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' if the stats have not been suppressed
            If Not bStatsSuppressed Then

                m_lReturn = oObject.CreateTransactions(v_lStatsFolderCnt, _
                                                      r_lDocumentId)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            m_lReturn& = AllocateTransactions(v_sOldDocumentRef:=sDocumentRef, v_lNewDocumentid:=r_lDocumentId)

            ' m_lReturn = oObject.Terminate

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            oObject = Nothing

            Exit Function

        Catch excep As Exception
            ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseClaimStatsDetail", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseClaimStatsDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return m_lReturn
        End Try
        Exit Function

    End Function


    Public Function FinaliseStats(v_lStatsFolderCnt As Long, _
                                v_lClaimId As Long, v_lTransactionTypeID As Long, _
                                v_sTransactionTypeCode As String, _
                                v_lCloned As Integer, _
                                v_lReverseClone As Integer, _
                                Optional r_bStatsSuppressed As Boolean = False, Optional ByVal sDocumentRef As String = "") As Long

        Dim lStatsFolderCnt As Long
        Dim lTransactionExportFolderCnt As Long
        Dim m_lReturn As Long

        Try

            FinaliseStats = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", _
                                                   vValue:=v_lClaimId, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", _
                                                   vValue:=v_lTransactionTypeID, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", _
                                                   vValue:=v_sTransactionTypeCode, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", _
                                                   vValue:=v_lStatsFolderCnt, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bstatssuppressed", _
                                                   vValue:=0, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Left(sDocumentRef, 3) = "CLD" Or Left(sDocumentRef, 3) = "CLC" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_cloned", _
                                                       vValue:=v_lCloned, _
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                       iDataType:=gPMConstants.PMEDataType.PMInteger)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="is_cloned_reversal", _
                                               vValue:=v_lReverseClone, _
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                               iDataType:=gPMConstants.PMEDataType.PMInteger)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If



            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACFinaliseStatsSQl, _
                                              sSQLName:=ACFinaliseStatsName, _
                                              bStoredProcedure:=True)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                FinaliseStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsDBNull(m_oDatabase.Parameters.Item("bstatssuppressed").Value) Then
                m_oDatabase.Parameters.Item("bstatssuppressed").Value = 0
            End If

            r_bStatsSuppressed = m_oDatabase.Parameters.Item("bstatssuppressed").Value



            Exit Function
        Catch excep As Exception
            FinaliseStats = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FinaliseStats", vApp:=ACApp, vClass:=ACClass, vMethod:="FinaliseStats", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return m_lReturn
        End Try
        Exit Function

    End Function


    Private Function AllocateTransactions(ByVal v_sOldDocumentRef As String, ByVal v_lNewDocumentid As Long) As Long


        AllocateTransactions = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("old_document_ref", v_sOldDocumentRef, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        m_lReturn = m_oDatabase.Parameters.Add("new_document_id", v_lNewDocumentid, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction("spu_allocate_transactions_by_docref", "Allocate Transactions", True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            AllocateTransactions = m_lReturn
        End If

    End Function

    Public Function RePostClonedClaimTransaction(ByVal v_lClaimId As Long, _
                         ByVal v_sDocumentRef As String, _
                         ByVal v_sRefNumber As String, _
                         Optional ByRef v_sTransactionType As String = "") As Long


        Dim oObject As Object
        Dim m_lReturn As Long
        Dim lTransactionTypeId As Long
        Dim vArray As Object
        Try

            RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, _
                                                                    v_sClassName:="bSIRCloneRIBatchProcess.Business", _
                                                                    v_sCallingAppName:=ACApp, _
                                                                    v_sUsername:=m_sUsername, _
                                                                    v_sPassword:=m_sPassword, _
                                                                    v_iUserID:=m_iUserID, _
                                                                    v_iSourceID:=m_iSourceID, _
                                                                    v_iLanguageID:=m_iLanguageID, _
                                                                    v_iCurrencyID:=m_iCurrencyID, _
                                                                    v_iLogLevel:=m_iLogLevel, _
                                                                    v_oDatabase:=m_oDatabase)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'oObject.ClaimId = v_lClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", _
                                                   vValue:=v_sTransactionType, _
                                                   iDirection:=PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL, _
                                              sSQLName:=ACGetTransactionTypeIDName, _
                                              bStoredProcedure:=True, _
                                              lNumberRecords:=gPMConstants.PMAllRecords, _
                                              vResultArray:=vArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If IsArray(vArray) Then
                'oObject.TransactionTypeId = vArray(0, 0)
                lTransactionTypeId = vArray(0, 0)
            Else
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            m_sDocumentRef = v_sDocumentRef
            m_lReturn = RaiseClaimCloneTransaction(v_lClaimId, v_sTransactionType)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' m_lReturn = oObject.Terminate

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            oObject = Nothing

            m_lReturn = AddDataFixUtilityLog(v_sPMNumber:=v_sRefNumber, v_sCreatedBy:="DataFixUtility", v_ClaimId:=v_lClaimId, _
                                   v_sOldDocumentRef:=v_sDocumentRef, v_sNewDocumentid:=0, v_bIsReversal:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            Exit Function

        Catch excep As Exception
            RePostClonedClaimTransaction = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RePostClonedClaimTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:="RePostClonedClaimTransaction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return m_lReturn
        End Try
        Exit Function

    End Function

    ''' <summary>
    ''' this method raise stats
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="sTransactionTypeCode"></param>
    '''  <param name="bIsCloneReversal"></param>
    '''  <param name="nStatsFolderCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RaiseClaimCloneTransaction(ByVal nClaimId As Integer, Optional ByVal sTransactionTypeCode As String = "", Optional ByVal bIsCloneReversal As Boolean = False, Optional ByVal nStatsFolderCnt As Integer = 0) As Integer
        Const kMethodName As String = "RaiseClaimTransaction"

        Dim obCLMChangeClaimStatus As bCLMChangeClaimStatus.Business
        Dim nReturn As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Get an instance of bACTDocumentReversal

            obCLMChangeClaimStatus = New bCLMChangeClaimStatus.Business
            nReturn = obCLMChangeClaimStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = obCLMChangeClaimStatus.SetProcessModes(vTransactionType:=sTransactionTypeCode)

            If Not Information.IsNothing(sTransactionTypeCode) Then

                m_sTransactionType = CStr(sTransactionTypeCode)
            End If

            If bIsCloneReversal = True Then
                obCLMChangeClaimStatus.IsCloneReversal = True
                nReturn = DirectCast(obCLMChangeClaimStatus.RaiseClonedTransactions(nClaimId:=nClaimId, nStatsFolderCnt:=nStatsFolderCnt), Integer)
            Else
                obCLMChangeClaimStatus.IsCloned = 1
                Me.IsCloned = 1
                nReturn = DirectCast(RaiseTransactions(v_lClaimId:=nClaimId), Integer)
            End If
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue And nReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RaiseClaimCloneTransaction, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            obCLMChangeClaimStatus.Dispose()
            obCLMChangeClaimStatus = Nothing

        End Try
        Return nResult
    End Function

    Public Function RaiseTransactions(ByVal v_lClaimId As Integer) As Integer
        Return RaiseTransactions(v_lClaimId:=v_lClaimId, v_bSavedStats:=False, r_lDocumentId:=0, r_bFromSAM:=False)
    End Function

    ''' <summary>
    ''' RaiseTransactions
    ''' </summary>
    ''' <param name="v_lClaimId"></param>
    ''' <param name="v_bSavedStats"></param>
    ''' <param name="r_lDocumentId"></param>
    ''' <param name="r_bFromSAM"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RaiseTransactions(ByVal v_lClaimId As Integer, ByVal v_bSavedStats As Boolean, ByRef r_lDocumentId As Integer, ByRef r_bFromSAM As Boolean) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RaiseTransactions"

        Dim oObject As bControlTransClaims.Automated
        Dim oObjectbCLMChangeClaimStatus As bCLMChangeClaimStatus.Business
        Dim oArray(,) As Object
        Dim nStatsFolderCnt As Integer
        Dim bStatsSuppressed As Boolean
        Dim oStatsFolderCnt(,) As Object = Nothing
        Dim bPaymentRefCheckEnabled As Boolean
        Dim nRecordsAffected As Integer
        Dim nReserveAmount As Decimal
        Try

            nResult = PMEReturnCode.PMTrue

            oObject = New bControlTransClaims.Automated
            oObjectbCLMChangeClaimStatus = New bCLMChangeClaimStatus.Business
            m_lReturn = oObject.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = oObjectbCLMChangeClaimStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = oObjectbCLMChangeClaimStatus.SetProcessModes(vTransactionType:=m_sTransactionType)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            oObject.ClaimID = v_lClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=m_sTransactionType, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL, sSQLName:=ACGetTransactionTypeIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=oArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If IsArray(oArray) Then

                oObject.TransactionTypeID = oArray(0, 0)
            Else
                Return PMEReturnCode.PMFalse
            End If

            oObject.TransactionTypeCode = m_sTransactionType

            m_lReturn = oObjectbCLMChangeClaimStatus.GetProductDetails(v_lProductId:=v_lClaimId, r_bPaymentRefCheck:=bPaymentRefCheckEnabled)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetSystemOption Payment Ref Check Failed", PMELogLevel.PMLogError)
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ReserveAmount", _
                                                   vValue:=0, _
                                                   iDirection:=PMEParameterDirection.PMParamOutput, _
                                                   iDataType:=PMEDataType.PMCurrency)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransactionType", _
                                                   vValue:=m_sTransactionType, _
                                                   iDirection:=PMEParameterDirection.PMParamInput, _
                                                   iDataType:=PMEDataType.PMString)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", _
                                                   vValue:=v_lClaimId, _
                                                   iDirection:=PMEParameterDirection.PMParamInput, _
                                                   iDataType:=PMEDataType.PMLong)

            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACGetReserveAmountSQL, _
                                              sSQLName:=ACGetReserveAmountName, _
                                              bStoredProcedure:=True, _
                                              lRecordsAffected:=nRecordsAffected)

            nReserveAmount = m_oDatabase.Parameters.Item("ReserveAmount").Value
            ' Lets check there are work_stats before trying to
            'process them.
            Dim nClaimPaymentID As Integer = 0
            Dim sCreditAccountCode As String
            Dim oTaxAmountByTaxType As Object = Nothing
            Dim sTaxTypeCode As String
            Dim crTaxAmount As Decimal
            Dim nlBound As Integer
            Dim nUBound As Integer
            Dim nTaxTypeItem As Integer
            Dim bThisRevesionPresent As Boolean

            Dim nStatsFolderCnt_for_ThisRevesionPresent As Integer = 0
            Dim nC_CP_DocumentID As Integer = 0

            If nReserveAmount <> 0 Then
                ''Get stats folder cnt
                If m_sTransactionType = "C_CO" OrElse m_sTransactionType = "C_CR" Then
                    'lTransactionTypeID = 26 'claim open
                    If IsCloned = 1 Or IsCloned = True Then
                        m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=m_sTransactionType, v_lClaimId:=v_lClaimId)

                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            gPMFunctions.RaiseError(kMethodName, "CreateStatsFolder  Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        sCreditAccountCode = "CLMRES"

                        '' Create GRS stats detail entries
                        m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, 0, False)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    Else
                        m_lReturn = oObjectbCLMChangeClaimStatus.GetStatsFolderForClaim(v_lClaimId, oStatsFolderCnt)
                        If oStatsFolderCnt Is Nothing Then
                            m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=m_sTransactionType, v_lClaimId:=v_lClaimId)

                            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                                gPMFunctions.RaiseError(kMethodName, "CreateStatsFolder  Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            sCreditAccountCode = "CLMRES"

                            '' Create GRS stats detail entries
                            m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, 0, False)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If


                Else

                    m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=m_sTransactionType, v_lClaimId:=v_lClaimId)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "CreateStatsFolder Payment Ref Check Failed", PMELogLevel.PMLogError)
                    End If

                    '' Create GRS stats detail entries
                    m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, nClaimPaymentID, bThisRevesionPresent)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", PMELogLevel.PMLogError)
                    End If
                    If Not IsCloned Then
                        ' get tax lines grouped by tax type for this payment
                        If m_sTransactionType = "C_CP" Then
                            m_lReturn = GetClaimTaxAmountsByTaxType(v_iClaimPaymentId:=nClaimPaymentID, _
                                                        r_vResults:=oTaxAmountByTaxType)
                        ElseIf m_sTransactionType = "C_SA" OrElse m_sTransactionType = "C_RV" Then
                            m_lReturn = GetClaimTaxAmountsByTaxType(v_iClaimReceiptId:=nClaimPaymentID, _
                                                        r_vResults:=oTaxAmountByTaxType)
                        End If
                    End If

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetClaimPaymentTaxAmountsByTaxType Failed", PMELogLevel.PMLogError)
                    End If

                    Dim m_vClaimDetails As Object
                    Dim bPostClaimTax As Boolean

                    m_lReturn = oObjectbCLMChangeClaimStatus.GetClaimDetails(v_lClaimId, m_vClaimDetails)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetClaimDetails Failed", PMELogLevel.PMLogError)
                    End If

                    If Not IsArray(m_vClaimDetails) Then
                        RaiseError(kMethodName, "GetClaimDetails Failed to return any data", PMELogLevel.PMLogError)
                    Else
                        bPostClaimTax = CBool(ToSafeLong(m_vClaimDetails(kClaimDetailPostClaimsTaxes, 0), 0))
                    End If

                    If (m_sTransactionType = "C_RV" OrElse m_sTransactionType = "C_SA") AndAlso bPostClaimTax Then
                        ' Create stats for gross tax amount

                        If IsArray(oTaxAmountByTaxType) Then

                            nlBound = LBound(oTaxAmountByTaxType, 2)
                            nUBound = UBound(oTaxAmountByTaxType, 2)

                            For nTaxTypeItem = nlBound To nUBound
                                crTaxAmount = crTaxAmount + oTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, nTaxTypeItem)
                            Next
                            nTaxTypeItem = 0

                            ' Insert stats details records for Tax (One gross line for each tax type)
                            If crTaxAmount <> 0 Then
                                m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "TAG", sCreditAccountCode, 0, False, crTaxAmount)

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", PMELogLevel.PMLogError)
                                End If
                            End If

                        End If
                        ' process the tax rows..
                        If IsArray(oTaxAmountByTaxType) Then

                            nlBound = LBound(oTaxAmountByTaxType, 2)
                            nUBound = UBound(oTaxAmountByTaxType, 2)

                            For nTaxTypeItem = nlBound To nUBound

                                ' get the tax type details
                                sTaxTypeCode = Trim$(oTaxAmountByTaxType(kTaxTypeArrayPosCode, nTaxTypeItem))
                                crTaxAmount = oTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, nTaxTypeItem)

                                ' Insert stats details records for Tax (One gross line for each tax type)
                                If crTaxAmount <> 0 Then

                                    ' set tan / tag account code
                                    sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                                    ' Create stats for TAN amount
                                    m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "TAN", sCreditAccountCode, 0, False, -crTaxAmount)

                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", PMELogLevel.PMLogError)
                                    End If

                                End If

                            Next

                        End If

                    ElseIf m_sTransactionType = "C_CP" Then

                        ' process the tax rows..
                        If IsArray(oTaxAmountByTaxType) Then

                            nlBound = LBound(oTaxAmountByTaxType, 2)
                            nUBound = UBound(oTaxAmountByTaxType, 2)

                            For nTaxTypeItem = nlBound To nUBound

                                ' get the tax type details
                                sTaxTypeCode = Trim$(oTaxAmountByTaxType(kTaxTypeArrayPosCode, nTaxTypeItem))
                                crTaxAmount = oTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, nTaxTypeItem)


                                ' Insert stats details records for Tax (One gross line for each tax type)
                                If crTaxAmount <> 0 AndAlso bPostClaimTax Then

                                    ' set tan / tag account code
                                    sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                                    ' Create stats for gross tax amount
                                    m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "TAG", sCreditAccountCode, nClaimPaymentID, False, crTaxAmount)

                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", PMELogLevel.PMLogError)
                                    End If

                                End If

                            Next

                        End If
                    End If

                    If bThisRevesionPresent Then

                        m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsFolder(r_lStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:="C_CR", v_lClaimId:=v_lClaimId)

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "CreateStatsFolder Payment Ref Check Failed", PMELogLevel.PMLogError)
                        End If

                        nStatsFolderCnt_for_ThisRevesionPresent = nStatsFolderCnt

                        '' Create GRS stats detail entries
                        m_lReturn = oObjectbCLMChangeClaimStatus.CreateStatsDetails(nStatsFolderCnt, v_lClaimId, "GRS", "CLMRES", 0, False, 0, "C_CR")

                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "CreateStatsDetails Payment Ref Check Failed", PMELogLevel.PMLogError)
                        End If

                    End If
                End If

            End If
            'AK 040603 - get the stats folder only if it has not been passed
            If Not v_bSavedStats Then
                ' Lets check there are work_stats before trying to
                'process them.

                m_lReturn = oObject.GetStatsFolderForClaim(oStatsFolderCnt)

                If (m_lReturn = PMEReturnCode.PMTrue) AndAlso (Information.IsArray(oStatsFolderCnt)) Then

                    For iFolderIndex As Integer = oStatsFolderCnt.GetLowerBound(1) To oStatsFolderCnt.GetUpperBound(1)

                        If ToSafeInteger(oStatsFolderCnt(0, iFolderIndex)) <> 0 Then

                            ' set the document type based on the transaction type
                            ' in c_cp only claim payments can be made
                            ' in c_cr and c_co only reserve adjustments can be made
                            If IsCloned = 1 Or IsCloned = True Then
                                If Left(m_sDocumentRef, 3) = "CLC" Then
                                    oObject.DocumentTypeID = kClonedReversedDocumentTypeId
                                    'oObject.IsCloned = 1
                                    oObject.IsClonedReversal = 1
                                    m_IsClonedReversal = 1
                                    If IsArray(oArray) Then
                                        m_lTransactionTypeID = oArray(0, 0)
                                    End If
                                    m_lClaimID = v_lClaimId
                                ElseIf Left(m_sDocumentRef, 3) = "CLD" Then
                                    oObject.DocumentTypeID = kClonedDocumentTypeID
                                    oObject.IsCloned = 1
                                    m_IsClonedReversal = 0
                                End If
                            ElseIf m_sTransactionType = "C_CP" Then
                                oObject.DocumentTypeID = kClaimPaymentDocumentTypeID
                            ElseIf m_sTransactionType = "C_SA" OrElse m_sTransactionType = "C_RV" Then
                                oObject.DocumentTypeID = kClaimReceiptDocumentTypeID
                            End If

                            ' Apply coinsurance and reinsurance to create stats.
                            m_lReturn = oObject.CreateStatsForCoinsReins(CInt(oStatsFolderCnt(0, iFolderIndex)))

                            ' get the stats folder cnt
                            nStatsFolderCnt = CInt(oStatsFolderCnt(0, iFolderIndex))

                            ' finalise the stats folders details and determine whether
                            ' the transactions should be suppressed

                            m_lReturn = FinaliseStats(nStatsFolderCnt, bStatsSuppressed)

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            ' if the stats have not been suppressed
                            If Not bStatsSuppressed Then

                                m_lReturn = oObject.CreateTransactions(nStatsFolderCnt, r_lDocumentId)
                                If m_lReturn <> PMEReturnCode.PMTrue AndAlso m_lReturn <> PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMFalse
                                ElseIf m_lReturn = PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMNotFound
                                End If

                                Dim sAutoReceipt As String = ""
                                m_lReturn = bPMFunc.GetSystemOption(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, 5117, sAutoReceipt)

                                Dim bAutomateReceiptGeneration As Boolean
                                bAutomateReceiptGeneration = ToSafeInteger(sAutoReceipt, "0") = "1"

                                ' update the payment associated with the stats folder (if there is one)
                                ' with the associated document id; creating a direct link between
                                ' payments and accounts.
                                If m_sTransactionType = "C_CP" Then
                                    m_lReturn = oObjectbCLMChangeClaimStatus.UpdatePaymentDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFalse
                                    End If

                                    If bPaymentRefCheckEnabled Then
                                        m_lReturn = oObjectbCLMChangeClaimStatus.UpdatePaymentReference(v_lDocument_Id:=r_lDocumentId)
                                        If m_lReturn <> PMEReturnCode.PMTrue Then
                                            Return PMEReturnCode.PMFalse
                                        End If
                                    End If
                                Else
                                    m_lReturn = oObjectbCLMChangeClaimStatus.UpdateReceiptDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFalse
                                    End If
                                End If
                            End If

                            If nStatsFolderCnt_for_ThisRevesionPresent = nStatsFolderCnt Then
                                If nC_CP_DocumentID <> 0 Then
                                    r_lDocumentId = nC_CP_DocumentID
                                End If
                            Else
                                nC_CP_DocumentID = r_lDocumentId
                            End If
                        End If
                    Next iFolderIndex
                End If

            Else
                ' Get the list of stats folder for this claim
                If r_bFromSAM Then
                    m_lReturn = oObjectbCLMChangeClaimStatus.GetStatsFolderForClaim(v_lClaimId, oStatsFolderCnt, "C_CP")
                Else
                    m_lReturn = oObjectbCLMChangeClaimStatus.GetStatsFolderForClaim(v_lClaimId, oStatsFolderCnt)
                End If
                If IsArray(oStatsFolderCnt) Then

                    For iFolderIndex As Integer = oStatsFolderCnt.GetLowerBound(1) To oStatsFolderCnt.GetUpperBound(1)

                        If CInt(oStatsFolderCnt(0, iFolderIndex)) <> 0 Then

                            oObject.DocumentTypeID = 28

                            ' Apply coinsurance and reinsurance to create stats.

                            m_lReturn = oObject.CreateStatsForCoinsReins(CInt(oStatsFolderCnt(0, iFolderIndex)))

                            nStatsFolderCnt = CInt(oStatsFolderCnt(0, iFolderIndex))

                            ' finalise the stats folders details and determine whether
                            ' the transactions should be suppressed

                            m_lReturn = FinaliseStats(nStatsFolderCnt, bStatsSuppressed)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            ' if the stats have not been suppressed
                            If Not bStatsSuppressed Then

                                ' just raise these transactions now...
                                ' all the other steps have already been processed before

                                m_lReturn = oObject.CreateTransactions(nStatsFolderCnt, r_lDocumentId)

                                If m_lReturn <> PMEReturnCode.PMTrue AndAlso m_lReturn <> PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMFalse
                                ElseIf m_lReturn = PMEReturnCode.PMNotFound Then
                                    Return PMEReturnCode.PMNotFound
                                End If

                                ' update the payment associated with the stats folder (if there is one)
                                ' with the associated document id; creating a direct link between
                                ' payments and accounts.
                                If bThisRevesionPresent = True Then

                                    Dim vDocumentType As Object
                                    Dim sql As String

                                    sql = "select dt.code from DocumentType dt join Document d on d.documenttype_id =  dt.documenttype_id where d.document_id = {document_id}"

                                    m_oDatabase.Parameters.Clear()

                                    m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", _
                                                                                vValue:=r_lDocumentId, _
                                                                                iDirection:=PMEParameterDirection.PMParamInput, _
                                                                                iDataType:=PMEDataType.PMLong)

                                    If m_oDatabase.SQLSelect( _
                                                            sSQL:=sql, _
                                                            sSQLName:="select document_type from Document", _
                                                            bStoredProcedure:=False, _
                                                            vResultArray:=vDocumentType, _
                                                            lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                                        Return PMEReturnCode.PMFalse
                                    End If

                                    If Trim(vDocumentType(0, 0)) <> "CLA" Then
                                        m_lReturn = oObjectbCLMChangeClaimStatus.UpdatePaymentDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                    End If
                                Else
                                    m_lReturn = oObjectbCLMChangeClaimStatus.UpdatePaymentDocumentDetails(v_lStatsFolderCnt:=nStatsFolderCnt)
                                End If

                                If m_lReturn <> PMEReturnCode.PMTrue Then
                                    Return PMEReturnCode.PMFalse
                                End If

                                If bPaymentRefCheckEnabled Then
                                    m_lReturn = oObjectbCLMChangeClaimStatus.UpdatePaymentReference(v_lDocument_Id:=r_lDocumentId)
                                    If m_lReturn <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFalse
                                    End If
                                End If
                            End If

                            If nStatsFolderCnt_for_ThisRevesionPresent = nStatsFolderCnt Then
                                If nC_CP_DocumentID <> 0 Then
                                    r_lDocumentId = nC_CP_DocumentID
                                End If
                            Else
                                nC_CP_DocumentID = r_lDocumentId
                            End If

                        End If
                    Next iFolderIndex
                End If
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="RaiseTransactions Failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:="RaiseTransactions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        Finally
            If oObject IsNot Nothing Then
                oObject.Dispose()
                oObject = Nothing
            End If
        End Try
    End Function

    Public Function FinaliseStats(ByRef v_lStatsFolderCnt As Integer, ByRef r_bStatsSuppressed As Boolean) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(m_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=CStr(m_lTransactionTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(v_lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bstatssuppressed", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nIsCloned", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nIsCloned_reversal", vValue:=m_IsClonedReversal, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACFinaliseStatsForUtilitySQl, sSQLName:=ACFinaliseStatsForUtilityName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Convert.IsDBNull(m_oDatabase.Parameters.Item("bstatssuppressed").Value) Or IsNothing(m_oDatabase.Parameters.Item("bstatssuppressed").Value) Then
                m_oDatabase.Parameters.Item("bstatssuppressed").Value = 0
            End If

            r_bStatsSuppressed = m_oDatabase.Parameters.Item("bstatssuppressed").Value

            m_lReturn = m_oDatabase.SQLCommitTrans()

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FinaliseStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FinaliseStats", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function IsDocumentInStats(ByVal v_sDocumentRef As String, ByRef r_lStatus As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentINStatsSQL, sSQLName:=ACGetDocumentINStatsName, bStoredProcedure:=ACGetDocumentINStatsStored, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                r_lStatus = gPMConstants.PMEReturnCode.PMNotFound
            Else
                r_lStatus = gPMConstants.PMEReturnCode.PMTrue
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsDocumentInAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsDocumentInAccount", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function AddDataFixUtilityLog _
        (ByVal v_sPMNumber As String, _
        ByVal v_sCreatedBy As String, _
        ByVal v_sOldDocumentRef As String, _
        ByVal v_sNewDocumentid As Integer, _
        ByVal v_bIsReversal As Boolean, _
        Optional ByVal v_lInsuranceFileCnt As Long = 0, _
        Optional ByVal v_ClaimId As Long = 0,
        Optional ByVal v_AllocationID As Integer = 0,
        Optional ByVal v_AssociatedDocRef As String = "",
         Optional ByVal v_bIsOnlyGenerate As Boolean = False) As Long

        Dim lRecordsAffected As Long

        Try

            AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()



            With m_oDatabase

                ' Add the Input Params
                m_lReturn = .Parameters.Add(
                        sName:="PMNumber",
                        vValue:=v_sPMNumber,
                        iDirection:=PMEParameterDirection.PMParamInput,
                        iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                      sName:="Created_by",
                      vValue:=v_sCreatedBy,
                      iDirection:=PMEParameterDirection.PMParamInput,
                      iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                m_lReturn = .Parameters.Add(
                      sName:="insurance_file_cnt",
                      vValue:=v_lInsuranceFileCnt,
                      iDirection:=PMEParameterDirection.PMParamInput,
                      iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                      sName:="old_document_ref",
                      vValue:=v_sOldDocumentRef,
                      iDirection:=PMEParameterDirection.PMParamInput,
                      iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                       sName:="new_document_id",
                       vValue:=v_sNewDocumentid,
                       iDirection:=PMEParameterDirection.PMParamInput,
                       iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                ' Add ExportFolderCnt as an INPUT param for an insert
                m_lReturn = .Parameters.Add(
                      sName:="is_reversal",
                      vValue:=v_bIsReversal,
                      iDirection:=PMEParameterDirection.PMParamInput,
                      iDataType:=gPMConstants.PMEDataType.PMBoolean)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                      sName:="Claim_id",
                      vValue:=v_ClaimId,
                      iDirection:=PMEParameterDirection.PMParamInput,
                      iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                     sName:="AllocationID",
                     vValue:=v_AllocationID,
                     iDirection:=PMEParameterDirection.PMParamInput,
                     iDataType:=gPMConstants.PMEDataType.PMLong)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                     sName:="AssociatedDocRef",
                     vValue:=v_AssociatedDocRef,
                     iDirection:=PMEParameterDirection.PMParamInput,
                     iDataType:=gPMConstants.PMEDataType.PMString)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                m_lReturn = .Parameters.Add(
                     sName:="IsOnlyGenerate",
                     vValue:=v_bIsOnlyGenerate,
                     iDirection:=PMEParameterDirection.PMParamInput,
                     iDataType:=gPMConstants.PMEDataType.PMBoolean)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End With

            ' Execute Add Trans Export Details SQL Statement
            m_lReturn = m_oDatabase.SQLAction(
                sSQL:="spu_DataFixUtility_log_add",
                sSQLName:="spu_DataFixUtility_log_add",
                bStoredProcedure:=True,
                lRecordsAffected:=lRecordsAffected)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If


            Exit Function
        Catch excep As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddDataFixUtilityLog Fail", vApp:=ACApp, vClass:=ACClass, vMethod:="AddDataFixUtilityLog", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            AddDataFixUtilityLog = gPMConstants.PMEReturnCode.PMError
        End Try
        Exit Function
    End Function

    Public Function AddTransdetailEx(V_nDocumentId As Long) As Long

        Const kMethodName As String = "AddTransdetailEx"

        Try
            AddTransdetailEx = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("Document_Id", V_nDocumentId, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction("spu_ACT_Add_TransDetailEx", "Add Collection Buckets", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddTransdetailEx = m_lReturn
            End If
            Exit Function
        Catch excep As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTransdetailEx Fail", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTransdetailEx", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            AddTransdetailEx = gPMConstants.PMEReturnCode.PMError
        End Try
        Exit Function

    End Function

    Public Function UpdatePolicyToQuote(V_nInsuranceFileCnt As Long) As Long

        Const kMethodName As String = "UpdatePolicyToQuote"

        Try
            UpdatePolicyToQuote = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(V_nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            '
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdatePolicyToQuote = m_lReturn
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdInsuranceFileSQL, sSQLName:=ACUpdInsuranceFileName, bStoredProcedure:=ACUpdInsuranceFileStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdatePolicyToQuote = m_lReturn
            End If
            Exit Function
        Catch excep As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyToQuote Fail", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyToQuote", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            UpdatePolicyToQuote = gPMConstants.PMEReturnCode.PMError
        End Try
        Exit Function

    End Function

    Public Function AddReversalDocument(ByVal v_lOldDocumentRef As String, ByVal v_nStatsFolderCnt As Integer, Optional ByRef r_vDocumentID As Integer = Nothing _
                                                  ) As Integer

        Try
            AddReversalDocument = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=(r_vDocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add("oldDocRef", v_lOldDocumentRef, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add("nStatsFolderCnt", v_nStatsFolderCnt, PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.SQLAction(ACAddDocumentSQL, ACAddDocumentName, True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddReversalDocument = m_lReturn
            End If

            r_vDocumentID = m_oDatabase.Parameters.Item("document_id").Value

            Exit Function
        Catch excep As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddReversalDocument", vApp:=ACApp, vClass:=ACClass, vMethod:="AddReversalDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            AddReversalDocument = gPMConstants.PMEReturnCode.PMError
        End Try
        Exit Function

    End Function

    Public Function AddReversalTransdetail(ByVal v_lOldDocumentRef As String, ByVal r_vDocumentID As Integer _
                                                  ) As Integer

        Try
            AddReversalTransdetail = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewDocumentId", vValue:=(r_vDocumentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sOldDocumentRef", vValue:=v_lOldDocumentRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(ACAddReverseTransdetaiSQL, ACAddReverseTransdetaiName, True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                AddReversalTransdetail = m_lReturn
            End If
            Exit Function
        Catch excep As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddReversalTransdetail", vApp:=ACApp, vClass:=ACClass, vMethod:="AddReversalTransdetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            AddReversalTransdetail = gPMConstants.PMEReturnCode.PMError
        End Try
        Exit Function

    End Function
    Public Function UpdateSQLAction(ByVal sSQL As String) As Integer
        'Dim result As Integer = 0
        Try

            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=sSQL, bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_lReturn

        Catch excep As System.Exception

            m_lReturn = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateSQLAction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateSQLAction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return m_lReturn
        End Try
    End Function

    Public Function BeginTrans() As Long


        BeginTrans = gPMConstants.PMEReturnCode.PMTrue

        ' Begin the Transaction
        m_lReturn = m_oDatabase.SQLBeginTrans

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            BeginTrans = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function


        Exit Function

    End Function


    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Public Function CommitTrans() As Long



        CommitTrans = gPMConstants.PMEReturnCode.PMTrue

        ' Begin the Transaction
        m_lReturn = m_oDatabase.SQLCommitTrans

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            CommitTrans = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function



    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Public Function RollbackTrans() As Long

        RollbackTrans = gPMConstants.PMEReturnCode.PMTrue

        ' Begin the Transaction
        m_lReturn = m_oDatabase.SQLRollbackTrans

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RollbackTrans = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        Exit Function

    End Function


    Public Function ProcessTransactions(lClaimID As Long, sDocRef As String, v_bRePost As Boolean, v_bRIRefresh As Boolean, Optional v_sRefNumber As String = "") As Long

        Dim lReturn As Long
        Dim sTransactionCode As String = ""
        Dim bIsOnlyRIRefresh As Boolean

        ProcessTransactions = gPMConstants.PMEReturnCode.PMTrue

        Try


            ' This reserve amount is corrupted for claim_id 420801 correct this amount first  
            'If lClaimID = 420801 Then
            '    lReturn = UpdateReserve(lClaimID)
            'End If


            lReturn = GetTransactionTypeCode(v_sDocumentRef:=sDocRef, r_Transaction_Type_Code:=sTransactionCode)
            If Left(sDocRef, 3) = "" And v_bRIRefresh Then
                bIsOnlyRIRefresh = True
            ElseIf Left(sDocRef, 3) = "" Then
                Return 0
                Exit Function
            End If

            Call BeginTrans()

            If Not v_bRePost Then
                If Not bIsOnlyRIRefresh Then
                    lReturn = ReverseClaimTransactions(lClaimID, sTransactionCode, sDocRef)
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        'Return gPMConstants.PMEReturnCode.PMFalse
                        GoTo Err
                    End If

                    m_lReturn = AddDataFixUtilityLog(v_sPMNumber:=v_sRefNumber, v_sCreatedBy:="DataFixUtility", v_ClaimId:=lClaimID,
                           v_sOldDocumentRef:=sDocRef, v_sNewDocumentid:=0, v_bIsReversal:=True)
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        'Return gPMConstants.PMEReturnCode.PMFalse
                        GoTo Err
                    End If
                End If

            ElseIf v_bRePost Then
                If Not bIsOnlyRIRefresh Then
                    lReturn = ReverseClaimTransactions(lClaimID, sTransactionCode, sDocRef)
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        'Return gPMConstants.PMEReturnCode.PMFalse
                        GoTo Err
                    End If

                    m_lReturn = AddDataFixUtilityLog(v_sPMNumber:=v_sRefNumber, v_sCreatedBy:="DataFixUtility", v_ClaimId:=lClaimID,
                           v_sOldDocumentRef:=sDocRef, v_sNewDocumentid:=0, v_bIsReversal:=True)
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        'Return gPMConstants.PMEReturnCode.PMFalse
                        GoTo Err
                    End If
                End If
                If v_bRIRefresh Then
                    lReturn = RecalculateRI(lClaimID)
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        'Return gPMConstants.PMEReturnCode.PMFalse
                        GoTo Err
                    End If
                End If

                If Not bIsOnlyRIRefresh Then
                    lReturn = RaiseClaimTransaction(lClaimID, sTransactionCode)
                    If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        GoTo Err
                    End If
                End If
                m_lReturn = AddDataFixUtilityLog(v_sPMNumber:=v_sRefNumber, v_sCreatedBy:="DataFixUtility", v_ClaimId:=lClaimID,
                                   v_sOldDocumentRef:=sDocRef, v_sNewDocumentid:=0, v_bIsReversal:=False)
                If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    'Return gPMConstants.PMEReturnCode.PMFalse
                    GoTo Err
                End If
            End If



            Call CommitTrans()
            Exit Function
Err:
            Call RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse

        Catch excep As System.Exception

            'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFailedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
            Call RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function ReverseClaimTransactions(ByVal v_lClaimId As Long, ByVal v_sTransactionTypeCode As String, Optional sDocRef As String = "") As Long
        Const kMethodName As String = "ReverseClaimTransactions"

        Dim r_vClaimDocuments As Object
        Dim lCount As Long
        Dim oDocumentReversal As Object
        Dim lStatsFolderCnt As Long
        Dim m_lReturn As Long

        Try

            ReverseClaimTransactions = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=v_sTransactionTypeCode, v_lClaimId:=v_lClaimId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ReverseClaimStats(v_lClaimId:=v_lClaimId, v_lStatsFolderCnt:=lStatsFolderCnt, sDocRef:=sDocRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = ReverseTransactions(v_lClaimId:=v_lClaimId, v_lStatsFolderCnt:=lStatsFolderCnt, m_sTransactionCode:=v_sTransactionTypeCode, sDocRef:=sDocRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse

        End Try

    End Function


    Public Function GenerateClaimTransactions(lClaimID As Long, sDocRef As String, v_bRIRefresh As Boolean, Optional v_sRefNumber As String = "") As Long

        Dim lReturn As Long
        Dim sTransactionCode As String = ""
        Dim vArray As Object


        GenerateClaimTransactions = gPMConstants.PMEReturnCode.PMTrue

        Try

            Call BeginTrans()

            If v_bRIRefresh Then
                lReturn = RecalculateRI(lClaimID)
                If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    Throw New Exception
                End If
            End If


            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                   vValue:=lClaimID,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_CLM_Get_Transaction_Code",
                                              sSQLName:="Get Transaction code",
                                              bStoredProcedure:=True,
                                              lNumberRecords:=PMAllRecords,
                                              vResultArray:=vArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Throw New Exception
            End If

            If IsArray(vArray) Then
                sTransactionCode = vArray(0, 0)
            Else
                Throw New Exception
            End If


            lReturn = RaiseClaimTransaction(lClaimID, sTransactionCode)
            If lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                Throw New Exception
            End If

            m_lReturn = AddDataFixUtilityLog(v_sPMNumber:=v_sRefNumber, v_sCreatedBy:="DataFixUtility", v_ClaimId:=lClaimID,
                               v_sOldDocumentRef:=sDocRef, v_sNewDocumentid:=0, v_bIsReversal:=False, v_bIsOnlyGenerate:=True)


            Call CommitTrans()
            Exit Function

        Catch excep As System.Exception

            'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFailedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
            Call RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function


    Public Function CreateStatsFolder(ByVal v_sTransactionTypeCode As String, _
                                   ByVal v_lClaimId As Long, _
                                   ByRef r_lStatsFolderCnt As Long) As Long

        Dim lRecordsAffected As Long
        Dim vResultArray As Object
        Dim sSQL As String
        Dim m_lReturn As Long

        Try

            CreateStatsFolder = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", _
                                                  vValue:=0, _
                                                  iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, _
                                                  iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", _
                                                   vValue:=v_sTransactionTypeCode, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", _
                                                   vValue:=m_iUserID, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", _
                                                   vValue:=v_lClaimId, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' Execute Add Stats Folder SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_clm_add_stats_folder", _
                                              sSQLName:="AddStatsFolderClaims", _
                                              bStoredProcedure:=True, _
                                              lRecordsAffected:=lRecordsAffected)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Cnt of the record inserted
            r_lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value


            If (r_lStatsFolderCnt < 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function ReverseClaimStats(ByVal v_lClaimId As Long, ByVal v_lStatsFolderCnt As Long, Optional ByVal sDocRef As String = "") As Long


        Dim m_lReturn As Long

        Try

            ReverseClaimStats = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add( _
                    sName:="claim_id", _
                    vValue:=v_lClaimId, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add( _
                    sName:="New_Stats_Folder_Cnt", _
                    vValue:=v_lStatsFolderCnt, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If sDocRef <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add( _
                    sName:="oldDocRef", _
                    vValue:=sDocRef, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLAction( _
                                    sSQL:=ACReverseClaimStatsSQL, _
                                    sSQLName:=ACReverseClaimStatsName, _
                                    bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function


    Public Function AllocateDocuments(ByVal AllocationId As Integer, ByVal AllocatedDocRef As String, ByVal AssociatedDocRef As String) As Long

        Dim oObject As Object
        Dim lReturn As Long
        Dim vArray As Object
        Dim lOriginalDocId As Long
        Dim lReversedDocId As Long
        Dim vOriginalResultArray(,) As Object
        Dim vReversedResultArray(,) As Object
        Dim oAllocationManual As bACTAllocationManual.Business
        Dim lAccountID As Integer
        Dim vKeyArray(1, 3) As Object
        Dim vTrans(0) As Object

        Try
            AllocateDocuments = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            lReturn = m_oDatabase.Parameters.Add(sName:="v_iAllocationId", vValue:=AllocationId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAllocationReverseSQl, _
                                              sSQLName:=ACFinaliseStatsName, _
                                              bStoredProcedure:=True)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=AllocatedDocRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentDetailsSQL, _
                                                sSQLName:=ACGetDocumentDetailsName, _
                                                bStoredProcedure:=ACGetDocumentDetailsStored, _
                                                vResultArray:=vArray, _
                                                bKeepNulls:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return lReturn
            End If

            If Not IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lOriginalDocId = CLng(vArray(0, 0))

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=AssociatedDocRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocumentDetailsSQL, _
                                                sSQLName:=ACGetDocumentDetailsName, _
                                                bStoredProcedure:=ACGetDocumentDetailsStored, _
                                                vResultArray:=vArray, _
                                                bKeepNulls:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not IsArray(vArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReversedDocId = CLng(vArray(0, 0))

            'lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, _
            '                                                    v_sClassName:="bControlTrans.Automated", _
            '                                                    v_sCallingAppName:=ACApp, _
            '                                                    v_sUsername:=m_sUsername, _
            '                                                    v_sPassword:=m_sPassword, _
            '                                                    v_iUserID:=m_iUserID, _
            '                                                    v_iSourceID:=m_iSourceID, _
            '                                                    v_iLanguageID:=m_iLanguageID, _
            '                                                    v_iCurrencyID:=m_iCurrencyID, _
            '                                                    v_iLogLevel:=m_iLogLevel, _
            '                                                    v_oDatabase:=m_oDatabase)
            'If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            'lReturn = oObject.AllocateDocuments(lOriginalDocId, lReversedDocId)
            oAllocationManual = New bACTAllocationManual.Business

            If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(AllocateDocuments, AllocateDocuments & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add(sName:="document_id", vValue:=CStr(lOriginalDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                .Parameters.Add(sName:="iAllocationId", vValue:=AllocationId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                lReturn = .SQLSelect(sSQL:=ACGetTransDetailByDocSQL, sSQLName:=ACGetTransDetailByDocName, bStoredProcedure:=ACGetTransDetailByDocStored, vResultArray:=vOriginalResultArray)
            End With

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AllocateDocuments, AllocateDocuments & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add(sName:="document_id", vValue:=CStr(lReversedDocId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                .Parameters.Add(sName:="iAccountId", vValue:=vOriginalResultArray(1, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                .Parameters.Add(sName:="nAmount", vValue:=vOriginalResultArray(2, 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDecimal)
                lReturn = .SQLSelect(sSQL:=ACGetTransDetailByDocSQL, sSQLName:=ACGetTransDetailByDocName, bStoredProcedure:=ACGetTransDetailByDocStored, vResultArray:=vReversedResultArray)
            End With

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AllocateDocuments, AllocateDocuments & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not Information.IsArray(vOriginalResultArray) Or Not Information.IsArray(vReversedResultArray) Then
                ' transactions not available to allocate
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If vOriginalResultArray.GetUpperBound(1) <> vReversedResultArray.GetUpperBound(1) Then
                ' just to ensure number account lines are same else leave alone
                Return gPMConstants.PMEReturnCode.PMTrue
                Exit Function
            End If

            For lCount As Integer = 0 To vReversedResultArray.GetUpperBound(1)

                vTrans(0) = CStr(vReversedResultArray(0, lCount)) & "|" & CStr(vReversedResultArray(2, lCount))

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vOriginalResultArray(1, lCount)

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(vOriginalResultArray(0, lCount)) & "|" & CStr(vOriginalResultArray(2, lCount))

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vTrans

                lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeyArray)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(AllocateDocuments, AllocateDocuments & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                lReturn = oAllocationManual.Start()

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(AllocateDocuments, AllocateDocuments & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            Next

            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            Return gPMConstants.PMEReturnCode.PMFalse

        Finally
            oAllocationManual.Dispose()
        End Try

        Return lReturn
    End Function

    Public Function ReverseTransactions(ByVal v_lClaimId As Long, ByVal v_lStatsFolderCnt As Long, ByVal m_sTransactionCode As String, Optional sDocRef As String = "") As Long

        Const kMethodName As String = "ReverseTransactions"

        Dim oObject As Object
        Dim vArray As Object
        Dim bStatsSuppressed As Boolean
        Dim r_lDocumentId As Long

        Dim oAllocationManual As Object
        Dim lRow As Long
        Dim lTransDetailId As Long
        Dim vCreditTransactions As Object

        Dim vKeyArray(1, 3) As Object
        Dim lRecordsAffected As Long
        Dim lDocumentId As Long
        Dim lCount As Long
        Dim vTrans(0) As Object
        Dim sSQL As String
        Dim vTransactions As Object
        Dim m_lReturn As Long
        Dim lTransactionTypeId As Long
        Dim r_lTransactionExportFolderCnt As Integer
        Dim nDocumentId As Integer
        Try

            ReverseTransactions = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, _
                                                                v_sClassName:="bControlTransClaims.Automated", _
                                                                v_sCallingAppName:=ACApp, _
                                                                v_sUsername:=m_sUsername, _
                                                                v_sPassword:=m_sPassword, _
                                                                v_iUserID:=m_iUserID, _
                                                                v_iSourceID:=m_iSourceID, _
                                                                v_iLanguageID:=m_iLanguageID, _
                                                                v_iCurrencyID:=m_iCurrencyID, _
                                                                v_iLogLevel:=m_iLogLevel, _
                                                                v_oDatabase:=m_oDatabase)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.ClaimId = v_lClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", _
                                                   vValue:=m_sTransactionCode, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL, _
                                              sSQLName:=ACGetTransactionTypeIDName, _
                                              bStoredProcedure:=True, _
                                              lNumberRecords:=PMAllRecords, _
                                              vResultArray:=vArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If IsArray(vArray) Then
                oObject.TransactionTypeId = vArray(0, 0)
                lTransactionTypeId = vArray(0, 0)
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.TransactionTypeCode = m_sTransactionCode


            '  oObject.IsClonedReversal = 1
            ' oObject.documenttypeId = 10


            ' finalise the stats folders details and determine whether
            ' the transactions should be suppressed
            m_lReturn = FinaliseStats(v_lStatsFolderCnt, v_lClaimId, lTransactionTypeId, m_sTransactionCode, 0, 1, bStatsSuppressed)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddReversalDocument(v_lOldDocumentRef:=sDocRef, v_nStatsFolderCnt:=v_lStatsFolderCnt, r_vDocumentID:=nDocumentId)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' if the stats have not been suppressed
            '   If Not bStatsSuppressed Then

            'm_lReturn = oObject.CreateTransactions(v_lStatsFolderCnt, r_lDocumentId)
            m_lReturn = CreateExportFolder(v_lStatsFolderCnt, r_lTransactionExportFolderCnt)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CreateExportReverseDetails(r_lTransactionExportFolderCnt, sDocRef)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = AddReversalTransdetail(v_lOldDocumentRef:=sDocRef, r_vDocumentID:=nDocumentId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to post transactions", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            '  End If

            'Get an instance of bACTAllocationManual component to do the allocation
            'If gPMComponentServices.CreateBusinessObject( _
            '        r_oObject:=oAllocationManual, _
            '        v_sClassName:="bACTAllocationManual.Business", _
            '        v_sCallingAppName:=ACApp, _
            '        v_sUsername:=m_sUsername$, _
            '        v_sPassword:=m_sPassword$, _
            '        v_iUserID:=m_iUserID%, _
            '        v_iSourceID:=m_iSourceID%, _
            '        v_iLanguageID:=m_iLanguageID%, _
            '        v_iCurrencyID:=m_iCurrencyID%, _
            '        v_iLogLevel:=m_iLogLevel, _
            '        v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If




            'sSQL = " Select Account_id,transdetail_id , OutStanding_Amount ,transdetail_type_id From TransDetail Where OutStanding_Amount <> 0 AND Document_id=" & r_lDocumentId

            'm_lReturn& = m_oDatabase.SQLSelect( _
            '                                         sSQL:=sSQL, _
            '                                         sSQLName:="GetTransDetails", _
            '                                         bStoredProcedure:=False, _
            '                                         vResultArray:=vTransactions)

            'If (m_lReturn& <> 1) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If


            'm_oDatabase.Parameters.Clear()


            'sSQL = " Select Account_id,transdetail_id , OutStanding_Amount ,transdetail_type_id From TransDetail Where Document_id=( " & _
            '        "Select  TOP 1 d.document_id  from Stats_Folder Join Document D On d.document_ref =stats_folder.document_ref " & _
            '                "Where stats_folder.stats_folder_cnt < " & v_lStatsFolderCnt & " And stats_folder.loss_id =" & v_lClaimId & ")"



            'm_lReturn& = m_oDatabase.SQLSelect( _
            '                                         sSQL:=sSQL, _
            '                                         sSQLName:="GetTransDetails", _
            '                                         bStoredProcedure:=False, _
            '                                         vResultArray:=vCreditTransactions)


            'If (m_lReturn& <> 1) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If
            'If IsArray(vTransactions) And IsArray(vCreditTransactions) Then
            '    For lCount = 0 To UBound(vTransactions, 2)

            '        For lRow = 0 To UBound(vCreditTransactions, 2)

            '            If vTransactions(0, lCount) = vCreditTransactions(0, lRow) Then
            '                If Math.Abs(vTransactions(2, lCount)) = Math.Abs(vCreditTransactions(2, lRow)) And _
            '                        vTransactions(3, lCount) = vCreditTransactions(3, lRow) Then

            '                    vTrans(0) = vTransactions(1, lCount) & "|" & vTransactions(2, lCount)

            '                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = "account_id"
            '                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vCreditTransactions(0, lRow)

            '                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = "trans_detail_id"
            '                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = vCreditTransactions(1, lRow) & "|" & vCreditTransactions(2, lRow)

            '                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = "trans_detail_ids"
            '                    vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vTrans

            '                    m_lReturn = oAllocationManual.SetKeys(vKeyArray:=vKeyArray)

            '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '                        Return gPMConstants.PMEReturnCode.PMFalse
            '                    End If

            '                    m_lReturn = oAllocationManual.Start()

            '                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '                        Return gPMConstants.PMEReturnCode.PMFalse
            '                    End If
            '                End If
            '            End If
            '        Next
            '    Next
            'End If

            ' m_lReturn = oObject.Terminate

            'If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            oObject = Nothing

            Exit Function
        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Function CreateExportReverseDetails(ByVal v_lTransactionExportFolderCnt As Integer, Optional sDocRef As String = "") As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="nNewTransactionExportFolderCnt", vValue:=CStr(v_lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        m_lReturn = m_oDatabase.Parameters.Add(sName:="sOldDocumentRef", vValue:=sDocRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportRevDetailsSQL, sSQLName:=ACAddExportRevDetailsName, bStoredProcedure:=True, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Public Function RecalculateRI(ByVal lClaimID As Long) As Long
        Dim m_lReturn As Long
        Dim m_vIsRI2007 As Object
        Try

            RecalculateRI = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=m_vIsRI2007)

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=lClaimID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_vIsRI2007 <> "1" Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:="Spu_claim_recalculate_reinsurance_NOT2007", sSQLName:="RecalculateRI", bStoredProcedure:=True)

            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Claim_Recalculate_Reinsurance", sSQLName:="RecalculateRI", bStoredProcedure:=True)

            End If
            If (m_lReturn& <> gPMConstants.PMEReturnCode.PMTrue) Then
                RecalculateRI = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            Exit Function
        Catch excep As System.Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMFalse

            'gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetFailedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFailedTransaction", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return RecalculateRI
        End Try

    End Function

    Public Function RaiseClaimTransaction(ByVal v_lClaimId As Long, Optional ByVal v_sTransactionTypeCode As String = "", Optional v_bIsCloneReversal As Boolean = False, Optional v_lStatsFolderCnt As Long = 0) As Long


        Dim obCLMChangeClaimStatus As Object
        Dim m_lReturn As Long

        Try

            RaiseClaimTransaction = gPMConstants.PMEReturnCode.PMTrue

            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=obCLMChangeClaimStatus, _
            '                                                 v_sClassName:="BCLMCHANGECLAIMSTATUS.BUSINESS", _
            '                                                 v_sCallingAppName:=ACApp, _
            '                                                 v_sUsername:=m_sUsername, _
            '                                                 v_sPassword:=m_sPassword, _
            '                                                 v_iUserID:=m_iUserID, _
            '                                                 v_iSourceID:=m_iSourceID, _
            '                                                 v_iLanguageID:=m_iLanguageID, _
            '                                                 v_iCurrencyID:=m_iCurrencyID, _
            '                                                 v_iLogLevel:=m_iLogLevel, _
            '              v_oDatabase:=m_oDatabase)

            'If (m_lReturn& <> gPMConstants.PMEReturnCode.PMTrue) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If


            'm_lReturn = obCLMChangeClaimStatus.SetProcessModes(vTransactionType:=v_sTransactionTypeCode)

            'obCLMChangeClaimStatus.IsCloned = 1

            m_lReturn = RaiseTransactions(v_lClaimId:=v_lClaimId, v_sTransactionType:=v_sTransactionTypeCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            ' If you want to rollback a transaction or something, do it here

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function RaiseTransactions(ByVal v_lClaimId As Long, _
                         Optional ByVal v_bSavedStats As Boolean = False, _
                         Optional ByRef r_lDocumentId As Long = 0, _
                         Optional ByRef v_sTransactionType As String = "") As Long


        Dim oObject As Object
        Dim vArray As Object
        Dim lStatsFolderCnt As Long
        Dim bStatsSuppressed As Boolean
        'Dim lWorkStatsFolderCnt As Long
        Dim vStatsFolderCnt As Object
        Dim iFolderIndex As Integer
        Dim vClaimDetails As Object
        Dim sOptionValue As String
        Dim bPaymentRefCheckEnabled As Boolean
        Dim lRecordsAffected As Long
        Dim lReserveAmount As Double
        Dim m_lReturn As Long
        Dim lTransactionTypeId As Long

        Try

            RaiseTransactions = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oObject, _
                                                                v_sClassName:="bControlTransClaims.Automated", _
                                                                v_sCallingAppName:=ACApp, _
                                                                v_sUsername:=m_sUsername, _
                                                                v_sPassword:=m_sPassword, _
                                                                v_iUserID:=m_iUserID, _
                                                                v_iSourceID:=m_iSourceID, _
                                                                v_iLanguageID:=m_iLanguageID, _
                                                                v_iCurrencyID:=m_iCurrencyID, _
                                                                v_iLogLevel:=m_iLogLevel, _
                                                                v_oDatabase:=m_oDatabase)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.ClaimId = v_lClaimId

            'Need to get the transaction type id from the code...
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", _
                                                   vValue:=v_sTransactionType, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeIDSQL, _
                                              sSQLName:=ACGetTransactionTypeIDName, _
                                              bStoredProcedure:=True, _
                                              lNumberRecords:=PMAllRecords, _
                                              vResultArray:=vArray)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If IsArray(vArray) Then
                oObject.TransactionTypeId = vArray(0, 0)
                lTransactionTypeId = vArray(0, 0)
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oObject.TransactionTypeCode = v_sTransactionType



            ' Lets check there are work_stats before trying to
            'process them.
            Dim lClaimPaymentID As Long
            Dim sCreditAccountCode As String
            Dim vTaxAmountByTaxType As Object
            Dim sTaxTypeCode As String
            Dim crTaxAmount As Double
            Dim llBound As Integer
            Dim lUBOund As Integer
            Dim lTaxTypeItem As Integer
            Dim r_bThisRevesionPresent As Boolean

            ''Get stats folder cnt

            If v_sTransactionType = "C_CO" Or v_sTransactionType = "C_CR" Then
                'lTransactionTypeID = 26 'claim open
                m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=v_sTransactionType, v_lClaimId:=v_lClaimId)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                sCreditAccountCode = "CLMRES"

                '' Create GRS stats detail entries
                m_lReturn = CreateStatsDetails(lStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, 0, False, sTransactionTypeCode:=v_sTransactionType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                m_lReturn = CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=v_sTransactionType, v_lClaimId:=v_lClaimId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '' Create GRS stats detail entries
                m_lReturn = CreateStatsDetails(lStatsFolderCnt, v_lClaimId, "GRS", sCreditAccountCode, lClaimPaymentID, r_bThisRevesionPresent, sTransactionTypeCode:=v_sTransactionType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            End If


            ' ******** For Tax Calculation 

            ' get tax lines grouped by tax type for this payment
            If v_sTransactionType = "C_CP" Then
                m_lReturn = GetClaimTaxAmountsByTaxType(v_iClaimPaymentId:=lClaimPaymentID, _
                                            r_vResults:=vTaxAmountByTaxType)
            ElseIf v_sTransactionType = "C_SA" OrElse v_sTransactionType = "C_RV" Then
                m_lReturn = GetClaimTaxAmountsByTaxType(v_iClaimReceiptId:=lClaimPaymentID, _
                                            r_vResults:=vTaxAmountByTaxType)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Dim m_vClaimDetails As Object
            Dim bPostClaimTax As Boolean

            m_lReturn = GetClaimDetails(v_lClaimId, m_vClaimDetails)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not IsArray(m_vClaimDetails) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                bPostClaimTax = CBool(ToSafeLong(m_vClaimDetails(kClaimDetailPostClaimsTaxes, 0), 0))
            End If


            If (v_sTransactionType = "C_RV" Or v_sTransactionType = "C_SA") And bPostClaimTax Then
                ' Create stats for gross tax amount

                If IsArray(vTaxAmountByTaxType) Then

                    llBound = LBound(vTaxAmountByTaxType, 2)
                    lUBOund = UBound(vTaxAmountByTaxType, 2)

                    For lTaxTypeItem = llBound To lUBOund
                        crTaxAmount = crTaxAmount + vTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, lTaxTypeItem)
                    Next
                    lTaxTypeItem = 0

                    ' Insert stats details records for Tax (One gross line for each tax type)
                    If crTaxAmount <> 0 Then
                        m_lReturn = CreateStatsDetails(lStatsFolderCnt, v_lClaimId, "TAG", sCreditAccountCode, 0, False, crTaxAmount, sTransactionTypeCode:=v_sTransactionType)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End If

                End If
                ' process the tax rows..
                If IsArray(vTaxAmountByTaxType) Then

                    llBound = LBound(vTaxAmountByTaxType, 2)
                    lUBOund = UBound(vTaxAmountByTaxType, 2)

                    For lTaxTypeItem = llBound To lUBOund

                        ' get the tax type details
                        sTaxTypeCode = Trim$(vTaxAmountByTaxType(kTaxTypeArrayPosCode, lTaxTypeItem))
                        crTaxAmount = vTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, lTaxTypeItem)

                        ' Insert stats details records for Tax (One gross line for each tax type)
                        If crTaxAmount <> 0 Then

                            ' set tan / tag account code
                            sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                            ' Create stats for TAN amount
                            m_lReturn = CreateStatsDetails(lStatsFolderCnt, v_lClaimId, "TAN", sCreditAccountCode, 0, False, -crTaxAmount, sTransactionTypeCode:=v_sTransactionType)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        End If

                    Next

                End If

            ElseIf v_sTransactionType = "C_CP" Then

                ' process the tax rows..
                If IsArray(vTaxAmountByTaxType) Then

                    llBound = LBound(vTaxAmountByTaxType, 2)
                    lUBOund = UBound(vTaxAmountByTaxType, 2)

                    For lTaxTypeItem = llBound To lUBOund

                        ' get the tax type details
                        sTaxTypeCode = Trim$(vTaxAmountByTaxType(kTaxTypeArrayPosCode, lTaxTypeItem))
                        crTaxAmount = vTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, lTaxTypeItem)


                        ' Insert stats details records for Tax (One gross line for each tax type)
                        If crTaxAmount <> 0 And bPostClaimTax Then

                            ' set tan / tag account code
                            sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                            ' Create stats for gross tax amount
                            m_lReturn = CreateStatsDetails(lStatsFolderCnt, v_lClaimId, "TAG", sCreditAccountCode, lClaimPaymentID, False, crTaxAmount, v_sTransactionType)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                        End If

                    Next

                End If
            End If




            ' ******** End For Tax Calculation 


            'AK 040603 - get the stats folder only if it has not been passed
            If v_bSavedStats = False Then
                ' Lets check there are work_stats before trying to
                'process them.
                m_lReturn = oObject.GetStatsFolderForClaim(vStatsFolderCnt)

                'If (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (IsArray(vStatsFolderCnt)) Then

                'For iFolderIndex = LBound(vStatsFolderCnt, 2) To UBound(vStatsFolderCnt, 2)

                If (lStatsFolderCnt > 0) Then

                    ' set the document type based on the transaction type
                    ' in c_cp only claim payments can be made
                    ' in c_cr and c_co only reserve adjustments can be made

                    If v_sTransactionType = "C_CP" Then
                        oObject.documenttypeId = 28
                    ElseIf v_sTransactionType = "C_SA" Or v_sTransactionType = "C_RV" Then
                        oObject.documenttypeId = 29
                    End If


                    ' Apply coinsurance and reinsurance to create stats.
                    m_lReturn = oObject.CreateStatsForCoinsReins(CLng(lStatsFolderCnt))

                    ' get the stats folder cnt
                    ' lStatsFolderCnt = lStatsFolderCnt

                    ' finalise the stats folders details and determine whether
                    ' the transactions should be suppressed
                    m_lReturn = FinaliseStats(lStatsFolderCnt, v_lClaimId, lTransactionTypeId, v_sTransactionType, 1, 0, bStatsSuppressed)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' if the stats have not been suppressed
                    If Not bStatsSuppressed Then

                        m_lReturn = oObject.CreateTransactions(lStatsFolderCnt, _
                                                              r_lDocumentId)
                        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If
                End If
                ''    Next iFolderIndex
                ''End If
            End If
            '  m_lReturn = oObject.Terminate

            'If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            oObject = Nothing

            Exit Function

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Public Function CreateStatsDetails(ByVal v_lStatsFolderCnt As Long, _
                                  ByVal v_lClaimId As Long, _
                                  ByVal v_sStatsDetailType As String, _
                                  ByVal sCreditAccountCode As String, _
                                  ByRef lClaimPaymentID As Long, _
                                   ByRef r_bThisRevesionPresent As Boolean, _
                                    Optional ByVal dTaxamount As Double = 0, _
                                    Optional ByVal sTransactionTypeCode As String = "") As Long

        Dim lRecordsAffected As Long
        Dim vTaxAmountByTaxType As Object
        Dim m_lReturn As Integer


        Try
            CreateStatsDetails = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claimpayment_id", _
                                               vValue:=0, _
                                               iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, _
                                               iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ThisRevesionPresent", _
                                                    vValue:=0, _
                                                    iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, _
                                                    iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", _
                                                   vValue:=v_lStatsFolderCnt, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", _
                                                   vValue:=sTransactionTypeCode, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id", _
                                                   vValue:=v_lClaimId, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="StatsDetailType", _
                                                  vValue:=v_sStatsDetailType, _
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                   iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="CreditAccountCode", _
                                                  vValue:=sCreditAccountCode, _
                                                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                  iDataType:=gPMConstants.PMEDataType.PMString)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_sStatsDetailType = "TAG" Or v_sStatsDetailType = "TAN" Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="TaxAmount", _
                                                      vValue:=dTaxamount, _
                                                      iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                                                      iDataType:=gPMConstants.PMEDataType.PMDouble)

                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsGRSDetailsSQL, _
                                              sSQLName:=ACAddStatsGRSDetailsName, _
                                              bStoredProcedure:=True, _
                                              lRecordsAffected:=lRecordsAffected)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_bThisRevesionPresent = ToSafeBoolean(m_oDatabase.Parameters.Item("ThisRevesionPresent").Value)

            If sTransactionTypeCode = "C_CP" Or sTransactionTypeCode = "C_SA" Or sTransactionTypeCode = "C_RV" Then
                lClaimPaymentID = m_oDatabase.Parameters.Item("ClaimPayment_Id").Value
                If (lClaimPaymentID < 1) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            Exit Function
        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

    End Function

    Private Function GetClaimTaxAmountsByTaxType( _
                            ByRef r_vResults(,) As Object, _
                   Optional ByVal v_iClaimPaymentId As Integer = 0, _
                   Optional ByVal v_iClaimReceiptId As Integer = 0, _
                   Optional ByVal v_iClaimReceiptItemId As Integer = 0) As Integer

        Const kMethodName As String = "GetClaimTaxAmountsByTaxType"
        Dim m_lReturn As Long
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                If v_iClaimPaymentId <> 0 Then
                    m_lReturn = .Parameters.Add(sName:="claim_payment_id", vValue:=CStr(v_iClaimPaymentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If

                If v_iClaimReceiptId <> 0 Then
                    m_lReturn = .Parameters.Add(sName:="claim_receipt_id", vValue:=CStr(v_iClaimReceiptId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
                If v_iClaimReceiptItemId <> 0 Then
                    m_lReturn = .Parameters.Add(sName:="claim_receipt_item_id", vValue:=CStr(v_iClaimReceiptItemId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = m_lReturn
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus failed to add Claim_ID parameter.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        Return result
                    End If
                End If
                ' Execute selection Query

                If m_oDatabase.SQLSelect(sSQL:=kGetClaimTaxAmountsByTaxTypeSQL, sSQLName:=kGetClaimTaxAmountsByTaxTypeName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResults) <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, kGetClaimTaxAmountsByTaxTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

        End Try
        Return result
    End Function

    Public Function GetClaimDetails(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimDetailSQL, sSQLName:=ACGetClaimDetailName, bStoredProcedure:=True, vResultArray:=r_vResultArray)



            If result = gPMConstants.PMEReturnCode.PMTrue Then
                If Not Information.IsArray(r_vResultArray) Then
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function AddDataFixUtilityLog(ByVal v_sCreatedBy As String, ByVal v_sBordereauReference As String,
                                        ByVal v_sDepositNumber As String) As Long
        Dim nResult As Integer = 0
        Dim nRecordsAffected As Integer
        Try

            nResult = PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Created_by", vValue:=v_sCreatedBy,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="BordereauReference", vValue:=v_sBordereauReference,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DepositNumber", vValue:=v_sDepositNumber,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DomainAaccount", vValue:=System.Security.Principal.WindowsIdentity.GetCurrent().Name,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ApplicationLoggedInAccount", vValue:=m_sUsername,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_DataFixUtility_log_add",
                                              sSQLName:="spu_DataFixUtility_log_add",
                                              bStoredProcedure:=True,
                                              lRecordsAffected:=nRecordsAffected)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError,
                               sMsg:="GetPolicyVersionDocument Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetPolicyVersionDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return nResult
        End Try
    End Function

    Public Function SearchBordereau(ByVal nDepositNumber As String, ByVal sBordereauReference As String,
                                    ByVal nUserID As Integer, ByRef r_oResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            If nDepositNumber <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="DepositNumber", vValue:=nDepositNumber,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If
            End If

            If sBordereauReference <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="BordereauReference", vValue:=sBordereauReference,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=nUserID,
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=KSearchBordereauSQL,
                                              sSQLName:=KSearchBordereauName,
                                              bStoredProcedure:=KSearchBordereauStored,
                                              vResultArray:=r_oResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError,
                               sMsg:="GetPolicyVersionDocument Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetPolicyVersionDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function AddMissingTask(ByVal nBordereauTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="BordereauTransactionId", vValue:=CStr(nBordereauTransactionId),
                                                   iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=KAddMissingTaskSQL,
                                              sSQLName:=KAddMissingTaskName,
                                              bStoredProcedure:=KAddMissingTaskStored)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception
            result = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=PMELogLevel.PMLogOnError,
                               sMsg:="GetPolicyVersionDocument Failed", vApp:=ACApp, vClass:=ACClass,
                               vMethod:="GetPolicyVersionDocument", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' This will create or refresh an entire reinsurance arrangement for
    ' the given risk, including the original reinsurance
    ' ***************************************************************** '
    Public Function CalculateRI(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim m_vIsRI2007 As Object
        Try

            result = PMEReturnCode.PMTrue
            result = bPMFunc.getProductOptionValue(v_sUsername:="", v_sPassword:="", v_iUserID:=0, v_iMainSourceID:=0, v_iLanguageID:=0, v_iCurrencyID:=0, v_iLogLevel:=0, v_sCallingAppName:="", v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=1, r_vUnderwriting:=m_vIsRI2007)

            If m_vIsRI2007 <> "1" Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "transtype", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRefreshRISQL, sSQLName:=ACRefreshRIName, bStoredProcedure:=ACRefreshRIStored)
            Else
                ' Add parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", v_lRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                bPMAddParameter.AddParameterLite(m_oDatabase, "Trans_type", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRefreshRI2007SQL, sSQLName:=ACRefreshRI2007Name, bStoredProcedure:=ACRefreshRI2007Stored)

            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACRefreshRI2007SQL)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return result
        Catch ex As Exception
            result = PMEReturnCode.PMFalse
            'Debugger.Break()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateRI", vErrNo:=Information.Err().Number, vErrDesc:=ex.Message)
            Return result
        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
    End Function

    '******************************************************************************
    ' Update the Comment of reversal document in Transdetail table
    '******************************************************************************
    Public Function UpdateTransDetailComment(ByVal v_lDocument_ID As Integer, ByVal v_sOldDocumentRef As String) As Integer

        Dim result As Integer = 0
        On Error GoTo Catch_Renamed
        result = gPMConstants.PMEReturnCode.PMTrue
        m_oDatabase.Parameters.Clear()

        result = m_oDatabase.Parameters.Add(sName:="NewDocument_ID", vValue:=CStr(v_lDocument_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.Parameters.Add(sName:="OldDocumentRef", vValue:=v_sOldDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        result = m_oDatabase.SQLAction(sSQL:=ACUpdTransCommentSQL, sSQLName:=ACUpdTransCommentName, bStoredProcedure:=ACUpdTransCommentStored)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Return result
        End If

        GoTo Finally_Renamed

Catch_Renamed:

        result = gPMConstants.PMEReturnCode.PMError
        bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateTransDetailComment Faild", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTransDetailComment()", vErrNo:=CStr(Information.Err().Number), vErrDesc:=Information.Err().Description)

Finally_Renamed:
        Return result

    End Function

    '******************************************************************************
    ' Check is there any claim created on this policy version
    '******************************************************************************
    Public Function GetClaimOnPolicyVersion(ByVal v_lInsurance_File_cnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_cnt", vValue:=v_lInsurance_File_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimSQL, sSQLName:=ACGetClaimName, bStoredProcedure:=ACGetClaimStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllPolicyVersion", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function

    Public Function CreateStats(ByRef nStatsFolderCnt As Integer, Optional sDocumentRef As String = "") As Integer

        Dim nResult As Integer
        Dim nTransactionExportFolderCnt As Integer
        Dim oTransactions As Object
        Dim bIsPT As Boolean
        Dim sPaymentMethod As String
        Dim r_sFailureReason As String


        nResult = gPMConstants.PMEReturnCode.PMTrue

        Dim obControlTrans As Object
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=obControlTrans, v_sClassName:="bControlTrans.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("CreateStats", "Initilizing the componenet bControlTrans.Automated failed", gPMConstants.PMELogLevel.PMLogError)
            Return nResult
        End If


        If m_sNextOrionDocRef.Trim() = "" Then
            'Get the next document ref number
            m_lReturn = GetNextOrionDocRef(r_sDocumentRef:=m_sNextOrionDocRef)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", CreateStats, GetNextOrionDocRef Failed")
            End If
        End If
        If Not m_bIsSDDTransaction Then
            If m_bIsCloned Then

                m_bIsPT = bIsPT
                m_nClonedReversalDocumentID = 0
                If bIsPT = False Then
                    m_lReturn = CreateStatsFolder(lStatsFolderCnt:=nStatsFolderCnt, IsClonedReverse:=True)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = CreateClonedStatsDetails(lStatsFolderCnt:=nStatsFolderCnt, lClonedInsuranceFileCnt:=m_lInsuranceFileCnt)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = CreateClonedExport(nStatsFolderCnt:=nStatsFolderCnt, r_nTransactionExportFolderCnt:=nTransactionExportFolderCnt, nInsuranceFileCnt:=m_lInsuranceFileCnt)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = PostDocument(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_vTransactions:=oTransactions, r_nDocumentID:=m_nClonedReversalDocumentID)
                    'm_lReturn = SendToOrion(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_lDocumentId:=m_nClonedReversalDocumentID)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If


            End If
        Else
            m_lReturn = CreateStatsFolder(lStatsFolderCnt:=nStatsFolderCnt, sDocumentRef:=sDocumentRef)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If m_bReversePT Then
            'm_lReturn = CreatePTStatsDetails(lStatsFolderCnt:=nStatsFolderCnt, lPTInsuranceFileCnt:=m_lClonedInsuranceFileCnt)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    Return gPMConstants.PMEReturnCode.PMFalse
            'End If

            'End If


            m_lReturn = CreateStatsDetails(lStatsFolderCnt:=nStatsFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CreateExport(lStatsFolderCnt:=nStatsFolderCnt, lTransactionExportFolderCnt:=nTransactionExportFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to create Transactions"
                Return nResult
            End If

            m_lReturn = PostDocument(v_lTransactionExportFolderCnt:=nTransactionExportFolderCnt, r_vTransactions:=oTransactions, sPaymentMethod:=sPaymentMethod, r_sFailureReason:=r_sFailureReason)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                m_sMessage = "Failed to post Transactions to Orion" + r_sFailureReason
                Return nResult
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

        End If
        m_sNextOrionDocRef = ""

        Return m_lReturn

    End Function

    ' ***************************************************************** '
    ' Name: CreateExport (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExport(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CreateExportFolderClone(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not (lStatsFolderCnt > 0) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CreateExportDetailsClone(lStatsFolderCnt:=lStatsFolderCnt, lTransactionExportFolderCnt:=lTransactionExportFolderCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateExportFolder (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExportFolderClone(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            If lStatsFolderCnt = Nothing Then
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=lStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an OUTPUT param for an insert
            If lTransactionExportFolderCnt = Nothing Then
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=lTransactionExportFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Folder SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportFolderSQL, sSQLName:=ACAddExportFolderName, bStoredProcedure:=ACAddExportFolderStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        lTransactionExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value
        If lTransactionExportFolderCnt < 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreateExportDetails (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateExportDetailsClone(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(lStatsFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(lTransactionExportFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportDetailsSQL, sSQLName:=ACAddExportDetailsName, bStoredProcedure:=ACAddExportDetailsStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function CreateStatsFolder(ByRef lStatsFolderCnt As Integer, Optional ByVal IsClonedReverse As Boolean = False, Optional ByVal sDocumentRef As String = "") As Integer
        Dim nResult As Integer
        Dim lRecordsAffected As Integer


        nResult = PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=CStr(0), iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(m_lInsuranceFileCnt), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = .Parameters.Add(sName:="user_name", vValue:=m_sUsername, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)

            m_lReturn = .Parameters.Add(sName:="next_orion_doc_ref", vValue:=m_sNextOrionDocRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)


            If m_bBackDateMTA = True Then
                m_lReturn = .Parameters.Add(sName:="is_out_of_sequence", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If

            If m_bIsPT Then
                m_lReturn = .Parameters.Add(sName:="transfer_date", vValue:=m_dtTransferDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)
            End If

            If IsClonedReverse = True Then
                m_lReturn = .Parameters.Add(sName:="is_cloned_reverse", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
            End If

            If sDocumentRef <> "" Then
                m_lReturn = .Parameters.Add(sName:="Original_Doc_Ref", vValue:=sDocumentRef, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMString)
            End If
        End With

        ' Execute Add Stats Folder SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_add_stats_folder_Datafix", sSQLName:=ACAddStatsFolderName, bStoredProcedure:=ACAddStatsFolderStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        lStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value
        If lStatsFolderCnt < 1 Then
            Return PMEReturnCode.PMFalse
        End If

        Return nResult

    End Function

    Private Function CreatePTStatsDetails( _
        lStatsFolderCnt As Long, lPTInsuranceFileCnt As Long) As Long

        Dim lRecordsAffected As Long


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase


            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add( _
                  sName:="PTInsuranceFileCnt", _
                  vValue:=lPTInsuranceFileCnt, _
                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                  iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = .Parameters.Add( _
                 sName:="StatsFolderCnt", _
                 vValue:=lStatsFolderCnt, _
                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                 iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End With

        ' Execute Add Stats Details SQL Statement
        'SR 03/11/2K
        ' This SP takes more than 30 seconds.
        ' Since the timeout in PMDAO is 15 secs only, it terminates with timeout error. need checking.
        m_lReturn = m_oDatabase.SQLAction( _
            sSQL:=ACAddPTStatsDetailsSQL, _
            sSQLName:=ACAddPTStatsDetailsName, _
            bStoredProcedure:=ACAddPTStatsDetailsStored, _
            lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        Return m_lReturn
    End Function

    ' ***************************************************************** '
    ' Name: CreateStatsDetails (Private)
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function CreateStatsDetails(ByRef lStatsFolderCnt As Integer) As Integer

        Dim nResult As Integer
        Dim nRecordsAffected As Integer



        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=lStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="only_ri", vValue:=IIf(m_bIsPT, 1, IIf(m_bIsCloned, 2, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Stats Details SQL Statement
        'SR 03/11/2K
        ' This SP takes more than 30 seconds.
        ' Since the timeout in PMDAO is 15 secs only, it terminates with timeout error. need checking.
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddStatsDetailsCloneSQL, sSQLName:=ACAddStatsDetailsCloneName, bStoredProcedure:=ACAddStatsDetailsCloneStored, lRecordsAffected:=nRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SR  03/11/2K - It always returns zero even if some detail records are added.
        'Needs checking
        '    If (lRecordsAffected < 1) Then
        '        CreateStatsDetails = PMFalse
        '        Exit Function
        '    End If

        Return nResult

    End Function

    Private Function CreateClonedStatsDetails( _
   lStatsFolderCnt As Long, lClonedInsuranceFileCnt As Long) As Integer

        Dim lRecordsAffected As Long



        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase


            ' Add StatsFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add( _
                  sName:="ClonedInsuranceFileCnt", _
                  vValue:=lClonedInsuranceFileCnt, _
                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                  iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_lReturn = .Parameters.Add( _
                 sName:="StatsFolderCnt", _
                 vValue:=lStatsFolderCnt, _
                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                 iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End With

        ' Execute Add Stats Details SQL Statement
        'SR 03/11/2K
        ' This SP takes more than 30 seconds.
        ' Since the timeout in PMDAO is 15 secs only, it terminates with timeout error. need checking.
        m_lReturn = m_oDatabase.SQLAction( _
            sSQL:=ACAddClonedStatsDetailsSQL, _
            sSQLName:=ACAddClonedStatsDetailsName, _
            bStoredProcedure:=ACAddClonedStatsDetailsStored, _
            lRecordsAffected:=lRecordsAffected)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If
        Return m_lReturn
    End Function

    Private Function CreateClonedExport(ByRef nStatsFolderCnt As Integer, ByRef r_nTransactionExportFolderCnt As Integer,
                                      ByVal nInsuranceFileCnt As Integer) As Integer
        Dim nResult As Integer
        Dim nTransactionExportFolderCnt As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        m_lReturn = CreateExportFolderMissingSDR(lStatsFolderCnt:=nStatsFolderCnt,
                                       lTransactionExportFolderCnt:=nTransactionExportFolderCnt)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (nTransactionExportFolderCnt > 0) = False Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = CreateClonedExportDetails(nInsuranceFileCnt:=nInsuranceFileCnt,
                                              nTransactionExportFolderCnt:=nTransactionExportFolderCnt,
                                              nStatsFolderCnt:=nStatsFolderCnt)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        r_nTransactionExportFolderCnt = nTransactionExportFolderCnt
        Return nResult
    End Function

    Private Function CreateClonedExportDetails(ByVal nInsuranceFileCnt As Integer, ByVal nTransactionExportFolderCnt As Integer, ByVal nStatsFolderCnt As Integer) _
       As Integer

        Dim nRecordsAffected As Integer
        Dim nResult As Integer

        nResult = gPMConstants.PMEReturnCode.PMTrue
        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            m_lReturn = .Parameters.Add(sName:="ClonedInsuranceFileCnt", vValue:=nInsuranceFileCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an INPUT param for an insert
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt",
                                        vValue:=nTransactionExportFolderCnt,
                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                        iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="Stats_folder_cnt",
                                       vValue:=nStatsFolderCnt,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Details SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kCopyClonedExportDetailsSQL,
                                          sSQLName:=kCopyClonedExportDetailsName,
                                          bStoredProcedure:=kCopyClonedExportDetailsStored,
                                          lRecordsAffected:=nRecordsAffected)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return nResult
    End Function

    Private Function ProcessClonedTransactions(nStatsFolderCnt As Integer,
                                              ByVal oTransactions(,) As Object) As Integer

        Dim oAllocationManual As Object
        Dim iRow As Integer
        Dim oCreditTransactions(,) As Object
        Dim oKeyArray(1, 3) As Object
        Dim iCount As Integer
        Dim oTrans(0) As Object
        Dim nResult As Integer
        Try

            nResult = PMEReturnCode.PMTrue

            'Get an instance of bACTAllocationManual component to do the allocation
            If CreateBusinessObject(
                r_oObject:=oAllocationManual,
                v_sClassName:="bACTAllocationManual.Business",
                v_sCallingAppName:=ACApp,
                v_sUsername:=m_sUsername$,
                v_sPassword:=m_sPassword$,
                v_iUserID:=m_iUserID%,
                v_iSourceID:=m_iSourceID%,
                v_iLanguageID:=m_iLanguageID%,
                v_iCurrencyID:=m_iCurrencyID%,
                v_iLogLevel:=m_iLogLevel,
                v_oDatabase:=m_oDatabase) <> PMEReturnCode.PMTrue Then

                Return PMEReturnCode.PMFalse

            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nStatsFolderCnt", vValue:=nStatsFolderCnt,
                                                      iDirection:=PMEParameterDirection.PMParamInput,
                                                      iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt", vValue:=m_lInsuranceFileCnt,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_clm_get_transdetails_RI", sSQLName:="GetTransDetails",
                                              bStoredProcedure:=True, vResultArray:=oCreditTransactions)

            If (nStatsFolderCnt < 1) Then
                Return PMEReturnCode.PMFalse
            End If

            For iCount = 0 To UBound(oTransactions, 2)

                For iRow = 0 To UBound(oCreditTransactions, 2)

                    If oTransactions(0, iCount) = oCreditTransactions(0, iRow) Then
                        If Math.Abs(ToSafeInteger(oTransactions(2, iCount))) = Math.Abs(ToSafeInteger(oCreditTransactions(2, iRow))) AndAlso
                           oTransactions(3, iCount) = oCreditTransactions(3, iRow) Then

                            oTrans(0) = oTransactions(1, iCount) & "|" & oTransactions(2, iCount)

                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 0) = ACTKeyNameAccountID
                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 0) = oCreditTransactions(0,
                                                                                                                   iRow)

                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 1) = ACTKeyNameTransDetailID
                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 1) =
                                oCreditTransactions(1, iRow) & "|" & oCreditTransactions(2, iRow)

                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyName, 2) = ACTKeyNameTransDetailIDs
                            oKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, 2) = oTrans

                            m_lReturn = oAllocationManual.SetKeys(vKeyArray:=oKeyArray)

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            m_lReturn = oAllocationManual.Start()

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="IsPortfolioTransferVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClonedTransaction", vErrNo:=Err.Number, vErrDesc:=ex.Message, excep:=ex)
        Finally
            oAllocationManual.Dispose()
            oAllocationManual = Nothing
        End Try
        Return nResult
    End Function

    Private Function CreateExportFolderMissingSDR(ByRef lStatsFolderCnt As Integer, ByRef lTransactionExportFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        With m_oDatabase

            ' Add the Input Params
            If lStatsFolderCnt = Nothing Then
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="stats_folder_cnt", vValue:=lStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add ExportFolderCnt as an OUTPUT param for an insert
            If lTransactionExportFolderCnt = Nothing Then
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=lTransactionExportFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        ' Execute Add Trans Export Folder SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddExportFolderSQL, sSQLName:=ACAddExportFolderName, bStoredProcedure:=ACAddExportFolderStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the Cnt of the record inserted
        lTransactionExportFolderCnt = m_oDatabase.Parameters.Item("transaction_export_folder_cnt").Value
        If lTransactionExportFolderCnt < 1 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function PostDocument(ByVal v_lTransactionExportFolderCnt As Integer, Optional ByRef r_vTransactions(,) As Object = Nothing, Optional ByVal sPaymentMethod As String = "", Optional ByRef r_nDocumentID As Integer = 0, Optional ByRef r_sFailureReason As String = "") As Integer

        Dim oTransactionBusiness As bPMBTransactions.Automated
        Dim nDocumentId As Integer



        'Create an instance of PMBTransaction
        oTransactionBusiness = New bPMBTransactions.Automated()

        'Initialise the object
        m_lReturn = oTransactionBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, vDatabase:=m_oDatabase)

        'Set the process modes
        m_lReturn = oTransactionBusiness.SetProcessModes(m_iTask, m_lNavigate, m_lProcessMode, m_sTransactionType, m_dtEffectiveDate)

        'Call the function to Send the Transaction to Orion
        m_lReturn = oTransactionBusiness.SendToOrion(v_lTransactionExportFolderCnt, r_lDocumentId:=nDocumentId, r_sFailureReason:=r_sFailureReason)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMError
        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If
        r_nDocumentID = nDocumentId

        Dim oEnableDebitOrder As Object

        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="", _
                     v_sPassword:="", v_iUserID:=0, _
                     v_iMainSourceID:=0, v_iLanguageID:=0, _
                     v_iCurrencyID:=0, v_iLogLevel:=0, _
                     v_sCallingAppName:="", _
                     v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableDebitOrder, _
                     v_vBranch:=1, _
                     r_vUnderwriting:=oEnableDebitOrder)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_sMessage = "Failed to get product option."
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If ToSafeString(oEnableDebitOrder, "") = "1" Then
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("Document_Id", nDocumentId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(kAddTransDetailExSQL, kAddTransDetailExName, kAddTransDetailExStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        'Get an Array of TransDetailsId and Amount From lDocumentID
        Dim sSQL As String = ""
        m_oDatabase.Parameters.Clear()
        sSQL = "Select Account_id,transdetail_id , OutStanding_Amount, transdetail_type_id From TransDetail " & _
               " Where Document_id=" & CStr(nDocumentId) & "  and Amount<>0"
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransDetails", bStoredProcedure:=False, vResultArray:=r_vTransactions)

        ' Database error encountered
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If

        Return m_lReturn

    End Function

    Public Function CalculateAgentCommission(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String, ByRef r_vntResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            'Add the parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_file_cnt", vValue:=CStr(nInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'DN 21/02/03 - ISS 2274:Pass the transaction type as an extra parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Call the Stored procedure
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCalculateAgentCommissionSQL, sSQLName:=ACCalculateAgentCommissionName, bStoredProcedure:=ACCalculateAgentCommisionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vntResult)


            Return m_lReturn

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAgentCommission", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    Public Function RecalculateRiskTaxes(ByVal nInsuranceFileCnt As Integer, ByVal nRiskCnt As Integer, ByVal v_lTask As Integer,
                                        ByVal sTransactionType As String) As Integer

        Dim nResult As Integer
        Const kMethodName As String = "RecalculateRiskTaxes"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oSIRRITax As bSIRRITax.Business
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            oSIRRITax = New bSIRRITax.Business
            m_lReturn = oSIRRITax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRRITax.InsuranceFileCnt = nInsuranceFileCnt


            ' recalculate the policy risk taxes
            'Developer Guide No.20
            oSIRRITax.m_oDatabase = m_oDatabase
            lReturn = oSIRRITax.RecalculatePolicyRiskTaxes(v_lRiskCnt:=nRiskCnt, v_lTask:=0,
                                                                     v_sTransactionType:=sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "RecalculatePolicyRiskTaxes Failed",
                                                gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            oSIRRITax.Dispose()
            oSIRRITax = Nothing


        End Try
        Return nResult
    End Function

    Public Function UpdateInsuranceFilePremium(ByVal nInsuranceFileCnt As Integer) As Long
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()
            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            result = m_oDatabase.SQLAction(sSQL:="spu_Update_Insurance_File_Premium_DataFix", sSQLName:="UpdateInsuranceFilePremium", bStoredProcedure:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result
        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInsuranceFilePremium Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFilePremium", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result
        End Try

    End Function

    Public Function GetTransactionTypeCode(ByVal v_sDocumentRef As String, ByRef r_Transaction_Type_Code As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray As Object

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransactionTypeCodeSQL, sSQLName:=ACGetTransactionTypeCodeName, bStoredProcedure:=ACGetTransactionTypeCodeStored, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then
                r_Transaction_Type_Code = vResultArray(0, 0)
            Else
                r_Transaction_Type_Code = ""
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsDocumentInAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsDocumentInAccount", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)

            Return result
        End Try
    End Function


    Public Function ProceedTaxFix(ByVal v_sDocumentRef As String) As Integer


        Dim aoResultArray(,) As Object
        Dim result As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sAppName As String = "ProceedTaxFix"

        Try

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="DocumentRef", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACTaxFixSQL,
                                                sSQLName:=ACTaxFixName,
                                                bStoredProcedure:=ACTaxFixStored)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If



            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception

            End If


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If


            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ProceedTaxFix Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try

    End Function

    Public Function RefreshTax(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            result = m_oDatabase.Parameters.Add(sName:="transaction_type", vValue:=sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            result = m_oDatabase.SQLAction(sSQL:="spu_Refresh_Tax_DataFix", sSQLName:="RefreshTax", bStoredProcedure:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If
            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RefreshTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateTaxCalc", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try

    End Function

End Class
