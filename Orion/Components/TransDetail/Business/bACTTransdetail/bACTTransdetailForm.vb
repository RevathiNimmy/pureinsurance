Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports Microsoft.CodeAnalysis.VisualBasic
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 08/08/1997
    ' 
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Transdetail.
    '
    ' Edit History: TF191198 - amendments for EMU database changes
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 26/01/2004
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    'FSA Phase 3.2
    Private m_oDocumentPost As Object
    Private m_oAllocationManual As Object
    Private m_oDocument As bACTDocument.Form
    Private m_oDocumentReversal As Object
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oCurrencyConvert As Object
    'FSA Phase 3.2 End

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Collection of Transdetails (Private)
    Private m_oTransDetails As Transdetails

    Private m_oLookup As BPMLOOKUP.Business

    ' Database Class (Private)

    Public m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' NavigatorV3 variables
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    Private m_bSuppressDecimalValues As Boolean

    ' PRIVATE Data Members (End)

    ' CTAF 20030411 - Constants for the CacheArray start
    Private Const ACArrayUserID As Integer = 0
    Private Const ACArraySourceID As Integer = 1
    Private Const ACArrayAccountID As Integer = 2
    Private Const ACArrayCurrencyID As Integer = 3
    Private Const ACArrayAmount As Integer = 4
    Private Const ACArrayComment As Integer = 5
    Private Const ACArrayBaseAmount As Integer = 6
    Private Const ACArrayExchangeRate As Integer = 7
    Private Const ACArrayEuroAmount As Integer = 8
    Private Const ACArrayDepartmentID As Integer = 9
    Private Const ACArrayInsuranceRef As Integer = 10
    Private Const ACArrayPurchaseOrderNo As Integer = 11
    Private Const ACArrayPurchaseInvoiceNo As Integer = 12
    Private Const kRoundingUpto As Integer = 2

    ' CTAF 20030411 - Constants for the CacheArray end

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    Public Property CallingAppName() As String
        Get
            Return m_sCallingAppName
        End Get
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public Property PMAuthorityLevel() As Integer
        Get
            Return m_lPMAuthorityLevel
        End Get
        Set(ByVal Value As Integer)
            m_lPMAuthorityLevel = Value
        End Set
    End Property

    Public Property Status() As Integer
        Get
            Return m_lStatus
        End Get
        Set(ByVal Value As Integer)
            m_lStatus = Value
        End Set
    End Property

    ' CF031298
    Public ReadOnly Property Details() As Transdetails
        Get
            Return m_oTransDetails
        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oTransDetails.Count()
                    m_lCurrentRecord = m_oTransDetails.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oTransDetails.Count()

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

    'Public Property Get Details() As bACTTransdetail.Transdetails
    '
    '  Set Details = m_oTransdetails
    '

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetKeys
    '
    ' Description: Navigator SetKeys function.
    '
    ' ***************************************************************** '
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeys
    '
    ' Description: Navigator GetKeys function.
    '
    ' ***************************************************************** '

    Public Function GetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            vKeyArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeysFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSummary
    '
    ' Description: GetSummary Navigator function.
    '
    ' ***************************************************************** '
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StartFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

            ' Set database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialisation Code.

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create Transdetails Collection
            m_oTransDetails = New Transdetails()
            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            'Get the Decimal Suppression flag
            Dim sTempOptionValue As String = ""
            bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID,
                                          v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName,
                                          v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableDecimalsSuppression, v_vBranch:=m_iSourceID, r_vUnderwriting:=sTempOptionValue)

            If sTempOptionValue.Trim = "1" Then
                m_bSuppressDecimalValues = True
            End If



            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

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
                m_oTransDetails = Nothing

                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oAllocationManual IsNot Nothing Then
                    m_oAllocationManual.Dispose()
                    m_oAllocationManual = Nothing
                End If
                If m_oDocument IsNot Nothing Then
                    m_oDocument.Dispose()
                    m_oDocument = Nothing
                End If
                If m_oDocumentReversal IsNot Nothing Then
                    m_oDocumentReversal.Dispose()
                    m_oDocumentReversal = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If

                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
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

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single Transdetail directly into the database.
    '        Note: The Transdetail will NOT be added to the collection.
    '
    ' ***************************************************************** '

    Public Function DirectAdd(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing,
                              Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing,
                              Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing,
                              Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing,
                              Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing,
                              Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing,
                              Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing,
                              Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing,
                              Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing,
                              Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing,
                              Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing,
                              Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing,
                              Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing,
                              Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing,
                              Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing,
                              Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing,
                              Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vCurrencyBaseDate As Object = Nothing,
                              Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vAccountBaseDate As Object = Nothing,
                              Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vSystemBaseDate As Object = Nothing,
                              Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing,
                              Optional ByRef vTypeCode As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing,
                              Optional ByRef vTaxBandID As Object = Nothing, Optional ByRef vClaimReference As Object = Nothing,
                              Optional ByRef vBalanceType As Object = Nothing, Optional ByRef vRiskTransfer As Object = Nothing, Optional ByRef vDueDate As Object = Nothing,
                              Optional ByVal oFeeType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransDetail As Transdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Transdetail
            oTransDetail = New Transdetail()

            If Convert.IsDBNull(vRiskTransfer) Or Informations.IsNothing(vRiskTransfer) Or Informations.IsNothing(vRiskTransfer) Then

                vRiskTransfer = GetRiskTransferStatus(lAccountID:=vAccountID, lDocumentID:=vDocumentID, cAmount:=vAmount)
            End If

            ' Populate Transdetail Attributes

            m_lReturn = SetProperties(oTransDetail, gPMConstants.PMEComponentAction.PMAdd, vTransdetailID:=vTransdetailID, vAccountID:=vAccountID,
                                      vPostingstatusID:=vPostingstatusID, vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID,
                                      vDocumentID:=vDocumentID, vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate,
                                      vAmount:=vAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched,
                                      vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded,
                                      vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate,
                                      vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID,
                                      vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment,
                                      vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity, vRefUnits:=vRefUnits,
                                      vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID, vInsuranceRefIndex:=vInsuranceRefIndex,
                                      vCurrencyBaseXrate:=vCurrencyBaseXrate, vCurrencyBaseDate:=vCurrencyBaseDate, vAccountBaseXrate:=vAccountBaseXrate,
                                      vAccountBaseDate:=vAccountBaseDate, vSystemBaseXrate:=vSystemBaseXrate, vSystemBaseDate:=vSystemBaseDate,
                                      vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference, vTypeCode:=vTypeCode, vTaxGroupID:=vTaxGroupID,
                                      vTaxBandID:=vTaxBandID, vClaimReference:=vClaimReference, vBalanceType:=vBalanceType, vRiskTransfer:=vRiskTransfer, vDueDate:=vDueDate, oFeeType:=oFeeType)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Transdetail to the Database
            m_lReturn = AddItem(oTransDetail)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the Transdetail Added

            If Not Informations.IsNothing(vTransdetailID) Then
                vTransdetailID = oTransDetail.TransdetailID
            End If

            oTransDetail = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Transdetail directly from the database.
    '        Note: The Transdetail will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(ByRef vTransdetailID As Object) As Integer

        Dim result As Integer = 0
        Dim oTransDetail As Transdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Transdetail
            oTransDetail = New Transdetail()

            ' Populate Transdetail Attributes

            m_lReturn = SetProperties(oTransDetail, gPMConstants.PMEComponentAction.PMDelete, vTransdetailID:=vTransdetailID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Transdetail to the Database
            m_lReturn = DeleteItem(oTransDetail)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oTransDetail = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Transdetail.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vCurrencyBaseDate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vAccountBaseDate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vSystemBaseDate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vTransdetailID:=vTransdetailID, vAccountID:=vAccountID, vPostingstatusID:=vPostingstatusID, vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=vDocumentID, vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=vAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity, vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID, vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate, vCurrencyBaseDate:=vCurrencyBaseDate, vAccountBaseXrate:=vAccountBaseXrate, vAccountBaseDate:=vAccountBaseDate, vSystemBaseXrate:=vSystemBaseXrate, vSystemBaseDate:=vSystemBaseDate, vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference, vTypeCode:=vTypeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=vID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected

            lRecordCount = m_oDatabase.Records.Count

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer
        Return GetCaptions(vID:=vID, vFieldArray:=vFieldArray, vResultArray:=vResultArray, vTable:=Nothing)
    End Function

    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer

        Dim result As Integer = 0

        Dim oFields As DataRow

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that this record exists
            m_lReturn = CheckID(vID:=vID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned

            oFields = m_oDatabase.Records.Item(0).Fields

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - scalability change - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type

                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, DbType.String, DbType.String, DbType.String, DbType.String

                                vResults(lSub) = ""

                                'Case DbType.Date, adDBDate
                            Case DbType.Date

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ************************************************************************* '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Transdetails and populate the Collection
    '
    ' ************************************************************************* '

    Public Function GetDetails(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vOSAmounts As Object = Nothing, Optional ByRef vLockMode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oTransDetail As Transdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oTransDetails.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection

            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vTransdetailID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vTransdetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vTransdetailID =" & CStr(vTransdetailID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the TransdetailID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Transdetail_id", vValue:=vTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                'Tomo31012002 - Changed from PMInteger

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                'BB No Detail ID so we should have a Document ID
                ' to get all items belonging to a given Document

                'BB Check Document ID is present

                If Informations.IsNothing(vDocumentID) Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Document ID is missing", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                    Return result
                End If

                'BB Check Document ID is numeric

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vDocumentID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vDocumentID =" & CStr(vDocumentID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")
                    Return result
                End If

                'BB Add the Document ID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="document_id", vValue:=vDocumentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected

            lRecordCount = m_oDatabase.Records.Count

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 0 To lRecordCount - 1
                    ' Create New Transdetail
                    oTransDetail = New Transdetail()

                    m_lReturn = SetPropertiesFromDB(oTransDetail:=oTransDetail, lRecordNumber:=lSub)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add Transdetail to collection
                    If (m_oTransDetails.Count = 0) Then
                        m_oTransDetails.Add(Nothing)
                    End If
                    m_lReturn = m_oTransDetails.Add(oNewTransdetail:=oTransDetail)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oTransDetail = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required Transdetails and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing,
                            Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing,
                            Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing,
                            Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing,
                            Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing,
                            Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing,
                            Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing,
                            Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing,
                            Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing,
                            Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing,
                            Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing,
                            Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing,
                            Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing,
                            Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing,
                            Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing,
                            Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing,
                            Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing,
                            Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing,
                            Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing,
                            Optional ByRef vBaseCurrencyID As Object = Nothing, Optional ByRef vAccountCurrencyID As Object = Nothing,
                            Optional ByRef vSystemCurrencyID As Object = Nothing, Optional ByRef vAccountAmount As Object = Nothing,
                            Optional ByRef vAccountAmountUnrounded As Object = Nothing, Optional ByRef vSystemAmount As Object = Nothing,
                            Optional ByRef vSystemAmountUnrounded As Object = Nothing, Optional ByRef vOSCurrencyAmount As Object = Nothing,
                            Optional ByRef vOSBaseAmount As Object = Nothing, Optional ByRef vOSAccountAmount As Object = Nothing,
                            Optional ByRef vOSSystemAmount As Object = Nothing, Optional ByRef vAmountUpdated As Object = Nothing,
                            Optional ByRef vClaimReference As Object = Nothing, Optional ByRef vBalanceType As Object = Nothing,
                            Optional ByRef vDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransDetail As Transdetail
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oTransDetails.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oTransDetail = m_oTransDetails.Item(m_lCurrentRecord)

            ' Get the Transdetail Property Values
            m_lReturn = GetProperties(oTransDetail, iStatus, vTransdetailID:=vTransdetailID, vAccountID:=vAccountID, vPostingstatusID:=vPostingstatusID,
                                      vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=vDocumentID,
                                      vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=vAmount,
                                      vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount,
                                      vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount,
                                      vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef,
                                      vOperatorID:=vOperatorID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo,
                                      vDepartment:=vDepartment, vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity,
                                      vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID,
                                      vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate, vAccountBaseXrate:=vAccountBaseXrate,
                                      vSystemBaseXrate:=vSystemBaseXrate, vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference,
                                      vTypeCode:=vTypeCode, vBaseCurrencyID:=vBaseCurrencyID, vAccountCurrencyID:=vAccountCurrencyID,
                                      vSystemCurrencyID:=vSystemCurrencyID, vAccountAmount:=vAccountAmount, vAccountAmountUnrounded:=vAccountAmountUnrounded,
                                      vSystemAmount:=vSystemAmount, vSystemAmountUnrounded:=vSystemAmountUnrounded, vOSCurrencyAmount:=vOSCurrencyAmount,
                                      vOSBaseAmount:=vOSBaseAmount, vOSAccountAmount:=vOSAccountAmount, vOSSystemAmount:=vOSSystemAmount,
                                      vAmountUpdated:=vAmountUpdated, vClaimReference:=vClaimReference, vBalanceType:=vBalanceType, vDueDate:=vDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oTransDetail = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result

        End Try
    End Function
    'eck050600
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRClaim.
    '
    '
    ' ***************************************************************** '

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray As Object
        Dim vFieldArray As Object
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}
            ' Setup Lookup Table Names
            ReDim vTabArray(3, 0)

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = gACTLibrary.ACTLookupCurrency

            ' {* USER DEFINED CODE (End) *}

            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            vFieldArray = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValue  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied Transdetail into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing,
                            Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing,
                            Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing,
                            Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing,
                            Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing,
                            Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing,
                            Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing,
                            Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing,
                            Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing,
                            Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing,
                            Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing,
                            Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing,
                            Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing,
                            Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing,
                            Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing,
                            Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing,
                            Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vCurrencyBaseDate As Object = Nothing,
                            Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vAccountBaseDate As Object = Nothing,
                            Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vSystemBaseDate As Object = Nothing,
                            Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing,
                            Optional ByRef vTypeCode As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing,
                            Optional ByRef vTaxBandID As Object = Nothing, Optional ByRef vClaimReference As Object = Nothing,
                            Optional ByRef vBalanceType As Object = Nothing, Optional ByRef vDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransDetail As Transdetail
        Dim vRiskTransfer As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)

            If m_oTransDetails.Count() <> (lRow) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Transdetail
            oTransDetail = New Transdetail()

            'Get the Risk Transfer Agreement for this Account (returns null if not an insurer account)

            vRiskTransfer = GetRiskTransferStatus(lAccountID:=vAccountID, lDocumentID:=vDocumentID, cAmount:=vAmount)

            ' Populate Transdetail Attributes

            m_lReturn = SetProperties(oTransDetail, gPMConstants.PMEComponentAction.PMAdd, vTransdetailID:=vTransdetailID, vAccountID:=vAccountID,
                                      vPostingstatusID:=vPostingstatusID, vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID,
                                      vDocumentID:=vDocumentID, vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate,
                                      vAmount:=vAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched,
                                      vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded,
                                      vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate,
                                      vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID,
                                      vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment,
                                      vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity,
                                      vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID,
                                      vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate, vCurrencyBaseDate:=vCurrencyBaseDate,
                                      vAccountBaseXrate:=vAccountBaseXrate, vAccountBaseDate:=vAccountBaseDate, vSystemBaseXrate:=vSystemBaseXrate,
                                      vSystemBaseDate:=vSystemBaseDate, vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference,
                                      vTypeCode:=vTypeCode, vTaxGroupID:=vTaxGroupID, vTaxBandID:=vTaxBandID, vClaimReference:=vClaimReference,
                                      vBalanceType:=vBalanceType, vRiskTransfer:=vRiskTransfer, vDueDate:=vDueDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oTransDetail = Nothing
                Return result
            End If

            ' Add Transdetail to collection
            If (m_oTransDetails.Count = 0) Then
                m_oTransDetails.Add(Nothing)
            End If
            m_lReturn = m_oTransDetails.Add(oNewTransdetail:=oTransDetail)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oTransDetail = Nothing
                Return result
            End If

            oTransDetail = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the Transdetail
    '              specified and updates the Transdetail with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vCurrencyBaseDate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vAccountBaseDate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vSystemBaseDate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vTaxBandID As Object = Nothing, Optional ByRef vClaimReference As Object = Nothing, Optional ByRef vBalanceType As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oTransDetail As Transdetail
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oTransDetails.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oTransDetail = m_oTransDetails.Item(lRow)

            ' Check the Status of the Transdetail

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oTransDetail.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update Transdetail Attributes

            m_lReturn = SetProperties(oTransDetail, iStatus, vTransdetailID:=vTransdetailID, vAccountID:=vAccountID, vPostingstatusID:=vPostingstatusID, vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=vDocumentID, vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=vAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity, vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID, vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate, vCurrencyBaseDate:=vCurrencyBaseDate, vAccountBaseXrate:=vAccountBaseXrate, vAccountBaseDate:=vAccountBaseDate, vSystemBaseXrate:=vSystemBaseXrate, vSystemBaseDate:=vSystemBaseDate, vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference, vTypeCode:=vTypeCode, vTaxGroupID:=vTaxGroupID, vTaxBandID:=vTaxBandID, vClaimReference:=vClaimReference, vBalanceType:=vBalanceType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oTransDetail = Nothing
                Return result
            End If

            ' Release reference to Transdetail
            oTransDetail = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Transdetail can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oTransDetail As Transdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oTransDetails.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oTransDetail = m_oTransDetails.Item(lRow)

            ' Check the Status of the Transdetail

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oTransDetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oTransDetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oTransDetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Transdetail
            oTransDetail = Nothing

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oTransDetails.Count()
                Select Case m_oTransDetails.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oTransDetail As Transdetail
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oTransDetails.Count()
                oTransDetail = m_oTransDetails.Item(lSub)

                Select Case oTransDetail.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = AddItem(oTransDetail)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = UpdateItem(oTransDetail)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = BeginTrans()
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = DeleteItem(oTransDetail)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oTransDetail = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CommitTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oTransDetails.Count()

                        ' With the item
                        With m_oTransDetails.Item(lSub)

                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oTransDetails.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = RollbackTrans()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadTransCache
    '
    ' Description: Loads the cache from the database
    '              Used by iACTTransaction.TransDetailCache
    '
    ' History: 11/04/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function LoadTransCache(ByVal v_lUserID As Integer, ByVal v_lSourceID As Integer, ByRef r_lAccountID As Integer, ByRef r_lCurrencyID As Integer, ByRef r_dAmount As Double, ByRef r_sComment As String, ByRef r_dBaseAmount As Double, ByRef r_dExchangeRate As Double, ByRef r_dAmountInEuros As Double, ByRef r_lDepartmentID As Integer, ByRef r_sInsuranceRef As String, ByRef r_sPurchaseOrderNo As String, ByRef r_sPurchaseInvoiceNo As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters

            m_oDatabase.Parameters.Clear()

            ' Add user_id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=v_lUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add source_id

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_lSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL ACSelCacheStored

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelCacheSQL, sSQLName:=ACSelCacheName, bStoredProcedure:=ACSelCacheStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQLSelect failed for " & ACSelCacheSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="LoadTransCache", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Just exit if we have nothing
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Assign the return results

            r_lAccountID = CInt(vResultArray(ACArrayAccountID, 0))

            r_lCurrencyID = CInt(vResultArray(ACArrayCurrencyID, 0))

            r_dAmount = CDbl(vResultArray(ACArrayAmount, 0))

            r_sComment = CStr(vResultArray(ACArrayComment, 0))

            r_dBaseAmount = CDbl(vResultArray(ACArrayBaseAmount, 0))

            r_dExchangeRate = CDbl(vResultArray(ACArrayExchangeRate, 0))

            r_dAmountInEuros = CDbl(vResultArray(ACArrayEuroAmount, 0))

            If CStr(vResultArray(ACArrayDepartmentID, 0)) = "" Then
                r_lDepartmentID = 0
            Else

                r_lDepartmentID = CInt(vResultArray(ACArrayDepartmentID, 0))
            End If

            r_sInsuranceRef = CStr(vResultArray(ACArrayInsuranceRef, 0))

            r_sPurchaseOrderNo = CStr(vResultArray(ACArrayPurchaseOrderNo, 0))

            r_sPurchaseInvoiceNo = CStr(vResultArray(ACArrayPurchaseInvoiceNo, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoadTransCache Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadTransCache", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveTransCache
    '
    ' Description: Saves the cached information to the database
    '              Used by iACTTransaction.TransDetailCache
    '
    ' History: 11/04/2003 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function SaveTransCache(ByVal v_lUserID As Integer, ByVal v_lSourceID As Integer, ByVal v_lAccountID As Integer, ByVal v_lCurrencyID As Integer, ByVal v_dAmount As Double, ByVal v_sComment As String, ByVal v_dBaseAmount As Double, ByVal v_dExchangeRate As Double, ByVal v_dAmountInEuros As Double, ByVal v_lDepartmentID As Integer, ByVal v_sInsuranceRef As String, ByVal v_sPurchaseOrderNo As String, ByVal v_sPurchaseInvoiceNo As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=v_lUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_lSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="account_id", vValue:=v_lAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @currency_id smallint,

            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=v_lCurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @amount numeric(19, 4),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="amount", vValue:=v_dAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @comment varchar(255),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="comment", vValue:=v_sComment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @base_amount numeric(19, 4),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="base_amount", vValue:=v_dBaseAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @exchange_rate numeric(19, 4),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="exchange_rate", vValue:=v_dExchangeRate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @euro_amount numeric(19, 4),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="euro_amount", vValue:=v_dAmountInEuros, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @department_id int,
            If v_lDepartmentID < 1 Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="department_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else

                m_lReturn = m_oDatabase.Parameters.Add(sName:="department_id", vValue:=v_lDepartmentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @insurance_ref varchar(255),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref", vValue:=v_sInsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @purchase_order_no varchar(255),

            m_lReturn = m_oDatabase.Parameters.Add(sName:="purchase_order_no", vValue:=v_sPurchaseOrderNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    @purchase_invoice_no varchar(255)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="purchase_invoice_no", vValue:=v_sPurchaseInvoiceNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCacheSQL, sSQLName:=ACUpdateCacheName, bStoredProcedure:=ACUpdateCacheStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveTransCache Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveTransCache", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PostSuspendedTransaction
    '
    ' Description: Posts a transaction to the PF_Accounts_transactions table.
    '
    ' Written By: Simon.Baynes
    '
    ' Returns: PMTrue for success
    '
    ' Date: 22/10/2003
    '
    ' ***************************************************************** '
    Public Function PostSuspendedTransaction(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lAccountID As Integer, ByVal v_lTransdetailID As Integer, ByVal v_lTransactionType As Integer) As Integer
        Return PostSuspendedTransaction(v_lPremiumFinanceCnt:=v_lPremiumFinanceCnt, v_lPremiumFinanceVersion:=v_lPremiumFinanceVersion, v_lAccountID:=v_lAccountID, v_lTransdetailID:=v_lTransdetailID, v_lTransactionType:=v_lTransactionType, v_sSpare:="")
    End Function

    Public Function PostSuspendedTransaction(ByVal v_lPremiumFinanceCnt As Integer, ByVal v_lPremiumFinanceVersion As Integer, ByVal v_lAccountID As Integer, ByVal v_lTransdetailID As Integer, ByVal v_lTransactionType As Integer, ByVal v_sSpare As String) As Integer

        Dim result As Integer = 0

        Const sSQL As String = "spu_PFAccountsTransactions_Add"

        Const sSQLName As String = "AddSuspendedTransaction"
        Const sSQLStored As Boolean = True

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=v_lPremiumFinanceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=v_lPremiumFinanceVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="AccountID", vValue:=v_lAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="TransDetailID", vValue:=v_lTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="TransactionType", vValue:=v_lTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Spare", vValue:=v_sSpare, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLAction(sSQL:=sSQL, sSQLName:=sSQLName, bStoredProcedure:=sSQLStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PostSuspendedTransaction failed", vApp:=ACApp, vClass:=ACClass, vMethod:=result)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostSuspendedTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostSuspendedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    'FSA Phase 3.2 Functions
    Public Function GetTransDetailTypeCode(ByVal lTransdetailTypeID As Integer, ByRef sTransdetailTypeCode As String) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetTransDetailTypeCode
        ' PURPOSE: Get TransdetailType Code
        ' AUTHOR: Elaine Knott
        ' DATE: JAN 2005
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="TransdetailTypeId", vValue:=lTransdetailTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:="SELECT code from transdetail_type where transdetail_type_id = {TransdetailTypeId}", sSQLName:="GetTransDetailTypeCode", bStoredProcedure:=False, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - GetTransDetailTypeCode -  Failed")
                End If

                If Not Informations.IsArray(vResultArray) Then
                    sTransdetailTypeCode = ""
                    Return result
                End If

                sTransdetailTypeCode = CStr(vResultArray(0, 0))

            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransDetailTypeCode", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    Public Function MoveTransactionToSuspense(ByVal lTransdetailID As Integer, ByVal lDestinationAccountID As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double) As Integer
        Return MoveTransactionToSuspense(lTransdetailID:=lTransdetailID, lDestinationAccountID:=lDestinationAccountID, lLinkedTransdetailID:=lLinkedTransdetailID, dLinkedPercentage:=dLinkedPercentage, vPFPremiumFinanceCnt:=Nothing, vPFPremiumFinanceVersion:=Nothing)
    End Function

    Public Function MoveTransactionToSuspense(ByVal lTransdetailID As Integer, ByVal lDestinationAccountID As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double, ByVal vPFPremiumFinanceCnt As Object, ByVal vPFPremiumFinanceVersion As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function Move Transaction To Suspense
        ' PURPOSE:        Write Document to move transaction into suspense
        '                 to the suspense file
        ' AUTHOR:         Elaine Knott
        ' DATE:           DEC 2004
        ' REMARKS:        FSA Phase 3.2
        ' ---------------------------------------------------------------------------
        Dim result As Integer = 0
        Dim lInsuranceFileCnt As String = ""
        Dim sDocumentRef As String = ""
        Dim lDocumentID, iCompanyID, lTransDetailId_1, lTransDetailId_2 As Object
        Dim iCurrencyID, iAccountCurrencyID, iSystemCurrencyID As Integer
        Dim lAccountID As Integer
        Dim cBaseAmount, cCurrencyAmount, cAccountAmount, cSystemAmount As Decimal
        Dim vCurrencyBaseXrate As Object = Nothing
        Dim vAccountBaseXrate As Object = Nothing
        Dim vSystemBaseXrate As Object = Nothing
        Dim lTransdetailTypeID As Integer
        Dim sSpare As String = ""

        Dim vKeyArray As Array
        Dim vAllocationTrans As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDocument Is Nothing Then

                m_oDocument = New bACTDocument.Form
                m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - Failed to create bACTDocument.Form")
            End If

            If m_oDocumentPost Is Nothing Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oDocumentPost, v_sClassName:="bACTDocumentPost.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - Failed to create bACTDocumentPost.Form")
            End If

            If m_oAllocationManual Is Nothing Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationManual, v_sClassName:="bACTAllocationManual.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - Failed to create bACTAllocationManual.Form")
            End If

            'Get information from transaction which is to be moved to suspense

            m_lReturn = GetDetails(vTransdetailID:=lTransdetailID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransdetail.Form - MoveTransactionToSuspense - GetDetails Failed")
            End If

            'Get information from transaction which is to be moved to suspense

            m_lReturn = GetNext(vAccountID:=lAccountID, vCompanyID:=iCompanyID, vCurrencyID:=iCurrencyID, vOSCurrencyAmount:=cCurrencyAmount, vOSBaseAmount:=cBaseAmount, vOSAccountAmount:=cAccountAmount, vOSSystemAmount:=cSystemAmount, vCurrencyBaseXrate:=vCurrencyBaseXrate, vAccountBaseXrate:=vAccountBaseXrate, vSystemBaseXrate:=vSystemBaseXrate, vAccountCurrencyID:=iAccountCurrencyID, vSystemCurrencyID:=iSystemCurrencyID, vTransdetailTypeID:=lTransdetailTypeID, vDocumentID:=lDocumentID, vSpare:=sSpare)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransdetail.Form - MoveTransactionToSuspense - GetDetNext Failed")
            End If

            'Get Insurance File  from the document related to the transaction to be suspended

            m_lReturn = m_oDocument.GetDetails(vDocumentID:=lDocumentID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTDocument.GetDetails Failed")
            End If

            m_lReturn = m_oDocument.GetNext(vInsuranceFileCnt:=lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTDocument.GetNext Failed")
            End If

            ' Create Document

            m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=ToSafeInteger(gACTLibrary.ACTDocTypeJournal), v_sDocumentRef:=ToSafeString(sDocumentRef),
                                                    v_dtDocumentDate:=ToSafeDate(DateTime.Today), v_sComment:="", v_lInsuranceFileCnt:=ToSafeInteger(lInsuranceFileCnt),
                                                    r_vDocumentID:=lDocumentID, r_vDocSourceID:=iCompanyID, v_sReason:="", r_vSubBranchID:=iCompanyID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTDocumentPost.AddDocument Failed")
            End If

            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=ToSafeInteger(lAccountID), v_iCurrencyID:=ToSafeInteger(iCurrencyID), v_cAmount:=ToSafeDecimal(-cBaseAmount),
                                                       v_cCurrencyAmount:=ToSafeDecimal(-cCurrencyAmount), v_vdCurrencyBaseXRate:=vCurrencyBaseXrate, r_vTransDetailId:=lTransDetailId_1,
                                                       v_vDocumentSequence:=1, v_vBaseAmountUnrounded:=ToSafeDecimal(-cBaseAmount), v_vCurrencyAmountUnrounded:=ToSafeDecimal(-cCurrencyAmount),
                                                       v_vAccountingDate:=ToSafeDate(DateTime.Today), v_vDocSourceID:=ToSafeInteger(iCompanyID), v_vSubBranchId:=ToSafeInteger(iCompanyID), v_vAccountBaseXrate:=vAccountBaseXrate,
                                                       v_vSystemBaseXrate:=vSystemBaseXrate, v_vTransdetailTypeID:=ToSafeInteger(lTransdetailTypeID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTDocumentPost.AddTransaction Failed")
            End If

            m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=ToSafeInteger(lDestinationAccountID), v_iCurrencyID:=ToSafeInteger(iCurrencyID), v_cAmount:=ToSafeDecimal(cBaseAmount),
                                                       v_cCurrencyAmount:=ToSafeDecimal(cCurrencyAmount), v_vdCurrencyBaseXRate:=vCurrencyBaseXrate, r_vTransDetailId:=lTransDetailId_2,
                                                       v_vDocumentSequence:=2, v_vBaseAmountUnrounded:=ToSafeDecimal(cBaseAmount), v_vCurrencyAmountUnrounded:=ToSafeDecimal(cCurrencyAmount),
                                                       v_vAccountingDate:=ToSafeDate(DateTime.Today), v_vDocSourceID:=ToSafeInteger(iCompanyID), v_vSubBranchId:=ToSafeInteger(iCompanyID),
                                                       v_vAccountBaseXrate:=vAccountBaseXrate, v_vSystemBaseXrate:=vSystemBaseXrate, v_vTransdetailTypeID:=ToSafeInteger(lTransdetailTypeID))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTDocumentPost.AddTransaction Failed")
            End If

            m_lReturn = m_oDocumentPost.Commit()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTDocumentPost.Commit Failed")
            End If

            'Use the bACTAllocationManual component to do the allocation
            If m_oAllocationManual Is Nothing Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationManual, v_sClassName:="bACTAllocationManual.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - Failed to get bACTAllocationManual")
            End If

            ReDim vAllocationTrans(0)

            vAllocationTrans(0) = CStr(lTransDetailId_1) & "|" & CStr(cBaseAmount)
            vKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 3}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountID
            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(lTransdetailID) & "|" & CStr(-cBaseAmount)

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationTrans

            'Perform the allocation

            m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=CType(vKeyArray, Array))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTAllocationManual.SetKeys Failed")
            End If

            m_lReturn = m_oAllocationManual.Start()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - MoveTransactionToSuspense - bACTAllocationManual.Start Failed")
            End If

            'Write Record to Suspense

            m_lReturn = CreateSuspendedTransaction(lSuspendedTransdetailId:=lTransdetailID, lLinkedTransdetailID:=lLinkedTransdetailID, dLinkedPercentage:=dLinkedPercentage, vPFPremFinanceCnt:=vPFPremiumFinanceCnt, vPFPremFinanceVersion:=vPFPremiumFinanceVersion, lInsuranceFileCnt:=CInt(lInsuranceFileCnt), lDestinationAccountID:=lDestinationAccountID, lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, lTransdetailTypeID:=lTransdetailTypeID, sSpare:=sSpare)

            'Also  add this to the terminate
            If Not (m_oDocumentPost Is Nothing) Then

                m_oDocumentPost.Dispose()
                m_oDocumentPost = Nothing
            End If
            If Not (m_oAllocationManual Is Nothing) Then

                m_oAllocationManual.Dispose()
                m_oAllocationManual = Nothing
            End If
            If Not (m_oDocument Is Nothing) Then

                m_oDocument.Dispose()
                m_oDocument = Nothing
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveTransactionToSuspense", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer) As Integer
        Return ReleaseSuspendedTransactions(lAllocationId:=lAllocationId,
                                             vLinkedTransdetailID:=Nothing,
                                             vDocumentID:=Nothing,
                                             vPFPremiumFinanceCnt:=Nothing,
                                             vPFPremiumFinanceVersion:=Nothing,
                                             vPercentage:=Nothing,
                                             vAmount:=Nothing,
                                             vSuspendedTransdetailID:=Nothing,
                                             vAccountingDate:=Nothing,
                                             vInsuranceFileCnt:=Nothing,
                                             vLastInstalment:=False,
                                             v_iInstalmentID:=0)
    End Function

    Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer,
                                                ByVal vLinkedTransdetailID As Object) As Integer
        Return ReleaseSuspendedTransactions(lAllocationId:=lAllocationId,
                                             vLinkedTransdetailID:=vLinkedTransdetailID,
                                             vDocumentID:=Nothing,
                                             vPFPremiumFinanceCnt:=Nothing,
                                             vPFPremiumFinanceVersion:=Nothing,
                                             vPercentage:=Nothing,
                                             vAmount:=Nothing,
                                             vSuspendedTransdetailID:=Nothing,
                                             vAccountingDate:=Nothing,
                                             vInsuranceFileCnt:=Nothing,
                                             vLastInstalment:=False,
                                             v_iInstalmentID:=0)
    End Function

    Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer,
                                             ByVal vLinkedTransdetailID As Object,
                                             ByVal vAccountingDate As Object) As Integer
        Return ReleaseSuspendedTransactions(lAllocationId:=lAllocationId,
                                             vLinkedTransdetailID:=vLinkedTransdetailID,
                                             vDocumentID:=Nothing,
                                             vPFPremiumFinanceCnt:=Nothing,
                                             vPFPremiumFinanceVersion:=Nothing,
                                             vPercentage:=Nothing,
                                             vAmount:=Nothing,
                                             vSuspendedTransdetailID:=Nothing,
                                             vAccountingDate:=vAccountingDate,
                                             vInsuranceFileCnt:=Nothing,
                                             vLastInstalment:=False,
                                             v_iInstalmentID:=0)
    End Function

    Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer,
                                                 ByVal vPercentage As Object,
                                                 ByVal vSuspendedTransdetailID As Object,
                                                 ByVal vInsuranceFileCnt As Object) As Integer
        Return ReleaseSuspendedTransactions(lAllocationId:=lAllocationId,
                                             vLinkedTransdetailID:=Nothing,
                                             vDocumentID:=Nothing,
                                             vPFPremiumFinanceCnt:=Nothing,
                                             vPFPremiumFinanceVersion:=Nothing,
                                             vPercentage:=vPercentage,
                                             vAmount:=Nothing,
                                             vSuspendedTransdetailID:=vSuspendedTransdetailID,
                                             vAccountingDate:=Nothing,
                                             vInsuranceFileCnt:=vInsuranceFileCnt,
                                             vLastInstalment:=False,
                                             v_iInstalmentID:=0)
    End Function

    Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer,
                                     ByVal vPFPremiumFinanceCnt As Object,
                                     ByVal vPFPremiumFinanceVersion As Object,
                                     ByVal vPercentage As Object,
                                     ByVal vAmount As Object,
                                     ByVal vSuspendedTransdetailID As Object,
                                     ByVal vLastInstalment As Boolean,
                                     ByVal v_iInstalmentID As Integer) As Integer
        Return ReleaseSuspendedTransactions(lAllocationId:=lAllocationId,
                                             vLinkedTransdetailID:=Nothing,
                                             vDocumentID:=Nothing,
                                             vPFPremiumFinanceCnt:=vPFPremiumFinanceCnt,
                                             vPFPremiumFinanceVersion:=vPFPremiumFinanceVersion,
                                             vPercentage:=vPercentage,
                                             vAmount:=vAmount,
                                             vSuspendedTransdetailID:=vSuspendedTransdetailID,
                                             vAccountingDate:=Nothing,
                                             vInsuranceFileCnt:=Nothing,
                                             vLastInstalment:=vLastInstalment,
                                             v_iInstalmentID:=v_iInstalmentID)
    End Function

    ''' <summary>
    ''' Move Transaction To Suspense
    ''' </summary>
    ''' <param name="lAllocationId"></param>
    ''' <param name="vLinkedTransdetailID"></param>
    ''' <param name="vDocumentID"></param>
    ''' <param name="vPFPremiumFinanceCnt"></param>
    ''' <param name="vPFPremiumFinanceVersion"></param>
    ''' <param name="vPercentage"></param>
    ''' <param name="vAmount"></param>
    ''' <param name="vSuspendedTransdetailID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vInsuranceFileCnt"></param>
    ''' <param name="vLastInstalment"></param>
    ''' <param name="v_iInstalmentID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ReleaseSuspendedTransactions(ByVal lAllocationId As Integer,
                                                 ByVal vLinkedTransdetailID As Object,
                                                 ByVal vDocumentID As Object,
                                                 ByVal vPFPremiumFinanceCnt As Object,
                                                 ByVal vPFPremiumFinanceVersion As Object,
                                                 ByVal vPercentage As Object,
                                                 ByVal vAmount As Object,
                                                 ByVal vSuspendedTransdetailID As Object,
                                                 ByVal vAccountingDate As Object,
                                                 ByVal vInsuranceFileCnt As Object,
                                                 ByVal vLastInstalment As Boolean,
                                                 ByVal v_iInstalmentID As Integer) As Integer



        Const kSuspenseSuspendedTransactionId As Integer = 0
        Const kSuspenseLinkedPrecentage As Integer = 2
        Const kSuspenseInsuranceFileCnt As Integer = 5
        Const kSuspenseDestinationAccountId As Integer = 6
        'Const kSuspenseDocumentTypeId As Integer = 7
        Const kSuspenseTransactionTypeId As Integer = 8
        Const kSuspenseOutstandingAmount As Integer = 10
        Const kSuspenseOriginalDocumentRef As Integer = 11
        Const kSuspenseTransdetailTypeCode As Integer = 12
        Const kReleasedDocumentTypeId As Integer = 1

        Dim nResult As Integer
        Dim dPaymentPercentage As Double
        Dim crAmount As Decimal
        Dim crPaymentCurrency As Decimal
        Dim crPaymentBase As Decimal
        Dim nInsuranceFileCnt As Integer
        Dim dLinkedPercentage As Double
        Dim nTransdetailTypeID As Integer
        Dim nDestinationAccountID As Integer
        Dim nNumberRangeId As Integer = 0
        Dim sDocumentRef As String = ""
        Dim nDocumentID As Object = 0
        Dim nSuspendedTransdetailId As Integer = 0
        Dim nTransDetailId_1 As Object = 0
        Dim nTransDetailId_2 As Object = 0
        Dim nCompanyID As Object = 0
        Dim nCurrencyID As Integer = 0
        Dim nAccountCurrencyID As Integer = 0
        Dim nSystemCurrencyID As Integer = 0
        Dim nAccountID As Integer = 0
        Dim crBaseAmount As Decimal = 0
        Dim crCurrencyAmount As Decimal = 0
        Dim crAccountAmount As Decimal = 0
        Dim crSystemAmount As Decimal = 0
        Dim dCurrencyBaseXrate As Double = 0
        Dim oAccountBaseXrate As Object = Nothing
        Dim oSystemBaseXrate As Object = Nothing
        Dim sSpare As String = ""
        Dim sReleasedDocumentRef As String = ""
        Dim sTransdetailTypeCode As String = ""
        Dim sPrefixspare As String = ""
        Dim dtAccountingDate As Date
        Dim aKeyArray As Array
        Dim oReleaseArray(,) As Object = Nothing
        Dim oAllocationTrans As Object = Nothing
        Dim oResultArray(,) As Object = Nothing
        Dim oAllocation(,) As Object = Nothing
        Dim oDocDate As Object = Nothing
        Dim sInsuranceRef As String = ""
        Dim sSpareNew As String = ""
        Dim nAllocationBatchId As Integer = 0
        Dim nAllocationBatch_Id As Integer = 0

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue
            'Set accounting date to 'today' if none passed thru

            If Informations.IsNothing(vAccountingDate) Then
                dtAccountingDate = DateTime.Today
            Else
                dtAccountingDate = CDate(vAccountingDate)
            End If

            'Default empty Parameters

            If Not Informations.IsNothing(vPercentage) Then

                If Convert.IsDBNull(vPercentage) Or Informations.IsNothing(vPercentage) Then
                    dPaymentPercentage = 1
                Else
                    dPaymentPercentage = CDbl(vPercentage)
                End If
            Else
                dPaymentPercentage = 1
            End If

            If Not Informations.IsNothing(vAmount) Then

                If Convert.IsDBNull(vAmount) Or Informations.IsNothing(vAmount) Then
                    crAmount = 0
                Else
                    crAmount = CDec(vAmount)
                End If
            Else
                crAmount = 0
            End If

            'Get suspended Transactions for Linked Transaction
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="AllocationID", vValue:=lAllocationId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If Not Informations.IsNothing(vLinkedTransdetailID) Then
                    m_lReturn = .Parameters.Add(sName:="LinkedTransdetailId", vValue:=vLinkedTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="LinkedTransdetailId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vDocumentID) Then
                    m_lReturn = .Parameters.Add(sName:="DocumentID", vValue:=vDocumentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="DocumentID", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vPFPremiumFinanceCnt) Then
                    m_lReturn = .Parameters.Add(sName:="PFpremFinanceCnt", vValue:=vPFPremiumFinanceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="PFpremFinanceCnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vPFPremiumFinanceVersion) Then
                    m_lReturn = .Parameters.Add(sName:="PFpremFinanceVersion", vValue:=vPFPremiumFinanceVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="PFpremFinanceVersion", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                End If

                If Not Informations.IsNothing(vSuspendedTransdetailID) Then
                    m_lReturn = .Parameters.Add(sName:="SuspendedTransdetailID", vValue:=vSuspendedTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else
                    m_lReturn = .Parameters.Add(sName:="SuspendedTransdetailID", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                m_lReturn = .SQLSelect(sSQL:=ACSelectSuspendedTransactionSQL, sSQLName:=ACSelectSuspendedTransactionName, bStoredProcedure:=ACSelectSuspendedTransactionStored, vResultArray:=oResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Select supended transactions failed")
                End If

            End With

            If Not Informations.IsArray(oResultArray) Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            m_lReturn = GetAllocationbatch(v_nAllocationId:=nAllocationBatchId,
                                             r_nAllocationBatchId:=nAllocationBatch_Id)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Failed -GetAllocationbatch")
            End If


            'If we have records create necessary objects
            If m_oDocumentPost Is Nothing Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oDocumentPost, v_sClassName:="bACTDocumentPost.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Failed to create bACTDocumentPost")
            End If

            If m_oAllocationManual Is Nothing Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationManual, v_sClassName:="bACTAllocationManual.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Failed to Create bACTAllocation Manual")
            End If

            'Loop through suspended transactions

            For iCount As Integer = 0 To oResultArray.GetUpperBound(1)

                If gPMFunctions.NullToDecimal(CStr(oResultArray(kSuspenseOutstandingAmount, iCount))) <> 0 Then

                    Dim lngRetValue, lngTmpSuspTransId As Long
                    Dim oArrReslt(,) As Object = Nothing

                    lngTmpSuspTransId = NullToLong(oResultArray(kSuspenseSuspendedTransactionId, iCount))
                    lngRetValue = 0

                    With m_oDatabase
                        .Parameters.Clear()

                        m_lReturn = .Parameters.Add(
                            sName:="SuspendedTransdetailId",
                            vValue:=lngTmpSuspTransId,
                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                            iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = .Parameters.Add(
                          sName:="ReturnValue",
                          vValue:=0,
                          iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                          iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = .SQLSelect(
                                    sSQL:=ACIsSuspendedTransPostedSQL,
                                    sSQLName:=ACIsSuspendedTransPostedName,
                                    bStoredProcedure:=ACIsSuspendedTransPostedStored,
                                    vResultArray:=oArrReslt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error Message
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                               sMsg:="Failed to call spu_ACT_IsSuspendedTransactionPosted",
                                               vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseSuspendedTransactions",
                                               vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        End If

                        lngRetValue = .Parameters.Item("ReturnValue").Value
                    End With

                    If lngRetValue <> 1 Then

                        'If there is money outstanding on suspended transaction release it
                        'Get Details from Suspended Record

                        nSuspendedTransdetailId = gPMFunctions.NullToLong(CStr(oResultArray(kSuspenseSuspendedTransactionId, iCount)))

                        ' nDocumentTypeId = gPMFunctions.NullToLong(CStr(oResultArray(kSuspenseDocumentTypeId, iCount)))

                        nInsuranceFileCnt = gPMFunctions.NullToLong(CStr(oResultArray(kSuspenseInsuranceFileCnt, iCount)))

                        dLinkedPercentage = gPMFunctions.NullToDouble(CStr(oResultArray(kSuspenseLinkedPrecentage, iCount)))

                        nTransdetailTypeID = gPMFunctions.NullToLong(CStr(oResultArray(kSuspenseTransactionTypeId, iCount)))

                        nDestinationAccountID = gPMFunctions.NullToLong(CStr(oResultArray(kSuspenseDestinationAccountId, iCount)))

                        sReleasedDocumentRef = gPMFunctions.NullToString(CStr(oResultArray(kSuspenseOriginalDocumentRef, iCount)))

                        sTransdetailTypeCode = gPMFunctions.NullToString(CStr(oResultArray(kSuspenseTransdetailTypeCode, iCount)))

                        '  Get Transdetail Information for Suspended Transaction

                        m_lReturn = GetDetails(vTransdetailID:=nSuspendedTransdetailId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - GetDetails Failed")
                        End If

                        m_lReturn = GetNext(vAccountID:=nAccountID, vCompanyID:=nCompanyID, vCurrencyID:=nCurrencyID, vCurrencyAmount:=crCurrencyAmount, vAmount:=crBaseAmount, vAccountAmount:=crAccountAmount, vSystemAmount:=crSystemAmount, vCurrencyBaseXrate:=dCurrencyBaseXrate, vAccountBaseXrate:=oAccountBaseXrate, vSystemBaseXrate:=oSystemBaseXrate, vAccountCurrencyID:=nAccountCurrencyID, vSystemCurrencyID:=nSystemCurrencyID, vDocumentID:=nDocumentID, vSpare:=sSpare, vInsuranceRef:=sInsuranceRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - GetNext Failed")
                        End If

                        If m_oAutoNumber Is Nothing Then


                            m_oAutoNumber = New bACTAutoNumber.Business
                            m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Create bACTAutoNumber Failed")
                                Return nResult
                            End If
                        End If

                        ' Get the number range

                        m_lReturn = m_oAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=nNumberRangeId)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTAutoNumber.GetNumberRange Failed")
                            Return nResult
                        End If

                        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                        m_lReturn = m_oAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=nNumberRangeId, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTAutoNumber.GenerateDocumentReferenceNumber Failed")
                            Return nResult
                        End If

                        ' Format the number
                        sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef


                        If sTransdetailTypeCode = "COMSUSP" Then
                            m_lReturn = GetInsuranceFileCnt(vPFPremiumFinanceCnt, vPFPremiumFinanceVersion, nInsuranceFileCnt)
                        End If
                        ' Create Document
                        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=ToSafeInteger(kReleasedDocumentTypeId),
                                                                v_sDocumentRef:=ToSafeString(sDocumentRef),
                                                                v_dtDocumentDate:=ToSafeDate(dtAccountingDate),
                                                                v_sComment:="",
                                                                v_lInsuranceFileCnt:=ToSafeInteger(nInsuranceFileCnt),
                                                                r_vDocumentID:=nDocumentID,
                                                                r_vDocSourceID:=nCompanyID,
                                                                v_sReason:="",
                                                                r_vSubBranchID:=nCompanyID)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTDocumentPost.AddDocument Failed")
                        End If

                        'FSA Phase 3 Partial Commission Movements
                        'If this isn't an instalment and we have a linked transaction
                        'get the allocation percentage/value
                        If Informations.IsNothing(vPFPremiumFinanceCnt) And Not Informations.IsNothing(vLinkedTransdetailID) Then

                            With m_oDatabase

                                .Parameters.Clear()

                                m_lReturn = .Parameters.Add(sName:="AllocationID", vValue:=lAllocationId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                m_lReturn = .Parameters.Add(sName:="LinkedTransdetailId", vValue:=vLinkedTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                m_lReturn = .Parameters.Add(sName:="SuspendedTransdetailId", vValue:=nSuspendedTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                                m_lReturn = .SQLSelect(sSQL:=ACGetAllocationPartSQL, sSQLName:=ACGetAllocationPartName, bStoredProcedure:=ACGetAllocationPartStored, vResultArray:=oAllocation)

                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Get Allocation Part failed")
                                End If

                                If Not Informations.IsArray(oAllocation) Then
                                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - Get Allocation Part failed")
                                End If

                                crAmount = CDec(oAllocation(0, 0))

                                dPaymentPercentage = CDbl(oAllocation(1, 0))

                            End With
                        End If
                        'FSA Phase 3 Partial Commission Movements End

                        If crAmount <> 0 Then
                            'We have been passed a final settlement
                            crPaymentCurrency = gPMFunctions.ToSafeRound(crAmount, kRoundingUpto, m_bSuppressDecimalValues)
                            crPaymentBase = gPMFunctions.ToSafeRound(crPaymentCurrency * dCurrencyBaseXrate, kRoundingUpto, m_bSuppressDecimalValues)
                        Else
                            crPaymentCurrency = gPMFunctions.ToSafeRound(crCurrencyAmount * dLinkedPercentage * dPaymentPercentage, kRoundingUpto, m_bSuppressDecimalValues)
                            crPaymentBase = gPMFunctions.ToSafeRound(crBaseAmount * dLinkedPercentage * dPaymentPercentage, kRoundingUpto, m_bSuppressDecimalValues)
                        End If
                        If vLastInstalment AndAlso crAmount <> 0 Then
                            If Math.Abs(CInt(Math.Round(Math.Round(CDec(oResultArray(kSuspenseOutstandingAmount, iCount)), 2) Mod Math.Round(CDec(crPaymentCurrency), 2), 2))) = 0 Then
                                crPaymentCurrency = crPaymentCurrency + Math.Round(Math.Round(CDec(oResultArray(kSuspenseOutstandingAmount, iCount)), 2) Mod Math.Round(CDec(crPaymentCurrency), 2), 2)
                                crPaymentBase = crPaymentBase + Math.Round(Math.Round(CDec(oResultArray(kSuspenseOutstandingAmount, iCount)), 2) Mod Math.Round(CDec(crPaymentBase), 2), 2)
                            Else
                                crPaymentCurrency = Math.Round(Math.Round(CDec(oResultArray(kSuspenseOutstandingAmount, iCount)), 2) Mod Math.Round(CDec(crPaymentCurrency), 2), 2)
                                crPaymentBase = Math.Round(Math.Round(CDec(oResultArray(kSuspenseOutstandingAmount, iCount)), 2) Mod Math.Round(CDec(crPaymentBase), 2), 2)
                            End If
                        End If

                        If sTransdetailTypeCode = "COMMPAY" Then
                            sPrefixspare = "COMM PAY "
                        End If

                        If sTransdetailTypeCode = "TAXPAY" Then
                            sPrefixspare = "TAX PAY "
                        End If

                        If sTransdetailTypeCode = "COMSUSP" Then
                            sSpareNew = "COMM"
                        Else
                            sSpareNew = sPrefixspare & sReleasedDocumentRef
                        End If

                        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=ToSafeInteger(nAccountID), v_iCurrencyID:=ToSafeInteger(nCurrencyID), v_cAmount:=ToSafeDecimal(-crPaymentBase),
                                                                   v_cCurrencyAmount:=ToSafeDecimal(-crPaymentCurrency), v_vdCurrencyBaseXRate:=ToSafeDouble(dCurrencyBaseXrate), r_vTransDetailId:=nTransDetailId_1,
                                                                   v_vDocumentSequence:=1, v_vInsuranceRef:=ToSafeString(sInsuranceRef), v_vSpare:=ToSafeString(sSpareNew), v_vBaseAmountUnrounded:=ToSafeDecimal(-crPaymentBase),
                                                                   v_vCurrencyAmountUnrounded:=ToSafeDecimal(-crPaymentCurrency), v_vAccountingDate:=ToSafeDate(dtAccountingDate), v_vDocSourceID:=ToSafeInteger(nCompanyID),
                                                                   v_vSubBranchId:=ToSafeInteger(nCompanyID), v_vAccountBaseXrate:=oAccountBaseXrate, v_vSystemBaseXrate:=oSystemBaseXrate,
                                                                   v_vTransdetailTypeID:=ToSafeInteger(nTransdetailTypeID))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTDocumentPost.AddTransaction Failed")
                        End If

                        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=ToSafeInteger(nDestinationAccountID), v_iCurrencyID:=ToSafeInteger(nCurrencyID), v_cAmount:=ToSafeDecimal(crPaymentBase),
                                                                   v_cCurrencyAmount:=ToSafeDecimal(crPaymentCurrency), v_vdCurrencyBaseXRate:=ToSafeDouble(dCurrencyBaseXrate), r_vTransDetailId:=nTransDetailId_2,
                                                                   v_vDocumentSequence:=2, v_vInsuranceRef:=ToSafeString(sInsuranceRef), v_vSpare:=ToSafeString(sSpareNew), v_vBaseAmountUnrounded:=ToSafeDecimal(crPaymentBase),
                                                                   v_vCurrencyAmountUnrounded:=ToSafeDecimal(crPaymentCurrency), v_vAccountingDate:=ToSafeDate(dtAccountingDate), v_vDocSourceID:=ToSafeInteger(nCompanyID),
                                                                   v_vSubBranchId:=ToSafeInteger(nCompanyID), v_vAccountBaseXrate:=oAccountBaseXrate, v_vSystemBaseXrate:=oSystemBaseXrate,
                                                                   v_vTransdetailTypeID:=ToSafeInteger(nTransdetailTypeID))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTDocumentPost.AddTransaction Failed")
                        End If

                        m_lReturn = m_oDocumentPost.Commit()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTDocumentPost.Commit Failed")
                        End If

                        ReDim oAllocationTrans(0)

                        oAllocationTrans(0) = CStr(nTransDetailId_1) & "|" & CStr(-crPaymentBase)
                        aKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 5}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameAllocatingSuspense
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = nAccountID
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(nSuspendedTransdetailId) & "|" & CStr(crPaymentBase)

                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = oAllocationTrans

                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = True

                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameAllocationBatchId
                        aKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = nAllocationBatch_Id

                        'Perform the allocation

                        m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=CType(aKeyArray, Array))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTAllocationManual.SetKeys Failed")
                        End If

                        m_lReturn = m_oAllocationManual.Start()

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - bACTAllocationManual.Start Failed")
                        End If

                        If Not Informations.IsArray(oReleaseArray) Then
                            ReDim oReleaseArray(2, 0)
                        Else

                            ReDim Preserve oReleaseArray(2, oReleaseArray.GetUpperBound(1) + 1)
                        End If

                        oReleaseArray(0, oReleaseArray.GetUpperBound(1)) = nSuspendedTransdetailId

                        oReleaseArray(1, oReleaseArray.GetUpperBound(1)) = nTransDetailId_2

                        oReleaseArray(2, oReleaseArray.GetUpperBound(1)) = lAllocationId
                    End If
                End If
            Next iCount

            'Write Record to Release
            If Informations.IsArray(oReleaseArray) Then

                For iCount As Integer = 0 To oReleaseArray.GetUpperBound(1)

                    m_lReturn = CreateReleasedTransaction(lSuspendedTransdetailId:=CInt(oReleaseArray(0, iCount)),
                                                          lDestinationTransdetailId:=CInt(oReleaseArray(1, iCount)),
                                                          vAllocationId:=CInt(oReleaseArray(2, iCount)),
                                                          v_iInstalmentID:=v_iInstalmentID)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - ReleaseSuspendedTransactions - CreateReleasedTransactions Failed")
                    End If
                Next iCount
            End If

            Return nResult
        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function RecallReleasedTransaction() As Integer
        Return RecallReleasedTransaction(lTriggerTransdetailId:=0, lAllocationId:=0)
    End Function

    Public Function RecallReleasedTransaction(ByVal lTriggerTransdetailId As Integer) As Integer
        Return RecallReleasedTransaction(lTriggerTransdetailId:=lTriggerTransdetailId, lAllocationId:=0)
    End Function

    Public Function RecallReleasedTransaction(ByVal lTriggerTransdetailId As Integer, ByVal lAllocationId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function Recall Transactions To Suspense
        ' PURPOSE: Write Document Reversals to move transaction back into suspense
        '          to the suspense file
        '          USED BY REVERSE ALLOCATIONS ONLY
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lSuspendedTransdetailId, lReleasedTransdetailId As Integer
        Dim vAssociatedDocuments As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get suspended Transactions for Linked Transaction
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="TriggerTransdetailId", vValue:=lTriggerTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="AllocationId", vValue:=lAllocationId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACSelectReleasedTransactionSQL, sSQLName:=ACSelectReleasedTransactionName, bStoredProcedure:=ACSelectReleasedTransactionStored, vResultArray:=vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - RecallReleasedTransactions - SelectReleasedTransactions Failed")
                End If

            End With

            ' Do we have any records ?

            If Not Informations.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                Return result
            End If

            'If we have records create necessary objects

            If m_oDocumentReversal Is Nothing Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oDocumentReversal, v_sClassName:="bACTDocumentReversal.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - RecallReleasedTransactions - Create bACTDocumentReversal Failed")
            End If

            For lCount As Integer = 0 To vResultArray.GetUpperBound(1)

                lSuspendedTransdetailId = CInt(vResultArray(0, lCount))

                lReleasedTransdetailId = CInt(vResultArray(1, lCount))

                'Undo any allocation
                'ACHTUNG
                'sSQL = "SELECT allocation_id from allocation where transdetail_id = " & lReleasedTransdetailId

                m_oDocumentReversal.DocumentId = 0

                m_oDocumentReversal.TransDetailId = lReleasedTransdetailId

                m_lReturn = m_oDocumentReversal.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - RecallReleasedTransactions - bACTDocumentReversal.Start Failed")
                End If

                ReDim vAssociatedDocuments(5, 0)

                vAssociatedDocuments(1, 0) = lSuspendedTransdetailId

                vAssociatedDocuments(5, 0) = lAllocationId

                m_lReturn = m_oDocumentReversal.RecallReleasedAccountsTransaction(vAssociatedDocuments:=vAssociatedDocuments)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - RecallReleasedTransactions - bACTDocumentReversal.Start Failed")
                End If

            Next lCount

            'Also  add this to the terminate
            If Not (m_oDocumentReversal Is Nothing) Then

                m_oDocumentReversal.Dispose()
                m_oDocumentReversal = Nothing
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RecallReleasedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function RewriteSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer) As Integer
        Return RewriteSuspendedTransactions(lOldTriggerTransdetailID:=lOldTriggerTransdetailID, vNewTriggerTransdetailId:=Nothing, vPFPremiumFinanceCnt:=Nothing, vPFPremiumFinanceVersion:=Nothing)
    End Function

    Public Function RewriteSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer, ByVal vNewTriggerTransdetailId As Object, ByVal vPFPremiumFinanceCnt As Object, ByVal vPFPremiumFinanceVersion As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function To Replace Trigger transaction in call od DD reversals
        ' PURPOSE: Deletes existing suspended Transactions
        '          and writes a new one
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get suspended Transactions for Linked Transaction
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="OldTriggerTransdetailId", vValue:=lOldTriggerTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If Not Informations.IsNothing(vNewTriggerTransdetailId) Then

                    m_lReturn = .Parameters.Add(sName:="NewTriggerTransdetailId", vValue:=vNewTriggerTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="NewTriggerTransdetailId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vPFPremiumFinanceCnt) Then

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceCnt", vValue:=vPFPremiumFinanceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceCnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vPFPremiumFinanceVersion) Then

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceVersion", vValue:=vPFPremiumFinanceVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceVersion", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                m_lReturn = .SQLAction(sSQL:=ACRewriteSuspendedTransactionSQL, sSQLName:=ACRewriteSuspendedTransactionName, bStoredProcedure:=ACRewriteSuspendedTransactionStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - RewriteSuspendedTransactions - RewriteSuspendedTransactions Failed")
                End If

            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="RewriteSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function
    'New for FSA Phase IV
    Public Function FinanceSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer, ByVal vPlanTransdetailId As Integer) As Integer
        Return FinanceSuspendedTransactions(lOldTriggerTransdetailID:=lOldTriggerTransdetailID, vPlanTransdetailId:=vPlanTransdetailId, vPFPremiumFinanceCnt:=Nothing, vPFPremiumFinanceVersion:=Nothing, vDepositTransdetailId:=Nothing, vDepositPercentage:=Nothing)
    End Function

    Public Function FinanceSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer, ByVal vPlanTransdetailId As Integer, ByVal vDepositTransdetailId As Object, ByVal vDepositPercentage As Object) As Integer
        Return FinanceSuspendedTransactions(lOldTriggerTransdetailID:=lOldTriggerTransdetailID, vPlanTransdetailId:=vPlanTransdetailId, vPFPremiumFinanceCnt:=Nothing, vPFPremiumFinanceVersion:=Nothing, vDepositTransdetailId:=vDepositTransdetailId, vDepositPercentage:=vDepositPercentage)
    End Function

    Public Function FinanceSuspendedTransactions(ByVal lOldTriggerTransdetailID As Integer, ByVal vPlanTransdetailId As Integer, ByVal vPFPremiumFinanceCnt As Object, ByVal vPFPremiumFinanceVersion As Object, ByVal vDepositTransdetailId As Object, ByVal vDepositPercentage As Object) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function To Replace Trigger transactions for Financed Transactions
        ' PURPOSE: Sets existing suspended Transactions as deleted
        '          and writes a new ones
        ' AUTHOR: Elaine Knott
        ' DATE: FEB 2005
        ' REMARKS: FSA Phase 4
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get suspended Transactions for Linked Transaction
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="OldTriggerTransdetailId", vValue:=lOldTriggerTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="PlanTransdetailId", vValue:=vPlanTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If Not Informations.IsNothing(vPFPremiumFinanceCnt) Then

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceCnt", vValue:=vPFPremiumFinanceCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceCnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vPFPremiumFinanceVersion) Then

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceVersion", vValue:=vPFPremiumFinanceVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="PFPremFinanceVersion", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vDepositTransdetailId) Then

                    m_lReturn = .Parameters.Add(sName:="DepositTransdetailId", vValue:=vDepositTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="DepositTransdetailId", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                If Not Informations.IsNothing(vDepositPercentage) Then

                    m_lReturn = .Parameters.Add(sName:="DepositPercentage", vValue:=vDepositPercentage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                Else

                    m_lReturn = .Parameters.Add(sName:="DepositPercentage", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
                End If

                m_lReturn = .SQLAction(sSQL:=ACFinanceSuspendedTransactionSQL, sSQLName:=ACFinanceSuspendedTransactionName, bStoredProcedure:=ACFinanceSuspendedTransactionStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - FinanceSuspendedTransactions - FinanceSuspendedTransactions Failed")
                End If

            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="FinanceSuspendedTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function CheckSuspendedAllocation(ByVal lLinkedTransdetailID As Integer, ByRef lAllocationId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: Function Move Transaction To Suspense
        ' PURPOSE: Write Document to move transaction into suspense
        '          to the suspense file
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get suspended Transactions for Linked Transaction
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="TransdetailId", vValue:=lLinkedTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=ACSelectSuspendedAllocationSQL, sSQLName:=ACSelectSuspendedAllocationName, bStoredProcedure:=ACSelectSuspendedAllocationStored, vResultArray:=vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - CheckSuspendedAllocation - SelectSuspendedAllocation Failed")
                End If

                lAllocationId = CInt(vResults(0, 0))
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckSuspendedAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function CreateSuspendedTransaction(ByVal lSuspendedTransdetailId As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double, ByVal vPFPremFinanceCnt As Object, ByVal vPFPremFinanceVersion As Object, ByVal lInsuranceFileCnt As Integer, ByVal lDestinationAccountID As Integer, ByVal lDocumentTypeId As Integer, ByVal lTransdetailTypeID As Integer, ByVal sSpare As String) As Integer '(RC) PLICO 9-10
        Return CreateSuspendedTransaction(lSuspendedTransdetailId:=lSuspendedTransdetailId, lLinkedTransdetailID:=lLinkedTransdetailID, dLinkedPercentage:=dLinkedPercentage, vPFPremFinanceCnt:=vPFPremFinanceCnt, vPFPremFinanceVersion:=vPFPremFinanceVersion, lInsuranceFileCnt:=lInsuranceFileCnt, lDestinationAccountID:=lDestinationAccountID, lDocumentTypeId:=lDocumentTypeId, lTransdetailTypeID:=lTransdetailTypeID, sSpare:=sSpare, bManuallyReleased:=False, bReleasedOnFullSettlement:=False, bReleasedForWholePosting:=False, bReleasedOnPolicyEffective:=False)
    End Function

    Public Function CreateSuspendedTransaction(ByVal lSuspendedTransdetailId As Integer, ByVal lLinkedTransdetailID As Integer, ByVal dLinkedPercentage As Double, ByVal vPFPremFinanceCnt As Object, ByVal vPFPremFinanceVersion As Object, ByVal lInsuranceFileCnt As Integer, ByVal lDestinationAccountID As Integer, ByVal lDocumentTypeId As Integer, ByVal lTransdetailTypeID As Integer, ByVal sSpare As String, ByVal bManuallyReleased As Boolean, ByVal bReleasedOnFullSettlement As Boolean, ByVal bReleasedForWholePosting As Boolean, ByVal bReleasedOnPolicyEffective As Boolean) As Integer '(RC) PLICO 9-10

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CreateSuspendedTransaction
        ' PURPOSE: Write transactions To Suspense File
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lDestinationAccountID = 0 Then
                Return result
            End If

            If Not False And gPMFunctions.ToSafeString(sSpare) = "" Then
                sSpare = "COMM"
            End If

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="SuspendedTransdetailId", vValue:=lSuspendedTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="LinkedTransdetailID", vValue:=lLinkedTransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="LinkedPercentage", vValue:=dLinkedPercentage, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                m_lReturn = .Parameters.Add(sName:="PremiumFinanceCnt", vValue:=gPMFunctions.ToSafeLong(vPFPremFinanceCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="PremiumFinanceVersion", vValue:=gPMFunctions.ToSafeLong(vPFPremFinanceVersion), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If lInsuranceFileCnt <> 0 Then

                    m_lReturn = .Parameters.Add(sName:="InsuranceFileCnt", vValue:=lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="InsuranceFileCnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                m_lReturn = .Parameters.Add(sName:="DestinationAccountID", vValue:=lDestinationAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="DocumentTypeId", vValue:=lDocumentTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="TransdetailTypeId", vValue:=lTransdetailTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="Spare", vValue:=gPMFunctions.ToSafeString(sSpare), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                m_lReturn = .Parameters.Add(sName:="IsDeleted", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                '(RC) PLICO 9-10

                m_lReturn = .Parameters.Add(sName:="manually_released", vValue:=bManuallyReleased, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                m_lReturn = .Parameters.Add(sName:="released_on_full_settlement", vValue:=bReleasedOnFullSettlement, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                m_lReturn = .Parameters.Add(sName:="released_for_whole_posting", vValue:=bReleasedForWholePosting, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                m_lReturn = .Parameters.Add(sName:="released_on_policy_effective", vValue:=bReleasedOnPolicyEffective, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

                '        m_lReturn& = .Parameters.Add( _
                ''            sName:="manually_released", _
                ''            vValue:=bRatesForInformationOnly, _
                ''            idirection:=PMParamInput, _
                ''            iDataType:=PMBoolean)

                m_lReturn = .SQLAction(sSQL:=ACAddSuspendedTransactionSQL, sSQLName:=ACAddSuspendedTransactionName, bStoredProcedure:=ACAddSuspendedTransactionStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - CreateSuspendedTransactions - ACAddSuspendedTransactionSQL Failed")
                End If
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSuspendedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function

    Public Function CreateReleasedTransaction(ByVal lSuspendedTransdetailId As Integer,
                                          ByVal lDestinationTransdetailId As Integer) As Integer
        Return CreateReleasedTransaction(lSuspendedTransdetailId:=lSuspendedTransdetailId,
                                         lDestinationTransdetailId:=lDestinationTransdetailId,
                                         vAllocationId:=Nothing,
                                         v_iInstalmentID:=0)
    End Function

    Public Function CreateReleasedTransaction(ByVal lSuspendedTransdetailId As Integer,
                                              ByVal lDestinationTransdetailId As Integer,
                                              ByVal vAllocationId As Object,
                                              ByVal v_iInstalmentID As Integer) As Integer

        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: CreateReleasedTransaction
        ' PURPOSE: Write transactions To Released File
        ' AUTHOR: Elaine Knott
        ' DATE: DEC 2004
        ' REMARKS: FSA Phase 3.2
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="SuspendedTransdetailId", vValue:=lSuspendedTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .Parameters.Add(sName:="DestinationTransdetailId", vValue:=lDestinationTransdetailId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If Not Informations.IsNothing(vAllocationId) Then

                    m_lReturn = .Parameters.Add(sName:="AllocationID", vValue:=vAllocationId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                Else

                    m_lReturn = .Parameters.Add(sName:="AllocationID", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                End If

                m_lReturn = .Parameters.Add(sName:="ReleaseDate", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
                m_lReturn = .Parameters.Add(sName:="InstalmentID", vValue:=v_iInstalmentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLAction(sSQL:=ACAddReleasedTransactionSQL, sSQLName:=ACAddReleasedTransactionName, bStoredProcedure:=ACAddReleasedTransactionStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", bACTTransDetail.Form - CreateReleasedTransactions - ACAddReleasedTransactionSQL Failed")
                End If
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateReleasedTransaction", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally


        End Try
        Return result
    End Function
    'UPGRADE_NOTE: (7001) The following declaration (GetBaseAmountFromCurrency) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal) As Integer
    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CreateReleasedTransaction
    ' PURPOSE: Write transactions To Released File
    ' AUTHOR: Elaine Knott
    ' DATE: DEC 2004
    ' REMARKS: FSA Phase 3.2
    ' ---------------------------------------------------------------------------
    '
    'Dim result As Integer = 0
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If m_oCurrencyConvert Is Nothing Then
    'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oCurrencyConvert, v_sClassName:="bACTCurrencyConvert.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", " +   + ", bACTTransDetail.Form - GetBaseAmountFromCurrency - Failed to Create bACTCurrencyConvert.Form")

    '
    '

    'm_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_iCompanyID, cBaseAmount:=r_cBaseAmount, cCurrencyAmount:=v_cCurrencyAmount)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Throw New System.Exception(Constants.vbObjectError.ToString() + ", " +   + ", bACTTransDetail.Form - GetBaseAmountFromCurrency - bACTCurrencyConvert.ConvertCurrencyToBase Failed")

    '
    'GoTo Finally_Renamed
    '
    '----------------------------------------------------------------------------------------
    'Only for Debugging, the code will never execute this line
    '----------------------------------------------------------------------------------------
    'Resume 
    '
    '
    'Select Case Informations.Err().Number
    'Case Else

    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBaseAmountFromCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'GoTo Finally_Renamed

    '
    'Finally_Renamed: '
    'Return result
    '

    'FSA Phase 3.2 End

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oTransDetail As Transdetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection

        m_oDatabase.Parameters.Clear()

        ' Add TransdetailID as an OUTPUT param for an insert

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transdetail_id", vValue:=oTransDetail.TransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the required INPUT parameters
        m_lReturn = AddInputParam(oTransDetail:=oTransDetail)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted

        oTransDetail.TransdetailID = m_oDatabase.Parameters.Item("Transdetail_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeriveIndex
    '
    ' Description: Given a policy number, this function will strip the
    ' numeric bit off the end, so it can be writtento an indexed numeric
    ' column and used as a more efficient search parameter in FindTransaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (DeriveIndex) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function DeriveIndex(ByRef sInsuranceRef As String, ByRef dInsuranceRefIndex As Double) As Integer
    '
    '
    'Dim result As Integer = 0
    'Dim iRightLen As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sInsuranceRef = sInsuranceRef.Trim()
    '
    'If sInsuranceRef = "" Then
    'Return result

    '
    'iRightLen = 0
    '
    'For 'i As Integer = sInsuranceRef.Length To 1 Step -1
    'Dim dbNumericTemp As Double
    'If Double.TryParse(sInsuranceRef.Substring(i - 1, 1), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
    'iRightLen += 1
    'Else
    'Exit For

    'Next i
    '
    'If iRightLen > 0 Then
    'dInsuranceRefIndex = CDbl(sInsuranceRef.Substring(sInsuranceRef.Length - iRightLen))

    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '

    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="DeriveIndexFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeriveIndex", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oTransDetail As Transdetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection

        m_oDatabase.Parameters.Clear()

        ' Add TransdetailID as an INPUT param for an update

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Transdetail_id", vValue:=oTransDetail.TransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = AddInputParam(oTransDetail:=oTransDetail)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check to see that the record was updated OK

        If lRecordsAffected > 0 Then
            ' Updated No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oTransDetail As Transdetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection

        m_oDatabase.Parameters.Clear()

        ' Add the TransdetailID INPUT parameter

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Transdetail_id", vValue:=oTransDetail.TransdetailID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied Transdetail properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oTransDetail As Transdetail, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0

        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1

        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields

        ' Populate Base Details

        With oTransDetail

            .TransdetailID = oFields("transdetail_id")

            If Convert.IsDBNull(oFields("account_id")) Or Informations.IsNothing(oFields("account_id")) Then
                .AccountID = 0
            Else
                .AccountID = oFields("account_id")
            End If

            If Convert.IsDBNull(oFields("postingstatus_id")) Or Informations.IsNothing(oFields("postingstatus_id")) Then
                .PostingstatusID = 0
            Else
                .PostingstatusID = oFields("postingstatus_id")
            End If

            If Convert.IsDBNull(oFields("company_id")) Or Informations.IsNothing(oFields("company_id")) Then
                .CompanyID = 0
            Else
                .CompanyID = oFields("company_id")
            End If

            If Convert.IsDBNull(oFields("currency_id")) Or Informations.IsNothing(oFields("currency_id")) Then
                .CurrencyID = 0
            Else
                .CurrencyID = oFields("currency_id")
            End If

            If Convert.IsDBNull(oFields("period_id")) Or Informations.IsNothing(oFields("period_id")) Then
                .PeriodID = 0
            Else
                .PeriodID = oFields("period_id")
            End If

            If Convert.IsDBNull(oFields("document_id")) Or Informations.IsNothing(oFields("document_id")) Then
                .DocumentID = 0
            Else
                .DocumentID = oFields("document_id")
            End If

            If Convert.IsDBNull(oFields("document_sequence")) Or Informations.IsNothing(oFields("document_sequence")) Then
                .DocumentSequence = 0
            Else
                .DocumentSequence = oFields("document_sequence")
            End If

            If Convert.IsDBNull(oFields("accounting_date")) Or Informations.IsNothing(oFields("accounting_date")) Then

                '.AccountingDate = #12/30/1899#
                .AccountingDate = DateTime.MinValue
            Else
                .AccountingDate = oFields("accounting_date")
            End If

            If Convert.IsDBNull(oFields("amount")) Or Informations.IsNothing(oFields("amount")) Then
                .Amount = 0
            Else
                .Amount = oFields("amount")
            End If

            If Convert.IsDBNull(oFields("base_amount_unrounded")) Or Informations.IsNothing(oFields("base_amount_unrounded")) Then
                .BaseAmountUnrounded = 0
            Else
                .BaseAmountUnrounded = oFields("base_amount_unrounded")
            End If

            .FullyMatched = oFields("fully_matched")

            If Convert.IsDBNull(oFields("currency_amount")) Or Informations.IsNothing(oFields("currency_amount")) Then
                .CurrencyAmount = 0
            Else
                .CurrencyAmount = oFields("currency_amount")
            End If

            If Convert.IsDBNull(oFields("currency_amount_unrounded")) Or Informations.IsNothing(oFields("currency_amount_unrounded")) Then
                .CurrencyAmountUnrounded = 0
            Else
                .CurrencyAmountUnrounded = oFields("currency_amount_unrounded")
            End If

            If Convert.IsDBNull(oFields("euro_currency_id")) Or Informations.IsNothing(oFields("euro_currency_id")) Then
                .EuroCurrencyId = 0
            Else
                .EuroCurrencyId = oFields("euro_currency_id")
            End If

            If Convert.IsDBNull(oFields("euro_amount")) Or Informations.IsNothing(oFields("euro_amount")) Then
                .EuroAmount = 0
            Else
                .EuroAmount = oFields("euro_amount")
            End If

            If Convert.IsDBNull(oFields("euro_base_xrate")) Or Informations.IsNothing(oFields("euro_base_xrate")) Then
                .EuroBaseXrate = 0
            Else
                .EuroBaseXrate = oFields("euro_base_xrate")
            End If

            If Convert.IsDBNull(oFields("euro_ccy_xrate")) Or Informations.IsNothing(oFields("euro_ccy_xrate")) Then
                .EuroCcyXrate = 0
            Else
                .EuroCcyXrate = oFields("euro_ccy_xrate")
            End If

            If Convert.IsDBNull(oFields("comment")) Or Informations.IsNothing(oFields("comment")) Then
                .Comment = ""
            Else
                .Comment = oFields("comment")
            End If

            If Convert.IsDBNull(oFields("insurance_ref")) Or Informations.IsNothing(oFields("insurance_ref")) Then
                .InsuranceRef = ""
            Else
                .InsuranceRef = oFields("insurance_ref")
            End If

            If Convert.IsDBNull(oFields("operator_id")) Or Informations.IsNothing(oFields("operator_id")) Then
                .OperatorID = 0
            Else
                .OperatorID = oFields("operator_id")
            End If

            If Convert.IsDBNull(oFields("purchase_order_no")) Or Informations.IsNothing(oFields("purchase_order_no")) Then
                .PurchaseOrderNo = ""
            Else
                .PurchaseOrderNo = oFields("purchase_order_no")
            End If

            If Convert.IsDBNull(oFields("purchase_invoice_no")) Or Informations.IsNothing(oFields("purchase_invoice_no")) Then
                .PurchaseInvoiceNo = ""
            Else
                .PurchaseInvoiceNo = oFields("purchase_invoice_no")
            End If

            If Convert.IsDBNull(oFields("department")) Or Informations.IsNothing(oFields("department")) Then
                .Department = ""
            Else
                .Department = oFields("department")
            End If

            If Convert.IsDBNull(oFields("spare")) Or Informations.IsNothing(oFields("spare")) Then
                .Spare = ""
            Else
                .Spare = oFields("spare")
            End If

            If Convert.IsDBNull(oFields("ref_date")) Or Informations.IsNothing(oFields("ref_date")) Then

                .RefDate = DateTime.MinValue
            Else
                .RefDate = oFields("ref_date")
            End If

            If Convert.IsDBNull(oFields("ref_amount")) Or Informations.IsNothing(oFields("ref_amount")) Then
                .RefAmount = 0
            Else
                .RefAmount = oFields("ref_amount")
            End If

            If Convert.IsDBNull(oFields("ref_quantity")) Or Informations.IsNothing(oFields("ref_quantity")) Then
                .RefQuantity = 0
            Else
                .RefQuantity = oFields("ref_quantity")
            End If

            If Convert.IsDBNull(oFields("ref_units")) Or Informations.IsNothing(oFields("ref_units")) Then
                .RefUnits = ""
            Else
                .RefUnits = oFields("ref_units")
            End If

            If Convert.IsDBNull(oFields("department_id")) Or Informations.IsNothing(oFields("department_id")) Then
                .DepartmentID = 0
            Else
                .DepartmentID = oFields("department_id")
            End If

            .UnderwritingYearID = oFields("underwriting_year_id")

            .InsuranceRefIndex = oFields("insurance_ref_index")

            If Convert.IsDBNull(oFields("currency_base_xrate")) Or Informations.IsNothing(oFields("currency_base_xrate")) Then
                .CurrencyBaseXrate = CStr(0)
            Else
                .CurrencyBaseXrate = oFields("currency_base_xrate")
            End If

            If Convert.IsDBNull(oFields("account_base_xrate")) Or Informations.IsNothing(oFields("account_base_xrate")) Then
                .AccountBaseXrate = CStr(0)
            Else
                .AccountBaseXrate = oFields("account_base_xrate")
            End If

            If Convert.IsDBNull(oFields("system_base_xrate")) Or Informations.IsNothing(oFields("system_base_xrate")) Then
                .SystemBaseXrate = CStr(0)
            Else
                .SystemBaseXrate = oFields("system_base_xrate")
            End If

            If Convert.IsDBNull(oFields("transdetail_type_id")) Or Informations.IsNothing(oFields("transdetail_type_id")) Then
                .TransdetailTypeID = 0
            Else
                .TransdetailTypeID = oFields("transdetail_type_id")
            End If

            If Convert.IsDBNull(oFields("reference")) Or Informations.IsNothing(oFields("reference")) Then
                .Reference = ""
            Else
                .Reference = oFields("reference")
            End If

            If Convert.IsDBNull(oFields("type_code")) Or Informations.IsNothing(oFields("type_code")) Then
                .TypeCode = ""
            Else
                .TypeCode = oFields("type_code")
            End If

            If Convert.IsDBNull(oFields("amount_currency_id")) Or Informations.IsNothing(oFields("amount_currency_id")) Then
                .BaseCurrencyID = 0
            Else
                .BaseCurrencyID = oFields("amount_currency_id")
            End If

            If Convert.IsDBNull(oFields("account_currency_id")) Or Informations.IsNothing(oFields("account_currency_id")) Then
                .AccountCurrencyID = 0
            Else
                .AccountCurrencyID = oFields("account_currency_id")
            End If

            If Convert.IsDBNull(oFields("account_amount")) Or Informations.IsNothing(oFields("account_amount")) Then
                .AccountAmount = 0
            Else
                .AccountAmount = oFields("account_amount")
            End If

            If Convert.IsDBNull(oFields("account_amount_unrounded")) Or Informations.IsNothing(oFields("account_amount_unrounded")) Then
                .AccountAmountUnrounded = 0
            Else
                .AccountAmountUnrounded = oFields("account_amount_unrounded")
            End If

            If Convert.IsDBNull(oFields("system_currency_id")) Or Informations.IsNothing(oFields("system_currency_id")) Then
                .SystemCurrencyID = 0
            Else
                .SystemCurrencyID = oFields("system_currency_id")
            End If

            If Convert.IsDBNull(oFields("system_amount")) Or Informations.IsNothing(oFields("system_amount")) Then
                .SystemAmount = 0
            Else
                .SystemAmount = oFields("system_amount")
            End If

            If Convert.IsDBNull(oFields("system_amount_unrounded")) Or Informations.IsNothing(oFields("system_amount_unrounded")) Then
                .SystemAmountUnrounded = 0
            Else
                .SystemAmountUnrounded = oFields("system_amount_unrounded")
            End If

            If Convert.IsDBNull(oFields("outstanding_currency_amount")) Or Informations.IsNothing(oFields("outstanding_currency_amount")) Then
                .OSCurrencyAmount = 0
            Else
                .OSCurrencyAmount = oFields("outstanding_currency_amount")
            End If

            If Convert.IsDBNull(oFields("outstanding_amount")) Or Informations.IsNothing(oFields("outstanding_amount")) Then
                .OSBaseAmount = 0
            Else
                .OSBaseAmount = oFields("outstanding_amount")
            End If

            If Convert.IsDBNull(oFields("outstanding_account_amount")) Or Informations.IsNothing(oFields("outstanding_account_amount")) Then
                .OSAccountAmount = 0
            Else
                .OSAccountAmount = oFields("outstanding_account_amount")
            End If

            If Convert.IsDBNull(oFields("outstanding_system_amount")) Or Informations.IsNothing(oFields("outstanding_system_amount")) Then
                .OSSystemAmount = 0
            Else
                .OSSystemAmount = oFields("outstanding_system_amount")
            End If

            If Convert.IsDBNull(oFields("amount_updated")) Or Informations.IsNothing(oFields("amount_updated")) Then

                .AmountUpdated = DateTime.MinValue
            Else
                .AmountUpdated = oFields("amount_updated")
            End If

            If Convert.IsDBNull(oFields("tax_group_id")) Or Informations.IsNothing(oFields("tax_group_id")) Then
                .TaxGroupID = 0
            Else
                .TaxGroupID = oFields("tax_group_id")
            End If

            If Convert.IsDBNull(oFields("tax_band_id")) Or Informations.IsNothing(oFields("tax_band_id")) Then
                .TaxBandID = 0
            Else
                .TaxBandID = oFields("tax_band_id")
            End If

            If Convert.IsDBNull(oFields("claim_ref")) Or Informations.IsNothing(oFields("claim_ref")) Then

                .ClaimReference = Nothing
            Else
                .ClaimReference = oFields("claim_ref")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

        End With

        Return result

    End Function
    ''' <summary>
    ''' SetProperties
    ''' </summary>
    ''' <param name="oTransDetail"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vAccountID"></param>
    ''' <param name="vPostingstatusID"></param>
    ''' <param name="vCompanyID"></param>
    ''' <param name="vCurrencyID"></param>
    ''' <param name="vPeriodID"></param>
    ''' <param name="vDocumentID"></param>
    ''' <param name="vDocumentSequence"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vAmount"></param>
    ''' <param name="vBaseAmountUnrounded"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vCurrencyAmount"></param>
    ''' <param name="vCurrencyAmountUnrounded"></param>
    ''' <param name="vEuroCurrencyId"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXrate"></param>
    ''' <param name="vComment"></param>
    ''' <param name="vInsuranceRef"></param>
    ''' <param name="vOperatorID"></param>
    ''' <param name="vPurchaseOrderNo"></param>
    ''' <param name="vPurchaseInvoiceNo"></param>
    ''' <param name="vDepartment"></param>
    ''' <param name="vSpare"></param>
    ''' <param name="vRefDate"></param>
    ''' <param name="vRefAmount"></param>
    ''' <param name="vRefQuantity"></param>
    ''' <param name="vRefUnits"></param>
    ''' <param name="vDepartmentID"></param>
    ''' <param name="vUnderwritingYearID"></param>
    ''' <param name="vInsuranceRefIndex"></param>
    ''' <param name="vCurrencyBaseXrate"></param>
    ''' <param name="vCurrencyBaseDate"></param>
    ''' <param name="vAccountBaseXrate"></param>
    ''' <param name="vAccountBaseDate"></param>
    ''' <param name="vSystemBaseXrate"></param>
    ''' <param name="vSystemBaseDate"></param>
    ''' <param name="vTransdetailTypeID"></param>
    ''' <param name="vReference"></param>
    ''' <param name="vTypeCode"></param>
    ''' <param name="vTaxGroupID"></param>
    ''' <param name="vTaxBandID"></param>
    ''' <param name="vClaimReference"></param>
    ''' <param name="vBalanceType"></param>
    ''' <param name="vRiskTransfer"></param>
    ''' <param name="vDueDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetProperties(ByRef oTransDetail As Transdetail, ByRef iStatus As Integer, Optional ByRef vTransdetailID As Object = Nothing,
                                   Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing,
                                   Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing,
                                   Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing,
                                   Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing,
                                   Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing,
                                   Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing,
                                   Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing,
                                   Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing,
                                   Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing,
                                   Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing,
                                   Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing,
                                   Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing,
                                   Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing,
                                   Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing,
                                   Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing,
                                   Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing,
                                   Optional ByRef vCurrencyBaseDate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing,
                                   Optional ByRef vAccountBaseDate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing,
                                   Optional ByRef vSystemBaseDate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing,
                                   Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing,
                                   Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vTaxBandID As Object = Nothing,
                                   Optional ByRef vClaimReference As Object = Nothing, Optional ByRef vBalanceType As Object = Nothing,
                                   Optional ByRef vRiskTransfer As Object = Nothing, Optional ByRef vDueDate As Object = Nothing, Optional ByVal oFeeType As Object = Nothing) As Integer

        Dim bDataChanged As Boolean



        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            m_lReturn = CheckMandatory(vTransdetailID:=vTransdetailID, vAccountID:=vAccountID, vPostingstatusID:=vPostingstatusID, vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=vDocumentID, vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=vAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity, vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID, vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate, vCurrencyBaseDate:=vCurrencyBaseDate, vAccountBaseXrate:=vAccountBaseXrate, vAccountBaseDate:=vAccountBaseDate, vSystemBaseXrate:=vSystemBaseXrate, vSystemBaseDate:=vSystemBaseDate, vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference, vTypeCode:=vTypeCode, vTaxGroupID:=vTaxGroupID, vTaxBandID:=vTaxBandID, vRiskTransfer:=vRiskTransfer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Default Any Missing Parameters

            m_lReturn = DefaultParameters(bDefaultAll:=False, vTransdetailID:=vTransdetailID, vAccountID:=vAccountID, vPostingstatusID:=vPostingstatusID,
                                          vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=vDocumentID,
                                          vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=vAmount,
                                          vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount,
                                          vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vEuroCurrencyId:=vEuroCurrencyId,
                                          vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXrate:=vEuroCcyXrate,
                                          vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID, vPurchaseOrderNo:=vPurchaseOrderNo,
                                          vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefDate:=vRefDate,
                                          vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity, vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID,
                                          vUnderwritingYearID:=vUnderwritingYearID, vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate,
                                          vCurrencyBaseDate:=vCurrencyBaseDate, vAccountBaseXrate:=vAccountBaseXrate, vAccountBaseDate:=vAccountBaseDate,
                                          vSystemBaseXrate:=vSystemBaseXrate, vSystemBaseDate:=vSystemBaseDate, vTransdetailTypeID:=vTransdetailTypeID,
                                          vReference:=vReference, vTypeCode:=vTypeCode, vTaxGroupID:=vTaxGroupID, vTaxBandID:=vTaxBandID,
                                          vClaimReference:=vClaimReference, vBalanceType:=vBalanceType, vRiskTransfer:=vRiskTransfer)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If

        ' Validate Parameters
        m_lReturn = Validate(vTransdetailID:=vTransdetailID, vAccountID:=vAccountID, vPostingstatusID:=vPostingstatusID, vCompanyID:=vCompanyID, vCurrencyID:=vCurrencyID, vPeriodID:=vPeriodID, vDocumentID:=vDocumentID, vDocumentSequence:=vDocumentSequence, vAccountingDate:=vAccountingDate, vAmount:=vAmount, vBaseAmountUnrounded:=vBaseAmountUnrounded, vFullyMatched:=vFullyMatched, vCurrencyAmount:=vCurrencyAmount, vCurrencyAmountUnrounded:=vCurrencyAmountUnrounded, vEuroCurrencyId:=vEuroCurrencyId, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXrate:=vEuroCcyXrate, vComment:=vComment, vInsuranceRef:=vInsuranceRef, vOperatorID:=vOperatorID, vPurchaseOrderNo:=vPurchaseOrderNo, vPurchaseInvoiceNo:=vPurchaseInvoiceNo, vDepartment:=vDepartment, vSpare:=vSpare, vRefDate:=vRefDate, vRefAmount:=vRefAmount, vRefQuantity:=vRefQuantity, vRefUnits:=vRefUnits, vDepartmentID:=vDepartmentID, vUnderwritingYearID:=vUnderwritingYearID, vInsuranceRefIndex:=vInsuranceRefIndex, vCurrencyBaseXrate:=vCurrencyBaseXrate, vCurrencyBaseDate:=vCurrencyBaseDate, vAccountBaseXrate:=vAccountBaseXrate, vAccountBaseDate:=vAccountBaseDate, vSystemBaseXrate:=vSystemBaseXrate, vSystemBaseDate:=vSystemBaseDate, vTransdetailTypeID:=vTransdetailTypeID, vReference:=vReference, vTypeCode:=vTypeCode, vTaxGroupID:=vTaxGroupID, vTaxBandID:=vTaxBandID, vRiskTransfer:=vRiskTransfer)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oTransDetail

            If Not Informations.IsNothing(vTransdetailID) And (Not Informations.IsDBNull(vTransdetailID)) Then
                If .TransdetailID <> vTransdetailID Then
                    .TransdetailID = vTransdetailID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAccountID) And (Not Informations.IsDBNull(vAccountID)) Then
                If .AccountID <> vAccountID Then
                    .AccountID = vAccountID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPostingstatusID) And (Not Informations.IsDBNull(vPostingstatusID)) Then
                If .PostingstatusID <> vPostingstatusID Then
                    .PostingstatusID = vPostingstatusID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCompanyID) And (Not Informations.IsDBNull(vCompanyID)) Then
                If .CompanyID <> vCompanyID Then
                    .CompanyID = vCompanyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyID) And (Not Informations.IsDBNull(vCurrencyID)) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPeriodID) And (Not Informations.IsDBNull(vPeriodID)) Then
                If .PeriodID <> vPeriodID Then
                    .PeriodID = vPeriodID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDocumentID) And (Not Informations.IsDBNull(vDocumentID)) Then
                If .DocumentID <> vDocumentID Then
                    .DocumentID = vDocumentID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDocumentSequence) And (Not Informations.IsDBNull(vDocumentSequence)) Then
                If .DocumentSequence <> vDocumentSequence Then
                    .DocumentSequence = vDocumentSequence
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAccountingDate) And (Not Informations.IsDBNull(vAccountingDate)) Then

                If .AccountingDate <> CDate(vAccountingDate) Then
                    .AccountingDate = CDate(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, vAccountingDate)) 'get rid of time data if present
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAmount) And (Not Informations.IsDBNull(vAmount)) Then
                If .Amount <> vAmount Then
                    .Amount = Math.Round(vAmount, 2)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vBaseAmountUnrounded) And (Not Informations.IsDBNull(vBaseAmountUnrounded)) Then

                If (.BaseAmountUnrounded <> vBaseAmountUnrounded) OrElse (vBaseAmountUnrounded = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .BaseAmountUnrounded = vBaseAmountUnrounded
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vFullyMatched) And (Not Informations.IsDBNull(vFullyMatched)) Then
                If .FullyMatched <> vFullyMatched Then
                    .FullyMatched = vFullyMatched
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyAmount) And (Not Informations.IsDBNull(vCurrencyAmount)) Then
                If .CurrencyAmount <> vCurrencyAmount Then
                    .CurrencyAmount = vCurrencyAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyAmountUnrounded) And (Not Informations.IsDBNull(vCurrencyAmountUnrounded)) Then

                If (.CurrencyAmountUnrounded <> vCurrencyAmountUnrounded) OrElse (vCurrencyAmountUnrounded = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .CurrencyAmountUnrounded = vCurrencyAmountUnrounded
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroCurrencyId) And (Not Informations.IsDBNull(vEuroCurrencyId)) Then

                If (.EuroCurrencyId <> vEuroCurrencyId) OrElse (vEuroCurrencyId = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .EuroCurrencyId = vEuroCurrencyId
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroAmount) And (Not Informations.IsDBNull(vEuroAmount)) Then

                If (.EuroAmount <> vEuroAmount) OrElse (vEuroAmount = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .EuroAmount = vEuroAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroBaseXRate) And (Not Informations.IsDBNull(vEuroBaseXRate)) Then

                If (.EuroBaseXrate <> vEuroBaseXRate) OrElse (vEuroBaseXRate = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .EuroBaseXrate = vEuroBaseXRate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroCcyXrate) And (Not Informations.IsDBNull(vEuroCcyXrate)) Then

                If (.EuroCcyXrate <> vEuroCcyXrate) OrElse (vEuroCcyXrate = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .EuroCcyXrate = vEuroCcyXrate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vComment) And (Not Informations.IsDBNull(vComment)) Then

                If CStr(.Comment).Trim() <> vComment.Trim() Then
                    .Comment = vComment
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vInsuranceRef) And (Not Informations.IsDBNull(vInsuranceRef)) Then

                If CStr(.InsuranceRef).Trim() <> CStr(vInsuranceRef).Trim() Then
                    .InsuranceRef = vInsuranceRef
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOperatorID) And (Not Informations.IsDBNull(vOperatorID)) Then

                If CStr(.OperatorID).Trim() <> CStr(vOperatorID).Trim() Then
                    .OperatorID = CInt(vOperatorID)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPurchaseOrderNo) And (Not Informations.IsDBNull(vPurchaseOrderNo)) Then
                If CStr(vPurchaseOrderNo).Trim() <> "" Then

                    If CStr(.PurchaseOrderNo).Trim() <> CStr(vPurchaseOrderNo).Trim() Then
                        .PurchaseOrderNo = vPurchaseOrderNo
                        bDataChanged = True
                    End If
                End If
            End If

            If Not Informations.IsNothing(vPurchaseInvoiceNo) And (Not Informations.IsDBNull(vPurchaseInvoiceNo)) Then
                If CStr(vPurchaseInvoiceNo).Trim() <> "" Then

                    If CStr(.PurchaseInvoiceNo).Trim() <> CStr(vPurchaseInvoiceNo).Trim() Then
                        .PurchaseInvoiceNo = vPurchaseInvoiceNo
                        bDataChanged = True
                    End If
                End If
            End If

            If Not Informations.IsNothing(vDepartment) And (Not Informations.IsDBNull(vDepartment)) Then
                If CStr(vDepartment).Trim() <> "" Then

                    If CStr(.Department).Trim() <> CStr(vDepartment).Trim() Then
                        .Department = vDepartment
                        bDataChanged = True
                    End If
                End If
            End If

            If Not Informations.IsNothing(vSpare) And (Not Informations.IsDBNull(vSpare)) Then
                If vSpare.Trim() <> "" Then

                    If CStr(.Spare).Trim() <> CStr(vSpare).Trim() Then
                        .Spare = vSpare.Trim()
                        bDataChanged = True
                    End If
                End If
            End If

            If Not Informations.IsNothing(vRefDate) And (Not Informations.IsDBNull(vRefDate)) Then

                If .RefDate <> CDate(vRefDate) Then
                    .RefDate = CDate(gPMFunctions.FormatField(gPMConstants.PMEFormatStyle.PMFormatDateShort, vRefDate)) 'get rid of time data if present
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vRefAmount) And (Not Informations.IsDBNull(vRefAmount)) Then
                If .RefAmount <> vRefAmount Then
                    .RefAmount = vRefAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vRefQuantity) And (Not Informations.IsDBNull(vRefQuantity)) Then

                If (.RefQuantity <> vRefQuantity) OrElse (vRefQuantity = 0) Then 'The empty/not empty bit is required because vb thinks Empty equals 0 when we may be initialising the value to 0, which would not get done without the extra test
                    .RefQuantity = vRefQuantity
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vRefUnits) And (Not Informations.IsDBNull(vRefUnits)) Then

                If CStr(.RefUnits).Trim() <> CStr(vRefUnits).Trim() Then
                    .RefUnits = vRefUnits
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDepartmentID) And (Not Informations.IsDBNull(vDepartmentID)) Then
                If .DepartmentID <> vDepartmentID Then
                    .DepartmentID = vDepartmentID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vUnderwritingYearID) And Not Informations.IsDBNull(vUnderwritingYearID) Then
                If .UnderwritingYearID <> vUnderwritingYearID Then
                    .UnderwritingYearID = vUnderwritingYearID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vInsuranceRefIndex) And Not Convert.IsDBNull(vInsuranceRefIndex) Then
                If .InsuranceRefIndex <> vInsuranceRefIndex Then
                    .InsuranceRefIndex = vInsuranceRefIndex
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyBaseXrate) And (Not Informations.IsDBNull(vCurrencyBaseXrate)) Then
                If .CurrencyBaseXrate <> vCurrencyBaseXrate Then
                    .CurrencyBaseXrate = vCurrencyBaseXrate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyBaseDate) And (Not Informations.IsDBNull(vCurrencyBaseDate)) Then
                'The date is not stored on the Transdetail record, so no need to check if it has changed.
                .CurrencyBaseDate = vCurrencyBaseDate
            End If

            If Not Informations.IsNothing(vAccountBaseXrate) And (Not Informations.IsDBNull(vAccountBaseXrate)) Then
                If .AccountBaseXrate <> vAccountBaseXrate Then
                    .AccountBaseXrate = vAccountBaseXrate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAccountBaseDate) And (Not Informations.IsDBNull(vAccountBaseDate)) Then
                'The date is not stored on the Transdetail record, so no need to check if it has changed.
                .AccountBaseDate = vAccountBaseDate
            End If

            If Not Informations.IsNothing(vSystemBaseXrate) And (Not Informations.IsDBNull(vSystemBaseXrate)) Then
                If .SystemBaseXrate <> vSystemBaseXrate Then
                    .SystemBaseXrate = vSystemBaseXrate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vSystemBaseDate) And (Not Informations.IsDBNull(vSystemBaseDate)) Then
                'The date is not stored on the Transdetail record, so no need to check if it has changed.
                .SystemBaseDate = vSystemBaseDate
            End If

            If Not Informations.IsNothing(vTransdetailTypeID) And (Not Informations.IsDBNull(vTransdetailTypeID)) Then
                If .TransdetailTypeID <> vTransdetailTypeID Then
                    .TransdetailTypeID = vTransdetailTypeID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vReference) And (Not Informations.IsDBNull(vReference)) Then
                If .Reference <> vReference Then
                    .Reference = vReference
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vTypeCode) And (Not Informations.IsDBNull(vTypeCode)) Then
                If .TypeCode <> vTypeCode Then
                    .TypeCode = vTypeCode
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vTaxGroupID) And (Not Informations.IsDBNull(vTaxGroupID)) Then
                If .TaxGroupID <> CInt(vTaxGroupID) Then
                    .TaxGroupID = CInt(vTaxGroupID)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vTaxBandID) And (Not Informations.IsDBNull(vTaxBandID)) Then
                If .TaxBandID <> CInt(vTaxBandID) Then
                    .TaxBandID = CInt(vTaxBandID)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vClaimReference) And (Not Informations.IsDBNull(vClaimReference)) Then
                If .ClaimReference <> vClaimReference Then
                    .ClaimReference = vClaimReference
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vBalanceType) And (Not Informations.IsDBNull(vBalanceType)) Then
                If .BalanceType <> vBalanceType Then
                    .BalanceType = vBalanceType
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vRiskTransfer) And (Not Informations.IsDBNull(vRiskTransfer)) Then
                .RiskTransfer = vRiskTransfer
                bDataChanged = True
            End If

            If Not Informations.IsNothing(vDueDate) And (Not Informations.IsDBNull(vDueDate)) Then
                .DueDate = vDueDate
                bDataChanged = True
            End If

            If Not Informations.IsNothing(oFeeType) And (Not Informations.IsDBNull(oFeeType)) Then
                .FeeType = oFeeType
                bDataChanged = True
            End If

            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return m_lReturn

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied Transdetail property values.
    '
    ' ***************************************************************** '

    Private Function GetProperties(ByRef oTransDetail As Transdetail, ByRef iStatus As Integer, Optional ByRef vTransdetailID As Object = Nothing,
                                   Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing,
                                   Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing,
                                   Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing,
                                   Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing,
                                   Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing,
                                   Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing,
                                   Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing,
                                   Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing,
                                   Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing,
                                   Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing,
                                   Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing,
                                   Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing,
                                   Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing,
                                   Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing,
                                   Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing,
                                   Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing,
                                   Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing,
                                   Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing,
                                   Optional ByRef vTypeCode As Object = Nothing, Optional ByRef vBaseCurrencyID As Object = Nothing,
                                   Optional ByRef vAccountCurrencyID As Object = Nothing, Optional ByRef vSystemCurrencyID As Object = Nothing,
                                   Optional ByRef vAccountAmount As Object = Nothing, Optional ByRef vAccountAmountUnrounded As Object = Nothing,
                                   Optional ByRef vSystemAmount As Object = Nothing, Optional ByRef vSystemAmountUnrounded As Object = Nothing,
                                   Optional ByRef vOSCurrencyAmount As Object = Nothing, Optional ByRef vOSBaseAmount As Object = Nothing,
                                   Optional ByRef vOSAccountAmount As Object = Nothing, Optional ByRef vOSSystemAmount As Object = Nothing,
                                   Optional ByRef vAmountUpdated As Object = Nothing, Optional ByRef vClaimReference As Object = Nothing,
                                   Optional ByRef vBalanceType As Object = Nothing, Optional ByRef vDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With oTransDetail

            vTransdetailID = .TransdetailID
            vAccountID = .AccountID
            vPostingstatusID = .PostingstatusID
            vCompanyID = .CompanyID
            vCurrencyID = .CurrencyID
            vPeriodID = .PeriodID
            vDocumentID = .DocumentID
            vDocumentSequence = .DocumentSequence
            vAccountingDate = .AccountingDate
            vAmount = .Amount
            vBaseAmountUnrounded = .BaseAmountUnrounded
            vFullyMatched = .FullyMatched
            vCurrencyAmount = .CurrencyAmount
            vCurrencyAmountUnrounded = .CurrencyAmountUnrounded
            vEuroCurrencyId = .EuroCurrencyId
            vEuroAmount = .EuroAmount
            vEuroBaseXRate = .EuroBaseXrate
            vEuroCcyXrate = .EuroCcyXrate
            vComment = .Comment
            vInsuranceRef = .InsuranceRef
            vOperatorID = .OperatorID
            vPurchaseOrderNo = .PurchaseOrderNo
            vPurchaseInvoiceNo = .PurchaseInvoiceNo
            vDepartment = .Department
            vSpare = .Spare
            vRefDate = .RefDate
            vRefAmount = .RefAmount
            vRefQuantity = .RefQuantity
            vRefUnits = .RefUnits
            vDepartmentID = .DepartmentID
            vUnderwritingYearID = .UnderwritingYearID
            vInsuranceRefIndex = .InsuranceRefIndex
            vCurrencyBaseXrate = .CurrencyBaseXrate
            vAccountBaseXrate = .AccountBaseXrate
            vSystemBaseXrate = .SystemBaseXrate
            vTransdetailTypeID = .TransdetailTypeID
            vReference = .Reference
            vTypeCode = .TypeCode
            vBaseCurrencyID = .BaseCurrencyID
            vAccountCurrencyID = .AccountCurrencyID
            vSystemCurrencyID = .SystemCurrencyID
            vAccountAmount = .AccountAmount
            vAccountAmountUnrounded = .AccountAmountUnrounded
            vSystemAmount = .SystemAmount
            vSystemAmountUnrounded = .SystemAmountUnrounded
            vOSCurrencyAmount = .OSCurrencyAmount
            vOSBaseAmount = .OSBaseAmount
            vOSAccountAmount = .OSAccountAmount
            vOSSystemAmount = .OSSystemAmount
            vAmountUpdated = .AmountUpdated
            vClaimReference = .ClaimReference
            vBalanceType = .BalanceType
            iStatus = .DatabaseStatus
            vDueDate = .DueDate

        End With

        Return result

    End Function
    ''' <summary>
    ''' AddInputParam
    ''' </summary>
    ''' <param name="oTransDetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddInputParam(ByRef oTransDetail As Transdetail) As Integer

        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If oTransDetail.AccountID < 1 Then
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="account_id", vValue:=oTransDetail.AccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.PostingstatusID < 1 Then
                m_lReturn = .Parameters.Add(sName:="postingstatus_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="postingstatus_id", vValue:=oTransDetail.PostingstatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.CompanyID < 1 Then
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="company_id", vValue:=oTransDetail.CompanyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.CurrencyID < 1 Then
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_id", vValue:=oTransDetail.CurrencyID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.PeriodID < 1 Then
                m_lReturn = .Parameters.Add(sName:="period_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="period_id", vValue:=oTransDetail.PeriodID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.DocumentID < 1 Then
                m_lReturn = .Parameters.Add(sName:="document_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="document_id", vValue:=oTransDetail.DocumentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="document_sequence", vValue:=oTransDetail.DocumentSequence, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="accounting_date", vValue:=oTransDetail.AccountingDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="amount", vValue:=oTransDetail.Amount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.BaseAmountUnrounded = 0 Then
                m_lReturn = .Parameters.Add(sName:="base_amount_unrounded", vValue:=oTransDetail.Amount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            Else
                m_lReturn = .Parameters.Add(sName:="base_amount_unrounded", vValue:=oTransDetail.BaseAmountUnrounded, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="fully_matched", vValue:=oTransDetail.FullyMatched, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="currency_amount", vValue:=oTransDetail.CurrencyAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.CurrencyAmountUnrounded = 0 Then
                m_lReturn = .Parameters.Add(sName:="currency_amount_unrounded", vValue:=oTransDetail.CurrencyAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_amount_unrounded", vValue:=oTransDetail.CurrencyAmountUnrounded, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.EuroCurrencyId < 1 Then
                m_lReturn = .Parameters.Add(sName:="euro_currency_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="euro_currency_id", vValue:=oTransDetail.EuroCurrencyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            m_lReturn = .Parameters.Add(sName:="euro_amount", vValue:=oTransDetail.EuroAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="euro_base_xrate", vValue:=oTransDetail.EuroBaseXrate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="euro_ccy_xrate", vValue:=oTransDetail.EuroCcyXrate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.Comment.Trim() = "" Then
                m_lReturn = .Parameters.Add(sName:="comment", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="comment", vValue:=oTransDetail.Comment, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.InsuranceRef.Trim() = "" Then
                m_lReturn = .Parameters.Add(sName:="insurance_ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="insurance_ref", vValue:=oTransDetail.InsuranceRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="operator_id", vValue:=oTransDetail.OperatorID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.PurchaseOrderNo.Trim() = "" Then
                m_lReturn = .Parameters.Add(sName:="purchase_order_no", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="purchase_order_no", vValue:=oTransDetail.PurchaseOrderNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.PurchaseInvoiceNo.Trim() = "" Then
                m_lReturn = .Parameters.Add(sName:="purchase_invoice_no", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="purchase_invoice_no", vValue:=oTransDetail.PurchaseInvoiceNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.Department.Trim() = "" Then
                m_lReturn = .Parameters.Add(sName:="department", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="department", vValue:=oTransDetail.Department, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.Spare Is DBNull.Value Then
                m_lReturn = .Parameters.Add(sName:="spare", vValue:="", iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="spare", vValue:=oTransDetail.Spare, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ref_date", vValue:=oTransDetail.RefDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ref_amount", vValue:=oTransDetail.RefAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ref_quantity", vValue:=oTransDetail.RefQuantity, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="ref_units", vValue:=oTransDetail.RefUnits, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsDBNull(oTransDetail.InsuranceRefIndex) OrElse oTransDetail.InsuranceRefIndex = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref_index", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_ref_index", vValue:=oTransDetail.InsuranceRefIndex, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oTransDetail.DepartmentID = 0 Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="department_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="department_id", vValue:=oTransDetail.DepartmentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToLong(oTransDetail.UnderwritingYearID) = 0 Then
                m_lReturn = .Parameters.Add(sName:="underwriting_year_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="underwriting_year_id", vValue:=oTransDetail.UnderwritingYearID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToDouble(oTransDetail.CurrencyBaseXrate) = 0 Then
                m_lReturn = .Parameters.Add(sName:="currency_base_xrate", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_base_xrate", vValue:=oTransDetail.CurrencyBaseXrate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToDate(oTransDetail.CurrencyBaseDate) = DateTime.MinValue OrElse gPMFunctions.NullToDate(oTransDetail.CurrencyBaseDate) = #12/30/1899# OrElse gPMFunctions.NullToDate(oTransDetail.CurrencyBaseDate) = #12/29/1899# Then
                m_lReturn = .Parameters.Add(sName:="currency_base_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="currency_base_date", vValue:=oTransDetail.CurrencyBaseDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If ToSafeDecimal(oTransDetail.AccountBaseXrate) = 0 Then
                m_lReturn = .Parameters.Add(sName:="account_base_xrate", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="account_base_xrate", vValue:=oTransDetail.AccountBaseXrate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToDate(oTransDetail.AccountBaseDate) = DateTime.MinValue OrElse gPMFunctions.NullToDate(oTransDetail.AccountBaseDate) = #12/30/1899# OrElse gPMFunctions.NullToDate(oTransDetail.AccountBaseDate) = #12/29/1899# Then
                m_lReturn = .Parameters.Add(sName:="account_base_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="account_base_date", vValue:=oTransDetail.AccountBaseDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToDouble(oTransDetail.SystemBaseXrate) = 0 Then
                m_lReturn = .Parameters.Add(sName:="system_base_xrate", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            Else
                m_lReturn = .Parameters.Add(sName:="system_base_xrate", vValue:=oTransDetail.SystemBaseXrate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToDate(oTransDetail.SystemBaseDate) = DateTime.MinValue OrElse gPMFunctions.NullToDate(oTransDetail.SystemBaseDate) = #12/30/1899# OrElse gPMFunctions.NullToDate(oTransDetail.SystemBaseDate) = #12/29/1899# Then
                m_lReturn = .Parameters.Add(sName:="system_base_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="system_base_date", vValue:=oTransDetail.SystemBaseDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToInteger(oTransDetail.TransdetailTypeID) = 0 Then
                m_lReturn = .Parameters.Add(sName:="transdetail_type_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = .Parameters.Add(sName:="transdetail_type_id", vValue:=oTransDetail.TransdetailTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToString(oTransDetail.Reference) = "" Then
                m_lReturn = .Parameters.Add(sName:="reference", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="reference", vValue:=oTransDetail.Reference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToString(oTransDetail.TypeCode) = "" Then
                m_lReturn = .Parameters.Add(sName:="type_code", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="type_code", vValue:=oTransDetail.TypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToString(CStr(oTransDetail.TaxGroupID)) = "" Then
                m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="tax_group_id", vValue:=oTransDetail.TaxGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToString(CStr(oTransDetail.TaxBandID)) = "" Then
                m_lReturn = .Parameters.Add(sName:="tax_band_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="tax_band_id", vValue:=oTransDetail.TaxBandID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'S4B Claims Enhancements R&D 2005
            If gPMFunctions.NullToString(oTransDetail.ClaimReference) = "" Then
                m_lReturn = .Parameters.Add(sName:="claim_ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="claim_ref", vValue:=oTransDetail.ClaimReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            ' Float Balance and Pre-Payment
            If gPMFunctions.NullToString(oTransDetail.BalanceType) = "" Then

                m_lReturn = .Parameters.Add(sName:="balance_type", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else

                m_lReturn = .Parameters.Add(sName:="balance_type", vValue:=oTransDetail.BalanceType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If gPMFunctions.NullToString(oTransDetail.RiskTransfer) = "" OrElse gPMFunctions.NullToString(oTransDetail.RiskTransfer) = PMConst.PMFalse Then
                m_lReturn = .Parameters.Add(sName:="risk_transfer", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else

                m_lReturn = .Parameters.Add(sName:="risk_transfer", vValue:=oTransDetail.RiskTransfer, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.NullToDate(oTransDetail.DueDate) = DateTime.MinValue OrElse gPMFunctions.NullToDate(oTransDetail.DueDate) = #12/30/1899# OrElse gPMFunctions.NullToDate(oTransDetail.DueDate) = #12/29/1899# Then
                m_lReturn = .Parameters.Add(sName:="due_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = .Parameters.Add(sName:="due_date", vValue:=oTransDetail.DueDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If gPMFunctions.NullToString(oTransDetail.FeeType) = "" Then
                m_lReturn = .Parameters.Add(sName:="FeeType", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="FeeType", vValue:=oTransDetail.FeeType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters (Public)
    '
    ' Description: Sets the Default Values for a Transdetail.
    '
    ' ***************************************************************** '

    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing,
                                       Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing,
                                       Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing,
                                       Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing,
                                       Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing,
                                       Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing,
                                       Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing,
                                       Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing,
                                       Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing,
                                       Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing,
                                       Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing,
                                       Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing,
                                       Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing,
                                       Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing,
                                       Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing,
                                       Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing,
                                       Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing,
                                       Optional ByRef vCurrencyBaseDate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing,
                                       Optional ByRef vAccountBaseDate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing,
                                       Optional ByRef vSystemBaseDate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing,
                                       Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing,
                                       Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vTaxBandID As Object = Nothing,
                                       Optional ByRef vClaimReference As Object = Nothing, Optional ByRef vBalanceType As Object = Nothing,
                                       Optional ByRef vRiskTransfer As Object = Nothing, Optional ByRef vDueDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If (Informations.IsNothing(vTransdetailID)) OrElse (vTransdetailID.Equals(0)) OrElse (bDefaultAll) Then
            vTransdetailID = 0
        End If

        If (Informations.IsNothing(vAccountID)) OrElse (vAccountID.Equals(0)) OrElse (bDefaultAll) Then
            vAccountID = 0
        End If

        If (Informations.IsNothing(vPostingstatusID)) OrElse (vPostingstatusID.Equals(0)) OrElse (bDefaultAll) Then
            vPostingstatusID = 0
        End If

        If (Informations.IsNothing(vCompanyID)) OrElse (vCompanyID.Equals(0)) OrElse (bDefaultAll) Then
            vCompanyID = 0
        End If

        If (Informations.IsNothing(vCurrencyID)) OrElse (vCurrencyID.Equals(0)) OrElse (bDefaultAll) Then
            vCurrencyID = 0
        End If

        If (Informations.IsNothing(vPeriodID)) OrElse (vPeriodID.Equals(0)) OrElse (bDefaultAll) Then
            vPeriodID = 0
        End If

        If (Informations.IsNothing(vDocumentID)) OrElse (vDocumentID.Equals(0)) OrElse (bDefaultAll) Then
            vDocumentID = 0
        End If

        If (Informations.IsNothing(vDocumentSequence)) OrElse (vDocumentSequence.Equals(0)) OrElse (bDefaultAll) Then
            vDocumentSequence = 0
        End If

        If (Informations.IsNothing(vAccountingDate)) OrElse (vAccountingDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vAccountingDate = DateTime.Now
        End If

        If (Informations.IsNothing(vAmount)) OrElse (vAmount.Equals(0)) OrElse (bDefaultAll) Then
            vAmount = 0
        End If

        If (Informations.IsNothing(vBaseAmountUnrounded)) OrElse (vBaseAmountUnrounded.Equals(0)) OrElse (bDefaultAll) Then
            vBaseAmountUnrounded = 0
        End If

        If (Informations.IsNothing(vFullyMatched)) OrElse (vFullyMatched.Equals(0)) OrElse (bDefaultAll) Then
            vFullyMatched = 0
        End If

        If (Informations.IsNothing(vCurrencyAmount)) OrElse (vCurrencyAmount.Equals(0)) OrElse (bDefaultAll) Then
            vCurrencyAmount = 0
        End If

        If (Informations.IsNothing(vCurrencyAmountUnrounded)) OrElse (vCurrencyAmountUnrounded.Equals(0)) OrElse (bDefaultAll) Then
            vCurrencyAmountUnrounded = 0
        End If

        If (Informations.IsNothing(vEuroCurrencyId)) OrElse (vEuroCurrencyId.Equals(0)) OrElse (bDefaultAll) Then
            vEuroCurrencyId = 0
        End If

        If (Informations.IsNothing(vEuroAmount)) OrElse (vEuroAmount.Equals(0)) OrElse (bDefaultAll) Then
            vEuroAmount = 0
        End If

        If (Informations.IsNothing(vEuroBaseXRate)) OrElse (vEuroBaseXRate.Equals(0)) OrElse (bDefaultAll) Then
            vEuroBaseXRate = 1
        End If

        If (Informations.IsNothing(vEuroCcyXrate)) OrElse (vEuroCcyXrate.Equals(0)) OrElse (bDefaultAll) Then
            vEuroCcyXrate = 1
        End If

        If (Informations.IsNothing(vComment)) OrElse (Convert.IsDBNull(vComment) OrElse Informations.IsNothing(vComment)) OrElse (bDefaultAll) Then
            vComment = ""
        End If

        If (Informations.IsNothing(vInsuranceRef)) OrElse (Convert.IsDBNull(vInsuranceRef) OrElse Informations.IsNothing(vInsuranceRef)) OrElse (bDefaultAll) Then
            vInsuranceRef = ""
        End If

        If (Informations.IsNothing(vOperatorID)) OrElse (vOperatorID.Equals(0)) OrElse (bDefaultAll) Then
            vOperatorID = 0
        End If

        If (Informations.IsNothing(vPurchaseOrderNo)) OrElse (Convert.IsDBNull(vPurchaseOrderNo) OrElse Informations.IsNothing(vPurchaseOrderNo)) OrElse (bDefaultAll) Then
            vPurchaseOrderNo = ""
        End If

        If (Informations.IsNothing(vPurchaseInvoiceNo)) OrElse (Convert.IsDBNull(vPurchaseInvoiceNo) OrElse Informations.IsNothing(vPurchaseInvoiceNo)) OrElse (bDefaultAll) Then
            vPurchaseInvoiceNo = ""
        End If

        If (Informations.IsNothing(vDepartment)) OrElse (Convert.IsDBNull(vDepartment) OrElse Informations.IsNothing(vDepartment)) OrElse (bDefaultAll) Then
            vDepartment = ""
        End If

        If (Informations.IsNothing(vSpare)) OrElse (Convert.IsDBNull(vSpare) OrElse Informations.IsNothing(vSpare)) OrElse (bDefaultAll) Then
            vSpare = ""
        End If

        If (Informations.IsNothing(vRefDate)) OrElse (vRefDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vRefDate = DateTime.Now
        End If

        If (Informations.IsNothing(vRefAmount)) OrElse (vRefAmount.Equals(0)) OrElse (bDefaultAll) Then
            vRefAmount = 0
        End If

        If (Informations.IsNothing(vRefQuantity)) OrElse (vRefQuantity.Equals(0)) OrElse (bDefaultAll) Then
            vRefQuantity = 0
        End If

        If (Informations.IsNothing(vRefUnits)) OrElse (vRefUnits.Equals(0)) OrElse (bDefaultAll) Then
            vRefUnits = 0
        End If

        If (Convert.IsDBNull(vDepartmentID) OrElse Informations.IsNothing(vDepartmentID)) OrElse (vDepartmentID.Equals(0)) OrElse (bDefaultAll) Then
            vDepartmentID = 0
        End If

        If (Informations.IsNothing(vUnderwritingYearID)) OrElse (Object.Equals(vUnderwritingYearID, Nothing)) OrElse (bDefaultAll) Then

            vUnderwritingYearID = DBNull.Value
        End If

        If (Informations.IsNothing(vInsuranceRefIndex)) OrElse (Object.Equals(vInsuranceRefIndex, Nothing)) OrElse (bDefaultAll) Then

            vInsuranceRefIndex = DBNull.Value
        End If

        If (Informations.IsNothing(vCurrencyBaseXrate)) OrElse (vCurrencyBaseXrate.Equals(0)) OrElse (bDefaultAll) Then
            vCurrencyBaseXrate = 0
        End If

        If (Informations.IsNothing(vCurrencyBaseDate)) OrElse (vCurrencyBaseDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vCurrencyBaseDate = vAccountingDate
        End If

        If (Informations.IsNothing(vAccountBaseXrate)) OrElse (vAccountBaseXrate.Equals(0)) OrElse (bDefaultAll) Then
            vAccountBaseXrate = 0
        End If

        If (Informations.IsNothing(vAccountBaseDate)) OrElse (vAccountBaseDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vAccountBaseDate = vAccountingDate
        End If

        If (Informations.IsNothing(vSystemBaseXrate)) OrElse (vSystemBaseXrate.Equals(0)) OrElse (bDefaultAll) Then
            vSystemBaseXrate = 0
        End If

        If (Informations.IsNothing(vSystemBaseDate)) OrElse (vSystemBaseDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vSystemBaseDate = vAccountingDate
        End If

        If (Informations.IsNothing(vTransdetailTypeID)) OrElse (vTransdetailTypeID.Equals(0)) OrElse (bDefaultAll) Then
            vTransdetailTypeID = 0
        End If

        If (Informations.IsNothing(vReference)) OrElse (bDefaultAll) Then
            vReference = ""
        End If

        If (Informations.IsNothing(vTypeCode)) OrElse (bDefaultAll) Then
            vTypeCode = ""
        End If

        If (Informations.IsNothing(vTaxGroupID)) OrElse (vTaxGroupID.Equals(0)) OrElse (bDefaultAll) Then
            vTaxGroupID = 0
        End If

        If (Informations.IsNothing(vTaxBandID)) OrElse (vTaxBandID.Equals(0)) OrElse (bDefaultAll) Then
            vTaxBandID = 0
        End If

        If (Informations.IsNothing(vClaimReference)) OrElse (bDefaultAll) Then
            vClaimReference = ""
        End If

        If (Informations.IsNothing(vBalanceType)) OrElse (bDefaultAll) Then
            vBalanceType = ""
        End If

        If (Informations.IsNothing(vRiskTransfer)) OrElse (bDefaultAll) Then
            vRiskTransfer = ""
        End If

        If (Informations.IsNothing(vDueDate)) OrElse (vDueDate.Equals(DateTime.FromOADate(0))) OrElse (bDefaultAll) Then
            vDueDate = ""
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Transdetail.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vCurrencyBaseDate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vAccountBaseDate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vSystemBaseDate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vTaxBandID As Object = Nothing, Optional ByRef vRiskTransfer As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        If (Informations.IsNothing(vTransdetailID)) Or (Object.Equals(vTransdetailID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vAccountID)) Or (Object.Equals(vAccountID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vPostingstatusID)) Or (Object.Equals(vPostingstatusID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vCompanyID)) Or (Object.Equals(vCompanyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vCurrencyID)) Or (Object.Equals(vCurrencyID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vPeriodID)) Or (Object.Equals(vPeriodID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vDocumentID)) Or (Object.Equals(vDocumentID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vDocumentSequence)) Or (Object.Equals(vDocumentSequence, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vAccountingDate)) Or (Object.Equals(vAccountingDate, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vAmount)) Or (Object.Equals(vAmount, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vFullyMatched)) Or (Object.Equals(vFullyMatched, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        If (Informations.IsNothing(vCurrencyAmount)) Or (Object.Equals(vCurrencyAmount, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Transdetail for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vAccountID As Object = Nothing, Optional ByRef vPostingstatusID As Object = Nothing, Optional ByRef vCompanyID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vPeriodID As Object = Nothing, Optional ByRef vDocumentID As Object = Nothing, Optional ByRef vDocumentSequence As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vAmount As Object = Nothing, Optional ByRef vBaseAmountUnrounded As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vCurrencyAmount As Object = Nothing, Optional ByRef vCurrencyAmountUnrounded As Object = Nothing, Optional ByRef vEuroCurrencyId As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXrate As Object = Nothing, Optional ByRef vComment As Object = Nothing, Optional ByRef vInsuranceRef As Object = Nothing, Optional ByRef vOperatorID As Object = Nothing, Optional ByRef vPurchaseOrderNo As Object = Nothing, Optional ByRef vPurchaseInvoiceNo As Object = Nothing, Optional ByRef vDepartment As Object = Nothing, Optional ByRef vSpare As Object = Nothing, Optional ByRef vRefDate As Object = Nothing, Optional ByRef vRefAmount As Object = Nothing, Optional ByRef vRefQuantity As Object = Nothing, Optional ByRef vRefUnits As Object = Nothing, Optional ByRef vDepartmentID As Object = Nothing, Optional ByRef vUnderwritingYearID As Object = Nothing, Optional ByRef vInsuranceRefIndex As Object = Nothing, Optional ByRef vCurrencyBaseXrate As Object = Nothing, Optional ByRef vCurrencyBaseDate As Object = Nothing, Optional ByRef vAccountBaseXrate As Object = Nothing, Optional ByRef vAccountBaseDate As Object = Nothing, Optional ByRef vSystemBaseXrate As Object = Nothing, Optional ByRef vSystemBaseDate As Object = Nothing, Optional ByRef vTransdetailTypeID As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vTypeCode As Object = Nothing, Optional ByRef vTaxGroupID As Object = Nothing, Optional ByRef vTaxBandID As Object = Nothing, Optional ByRef vRiskTransfer As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}

        'Modified,as per vb code change Double.TryParse to IsNumeric and add an extra condition for check dbnull
        If (Not Informations.IsNothing(vTransdetailID)) And (Not Informations.IsNumeric(vTransdetailID)) And (Not Informations.IsDBNull(vTransdetailID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vAccountID)) And (Not Informations.IsNumeric(vAccountID)) And (Not Informations.IsDBNull(vAccountID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vPostingstatusID)) And (Not Informations.IsNumeric(vPostingstatusID)) And (Not Informations.IsDBNull(vPostingstatusID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vCompanyID)) And (Not Informations.IsNumeric(vCompanyID)) And (Not Informations.IsDBNull(vCompanyID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vCurrencyID)) And (Not Informations.IsNumeric(vCurrencyID)) And (Not Informations.IsDBNull(vCurrencyID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vPeriodID)) And (Not Informations.IsNumeric(vPeriodID)) And (Not Informations.IsDBNull(vPeriodID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vDocumentID)) And (Not Informations.IsNumeric(vDocumentID)) And (Not Informations.IsDBNull(vDocumentID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vDocumentSequence)) And (Not Informations.IsNumeric(vDocumentSequence)) And (Not Informations.IsDBNull(vDocumentSequence)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Not Informations.IsNothing(vAccountingDate)) And (Not Informations.IsDate(vAccountingDate)) And (Not Informations.IsDBNull(vAccountingDate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vFullyMatched)) And (Not Informations.IsNumeric(vFullyMatched)) And (Not Informations.IsDBNull(vFullyMatched)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vAmount)) And (Not Informations.IsNumeric(vAmount)) And (Not Informations.IsDBNull(vAmount)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vBaseAmountUnrounded)) And (Not Informations.IsNumeric(vBaseAmountUnrounded)) And (Not Informations.IsDBNull(vBaseAmountUnrounded)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vCurrencyAmount)) And (Not Informations.IsNumeric(vCurrencyAmount)) And (Not Informations.IsDBNull(vCurrencyAmount)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vCurrencyAmountUnrounded)) And (Not Informations.IsNumeric(vCurrencyAmountUnrounded)) And (Not Informations.IsDBNull(vCurrencyAmountUnrounded)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vEuroCurrencyId)) And (Not Informations.IsNumeric(vEuroCurrencyId)) And (Not Informations.IsDBNull(vEuroCurrencyId)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vEuroAmount)) And (Not Informations.IsNumeric(vEuroAmount)) And (Not Informations.IsDBNull(vEuroAmount)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vEuroBaseXRate)) And (Not Informations.IsNumeric(vEuroBaseXRate)) And (Not Informations.IsDBNull(vEuroBaseXRate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vEuroCcyXrate)) And (Not Informations.IsNumeric(vEuroCcyXrate)) And (Not Informations.IsDBNull(vEuroCcyXrate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Not Informations.IsNothing(vRefDate)) And (Not Informations.IsDate(vRefDate)) And (Not Informations.IsDBNull(vRefDate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vDepartmentID)) And (Not Informations.IsNumeric(vDepartmentID)) And (Not Informations.IsDBNull(vDepartmentID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vCurrencyBaseXrate)) And (Not Informations.IsNumeric(vCurrencyBaseXrate)) And (Not Informations.IsDBNull(vCurrencyBaseXrate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Not Informations.IsNothing(vCurrencyBaseDate)) And (Not Informations.IsDate(vCurrencyBaseDate)) And (Not Informations.IsDBNull(vCurrencyBaseDate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vAccountBaseXrate)) And (Not Informations.IsNumeric(vAccountBaseXrate)) And (Not Informations.IsDBNull(vAccountBaseXrate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Not Informations.IsNothing(vAccountBaseDate)) And (Not Informations.IsDate(vAccountBaseDate)) And (Not Informations.IsDBNull(vAccountBaseDate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (Not Informations.IsNothing(vSystemBaseXrate)) And (Not Informations.IsNumeric(vSystemBaseXrate)) And (Not Informations.IsDBNull(vSystemBaseXrate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If (Not Informations.IsNothing(vSystemBaseDate)) And (Not Informations.IsDate(vSystemBaseDate)) And (Not Informations.IsDBNull(vSystemBaseDate)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vTransdetailTypeID)) And (Not Informations.IsNumeric(vTransdetailTypeID)) And (Not Informations.IsDBNull(vTransdetailTypeID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vTaxGroupID)) And (Not Informations.IsNumeric(vTaxGroupID)) And (Not Informations.IsDBNull(vTaxGroupID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        If (Not Informations.IsNothing(vTaxBandID)) And (Not Informations.IsNumeric(vTaxBandID)) And (Not Informations.IsDBNull(vTaxBandID)) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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

            m_lReturn = m_oDatabase.SQLBeginTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

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

            m_lReturn = m_oDatabase.SQLCommitTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

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

            m_lReturn = m_oDatabase.SQLRollbackTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    Public Function GetRiskTransferStatus(ByVal lAccountID As Integer, ByVal lDocumentID As Integer, ByVal cAmount As Decimal) As gPMConstants.PMEReturnCode

        Dim sSQL As String = ""
        Dim vDocumentType(,) As Object = Nothing
        Dim lDocumentType As Integer
        Dim vRiskTransferAgreement As Object = Nothing
        Dim vLedgerCode(,) As Object = Nothing
        Dim sLedgerCode As String = ""

        Try

            'First check that this is an insurer
            With m_oDatabase
                'Get the LedgerType for this account
                sSQL = "select lt.code from account " & "inner join ledger l on account.ledger_id = l.ledger_id " & "inner join ledgertype lt on l.ledgertype_id = lt.ledgertype_id " & "where account.Account_id = {lAccountID}"

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="lAccountID", vValue:=lAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="Get Ledger Type", bStoredProcedure:=False, vResultArray:=vLedgerCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetRiskTransferStatus Failed to get LedgerType - bACTTransdetailForm")
                End If
            End With

            If Informations.IsArray(vLedgerCode) Then

                sLedgerCode = gPMFunctions.ToSafeString(CStr(vLedgerCode(0, 0))).Trim()
            End If

            If sLedgerCode.ToUpper() = "I" Then 'We have an insurer

                'Get the Risk Transfer Agreement flag
                With m_oDatabase

                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="DocumentId", vValue:=lDocumentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .Parameters.Add(sName:="AccountId", vValue:=lAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLSelect(sSQL:=AC_SQL_RiskTransferStatus_SQL, sSQLName:=AC_SQL_RiskTransferStatus_Name, bStoredProcedure:=AC_SQL_RiskTransferStatus_SP, vResultArray:=vRiskTransferAgreement)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetRiskTransferStatus Failed to get Risk_Transfer_Agreement - bACTTransdetailForm")
                    End If
                End With

                If Informations.IsArray(vRiskTransferAgreement) Then

                    If gPMFunctions.ToSafeBoolean(vRiskTransferAgreement(0, 0)) Then

                        vRiskTransferAgreement = 1
                    Else

                        vRiskTransferAgreement = 0
                    End If
                Else

                    vRiskTransferAgreement = 0
                End If

                'Get the DocumentType
                With m_oDatabase
                    sSQL = "SELECT DocumentType_id from Document where Document_ID = {lDocumentID}"

                    .Parameters.Clear()

                    m_lReturn = .Parameters.Add(sName:="lDocumentID", vValue:=lDocumentID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetDopcumentType", bStoredProcedure:=False, vResultArray:=vDocumentType)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(Constants.vbObjectError.ToString() + ", " + +", GetRiskTransferStatus Failed to get DocumentType - bACTTransdetailForm")
                    End If
                End With
                If Informations.IsArray(vDocumentType) Then

                    lDocumentType = (CInt(vDocumentType(0, 0)))
                End If

                'Finally work out the RT Status
                Select Case lDocumentType
                    Case 4, 15, 17, 31, 35, 29, 1 'SND, SRD, SED, SHD, TRD, CLR, JN (+ve only)
                        If lDocumentType <> 1 Or (lDocumentType = 1 And cAmount < 0) Then

                            If CDbl(vRiskTransferAgreement) = 0 Then ' No Agreement
                                Return gPMConstants.PMEReturnCode.PMTrue ' RT status Raised
                            Else
                                Return gPMConstants.PMEReturnCode.PMFalse ' RT - 0 for Insurer with RT Agreement
                            End If
                        Else
                            Return gPMConstants.PMEReturnCode.PMFalse ' RT - 0 for -ve JN (negative journals)
                        End If
                    Case Else
                        Return gPMConstants.PMEReturnCode.PMFalse ' RT - 0 for all other transaction type
                End Select

            Else

                Return Nothing ' RT - null for all non Insurer Transactions
            End If

        Catch excep As System.Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskTransferStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ReleaseManualTransactions
    '
    ' Description: Releases Manually Suspended Transactions
    '
    ' History: Created 31 Jul 2007 - RC
    ' ***************************************************************** '
    Public Function ReleaseManualTransactions() As Integer

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object = Nothing
            Dim vLinkedTransdetailID As String = ""
            Dim lAllocationId As Integer
            Dim lvalue As Long

            result = gPMConstants.PMEReturnCode.PMTrue

            'Call spu_ACT_ManualSuspendedAccountsTransactions_Sel

            IsReleaseManualTransactionStarted(v_lReturnValue:=lvalue)
            If lvalue <> 1 Then
                SetReleaseManualTransactionStarted(1)
            Else
                ReleaseManualTransactions = PMEReturnCode.PMMAlreadyInUse
                Exit Function
            End If
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.SQLSelect("spu_ACT_ManualSuspendedAccountsTransactions_Sel", "Select Manual Suspended Accounts Transactions", True, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call spu_ACT_ManualSuspendedAccountsTransactions_Sel", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseManualTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If

            If Informations.IsArray(vResultArray) Then

                'Loop through the results calling ReleaseSuspendedTransactions
                'passing through linked_transdetail_id and allocation_id

                For i As Integer = 0 To vResultArray.GetUpperBound(1)

                    vLinkedTransdetailID = gPMFunctions.ToSafeString(CStr(vResultArray(0, i))).Trim()

                    lAllocationId = CInt(gPMFunctions.ToSafeString(CStr(vResultArray(1, i))).Trim())

                    m_lReturn = ReleaseSuspendedTransactions(lAllocationId:=lAllocationId, vLinkedTransdetailID:=vLinkedTransdetailID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call ReleaseSuspendedTransactions", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseManualTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    End If

                Next

            End If
            SetReleaseManualTransactionStarted(0)
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReleaseManualTransactions", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseManualTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            SetReleaseManualTransactionStarted(0)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: IsReleaseManualTransactionStarted
    '
    ' Description: If This process is started set the DB flag. So the another user can not run again.
    '
    ' ***************************************************************** '
    Private Function IsReleaseManualTransactionStarted(ByRef v_lReturnValue As Long) As Long
        Dim m_lReturn As Long


        IsReleaseManualTransactionStarted = gPMConstants.PMEReturnCode.PMTrue
        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("ReturnValue", v_lReturnValue, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = .SQLSelect("spu_ACT_GetSetReleaseManualTransProcessFlag",
                "Get ReleaseManualTransProcessStarted Flag", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername,
                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                        sMsg:="Failed to call spu_ACT_GetSetReleaseManualTransProcessFlag",
                        vApp:=ACApp,
                        vClass:=ACClass,
                        vMethod:="IsReleaseManualTransactionStarted",
                        vErrNo:=Informations.Err.Number,
                        vErrDesc:=Informations.Err.Description)
            End If

            If Convert.IsDBNull(.Parameters.Item("ReturnValue").Value) Then
                v_lReturnValue = 0
            Else
                v_lReturnValue = (.Parameters.Item("ReturnValue").Value).ToString.Trim
            End If
        End With

        Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: SetReleaseManualTransactionStarted
    '
    ' Description: If This process is started set the DB flag. So the another user can not run again.
    '
    ' ***************************************************************** '
    Private Function SetReleaseManualTransactionStarted(ByRef v_lIsStarted As Long) As Long
        Dim m_lReturn As Long


        SetReleaseManualTransactionStarted = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("IsStarted", v_lIsStarted, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            m_lReturn = .SQLSelect("spu_ACT_GetSetReleaseManualTransProcessFlag",
                "Set ReleaseManualTransProcessStarted Flag", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(sUsername:=m_sUsername,
                        iType:=gPMConstants.PMELogLevel.PMLogOnError,
                        sMsg:="Failed to call spu_ACT_GetSetReleaseManualTransProcessFlag",
                        vApp:=ACApp,
                        vClass:=ACClass,
                        vMethod:="SetReleaseManualTransactionStarted",
                        vErrNo:=Informations.Err.Number,
                        vErrDesc:=Informations.Err.Description)
            End If
        End With

        Exit Function

    End Function




    Private Function GetInsuranceFileCnt(ByVal vPremiumFinanceCnt As Integer,
                                        ByVal vPremiumFinanceVersion As Integer,
                                        ByRef iInsuranceFileCnt As Integer) As Integer

        Const kMethodName As String = "GetInsuranceFileCnt"
        Dim result As Integer = 0
        Dim vResultsArray(,) As Object = Nothing
        Dim lReturn As Long



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        lReturn& = m_oDatabase.Parameters.Add("premiumfinance_cnt", vPremiumFinanceCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "GetInsuranceFileCnt Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        lReturn& = m_oDatabase.Parameters.Add("premiumfinance_version", vPremiumFinanceVersion, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "GetInsuranceFileCnt Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Execute selection Query
        lReturn = m_oDatabase.SQLSelect(
                                sSQL:=ACGetInsuranceFileCntSQL,
                                sSQLName:=ACGetInsuranceFileCntName,
                                bStoredProcedure:=ACGetInsuranceFileCntStored,
                                vResultArray:=vResultsArray)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, ACGetInsuranceFileCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If Informations.IsArray(vResultsArray) Then
            iInsuranceFileCnt = ToSafeLong(vResultsArray(0, 0))
        End If

        Return result

    End Function
    ''' <summary>
    ''' GetAllocationbatch
    ''' </summary>
    ''' <param name="v_nAllocationId"></param>
    ''' <param name="r_nAllocationBatchId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetAllocationbatch(ByVal v_nAllocationId As Integer,
                                        ByRef r_nAllocationBatchId As Integer) As Integer

        Const kMethodName As String = "GetAllocationbatch"
        Dim oResult(,) As Object = Nothing
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add("allocation_id", v_nAllocationId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetAllocationbatch Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Execute selection Query
            m_lReturn = m_oDatabase.SQLSelect(
                                    sSQL:="spu_act_get_allocation_batch_by_allocation",
                                    sSQLName:="GetAllocationbatch",
                                    bStoredProcedure:=True,
                                    vResultArray:=oResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                RaiseError(kMethodName, "spu_act_get_allocation_batch_by_allocation Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Informations.IsArray(oResult) Then
                r_nAllocationBatchId = ToSafeLong(oResult(0, 0))
            End If

            Return nResult

        Catch excep As Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationbatch", vApp:=ACApp, vClass:=ACClass, vMethod:="ReleaseManualTransactions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try

    End Function
End Class
