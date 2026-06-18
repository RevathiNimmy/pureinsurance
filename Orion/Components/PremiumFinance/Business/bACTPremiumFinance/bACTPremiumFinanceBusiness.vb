Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Automated
    '
    ' Date: 07/04/1998
    '
    ' Description: Creatable Automated class which contains all the
    '              Automated methods which can be called by Navigator.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 03/04/2007
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    '   Sirius Database
    'eck130700
    'Private m_oSIRDatabase As dPMDAO.Database
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    Private m_oAllocateManual As bACTAllocationManual.Business
    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oExplorer As Object
    Private m_oOrionLink As Object
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_oTransDetail As bACTTransdetail.Form

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Current Record Pointer
    Private m_lCurrentRecord As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date


    ' Calling Application name.

    ' Component Services object

    'Abort Transaction
    Private m_bAbortTrans As Boolean
    Private m_bTransStarted As Boolean

    Private m_sUnderwritingORAgency As String = ""
    Private Const k_AGENT_TYPE_BROKER As Integer = 1


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            m_sCallingAppName = Value

        End Set
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



    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property



    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing, Optional ByRef vSIRDatabase As Object = Nothing) As Integer




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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level





            m_lReturn = gPMComponentServices.CheckDatabase(v_sUsername:=sUsername, v_iSourceID:=iSourceID, v_iLanguageID:=iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_bAbortTrans = True
            m_bTransStarted = False

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now


            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oAllocateManual IsNot Nothing Then
                    m_oAllocateManual.Dispose()
                    m_oAllocateManual = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oExplorer IsNot Nothing Then
                    m_oExplorer.Dispose()
                    m_oExplorer = Nothing
                End If
                If m_oOrionLink IsNot Nothing Then
                    m_oOrionLink.Dispose()
                    m_oOrionLink = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
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


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Post Transactions
    '
    ' Description: Creates transactions to transfer Premium Finance Debt
    '
    ' eck271100 Pass noofInstallments & optional Interest amount
    '
    ' DD 24/06/2003: Tidied up for 1.9 Instalments
    ' ***************************************************************** '
    Public Function PostTransactions(ByVal v_lClientAccountID As Integer, ByVal v_lPremFinanceAccountID As Integer, ByVal v_cFinanceAmount As Decimal, ByRef r_lTransDetailID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, Optional ByVal v_vInterestCost As Object = Nothing, Optional ByRef r_lTransDetailID2 As Integer = 0, Optional ByVal v_sPlanReference As String = "", Optional ByVal v_sTransType As String = "", Optional ByVal v_sPlanTransType As String = "") As Integer

        Dim result As Integer = 0
        Dim cDebitAmount, cCreditAmount, cInterestCost As Decimal
        Dim iDocSeq As Integer
        Dim vPayments As Object
        ' Objects

        Dim lNumberRangeID As Integer

        ' Document paramters
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date
        Dim sComment As String = ""
        Dim dtNextDate As Date


        Dim sSQL As String = ""
        'EK 310100
        Dim sFullPath As String = ""
        Dim iBaseCurrencyID As Integer

        Dim sACTAutoNumberRangeCode As String = ""
        Dim sACTAutoNumberGroupCodeDocument As String = ""
        Dim iACTDocType As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_cDeposit > 0 Then
                v_cFinanceAmount -= v_cDeposit
            End If

            cCreditAmount = v_cFinanceAmount * -1
            cDebitAmount = v_cFinanceAmount

            If Not Informations.IsNothing(v_vInterestCost) Then
                cDebitAmount -= cInterestCost
            End If

            dtDocumentDate = DateTime.Now
            dtDocumentDate = dtDocumentDate.AddDays(v_iDaysDelay)
            '
            If Not m_bTransStarted Then
                ' Get an instance of DocumentPost if needed
                If m_oDocumentPost Is Nothing Then



                    m_oDocumentPost = New bACTDocumentPost.Form
                    m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                End If
                'EK 060300 Use correct auto numbering sequence
                ' Get an instance of auto number if needed
                If m_oAutoNumber Is Nothing Then



                    m_oAutoNumber = New bACTAutoNumber.Business
                    m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                End If

                m_lReturn = bPMFunc.getUnderwritingOrAgency(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingORAgency)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Debit
                If v_sTransType = "D" Then

                    sACTAutoNumberRangeCode = gACTLibrary.ACTAutoNumberRangeCodeIND
                    sACTAutoNumberGroupCodeDocument = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef42
                    iACTDocType = gACTLibrary.ACTDocTypeInstalmentNBDebit
                ElseIf v_sTransType = "C" Then
                    'Credit

                    sACTAutoNumberRangeCode = gACTLibrary.ACTAutoNumberRangeCodeINC
                    sACTAutoNumberGroupCodeDocument = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef43
                    iACTDocType = gACTLibrary.ACTDocTypeInstalmentNBCredit
                End If

                'ADO #39472: Override for claim recovery plans (SR/TPR) — use ICD/ICC
                If v_sPlanTransType = "SR" OrElse v_sPlanTransType = "TPR" Then
                    If v_sTransType = "D" Then
                        sACTAutoNumberRangeCode = gACTLibrary.ACTAutoNumberRangeCodeICD
                        sACTAutoNumberGroupCodeDocument = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef59
                        iACTDocType = gACTLibrary.ACTDocTypeInstalmentClaimDebit
                    ElseIf v_sTransType = "C" Then
                        sACTAutoNumberRangeCode = gACTLibrary.ACTAutoNumberRangeCodeICC
                        sACTAutoNumberGroupCodeDocument = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef60
                        iACTDocType = gACTLibrary.ACTDocTypeInstalmentClaimCredit
                    End If
                End If




                ' Get the number range

                m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=sACTAutoNumberGroupCodeDocument, v_sRangeCode:=sACTAutoNumberRangeCode, r_lNumberRangeID:=lNumberRangeID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Generate the next number
                'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=sACTAutoNumberRangeCode)
                'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Format the number
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                sDocumentRef = sACTAutoNumberRangeCode & sDocumentRef


                ' Start Transaction to enable Rollback if any part of posting fails
                m_lReturn = BeginTrans()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Post Transactions.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If

                ' Generate document
                'eck310801 changed Doc Type from Adjustment to Journal

                m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=iACTDocType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:="Premium Finance Transfer")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If
                m_bTransStarted = True
            End If

            'Get the Company's base currency

            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceID, r_iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            iDocSeq += 1


            m_lReturn = m_oDocumentPost.AddAdjustmentTransaction(v_lAccountID:=v_lClientAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=cCreditAmount, v_cCurrencyAmount:=cCreditAmount, v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=r_lTransDetailID, v_vDocumentSequence:=iDocSeq, v_vInsuranceRef:=v_sPlanReference, v_vSpare:="Premium Finance Credit " & sDocumentRef, v_vAccountingDate:=dtDocumentDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = RollbackTrans()
                Return result
            End If

            ReDim vPayments(0)

            vPayments(0) = cDebitAmount

            dtNextDate = dtDocumentDate

            For iCount As Integer = vPayments.GetLowerBound(0) To vPayments.GetUpperBound(0)
                iDocSeq += 1



                m_lReturn = m_oDocumentPost.AddAdjustmentTransaction(v_lAccountID:=v_lPremFinanceAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=CDec(vPayments(iCount)), v_cCurrencyAmount:=CDec(vPayments(iCount)), v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=iDocSeq, r_vTransDetailId:=r_lTransDetailID2, v_vInsuranceRef:=v_sPlanReference, v_vSpare:="Premium Finance Debit " & sDocumentRef, v_vAccountingDate:=dtNextDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_lReturn = RollbackTrans()
                    Return result
                End If
                dtNextDate = dtNextDate.AddYears(1)
            Next iCount
            'eck Commission Income Reduction
            ' Commit the document

            m_lReturn = m_oDocumentPost.Commit()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFail
                m_lReturn = RollbackTrans()
                Return result
            End If

            ' Commit Transaction
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Commit Premium Finance Posting.", vApp:=ACApp, vClass:=ACClass, vMethod:="Post Transactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostTransactions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Start
    '
    ' Description: Navigator Start function. Entry point into Navigator.
    '
    ' ***************************************************************** '
    Public Function Start() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: TransactPremiumFinance(Public)
    '
    ' ***************************************************************** '
    'developer guide no. 33
    Public Function TransactPremiumFinance(ByVal v_lClientAccount As Integer, ByVal v_lPremFinanceAccount As Integer, ByVal v_cFinanceAmount As Decimal, ByVal v_vTransactionIDs(,) As Object, ByVal v_iCompanyID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, ByVal v_lDepositTransId As Integer, Optional ByVal v_vInterestCost As Object = Nothing, Optional ByVal v_sPlanReference As String = "", Optional ByVal v_lAgentCnt As Integer = 0, Optional ByVal v_lAgentType As Integer = 0, Optional ByRef r_lProviderTransDetailID As Integer = 0, Optional ByVal v_sPlanTransType As String = "") As Integer


        Dim result As Integer = 0
        Dim vTransactionID As String = ""
        Dim vTransactionIDs As Object
        Dim sDocumentRef As String = ""
        Dim lTransDetailID, lSuspenseAccount, lTransDetailID2 As Integer

        Dim lSubAgentAccountId, lClientTransdetailId As Integer
        Dim vSubAgentTransactionID As String = ""
        Dim vSubAgentTransactionIDs As Object = Nothing
        Dim nAccount As Integer
        'FSA Phase IV Variables

        Dim vFinancedTransactions As Object
        Dim vSuspenseCredit As String = ""
        Dim lDepositTransDetailID As Integer
        Dim dDepositPercentage As Double
        Dim lFinancedTrans As Integer
        Dim sInsuranceRef as String=""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Installment Suspense Account
            m_lReturn = GetAccountID(lSuspenseAccount, "ISUSP", v_iCompanyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_iSourceID = v_iCompanyID


            vTransactionIDs = v_vTransactionIDs.Clone()

            sInsuranceRef=v_vTransactionIDs(1,0)
            m_lReturn = GetFinanceAmount(v_vTransactionIDs:=vTransactionIDs, v_vFinanceAmount:=v_cFinanceAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If



            vFinancedTransactions = vTransactionIDs

            'SJ 16/08/2004 - start
            'Debit the client and credit the sub agent
            If v_vTransactionIDs.GetUpperBound(1) = 0 Then

                m_lReturn = GetSubAgentAccountId(v_lTransdetailId:=CInt(v_vTransactionIDs(0, 0)), r_lSubAgentAccountId:=lSubAgentAccountId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
                If lSubAgentAccountId > 0 Then

                    m_lReturn = GetClientTransactionForSubAgent(v_lTransdetailId:=CInt(v_vTransactionIDs(0, 0)), r_lClientTransdetailId:=lClientTransdetailId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                End If

                If lClientTransdetailId > 0 Then
                    m_lReturn = PostTransactions(v_lClientAccountID:=v_lClientAccount, v_lPremFinanceAccountID:=lSubAgentAccountId, v_cFinanceAmount:=(v_cFinanceAmount * -1), r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=0, v_iInstallments:=v_iInstallments, v_cDeposit:=0, v_vInterestCost:=0, r_lTransDetailID2:=lTransDetailID2, v_sPlanReference:=sInsuranceRef, v_sPlanTransType:=v_sPlanTransType)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return m_lReturn
                    End If
                    vSubAgentTransactionID = CStr(lTransDetailID) & "|" & (CStr(v_cFinanceAmount))
                    ReDim vSubAgentTransactionIDs(0)

                    vSubAgentTransactionIDs(0) = CStr(lClientTransdetailId) & "|" & (CStr(v_cFinanceAmount * -1))
                    m_bTransStarted = False
                End If
            End If

            nAccount = 0
            If Not Informations.IsNothing(v_lAgentCnt) And v_lAgentCnt <> 0 And v_lAgentType = k_AGENT_TYPE_BROKER Then
                nAccount = CInt(v_vTransactionIDs(3, 0))
            Else
                nAccount = v_lClientAccount
            End If
            'Post Credit To Client
            'DJM 26/02/2004 : No delay on the transactions applied to the client, only on ones to the premium provider.
            m_lReturn = PostTransactions(v_lClientAccountID:=nAccount, v_lPremFinanceAccountID:=lSuspenseAccount, v_cFinanceAmount:=v_cFinanceAmount, r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=0, v_iInstallments:=v_iInstallments, v_cDeposit:=0, v_vInterestCost:=0, r_lTransDetailID2:=lTransDetailID2, v_sPlanReference:=sInsuranceRef, v_sTransType:="C", v_sPlanTransType:=v_sPlanTransType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            m_bTransStarted = False

            'FSA Phase IV
            'vTransactionID = lTransDetailID & "|" & (v_cFinanceAmount * -1)
            vSuspenseCredit = CStr(lTransDetailID) & "|" & (CStr(v_cFinanceAmount * -1))


            vTransactionID = CStr(lTransDetailID2) & "|" & CStr(v_cFinanceAmount)

            'Post Debit to Client (Deposit)
            'DJM 26/02/2004 : No delay on the transactions applied to the client, only on ones to the premium provider.
            'FSA Phase IV Return the Deposit transdetail
            If v_cDeposit > 0 Then
                m_lReturn = PostTransactions(v_lClientAccountID:=lSuspenseAccount, v_lPremFinanceAccountID:=v_lClientAccount, v_cFinanceAmount:=v_cDeposit, r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=0, v_iInstallments:=v_iInstallments, v_cDeposit:=0, v_vInterestCost:=0, v_sPlanReference:=sInsuranceRef, r_lTransDetailID2:=lDepositTransDetailID, v_sTransType:="D", v_sPlanTransType:=v_sPlanTransType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                    Return result
                End If

                'FSA Phase IV Calculate deposit percentage
                dDepositPercentage = v_cDeposit / v_cFinanceAmount

                m_bTransStarted = False

                ReDim vTransactionIDs(1)

                vTransactionIDs(0) = CStr(lTransDetailID) & "|" & (CStr(v_cDeposit * -1))
            End If

            'Post Debit to Finance Provider
            m_lReturn = PostTransactions(v_lClientAccountID:=lSuspenseAccount, v_lPremFinanceAccountID:=v_lPremFinanceAccount, v_cFinanceAmount:=v_cFinanceAmount - v_cDeposit, r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=v_iDaysDelay, v_iInstallments:=v_iInstallments, v_cDeposit:=0, v_vInterestCost:=0, v_sPlanReference:=sInsuranceRef, r_lTransDetailID2:=r_lProviderTransDetailID, v_sTransType:="D", v_sPlanTransType:=v_sPlanTransType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            'FSA Phase IV

            'Update the suspended accounts
            If m_oTransDetail Is Nothing Then ' Create an instance of the Trans Detail business object

                m_oTransDetail = New bACTTransdetail.Form
                m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "Failed to Initialise Trans Detail", ACApp, ACClass, "PostPlan", Informations.Err().Number, Informations.Err().Description)
                    Return result
                End If
            End If


            For lCount As Integer = 0 To vFinancedTransactions.GetUpperBound(0)
                If v_cDeposit <> 0 Then

                    lFinancedTrans = CInt(CStr(vFinancedTransactions(lCount)).Substring(0, CStr(vFinancedTransactions(lCount)).IndexOf("|"c)))

                    m_lReturn = m_oTransDetail.FinanceSuspendedTransactions(lOldTriggerTransdetailID:=lFinancedTrans, vPlanTransdetailId:=r_lProviderTransDetailID, vDepositTransdetailId:=lDepositTransDetailID, vDepositPercentage:=dDepositPercentage)
                Else

                    m_lReturn = m_oTransDetail.FinanceSuspendedTransactions(lOldTriggerTransdetailID:=lFinancedTrans, vPlanTransdetailId:=r_lProviderTransDetailID)

                End If
            Next lCount
            'Allocations moved to the end of the procedure
            'So that Suspended records for Financed records will have been updated
            'Allocate Credit Posted to Client to Initial Debit.


            m_lReturn = Allocate(v_lClientAccount:=v_lClientAccount, v_vTransaction:=vSuspenseCredit, v_vTransactions:=vFinancedTransactions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            'FSA Phase IV End
            If v_cDeposit > 0 Then

                vTransactionIDs(1) = CStr(lTransDetailID) & "|" & CStr((v_cFinanceAmount - v_cDeposit) * -1)
            Else
                ReDim vTransactionIDs(0)

                vTransactionIDs(0) = CStr(lTransDetailID) & "|" & (CStr(v_cFinanceAmount * -1))
            End If

            'Allocate Suspense Posting (to zero balance).

            m_lReturn = Allocate(v_lClientAccount:=lSuspenseAccount, v_vTransaction:=vTransactionID, v_vTransactions:=vTransactionIDs)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            If lClientTransdetailId > 0 Then

                m_lReturn = Allocate(v_lClientAccount:=v_lClientAccount, v_vTransaction:=vSubAgentTransactionID, v_vTransactions:=vSubAgentTransactionIDs)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactPremiumFinance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactPremiumFinance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetAccountID(ByRef r_lSupplierID As Integer, ByVal v_sShortCode As String, ByVal v_iCompanyID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetAccountID
        ' PURPOSE: Rewritten for handling Multi-Branch accounts
        ' AUTHOR: Danny Davis
        ' DATE: 10 July 2003, 05:03 PM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            Dim lRecordCount As Integer = 0

            'Returns the account ID from the short_code

            result = gPMConstants.PMEReturnCode.PMTrue
            r_lSupplierID = 0

            With m_oDatabase

                .Parameters.Clear()
                .Parameters.Add("company_id", CStr(v_iCompanyID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'developer guide no. 85
                .Parameters.Add("sub_branch_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("ShortCode", v_sShortCode.Trim(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                'developer guide no. 85
                .Parameters.Add("AccountID", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                'developer guide no. 39
                m_lReturn = .SQLSelect("SPU_ACT_GET_ACCOUNTID_FROM_SHORTCODE", "SPU_ACT_GET_ACCOUNTID_FROM_SHORTCODE", bStoredProcedure:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            'Get the account ID
            r_lSupplierID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("AccountID").Value)


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    ' PUBLIC Methods (End)

    ' ***************************************************************** '
    ' Name: Allocate(Public)
    '
    ' ***************************************************************** '
    Public Function Allocate(ByVal v_lClientAccount As Integer, ByVal v_vTransaction As Object, ByVal v_vTransactions As Object) As Integer

        Dim result As Integer = 0
        Dim vKeys As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ReDim vKeys(1, 2)
            ' AccountID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = v_lClientAccount

            ' Premium Finance Transaction ID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = v_vTransaction

            ' Matched Transactions

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = v_vTransactions

            ' Create an instance of the OrionLink business object


            m_oAllocateManual = New bACTAllocationManual.Business
            m_lReturn = m_oAllocateManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Initialise the manual alloction", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If



            m_lReturn = m_oAllocateManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

            ' Set the keys

            m_lReturn = m_oAllocateManual.SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start it

            m_oAllocateManual.CompanyId = m_iSourceID

            m_lReturn = m_oAllocateManual.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oAllocateManual.Dispose()


            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: Get Finance Amount (Private)
    '
    ' Description: Gets Finance Amount for Client Transactions
    '
    ' ***************************************************************** '
    Private Function GetFinanceAmount(ByRef v_vTransactionIDs As Array, ByRef v_vFinanceAmount As Double) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim lTransactionID As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim cFinanceAmount, cTransAmount, cMatchAmount, cDeposit As Decimal



        result = gPMConstants.PMEReturnCode.PMTrue
        Dim vTransactionIDs(v_vTransactionIDs.GetUpperBound(1)) As Object
        For lRow As Integer = v_vTransactionIDs.GetLowerBound(1) To v_vTransactionIDs.GetUpperBound(1)


            lTransactionID = CInt(v_vTransactionIDs(0, lRow))

            'Get full amount in base currency
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("nTransactionID", gPMFunctions.ToSafeString(lTransactionID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .SQLSelect("spu_ACT_Get_FinanceAmount", "spu_ACT_Get_FinanceAmount", True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMError

                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run " & "spu_ACT_Get_FinanceAmount", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinanceAmount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If

                cTransAmount = .Records.Item(0).Fields(0)
            End With

            'Get matched amount in base currency
            sSQL = "SELECT sum(base_match_amount) " &
                   "FROM TransMatch " &
                   "WHERE transdetail_id = " & CStr(lTransactionID) &
                   " AND allocationdetail_id IS NOT NULL " &
                   "GROUP BY transdetail_id"

            ' #MatchAmounts
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectMatchAmounts", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on m_oDatabase.SQLSelect ' #MatchAmounts", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFinanceAmount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            If Not Informations.IsArray(vResultArray) Then
                cMatchAmount = 0
            Else

                cMatchAmount = CDec(vResultArray(0, 0))
            End If
            cTransAmount = cTransAmount - cMatchAmount - cDeposit
            cFinanceAmount += cTransAmount
            vTransactionIDs(lRow) = CStr(lTransactionID) & "|" & CStr(cTransAmount)
        Next lRow

        v_vFinanceAmount = cFinanceAmount

        v_vTransactionIDs = vTransactionIDs
        Return result

    End Function
    ' ***************************************************************** '


    ' ***************************************************************** '
    ' Name: Get Document Reference for Transaction (Private)
    '
    ' Description: Gets Document Reference for  Transaction
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetDocumentRef) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetDocumentRef(ByVal v_lTransactionId As Object, ByRef v_sDocumentRef As Object) As Integer
    '
    'Dim result As Integer = 0
    'Dim oFields As ADODB.Fields
    'Dim sSQL As String = ""
    'Dim lRecordCount, lRow As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Try 
    '
    'm_oDatabase.Parameters.Clear()
    '

    'm_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransactionId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'sSQL = ""
    'sSQL = "SELECT D.document_ref " & Strings.Chr(13) & Strings.Chr(10)
    'sSQL = sSQL & "FROM document D, " & Strings.Chr(13) & Strings.Chr(10)
    'sSQL = sSQL & "transdetail T " & Strings.Chr(13) & Strings.Chr(10)
    'sSQL = sSQL & "WHERE T.document_id = D.document_id " & Strings.Chr(13) & Strings.Chr(10)
    'sSQL = sSQL & "AND T.transdetail_id = {transdetail_id}"
    'With m_oDatabase
    'm_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="ACSelectDocumentRef", bStoredProcedure:=False)
    '
    '
    ' Database error encountered
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMError
    'End If
    '
    ' Set return value
    'lRecordCount = .Records.Count()
    ' Record Count includes Doc and Transactions so we need at least 1 of each
    'If lRecordCount <> 1 Then
    ' No enough rows retreived
    'result = gPMConstants.PMEReturnCode.PMFalse
    'Else
    ' Rows retrieved successfully
    'result = gPMConstants.PMEReturnCode.PMTrue
    'End If
    'oFields = m_oDatabase.Records.Item(1).Fields()
    'With oFields
    'AK 230702 - scalability - check for null values

    'If Not (Convert.IsDBNull(oFields("document_ref")) Or IsNothing(oFields("document_ref"))) Then

    'v_sDocumentRef = oFields("document_ref").Trim()
    'Else

    'v_sDocumentRef = ""
    'End If
    'End With
    'End With
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocumentRef Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocumentRef", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    'DC240402 -Start
    ' ***************************************************************** '
    ' Name: TransactIts4MeDeposit(Public)
    '
    ' ***************************************************************** '
    Public Function TransactIts4meDeposit(ByVal v_lClientAccount As Integer, ByVal v_lPremFinanceAccount As Integer, ByVal v_cFinanceAmount As Decimal, ByVal v_vTransactionIDs As Object, ByVal v_iCompanyID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, ByRef v_lDepositTransId As Integer, Optional ByRef v_vInterestCost As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim sDocumentRef As String = ""
        Dim lTransDetailID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_iSourceID = v_iCompanyID

            m_lReturn = PostTransactions(v_lClientAccountID:=v_lClientAccount, v_lPremFinanceAccountID:=v_lPremFinanceAccount, v_cFinanceAmount:=v_cDeposit, r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=v_iDaysDelay, v_iInstallments:=v_iInstallments, v_cDeposit:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            v_lDepositTransId = lTransDetailID

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransactIts4MeDeposit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransactIts4MeDeposit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'DC240402 -End

    Private Function GetSubAgentAccountId(ByVal v_lTransdetailId As Integer, ByRef r_lSubAgentAccountId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSubAgentAccountId
        ' PURPOSE: Get sub agent account if it exists
        ' AUTHOR: Steve James
        ' DATE: 16 August 2004
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResultArray(,) As Object = Nothing

        r_lSubAgentAccountId = 0

        With m_oDatabase

            .Parameters.Clear()
            .Parameters.Add("transdetail_id", CStr(v_lTransdetailId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect("spu_ACT_get_sub_agent", "spu_ACT_get_sub_agent", bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        If Not Informations.IsArray(vResultArray) Then
            Return result
        End If

        'Get the sub agent account ID

        If vResultArray.GetUpperBound(1) = 0 Then
            'Only get if there is one sub agent

            r_lSubAgentAccountId = CInt(vResultArray(1, 0))
        End If


        Return result


    End Function


    Private Function GetClientTransactionForSubAgent(ByVal v_lTransdetailId As Integer, ByRef r_lClientTransdetailId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetClientTransactionForSubAgent
        ' PURPOSE: Get sub agent account if it exists
        ' AUTHOR: Steve James
        ' DATE: 16 August 2004
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vResultArray(,) As Object = Nothing

        r_lClientTransdetailId = 0

        With m_oDatabase

            .Parameters.Clear()
            .Parameters.Add("transdetail_id", CStr(v_lTransdetailId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect("{call spu_ACT_get_client_for_sub_agent(?)}", "spu_ACT_get_client_for_sub_agent", bStoredProcedure:=True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        If Not Informations.IsArray(vResultArray) Then
            Return result
        End If

        'Get the sub agent account ID

        If vResultArray.GetUpperBound(1) = 0 Then
            'Only get if there is one sub agent

            r_lClientTransdetailId = CInt(vResultArray(0, 0))
        End If

        Return result


    End Function


    '(RC) PLICO 9-10
    ' ***************************************************************** '
    ' Name: CancelPremiumFinance(Public)
    '
    ' ***************************************************************** '
    Public Function CancelPremiumFinance(ByVal v_lClientAccount As Integer, ByVal v_lPremFinanceAccount As Integer, ByVal v_cFinanceAmount As Decimal, ByVal v_vTransactionIDs() As Object, ByVal v_iCompanyID As Integer, ByVal v_iDaysDelay As Integer, ByVal v_iInstallments As Integer, ByVal v_cDeposit As Decimal, ByVal v_lDepositTransId As Integer, Optional ByVal v_vInterestCost As Object = Nothing, Optional ByVal v_sPlanReference As String = "", Optional ByRef r_lProviderTransDetailID As Integer = 0) As Integer


        Dim result As Integer = 0
        Dim vTransactionID As String = ""
        Dim vTransactionIDs As Object
        Dim sDocumentRef As String = ""
        Dim lTransDetailID, lSuspenseAccount, lTransDetailID2 As Integer

        Dim vFinancedTransactions As Object
        Dim vSuspenseCredit As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Installment Suspense Account
            m_lReturn = GetAccountID(lSuspenseAccount, "ISUSP", v_iCompanyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            m_iSourceID = v_iCompanyID


            vTransactionIDs = v_vTransactionIDs

            m_lReturn = GetFinanceAmount(v_vTransactionIDs:=vTransactionIDs, v_vFinanceAmount:=v_cFinanceAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If



            vFinancedTransactions = vTransactionIDs

            'Post Credit to Finance Provider
            m_lReturn = PostTransactions(v_lClientAccountID:=v_lPremFinanceAccount, v_lPremFinanceAccountID:=lSuspenseAccount, v_cFinanceAmount:=v_cFinanceAmount - v_cDeposit, r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=v_iDaysDelay, v_iInstallments:=v_iInstallments, v_cDeposit:=0, v_vInterestCost:=0, v_sPlanReference:=v_sPlanReference, r_lTransDetailID2:=r_lProviderTransDetailID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            vSuspenseCredit = CStr(lTransDetailID) & "|" & CStr(v_cFinanceAmount)


            vTransactionID = CStr(lTransDetailID2) & "|" & (CStr(v_cFinanceAmount * -1))

            'Post Debit To Client and Credit to Suspence
            m_lReturn = PostTransactions(v_lClientAccountID:=lSuspenseAccount, v_lPremFinanceAccountID:=v_lClientAccount, v_cFinanceAmount:=v_cFinanceAmount, r_lTransDetailID:=lTransDetailID, v_iDaysDelay:=0, v_iInstallments:=v_iInstallments, v_cDeposit:=0, v_vInterestCost:=0, r_lTransDetailID2:=lTransDetailID2, v_sPlanReference:=v_sPlanReference)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If


            m_lReturn = Allocate(v_lClientAccount:=v_lClientAccount, v_vTransaction:=vSuspenseCredit, v_vTransactions:=vFinancedTransactions)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If


            ReDim vTransactionIDs(0)

            vTransactionIDs(0) = CStr(lTransDetailID) & "|" & (CStr(v_cFinanceAmount))

            'Allocate Suspense Posting (to zero balance).

            m_lReturn = Allocate(v_lClientAccount:=lSuspenseAccount, v_vTransaction:=vTransactionID, v_vTransactions:=vTransactionIDs)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
                Return result
            End If

            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CancelPremiumFinance Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CancelPremiumFinance", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
