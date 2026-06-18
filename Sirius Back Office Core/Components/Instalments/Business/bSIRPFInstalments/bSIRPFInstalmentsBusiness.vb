Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 6-Aug-2001
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              Instalments.
    '
    ' Edit History:
    ' DD060801  - Created
    ' ***************************************************************** '
    'Paul Test SS


    ' ************************************************
    ' Added to replace global variables 13/01/2004
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

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    Private m_oOrionInstalment As bACTInstalments.Business

    Private m_oCreditControlItem As bACTCreditControlItem.Business

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_dtStartDate As Date
    Private m_dtCoverStartDate As Date

    Dim lPrevPremiumFinanceCnt As Integer
    Private lPMAuthorityLevel As Integer

    'enum representing columns in return array for SP call: spu_SIR_Spoke_ExportInstalmentGeneration_GetInstalments
    'Declaration: Me.CreateTPRInstalments.vPlanInstalments
    Private Enum eACPlanInstalments
        InstalmentId = 0
        DueDate
        InstalmentNumber
        Amount
    End Enum

    'enum representing columns in array for SP call: spu_SIR_Spoke_ExportInstalmentGeneration_GetInstalments
    'Declaration: Me.IExport_Export.vRequiredInstalments
    Private Enum eACRequiredInstalments
        DueDate = 0
        InstalmentNumber
        Fee
        Tax
        Commission
        Amount
        TransactionCode
    End Enum

    'Developer Guide No 39
    Private Const ksSPGetInstalmentsSQL As String = "spu_SIR_Spoke_ExportInstalmentsGeneration_GetInstalments"
    Private Const ksSPGetInstalmentsName As String = "ExportInstalmentsGenerationGetInstalments"
    Private Const ksSPGetInstalmentsStored As Boolean = True
    Private Const V_bPartialPayment As Boolean = True

    ' Alix Bergeret - 31/03/2003 - Issue 2914
    Public Enum AlignPremiumToOptions
        AlignWithPolicyRenewalDate = 0
        AlignWithClientPreference = 1
    End Enum

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property
    Private _sPlanStatus As String
    Private _nPFFinancePlanCnt As Integer
    Private _nPFFinancePlanVersion As Integer

    Public Property PlanStatus() As String
        Get
            Return _sPlanStatus
        End Get
        Set(ByVal value As String)
            _sPlanStatus = value
        End Set
    End Property
    Public Property PFFinancePlanCnt() As Integer
        Get
            Return _nPFFinancePlanCnt
        End Get
        Set(ByVal value As Integer)
            _nPFFinancePlanCnt = value
        End Set
    End Property
    Public Property PFFinancePlanVersion() As Integer
        Get
            Return _nPFFinancePlanVersion
        End Get
        Set(ByVal value As Integer)
            _nPFFinancePlanVersion = value
        End Set
    End Property

    Public Property CallingAppName() As String = String.Empty
    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    '
    ' Name: LastBatchNumber
    '
    ' Description:  Returns an the last Batch Number used for a particular
    '               payment method.
    '
    ' History: 13/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function LastBatchNumber(Optional ByRef lPaymentMethod As Integer = 1) As Integer
        'Release 1 - only supports one payment type

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="@PaymentMethod", vValue:=CStr(lPaymentMethod), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        'Developer Guide No 39
        m_oDatabase.SQLSelect("spu_PFInstalmentsGetNextBatchNo", "Last Batch Number", True)
        'SD 31/07/2002 Scalability changes
        Return CInt(m_oDatabase.Records.Item(1).Fields()(NEXTBATCHNUMBER) - 1)
    End Function

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

            'SD 31/07/2002 scalability changes

            m_oOrionInstalment = New bACTInstalments.Business
            m_lReturn = m_oOrionInstalment.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)

            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            '***LookupBegin***
            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            '***LookupEnd***


            m_oCreditControlItem = New bACTCreditControlItem.Business
            m_lReturn = m_oCreditControlItem.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=m_oDatabase)

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

    Private Function ProcessPartialPayment(ByVal v_lInstalmentId As Integer, ByRef v_cPartialPayAmount As Decimal) As Integer

        Dim result As Integer = 0


        Dim lNewInstalmentId As Integer
        Dim sErrorMessage As String = ""
        Dim sValue As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        sErrorMessage = "ProcessPartialPayment Failed"

        With m_oDatabase

            .Parameters.Clear()

            'Add Instalment ID
            m_lReturn = .Parameters.Add(sName:="PFInstalment_ID", vValue:=CStr(v_lInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to add Instalment Id to " & ACPartialPaymentInstalmentsName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                'RollbackTrans
                Return m_lReturn
            End If

            'Add Partial Payment Amount
            m_lReturn = .Parameters.Add(sName:="PartialPayAmount", vValue:=CStr(v_cPartialPayAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to add Partial Payment Amount to " & ACPartialPaymentInstalmentsName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                'RollbackTrans
                Return m_lReturn
            End If

            'Add the New PF Instalments ID as output
            m_lReturn = .Parameters.Add(sName:="NewPFInstalment_ID", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to add New Instalment ID to " & ACPartialPaymentInstalmentsName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                'RollbackTrans
                Return m_lReturn
            End If

            m_lReturn = .SQLAction(sSQL:=ACPartialPaymentInstalmentsSQL, sSQLName:=ACPartialPaymentInstalmentsName, bStoredProcedure:=ACPartialPaymentInstalmentsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to process " & ACPartialPaymentInstalmentsName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                'RollbackTrans
                Return m_lReturn
            End If

            lNewInstalmentId = .Parameters.Item("NewPFInstalment_ID").Value

            If lNewInstalmentId = 0 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to return a New Instalment ID from " & ACPartialPaymentInstalmentsName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                'RollbackTrans
                Return gPMConstants.PMEReturnCode.PMError
            End If

            'Get the product option

            bPMFunc.getProductOptionValue(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTGenerateAdvanceCreditControlForInstalments, v_vBranch:=m_iSourceID, r_vUnderwriting:=sValue)

            If gPMFunctions.NullToString(sValue).Trim = "1" Then
                ' Clear the parameters for the next SQL Statement
                .Parameters.Clear()

                'Add Instalment ID
                m_lReturn = .Parameters.Add(sName:="PFInstalment_ID", vValue:=CStr(v_lInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to add Instalment Id to " & ACPartialPaymentCCIName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    'RollbackTrans
                    Return m_lReturn
                End If

                'Add Partial Payment Amount
                m_lReturn = .Parameters.Add(sName:="PartialPayAmount", vValue:=CStr(v_cPartialPayAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to add Partial Payment Amount to " & ACPartialPaymentCCIName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    'RollbackTrans
                    Return m_lReturn
                End If

                'Add the New PF Instalments ID as output
                m_lReturn = .Parameters.Add(sName:="NewPFInstalment_ID", vValue:=CStr(lNewInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to add New Instalment ID to " & ACPartialPaymentCCIName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    'RollbackTrans
                    Return m_lReturn
                End If

                m_lReturn = .SQLAction(sSQL:=ACPartialPaymentCCISQL, sSQLName:=ACPartialPaymentCCIName, bStoredProcedure:=ACPartialPaymentCCIStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to process " & ACPartialPaymentCCIName, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPartialPayment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    'RollbackTrans
                    Return m_lReturn
                End If

            End If

        End With

        Return result

    End Function

    Private Function RoundDown2DP(ByRef v_cOriginalAmount As Decimal) As Decimal
        'Purpose: Rounds a currency value down to 2 decimal places
        Return Math.Floor(v_cOriginalAmount * 100) / 100
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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                End If
                m_oLookup = Nothing

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                m_oCreditControlItem = Nothing
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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Instalment directly into the database.
    '        Note: The Instalment will NOT be added to the collection.
    '
    ' History
    ' PSL 12/11/2002 Now does tax and commission
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByVal v_vpfprem_finance_cnt As Integer = 0, Optional ByVal v_vpfprem_finance_version As Integer = 0, Optional ByVal v_vInstalmentNumber As Integer = 0, Optional ByVal v_vDueDate As Object = Nothing, Optional ByVal v_vFee As Double = 0, Optional ByVal v_vTax As Double = 0, Optional ByVal v_vCommission As Double = 0, Optional ByVal v_vAmount As Double = 0, Optional ByVal v_vTransactionCode As Object = Nothing, Optional ByVal v_vStatus As Integer = 0, Optional ByVal v_vBatchNumber As Byte = 0, Optional ByVal v_vBatchExportDate As Object = Nothing, Optional ByVal v_vPostedDate As Object = Nothing, Optional ByVal v_vPFTransaction_id As Byte = 0, Optional ByVal v_vMediaHistoryId As Object = Nothing) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SW CQ 1095 12/05/2003 Check that the transaction_id is not set to zero, there are records in
            'the database where it has been set to zero and this should never happen

            If Not Informations.IsNothing(v_vPFTransaction_id) Then
                If v_vPFTransaction_id = 0 Then

                    v_vPFTransaction_id = Nothing
                End If
            End If

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the required INPUT parameters
                m_lReturn = .Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(v_vpfprem_finance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(v_vpfprem_finance_version), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="InstalmentNumber", vValue:=CStr(v_vInstalmentNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                AddInputParameters(v_vDueDate, v_vFee, v_vTax, v_vCommission, v_vAmount, v_vTransactionCode, v_vStatus, v_vBatchNumber, v_vBatchExportDate, v_vPostedDate, v_vPFTransaction_id)

                'If Informations.IsNothing(v_vMediaHistoryId) Then

                'm_lReturn = .Parameters.Add(sName:="pfmediatype_history_id", vValue:=CStr(v_vMediaHistoryId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Else

                m_lReturn = .Parameters.Add(sName:="pfmediatype_history_id", vValue:=v_vMediaHistoryId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                'End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectEdit  (Public)
    '
    ' Description: Edits a single Instalment directly into the database.
    '        Note: The Instalment will NOT be added to the collection.
    '
    ' History
    ' PSL 12/11/2002 Now does tax and commission
    '
    '
    ' ***************************************************************** '
    Public Function DirectEdit(Optional ByVal v_vpfprem_finance_cnt As Object = Nothing, Optional ByVal v_vpfprem_finance_version As Object = Nothing, Optional ByVal v_vInstalmentNumber As Object = Nothing, Optional ByVal v_vDueDate As Object = Nothing, Optional ByVal v_vFee As Double = 0, Optional ByVal v_vTax As Double = 0, Optional ByVal v_vCommission As Double = 0, Optional ByVal v_vAmount As Double = 0, Optional ByVal v_vTransactionCode As Object = Nothing, Optional ByVal v_vStatus As Integer = 0, Optional ByVal v_vBatchNumber As Byte = 0, Optional ByVal v_vBatchExportDate As Object = Nothing, Optional ByVal v_vPostedDate As Object = Nothing, Optional ByVal v_vPFTransaction_id As Byte = 0) As Integer


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SW CQ 1095 12/05/2003 Check that the transaction_id is not set to zero, there are records in
            'the database where it has been set to zero and this should never happen

            If Not Informations.IsNothing(v_vPFTransaction_id) Then
                If v_vPFTransaction_id = 0 Then

                    v_vPFTransaction_id = Nothing
                End If
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(v_vpfprem_finance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(v_vpfprem_finance_version), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentNumber", vValue:=CStr(v_vInstalmentNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            AddInputParameters(v_vDueDate, v_vFee, v_vTax, v_vCommission, v_vAmount, v_vTransactionCode, v_vStatus, v_vBatchNumber, v_vBatchExportDate, v_vPostedDate, v_vPFTransaction_id)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Changes done as per VB code
                'DirectAdd = gPMConstants.PMEReturnCode.PMFalse
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectEdit Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectEdit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Instalment directly from the database.
    '        Note: The Instalment will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByVal v_vpfprem_finance_cnt As Object = Nothing, Optional ByVal v_vpfprem_finance_version As Object = Nothing, Optional ByVal v_vInstalmentNumber As Object = Nothing) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters

                m_lReturn = .Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(v_vpfprem_finance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(v_vpfprem_finance_version), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                m_lReturn = .Parameters.Add(sName:="InstalmentNumber", vValue:=CStr(v_vInstalmentNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Instalments and populate the Collection
    '
    ' History
    ' PSL 12/11/2002 Now does tax and commission
    '
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetDetails(ByVal v_vpfprem_finance_cnt As Object, ByVal v_vpfprem_finance_version As Object, ByRef r_vDetailsArray As Object, ByRef r_lInstalmentsToGo As Integer, ByRef r_lInstalmentsProcessed As Integer, Optional ByRef r_vInstalmentNumber As Object = Nothing, Optional ByRef r_vBatchNumber As Object = Nothing, Optional ByRef r_vDueDate As Object = Nothing, Optional ByRef r_vFilter As Object = Nothing, Optional ByVal v_vLockMode As Object = Nothing, Optional ByRef r_lRemainingInstalments As Integer = 0) As Integer

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Non-optional parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(v_vpfprem_finance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(v_vpfprem_finance_version), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(v_vLockMode)) Or (Not Double.TryParse(CStr(v_vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                v_vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            'Get Details for a single instalment

            If Not Informations.IsNothing(r_vInstalmentNumber) Then

                ' Add optional parameters

                m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentNumber", vValue:=CStr(r_vInstalmentNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, vResultArray:=r_vDetailsArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                'get all Instalments for matching criteria
            Else

                ' Add optional parameters


                m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchNumber", vValue:=If(Informations.IsNothing(r_vBatchNumber), DBNull.Value, r_vBatchNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)



                m_lReturn = m_oDatabase.Parameters.Add(sName:="DueDate", vValue:=If(Informations.IsNothing(r_vDueDate), DBNull.Value, r_vDueDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)



                m_lReturn = m_oDatabase.Parameters.Add(sName:="filter", vValue:=If(Informations.IsNothing(r_vFilter), PFFilterNone, r_vFilter), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, vResultArray:=r_vDetailsArray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    result = gPMConstants.PMEReturnCode.PMError

                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' How many records were selected
                '        lRecordCount = m_oDatabase.Records.Count

                ' Do we have any records ?
                If Not Informations.IsArray(r_vDetailsArray) Then
                    '        If (lRecordCount < 1) Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, set the Business Properties
                '        r_lInstalmentsToGo = m_oDatabase.Records.Item(1).Fields.Item("NoOfInstallments").Value - _
                ''            m_oDatabase.Records.Item(1).Fields.Item("InstalmentsProcessed").Value
                '        r_lInstalmentsProcessed = m_oDatabase.Records.Item(1).Fields.Item("InstalmentsProcessed").Value

                If r_vDetailsArray.GetUpperBound(1) > 0 Then

                    r_lInstalmentsToGo = CInt(r_vDetailsArray(11, 1))

                    r_lInstalmentsProcessed = CInt(r_vDetailsArray(12, 1))
                    r_lRemainingInstalments = CInt(r_vDetailsArray(22, 1))
                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetInstalments
    '
    ' Description: This function returns instalments for the given plan
    '
    ' History:
    '   PF060901 - Created
    ' ***************************************************************** '
    'Developer Guide No. 101
    'Public Function GetInstalments(ByVal v_lFinancePlanCnt As Integer, ByVal v_lFinancePlanVersion As Integer, ByRef r_vFinancePlanArray As Object, Optional ByVal v_lBatchNumber As Object = Nothing, Optional ByVal v_dtDueDate As Object = Nothing, Optional ByVal v_lFilter As Object = Nothing) As Integer
    Public Function GetInstalments(ByVal v_lFinancePlanCnt As Object, ByVal v_lFinancePlanVersion As Object, ByRef r_vFinancePlanArray(,) As Object, Optional ByVal v_lBatchNumber As Object = Nothing, Optional ByVal v_dtDueDate As Object = Nothing, Optional ByVal v_lFilter As Object = Nothing) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build database query to retrieve items
            m_oDatabase.Parameters.Clear()

            ' Finance Plan Cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_cnt", vValue:=v_lFinancePlanCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            ' Finance Plan Version
            'Developer Guide No 98
            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_version", vValue:=v_lFinancePlanVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Batch Number


            m_lReturn = m_oDatabase.Parameters.Add(sName:="BatchNumber", vValue:=If(Informations.IsNothing(v_lBatchNumber), DBNull.Value, v_lBatchNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Due Date


            m_lReturn = m_oDatabase.Parameters.Add(sName:="DueDate", vValue:=If(Informations.IsNothing(v_dtDueDate), DBNull.Value, v_dtDueDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            ' Filter


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Filter", vValue:=If(Informations.IsNothing(v_lFilter), DBNull.Value, v_lFilter), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0, vResultArray:=r_vFinancePlanArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetStatusList
    '
    ' Description:  Returns an array with a list of the status codes
    '
    ' History: 14/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function GetStatusList(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spe_PFInstalments_selstatus", sSQLName:="Select Status", bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMError
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatusList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatusList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetStatusCodeList
    '
    ' Description: This function returns instalments for the given plan
    '
    ' Return Array:
    '   0 - pfinstalments_status_id
    '   1 - code
    '   2 - description
    '
    ' History:
    '   PF060901 - Created
    ' ***************************************************************** '
    Public Function GetStatusCodeList(ByRef r_vStatusListArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build database query to retrieve items
            m_oDatabase.Parameters.Clear()

            ' Execute the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllStatusSQL, sSQLName:=ACGetAllStatusName, bStoredProcedure:=ACGetAllStatusStored, lNumberRecords:=0, vResultArray:=r_vStatusListArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetStatusCodeList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetStatusCodeList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetTransactionCodeList
    '
    ' Description: This function returns instalments for the given plan
    '
    ' Return Array:
    '   0 - pfinstalments_transaction_id
    '   1 - code
    '   2 - description
    '
    ' History:
    '   PF060901 - Created
    ' ***************************************************************** '
    Public Function GetTransactionCodeList(ByRef r_vTransactionListArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Build database query to retrieve items
            m_oDatabase.Parameters.Clear()

            ' Execute the query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllTransCodeSQL, sSQLName:=ACGetAllTransCodeName, bStoredProcedure:=ACGetAllTransCodeStored, lNumberRecords:=0, vResultArray:=r_vTransactionListArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransactionCodeList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionCodeList", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ***************************************************************** '
    '
    ' Name: ChangeStatus
    '
    ' Description:  Changes the status and transaction code of a
    '               single instalment record
    '
    ' History: 13/08/2001 DD - Created.
    '          DD 19/06/2003: Rewritten for 1.9 Instalments
    '
    ' ***************************************************************** '
    Public Function ChangeStatus(ByVal lPFInstalmentsID As Integer, Optional ByRef nStatus As Integer = 0, Optional ByRef nTransactionCode As Object = Nothing, Optional ByVal bIsUsingDocRef As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim sParams As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()
            sParams = ""

            'Get the parameters set
            m_oDatabase.Parameters.Add("pfinstalments_id", CStr(lPFInstalmentsID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            'Developer Guide No 85
            m_oDatabase.Parameters.Add("Status", If(Informations.IsNothing(nStatus), DBNull.Value, CStr(nStatus)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)



            'Developer Guide No.: 85 (15-09-2010)
            m_oDatabase.Parameters.Add("TransactionCode", If(Informations.IsNothing(nTransactionCode) OrElse Convert.IsDBNull(nTransactionCode), DBNull.Value, nTransactionCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_oDatabase.Parameters.Add("isUsingDocRef", bIsUsingDocRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.SQLAction("spe_PFInstalments_updstatus", "PFInstalments Update Status", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ChangeStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ChangeStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CreateBatch
    '
    ' Description:  Takes a set of Instalments due on a date and creates
    '               a new batch number and marks them
    '
    ' History: 06/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function CreateBatch(ByRef dtDueDate As Date, ByRef nPaymentMethod As Integer, ByRef nBatchNumber As Integer, ByRef lRecordCount As Integer) As Integer

        Dim result As Integer = 0
        Try

            BeginTrans()

            'Clear the parameter list
            m_oDatabase.Parameters.Clear()

            'add the parameters
            m_lReturn = m_oDatabase.Parameters.Add("PaymentMethod", CStr(nPaymentMethod), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("DueDate", dtDueDate.ToString, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add("BatchNumber", CStr(0), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'execute the Stored Procedure
            m_lReturn = m_oDatabase.SQLAction(ACCreateBatchSQL, ACCreateBatchName, ACCreateBatchStored, lRecordCount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                nBatchNumber = m_oDatabase.Parameters.Item("BatchNumber").Value
                CommitTrans()
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateBatch Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatch", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        RollbackTrans()
        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateInstalments
    '
    ' Description: Creates a set of instalments for a Plan
    '
    ' History: 06/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    'Public Function CreateInstalmentsold(ByVal v_nNumberOfInstalments As Integer, _
    ''    ByVal v_dFirstInstalment As Double, ByVal v_dFirstFee As Double, _
    ''    ByVal v_dOtherInstalment As Double, ByVal v_dOtherFee As Double, _
    ''    ByVal v_dtStartDate As Date, _
    ''    ByVal v_sPeriodType As String, ByVal v_sPeriodValue As String, _
    ''    ByVal v_vpfprem_finance_cnt As Variant, _
    ''    ByVal v_vpfprem_finance_version As Variant) As Long
    '
    'Dim nInstalment As Integer
    'Dim dtCurrentDate As Date
    'Dim nDelimetedValues As Integer
    'Dim vResult As Variant
    'Dim bTransStarted As Boolean
    '
    '    On Error GoTo Err_CreateInstalments
    '
    '    bTransStarted = False
    '
    '    If DateDiff("d", Now, v_dtStartDate) < PFDirectDebitDelay Then
    '        ' Instalments cannot start within 10 days
    '
    '        CreateInstalmentsold = PMFalse
    '
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="You cannot create instalments beginning within 10 days.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CreateInstalments", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '        Exit Function
    '    End If
    '
    '    'Check to see if existing Instalments have been paid
    '    With m_oDatabase
    '        .Parameters.Clear
    '        m_lReturn = .Parameters.Add("pfprem_finance_cnt", _
    ''            v_vpfprem_finance_cnt, PMParamInput, PMLong)
    '        m_lReturn = .Parameters.Add("pfprem_finance_version", _
    ''            v_vpfprem_finance_version, PMParamInput, PMLong)
    '        m_lReturn = .Parameters.Add("filter", _
    ''            PFFilterGetPaidOnly, PMParamInput, PMLong)
    '        m_lReturn = .Parameters.Add("batchnumber", _
    ''            Null, PMParamInput, PMLong)
    '        m_lReturn = .Parameters.Add("duedate", _
    ''            Null, PMParamInput, PMLong)
    '
    '        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, _
    ''            sSQLName:=ACGetAllDetailsName, _
    ''            bStoredProcedure:=ACGetAllDetailsStored, _
    ''            lNumberRecords:=PMAllRecords, vResultArray:=vResult)
    '    End With
    '
    '    If IsArray(vResult) Then
    '        'There are actioned records - the instalments cannot be re-created
    '        CreateInstalmentsold = PMError
    '
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="You cannot re-create instalments that have already been actioned.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CreateInstalments", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '        Exit Function
    '    End If
    '
    '    'Use a transaction to protect the data
    '    m_lReturn = BeginTrans()
    '    If m_lReturn <> PMTrue Then
    '        CreateInstalmentsold = PMFalse
    '        Exit Function
    '    Else
    '        bTransStarted = True
    '    End If
    '
    '    With m_oDatabase
    '        .Parameters.Clear
    '        m_lReturn = .Parameters.Add("finance_cnt", v_vpfprem_finance_cnt, PMParamInput, PMLong)
    '        m_lReturn = .Parameters.Add("finance_version", v_vpfprem_finance_version, PMParamInput, PMLong)
    '
    '        m_lReturn = .SQLAction(sSQL:="{call spe_PFInstalments_Delete (?,?)}", _
    ''            sSQLName:="PFInstalmentsDelete", _
    ''            bStoredProcedure:=True)
    '    End With
    '
    '    If m_lReturn <> PMTrue Then
    '        'Problem deleting
    '        CreateInstalmentsold = PMFalse
    '
    '        LogMessage m_sUsername, _
    ''            iType:=PMLogOnError, _
    ''            sMsg:="Cannot delete Instalments before recreate.", _
    ''            vApp:=ACApp, _
    ''            vClass:=ACClass, _
    ''            vMethod:="CreateInstalments", _
    ''            vErrNo:=Err.Number, _
    ''            vErrDesc:=Err.Description
    '
    '        RollbackTrans
    '        Exit Function
    '    End If
    '
    '    dtCurrentDate = v_dtStartDate
    '    nDelimetedValues = CountOfDelimeters(v_sPeriodValue) + 1
    '
    '    'loop through, creating the instalments
    '    'an additional one will be created with instalment number 0
    '    For nInstalment = 0 To v_nNumberOfInstalments
    '
    '        If nInstalment = 0 Then
    '            'Special entry to initiate the DDM
    '            'due today so that it goes asap
    '
    '            m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_vpfprem_finance_cnt, _
    ''                v_vpfprem_finance_version:=v_vpfprem_finance_version, _
    ''                v_vInstalmentNumber:=nInstalment, _
    ''                v_vFee:=0, _
    ''                v_vAmount:=0, _
    ''                v_vStatus:=PFStatusNew, _
    ''                v_vTransactionCode:=PFTransactionCreate, _
    ''                v_vDueDate:=Date, v_vPFTransaction_id:=0)
    '        Else
    '            If nInstalment = 1 Then
    '                'First one
    '                m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_vpfprem_finance_cnt, _
    ''                    v_vpfprem_finance_version:=v_vpfprem_finance_version, _
    ''                    v_vInstalmentNumber:=nInstalment, _
    ''                    v_vFee:=v_dFirstFee, _
    ''                    v_vAmount:=v_dFirstInstalment, _
    ''                    v_vStatus:=PFStatusNew, _
    ''                    v_vTransactionCode:=PFTransactionFirst, _
    ''                    v_vDueDate:=dtCurrentDate, v_vPFTransaction_id:=0)
    '            ElseIf nInstalment = v_nNumberOfInstalments Then
    '                'Last one
    '                m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_vpfprem_finance_cnt, _
    ''                    v_vpfprem_finance_version:=v_vpfprem_finance_version, _
    ''                    v_vInstalmentNumber:=nInstalment, _
    ''                    v_vFee:=v_dOtherFee, _
    ''                    v_vAmount:=v_dOtherInstalment, _
    ''                    v_vStatus:=PFStatusNew, _
    ''                    v_vTransactionCode:=PFTransactionLast, _
    ''                    v_vDueDate:=dtCurrentDate, v_vPFTransaction_id:=0)
    '                '??? Do we mark this as last???
    '            Else
    '                'Rest
    '                m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_vpfprem_finance_cnt, _
    ''                    v_vpfprem_finance_version:=v_vpfprem_finance_version, _
    ''                    v_vInstalmentNumber:=nInstalment, _
    ''                    v_vFee:=v_dOtherFee, _
    ''                    v_vAmount:=v_dOtherInstalment, _
    ''                    v_vStatus:=PFStatusNew, _
    ''                    v_vTransactionCode:=PFTransactionOngoing, _
    ''                    v_vDueDate:=dtCurrentDate, v_vPFTransaction_id:=0)
    '            End If
    '
    '            'move to the next date
    '            dtCurrentDate = CalculateNextDate(dtCurrentDate, v_sPeriodType)
    '        End If
    '
    '        If m_lReturn <> PMTrue Then
    '            'Add failure
    '            CreateInstalmentsold = PMFalse
    '
    '            LogMessage m_sUsername, _
    ''                iType:=PMLogOnError, _
    ''                sMsg:="Cannot add instalment.", _
    ''                vApp:=ACApp, _
    ''                vClass:=ACClass, _
    ''                vMethod:="CreateInstalments", _
    ''                vErrNo:=Err.Number, _
    ''                vErrDesc:=Err.Description
    '
    '            RollbackTrans
    '            Exit Function
    '        End If
    '    Next nInstalment
    '
    '    'Commit the lot
    '    CommitTrans
    '    CreateInstalments = PMTrue
    '
    '    Exit Function
    '
    'Err_CreateInstalments:
    '
    '    CreateInstalmentsold = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="CreateInstalments Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="CreateInstalments", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    If bTransStarted Then
    '        RollbackTrans
    '    End If
    '
    '    Exit Function
    '
    'End Function


    ' ***************************************************************** '
    '
    ' Name: MergeInstalments
    '
    ' Description:  Takes instalments from current plan and merges to a
    '               parent plan along with its other children.
    '
    ' History: 06/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function MergeInstalments(ByVal v_vpfprem_finance_cnt As Integer, ByVal v_vpfprem_finance_version As Integer) As Integer
        Dim result As Integer = 0
        Dim nParentPlanCnt, nParentPlanVersion As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim nInstalment, nInstalments As Integer
        Dim dtDueDate As Date
        Dim dFee, dTax, dCommission, dAmount As Double
        Dim nTransactionCode As Integer
        Dim bTransStarted As Boolean
        Dim lInstalmentsToGo, lInstalmentsProcessed As Integer
        Dim bFirstDone As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'See if this Plan is a parent or child
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add("financeplancnt", CStr(v_vpfprem_finance_cnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_oDatabase.Parameters.Add("financeplanversion", CStr(v_vpfprem_finance_version), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'Developer Guide No 39
            'm_lReturn = m_oDatabase.SQLSelect("{call spu_PFPremiumFinance_sel_single (?,?)}", "Select Single Finance Plan", True, , vResultArray)
            m_lReturn = m_oDatabase.SQLSelect("spu_PFPremiumFinance_sel_single", "Select Single Finance Plan", True, , vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Locate Finance Plan failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            If CDbl(vResultArray(bSIRPremFinConst.k_PFPlanIsParentPlan, 0)) = 1 Then
                'this is a parent - create an array with the child instalments
                nParentPlanCnt = v_vpfprem_finance_cnt
                nParentPlanVersion = v_vpfprem_finance_version
            ElseIf CStr(vResultArray(bSIRPremFinConst.k_PFPlanParentPlanCnt, 0)) <> "" Then
                'this is a child of a parent - create an array with the other children

                nParentPlanCnt = CInt(vResultArray(bSIRPremFinConst.k_PFPlanParentPlanCnt, 0))

                nParentPlanVersion = CInt(vResultArray(bSIRPremFinConst.k_PFPlanParentPlanVersion, 0))
            Else
                'this is a single plan - no merge necessary
                Return result
            End If

            'Select the total instalments
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add("pfprem_finance_cnt", CStr(nParentPlanCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_oDatabase.Parameters.Add("pfprem_finance_version", CStr(nParentPlanVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Use a transaction to protect the data
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                bTransStarted = True
            End If

            'delete the unpaid instalments
            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLAction("spe_PFInstalments_del", "Delete unpaid parent Instalments", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete unpaid parent Instalments failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'get the list of unpaid child instalments
            'Developer Guide No 39
            m_lReturn = m_oDatabase.SQLSelect("spu_PFInstalments_children", "Select Child Instalments", True, gPMConstants.PMAllRecords, vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vResultArray) Then
                RollbackTrans()
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Locate Child Instalments failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Read in the paid set to work out the next
            'instalment number
            v_vpfprem_finance_cnt = nParentPlanCnt
            v_vpfprem_finance_version = nParentPlanVersion

            GetDetails(v_vpfprem_finance_cnt, v_vpfprem_finance_version, vResultArray, lInstalmentsToGo, lInstalmentsProcessed, r_vFilter:=PFFilterGetPaidOnly)

            'Loop through the instalments

            nInstalments = vResultArray.GetUpperBound(1)

            dtDueDate = CDate(vResultArray(ChildDueDate_COL, 0))
            dFee = 0
            dTax = 0
            dCommission = 0
            dAmount = 0
            nTransactionCode = 0
            nInstalment = 1

            For nItem As Integer = 0 To nInstalments
                'Those instalments that fall on the same day are merged

                If CDate(vResultArray(ChildDueDate_COL, nItem)) <> dtDueDate Then
                    'We're on to the next due date so save what we've got
                    If nTransactionCode = 0 And (Not bFirstDone) Then
                        'this is the first so insert a Create DDM for today

                        lReturn = DirectAdd(v_vpfprem_finance_cnt, v_vpfprem_finance_version, 0, DateTime.Today, 0, 0, 0, 0, PFTransactionCreate, v_vStatus:=bSIRPremFinConst.PFStatusNew)
                        If lReturn = gPMConstants.PMEReturnCode.PMError Then Throw New Exception()

                        'and mark the transaction as the first
                        nTransactionCode = PFTransactionFirst

                        bFirstDone = True
                    Else
                        'this is in the middle section
                        nTransactionCode = PFTransactionOngoing
                    End If

                    lReturn = DirectAdd(v_vpfprem_finance_cnt, v_vpfprem_finance_version, nInstalment, dtDueDate, dFee, dTax, dCommission, dAmount, nTransactionCode)
                    If lReturn = gPMConstants.PMEReturnCode.PMError Then Throw New Exception()

                    'Reset the total
                    dFee = 0
                    dAmount = 0
                    dTax = 0
                    dCommission = 0
                    nInstalment += 1
                End If

                'Merge figures

                dtDueDate = CDate(vResultArray(ChildDueDate_COL, nItem))

                dFee += CDbl(vResultArray(ChildFee_COL, nItem))

                dTax += CDbl(vResultArray(ChildTax_COL, nItem))

                dCommission += CDbl(vResultArray(ChildCommission_COL, nItem))

                dAmount += CDbl(vResultArray(ChildAmount_COL, nItem))
            Next nItem

            'Add the last one
            lReturn = DirectAdd(v_vpfprem_finance_cnt, v_vpfprem_finance_version, nInstalment, dtDueDate, dFee, dTax, dCommission, dAmount, nTransactionCode)
            If lReturn = gPMConstants.PMEReturnCode.PMError Then Throw New Exception()

            CommitTrans()

            'Clean up
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="MergeInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="MergeInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If bTransStarted Then
                RollbackTrans()
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PostInstalments
    '
    ' Description:  Posts the Pending instalments within a batch to Orion.
    '
    ' History: 10/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function PostInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsPosted As Integer, ByRef r_lFailedPosting As Integer, Optional ByVal v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim nStart, nEnd As Integer
        Dim lInstalmentNo As Integer
        Dim bDoStatusUpdate As Boolean
        Dim lInstalmentTransDetailID As Integer

        Try

            r_lFailedPosting = gPMConstants.PMEReturnCode.PMFalse

            BeginTrans()

            'Select the Batch of records for posting
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("BatchID", CStr(lBatchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("ForRecall", CStr(0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                .Parameters.Add("INCDocumentRef", v_sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                'Developer Guide No.: 39
                m_lReturn = .SQLSelect("spe_PFInstalments_selectbatch", "Select Instalments Batch", True, gPMConstants.PMAllRecords, vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get the result from call to spu_PFInstalments_selectbatch", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Not Informations.IsArray(vResultArray) Then
                'There is nothing to do
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim bPaymentHubEnabled As Boolean = False
            Dim sOptionValue As String = "0"
            If CallingAppName.ToString.ToUpper = "SIRIUSEXPORT" Then
                m_lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionPaymentHubEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    bPaymentHubEnabled = (sOptionValue = "1")
                End If
            End If
            'TN20010912 - change 1 to 2

            nStart = vResultArray.GetLowerBound(1)

            nEnd = vResultArray.GetUpperBound(1)

            For nRecord As Integer = nStart To nEnd

                bDoStatusUpdate = True
                'only post child transactions

                If CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentTransStatusID, nRecord)).Trim() <> "0C" And CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentTransStatusID, nRecord)).Trim() <> "0N" Then
                    If ToSafeLong(vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord), 0) <> 0 Then

                        lInstalmentNo = CInt(vResultArray(gHUBSpokeConstants.eddPFInstalmentNo, nRecord))

                        If CallingAppName.ToString.ToUpper = "SIRIUSEXPORT" AndAlso bPaymentHubEnabled AndAlso ToSafeInteger(vResultArray(gHUBSpokeConstants.eddPFInstalmentViaPaymentHub, nRecord), 0) <> 0 Then
                            m_lReturn = ProcessOnlineCardPaymentForInstalment(v_nPFInstalmentsId:=lInstalmentNo, v_dPremiumAmount:=CDec(vResultArray(gHUBSpokeConstants.eddAmount, nRecord)))
                            If m_lReturn = PMEReturnCode.PMFail Then
                                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessOnlineCardPaymentForInstalment - Instalment with instalment id " & vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord) & " has failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOnlineCardPaymentForInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            End If
                            If m_lReturn = PMEReturnCode.PMTrue Then
                                m_lReturn = CType(PostInstalmentToOrion(v_cAmount:=CDec(vResultArray(gHUBSpokeConstants.eddAmount, nRecord)), v_lPlanTransDetailID:=CInt(vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord)), r_lInstalmentTransDetailId:=lInstalmentTransDetailID, v_lInstalmentId:=CInt(vResultArray(gHUBSpokeConstants.eddPFInstalmentID, nRecord)), v_lInstallmentNo:=lInstalmentNo), gPMConstants.PMEReturnCode)
                            End If
                        Else
                            m_lReturn = CType(PostInstalmentToOrion(v_cAmount:=CDec(vResultArray(gHUBSpokeConstants.eddAmount, nRecord)), v_lPlanTransDetailID:=CInt(vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord)), r_lInstalmentTransDetailId:=lInstalmentTransDetailID, v_lInstalmentId:=CInt(vResultArray(gHUBSpokeConstants.eddPFInstalmentID, nRecord)), v_lInstallmentNo:=lInstalmentNo), gPMConstants.PMEReturnCode)
                        End If
                    Else
                        'No corresponding posting. Data error
                        m_lReturn = PMEReturnCode.PMTrue
                        lInstalmentTransDetailID = 0
                        bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Instalment plan had no corresponding transaction. Client: " + ToSafeString(vResultArray(eddClientCode, nRecord)), vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If
                Else
                    If CStr(vResultArray(eddPFInstalmentTransStatusID, nRecord)).Trim() <> "0C" And CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentTransStatusID, nRecord)).Trim() <> "0N" And CDbl(vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord)) > 0 Then
                        bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="PostInstalments - Instalment with instalment id " & vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord) & " has no Instalment Plan transaction ID", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        bDoStatusUpdate = False
                    End If
                    lInstalmentTransDetailID = 0
                End If

                'only mark as posted if its succesful
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue And bDoStatusUpdate = True Then
                    'Mark the Instalment as posted
                    With m_oDatabase

                        .Parameters.Clear()

                        .Parameters.Add("pfinstalments_id", CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentID, nRecord)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        m_oDatabase.Parameters.Add("TransactionID", CStr(lInstalmentTransDetailID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACPostItemSQL, sSQLName:=ACPostItemName, bStoredProcedure:=ACPostItemStored)

                    End With

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Marking Batch as posted failure
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostInstalments failed to mark instalment as posted.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    r_lFailedPosting = gPMConstants.PMEReturnCode.PMTrue
                End If
                m_lReturn = CType(StatusUpdate(v_vInstalmentID:=CStr(vResultArray(gHUBSpokeConstants.eddPFInstalmentID, nRecord)), v_nPlanTransactionId:=CInt(vResultArray(gHUBSpokeConstants.eddPFPlanTransactionID, nRecord))), gPMConstants.PMEReturnCode)
                lPrevPremiumFinanceCnt = 0

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Next nRecord

            'commit the changes
            CommitTrans()

            r_lRecordsPosted = nEnd + 1


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()

            Return result




            Return result
        End Try
    End Function
    Private Function ProcessOnlineCardPaymentForInstalment(ByVal v_nPFInstalmentsId As Integer, ByVal v_dPremiumAmount As Decimal) As Integer

        Dim sTransactionId As String = ""
        Dim sIntegerationId As String = ""
        Dim sTokenId As String = ""
        Dim sPaymentMethod As String = ""
        Dim sMediaType As String = ""
        Dim dPremiumAmount As Decimal = 0
        Dim sCurrencyCode As String = ""
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim nPartyCnt As Integer
        Dim obPaymenthub As bSIRPaymentHubWrapper.Business = Nothing

        ' If bPaymentHubEnabled Then
        Try

            nResult = GetCreditCardDetails(v_nPFInstalmentId:=v_nPFInstalmentsId, r_sIntegerationId:=sIntegerationId,
                                                     r_sTokenId:=sTokenId, r_sMediaTypeCode:=sMediaType, r_sCurrencyCode:=sCurrencyCode, r_nPartyCnt:=nPartyCnt)

            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If sMediaType IsNot Nothing AndAlso sMediaType.ToUpper = "OCP" Then

                obPaymenthub = New bSIRPaymentHubWrapper.Business()
                nResult = obPaymenthub.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If nResult <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

                Dim sStatus As String
                Dim oPaymentHubResponseParameters As New bSIRPaymentHubWrapper.PaymentHubResponseParameters()
                sTransactionId = System.Guid.NewGuid.ToString()
                sStatus = obPaymenthub.ProcessPurchase(strTransactionID:=sTransactionId, IntegrationToken:=sIntegerationId,
                                                           TokenID:=sTokenId, oPaymentHubResponseParameters:=oPaymentHubResponseParameters,
                                                           v_dTransactionValue:=v_dPremiumAmount, v_sTransactionCurrencyCode:=sCurrencyCode, v_nPartyCnt:=nPartyCnt)

                If sStatus Is Nothing OrElse sStatus.ToUpper <> "0" Then
                    'Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessPurchase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessPurchase", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    nResult = PMEReturnCode.PMFail
                Else
                    nResult = PMEReturnCode.PMTrue
                End If
            End If

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="ProcessOnlineCardPaymentForInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessOnlineCardPaymentForInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return nResult
        End Try
        Return nResult
    End Function
    Private Function GetCreditCardDetails(ByVal v_nPFInstalmentId As Integer, ByRef r_sMediaTypeCode As String, ByRef r_sCurrencyCode As String, ByRef r_sTokenId As String,
                                          ByRef r_sIntegerationId As String, ByRef r_nPartyCnt As Integer) As Integer
        Dim nResult As Integer = 0
        Dim oResult(,) As Object = Nothing

        Try

            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="nPFInstalmentsId", vValue:=CStr(v_nPFInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetInstalmentPaymentHubDetailsSQL, sSQLName:=kGetInstalmentPaymentHubDetailsName, bStoredProcedure:=kGetInstalmentPaymentHubDetailsStored, vResultArray:=oResult)
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oResult) Then
                Return PMEReturnCode.PMNotFound
            End If

            r_sMediaTypeCode = ToSafeString(oResult(kIndexMediaType, 0), "")
            r_sCurrencyCode = ToSafeString(oResult(kIndexMediaType, 0), "")
            r_sIntegerationId = ToSafeString(oResult(kIndexIntegerationToken, 0), "")
            r_sTokenId = ToSafeString(oResult(kIndexTokenId, 0), "")
            r_nPartyCnt = ToSafeInteger(oResult(kIndexPartyCnt, 0), 0)
            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetCreditCardDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCreditCardDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try

    End Function
    ' ***************************************************************** '
    '
    ' Name: PostMultipleInstalments
    '
    ' Description:  Posts the instalmentID's passed in either as a single
    '               dimension array or a long value for
    '               single postings, returns  r_vTransDetail (an array of equal
    '               dimension containing the resultant transdetail ID's or a single
    '               value for single instalment)
    '
    ' History: 20/01/2003 Created. STEVE WATTON
    '          03/06/2003 Changed name of this function to PostMultipleInstalments
    '                     as it is now called from the close batch import.
    '          28/12/2004   Added one function StatusUpdate and called JT
    ' ***************************************************************** '
    'Developer Guide No 33
    Public Function PostMultipleInstalments(ByVal v_vInstalmentID As Object, Optional ByRef r_vTransDetailID As Object = Nothing, Optional ByVal v_lCashDrawerID As Integer = 0,
                                            Optional ByVal v_dtTransactionDate As Date = #12/30/1899#, Optional ByVal v_vCashListItemID As Integer = 0, Optional ByRef v_lFirstTransDetailID As Integer = 0, Optional ByVal v_sSpare As String = "", Optional ByVal v_iCashListCurrencyID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lUBound As Integer
        Dim vResultArray(,) As Object = Nothing
        Dim lTempInstTDID, lTempInstID, lFirstInstTDID As Integer
        Dim sWriteOffReason As String = String.Empty
        Dim nWriteOffTransDetailId As Integer
        Dim nWriteOffReasonId As Integer
        Dim oWriteOffReason(,) As Object = Nothing
        Dim oResultWriteOffArray(,) As Object = Nothing
        ' AAB - 03-Jul-2003 - Changed the values to accomodate new array
        '                     BankShortCode is no longer used
        Const ACAmount As Integer = 0
        'Const ACBankShortCode As Integer = 1
        Const ACPlanTransactionID As Integer = 1

        Try

            'not sure whether passed a single dimension array containing multiple values
            'or a single value
            'AAB - 08-12-03 changed to 2 dem array per Danny Davis to support partial pay.
            If Informations.IsArray(v_vInstalmentID) Then
                lUBound = v_vInstalmentID.GetUpperBound(1)
            Else

                If Val(CStr(v_vInstalmentID)) <> 0 Then
                    lUBound = 0
                    'copy value to temp variable

                    lTempInstID = CInt(v_vInstalmentID)
                    'redeclare as array
                    ReDim v_vInstalmentID(lUBound)
                    'copy value back into the first element of array

                    v_vInstalmentID(lUBound) = lTempInstID
                Else
                    'no valid agrument passed
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            'we now have an array which will have at least on dimension

            'begin a transaction
            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For lCount As Integer = 0 To lUBound

                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("instalmentid", CStr(CInt(v_vInstalmentID(0, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .SQLSelect(sSQL:=ACSelectInstalmentPostDetailsSQL, sSQLName:=ACSelectInstalmentPostDetailsName, bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End With

                If Informations.IsArray(vResultArray) Then
                    If (v_vInstalmentID.GetUpperBound(0) >= 5 AndAlso ToSafeInteger(v_vInstalmentID(5, lCount)) > 0) Then
                        With m_oDatabase
                            .Parameters.Clear()
                            .Parameters.Add("write_off_reason_id", CStr(CInt(v_vInstalmentID(5, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                            m_lReturn = .SQLSelect(sSQL:=kSelectWriteOffReasonSQL, sSQLName:=kSelectWriteOffReasonName, bStoredProcedure:=True, vResultArray:=oResultWriteOffArray, lNumberRecords:=gPMConstants.PMAllRecords)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RollbackTrans()
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If

                            If Informations.IsArray(oResultWriteOffArray) Then
                                sWriteOffReason = oResultWriteOffArray(1, 0)
                                nWriteOffReasonId = ToSafeInteger(v_vInstalmentID(5, lCount))
                            End If
                        End With
                    End If
                    Dim nPremiumFinanceCnt, nPremiumFinanceVersion As Integer
                    Dim nSpreadRI As Integer
                    Dim crTaxAmount As Decimal
                    Dim nTotalInstalment As Integer = 0
                    Dim m_nSpreadCommission As Integer
                    Dim nBaseCurrencyID As Integer
                    m_lReturn = GetPFFromInstalmentsID(v_nInstalmentsID:=CInt(v_vInstalmentID(0, lCount)), r_nPremiumFinanceCnt:=nPremiumFinanceCnt, r_nPremiumFinanceVersion:=nPremiumFinanceVersion, r_nSpreadCommission:=m_nSpreadCommission, r_nSpreadRI:=nSpreadRI, r_crTaxAmount:=crTaxAmount, r_nTotalInstalment:=nTotalInstalment)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("GetPFFromInstalmentsID", "v_lInstalmentsID:=" & CInt(v_vInstalmentID(0, lCount)), gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'call existing functionality to post the instalment.
                    Dim dInsAmount As Decimal = ToSafeDecimal(vResultArray(ACAmount, 0))
                    Dim dPartialAmtInReceiptCurrency As Decimal = 0

                    If v_vInstalmentID.GetUpperBound(0) > 2 Then
                        dInsAmount = ToSafeDecimal(v_vInstalmentID(3, lCount))

                        If v_vInstalmentID(1, lCount) <> 0 Then

                            dPartialAmtInReceiptCurrency = dInsAmount


                            'dPartialAmtInReceiptCurrency = v_vInstalmentID(4, lCount)
                        End If
                    End If
                    'ConvertCurrencyStringToValue(vResultArray(ACAmount, 0))
                    If (v_vInstalmentID.GetUpperBound(0) >= 5 AndAlso ToSafeInteger(v_vInstalmentID(5, lCount)) > 0) Then
                        dInsAmount = ConvertCurrencyStringToValue(vResultArray(ACAmount, 0))
                    End If
                    m_lReturn = CType(PostInstalmentToOrion(v_cAmount:=dInsAmount, v_lPlanTransDetailID:=CInt(vResultArray(ACPlanTransactionID, 0)), r_lInstalmentTransDetailId:=lTempInstTDID,
                                                            v_lInstalmentId:=CInt(v_vInstalmentID(0, lCount)), v_lCashDrawerID:=v_lCashDrawerID, v_cPartialPayAmount:=CDec(v_vInstalmentID(1, lCount)),
                                                            v_bWriteOff:=CInt(v_vInstalmentID(2, lCount)) = 0, v_dtTransactionDate:=v_dtTransactionDate, v_sSpare:=v_sSpare, v_iCashListCurrencyID:=v_iCashListCurrencyID, v_dPartialAmtInReceiptCurrency:=dPartialAmtInReceiptCurrency,
                                                            sWriteOffReason:=sWriteOffReason, nWriteoffTransDetailId:=nWriteOffTransDetailId), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' KG 07/08/03
                    ' Copy of first TransDetailID.
                    If lCount = 0 Then lFirstInstTDID = lTempInstTDID

                    v_lFirstTransDetailID = lFirstInstTDID

                    '30/4/2003 SW CQ904 pass in transaction date
                    'update the instalment record with the transdetail id returned above and change the status to collected

                    m_lReturn = CType(UpdateInstalmentAsPosted(v_lPFInstalmentId:=CInt(v_vInstalmentID(0, lCount)), v_lTransDetailID:=lTempInstTDID, v_dtTransactionDate:=v_dtTransactionDate,
                                                               nWriteOffReasonId:=nWriteOffReasonId, nWriteOffTransDetailId:=nWriteOffTransDetailId), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Else
                    RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(r_vTransDetailID) Then
                    r_vTransDetailID(lCount) = lTempInstTDID
                Else
                    r_vTransDetailID = lTempInstTDID
                End If
            Next

            ' KG 07/08/03
            ' CQ 2056 - Update the CashListItem record with the TransDetailID of the first instalment

            If Not Informations.IsNothing(v_vCashListItemID) Then
                m_lReturn = CType(UpdateCashListItemRecord(lFirstInstTDID, v_vCashListItemID), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If



            'attempt to commit the transaction

            If CommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            For lCount As Integer = 0 To lUBound
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("instalmentid", CStr(CInt(v_vInstalmentID(0, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    m_lReturn = .SQLSelect(sSQL:=ACSelectInstalmentPostDetailsSQL, sSQLName:=ACSelectInstalmentPostDetailsName, bStoredProcedure:=True, vResultArray:=vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RollbackTrans()
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                m_lReturn = CType(StatusUpdate(v_vInstalmentID:=CInt(v_vInstalmentID(0, lCount)), v_nPlanTransactionId:=CInt(vResultArray(ACPlanTransactionID, 0))), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RollbackTrans()
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostMultipleInstalments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostMultipleInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            RollbackTrans()

            Return result

        End Try
    End Function
    Private Function GetSchemeCurrency(ByVal nPremiumFinanceCnt As Integer, ByVal nPremiumFinanceVersion As Integer, ByRef r_nCurrencyID As Integer, ByRef r_dRate As Double) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetSchemeCurrency
        ' PURPOSE: Returns the Base Currency used by the Finance Scheme
        ' AUTHOR: Danny Davis
        ' DATE: 04 August 2004, 09:50 AM
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim nResult As Integer = 0
        Dim oResultArray As Object(,) = Nothing

        nResult = gPMConstants.PMEReturnCode.PMFalse
        r_dRate = 1


        With m_oDatabase

            .Parameters.Clear()
            m_lReturn = .Parameters.Add(sName:="pfprem_finance_cnt", vValue:=CStr(nPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            m_lReturn = .Parameters.Add(sName:="pfprem_finance_version", vValue:=CStr(nPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'Developer Guide No 39
            m_lReturn = .SQLSelect(sSQL:="spu_PFScheme_GetCurrency", sSQLName:="spu_PFScheme_GetCurrency", bStoredProcedure:=True, vResultArray:=oResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If


            If Informations.IsArray(oResultArray) Then
                r_nCurrencyID = ToSafeInteger(oResultArray(5, 0))

                If ToSafeInteger(oResultArray(2, 0)) = 1 Then
                    r_nCurrencyID = ToSafeInteger(oResultArray(0, 0))
                    r_dRate = ToSafeDouble(oResultArray(1, 0))
                End If
            End If
        End With

        nResult = gPMConstants.PMEReturnCode.PMTrue

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: GETPFFromInstalmentsID
    '
    ' Description: Get the Premium Finance Count and version from the Instalments ID
    '
    ' History: 18/12/2002 PSL - Created
    ' AAB-19-September-2003 - Added r_nSpreadRI for RI suspense
    ' ***************************************************************** '
    Private Function GetPFFromInstalmentsID(ByVal v_nInstalmentsID As Integer, ByRef r_nPremiumFinanceCnt As Integer, ByRef r_nPremiumFinanceVersion As Integer, ByRef r_nSpreadCommission As Integer, ByRef r_nSpreadRI As Integer, ByRef r_crTaxAmount As Decimal, ByRef r_nTotalInstalment As Integer) As Integer

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        ' Retrieve in house status for associated scheme
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InstalmentsID", vValue:=CStr(v_nInstalmentsID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPFFromInstalmentsIDSQL, sSQLName:=ACGetPFFromInstalmentsIDName, bStoredProcedure:=ACGetPFFromInstalmentsIDStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        If m_oDatabase.Records.Count() >= 1 Then
            ' Retrieve the return value
            'Developer Guide No 162
            r_nPremiumFinanceCnt = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("pfPrem_Finance_cnt"))
            r_nPremiumFinanceVersion = gPMFunctions.NullToLong(m_oDatabase.Records.Item(0).Fields("pfPrem_Finance_version"))
            r_nSpreadCommission = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("Spread_Commission"))
            r_nSpreadRI = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("Spread_ri"))
            r_crTaxAmount = gPMFunctions.ToSafeCurrency(m_oDatabase.Records.Item(0).Fields("tax"))
            r_nTotalInstalment = gPMFunctions.ToSafeInteger(m_oDatabase.Records.Item(0).Fields("NoOfInstallments"))
        End If
        Return m_lReturn

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateInstalmentAsPosted
    '
    ' Description:  Updates the pftransaction_id and status of an instalment record
    '               folloing a posting
    '
    ' History: 23/01/2003 SW - Created.
    '
    ' ***************************************************************** '
    'sw added transaction date as param CQ 904 01/05/2003
    Public Function UpdateInstalmentAsPosted(ByVal v_lPFInstalmentId As Integer, ByVal v_lTransDetailID As Integer, ByVal v_dtTransactionDate As Date,
                                             nWriteOffReasonId As Integer,
                                             nWriteOffTransDetailId As Integer) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("instalmentid", CStr(v_lPFInstalmentId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("transdetailid", CStr(v_lTransDetailID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("transactiondate", (v_dtTransactionDate), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("writeoffreasonid", CInt(nWriteOffReasonId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.Parameters.Add("writeofftransdetailid", CInt(nWriteOffTransDetailId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

            If m_oDatabase.SQLAction(sSQL:=ACUpdateInstalmentAsPostedSQL, sSQLName:=ACUpdateInstalmentAsPostedName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result



            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateInstalmentAsPosted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInstalmentAsPosted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ''' <summary>
    ''' Recalls a batch of posted instalments
    ''' </summary>
    ''' <param name="lBatchID"></param>
    ''' <param name="r_lRecordsRecalled"></param>
    ''' <param name="r_lFailedRecall"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsRecalled As Integer, ByRef r_lFailedRecall As Integer) As Integer
        Return RecallInstalments(lBatchID:=lBatchID, r_lRecordsRecalled:=r_lRecordsRecalled, r_lFailedRecall:=r_lFailedRecall, v_sDocumentRef:="", nPFInstalmentsID:=0, bRecallInstalmentFromInstalmentMaint:=False, vInstalmentstoRecall:=Nothing)
    End Function
    ''' <summary>
    ''' Recalls a batch of posted instalments
    ''' </summary>
    ''' <param name="lBatchID"></param>
    ''' <param name="r_lRecordsRecalled"></param>
    ''' <param name="r_lFailedRecall"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsRecalled As Integer, ByRef r_lFailedRecall As Integer, ByVal v_sDocumentRef As String) As Integer
        Return RecallInstalments(lBatchID:=lBatchID, r_lRecordsRecalled:=r_lRecordsRecalled, r_lFailedRecall:=r_lFailedRecall, v_sDocumentRef:=v_sDocumentRef, nPFInstalmentsID:=0, bRecallInstalmentFromInstalmentMaint:=False, vInstalmentstoRecall:=Nothing)
    End Function
    ''' <summary>
    ''' Recalls a batch of posted instalments
    ''' </summary>
    ''' <param name="lBatchID"></param>
    ''' <param name="r_lRecordsRecalled"></param>
    ''' <param name="r_lFailedRecall"></param>
    ''' <param name="nPFInstalmentsID"></param>
    ''' <param name="bRecallInstalmentFromInstalmentMaint"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsRecalled As Integer, ByRef r_lFailedRecall As Integer, ByVal nPFInstalmentsID As Integer, ByVal bRecallInstalmentFromInstalmentMaint As Boolean) As Integer
        Return RecallInstalments(lBatchID:=lBatchID, r_lRecordsRecalled:=r_lRecordsRecalled, r_lFailedRecall:=r_lFailedRecall, v_sDocumentRef:="", nPFInstalmentsID:=nPFInstalmentsID, bRecallInstalmentFromInstalmentMaint:=bRecallInstalmentFromInstalmentMaint, vInstalmentstoRecall:=Nothing)
    End Function
    ''' <summary>
    ''' Recalls a batch of posted instalments
    ''' </summary>
    ''' <param name="lBatchID"></param>
    ''' <param name="r_lRecordsRecalled"></param>
    ''' <param name="r_lFailedRecall"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <param name="nPFInstalmentsID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsRecalled As Integer, ByRef r_lFailedRecall As Integer, ByVal v_sDocumentRef As String, ByVal nPFInstalmentsID As Integer) As Integer
        Return RecallInstalments(lBatchID:=lBatchID, r_lRecordsRecalled:=r_lRecordsRecalled, r_lFailedRecall:=r_lFailedRecall, v_sDocumentRef:=v_sDocumentRef, nPFInstalmentsID:=nPFInstalmentsID, bRecallInstalmentFromInstalmentMaint:=False, vInstalmentstoRecall:=Nothing)
    End Function
    ''' <summary>
    ''' Recalls a batch of posted instalments
    ''' </summary>
    ''' <param name="lBatchID"></param>
    ''' <param name="r_lRecordsRecalled"></param>
    ''' <param name="r_lFailedRecall"></param>
    ''' <param name="v_sDocumentRef"></param>
    ''' <param name="vInstalmentstoRecall"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsRecalled As Integer, ByRef r_lFailedRecall As Integer, ByVal v_sDocumentRef As String, Optional ByVal vInstalmentstoRecall As ArrayList = Nothing) As Integer
        Return RecallInstalments(lBatchID:=lBatchID, r_lRecordsRecalled:=r_lRecordsRecalled, r_lFailedRecall:=r_lFailedRecall, v_sDocumentRef:=v_sDocumentRef, nPFInstalmentsID:=0, bRecallInstalmentFromInstalmentMaint:=False, vInstalmentstoRecall:=vInstalmentstoRecall)
    End Function
    ' ***************************************************************** '
    '
    ' Name: RecallInstalments
    '
    ' Description:  Recalls a batch of posted instalments from Orion.
    '
    ' History: 20/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RecallInstalments(ByVal lBatchID As Integer, ByRef r_lRecordsRecalled As Integer, ByRef r_lFailedRecall As Integer, ByVal v_sDocumentRef As String, ByVal nPFInstalmentsID As Integer,
                  ByVal bRecallInstalmentFromInstalmentMaint As Boolean, ByVal vInstalmentstoRecall As ArrayList) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecallInstalments"
        Const kInstalmentRejected As Integer = 2
        Const kPFInstalmentsStatusCollected As String = "Collected"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vBatchInstalments(,) As Object = Nothing
        Dim sCreditControlEnabled As String = ""
        Dim bCreditControlEnabled As Boolean
        Dim lLBound, lUBound As Integer
        Dim bRetryLimitReached As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' get the sytem option value for option "credit control enabled"
            If lBatchID > 0 Then
                lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionCreditControlEnabled, r_sOptionValue:=sCreditControlEnabled, v_iSourceID:=1), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "GetSystemOption :- " & kSystemOptionCreditControlEnabled & " Failed")
                ElseIf sCreditControlEnabled = "1" Then
                    bCreditControlEnabled = True
                End If
            End If

            ' get the instalments for the specified batch number
            If nPFInstalmentsID = 0 Then
                lReturn = GetInstalmentsFromBatch(lBatchID, vBatchInstalments, v_sDocumentRef)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError(kMethodName, "GetInstalmentsFromBatch Failed to return any instalments for batchid -" & lBatchID, gPMConstants.PMELogLevel.PMLogError)
                    Else
                        gPMFunctions.RaiseError(kMethodName, "GetInstalmentsFromBatch Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            Else
                lReturn = GetInstalmentForRecall(nPFInstalmentsID, vBatchInstalments)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Throw New ApplicationException("GetInstalmentForRecall Failed to return any instalments for pfinstalment_id -" & nPFInstalmentsID)
                    Else
                        Throw New ApplicationException("GetInstalmentForRecall() method Failed")
                    End If
                End If
            End If


            lLBound = vBatchInstalments.GetLowerBound(1)

            lUBound = vBatchInstalments.GetUpperBound(1)

            r_lFailedRecall = gPMConstants.PMEReturnCode.PMFalse
            r_lRecordsRecalled = 0

            ' for each instalment in the batch
            For lInstalment As Integer = lLBound To lUBound
                ' Sankar - PN 61481 - Added a if condition to check whether the status is completed
                Dim bRecallInstalment As Boolean = False

                If CStr(vBatchInstalments(gHUBSpokeConstants.eddPFInstalmentStatusDescription, lInstalment)) = kPFInstalmentsStatusCollected AndAlso ToSafeString(v_sDocumentRef) <> String.Empty Then
                    bRecallInstalment = True
                ElseIf CStr(vBatchInstalments(gHUBSpokeConstants.eddPFInstalmentStatusDescription, lInstalment)) <> kPFInstalmentsStatusCollected AndAlso ToSafeString(v_sDocumentRef) = String.Empty Then
                    bRecallInstalment = True
                ElseIf ToSafeString(vBatchInstalments(gHUBSpokeConstants.eddPFInstalmentStatusDescription, lInstalment)) <> kPFInstalmentsStatusCollected AndAlso Not Informations.IsNothing(vInstalmentstoRecall) AndAlso
                vInstalmentstoRecall.Contains(gPMFunctions.ToSafeInteger(vBatchInstalments.GetValue(gHUBSpokeConstants.eddPFInstalmentID, lInstalment))) AndAlso ToSafeString(v_sDocumentRef) <> String.Empty Then
                    bRecallInstalment = True
                End If
                If bRecallInstalmentFromInstalmentMaint Then
                    bRecallInstalment = True
                End If
                If bRecallInstalment = True Then
                    ' attempt to recall it
                    lReturn = CType(RecallInstalment(v_sPFInstalmentsTransactionCode:=CStr(vBatchInstalments(gHUBSpokeConstants.eddPFInstalmentTransStatusID, lInstalment)).Trim(), v_lPFPlanTransactionID:=CInt(vBatchInstalments(gHUBSpokeConstants.eddPFPlanTransactionID, lInstalment)), v_lPFTransactionId:=gPMFunctions.ToSafeLong((vBatchInstalments(gHUBSpokeConstants.eddTransactionID, lInstalment)), 0), v_lPFInstalmentsId:=gPMFunctions.ToSafeLong((vBatchInstalments(gHUBSpokeConstants.eddPFInstalmentID, lInstalment)), 0), v_lPFInstalmentsStatusId:=gPMFunctions.ToSafeLong((vBatchInstalments(gHUBSpokeConstants.eddPFInstalmentStatusID, lInstalment)), 0), v_sDefaultCreditControlItemReason:="Instalment Recalled Manually", v_bCreditControlEnabled:=bCreditControlEnabled, r_bRetryLimitReached:=bRetryLimitReached, v_lProcessMode:=kInstalmentRejected), gPMConstants.PMEReturnCode)

                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' set indicator to return the fact that at least one item failed to be recalled
                        If lReturn = gPMConstants.PMEReturnCode.PMFail Then
                            r_lFailedRecall = gPMConstants.PMEReturnCode.PMTrue
                        End If
                    Else
                        If Trim$(vBatchInstalments(eddPFInstalmentTransStatusID, lInstalment)) <> "0C" And Trim$(vBatchInstalments(eddPFInstalmentTransStatusID, lInstalment)) <> "0N" And ToSafeLong(vBatchInstalments(eddTransactionID, lInstalment), 0) > 0 Then
                            ' count only when a recall was attempted
                            ' record how many instalments are successfully recalled
                            r_lRecordsRecalled += 1
                        End If
                    End If

                    If bRetryLimitReached Then
                        ' raise a work manager task
                        ' TODO: WASNT PREVIOUSLY SUPPORTED - SO NOT DONE UNTIL
                        ' SOMEONE REQUIRES THIS FUNCTIONALITY FROM THE EXISTING INTERFACE
                    End If
                End If
            Next lInstalment

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetInstalmentsFromBatch
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function GetInstalmentsFromBatch(ByVal v_lBatchID As Integer, ByRef r_vResults(,) As Object, Optional ByVal v_sDocumentRef As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetInstalmentsFromBatch"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="BatchID", v_vValue:=v_lBatchID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="ForRecall", v_vValue:=1, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="ForReview", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)
        m_lReturn = CType(AddInputParameter(v_sName:="INCDocumentRef", v_vValue:=v_sDocumentRef, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

        ' Execute selection Query
        lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInstalmentsFromBatchSQL, sSQLName:=kGetInstalmentsFromBatchName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kGetInstalmentsFromBatchSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Not Informations.IsArray(r_vResults) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result
    End Function
    ''' <summary>
    ''' Recall Posted Instalment
    ''' </summary>
    ''' <param name="v_sPFInstalmentsTransactionCode"></param>
    ''' <param name="v_lPFPlanTransactionID"></param>
    ''' <param name="v_lPFTransactionId"></param>
    ''' <param name="v_lPFInstalmentsId"></param>
    ''' <param name="v_lPFInstalmentsStatusId"></param>
    ''' <param name="v_sDefaultCreditControlItemReason"></param>
    ''' <param name="v_bCreditControlEnabled"></param>
    ''' <param name="r_bRetryLimitReached"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalment(ByVal v_sPFInstalmentsTransactionCode As String, ByVal v_lPFPlanTransactionID As Integer, ByVal v_lPFTransactionId As Integer, ByVal v_lPFInstalmentsId As Integer,
                                     ByVal v_lPFInstalmentsStatusId As Integer, ByVal v_sDefaultCreditControlItemReason As String, ByVal v_bCreditControlEnabled As Boolean, ByRef r_bRetryLimitReached As Boolean) As Integer
        Return RecallInstalment(v_sPFInstalmentsTransactionCode:=v_sPFInstalmentsTransactionCode, v_lPFPlanTransactionID:=v_lPFPlanTransactionID, v_lPFTransactionId:=v_lPFTransactionId, v_lPFInstalmentsId:=v_lPFInstalmentsId,
        v_lPFInstalmentsStatusId:=v_lPFInstalmentsStatusId, v_sDefaultCreditControlItemReason:=v_sDefaultCreditControlItemReason, v_bCreditControlEnabled:=v_bCreditControlEnabled, r_bRetryLimitReached:=r_bRetryLimitReached, v_lProcessMode:=0, bRecallInstalmentFromInstalmentMaint:=False)
    End Function
    ''' <summary>
    ''' Recall Posted Instalment
    ''' </summary>
    ''' <param name="v_sPFInstalmentsTransactionCode"></param>
    ''' <param name="v_lPFPlanTransactionID"></param>
    ''' <param name="v_lPFTransactionId"></param>
    ''' <param name="v_lPFInstalmentsId"></param>
    ''' <param name="v_lPFInstalmentsStatusId"></param>
    ''' <param name="v_sDefaultCreditControlItemReason"></param>
    ''' <param name="v_bCreditControlEnabled"></param>
    ''' <param name="r_bRetryLimitReached"></param>
    ''' <param name="v_lProcessMode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function RecallInstalment(ByVal v_sPFInstalmentsTransactionCode As String, ByVal v_lPFPlanTransactionID As Integer, ByVal v_lPFTransactionId As Integer, ByVal v_lPFInstalmentsId As Integer,
                                     ByVal v_lPFInstalmentsStatusId As Integer, ByVal v_sDefaultCreditControlItemReason As String, ByVal v_bCreditControlEnabled As Boolean, ByRef r_bRetryLimitReached As Boolean, ByVal v_lProcessMode As Integer) As Integer
        Return RecallInstalment(v_sPFInstalmentsTransactionCode:=v_sPFInstalmentsTransactionCode, v_lPFPlanTransactionID:=v_lPFPlanTransactionID, v_lPFTransactionId:=v_lPFTransactionId, v_lPFInstalmentsId:=v_lPFInstalmentsId,
        v_lPFInstalmentsStatusId:=v_lPFInstalmentsStatusId, v_sDefaultCreditControlItemReason:=v_sDefaultCreditControlItemReason, v_bCreditControlEnabled:=v_bCreditControlEnabled, r_bRetryLimitReached:=r_bRetryLimitReached, v_lProcessMode:=v_lProcessMode, bRecallInstalmentFromInstalmentMaint:=False)
    End Function


    ' ***************************************************************** '
    ' Name: RecallInstalment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-06-2007 : Instalment_Import
    ' ***************************************************************** '
    Public Function RecallInstalment(ByVal v_sPFInstalmentsTransactionCode As String, ByVal v_lPFPlanTransactionID As Integer, ByVal v_lPFTransactionId As Integer, ByVal v_lPFInstalmentsId As Integer, ByVal v_lPFInstalmentsStatusId As Integer, ByVal v_sDefaultCreditControlItemReason As String, ByVal v_bCreditControlEnabled As Boolean, ByRef r_bRetryLimitReached As Boolean, ByVal v_lProcessMode As Integer,
                                     ByVal bRecallInstalmentFromInstalmentMaint As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RecallInstalment"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lReversalTransDetailId As Integer
        Dim nInsuranceFileCnt As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            lReturn = GetParentPlanPK(v_lPFInstalmentId:=v_lPFInstalmentsId, r_lPremFinanceCount:=PFFinancePlanCnt, r_lPremFinanceVersion:=PFFinancePlanVersion, r_sPlanStatus:=PlanStatus, r_nInsuranceFileCnt:=nInsuranceFileCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetParentPlanPK Failed", gPMConstants.PMELogLevel.PMLogError)
                Return lReturn
            End If

            ' verify that the transaction was posted before attempting to recall it
            If v_sPFInstalmentsTransactionCode <> "0C" And v_sPFInstalmentsTransactionCode <> "0N" And v_lPFTransactionId > 0 Then

                ' recall instalment from accounts

                lReturn = m_oOrionInstalment.RecallInstalment(v_lPlanTransDetailID:=v_lPFPlanTransactionID, v_lInstalmentTransdetailID:=v_lPFTransactionId)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    result = gPMConstants.PMEReturnCode.PMFail

                    ' recall failed so reset the status on the instalment back to what it was before
                    lReturn = CType(ChangeStatus(v_lPFInstalmentsId, v_lPFInstalmentsStatusId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ChangeStatus Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    Return result

                Else

                    ' clear the transaction details on the instalment

                    lReversalTransDetailId = gPMFunctions.ToSafeLong(m_oOrionInstalment.ReversalTransDetailId, 0)

                    lReturn = CType(ClearInstalmentsTransactionDetails(v_lPFInstalmentsId:=v_lPFInstalmentsId, v_lReversalTransDetailId:=lReversalTransDetailId), gPMConstants.PMEReturnCode)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "ResetInstalmentTransactionDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If v_bCreditControlEnabled AndAlso PlanStatus <> PFStatusIndCancelled AndAlso PlanStatus <> PFStatusIndSuperseded Then

                        ' setup credit control item for instalment
                        lReturn = CType(SetupCreditControlItemForInstalment(v_lPFInstalmentsId, v_sDefaultCreditControlItemReason, v_lProcessMode), gPMConstants.PMEReturnCode)

                        ' dont log or raise any errors if the credit control fails - this may be because
                        ' the user hasnt configured credit control for instalments

                    End If

                    ' see if instalment collection should be retried
                    If Not bRecallInstalmentFromInstalmentMaint AndAlso Convert.ToString(m_sCallingAppName).ToUpper() <> "IPMBFINANCEPLANMAINT" Then
                        lReturn = CType(RetryInstalmentCollection(v_lPFInstalmentsId), gPMConstants.PMEReturnCode)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            If lReturn = gPMConstants.PMEReturnCode.PMFail Then
                                r_bRetryLimitReached = True
                            Else
                                gPMFunctions.RaiseError(kMethodName, "RetryInstalmentCollection Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If

                End If
            End If

            If PlanStatus = PFStatusIndCancelled OrElse PlanStatus = PFStatusIndSuperseded Then
                lReturn = m_oOrionInstalment.SettleCancelledSupersededPlan(v_nPlanTransDetailID:=v_lPFPlanTransactionID, v_nPremiumFinanceCnt:=PFFinancePlanCnt, v_nPremiumFinanceVersion:=PFFinancePlanVersion, v_bPlanHasSinglePolicy:=True)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SettleCancelledSupersededPlan Failed", gPMConstants.PMELogLevel.PMLogError)
                    Return lReturn
                End If

                If m_sCallingAppName <> "SiriusImport" Then
                    lReturn = SetupCreditControlDetailsForCancelledSupersededPlan(nInsuranceFileCnt)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "SetupCreditControlDetailsForCancelledSupersededPlan Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    lReturn = CreateTaskInstance(sDescription:="SED has been added to client account because plan is either superseded or cancelled.")
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "CreateTaskInstance Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

            End If
        Catch Ex As Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=Ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateInstalmentHistoryItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-10-2007 : ARUDDS Phase II
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CreateInstalmentHistoryItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateInstalmentHistoryItem(ByVal v_lPFInstalmentId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "CreateInstalmentHistoryItem"
    '
    'Dim lReturn As gPMConstants.PMEReturnCode
    '
    'On Error GoTo Catch_Renamed
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear Down Database Parameters
    'm_oDatabase.Parameters.Clear()
    '
    ' Add Required Stored Procedure Parameters
    'm_lReturn = CType(AddInputParameter(v_sName:="pfinstalments_id", v_vValue:=v_lPFInstalmentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
    '
    ' Execute Action Query
    'lReturn = m_oDatabase.SQLAction(sSQL:=kCreateInstalmentHistoryItemSQL, sSQLName:=kCreateInstalmentHistoryItemSQL, bStoredProcedure:=True)
    '
    'If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'gPMFunctions.RaiseError(kMethodName, kCreateInstalmentHistoryItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
    'End If
    '
    'GoTo Finally_Renamed
    '
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    'Return result
    'End Function


    ' ***************************************************************** '
    ' Name: RetryInstalmentCollection
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 04-06-2007 : Instalment_Import Changes
    ' ***************************************************************** '
    Private Function RetryInstalmentCollection(ByVal v_lPFInstalmentsId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RetryInstalmentCollection"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRetryFailureReason As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        lReturn = CType(AddInputParameter(v_sName:="pfinstalments_id", v_vValue:=v_lPFInstalmentsId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        lReturn = CType(AddOutputParameter(v_sName:="failurereason", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kRetryInstalmentCollectionSQL, sSQLName:=kRetryInstalmentCollectionName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, kRetryInstalmentCollectionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lRetryFailureReason = gPMFunctions.ToSafeInteger(m_oDatabase.Parameters.Item("failurereason").Value, 0)

        ' if the return value is 1 then the failure count has breached the retry limit
        ' defined on the schemes rate
        If lRetryFailureReason = 1 Then
            result = gPMConstants.PMEReturnCode.PMFail
        End If

        Return result
    End Function


    ' ***************************************************************** '
    ' Name: ClearInstalmentsTransactionDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 01-06-2007 : Instalment_Import
    ' ***************************************************************** '
    'Developer Guie No 101
    Private Function ClearInstalmentsTransactionDetails(ByVal v_lPFInstalmentsId As Integer, Optional ByVal v_lReversalTransDetailId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ClearInstalmentsTransactionDetails"

        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()


        ' Add Required Stored Procedure Parameters
        m_lReturn = CType(AddInputParameter(v_sName:="pfinstalments_id", v_vValue:=v_lPFInstalmentsId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        If v_lReversalTransDetailId > 0 Then
            m_lReturn = CType(AddInputParameter(v_sName:="pfinstalmentsreverse_id", v_vValue:=v_lReversalTransDetailId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
        End If

        ' Execute Action Query
        lReturn = m_oDatabase.SQLAction(sSQL:=kClearInstalmentsTransactionDetailsSQL, sSQLName:=kClearInstalmentsTransactionDetailsName, bStoredProcedure:=True)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kClearInstalmentsTransactionDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function



    '*************************************************************************
    'Name:          CalculateNextDate
    'Description:   Gets the Next Valid instalment Date falling on the same
    '               day of week or month as appropriate
    'History:       TR071102 - Created as per TS23
    '*************************************************************************
    Private Function CalculateNextDate(ByVal v_dtInputDate As Date, ByVal v_sFrequencyInterval As String, ByVal v_iFrequencyAmount As Integer,
                                      ByVal v_iDayOfWeekOrMonth As Integer, ByRef r_dtOutputDate As Date,
                                      Optional ByVal v_enAlignPremiumTo As AlignPremiumToOptions = AlignPremiumToOptions.AlignWithClientPreference,
                                      Optional ByVal v_bCalculatingNoOfInsalments As Boolean = False, Optional ByVal bIsQuarterFrequencyAmount As Boolean = False,
                                      Optional ByVal v_iFirstInstalmentDateAlignWithDayInMonth As Integer = 0,
                                      Optional ByVal v_iSingleInstalmentPerMonth As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim iDayOfMonth, iInputDayOfWeek, iTargetDay As Integer




        result = gPMConstants.PMEReturnCode.PMTrue

        'Make sure required parameters are passed in
        If v_iFrequencyAmount <= 0 Then
            gPMFunctions.RaiseError("v_iFrequencyAmount <= 0", "Invalid Parameter", gPMConstants.PMELogLevel.PMLogError)
        End If

        '*******************
        'Monthly Frequencies
        '*******************
        If v_sFrequencyInterval.Trim.ToUpper = "M" Then

            r_dtOutputDate = v_dtInputDate.AddMonths(v_iFrequencyAmount)
            ''handle in case input and output date like 31/08/2017 and 30/09/2017
            'If Day(r_dtOutputDate) <> Day(v_dtInputDate) AndAlso Day(r_dtOutputDate) < v_iDayOfWeekOrMonth Then
            '    r_dtOutputDate = DateAdd("d", 1, r_dtOutputDate)
            'End If

            If v_enAlignPremiumTo = Business.AlignPremiumToOptions.AlignWithClientPreference Then
                iTargetDay = v_iDayOfWeekOrMonth
            ElseIf v_iFirstInstalmentDateAlignWithDayInMonth = 1 Then
                iTargetDay = v_iDayOfWeekOrMonth
            ElseIf CDate(m_dtStartDate) < CDate("01 January 1764") Then
                iTargetDay = v_iDayOfWeekOrMonth
            Else
                If m_sTransactionType = "MTA" OrElse m_sTransactionType = "MTR" Then
                    iTargetDay = m_dtCoverStartDate.Day
                Else
                    iTargetDay = m_dtStartDate.Day
                End If
                'iTargetDay = DatePart("d", IF(m_sTransactionType = "NB", m_dtStartDate, m_dtCoverStartDate))
            End If

            If v_enAlignPremiumTo = Business.AlignPremiumToOptions.AlignWithClientPreference Then
                'Now match the actual day of the month
                iDayOfMonth = Informations.DatePart("d", r_dtOutputDate, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                'Check that this is the required date
                If iDayOfMonth <> iTargetDay Then
                    If iDayOfMonth > iTargetDay Then
                        Do Until iDayOfMonth <= iTargetDay Or r_dtOutputDate.Month <> r_dtOutputDate.AddDays(-1).Month
                            r_dtOutputDate = r_dtOutputDate.AddDays(-1)
                            iDayOfMonth = Informations.DatePart("d", r_dtOutputDate, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                        Loop
                    Else
                        Do Until iDayOfMonth >= iTargetDay Or r_dtOutputDate.Month <> r_dtOutputDate.AddDays(1).Month
                            r_dtOutputDate = r_dtOutputDate.AddDays(1)
                            iDayOfMonth = Informations.DatePart("d", r_dtOutputDate, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                        Loop
                        'If preferred day in Month does not exists in a month then the ins date will be 1st of next month
                        If v_iSingleInstalmentPerMonth <> 1 AndAlso (iDayOfMonth < iTargetDay) And (r_dtOutputDate.Month + 1 = r_dtOutputDate.AddDays(1).Month) And
                            (v_iFrequencyAmount <> 3 OrElse Not bIsQuarterFrequencyAmount AndAlso (v_iSingleInstalmentPerMonth <> 1 OrElse iTargetDay > System.DateTime.DaysInMonth((r_dtOutputDate.Year.ToString), (r_dtOutputDate.Month.ToString)))) Then
                            r_dtOutputDate = r_dtOutputDate.AddDays(1)
                        End If
                    End If
                End If
            End If

            If v_enAlignPremiumTo = Business.AlignPremiumToOptions.AlignWithPolicyRenewalDate Then
                If r_dtOutputDate.Day <> v_dtInputDate.Day Then
                    r_dtOutputDate = Informations.DateAdd("d", 1, r_dtOutputDate)
                ElseIf m_dtCoverStartDate.Day <> v_dtInputDate.Day AndAlso m_dtCoverStartDate.Day <= Date.DaysInMonth(r_dtOutputDate.Year, If((r_dtOutputDate.Month - 1) = 0, 12, (r_dtOutputDate.Month - 1))) Then
                    'This condition introduced to handle Feb month based calculation which created wrong instament date in case of 29,30,31 Decemeber inception
                    'date as feb doesnt have these days and it moves instalment to 1st March which results in incorrect subsequent instalment dates.
                    Do Until r_dtOutputDate.Day = m_dtCoverStartDate.Day
                        r_dtOutputDate = Informations.DateAdd("d", -1, r_dtOutputDate)
                    Loop
                End If
            End If
            '******************
            'Weekly Frequencies
            '******************
        ElseIf v_sFrequencyInterval.Trim.ToUpper = "W" Then

            'Find out what day of the week the Input Date falls on
            iInputDayOfWeek = Informations.Weekday(v_dtInputDate, DayOfWeek.Monday)

            'Check if we want to align to client pref or start date
            If v_enAlignPremiumTo = Business.AlignPremiumToOptions.AlignWithClientPreference Then
                iTargetDay = v_iDayOfWeekOrMonth
            Else
                iTargetDay = Informations.Weekday(m_dtStartDate, DayOfWeek.Monday)
            End If

            'TR - Work out the actual date
            If iInputDayOfWeek >= iTargetDay Then
                r_dtOutputDate = v_dtInputDate.AddDays(7 * v_iFrequencyAmount).AddDays(iTargetDay).AddDays(-iInputDayOfWeek)
            Else
                r_dtOutputDate = v_dtInputDate.AddDays(iTargetDay).AddDays(-iInputDayOfWeek).AddDays(7 * (v_iFrequencyAmount - 1))
            End If
        End If

        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: CountOfDelimeters
    '
    ' Description: Returns the number of delimeters in a string
    '
    ' History: 07/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CountOfDelimeters) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CountOfDelimeters(ByRef sSource As String, Optional ByRef sDelimeter As String = ",") As Integer
    '
    'Dim result As Integer = 0
    'Dim nCount, nPosition As Integer
    'Dim sRemainder As String = ""
    '
    'Try 
    '
    'nCount = 0
    'sRemainder = sSource
    'nPosition = 1
    'Do While nPosition <> 0
    'nPosition = Strings.InStr(nPosition, sRemainder, sDelimeter, CompareMethod.Binary)
    'If nPosition > 0 Then
    'nCount += 1
    'nPosition += 1
    'End If
    'Loop 
    '
    '
    'Return nCount
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = -1
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CountOfDelimeters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CountOfDelimeters", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: SplitDelimeted
    '
    ' Description:  Splits a delimeted string to return the item in
    '               postion requested.
    '
    ' History: 07/08/2001 DD - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (SplitDelimeted) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SplitDelimeted(ByRef sSource As String, ByRef nItem As Integer, Optional ByRef sDelimeter As String = ",") As String
    '
    'Dim result As String = String.Empty
    'Dim nStart, nEnd As Integer
    '
    'Try 
    '
    'Move through the string and stop at the delimeter
    'nStart = 1
    'For 'nLoop As Integer = 1 To nItem
    'nEnd = Strings.InStr(nStart, sSource, sDelimeter, CompareMethod.Binary) + 1
    'If nEnd = 1 Or nLoop = nItem Then
    'get out if we've run out of delimeters
    'Exit For
    'Else
    'nStart = nEnd
    'End If
    'Next nLoop
    '
    '
    'If nEnd = 1 Then
    'Return Mid(sSource, nStart)
    'Else
    'Return Mid(sSource, nStart, nEnd - nStart - 1)
    'End If
    '
    'Catch 
    'End Try
    '
    '
    '
    'result = ""
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SplitDelimeted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SplitDelimeted", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    '
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

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PostInstalmentToOrion
    '
    ' Description: Post instalments to orion
    '
    ' History: 12/09/2001 TN - Created.
    '          07/01/2003 AAB - Added code to remove the credit control
    '                           item based on the instalmentID.
    ' ***************************************************************** '
    Private Function PostInstalmentToOrion(ByVal v_cAmount As Decimal, ByVal v_lPlanTransDetailID As Integer, ByRef r_lInstalmentTransDetailId As Integer, ByVal v_lInstalmentId As Integer, Optional ByVal v_lCashDrawerID As Integer = 0, Optional ByVal v_cPartialPayAmount As Decimal = 0, Optional ByVal v_bWriteOff As Boolean = False, Optional ByVal v_dtTransactionDate As Date = #12/30/1899#, Optional ByVal v_lInstallmentNo As Integer = 0, Optional ByVal v_sSpare As String = "",
                                           Optional ByVal v_iCashListCurrencyID As Integer = 0,
                                           Optional ByVal v_dPartialAmtInReceiptCurrency As Decimal = 0, Optional ByVal sWriteOffReason As String = "", Optional ByRef nWriteoffTransDetailId As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim sErrorMessage As String = ""

        Dim sCreditControlEnabled As String = ""

        sErrorMessage = "PostInstalmentToOrion Failed"

        result = gPMConstants.PMEReturnCode.PMTrue

        If v_cPartialPayAmount > 0 Then
            If Not v_bWriteOff Then
                m_lReturn = CType(ProcessPartialPayment(v_lInstalmentId:=v_lInstalmentId, v_cPartialPayAmount:=v_cPartialPayAmount), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage & " to process Partial Payment", vApp:=ACApp, vClass:=ACClass, vMethod:="PostInstalmentToOrion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            If sWriteOffReason Is Nothing Then
                sWriteOffReason = ""
            End If
            If v_dPartialAmtInReceiptCurrency <> 0 AndAlso sWriteOffReason.Trim = "" Then
                v_cPartialPayAmount = v_dPartialAmtInReceiptCurrency
            End If

            m_lReturn = m_oOrionInstalment.PostInstalment(v_lPlanTransDetailID:=v_lPlanTransDetailID, v_cAmount:=v_cAmount, v_bPostAsCash:=False, v_lInstalmentTransdetailID:=r_lInstalmentTransDetailId, v_lInstalmentID:=v_lInstalmentId, v_lCashDrawerID:=v_lCashDrawerID, v_bPartialPayment:=True, v_cPartialAmount:=v_cPartialPayAmount, v_bWriteOff:=v_bWriteOff, v_dtTransactionDate:=v_dtTransactionDate, v_lInstallmentNo:=v_lInstallmentNo, v_sSpare:=v_sSpare, v_iCashListCurrencyID:=v_iCashListCurrencyID, sWriteOffReason:=sWriteOffReason, nWriteoffTransDetailId:=nWriteoffTransDetailId)

        Else

            m_lReturn = m_oOrionInstalment.PostInstalment(v_lPlanTransDetailID:=v_lPlanTransDetailID, v_cAmount:=v_cAmount, v_bPostAsCash:=False, v_lInstalmentTransdetailID:=r_lInstalmentTransDetailId, v_lInstalmentID:=v_lInstalmentId, v_lCashDrawerID:=v_lCashDrawerID, v_dtTransactionDate:=v_dtTransactionDate, v_lInstallmentNo:=v_lInstallmentNo, v_sSpare:=v_sSpare, v_iCashListCurrencyID:=v_iCashListCurrencyID, sWriteOffReason:=sWriteOffReason, nWriteoffTransDetailId:=nWriteoffTransDetailId)
        End If

        ' Remove the credit control item based on the instalments id passed in.
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            ' get the sytem option value for option "credit control enabled"
            m_lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionCreditControlEnabled, r_sOptionValue:=sCreditControlEnabled, v_iSourceID:=1), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sErrorMessage = "Failed to Get Credit Control System Option"
                Throw New Exception()
            ElseIf sCreditControlEnabled = "1" Then
                With m_oDatabase
                    ' Clear the parameters
                    .Parameters.Clear()


                    'Add InstalmentsId to the stored proc
                    m_lReturn = .Parameters.Add(sName:="PFInstalments_Id", vValue:=CStr(v_lInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sErrorMessage = "PostInstalmentToOrion failed to add PFInstalments_Id to the " &
                                            "stroed proc"
                        Throw New Exception()
                    End If

                    'run the stored proc
                    m_lReturn = .SQLAction(sSQL:=ACDeleteCreditControlItemSQL, sSQLName:=ACDeleteCreditControlItemName, bStoredProcedure:=ACDeleteCreditControlItemStored)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sErrorMessage = "PostInstalmentToOrion failed to run " &
                                            ACDeleteCreditControlItemSQL
                        Throw New Exception()
                    End If

                End With
            End If
        End If
        m_oOrionInstalment.BankID = 0

        Return m_lReturn

    End Function

    ' ***************************************************************** '
    '
    ' Name: RecallInstalmentFromOrion
    '
    ' Description: recall instalments from Orion
    '
    ' History: 12/09/2001 TN - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RecallInstalmentFromOrion) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RecallInstalmentFromOrion(ByVal v_lPlanTransDetailID As Integer, ByVal v_lInstalmentTransDetailId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oOrionInstalment.RecallInstalment(v_lPlanTransDetailID:=v_lPlanTransDetailID, v_lInstalmentTransDetailId:=v_lInstalmentTransDetailId)
    '
    '
    'Return m_lReturn
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecallInstalmentFromOrion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecallInstalmentFromOrion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: GetPolicyDetails
    '
    ' Description: Get Policy Details for Alternative Payment Method screen
    '
    ' History: TF151002 - Created
    '
    ' ***************************************************************** '
    Public Function GetPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lGISSchemeID As Integer, ByRef r_sDataModelCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACGetPolicyDetailsSQL, sSQLName:=ACGetPolicyDetailsName, bStoredProcedure:=ACGetPolicyDetailsStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & ACGetPolicyDetailsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails")
                    Return result
                End If

                r_lGISSchemeID = gPMFunctions.NullToLong(.Records.Item(1).Fields("gis_scheme_id"))
                r_sDataModelCode = gPMFunctions.NullToString(.Records.Item(1).Fields("data_model_code")).Trim()
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateDocumentComment
    '
    ' Description: Update Document Comment with Alternative Payment Method
    '
    ' History: TF151002 - Created
    '
    ' ***************************************************************** '
    Public Function UpdateDocumentComment(ByVal v_lTransDetailID As Integer, ByVal v_sComment As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(v_lTransDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="comment", vValue:=v_sComment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLAction(sSQL:=ACUpdateDocumentCommentSQL, sSQLName:=ACUpdateDocumentCommentName, bStoredProcedure:=ACUpdateDocumentCommentStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process " & ACUpdateDocumentCommentSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumentComment")
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDocumentComment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocumentComment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateCashListItemRecord
    '
    ' Description: Update the CashListItem record with the TransDetailID (of the first instalment)
    '
    ' Author: Kevin Grandison
    ' Date: 07/08/2003
    '
    ' ***************************************************************** '
    Public Function UpdateCashListItemRecord(ByVal v_lTransDetailID As Integer, ByVal v_lCashListItemID As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim sSQLUpdate As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQLUpdate = "UPDATE CashListItem SET TransDetail_ID = " & v_lTransDetailID & ", AllocationStatus_Id = 3 WHERE CashListItem_ID = " & CStr(v_lCashListItemID)

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .SQLAction(sSQL:=sSQLUpdate, sSQLName:="UpdateCashListItem", bStoredProcedure:=False)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process UpdateCashListItem", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListItemRecord")
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateCashListItemRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateCashListItemRecord", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Private Sub AddInputParameters(ByRef vDueDate As Object, ByRef vFee As Double, ByRef vTax As Double, ByRef vCommission As Double, ByRef vAmount As Double, ByRef vTransactionCode As Object, ByRef vStatus As Integer, ByRef vBatchNumber As Byte, ByRef vBatchExportDate As Object, ByRef vPostedDate As Object, ByRef vPFTransaction_id As Byte)

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="DueDate", vValue:=CStr(vDueDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            m_lReturn = .Parameters.Add(sName:="Fee", vValue:=CStr(vFee), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            m_lReturn = .Parameters.Add(sName:="Tax", vValue:=CStr(vTax), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            m_lReturn = .Parameters.Add(sName:="Commission", vValue:=CStr(vCommission), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            m_lReturn = .Parameters.Add(sName:="Amount", vValue:=CStr(vAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            m_lReturn = .Parameters.Add(sName:="TransactionCode", vValue:=CStr(vTransactionCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = .Parameters.Add(sName:="Status", vValue:=CStr(vStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If False Then

                vBatchNumber = Nothing
            End If
            m_lReturn = .Parameters.Add(sName:="BatchNumber", vValue:=CStr(vBatchNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If False Then


                vBatchExportDate = DBNull.Value
            End If


            m_lReturn = .Parameters.Add(sName:="BatchExportDate", vValue:=CStr(vBatchExportDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If False Then


                vPostedDate = DBNull.Value
            End If

            m_lReturn = .Parameters.Add(sName:="PostedDate", vValue:=CStr(vPostedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If False Then

                vPFTransaction_id = Nothing
            End If
            m_lReturn = .Parameters.Add(sName:="PFTransaction_id", vValue:=CStr(vPFTransaction_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End With

    End Sub


    Public Function GetMediaHistoryId(ByVal lPremFinanceCnt As Integer, ByVal lPremFinanceVersion As Integer, ByRef vMediaHistoryPrev As Object, ByRef vMediaHistoryCurrent As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMediaHistoryId"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Check to see if existing Instalments have been paid
            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add("pfprem_finance_cnt", CStr(lPremFinanceCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add("pfprem_finance_version", CStr(lPremFinanceVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add("media_history_prev_id", CStr(vMediaHistoryPrev), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add("media_history_curr_id", CStr(vMediaHistoryCurrent), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetMediaHistoryIdSQL, sSQLName:=kGetMediaHistoryIdName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                    If Not (Convert.IsDBNull(.Parameters.Item("media_history_curr_id").Value) Or Informations.IsNothing(.Parameters.Item("media_history_curr_id").Value)) Then
                        vMediaHistoryCurrent = .Parameters.Item("media_history_curr_id").Value
                    End If

                    If Not (Convert.IsDBNull(.Parameters.Item("media_history_prev_id").Value) Or Informations.IsNothing(.Parameters.Item("media_history_prev_id").Value)) Then
                        vMediaHistoryPrev = .Parameters.Item("media_history_prev_id").Value
                    End If

                Else

                End If
            End With
            Return result

        Catch ex As Exception

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
    '
    ' Name: CreateInstalments
    '
    ' Description:  Creates instalment records for finance plan
    '               New version which takes into account tax, fee and commission and spreads them appropiately
    '
    ' History:  PSL 12/11/2002 - Created
    '           SMJB 18/09/03 - CQ1733 Separated interest from fees
    '                                   & changed interest logic
    '
    ' ***************************************************************** '
    Public Function CreateInstalments(ByVal v_lPremFinanceCnt As Integer, ByVal v_lPremFinanceVersion As Integer, ByVal v_dFirstInstalmentDate As Date,
                                      ByVal v_cFirstInstalmentAmount As Decimal, ByVal v_dNextInstalmentDate As Date, ByVal v_cNextInstalmentAmount As Decimal,
                                      ByVal v_dLastInstalmentDate As Date, ByVal v_cTax As Decimal, ByVal v_cInterest As Decimal, ByVal v_cFee As Decimal,
                                      ByVal v_cCommission As Decimal, ByVal v_cTotalAmount As Decimal, ByVal v_sMediaTypeValidationCode As String,
                                      ByVal v_sPFSchemeTypeCode As String, ByVal v_iDayOfWeekOrMonth As Integer, ByVal v_iFrequencyAmount As Integer,
                                      ByVal v_sFrequencyInterval As String, ByVal v_bChargeTaxToFirstInstalment As Boolean, ByVal v_bChargeFeeToFirstInstalment As Boolean, Optional ByRef v_iAlignDateTo As Integer = 1, Optional ByRef v_cDepositAmount As Decimal = 0,
                                      Optional ByRef v_bCreateDepositRecord As Boolean = False, Optional ByRef v_bCreateDirectDebitMandate As Boolean = True,
                                      Optional ByRef v_sTransactionType As String = "", Optional ByVal v_iSingleInstalmentPerMonth As Integer = 0,
                                      Optional ByVal v_iFirstInstalmentDateAlignWithDayInMonth As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bInTransaction As Boolean
        Dim sErrorMessage As String = ""
        Dim cInstalmentTax, cInstalmentFee, cInstalmentCommission, cInstalmentInterest, cFirstInstalmentTax, cFirstInstalmentFee, cFirstInstalmentInterest, cFirstInstalmentCommission, cRunningTax As Decimal 'Used so that last payment takes up slack from rounding
        Dim cRunningFee As Decimal 'Used so that last payment takes up slack from rounding
        Dim cRunningCommission As Decimal 'Used so that last payment takes up slack from rounding
        Dim cRunningInterest As Decimal
        Dim iNoOfInstalments, iCurrentInstalment As Integer
        Dim vResult(,) As Object = Nothing
        Dim dtCurrentDate, dtNextDate As Date
        Dim iCount As Integer
        Dim vPFMediaHistoryCurrId As Object = Nothing
        Dim vPFMediaHistoryPrevId As Object = Nothing
        Dim bisTMPPolicy As Boolean
        Dim lStatusID As Integer
        Try

            ' Alix - Align to policy start date, we need to get it!
            If v_iAlignDateTo = 0 Then
                m_lReturn = CType(GetPolicyStartDate(v_lPremFinancePlanCnt:=v_lPremFinanceCnt, r_dtStartDate:=m_dtStartDate, r_dtCoverStartDate:=m_dtCoverStartDate), gPMConstants.PMEReturnCode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    'Align to customer prefence
                    v_iAlignDateTo = 1
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrorMessage = "Failed to get policy start date."
                    Throw New Exception()
                End If
            End If

            'Check to see if existing Instalments have been paid
            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add("pfprem_finance_cnt", CStr(v_lPremFinanceCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add("pfprem_finance_version", CStr(v_lPremFinanceVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add("filter", CStr(PFFilterGetPaidOnly), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No 85
                m_lReturn = .Parameters.Add("batchnumber", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'Developer Guide No 85
                m_lReturn = .Parameters.Add("duedate", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResult)
            End With

            If Informations.IsArray(vResult) Then
                'There are actioned records - the instalments cannot be re-created
                sErrorMessage = "You cannot re-create instalments that have " & "already been actioned."
                Throw New Exception()
            End If

            'Use a transaction to protect the data
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                bInTransaction = True
            End If

            'Remove any existing Instalments to do them again
            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .Parameters.Add("finance_cnt", CStr(v_lPremFinanceCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                m_lReturn = .Parameters.Add("finance_version", CStr(v_lPremFinanceVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'Developer Guide No 39
                m_lReturn = .SQLAction("spu_PFInstalments_Delete", sSQLName:="PFInstalmentsDelete", bStoredProcedure:=True)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'Problem deleting
                sErrorMessage = "Cannot delete Instalments before recreate."
                Throw New Exception()
            End If

            'Special entry to initiate the DDM
            'due today so that it goes asap
            'Only if the payment method is by bank and it's in-house
            ' SET 11/08/2004 ISS13980 - only if required
            m_lReturn = CType(IsTMPProductPolicy(v_lPremFinanceCnt, bisTMPPolicy), gPMConstants.PMEReturnCode)
            'Developer Guide No 149,115,44
            If v_sMediaTypeValidationCode.Trim().ToUpper() = "BANK" AndAlso CBool(Convert.ToString(v_sPFSchemeTypeCode = "IH").Trim().ToUpper()) AndAlso (v_bCreateDirectDebitMandate) Then

                m_lReturn = CType(GetMediaHistoryId(lPremFinanceCnt:=v_lPremFinanceCnt, lPremFinanceVersion:=v_lPremFinanceVersion, vMediaHistoryPrev:=vPFMediaHistoryPrevId, vMediaHistoryCurrent:=vPFMediaHistoryCurrId), gPMConstants.PMEReturnCode)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sErrorMessage = "Cannot get Media History Id for the Instalment."
                End If

                If v_sTransactionType.Trim() = "REN" And bisTMPPolicy Then

                Else
                    m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=0, v_vFee:=0, v_vTax:=0, v_vCommission:=0, v_vAmount:=0, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionCreate, v_vDueDate:=DateTime.Today, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Problem deleting
                        sErrorMessage = "Failed to create dummy instalment for DDM."
                        Throw New Exception()
                    End If
                End If
            End If

            ' Added create the deposit record,
            If v_bCreateDepositRecord And v_cDepositAmount > 0 Then
                m_lReturn = CType(GetMediaHistoryId(lPremFinanceCnt:=v_lPremFinanceCnt, lPremFinanceVersion:=v_lPremFinanceVersion, vMediaHistoryPrev:=vPFMediaHistoryPrevId, vMediaHistoryCurrent:=vPFMediaHistoryCurrId), gPMConstants.PMEReturnCode)
                m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=0, v_vFee:=0, v_vTax:=0, v_vCommission:=0, v_vAmount:=v_cDepositAmount, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionDeposit, v_vDueDate:=DateTime.Today, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId) ' Sankar added vPFMediaHistoryCurrId

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Problem deleting
                    sErrorMessage = "Failed to create Deposit Record."
                    Throw New Exception()
                End If
            End If
            ' END CHANGES - Changed By: AAB  - Changed On: 08-Oct-2003 12:38

            'Create First Instalment which may be a different amount to the others
            dtCurrentDate = v_dFirstInstalmentDate

            If v_cTotalAmount <> 0 Then
                'Calculate the commission as an equal proportion of each instament
                cFirstInstalmentCommission = RoundDown2DP(v_cFirstInstalmentAmount / v_cTotalAmount * v_cCommission)
            Else
                cFirstInstalmentCommission = 0
            End If

            'TR - If Tax is to be spread over all instalments calculate equal
            'proportions
            If Not v_bChargeTaxToFirstInstalment Then
                If v_cTotalAmount <> 0 Then
                    cFirstInstalmentTax = RoundDown2DP(v_cFirstInstalmentAmount / v_cTotalAmount * v_cTax)
                Else
                    cFirstInstalmentTax = 0
                End If
            Else
                cFirstInstalmentTax = v_cTax
            End If

            'TR - If Fee is to be spread over all instalments calculate equal
            'proportions
            If Not v_bChargeFeeToFirstInstalment Then
                If v_cTotalAmount <> 0 Then
                    cFirstInstalmentFee = RoundDown2DP(v_cFirstInstalmentAmount / v_cTotalAmount * v_cFee)
                Else
                    cFirstInstalmentFee = 0
                End If
            Else
                cFirstInstalmentFee = v_cFee
            End If

            If v_cTotalAmount <> 0 Then
                'SMJB 18/09/03 Added same calculation for interest
                cFirstInstalmentInterest = RoundDown2DP(v_cFirstInstalmentAmount / v_cTotalAmount * v_cInterest)
            Else
                cFirstInstalmentInterest = 0
            End If

            cRunningTax = cFirstInstalmentTax
            cRunningFee = cFirstInstalmentFee
            cRunningCommission = cFirstInstalmentCommission
            cRunningInterest = cFirstInstalmentInterest

            m_lReturn = CType(IsTMPProductPolicy(v_lPremFinanceCnt, bisTMPPolicy), gPMConstants.PMEReturnCode)

            m_lReturn = CType(GetMediaHistoryId(lPremFinanceCnt:=v_lPremFinanceCnt, lPremFinanceVersion:=v_lPremFinanceVersion, vMediaHistoryPrev:=vPFMediaHistoryPrevId, vMediaHistoryCurrent:=vPFMediaHistoryCurrId), gPMConstants.PMEReturnCode)

            If v_sTransactionType.Trim() = "REN" And bisTMPPolicy Then 'Ongoning for TMPs in Renewal and for MTA
                If m_dtCoverStartDate <= DateTime.Now Then
                    m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=1, v_vFee:=cFirstInstalmentFee + cRunningInterest, v_vTax:=cFirstInstalmentTax, v_vCommission:=cFirstInstalmentCommission, v_vAmount:=v_cFirstInstalmentAmount, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionFirst, v_vDueDate:=v_dFirstInstalmentDate, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)
                Else
                    m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=1, v_vFee:=cFirstInstalmentFee + cRunningInterest, v_vTax:=cFirstInstalmentTax, v_vCommission:=cFirstInstalmentCommission, v_vAmount:=v_cFirstInstalmentAmount, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionOngoing, v_vDueDate:=v_dFirstInstalmentDate, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Problem deleting
                    sErrorMessage = "Failed to create Ongoing instalment for TMP Policy."
                    Throw New Exception()
                End If
            ElseIf v_sTransactionType.Trim() = "MTA" Then
                m_lReturn = CType(getInstalmentStatus(v_lPremFinancePlanCnt:=v_lPremFinanceCnt, v_lPremFinancePlanVersion:=v_lPremFinanceVersion - 1, r_lStatusId:=lStatusID), gPMConstants.PMEReturnCode)
                If lStatusID = 1 Then
                    m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=1, v_vFee:=cFirstInstalmentFee + cRunningInterest, v_vTax:=cFirstInstalmentTax, v_vCommission:=cFirstInstalmentCommission, v_vAmount:=v_cFirstInstalmentAmount, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionFirst, v_vDueDate:=v_dFirstInstalmentDate, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)
                Else
                    m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=1, v_vFee:=cFirstInstalmentFee + cRunningInterest, v_vTax:=cFirstInstalmentTax, v_vCommission:=cFirstInstalmentCommission, v_vAmount:=v_cFirstInstalmentAmount, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionOngoing, v_vDueDate:=v_dFirstInstalmentDate, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Problem deleting
                    sErrorMessage = "Failed to create Ongoing instalment for TMP Policy."
                    Throw New Exception()
                End If
            Else
                'TR - Add the first instalment
                m_lReturn = DirectAdd(v_vpfprem_finance_cnt:=v_lPremFinanceCnt, v_vpfprem_finance_version:=v_lPremFinanceVersion, v_vInstalmentNumber:=1, v_vFee:=cFirstInstalmentFee + cRunningInterest, v_vTax:=cFirstInstalmentTax, v_vCommission:=cFirstInstalmentCommission, v_vAmount:=v_cFirstInstalmentAmount, v_vStatus:=bSIRPremFinConst.PFStatusNew, v_vTransactionCode:=PFTransactionFirst, v_vDueDate:=v_dFirstInstalmentDate, v_vPFTransaction_id:=0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Problem deleting
                    sErrorMessage = "Failed to create First instalment."
                    Throw New Exception()
                End If
            End If

            'Calculate No of instalments

            ' DD 10/11/2003 - if there is only 1 instalment then skip the rest
            If v_dNextInstalmentDate > v_dFirstInstalmentDate Then
                If v_iAlignDateTo = 1 Then
                    If gPMFunctions.ToSafeLong(v_iDayOfWeekOrMonth) <= 0 Then
                        'When default not available
                        v_iDayOfWeekOrMonth = v_dNextInstalmentDate.Day
                    End If

                    If v_sFrequencyInterval = "m" And v_iFrequencyAmount = 1 Then
                        If v_iDayOfWeekOrMonth > v_dFirstInstalmentDate.Day Then
                            If v_dFirstInstalmentDate.AddDays(1).Month = v_dFirstInstalmentDate.Month Then
                                If v_iSingleInstalmentPerMonth = 0 And v_iDayOfWeekOrMonth > System.DateTime.DaysInMonth(dtCurrentDate.Year.ToString, (dtCurrentDate.AddMonths(-1).Month.ToString)) Then
                                    dtCurrentDate = dtCurrentDate.AddMonths(-1)
                                End If
                            End If
                        End If
                    End If
                Else
                    'This condition introduced to handle Feb month based calculation which created wrong instament date in case of 29,30,31 Decemeber inception
                    'date as feb doesnt have these days and it moves instalment to 1st March which results in incorrect subsequent instalment dates.
                    'PM048827(TFS-21798)
                    If v_dNextInstalmentDate.Day <> m_dtStartDate.Day And m_sTransactionType <> "MTR" Then
                        v_iDayOfWeekOrMonth = m_dtStartDate.Day
                    Else
                        v_iDayOfWeekOrMonth = v_dNextInstalmentDate.Day
                    End If

                    If v_sFrequencyInterval = "m" And v_iFrequencyAmount = 1 Then
                        If v_iDayOfWeekOrMonth > v_dFirstInstalmentDate.Day Then
                            If v_dFirstInstalmentDate.AddDays(1).Month > v_dFirstInstalmentDate.Month Then
                                dtCurrentDate = dtCurrentDate.AddMonths(-1)
                            End If
                        End If
                    End If
                End If

                Dim bIsQuarterFrequencyAmount As Boolean
                If v_iFrequencyAmount = 3 Then
                    bIsQuarterFrequencyAmount = True
                End If

                'If v_iFrequencyAmount = 3 And m_dtStartDate < v_dFirstInstalmentDate Then
                '    dtCurrentDate = v_dNextInstalmentDate.AddMonths(-3)
                '    'ElseIf v_iFrequencyAmount = 6 And ((v_sTransactionType.Trim() = "NB" And m_dtStartDate < v_dFirstInstalmentDate) Or (v_sTransactionType.Trim() = "MTA" And m_dtCoverStartDate < v_dFirstInstalmentDate)) Then
                'ElseIf v_iFrequencyAmount = 6 Then
                '    dtCurrentDate = v_dNextInstalmentDate.AddMonths(-6)
                '    'dtCurrentDate = m_dtCoverStartDate
                'End If

                m_lReturn = CType(CalculateNoOfInstalments(dtCurrentDate, v_dLastInstalmentDate, v_sFrequencyInterval, v_iFrequencyAmount,
                                                           v_iDayOfWeekOrMonth, iNoOfInstalments, v_iAlignDateTo, bIsQuarterFrequencyAmount:=bIsQuarterFrequencyAmount,
                                                            v_iFirstInstalmentDateAlignWithDayInMonth:=v_iFirstInstalmentDateAlignWithDayInMonth, v_iSingleInstalmentPerMonth:=v_iSingleInstalmentPerMonth), gPMConstants.PMEReturnCode)

                'TR - Add the First and last instalment on (excluded in count) and NEXT
                iNoOfInstalments += 2

                'TR - Now add all the other instalments
                iCount = 2
                iCurrentInstalment = 2

                 If v_iAlignDateTo = Business.AlignPremiumToOptions.AlignWithPolicyRenewalDate Then
                    dtCurrentDate = m_dtCoverStartDate
                 End If

                Do While iCurrentInstalment <= iNoOfInstalments
                    ' Alix Bergeret - 31/03/2003 - Added "Align To" parameter
                    CalculateNextDate(dtCurrentDate, v_sFrequencyInterval, v_iFrequencyAmount * (iCount - 1), v_iDayOfWeekOrMonth, dtNextDate, v_iAlignDateTo, bIsQuarterFrequencyAmount:=bIsQuarterFrequencyAmount, v_bCalculatingNoOfInsalments:=False, v_iFirstInstalmentDateAlignWithDayInMonth:=v_iFirstInstalmentDateAlignWithDayInMonth, v_iSingleInstalmentPerMonth:=v_iSingleInstalmentPerMonth)
                    iCount += 1
                    If dtNextDate <= v_dFirstInstalmentDate Or (dtNextDate > v_dLastInstalmentDate AndAlso iCurrentInstalment > iNoOfInstalments) Then
                        ' Carry on looping until 2d date is after 1st date!
                        If dtNextDate <= v_dFirstInstalmentDate Then
                            Continue Do
                        Else
                            Exit Do
                        End If
                    Else
                        'Add instalment record
                        If iCurrentInstalment = iNoOfInstalments OrElse dtNextDate = v_dLastInstalmentDate Then
                            'If its the last instalment then make sure we pick what's left
                            'From rounding
                            cInstalmentTax = v_cTax - cRunningTax
                            cInstalmentFee = v_cFee - cRunningFee
                            cInstalmentCommission = v_cCommission - cRunningCommission
                            cInstalmentInterest = v_cInterest - cRunningInterest

                            'The last instalment is marked as such with the transaction code
                            m_lReturn = DirectAdd(v_lPremFinanceCnt, v_lPremFinanceVersion, iCurrentInstalment, v_dLastInstalmentDate, cInstalmentFee + cInstalmentInterest, cInstalmentTax, cInstalmentCommission, v_cNextInstalmentAmount, PFTransactionLast, bSIRPremFinConst.PFStatusNew, 0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                'Problem deleting
                                sErrorMessage = "Failed to create last instalment."
                                Throw New Exception()
                            End If
                        Else
                            cInstalmentTax = RoundDown2DP((v_cTax - cFirstInstalmentTax) / (iNoOfInstalments - 1))
                            cInstalmentFee = RoundDown2DP((v_cFee - cFirstInstalmentFee) / (iNoOfInstalments - 1))
                            cInstalmentCommission = RoundDown2DP((v_cCommission - cFirstInstalmentCommission) / (iNoOfInstalments - 1))
                            cInstalmentInterest = RoundDown2DP((v_cInterest - cFirstInstalmentInterest) / (iNoOfInstalments - 1))

                            cRunningTax += cInstalmentTax
                            cRunningFee += cInstalmentFee
                            cRunningCommission += cInstalmentCommission
                            cRunningInterest += cInstalmentInterest

                            m_lReturn = DirectAdd(v_lPremFinanceCnt, v_lPremFinanceVersion, iCurrentInstalment, dtNextDate, cInstalmentFee + cInstalmentInterest, cInstalmentTax, cInstalmentCommission, v_cNextInstalmentAmount, PFTransactionOngoing, bSIRPremFinConst.PFStatusNew, 0, v_vMediaHistoryId:=vPFMediaHistoryCurrId)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                'Problem deleting
                                sErrorMessage = "Failed to create instalment."
                                Throw New Exception()
                            End If
                        End If
                        iCurrentInstalment += 1
                    End If
                Loop
            End If

            'Commit the lot
            CommitTrans()


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            If bInTransaction Then
                RollbackTrans()
            End If
            Return result
        End Try
    End Function

    Private Function GetPolicyStartDate(ByVal v_lPremFinancePlanCnt As Integer, ByRef r_dtStartDate As Date, ByRef r_dtCoverStartDate As Date) As Integer

        Dim result As Integer = 0

        Dim vResult(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("pfprem_finance_cnt", CStr(v_lPremFinancePlanCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "v_lPremFinancePlanCnt:=" & v_lPremFinancePlanCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPolicyStartDateSQL, sSQLName:=ACSelectPolicyStartDateName, bStoredProcedure:=ACSelectPolicyStartDateStored, vResultArray:=vResult)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectPolicyStartDateSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Get the date
        If Informations.IsArray(vResult) Then

            r_dtStartDate = CDate(vResult(0, 0))
            r_dtCoverStartDate = CDate(vResult(1, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result
    End Function


    '*************************************************************************
    'Name:          CalculateNoOfInstalments
    'Description:   Using a Start Date and an End Date calculates the number
    '               of instalments in-between by adding the Frequency Period
    '               Excludes count of Intalments on 2 provided dates.
    'DONT CHANGE THIS WITHOUT CHANGING THE SAME METHOD IN bSIRPremiumFinance!!!
    'History:       TR121202 - Created as per TS23
    '*************************************************************************
    Private Function CalculateNoOfInstalments(ByVal v_dtStartDate As Date, ByVal v_dtEndDate As Date, ByVal v_sFrequencyInterval As String,
                                              ByVal v_iFrequencyAmount As Integer, ByVal v_iDayOfWeekOrMonth As Integer, ByRef r_iNoOfInstalments As Integer,
                                              ByVal v_enAlignPremiumTo As AlignPremiumToOptions,
                                              ByVal v_iFirstInstalmentDateAlignWithDayInMonth As Integer, Optional v_iSingleInstalmentPerMonth As Integer = 0,
                                              Optional ByVal bIsQuarterFrequencyAmount As Boolean = False) As Integer

        'TR - Declare local variables
        Dim result As Integer = 0
        Dim iCount, iNoofInstalments As Integer
        Dim dtLastDate, dtDate As Date



        'TR - Assume failure
        result = gPMConstants.PMEReturnCode.PMFail

        'TR - Make sure that the earlier date is Earlier
        If v_dtStartDate > v_dtEndDate Then
            r_iNoOfInstalments = 0
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        'TR - Initialise variables
        iCount = 0
        iNoofInstalments = 0
        dtDate = v_dtStartDate
        If v_enAlignPremiumTo = Business.AlignPremiumToOptions.AlignWithPolicyRenewalDate Then
            dtLastDate = m_dtCoverStartDate
        Else
            dtLastDate = dtDate
        End If

        'TR - Loop through instalments
        Do
            'v_sFrequencyInterval is the number of units from the db. This is
            'different in bSIRPremiumFinance, as it uses the number of occurrances p.a or pcm.
            CalculateNextDate(dtLastDate, v_sFrequencyInterval, v_iFrequencyAmount * (iCount + 1), v_iDayOfWeekOrMonth, dtDate, v_enAlignPremiumTo, True, v_iFirstInstalmentDateAlignWithDayInMonth:=v_iFirstInstalmentDateAlignWithDayInMonth, v_iSingleInstalmentPerMonth:=v_iSingleInstalmentPerMonth)
            ' RDT - Just use the Date part of the datetime field
            iCount += 1
            If v_dtStartDate >= dtDate AndAlso iCount < 1000 Then
                Continue Do
            End If
            If Math.Floor(dtDate.ToOADate) >= Math.Floor(v_dtEndDate.ToOADate) Then
                Exit Do
            End If
            If dtDate.Month = v_dtStartDate.Month AndAlso dtDate.Day < dtLastDate.Day Then
                Continue Do
            End If

            iNoofInstalments += 1
        Loop

        'TR - Return the last date
        r_iNoOfInstalments = iNoofInstalments

        'TR - Success
        Return gPMConstants.PMEReturnCode.PMTrue

    End Function


    Public Function PayTPInstalment(ByVal v_lPFInstalmentsId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PayTPInstalment
        ' PURPOSE:
        ' AUTHOR: Paul Cunningham
        ' DATE: 01 April 2003, 10:22:04
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "PayTPInstalment"

        Dim lPremFinanceCount, lPremFinanceVersion As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Mark the instalment as collected
            If MarkInstalmentStatus(v_lPFInstalmentId:=v_lPFInstalmentsId, v_iStatus:=bSIRPremFinConst.PFStatusCollected) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to mark instalment as collected")
            End If

            'Get the primary key of the parent plan so that we can do the auto close
            If GetParentPlanPK(v_lPFInstalmentId:=v_lPFInstalmentsId, r_lPremFinanceCount:=lPremFinanceCount, r_lPremFinanceVersion:=lPremFinanceVersion) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get parent plan primary key")
            End If

            If AutoClosePlan(v_lPremFinanceCount:=lPremFinanceCount, v_lPremFinanceVersion:=lPremFinanceVersion) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to auto close the plan")
            End If

            'If the plan is TPR then call Claims to process the credit
            'NOTE - Currently out of scope

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select

        Finally
        End Try
        Return result

    End Function

    Public Function ReverseTPInstalment(ByVal v_lPFInstalmentsId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: ReverseTPInstalment
        ' PURPOSE:
        ' AUTHOR: Paul Cunningham
        ' DATE: 01 April 2003, 10:31:30
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "ReverseTPInstalment"

        Dim lPremFinanceCount, lPremFinanceVersion As Integer

        Dim oSIRPFBusiness As Object = Nothing

        Dim vPlan As Object = Nothing

        Const klFirstRecord As Integer = 0
        Const klPFStatusIndComplete As String = "040"


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            'Mark the instalment as failed
            If MarkInstalmentStatus(v_lPFInstalmentId:=v_lPFInstalmentsId, v_iStatus:=bSIRPremFinConst.PFStatusFailed) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to mark instalment as failed")
            End If

            'Get the primary key of the parent plan so that we can use it to get the plan
            If GetParentPlanPK(v_lPFInstalmentId:=v_lPFInstalmentsId, r_lPremFinanceCount:=lPremFinanceCount, r_lPremFinanceVersion:=lPremFinanceVersion) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get parent plan primary key")
            End If

            'Get the business object

            'Get the business object
            If GetBusinessObject(v_sClass:="bSIRPremiumFinance.Business", r_oObject:=oSIRPFBusiness) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get business object: bSIRPremiumFinance.Business")
            End If

            'Get the plan details from the database

            If oSIRPFBusiness.GetSingleFinancePlan(v_lFinanceCount:=ToSafeInteger(lPremFinanceCount), v_lFinanceVersion:=ToSafeInteger(lPremFinanceVersion), r_vPFPremiumFinance:=vPlan) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get plan details")
            End If

            'Update the plan to LIVE if it is currently COMPLETED

            If CStr(vPlan(bSIRPremFinConst.k_PFPlanStatusInd, klFirstRecord)) = klPFStatusIndComplete Then

                If oSIRPFBusiness.StatusUpdate(vPremiumFinanceCnt:=ToSafeInteger(lPremFinanceCount), vPremiumFinanceVersion:=ToSafeInteger(lPremFinanceVersion), vStatusInd:=bSIRPremFinConst.PFStatusIndLive) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to update the plan status")
                End If
            End If

            'If the plan is TPR then call Claims to process the reverse
            'NOTE - Currently out of scope

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError
                    Return result

            End Select

        Finally
            If Not (oSIRPFBusiness Is Nothing) Then

                oSIRPFBusiness.Dispose()
                oSIRPFBusiness = Nothing
            End If



        End Try
        Return result

    End Function

    Public Function AutoClosePlan(ByVal v_lPremFinanceCount As Integer, ByVal v_lPremFinanceVersion As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: AutoClosePlan
        ' PURPOSE:
        ' AUTHOR: Paul Cunningham
        ' DATE: 01 April 2003, 10:33:30
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "AutoClosePlan"

        Dim oSIRPFBusiness As Object = Nothing

        Dim bPlanPaid As Boolean



        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            'Get the business object
            If GetBusinessObject(v_sClass:="bSIRPremiumFinance.Business", r_oObject:=oSIRPFBusiness) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get business object: bSIRPremiumFinance.Business")
            End If

            'Get the plan details from the database
            If GetPlanPaid(v_lPremFinanceCount:=v_lPremFinanceCount, v_lPremFinanceVersion:=v_lPremFinanceVersion, r_bPlanPaid:=bPlanPaid) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to get plan paid details")
            End If

            'Update the plan to completed if its paid
            If bPlanPaid Then


                If oSIRPFBusiness.StatusUpdate(vPremiumFinanceCnt:=ToSafeInteger(v_lPremFinanceCount), vPremiumFinanceVersion:=ToSafeInteger(v_lPremFinanceVersion), vStatusInd:=bSIRPremFinConst.PFStatusIndCompleted) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Unable to update the plan status")
                End If

            End If

            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError
                    Return result

            End Select

        Finally
            If Not (oSIRPFBusiness Is Nothing) Then

                oSIRPFBusiness.Dispose()
                oSIRPFBusiness = Nothing
            End If



        End Try
        Return result

    End Function

    Private Function GetPlanPaid(ByVal v_lPremFinanceCount As Integer, ByVal v_lPremFinanceVersion As Integer, ByRef r_bPlanPaid As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetPlanPaid
        ' PURPOSE:
        ' AUTHOR: Paul Cunningham
        ' DATE: 01 April 2003, 14:50:30
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "GetPlanPaid"

        Dim vDetails(,) As Object = Nothing

        'Constants for the return array
        Const klACTotalCollectedAmount As Integer = 0
        Const klACTotalCostAmount As Integer = 1

        Const klFirstRow As Integer = 0

        Try



            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase
                .Parameters.Clear()

                'Add parameters...
                If .Parameters.Add(sName:="lPremFinanceCnt", vValue:=CStr(v_lPremFinanceCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error adding parameter: lPremFinanceCnt")
                End If

                If .Parameters.Add(sName:="lPremFinanceVersion", vValue:=CStr(v_lPremFinanceCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error adding parameter: lPremFinanceVersion")
                End If

                'Execute SQL Statement
                If .SQLSelect(sSQL:=ACGetPlanPaidSQL, sSQLName:=ACGetPlanPaidName, bStoredProcedure:=ACGetPlanPaidStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vDetails) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error calling SQLSelect: " & ACGetPlanPaidSQL)
                End If
            End With

            'Any results
            If Informations.IsArray(vDetails) Then
                'Is the amount collected >= the cost of the plan


                If CDbl(vDetails(klACTotalCollectedAmount, klFirstRow)) >= v_lPremFinanceVersion = CBool(vDetails(klACTotalCostAmount, klFirstRow)) Then

                    r_bPlanPaid = True
                Else
                    r_bPlanPaid = False
                End If

                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select
        Finally
        End Try


        Return result

    End Function


    Private Function MarkInstalmentStatus(ByVal v_lPFInstalmentId As Integer, ByVal v_iStatus As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: MarkInstalmentStatus
        ' PURPOSE: Marks the Instalment as the passed Status
        ' AUTHOR: Paul Cunningham
        ' DATE: 01 April 2003, 10:26:56
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "MarkInstalmentStatus"


        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase
                .Parameters.Clear()

                'Add parameters...
                If .Parameters.Add(sName:="lPFInstalmentId", vValue:=CStr(v_lPFInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error adding parameter: lPFInstalmentId")
                End If

                If .Parameters.Add(sName:="lStatus", vValue:=CStr(v_iStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error adding parameter: lStatus")
                End If

                'Execute SQL Statement
                If .SQLAction(sSQL:=ACMarkInstalmentStatusSQL, sSQLName:=ACMarkInstalmentStatusName, bStoredProcedure:=ACMarkInstalmentStatusStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error calling SQLAction: " & ACMarkInstalmentStatusSQL)
                End If
            End With

            result = gPMConstants.PMEReturnCode.PMTrue

            Return result


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select
        Finally
        End Try

        Return result

    End Function

    Private Function GetParentPlanPK(ByVal v_lPFInstalmentId As Integer, ByRef r_lPremFinanceCount As Integer, ByRef r_lPremFinanceVersion As Integer, Optional ByRef r_sPlanStatus As String = "", Optional ByRef r_nInsuranceFileCnt As Integer = 0) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetParentPlanPK
        ' PURPOSE: Get the Primary Key of the plan the instalment belongs to
        ' AUTHOR: Paul Cunningham
        ' DATE: 01 April 2003, 10:46:35
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "GetParentPlanPK"

        Dim vPlans(,) As Object = Nothing

        'Constants for the return array
        Const klACPremFinanceCount As Integer = 0
        Const klACPremFinanceVersion As Integer = 1
        Const klACPlanStatus As Integer = 2
        Const klACInsuranceFileCnt As Integer = 3

        Const klFirstRow As Integer = 0


        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase
                .Parameters.Clear()

                'Add parameters...
                If .Parameters.Add(sName:="lPFInstalmentId", vValue:=CStr(v_lPFInstalmentId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", Error adding parameter: lPFInstalmentId")
                End If

                'Execute SQL Statement
                If .SQLSelect(sSQL:=ACGetPlanPKSQL, sSQLName:=ACGetPlanPKName, bStoredProcedure:=ACGetPlanPKStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vPlans) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "Error calling SQLSelect: " & ACGetPlanPKSQL)
                End If
            End With

            'Any results?
            If Informations.IsArray(vPlans) Then

                r_lPremFinanceCount = CInt(vPlans(klACPremFinanceCount, klFirstRow))
                r_lPremFinanceVersion = CInt(vPlans(klACPremFinanceVersion, klFirstRow))
                r_sPlanStatus = vPlans(klACPlanStatus, klFirstRow)
                r_nInsuranceFileCnt = CInt(vPlans(klACInsuranceFileCnt, klFirstRow))


                r_sPlanStatus = vPlans(klACPlanStatus, klFirstRow)

                r_nInsuranceFileCnt = CInt(vPlans(klACInsuranceFileCnt, klFirstRow))

                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                r_lPremFinanceCount = 0
                r_lPremFinanceVersion = 0

                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select
        Finally
        End Try


        Return result

    End Function

    Private Function GetBusinessObject(ByVal v_sClass As String, ByRef r_oObject As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetBusinessObject
        ' PURPOSE: Create an instance of the required business object
        ' AUTHOR: Paul Cunningham
        ' DATE: 26 March 2003, 09:31:04
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Const ACMethod As String = "GetBusinessObject"


        Try
            result = gPMConstants.PMEReturnCode.PMFalse

            If gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClass, v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ACMethod + ", " + "gPMComponentServices failed to create business object: " & v_sClass)
            End If

            result = gPMConstants.PMEReturnCode.PMTrue

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------

        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Constants.vbObjectError
                    ' Log internal failure.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Informations.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Informations.Err().Source, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result

                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMError

                    Return result

            End Select
        Finally
        End Try

        Return result

    End Function

    Private Function StatusUpdate(ByRef v_vInstalmentID As Integer, Optional ByVal v_nPlanTransactionId As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim lPremiumFinanceCnt, lPremiumFinanceVersion As Integer
        Dim lNoInstalments As Integer
        Dim oPFPremiumFinance As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue
        'Get the business object

        'Get the business object
        If GetBusinessObject(v_sClass:="bSIRPremiumFinance.Business", r_oObject:=oPFPremiumFinance) <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + ", Unable to get business object: bSIRPremiumFinance.Business")
        End If

        m_lReturn = CType(GetParentPlanPK(v_lPFInstalmentId:=v_vInstalmentID, r_lPremFinanceCount:=lPremiumFinanceCnt, r_lPremFinanceVersion:=lPremiumFinanceVersion), gPMConstants.PMEReturnCode)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If

        If lPremiumFinanceCnt <> lPrevPremiumFinanceCnt Then

            'm_lReturn = oPFPremiumFinance.GetInstalmentsRemaining(v_lpfprem_finance_cnt:=lPremiumFinanceCnt, v_lpfprem_finance_version:=lPremiumFinanceVersion, r_lNoInstalments:=lNoInstalments)
            m_lReturn = GetInstalmentsRemaining(v_lpfprem_finance_cnt:=lPremiumFinanceCnt, v_lpfprem_finance_version:=lPremiumFinanceVersion, r_lNoInstalments:=lNoInstalments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            If lNoInstalments = 0 Then
                Dim nInstalmentTransDetailId As Integer
                Dim dOutstandingAmount As Decimal

                m_lReturn = GetPlanTransactionOutstanding(v_nPlanTrancationId:=v_nPlanTransactionId, r_dOutstandingAmount:=dOutstandingAmount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                If dOutstandingAmount <> 0.0 Then
                    m_lReturn = m_oOrionInstalment.PostInstalment(v_lPlanTransDetailID:=v_nPlanTransactionId,
                                              v_cAmount:=dOutstandingAmount, v_bPostAsCash:=False,
                                              v_lInstalmentTransdetailID:=nInstalmentTransDetailId,
                                              v_lInstalmentID:=v_vInstalmentID, v_sSpare:="Rounding", v_iCashListCurrencyID:=m_iCurrencyID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFail
                    End If
                End If

                m_lReturn = oPFPremiumFinance.StatusUpdate(vPremiumFinanceCnt:=ToSafeInteger(lPremiumFinanceCnt), vPremiumFinanceVersion:=ToSafeInteger(lPremiumFinanceVersion), vStatusInd:=bSIRPremFinConst.PFStatusIndCompleted)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If
            End If

            If CommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                RollbackTrans()
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            lPrevPremiumFinanceCnt = lPremiumFinanceCnt

        End If
        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMTrue
        End If

        Return result

    End Function
    ''' <summary>
    ''' PlanStatusUpdate
    ''' </summary>
    ''' <param name="vPremiumFinanceCnt"></param>
    ''' <param name="vPremiumFinanceVersion"></param>
    ''' <param name="vStatusInd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PlanStatusUpdate(ByVal vPremiumFinanceCnt As Integer, ByVal vPremiumFinanceVersion As Integer, ByVal vStatusInd As String) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set variables for update call

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceCnt", vValue:=gPMFunctions.ToSafeString(vPremiumFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="PremiumFinanceVersion", vValue:=gPMFunctions.ToSafeString(vPremiumFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=vStatusInd, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Update existing record
                m_lReturn = m_oDatabase.SQLAction(ACPFPremiumFinanceUpdateStatusSQL, ACPFPremiumFinanceUpdateStatusName, ACPFPremiumFinanceUpdateStatusStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StatusUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StatusUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSystemOptionLite
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 15-02-2005 : Credit Control RetroFit
    ' ***************************************************************** '
    Private Function GetSystemOptionLite(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iSourceID As Integer) As Integer

        Const kMethodName As String = "GetSystemOptionLite"
        Dim lReturn As gPMConstants.PMEReturnCode
        lReturn = gPMConstants.PMEReturnCode.PMTrue
        lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=v_iOptionNumber, r_sOptionValue:=r_sOptionValue, v_iSourceID:=m_iSourceID), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
        End If
        Return lReturn
    End Function

    ' ***************************************************************** '
    ' Name: SetupCreditControlItemForInstalment
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-05-2007 : Instalment Import Changes
    ' ***************************************************************** '
    Private Function SetupCreditControlItemForInstalment(ByVal v_lInstalmentId As Integer, ByVal v_sDefaultCreditControlItemReason As String, Optional ByVal v_lProcessMode As Integer = 0) As Integer

        Const kMethodName As String = "SetupCreditControlItemForInstalment"

        Dim lReturn As gPMConstants.PMEReturnCode

        lReturn = gPMConstants.PMEReturnCode.PMTrue


        lReturn = m_oCreditControlItem.SetupCreditControlItemForInstalment(v_lInstalmentId, v_sDefaultCreditControlItemReason, v_lProcessMode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetupCreditControlItemForInstalment Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        Return lReturn
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
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddInputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                    ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddOutputParameter
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Private Function AddOutputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddOutputParameter"

        Dim lReturn As gPMConstants.PMEReturnCode

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=v_iType)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, " Failed to add parameter name:" & v_sName &
                                    ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), gPMConstants.PMELogLevel.PMLogError)
        End If

        Return result
    End Function
    Private Function IsTMPProductPolicy(ByVal v_lPremFinancePlanCnt As Integer, ByRef r_bIsTMPProduct As Boolean) As Integer

        Dim result As Integer = 0

        Dim vResult(,) As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("pfprem_finance_cnt", CStr(v_lPremFinancePlanCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "v_lPremFinancePlanCnt:=" & v_lPremFinancePlanCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectProductTypefromPFSQL, sSQLName:=ACSelectProductTypefromPFName, bStoredProcedure:=True, vResultArray:=vResult)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectProductTypefromPFSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        'Get the date
        If Informations.IsArray(vResult) Then
            r_bIsTMPProduct = gPMFunctions.ToSafeBoolean(vResult(0, 0))
        End If

        Return result
    End Function

    Private Function getInstalmentStatus(ByVal v_lPremFinancePlanCnt As Integer, ByVal v_lPremFinancePlanVersion As Integer, ByRef r_lStatusId As Integer) As Integer
        Dim result As Integer = 0

        Dim vResult(,) As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add("pfprem_finance_cnt", CStr(v_lPremFinancePlanCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "v_lPremFinancePlanCnt:=" & v_lPremFinancePlanCnt, gPMConstants.PMELogLevel.PMLogError)
        End If
        m_lReturn = m_oDatabase.Parameters.Add("pfprem_finance_version", CStr(v_lPremFinancePlanVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "v_lPremFinancePlanVersion:=" & v_lPremFinancePlanVersion, gPMConstants.PMELogLevel.PMLogError)
        End If
        m_lReturn = m_oDatabase.Parameters.Add("statusId", CStr(r_lStatusId), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "r_lStatusId:=" & r_lStatusId, gPMConstants.PMELogLevel.PMLogError)
        End If
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectFirstInstalmentStatusPFSQL, sSQLName:=ACSelectFirstInstalmentStatusPFName, bStoredProcedure:=True, vResultArray:=vResult)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectFirstInstalmentStatusPFSQL", gPMConstants.PMELogLevel.PMLogError)
        End If

        r_lStatusId = gPMFunctions.ToSafeLong(m_oDatabase.Parameters.Item("statusId").Value)
        Return result
    End Function

    Private Function SetupCreditControlDetailsForCancelledSupersededPlan(ByVal v_nInsurance_file_cnt As Integer) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SetupCreditControlDetailsForCancelledSupersededPlan"
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' update premium finance plan
            AddParameterLite(m_oDatabase, "insurance_file_cnt", v_nInsurance_file_cnt, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "business_type", "IMPORT", PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute sql 
            nResult = m_oDatabase.SQLAction(sSQL:="spu_ACT_Add_Credit_Control_Item_InsFile", sSQLName:="spu_ACT_Add_Credit_Control_Item_InsFile", bStoredProcedure:=True)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=spu_ACT_Add_Credit_Control_Item_InsFile", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            Return gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult

    End Function

    Private Function CreateTaskInstance(ByVal sDescription As String) As Integer
        Dim nResult As Integer
        Const kMethodName As String = "CreateTaskInstance"
        Try

            AddParameterLite(m_oDatabase, "pmwrk_task_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "pmwrk_task_group_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "pmuser_group_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "pmuser_id", 0, PMEParameterDirection.PMParamOutput, PMEDataType.PMLong)

            ' Execute sql
            nResult = m_oDatabase.SQLSelect("spu_ACT_Import_Get_Default_Task_Details", "spu_ACT_Import_Get_Default_Task_Details", True)

            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=spu_ACT_Import_Get_Default_Task_Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            Dim taskId As Integer = ToSafeInteger(m_oDatabase.Parameters.Item("pmwrk_task_id").Value, 0)
            Dim taskGroupId As Integer = ToSafeInteger(m_oDatabase.Parameters.Item("pmwrk_task_group_id").Value, 0)
            Dim userGroupId As Integer = ToSafeInteger(m_oDatabase.Parameters.Item("pmuser_group_id").Value, 0)

            AddParameterLite(m_oDatabase, "pmwrk_task_instance_cnt", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong, True)
            AddParameterLite(m_oDatabase, "pmwrk_task_group_id", taskGroupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "pmwrk_task_id", taskId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "customer", "Instalment Recall", PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "task_due_date", Date.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "pmuser_group_id", userGroupId, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "user_id", m_iUserID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "description", sDescription, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "task_status", CStr(0), PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            AddParameterLite(m_oDatabase, "is_urgent", 0, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "date_created", Date.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "created_by_id", m_iUserID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "last_modified", Date.Now, PMEParameterDirection.PMParamInput, PMEDataType.PMDate)
            AddParameterLite(m_oDatabase, "modified_by_id", m_iUserID, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "is_visible", 1, PMEParameterDirection.PMParamInput, PMEDataType.PMLong)
            AddParameterLite(m_oDatabase, "workflow_information", "", PMEParameterDirection.PMParamInput, PMEDataType.PMString)

            ' Execute sql
            nResult = m_oDatabase.SQLAction("spe_PMWrk_Task_Instance_add", "spe_PMWrk_Task_Instance_add", True)
            If nResult <> PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=spe_PMWrk_Task_Instance_add", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch excep As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult)
            Return gPMConstants.PMEReturnCode.PMError
        End Try

        Return nResult
    End Function

    Private Function GetPlanTransactionOutstanding(ByVal v_nPlanTrancationId As Integer, ByRef r_dOutstandingAmount As Decimal) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPlanTransactionOutstanding"

        Dim vResult(,) As Object = Nothing
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("nPlanTransactionId", v_nPlanTrancationId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "v_nPlanTrancationId:=" & v_nPlanTrancationId, gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPlanOutstandingAmountSQL, sSQLName:=ACGetPlanOutstandingAmountName, bStoredProcedure:=True, vResultArray:=vResult)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACSelectFirstInstalmentStatusPFSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_dOutstandingAmount = ToSafeDecimal(vResult(0, 0))
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
        End Try
        Return result
    End Function
    ''' <summary>
    ''' Call this method to know reverse allocation authority and no of days not exceed.
    ''' </summary>
    ''' <param name="nUserID"></param>
    ''' <param name="nPFInstalmentsID"></param>
    ''' <param name="r_bCanBeRecalled"></param>
    ''' <param name="r_bAllocationDaysExceeded"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CanInstalmentBeRecalled(ByVal nUserID As Integer, ByVal nPFInstalmentsID As Integer, ByRef r_bCanBeRecalled As Boolean, ByRef r_bAllocationDaysExceeded As Boolean) As Integer

        Try
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = m_oDatabase.Parameters.Add("user_id", nUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("pfinstalments_id", nPFInstalmentsID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("can_be_recalled", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.Parameters.Add("allocation_days_exceeded", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLAction(sSQL:=kCanInstalmentBeRecalledSQL, sSQLName:=kCanInstalmentBeRecalledName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("faild to execute kCanInstalmentBeRecalledSQL")
            End If

            r_bCanBeRecalled = ToSafeBoolean(m_oDatabase.Parameters.Item("can_be_recalled").Value)
            r_bAllocationDaysExceeded = ToSafeBoolean(m_oDatabase.Parameters.Item("allocation_days_exceeded").Value)

        Catch ex As Exception
            m_lReturn = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CanInstalmentBeRecalled Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CanInstalmentBeRecalled", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
        Return m_lReturn
    End Function
    ''' <summary>
    ''' Call this method to Recall instalment
    ''' </summary>
    ''' <param name="nPFInstalmentsID"></param>
    ''' <param name="sPFPlanStatusInd"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReverseCollectedInstalment(ByVal nPFInstalmentsID As Integer, ByVal sPFPlanStatusInd As String) As Integer

        Dim oSIRPFBusiness As Object = Nothing
        Dim nRecalled As Integer
        Dim nNotRecalled As Integer
        Dim bTransStarted As Boolean
        Dim aoFinanceArray(,) As Object = Nothing
        Try
            m_lReturn = gPMConstants.PMEReturnCode.PMTrue

            If GetBusinessObject(v_sClass:="bSIRPremiumFinance.Business", r_oObject:=oSIRPFBusiness) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMError
                Throw New ApplicationException("Unable to get business object: bSIRPremiumFinance.Business.")
            End If
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                bTransStarted = True
            End If
            'Recall Instalments
            m_lReturn = RecallInstalments(lBatchID:=0, r_lRecordsRecalled:=nRecalled, r_lFailedRecall:=nNotRecalled,
                                    nPFInstalmentsID:=nPFInstalmentsID,
                                    bRecallInstalmentFromInstalmentMaint:=True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("RecallInstalments() method Failed")
            End If
            If nNotRecalled = 0 Then
                'Update instalment status to 'New' after Recall
                m_lReturn = oSIRPFBusiness.UpdateInstalmentStatus(lPFInstalmentId:=ToSafeInteger(nPFInstalmentsID), lPFInstalmentStatusId:=1)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("UpdateInstalmentStatus() method Failed.")
                End If
                'Update instalment reason to reverse after Recall
                m_lReturn = UpdateInstalmentResult(nPFInstalmentId:=nPFInstalmentsID, sPFInstalmentResultCode:="REV")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("UpdateInstalmentResult() method Failed.")
                End If
                'Update instalment delete cashlist item after Recall
                m_lReturn = DeleteCashListItemInstalments(nPFInstalmentId:=nPFInstalmentsID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New ApplicationException("DeleteCashListItemInstalments() method Failed.")
                End If
                If sPFPlanStatusInd = PFStatusIndCompleted Then
                    'Get finance count and their version using PFIntalment_Id
                    m_lReturn = GetFinancePlanDetails(nPFInstalmentsID, aoFinanceArray)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("GetFinancePlanDetails method Failed.")
                    End If
                    'Update plan status Completed to live after Recall.
                    m_lReturn = oSIRPFBusiness.StatusUpdate(vPremiumFinanceCnt:=aoFinanceArray(0, 0), vPremiumFinanceVersion:=aoFinanceArray(1, 0), vStatusInd:=PFStatusIndLive)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New ApplicationException("failed StatusUpdate() method.")
                    End If
                End If
            End If
            CommitTrans()
        Catch ex As Exception
            m_lReturn = PMEReturnCode.PMError
            If bTransStarted Then
                RollbackTrans()
            End If
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseCollectedInstalment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseCollectedInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        Finally
            If Not oSIRPFBusiness Is Nothing Then
                oSIRPFBusiness.Dispose()
                oSIRPFBusiness = Nothing
            End If
        End Try
        Return m_lReturn
    End Function
    ''' <summary>
    ''' Call this method to get no of instalment which going to Recall.
    ''' </summary>
    ''' <param name="nPFInstalmentsID"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetInstalmentForRecall(ByVal nPFInstalmentsID As Integer, ByRef r_oResults(,) As Object) As Integer

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = m_oDatabase.Parameters.Add("pfinstalments_id", nPFInstalmentsID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

        ' Execute selection Query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetInstalmentForRecallSQL, sSQLName:=kGetInstalmentForRecallName, bStoredProcedure:=True, vResultArray:=r_oResults)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = gPMConstants.PMEReturnCode.PMError
            Throw New ApplicationException("failed to execute kGetInstalmentForRecallSQL")
        End If
        If Not Informations.IsArray(r_oResults) Then
            m_lReturn = gPMConstants.PMEReturnCode.PMNotFound
        End If
        Return m_lReturn
    End Function
    ''' <summary>
    ''' Call this method to update instalment status and update result code.
    ''' </summary>
    ''' <param name="nPFInstalmentId"></param>
    ''' <param name="sPFInstalmentResultCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function UpdateInstalmentResult(ByVal nPFInstalmentId As Integer, ByVal sPFInstalmentResultCode As String) As Integer

        Dim nPFInstalmentResultId As Integer

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = m_oDatabase.Parameters.Add("code", sPFInstalmentResultCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
        m_lReturn = m_oDatabase.Parameters.Add("pfinstalments_result_id", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMInteger)

        ' Execute Action Query
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kGetPfInstalmentsResultIdSQL, sSQLName:=kGetPfInstalmentsResultIdName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("failed to execute kGetPfInstalmentsResultIdSQL")
        End If

        nPFInstalmentResultId = ToSafeInteger(m_oDatabase.Parameters.Item("pfinstalments_result_id").Value)

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="pfinstalments_id", v_vValue:=nPFInstalmentId, v_iType:=gPMConstants.PMEDataType.PMInteger)
        m_lReturn = AddInputParameter(v_sName:="pfinstalments_result_id", v_vValue:=nPFInstalmentResultId, v_iType:=gPMConstants.PMEDataType.PMInteger)

        ' Execute Action Query
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kUpdateInstalmentResultSQL, sSQLName:=kUpdateInstalmentResultName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("failed to execute kUpdateInstalmentResultSQL")
        End If

        Return m_lReturn
    End Function
    ''' <summary>
    ''' Call this method to delete cash list item.
    ''' </summary>
    ''' <param name="nPFInstalmentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeleteCashListItemInstalments(ByVal nPFInstalmentId As Integer) As Integer

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = AddInputParameter(v_sName:="pfinstalments_id", v_vValue:=nPFInstalmentId, v_iType:=gPMConstants.PMEDataType.PMInteger)
        ' Execute Action Query
        m_lReturn = m_oDatabase.SQLAction(sSQL:=kDeleteCashListItemInstalmentsSQL, sSQLName:=kDeleteCashListItemInstalmentsName, bStoredProcedure:=True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("failed to execute kDeleteCashListItemInstalmentsSQL")
        End If

        Return m_lReturn
    End Function
    ''' <summary>
    ''' Call this method to return finance count and their version
    ''' </summary>
    ''' <param name="nPFInstalmentsID"></param>
    ''' <param name="r_aoResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetFinancePlanDetails(ByVal nPFInstalmentsID As Integer, ByRef r_aoResults(,) As Object) As Integer

        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters
        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        m_lReturn = m_oDatabase.Parameters.Add("pfinstalments_id", nPFInstalmentsID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

        ' Execute selection Query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetFinancePlanDetailsSQL, sSQLName:=kGetFinancePlanDetailsSQLName, bStoredProcedure:=True, vResultArray:=r_aoResults)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException("failed to execute kGetFinancePlanDetailsSQL")
        End If

        If Not Informations.IsArray(r_aoResults) Then
            m_lReturn = gPMConstants.PMEReturnCode.PMNotFound
        End If
        Return m_lReturn
    End Function

    Public Function IsInstalmentsPosted(ByVal v_nPfinstalmentID As Integer) As Integer

        Dim nPfTransactiionID As Integer
        Dim nResult As Integer = 0

        Dim vResult(,) As Object = Nothing


        nResult = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        nResult = m_oDatabase.Parameters.Add("instalmentid", CStr(v_nPfinstalmentID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "v_nPfinstalmentID:=" & v_nPfinstalmentID, gPMConstants.PMELogLevel.PMLogError)
            Return nResult
        End If

        nResult = m_oDatabase.SQLSelect(sSQL:=ACGetPFTransactionIDSQL, sSQLName:=ACGetPFTransactionIDName, bStoredProcedure:=ACGetPFTransactionIDtored, vResultArray:=vResult)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", ACGetPFTransactionIDSQL, gPMConstants.PMELogLevel.PMLogError)
            Return nResult
        End If

        If Informations.IsArray(vResult) Then
            nPfTransactiionID = vResult(0, 0)
            If nPfTransactiionID = 0 Then
                nResult = gPMConstants.PMEReturnCode.PMTrue
            Else
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

        Else
            nResult = gPMConstants.PMEReturnCode.PMTrue
        End If

        Return nResult
    End Function

    Private Function GetInstalmentsRemaining(ByVal v_lpfprem_finance_cnt As Integer, ByVal v_lpfprem_finance_version As Integer, ByRef r_lNoInstalments As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_cnt", vValue:=gPMFunctions.ToSafeString(v_lpfprem_finance_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="pfprem_finance_version", vValue:=gPMFunctions.ToSafeString(v_lpfprem_finance_version), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInstalmentsRemainingSQL, sSQLName:=ACGetInstalmentsRemainingName, bStoredProcedure:=ACGetInstalmentsRemainingStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lNoInstalments = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInstalmentsRemaining Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInstalmentsRemaining", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
