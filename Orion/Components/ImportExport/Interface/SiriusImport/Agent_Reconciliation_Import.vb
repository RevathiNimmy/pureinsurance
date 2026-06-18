Imports System.Collections.ObjectModel
Imports System.Xml
Imports System.IO
Imports System.Text
Imports SharedFiles
Imports System.Data
Imports Sirius.SBO.Import.Excel_Import_Library

Friend NotInheritable Class Agent_Reconciliation_Import : Inherits ImportBase
    'Developer guide no.
    Dim m_vKeys() As Object
    Dim m_iCount As Integer = 0
    Private m_nNoOfTotalRecords As Integer
    Private m_nNoOfRejections As Integer
    Private m_oAllocationManual As bACTAllocationManual.Business
    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oFindTransactions As bACTFindTransaction.Business
    Private m_oDocumentReversal As bACTDocumentReversal.Business
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    '  Private m_oTransDetail As bACTTransDetail.F

    Private m_lReturn As Integer
    Private m_lWriteOffDrAcc As Integer
    Private m_lWriteOffCrAcc As Integer
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer = 1
    Private m_iSourceID As Integer = 1
    Private m_iLanguageID As Integer = 1
    Private m_iCurrencyID As Integer = 26
    Private m_lCompanyID As Integer = 1
    Private m_iLogLevel As Integer = 1
    Private m_dWriteoffToleranceAmt As Decimal
    Private m_dWriteoffToleranceTransLevelAmt As Decimal
    Private m_lWriteoffToleranceCurr As Integer = 0
    Private m_iWriteOffReasonID As Integer = 6
    Private m_iBaseCurrencyId As Integer = 0
    Private m_sImportedPath As String = String.Empty

    Public Const ACWriteOffToleranceAmountOptionNo As String = "5242"
    Public Const ACWriteOffToleranceCurrencyOptionNo As String = "5243"


#Region "Creator"
    Public Sub New(ByVal oXML As XmlDocument)
        MyBase.New(oXML)
    End Sub

#End Region

