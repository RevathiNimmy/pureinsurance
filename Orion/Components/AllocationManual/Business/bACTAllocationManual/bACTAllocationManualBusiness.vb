Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no.129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ************************************************
    ' Added to replace global variables 09/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************
    Private Const ACClass As String = "Business"

    ' Return value
    Private m_lReturn As Integer

    ' Object parameter members.
    Private m_lStatus As Integer
    Private m_lErrorNumber As Integer

    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_lCompanyId As Integer
    Private m_lPeriodID As Integer
    'DD 01/08/2002: For Multi-Branch Accounting
    Private m_lSubBranchID As Integer

    Private m_bDDReversal As Boolean

    ' Instance of database object
    Public m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean

    Private m_oAllocationDetail As bACTAllocationDetail.Form
    Private m_oAllocation As bACTAllocation.Form
    Private m_oTransMatch As bACTTransmatch.Form
    Private m_oMatchGroup As bACTMatchgroup.Form
    Private m_oAllocationPost As Object
    Private m_oTransDetail As bACTTransDetail.Form
    Private m_oDocument As bACTDocument.Form
    Private m_oCashListItem As Object
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    ' Declare an instance of the Write off object
    Public m_oWriteOffReason As Object

    Private m_lBatchId As Integer

    Private m_lAccountId As Integer
    Private m_lAllocatedTransId As Integer
    'eck010800
    Private m_cAllocatedTransAmount As Decimal

    'sw added 17/01/2003
    Private m_lCashListItemID As Integer

    Private m_vTransDetailIds As Object

    Private m_lAllocationId As Integer
    Private m_nAllocationBatchId As Integer
    Private m_dtAllocationDate As Date
    Private m_lAllocationstatusId As Integer

    Private m_lMatchId As Integer

    Private m_iBaseCurrency As Integer

    Private m_oS4BDatabase As dPMDAO.Database
    Private m_sCommissionOption As String = ""
    Private m_oSystemOption As bSIROptions.Business
    Private m_oCommissionPost As Object
    Private m_bWithDID As Boolean

    ' Stores the List data from the business object.
    Private m_vListData As Object

    Private m_vTransData(,) As Object

    Private m_vKeyArray(,) As Object
    Private m_lWriteOffReasonId As Integer

    'Write off amount
    Private m_cWriteOffAmount As Decimal
    Private m_cWriteOffBaseAmount As Decimal
    'Currency difference amount
    Private m_cCurrencyDiffAmount As Decimal
    Private m_bIsCurrencyDifference As Boolean
    Private m_bAllocatingSuspense As Boolean

    Private m_bAllocatingReversal As Boolean

    ''Round Off amount
    Private m_cRoundOffAmount As Decimal
    Private m_cRoundOffBaseAmount As Decimal
    Private m_vRoundOffTransDetailId As Object
    Private m_bValidWriteOffAccount As Boolean

    'Private m_bIsNegativePaymentWithWriteOff As Boolean

    'Public Property IsNegativePaymentWithWriteOff() As Integer
    '    Get
    '        Return m_bIsNegativePaymentWithWriteOff
    '    End Get
    '    Set(ByVal Value As Integer)
    '        m_bIsNegativePaymentWithWriteOff = Value
    '    End Set
    'End Property

    Private m_nAllocatedTransExId As Integer
    Private m_nWriteOffAllocationId As Integer
    Private m_nWriteOffTransDetailId As Integer
    ' Product family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    Public Property CompanyId() As Integer
        Get
            Return m_lCompanyId
        End Get
        Set(ByVal Value As Integer)
            m_lCompanyId = Value
        End Set
    End Property
    Public Property IsValidWriteOffAccount() As Boolean
        Get

            Return m_bValidWriteOffAccount

        End Get
        Set(ByVal Value As Boolean)

            m_bValidWriteOffAccount = Value

        End Set
    End Property

    Public WriteOnly Property CommissionOption() As String
        Set(ByVal Value As String)
            m_sCommissionOption = Value
        End Set
    End Property
    Public WriteOnly Property AllocatingSuspense() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllocatingSuspense = Value
        End Set
    End Property

    Public WriteOnly Property AllocatingReversal() As Boolean
        Set(ByVal Value As Boolean)
            m_bAllocatingReversal = Value
        End Set
    End Property
    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


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


            ' Get an instance of the database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            ' Remove component services


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            'eck040101
            ' Get the Commission Transfer settings
            m_lReturn = GetOption(v_iOptionNumber:=16, r_sOptionValue:=m_sCommissionOption)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to read system option for Commission Option assuming Insurer " &
                                   "Settled.", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                m_sCommissionOption = "2"
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
    '
    ' Name: SetProcessModes
    '
    ' Description:
    '
    ' History: 16/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 01/09/1999 CTAF - Created.
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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oAllocationDetail IsNot Nothing Then
                    m_oAllocationDetail.Dispose()
                    m_oAllocationDetail = Nothing
                End If
                If m_oAllocation IsNot Nothing Then
                    m_oAllocation.Dispose()
                    m_oAllocation = Nothing
                End If
                If m_oWriteOffReason IsNot Nothing Then
                    m_oWriteOffReason.Dispose()
                    m_oWriteOffReason = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oCashListItem IsNot Nothing Then
                    m_oCashListItem.Dispose()
                    m_oCashListItem = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub



    ' ***************************************************************** '
    '
    ' Name: SetStatus
    '
    ' Description:
    '
    ' History: 02/09/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SetStatus(ByVal sProcessStatus As String, ByVal sMapStatus As String, ByVal sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' SetKeys
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim nResult As Integer
        Dim oParse As Object = Nothing

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_vKeyArray = vKeyArray.Clone()

            '    ' Step through the key array.
            For iRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                Select Case Convert.ToString(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, iRow)).Trim()
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.ACTKeyNameTransDetailID

                        oParse = CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow)).Split("|")

                        m_lAllocatedTransId = CInt(oParse(0))

                        m_cAllocatedTransAmount = CDec(oParse(1))

                    Case PMNavKeyConst.ACTKeyNameTransDetailIDs

                        m_vTransDetailIds = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow)

                    Case PMNavKeyConst.ACTKeyNameWriteOffReasonId

                        m_lWriteOffReasonId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.ACTKeyNameWriteOffAmount

                        m_cWriteOffAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))

                    Case PMNavKeyConst.ACTKeyNameCashListItemId

                        m_lCashListItemID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.ACTKeyNameCurrencyDifference

                        m_cCurrencyDiffAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                        'FSA Phase 3.2
                    Case PMNavKeyConst.ACTKeyNameAllocatingSuspense

                        m_bAllocatingSuspense = CBool(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.ACTKeyNameRoundOffAmount
                        m_cRoundOffAmount = CDec(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.ACTKeyNameRoundOffTransDetailId
                        m_vRoundOffTransDetailId = vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow)
                    Case PMNavKeyConst.ACTKeyNameAllocationBatchId
                        m_nAllocationBatchId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.kTKeyNameTransDetailExID
                        m_nAllocatedTransExId = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                    Case PMNavKeyConst.ACTKeyNameTransactionDate
                        m_dtAllocationDate = (vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, iRow))
                End Select

            Next iRow

            'If we have a currency difference amount then set it as the write off amount
            'and flag as currency difference.
            If m_cCurrencyDiffAmount <> 0 Then
                m_cWriteOffAmount = m_cCurrencyDiffAmount
                m_bIsCurrencyDifference = True
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' GetKeys
    ''' </summary>
    ''' <param name="oKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetKeys(ByRef oKeyArray(,) As Object) As Integer

        Dim nResult As Integer
        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Initialise the key array with the number of
        ' keys needed to be returned.
        ' Note: Remember arrays are zero based.
        ReDim oKeyArray(1, 3)

        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAllocationId
        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAllocationId

        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameAllocationBatchId
        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = m_nAllocationBatchId

        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.kTKeyNameWriteOffAllocationId
        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = m_nWriteOffAllocationId

        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.kTKeyNameWriteOffTransDetailId
        oKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = m_nWriteOffTransDetailId
        Return nResult
    End Function

    ''' <summary>
    ''' Start
    ''' </summary>
    ''' <returns></returns>
    Public Function Start() As Integer
        Return Start(v_bDisableTransactions:=False)
    End Function

    Public Function Start(ByVal v_bDisableTransactions As Boolean) As Integer
        Dim nResult As Integer
        Dim nAllocationDetailId As Integer = 0
        Dim oTransIDs(,) As Object = Nothing
        Dim nFullyMatched As Integer = 0
        Dim nDirectDebitTrans As Integer
        Dim nLedgerID As Integer = 0
        Dim sLedgerTypeCode As String = ""

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Not v_bDisableTransactions Then
                nResult = BeginTrans()
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Only Do this section if there is a valid array of transactions.
            'In most cases now, there will be no transactions
            If Informations.IsArray(m_vTransDetailIds) Then
                nResult = GetTransactionData()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    If Not v_bDisableTransactions Then
                        nResult = RollbackTrans()
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                nResult = gPMFunctions.ShellSort2DArray(m_vTransData, ACTransDocumentId)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    If Not v_bDisableTransactions Then
                        nResult = RollbackTrans()
                    End If
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            If m_nAllocationBatchId = 0 Then
                If m_dtAllocationDate = DateTime.MinValue Then
                    m_dtAllocationDate = DateTime.Now
                End If
                nResult = CreateAllocationBatch(0, m_nAllocationBatchId, m_dtAllocationDate)
                If (nResult <> gPMConstants.PMEReturnCode.PMTrue) Then
                    If v_bDisableTransactions = False Then
                        nResult = RollbackTrans()
                    End If

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            nResult = CreateAllocation()
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                If Not v_bDisableTransactions Then
                    nResult = RollbackTrans()
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Again, only do this if there are any Transactions
            If Informations.IsArray(m_vTransData) Then

                nDirectDebitTrans = 0
                For iRow As Integer = m_vTransData.GetLowerBound(1) To m_vTransData.GetUpperBound(1)
                    If CStr(m_vTransData(ACTransSpare, iRow)) = "DIRECTDEBIT" Then
                        'Check Ledger is Client (LedgerTypeCode='D')
                        GetLedgerForAccount(v_lAccountID:=CInt(m_vTransData(ACTransAccountID, iRow)), r_lLedgerID:=nLedgerID, r_sLedgerTypeCode:=sLedgerTypeCode)
                        If sLedgerTypeCode = "D" Then
                            nDirectDebitTrans = CInt(m_vTransData(ACTransDetailId, iRow))
                            Exit For
                        End If
                    End If
                Next iRow

                m_lSubBranchID = 0
                For iRow As Integer = m_vTransData.GetLowerBound(1) To m_vTransData.GetUpperBound(1)

                    nResult = CreateAllocationDetail(lRow:=iRow, lAllocationDetailId:=nAllocationDetailId, lWriteOffReasonId:=m_lWriteOffReasonId, cWriteOffAmount:=m_cWriteOffAmount, r_lFullyMatched:=nFullyMatched, cRoundOffAmount:=m_cRoundOffAmount, vRoundOffTransDetailId:=m_vRoundOffTransDetailId)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        If Not v_bDisableTransactions Then
                            nResult = RollbackTrans()
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    nResult = UpdateTransdetail(v_lRow:=iRow, v_lFullyMatched:=nFullyMatched)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        If Not v_bDisableTransactions Then
                            nResult = RollbackTrans()
                        End If

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    nResult = UpdateCashListItem(v_lRow:=iRow, v_lAllocationStatusId:=gACTLibrary.ACTAllocationStatusAllocated)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        If Not v_bDisableTransactions Then
                            nResult = RollbackTrans()
                        End If
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If m_sCommissionOption <> AsDebited Then
                        If Informations.IsArray(oTransIDs) Then

                            ReDim Preserve oTransIDs(0, oTransIDs.GetUpperBound(1) + 1)
                        Else
                            ReDim oTransIDs(0, 0)
                        End If

                        oTransIDs(0, oTransIDs.GetUpperBound(1)) = m_vTransData(ACTransDetailId, iRow)
                    End If

                Next iRow
            End If

            If Not v_bDisableTransactions Then
                nResult = CommitTrans()
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' GetTransactionData
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetTransactionData() As Integer

        Dim nResult As Integer
        Dim oTransDetailIds As Object = Nothing
        Dim oTransPayments As Object = Nothing

        Dim iRow As Integer = 0
        Dim oTransDetailId As Object = Nothing
        Dim oCompanyID As Object = Nothing
        Dim oFullyMatched As Object = Nothing
        Dim oCurrencyID As Object = Nothing
        Dim oBaseAmount As Object = Nothing
        Dim oCurrencyAmount As Object = Nothing
        Dim oBaseAmountUnrounded As Object = Nothing
        Dim oCurrencyAmountUnrounded As Object = Nothing
        Dim oCurrencyBaseXRate As Object = Nothing
        Dim oDocumentID As Object = Nothing
        Dim oDocumentSequence As Object = Nothing
        'From Document
        Dim oDocumentRef As Object = Nothing
        Dim oDocumentTypeID As Object = Nothing
        Dim oDocumentDate As Object = Nothing
        'From TransMatch
        Dim crBaseMatchAmount As Decimal
        Dim crCurrencyMatchAmount As Decimal

        Dim crAllocatedDiff As Decimal
        Dim oParse As Object = Nothing
        Dim oSpare As Object = Nothing
        Dim oAccountID As Object = Nothing
        Dim crCurrencyPaymentAmount As Decimal
        'Dim crBasePaymentAmount As Decimal
        Dim crAccountAmount As Decimal
        Dim crSystemAmount As Decimal
        Dim oTransDetailExIds As Object = Nothing
        Dim oWriteOffReason As Object = Nothing
        Dim oWriteOffAmount As Object = Nothing
        Dim oAccountBaseXRate As Object = Nothing
        Dim oSystemBaseXRate As Object = Nothing
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsArray(m_vTransDetailIds) Then
                'Get the Current Company
                If m_lCompanyId = 0 Then
                    m_lCompanyId = m_iSourceID
                End If

                'Create Array of Transaction Details

                iRow = m_vTransDetailIds.GetUpperBound(0)
                ReDim oTransDetailIds(iRow + 1)

                ReDim oTransPayments(iRow + 1)
                ReDim oTransDetailExIds(iRow + 1)
                ReDim oWriteOffReason(iRow + 1)
                ReDim oWriteOffAmount(iRow + 1)

                oTransDetailIds(0) = m_lAllocatedTransId

                oTransPayments(0) = m_cAllocatedTransAmount

                oTransDetailExIds(0) = m_nAllocatedTransExId

                If m_bDDReversal Then
                    crAllocatedDiff = Math.Round(m_cAllocatedTransAmount, 2)
                End If

                For iRow = m_vTransDetailIds.GetLowerBound(0) To m_vTransDetailIds.GetUpperBound(0)

                    oParse = CStr(m_vTransDetailIds(iRow)).Split("|"c)

                    oTransDetailIds(iRow + 1) = oParse(0) 'TransDetailId

                    oTransPayments(iRow + 1) = oParse(1) 'BaseAmount

                    If DirectCast(oParse, Object()).GetUpperBound(0) = 4 Then
                        oTransDetailExIds(iRow + 1) = ToSafeLong(oParse(2))
                        oWriteOffReason(iRow + 1) = ToSafeLong(oParse(3))
                        oWriteOffAmount(iRow + 1) = ToSafeDouble(oParse(4))
                    End If

                    If m_bDDReversal Then
                        crAllocatedDiff += Math.Round(CDbl(CDbl(CStr(m_vTransDetailIds(iRow)).Substring((CStr(m_vTransDetailIds(iRow)).IndexOf("|"c) + 1) + 1))), 2)
                    End If
                Next iRow

                If m_bDDReversal Then
                    crAllocatedDiff -= m_cWriteOffAmount
                    'Now adjust Amount To Allocate by the difference

                    oTransPayments(0) = CDbl(oTransPayments(0)) - crAllocatedDiff
                    m_cAllocatedTransAmount -= crAllocatedDiff
                End If

                ' Get an instance of Transaction Details
                If m_oTransDetail Is Nothing Then


                    m_oTransDetail = New bACTTransDetail.Form
                    nResult = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If
                End If

                If m_oCurrencyConvert Is Nothing Then


                    m_oCurrencyConvert = New bACTCurrencyConvert.Form
                    nResult = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If
                End If

                ' Get the Company's base currency
                nResult = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_lCompanyId, r_iBaseCurrencyID:=m_iBaseCurrency)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If

                ReDim m_vTransData(ACTransLastField, oTransDetailIds.GetUpperBound(0))

                For iRow = oTransDetailIds.GetLowerBound(0) To oTransDetailIds.GetUpperBound(0)

                    nResult = m_oTransDetail.GetDetails(vTransdetailID:=oTransDetailIds(iRow))
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    nResult = m_oTransDetail.GetNext(vTransdetailID:=oTransDetailId,
                                                       vCompanyID:=oCompanyID,
                                                       vFullyMatched:=oFullyMatched,
                                                       vCurrencyID:=oCurrencyID,
                                                       vAmount:=oBaseAmount,
                                                       vBaseAmountUnrounded:=oBaseAmountUnrounded,
                                                       vCurrencyAmount:=oCurrencyAmount,
                                                       vCurrencyAmountUnrounded:=oCurrencyAmountUnrounded,
                                                       vCurrencyBaseXrate:=oCurrencyBaseXRate,
                                                       vDocumentID:=oDocumentID,
                                                       vDocumentSequence:=oDocumentSequence,
                                                       vSpare:=oSpare, vAccountID:=oAccountID,
                                                       vAccountAmount:=crAccountAmount,
                                                       vSystemAmount:=crSystemAmount,
                                                      vAccountBaseXrate:=oAccountBaseXRate,
                                                      vSystemBaseXrate:=oSystemBaseXRate)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    nResult = GetDocument(vDocumentID:=CInt(oDocumentID),
                                            vDocumentRef:=oDocumentRef,
                                            vDocumentTypeID:=oDocumentTypeID,
                                            vDocumentDate:=oDocumentDate)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    nResult = GetMatchPayment(v_lTransdetailID:=CInt(oTransDetailId), v_cBaseAmount:=crBaseMatchAmount, v_cCurrencyAmount:=crCurrencyMatchAmount)

                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                    m_vTransData(ACTransDetailId, iRow) = oTransDetailId
                    m_vTransData(ACTransFullyPaid, iRow) = oFullyMatched
                    m_vTransData(ACTransCurrencyId, iRow) = oCurrencyID
                    m_vTransData(ACTransBaseAmount, iRow) = oBaseAmount
                    m_vTransData(ACTransCurrencyAmount, iRow) = oCurrencyAmount

                    m_vTransData(ACTransBaseAmountUnrounded, iRow) = oBaseAmount
                    m_vTransData(ACTransCurrencyAmountUnrounded, iRow) = oCurrencyAmount

                    m_vTransData(ACTransCurrencyBaseXrate, iRow) = oCurrencyBaseXRate
                    m_vTransData(ACTransDocumentId, iRow) = oDocumentID
                    If oDocumentSequence = 1 Then
                        m_vTransData(ACTransIsPrimary, iRow) = gPMConstants.PMEReturnCode.PMTrue
                    Else
                        m_vTransData(ACTransIsPrimary, iRow) = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_vTransData(ACTransDocumentReference, iRow) = oDocumentRef
                    m_vTransData(ACTransDocumentTypeID, iRow) = oDocumentTypeID
                    m_vTransData(ACTransDocumentDate, iRow) = oDocumentDate
                    m_vTransData(ACTransMatchedBaseAmount, iRow) = crBaseMatchAmount
                    m_vTransData(ACTransMatchedCurrencyAmount, iRow) = crCurrencyMatchAmount

                    m_vTransData(ACTransPayBaseAmount, iRow) = oTransPayments(iRow)

                    If ToSafeInteger(oCurrencyID) <> m_iBaseCurrency Then
                        'Get payment amount in currency.

                        nResult = m_oCurrencyConvert.Convert(v_bConvertToBase:=False, v_lCurrencyID:=CInt(oCurrencyID), v_lCompanyId:=CInt(oCompanyID), r_cOriginalAmount:=CDec(oTransPayments(iRow)), r_cConvertedAmount:=crCurrencyPaymentAmount, r_vConversionRate:=oCurrencyBaseXRate)
                        'm_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=CInt(oCurrencyID), lCompanyID:=CInt(oCompanyID), cBaseAmount:=crBasePaymentAmount, cCurrencyAmount:=oTransPayments(iRow), vConversionDate:=Nothing)



                        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If
                        'm_vTransData(ACTransPayBaseAmount, iRow) = crCurrencyPaymentAmount
                        m_vTransData(ACTransPayCurrencyAmount, iRow) = crCurrencyPaymentAmount
                    Else
                        ' m_vTransData(ACTransPayBaseAmount, iRow) = oTransPayments(iRow)
                        m_vTransData(ACTransPayCurrencyAmount, iRow) = oTransPayments(iRow)
                    End If

                    m_vTransData(ACTransSpare, iRow) = oSpare
                    m_vTransData(ACTransAccountID, iRow) = oAccountID
                    m_vTransData(ACTransCompanyID, iRow) = oCompanyID
                     m_vTransData(kTransAccountAmount, iRow) = 0

                    If ToSafeDouble(oAccountBaseXRate) <> 0 Then
                        m_vTransData(kTransAccountAmount, iRow) = oTransPayments(iRow) / ToSafeDouble(oAccountBaseXRate)
                    End If
                    m_vTransData(kTransSystemAmount, iRow) = oTransPayments(iRow) / ToSafeDouble(oSystemBaseXRate, 1)

                    If ToSafeLong(oTransDetailExIds(iRow)) > 0 Then
                        m_vTransData(kTransDetailExId, iRow) = oTransDetailExIds(iRow)
                    End If
                    If ToSafeLong(oWriteOffReason(iRow)) > 0 Then
                        m_vTransData(kWriteOffReasonId, iRow) = oWriteOffReason(iRow)
                    End If

                    If ToSafeDouble(oWriteOffAmount(iRow)) <> 0 Then
                        m_vTransData(kWriteOffAmount, iRow) = oWriteOffAmount(iRow)
                    End If
                    If m_lAccountId = 0 Then
                        m_lAccountId = CInt(oAccountID)
                    End If
                    m_vTransData(kIsCurrencyDiff, iRow) = False

                    If iRow = 0 Then
                        m_cWriteOffBaseAmount = m_cWriteOffAmount
                        m_cRoundOffBaseAmount = m_cRoundOffAmount
                        'Get Write Off amount in Currency
                        If Convert.ToInt32(oCurrencyID) <> m_iBaseCurrency Then
                            If Convert.ToDecimal(oCurrencyBaseXRate) <> 0 Then
                                If m_cRoundOffAmount <> 0 Then
                                    nResult = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=CInt(oCurrencyID), lCompanyID:=CInt(oCompanyID), cBaseAmount:=m_cRoundOffBaseAmount, cCurrencyAmount:=m_cRoundOffAmount, vConversionDate:=Nothing, vConversionRate:=oCurrencyBaseXRate)
                                Else
                                    nResult = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=CInt(oCurrencyID), lCompanyID:=CInt(oCompanyID), cBaseAmount:=m_cWriteOffBaseAmount, cCurrencyAmount:=m_cWriteOffAmount, vConversionDate:=Nothing, vConversionRate:=oCurrencyBaseXRate)
                                End If
                            Else
                                If m_cRoundOffAmount <> 0 Then
                                    nResult = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=CInt(oCurrencyID), lCompanyID:=CInt(oCompanyID), cBaseAmount:=m_cRoundOffBaseAmount, cCurrencyAmount:=m_cRoundOffAmount)
                                Else
                                    nResult = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=CInt(oCurrencyID), lCompanyID:=CInt(oCompanyID), cBaseAmount:=m_cWriteOffBaseAmount, cCurrencyAmount:=m_cWriteOffAmount)
                                End If
                            End If

                            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception()
                            End If
                        End If
                        If m_bIsCurrencyDifference Then
                            m_vTransData(kIsCurrencyDiff, iRow) = True
                        End If
                    End If
                Next iRow

            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Transaction Data Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function


    Public Function DDReversalAllocation(ByRef sDocumentRef As String, ByRef sReversalDocumentRef As String) As Integer

        Dim result As Integer = 0
        Dim iCount As Integer
        Dim vKeys As Object = Nothing
        Dim vMatchTrans As Object = Nothing
        Dim vResultArray As Object = Nothing
        Dim vReverseArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_bDDReversal = True

            ReDim vKeys(1, 2)
            ReDim vMatchTrans(0)

            m_oDatabase.Parameters.Clear()
            'eck140501 need to pass company Id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_lCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_ref", vValue:=sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelClientPostingsSQL, sSQLName:=ACSelClientPostingsName, bStoredProcedure:=ACSelClientPostingsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Client Postings ", vApp:=ACApp, vClass:=ACClass, vMethod:="DDreversalAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                ' Reset the mouse pointer
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return result
            End If
            'Get the details for the reverse transaction
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(m_lCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="document_ref", vValue:=sReversalDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(CInt(vResultArray(0, 0))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelReversalSQL, sSQLName:=ACSelReversalName, bStoredProcedure:=ACSelReversalStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vReverseArray)

            ' AccountID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = CInt(vResultArray(0, 0))

            ' DD Reverse ID

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID



            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(CInt(vReverseArray(0, 0))) & "|" & CStr(CDec(vReverseArray(1, 0)))


            If vResultArray.GetUpperBound(1) = 0 Then
                'If only one transaction then it's not a subagent transaction.
                'Match on the DD reverse amount as fees and extras need to be left as outstanding.



                vMatchTrans(iCount) = CStr(CInt(vResultArray(1, iCount))) & "|" & (CStr(CDec(vReverseArray(1, 0)) * -1))
            Else
                'If only more than one transaction then it is a subagent transaction.
                'Match on the full amount of all lines as the second line moves all of
                'the fees and extras to the sub agent.

                ReDim vMatchTrans(vResultArray.GetUpperBound(1))


                For iCount = 0 To vResultArray.GetUpperBound(1)



                    vMatchTrans(iCount) = CStr(CInt(vResultArray(1, iCount))) & "|" & CStr(CDec(vResultArray(2, iCount)))
                Next
            End If

            'Client Trans

            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans

            ' Set the keys

            m_lReturn = SetKeys(vKeyArray:=vKeys)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:="DDReversalAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Start it
            m_lReturn = Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:="DDReversalAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DDReversalAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DDReversalAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' CreateAllocation
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateAllocation() As Integer

        Dim nResult As Integer

        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' Get an instance of DocumentPost if needed
            If m_oAllocation Is Nothing Then


                m_oAllocation = New bACTAllocation.Form
                nResult = m_oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If

            ' Create a new allocation header (DirectAdd to get the Id for the detail)

            nResult = m_oAllocation.DirectAdd(vAllocationID:=m_lAllocationId,
                                                vCompanyID:=m_lCompanyId,
                                                vAccountID:=m_lAccountId,
                                                vUserID:=m_iUserID,
                                                vAllocationDate:=m_dtAllocationDate,
                                                vAllocationstatusID:=gACTLibrary.ACTAllocationStatusAllocated,
                                                r_nAllocationBatchID:=m_nAllocationBatchId)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Read back the details for later

            nResult = m_oAllocation.GetDetails(vAllocationId:=m_lAllocationId)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            m_oAllocation.Dispose()

            m_oAllocation = Nothing
            Return nResult

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=excep)

            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' CreateAllocationDetail
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="lAllocationDetailId"></param>
    ''' <param name="lWriteOffReasonId"></param>
    ''' <param name="cWriteOffAmount"></param>
    ''' <param name="r_lFullyMatched"></param>
    ''' <param name="cRoundOffAmount"></param>
    ''' <param name="vRoundOffTransDetailId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAllocationDetail(ByVal lRow As Integer, ByRef lAllocationDetailId As Integer, Optional ByVal lWriteOffReasonId As Integer = 0, Optional ByVal cWriteOffAmount As Object = Nothing, Optional ByRef r_lFullyMatched As Integer = 0, Optional ByVal cRoundOffAmount As Object = 0, Optional ByVal vRoundOffTransDetailId As Object = Nothing) As Integer

        Dim nResult As Integer

        Dim nFullyMatched As PMEReturnCode
        Dim dNewOSCcyAmount As Double
        Dim dNewOSBaseAmount As Double
        Dim nWriteOffReasonId As Integer = 0
        Dim oWriteOffAmount As Object = Nothing
        Dim oGainLossAmount As Object = Nothing
        Dim oRoundOff_Amount As Object = Nothing
        Dim crAllocAccountAmount As Decimal = 0
        Dim crAllocSystemAmount As Decimal = 0
        Dim crTotalWOAmount As Decimal = 0

        nResult = PMEReturnCode.PMTrue

        If (Not lWriteOffReasonId.Equals(0)) AndAlso Not (lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) OrElse lWriteOffReasonId.Equals(KSAMBDXCalling)) Then
            nWriteOffReasonId = lWriteOffReasonId
        Else
            nWriteOffReasonId = 0
        End If

        If (Not Informations.IsNothing(cWriteOffAmount)) AndAlso (Not cWriteOffAmount.Equals(0)) Then
            If m_bIsCurrencyDifference Then
                oGainLossAmount = cWriteOffAmount
            Else
                oWriteOffAmount = cWriteOffAmount
            End If
        Else
            oWriteOffAmount = 0
            oGainLossAmount = 0
        End If
        If cRoundOffAmount <> 0 Then
            oRoundOff_Amount = cRoundOffAmount
        End If
        ' Get an instance of DocumentPost if needed
        If m_oAllocation Is Nothing Then


            m_oAllocationDetail = New bACTAllocationDetail.Form
            nResult = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
        End If

        ' Set the process modes for the busines object.
        nResult = m_oAllocationDetail.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        ' Check for errors.
        If nResult <> PMEReturnCode.PMTrue Then
            nResult = nResult
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for allocation details", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAllocationDetail")
            Return nResult
        End If

        ' Create a new allocation Detail record  (DirectAdd to get the Id for the detail)
        'What we are allocating this time

        Dim crAllocBaseAmount As Decimal = CDec(m_vTransData(ACTransPayBaseAmount, lRow))
        Dim crAllocCCyAmount As Decimal = CDec(m_vTransData(ACTransPayCurrencyAmount, lRow))

        'What is outstanding - Original amount minus matched to date
        Dim dOSBaseAmount As Decimal = CDec(CDec(m_vTransData(ACTransBaseAmount, lRow)) - CDec(m_vTransData(ACTransMatchedBaseAmount, lRow)))
        Dim dOSCcyAmount As Decimal = CDec(CDec(m_vTransData(ACTransCurrencyAmount, lRow)) - CDec(m_vTransData(ACTransMatchedCurrencyAmount, lRow)))

        If Convert.ToDecimal(dOSBaseAmount - crAllocBaseAmount) = 0 OrElse Convert.ToDecimal(dOSCcyAmount - crAllocCCyAmount) = 0 Then
            nFullyMatched = PMEReturnCode.PMTrue
        Else
            nFullyMatched = PMEReturnCode.PMFalse
        End If

        'See if we should write off this time
        If Convert.ToInt32(m_vTransData(ACTransDetailId, lRow)) <> m_lAllocatedTransId Then
            nWriteOffReasonId = 0
            oWriteOffAmount = 0
            oGainLossAmount = 0
        End If

        If ToSafeLong(m_vTransData(kWriteOffReasonId, lRow)) > 0 AndAlso ToSafeDouble(m_vTransData(kWriteOffAmount, lRow)) <> 0 Then
            nWriteOffReasonId = ToSafeLong(m_vTransData(kWriteOffReasonId, lRow))
            oWriteOffAmount = m_vTransData(kWriteOffAmount, lRow)
        End If
        'If cRoundOffAmount <> 0 Then
        If (oWriteOffAmount <> 0 Or oGainLossAmount <> 0) AndAlso Not (lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) OrElse lWriteOffReasonId.Equals(KSAMBDXCalling)) Then
            dNewOSBaseAmount = dOSBaseAmount - crAllocBaseAmount - m_cWriteOffBaseAmount
            dNewOSCcyAmount = dOSCcyAmount - crAllocCCyAmount - m_cWriteOffAmount
        Else
            dNewOSBaseAmount = dOSBaseAmount - crAllocBaseAmount
            dNewOSCcyAmount = dOSCcyAmount - crAllocCCyAmount
        End If
        'End If
        If Convert.ToDecimal(dNewOSBaseAmount) = 0 Then
            nFullyMatched = PMEReturnCode.PMTrue
        Else
            nFullyMatched = PMEReturnCode.PMFalse
        End If
        If oRoundOff_Amount <> 0 Then
            dNewOSBaseAmount = dOSBaseAmount - crAllocBaseAmount - m_cRoundOffBaseAmount
            dNewOSCcyAmount = dOSCcyAmount - crAllocCCyAmount - m_cRoundOffAmount
            nFullyMatched = PMEReturnCode.PMTrue
        End If

        If CDbl(m_vTransData(ACTransDetailId, lRow)) = m_lAllocatedTransId Then
            If m_bIsCurrencyDifference Then
                crAllocBaseAmount = CDbl(m_vTransData(ACTransPayBaseAmount, lRow)) + m_cWriteOffBaseAmount
                crAllocCCyAmount = CDbl(m_vTransData(ACTransPayCurrencyAmount, lRow)) + oGainLossAmount
                crAllocAccountAmount = CDec(m_vTransData(kTransAccountAmount, lRow)) + m_cWriteOffBaseAmount
                crAllocSystemAmount = CDec(m_vTransData(kTransSystemAmount, lRow)) + m_cWriteOffBaseAmount
            Else
                If m_cRoundOffAmount <> 0 And CDbl(m_vRoundOffTransDetailId) <> 0 OrElse (lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) OrElse lWriteOffReasonId.Equals(KSAMBDXCalling)) Then
                    crAllocBaseAmount = CDec(m_vTransData(ACTransPayBaseAmount, lRow))
                    crAllocCCyAmount = CDec(m_vTransData(ACTransPayCurrencyAmount, lRow))
                    crAllocAccountAmount = CDec(m_vTransData(kTransAccountAmount, lRow))
                    crAllocSystemAmount = CDec(m_vTransData(kTransSystemAmount, lRow))
                Else
                    crAllocBaseAmount = CDbl(m_vTransData(ACTransPayBaseAmount, lRow)) + m_cWriteOffBaseAmount
                    crAllocCCyAmount = CDbl(m_vTransData(ACTransPayCurrencyAmount, lRow)) + oWriteOffAmount
                    crAllocAccountAmount = CDec(m_vTransData(kTransAccountAmount, lRow)) + m_cWriteOffBaseAmount
                    crAllocSystemAmount = CDec(m_vTransData(kTransSystemAmount, lRow)) + m_cWriteOffBaseAmount
                End If
            End If
        Else
            crAllocBaseAmount = CDec(m_vTransData(ACTransPayBaseAmount, lRow))
            crAllocCCyAmount = CDec(m_vTransData(ACTransPayCurrencyAmount, lRow))
            crAllocAccountAmount = CDec(m_vTransData(kTransAccountAmount, lRow))
            crAllocSystemAmount = CDec(m_vTransData(kTransSystemAmount, lRow))
        End If


        If oRoundOff_Amount = 0 Then
            If m_lCashListItemID <> 0 And (m_lAllocatedTransId <> CInt(m_vTransData(ACTransDetailId, lRow))) Then
                If CStr(m_vTransData(ACTransDocumentReference, lRow)).Substring(0, 3).ToLower() = "swd" Then
                    nResult = m_oAllocationDetail.DirectAdd(vAllocationId:=m_lAllocationId, vAllocationDetailID:=lAllocationDetailId,
                                                              vOriginalCurrency:=m_vTransData(ACTransCurrencyId, lRow),
                                                              vTransdetailID:=m_vTransData(ACTransDetailId, lRow),
                                                              vDocumenttypeID:=m_vTransData(ACTransDocumentTypeID, lRow),
                                                              vAccountingDate:=m_dtAllocationDate, vDocumentRef:=m_vTransData(ACTransDocumentReference, lRow),
                                                              vOriginalDate:=m_vTransData(ACTransDocumentDate, lRow),
                                                              vAllocateToBase:=nResult,
                                                              vOrigBaseAmount:=m_vTransData(ACTransBaseAmount, lRow),
                                                              vOrigBaseAmountUnrounded:=m_vTransData(ACTransBaseAmountUnrounded, lRow),
                                                              vOrigCcyAmount:=m_vTransData(ACTransCurrencyAmount, lRow),
                                                              vOrigCcyAmountUnrounded:=m_vTransData(ACTransCurrencyAmountUnrounded, lRow),
                                                              vOrigXrate:=m_vTransData(ACTransCurrencyBaseXrate, lRow),
                                                              vEffectiveXrate:=m_vTransData(ACTransCurrencyBaseXrate, lRow),
                                                              vOsBaseAmount:=dOSBaseAmount, vOsCcyAmount:=dOSCcyAmount,
                                                              vAllocBaseAmount:=crAllocBaseAmount, vAllocCcyAmount:=crAllocCCyAmount,
                                                              vFullyMatched:=nFullyMatched, vWriteOffReasonID:=0, vWriteOffAmount:=cWriteOffAmount,
                                                              vNewOsCcyAmount:=dNewOSCcyAmount, vNewOsBaseAmount:=dNewOSBaseAmount, vLossGainAmount:=0,
                                                              vIsPrimary:=m_vTransData(ACTransIsPrimary, lRow),
                                                          r_crAllocAccountAmount:=crAllocAccountAmount,
                                                         r_crAllocSystemAmount:=crAllocSystemAmount,
                                                          nTransdetailExId:=m_vTransData(kTransDetailExId, lRow))

                Else

                    nResult = m_oAllocationDetail.DirectAdd(vAllocationId:=m_lAllocationId, vCashlistitemID:=m_lCashListItemID, vAllocationDetailID:=lAllocationDetailId,
                                                              vOriginalCurrency:=m_vTransData(ACTransCurrencyId, lRow), vTransdetailID:=m_vTransData(ACTransDetailId, lRow), vDocumenttypeID:=m_vTransData(ACTransDocumentTypeID, lRow), vAccountingDate:=m_dtAllocationDate, vDocumentRef:=m_vTransData(ACTransDocumentReference, lRow), vOriginalDate:=m_vTransData(ACTransDocumentDate, lRow), vAllocateToBase:=nResult, vOrigBaseAmount:=m_vTransData(ACTransBaseAmount, lRow), vOrigBaseAmountUnrounded:=m_vTransData(ACTransBaseAmountUnrounded, lRow), vOrigCcyAmount:=m_vTransData(ACTransCurrencyAmount, lRow), vOrigCcyAmountUnrounded:=m_vTransData(ACTransCurrencyAmountUnrounded, lRow), vOrigXrate:=m_vTransData(ACTransCurrencyBaseXrate, lRow), vEffectiveXrate:=m_vTransData(ACTransCurrencyBaseXrate, lRow),
                                                              vOsBaseAmount:=dOSBaseAmount, vOsCcyAmount:=dOSCcyAmount, vAllocBaseAmount:=crAllocBaseAmount,
                                                              vAllocCcyAmount:=crAllocCCyAmount, vFullyMatched:=nFullyMatched, vWriteOffReasonID:=0,
                                                              vWriteOffAmount:=oWriteOffAmount, vNewOsCcyAmount:=dNewOSCcyAmount, vNewOsBaseAmount:=dNewOSBaseAmount,
                                                              vLossGainAmount:=0, vIsPrimary:=m_vTransData(ACTransIsPrimary, lRow),
                                                          r_crAllocAccountAmount:=crAllocAccountAmount,
                                                         r_crAllocSystemAmount:=crAllocSystemAmount,
                                                          nTransdetailExId:=m_vTransData(kTransDetailExId, lRow))
                End If

            Else

                ' This is the cash list item transaction
                'add the allocation detail record with OUT the cashlistitemid
                nResult = m_oAllocationDetail.DirectAdd(vAllocationId:=m_lAllocationId,
                                                          vAllocationDetailID:=lAllocationDetailId,
                                                          vOriginalCurrency:=m_vTransData(ACTransCurrencyId, lRow),
                                                          vTransdetailID:=m_vTransData(ACTransDetailId, lRow),
                                                          vDocumenttypeID:=m_vTransData(ACTransDocumentTypeID, lRow),
                                                          vAccountingDate:=m_dtAllocationDate,
                                                          vDocumentRef:=m_vTransData(ACTransDocumentReference, lRow),
                                                          vOriginalDate:=m_vTransData(ACTransDocumentDate, lRow),
                                                          vAllocateToBase:=nResult,
                                                          vOrigBaseAmount:=m_vTransData(ACTransBaseAmount, lRow),
                                                          vOrigBaseAmountUnrounded:=m_vTransData(ACTransBaseAmountUnrounded, lRow),
                                                          vOrigCcyAmount:=m_vTransData(ACTransCurrencyAmount, lRow),
                                                          vOrigCcyAmountUnrounded:=m_vTransData(ACTransCurrencyAmountUnrounded, lRow),
                                                          vOrigXrate:=m_vTransData(ACTransCurrencyBaseXrate, lRow),
                                                          vEffectiveXrate:=m_vTransData(ACTransCurrencyBaseXrate, lRow),
                                                          vOsBaseAmount:=dOSBaseAmount,
                                                          vOsCcyAmount:=dOSCcyAmount,
                                                          vAllocBaseAmount:=crAllocBaseAmount,
                                                          vAllocCcyAmount:=crAllocCCyAmount,
                                                          vFullyMatched:=nFullyMatched,
                                                          vWriteOffReasonID:=0,
                                                          vWriteOffAmount:=oWriteOffAmount,
                                                          vNewOsCcyAmount:=dNewOSCcyAmount,
                                                          vNewOsBaseAmount:=dNewOSBaseAmount,
                                                          vLossGainAmount:=oGainLossAmount,
                                                          vIsPrimary:=m_vTransData(ACTransIsPrimary, lRow),
                                                          r_crAllocAccountAmount:=crAllocAccountAmount,
                                                         r_crAllocSystemAmount:=crAllocSystemAmount,
                                                          nTransdetailExId:=m_vTransData(kTransDetailExId, lRow))
            End If

            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If
        End If

        If m_oCurrencyConvert Is Nothing Then

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            nResult = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If
        End If

        ' Get the Company's base currency
        nResult = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_lCompanyId, r_iBaseCurrencyID:=m_iBaseCurrency)
        If nResult <> PMEReturnCode.PMTrue Then
            Return nResult
        End If

        If (oWriteOffAmount <> 0 Or oGainLossAmount <> 0) AndAlso Not m_lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) Then

            If ToSafeLong(m_vTransData(kWriteOffReasonId, lRow)) > 0 And ToSafeDouble(m_vTransData(kWriteOffAmount, lRow)) <> 0 Then

                m_lReturn = WriteOff(v_lAllocationDetailId:=lAllocationDetailId,
                                        v_iCurrencyID:=m_iBaseCurrency,
                                        v_cBaseAmount:=oWriteOffAmount,
                                        v_lWriteOffReasonID:=nWriteOffReasonId,
                                        v_vAccountID:=m_vTransData(ACTransAccountID, lRow),
                                        v_vIsCurrencyDifference:=m_vTransData(kIsCurrencyDiff, lRow),
                                        v_vCompanyID:=m_vTransData(ACTransCompanyID, lRow),
                                        nTransdetailEx_Id:=ToSafeInteger(m_vTransData(kTransDetailExId, lRow)), nRow:=lRow)

            Else

                nResult = WriteOff(v_lAllocationDetailId:=lAllocationDetailId, v_iCurrencyID:=m_iBaseCurrency,
                              v_cBaseAmount:=m_cWriteOffBaseAmount * -1, v_lWriteOffReasonID:=nWriteOffReasonId,
                              v_vAccountID:=m_vTransData(ACTransAccountID, lRow),
                            v_vIsCurrencyDifference:=m_vTransData(kIsCurrencyDiff, lRow),
                              v_vCompanyID:=m_vTransData(ACTransCompanyID, lRow),
                              nTransdetailEx_Id:=0, nRow:=lRow)


            End If
        ElseIf m_lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) AndAlso m_vTransData(ACTransSpare, lRow) = "WRITEOFF" AndAlso m_vTransData(ACTransBaseAmount, lRow) <> 0 Then
            m_lReturn = WriteOff(v_lAllocationDetailId:=lAllocationDetailId,
                                        v_iCurrencyID:=m_iBaseCurrency,
                                        v_cBaseAmount:=m_vTransData(ACTransBaseAmount, lRow),
                                        v_lWriteOffReasonID:=nWriteOffReasonId,
                                        v_vAccountID:=m_vTransData(ACTransAccountID, lRow),
                                        v_vIsCurrencyDifference:=m_vTransData(kIsCurrencyDiff, lRow),
                                        v_vCompanyID:=m_vTransData(ACTransCompanyID, lRow),
                                        nTransdetailEx_Id:=ToSafeInteger(m_vTransData(kTransDetailExId, lRow)), nRow:=lRow)
        End If

        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFail
        End If
        If oRoundOff_Amount <> 0 Then
            If m_oCurrencyConvert Is Nothing Then

                m_oCurrencyConvert = New bACTCurrencyConvert.Form
                nResult = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If

            nResult = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_lCompanyId, r_iBaseCurrencyID:=m_iBaseCurrency)
            If nResult <> PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = RoundOff(v_lTransDetailId:=CInt(m_vRoundOffTransDetailId), v_iCurrencyID:=m_iBaseCurrency, v_cBaseAmount:=m_cRoundOffBaseAmount * -1, v_vAccountID:=m_vTransData(ACTransAccountID, lRow), v_vIsCurrencyDifference:=Nothing, v_vCompanyID:=m_vTransData(ACTransCompanyID, lRow))
        End If

        If nResult <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFail
        End If
        r_lFullyMatched = nFullyMatched

        Return nResult
    End Function

    Public Function CreateMatchGroup() As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of DocumentPost if needed
        If m_oMatchGroup Is Nothing Then

            'Developer Guide no 218

            m_oMatchGroup = New bACTMatchgroup.Form
            m_lReturn = m_oMatchGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If

        ' Set the period id from the date
        m_lReturn = GetPeriodIdForDate(r_lPeriodId:=m_lPeriodID, v_dtAccountingDate:=DateTime.Now, v_lSubBranchID:=m_lSubBranchID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Do a Direct add so we can get the ID


        m_lReturn = m_oMatchGroup.DirectAdd(vMatchId:=m_lMatchId, vPeriodID:=m_lPeriodID, vCompanyID:=m_lCompanyId, vMatchDate:=DateTime.Now)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Read back the added record into the collection

        m_lReturn = m_oMatchGroup.GetDetails(m_lMatchId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_oMatchGroup.Dispose()

        m_oMatchGroup = Nothing



        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateMatchGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateMatchGroup", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    Public Function CreateTransMatchForRoundOff(ByVal lTranDetailIdofRoundOff As Integer, ByVal lTransDetailId As Integer, ByVal cRoundOffAmount As Decimal) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("transdetailid_roundoff", CStr(lTranDetailIdofRoundOff), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("transdetailid_snd", CStr(lTransDetailId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("RoundOff_amount", CStr(cRoundOffAmount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
            m_lReturn = .SQLAction(sSQL:=ACUpdateRoundOffTransMatchSQL, sSQLName:=ACUpdateRoundOffTransMatchName, bStoredProcedure:=True)
        End With


        Return result
    End Function

    Public Function CreateTransMatch(ByVal lRow As Integer, ByVal lAllocationDetailId As Integer) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.101
        Dim vTransmatchId As Object = Nothing

        ' Get an instance of AllocationDetail if needed

        If m_oTransMatch Is Nothing Then
            'Object name should match with dll.name.

            m_oTransMatch = New bACTTransmatch.Form
            m_lReturn = m_oTransMatch.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If

        ' Set the process modes for the busines object.

        m_lReturn = m_oTransMatch.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

        ' Check for errors.
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set the process modes for the business object", vApp:=ACApp, vClass:="CreateTransMatch")
            Return result
        End If

        'The Write Off needs to be included in the Allocation to balance the total
        If CDbl(m_vTransData(ACTransDetailId, lRow)) = m_lAllocatedTransId Then
            If m_bIsCurrencyDifference Then
                m_lReturn = m_oTransMatch.DirectAdd(vTransmatchId:=vTransmatchId, vAllocationdetailID:=lAllocationDetailId, vTransDetailId:=m_vTransData(ACTransDetailId, lRow), vMatchId:=m_lMatchId, vCurrencyID:=m_vTransData(ACTransCurrencyId, lRow), vBaseMatchAmount:=CDbl(m_vTransData(ACTransPayBaseAmount, lRow)) + m_cWriteOffBaseAmount, vCurrencyMatchAmount:=CDbl(m_vTransData(ACTransPayCurrencyAmount, lRow)) + m_cWriteOffAmount, vCurrencyMatchXRate:=m_vTransData(ACTransCurrencyBaseXrate, lRow))
            Else
                If m_cRoundOffAmount <> 0 And CDbl(m_vRoundOffTransDetailId) <> 0 Then
                    m_lReturn = m_oTransMatch.DirectAdd(vTransmatchId:=vTransmatchId, vAllocationdetailID:=lAllocationDetailId, vTransDetailId:=m_vTransData(ACTransDetailId, lRow), vMatchId:=m_lMatchId, vCurrencyID:=m_vTransData(ACTransCurrencyId, lRow), vBaseMatchAmount:=m_vTransData(ACTransPayBaseAmount, lRow), vCurrencyMatchAmount:=m_vTransData(ACTransPayCurrencyAmount, lRow), vCurrencyMatchXRate:=m_vTransData(ACTransCurrencyBaseXrate, lRow))
                Else
                    If m_sCallingAppName = "CreditControlCLI" OrElse m_sCallingAppName = "iACTCreditControlProcessing" OrElse m_sCallingAppName = "bACTImportSiriusTrans" Then
                        m_lReturn = m_oTransMatch.DirectAdd(vTransmatchId:=vTransmatchId, vAllocationdetailID:=lAllocationDetailId, vTransDetailId:=m_vTransData(ACTransDetailId, lRow), _
                                                            vMatchId:=m_lMatchId, vCurrencyID:=m_vTransData(ACTransCurrencyId, lRow), _
                                                            vBaseMatchAmount:=CDbl(m_vTransData(ACTransPayBaseAmount, lRow)), _
                                                            vCurrencyMatchAmount:=CDbl(m_vTransData(ACTransPayCurrencyAmount, lRow)), _
                                                            vCurrencyMatchXRate:=m_vTransData(ACTransCurrencyBaseXrate, lRow))
                    Else
                        m_lReturn = m_oTransMatch.DirectAdd(vTransmatchId:=vTransmatchId, _
                        vAllocationdetailID:=lAllocationDetailId, _
                        vTransDetailId:=m_vTransData(ACTransDetailId, lRow), _
                        vMatchId:=m_lMatchId, _
                        vCurrencyID:=m_vTransData(ACTransCurrencyId, lRow), _
                        vBaseMatchAmount:=CDbl(m_vTransData(ACTransPayBaseAmount, lRow)) + m_cWriteOffBaseAmount, _
                        vCurrencyMatchAmount:=CDbl(m_vTransData(ACTransPayCurrencyAmount, lRow)) + m_cWriteOffAmount, _
                        vCurrencyMatchXRate:=m_vTransData(ACTransCurrencyBaseXrate, lRow))
                    End If
                End If
            End If
        Else

            m_lReturn = m_oTransMatch.DirectAdd(vTransmatchId:=vTransmatchId, vAllocationdetailID:=lAllocationDetailId, vTransDetailId:=m_vTransData(ACTransDetailId, lRow), vMatchId:=m_lMatchId, vCurrencyID:=m_vTransData(ACTransCurrencyId, lRow), vBaseMatchAmount:=m_vTransData(ACTransPayBaseAmount, lRow), vCurrencyMatchAmount:=m_vTransData(ACTransPayCurrencyAmount, lRow), vCurrencyMatchXRate:=m_vTransData(ACTransCurrencyBaseXrate, lRow))
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start the business object Match Post", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransMatch")
            Return result
        End If


        m_oTransMatch.Dispose()


        m_oTransMatch = Nothing

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateTransMatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateTransMatch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function


    ' RAW 01/04/2003 : ISS2854 : added
    ' ***************************************************************** '
    ' Name: UpdateTransdetail
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function UpdateTransdetail(ByVal v_lRow As Integer, ByVal v_lFullyMatched As Integer) As Integer

        Dim result As Integer = 0
        Const ksMyProcedureName As String = "UpdateTransdetail"
        Dim lMyReturn As gPMConstants.PMEReturnCode
        Dim lReturn As Integer



        lMyReturn = gPMConstants.PMEReturnCode.PMTrue


        ' Get an instance of Transaction Details
        If m_oTransDetail Is Nothing Then


            m_oTransDetail = New bACTTransDetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If


        ' get details for just the transdetail row concerned

        lReturn = m_oTransDetail.GetDetails(vTransDetailId:=m_vTransData(ACTransDetailId, v_lRow))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get transdetail details", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

            lMyReturn = gPMConstants.PMEReturnCode.PMFalse
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If


        ' update the transdetail properties

        lReturn = m_oTransDetail.EditUpdate(lRow:=1, vFullyMatched:=v_lFullyMatched)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update transdetail details", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

            lMyReturn = gPMConstants.PMEReturnCode.PMFalse
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If


        ' save details to database

        lReturn = m_oTransDetail.Update()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save transdetail to database", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

            lMyReturn = gPMConstants.PMEReturnCode.PMFalse
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If

        result = lMyReturn
        m_lReturn = lMyReturn
        Return result

    End Function

    ' RAW 01/04/2003 : ISS2854 : added
    ' ***************************************************************** '
    ' Name: UpdateCashListItem
    '
    ' Description:
    '
    ' ***************************************************************** '
    Private Function UpdateCashListItem(ByVal v_lRow As Integer, ByVal v_lAllocationStatusId As Integer) As Integer

        Dim result As Integer = 0
        Const ksMyProcedureName As String = "UpdateCashListItem"
        Dim lMyReturn As gPMConstants.PMEReturnCode
        Dim lReturn As Integer
        Dim sSQL As String = String.Empty
        Dim vResultArray As Object = Nothing



        lMyReturn = gPMConstants.PMEReturnCode.PMTrue
        If CInt(m_vTransData(ACTransDetailId, v_lRow)) <> 0 Then
            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CInt(m_vTransData(ACTransDetailId, v_lRow)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                sSQL = "select cashlistitem_id from cashlistitem where transdetail_id = {transdetail_id}"
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetMatchPayment", bStoredProcedure:=False, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If IsArray(vResultArray) Then
                    m_lCashListItemID = vResultArray(0, 0)
                Else
                    m_lCashListItemID = 0
                End If
            End With
        End If

        If m_lCashListItemID <> 0 Then

        
        Else
            ' this transaction is NOT for a cash list item

            ' get out of here now - note this is not an error
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If


        ' Get an instance of Transaction Details
        If m_oCashListItem Is Nothing Then
            'Developer Guide No 218
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oCashListItem, v_sClassName:="bACTCashlistitem.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' get details for just the cashlistitem row concerned

        lReturn = m_oCashListItem.GetDetails(vCashListItemId:=ToSafeInteger(m_lCashListItemID))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get cashlistitem details", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

            lMyReturn = gPMConstants.PMEReturnCode.PMFalse
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If


        ' update the cashlistitem properties
        Dim vCashListItem(gACTLibrary.eCashListItem.LastItem + 1) As Object

        vCashListItem(gACTLibrary.eCashListItem.AllocationstatusID) = v_lAllocationStatusId


        lReturn = m_oCashListItem.EditUpdate(lRow:=1, v_vCashListItem:=DirectCast(vCashListItem, Object()))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="failed to update cashlistitem details", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

            lMyReturn = gPMConstants.PMEReturnCode.PMFalse
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If


        ' save details to database

        lReturn = m_oCashListItem.Update()

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to save cashlistitem to database", vApp:=ACApp, vClass:=ACClass, vMethod:=ksMyProcedureName)

            lMyReturn = gPMConstants.PMEReturnCode.PMFalse
            result = lMyReturn
            m_lReturn = lMyReturn
            Return result
        End If

        result = lMyReturn
        m_lReturn = lMyReturn
        Return result

    End Function


    ' Get the details of a particular document into the class
    Private Function GetDocument(ByVal vDocumentID As Integer, ByRef vDocumentRef As Object, ByRef vDocumentTypeID As Object, ByRef vDocumentDate As Object) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        m_oDocument = New bACTDocument.Form
        m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse

        End If


        m_lReturn = m_oDocument.GetDetails(vDocumentID:=vDocumentID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDocument.GetDetails(vDocumentID:=vDocumentID)


        m_lReturn = m_oDocument.GetNext(vDocumentID:=vDocumentID, vDocumentRef:=vDocumentRef, vDocumentTypeID:=vDocumentTypeID, vDocumentDate:=vDocumentDate)



        m_oDocument.Dispose()


        Return result

    End Function
    'eck040101
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Private Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean


        result = gPMConstants.PMEReturnCode.PMTrue

        If m_oSystemOption Is Nothing Then

            ' Get Reference to Database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=bCloseDatabase, r_oCheckedDatabase:=m_oS4BDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Get Option Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Get Instance of System Option Business

            m_oSystemOption = New bSIROptions.Business
            m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oS4BDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End If



        m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to get the option", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        m_oSystemOption.Dispose()


        m_oSystemOption = Nothing
        m_lReturn = m_oS4BDatabase.CloseDatabase()

        m_oS4BDatabase = Nothing

        Return result

    End Function
    'eck040101
    ' ***************************************************************** '
    ' Name: Get ledger for Transaction(Private)
    '
    ' Description: Gets Account Type Debit Transaction
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetAccountType) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetAccountType(ByVal v_lAccountID As Integer, ByRef v_sAccountType As String) As Integer
    '
    'Dim result As Integer = 0
    'Dim oFields As ADODB.Fields
    'Dim sSQL As String = ""
    'Dim lRecordCount As Integer
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Try 
    'eck010801
    'm_oDatabase.Parameters.Clear()
    ''
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=CStr(v_lAccountID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'sSQL = ""
    'sSQL = "SELECT L.ledger_short_name  " & Strings.Chrw(13) & Strings.Chrw(10)
    'sSQL = sSQL & "FROM ledger L, " & Strings.Chrw(13) & Strings.Chrw(10)
    'sSQL = sSQL & "account A " & Strings.Chrw(13) & Strings.Chrw(10)
    'sSQL = sSQL & "WHERE L.ledger_id = A.ledger_id " & Strings.Chrw(13) & Strings.Chrw(10)
    'sSQL = sSQL & "AND A.account_id = {account_id}"
    '
    'With m_oDatabase
    'm_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="SelectAccountLedgerName", bStoredProcedure:=False)
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
    '
    'oFields = m_oDatabase.Records.Item(1).Fields()
    'With oFields
    'v_sAccountType = oFields("ledger_short_name").Trim()
    'End With
    'Select Case v_sAccountType
    'Case "SA", "UB"
    'v_sAccountType = ClientPayment
    'Case "IN"
    'v_sAccountType = InsurerSetted
    'Case Else
    'v_sAccountType = ""
    'End Select
    'End With
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAccountType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAccountType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    'eck040101
    ' ***************************************************************** '
    ' Name: PostCommission (Private)
    '
    ' Description: Transfer Commission to Earned Account.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PostCommission) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer
    '
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    'If m_oCommissionPost Is Nothing Then
    'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oCommissionPost, v_sClassName:="bACTCommissionPost.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'm_lReturn = m_oCommissionPost.PostCommission(v_sCommissionOption:=v_sCommissionOption, v_iCompanyID:=v_iCompanyID, v_lTransactionId:=v_lTransactionId)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to post the commission", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    'End If
    '

    'm_lReturn = m_oCommissionPost.Terminate()
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to terminate CommissionPost", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    'End If
    '
    'm_oCommissionPost = Nothing
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    Private Function GetPeriodIdForDate(ByRef r_lPeriodId As Integer, ByVal v_dtAccountingDate As Date, ByVal v_lSubBranchID As Integer) As Integer

        Dim result As Integer = 0
        Dim lPeriodID As Integer
        Dim oPeriod As bACTPeriod.Form




        result = gPMConstants.PMEReturnCode.PMTrue



        oPeriod = New bACTPeriod.Form
        m_lReturn = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse

        End If



        m_lReturn = oPeriod.GetPeriodForDate(dtDateInPeriod:=v_dtAccountingDate, lPeriodID:=lPeriodID, vSubBranchID:=v_lSubBranchID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        r_lPeriodId = lPeriodID


        oPeriod.Dispose()



        oPeriod = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: GetSymbolForCurrency
    '
    ' Description:
    '
    ' History: 01/10/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetSymbolForCurrency(ByVal v_lCurrencyID As Integer, ByRef r_sSymbol As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT symbol FROM Currency WHERE currency_id = " & _
                   v_lCurrencyID

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetSymbolForCurrency", bStoredProcedure:=False, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the result
            If Informations.IsArray(vResultArray) Then

                r_sSymbol = CStr(vResultArray(0, 0)).Trim()
            Else
                ' Default to GBP
                r_sSymbol = "£"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSymbolForCurrency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSymbolForCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' GetMatchPayment
    ''' </summary>
    ''' <param name="v_lTransdetailID"></param>
    ''' <param name="v_cBaseAmount"></param>
    ''' <param name="v_cCurrencyAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMatchPayment(ByVal v_lTransdetailID As Integer, ByRef v_cBaseAmount As Decimal, ByRef v_cCurrencyAmount As Decimal) As Integer

        Dim nResult As Integer
        Dim oResultArray(,) As Object = Nothing
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            v_cBaseAmount = 0
            v_cCurrencyAmount = 0

            With m_oDatabase

                .Parameters.Clear()
                ' Add transdetail_id
                nResult = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                nResult = .SQLSelect( _
                       sSQL:=kGetMatchPaymentSQL, _
                       sSQLName:=kGetMatchPaymentName, _
                       bStoredProcedure:=kGetMatchPaymentStored, _
                       vResultArray:=oResultArray)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("GetMatchPayment", "GetMatchPayment Failed ", Constants.vbObjectError)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With
            ' Get the result
            v_cBaseAmount = 0
            v_cCurrencyAmount = 0
            If Informations.IsArray(oResultArray) Then

                For iRow As Integer = oResultArray.GetLowerBound(1) To oResultArray.GetUpperBound(1)

                    If CStr(oResultArray(0, iRow)) > "" Then

                        v_cBaseAmount += CDec(oResultArray(1, iRow))

                        v_cCurrencyAmount += CDec(oResultArray(2, iRow))
                    End If
                Next iRow
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMatchPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMatchPayment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    ''' <summary>
    ''' WriteOff
    ''' </summary>
    ''' <param name="v_lAllocationDetailId"></param>
    ''' <param name="v_iCurrencyID"></param>
    ''' <param name="v_cBaseAmount"></param>
    ''' <param name="v_lWriteOffReasonID"></param>
    ''' <param name="v_vAccountID"></param>
    ''' <param name="v_vIsCurrencyDifference"></param>
    ''' <param name="v_vCompanyID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>

    Public Function WriteOff(ByVal v_lAllocationDetailId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseAmount As Decimal, _
                             ByVal v_lWriteOffReasonID As Integer, ByVal v_vAccountID As Object, _
                             ByVal v_vIsCurrencyDifference As Object, ByVal v_vCompanyID As Object) As Integer

        Return WriteOff(v_lAllocationDetailId:=v_lAllocationDetailId, v_iCurrencyID:=v_iCurrencyID, v_cBaseAmount:=v_cBaseAmount, _
                        v_lWriteOffReasonID:=v_lWriteOffReasonID, v_vAccountID:=v_vAccountID, v_vIsCurrencyDifference:=v_vIsCurrencyDifference, _
                        v_vCompanyID:=v_vCompanyID, nTransdetailEx_Id:=0, nRow:=-1)
    End Function

    Public Function WriteOff(ByVal v_lAllocationDetailId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseAmount As Decimal, _
                             ByVal v_lWriteOffReasonID As Integer, ByVal v_vAccountID As Object, _
                             ByVal v_vIsCurrencyDifference As Object, ByVal v_vCompanyID As Object, _
                             ByVal nTransdetailEx_Id As Integer, ByVal nRow As Integer) As Integer

        Dim nResult As Integer

        Dim nWOAccountId As Integer
        Dim nDocumentID As Integer
        Dim nTransdetailId As Integer
        Dim nAccountID As Integer
        Dim bIsCurrencyDifference As Boolean
        Dim nCompanyID As Integer

        Dim oPeriodID As Integer
        Dim oCurrencyID As Object = Nothing
        Dim nPeriodID As Integer
        Dim nLedgerID As Integer
        Dim sLedgerTypeCode As String = ""
        Dim crAmount As Decimal

        Dim oFullyMatched As Object = Nothing
        Dim oCurrencyAmount As Object = Nothing
        Dim oCurrencyBaseXRate As Object = Nothing

        Dim oComment As Object = Nothing
        Dim oInsuranceRef As Object = Nothing
        Dim nPurchaseOrderNo As Integer
        Dim nPurchaseInvoiceNo As Integer
        Dim nDepartment As Integer
        Dim oSpare As Object = Nothing

        Dim oRefAmount As Object = Nothing
        Dim oRefQuantity As Object
        Dim oRefUnits As Object = Nothing
        Dim oAccountingDate As Object

        Dim nWriteOffReasonId As Integer
        Dim oBaseAmountUnrounded As Object
        Dim oCurrencyAmountUnrounded As Object
        Dim oEuroCurrencyID As Object
        Dim oEuroAmount As Object
        Dim oEuroBaseXRate As Object
        Dim oEuroCcyXRate As Object
        Dim nSubBranchID As Integer

        Dim oAuditSetID As Object
        Dim oBatchID As Object
        Dim oDocumentRef As Object = Nothing
        Dim dtAuthorisedDate As Date
        Dim dtDocumentDate As Date
        Dim dtCreatedDate As Date
        Dim sRangeCode As String = ""
        Dim sReference As String = ""

        'Auto numbering
        Dim nNumberRangeID As Integer
        Dim oDocument As bACTDocument.Form = Nothing
        Dim oTransDetail As bACTTransdetail.Form = Nothing
        Dim oAutoNumber As bACTAutoNumber.Business = Nothing
        Dim oPeriod As bACTPeriod.Form = Nothing
        Dim oMatchPost As Object = Nothing
        Dim nTransdetailTypeID As Integer
        Dim nAllocationDetailId As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If Informations.IsNothing(v_vAccountID) Then
                nAccountID = m_lAccountId
            Else

                nAccountID = CInt(v_vAccountID)
            End If

            If Informations.IsNothing(v_vIsCurrencyDifference) Then
                bIsCurrencyDifference = m_bIsCurrencyDifference
            Else

                bIsCurrencyDifference = CBool(v_vIsCurrencyDifference)
            End If

            If Informations.IsNothing(v_vCompanyID) Then
                nCompanyID = m_lCompanyId
            Else

                nCompanyID = CInt(v_vCompanyID)
            End If

            If oDocument Is Nothing Then


                oDocument = New bACTDocument.Form
                nResult = oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If

            ' Create TransDetail object
            If oTransDetail Is Nothing Then

                oTransDetail = New bACTTransDetail.Form
                nResult = oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If

            ' Create AutoNumber object
            If oAutoNumber Is Nothing Then

                oAutoNumber = New bACTAutoNumber.Business
                nResult = oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If

            ' Create Period object
            If oPeriod Is Nothing Then


                oPeriod = New bACTPeriod.Form
                nResult = oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            End If

            ' Create Trans Match object
            If ((m_oAllocationDetail Is Nothing) = True) Then

                m_oAllocationDetail = New bACTAllocationDetail.Form
                nResult = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If (nResult <> gPMConstants.PMEReturnCode.PMTrue) Then
                    Return nResult
                End If

            End If

            ' Set the process modes for the busines object.
            m_lReturn = m_oAllocationDetail.SetProcessModes( _
                    vTask:=CObj(m_iTask), _
                    vNavigate:=CObj(m_lNavigate), _
                    vProcessMode:=CObj(m_lProcessMode), _
                    vTransactionType:=CObj(m_sTransactionType), _
                    vEffectiveDate:=CObj(m_dtEffectiveDate))

            ' Get the ledger for the account
            nResult = GetLedgerForAccount(v_lAccountID:=nAccountID, r_lLedgerID:=nLedgerID, r_sLedgerTypeCode:=sLedgerTypeCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Get the write off account for this ledger
            If Not m_lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) Then
                If Not bIsCurrencyDifference Then

                    nResult = GetWriteOffAccount(v_sLedgerTypeCode:=sLedgerTypeCode, r_lWOAccountID:=nWOAccountId)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Write Off Account for Ledger=" & sLedgerTypeCode & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                Else
                    nResult = GetExchangeDiffAccount(v_cCurrencyDifference:=v_cBaseAmount, r_lAccountID:=nWOAccountId)
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get Exchange Write Off Account for Amount=" & v_cBaseAmount & ".", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return nResult
                    End If
                End If
            End If

            'Get number for write off range
            If Not bIsCurrencyDifference Then

                nResult = oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef14, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSwd, r_lNumberRangeID:=nNumberRangeID)
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSwd
            Else

                nResult = oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef49, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeSCD, r_lNumberRangeID:=nNumberRangeID)
                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSCD
            End If
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Get the next number
            'Note:- GenerateNumber is related with  GenerateDocumentReferenceNumber
            nResult = oAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sReference, v_lNumberRangeID:=nNumberRangeID)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Generate a document
            If m_vTransData IsNot Nothing AndAlso nRow > -1 _
                AndAlso ToSafeInteger(m_vTransData(ACTransCurrencyId, nRow)) <> m_iBaseCurrency _
                AndAlso Not bIsCurrencyDifference Then
                oCurrencyID = ToSafeInteger(m_vTransData(ACTransCurrencyId, 0)) 'WriteOff Currency ID should be the same as the Transaction on which we are writting off the amount
            Else
                oCurrencyID = v_iCurrencyID
            End If

            nDocumentID = 0
            oAuditSetID = 0
            dtAuthorisedDate = DateTime.Now
            oBatchID = 0
            dtDocumentDate = DateTime.Now
            dtCreatedDate = DateTime.Now
            nWriteOffReasonId = v_lWriteOffReasonID

            If Not bIsCurrencyDifference Then
                oComment = "Write Off Document (Generated)"
                If sReference.Trim() <> "" Then
                    oDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeSwd & sReference
                End If
            Else
                oComment = "Currency exchange difference (Generated)"
                If sReference.Trim() <> "" Then
                    oDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeSCD & sReference
                End If

            End If

            nResult = oPeriod.GetPostingPeriodForDate(dtDateInPeriod:=dtDocumentDate, lPeriodID:=nPeriodID, lLedgerID:=nLedgerID)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Re-assign the period_id
            oPeriodID = nPeriodID

            nResult = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=nSubBranchID, v_vAccountID:=CStr(nAccountID))
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Add it...
            If Not bIsCurrencyDifference Then
                nResult = oDocument.DirectAdd(vDocumentID:=nDocumentID, vCompanyID:=nCompanyID, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, _
                                                vDocumenttypeID:=gACTLibrary.ACTDocTypeWriteOff, vAuditsetID:=oAuditSetID, vBatchID:=oBatchID, _
                                                vDocumentRef:=oDocumentRef, vDocumentDate:=dtDocumentDate, vCreatedDate:=dtCreatedDate, _
                                                vAuthorisedDate:=dtAuthorisedDate, vComment:=oComment, vWriteOffReasonID:=nWriteOffReasonId)
            Else
                nResult = oDocument.DirectAdd(vDocumentID:=nDocumentID, vCompanyID:=nCompanyID, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, _
                                                vDocumenttypeID:=gACTLibrary.ACTDocTypeCurrencyDifferenceCredit, vAuditsetID:=oAuditSetID, _
                                                vBatchID:=oBatchID, vDocumentRef:=oDocumentRef, vDocumentDate:=dtDocumentDate, vCreatedDate:=dtCreatedDate, _
                                                vAuthorisedDate:=dtAuthorisedDate, vComment:=oComment, vWriteOffReasonID:=nWriteOffReasonId)
            End If
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If m_lWriteOffReasonId.Equals(kSAMInsurerPaymentCalling) Then

                nResult = UpdateWriteOffDocument_Id(m_vTransData(ACTransDetailId, nRow), nDocumentID)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return nResult
                End If
            Else

                oAccountingDate = dtDocumentDate
                crAmount = v_cBaseAmount
                oBaseAmountUnrounded = v_cBaseAmount
                oCurrencyAmountUnrounded = v_cBaseAmount
                oFullyMatched = 1
                oCurrencyAmount = v_cBaseAmount
                oCurrencyBaseXRate = 1
                oRefAmount = 0
                oRefQuantity = 0
                oRefUnits = 0
                oEuroCurrencyID = 0
                oEuroAmount = 0
                oEuroBaseXRate = 1
                oEuroCcyXRate = 1

                If m_vTransData IsNot Nothing AndAlso nRow > -1 _
                    AndAlso ToSafeInteger(m_vTransData(ACTransCurrencyId, nRow)) <> m_iBaseCurrency _
                    AndAlso Not bIsCurrencyDifference Then
                    Dim crCurrencyAmount As Decimal

                    If m_oCurrencyConvert Is Nothing Then

                        m_oCurrencyConvert = New bACTCurrencyConvert.Form
                        nResult = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword,
                                                                iUserID:=m_iUserID, iSourceID:=m_iSourceID,
                                                                iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID,
                                                                iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                        If nResult <> PMEReturnCode.PMTrue Then
                            Return nResult
                        End If
                    End If
                    oCurrencyBaseXRate = m_vTransData(ACTransCurrencyBaseXrate, nRow) 'BaseXRate of the Transaction on which we are writting off the amount
                    m_oCurrencyConvert.ConvertBaseToCurrency(ToSafeInteger(m_vTransData(ACTransCurrencyId, nRow)), nCompanyID, v_cBaseAmount, crCurrencyAmount, Date.Today(), oCurrencyBaseXRate)
                    oCurrencyAmount = crCurrencyAmount
                    oCurrencyAmountUnrounded = crCurrencyAmount
                End If


                If Not bIsCurrencyDifference Then
                    oComment = "Write Off Transaction"
                    nResult = GetTransDetailTypeID("WRITEOFF", nTransdetailTypeID)
                Else
                    oComment = "Currency Exchange Profit/Loss"
                    nResult = GetTransDetailTypeID("GAINLOSS", nTransdetailTypeID)
                End If

                If nTransdetailTypeID = 0 Then
                    nTransdetailTypeID = 1
                End If

                ' Generate a transaction for the sales/purchase ledger
                nResult = oTransDetail.DirectAdd(vTransdetailID:=nTransdetailId, vAccountID:=nAccountID,
                                                   vPostingstatusID:=gACTLibrary.ACTPostStatusPosted, vCompanyID:=nCompanyID, vCurrencyID:=oCurrencyID,
                                                   vPeriodID:=oPeriodID, vDocumentID:=nDocumentID, vDocumentSequence:=1, vAccountingDate:=oAccountingDate,
                                                   vAmount:=crAmount, vBaseAmountUnrounded:=oBaseAmountUnrounded, vFullyMatched:=oFullyMatched,
                                                   vCurrencyAmount:=oCurrencyAmount, vCurrencyAmountUnrounded:=oCurrencyAmountUnrounded,
                                                   vCurrencyBaseXrate:=oCurrencyBaseXRate, vEuroCurrencyId:=oEuroCurrencyID, vEuroAmount:=oEuroAmount,
                                                   vEuroBaseXRate:=oEuroBaseXRate, vEuroCcyXrate:=oEuroCcyXRate, vComment:=oComment, vInsuranceRef:=oInsuranceRef,
                                                   vOperatorID:=m_iUserID, vPurchaseOrderNo:=nPurchaseOrderNo,
                                                   vPurchaseInvoiceNo:=nPurchaseInvoiceNo, vDepartment:=nDepartment, vSpare:=oSpare, vRefQuantity:=oRefQuantity,
                                                   vTransdetailTypeID:=nTransdetailTypeID)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If

                If Not bIsCurrencyDifference Then
                    nResult = m_oAllocationDetail.DirectAdd(
                            vAllocationId:=m_lAllocationId,
                            vAllocationDetailID:=v_lAllocationDetailId,
                            vOriginalCurrency:=oCurrencyID,
                            vTransdetailID:=nTransdetailId,
                            vDocumenttypeID:=gACTLibrary.ACTDocTypeWriteOff,
                            vAccountingDate:=DateTime.Now,
                            vDocumentRef:=oDocumentRef,
                            vOriginalDate:=DateTime.Now,
                            vAllocateToBase:=nResult,
                            vOrigBaseAmount:=crAmount,
                            vOrigBaseAmountUnrounded:=oBaseAmountUnrounded,
                            vOrigCcyAmount:=oCurrencyAmount,
                            vOrigCcyAmountUnrounded:=oCurrencyAmountUnrounded,
                            vOrigXrate:=oCurrencyBaseXRate,
                            vEffectiveXrate:=oCurrencyBaseXRate,
                            vOsBaseAmount:=crAmount,
                            vOsCcyAmount:=oCurrencyAmount,
                            vAllocBaseAmount:=v_cBaseAmount,
                            vAllocCcyAmount:=oCurrencyAmount,
                            vFullyMatched:=1,
                            vWriteOffReasonID:=nWriteOffReasonId, vWriteOffAmount:=crAmount,
                            vNewOsCcyAmount:=0,
                            vNewOsBaseAmount:=0,
                            vLossGainAmount:=0,
                            r_crAllocAccountAmount:=v_cBaseAmount,
                            r_crAllocSystemAmount:=v_cBaseAmount,
                            nTransdetailExId:=nTransdetailEx_Id,
                            vIsPrimary:=0)

                Else
                    nResult = m_oAllocationDetail.DirectAdd(
                            vAllocationId:=m_lAllocationId,
                            vAllocationDetailID:=v_lAllocationDetailId,
                            vOriginalCurrency:=oCurrencyID,
                            vTransdetailID:=nTransdetailId,
                            vDocumenttypeID:=gACTLibrary.ACTDocTypeCurrencyDifferenceCredit,
                            vAccountingDate:=DateTime.Now,
                            vDocumentRef:=oDocumentRef,
                            vOriginalDate:=DateTime.Now,
                            vAllocateToBase:=nResult,
                            vOrigBaseAmount:=crAmount,
                            vOrigBaseAmountUnrounded:=oBaseAmountUnrounded,
                            vOrigCcyAmount:=oCurrencyAmount,
                            vOrigCcyAmountUnrounded:=oCurrencyAmountUnrounded,
                            vOrigXrate:=oCurrencyBaseXRate,
                            vEffectiveXrate:=oCurrencyBaseXRate,
                            vOsBaseAmount:=crAmount,
                            vOsCcyAmount:=oCurrencyAmount,
                            vAllocBaseAmount:=oCurrencyAmount,
                            vAllocCcyAmount:=oCurrencyAmount,
                            vFullyMatched:=1,
                            vWriteOffReasonID:=nWriteOffReasonId, vWriteOffAmount:=0,
                            vNewOsCcyAmount:=0,
                            vNewOsBaseAmount:=0,
                            vLossGainAmount:=crAmount,
                            r_crAllocAccountAmount:=oCurrencyAmount,
                            r_crAllocSystemAmount:=oCurrencyAmount,
                            nTransdetailExId:=nTransdetailEx_Id,
                            vIsPrimary:=0)

                End If
                ' Generate a transaction for the nominal ledger write off account
                crAmount = v_cBaseAmount * -1
                oCurrencyAmount = oCurrencyAmount * -1
                oBaseAmountUnrounded = v_cBaseAmount * -1
                oCurrencyAmountUnrounded = oCurrencyAmountUnrounded * -1
                oEuroAmount = 0
                If Not bIsCurrencyDifference Then
                    oComment = "Matching Write Off Transaction"
                Else
                    oComment = "Matching Currency Exchange Transaction"
                End If

                'This side is not fully matched

                nResult = oTransDetail.DirectAdd(vTransdetailID:=nTransdetailId, vAccountID:=nWOAccountId, vPostingstatusID:=gACTLibrary.ACTPostStatusPosted,
                                                   vCompanyID:=nCompanyID, vCurrencyID:=oCurrencyID, vPeriodID:=oPeriodID, vDocumentID:=nDocumentID, vDocumentSequence:=2,
                                                   vAccountingDate:=oAccountingDate, vAmount:=crAmount, vBaseAmountUnrounded:=oBaseAmountUnrounded, vFullyMatched:=0,
                                                   vCurrencyAmount:=oCurrencyAmount, vCurrencyAmountUnrounded:=oCurrencyAmountUnrounded, vCurrencyBaseXrate:=oCurrencyBaseXRate,
                                                   vEuroCurrencyId:=oEuroCurrencyID, vEuroAmount:=oEuroAmount, vEuroBaseXRate:=oEuroBaseXRate, vEuroCcyXrate:=oEuroCcyXRate,
                                                   vComment:=oComment, vInsuranceRef:=oInsuranceRef, vOperatorID:=m_iUserID, vPurchaseOrderNo:=nPurchaseOrderNo,
                                                   vPurchaseInvoiceNo:=nPurchaseInvoiceNo, vDepartment:=nDepartment, vSpare:=oSpare, vRefQuantity:=oRefQuantity,
                                                   vTransdetailTypeID:=nTransdetailTypeID)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add transaction.", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return nResult
                End If
                ' Update the allocation detail table to show the write off
                If Not m_oAllocationDetail Is Nothing Then
                    If Not bIsCurrencyDifference Then
                        nResult = m_oAllocationDetail.SetWriteOff(
                            v_lAllocationDetailID:=nAllocationDetailId,
                            v_cWriteOffAmount:=crAmount,
                            v_lWriteOffReasonID:=nWriteOffReasonId)
                    Else
                        nResult = m_oAllocationDetail.SetLossGain(
                            v_lAllocationDetailID:=nAllocationDetailId,
                            v_cLossGainAmount:=crAmount)
                    End If
                    If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return nResult
                    End If
                End If
            End If

            oPeriod.Dispose()
            oPeriod = Nothing

            oAutoNumber.Dispose()
            oAutoNumber = Nothing

            oDocument.Dispose()
            oDocument = Nothing

            oTransDetail.Dispose()
            oTransDetail = Nothing

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    '***************************
    '   Round Off trans match
    '*****************************************
    Public Function RoundOff(ByVal v_lTransDetailId As Integer, ByVal v_iCurrencyID As Integer, ByVal v_cBaseAmount As Decimal, ByVal v_vAccountID As Object, ByVal v_vIsCurrencyDifference As Object, ByVal v_vCompanyID As Object) As Integer

        'Variables
        Dim result As Integer = 0


        'Parameters
        Dim lAccountID As Integer
        Dim bIsCurrencyDifference As Boolean
        Dim lCompanyID As Integer

        'Transdetail Parameters

        Dim sLedgerTypeCode As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_NOTE: (1021) IsMissing() was changed to Informations. Informations. Informations. Informations. IsNothing(). More Information: http://www.vbtonet.com/ewis/ewi1021.aspx
            If Informations.IsNothing(v_vAccountID) Then
                lAccountID = m_lAccountId
            Else
                'UPGRADE_WARNING: (1068) v_vAccountID of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lAccountID = CInt(v_vAccountID)
            End If

            'UPGRADE_NOTE: (1021) IsMissing() was changed to Informations. Informations. Informations. Informations. IsNothing(). More Information: http://www.vbtonet.com/ewis/ewi1021.aspx
            If Informations.IsNothing(v_vIsCurrencyDifference) Then
                bIsCurrencyDifference = m_bIsCurrencyDifference
            Else
                'UPGRADE_WARNING: (1068) v_vIsCurrencyDifference of type Variant is being forced to Boolean. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                bIsCurrencyDifference = CBool(v_vIsCurrencyDifference)
            End If

            'UPGRADE_NOTE: (1021) IsMissing() was changed to Informations. Informations. Informations. Informations. IsNothing(). More Information: http://www.vbtonet.com/ewis/ewi1021.aspx
            If Informations.IsNothing(v_vCompanyID) Then
                lCompanyID = m_lCompanyId
            Else
                'UPGRADE_WARNING: (1068) v_vCompanyID of type Variant is being forced to Integer. More Information: http://www.vbtonet.com/ewis/ewi1068.aspx
                lCompanyID = CInt(v_vCompanyID)
            End If


            ' match the transaction

            ' Update the allocation detail table to show the write off
            If Not (m_oAllocationDetail Is Nothing) Then
                If Not bIsCurrencyDifference Then
                    'UPGRADE_TODO: (1067) Member SetroundOff is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                    m_lReturn = m_oAllocationDetail.SetroundOff(v_lTransDetailId:=v_lTransDetailId, v_cRoundOffAmount:=v_cBaseAmount)
                    '        Else
                    '            m_lReturn = m_oAllocationDetail.SetLossGain( _
                    ''                    v_lAllocationDetailId:=v_lAllocationDetailId, _
                    ''                    v_cLossGainAmount:=cAmount)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="roundOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="roundOff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetEuroCurrencyID
    '
    ' Description: Gets the currency id for the euro
    '
    ' ***************************************************************** '
    Private Function GetEuroCurrencyID(ByRef r_lCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Dim oCurrency As bACTCurrency.Form = Nothing
        Dim iCurrencyID As Integer

        Const EURO_ISO As String = "EUR"



        result = gPMConstants.PMEReturnCode.PMTrue


        If oCurrency Is Nothing Then


            oCurrency = New bACTCurrency.Form
            m_lReturn = oCurrency.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' Get the code for Euro's

        m_lReturn = oCurrency.GetCurrencyIdFromISO(v_sISOCode:=EURO_ISO, r_iCurrencyID:=iCurrencyID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Return it...
        r_lCurrencyID = iCurrencyID

        ' Terminate the object

        oCurrency.Dispose()
        oCurrency = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetLedgerForAccount
    '
    ' Description: Gets the ledger_id and ledger type code
    ' for the passed account.
    '
    ' DD 03/07/2003: Rewritten as it will not work in a
    ' multi-branch set-up where Ledgers are duplicated per sub-branch
    ' ***************************************************************** '
    Private Function GetLedgerForAccount(ByVal v_lAccountID As Integer, ByRef r_lLedgerID As Integer, ByRef r_sLedgerTypeCode As String) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("account_id", CStr(v_lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'developer guide no.85
            .Parameters.Add("ledger_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'developer guide no.85
            .Parameters.Add("ledgertype_code", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMString)
            'developer guide no.39
            m_lReturn = .SQLSelect("spu_ACT_Get_LedgerType_Code", "Get Ledger Type Code for Account", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call spu_ACT_Get_Ledger_Code", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLedgerForAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If
            'developer guide no.260
            r_lLedgerID = CInt(.Parameters.Item("ledger_id").Value.ToString().Trim())
            r_sLedgerTypeCode = .Parameters.Item("ledgertype_code").Value.Trim()
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetWriteOffAccount
    '
    ' Description: Gets the write off account_id depending on sales
    '              or purchase ledger.
    '
    ' DD 03/07/2003: Uses LedgerTypeCode instead of ID
    '
    ' ***************************************************************** '
    Public Function GetWriteOffAccount(ByVal v_sLedgerTypeCode As String, ByRef r_lWOAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""
        Dim oAllocation As bACTAllocation.Form = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue


        Select Case v_sLedgerTypeCode
            ' Sales ledger - Client and sub agent and Agent accounts
            Case "D", "B", "A", "I"

                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffDebtorAccount, r_sOptionValue:=sShortCode)

                ' Purchase ledger - Purchase accounts, Commission agent account
            Case "C", "O"

                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffCrebitorAccount, r_sOptionValue:=sShortCode)

                'Insurer Payment
            Case "IP"

                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffInterMediateAccount, r_sOptionValue:=sShortCode)

                ' Another ledger
            Case Else

                result = gPMConstants.PMEReturnCode.PMError
                m_bValidWriteOffAccount = True
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot write off on this ledger : " & v_sLedgerTypeCode, vApp:=ACApp, vClass:=ACClass, vMethod:="GetWriteOffAccount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result

        End Select
        If oAllocation Is Nothing Then



            oAllocation = New bACTAllocation.Form
            m_lReturn = oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' Get the account_id from the business

        m_lReturn = oAllocation.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=r_lWOAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        oAllocation.Dispose()
        oAllocation = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    ' Name: GetExchangeDiffAccount
    '
    ' Description: Gets the account_id to post debit or credit currecny exchange differences
    '                   depends if we have a debit/credit
    '                   and also uses the company_id to pick the correct one
    '
    ' History: 1/10/2003
    '                   PN 7129 created for use in recognising currency diferences and
    '                   posting to the correct account.
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetExchangeDiffAccount(ByVal v_cCurrencyDifference As Object, ByRef r_lAccountID As Integer) As Integer

        Dim result As Integer = 0
        Dim sShortCode As String = ""
        Dim oAllocation As bACTAllocation.Form = Nothing
        Dim lWOAccountId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_cCurrencyDifference > 0 Then
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACCurrencyDifferenceDebitAccount, r_sOptionValue:=sShortCode)
            Else
                m_lReturn = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACCurrencyDifferenceCrebitAccount, r_sOptionValue:=sShortCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or sShortCode = "" Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAllocation Is Nothing Then

                oAllocation = New bACTAllocation.Form
                m_lReturn = oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Get the account_id from the business
            ' Allocation caters for the company_id where appropriate

            m_lReturn = oAllocation.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=lWOAccountId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            r_lAccountID = lWOAccountId

            oAllocation.Dispose()
            oAllocation = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExchangeDiffAccount failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExchangeDiffAccount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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
    ''' <summary>
    ''' CreateAllocationBatch
    ''' </summary>
    ''' <param name="v_nReversedAllocationBatchID"></param>
    ''' <param name="r_nAllocationBatchID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAllocationBatch(ByVal v_nReversedAllocationBatchID As Integer, _
            ByRef r_nAllocationBatchID As Integer, ByVal v_dtAllocationDate As Object) As Integer

        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("nAllocation_batch_id", CStr(r_nAllocationBatchID), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            If ToSafeLong(v_nReversedAllocationBatchID) > 0 Then
                .Parameters.Add("nReversed_allocation_batch_id", CStr(v_nReversedAllocationBatchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            .Parameters.Add("allocationbatch_date", ToSafeDate(v_dtAllocationDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            nResult = .SQLAction( _
                                kAddAllocationBatchSQL, _
                                    kAddAllocationBatchName, _
                                       kAddAllocationBatchStored)
            If nResult = gPMConstants.PMEReturnCode.PMTrue Then
                r_nAllocationBatchID = m_oDatabase.Parameters.Item("nAllocation_batch_id").Value
            End If
        End With

        Return nResult


    End Function
    ''' <summary>
    ''' GetTransDetailTypeID
    ''' </summary>
    ''' <param name="sTransdetailTypeCode"></param>
    ''' <param name="nTransdetailTypeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTransDetailTypeID(ByVal sTransdetailTypeCode As String, _
            ByRef nTransdetailTypeID As Integer) As Integer

        Dim oTransDetailTypeId As Object(,) = Nothing
        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue
        With m_oDatabase

            .Parameters.Clear()

            nResult = .Parameters.Add( _
                    sName:="code", _
                    vValue:=sTransdetailTypeCode, _
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, _
                    iDataType:=gPMConstants.PMEDataType.PMString)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetTransDetailTypeID", "GetTransDetailTypeID Failed to get TransdetailTypeID - bACTImportSiriusTrans.Form", Constants.vbObjectError)
            End If

            nResult = .SQLSelect( _
                    sSQL:=kGetTransDetailTypeIDSQL, _
                    sSQLName:=kGetTransDetailTypeIDName, _
                    bStoredProcedure:=kGetTransDetailTypeIDStored, _
                    vResultArray:=oTransDetailTypeId)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("GetTransDetailTypeID", "GetTransDetailTypeID Failed to get TransdetailTypeID - bACTImportSiriusTrans.Form", ObjectError)
            End If
        End With

        If Informations.IsArray(oTransDetailTypeId) Then
            nTransdetailTypeID = NullToLong(oTransDetailTypeId(0, 0))
        Else
            nTransdetailTypeID = 1 'Default to Journal
        End If

        Return nResult

    End Function

    ''' <summary>
    ''' GetWriteOffDiffAccount for Instalment Write Off
    ''' </summary>
    ''' <param name="crCurrencyDifference"></param>
    ''' <param name="r_nAccountID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWriteOffDiffAccount(ByVal crCurrencyDifference As Decimal, ByRef r_nAccountID As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sShortCode As String = String.Empty
        Dim oAllocation As bACTAllocation.Form = Nothing
        Dim nWOAccountId As Integer

        Try

            If crCurrencyDifference > 0 Then
                nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffDebtorAccount, r_sOptionValue:=sShortCode)
            Else
                nResult = bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=ACWriteOffCrebitorAccount, r_sOptionValue:=sShortCode)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Or String.IsNullOrEmpty(sShortCode) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oAllocation Is Nothing Then

                oAllocation = New bACTAllocation.Form
                nResult = oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            nResult = oAllocation.GetAccountID(v_sShortCode:=sShortCode, r_lAccountID:=nWOAccountId)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            r_nAccountID = nWOAccountId

            oAllocation.Dispose()
            oAllocation = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetWriteOffDiffAccount failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetWriteOffDiffAccount", excep:=excep)

            Return nResult

        End Try
    End Function

    Public Function UpdateWriteOffDocument_Id(ByVal lTransDetailId As Integer, ByVal lDocumentId As Integer) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("nTransdetail_id", CStr(lTransDetailId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("nDocument_id", CStr(lDocumentId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                nResult = .SQLAction(sSQL:=ACUpdateWriteOffDocumentIdSQL, sSQLName:=ACUpdateWriteOffDocumentIdName, bStoredProcedure:=True)

                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return nResult

        Catch ex As Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="spu_update_writeoff_documentId", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateWriteOffDocument_Id", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return nResult
        End Try
    End Function
End Class
