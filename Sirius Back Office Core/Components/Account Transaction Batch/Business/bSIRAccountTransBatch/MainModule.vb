Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
'Modified by Vijay Pal on 5/31/2010 1:17:57 PM refer developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("MainModule_NET.MainModule")> _
Module MainModule

    Public Const ACApp As String = "bSIRAccountTransBatch"

    Private Const ACClass As String = "MainModule"
    Private Const ACSelectAccountsPartyQueueStored As Boolean = True
    Private Const ACSelectAccountsPartyQueueName As String = "SelectAccountsPartyQueue"
    Private Const ACSelectAccountsPartyQueueSQL As String = "{call spu_accounts_party_queue_sel}"

    Private Const ACSelectAccountsTransactionQueueStored As Boolean = True
    Private Const ACSelectAccountsTransactionQueueName As String = "SelectAccountsTransactionQueue"
    Private Const ACSelectAccountsTransactionQueueSQL As String = "{call spu_accounts_transaction_queue_sel}"

    Private Const ACUpdateAccountsPartyQueueStored As Boolean = True
    Private Const ACUpdateAccountsPartyQueueName As String = "UpdateAccountsPartyQueue"
    Private Const ACUpdateAccountsPartyQueueSQL As String = "{call spu_accounts_party_queue_upd (?,?,?,?)}"

    Private Const ACUpdateAccountsTransactionQueueStored As Boolean = True
    Private Const ACUpdateAccountsTransactionQueueName As String = "UpdateAccountsTransactionQueue"
    Private Const ACUpdateAccountsTransactionQueueSQL As String = "{call spu_accounts_transaction_queue_upd (?,?,?,?)}"

    Private Const ACPartyCnt As Integer = 0
    Private Const ACTransactionExportFolderCnt As Integer = 0
    Private Const ACCreateDate As Integer = 1
    Private Const ACCommitInd As Integer = 2
    Private Const ACCommitDate As Integer = 3

    Private Const ACCommitOk As Integer = 1
    Private Const ACCommitFail As Integer = 2

    Private Const ACRegKeyMultiBranchAccounting As String = "MultiBranchAccounting"
    Private Const ACRegStringExecutable As String = "Executable"
    Private Const ACRegStringSubscriber As String = "Subscriber"
    Private Const ACRegStringSubscriberDB As String = "SubscriberDB"
    Private Const ACRegStringPublisher As String = "Publisher"
    Private Const ACRegStringDistributor As String = "Distributor"
    Private Const ACRegStringDistributorSecurityMode As String = "DistributorSecurityMode"
    Private Const ACRegStringPublisherDB As String = "PublisherDB"

    Private m_oOrionUpdate As bSIROrionUpdate.Business
    Private m_oTransactions As bPMBTransactions.Automated

    Private m_oSiriusDatabase As dPMDAO.Database
    Private m_oOrionDatabase As dPMDAO.Database
    'SD 31/07/2002 Scalability Changes
    'Private m_oComponentServices As PMServerBusinessCS

    Private m_lReturn As gPMConstants.PMEReturnCode

    'Public g_sUsername As String
    'Public g_sPassword As String
    'Public g_iUserId As Integer
    'Public g_iSourceId As Integer
    'Public g_iLanguageId As Integer
    'Public g_iCurrencyId As Integer
    'Public g_iLoglevel As Integer
    'Public g_sCallingAppName As String

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserId As Integer
    Private m_iSourceId As Integer
    Private m_iLanguageId As Integer
    Private m_iCurrencyId As Integer
    Private m_iLoglevel As Integer
    Private m_sCallingAppName As String = ""


    Public Sub Main()

        'Initialise objects and database references
        m_lReturn = CType(Initialise(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Exit Sub
        End If

        'Process the account queue and create the accounts
        m_lReturn = CType(ProcessAccountsQueue(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ProcessAccountsQueue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Exit Sub
        End If

        'Run a script to run the "distribution" agent to replicate accounts
        'to the branch databases

        '    m_lReturn = ReplicateAccountData()
        '
        '    If m_lReturn <> PMTrue Then
        '        LogMessage _
        ''            iType:=PMError, _
        ''            sMsg:="ReplicateAccountData Failed", _
        ''            vApp:=ACApp, _
        ''            vClass:=ACClass, _
        ''            vMethod:="Main"
        '        Exit Sub
        '    End If

        'Process the transactions queue and create the accounts
        m_lReturn = CType(ProcessTransactionsQueue(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ProcessTransactionsQueue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Exit Sub
        End If

        'Terminate objects and database references
        m_lReturn = CType(Terminate(), gPMConstants.PMEReturnCode)



    End Sub
    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 09/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function Initialise() As Integer




        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_sUsername = "sirius"
        m_sPassword = "sirius"
        m_iUserId = 1
        m_iSourceId = 1
        m_iLanguageId = 1
        m_iCurrencyId = 1
        m_iLoglevel = 6
        m_sCallingAppName = ACApp

        'SD 31/07/2002
        '    Set m_oComponentServices = New PMServerBusinessCS

        'Create sirius database
        m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceId, v_iLanguageID:=m_iLanguageId, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=m_oSiriusDatabase), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create Sirius database", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Create orion database
        m_lReturn = CType(gPMComponentServices.NewDatabase(v_sUsername:=m_sUsername, v_iSourceID:=m_iSourceId, v_iLanguageID:=m_iLanguageId, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_oDatabase:=m_oOrionDatabase), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create Orion database", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Create bSIROrionUpdate.Business
        m_oOrionUpdate = New bSIROrionUpdate.Business
        m_lReturn = CType(m_oOrionUpdate.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserId, iSourceID:=m_iSourceId, iLanguageID:=m_iLanguageId, iCurrencyID:=m_iCurrencyId, iLogLevel:=m_iLoglevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create bSIROrionUpdate.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Create bPMBTransactions.Automated
        'SD 31/07/2002
        m_oTransactions = New bPMBTransactions.Automated
        m_lReturn = CType(m_oTransactions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserId, iSourceID:=m_iSourceId, iLanguageID:=m_iLanguageId, iCurrencyID:=m_iCurrencyId, iLogLevel:=m_iLoglevel, sCallingAppName:=ACApp), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to create bPMBTransactions.Automated", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 09/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function Terminate() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'SD 31/07/2002 Scalability Changes
        '    Set m_oComponentServices = Nothing

        ' Terminate account business object
        If Not (m_oOrionUpdate Is Nothing) Then

            m_oOrionUpdate.Dispose()
            m_oOrionUpdate = Nothing
        End If

        ' Terminate transactions business object
        If Not (m_oTransactions Is Nothing) Then

            m_oTransactions.Dispose()
            m_oTransactions = Nothing
        End If

        'Terminate Sirius database
        If Not (m_oSiriusDatabase Is Nothing) Then
            m_lReturn = m_oSiriusDatabase.CloseDatabase()
            m_oSiriusDatabase = Nothing
        End If

        'Terminate Orion database
        If Not (m_oOrionDatabase Is Nothing) Then
            m_lReturn = m_oOrionDatabase.CloseDatabase()
            m_oOrionDatabase = Nothing
        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: ProcessAccountsQueue
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessAccountsQueue() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vAccountsPartyQueueArray(,) As Object = Nothing
        Dim lUboundAccountsPartyQueueArray, lPartyCnt As Integer
        Dim iCommitInd As Integer

        'Get a list of all the accounts we need to create/update
        m_lReturn = CType(GetAccountsPartyQueue(r_vAccountsPartyQueueArray:=vAccountsPartyQueueArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetAccountsPartyQueue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccountsQueue")
            Return gPMConstants.PMEReturnCode.PMError
        End If

        If Not Information.IsArray(vAccountsPartyQueueArray) Then
            'No accounts to create so exit here
            Return result
        End If


        lUboundAccountsPartyQueueArray = vAccountsPartyQueueArray.GetUpperBound(1)

        'Loop around all the uncommitted accounts in the queue
        For lCnt As Integer = 0 To lUboundAccountsPartyQueueArray

            lPartyCnt = CInt(vAccountsPartyQueueArray(ACPartyCnt, lCnt))
            'Create/Update the account

            m_lReturn = m_oOrionUpdate.SiriusToOrionBatch(v_lPartyCnt:=lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oOrionUpdate.SiriusToOrionBatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccountsQueue")
                iCommitInd = ACCommitFail
            Else
                iCommitInd = ACCommitOk
            End If

            'Update the status on the account_party_queue table
            m_lReturn = CType(UpdateAccountsPartyQueue(v_lPartyCnt:=lPartyCnt, v_iCommitInd:=iCommitInd), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UdateAccountsPartyQueue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessAccountsQueue")
            End If

        Next lCnt

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ProcessTransactionsQueue
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function ProcessTransactionsQueue() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vAccountsTransactionQueueArray(,) As Object = Nothing
        Dim lUboundAccountsTransactionQueueArray, lTransactionExportFolderCnt As Integer
        Dim iCommitInd As Integer

        'Get a list of all the accounts we need to create/update
        m_lReturn = CType(GetAccountsTransactionQueue(r_vAccountsTransactionQueueArray:=vAccountsTransactionQueueArray), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="GetAccountsTransactionQueue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionsQueue")
            Return gPMConstants.PMEReturnCode.PMError
        End If

        If Not Information.IsArray(vAccountsTransactionQueueArray) Then
            'No transactions to create so exit here
            Return result
        End If


        lUboundAccountsTransactionQueueArray = vAccountsTransactionQueueArray.GetUpperBound(1)

        'Loop around all the uncommitted accounts in the queue
        For lCnt As Integer = 0 To lUboundAccountsTransactionQueueArray


            lTransactionExportFolderCnt = CInt(vAccountsTransactionQueueArray(ACTransactionExportFolderCnt, lCnt))

            'Create/Update the transactions

            m_lReturn = m_oTransactions.SendToOrion(v_lTransactionFolderCnt:=lTransactionExportFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oTransactions.SendToOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionsQueue")
                iCommitInd = ACCommitFail
            Else
                iCommitInd = ACCommitOk
            End If

            'Update the status on the account_party_queue table
            m_lReturn = CType(UpdateAccountsTransactionQueue(v_lTransactionExportFolderCnt:=lTransactionExportFolderCnt, v_iCommitInd:=iCommitInd), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UdateAccountsPartyQueue Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessTransactionsQueue")
            End If

        Next lCnt

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountsPartyQueue
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetAccountsPartyQueue(ByRef r_vAccountsPartyQueueArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oSiriusDatabase

            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=ACSelectAccountsPartyQueueSQL, sSQLName:=ACSelectAccountsPartyQueueName, bStoredProcedure:=ACSelectAccountsPartyQueueStored, lNumberRecords:=10000, vResultArray:=r_vAccountsPartyQueueArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to get AccountsPartyQueue from database", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountsPartyQueue")
                Return result
            End If
        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAccountsTransactionQueue
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetAccountsTransactionQueue(ByRef r_vAccountsTransactionQueueArray(,) As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oSiriusDatabase

            .Parameters.Clear()

            m_lReturn = .SQLSelect(sSQL:=ACSelectAccountsTransactionQueueSQL, sSQLName:=ACSelectAccountsTransactionQueueName, bStoredProcedure:=ACSelectAccountsTransactionQueueStored, lNumberRecords:=10000, vResultArray:=r_vAccountsTransactionQueueArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to get AccountsTransactionQueue from database", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountsTransactionQueue")
                Return result
            End If
        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateAccountsPartyQueue
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateAccountsPartyQueue(ByVal v_lPartyCnt As Integer, ByVal v_iCommitInd As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oSiriusDatabase

            .Parameters.Clear()

            'Party_cnt
            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create_date

            'Modified by Vijay Pal on 5/31/2010 1:18:12 PM refer developer guide no. 85
            'm_lReturn = .Parameters.Add(sName:="create_date", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=DBNull.Value, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_ind
            m_lReturn = .Parameters.Add(sName:="commit_ind", vValue:=CStr(v_iCommitInd), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_date
            m_lReturn = .Parameters.Add(sName:="commit_date", vValue:=DateTimeHelper.ToString(DateTime.Now), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACUpdateAccountsPartyQueueSQL, sSQLName:=ACUpdateAccountsPartyQueueName, bStoredProcedure:=ACUpdateAccountsPartyQueueStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateAccountsTransactionQueue
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateAccountsTransactionQueue(ByVal v_lTransactionExportFolderCnt As Integer, ByVal v_iCommitInd As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oSiriusDatabase

            .Parameters.Clear()

            'transaction_export_folder_cnt
            m_lReturn = .Parameters.Add(sName:="transaction_export_folder_cnt", vValue:=CStr(v_lTransactionExportFolderCnt), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create_date

            'Modified by Vijay Pal on 5/31/2010 1:18:32 PM refer developer guide no. 85
            'm_lReturn = .Parameters.Add(sName:="create_date", vValue:=CStr(DBNull.Value), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = .Parameters.Add(sName:="create_date", vValue:=DBNull.Value, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_ind
            m_lReturn = .Parameters.Add(sName:="commit_ind", vValue:=CStr(v_iCommitInd), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'commit_date
            m_lReturn = .Parameters.Add(sName:="commit_date", vValue:=DateTimeHelper.ToString(DateTime.Now), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACUpdateAccountsTransactionQueueSQL, sSQLName:=ACUpdateAccountsTransactionQueueName, bStoredProcedure:=ACUpdateAccountsTransactionQueueStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: ReplicateAccountData
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (ReplicateAccountData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function ReplicateAccountData() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim vExecutable As String = ""
    'Dim vSubscriber As String = ""
    'Dim vSubscriberDB As String = ""
    'Dim vPublisher As String = ""
    'Dim vDistributor As String = ""
    'Dim vDistributorSecurityMode As String = ""
    'Dim vPublisherDB, sSQL As String
    '
    ' Get the parameters to pass to the command
    'm_lReturn = CType(GetMultiBranchRegistrySettings(r_vExecutable:=vExecutable, r_vSubscriber:=vSubscriber, r_vSubscriberDB:=vSubscriberDB, r_vPublisher:=vPublisher, r_vDistributor:=vDistributor, r_vDistributorSecurityMode:=vDistributorSecurityMode, r_vPublisherDB:=vPublisherDB), gPMConstants.PMEReturnCode)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="ReplicateAccountData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplicateAccountData")
    'Return result
    'End If
    '
    'Build the sql string
    'sSQL = "EXEC master..xp_cmdshell '"
    'sSQL = sSQL & vExecutable
    'sSQL = sSQL & " -Subscriber [" & vSubscriber & "]"
    'sSQL = sSQL & " -SubscriberDB [" & vSubscriberDB & "]"
    'sSQL = sSQL & " -Publisher [" & vPublisher & "]"
    'sSQL = sSQL & " -Distributor [" & vDistributor & "]"
    'sSQL = sSQL & " -DistributorSecurityMode " & vDistributorSecurityMode
    'sSQL = sSQL & " -PublisherDB [" & vPublisherDB & "]'"
    '
    'Run it against the database
    'With m_oSiriusDatabase
    '
    '.Parameters.Clear()
    'm_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:="ReplicateAccountData", bStoredProcedure:=False)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMEReturnCode.PMError, sMsg:="Failed to run distribution script", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplicateAccountData")
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReplicateAccountData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplicateAccountData", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetMultiBranchRegistrySettings
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetMultiBranchRegistrySettings(Optional ByRef r_vExecutable As Object = Nothing, Optional ByRef r_vSubscriber As Object = Nothing, Optional ByRef r_vSubscriberDB As Object = Nothing, Optional ByRef r_vPublisher As Object = Nothing, Optional ByRef r_vDistributor As Object = Nothing, Optional ByRef r_vDistributorSecurityMode As Object = Nothing, Optional ByRef r_vPublisherDB As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Executable

        If Not Information.IsNothing(r_vExecutable) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringExecutable, r_vValue:=CStr(r_vExecutable)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'Subscriber

        If Not Information.IsNothing(r_vSubscriber) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringSubscriber, r_vValue:=CStr(r_vSubscriber)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'SubscriberDB

        If Not Information.IsNothing(r_vSubscriberDB) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringSubscriberDB, r_vValue:=CStr(r_vSubscriberDB)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'Publisher

        If Not Information.IsNothing(r_vPublisher) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringPublisher, r_vValue:=CStr(r_vPublisher)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'Distributor

        If Not Information.IsNothing(r_vDistributor) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringDistributor, r_vValue:=CStr(r_vDistributor)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'DistributorSecurityMode

        If Not Information.IsNothing(r_vDistributorSecurityMode) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringDistributorSecurityMode, r_vValue:=CStr(r_vDistributorSecurityMode)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'PublisherDB

        If Not Information.IsNothing(r_vPublisherDB) Then


            m_lReturn = CType(GetMultiBranchRegSetting(v_sSettingName:=ACRegStringPublisherDB, r_vValue:=CStr(r_vPublisherDB)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetMultiBranchRegSetting
    '
    ' Description:
    '
    ' History: 10/07/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetMultiBranchRegSetting(ByVal v_sSettingName As String, ByRef r_vValue As String) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
        Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
        Dim eProductFamily As gPMConstants.PMEProductFamily
        Dim sTemp As String = ""

        eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
        eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLServer
        eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        sMessage = "Failed to get registry setting for SiriusSolutions\Server\" & ACRegKeyMultiBranchAccounting & "\" & v_sSettingName

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:=v_sSettingName, r_sSettingValue:=sTemp, v_sSubKey:=ACRegKeyMultiBranchAccounting), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sTemp = "" Then

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetMultiBranchRegSetting")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_vValue = sTemp.Trim()


        Return result

    End Function
End Module