#Region "Methods"

    Protected Overrides Sub CreateBatch()


        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue
        Try
            ' Add parameters
            AddParameterLite(m_oDatabase, "batch_code", BatchCode, PMEParameterDirection.PMParamInput, PMEDataType.PMString, True)
            AddParameterLite(m_oDatabase, "batch_ref", DBNull.Value, PMEParameterDirection.PMParamInputOutput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "interface_code", InterfaceName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "batch_id", System.DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)

            ' Execute command
            nReturn = m_oDatabase.SQLAction("spu_ACT_Import_CreateBatch", "Create Batch", True)
            If nReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Unable to execute 'spu_ACT_Import_CreateBatch'")
            End If

            ' Get batch id
            m_iBatchID = Util.ToSafeInt(m_oDatabase.Parameters.Item("batch_id").Value, 0)
            m_sBatchRef = m_oDatabase.Parameters.Item("batch_ref").Value


        Catch ex As Exception

            Throw New Exception("Unable to create new import batch", ex)

        Finally

            ' Check for successful creation of batch
            If m_iBatchID = 0 Then
                Throw New Exception(String.Format("Batch '{0}' has already been imported", m_oXML.BatchReference))
            End If
        End Try

    End Sub


    Private Sub InitObjects()

        m_oCurrencyConvert = New bACTCurrencyConvert.Form
        m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID,
                                                      iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                      iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        m_oCurrencyConvert.GetBaseCurrency(m_lCompanyID, m_iBaseCurrencyId)


        m_oAllocationManual = New bACTAllocationManual.Business
        m_oAllocationManual.Initialise(
                    sUsername:="",
                    sPassword:="",
                    iUserID:=m_iUserID,
                    iSourceID:=m_iSourceID,
                    iLanguageID:=m_iLanguageID,
                    iCurrencyID:=m_iCurrencyID,
                    iLogLevel:=PMELogLevel.PMLogError,
                    sCallingAppName:=ACApp,
                    vDatabase:=m_oDatabase)


        m_lReturn = m_oAllocationManual.GetWriteOffAccount("D", m_lWriteOffDrAcc)
        m_lReturn = m_oAllocationManual.GetWriteOffAccount("C", m_lWriteOffCrAcc)

        ' Writeoff tolerance amount and currency
        Dim sValue As String = ""
        m_lReturn = GetSystemOptionValue(CInt(ACWriteOffToleranceAmountOptionNo), sValue)

        m_dWriteoffToleranceTransLevelAmt = 0
        m_dWriteoffToleranceAmt = 0

        If sValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(sValue) Then
            m_dWriteoffToleranceAmt = gPMFunctions.ToSafeDecimal(sValue)
            ' m_dWriteoffToleranceTransLevelAmt = gPMFunctions.ToSafeDecimal(sValue)

        End If
        m_lReturn = GetSystemOptionValue(CInt(ACWriteOffToleranceCurrencyOptionNo), sValue)

        If sValue IsNot Nothing AndAlso Not String.IsNullOrEmpty(sValue) Then
            m_lWriteoffToleranceCurr = gPMFunctions.ToSafeInteger(sValue)
        Else
            m_lWriteoffToleranceCurr = m_iBaseCurrencyId
        End If

        'm_lWriteoffToleranceAmt convert to base
        'Check amount in base so convert Authority amount to base.

        If m_lWriteoffToleranceCurr <> m_iBaseCurrencyId Then
            m_lReturn = m_oCurrencyConvert.Convert(v_bConvertToBase:=True, v_lCurrencyID:=m_lWriteoffToleranceCurr, v_lCompanyId:=m_lCompanyID, r_cOriginalAmount:=m_dWriteoffToleranceAmt, r_cConvertedAmount:=m_dWriteoffToleranceAmt)
        End If


        m_oAutoNumber = New bACTAutoNumber.Business
        m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword,
                                                 iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                 iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                 iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp,
                                                 vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=Agent_Reconciliation_Import", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_oDocumentPost = New bACTDocumentPost.Form
        m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword,
                                                   iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                   iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                   iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp,
                                                   vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=Agent_Reconciliation_Import", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_oFindTransactions = New bACTFindTransaction.Business
        m_lReturn = m_oFindTransactions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword,
                                                   iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                   iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                   iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp,
                                                   vDatabase:=m_oDatabase)

        m_oDocumentReversal = New bACTDocumentReversal.Business
        m_lReturn = m_oDocumentReversal.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword,
                                                  iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                  iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                  iLogLevel:=PMELogLevel.PMLogError, sCallingAppName:=ACApp,
                                                  vDatabase:=m_oDatabase)





    End Sub

    Private Sub ReleaseOjects()


        If m_oAllocationManual IsNot Nothing Then
            m_oAllocationManual.Dispose()
            m_oAllocationManual = Nothing
        End If

        If m_oDocumentPost IsNot Nothing Then
            m_oDocumentPost.Dispose()
            m_oDocumentPost = Nothing
        End If

        If m_oAutoNumber IsNot Nothing Then
            m_oAutoNumber.Dispose()
            m_oAutoNumber = Nothing
        End If

        If m_oFindTransactions IsNot Nothing Then
            m_oFindTransactions.Dispose()
            m_oFindTransactions = Nothing
        End If

        If m_oDocumentReversal IsNot Nothing Then
            m_oDocumentReversal.Dispose()
            m_oDocumentReversal = Nothing
        End If

        If m_oCurrencyConvert IsNot Nothing Then
            m_oCurrencyConvert.Dispose()
            m_oCurrencyConvert = Nothing
        End If

    End Sub


    Private Function GetSystemOptionValue(ByRef v_iOptionNumber As Integer, ByRef r_sValue As String) As Integer

        Dim result As Integer = 0
        Dim oOptions As bSIROptions.Business



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of component services

        oOptions = New bSIROptions.Business
        m_lReturn = oOptions.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the value for this option

        m_lReturn = oOptions.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sValue)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then

            oOptions = Nothing

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get system option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOptionValue")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        'If system option not found, default to zero
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            r_sValue = "0"
        End If


        oOptions.Dispose()
        oOptions = Nothing

        Return result

    End Function



    Protected Overrides Sub ProcessElement()
    End Sub

    Public Overrides Sub ProcessImport()

        Try
            ' Create batch header
            OutputLine("Creating a batch...")

            CreateBatch()
            NoOfTotalRecords = 0
            NoOfRejections = 0
            InitObjects()

            Dim iAutoReconciliationRSID As Integer = 0

            OutputLine("Saving data file into database tables...")
            m_lReturn = XMLToDatabase(iAutoReconciliationRSID)

            If m_lReturn Then
                OutputLine("Validating data...")
                ValidateRecords(iAutoReconciliationRSID)


                OutputLine("Processing data file...")
                Processing(iAutoReconciliationRSID)


                UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)
                OutputLine("Auto Agent Reconciliation Process completed. Total request processed - " & NoOfTotalRecords)

            Else
                UpdateImportBatchStatus(kBatchStatusFailed, NoOfTotalRecords, NoOfRejections)
                CreateWorkManagerTask(String.Format("Auto Agent Reconciliation Process File data already exists for same Date/Time for {0} ", m_oXML.Filename))
            End If

            ReleaseOjects()

        Catch ex As Exception
            ReleaseOjects()
            UpdateImportBatchStatus(kBatchStatusComplete, NoOfTotalRecords, NoOfRejections)

            ' Raise exception
            Throw New Exception("Unable to process import file.", ex)
        Finally
            Dim sProcessedFilename As String = String.Empty
            Dim sFileExtension = Path.GetExtension(m_oXML.Filename)
            sProcessedFilename = Path.GetFileNameWithoutExtension(m_oXML.Filename)
            sProcessedFilename = ImportedPath & "\" & sProcessedFilename & "_" & m_sBatchRef & "_" & Now.ToFileTime & sFileExtension

            Try
                File.Move(m_oXML.Filename, sProcessedFilename)
            Catch ex As Exception
                CreateWorkManagerTask(String.Format("Import Completed but unable to move import file from {0} to {1}.", m_oXML.Filename, sProcessedFilename))
            End Try
        End Try

    End Sub

    Private Sub GetPendingPremiumReconciliationRecords(ByVal v_iAutoReconciliationRSID As Integer, ByRef r_vPremReconArray As Object(,))

        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Auto_ReconciliationRS_ID", v_iAutoReconciliationRSID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_sel_Premium_ReconciliationRS", sSQLName:="Agent_Reconciliation_Import", bStoredProcedure:=True, vResultArray:=r_vPremReconArray)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable To execute 'spu_sel_Premium_ReconciliationRS'")
        End If

    End Sub

    Private Sub GetReceiptMatchingAccountTotal(ByVal v_iAgentAccountID As Integer, ByVal v_dAgentGroupTotal As Decimal, ByRef r_ReceiptArray As Object(,))
        'get the SRP for the agent account total 
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("AgentAccountId", v_iAgentAccountID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("Amount", v_dAgentGroupTotal, PMEParameterDirection.PMParamInput, PMEDataType.PMDecimal)
        m_oDatabase.Parameters.Add("ToleranceAmount", m_dWriteoffToleranceAmt, PMEParameterDirection.PMParamInput, PMEDataType.PMDecimal)
        m_oDatabase.Parameters.Add("ToleranceCurrency", m_lWriteoffToleranceCurr, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_GetReceiptsForAutoRecon", sSQLName:="Agent_Reconciliation_Import", bStoredProcedure:=True, vResultArray:=r_ReceiptArray)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_GetReceiptsForAutoRecon'")
        End If
    End Sub

    Private Sub GetAccountEntries(ByVal v_Premium_ReconciliationRS_ID As Integer, ByRef r_AccountArray As Object(,))
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Premium_ReconciliationRS_ID", v_Premium_ReconciliationRS_ID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_sel_Auto_Reconciliation_Allocation_Data", sSQLName:="Agent_Reconciliation_Import", bStoredProcedure:=True, vResultArray:=r_AccountArray)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_sel_Auto_Reconciliation_Allocation_Data'")
        End If
    End Sub

    Private Sub ValidateRecords(ByVal v_iAutoReconciliationRSID As Integer)
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Auto_ReconciliationRS_ID", v_iAutoReconciliationRSID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.SQLSelect("spu_Auto_Reconciliation_Validation", "Validate_Premium_Statement_Load", True)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_Auto_Reconciliation_Validation'")
        End If

    End Sub

    Private Sub Processing(ByVal v_iAutoReconciliationRSID As Integer)


        Dim iAgentAccountId As Integer = 0
        Dim iCashReceiptTransDetailID As Integer = 0 'find out based on sum of account entry amount paid
        Dim iPremiumTransDetailsID As Integer = 0
        Dim dWriteOffAmount As Double = 0
        Dim iWriteOffAccountId As Integer = 0
        Dim sInsuranceRef As String = ""
        Dim dtAccountingDate As Date = Now()
        Dim iWriteOffTransDetailsID As Integer = 0
        Dim sCodeRange As String = ""
        Dim dCashReceiptAmountToAdjust As Double = 0
        Dim dARCNetPaidAmount As Double = 0
        Dim dCashReceiptAmount As Double = 0
        Dim iPremium_ReconciliationRS_ID As Integer = 0
        Dim sAgentAccountRef As String = Nothing
        Dim dAgentAccountTotal As Decimal = 0
        Dim dPureAccountNetAmount As Decimal = 0
        Dim dARCCommPaidAmount As Decimal = 0
        'loop around all RS IDs for current ID

        Dim vPremReconArray As Object(,) = Nothing
        Dim vAccountArray As Object(,) = Nothing
        Dim vReceiptArray As Object(,) = Nothing
        Dim ds As DataSet = Nothing
        Dim vAgentTransArray As Object(,) = Nothing
        Dim vMatchTrans As Object()
        Dim sACCReference_Number As String
        Dim iReceiptDocumentId As Integer = 0
        Dim iAllocationID As String = Nothing
        Dim sReceiptDocumentRef As String = ""
        Dim sTransDocumentRef As String = ""
        'get the Premium Reconciliation RS records

        GetPendingPremiumReconciliationRecords(v_iAutoReconciliationRSID, vPremReconArray)

        If Not Information.IsArray(vPremReconArray) Then
            'No pending records to process so exit 
            Exit Sub
        Else
            'Loop through each record and process it. 
            For lRow As Integer = vPremReconArray.GetLowerBound(1) To vPremReconArray.GetUpperBound(1)
                iPremium_ReconciliationRS_ID = gPMFunctions.ToSafeInteger(vPremReconArray(0, lRow))
                sAgentAccountRef = gPMFunctions.ToSafeString(vPremReconArray(1, lRow))
                dAgentAccountTotal = gPMFunctions.ToSafeDecimal(vPremReconArray(2, lRow))

                m_lReturn = m_oFindTransactions.GetAccountID(sAgentAccountRef, iAgentAccountId)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("Invalid Account Shortcode'")
                End If

                'Get the receipt = agent account total 
                GetReceiptMatchingAccountTotal(iAgentAccountId, dAgentAccountTotal, vReceiptArray)

                Dim bContinue As Boolean = True


                If Not Information.IsArray(vReceiptArray) Then
                    UpdateAccountEntryFields(iPremium_ReconciliationRS_ID, "U", "", 0, "Unmatched Agent " & sAgentAccountRef & " receipt in Pure")
                    bContinue = False
                End If

                If bContinue Then
                    iCashReceiptTransDetailID = gPMFunctions.ToSafeDecimal(vReceiptArray(0, 0))
                    dCashReceiptAmount = gPMFunctions.ToSafeDecimal(vReceiptArray(1, 0))
                    iReceiptDocumentId = gPMFunctions.ToSafeDecimal(vReceiptArray(2, 0))
                    sReceiptDocumentRef = gPMFunctions.ToSafeString(vReceiptArray(3, 0))

                    Dim dReceiptWriteoffAmt As Double = Math.Abs(dAgentAccountTotal) - Math.Abs(dCashReceiptAmount)



                    If dReceiptWriteoffAmt <> 0 Then
                        If dReceiptWriteoffAmt < 0 Then
                            iWriteOffAccountId = m_lWriteOffCrAcc

                        Else
                            iWriteOffAccountId = m_lWriteOffDrAcc

                        End If


                        Dim dAgentWriteOffAmt As Double = dReceiptWriteoffAmt * -1


                        PostWriteOff(iAgentAccountId, iWriteOffAccountId, m_lWriteoffToleranceCurr, dReceiptWriteoffAmt, dAgentWriteOffAmt, 0, sReceiptDocumentRef,
                                                                dtAccountingDate, "", iWriteOffTransDetailsID, "SWD", 1)


                        Dim vMatchTransReceipt(0) As Object



                        vMatchTransReceipt(0) = iWriteOffTransDetailsID & "|" & dAgentWriteOffAmt



                        AllocateTransaction(iAgentAccountId, iCashReceiptTransDetailID, dReceiptWriteoffAmt, vMatchTransReceipt, iAllocationID)

                        dCashReceiptAmount = dCashReceiptAmount - dReceiptWriteoffAmt
                    End If

                    'get individual account entries 
                    GetAccountEntries(iPremium_ReconciliationRS_ID, vAccountArray)

                    If Not Information.IsArray(vAccountArray) Then
                        Exit For
                        'no account entries to process
                        ' gPMFunctions.RaiseError("Processing()", "Unable to retrieve Account Entry records from given ReconciliationRS_Id = " + iPremium_ReconciliationRS_ID, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    For lAccRow As Integer = vAccountArray.GetLowerBound(1) To vAccountArray.GetUpperBound(1)

                        dARCNetPaidAmount = gPMFunctions.ToSafeDecimal(vAccountArray(8, lAccRow))
                        sACCReference_Number = gPMFunctions.ToSafeString(vAccountArray(0, lAccRow))
                        iPremiumTransDetailsID = gPMFunctions.ToSafeInteger(Split(gPMFunctions.ToSafeString(vAccountArray(0, lAccRow)), "/")(0))

                        GetPureTransDetailAmount(iPremiumTransDetailsID, iAgentAccountId, vAgentTransArray)

                        dPureAccountNetAmount = 0
                        sInsuranceRef = ""
                        sTransDocumentRef = ""

                        If vAgentTransArray IsNot Nothing Then
                            dPureAccountNetAmount = gPMFunctions.ToSafeDecimal(vAgentTransArray(2, 0))
                            sInsuranceRef = gPMFunctions.ToSafeString(vAgentTransArray(3, 0))
                            sTransDocumentRef = gPMFunctions.ToSafeString(vAgentTransArray(4, 0))
                        End If

                        dWriteOffAmount = dARCNetPaidAmount - dPureAccountNetAmount

                        dWriteOffAmount = 0

                        If dARCNetPaidAmount = 0 Then
                            UpdateAccountEntryFields(iPremium_ReconciliationRS_ID, "U", "", 0, "Net Amount Paid is zero.")
                        Else

                            Dim bAllocated As Boolean = False

                            If dPureAccountNetAmount <> 0 Then
                                If dPureAccountNetAmount = (dARCNetPaidAmount + dWriteOffAmount) Then

                                    If Math.Abs(dWriteOffAmount) <= Math.Abs(m_dWriteoffToleranceTransLevelAmt) Then
                                        Dim dAgentWriteOffAmt As Double = 0

                                        If dWriteOffAmount <> 0 Then
                                            If dWriteOffAmount <= 0 Then
                                                iWriteOffAccountId = m_lWriteOffCrAcc
                                                dWriteOffAmount = Math.Abs(dWriteOffAmount) * -1
                                            Else
                                                iWriteOffAccountId = m_lWriteOffDrAcc
                                                dWriteOffAmount = Math.Abs(dWriteOffAmount)
                                            End If

                                            dAgentWriteOffAmt = dWriteOffAmount * -1

                                            PostWriteOff(iAgentAccountId, iWriteOffAccountId, m_lWriteoffToleranceCurr, dWriteOffAmount, dAgentWriteOffAmt, 0, sInsuranceRef,
                                                                     dtAccountingDate, "", iWriteOffTransDetailsID, "SWD", 1)
                                        End If

                                        ' writeoff entry
                                        If dWriteOffAmount <> 0 Then
                                            ReDim vMatchTrans(vAgentTransArray.GetUpperBound(1) + 1)
                                        Else
                                            ReDim vMatchTrans(vAgentTransArray.GetUpperBound(1))
                                        End If

                                        For lTransRow As Integer = vAgentTransArray.GetLowerBound(1) To vAgentTransArray.GetUpperBound(1)

                                            vMatchTrans(lTransRow) = CStr(vAgentTransArray(0, lTransRow)) & "|" & CStr(vAgentTransArray(1, lTransRow))
                                        Next lTransRow

                                        If dWriteOffAmount <> 0 Then
                                            vMatchTrans(vAgentTransArray.GetUpperBound(1) + 1) = iWriteOffTransDetailsID & "|" & dAgentWriteOffAmt
                                        End If

                                        AllocateTransaction(iAgentAccountId, iCashReceiptTransDetailID, dARCNetPaidAmount * -1, vMatchTrans, iAllocationID)

                                        UpdateAccountEntryFields(iPremium_ReconciliationRS_ID, "M", sACCReference_Number, gPMFunctions.ToSafeInteger(iAllocationID), "")


                                        bAllocated = True
                                    End If
                                End If

                            End If
                            'reverse of xml amount
                            If Not bAllocated Then
                                UpdateAccountEntryFields(iPremium_ReconciliationRS_ID, "E", sACCReference_Number, 0, sTransDocumentRef)

                            End If
                        End If
                    Next lAccRow

                    ' Create reversal for exception records
                    ' E entry 
                    Dim vReversalArray As Object(,) = Nothing
                    Dim sDocumentRef As String = ""
                    Dim dReversalTotalAmount As Double = 0
                    Dim sRevTotalDocumentRef As String = ""
                    GetExceptionTransactionForReversal(iPremium_ReconciliationRS_ID, vReversalArray)
                    If vReversalArray IsNot Nothing Then
                        dReversalTotalAmount = gPMFunctions.ToSafeDouble(vReversalArray(2, 0))
                        If dReversalTotalAmount <> 0 Then
                            Dim dReversalAmount As Double = 0
                            Dim sACCReference As String = ""
                            Dim iReversalDocumentID As Integer = 0
                            Dim sTransDetailDocumentRef As String = ""
                            ReverseReceipt(iReceiptDocumentId, sReceiptDocumentRef, dReversalTotalAmount, dReversalTotalAmount, sRevTotalDocumentRef, iReversalDocumentID)

                            For lReversalRow As Integer = vReversalArray.GetLowerBound(1) To vReversalArray.GetUpperBound(1)
                                sACCReference = gPMFunctions.ToSafeString(vReversalArray(0, lReversalRow))
                                dReversalAmount = gPMFunctions.ToSafeDouble(vReversalArray(1, lReversalRow))
                                sTransDetailDocumentRef = gPMFunctions.ToSafeString(vReversalArray(3, lReversalRow))
                                If String.IsNullOrEmpty(sTransDetailDocumentRef) Then
                                    sTransDetailDocumentRef = sReceiptDocumentRef
                                End If
                                ReverseReceiptTransaction(iReversalDocumentID, sTransDetailDocumentRef, dReversalAmount, dReversalAmount, sDocumentRef)
                                UpdateAccountEntryFields(iPremium_ReconciliationRS_ID, "E", sACCReference, 0, sDocumentRef & " replacement of " & sReceiptDocumentRef & ". Auto Reconciliation = " & v_iAutoReconciliationRSID)
                            Next lReversalRow
                        End If
                    End If

                End If

        Next lRow
        End If

    End Sub

    Private Sub GetExceptionTransactionForReversal(ByVal v_PremiumReconciliationRS_ID As Integer, ByRef vResultArray As Object)

        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Premium_ReconciliationRS_ID", v_PremiumReconciliationRS_ID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_exception_records_for_reversal", sSQLName:="Agent_Reconciliation_Import", bStoredProcedure:=True, vResultArray:=vResultArray)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_get_exception_records_for_reversal'")
        End If
    End Sub

    Private Function UpdateAccountEntryFields(ByVal v_PremiumReconciliationRS_ID As Integer, ByVal v_sTransactionStatus As String,
                                                ByVal v_sACCReferenceNumber As String, ByVal v_iAllocationID As Integer, ByVal v_sComment As String) As Integer


        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Premium_ReconciliationRS_ID", v_PremiumReconciliationRS_ID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("ACC_Reference_Number", v_sACCReferenceNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("TransactionStatus", v_sTransactionStatus, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Allocation_ID", v_iAllocationID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("Comments", v_sComment, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        ' Execute command
        nReturn = m_oDatabase.SQLSelect("spu_Update_Account_Entry", "Insert_Premium_Statement_Load", True)

        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute 'spu_Update_Account_Entry'")
        End If
        Return nReturn


    End Function


    Private Function ReverseReceipt(ByVal v_lReceiptDocumentID As Integer, ByVal v_sReceiptDocumentRef As String, ByVal v_dReverseAmount As Double, ByVal v_dReverseCurrencyAmount As Double, ByRef r_sDocumentRef As String, ByRef r_iDocumentID As Integer) As Integer

        m_lReturn = m_oDocumentReversal.DoPartialReceiptReversal(v_lReceiptDocumentID, v_sReceiptDocumentRef, v_dReverseAmount, v_dReverseCurrencyAmount, r_sDocumentRef)
        r_iDocumentID = m_oDocumentReversal.DocumentId
        Return m_lReturn

    End Function

    Private Function ReverseReceiptTransaction(ByVal v_lReceiptDocumentID As Integer, ByVal v_sReceiptDocumentRef As String, ByVal v_dReverseAmount As Double, ByVal v_dReverseCurrencyAmount As Double, ByRef r_sDocumentRef As String) As Integer

        m_lReturn = m_oDocumentReversal.DoReceiptTransdetail(v_lReceiptDocumentID, v_sReceiptDocumentRef, v_dReverseAmount, v_dReverseCurrencyAmount, r_sDocumentRef)

        Return m_lReturn

    End Function




    Private Sub GetPureTransDetailAmount(ByVal v_itransdetailID As Integer, ByVal v_iAgentAccountID As Integer, ByRef vTransArray As Object)


        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("TransdetailID", v_itransdetailID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("AgentAccountId", v_iAgentAccountID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_Agent_Net_Amount", sSQLName:="Agent_Reconciliation_Import", bStoredProcedure:=True, vResultArray:=vTransArray)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_get_Agent_Net_Amount'")
        End If

    End Sub
    Public Function GetAccountKeyFromCode(ByRef r_lAccountKey As Integer, ByVal v_lAccountRef As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT account_key FROM Account WHERE short_code = '" & v_lAccountRef & "'"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountKey", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vResultArray) Then

                r_lAccountKey = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountCodeFromI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountKeyFromId", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function AllocateTransaction(ByVal v_lAgentAccountID As Integer, ByVal v_iCashReceiptTransDetailsId As Integer,
                                             ByVal v_dCashReceiptAmount As Double, ByVal vTransMatch As Object(), ByRef r_iAllocationID As String) As Integer

        Dim vKeys As Object
        Dim vGetKeysArray As Object(,) = Nothing

        ReDim vKeys(1, 2)
        ' AccountID

        vKeys(0, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(1, 0) = v_lAgentAccountID 'm_lAccountID

        ' AllocatedTransID

        vKeys(0, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeys(1, 1) = CStr(v_iCashReceiptTransDetailsId) & "|" & CStr(v_dCashReceiptAmount)


        'Dim vMatchTrans(0) As Object

        'vMatchTrans(0) = CStr(v_lAccountEntryTransDetailID) & "|" & CStr(v_dAccountEntryAmount)

        'If v_dWriteOffAmt <> 0 Then
        '    vMatchTrans(0) = CStr(v_lAccountEntryTransDetailID) & "|" & CStr(v_dAccountEntryAmount) & "|" & v_iWriteOffTransDetailID & "|" & v_lWriteOffReasonID & "|" & v_dWriteOffAmt
        'End If

        vKeys(0, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
        vKeys(1, 2) = vTransMatch


        m_lReturn = m_oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        ' Set the keys


        m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=vKeys)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAllocationManual.SetKeys", "Function failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Pass accross the company for the document

        m_oAllocationManual.CompanyId = 1

        m_oAllocationManual.AllocatingReversal = False

        'Start it
        'Add the line,to assign the database
        m_oAllocationManual.m_oDatabase = m_oDatabase

        m_lReturn = m_oAllocationManual.Start()

        m_oAllocationManual.GetKeys(vGetKeysArray)

        If vGetKeysArray IsNot Nothing Then
            r_iAllocationID = vGetKeysArray(1, 0)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oAllocationManual.Start", "Function failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return m_lReturn
    End Function

    Private Function PostWriteOff(ByVal v_iAgentAccountID As Integer,
                                                 ByVal v_iWriteOffAccID As Integer,
                                                 ByVal nBaseCurrencyID As Integer,
                                                 ByVal dWriteOffAmount As Double,
                                                 ByVal dWriteoffAgentAmt As Double,
                                                 ByVal nInsuranceFileCnt As Integer,
                                                 ByVal sInsuranceRef As String,
                                                 ByVal dtAccountingDate As Date,
                                                 ByVal sComment As String,
                                                 ByRef r_iWriteOffTransDetailId As Integer,
                                                 ByVal sRangeCode As String, ByVal dXRate As Double) As Integer

        Dim nNumberRangeID As Integer
        Dim nNumber As Integer
        Dim sDocumentRef As String = String.Empty

        Const kMethodName As String = "PostInstalmentWriteOff"

        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        If dWriteOffAmount = 0 Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        'gACTLibrary.ACTAutoNumberRangeCodeSwd
        Try

            nReturn = m_oAutoNumber.GetNumberRange(
                        v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef12,
                        v_sRangeCode:=sRangeCode,
                        r_lNumberRangeID:=nNumberRangeID)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Generate the next number
            nReturn = m_oAutoNumber.GenerateNumber(
                    v_lNumberRangeID:=nNumberRangeID,
                    v_iUserID:=m_iUserID,
                    v_iCompanyID:=m_lCompanyID,
                    r_lNumber:=nNumber)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GenerateNumber", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            sDocumentRef = FormatDocumentRef(sRangeCode, nNumber)

            nReturn = m_oDocumentPost.AddDocument(
                    v_lDocumentTypeId:=gACTLibrary.ACTDocTypeWriteOff,
                    v_sDocumentRef:=sDocumentRef,
                    v_dtDocumentDate:=dtAccountingDate,
                    v_sComment:=sComment,
                    r_vDocSourceID:=1,
                    r_vSubBranchID:=1,
                    v_lInsuranceFileCnt:=nInsuranceFileCnt)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTInstalments - PostInstalmentWriteOff - bACTTransdetail.PostInstalmentWriteOff Failed")
            End If

            nReturn = m_oDocumentPost.AddTransaction(
                    v_lAccountID:=v_iAgentAccountID,
                    v_iCurrencyID:=nBaseCurrencyID,
                    v_cAmount:=dWriteoffAgentAmt,
                    v_cCurrencyAmount:=dWriteoffAgentAmt * dXRate,
                    v_vdCurrencyBaseXRate:=dXRate,
                    r_vTransDetailId:=r_iWriteOffTransDetailId,
                    v_vDocumentSequence:=1,
                    v_vInsuranceRef:=sInsuranceRef,
                    v_vAccountingDate:=dtAccountingDate,
                    v_vComment:=sComment)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId:=" & v_iAgentAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturn = m_oDocumentPost.AddTransaction(
                    v_lAccountID:=v_iWriteOffAccID,
                    v_iCurrencyID:=nBaseCurrencyID,
                    v_cAmount:=dWriteOffAmount,
                    v_cCurrencyAmount:=dWriteOffAmount * dXRate,
                    v_vdCurrencyBaseXRate:=dXRate,
                    v_vDocumentSequence:=2,
                    v_vInsuranceRef:=sInsuranceRef,
                    v_vAccountingDate:=dtAccountingDate,
                    v_vComment:=sComment)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddTransaction", "v_lAccountId=" & v_iWriteOffAccID, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As System.Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostInstalmentWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError

        End Try
        Return nReturn
    End Function

    Private Sub CheckIfAutoReconXMLExists(ByRef r_bIfXMLDataExists As Boolean, ByVal dtDate As DateTime)

        Dim vArray As Object = Nothing
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("DateGenerated", dtDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)


        m_lReturn = m_oDatabase.SQLSelect("spu_Check_AutoReconciliation_Data_Exists", "Check_XML_Aldready_Exists", True, vResultArray:=vArray)

        If m_lReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_Check_AutoReconciliation_Data_Exists'")
        End If

        If Information.IsArray(vArray) Then
            r_bIfXMLDataExists = IIf(vArray(0, 0) > 0, True, False)
        End If

    End Sub
    Protected Function XMLToDatabase(ByRef r_iAutoReconciliationRS_ID As Integer) As Integer

        Dim iReturn As Integer = 0
        Dim bIfXMLDataExists As Boolean = False

        Try

            Dim oDocument As New Xml.XmlDocument
            oDocument.Load(m_oXML.Filename)

            Dim sInsurerID As String = GetXMLNodeValue(oDocument.DocumentElement.SelectSingleNode("Insurer/PEMId"))
            Dim sInsurerName As String = GetXMLNodeValue(oDocument.DocumentElement.SelectSingleNode("Insurer/CompanyName"))
            Dim dtDateGenerated As DateTime = GetXMLNodeValue(oDocument.DocumentElement.SelectSingleNode("DateGenerated"))
            Dim dtTimeGenerated As DateTime = GetXMLNodeValue(oDocument.DocumentElement.SelectSingleNode("TimeGenerated"))
            Dim iAutoReconciliationRS_ID As Integer = 0

            Dim dtDate As New DateTime(dtDateGenerated.Year, dtDateGenerated.Month, dtDateGenerated.Day, dtTimeGenerated.Hour, dtTimeGenerated.Minute, dtTimeGenerated.Second)

            ' Check with this date already file exists in system, if so then it might be duplicate so exit the process. 
            CheckIfAutoReconXMLExists(bIfXMLDataExists, dtDate)

            If Not bIfXMLDataExists Then
                BeginTrans()
                iReturn = InsertPremiumStatementLoad(sInsurerID, sInsurerName, dtDate, iAutoReconciliationRS_ID)

                r_iAutoReconciliationRS_ID = iAutoReconciliationRS_ID

                Dim oNodeListPremiumReconciliationRs As Xml.XmlNodeList = oDocument.SelectNodes("//PremiumReconciliationRs")

                If IsValidXMLNodeList(oNodeListPremiumReconciliationRs) Then

                    'Start pushing data into table - Import_Receipt_Reconciliation
                    For iCnt As Integer = 0 To oNodeListPremiumReconciliationRs.Count - 1
                        NoOfTotalRecords += 1
                        Dim oNodePremiumReconciliationRs As Xml.XmlNode = oNodeListPremiumReconciliationRs.Item(iCnt)

                        If IsValidXMLNode(oNodePremiumReconciliationRs) Then

                            Dim sNodePremiumReconciliationRsID As String = GetXMLNodeValue(oNodePremiumReconciliationRs.Attributes.GetNamedItem("Id"))
                            Dim sInttAgentPEMID As String = GetXMLNodeValue(oNodePremiumReconciliationRs.SelectSingleNode("Intermediary/PEMId"))
                            Dim sInttAgentName As String = GetXMLNodeValue(oNodePremiumReconciliationRs.SelectSingleNode("Intermediary/CompanyName"))
                            Dim sStatementAgentRef As String = GetXMLNodeValue(oNodePremiumReconciliationRs.SelectSingleNode("Statement/AgencyAccountRef"))
                            Dim sPaymentAgentRef As String = GetXMLNodeValue(oNodePremiumReconciliationRs.SelectSingleNode("Statement/PaymentDetail/ReferenceNumber"))
                            Dim iPremiumReconcilationRSId As Integer

                            iReturn = InsertPremiumReconciliationRS(iAutoReconciliationRS_ID, sNodePremiumReconciliationRsID, sInttAgentPEMID, sInttAgentName, sStatementAgentRef, sPaymentAgentRef, iPremiumReconcilationRSId)

                            Dim oNodeListAccountEntry As Xml.XmlNodeList = oNodePremiumReconciliationRs.SelectNodes("AccountEntry")

                            If IsValidXMLNodeList(oNodeListAccountEntry) Then
                                For iPaymentCnt As Integer = 0 To oNodeListAccountEntry.Count - 1
                                    Dim oNodeAccountEntry As Xml.XmlNode = oNodeListAccountEntry.Item(iPaymentCnt)

                                    If IsValidXMLNode(oNodeAccountEntry) Then
                                        Dim sReferenceNumber As String = GetXMLNodeValue(oNodeAccountEntry.SelectSingleNode("ReferenceNumber"))
                                        Dim sPolicyNumber As String = GetXMLNodeValue(oNodeAccountEntry.SelectSingleNode("Policy/PolicyNumber"))
                                        Dim sEffectiveDate As DateTime = CDate(GetXMLNodeValue(oNodeAccountEntry.SelectSingleNode("StartDate")))
                                        Dim sClientName As String = GetXMLNodeValue(oNodeAccountEntry.SelectSingleNode("Insured/CompanyName"))
                                        Dim sRevenueType As String = GetXMLNodeValue(oNodeAccountEntry.SelectSingleNode("TypeCode/Value"))
                                        Dim dtPostedDate As DateTime = CDate(GetXMLNodeValue(oNodeAccountEntry.SelectSingleNode("//PostedDate")))
                                        Dim oNodeAmountPaid As Xml.XmlNode = oNodeAccountEntry.SelectSingleNode("AmountPaid")
                                        Dim oNodeAmountPayable As Xml.XmlNode = oNodeAccountEntry.SelectSingleNode("AmountPayable")
                                        Dim dGrossAmtPayable As Double = ToSafeDouble(GetXMLNodeValue(oNodeAmountPayable.SelectSingleNode("GrossAmount")))
                                        Dim dCommissionPayable As Double = ToSafeDouble(GetXMLNodeValue(oNodeAmountPayable.SelectSingleNode("BrokerageAmount")))
                                        Dim dNetAmtPayable As Double = ToSafeDouble(GetXMLNodeValue(oNodeAmountPayable.SelectSingleNode("Amount")))
                                        Dim dGrossAmtPaid As Double = ToSafeDouble(GetXMLNodeValue(oNodeAmountPaid.SelectSingleNode("GrossAmount")))
                                        Dim dCommissionPaid As Double = ToSafeDouble(GetXMLNodeValue(oNodeAmountPaid.SelectSingleNode("BrokerageAmount")))
                                        Dim dNetAmtPaid As Double = ToSafeDouble(GetXMLNodeValue(oNodeAmountPaid.SelectSingleNode("Amount")))
                                        Dim iIsPremiumFinanceTransaction As Integer = Text2Bit(GetXMLNodeValue(oNodeAmountPayable.SelectSingleNode("PaidDirectInd/Value")))
                                        Dim sTransactionStatus As String = "P" 'Pending Status

                                        iReturn = InsertAccountEntryData(iPremiumReconcilationRSId, sReferenceNumber, sEffectiveDate, sClientName, sPolicyNumber,
                                                                                                 dGrossAmtPayable, dCommissionPayable, dNetAmtPayable, dGrossAmtPaid, dCommissionPaid,
                                                                                                 dNetAmtPaid, sRevenueType, dtPostedDate, iIsPremiumFinanceTransaction, sTransactionStatus)


                                    End If

                                Next
                            End If

                        End If

                    Next
                    CommitTrans()
                End If
            Else

                Return False
            End If
        Catch ex As Exception
            RollbackTrans()
            Throw New ApplicationException("Unable to Import Agent Auto Reconciliation XML into Pure database.", ex)

        End Try

        Return iReturn
    End Function



    Private Function Text2Bit(ByVal v_sValue As String) As Integer

        v_sValue = ToSafeString(v_sValue)

        v_sValue = v_sValue.Trim().ToUpper

        Dim sRetVal As Integer = 0

        If v_sValue.StartsWith("Y") Or v_sValue.StartsWith("T") Then
            sRetVal = 1
        End If

        Return sRetVal

    End Function

    Private Function InsertPremiumStatementLoad(ByVal v_sInsurerID As String, ByVal v_sInsurerName As String,
                                                    ByVal v_dtDateGenerated As DateTime, ByRef v_rAutoReconciliationRS_ID As Integer) As Integer


        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Auto_ReconciliationRS_ID", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("InsurerID", v_sInsurerID, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("InsurerName", v_sInsurerName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("DateGenerated", v_dtDateGenerated, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)

        ' Execute command
        nReturn = m_oDatabase.SQLSelect("spu_add_Auto_ReconciliationRS", "Insert_Premium_Statement_Load", True)

        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_add_Auto_ReconciliationRS'")
        Else
            v_rAutoReconciliationRS_ID = Util.ToSafeInt(m_oDatabase.Parameters.Item("Auto_ReconciliationRS_ID").Value, 0)
        End If
        Return nReturn
    End Function

    Private Function InsertPremiumReconciliationRS(ByVal v_iAutoReconciliationRS_ID As Integer, ByVal v_sReconciliationID As String, ByVal v_sInttAgentPEMID As String,
                                                    ByVal v_sInttAgentName As String, ByVal v_sStatementAgentRef As String, ByVal sPaymentAgentRef As String,
                                                       ByRef r_iPremiumReconciliationID As Integer) As Integer


        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Auto_ReconciliationRS_ID", v_iAutoReconciliationRS_ID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("Reconciliation_ID", v_sReconciliationID, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Agent_Group_Code", v_sInttAgentPEMID, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Agent_Group_Name", v_sInttAgentName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Agent_Account_Ref", v_sStatementAgentRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Payment_Reference_Number", sPaymentAgentRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Premium_ReconciliationRS_ID", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)

        ' Execute command
        nReturn = m_oDatabase.SQLSelect("spu_add_Premium_ReconciliationRS", "Insert_Premium_ReconciliationRS", True)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_add_Premium_ReconciliationRS'")
        Else
            r_iPremiumReconciliationID = Util.ToSafeInt(m_oDatabase.Parameters.Item("Premium_ReconciliationRS_ID").Value, 0)
        End If
        Return nReturn
    End Function



    Private Function InsertAccountEntryData(ByVal PremiumReconciliationRsID As Integer, ByVal v_sReferenceNumber As String,
                                                    ByVal v_dtEffectiveDate As DateTime, ByVal v_sClientName As String,
                                                    ByVal v_dPolicyNumber As String, ByVal v_dGrossAmountDue As Double,
                                                    ByVal v_dCommissionDue As Double, ByVal v_dNetAmountDue As Double,
                                                    ByVal v_dGrossAmountPaid As Double, ByVal v_dCommissionPaid As Double,
                                                    ByVal v_dNetAmountPaid As Double, ByVal v_sRevenueType As String,
                                                    ByVal v_dtPostedDate As DateTime, ByVal v_iPremiumFinanceTransaction As Integer,
                                                    ByVal v_sTransactionStatus As String) As Integer


        Dim nReturn As PMEReturnCode = PMEReturnCode.PMTrue

        ' Add parameters
        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add("Premium_ReconciliationRS_ID", PremiumReconciliationRsID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("batch_id", m_iBatchID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("Reference_Number", v_sReferenceNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Effective_Date", v_dtEffectiveDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        m_oDatabase.Parameters.Add("Client_Name", v_sClientName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Policy_Number", v_dPolicyNumber, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Gross_Amount_Due", v_dGrossAmountDue, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
        m_oDatabase.Parameters.Add("Commission_Due", v_dCommissionDue, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
        m_oDatabase.Parameters.Add("Net_Amount_Due", v_dNetAmountDue, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
        m_oDatabase.Parameters.Add("Gross_Amount_Paid", v_dGrossAmountPaid, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
        m_oDatabase.Parameters.Add("Commission_Paid", v_dCommissionPaid, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
        m_oDatabase.Parameters.Add("Net_Amount_Paid", v_dNetAmountPaid, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
        m_oDatabase.Parameters.Add("Revenue_Type", v_sRevenueType, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Posted_Date", v_dtPostedDate, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
        m_oDatabase.Parameters.Add("Premium_Finance_Transaction", v_iPremiumFinanceTransaction, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("Transaction_Status", v_sTransactionStatus, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
        m_oDatabase.Parameters.Add("Allocation_ID", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
        m_oDatabase.Parameters.Add("Comments", "", PMEParameterDirection.PMParamInput, PMEDataType.PMString)

        ' Execute command
        nReturn = m_oDatabase.SQLSelect("spu_add_Account_Entry_RS", "Agent_Reconciliation_Import", True)
        If nReturn <> PMEReturnCode.PMTrue Then
            Throw New ApplicationException("Unable to execute ‘spu_add_Account_Entry_RS'")
        End If

        Return nReturn
    End Function

    Private Function MakeQueryStr(v_sValue As String) As String

        Return "'" & Replace(v_sValue, "'", "''") & "'"

    End Function

    Private Function GetXMLNodeValue(ByRef r_oNode As Xml.XmlNode) As String
        Dim sRet As String = ""

        If r_oNode IsNot Nothing Then
            sRet = r_oNode.InnerText.Trim
        End If

        Return sRet
    End Function
    Private Function IsValidXMLNodeList(ByRef r_oNodeList As Xml.XmlNodeList) As Boolean
        Dim bRetVal As Boolean = False

        If r_oNodeList IsNot Nothing Then
            If r_oNodeList.Count > 0 Then
                bRetVal = True
            End If
        End If

        Return bRetVal
    End Function
    Private Function IsValidXMLNode(ByRef r_oNode As Xml.XmlNode) As Boolean
        Dim bRetVal As Boolean = False
        If r_oNode IsNot Nothing Then
            bRetVal = True
        End If

        Return bRetVal
    End Function

    Private Function ReadFromTextFile(ByVal v_sFilePath As String) As String
        Dim sRetVal As String = ""

        Try
            If File.Exists(v_sFilePath) Then
                sRetVal = File.ReadAllText(v_sFilePath, Encoding.GetEncoding("utf-8"))
            End If

            Return sRetVal

        Catch ex As Exception
            Return ""
        End Try

    End Function





#End Region

#Region "Public Properties"
    ''' <summary>
    ''' Specifies the batch code for this interface
    ''' </summary>
    ''given the Batch Code coz it is must overrides property of Import base class
    Public Overrides ReadOnly Property BatchCode() As String
        Get
            Return "ARI"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the active interface name
    ''' </summary>
    Public Overrides ReadOnly Property InterfaceName() As String
        Get
            Return "Agent_Reconciliation_Import"
        End Get
    End Property

    ''' <summary>
    ''' Specifies the number of records in batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfTotalRecords() As Integer
        Get
            Return m_nNoOfTotalRecords
        End Get
        Set(ByVal value As Integer)
            m_nNoOfTotalRecords = value
        End Set
    End Property

    ''' <summary>
    ''' Specifies the no of rejected records in the batch for this Class
    ''' </summary>
    ''' <returns></returns>
    Public Overrides Property NoOfRejections() As Integer
        Get
            Return m_nNoOfRejections
        End Get
        Set(ByVal value As Integer)
            m_nNoOfRejections = value
        End Set
    End Property

    Public ReadOnly Property ImportedPath() As String
        Get
            ' If we haven't got the path yet, get it
            If (m_sImportedPath.Length = 0) Then
                m_sImportedPath = GetSystemOption(ACImportedPathOption)
            End If

            ' Check it exists
            If Not Directory.Exists(m_sImportedPath) Then
                ' If we can create it do so, else raise error
                Directory.CreateDirectory(m_sImportedPath)
            End If

            ' If we made it this far return the path
            Return m_sImportedPath
        End Get
    End Property
#End Region
End Class
