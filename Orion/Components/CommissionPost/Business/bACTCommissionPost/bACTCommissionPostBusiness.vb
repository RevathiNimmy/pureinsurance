Option Strict Off
Option Explicit On
Imports System.Data
Imports SSP.Shared
'Developer Guide No 129
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
    ' RAW 01/04/2003 : ISS2854 : allocate cash to the bound transaction instead of the originals
    '                            tighten up error handling a little
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 24/10/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    Private m_bCloseDatabase As Boolean
    Private m_oExplorer As bACTExplorer.Form
    Private m_oOrionLink As Object
    Private m_oAllocationManual As bACTAllocationManual.Business
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oTransdetail As bACTTransdetail.Form

    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oPeriod As bACTPeriod.Form

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
    Private m_lCommissionAccountId As Integer
    Private m_sCommissionAccountCode As String = ""
    Private m_dCommissionAmount As Double
    Private m_lReportMapId As Integer
    Private m_lCommissionEarnedAccountId As Integer
    Private m_sCommissionEarnedAccountCode As String = ""
    Private m_sInsuranceRef As String = ""
    Private m_lOriginalTransDetailID As Integer
    Private m_lPaymentTransaction As Integer
    Private m_lDestinationTransaction As Integer 'FSA Phase 3.2
    'TR - For Auto Batch Payment Run
    Private m_lAllocationStatusID As Integer
    Private m_lCashListItemPaymentStatus As Integer
    Private m_lCashListItemPayTypeID As Integer

    Private m_oPMAutoNumber As bACTAutoNumber.Business
    Private m_oPMDocumentPost As bACTDocumentPost.Form

    'DC260606 Datasure : control movement of taxes
    Private m_lTaxDueAccountId As Integer
    Private m_lTaxAccountId As Integer
    Private m_dTaxAmount As Double

    Friend WriteOnly Property CommissionAccountId() As Integer
        Set(ByVal Value As Integer)
            If m_lCommissionAccountId <> Value Then
                m_lCommissionAccountId = Value
            End If
        End Set
    End Property
    Friend WriteOnly Property OriginalTransDetailId() As Integer
        Set(ByVal Value As Integer)
            If m_lOriginalTransDetailID <> Value Then
                m_lOriginalTransDetailID = Value
            End If
        End Set
    End Property
    Friend WriteOnly Property CommissionAmount() As Double
        Set(ByVal Value As Double)
            If m_dCommissionAmount <> Value Then
                m_dCommissionAmount = Value
            End If
        End Set
    End Property
    Friend WriteOnly Property PaymentTransactionId() As Integer
        Set(ByVal Value As Integer)
            If m_lPaymentTransaction <> Value Then
                m_lPaymentTransaction = Value
            End If
        End Set
    End Property
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
    '*************************************************************************
    'Function :     ReformatTaxArray
    'Description :  Takes the array in the format returned by CalculateTax and
    '               reformats it into the format expected by bACTInsurerPayment
    'History :      SMJB 23/09/03 Created for CQ2493
    '               Translation routine taken from bACTInsurerPayment.CalcTaxesForTax
    '*************************************************************************
    '
    Private Function ReformatTaxArray(ByRef vTaxCalcResultArray() As Object) As Integer

        Dim result As Integer = 0
        'developer Guide No 31
        Dim vMyResultArray(,) As Object

        Const m_klTaxColIndex_LBound As Integer = 0
        Const m_klTaxColIndex_TaxTypeCode As Integer = 1
        Const m_klTaxColIndex_CurrencyTaxAmount As Integer = 3
        Const m_klTaxColIndex_UBound As Integer = 9
        'Incoming array is in the format:
        'vTaxArray(0) = "GST"
        'vTaxArray(1) = "-1.25"
        'vTaxArray(2) = "WHT"
        'vTaxArray(3) = "2.5"        (Or similar)

        'Outgoing array must be an array containing a 2 dimensional array

        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lTaxCount As Integer = CInt((vTaxCalcResultArray.GetUpperBound(0) + 1) / 2)
        vMyResultArray = Array.CreateInstance(GetType(Object), New Integer() {m_klTaxColIndex_UBound - m_klTaxColIndex_LBound + 1, lTaxCount}, New Integer() {m_klTaxColIndex_LBound, 0})

        ' Because TaxCalcResultArray holds data in pairs step through in increments of 2
        Dim j As Integer = -1
        For i As Integer = vTaxCalcResultArray.GetLowerBound(0) To vTaxCalcResultArray.GetUpperBound(0) - 1 Step 2

            j += 1 ' increment tax type counter



            vMyResultArray(m_klTaxColIndex_TaxTypeCode, j) = vTaxCalcResultArray(i)


            vMyResultArray(m_klTaxColIndex_CurrencyTaxAmount, j) = vTaxCalcResultArray(i + 1)
        Next i

        Erase vTaxCalcResultArray
        ReDim vTaxCalcResultArray(1)

        vTaxCalcResultArray(1) = vMyResultArray

        Return result
    End Function

    ' PUBLIC Methods (Begin)
    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level



            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
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


            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oTransdetail = New bACTTransdetail.Form
            m_lReturn = m_oTransdetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oAutoNumber = New bACTAutoNumber.Business
            m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_oPeriod = New bACTPeriod.Form
            m_lReturn = m_oPeriod.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Initialise"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate (Standard Method)
    '
    ' Description: Entry point for any termination code for this
    '              object.
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
                End If
                m_oDatabase = Nothing
                If m_oPMAutoNumber IsNot Nothing Then
                    m_oPMAutoNumber.Dispose()
                    m_oPMAutoNumber = Nothing
                End If
                If m_oPMDocumentPost IsNot Nothing Then
                    m_oPMDocumentPost.Dispose()
                    m_oPMDocumentPost = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oTransdetail IsNot Nothing Then
                    m_oTransdetail.Dispose()
                    m_oTransdetail = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oPeriod IsNot Nothing Then
                    m_oPeriod.Dispose()
                    m_oPeriod = Nothing
                End If

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("SetProcessModes"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetKeys (Standard Method)
    '
    ' Description: Stores all of the parameter members with the key
    '              array.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check we have a vaild array.
            If Not Informations.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)
                ' Assign the parameter member with the
                ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()

                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("SetKeys"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys (Standard Method)
    '
    ' Description: Stores all of the key array with the parameter
    '              members.
    '
    ' ***************************************************************** '
    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim vKeyArray(0, 0)


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetKeys"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: EarnCommission
    '
    ' Description: Creates transactions to update the Gross amount
    '
    ' History: Pass Cost Centre MApping ID eck 250101
    '
    ' ***************************************************************** '
    Public Function EarnCommission(ByVal v_lCommissionAccountID As Integer, ByVal v_lCommissionEarnedAccountID As Integer, ByVal v_dCommissionAmount As Double, ByVal v_sInsuranceRef As String, ByVal v_sDocumentRef As String, ByVal v_lReportMapId As Integer, Optional ByVal v_vDocumentDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "EarnCommission"

        'Static lCommissionAccountID As Integer

        'Dim dCreditAmt, dDebitAmt As Double
        Dim lNumberRangeId As Integer
        'Dim lNumber As Integer
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate, dtRefDate, dtAccountingDate As Date
        Dim sComment As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        'Dim lRow, lNodeId As Integer
        'Dim iLedgerId As Integer
        Dim sFullPath As String = ""
        Dim iBaseCurrencyID As Integer
        Dim vTransdetailTypeId As Object = Nothing
        Dim bTransStarted As Boolean
        Dim vPeriodDetails As Object = Nothing
        Dim lCurrentPeriodID, lPreviousPeriodID As Integer
        Dim dtPeriodEndDate, dtPeriodStartDate As Date

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the dates for the transaction

            If Informations.IsNothing(v_vDocumentDate) Then
                dtDocumentDate = DateTime.Now
                dtRefDate = dtDocumentDate
                dtAccountingDate = dtDocumentDate
            Else
                dtDocumentDate = v_vDocumentDate
                dtRefDate = dtDocumentDate
                dtAccountingDate = dtDocumentDate


                m_lReturn = m_oPeriod.GetCurrentPeriodDetails(r_vDetails:=vPeriodDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetCurrentPeriodDetails", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If Not Informations.IsArray(vPeriodDetails) Then
                    gPMFunctions.RaiseError("IsArray(vResultArray)", "No Results Returned", gPMConstants.PMELogLevel.PMLogError)
                Else

                    lCurrentPeriodID = CInt(vPeriodDetails(0, 0))
                End If


                m_lReturn = m_oPeriod.GetPreviousPeriodID(lPeriodID:=lCurrentPeriodID, lPreviousPeriodID:=lPreviousPeriodID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetPreviousPeriodID", "lPeriodID:=" & lCurrentPeriodID, gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oPeriod.GetDetails(vPeriodID:=lPreviousPeriodID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetDetails", "vPeriodID:=" & lPreviousPeriodID, gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oPeriod.GetNext(vPeriodEndDate:=dtPeriodEndDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                dtPeriodStartDate = dtPeriodEndDate.AddSeconds(1)

                If dtAccountingDate < dtPeriodStartDate Then
                    dtAccountingDate = dtPeriodStartDate
                End If
            End If

            'Get Transdetail_type_id for payment transactions
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCommPayTransDetailTypeSQL, sSQLName:=ACGetCommPayTransDetailTypeName, bStoredProcedure:=ACGetCommPayTransDetailTypeStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=ACGetCommPayTransDetailTypeName", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("IsArray(vResultArray)", "No Results Returned", gPMConstants.PMELogLevel.PMLogError)
            Else


                vTransdetailTypeId = vResultArray(0, 0)
            End If

            'Get the number range

            m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "v_sGroupCode:=" & gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Generate the next number
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_lNumberRangeID:=lNumberRangeId)
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "v_lNumberRangeID:=" & lNumberRangeId, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If sDocumentRef.Trim() <> "" Then
                sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef
            End If
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)

            'Start Transaction to enable Rollback if any part of posting fails
            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bTransStarted = True

            'Create transaction header

            m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:=sComment, r_vDocSourceID:=m_iSourceID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
            End If



            'Get the Company's base currency

            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceID, r_iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oCurrencyConvert.GetBaseCurrency", "v_lCompanyID:=" & m_iSourceID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create commission transacted side of transaction

            m_lReturn = m_oDocumentPost.AddAdjustmentTransaction(v_lAccountID:=v_lCommissionAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_dCommissionAmount * -1, v_cCurrencyAmount:=v_dCommissionAmount * -1, r_vTransDetailId:=m_lPaymentTransaction, v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=1, v_vInsuranceRef:=v_sInsuranceRef, v_vSpare:="COMM PAY " & v_sDocumentRef, v_vRefDate:=dtRefDate, v_vAccountingDate:=dtAccountingDate, v_vTransdetailTypeID:=vTransdetailTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddAdjustmentTransaction", "v_lAccountID:=" & v_lCommissionAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create Commission earned side of transaction

            m_lReturn = m_oDocumentPost.AddAdjustmentTransaction(v_lAccountID:=v_lCommissionEarnedAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_dCommissionAmount, v_cCurrencyAmount:=v_dCommissionAmount, r_vTransDetailId:=m_lDestinationTransaction, v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=2, v_vInsuranceRef:=v_sInsuranceRef, v_vDepartment:=v_lReportMapId, v_vSpare:="COMM PAY " & v_sDocumentRef, v_vRefDate:=dtRefDate, v_vAccountingDate:=dtAccountingDate, v_vTransdetailTypeID:=vTransdetailTypeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddAdjustmentTransaction", "v_lAccountID:=" & v_lCommissionEarnedAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Commit the document

            m_lReturn = m_oDocumentPost.Commit()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.commit", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Commit Transaction
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bTransStarted = False


        Catch ex As Exception

            If bTransStarted Then
                m_lReturn = RollbackTrans()
            End If

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Start"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: MoveTax
    '
    ' Description: Creates transactions to move tax to tax due
    '
    ' History: DC 260303
    '
    ' ***************************************************************** '
    Public Function MoveTax(ByVal v_lTaxAccountID As Integer, ByVal v_lTaxDueAccountID As Integer, ByVal v_dTaxAmount As Double, ByVal v_sInsuranceRef As String, ByVal v_sDocumentRef As String, ByVal v_lReportMapId As Integer, Optional ByVal v_vDocumentDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "MoveTax"

        'Static lTaxAccountID As Integer

        'Dim dCreditAmt, dDebitAmt As Double
        Dim lNumberRangeId As Integer
        'Dim lNumber As Integer
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate, dtRefDate, dtAccountingDate As Date
        Dim sComment As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim sSQL As String = ""
        'Dim lRow, lNodeId As Integer
        'Dim iLedgerId As Integer
        Dim sFullPath As String = ""
        Dim iBaseCurrencyID As Integer
        Dim vTransdetailTypeId As Object = Nothing
        Dim bTransStarted As Boolean
        Dim vPeriodDetails As Object = Nothing
        Dim lCurrentPeriodID, lPreviousPeriodID As Integer
        Dim dtPeriodEndDate, dtPeriodStartDate As Date

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Set the dates for the transaction

            If Informations.IsNothing(v_vDocumentDate) Then
                dtDocumentDate = DateTime.Now
                dtRefDate = dtDocumentDate
                dtAccountingDate = dtDocumentDate
            Else
                dtDocumentDate = v_vDocumentDate
                dtRefDate = dtDocumentDate
                dtAccountingDate = dtDocumentDate


                m_lReturn = m_oPeriod.GetCurrentPeriodDetails(r_vDetails:=vPeriodDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetCurrentPeriodDetails", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                If Not Informations.IsArray(vPeriodDetails) Then
                    gPMFunctions.RaiseError("IsArray(vResultArray)", "No Results Returned", gPMConstants.PMELogLevel.PMLogError)
                Else

                    lCurrentPeriodID = CInt(vPeriodDetails(0, 0))
                End If


                m_lReturn = m_oPeriod.GetPreviousPeriodID(lPeriodID:=lCurrentPeriodID, lPreviousPeriodID:=lPreviousPeriodID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetPreviousPeriodID", "lPeriodID:=" & lCurrentPeriodID, gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oPeriod.GetDetails(vPeriodID:=lPreviousPeriodID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetDetails", "vPeriodID:=" & lPreviousPeriodID, gPMConstants.PMELogLevel.PMLogError)
                End If


                m_lReturn = m_oPeriod.GetNext(vPeriodEndDate:=dtPeriodEndDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oPeriod.GetNext", "Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                dtPeriodStartDate = dtPeriodEndDate.AddSeconds(1)

                If dtAccountingDate < dtPeriodStartDate Then
                    dtAccountingDate = dtPeriodStartDate
                End If
            End If

            'Get Transdetail_type_id for payment transactions
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTaxPayTransDetailTypeSQL, sSQLName:=ACGetTaxPayTransDetailTypeName, bStoredProcedure:=ACGetTaxPayTransDetailTypeStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQLName:=ACGetCommPayTransDetailTypeName", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("IsArray(vResultArray)", "No Results Returned", gPMConstants.PMELogLevel.PMLogError)
            Else


                vTransdetailTypeId = vResultArray(0, 0)
            End If

            'Get the number range

            m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oAutoNumber.GetNumberRange", "v_sGroupCode:=" & gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Generate the next number
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_lNumberRangeID:=lNumberRangeId)
            'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                gPMFunctions.RaiseError("m_oAutoNumber.GenerateDocumentReferenceNumber", "v_lNumberRangeID:=" & lNumberRangeId, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Format the number
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If sDocumentRef.Trim() <> "" Then
                sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef
            End If
            'End (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)

            'Start Transaction to enable Rollback if any part of posting fails
            m_lReturn = BeginTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("BeginTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bTransStarted = True

            'Create transaction header

            m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:=sComment, r_vDocSourceID:=m_iSourceID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddDocument", "v_sDocumentRef:=" & sDocumentRef, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Get the Company's base currency

            m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=m_iSourceID, r_iBaseCurrencyID:=iBaseCurrencyID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oCurrencyConvert.GetBaseCurrency", "v_lCompanyID:=" & m_iSourceID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create tax transacted side of transaction

            m_lReturn = m_oDocumentPost.AddAdjustmentTransaction(v_lAccountID:=v_lTaxAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_dTaxAmount * -1, v_cCurrencyAmount:=v_dTaxAmount * -1, r_vTransDetailId:=m_lPaymentTransaction, v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=1, v_vInsuranceRef:=v_sInsuranceRef, v_vSpare:="TAX PAY " & v_sDocumentRef, v_vRefDate:=dtRefDate, v_vAccountingDate:=dtAccountingDate, v_vTransdetailTypeID:=vTransdetailTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddAdjustmentTransaction", "v_lAccountID:=" & v_lTaxAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create tax moved side of transaction

            m_lReturn = m_oDocumentPost.AddAdjustmentTransaction(v_lAccountID:=v_lTaxDueAccountID, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_dTaxAmount, v_cCurrencyAmount:=v_dTaxAmount, r_vTransDetailId:=m_lDestinationTransaction, v_vdCurrencyBaseXRate:=1, v_vDocumentSequence:=2, v_vInsuranceRef:=v_sInsuranceRef, v_vDepartment:=v_lReportMapId, v_vSpare:="TAX PAY " & v_sDocumentRef, v_vRefDate:=dtRefDate, v_vAccountingDate:=dtAccountingDate, v_vTransdetailTypeID:=vTransdetailTypeId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.AddAdjustmentTransaction", "v_lAccountID:=" & v_lTaxDueAccountID, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Commit the document

            m_lReturn = m_oDocumentPost.Commit()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDocumentPost.commit", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Commit Transaction
            m_lReturn = CommitTrans()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CommitTrans", "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            bTransStarted = False


        Catch ex As Exception

            If bTransStarted Then
                m_lReturn = RollbackTrans()
            End If

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PostCommission (Public)
    '
    ' Description: Performs the Automated Action dependant on the Task
    '              Process Mode etc.
    'eck180500 New Parameter for Company
    'AR20041119 - PN11221 Add optional parameter to exclude posting
    '                     if the original transaction was a DID
    ' ***************************************************************** '
    Public Function PostCommission(ByVal v_sCommissionOption As String, ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer, Optional ByVal v_bExcludeDID As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim v_vResults(,) As Object = Nothing
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'eck180500
            'eck090701 Retrieve original company for earned transaction
            'Select company along with original transaction Informations
            'm_iSourceID = v_iCompanyID
            '
            Select Case v_sCommissionOption
                Case "0"

                    m_lReturn = GetCommissionForDebit(v_lTransDetailID:=v_lTransactionId, v_vResults:=v_vResults)
                Case "1"
                    'AR20041119 - PN11221 Pass ExcludeDID parameter

                    m_lReturn = GetCommissionForClient(v_lTransDetailID:=v_lTransactionId, v_vResults:=v_vResults, v_bExcludeDID:=v_bExcludeDID)
                Case "2"

                    m_lReturn = GetCommissionForInsurer(v_lTransDetailID:=v_lTransactionId, v_vResults:=v_vResults)

                Case "4"

                    m_lReturn = GetCommissionForClientDID(v_lTransDetailID:=v_lTransactionId, v_vResults:=v_vResults)

                Case "666"

                    m_lReturn = GetCommissionForDID(v_lTransDetailID:=v_lTransactionId, v_vResults:=v_vResults)
            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(v_vResults) Then
                Return result
            End If


            For lRow As Integer = v_vResults.GetLowerBound(1) To v_vResults.GetUpperBound(1)


                m_lCommissionAccountId = CInt(v_vResults(0, lRow))

                m_dCommissionAmount = CDbl(v_vResults(1, lRow))

                m_sInsuranceRef = CStr(v_vResults(2, lRow))
                'eck260600

                m_lOriginalTransDetailID = CInt(v_vResults(3, lRow))
                'eck090701 Retrieve original company for earned transaction

                m_iSourceID = CInt(v_vResults(4, lRow))

                m_lReturn = GetDocumentDetails(v_lTransactionId:=m_lOriginalTransDetailID, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'eck040501 make sure it hasn't already moved some how or other
                'eck131102 But don't Exit leaving others unmoved !!!!!!!!!!!!
                '    If HasCommissionMoved(v_lTransDetailID:=m_lOriginalTransDetailID) = True Then
                '         Exit Function
                '    End If
                If Not HasCommissionMoved(v_lTransDetailID:=m_lOriginalTransDetailID) Then
                    'eck 250101 get cost centre mapping ID
                    m_lReturn = GetCommissionEarnedAccount(v_lCommissionAccountID:=m_lCommissionAccountId, v_lReportMapId:=m_lReportMapId, v_lCommissionEarnedAccountID:=m_lCommissionEarnedAccountId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMFail Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="A Commission Income account has not been set up for the commission suspense account", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostCommission"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                            Throw New Exception() 'eck131102
                        End If
                        Return result
                    End If

                    If v_sCommissionOption = "0" Then
                        m_lReturn = EarnCommission(v_lCommissionAccountID:=m_lCommissionAccountId, v_lCommissionEarnedAccountID:=m_lCommissionEarnedAccountId, v_dCommissionAmount:=m_dCommissionAmount, v_sInsuranceRef:=m_sInsuranceRef, v_sDocumentRef:=sDocumentRef, v_lReportMapId:=m_lReportMapId, v_vDocumentDate:=dtDocumentDate)
                    Else
                        m_lReturn = EarnCommission(v_lCommissionAccountID:=m_lCommissionAccountId, v_lCommissionEarnedAccountID:=m_lCommissionEarnedAccountId, v_dCommissionAmount:=m_dCommissionAmount, v_sInsuranceRef:=m_sInsuranceRef, v_sDocumentRef:=sDocumentRef, v_lReportMapId:=m_lReportMapId)
                    End If
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                        Return result
                    End If
                    'eck 0602010 Allocate commission
                    m_lReturn = AllocateCommission()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                        Return result
                    End If

                    'FSA Phase 3.2 Release From Suspense

                    m_lReturn = m_oTransdetail.CreateReleasedTransaction(lSuspendedTransdetailId:=m_lOriginalTransDetailID, lDestinationTransdetailId:=m_lDestinationTransaction)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                        Return result
                    End If

                End If 'eck131102
            Next lRow
            Return result

        Catch excep As System.Exception


            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostCommission"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DC260606 Datasure : control movement of taxes
    ' ***************************************************************** '
    ' Name: PostCommissionTax (Public)
    '
    ' Description: Performs the Automated Action dependant on the Task
    '              Process Mode etc.
    ' ***************************************************************** '
    Public Function PostCommissionTax(ByVal v_iCompanyID As Integer, ByVal v_lTransactionId As Integer) As Integer

        Dim result As Integer = 0

        Dim v_vResults(,) As Object = Nothing
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = GetCommissionTax(v_lTransDetailID:=v_lTransactionId, v_vResults:=v_vResults)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(v_vResults) Then
                Return result
            End If


            For lRow As Integer = v_vResults.GetLowerBound(1) To v_vResults.GetUpperBound(1)


                m_lTaxAccountId = CInt(v_vResults(0, lRow))

                m_dTaxAmount = CDbl(v_vResults(1, lRow))

                m_sInsuranceRef = CStr(v_vResults(2, lRow))

                m_lOriginalTransDetailID = CInt(v_vResults(3, lRow))

                m_iSourceID = CInt(v_vResults(4, lRow))

                m_lReturn = GetDocumentDetails(v_lTransactionId:=m_lOriginalTransDetailID, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Not HasTaxMoved(v_lTransDetailID:=m_lOriginalTransDetailID) Then

                    m_lReturn = GetTaxDueAccount(v_lTaxAccountID:=m_lTaxAccountId, v_lReportMapId:=m_lReportMapId, v_lTaxDueAccountID:=m_lTaxDueAccountId)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="A Tax Due account has not been set up for the tax suspense account", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostCommissionTax"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)

                        Throw New Exception()

                        Return result
                    End If

                    m_lReturn = MoveTax(v_lTaxAccountID:=m_lTaxAccountId, v_lTaxDueAccountID:=m_lTaxDueAccountId, v_dTaxAmount:=m_dTaxAmount, v_sInsuranceRef:=m_sInsuranceRef, v_sDocumentRef:=sDocumentRef, v_lReportMapId:=m_lReportMapId, v_vDocumentDate:=dtDocumentDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                        Return result
                    End If

                    m_lReturn = AllocateCommissionTax()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                        Return result
                    End If


                    m_lReturn = m_oTransdetail.CreateReleasedTransaction(lSuspendedTransdetailId:=m_lOriginalTransDetailID, lDestinationTransdetailId:=m_lDestinationTransaction)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                        Return result
                    End If

                End If

            Next lRow

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostCommissionTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostCommissionTax"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function PostPartCommission(ByVal v_lCommissionSuspendedTransDetailId As Integer, ByVal v_dPercentage As Double, ByVal v_bLastInstalment As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PostPartCommission
        ' PURPOSE: Wrapper for the PostPartCommission use case
        ' AUTHOR: Paul Cunnigham
        ' DATE: 20 November 2002, 09:27:49
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim oPostPartCommission As PostPartCommission


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Create the class to process the work
            oPostPartCommission = New PostPartCommission()

            With oPostPartCommission
                .Business = Me
                .Database = m_oDatabase

                If .Start(r_lCommissionSuspendedTransDetailId:=v_lCommissionSuspendedTransDetailId, r_dPercentage:=v_dPercentage, r_bLastInstalment:=v_bLastInstalment) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If
            End With

            oPostPartCommission = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostPartCommission"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

            End Select

        Finally



        End Try
        Return result
    End Function

    'eck270601
    ' ***************************************************************** '
    ' Name: PostEffectiveCommission (Public)
    '
    ' Description: Post Commission based on policy effective date
    '              Process Mode etc.
    '
    ' eck 23/10/2001 Use Company to get the correct Document
    '
    ' ***************************************************************** '
    Public Function PostEffectiveCommission(ByVal v_sDocumentRef As String, ByVal v_lDocumentCompanyId As Integer) As Integer

        Dim result As Integer = 0

        Dim v_vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'eck 241001 New Paremeter For Company

            m_lReturn = GetCommissionForEffective(v_sDocumentRef:=v_sDocumentRef, v_lDocumentCompanyId:=v_lDocumentCompanyId, v_vResults:=v_vResults)

            'eck241001 set return code
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Not Informations.IsArray(v_vResults) Then
                Return result
            End If

            ' PWF 21/02/2002 *****************************************************
            ' Stopped early function exits in this loop, it prevents the movement
            ' where multiple commission transactions exist on the document
            '
            ' Once we hit this loop we do not exit the function until it ends
            ' simply update the return flag with the latest result and continue.
            ' ********************************************************************

            For lRow As Integer = v_vResults.GetLowerBound(1) To v_vResults.GetUpperBound(1)


                m_lCommissionAccountId = CInt(v_vResults(0, lRow))

                m_dCommissionAmount = CDbl(v_vResults(1, lRow))

                m_sInsuranceRef = CStr(v_vResults(2, lRow))

                m_lOriginalTransDetailID = CInt(v_vResults(3, lRow))

                m_iSourceID = CInt(v_vResults(4, lRow))

                'check that it hasn't already moved
                If HasCommissionMoved(v_lTransDetailID:=m_lOriginalTransDetailID) Then
                    m_lReturn = gPMConstants.PMEReturnCode.PMInvalidRequest
                Else
                    'Get cost centre mapping ID
                    m_lReturn = GetCommissionEarnedAccount(v_lCommissionAccountID:=m_lCommissionAccountId, v_lReportMapId:=m_lReportMapId, v_lCommissionEarnedAccountID:=m_lCommissionEarnedAccountId)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        'Pass cost centre mapping Id
                        m_lReturn = EarnCommission(v_lCommissionAccountID:=m_lCommissionAccountId, v_lCommissionEarnedAccountID:=m_lCommissionEarnedAccountId, v_dCommissionAmount:=m_dCommissionAmount, v_sInsuranceRef:=m_sInsuranceRef, v_sDocumentRef:=v_sDocumentRef, v_lReportMapId:=m_lReportMapId)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostEffectiveCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostEffectiveCommission"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                        Else
                            'Allocate commission
                            m_lReturn = AllocateCommission()
                        End If
                    End If
                End If
            Next lRow


            Return m_lReturn

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostEffectiveCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostEffectiveCommission"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: Get Alloaction Status (Private)
    '
    ' Description: Adds the Key Parameters.
    '
    ' ***************************************************************** '
    ' ***************************************************************** '
    ' Name: Get Commission For Insurer (Private)
    '
    ' Description: Gets Commission Details For Insurer Transaction
    '
    ' ***************************************************************** '
    Private Function GetCommissionForInsurer(ByVal v_lTransDetailID As Integer, ByRef v_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim lRecordCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionForInsurerSQL, sSQLName:=ACSelectCommissionForInsurerName, bStoredProcedure:=ACSelectCommissionForInsurerStored)
            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount < 1 Then
                ' No enough rows retreived
                'Don't return as error
                '            GetCommissionForInsurer = PMFalse
                Return result
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
                'eck090701 Retrieve original company for earned transaction
                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = gPMFunctions.NullToString(oFields("account_id")).Trim()

                        v_vResults(1, lRow - 1) = gPMFunctions.NullToString(oFields("amount")).Trim()

                        v_vResults(2, lRow - 1) = gPMFunctions.NullToString(oFields("insurance_ref")).Trim()

                        v_vResults(3, lRow - 1) = gPMFunctions.NullToString(oFields("transdetail_id")).Trim()
                        'eck090701 Retrieve original company for earned transaction

                        v_vResults(4, lRow - 1) = gPMFunctions.NullToString(oFields("company_id")).Trim()
                    End With
                Next lRow
            End If


        End With
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: Get Commission For DID (Private)
    '
    ' Description: Gets Commission Details For Insurer DID Transactions
    '
    ' ***************************************************************** '
    Private Function GetCommissionForDID(ByVal v_lTransDetailID As Integer, ByRef v_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim lRecordCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionForDIDSQL, sSQLName:=ACSelectCommissionForDIDName, bStoredProcedure:=ACSelectCommissionForDIDStored)
            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount < 1 Then
                ' No enough rows retreived
                'Don't return as error
                '            GetCommissionForDID = PMFalse
                Return result
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
                'eck090701 Retrieve original company for earned transaction
                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = gPMFunctions.NullToString(oFields("account_id")).Trim()

                        v_vResults(1, lRow - 1) = gPMFunctions.NullToString(oFields("amount")).Trim()

                        v_vResults(2, lRow - 1) = gPMFunctions.NullToString(oFields("insurance_ref")).Trim()

                        v_vResults(3, lRow - 1) = gPMFunctions.NullToString(oFields("transdetail_id")).Trim()
                        'eck090701 Retrieve original company for earned transaction

                        v_vResults(4, lRow - 1) = gPMFunctions.NullToString(oFields("company_id")).Trim()
                    End With
                Next lRow
            End If


        End With
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: Get Commission For Client (Private)
    '
    ' Description: Gets Commission Details for Client Transaction
    '
    ' ***************************************************************** '
    Private Function GetCommissionForClient(ByVal v_lTransDetailID As Integer, ByRef v_vResults(,) As Object, ByRef v_bExcludeDID As Boolean) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim lRecordCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue



        With m_oDatabase

            .Parameters.Clear()


            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'AR20041119 - PN11221
            m_lReturn = .Parameters.Add(sName:="exclude_did", vValue:=CStr(If(v_bExcludeDID, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionForClientSQL, sSQLName:=ACSelectCommissionForClientName, bStoredProcedure:=ACSelectCommissionForClientStored)
            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount < 1 Then
                ' No enough rows retreived
                'sj 06/03/2003 - start
                'ISS2712
                '            GetCommissionForClient = PMFalse
                'sj 06/03/2003 - end
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
                'eck090701 Retrieve original company for earned transaction
                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = gPMFunctions.NullToString(oFields("account_id")).Trim()

                        v_vResults(1, lRow - 1) = gPMFunctions.NullToString(oFields("amount")).Trim()

                        v_vResults(2, lRow - 1) = gPMFunctions.NullToString(oFields("insurance_ref")).Trim()

                        v_vResults(3, lRow - 1) = gPMFunctions.NullToString(oFields("transdetail_id")).Trim()
                        'eck090701 Retrieve original company for earned transaction

                        v_vResults(4, lRow - 1) = gPMFunctions.NullToString(oFields("company_id")).Trim()
                    End With
                Next lRow
            End If


        End With
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Get Commission Tax (Private)
    '
    ' Description: Gets Commission Tax Transactions
    '
    ' ***************************************************************** '
    Private Function GetCommissionTax(ByVal v_lTransDetailID As Integer, ByRef v_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim lRecordCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionTaxSQL, sSQLName:=ACSelectCommissionTaxName, bStoredProcedure:=ACSelectCommissionTaxStored)

            ' Database error encountered
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount < 1 Then
                ' No enough rows retreived
                'sj 06/03/2003 - start
                'ISS2712
                '            GetCommissionForClient = PMFalse
                'sj 06/03/2003 - end
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue

                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = gPMFunctions.NullToString(oFields("account_id")).Trim()

                        v_vResults(1, lRow - 1) = gPMFunctions.NullToString(oFields("amount")).Trim()

                        v_vResults(2, lRow - 1) = gPMFunctions.NullToString(oFields("insurance_ref")).Trim()

                        v_vResults(3, lRow - 1) = gPMFunctions.NullToString(oFields("transdetail_id")).Trim()

                        v_vResults(4, lRow - 1) = gPMFunctions.NullToString(oFields("company_id")).Trim()
                    End With
                Next lRow
            End If


        End With
        Return result

    End Function

    'eck100203
    ' ***************************************************************** '
    ' Name: Get Commission For Client DID (Private)
    '
    ' Description: Gets Commission Details for Client with DID Transaction
    '
    ' ***************************************************************** '
    Private Function GetCommissionForClientDID(ByVal v_lTransDetailID As Integer, ByRef v_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21
        Dim oFields As DataRow
        Dim sSQL As String = ""
        Dim lRecordCount As Integer



        result = gPMConstants.PMEReturnCode.PMTrue



        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionForClientDIDSQL, sSQLName:=ACSelectCommissionForClientDIDName, bStoredProcedure:=ACSelectCommissionForClientDIDStored)
            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount < 1 Then
                ' No enough rows retreived
                'DC201103 PN8386 -no not set to false as no commission is valid
                'GetCommissionForClientDID = PMFalse
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
                'eck090701 Retrieve original company for earned transaction
                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = oFields("account_id").Trim()

                        v_vResults(1, lRow - 1) = oFields("amount").Trim()

                        v_vResults(2, lRow - 1) = oFields("insurance_ref").Trim()

                        v_vResults(3, lRow - 1) = oFields("transdetail_id").Trim()
                        'eck090701 Retrieve original company for earned transaction

                        v_vResults(4, lRow - 1) = oFields("company_id").Trim()
                    End With
                Next lRow
            End If


        End With
        Return result

    End Function
    ' ***************************************************************** '
    ' Name: Get Commission For Debit (Private)
    '
    ' Description: Gets Commission Details For Debit Transaction
    '
    ' ***************************************************************** '
    Private Function GetCommissionForDebit(ByVal v_lTransDetailID As Integer, ByRef v_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim lRecordCount As Integer

        result = gPMConstants.PMEReturnCode.PMTrue



        'RWH(06/12/2000) Clear params first.
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        'eck260600
        'Get Brokerage TransactionID
        'eck090701 Retrieve original company for earned transaction
        'FSA Phase 3.2 Modified SQL to select fees for movement too
        sSQL = ""
        sSQL = "SELECT T2.account_id,T2.amount,T2.insurance_ref,T2.transdetail_id,T2.company_id  " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM transdetail T JOIN transdetail T2 ON T2.document_id = T.document_id " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "JOIN transdetail_type TP  ON T2.transdetail_type_id = TP.transdetail_type_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE T2.transdetail_type_id = TP.transdetail_type_id " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND TP.code in ('BROK','FEE','CFEE') " & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "AND T.transdetail_id = {transdetail_id}"
        With m_oDatabase
            m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:=ACSelectCommissionForDebitName, bStoredProcedure:=ACSelectCommissionForDebitStored)


            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            ' Record Count includes Doc and Transactions so we need at least 1 of each
            If lRecordCount < 1 Then
                ' No enough rows retreived
                ' PWF 12/09/2002 - Don't return false, this is not an error,
                '                  we just don't have any commission
                'GetCommissionForDebit = PMFalse
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
                'eck090701 Retrieve original company for earned transaction
                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = gPMFunctions.NullToString(oFields("account_id")).Trim()

                        v_vResults(1, lRow - 1) = gPMFunctions.NullToString(oFields("amount")).Trim()

                        v_vResults(2, lRow - 1) = gPMFunctions.NullToString(oFields("insurance_ref")).Trim()

                        v_vResults(3, lRow - 1) = gPMFunctions.NullToString(oFields("transdetail_id")).Trim()
                        'eck090701 Retrieve original company for earned transaction

                        v_vResults(4, lRow - 1) = gPMFunctions.NullToString(oFields("company_id")).Trim()
                    End With
                Next lRow
            End If
        End With
        Return result

    End Function

    'eck270601
    ' ***************************************************************** '
    ' Name: Get Commission For Effective (Private)
    '
    ' Description: Gets Commission Details For Effective Policy
    ' eck230101 Pass Socument Company
    ' ***************************************************************** '
    Private Function GetCommissionForEffective(ByVal v_sDocumentRef As String, ByVal v_lDocumentCompanyId As Integer, ByRef v_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim lRecordCount As Integer

        result = gPMConstants.PMEReturnCode.PMTrue



        'RWH(06/12/2000) Clear params first.
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_ref", vValue:=v_sDocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        'eck240301
        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_id", vValue:=CStr(v_lDocumentCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        'eck260600
        'Get Brokerage TransactionID
        'PN'eck 261102 replaced with a stored procedure

        'PN11230 was ('eck 261102 replaced with a stored procedure)
        '    sSQL = ""
        '    sSQL = "SELECT T.account_id,T.amount,T.insurance_ref,T.transdetail_id,T.company_id  " & vbCrLf
        '    sSQL = sSQL & "FROM transdetail T, document D " & vbCrLf
        '    sSQL = sSQL & "WHERE T.spare = 'BROK' " & vbCrLf
        '    sSQL = sSQL & "AND T.document_id = D.document_id " & vbCrLf
        '    sSQL = sSQL & "AND D.document_ref = {document_ref}"
        '    sSQL = sSQL & "AND D.company_id = {company_id}"



        With m_oDatabase
            m_lReturn = .SQLSelect(sSQL:=ACSelectCommissionForEffectiveSQL, sSQLName:=ACSelectCommissionForEffectiveName, bStoredProcedure:=ACSelectCommissionForEffectiveStored)
            'PN11230

            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set return value
            lRecordCount = .Records.Count()
            If lRecordCount < 1 Then
                ' No enough rows retreived
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                ' Rows retrieved successfully
                result = gPMConstants.PMEReturnCode.PMTrue
                ReDim v_vResults(4, lRecordCount - 1)
                For lRow As Integer = 1 To lRecordCount
                    oFields = m_oDatabase.Records.Item(lRow - 1).Fields()
                    With oFields

                        v_vResults(0, lRow - 1) = gPMFunctions.NullToString(oFields("account_id")).Trim()

                        v_vResults(1, lRow - 1) = gPMFunctions.NullToString(oFields("amount")).Trim()

                        v_vResults(2, lRow - 1) = gPMFunctions.NullToString(oFields("insurance_ref")).Trim()

                        v_vResults(3, lRow - 1) = gPMFunctions.NullToString(oFields("transdetail_id")).Trim()

                        v_vResults(4, lRow - 1) = gPMFunctions.NullToString(oFields("company_id")).Trim()
                    End With
                Next lRow
            End If
        End With
        Return result

    End Function

    'DJM 20/01/2004 : Not used anymore.
    ''***************************************************************** '
    '' Name: Check if Account is Insurer(Private)
    ''
    '' Description: Check for Commission Already Moved
    ''
    '' ***************************************************************** '
    'Private Function IsInsurer(ByVal v_lTransDetailID As Long) As Boolean
    '
    'Dim oFields As DataRow 'developer guide no. 21
    'Dim sSQL As String
    'Dim lRecordCount As Long
    'Dim iIsInsurer As Integer
    '
    '    IsInsurer = False
    '
    '    On Error GoTo Err_IsInsurer
    '
    '    With m_oDatabase
    '
    '        .Parameters.Clear
    '
    '        m_lReturn = .Parameters.Add( _
    ''            sName:="transdetail_id", _
    ''            vValue:=v_lTransDetailID, _
    ''            idirection:=PMParamInput, _
    ''            iDataType:=PMLong)
    '
    '        m_lReturn = .Parameters.Add( _
    ''            sName:="IsInsurer", _
    ''            vValue:=iIsInsurer, _
    ''            idirection:=PMParamOutput, _
    ''            iDataType:=PMInteger)
    '
    '        m_lReturn& = .SQLAction( _
    ''            sSQL:=ACSelectIsInsurerSQL, _
    ''            sSQLName:=ACSelectIsInsurerName, _
    ''            bStoredProcedure:=ACSelectIsInsurerStored)
    '
    '        ' Database error encountered
    '
    '        If (m_lReturn& <> PMTrue) Then
    '            IsInsurer = False
    '            Exit Function
    '        End If
    '
    '        iIsInsurer = NullToLong(.Parameters.Item("IsInsurer").Value)
    '
    '        If iIsInsurer = 1 Then
    '            IsInsurer = True
    '        End If
    '
    '    End With
    'Exit Function
    '
    'Err_IsInsurer:
    '
    '    ' Error.
    '    IsInsurer = False
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="IsInsurer Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="IsInsurer", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function

    '***************************************************************** '
    ' Name: Check if Commission Already Moved(Private)
    '
    ' Description: Check for Commission Already Moved
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (IsCommissionEarned) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsCommissionEarned(ByVal v_lTransDetailID As Integer) As Boolean
    '
    'Dim result As Boolean = False
    'Dim oFields As DataRow 'developer guide no. 21
    'Dim sSQL As String = ""
    'Dim lRecordCount As Integer
    'Dim sEarned As String = ""
    '
    '
    'Try 
    '
    'With m_oDatabase
    '
    '.Parameters.Clear()
    '
    'm_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'm_lReturn = .Parameters.Add(sName:="earned", vValue:=sEarned, idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'm_lReturn = .SQLAction(sSQL:=ACSelectIsCommissionEarnedSQL, sSQLName:=ACSelectIsCommissionEarnedName, bStoredProcedure:=ACSelectIsCommissionEarnedStored)
    '
    ' Database error encountered
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return False
    'End If
    '
    'sEarned = gPMFunctions.NullToString(.Parameters.Item("earned").Value)
    '
    'If sEarned = "Y" Then
    'result = True
    'End If
    '
    'End With
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = False
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsCommissionEarned Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("IsCommissionEarned"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: GetCommissionEarnedAccount
    '
    ' Description: Creates transactions to update the Gross amount
    '
    ' History: Get Cost Centre Mapping Id eck 250101
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetCommissionEarnedAccount(ByVal v_lCommissionAccountID As Integer, ByRef v_lReportMapId As Integer, ByRef v_lCommissionEarnedAccountID As Object) As Integer


        ' Objects

        ' Transaction paramters
        Dim result As Integer = 0

        Dim lElementId As Integer
        Dim sElementName As String = ""

        Dim sFullPath As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If m_oExplorer Is Nothing Then



                m_oExplorer = New bACTExplorer.Form
                m_lReturn = m_oExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If




            lElementId = m_oExplorer.GetElementFromAccountID(v_lAccountId:=v_lCommissionAccountID)

            m_oExplorer.GetElementExtras(lElementId, vReportMapId:=Nothing, vAccountMapID:=v_lCommissionEarnedAccountID)

            'eck250101 Return cost centre mapping Id

            lElementId = m_oExplorer.GetElementFromAccountID(v_lAccountId:=v_lCommissionEarnedAccountID)


            m_oExplorer.GetElementExtras(lElementId, vReportMapId:=v_lReportMapId, vAccountMapID:=Nothing)

            'eck241001 set PMFail if no mapped account
            If v_lCommissionEarnedAccountID = 0 Then
                result = gPMConstants.PMEReturnCode.PMFail
            End If

            m_oExplorer.Dispose()

            m_oExplorer = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommissionEarnedAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetCommissionEarnedAccount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetTaxDueAccount
    '
    ' Description: Get Due Account Relating to Tax Account
    '
    ' History: DC 260303
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetTaxDueAccount(ByVal v_lTaxAccountID As Integer, ByRef v_lReportMapId As Integer, ByRef v_lTaxDueAccountID As Object) As Integer


        ' Objects

        ' Transaction paramters
        Dim result As Integer = 0

        Dim lElementId As Integer
        Dim sElementName As String = ""

        Dim sFullPath As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oExplorer Is Nothing Then


                m_oExplorer = New bACTExplorer.Form
                m_lReturn = m_oExplorer.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            lElementId = m_oExplorer.GetElementFromAccountID(v_lAccountId:=v_lTaxAccountID)

            m_oExplorer.GetElementExtras(lElementId, vReportMapId:=Nothing, vAccountMapID:=v_lTaxDueAccountID)

            lElementId = m_oExplorer.GetElementFromAccountID(v_lAccountId:=v_lTaxDueAccountID)


            m_oExplorer.GetElementExtras(lElementId, vReportMapId:=v_lReportMapId, vAccountMapID:=Nothing)

            If v_lTaxDueAccountID = 0 Then
                result = gPMConstants.PMEReturnCode.PMFail
            End If

            m_oExplorer.Dispose()

            m_oExplorer = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaxDueAccount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetTaxDueAccount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***************************************************************** '
    ' Name: Get ledger for Transaction(Private)
    '
    ' Description: Checks Has commission moved
    '
    ' ***************************************************************** '
    Private Function HasCommissionMoved(ByVal v_lTransDetailID As Integer) As Boolean

        Dim result As Boolean = False
        ' Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim sEarned As String = ""




        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="earned", vValue:=sEarned, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .SQLAction(sSQL:=ACSelectHasCommissionMovedSQL, sSQLName:=ACSelectHasCommissionMovedName, bStoredProcedure:=ACSelectHasCommissionMovedStored)

            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If

            sEarned = gPMFunctions.NullToString(.Parameters.Item("earned").Value)

            If sEarned = "Y" Then
                result = True
            End If

        End With
        Return result

    End Function

    '***************************************************************** '
    ' Name: Check if Tax has moved to Due account
    '
    ' Description: Checks Has Tax moved
    '
    ' ***************************************************************** '
    Private Function HasTaxMoved(ByVal v_lTransDetailID As Integer) As Boolean

        Dim result As Boolean = False
        '   Dim oFields As DataRow 'developer guide no. 21
        Dim sSQL As String = ""
        Dim sMoved As String = ""




        With m_oDatabase

            .Parameters.Clear()

            m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="moved", vValue:=sMoved, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = .SQLAction(sSQL:=ACSelectHasTaxMovedSQL, sSQLName:=ACSelectHasTaxMovedName, bStoredProcedure:=ACSelectHasTaxMovedStored)

            ' Database error encountered

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return False
            End If

            sMoved = gPMFunctions.NullToString(.Parameters.Item("moved").Value)

            If sMoved = "Y" Then
                result = True
            End If

        End With
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Get Document Reference for Transaction (Private)
    '
    ' Description: Gets Document Reference for  Transaction
    '
    ' ***************************************************************** '
    Private Function GetDocumentDetails(ByVal v_lTransactionId As Integer, ByRef v_sDocumentRef As String, ByRef v_dtDocumentDate As Date) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransactionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        sSQL = ""
        sSQL = sSQL & "SELECT" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    d.document_ref," & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    d.document_date" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "FROM document d" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "JOIN transdetail td" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "    ON td.document_id = d.document_id" & Strings.ChrW(13) & Strings.ChrW(10)
        sSQL = sSQL & "WHERE td.transdetail_id = {transdetail_id}"

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="ACSelectDocumentRef", bStoredProcedure:=False, vResultArray:=vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMError
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        v_sDocumentRef = gPMFunctions.NullToString(vResultArray(0, 0)).Trim()
        v_dtDocumentDate = gPMFunctions.NullToDate(vResultArray(1, 0))

        Return result

    End Function
    'eck060201
    Friend Function AllocateCommission() As Integer


        Dim result As Integer = 0
        Dim vMatchTrans As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Start Document Transaction

        If m_oAllocationManual Is Nothing Then

            m_oAllocationManual = New bACTAllocationManual.Business
            m_lReturn = m_oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = BeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Dim vKeys(1, 2) As Object
        ' AccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCommissionAccountId

        ReDim vKeys(1, 2)
        ' AccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lCommissionAccountId

        ' Original TransID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(m_lOriginalTransDetailID) & "|" & CStr(m_dCommissionAmount)

        ' Commission payment transaction
        ReDim vMatchTrans(0)

        vMatchTrans(0) = CStr(m_lPaymentTransaction) & "|" & CStr(m_dCommissionAmount * -1)

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans



        m_lReturn = m_oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        ' Set the keys

        m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=vKeys)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("matchTransactions"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Start it
        'eck 140501 pass accross the correct company

        m_oAllocationManual.CompanyId = m_iSourceID
        '

        m_lReturn = m_oAllocationManual.Start()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("MatchTransactions"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            m_lReturn = RollbackTrans()
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        'Commit the Allocation

        m_lReturn = CommitTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Err_AllocateCommission
            Return result
        End If


        m_oAllocationManual.Dispose()

        m_oAllocationManual = Nothing

        Return result

Err_AllocateCommission:

        result = gPMConstants.PMEReturnCode.PMError

        m_lReturn = RollbackTrans()

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllocateCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Get Document"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)


        Return result
    End Function

    'DC260606 Datasure
    Friend Function AllocateCommissionTax() As Integer

        Dim result As Integer = 0


        Dim vMatchTrans As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Start Document Transaction
        If m_oAllocationManual Is Nothing Then


            m_oAllocationManual = New bACTAllocationManual.Business
            m_lReturn = m_oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        m_lReturn = BeginTrans()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        Dim vKeys(1, 2) As Object
        ' AccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lTaxAccountId

        ReDim vKeys(1, 2)
        ' AccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lTaxAccountId

        ' Original TransID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(m_lOriginalTransDetailID) & "|" & CStr(m_dTaxAmount)

        ' Commission payment transaction
        ReDim vMatchTrans(0)

        vMatchTrans(0) = CStr(m_lPaymentTransaction) & "|" & CStr(m_dTaxAmount * -1)

        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans


        m_lReturn = m_oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)

        ' Set the keys

        m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=vKeys)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set navigator keys.", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("matchTransactions"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)


            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        ' Start it

        m_oAllocationManual.CompanyId = m_iSourceID


        m_lReturn = m_oAllocationManual.Start()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start Allocation Manual.", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("MatchTransactions"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            m_lReturn = RollbackTrans()


            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Commit the Allocation

        m_lReturn = CommitTrans()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GoTo Err_AllocateCommissionTax
            Return result
        End If


        m_oAllocationManual.Dispose()

        m_oAllocationManual = Nothing

        Return result

Err_AllocateCommissionTax:

        result = gPMConstants.PMEReturnCode.PMError

        m_lReturn = RollbackTrans()

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AllocateCommissionTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("Get Document"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)

        Return result
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("BeginTrans"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("CommitTrans"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("RollbackTrans"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

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


    '*************************************************************************
    'Function :     CopyRecordFromOneArrayToAnother
    'Description :  Takes a record from one 2 Dimnesional Array and copies it
    '               to another. Arrays must have the same First Dimension
    'History :      TR100103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function CopyRecordFromOneArrayToAnother(ByVal v_vSourceArray(,) As Object, ByRef r_vTargetArray(,) As Object, ByVal v_lNoOfFields As Integer, ByVal v_lSourceRecordNo As Integer, ByVal v_lTargetRecordNo As Integer) As Integer

        Dim result As Integer = 0



        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Do the copy for each field
        For lCountOfFields As Integer = 0 To v_lNoOfFields
            'TR - Copy over the record from the Whole Array to the
            'Account specific one


            r_vTargetArray(lCountOfFields, v_lTargetRecordNo) = v_vSourceArray(lCountOfFields, v_lSourceRecordNo)
        Next lCountOfFields

        Return result

    End Function

    '*************************************************************************
    'Function :     GetAllOutstandingCommission
    'Description :  Gets all the transDetail records, and the per Account
    '               total amount for Commissions, given an startdate
    'History :      TR100103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetAllOutstandingCommission(ByRef r_vTransArray(,) As Object) As Integer

        Dim result As Integer = 0


        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Get a list of all the Accounts with Commission amounts outstanding
        With m_oDatabase
            .Parameters.Clear()
            'TR - Run the stored Procedure
            m_lReturn = .SQLSelect(ACSelectFilteredTransDetailsSQL, ACSelectFilteredTransDetailsName, ACSelectFilteredTransDetailsStored, , r_vTransArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If

            'TR - Make sure that at least 1 record was found
            If Not Informations.IsArray(r_vTransArray) Then
                'TR - Not an error, but halt processing. No work to do.
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If
        End With

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: getPartyCntFromAccountID
    ' PURPOSE: gets party Count from AccountID
    ' AUTHOR: steve watton
    ' DATE: 28/01/2003
    ' RETURNS: PMTrue for success
    ' HISTORY: Copied from bACTCashListItemForm.cls for CQ2493 By SMJB - 23/09/03
    ' ---------------------------------------------------------------------------

    Public Function GetPartyCntFromAccountID(ByVal v_lAccountID As Integer, ByRef r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 39
        Const ACGetPartyCntFromAccountIDSQL As String = "spu_ACT_Get_PartyCnt_From_AccountID"
        Const ACGetPartyCntFromAccountIDName As String = "GetPartyCntFromAccountID"


        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add usergroupcode as an OUTPUT param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="accountid", vValue:=CStr(v_lAccountID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPartyCntFromAccountIDSQL, sSQLName:=ACGetPartyCntFromAccountIDName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            r_lPartyCnt = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyCntFromAccountID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetPartyCntFromAccountID"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*************************************************************************
    'Function :     UpdateTransDetailIsPaidStatus
    'Description :  Loops through all the TransDetail reocrds in the array
    '               and updates their Is_Paid status to 1
    'History :      TR130103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function UpdateTransDetailIsPaidStatus(ByVal v_vArrayOfTransDetailIDs() As Object) As Integer

        Dim result As Integer = 0



        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Do this for each TransDetail record in the array
        For iCount As Integer = v_vArrayOfTransDetailIDs.GetLowerBound(0) To v_vArrayOfTransDetailIDs.GetUpperBound(0)

            'TR - Update the record
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("Is_Paid", CStr(1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                .Parameters.Add("transdetail_id", CStr(v_vArrayOfTransDetailIDs(iCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                'TR - Run the stored Procedure
                m_lReturn = .SQLSelect(ACUpdateTransDetailsIsPaidSQL, ACUpdateTransDetailsIsPaidName, ACUpdateTransDetailsIsPaidStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMError
                End If
            End With
        Next iCount

        Return result

    End Function

    '*************************************************************************
    'Function :     GetLookupsForAccount
    'Description :  Gets some specific data for an account
    'History :      TR130103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetLookupsForAccount(ByVal v_lAccountID As Integer, ByRef r_lCurrencyID As Integer, ByRef r_lCompany_ID As Integer, ByRef r_lSubBranch_ID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Update the record
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("account_id", CStr(v_lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            'TR - Run the stored Procedure
            m_lReturn = .SQLSelect(ACGetAccountDetailsSQL, ACGetAccountDetailsName, ACGetAccountDetailsStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            ElseIf Informations.IsArray(vResultArray) Then

                r_lCurrencyID = CInt(Val(CStr(vResultArray(5, 0))))

                r_lCompany_ID = CInt(Val(CStr(vResultArray(1, 0))))

                r_lSubBranch_ID = CInt(Val(CStr(vResultArray(53, 0))))
            End If
        End With

        Return result

    End Function

    '*************************************************************************
    'Function :     GetAccountNameAndAddress
    'Description :  Gets the Payment Name and Address details for a given
    '               Account Number
    'History :      TR200103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetAccountNameAndAddress(ByVal v_lAccountID As Integer, ByRef r_sPaymentName As String, ByRef r_sAddress1 As String, ByRef r_sAddress2 As String, ByRef r_sAddress3 As String, ByRef r_sAddress4 As String, ByRef r_sPostalCode As String, ByRef r_iAddressCountry As Integer) As Integer

        Dim result As Integer = 0
        Dim oAccount As New bACTAccount.Form



        result = gPMConstants.PMEReturnCode.PMTrue


        oAccount = New bACTAccount.Form
        m_lReturn = oAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'TR - Use the Account object to get the required data

        m_lReturn = oAccount.GetDetails(vAccountID:=v_lAccountID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(CStr(gPMConstants.PMELogLevel.PMLogError), CInt("Failed to get details from the " & "account business object"), ACApp, ACClass, "GetAccountNameAndAddress")
            Return result
        End If


        m_lReturn = oAccount.GetNext(vAddress1:=r_sAddress1, vAddress2:=r_sAddress2, vAddress3:=r_sAddress3, vAddress4:=r_sAddress4, vPostalCode:=r_sPostalCode, vAddressCountry:=r_iAddressCountry, vPaymentName:=r_sPaymentName)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(CStr(gPMConstants.PMELogLevel.PMLogError), CInt("Failed to retreive the details " & "from the Account business object "), ACApp, ACClass, "GetAccountNameAndAddress")
        End If

        Return result

    End Function

    '*************************************************************************
    'Function :     GetAllocationStatus
    'Description :  Gets the AllocationStatus ID for a given Code
    'History :      TR210103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetAllocationStatus(ByVal v_sAllocationStatusCode As String, ByRef r_lAllocationStatusID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Update the record
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("allocationstatuscode", v_sAllocationStatusCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            'TR - Run the stored Procedure
            m_lReturn = .SQLSelect(ACGetAllocationStatusSQL, ACGetAllocationStatusName, ACGetAllocationStatusStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            ElseIf Informations.IsArray(vResultArray) Then

                r_lAllocationStatusID = CInt(Val(CStr(vResultArray(0, 0))))
            End If
        End With

        Return result

    End Function

    '*************************************************************************
    'Function :     GetCashListItemPaymentType
    'Description :  Gets the ID for the PaymentType passed in
    'History :      TR130103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetCashListItemPaymentType(ByVal v_sCashListPayTypeCode As String, ByRef r_lCashListType_ID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Update the record
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("CashListPayTypeCode", v_sCashListPayTypeCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            'TR - Run the stored Procedure
            m_lReturn = .SQLSelect(ACGetCashListItemPayTypeSQL, ACGetCashListItemPayTypeName, ACGetCashListItemPayTypeStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            ElseIf Informations.IsArray(vResultArray) Then

                r_lCashListType_ID = CInt(Val(CStr(vResultArray(0, 0))))
            End If
        End With

        Return result

    End Function

    '*************************************************************************
    'Function :     GetCashListItemPaymentStatus
    'Description :  Gets the ID for the PaymentStatus passed in
    'History :      TR210103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetCashListItemPaymentStatus(ByVal v_sPaymentStatusCode As String, ByRef r_lPaymentStatusID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        'TR - Assume success
        result = gPMConstants.PMEReturnCode.PMTrue

        'TR - Gets the record
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("PaymentStatusCode", v_sPaymentStatusCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            'TR - Run the stored Procedure
            m_lReturn = .SQLSelect(ACGetCashListItemPayStatusSQL, ACGetCashListItemPayStatusName, ACGetCashListItemPayStatusStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            ElseIf Informations.IsArray(vResultArray) Then

                r_lPaymentStatusID = CInt(Val(CStr(vResultArray(0, 0))))
            End If
        End With

        Return result

    End Function

    '*************************************************************************
    'Function :     GetNonAccountSpecificLookups
    'Description :  Gets the non-Acount specific data
    'History :      TR210103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function GetNonAccountSpecificLookups() As Integer

        Dim result As Integer = 0


        'TR - Get some global lookup data
        m_lReturn = GetAllocationStatus("U", m_lAllocationStatusID)

        'TR - Continue if this succeeded
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'TR - Get the CashListItemPaymentType_ID for this Commissions
            m_lReturn = GetCashListItemPaymentType("COMM", m_lCashListItemPayTypeID)
        End If

        'TR - Continue if this succeeded
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            'TR - Get the CashListItemPaymentStatus_ID for this
            m_lReturn = GetCashListItemPaymentStatus("ISS", m_lCashListItemPaymentStatus)
        End If

        'TR - Set return value
        Return m_lReturn

    End Function

    '*************************************************************************
    'Function :     IsPartyDueForPayment
    'Description :  Creates a party object and gets the next payment date for
    '               them. If it is due (i.e. todays date or earlier) returns
    '               True, otherwise False
    'History :      TR210103 - Created as per TS219 Section 4.7
    '*************************************************************************
    Private Function IsPartyDueForPayment(ByVal v_lParty_cnt As Integer) As Boolean

        Dim result As Boolean = False
        Dim obSirParty As Object = Nothing
        Dim dtPartyPaymentDueDate As Date
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TR - Create a Party Object to get the date
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=obSirParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TR - Use the Party object to find the next payment date

            m_lReturn = obSirParty.GetNextPaymentDate(ToSafeInteger(v_lParty_cnt), ToSafeDate(dtPartyPaymentDueDate))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(CStr(gPMConstants.PMELogLevel.PMLogError), CInt("Failed to get the Next Payment " & "Date for Party:=" & CStr(v_lParty_cnt)), ACApp, ACClass, "IsPartyDueForPayment")
                Return result
            End If

            'TR - See if the Date is todays or earlier

            Return dtPartyPaymentDueDate <= DateTime.Now

        Catch
        End Try


        result = False
        bPMFunc.LogMessage("", CInt(gPMConstants.PMELogLevel.PMLogOnError), CStr("Failed to get Payment Due Date for " & "Party"), ACApp, ACClass, "IsPartyDueForPayment", Informations.Err().Number, Informations.Err().Description)
        Return result
    End Function


    '******************************************************************************
    '        Function Name:  PostSuspendedTransaction
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  24-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In) - v_lSuspendedTransDetailId - Long     -
    '                        (In) - v_lSuspendedAccountID     - Long     -
    '                        (In) - v_dPercentage             - Double   -
    '                        (In) - v_bIsLastInstalment       - Boolean  -
    '                        (In) - v_lInsuranceFileCnt       - Long     -
    '                        (In) - v_sInsuranceRef           - String   -
    '                        (In) - v_bPartialPayment         - Boolean  -
    '                        (In) - v_lInstalmentID           - Long     -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:
    '
    '******************************************************************************
    Public Function PostSuspendedTransaction(ByVal v_lSuspendedTransDetailId As Integer, ByVal v_lSuspendedAccountID As Integer, ByVal v_dPercentage As Single, ByVal v_bIsLastInstalment As Boolean, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByVal v_bPartialPayment As Boolean, ByVal v_lInstalmentID As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim cTransDetailAmount, cAllocationDetailAmount, cAmountToRelease As Decimal
            Dim lSuspenseAccountID, lReleaseTransDetailID As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the allocation Amounts for the transactions we passed in
            m_lReturn = GetTransAndAllocationAmounts(v_lSuspendedTransDetailId:=v_lSuspendedTransDetailId, r_lSuspenseAccountID:=lSuspenseAccountID, r_cTransDetailAmount:=cTransDetailAmount, r_cAllocationDetailAmount:=cAllocationDetailAmount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedTransaction Failed Post Transaction Amounts", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedTransaction"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' If we have a partial payemnt then we need to use this amount instead of the transamount
            If v_bPartialPayment Then

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedTransaction Failed Post Partial Amounts", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedTransaction"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If

            ' Get the correct amount based on the last instalment flag we passed in
            If v_bIsLastInstalment Then
                cAmountToRelease = cTransDetailAmount - cAllocationDetailAmount
            Else
                cAmountToRelease = Math.Round((cTransDetailAmount) * v_dPercentage, 2)
            End If

            ' Post the transaction
            m_lReturn = PostSuspendedAmount(v_lSuspenseAccountID:=lSuspenseAccountID, v_lSuspendedAccountID:=v_lSuspendedAccountID, v_cAmountToRelease:=cAmountToRelease, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sInsuranceRef:=v_sInsuranceRef, r_lReleaseTransDetailId:=lReleaseTransDetailID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedTransaction Failed to PostSuspendedAmount", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedTransaction"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Allocate the Transaction
            CommissionAccountId = lSuspenseAccountID
            OriginalTransDetailId = v_lSuspendedTransDetailId
            ' This will be a debit amount (i.e. positive)
            CommissionAmount = cAmountToRelease
            PaymentTransactionId = lReleaseTransDetailID

            m_lReturn = AllocateCommission()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedTransaction Failed to AllocateTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedTransaction"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedTransaction"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    '******************************************************************************
    '        Function Name:  GetTransAndAllocationAmounts
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  24-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lSuspendedTransDetailId - Long      -
    '                        (In/Out) - r_lSuspenseAccountID      - Long      -
    '                        (In/Out) - r_cTransDetailAmount      - Currency  -
    '                        (In/Out) - r_cAllocationDetailAmount - Currency  -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This function get the Informations for a transdetail to
    '                        help with the posting suspended transactions
    '******************************************************************************
    Public Function GetTransAndAllocationAmounts(ByVal v_lSuspendedTransDetailId As Integer, ByRef r_lSuspenseAccountID As Integer, ByRef r_cTransDetailAmount As Decimal, ByRef r_cAllocationDetailAmount As Decimal) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="TransDetailId ", vValue:=CStr(v_lSuspendedTransDetailId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransAndAllocationAmounts Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetTransAndAllocationAmounts"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)

                    Return result

                End If

                m_lReturn = .SQLSelect(sSQL:=ACGetSuspenseDetailsSQL, sSQLName:=ACGetSuspenseDetailsName, bStoredProcedure:=ACGetSuspenseDetailsStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = m_lReturn

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransAndAllocationAmounts Failed to process the Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetTransAndAllocationAmounts"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)

                    Return result

                End If

                If .Records.Count() >= 1 Then
                    'developer guide no. 111
                    r_cTransDetailAmount = gPMFunctions.NullToCurrency(.Records.Item(0).Fields("TransDetailAmount"))
                    r_cAllocationDetailAmount = gPMFunctions.NullToCurrency(.Records.Item(0).Fields("AllocationDetailAmount"))
                    r_lSuspenseAccountID = gPMFunctions.NullToLong(.Records.Item(0).Fields("SuspendedAccountID"))
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    r_cTransDetailAmount = 0
                    r_cAllocationDetailAmount = 0
                    r_lSuspenseAccountID = 0
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransAndAllocationAmounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetTransAndAllocationAmounts"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function


    '******************************************************************************
    '        Function Name:  PostSuspendedAmount
    '******************************************************************************
    '           Created By:  Ahmed "Jay" Bishtawi
    '           Created On:  24-Sep-2003
    '******************************************************************************
    '       Parameters Are:
    '                        (In)     - v_lSuspenseAccountID    - Long      -
    '                        (In)     - v_lSuspendedAccountID   - Long      -
    '                        (In)     - v_cAmountToRelease      - Currency  -
    '                        (In/Out) - r_lReleaseTransDetailId - Long      -
    '
    ' Return Value Type Is:  Long -
    '******************************************************************************
    ' Function Description:  This function post the suspended amount to the account
    '
    '******************************************************************************
    Private Function PostSuspendedAmount(ByVal v_lSuspenseAccountID As Integer, ByVal v_lSuspendedAccountID As Integer, ByVal v_cAmountToRelease As Decimal, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByRef r_lReleaseTransDetailId As Integer) As Integer



        Dim result As Integer = 0
        Dim sGroupCode As String = ""
        Dim sRangeCode As String = ""
        Dim sDocumentRef As String = ""
        Dim lDocumentId, lNumberRangeId, lDocumentSequence As Integer
        Dim dtAccountingDate As Date
        Dim lTransDetailId As Integer

        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroBaseXrate As Object = Nothing
        Dim vdEuroCcyXrate As Object = Nothing
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim vdBaseAmountUnrounded As Object = Nothing
        Dim vdCurrencyAmountUnrounded As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
        sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn

        If m_oPMAutoNumber Is Nothing Then

            m_oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = m_oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To create bACTAutoNumber.Business", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If

        With m_oPMAutoNumber

            m_lReturn = .GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To GetNumberRange", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If
            'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = .GenerateDocumentReferenceNumber(v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_lNumberRangeID:=lNumberRangeId)
            'End (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To GenerateDocumentReferenceNumber", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End With

        'Get the base amount to post
        '(we pass in r_crAmount as currency amount and routine returns amount to post in cBaseAmount)
        cCurrencyAmount = v_cAmountToRelease


        'developer guide no. 98
        m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=m_iCurrencyID, v_iCompanyID:=m_iSourceID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=vdEuroCcyXrate, r_vEuroBaseXRate:=vdEuroBaseXrate, r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To GetBaseAmountFromCurrency", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If

        If m_oPMDocumentPost Is Nothing Then

            m_oPMDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oPMDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To create bACTDocumentPost.Form", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If
        'Start (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If sDocumentRef.Trim() <> "" Then
            sDocumentRef = sRangeCode & sDocumentRef
        End If
        'End (Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        dtAccountingDate = DateTime.Now

        With m_oPMDocumentPost


            m_lReturn = .AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="  ", r_vDocumentID:=lDocumentId, r_vDocSourceID:=m_iSourceID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To AddDocument", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Post the credit to the party account and get the TransDetailId (r_lPaymentTransId)
            'so that we can allocate it against the original commission amount
            lDocumentSequence = 1


            m_lReturn = .AddTransaction(v_lAccountID:=v_lSuspendedAccountID, v_vDocumentSequence:=lDocumentSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=lTransDetailId, v_vAccountingDate:=dtAccountingDate, v_vInsuranceRef:=v_sInsuranceRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Post the matching debit to the Transaction suspense account
            lDocumentSequence += 1

            m_lReturn = .AddTransaction(v_lAccountID:=v_lSuspenseAccountID, v_vDocumentSequence:=lDocumentSequence, v_iCurrencyID:=m_iCurrencyID, v_cAmount:=cBaseAmount * -1, v_cCurrencyAmount:=cCurrencyAmount * -1, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=r_lReleaseTransDetailId, v_vAccountingDate:=dtAccountingDate, v_vInsuranceRef:=v_sInsuranceRef)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To AddTransaction", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If


            m_lReturn = .Commit()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedAmount Failed To Commit", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("PostSuspendedAmount"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End With


        Return result

    End Function


    'developer guide no. 101
    Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Object, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Object, ByRef r_vEuroBaseXRate As Object, ByRef r_vCCyAmountUnrounded As Object, ByRef r_vBaseAmountUnrounded As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue



        r_vdCurrencyBaseXRate = 0


        m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_iCompanyID, cBaseAmount:=r_cBaseAmount, cCurrencyAmount:=v_cCurrencyAmount, vConversionDate:=v_dtAccountingDate, vConversionRate:=r_vdCurrencyBaseXRate, vIsMultiplier:=False, vRounded:=Nothing, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=r_lEuro, cEuroAmount:=r_cEuroAmount, vEuroCCyXrate:=r_vEuroCCyXrate, vEuroBaseXRate:=r_vEuroBaseXRate, vCCyAmountUnRounded:=r_vCCyAmountUnrounded, vBaseAmountUnRounded:=r_vBaseAmountUnrounded)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBaseAmountFromCurrency Failed To ConvertCurrencytoBase", vApp:=ACApp, vClass:=ACClass, vMethod:=CInt("GetBaseAmountFromCurrency"), vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If

        Return result

    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
