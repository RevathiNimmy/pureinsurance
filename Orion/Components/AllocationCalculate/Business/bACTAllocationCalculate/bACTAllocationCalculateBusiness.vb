Option Strict Off
Option Explicit On
'developer guide no 129
Imports SSP.Shared
Friend NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    ' Date: 03/12/1997
    '
    ' Description: Batch allocate methods
    '
    '
    '
    ' Edit History:
    ' RAW 12/3/2003 : ISS2893 : corrected calculate of RecordValues
    '                           handle write off amounts
    ' ***************************************************************** '


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

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    ' Task
    Private m_iTask As Integer
    ' Navigate
    Private m_lNavigate As Integer
    ' Process Mode
    Private m_lProcessMode As Integer
    ' Type of Business
    Private m_sTransactionType As String = ""
    ' Effective
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    ' Source ID

    Private m_lCashListId As Integer
    Private m_lAllocationId As Integer

    Private m_bAbortTrans As Boolean

    Private m_oAllocation As Object
    Private m_oAllocationDetail As Object
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oCashList As Object
    Private m_oCashListItem As Object
    Private m_oTransDetail As Object
    Private m_oCurrency As bACTCurrency.Form


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property AllocationId() As Integer
        Get
            Return m_lAllocationId
        End Get
        Set(ByVal Value As Integer)
            m_lAllocationId = Value
        End Set
    End Property

    Public Property CashListId() As Integer
        Get
            Return m_lCashListId
        End Get
        Set(ByVal Value As Integer)
            m_lCashListId = Value
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserId
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyId
            m_iLogLevel = iLogLevel

            ' CF170999
            ' New instance of Component Services

            'Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bAbortTrans = True

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocation, v_sClassName:="bACTAllocation.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationDetail, v_sClassName:="bACTAllocationDetail.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oCashList = GetOrionBusiness(v_sClassName:="bACTCashList.Form", v_vDatabase:=m_oDatabase)
            'TODO:
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oCashList, v_sClassName:="bACTCashList.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oCashListItem = GetOrionBusiness(v_sClassName:="bACTCashListItem.Form", v_vDatabase:=m_oDatabase)
            'TODO:
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oCashListItem, v_sClassName:="bACTCashListItem.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oCurrencyConvert = GetOrionBusiness(v_sClassName:="bACTCurrencyConvert.Form", v_vDatabase:=m_oDatabase)

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'TODO:
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oTransDetail, v_sClassName:="bACTTransDetail.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oCurrency = New bACTCurrency.Form
            m_lReturn = m_oCurrency.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Remove instance of component services

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
    ' Name: Terminate
    '
    ' Description: Standard terminate function
    '
    ' History: 25/02/2000 CTAF - Created.
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
                If m_oAllocation IsNot Nothing Then
                    m_oAllocation.Dispose()
                    m_oAllocation = Nothing
                End If
                If m_oAllocationDetail IsNot Nothing Then
                    m_oAllocationDetail.Dispose()
                    m_oAllocationDetail = Nothing
                End If
                If m_oCashList IsNot Nothing Then
                    m_oCashList.Dispose()
                    m_oCashList = Nothing
                End If
                If m_oCashListItem IsNot Nothing Then
                    m_oCashListItem.Dispose()
                    m_oCashListItem = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oCurrency IsNot Nothing Then
                    m_oCurrency.Dispose()
                    m_oCurrency = Nothing
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
    ' Name: CalculateRecordValues
    '
    ' Description: Public function to calculate the os amount and loss/gain
    '              of a single allocation detail.
    '
    ' ***************************************************************** '
    Public Function CalculateRecordValues(ByVal v_iOriginalCurrency As Integer, ByVal v_lCompanyID As Integer, ByVal v_bAllocateToBase As Boolean, ByVal v_vdOrigXrate As Object, ByVal v_vdEffectiveXrate As Object, ByVal v_cOsBaseAmount As Decimal, ByVal v_cOsCcyAmount As Decimal, ByRef r_cAllocBaseAmount As Decimal, ByRef r_cAllocCcyAmount As Decimal, ByRef r_cNewOsCcyAmount As Decimal, ByRef r_cNewOsBaseAmount As Decimal, ByRef r_cLossGainAmount As Decimal, ByRef r_bFullyMatched As Boolean, Optional ByRef r_vdAllocBaseAmountUnrounded As Decimal = Nothing, Optional ByRef r_cWriteOffBaseAmount As Decimal = 0, Optional ByRef r_cWriteOffCcyAmount As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim iOriginalCurrency As Integer
        Dim bAllocateToBase As Boolean
        Dim vdOrigXrate, vdEffectiveXrate As Object
        Dim cOsBaseAmount, cOsCcyAmount, cAllocBaseAmount, cAllocCcyAmount, cWriteOffBaseAmount, cWriteOffCcyAmount, cNewOsCcyAmount, cNewOsBaseAmount As Decimal
        Dim bFullyMatched As Boolean
        Dim cLossGainAmount, cAllocBaseAmountAtCurrentRate As Decimal
        Dim vdBaseRoundingDifference As Object = Nothing
        Dim vdAllocBaseAmountUnrounded As Decimal
        Dim vdLossGainAmount As Double
        Dim iBaseDecimalPlaces As gPMConstants.PMEVDecimalNoOfDP
        Dim iBaseCurrencyID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Input parameters
            iOriginalCurrency = v_iOriginalCurrency
            bAllocateToBase = v_bAllocateToBase


            vdOrigXrate = v_vdOrigXrate


            vdEffectiveXrate = v_vdEffectiveXrate
            cOsBaseAmount = v_cOsBaseAmount
            cOsCcyAmount = v_cOsCcyAmount

            ' Returned values
            cAllocBaseAmount = r_cAllocBaseAmount
            cAllocCcyAmount = r_cAllocCcyAmount
            cWriteOffBaseAmount = r_cWriteOffBaseAmount
            cWriteOffCcyAmount = r_cWriteOffCcyAmount
            cNewOsCcyAmount = r_cNewOsCcyAmount
            cNewOsBaseAmount = r_cNewOsBaseAmount
            bFullyMatched = r_bFullyMatched
            vdAllocBaseAmountUnrounded = r_vdAllocBaseAmountUnrounded

            ' If already fully matched then take no action

            If bFullyMatched Then
                Return result
            End If


            ' Determine if allocating to base or to currency
            'Allocating To Currency

            If Not bAllocateToBase Then
                If cAllocCcyAmount <> 0 Then

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=iOriginalCurrency, lCompanyID:=v_lCompanyID, cBaseAmount:=cAllocBaseAmount, cCurrencyAmount:=cAllocCcyAmount, vConversionDate:=Nothing, vBaseRoundingDifference:=vdBaseRoundingDifference, vConversionRate:=vdOrigXrate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                vdAllocBaseAmountUnrounded = cAllocBaseAmount

                If cWriteOffCcyAmount <> 0 Then

                    m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=iOriginalCurrency, lCompanyID:=v_lCompanyID, cBaseAmount:=cWriteOffBaseAmount, cCurrencyAmount:=cWriteOffCcyAmount, vConversionDate:=Nothing, vBaseRoundingDifference:=vdBaseRoundingDifference, vConversionRate:=vdOrigXrate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Else
                ' Allocate to base

                If cAllocBaseAmount <> 0 Then
                    ' Calc ccy equiv of allocated amount at Original rate

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iOriginalCurrency, lCompanyID:=v_lCompanyID, cBaseAmount:=cAllocBaseAmount, cCurrencyAmount:=cAllocCcyAmount, vConversionDate:=Nothing, vConversionRate:=vdOrigXrate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                vdAllocBaseAmountUnrounded = cAllocBaseAmount 'not really unrounded here because the base figure is as entered not calculated

                ' RAW 12/3/2003 : ISS2893 : added
                If cWriteOffBaseAmount <> 0 Then

                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iOriginalCurrency, lCompanyID:=v_lCompanyID, cBaseAmount:=cWriteOffBaseAmount, cCurrencyAmount:=cWriteOffCcyAmount, vConversionDate:=Nothing, vConversionRate:=vdOrigXrate)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            End If

            ' New outstanding amount

            ' RAW 12/03/2003 : ISS2893 : added write off amounts

            'SMJB: CQ969 01/08/03
            If cAllocCcyAmount < 0 Then
                cNewOsCcyAmount = cOsCcyAmount - cAllocCcyAmount - cWriteOffBaseAmount
            ElseIf cAllocCcyAmount > 0 Then  'Otherwise it will be positive so add it
                cNewOsCcyAmount = Math.Abs(cOsCcyAmount) - cAllocCcyAmount - cWriteOffBaseAmount
            End If

            If cAllocBaseAmount < 0 Then
                cNewOsBaseAmount = cOsBaseAmount - cAllocBaseAmount - cWriteOffCcyAmount
            ElseIf cAllocBaseAmount > 0 Then
                cNewOsBaseAmount = Math.Abs(cOsBaseAmount) - cAllocBaseAmount - cWriteOffCcyAmount
            End If

            ' RAW 12/3/2003 : ISS2893 : added
            bFullyMatched = (cNewOsBaseAmount = 0)

            ' Calculate loss or gain from base equiv of allocated ccy at Current rate

            m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=iOriginalCurrency, lCompanyID:=v_lCompanyID, cBaseAmount:=cAllocBaseAmountAtCurrentRate, cCurrencyAmount:=cAllocCcyAmount, vConversionDate:=Nothing, vBaseRoundingDifference:=vdBaseRoundingDifference, vConversionRate:=vdEffectiveXrate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Gain/ Loss = Base at original - Base at Current  (Neg amounts are gains here)
            'If Allocation made to Base No Gain or Loss is recorded


            If Not vdEffectiveXrate.Equals(vdOrigXrate) Then
                vdLossGainAmount = vdAllocBaseAmountUnrounded - cAllocBaseAmountAtCurrentRate
            End If


            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=v_lCompanyID, r_iBaseCurrencyID:=iBaseCurrencyID)

            m_lReturn = m_oCurrency.GetDetails(vCurrencyID:=iBaseCurrencyID)

            m_lReturn = m_oCurrency.GetNext(vDecimalPlaces:=iBaseDecimalPlaces)

            cLossGainAmount = gPMMaths.PMRoundupValueVDecimal(vdLossGainAmount, iBaseDecimalPlaces, gPMConstants.PMERoundupFactor.pmeRFactor50Up)

            'EK 100100 Reverse the sign to reflect Brokers position
            cLossGainAmount *= -1
            ' Return values

            r_cAllocBaseAmount = cAllocBaseAmount
            r_cAllocCcyAmount = cAllocCcyAmount

            r_cWriteOffBaseAmount = cWriteOffBaseAmount
            r_cWriteOffCcyAmount = cWriteOffCcyAmount

            r_cNewOsCcyAmount = cNewOsCcyAmount

            r_cNewOsBaseAmount = cNewOsBaseAmount
            r_cLossGainAmount = cLossGainAmount
            r_bFullyMatched = bFullyMatched
            r_vdAllocBaseAmountUnrounded = vdAllocBaseAmountUnrounded

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateRecordValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateRecordValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CalculateAllocationSet
    '
    ' Description: Calculate the os amount and loss/gain for a whole set
    '
    ' ***************************************************************** '
    Public Function CalculateAllocationSet(ByRef r_cTotalBaseAmount As Decimal, ByRef r_cTotalCcyAmount As Decimal, ByRef r_iCurrencyId As Integer, ByRef r_bSameCurrency As Boolean) As Integer

        Dim result As Integer = 0
        Dim cTotalBaseAmount, cTotalCcyAmount As Decimal
        Dim iCurrencyId As Integer
        Dim lCompanyID As Integer
        Dim cAllocBaseAmount, cAllocCcyAmount, cNewOsCcyAmount, cNewOsBaseAmount As Decimal
        Dim bFullyMatched As Boolean
        Dim bSameCurrency As Boolean
        Dim cLossGainAmount As Decimal


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' The allocation set

            m_lReturn = GetAllocation(v_lAllocationId:=m_lAllocationId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            If m_oAllocationDetail.Details.Count < 1 Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            iCurrencyId = m_oAllocationDetail.Details.Item(1).OriginalCurrency

            bSameCurrency = True
            cTotalBaseAmount = 0
            cTotalCcyAmount = 0

            ' Sequence through the set calculating and accumulating


            For lRow As Integer = 1 To m_oAllocationDetail.Details.Count

                With m_oAllocationDetail.Details.Item(CInt(lRow))


                    cAllocBaseAmount = .AllocBaseAmount

                    cAllocCcyAmount = .AllocCcyAmount

                    cNewOsCcyAmount = .NewOsCcyAmount

                    cNewOsBaseAmount = .NewOsBaseAmount

                    bFullyMatched = .FullyMatched

                    'Get the transdetail details


                    m_lReturn = m_oTransDetail.GetDetails(vTransdetailID:=m_oAllocationDetail.Details.Item(1).TransDetailID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Get the company ID from the transdetail record.

                    m_lReturn = m_oTransDetail.GetNext(vCompanyID:=lCompanyID.ToString)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = CalculateRecordValues(v_iOriginalCurrency:= .OriginalCurrency, v_lCompanyID:=lCompanyID, v_bAllocateToBase:= .AllocateToBase, v_vdOrigXrate:= .OrigXrate, v_vdEffectiveXrate:= .EffectiveXrate, v_cOsBaseAmount:= .OsBaseAmount, v_cOsCcyAmount:= .OsCcyAmount, r_cAllocBaseAmount:=cAllocBaseAmount, r_cAllocCcyAmount:=cAllocCcyAmount, r_cNewOsCcyAmount:=cNewOsCcyAmount, r_cNewOsBaseAmount:=cNewOsBaseAmount, r_cLossGainAmount:=cLossGainAmount, r_bFullyMatched:=bFullyMatched)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' and put the amounts back

                    .AllocBaseAmount = cAllocBaseAmount

                    .AllocCcyAmount = cAllocCcyAmount

                    .NewOsCcyAmount = cNewOsCcyAmount

                    .NewOsBaseAmount = cNewOsBaseAmount

                    .FullyMatched = bFullyMatched

                    .LossGainAmount = cLossGainAmount

                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

                    cTotalBaseAmount += cAllocBaseAmount
                    cTotalCcyAmount += cAllocCcyAmount


                    If .OriginalCurrency <> iCurrencyId Then
                        bSameCurrency = False
                    End If
                End With

            Next lRow

            r_bSameCurrency = bSameCurrency
            r_cTotalBaseAmount = cTotalBaseAmount

            If bSameCurrency Then
                r_cTotalCcyAmount = cTotalCcyAmount
                r_iCurrencyId = iCurrencyId
            Else
                r_cTotalCcyAmount = 0
                r_iCurrencyId = 0
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CalculateAllocationSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CalculateAllocationSet", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CreatePayment
    '
    ' Description: Create a payment to balance a set
    '
    ' ***************************************************************** '
    Public Function CreatePayment() As Integer
        Dim result As Integer = 0
        Try

            m_oDatabase.SQLBeginTrans()

            m_lReturn = CreatePaymentWork()


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                m_oDatabase.SQLCommitTrans()
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        ' Error.
        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    Public Function AllocateSet() As Integer

        Dim result As Integer = 0
        Dim cTotalBaseAmount, cTotalCcyAmount As Decimal
        Dim iCurrencyId As Integer
        Dim bSameCurrency As Boolean

        Try

            m_lReturn = CalculateAllocationSet(r_cTotalBaseAmount:=cTotalBaseAmount, r_cTotalCcyAmount:=cTotalCcyAmount, r_iCurrencyId:=iCurrencyId, r_bSameCurrency:=bSameCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If cTotalBaseAmount <> 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oAllocation.Details.Item(1).AllocationStatus = gACTLibrary.ACTAllocationStatusAllocated

            m_oAllocation.Details.Item(1).DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit


            m_lReturn = m_oAllocation.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oAllocationDetail.Update
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllocateSet Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AllocateSet", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function GetAllocationDetails() As Integer

        'Called from Form Class
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetAllocation(AllocationId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function IsTransInAllocation(ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0
        Dim lAllocatedTransdetailID As Integer

        Try


            m_oAllocationDetail.CurrentRecord = 0


            For iCount As Integer = 0 To m_oAllocationDetail.Details.Count - 1

                m_lReturn = m_oAllocationDetail.GetNext(vTransdetailID:=lAllocatedTransdetailID.ToString)

                If v_lTransactionId = lAllocatedTransdetailID Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

                If v_lTransactionId = lAllocatedTransdetailID Then
                    Return gPMConstants.PMEReturnCode.PMTrue
                End If

            Next iCount

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsTransInAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsTransInAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' Get the details of a particular allocation & it's details into the class
    Private Function GetAllocation(ByVal v_lAllocationId As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oAllocation.GetDetails(vAllocationId:=v_lAllocationId.ToString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        m_lReturn = m_oAllocationDetail.GetDetails(vAllocationId:=v_lAllocationId.ToString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CreatePaymentWork
    '
    ' Description: Create a payment to balance a set
    '
    ' ***************************************************************** '
    Private Function CreatePaymentWork() As Integer

        Dim result As Integer = 0
        Dim cTotalBaseAmount, cTotalCcyAmount As Decimal
        Dim iCurrencyId As Integer
        Dim lCompanyID As Integer
        Dim bSameCurrency As Boolean
        Dim cPaymentCcyAmount As Decimal
        Dim lCashListItemId As Integer
        Dim dtAccountingDate As Date
        Dim oAllocationCreate As Object = Nothing
        Dim oCashListPost As bACTCashListPost.Automated



        result = gPMConstants.PMEReturnCode.PMTrue
        ' Calculate the payment amount

        m_lReturn = CalculateAllocationSet(r_cTotalBaseAmount:=cTotalBaseAmount, r_cTotalCcyAmount:=cTotalCcyAmount, r_iCurrencyId:=iCurrencyId, r_bSameCurrency:=bSameCurrency)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        m_lReturn = m_oCashList.GetDetails(vCashListID:=m_lCashListId.ToString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        dtAccountingDate = m_oCashList.Details.Item(1).ListDate

        ' Use the currency total if whole set is in same ccy as cash list

        If bSameCurrency And m_oCashList.Details.Item(1).CurrencyID = iCurrencyId Then

            cPaymentCcyAmount = cTotalCcyAmount

        Else


            iCurrencyId = m_oCashList.Details.Item(1).CurrencyID

            lCompanyID = m_oCashList.Details.Item(1).CompanyID
            ' convert the base amount to currency

            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iCurrencyId, lCompanyID:=lCompanyID, cBaseAmount:=cTotalBaseAmount, cCurrencyAmount:=cPaymentCcyAmount, vConversionDate:=dtAccountingDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

        End If

        'Add the cashlist item
        Dim vCashListItem(gACTLibrary.eCashListItem.LastItem) As Object


        vCashListItem(gACTLibrary.eCashListItem.CashlistitemID) = lCashListItemId

        vCashListItem(gACTLibrary.eCashListItem.AllocationstatusID) = gACTLibrary.ACTAllocationStatusUnallocated

        vCashListItem(gACTLibrary.eCashListItem.MediatypeID) = gACTLibrary.ACTMediaTypeCheque

        vCashListItem(gACTLibrary.eCashListItem.CashListId) = m_lCashListId


        vCashListItem(gACTLibrary.eCashListItem.AccountID) = m_oAllocation.Details.Item(1).AccountID

        vCashListItem(gACTLibrary.eCashListItem.MediaRef) = "AutoCheque"


        vCashListItem(gACTLibrary.eCashListItem.OurRef) = m_oCashList.Details.Item(1).CashListRef

        vCashListItem(gACTLibrary.eCashListItem.TheirRef) = "Your ref"

        vCashListItem(gACTLibrary.eCashListItem.Amount) = cPaymentCcyAmount


        m_lReturn = m_oCashListItem.DirectAdd(DirectCast(vCashListItem, Object))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Retrieve the cashlistitem id from the array

        lCashListItemId = CInt(vCashListItem(gACTLibrary.eCashListItem.CashlistitemID))

        ' And post this cashlist item

        'Set oCashListPost = GetOrionBusiness(v_sClassName:="bACTCashListPost.Automated", v_vDatabase:=m_oDatabase)

        oCashListPost = New bACTCashListPost.Automated
        m_lReturn = oCashListPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = oCashListPost.PostCashList(v_vCashListId:=m_lCashListId, v_vCashListItemId:=lCashListItemId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' And a corresponding allocation detail

        'Set oAllocationCreate = GetOrionBusiness(v_sClassName:="bACTAllocationCreate.Automated", v_vDatabase:=m_oDatabase)
        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oAllocationCreate, v_sClassName:="bACTAllocationCreate.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Remove component services


        oAllocationCreate.AllocationId = m_lAllocationId

        m_lReturn = oAllocationCreate.CreateAllocationForCashlist(v_vCashListId:=m_lCashListId.ToString, v_vCashListItemId:=lCashListItemId.ToString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
