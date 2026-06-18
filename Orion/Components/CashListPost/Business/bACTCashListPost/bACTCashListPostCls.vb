Option Strict Off
Option Explicit On
Imports System.Globalization
Imports System.Text
'developer guide no.129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Automated_NET.Automated")> _
Public NotInheritable Class Automated
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 13/05/1997
    '
    ' Description: Creatable Form class which contains all the
    '              methods, business rules required for the
    '              CashListPost Form.
    '
    ' Edit History:
    '
    ' RAW 12/03/2003 : ISS2893 : correct allocations
    ' RAW 01/04/2003 : ISS2854 : correct setting of cashlistitem.allocationstatus_id
    ' CJB 24/03/2004 : Folgate Development : Store MediaRef & CashListItem.Receipt_Details
    '                  in TransDetail.Comment (in PostUnallocatedCash)
    ' DD  26/03/2004 : GJW : Added passing through of Underwriting Year to Transaction
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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
    Private Const ACClass As String = "Automated"
    Public Const KSAMBDXCalling As Integer = -88

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    'EK 040200
    Private m_oS4BDatabase As dPMDAO.Database

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
    Private m_dtAccountingDate As Date
    ' Status members
    Private m_sProcessStatus As New StringsHelper.FixedLengthString(2)
    Private m_sMapStatus As New StringsHelper.FixedLengthString(2)
    Private m_sStepStatus As New StringsHelper.FixedLengthString(2)

    Private m_bAbortTrans As Boolean
    'developer guide no. 101
    Private m_vCashListId As Object
    'developer guide no. 101
    Private m_vCashListItemId As Object

    Private m_vCashListItemIds As Object

    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oCashListItem As Object = Nothing 'bACTCashlistitem.Form
    Private m_oCashList As bACTCashList.Form
    Private m_oCashListDrawer As bACTCashListDrawer.Business
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oPMAutoNumber As bACTAutoNumber.Business
    Private m_oMatchPost As bACTMatchPost.Form
    Private m_oChequeProduction As Object = Nothing ' bACTChequeProduction.Business
    Private m_oAllocationCreate As Object
    Private m_oAllocationDetail As bACTAllocationDetail.Form
    Private m_oTransDetail As bACTTransDetail.Form
    Private m_oAllocationManual As bACTAllocationManual.Business
    Private m_oCreditCard As bACTCreditCard.Business

    Private m_oPMLookUp As bPMLookup.Business

    Private m_lAllocationID As Integer
    Private m_sCommissionOption As String = ""
    Private m_lTransactionId As Integer
    Private m_sMap As String = ""
    Private m_oSystemOption As bSIROptions.Business
    Private m_oCommissionPost As Object
    Private m_lPMAuthorityLevel As Integer
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lCashTransDetailID As Integer
    Private m_bWithDID As Boolean
    Private m_bChequeProduction As Boolean

    Private m_sUnderwritingOrAgency As String = ""
    Private m_lCashListTransactionID As Integer

    Private m_oBankAccount As bACTBankAccount.Form
    Private m_iNoWriteOffRequired As Integer
    Private m_bViaInsurerPayment As Boolean
    Private m_sCashDocRef As String
    Private m_sDocumentRef As String

    Private m_nTaxBandID As Integer
    Private m_crTaxAmount As Double
    Private m_nInsuranceFileCnt As Integer
    Private m_nBatchID As Integer
    Private m_bIsInsurerPaymentRoadMap As Boolean
    Private m_sReceiptTypeCode As String


    Public ReadOnly Property DocumentRef() As String
        Get
            Return m_sDocumentRef
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

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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

    Public ReadOnly Property StepStatus() As String
        Get

            ' Standard Property.

            ' Return the Steps Status
            Return m_sStepStatus.Value

        End Get
    End Property

    Public Property AbortTrans() As Boolean
        Get

            Return m_bAbortTrans

        End Get
        Set(ByVal Value As Boolean)

            m_bAbortTrans = Value

        End Set
    End Property

    Public Property CashTransId() As Integer
        Get

            Return m_lCashTransDetailID

        End Get
        Set(ByVal Value As Integer)

            m_lCashTransDetailID = Value

        End Set
    End Property

    Public Property ReceiptTypeCode() As String
        Get

            Return m_sReceiptTypeCode

        End Get
        Set(ByVal Value As String)

            m_sReceiptTypeCode = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    'eck 060201
    Public Property ChequeProduction() As Boolean
        Get
            Return m_bChequeProduction
        End Get
        Set(ByVal Value As Boolean)
            m_bChequeProduction = Value
        End Set
    End Property


    Public WriteOnly Property CashListTransactionID() As Integer
        Set(ByVal Value As Integer)
            m_lCashListTransactionID = Value
        End Set
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)


    ' PUBLIC Methods (Begin)

    ''' <summary>
    ''' Entry point for any initialisation code for this object
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String,
                               ByVal sPassword As String,
                               ByVal iUserID As Integer,
                               ByVal iSourceID As Integer,
                               ByVal iLanguageID As Integer,
                               ByVal iCurrencyID As Integer,
                               ByVal iLogLevel As Integer,
                               ByVal sCallingAppName As String,
                               Optional ByVal bStandAlone As Boolean = False,
                               Optional ByVal vDatabase As Object = Nothing) As Long

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Try


            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel
            m_lStatus = PMEReturnCode.PMCancel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername,
                                                           m_iSourceID,
                                                           m_iLanguageID,
                                                           v_lPMProductFamily:=PMEProductFamily.pmePFOrion,
                                                           r_bNewInstanceCreated:=m_bCloseDatabase,
                                                           r_oCheckedDatabase:=m_oDatabase,
                                                           v_vDatabase:=vDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            m_bAbortTrans = True


            m_oCashList = New bACTCashList.Form
            m_lReturn = m_oCashList.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'm_oCashListItem = New bACTCashlistitem.Form

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oCashListItem, v_sClassName:="bACTCashlistitem.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bACTCashlistitem.Form"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bSIRRiskScreen.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return m_lReturn
            End If
            m_lReturn = m_oCashListItem.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(m_oDatabase, dPMDAO.Database))
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            m_oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oCashListDrawer = New bACTCashListDrawer.Business
            m_lReturn = m_oCashListDrawer.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oMatchPost = New bACTMatchPost.Form
            m_lReturn = m_oMatchPost.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            'Create the autonumber object using component services


            m_oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = m_oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If


            m_oCreditCard = New bACTCreditCard.Business
            m_lReturn = m_oCreditCard.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oBankAccount = New bACTBankAccount.Form
            m_lReturn = m_oBankAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oPMLookUp = New BPMLOOKUP.Business
            m_lReturn = m_oPMLookUp.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            End If


            m_lCurrentRecord = 0 ' Set Current Record to zero
            m_lProcessMode = PMEProcessMode.PMProcessModeGeneric ' Set the ProcessMode to Generic
            m_sTransactionType = PMTransactionTypeGeneric ' Set the Type Of Business to New Business
            m_dtEffectiveDate = DateTime.Now ' Set the Effective Date to NOW
            m_lReturn = GetOption(v_iOptionNumber:=16, r_sOptionValue:=m_sCommissionOption) ' Get the Commission Transfer settings
            If m_lReturn <> PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername,
                                      iType:=PMELogLevel.PMLogOnError,
                                      sMsg:="Failed to read system option for Commission Option assuming Insurer Settled.",
                                      vApp:=ACApp, vClass:=ACClass,
                                      vMethod:="Initialise",
                                      vErrNo:=Informations.Err().Number,
                                      vErrDesc:=Informations.Err().Description)
                m_sCommissionOption = "2"
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function


    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_oPMAutoNumber IsNot Nothing Then
                    m_oPMAutoNumber.Dispose()
                    m_oPMAutoNumber = Nothing
                End If
                If m_oCashList IsNot Nothing Then
                    m_oCashList.Dispose()
                    m_oCashList = Nothing
                End If
                If m_oCashListItem IsNot Nothing Then
                    m_oCashListItem.Dispose()
                    m_oCashListItem = Nothing
                End If
                If m_oCashListDrawer IsNot Nothing Then
                    m_oCashListDrawer.Dispose()
                    m_oCashListDrawer = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oMatchPost IsNot Nothing Then
                    m_oMatchPost.Dispose()
                    m_oMatchPost = Nothing
                End If
                If m_oChequeProduction IsNot Nothing Then
                    m_oChequeProduction.Dispose()
                    m_oChequeProduction = Nothing
                End If
                If m_oAllocationCreate IsNot Nothing Then
                    m_oAllocationCreate.Dispose()
                    m_oAllocationCreate = Nothing
                End If
                If m_oAllocationDetail IsNot Nothing Then
                    m_oAllocationDetail.Dispose()
                    m_oAllocationDetail = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If
                If m_oCreditCard IsNot Nothing Then
                    m_oCreditCard.Dispose()
                    m_oCreditCard = Nothing
                End If
                If m_oBankAccount IsNot Nothing Then
                    m_oBankAccount.Dispose()
                    m_oBankAccount = Nothing
                End If
                If m_oPMLookUp IsNot Nothing Then
                    m_oPMLookUp.Dispose()
                    m_oPMLookUp = Nothing
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
    ' Name: GetSummary
    '
    ' Description: Returns the summary information to Navigator
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Public Function GetSummary(ByRef vSummaryArray As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Dont return anything
            vSummaryArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSummaryFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSummary", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    'EK 100100 Retrieve Allocated currency amount
    ' ***************************************************************** '
    ' Name: GetAllocationTotal
    '
    ' Description: Gets the total of the allocation
    '
    ' ***************************************************************** '
    Private Function GetAllocationTotal(ByVal v_lAllocationId As Integer, ByRef r_cAllocationAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add("Allocation_id", CStr(v_lAllocationId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllocationTotalSQL, sSQLName:="Get Allocation Totals", bStoredProcedure:=True, vResultArray:=vArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vArray) Then

            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                r_cAllocationAmount = CDec(vArray(0, 0))
            Else
                r_cAllocationAmount = 0
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAllocationIDForCashListItem
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function GetAllocationIDForCashListItem(ByVal v_lCashListItemID As Integer, ByRef r_lAllocationID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT MAX(allocation_id) " &
               "FROM allocationdetail " &
                   "WHERE cashlistitem_id = " & CStr(v_lCashListItemID)

        ' Perform query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllocationIDForCLI", bStoredProcedure:=False, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then

            If gPMFunctions.ToSafeString(vResultArray(0, 0)) <> "" Then

                r_lAllocationID = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            Else
                r_lAllocationID = 0
            End If
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetMatchedTransDetailIDsForCashListItem
    '
    ' Description: Get the ID's of transactions matched by the cash
    '
    ' FSA Phase 3.2
    '
    ' ***************************************************************** '
    Public Function GetMatchedTransDetailIDsForCashListItem(ByVal v_lCashListItemID As Integer, ByVal v_lAllocationId As Integer, ByRef v_vMatchedTransDetailIds(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT ad.transdetail_id " &
                   "FROM allocationdetail ad " &
                   "WHERE ad.Allocation_id = " & CStr(v_lAllocationId) &
                   " AND ad.cashlistitem_Id = " & CStr(v_lCashListItemID)

            ' Perform query
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetMatchedTransDetailIDsForCashListItem", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=v_vMatchedTransDetailIds)

            If Not Informations.IsArray(v_vMatchedTransDetailIds) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMatchedTransDetailIDsForCashListItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMatchedTransDetailIDsForCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function





    ' ***************************************************************** '
    ' Name: GetMatchIdForAllocation
    '
    ' Description: For the Allocation Id gets the match group
    '
    ' eck110102 Created
    '
    ' ***************************************************************** '
    Private Function GetMatchIdForAllocation(ByVal v_lAllocationId As Integer, ByRef r_lMatchId As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT MAX(tm.match_id) " &
               "FROM allocation a ," &
               "allocationdetail ad," &
               "transmatch tm " &
               "WHERE a.Allocation_id = " & CStr(v_lAllocationId) &
               " AND ad.allocation_id = a.allocation_id" &
               " AND ad.transdetail_id = tm.transdetail_id"

        ' Perform query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetMatchIdForCLI", bStoredProcedure:=False, vResultArray:=vResultArray)

        If Informations.IsArray(vResultArray) Then

            If gPMFunctions.ToSafeString(vResultArray(0, 0)) <> "" Then

                r_lMatchId = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            Else
                r_lMatchId = 0
            End If
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetAccountDetails
    '
    ' Author : Kevin Grandison
    ' Date : 28/07/03
    ' Description: Get some Account details
    '
    '
    ' ***************************************************************** '
    Private Function GetAccountDetails(ByVal v_lAccountId As Integer, ByRef r_vAccountArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT Company_ID,Sub_Branch_ID from Account WHERE Account_ID = " &
               v_lAccountId

        ' Perform query
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAccountDetails", bStoredProcedure:=False, vResultArray:=r_vAccountArray)

        If Not Informations.IsArray(r_vAccountArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: CheckTransExists
    '
    ' Description: Checks if a transaction has already been created for
    '              this cashlist. Likely reason for this is a partial
    '              allocation.
    '
    ' History: 19/08/1999 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CheckTransExists(ByRef r_lTransDetailID As Integer, ByVal v_lCashListId As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Construct the SQL
        sSQL = "SELECT transdetail_id " &
               "FROM cashlistitem " &
               "WHERE cashlist_id = {cashlist_id}"

        ' Clear the database parameters
        m_oDatabase.Parameters.Clear()

        ' Add the cashlist_id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlist_id", vValue:=CStr(v_lCashListId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Perform the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetExistingTrans", bStoredProcedure:=False, vResultArray:=vResultArray)

        ' Grab the result, if any
        If Informations.IsArray(vResultArray) Then

            If gPMFunctions.ToSafeString(vResultArray(0, 0)) <> "" Then

                r_lTransDetailID = gPMFunctions.ToSafeInteger(vResultArray(0, 0))
            End If
        End If

        Return result

    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <returns></returns>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=Nothing,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=0,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByRef r_cBaseAmount As Decimal,
                                        ByVal v_vCashListItemID As Object) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function

    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_dTransactionDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByRef r_cBaseAmount As Decimal,
                                        ByVal v_vCashListItemID As Object,
                                        ByVal v_dTransactionDate As Date) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=v_dTransactionDate,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function

    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                   ByVal v_vCashListItemID As Object) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=0,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function

    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                   ByVal v_vCashListItemID As Object,
                                   ByVal v_dTransactionDate As Date) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=0,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=v_dTransactionDate,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function

    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="sFailureReason"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                         v_vCashListItemID:=Nothing,
                                         v_vBatchId:=Nothing,
                                         r_cBaseAmount:=0,
                                         sFailureReason:=sFailureReason,
                                         nTaxBandID:=0,
                                         crTaxAmount:=CDec(0),
                                         r_nTaxTransdetailID:=0,
                                         sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)

    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="sFailureReason"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String, ByVal sInsurence_Ref As String, ByVal v_dTransactionDate As Date) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                         v_vCashListItemID:=Nothing,
                                         v_vBatchId:=Nothing,
                                         r_cBaseAmount:=0,
                                         sFailureReason:=sFailureReason,
                                         nTaxBandID:=0,
                                         crTaxAmount:=CDec(0),
                                         r_nTaxTransdetailID:=0,
                                         sInsurence_Ref:=sInsurence_Ref,
                                     v_dTransactionDate:=v_dTransactionDate,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)

    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="sFailureReason"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String, ByVal sInsurence_Ref As String) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                         v_vCashListItemID:=Nothing,
                                         v_vBatchId:=Nothing,
                                         r_cBaseAmount:=0,
                                         sFailureReason:=sFailureReason,
                                         nTaxBandID:=0,
                                         crTaxAmount:=CDec(0),
                                         r_nTaxTransdetailID:=0,
                                         sInsurence_Ref:=sInsurence_Ref,
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)

    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="sFailureReason"></param>
    ''' <param name="sInsurence_Ref"></param>
    ''' <param name="dtTransactionDate"></param>
    ''' <param name="bThirdPartyOnly"></param>
    ''' <param name="sPlanRef"></param>
    ''' <param name="dInsAmount"></param>
    ''' <param name="dOutstandingAmount"></param>
    ''' <param name="nPremiumFinanceVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, ByRef sFailureReason As String, ByVal sInsurence_Ref As String, ByVal dtTransactionDate As Date, ByVal bThirdPartyOnly As Boolean, ByVal sPlanRef As String, ByVal dInsAmount As Decimal, ByRef dOutstandingAmount As Decimal, ByRef nPremiumFinanceVersion As Integer) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                         v_vCashListItemID:=Nothing,
                                         v_vBatchId:=Nothing,
                                         r_cBaseAmount:=0,
                                         sFailureReason:=sFailureReason,
                                         nTaxBandID:=0,
                                         crTaxAmount:=CDec(0),
                                         r_nTaxTransdetailID:=0,
                                         sInsurence_Ref:=sInsurence_Ref,
                                     v_dTransactionDate:=dtTransactionDate,
                                     v_bThirdPartyOnly:=bThirdPartyOnly,
                                     v_sPlanRef:=sPlanRef,
                                     v_dInsAmount:=dInsAmount,
                                     r_dOutstandingAmount:=dOutstandingAmount,
                                     r_nPremiumFinanceVersion:=nPremiumFinanceVersion)

    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByVal v_vCashListItemID As Object,
                                        ByVal v_vBatchId As Object) As Integer
        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=v_vBatchId,
                                     r_cBaseAmount:=0,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="sFailureReason"></param>
    ''' <param name="r_nTaxTransdetailID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                       ByRef sFailureReason As String,
                                       ByRef r_nTaxTransdetailID As Integer) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=Nothing,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=0,
                                     sFailureReason:=sFailureReason,
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=r_nTaxTransdetailID,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="sFailureReason"></param>
    ''' <param name="r_nTaxTransdetailID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                       ByVal v_vCashListItemID As Object,
                                       ByRef sFailureReason As String,
                                       ByRef r_nTaxTransdetailID As Integer) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=Nothing,
                                     r_cBaseAmount:=0,
                                     sFailureReason:=sFailureReason,
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=r_nTaxTransdetailID,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByVal v_vCashListItemID As Object,
                                        ByVal v_vBatchId As Object,
                                        ByRef r_cBaseAmount As Decimal) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=v_vBatchId,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function

    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="sFailureReason"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                       ByVal v_vCashListItemID As Object,
                                       ByVal v_vBatchId As Object,
                                       ByRef r_cBaseAmount As Decimal,
                                       ByRef sFailureReason As String) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=v_vBatchId,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:=sFailureReason,
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="sFailureReason"></param>
    ''' <param name="nTaxBandID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                       ByVal v_vCashListItemID As Object,
                                       ByVal v_vBatchId As Object,
                                       ByRef r_cBaseAmount As Decimal,
                                       ByRef sFailureReason As String,
                                       ByVal nTaxBandID As Integer) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=v_vBatchId,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:=sFailureReason,
                                     nTaxBandID:=nTaxBandID,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="sFailureReason"></param>
    ''' <param name="nTaxBandID"></param>
    ''' <param name="crTaxAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByVal v_vCashListItemID As Object,
                                        ByVal v_vBatchId As Object,
                                        ByRef r_cBaseAmount As Decimal,
                                        ByRef sFailureReason As String,
                                        ByVal nTaxBandID As Integer,
                                        ByVal crTaxAmount As Double) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=v_vBatchId,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:=sFailureReason,
                                     nTaxBandID:=nTaxBandID,
                                     crTaxAmount:=crTaxAmount,
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=Nothing,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function


    'Public Function PostUnallocatedCash(ByVal v_vCashListID As Object, _
    '                                ByVal v_vCashListItemID As Object, _
    '                                ByVal v_vBatchId As Object, _
    '                                ByRef r_cBaseAmount As Decimal, _
    '                                ByRef sFailureReason As String, _
    '                                ByVal nTaxBandID As Integer, _
    '                                ByVal crTaxAmount As Double, _
    '                                ByRef r_nTaxTransdetailID As Integer, _
    '                                ByVal sInsurence_Ref As String) As Integer
    '    Return PostUnallocatedCash(v_vCashListID:=v_vCashListID, _
    '                                 v_vCashListItemID:=v_vCashListItemID, _
    '                                 v_vBatchId:=v_vBatchId, _
    '                                 r_cBaseAmount:=r_cBaseAmount, _
    '                                 sFailureReason:=sFailureReason, _
    '                                 nTaxBandID:=nTaxBandID, _
    '                                 crTaxAmount:=crTaxAmount, _
    '                                 r_nTaxTransdetailID:=r_nTaxTransdetailID, _
    '                                 sInsurence_Ref:=sInsurence_Ref)

    'End Function
    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="v_dTransactionDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByVal v_vCashListItemID As Object,
                                        ByVal v_vBatchId As Object,
                                        ByRef r_cBaseAmount As Decimal,
                                        ByVal v_dTransactionDate As Date) As Integer

        Return PostUnallocatedCash(v_vCashListID:=v_vCashListID,
                                     v_vCashListItemID:=v_vCashListItemID,
                                     v_vBatchId:=v_vBatchId,
                                     r_cBaseAmount:=r_cBaseAmount,
                                     sFailureReason:="",
                                     nTaxBandID:=0,
                                     crTaxAmount:=CDec(0),
                                     r_nTaxTransdetailID:=0,
                                     sInsurence_Ref:="",
                                     v_dTransactionDate:=v_dTransactionDate,
                                     v_bThirdPartyOnly:=False,
                                     v_sPlanRef:="",
                                     v_dInsAmount:=0,
                                     r_dOutstandingAmount:=0,
                                     r_nPremiumFinanceVersion:=0)
    End Function

    ''' <summary>
    ''' PostUnallocatedCash
    ''' </summary>
    ''' <param name="v_vCashListID"></param>
    ''' <param name="v_vCashListItemID"></param>
    ''' <param name="v_vBatchId"></param>
    ''' <param name="r_cBaseAmount"></param>
    ''' <param name="sFailureReason"></param>
    ''' <param name="nTaxBandID"></param>
    ''' <param name="crTaxAmount"></param>
    ''' <param name="r_nTaxTransdetailID"></param>
    ''' <param name="sInsurence_Ref"></param>
    ''' <param name="v_dTransactionDate"></param>
    ''' <param name="v_bThirdPartyOnly"></param>
    ''' <param name="v_sPlanRef"></param>
    ''' <param name="v_dInsAmount"></param>
    ''' <param name="r_dOutstandingAmount"></param>
    ''' <param name="r_nPremiumFinanceVersion"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function PostUnallocatedCash(ByVal v_vCashListID As Object,
                                        ByVal v_vCashListItemID As Object,
                                        ByVal v_vBatchId As Object,
                                        ByRef r_cBaseAmount As Decimal,
                                        ByRef sFailureReason As String,
                                        ByVal nTaxBandID As Integer,
                                        ByVal crTaxAmount As Double,
                                        ByRef r_nTaxTransdetailID As Integer,
                                        ByVal sInsurence_Ref As String,
                                        ByVal v_dTransactionDate As Date,
                                        ByVal v_bThirdPartyOnly As Boolean,
                                        ByVal v_sPlanRef As String,
                                        ByVal v_dInsAmount As Decimal,
                                        ByRef r_dOutstandingAmount As Decimal,
                                        ByRef r_nPremiumFinanceVersion As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oCashList As Object = Nothing
        Dim oCreditOrDebit As gACTLibrary.ACTEAccountSign
        Dim crBaseAmount As Decimal = 0
        Dim crCurrencyAmount As Decimal = 0
        Dim odCurrencyBaseXRate As Object = Nothing
        Dim dtAccountingDate As Date
        Dim nCurrencyID As Integer = 0
        Dim nAccountID As Integer = 0
        Dim nDocumentID As Integer = 0
        Dim nDocumentType As Integer = 0
        Dim nTransDetailID As Integer = 0
        Dim nCompanyId As Integer = 0
        Dim nCustAccntCompanyID As Integer = 0
        Dim nDrawerSubBranchId As Integer = 0
        Dim nCustAccntSubBranchID As Integer = 0
        Dim oAccountResultArray As Object(,) = Nothing
        Dim nEuroCurrencyID As Integer = 0
        Dim crEuroAmount As Decimal = 0
        Dim odEuroBaseXrate As Object = Nothing
        Dim odEuroCcyXrate As Object = Nothing
        Dim odBaseAmountUnrounded As Object = Nothing
        Dim odCurrencyAmountUnrounded As Object = Nothing
        Dim sOurRef As String = ""
        Dim sTheirRef As String = ""
        Dim sMediaRef As String = ""
        Dim sDocumentRef As String = ""
        Dim sComment As New StringBuilder
        Dim nNumberRangeID As Integer = 0
        Dim nCashListItemIDOne As Integer = 0
        Dim sGroupCode As String
        Dim sRangeCode As String = ""
        Dim nMediaTypeID As Integer = 0
        Dim nCashListTypeID As Integer = 0
        Dim nBankAccountID As Integer = 0
        Dim nChequeTransDetailID As Integer = 0
        Dim sItemStatusCode As String = ""
        Dim nCollectionBankAccountID As Integer = 0
        Dim sPostingAsBatchOption As String = ""
        Dim oReceiptTransDetails(,) As Object = Nothing
        Dim oUnderwritingYearID As Object = Nothing
        Dim bIsPosted As Boolean = False
        Dim sBankSpare As String = ""
        Dim nCashListItemID As Integer = 0
        Dim nLeadAccountID As Integer = 0
        Dim nLeadTransDetailID As Integer = 0
        Dim dLeadBaseAmount As Double = 0
        Dim oSplitReceiptTrans(,) As Object = Nothing
        Dim oTPTrans(1, 0) As Object
        Dim nCount As Integer = 0
        Dim nDocumentSequence As Integer = 1
        Dim nTransdetailTypeID As Integer = 0
        Dim nNumber As Integer = 0
        Dim nPaymentAccountID As Integer
        Dim nTaxBandAccountId As Integer
        Dim dbaseAmount As Double
        Dim nTPTransdetailID As Integer
        Dim lFinancerAccountId As Long

        Const kStatusIssued As String = "ISS"
        Try

            If Informations.IsNothing(v_vCashListItemID) Then
                nResult = GetCashListDetails(v_lCashListId:=v_vCashListID)
            Else
                nResult = GetCashListDetails(v_lCashListId:=v_vCashListID,
                                             v_vCashListItemID:=v_vCashListItemID)
            End If

            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            If crTaxAmount > 0 Then
                m_crTaxAmount = crTaxAmount
            End If
            If nTaxBandID > 0 Then
                m_nTaxBandID = nTaxBandID
            End If

            sPostingAsBatchOption = "0"

            If m_oCashList.Details.Item(0).CashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                If bPMFunc.GetSystemOption(m_sUsername, m_sPassword,
                                           m_iUserID, m_iSourceID,
                                           m_iLanguageID, m_iCurrencyID,
                                           m_iLogLevel, m_sCallingAppName,
                                           82, sPostingAsBatchOption) <> PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername,
                                       iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to get system option for Post Receipt as Batch (82)",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="PostUnallocatedCash",
                                       vErrNo:=Informations.Err().Number,
                                       vErrDesc:=Informations.Err().Description)
                End If
            End If

            If sPostingAsBatchOption = "1" And m_oCashListItem.Details.Count > 0 Then
                ReDim oReceiptTransDetails(2, CInt(m_oCashListItem.Details.Count - 1))
                sBankSpare = "RECONCILED"
            Else
                sBankSpare = ""
            End If

            If m_oCashList.Details.Item(0).IsSplitReceipt = True Then

                For lItem As Integer = 1 To m_oCashListItem.Details.Count
                    If m_oCashListItem.Details.Item(ToSafeInteger(lItem)).IsLeadAccount = True Then
                        nLeadAccountID = m_oCashListItem.Details.Item(ToSafeInteger(lItem)).AccountID
                    Else
                        nCashListItemID = m_oCashListItem.Details.Item(ToSafeInteger(ToSafeInteger(lItem))).CashlistitemID
                    End If
                Next
                ReDim oSplitReceiptTrans(2, CInt(m_oCashListItem.Details.Count - 2))
            End If

            For lItem As Integer = 1 To m_oCashListItem.Details.Count
                oCashList = m_oCashList.Details.Item(0)

                If ((oCashList.CashListTypeID = gACTLibrary.ACTCashListTypePayments) Or
                    (oCashList.CashListTypeID = ACTCashListTypeClaimPayments)) Then
                    nResult = GetTransDetailTypeID("CASHPAY", nTransdetailTypeID)
                Else
                    nResult = GetTransDetailTypeID("CASHREC", nTransdetailTypeID)
                End If

                If nTransdetailTypeID = 0 Then
                    nTransdetailTypeID = 1
                End If

                nResult = GetPostingStatusForCashListItem(m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CashlistitemID, bIsPosted)

                If nResult <> PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername,
                                       iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to Get Posting Status For CashListItem",
                                       vApp:=ACApp, vClass:=ACClass,
                                       vMethod:="PostUnallocatedCash",
                                       vErrNo:=Informations.Err().Number,
                                       vErrDesc:=Informations.Err().Description)
                    Return PMEReturnCode.PMFalse
                End If

                'We are coming through FindTransaction and have clicked Split Receipt Button
                If m_oCashList.Details.Item(0).IsSplitReceipt = True And bIsPosted = True Then
                    m_oCashListItem.Details.Item(ToSafeInteger(lItem)).TransdetailID = 0
                    bIsPosted = False
                End If

                If v_bThirdPartyOnly = True Then
                    bIsPosted = False
                End If

                If Not bIsPosted Then

                    With oCashList
                        If oCashList.CashList_drawer_id <> 0 Then
                            nResult = m_oCashListDrawer.GetDetails(v_lCashlistDrawerId:=oCashList.CashList_drawer_id,
                                                                   r_vCollectionAccountId:=nCollectionBankAccountID,
                                                                   r_vCompanyId:=nCompanyId, r_vSubBranchID:=nDrawerSubBranchId)
                        Else
                            nCompanyId = .CompanyID
                            nDrawerSubBranchId = .SubBranchID
                        End If

                        nCashListTypeID = .CashListTypeID
                        If ToSafeDate(v_dTransactionDate) <> DateTime.MinValue Then
                            dtAccountingDate = v_dTransactionDate
                        Else
                            dtAccountingDate = Informations.DateAdd("w", GetDaysDelay(nCashListTypeID,
                                                                                     oCashList.BankAccountID,
                                                                                     m_oCashListItem.Details.Item(ToSafeInteger(lItem)).MediaTypeID), .Listdate)
                        End If
                        nCurrencyID = .CurrencyID

                        If (.CashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                            sItemStatusCode = ""
                            nResult = GetPaymentStatusCode(m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CashlistitemID,
                                                           m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CashListItem_Payment_Status_id,
                                                           sItemStatusCode)
                            If nResult <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If
                        End If

                    End With

                    If (((oCashList.CashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                         (oCashList.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) AndAlso
                     sItemStatusCode = kStatusIssued) OrElse
                  (oCashList.CashListTypeID = gACTLibrary.ACTCashListTypeReceipts AndAlso
                  (Not m_oCashListItem.Details.Item(ToSafeInteger(lItem)).IsInstalment OrElse
                      v_bThirdPartyOnly = True OrElse m_sReceiptTypeCode = "TPPF")) Then 'Sankar - PN 56851

                        With oCashList
                            If (.CashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse (.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                                nDocumentType = gACTLibrary.ACTDocTypePayment
                                oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit '   DEBIT SUPPLIER
                                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef23
                                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSpy

                            ElseIf .CashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                                nDocumentType = gACTLibrary.ACTDocTypeReceipt
                                oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT SUPPLIER
                                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22
                                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrp
                            Else
                                nDocumentType = gACTLibrary.ACTDocTypeCashCredit
                                oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT CUSTOMER
                                sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef6
                                sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCcr
                            End If

                            nResult = m_oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode,
                                                                     v_sRangeCode:=sRangeCode,
                                                                     r_lNumberRangeID:=nNumberRangeID)
                            If nResult <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFalse
                            End If

                            ' Update the cashlist status to show that it can't be deleted
                            .CashListStatusID = gACTLibrary.ACTCashListStatusOpened
                            .DatabaseStatus = PMEComponentAction.PMEdit ' To Force Update

                            ' Load each Item up as a transaction
                            If (.CashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse (.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                                nDocumentType = gACTLibrary.ACTDocTypePayment
                                oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit '   DEBIT SUPPLIER
                            ElseIf .CashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                                nDocumentType = gACTLibrary.ACTDocTypeReceipt
                                oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT SUPPLIER
                            Else
                                nDocumentType = gACTLibrary.ACTDocTypeCashCredit
                                oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT CUSTOMER
                            End If

                        End With

                        With m_oCashListItem.Details.Item(ToSafeInteger(lItem))
                            ' Generate the next number
                            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                            If (.TransdetailID = 0 OrElse v_bThirdPartyOnly = True) Then
                                If Not (m_oCashList.Details.Item(0).IsSplitReceipt = True AndAlso nDocumentID <> 0) Then

                                    nResult = m_oPMAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=sRangeCode,
                                                                                              v_iUserID:=m_iUserID,
                                                                                              v_iCompanyID:=nCompanyId,
                                                                                              r_sDocumentRef:=sDocumentRef,
                                                                                              v_lNumberRangeID:=nNumberRangeID)
                                    If nResult <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFalse
                                    End If


                                    If sDocumentRef.Trim() <> "" Then
                                        sDocumentRef = sRangeCode & sDocumentRef
                                    End If

                                    nResult = ValidateDocRef(sDocumentRef, nCompanyId)
                                    If nResult <> PMEReturnCode.PMTrue Then
                                        nResult = PMEReturnCode.PMFail
                                        sFailureReason = "Failed to create the Document Reference."
                                        Return nResult
                                    End If
                                    m_sDocumentRef = sDocumentRef

                                    If m_oDocumentPost Is Nothing Then












                                        m_oDocumentPost = New bACTDocumentPost.Form
                                        nResult = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                                        If nResult <> PMEReturnCode.PMTrue Then
                                            Return PMEReturnCode.PMFalse
                                        End If
                                    End If

                                    nResult = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=nDocumentType,
                                                                          v_sDocumentRef:=sDocumentRef,
                                                                          v_dtDocumentDate:=dtAccountingDate,
                                                                          v_sComment:="Cash",
                                                                          r_vDocumentID:=nDocumentID,
                                                                          r_vDocSourceID:=nCompanyId,
                                                                          v_vBatchID:=v_vBatchId,
                                                                          r_vSubBranchID:=nDrawerSubBranchId)
                                End If

                                If m_oCashList.Details.Item(0).IsSplitReceipt AndAlso .IsLeadAccount Then
                                    crCurrencyAmount = .SplitTotal
                                Else
                                    crCurrencyAmount = .Amount
                                End If

                                If v_bThirdPartyOnly Then
                                    crCurrencyAmount = v_dInsAmount
                                End If

                                nCashListItemIDOne = .CashlistitemID

                                nResult = GetBaseAmountFromCurrency(v_iCurrencyID:=nCurrencyID,
                                                                    v_iCompanyID:=nCompanyId,
                                                                    r_cBaseAmount:=crBaseAmount,
                                                                    v_cCurrencyAmount:=crCurrencyAmount,
                                                                    r_vdCurrencyBaseXRate:= .CurrencyBaseXrate,
                                                                    v_dtAccountingDate:=dtAccountingDate,
                                                                    r_lEuro:=nEuroCurrencyID,
                                                                    r_cEuroAmount:=crEuroAmount,
                                                                    r_vEuroCCyXrate:=odEuroCcyXrate,
                                                                    r_vEuroBaseXRate:=odEuroBaseXrate,
                                                                    r_vCCyAmountUnrounded:=odCurrencyAmountUnrounded,
                                                                    r_vBaseAmountUnrounded:=odBaseAmountUnrounded)
                                If nResult <> PMEReturnCode.PMTrue Then
                                    Return PMEReturnCode.PMFail
                                End If

                                ' The document sequence 1 is reserved for the bank line
                                sOurRef = .OurRef.Trim()
                                sTheirRef = .TheirRef.Trim()
                                sMediaRef = .MediaRef.Trim()
                                nAccountID = .AccountID
                                nMediaTypeID = .MediaTypeID
                                sInsurence_Ref = .InsuranceRef

                                If v_bThirdPartyOnly Then
                                    m_lReturn = GetFinancerAccountID(v_sPlanRef, lFinancerAccountId)
                                    nAccountID = lFinancerAccountId
                                    m_lReturn = GetPolicyTransDetail(v_sPlanRef, nTPTransdetailID, dbaseAmount, r_nPremiumFinanceVersion)
                                End If
                                If v_bThirdPartyOnly AndAlso sInsurence_Ref = "" Then
                                    sInsurence_Ref = v_sPlanRef
                                End If

                                If nDocumentType = gACTLibrary.ACTDocTypeReceipt Then
                                    nResult = GetAccountDetails(nAccountID, oAccountResultArray)
                                    If nResult <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFail
                                    End If
                                    nCustAccntCompanyID = CInt(oAccountResultArray(0, 0))
                                    nCustAccntSubBranchID = CInt(oAccountResultArray(1, 0))
                                End If

                                crBaseAmount = gACTLibrary.ACTSigned(crBaseAmount, oCreditOrDebit)
                                crCurrencyAmount = gACTLibrary.ACTSigned(crCurrencyAmount, oCreditOrDebit)
                                odBaseAmountUnrounded = gACTLibrary.ACTSigned(odBaseAmountUnrounded, oCreditOrDebit)
                                odCurrencyAmountUnrounded = gACTLibrary.ACTSigned(odCurrencyAmountUnrounded, oCreditOrDebit)
                                nTransDetailID = 0

                                'Return a copy of the Base Amount for the PostAllocatedCashListItem function
                                'this saves a lot of of duplicate code
                                r_cBaseAmount = crBaseAmount

                                ' Folgate Development - Store MediaRef & CashListItem.Receipt_Details in TransDetail.Comment
                                ' Note that v_vComment used to be passed sOurRef & "/" & sTheirRef & "/" & sMediaRef
                                sComment = New StringBuilder("")
                                ' if there's a media ref, prefix it in the comment field
                                If sMediaRef <> "" Then
                                    sComment = New StringBuilder(sMediaRef)
                                End If
                                ' if there's some receipt details

                                If .Receipt_Details.Trim() <> "" Then
                                    ' just put the receipt details in the comment field
                                    If sComment.ToString() = "" Then
                                        'sComment = .Receipt_Details.Trim()
                                        sComment.Append(.Receipt_Details.Trim())
                                    Else
                                        ' since the comment field already has a media ref in then put a dash separating
                                        ' this and append the receipt details in after

                                        sComment.Append(" - " & .Receipt_Details.Trim())
                                    End If
                                End If

                                oUnderwritingYearID = .UnderwritingYearID

                                'If crBaseAmount <> 0 And crCurrencyAmount <> 0 Then
                                nResult = m_oDocumentPost.AddTransaction(r_vTransDetailId:=nTransDetailID,
                                                                         v_vDocumentSequence:=nDocumentSequence,
                                                                         v_lAccountID:=nAccountID,
                                                                         v_iCurrencyID:=nCurrencyID,
                                                                         v_cAmount:=crBaseAmount,
                                                                         v_vBaseAmountUnrounded:=odBaseAmountUnrounded,
                                                                         v_cCurrencyAmount:=crCurrencyAmount,
                                                                         v_vCurrencyAmountUnrounded:=odCurrencyAmountUnrounded,
                                                                         v_vdCurrencyBaseXRate:= .CurrencyBaseXrate,
                                                                         v_vEuroCurrencyId:=nEuroCurrencyID,
                                                                         v_vEuroAmount:=crEuroAmount,
                                                                         v_vEuroBaseXRate:=odEuroBaseXrate,
                                                                         v_vEuroCcyXrate:=odEuroCcyXrate,
                                                                         v_vComment:=sComment.ToString(),
                                                                         v_vInsuranceRef:=sInsurence_Ref.ToString(),
                                                                         v_vAccountingDate:=dtAccountingDate,
                                                                         v_vSpare:=sMediaRef,
                                                                         v_vSubBranchId:=nCustAccntSubBranchID,
                                                                         v_vDocSourceID:=nCustAccntCompanyID,
                                                                         v_vUnderwritingYearID:=oUnderwritingYearID,
                                                                         v_vAccountBaseXrate:= .AccountBaseXrate,
                                                                         v_vAccountBaseDate:= .CurrencyBaseDate,
                                                                         v_vSystemBaseXrate:= .SystemBaseXrate,
                                                                         v_vSystemBaseDate:= .CurrencyBaseDate,
                                                                         v_vTransdetailTypeID:=nTransdetailTypeID)
                                If nResult <> PMEReturnCode.PMTrue Then
                                    Return PMEReturnCode.PMFail
                                End If
                                'end if

                                If gPMFunctions.ToSafeDate(dtAccountingDate) <> DateTime.Today Then
                                    m_dtAccountingDate = dtAccountingDate
                                Else
                                    m_dtAccountingDate = DateTime.Today
                                End If

                                .TransdetailID = nTransDetailID
                                If m_oCashList.Details.Item(0).IsSplitReceipt Then
                                    If .AccountID = nLeadAccountID Then
                                        nLeadTransDetailID = nTransDetailID
                                        dLeadBaseAmount = crBaseAmount
                                    End If
                                End If

                                If v_bThirdPartyOnly Then
                                    oTPTrans(0, 0) = nTransDetailID
                                    oTPTrans(1, 0) = crBaseAmount
                                End If

                                ' Show that this cash list item has been posted
                                .AllocationstatusID = gACTLibrary.ACTAllocationStatusPosted
                                nChequeTransDetailID = nTransDetailID
                                CashTransId = nTransDetailID
                                ' Force update to take place later
                                .DatabaseStatus = PMEComponentAction.PMEdit
                                'If the Receipt/Payment is on a Credit Card then collect the payment
                                If .MediaTypeIssuerID <> 0 AndAlso .CC_Auth_Code.Trim() <> "" AndAlso .CC_Transaction_Code.Trim() <> "" Then
                                    nResult = m_oCreditCard.CollectPayment(lCashListItemID:=nCashListItemIDOne)
                                    If nResult <> PMEReturnCode.PMTrue Then
                                        Return PMEReturnCode.PMFail
                                    End If
                                End If
                                nPaymentAccountID = nAccountID
                            End If
                        End With

                        ' Now the bank line to balance
                        ' Use the account from the cash list drawer (if there is one)

                        If oCashList.CashList_drawer_id <> 0 Then
                            nAccountID = nCollectionBankAccountID
                        Else
                            nResult = GetAccountIdForBank(r_lAccountId:=nAccountID,
                                                          v_lBankAccountId:=oCashList.BankAccountID)
                        End If
                        If nResult <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFail
                        End If

                        nBankAccountID = nAccountID
                        If m_oCashList.Details.Item(0).IsSplitReceipt Then
                            If m_oCashListItem.Details.Item(ToSafeInteger(lItem)).AccountID <> nLeadAccountID Then
                                nAccountID = nLeadAccountID
                            End If
                        End If

                        ' Reverse the sign for the Bank Side
                        If (oCashList.CashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                            (oCashList.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                            oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit ' CREDIT BANK/DEBIT SUPPLIER
                        Else
                            oCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit ' DEBIT BANK/CREDIT CUSTOMER
                        End If

                        ' Totals from the Document
                        crCurrencyAmount = m_oDocumentPost.TotalCurrency
                        crBaseAmount = m_oDocumentPost.TotalBase

                        ' Sequence 1 indicates that this is the Primary Trans
                        crBaseAmount = gACTLibrary.ACTSigned(crBaseAmount, oCreditOrDebit)
                        crCurrencyAmount = gACTLibrary.ACTSigned(crCurrencyAmount, oCreditOrDebit)

                        ' Change from vDocumentSequence:=1 to lItem
                        ' Change from lItem to 2

                        ' Store MediaRef & CashListItem.Receipt_Details in TransDetail.Comment
                        ' Note that v_vComment used to be passed ""

                        nDocumentSequence += 1
                        nResult = m_oDocumentPost.AddTransaction(v_lAccountID:=nAccountID,
                                                                 v_vDocumentSequence:=nDocumentSequence,
                                                                 v_iCurrencyID:=nCurrencyID,
                                                                 v_cAmount:=crBaseAmount,
                                                                 v_cCurrencyAmount:=crCurrencyAmount,
                                                                 v_vdCurrencyBaseXRate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseXrate,
                                                                 v_vComment:=sComment.ToString(),
                                                                 v_vInsuranceRef:=sInsurence_Ref.ToString(),
                                                                 r_vTransDetailId:=nTransDetailID,
                                                                 v_vAccountingDate:=dtAccountingDate,
                                                                 v_vSpare:=sBankSpare,
                                                                 v_vSubBranchId:=nDrawerSubBranchId,
                                                                 v_vDocSourceID:=nCompanyId,
                                                                 v_vUnderwritingYearID:=oUnderwritingYearID,
                                                                 v_vAccountBaseDate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseDate,
                                                                 v_vSystemBaseDate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseDate,
                                                                 v_vSystemBaseXrate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).SystemBaseXrate,
                                                                 v_vTransdetailTypeID:=nTransdetailTypeID)
                        If nResult <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFail
                        End If

                        If m_oCashList.Details.Item(0).IsSplitReceipt Then
                            If m_oCashListItem.Details.Item(ToSafeInteger(lItem)).AccountID <> nLeadAccountID Then
                                oSplitReceiptTrans(0, nCount) = nTransDetailID
                                oSplitReceiptTrans(1, nCount) = crBaseAmount
                                nCount += 1
                            End If
                        Else
                            nDocumentSequence = 1
                        End If

                        'DD 06/11/2003 - add the Bank transdetail to the array for
                        'the batch posting at the end
                        If sPostingAsBatchOption = "1" Then
                            oReceiptTransDetails(0, lItem - 1) = nTransDetailID
                            oReceiptTransDetails(1, lItem - 1) = crBaseAmount
                            oReceiptTransDetails(2, lItem - 1) = crCurrencyAmount
                        End If

                        ' Reset the totals
                        m_oDocumentPost.TotalBase = 0
                        m_oDocumentPost.TotalCurrency = 0

                        'Posting Details to Cheque Production File
                        If (ChequeProduction) AndAlso (nMediaTypeID = 1) AndAlso
                            ((nCashListTypeID = gACTLibrary.ACTCashListTypePayments) OrElse
                             (nCashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments)) Then

                            nResult = PostCheque(lTransDetailID:=nChequeTransDetailID, lBankID:=nBankAccountID)
                        End If
                    End If

                    If (m_nTaxBandID <> 0 AndAlso m_crTaxAmount <> 0) AndAlso
                        (sItemStatusCode = kStatusIssued OrElse nCashListTypeID = gACTLibrary.ACTCashListTypeReceipts) Then

                        'Call a SPU to add tax_calculation, document and Stats_Detail
                        ' Generate the next number
                        sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
                        sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeJn
                        m_lReturn = m_oPMAutoNumber.GenerateNumber(v_lNumberRangeID:=nNumberRangeID,
                                                                    v_iUserID:=m_iUserID,
                                                                    v_iCompanyID:=nCompanyId,
                                                                    r_lNumber:=nNumber)
                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            Return PMEReturnCode.PMFail
                        End If

                        sDocumentRef = FormatDocumentRef(sRangeCode, nNumber)
                        Dim nRIPartyCnt As Integer

                        m_lReturn = m_oCashListItem.GetPartyCntFromAccountID(ToSafeInteger(nPaymentAccountID), ToSafeInteger(nRIPartyCnt))

                        m_lReturn = AddReInsurerPaymentReciept(nCompanyId,
                                                               m_nTaxBandID,
                                                               nCurrencyID,
                                                               m_crTaxAmount,
                                                               m_nInsuranceFileCnt,
                                                               nRIPartyCnt,
                                                               sDocumentRef)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFail
                        End If
                        'Call Add Transaction for TAX

                        m_lReturn = GetTransDetailTypeID("CASHTAXPAY", nTransdetailTypeID)
                        If nTransdetailTypeID = 0 Then
                            nTransdetailTypeID = 1
                        End If

                        If oCashList.CashListTypeID = gACTLibrary.ACTCashListTypePayments Then
                            oCreditOrDebit = ACTEAccountSign.acteSignDebit
                        Else
                            oCreditOrDebit = ACTEAccountSign.acteSignCredit
                        End If

                        crCurrencyAmount = m_crTaxAmount
                        m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=nCurrencyID,
                                                              v_iCompanyID:=nCompanyId,
                                                              r_cBaseAmount:=crBaseAmount,
                                                              v_cCurrencyAmount:=crCurrencyAmount,
                                                              r_vdCurrencyBaseXRate:=0,
                                                              v_dtAccountingDate:=dtAccountingDate,
                                                              r_lEuro:=nEuroCurrencyID,
                                                              r_cEuroAmount:=crEuroAmount,
                                                              r_vEuroCCyXrate:=odEuroCcyXrate,
                                                              r_vEuroBaseXRate:=odEuroBaseXrate,
                                                              r_vCCyAmountUnrounded:=odCurrencyAmountUnrounded,
                                                              r_vBaseAmountUnrounded:=odBaseAmountUnrounded)

                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            Return PMEReturnCode.PMFail
                        End If
                        ' Totals from the Document
                        ' Sequence 1 indicates that this is the Primary Trans
                        crBaseAmount = ACTSigned(crBaseAmount, oCreditOrDebit)
                        crCurrencyAmount = ACTSigned(crCurrencyAmount, oCreditOrDebit)

                        ' Store MediaRef & CashListItem.Receipt_Details in TransDetail.Comment
                        ' Note that v_vComment used to be passed ""
                        sComment = New StringBuilder("Reinsurance Tax")
                        'Changes for getting account Code for tax band id
                        Dim sTaxBandCode As String = String.Empty
                        Dim sAccountCodeForTaxBand As String
                        m_lReturn = m_oPMLookUp.GetCodeFromID(v_sTableName:="tax_band",
                                                              v_lID:=m_nTaxBandID,
                                                              r_sCode:=sTaxBandCode)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFail
                        End If
                        sAccountCodeForTaxBand = Trim("NOTA" + sTaxBandCode)

                        'Check that the account code exists or not
                        m_lReturn = DoAccountExists(sShortCode:=sAccountCodeForTaxBand)
                        If m_lReturn = PMEReturnCode.PMNotFound Then
                            'if account does not exists then create a account
                            Dim lParentNodeId As Long

                            m_lReturn = GetParentNodeIdForTax(lParentNodeId)
                            Dim oImportSiriusTrans As Object = Nothing

                            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oImportSiriusTrans,
                                                                    v_sClassName:="bACTImportSiriusTrans.Business",
                                                                    v_sCallingAppName:=ACApp,
                                                                    v_sUsername:=m_sUsername,
                                                                    v_sPassword:=m_sPassword,
                                                                    v_iUserID:=m_iUserID,
                                                                    v_iSourceID:=m_iSourceID,
                                                                    v_iLanguageID:=m_iLanguageID,
                                                                    v_iCurrencyID:=m_iCurrencyID,
                                                                    v_iLogLevel:=m_iLogLevel,
                                                                    v_oDatabase:=m_oDatabase)
                            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                                Return PMEReturnCode.PMFalse
                            End If

                            m_lReturn = oImportSiriusTrans.DeriveAccountID(r_lAccountId:=ToSafeInteger(nTaxBandAccountId),
                                                                             v_lParentNodeId:=ToSafeInteger(lParentNodeId),
                                                                             v_sRelativeCode:=ToSafeString(sAccountCodeForTaxBand),
                                                                             v_iAccountType:=4,
                                                                             v_lLedgerId:=1)

                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFail
                            End If
                            oImportSiriusTrans.dispose()
                            oImportSiriusTrans = Nothing
                        End If

                        If nTaxBandAccountId = 0 Then
                            m_lReturn = GetAccountIdFormTaxBandId(nTaxBandId:=m_nTaxBandID,
                                                                v_rAccountId:=nTaxBandAccountId)
                            If m_lReturn <> PMEReturnCode.PMTrue Then
                                Return PMEReturnCode.PMFail
                            End If
                        End If
                        'Get Account Id from tax account code ie 'NOTA'+ selected tax band code and Pass all othere 'required parameters to AddTransaction function
                        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=nTaxBandAccountId,
                                                                   v_vDocumentSequence:=3,
                                                                   v_iCurrencyID:=nCurrencyID,
                                                                   v_cAmount:=crBaseAmount,
                                                                   v_cCurrencyAmount:=crCurrencyAmount,
                                                                   v_vdCurrencyBaseXRate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseXrate,
                                                                   v_vComment:=sComment,
                                                                   r_vTransDetailId:=nTransDetailID,
                                                                   v_vAccountingDate:=dtAccountingDate,
                                                                   v_vSpare:="TAX",
                                                                   v_vSubBranchId:=nDrawerSubBranchId,
                                                                   v_vDocSourceID:=nCompanyId,
                                                                   v_vUnderwritingYearID:=oUnderwritingYearID,
                                                                   v_vAccountBaseDate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseDate,
                                                                   v_vSystemBaseDate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseDate,
                                                                   v_vSystemBaseXrate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).SystemBaseXrate,
                                                                   v_vTransdetailTypeID:=nTransdetailTypeID)
                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            Return PMEReturnCode.PMFail
                        End If
                        ' Pass Bank Account Id for Credit
                        ' call AddTransaction function for a contra entry.
                        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=nPaymentAccountID,
                                                                   v_vDocumentSequence:=4,
                                                                   v_iCurrencyID:=nCurrencyID,
                                                                   v_cAmount:=(crBaseAmount * -1),
                                                                   v_cCurrencyAmount:=(crCurrencyAmount * -1),
                                                                   v_vdCurrencyBaseXRate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseXrate,
                                                                   v_vComment:=sComment,
                                                                   r_vTransDetailId:=nTransDetailID,
                                                                   v_vAccountingDate:=dtAccountingDate,
                                                                   v_vSpare:="TAX",
                                                                   v_vSubBranchId:=nDrawerSubBranchId,
                                                                   v_vDocSourceID:=nCompanyId,
                                                                   v_vUnderwritingYearID:=oUnderwritingYearID,
                                                                   v_vAccountBaseDate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseDate,
                                                                   v_vSystemBaseDate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).CurrencyBaseDate,
                                                                   v_vSystemBaseXrate:=m_oCashListItem.Details.Item(ToSafeInteger(lItem)).SystemBaseXrate,
                                                                   v_vTransdetailTypeID:=nTransdetailTypeID)

                        r_nTaxTransdetailID = nTransDetailID
                        If (m_lReturn <> PMEReturnCode.PMTrue) Then
                            Return PMEReturnCode.PMFail
                        End If
                    End If
                End If
            Next lItem
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            'DD 06/11/2003 - Pass through the array for the contra-posting in the Bank Account
            If sPostingAsBatchOption = "1" Then
                If PostReceiptBatchTotal(nAccountID,
                                         nCompanyId,
                                         nDrawerSubBranchId,
                                         nCurrencyID,
                                         odCurrencyBaseXRate,
                                         oReceiptTransDetails,
                                         oUnderwritingYearID) <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If
            End If

            If m_oCashList.Details.Item(0).IsSplitReceipt Then
                If AllocateSplitReceipt(nLeadAccountID,
                                        nLeadTransDetailID,
                                        oSplitReceiptTrans) <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If
            End If

            If v_bThirdPartyOnly Then
                If AllocateTPInstalment(lFinancerAccountId,
                                        nTPTransdetailID, oTPTrans) <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If
                m_lReturn = GetPlanTransactionOutstanding(v_nPlanTrancationId:=nTPTransdetailID, r_dOutstandingAmount:=r_dOutstandingAmount)
            End If

            ' End by writing the cashlist updates back
            nResult = m_oCashList.Update
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            nResult = m_oCashListItem.Update
            If nResult <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            m_oDocumentPost = Nothing
            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostUnallocatedCash", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    Public Function ValidateDocRef(ByRef sDocRef As String, ByRef iCompanyId As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Doc_ref", vValue:=sDocRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="Company_id", vValue:=CStr(iCompanyId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDOCRefSQL, sSQLName:="GetDocRef", bStoredProcedure:=False, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vResultArray) Then 'Ensures that there is duplicate Doc ref.
            result = gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result
    End Function


    Private Function PostReceiptBatchTotal(ByVal lAccountID As Integer, ByVal iCompanyId As Integer, ByVal lSubBranchID As Integer, ByVal iCurrencyID As Integer, ByVal vCurrencyRate As Object, ByVal vTransactions(,) As Object, ByVal vUnderwritingYearID As Object) As Integer
        Dim result As Integer = 0
        Dim vKeyArray(,) As Object
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PostReceiptBatchTotal
        ' PURPOSE: Posts a Batch total to an account
        ' AUTHOR: Danny Davis
        ' DATE: 06 November 2003, 10:23:58
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim lNumberRangeID As Integer
        Dim sDocumentRef As String = ""
        Dim cTotalBaseAmount, cTotalCurrencyAmount As Decimal

        Dim lDocumentID, lTransDetailID As Integer

        Dim oAllocationManual As bACTAllocationManual.Business
        Dim vAllocationTrans(0) As Object



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get the number range for a Journal

        m_lReturn = m_oPMAutoNumber.GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1, v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, r_lNumberRangeID:=lNumberRangeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Generate the next number
        'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

        m_lReturn = m_oPMAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=gACTLibrary.ACTAutoNumberRangeCodeJn, v_iUserID:=m_iUserID, v_iCompanyID:=iCompanyId, r_sDocumentRef:=sDocumentRef, v_lNumberRangeID:=lNumberRangeID)
        'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        If sDocumentRef.Trim() <> "" Then
            sDocumentRef = gACTLibrary.ACTAutoNumberRangeCodeJn & sDocumentRef
        End If

        'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
        ' Add the Document

        m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=gACTLibrary.ACTDocTypeJournal, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=m_dtAccountingDate, v_sComment:="", r_vDocumentID:=lDocumentID, r_vDocSourceID:=iCompanyId, v_sReason:="", r_vSubBranchID:=lSubBranchID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Loop through the array building the totals
        cTotalBaseAmount = 0
        cTotalCurrencyAmount = 0
        For lRow As Integer = 0 To vTransactions.GetUpperBound(1)

            cTotalBaseAmount += CDbl(vTransactions(1, lRow))

            cTotalCurrencyAmount += CDbl(vTransactions(2, lRow))
        Next lRow

        ' Post the Credit (remember the TransDetail for Allocation)

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lAccountID, v_vDocumentSequence:=1, v_iCurrencyID:=iCurrencyID, v_cAmount:=-Math.Abs(cTotalBaseAmount), v_cCurrencyAmount:=-Math.Abs(cTotalCurrencyAmount), v_vdCurrencyBaseXRate:=vCurrencyRate, v_vComment:="", r_vTransDetailId:=lTransDetailID, v_vAccountingDate:=m_dtAccountingDate, v_vSpare:="RECONCILED", v_vSubBranchId:=lSubBranchID, v_vDocSourceID:=iCompanyId, v_vUnderwritingYearID:=vUnderwritingYearID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Post the Debit (back to the same Account)
        ' Ignore the TransDetail for this item

        m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lAccountID, v_vDocumentSequence:=2, v_iCurrencyID:=iCurrencyID, v_cAmount:=Math.Abs(cTotalBaseAmount), v_cCurrencyAmount:=Math.Abs(cTotalCurrencyAmount), v_vdCurrencyBaseXRate:=vCurrencyRate, v_vComment:="", r_vTransDetailId:=0, v_vAccountingDate:=DateTime.Today, v_vSubBranchId:=lSubBranchID, v_vDocSourceID:=iCompanyId, v_vUnderwritingYearID:=vUnderwritingYearID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' and signal to DocumentPost that its ok to update

        m_lReturn = m_oDocumentPost.Commit()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If

        'Use the bACTAllocationManual component to do the allocation
        oAllocationManual = New bACTAllocationManual.Business
        If oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

            Return result
        End If

        'Allocate each of the receipt debits manually
        For lRow As Integer = 0 To vTransactions.GetUpperBound(1)



            vAllocationTrans(0) = CStr(vTransactions(0, lRow)) & "|" & CStr(vTransactions(1, lRow))

            'Set keys for the AllocationManual component
            vKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 3}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = lAccountID


            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(lTransDetailID) & "|" & CStr(-Math.Abs(CDbl(vTransactions(1, lRow))))

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationTrans

            'Perform the allocation
            With oAllocationManual

                If .SetKeys(vKeyArray:=vKeyArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                    Return result
                End If


                If .Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocation failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostReceiptBatchTotal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With
        Next lRow


        oAllocationManual.Dispose()
        oAllocationManual = Nothing

        Return result


    End Function

    Public Function GetHiddenOption(ByRef r_sResult As String) As Integer

        Dim result As Integer = 0
        Dim oDatabase As dPMDAO.Database = Nothing
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'open sirius database
            If gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_oDatabase:=oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            sSQL = "SELECT value FROM hidden_options WHERE branch_id = {branch_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND option_number = {option_number}"

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="branch_id", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.Parameters.Add(sName:="option_number", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Hidden Option", bStoredProcedure:=False, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                m_lReturn = oDatabase.CloseDatabase()
                oDatabase = Nothing
                Return result
            End If


            r_sResult = gPMFunctions.ToSafeString(vResultArray(0, 0)).ToUpper()

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function PostAllocatedCashListItem(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer) As Integer
        Return PostAllocatedCashListItem(lCashListID:=lCashListID, lCashListItemId:=lCashListItemId, lInsuranceFileCnt:=lInsuranceFileCnt, sDocumentRef:=sDocumentRef, lWriteOffReasonID:=lWriteOffReasonID, cWriteOffAmount:=cWriteOffAmount, bCurrencyWriteOff:=bCurrencyWriteOff, r_iAllocationStatus:=r_iAllocationStatus, bIsPosted:=False, cAmtTobePosted:=0, bSpecificCashListItemId:=False, lPostAccountId:=0)
    End Function

    Public Function PostAllocatedCashListItem(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer, ByVal bSpecificCashListItemId As Boolean) As Integer
        Return PostAllocatedCashListItem(lCashListID:=lCashListID, lCashListItemId:=lCashListItemId, lInsuranceFileCnt:=lInsuranceFileCnt, sDocumentRef:=sDocumentRef, lWriteOffReasonID:=lWriteOffReasonID, cWriteOffAmount:=cWriteOffAmount, bCurrencyWriteOff:=bCurrencyWriteOff, r_iAllocationStatus:=r_iAllocationStatus, bIsPosted:=False, cAmtTobePosted:=0, bSpecificCashListItemId:=bSpecificCashListItemId, lPostAccountId:=0)
    End Function

    Public Function PostAllocatedCashListItem(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer, ByVal bIsPosted As Boolean, ByVal cAmtTobePosted As Decimal, ByVal bSpecificCashListItemId As Boolean, ByVal lPostAccountId As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: PostAllocatedCashListItem
        ' PURPOSE: Post Cash List Item and allocate to the passed in debt/credit
        ' AUTHOR: Danny Davis
        ' DATE: 18 January 2005, 13:51:21
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim vMatchTrans As Object = Nothing
        Dim vKeys As Object = Nothing
        Dim cBaseAmount As Decimal
        Dim lRows, lMatchRow As Integer
        Dim cTotal, cAmountLeftToAllocate, cCrTotal, cDrTotal, cAvailableTotal, cRequiredTotal, cAmounttoAllocate As Decimal
        Dim lAccountID, lCashListItemDocumentID As Integer
        Dim sInsuranceRef As String = ""
        Dim lMainRow As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_iAllocationStatus = gACTLibrary.ACTAllocationStatusUnallocated

            'Post it to accounts
            If Not bIsPosted Then
                m_lReturn = PostUnallocatedCash(v_vCashListID:=lCashListID, v_vCashListItemID:=lCashListItemId, v_vBatchId:=Nothing, r_cBaseAmount:=cBaseAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, PostUnallocatedCash failed.")
                End If
            Else
                If Informations.IsNothing(lCashListItemId) Then
                    m_lReturn = GetCashListDetails(v_lCashListId:=lCashListID)
                Else
                    m_lReturn = GetCashListDetails(v_lCashListId:=lCashListID,
                            v_vCashListItemID:=lCashListItemId)
                End If
            End If
            If lPostAccountId = 0 Then

                lAccountID = m_oCashListItem.Details.Item(1).AccountID
            Else
                lAccountID = lPostAccountId
            End If

            'Now allocate it off
            m_lReturn = GetTransactionsForAllocatedCashListItem(lAccountID:=lAccountID, lInsuranceFileCnt:=lInsuranceFileCnt, sDocumentRef:=sDocumentRef, lTransDetailID:=m_lCashTransDetailID, r_vResultArray:=vResultArray, lCashListItemId:=If(bSpecificCashListItemId, lCashListItemId, 0), v_bUseDocumentRef:=True)

            'Skip if nothing to do
            If Informations.IsArray(vResultArray) Then

                If vResultArray.GetUpperBound(1) = 0 Then
                    'We only have one transaction so quit here
                    Return result
                End If

                m_lReturn = CreateAllocationManual()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, " + "CreateAllocationManual " &
                                               "failed.")
                End If

                If m_oTransDetail Is Nothing Then


                    m_oTransDetail = New bACTTransdetail.Form
                    m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'Calculate the total amount of the debt/credit
                'and work out the write-off line

                lRows = vResultArray.GetUpperBound(1)
                cTotal = 0
                cAmountLeftToAllocate = 0
                cCrTotal = 0
                cDrTotal = 0


                For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                    'Match the CashListItemId to set the main transaction

                    If gPMFunctions.ToSafeDouble(vResultArray(9, lRow)) = lCashListItemId Then

                        cTotal = gPMFunctions.ToSafeDecimal(vResultArray(1, lRow))
                        lCashListItemDocumentID = gPMFunctions.ToSafeLong(vResultArray(10, lRow), 0)
                        lMainRow = lRow
                    Else
                        'All other rows will have the InsuranceRef
                        sInsuranceRef = gPMFunctions.ToSafeString(vResultArray(11, lRow))
                    End If

                    'Sum up the total debits and total credits

                    If gPMFunctions.ToSafeDouble(vResultArray(1, lRow)) < 0 Then

                        cCrTotal += gPMFunctions.ToSafeDouble(vResultArray(1, lRow))
                    Else

                        cDrTotal += gPMFunctions.ToSafeDouble(vResultArray(1, lRow))
                    End If
                Next lRow

                'Set the total as cAvailableTotal which has matching sign with
                'the main transaction then set the other one as cRequiredTotal

                If Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lMainRow))) = Math.Sign(cCrTotal) Then
                    cAvailableTotal = cCrTotal
                    cRequiredTotal = cDrTotal
                Else
                    cAvailableTotal = cDrTotal
                    cRequiredTotal = cCrTotal
                End If

                If cWriteOffAmount <> 0 Then
                    ReDim vKeys(1, 6)
                Else
                    ReDim vKeys(1, 3)
                End If

                ReDim vMatchTrans(vResultArray.GetUpperBound(1) - 1)

                cAmountLeftToAllocate = Math.Abs(cAvailableTotal)

                ' AllocationTransID

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                If Math.Abs(cAvailableTotal) <= Math.Abs(cRequiredTotal) Then
                    'If the Available Total <= Required Total then the below
                    'code will do full / partial allocation
                    lMatchRow = 0

                    For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                        cAmounttoAllocate = 0
                        If lRow = lMainRow Then



                            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & (CStr(Math.Abs(cTotal) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))))
                        Else

                            If Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) = Math.Sign(cAvailableTotal) Then


                                vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & gPMFunctions.ToSafeString(vResultArray(1, lRow), "0")
                            Else
                                'If this is the last item allocate the remaining funds rather than the proportional amount
                                'If lRow <> UBound(vResultArray, 2) Then
                                cAmounttoAllocate = gPMFunctions.ToSafeCurrency(Math.Abs(cAvailableTotal) * (Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) / Math.Abs(cRequiredTotal)))
                                cAmounttoAllocate = Math.Round(cAmounttoAllocate, 2)
                                cAmountLeftToAllocate -= cAmounttoAllocate

                                vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & (CStr(Math.Abs(cAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))))
                                'Else
                                'This is the last item
                                '     vMatchTrans(lMatchRow) = vResultArray(0, lRow) & "|" & CStr(cAmountLeftToAllocate * Sgn(vResultArray(1, lRow)))
                                'End If
                            End If
                            lMatchRow += 1
                        End If
                    Next lRow
                ElseIf (m_sCallingAppName = "iPMUQuoteCollectionProcess") Or (m_sCallingAppName = "SiriusTransactionService") Then

                    For lRow As Integer = 0 To lRows
                        If lRow = lMainRow Then



                            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & (CStr(Math.Abs(cRequiredTotal) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))))
                        Else

                            If Math.Abs(cAmountLeftToAllocate) <= Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) Then


                                vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & CStr(cAmountLeftToAllocate)
                                cAmountLeftToAllocate = 0
                            Else


                                vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & gPMFunctions.ToSafeString(vResultArray(1, lRow), "0")
                                cAmountLeftToAllocate -= gPMFunctions.ToSafeCurrency(vResultArray(1, lRow), 0)
                            End If

                            lMatchRow += 1
                        End If
                    Next lRow
                Else
                    'If the Available Total >Required Total then we need to take
                    'another approach e.g. to saturate the lowest amount first
                End If

                If Math.Abs(cAvailableTotal) <> Math.Abs(cRequiredTotal) Then
                    r_iAllocationStatus = gACTLibrary.ACTAllocationStatusPartial
                Else
                    r_iAllocationStatus = gACTLibrary.ACTAllocationStatusAllocated
                End If

                ' AccountID

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID


                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vResultArray(6, 0)

                ' CashListItemID

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans

                ' Pass cash list item Id

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCashListItemId

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = lCashListItemId
                'vKeys(PMKeyName, 3) = ACTKeyNameTransDetailId
                'vKeys(PMKeyValue, 3) = CashTransId

                If cWriteOffAmount <> 0 Then
                    ' Write Off Reason

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameWriteOffReasonId

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = lWriteOffReasonID

                    'WriteOff difference

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameWriteOffAmount

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = If(bCurrencyWriteOff, 0, cWriteOffAmount)

                    'Currency difference

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCurrencyDifference

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = If(bCurrencyWriteOff, cWriteOffAmount, 0)
                End If


                m_lReturn = m_oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oAllocationManual.SetProcessModes failed.")
                End If

                ' Set the keys

                m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=vKeys)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, " + "m_oAllocationManual.SetKeys " &
                                               "failed.")
                End If

                ' Start it


                m_oAllocationManual.CompanyId = vResultArray(7, 0)

                m_lReturn = m_oAllocationManual.Start()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, " + "m_oAllocationManual.Start " &
                                               "failed.")
                End If

                ' Terminate

                m_oAllocationManual.Dispose()
                m_oAllocationManual = Nothing

                'Get the TransDetail rows for the CashListItem

                m_lReturn = m_oTransDetail.GetDetails(vDocumentID:=lCashListItemDocumentID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oTransDetail.GetDetails Failed.")
                End If


                For lRow As Integer = 1 To m_oTransDetail.Details.Count
                    If sInsuranceRef.Length > 0 Then
                        'Update the InsuranceRef of TransDetail

                        m_lReturn = m_oTransDetail.EditUpdate(lRow, vInsuranceRef:=sInsuranceRef)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oTransDetail.EditUpdate Failed.")
                        End If
                    End If
                Next lRow

                'Save changes to Database

                m_lReturn = m_oTransDetail.Update
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oTransDetail.Update Failed.")
                End If

                'Now link the insurance file to the newly allocated receipt/payment
                'any related old unposted cashlistitem will be deleted
                With m_oDatabase
                    .Parameters.Clear()
                    .Parameters.Add("insurance_file_cnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("cashlistitem_id", CStr(lCashListItemId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    'Developer Guide No. 39
                    m_lReturn = .SQLAction("spu_ACT_Update_PolicyCashListItem", "Update_PolicyCashListItem", True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, spu_ACT_Update_PolicyCashListItem failed.")
                    End If
                End With
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PostAllocatedCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function


    ''' <summary>
    ''' Post Cash List Item and allocate to the passed in debt/credit
    ''' </summary>
    ''' <param name="lCashListID"></param>
    ''' <param name="lCashListItemId"></param>
    ''' <param name="lInsuranceFileCnt"></param>
    ''' <param name="sDocumentRef"></param>
    ''' <param name="lWriteOffReasonID"></param>
    ''' <param name="cWriteOffAmount"></param>
    ''' <param name="bCurrencyWriteOff"></param>
    ''' <param name="r_iAllocationStatus"></param>
    ''' <param name="bIsPosted"></param>
    ''' <param name="cAmtTobePosted"></param>
    ''' <param name="bSpecificCashListItemId"></param>
    ''' <param name="lPostAccountId"></param>
    ''' <returns></returns>
    Public Function PostAllocatedCashListItemSAM(ByVal lCashListID As Integer, ByVal lCashListItemId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal sDocumentRef As String, ByRef lWriteOffReasonID As Integer, ByRef cWriteOffAmount As Decimal, ByRef bCurrencyWriteOff As Boolean, ByRef r_iAllocationStatus As Integer, ByVal bIsPosted As Boolean, ByVal cAmtTobePosted As Decimal, ByVal bSpecificCashListItemId As Boolean, ByVal lPostAccountId As Integer) As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim vMatchTrans As Object = Nothing
        Dim vKeys As Object = Nothing
        Dim cBaseAmount As Decimal
        Dim lRows, lMatchRow As Integer
        Dim cTotal, cAmountLeftToAllocate, cCrTotal, cDrTotal, cAvailableTotal, cRequiredTotal, cAmounttoAllocate As Decimal
        Dim lAccountID, lCashListItemDocumentID As Integer
        Dim sInsuranceRef As String = ""
        Dim lMainRow As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_iAllocationStatus = gACTLibrary.ACTAllocationStatusUnallocated

            'Post it to accounts
            If Not bIsPosted Then
                m_lReturn = PostUnallocatedCash(v_vCashListID:=lCashListID, v_vCashListItemID:=lCashListItemId, v_vBatchId:=Nothing, r_cBaseAmount:=cBaseAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, PostUnallocatedCash failed.")
                End If
            End If
            If lPostAccountId = 0 Then

                lAccountID = m_oCashListItem.Details.Item(1).AccountID
            Else
                lAccountID = lPostAccountId
            End If

            'Now allocate it off
            m_lReturn = GetTransactionsForAllocatedCashListItem(lAccountID:=lAccountID, lInsuranceFileCnt:=lInsuranceFileCnt, sDocumentRef:=sDocumentRef, lTransDetailID:=m_lCashTransDetailID, r_vResultArray:=vResultArray, lCashListItemId:=If(bSpecificCashListItemId, lCashListItemId, 0), v_bUseDocumentRef:=True)

            'Skip if nothing to do
            If Informations.IsArray(vResultArray) Then

                If vResultArray.GetUpperBound(1) = 0 Then
                    'We only have one transaction so quit here
                    Return result
                End If

                m_lReturn = CreateAllocationManual()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, " + "CreateAllocationManual " &
                                               "failed.")
                End If

                If m_oTransDetail Is Nothing Then


                    m_oTransDetail = New bACTTransdetail.Form
                    m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

                'Calculate the total amount of the debt/credit
                'and work out the write-off line

                lRows = vResultArray.GetUpperBound(1)
                cTotal = 0
                cAmountLeftToAllocate = 0
                cCrTotal = 0
                cDrTotal = 0

                Dim lGrossRow As Integer

                For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                    'Match the CashListItemId to set the main transaction

                    If gPMFunctions.ToSafeDouble(vResultArray(9, lRow)) = lCashListItemId Then

                        cTotal = gPMFunctions.ToSafeDecimal(vResultArray(1, lRow))
                        lCashListItemDocumentID = gPMFunctions.ToSafeLong(vResultArray(10, lRow), 0)
                        lGrossRow = lRow
                    Else
                        'All other rows will have the InsuranceRef
                        sInsuranceRef = gPMFunctions.ToSafeString(vResultArray(11, lRow))
                    End If

                    If gPMFunctions.ToSafeString(vResultArray(8, lRow)).Trim() = "GROSS" Then
                        lGrossRow = lRow
                    End If
                    'Sum up the total debits and total credits

                    If gPMFunctions.ToSafeDouble(vResultArray(1, lRow)) < 0 Then

                        cCrTotal += gPMFunctions.ToSafeDouble(vResultArray(1, lRow))
                    Else

                        cDrTotal += gPMFunctions.ToSafeDouble(vResultArray(1, lRow))
                    End If
                Next lRow

                'Set the total as cAvailableTotal which has matching sign with
                'the main transaction then set the other one as cRequiredTotal

                If Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lMainRow))) = Math.Sign(cCrTotal) Then
                    cAvailableTotal = cCrTotal
                    cRequiredTotal = cDrTotal
                Else
                    cAvailableTotal = cDrTotal
                    cRequiredTotal = cCrTotal
                End If

                If cWriteOffAmount <> 0 Then
                    ReDim vKeys(1, 6)
                Else
                    ReDim vKeys(1, 3)
                End If

                If cWriteOffAmount > 0 AndAlso lWriteOffReasonID.Equals(KSAMBDXCalling) Then
                    cAvailableTotal = cAvailableTotal - cWriteOffAmount
                End If

                cAmountLeftToAllocate = Math.Abs(cAvailableTotal)

                ' AllocationTransID

                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID

                    'First try to adjust the amount in GROSS
                    Dim cTotalToAdjust As Double = Math.Abs(cAvailableTotal)

                    If Math.Abs(cAvailableTotal) <= Math.Abs(cRequiredTotal) Then
                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(vResultArray(0, lMainRow)) & "|" & (CStr(Math.Abs(cTotal) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lMainRow)))))

                        lMatchRow = 0

                        For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                            cAmounttoAllocate = 0

                            If lRow = lGrossRow Then

                                If Informations.IsArray(vMatchTrans) Then
                                    ReDim Preserve vMatchTrans(lMatchRow)
                                Else
                                    ReDim vMatchTrans(0)
                                End If

                                If Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) = Math.Sign(cAvailableTotal) Then

                                    vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & gPMFunctions.ToSafeString(vResultArray(1, lRow), "0")
                                Else
                                    Dim dAmtAdjusted As Double = Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))

                                    If Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) >= Math.Abs(cTotalToAdjust) Then
                                        dAmtAdjusted = Math.Abs(cTotalToAdjust)
                                    ElseIf Math.Abs(cAvailableTotal) = Math.Abs(cRequiredTotal) Then
                                        dAmtAdjusted = Math.Abs(cAvailableTotal)
                                    End If

                                    cAmounttoAllocate = dAmtAdjusted * (Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) / Math.Abs(cRequiredTotal))
                                    cTotalToAdjust = cTotalToAdjust - Math.Abs(cAmounttoAllocate)
                                    cAmounttoAllocate = Math.Round(cAmounttoAllocate, 2)
                                    cAmountLeftToAllocate -= cAmounttoAllocate

                                    vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & (CStr(Math.Abs(cAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))))

                                End If

                                lMatchRow += 1

                                Exit For
                            End If
                        Next lRow


                        For lRow As Integer = vResultArray.GetLowerBound(1) To vResultArray.GetUpperBound(1)
                            cAmounttoAllocate = 0

                            If lRow <> lMainRow AndAlso lRow <> lGrossRow AndAlso cTotalToAdjust > 0 Then

                                Dim dAmtAdjusted As Double = 0

                                If Informations.IsArray(vMatchTrans) Then
                                    ReDim Preserve vMatchTrans(lMatchRow)
                                Else
                                    ReDim vMatchTrans(0)
                                End If

                                If Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) = Math.Sign(cAvailableTotal) Then

                                    vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & gPMFunctions.ToSafeString(vResultArray(1, lRow), "0")
                                Else

                                    cAmounttoAllocate = cAvailableTotal * (Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) / Math.Abs(cRequiredTotal))
                                    dAmtAdjusted = Math.Abs(cAmounttoAllocate)
                                    cAmounttoAllocate = Math.Round(cAmounttoAllocate, 2)
                                    cAmountLeftToAllocate -= cAmounttoAllocate

                                    vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & (CStr(Math.Abs(cAmounttoAllocate) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))))

                                    If Math.Abs(dAmtAdjusted) >= Math.Abs(cTotalToAdjust) Then
                                        dAmtAdjusted = cTotalToAdjust
                                        cTotalToAdjust = 0
                                    Else
                                        cTotalToAdjust = cTotalToAdjust - dAmtAdjusted
                                    End If
                                End If

                                lMatchRow += 1
                            End If
                        Next lRow



                    ElseIf (m_sCallingAppName = "iPMUQuoteCollectionProcess") Or (m_sCallingAppName = "SiriusTransactionService") Then
                        Dim cAllocatedTotal As Decimal = 0
                        For lRow As Integer = 0 To lRows
                            If lRow = lMainRow Then

                            Else

                                If Informations.IsArray(vMatchTrans) Then
                                    ReDim Preserve vMatchTrans(lMatchRow)
                                Else
                                    ReDim vMatchTrans(0)
                                End If

                                If Math.Abs(cAmountLeftToAllocate) <= Math.Abs(gPMFunctions.ToSafeDouble(vResultArray(1, lRow))) Then

                                    vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & CStr(cAmountLeftToAllocate)
                                    cAllocatedTotal += cAmountLeftToAllocate
                                    cAmountLeftToAllocate = 0
                                Else

                                    vMatchTrans(lMatchRow) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & gPMFunctions.ToSafeString(vResultArray(1, lRow), "0")
                                    cAllocatedTotal += gPMFunctions.ToSafeCurrency(vResultArray(1, lRow), 0)
                                    cAmountLeftToAllocate -= gPMFunctions.ToSafeCurrency(vResultArray(1, lRow), 0)
                                End If

                                lMatchRow += 1
                            End If
                        Next lRow
                        For lRow As Integer = 0 To lRows
                            If lRow = lMainRow Then
                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = gPMFunctions.ToSafeString(vResultArray(0, lRow)) & "|" & (CStr(Math.Abs(cAllocatedTotal) * Math.Sign(gPMFunctions.ToSafeDouble(vResultArray(1, lRow)))))
                            End If
                        Next lRow
                    Else
                        'If the Available Total >Required Total then we need to take
                        'another approach e.g. to saturate the lowest amount first
                    End If

                    If Math.Abs(cAvailableTotal) <> Math.Abs(cRequiredTotal) Then
                        r_iAllocationStatus = gACTLibrary.ACTAllocationStatusPartial
                    Else
                        r_iAllocationStatus = gACTLibrary.ACTAllocationStatusAllocated
                    End If

                    ' AccountID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID


                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = vResultArray(6, 0)

                    ' CashListItemID

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs


                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans

                    ' Pass cash list item Id

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 3) = PMNavKeyConst.ACTKeyNameCashListItemId

                    vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 3) = lCashListItemId


                    If cWriteOffAmount <> 0 Then
                        ' Write Off Reason

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 4) = PMNavKeyConst.ACTKeyNameWriteOffReasonId

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 4) = lWriteOffReasonID

                        'WriteOff difference

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 5) = PMNavKeyConst.ACTKeyNameWriteOffAmount

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 5) = If(bCurrencyWriteOff, 0, cWriteOffAmount)

                        'Currency difference

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 6) = PMNavKeyConst.ACTKeyNameCurrencyDifference

                        vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 6) = If(bCurrencyWriteOff, cWriteOffAmount, 0)
                    End If


                    m_lReturn = m_oAllocationManual.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oAllocationManual.SetProcessModes failed.")
                    End If

                    ' Set the keys

                    m_lReturn = m_oAllocationManual.SetKeys(vKeyArray:=vKeys)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, " + "m_oAllocationManual.SetKeys " &
                                               "failed.")
                    End If

                    ' Start it


                    m_oAllocationManual.CompanyId = vResultArray(7, 0)

                    m_lReturn = m_oAllocationManual.Start()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, " + "m_oAllocationManual.Start " &
                                               "failed.")
                    End If

                    ' Terminate

                    m_oAllocationManual.Dispose()
                    m_oAllocationManual = Nothing

                    'Get the TransDetail rows for the CashListItem

                    m_lReturn = m_oTransDetail.GetDetails(vDocumentID:=lCashListItemDocumentID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oTransDetail.GetDetails Failed.")
                    End If


                    For lRow As Integer = 1 To m_oTransDetail.Details.Count
                        If sInsuranceRef.Length > 0 Then
                            'Update the InsuranceRef of TransDetail

                            m_lReturn = m_oTransDetail.EditUpdate(lRow, vInsuranceRef:=sInsuranceRef)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oTransDetail.EditUpdate Failed.")
                            End If
                        End If
                    Next lRow

                    'Save changes to Database

                    m_lReturn = m_oTransDetail.Update
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, m_oTransDetail.Update Failed.")
                    End If

                    'Now link the insurance file to the newly allocated receipt/payment
                    'any related old unposted cashlistitem will be deleted
                    With m_oDatabase
                        .Parameters.Clear()
                        .Parameters.Add("insurance_file_cnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        .Parameters.Add("cashlistitem_id", CStr(lCashListItemId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        'Developer Guide No. 39
                        m_lReturn = .SQLAction("spu_ACT_Update_PolicyCashListItem", "Update_PolicyCashListItem", True)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New System.Exception(m_lReturn.ToString() + ", PostAllocatedCashListItem, spu_ACT_Update_PolicyCashListItem failed.")
                        End If
                    End With
                End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="PostAllocatedCashListItemSAM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function

    'Start - Sankar - Bank Guarantee Bug Fixing
    Public Function UpdateBGAvailableBalance(ByVal lBGID As Integer, ByVal lReceiptAmt As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateBGAvailableBalance"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("BG_ID", CStr(lBGID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("Available_Bal", CStr(lReceiptAmt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                m_lReturn = .SQLAction(sSQL:=ACUpdateBGAvailableLimitSQL, sSQLName:=ACUpdateBGAvailableLimitName, bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, ACUpdateBGAvailableLimitSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End With

            Return result

        Catch ex As System.Exception


            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here



            Return result
        End Try
    End Function
    'End - Sankar - Bank Guarantee Bug Fixing

    ' ***************************************************************** '
    ' Name: PostAllocatedCash (Public)
    '
    ' Description: Having posted cash an allocated to this method
    '              will write the transmatch and allocation details
    '
    ' ***************************************************************** '
    Public Function PostAllocatedCash(ByVal v_vCashListID As Object) As Integer
        Return PostAllocatedCash(v_vCashListID:=v_vCashListID, v_vCashListItemID:=Nothing)
    End Function

    Public Function PostAllocatedCash(ByVal v_vCashListID As Object, ByVal v_vCashListItemID As Object) As Integer

        Dim result As Integer = 0
        Dim oCashList As Object
        Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign
        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim dtAccountingDate As Date
        Dim iCurrencyID As Integer
        Dim lAccountID, lTransDetailID As Integer
        'eck100500
        Dim iCompanyId As Integer

        'EK 100100 Extra propertiesfor Euro
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroBaseXrate As Object = Nothing
        Dim vdEuroCcyXrate As Object = Nothing
        Dim vdBaseAmountUnrounded As Double
        Dim vdCurrencyAmountUnrounded As Double
        '

        Dim sOurRef, sTheirRef, sMediaRef As String

        Dim sDocumentRef As String = ""

        Dim lMatchID As Integer

        Dim lCashListItemId, lAllocationID, lAllocationDetailId As Integer

        Dim cAllocationAmount, cAllocationCCyAmount As Decimal

        Dim sAccountType As String = ""
        Dim vTransIDs(,) As Object = Nothing

        Dim vPlans(,) As Object = Nothing
        Dim cCurrencyWriteOff As Decimal

        Dim cWriteOffAmount As Decimal
        Dim vWriteOffReasonId As Object = Nothing
        Dim bCurrencyDiff As Boolean

        Dim iBaseCurrencyID As Integer

        Dim vMatchedTransdetailIds(,) As Object = Nothing

        'DC150806
        Dim vDocDate As Date
        Dim nTaxAllocationDetailId As Long
        Dim nTaxTransDetailId As Long
        Dim crTaxAllocationAmount As Double
        Dim crTaxAllocationCCyAmount As Double

        Try

            result = PMEReturnCode.PMTrue
            ' Load up the collections with the cashlists
            bCurrencyDiff = False 'initialise

            m_lReturn = GetCashListDetails(v_lCashListId:=CInt(v_vCashListID), v_vCashListItemID:=v_vCashListItemID)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            oCashList = m_oCashList.Details.Item(0)

            ' Use CashList & CashListItems to Populate Document & Transactions
            With oCashList

                iCurrencyID = .CurrencyID
                iCompanyId = .CompanyID
                dtAccountingDate = .Listdate
                ' Update the cashlist status to show that it can't be deleted
                .CashListStatusID = gACTLibrary.ACTCashListStatusOpened
                If (.CashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit '   DEBIT SUPPLIER
                ElseIf .CashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT SUPPLIER
                Else
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT CUSTOMER
                End If
                .DatabaseStatus = PMEComponentAction.PMEdit ' To Force Update
            End With

            ' Load each Item up as a transaction

            For lItem As Integer = 1 To m_oCashListItem.Details.Count

                With m_oCashListItem.Details.Item(ToSafeInteger(lItem))
                    cCurrencyAmount = .Amount
                    crTaxAllocationCCyAmount = ToSafeDouble(.TaxAmount)
                    lCashListItemId = .CashlistitemID
                    lTransDetailID = .TransdetailID
                    sOurRef = .OurRef.Trim()
                    sTheirRef = .TheirRef.Trim()
                    sMediaRef = .MediaRef.Trim()
                    lAccountID = .AccountID
                    vdCurrencyBaseXRate = .CurrencyBaseXrate

                    'Get Allocation for cash list item
                    m_lReturn = GetAllocationIDForCashListItem(v_lCashListItemID:=lCashListItemId,
                                                               r_lAllocationID:=lAllocationID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If
                    'FSA Phase 3.2
                    'Get Allocation for cash list item
                    m_lReturn = GetMatchedTransDetailIDsForCashListItem(v_lCashListItemID:=lCashListItemId,
                                                                        v_lAllocationId:=lAllocationID,
                                                                        v_vMatchedTransDetailIds:=vMatchedTransdetailIds)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'Get Match Group for cash list item
                    m_lReturn = GetMatchIdForAllocation(v_lAllocationId:=lAllocationID, r_lMatchId:=lMatchID)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'Get base amount already allocated.
                    m_lReturn = GetAllocationTotal(v_lAllocationId:=lAllocationID, r_cAllocationAmount:=cAllocationAmount)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        cAllocationAmount = 0
                        cAllocationCCyAmount = 0
                    End If


                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iCurrencyID,
                                                                         lCompanyID:=iCompanyId,
                                                                         cBaseAmount:=cAllocationAmount,
                                                                         cCurrencyAmount:=cAllocationCCyAmount,
                    vConversionDate:=Nothing,
                                                                         vConversionRate:=vdCurrencyBaseXRate)

                    'Add allocation detail for the cash item
                    m_lReturn = PostCashAllocationDetail(v_lAllocationId:=lAllocationID,
                                                         v_lCashListItem:=lCashListItemId,
                                                         v_lTransdetailid:=lTransDetailID,
                                                         v_lAllocationDetailId:=lAllocationDetailId,
                                                         nTaxAllocationDetailID:=nTaxAllocationDetailId,
                                                         nTaxTransdetailID:=nTaxTransDetailId)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'Get Cash List Base Amount
                    m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID,
                                                          v_iCompanyID:=iCompanyId,
                                                          r_cBaseAmount:=cBaseAmount,
                                                          v_cCurrencyAmount:=cCurrencyAmount,
                                                          r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate,
                                                          v_dtAccountingDate:=dtAccountingDate,
                                                          r_lEuro:=lEuroCurrencyID,
                                                          r_cEuroAmount:=cEuroAmount,
                                                          r_vEuroCCyXrate:=vdEuroCcyXrate,
                                                          r_vEuroBaseXRate:=vdEuroBaseXrate,
                                                          r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded,
                                                          r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'Get Cash List Tax Base Amount
                    m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID,
                                                          v_iCompanyID:=iCompanyId,
                                                          r_cBaseAmount:=crTaxAllocationAmount,
                                                          v_cCurrencyAmount:=crTaxAllocationCCyAmount,
                                                          r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate,
                                                          v_dtAccountingDate:=dtAccountingDate,
                                                          r_lEuro:=lEuroCurrencyID,
                                                          r_cEuroAmount:=cEuroAmount,
                                                          r_vEuroCCyXrate:=vdEuroCcyXrate,
                                                          r_vEuroBaseXRate:=vdEuroBaseXrate,
                                                          r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded,
                                                          r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If


                    'Get Cash List Allocation Currency Amount
                    m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iCurrencyID,
                                                                         lCompanyID:=iCompanyId,
                                                                         cBaseAmount:=cAllocationAmount,
                                                                         cCurrencyAmount:=cAllocationCCyAmount,
                                                                         vConversionDate:=Nothing,
                                                                         vConversionRate:=vdCurrencyBaseXRate)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'Set the correct sign of amounts
                    cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
                    cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)
                    vdBaseAmountUnrounded = gACTLibrary.ACTSigned(vdBaseAmountUnrounded, eCreditOrDebit)
                    vdCurrencyAmountUnrounded = gACTLibrary.ACTSigned(vdCurrencyAmountUnrounded, eCreditOrDebit)
                    cAllocationAmount = gACTLibrary.ACTSigned(cAllocationAmount, eCreditOrDebit)
                    cAllocationCCyAmount = gACTLibrary.ACTSigned(cAllocationCCyAmount, eCreditOrDebit)
                    If m_bViaInsurerPayment And m_iNoWriteOffRequired = 1 Then
                        cBaseAmount = cAllocationAmount
                    End If
                    If cBaseAmount - (crTaxAllocationAmount * eCreditOrDebit) <> cAllocationAmount Then

                        cWriteOffAmount = cBaseAmount - cAllocationAmount
                        cCurrencyWriteOff = cCurrencyAmount - cAllocationCCyAmount

                        m_lReturn = CreateAllocationManual()
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to run function, CreateAllocationManual")
                        End If


                        m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyId:=iCompanyId, r_iBaseCurrencyID:=iBaseCurrencyID)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to run function, " &
                                                       "m_oCurrencyConvert.GetBaseCurrency")
                        End If


                        m_lReturn = m_oAllocationManual.WriteOff(v_lAllocationDetailId:=lAllocationDetailId,
                                                                 v_iCurrencyID:=iBaseCurrencyID,
                                                                 v_cBaseAmount:=(cWriteOffAmount * -1),
                                                                 v_lWriteOffReasonID:=1, v_vAccountID:=lAccountID,
                                                                 v_vIsCurrencyDifference:=True,
                                                                 v_vCompanyID:=iCompanyId)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , " + "Failed to run function, " &
                                                       "m_oAllocationManual.WriteOff")
                        End If
                    Else
                        cWriteOffAmount = 0
                        cCurrencyWriteOff = 0
                    End If




                    m_lReturn = m_oMatchPost.Commit
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    m_oDatabase.SQLCommitTrans()

                    m_lReturn = UpdateCashAllocationDetail(v_lAllocationId:=lAllocationID,
                                                           v_lTransdetailid:=lTransDetailID,
                                                           v_lAllocationDetailId:=lAllocationDetailId,
                                                           v_cAllocBaseAmount:=cAllocationAmount + (crTaxAllocationAmount * eCreditOrDebit),
                                                           v_cAllocCcyAmount:=cAllocationCCyAmount + ((crTaxAllocationAmount / vdCurrencyBaseXRate) * eCreditOrDebit),
                                                           v_cWriteOffAmount:=cWriteOffAmount,
                                                           v_cCurrencyWriteOff:=cCurrencyWriteOff,
                                                           v_lWriteOffReasonID:=vWriteOffReasonId)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If


                    m_lReturn = UpdateCashTransDetail(v_lTransdetailid:=lTransDetailID,
                                                      v_lFullyMatched:=m_oAllocationDetail.Details.Item(1).FullyMatched)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'And to which transaction

                    .TransdetailID = lTransDetailID


                    If m_oAllocationDetail.Details.Item(1).FullyMatched = PMEReturnCode.PMTrue Then
                        ' Show that this cash list item has been fully allocated

                        .AllocationstatusID = gACTLibrary.ACTAllocationStatusAllocated
                    End If

                    'Force update to take place later

                    .DatabaseStatus = PMEComponentAction.PMEdit

                    'Get Instalment Plans

                    m_lReturn = GetPlansForAllocation(v_lAllocationId:=lAllocationID,
                                                      v_lCashTransId:=lTransDetailID,
                                                      v_iCurrencyID:=iCurrencyID,
                                                      v_vPlans:=vPlans)
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFail
                    End If

                    'If necessary Transfer the Commission
                    'FSA Phase 3.2 Gross Simplification
                    If m_sCommissionOption <> AsDebited Then

                        If Informations.IsArray(vMatchedTransdetailIds) Then

                            For iMatchedCount As Integer = 0 To vMatchedTransdetailIds.GetUpperBound(1)
                                If Informations.IsArray(vTransIDs) Then

                                    ReDim Preserve vTransIDs(0, vTransIDs.GetUpperBound(1) + 1)
                                Else
                                    ReDim vTransIDs(0, 0)
                                End If
                                vTransIDs(0, vTransIDs.GetUpperBound(1)) = vMatchedTransdetailIds(0, iMatchedCount)
                            Next iMatchedCount
                        End If
                    End If
                    'FSA Phase 3.2End

                    'If the Receipt/Payment is on a Credit Card then collect the payment
                    If .MediaTypeIssuerID <> 0 And .CC_Auth_Code <> "" And .CC_Transaction_Code <> "" Then
                        m_lReturn = m_oCreditCard.CollectPayment(lCashListItemID:=lCashListItemId)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFail
                        End If
                    End If
                End With
            Next lItem

            ' End by writing the cashlist updates back


            m_lReturn = m_oCashList.Update
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            m_lReturn = m_oCashListItem.Update
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If

            m_oDatabase.SQLCommitTrans()
            If Informations.IsArray(vTransIDs) Then

                If m_oTransDetail Is Nothing Then












                    m_oTransDetail = New bACTTransdetail.Form
                    m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        Return PMEReturnCode.PMFalse
                    End If
                End If

                'DC150806 get preferred date based on payment date
                m_lReturn = GetPreferredDocumentDate(v_lCashListItemID:=lCashListItemId,
                                                     r_vPreferredDocumentDate:=vDocDate)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername,
                                       iType:=PMELogLevel.PMLogOnError,
                                       sMsg:="Failed to Get Preferred Document Date.",
                                       vApp:=ACApp, vClass:=ACClass, vMethod:="Start",
                                       vErrNo:=Informations.Err().Number,
                                       vErrDesc:=Informations.Err().Description)
                    Return PMEReturnCode.PMFalse
                End If

                If Convert.IsDBNull(vDocDate) Or Informations.IsNothing(vDocDate) Then
                    vDocDate = DateTime.Today
                End If

                For lRow As Integer = vTransIDs.GetLowerBound(1) To vTransIDs.GetUpperBound(1)
                    'FSA Phase 3.2
                    'DC150806 added preferred date
                    m_lReturn = m_oTransDetail.ReleaseSuspendedTransactions(lAllocationId:=lAllocationID,
                                                                            vLinkedTransdetailID:=vTransIDs(0, lRow),
                                                                            vAccountingDate:=vDocDate)
                    If m_lReturn <> PMEReturnCode.PMTrue And m_lReturn <> PMEReturnCode.PMNotFound Then
                        bPMFunc.LogMessage(m_sUsername,
                                           iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="Failed to Commit Commission Posting.",
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="PostAllocatedCash",
                                           vErrNo:=Informations.Err().Number,
                                           vErrDesc:=Informations.Err().Description)
                    End If
                Next lRow
            End If

            If Informations.IsArray(vPlans) Then
                For lRow As Integer = 0 To vPlans.GetUpperBound(1)
                    Dim dbNumericTemp As Double
                    If Not Double.TryParse(CStr(vPlans(4, lRow)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                        vPlans(4, lRow) = 0
                    End If

                    If CDec(vPlans(4, lRow)) <> 0 Then
                        m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=vPlans(6, lRow),
                                                              v_iCompanyID:=iCompanyId,
                                                              r_cBaseAmount:=vPlans(4, lRow),
                                                              v_cCurrencyAmount:=cCurrencyWriteOff,
                                                              r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate,
                                                              v_dtAccountingDate:=dtAccountingDate,
                                                              r_lEuro:=lEuroCurrencyID,
                                                              r_cEuroAmount:=cEuroAmount,
                                                              r_vEuroCCyXrate:=vdEuroCcyXrate,
                                                              r_vEuroBaseXRate:=vdEuroBaseXrate,
                                                              r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded,
                                                              r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)
                        If m_lReturn <> PMEReturnCode.PMTrue Then
                            Return PMEReturnCode.PMFail
                        End If
                    End If

                    m_lReturn = PayPlanInstalments(v_lPlanNo:=vPlans(0, lRow),
                                                   v_lPlanVersion:=vPlans(1, lRow),
                                                   v_cPlanPaid:=CDec(vPlans(2, lRow)) + CDec(vPlans(4, lRow)),
                                                   v_lPlanTransactionId:=vPlans(5, lRow),
                                                   v_iCurrencyID:=vPlans(6, lRow))
                    If m_lReturn <> PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError,
                                           sMsg:="Failed to Pay Instalments.",
                                           vApp:=ACApp, vClass:=ACClass, vMethod:="PostAllocatedCash",
                                           vErrNo:=Informations.Err().Number,
                                           vErrDesc:=Informations.Err().Description)
                    End If
                    'update the instalments
                Next lRow
            End If

            Return result

        Catch excep As System.Exception
            result = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostAllocatedCash", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: PostCashList (Public)
    '
    ' Description: Creates one document for the cash list id supplied
    '
    ' ***************************************************************** '
    Public Function PostCashlist(ByVal v_vCashListID As Object) As Integer
        Return PostCashlist(v_vCashListID:=v_vCashListID, v_vCashListItemID:=Nothing)
    End Function

    Public Function PostCashlist(ByVal v_vCashListID As Object, ByVal v_vCashListItemID As Object) As Integer

        Dim result As Integer = 0
        Dim oCashList As Object
        Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign
        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim dtAccountingDate As Date
        Dim iCurrencyID As Integer
        Dim lAccountID, lDocumentID, lDocumentType, lTransDetailID As Integer
        'eck100500
        Dim iCompanyId As Integer

        'EK 100100 Extra propertiesfor Euro
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroBaseXrate As Object = Nothing
        Dim vdEuroCcyXrate As Object = Nothing
        Dim vdBaseAmountUnrounded As Double
        Dim vdCurrencyAmountUnrounded As Double
        '

        Dim sOurRef, sTheirRef, sMediaRef As String

        Dim sDocumentRef As String = ""

        Dim lNumberRangeID As Integer

        Dim lMatchID As Integer

        Dim lCashListItemId, lAllocationID As Integer

        Dim cAllocationAmount, cAllocationCCyAmount As Decimal

        'EK 040200
        Dim sAccountType As String = ""
        Dim vTransIDs(,) As Object = Nothing

        'EK 220200
        Dim sGroupCode, sRangeCode As String
        'eck230600
        'DD 05/08/2002: Multi-Branch
        Dim lSubBranchID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load up the collections with the cashlists


            'developer guide no. 98
            m_lReturn = GetCashListDetails(v_lCashListId:=v_vCashListID, v_vCashListItemID:=v_vCashListItemID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            oCashList = m_oCashList.Details.Item(0)

            ' Use CashList & CashListItems to Populate Document & Transactions

            With oCashList


                iCurrencyID = .CurrencyID
                'eck100500

                iCompanyId = .CompanyID

                'DD 05/08/2002: Get the SubBranch for the Match Group

                m_lReturn = bACTFunc.GetSubBranchID(v_oDatabase:=m_oDatabase, r_lSubBranchID:=lSubBranchID, v_vBankAccountID:=oCashList.BankAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                'DD 05/08/2002: Added the parameter for the SubBranch.


                m_lReturn = m_oMatchPost.AddMatchGroup(v_dtMatchDate:= .Listdate, r_vMatchId:=lMatchID, r_vMatchSourceId:=iCompanyId, v_lSubBranchID:=lSubBranchID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                '
                'RM04-02-99
                'Document Type Payment/Receipt
                'Replaces Document Type CashDebit and CashCredit
                'For CashListType Payment/Receipt


                If (.CashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                    lDocumentType = gACTLibrary.ACTDocTypePayment
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit '   DEBIT SUPPLIER
                    'EK 220200
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef23
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSpy

                ElseIf .CashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                    lDocumentType = gACTLibrary.ACTDocTypeReceipt
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT SUPPLIER
                    'EK 220200
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef22
                    'eck020500
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeSrp
                Else
                    lDocumentType = gACTLibrary.ACTDocTypeCashCredit
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT CUSTOMER
                    'EK 220200
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef6
                    sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeCcr
                End If

                ' Get the number range
                'EK 220200 Pass predefined group and range codes

                m_lReturn = m_oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the cashlist status to show that it can't be deleted

                .CashListStatusID = gACTLibrary.ACTCashListStatusOpened

                .DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit ' To Force Update

            End With

            ' Load each Item up as a transaction

            For lItem As Integer = 1 To m_oCashListItem.Details.Count

                With oCashList

                    dtAccountingDate = .Listdate

                    If (.CashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                        lDocumentType = gACTLibrary.ACTDocTypePayment
                        eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit '   DEBIT SUPPLIER
                    ElseIf .CashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then
                        lDocumentType = gACTLibrary.ACTDocTypeReceipt
                        eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT SUPPLIER
                    Else
                        lDocumentType = gACTLibrary.ACTDocTypeCashCredit
                        eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit '   CREDIT CUSTOMER
                    End If

                End With

                'eck180500 Pass Company ID

                With m_oCashListItem.Details.Item(ToSafeInteger(lItem))

                    ' Generate the next number
                    'Start-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                    m_lReturn = m_oPMAutoNumber.GenerateDocumentReferenceNumber(v_sRangeCode:=sRangeCode, v_iUserID:=m_iUserID, v_iCompanyID:=iCompanyId, r_sDocumentRef:=sDocumentRef, v_lNumberRangeID:=lNumberRangeID)
                    'End-(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    If sDocumentRef.Trim() <> "" Then
                        sDocumentRef = sRangeCode & sDocumentRef
                    End If
                    'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    'eck100500


                    m_lReturn = m_oDocumentPost.AddDocument(v_lDocumentTypeId:=lDocumentType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="Cash", r_vDocumentID:=lDocumentID, r_vDocSourceID:=iCompanyId)




                    If .TransdetailID = 0 Then


                        cCurrencyAmount = .Amount




                        lCashListItemId = .CashlistitemID
                        m_lReturn = GetAllocationIDForCashListItem(v_lCashListItemID:=lCashListItemId, r_lAllocationID:=lAllocationID)
                        If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                            m_lReturn = GetAllocationTotal(v_lAllocationId:=lAllocationID, r_cAllocationAmount:=cAllocationAmount)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                cAllocationAmount = 0
                                cAllocationCCyAmount = 0
                            End If




                            m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=iCurrencyID, lCompanyID:=iCompanyId, cBaseAmount:=cAllocationAmount, cCurrencyAmount:=cAllocationCCyAmount, vConversionDate:=Nothing, vConversionRate:=vdCurrencyBaseXRate)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFail
                            End If

                            'EK 100100 Added extra parameters
                            m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID, v_iCompanyID:=iCompanyId, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=vdEuroCcyXrate, r_vEuroBaseXRate:=vdEuroBaseXrate, r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFail
                            End If

                            ' The document sequence 1 is reserved for the bank line

                            sOurRef = .OurRef.Trim()

                            sTheirRef = .TheirRef.Trim()

                            sMediaRef = .MediaRef.Trim()

                            lAccountID = .AccountID

                            cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
                            cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)
                            'EK 100100
                            vdBaseAmountUnrounded = gACTLibrary.ACTSigned(vdBaseAmountUnrounded, eCreditOrDebit)
                            vdCurrencyAmountUnrounded = gACTLibrary.ACTSigned(vdCurrencyAmountUnrounded, eCreditOrDebit)
                            'EK 7/12/99 Initialise variable
                            lTransDetailID = 0
                            '
                            ' CF 190899 - Check here if exists already. if it does, then dont
                            ' add it again, only need to transmatch it

                            'developer guide no. 98
                            m_lReturn = CheckTransExists(r_lTransDetailID:=lTransDetailID, v_lCashListId:=v_vCashListID)
                            'EK 100100 Pass extra parameters for Euro
                            If lTransDetailID = 0 Then
                                ' changed from lItem + 1 to lItem
                                ' changed from lItem to 1

                                m_lReturn = m_oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=1, v_lAccountID:=lAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyId:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXRate:=vdEuroBaseXrate, v_vEuroCcyXrate:=vdEuroCcyXrate, v_vComment:=sOurRef & "/" & sTheirRef & "/" & sMediaRef, v_vAccountingDate:=dtAccountingDate, v_vSpare:=sMediaRef)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    Return gPMConstants.PMEReturnCode.PMFail
                                End If
                            End If

                            ' CF 260299 - Only match the amount allocated
                            cBaseAmount = gACTLibrary.ACTSigned(cAllocationAmount, eCreditOrDebit)
                            cCurrencyAmount = gACTLibrary.ACTSigned(cAllocationCCyAmount, eCreditOrDebit)


                            m_lReturn = m_oMatchPost.AddMatchTrans(v_lAllocationdetailID:=lAllocationID, v_lTransDetailID:=lTransDetailID, v_iCurrencyID:=iCurrencyID, v_cBaseMatchAmount:=cBaseAmount, v_cCurrencyMatchAmount:=cCurrencyAmount, v_vdCurrencyMatchXRate:=vdCurrencyBaseXRate)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFail
                            End If



                            ' And to which transaction

                            .TransdetailID = lTransDetailID

                            ' Force update to take place later

                            .DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit

                        End If

                        If m_sCommissionOption <> AsDebited Then
                            If Informations.IsArray(vTransIDs) Then

                                ReDim Preserve vTransIDs(0, vTransIDs.GetUpperBound(1) + 1)
                            Else
                                ReDim vTransIDs(0, 0)
                            End If


                            vTransIDs(0, vTransIDs.GetUpperBound(1)) = lTransDetailID
                        End If
                    End If
                    'FSA Phase 3.2 End



                End With

                ' Now the bank line to balance

                m_lReturn = GetAccountIdForBank(r_lAccountId:=lAccountID, v_lBankAccountId:=oCashList.BankAccountID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                ' Reverse the sign for the Bank Side

                If (oCashList.CashListTypeID = gACTLibrary.ACTCashListTypePayments) Or (oCashList.CashListTypeID = gACTLibrary.ACTCashListTypeClaimPayments) Then
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit ' CREDIT BANK/DEBIT SUPPLIER
                Else
                    eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit ' DEBIT BANK/CREDIT CUSTOMER
                End If

                ' Totals from the Document

                cCurrencyAmount = m_oDocumentPost.TotalCurrency

                cBaseAmount = m_oDocumentPost.TotalBase

                ' Sequence 1 indicates that this is the Primary Trans
                cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
                cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)

                ' Change from vDocumentSequence:=1 to lItem
                ' Change from lItem to 2

                m_lReturn = m_oDocumentPost.AddTransaction(v_lAccountID:=lAccountID, v_vDocumentSequence:=2, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vComment:="", r_vTransDetailId:=lTransDetailID, v_vAccountingDate:=dtAccountingDate)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                ' and signal to DocumentPost that its ok to update


                m_lReturn = m_oDocumentPost.Commit()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                ' Reset the totals

                m_oDocumentPost.TotalBase = 0

                m_oDocumentPost.TotalCurrency = 0

            Next lItem

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' End by writing the cashlist updates back


            m_lReturn = m_oCashList.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If


            m_lReturn = m_oCashListItem.Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            m_oDocumentPost = Nothing


            m_lReturn = m_oMatchPost.Commit

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            m_oMatchPost = Nothing

            If Informations.IsArray(vTransIDs) Then

                'FSA Phase 3.2
                If m_oTransDetail Is Nothing Then


                    m_oTransDetail = New bACTTransdetail.Form
                    m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                'FSA Phase 3.2End

                For lRow As Integer = vTransIDs.GetLowerBound(1) To vTransIDs.GetUpperBound(1)

                    m_lReturn = m_oTransDetail.ReleaseSuspendedTransactions(lAllocationId:=lAllocationID, vLinkedTransdetailID:=vTransIDs(0, lRow))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Release  Suspended Transactions.", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashList", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If
                Next lRow
            End If
            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostCashlist", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetStatus (Standard Method)
    '
    ' Description: Set the Process, Map and Step status.
    ' Note:        A Property Get is provided for the Step Status only
    '              as this is the only one which this component can
    '              alter directly.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Stores all of the parameter members with the key array.
    ''' </summary>
    ''' <param name="vKeyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetKeys(ByRef vKeyArray(,) As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue

        Try
            Dim nInnerRow As Integer
            m_vCashListId = Nothing

            m_vCashListItemId = Nothing
            m_lAllocationID = 0

            If Not Informations.IsArray(vKeyArray) Then
                Return PMEReturnCode.PMFalse
            End If

            For nRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                If Not (vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, nRow) Is Nothing) Then
                    Select Case CStr(vKeyArray(PMENavLetGetKeyColPosition.PMKeyName, nRow)).Trim()

                        Case PMNavKeyConst.ACTKeyNameCashListId
                            m_vCashListId = vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow)

                        Case PMNavKeyConst.ACTKeyNameCashListItemId
                            m_vCashListItemId = vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow)

                        Case PMNavKeyConst.ACTKeyNameAllocationId
                            m_lAllocationID = CInt(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow))

                        Case PMNavKeyConst.kACTKeyNameTaxBandId
                            m_nTaxBandID = vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow)

                        Case PMNavKeyConst.kACTKeyNameTaxAmount
                            m_crTaxAmount = ToSafeCurrency(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow))

                        Case PMNavKeyConst.PMKeyNameInsuranceFileCnt
                            m_nInsuranceFileCnt = ToSafeLong(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow))

                        Case ACTKeyNameInsurerPayment
                            m_bViaInsurerPayment = True
                            For nInnerRow = vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow).GetLowerBound(1) To vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow).GetUpperBound(1)
                                Select Case Trim$(CStr(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow)(PMENavLetGetKeyColPosition.PMKeyName, nInnerRow)))
                                    Case "NoWriteOff"
                                        m_iNoWriteOffRequired = ToSafeInteger(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow)(PMENavLetGetKeyColPosition.PMKeyValue, nInnerRow))
                                End Select
                            Next nInnerRow
                        Case kPMKeyNameInsurerPaymentRoadMap
                            m_bIsInsurerPaymentRoadMap = ToSafeLong(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow))
                        Case PMKeyNameBatchID
                            m_nBatchID = ToSafeLong(vKeyArray(PMENavLetGetKeyColPosition.PMKeyValue, nRow))
                    End Select
                End If

            Next nRow

            If m_vCashListItemId = 0 Then
                m_vCashListItemId = Nothing
            End If

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
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

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.
            '    vkeyarray(PMKeyName, 0) = ACTKeyNameCashListTypeId
            '    vkeyarray(PMKeyValue, 0) = m_lCashListTypeID&

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 040200
    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String) As Integer
        Return GetOption(v_iOptionNumber:=v_iOptionNumber, r_sOptionValue:=r_sOptionValue, vDatabase:=Nothing)
    End Function

    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByRef vDatabase As Object) As Integer

        Dim result As Integer = 0
        Dim bCloseDatabase As Boolean
        Try


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



            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=r_sOptionValue, v_iSourceID:=m_iSourceID)

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

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Private Function GetPlansForAllocation(ByVal v_lAllocationId As Integer, ByVal v_lCashTransId As Integer, ByVal v_iCurrencyID As Integer, ByRef v_vPlans(,) As Object) As Integer

        Dim result As Integer = 0

        Dim vResultArray(,) As Object = Nothing
        result = gPMConstants.PMEReturnCode.PMTrue





        With m_oDatabase

            .Parameters.Clear()
            .Parameters.Add("AllocationId", CStr(v_lAllocationId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetPlansSQL, sSQLName:=ACGetPlansName, bStoredProcedure:=ACGetPlansStored, vResultArray:=vResultArray)
        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return result
        End If


        For i As Integer = 0 To vResultArray.GetUpperBound(1)
            If Not Informations.IsArray(v_vPlans) Then
                ReDim v_vPlans(6, 0)
            Else
                ReDim Preserve v_vPlans(6, v_vPlans.GetUpperBound(1) + 1)
            End If


            v_vPlans(0, v_vPlans.GetUpperBound(1)) = vResultArray(0, i)


            v_vPlans(1, v_vPlans.GetUpperBound(1)) = vResultArray(1, i)


            v_vPlans(2, v_vPlans.GetUpperBound(1)) = vResultArray(2, i)


            v_vPlans(3, v_vPlans.GetUpperBound(1)) = vResultArray(3, i)


            v_vPlans(4, v_vPlans.GetUpperBound(1)) = vResultArray(4, i)

            v_vPlans(5, v_vPlans.GetUpperBound(1)) = v_lCashTransId

            v_vPlans(6, v_vPlans.GetUpperBound(1)) = v_iCurrencyID
        Next i

        Return result

    End Function
    Private Function PayPlanInstalments(ByVal v_lPlanNo As Integer, ByVal v_lPlanVersion As Integer, ByVal v_cPlanPaid As Decimal, ByRef v_lPlanTransactionId As Integer, ByRef v_iCurrencyID As Integer) As Integer

        Dim result As Integer = 0
        Dim oSBODatabase As dPMDAO.Database = Nothing

        Dim vResultArray(,) As Object = Nothing
        Dim cAmount As Decimal
        Dim sSQL As String = ""
        Dim vStatus(,) As Object = Nothing
        Dim lStatusId As Integer
        Const InstalmentNo As Integer = 2


        result = gPMConstants.PMEReturnCode.PMTrue





        m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, r_bNewInstanceCreated:=True, r_oCheckedDatabase:=oSBODatabase)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With oSBODatabase

            .Parameters.Clear()
            .Parameters.Add("pfprem_finance_cnt", CStr(v_lPlanNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("pfprem_finance_version", CStr(v_lPlanVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'developer guide no.85
            .Parameters.Add("InstalmentNumber", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=SelPlanInstalmentsSQL, sSQLName:=SelPlanInstalmentsName, bStoredProcedure:=SelPlanInstalmentsStored, vResultArray:=vResultArray)
        End With

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = oSBODatabase.CloseDatabase()
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
            m_lReturn = oSBODatabase.CloseDatabase()
            Return result
        End If


        sSQL = "select PFInstalments_status_id from PFInstalments_status " &
               " where description = 'Collected'"

        m_lReturn = oSBODatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetStatusInd", bStoredProcedure:=False, vResultArray:=vStatus)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            m_lReturn = oSBODatabase.CloseDatabase()
            Return result
        End If


        lStatusId = CInt(vStatus(0, 0))


        For i As Integer = 0 To vResultArray.GetUpperBound(1)


            cAmount += gPMFunctions.ToSafeDecimal(vResultArray(5, i))
            If cAmount > v_cPlanPaid Then
                Exit For
            End If

            If gPMFunctions.ToSafeInteger(vResultArray(7, i)) <> lStatusId Then

                With oSBODatabase
                    .Parameters.Clear()
                    .Parameters.Add("pfprem_finance_cnt", CStr(v_lPlanNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("pfprem_finance_version", CStr(v_lPlanVersion), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    .Parameters.Add("InstalmentNumber", CStr(gPMFunctions.ToSafeInteger(vResultArray(InstalmentNo, i))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                    .Parameters.Add("TransactionID", CStr(v_lPlanTransactionId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                    m_lReturn = .SQLAction(sSQL:=PayPlanInstalmentSQL, sSQLName:=PayPlanInstalmentName, bStoredProcedure:=PayPlanInstalmentStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Marking Batch as posted failure
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Pay Instalment.", vApp:=ACApp, vClass:=ACClass, vMethod:="PayPlanInstalments", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Exit For
                    End If
                End With
            End If


        Next i

        m_lReturn = oSBODatabase.CloseDatabase()



        Return result

    End Function

    ''' <summary>
    ''' Performs the Automated Action dependant on the Task Process Mode etc.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Start() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim m_sIncludeInsurerPaymentMultiStep As String = String.Empty
        Dim oOptionValue As Object = Nothing
        Dim bIsPosted As Boolean
        Const kIncludeInsurerPaymentMultiStep As Integer = 5143
        Try


            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername,
                                                      v_sPassword:=m_sPassword,
                                                      v_iUserID:=m_iUserID,
                                                      v_iMainSourceID:=m_iSourceID,
                                                      v_iLanguageID:=m_iLanguageID,
                                                      v_iCurrencyID:=m_iCurrencyID,
                                                      v_iLogLevel:=m_iLogLevel,
                                                      v_sCallingAppName:=m_sCallingAppName,
                                                      v_vOptionNumber:=SIRHiddenOptions.SIROPTMultiStepApproval,
                                                      v_vBranch:=1,
                                                      r_vUnderwriting:=oOptionValue)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Throw New System.Exception("Failed to getProductOptionValue.")
            End If

            If oOptionValue IsNot Nothing AndAlso oOptionValue = "1" Then
                m_lReturn = GetOption(v_iOptionNumber:=kIncludeInsurerPaymentMultiStep,
                                      r_sOptionValue:=m_sIncludeInsurerPaymentMultiStep)
                If (m_lReturn <> PMEReturnCode.PMTrue) Then
                    Throw New System.Exception("Failed to GetOption.")
                End If

                If m_sIncludeInsurerPaymentMultiStep = "1" And ToSafeLong(m_vCashListId) > 0 Then

                    m_lReturn = GetPostingStatusForCashList(m_vCashListId, bIsPosted)
                    If (m_lReturn <> PMEReturnCode.PMTrue) Then
                        Throw New System.Exception("GetPostingStatusForCashList Failed.")
                    End If

                    If bIsPosted = False Then
                        m_lStatus = PMEReturnCode.PMOK
                        Return PMEReturnCode.PMTrue
                    End If
                End If

            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()
            'eck100700

            If Convert.IsDBNull(m_vCashListItemId) Or Informations.IsNothing(m_vCashListItemId) Then
                m_lReturn = PostAllocatedCash(v_vCashListID:=m_vCashListId)
            Else
                m_lReturn = PostAllocatedCash(v_vCashListID:=m_vCashListId, v_vCashListItemID:=m_vCashListItemId)
            End If


            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' RAW 12/03/2003 : ISS2893 : added
                m_lStatus = PMEReturnCode.PMCancel ' force navigator to stop
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogError, sMsg:="Failed to post allocated cash :" & m_vCashListId, vApp:=ACApp, vClass:=ACClass, vMethod:="Start")
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return PMEReturnCode.PMFalse
            Else
                'eck060900
                m_lStatus = PMEReturnCode.PMOK
                '
                m_lReturn = m_oDatabase.SQLCommitTrans()
                Return PMEReturnCode.PMTrue
            End If

            Return nResult
        Catch ex As System.Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return nResult
        End Try


    End Function

    ' PUBLIC Methods (End)

    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

        Catch excep As System.Exception



            ' Error.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' Load the details into the CashList & CashListItem Collections
    Private Function GetCashListDetails(ByVal v_lCashListId As Integer, Optional ByRef v_vCashListItemID As Object = Nothing) As Integer
        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oCashList.GetDetails(vCashListID:=v_lCashListId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' Now get the details


        m_lReturn = m_oCashListItem.GetDetails(vCashlistitemID:=v_vCashListItemID, vCashListID:=ToSafeInteger(v_lCashListId))

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function
    ' Using bank account business
    ' Looks up the account id from a bank account id
    '
    Private Function GetAccountIdForBank(ByRef r_lAccountId As Integer, ByVal v_lBankAccountId As Integer) As Integer

        Dim result As Integer = 0
        Const kBankSuspenseAccountType As gPMConstants.PMEReturnCode = 2
        Dim oBankAccount As bACTBankAccount.Form
        Dim lDefaultBankAccountID As Integer
        Dim iBankAccountTypeId As Integer



        result = gPMConstants.PMEReturnCode.PMTrue



        oBankAccount = New bACTBankAccount.Form
        m_lReturn = oBankAccount.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With oBankAccount


            m_lReturn = .GetDetails(vBankAccountId:=v_lBankAccountId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = .GetNext(vAccountId:=r_lAccountId, vBankAccountTypeId:=iBankAccountTypeId, vDefaultBankAccountID:=lDefaultBankAccountID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.ToSafeInteger(iBankAccountTypeId) = kBankSuspenseAccountType And gPMFunctions.ToSafeLong(lDefaultBankAccountID) <> 0 Then
                r_lAccountId = lDefaultBankAccountID
            End If

        End With

        oBankAccount = Nothing

        Return result

    End Function

    Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Object, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Object, ByRef r_vEuroBaseXRate As Object, ByRef r_vCCyAmountUnrounded As Object, ByRef r_vBaseAmountUnrounded As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = m_oCurrencyConvert.ConvertCurrencytoBase(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_iCompanyID, cBaseAmount:=r_cBaseAmount, cCurrencyAmount:=v_cCurrencyAmount, vConversionDate:=v_dtAccountingDate, vConversionRate:=r_vdCurrencyBaseXRate, vIsMultiplier:=False, vRounded:=True, vBaseRoundingDifference:=Nothing, vCurrencyRoundingDifference:=Nothing, vFormattedBase:=Nothing, vFormattedCurrency:=Nothing, lEuro:=r_lEuro, cEuroAmount:=r_cEuroAmount, vEuroCCyXrate:=r_vEuroCCyXrate, vEuroBaseXRate:=r_vEuroBaseXRate, vCCyAmountUnRounded:=r_vCCyAmountUnrounded, vBaseAmountUnRounded:=r_vBaseAmountUnrounded)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PostCheque (Private)
    '
    ' Description: Adds a new Record to the cheque production Table
    '
    ' ***************************************************************** '


    Private Function PostCheque(ByRef lTransDetailID As Integer, ByRef lBankID As Integer) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If m_oChequeProduction Is Nothing Then


            'm_oChequeProduction = New bACTChequeProduction.Business

            m_oChequeProduction = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oChequeProduction, v_sClassName:="bACTChequeProduction.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bSIRRiskScreen.Business"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bACTChequeProduction.Business", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'm_lReturn = m_oChequeProduction.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            m_lReturn = m_oChequeProduction.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(m_sCallingAppName), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

        End If


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oChequeProduction.AddCheque(lTransdetailId:=ToSafeInteger(lTransDetailID), lBankID:=ToSafeInteger(lBankID))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ''' <summary>
    ''' Adds AllocationDetail record for posted Cash
    ''' </summary>
    ''' <param name="v_lAllocationId"></param>
    ''' <param name="v_lCashListItem"></param>
    ''' <param name="v_lTransdetailid"></param>
    ''' <param name="v_lAllocationDetailId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PostCashAllocationDetail(ByRef v_lAllocationId As Object,
                                              ByRef v_lCashListItem As Integer,
                                              ByRef v_lTransdetailid As Object,
                                              ByRef v_lAllocationDetailId As Object,
                                              ByVal nTaxTransdetailID As Object,
                                              ByVal nTaxAllocationDetailID As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oResultArray As Object(,) = Nothing
        Dim crCashTaxAmount As Object


        If m_oAllocationCreate Is Nothing Then
            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationCreate,
                                                                  v_sClassName:="bACTAllocationCreate.Automated",
                                                                  v_sCallingAppName:=ACApp,
                                                                  v_sUsername:=m_sUsername,
                                                                  v_sPassword:=m_sPassword,
                                                                  v_iUserID:=m_iUserID,
                                                                  v_iSourceID:=m_iSourceID,
                                                                  v_iLanguageID:=m_iLanguageID,
                                                                  v_iCurrencyID:=m_iCurrencyID,
                                                                  v_iLogLevel:=m_iLogLevel,
                                                                  v_oDatabase:=m_oDatabase)
        End If

        m_oDatabase.Parameters.Clear()
        m_lReturn = m_oDatabase.Parameters.Add(sName:="nTransdetailID",
                                                vValue:=v_lTransdetailid,
                                                iDirection:=PMEParameterDirection.PMParamInput,
                                                iDataType:=PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetTaxTransdetailidSQL,
                                           sSQLName:=kGetTaxTransdetailidName,
                                           bStoredProcedure:=kGetTaxTransdetailidStored,
                                           vResultArray:=oResultArray)

        If (m_lReturn <> PMEReturnCode.PMTrue) Then
            Return PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(oResultArray) Then
            If ToSafeLong(oResultArray(0, 0)) > 0 Then
                nTaxTransdetailID = oResultArray(0, 0)
                crCashTaxAmount = oResultArray(1, 0)
            End If
        End If


        m_lReturn = m_oAllocationCreate.GetAllocation(v_lAllocationID:=ToSafeInteger(v_lAllocationId))
        If m_lReturn <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oAllocationCreate.CreateAllocationDetail(v_lTransDetailId:=v_lTransdetailid,
                                                               v_lAllocationID:=v_lAllocationId,
                                                               v_vAllocationDetailId:=v_lAllocationDetailId,
                                                               crCashTaxAmount:=crCashTaxAmount,
                                                               bAllocatingCashTaxAmount:=False)
        If m_lReturn <> PMEReturnCode.PMTrue Then
            Return PMEReturnCode.PMFalse
        End If

        If nTaxTransdetailID > 0 And crCashTaxAmount <> 0 Then
            m_lReturn = m_oAllocationCreate.CreateAllocationDetail(v_lTransDetailId:=nTaxTransdetailID,
                                                                   v_lAllocationID:=v_lAllocationId,
                                                                   v_vAllocationDetailId:=nTaxAllocationDetailID,
                                                                   crCashTaxAmount:=crCashTaxAmount,
                                                                   bAllocatingCashTaxAmount:=True)
            If (m_lReturn <> PMEReturnCode.PMTrue) Then
                Return PMEReturnCode.PMFalse
            End If
        End If

        Return nResult
    End Function

    'eck110102
    ' ***************************************************************** '
    ' Name: UpdateCashAllocationDetail (Private)
    '
    ' Description: Updates AllocationDetail record with matched amounts
    '
    ' ***************************************************************** '

    Private Function UpdateCashAllocationDetail(ByRef v_lAllocationId As Object, ByRef v_lTransdetailid As Integer, ByRef v_lAllocationDetailId As Integer, ByRef v_cAllocBaseAmount As Decimal, ByRef v_cAllocCcyAmount As Decimal, ByRef v_cWriteOffAmount As Decimal, ByRef v_cCurrencyWriteOff As Decimal, ByRef v_lWriteOffReasonID As Object) As Integer

        Dim result As Integer = 0
        Dim cNewOSBaseAmount As Decimal ' RAW 12/03/2003 : ISS2893 : added
        Dim cNewOSCcyAmount As Decimal ' RAW 12/03/2003 : ISS2893 : added

        Dim cOsBaseAmount, cOsCcyAmount As Decimal
        Dim lFullyMatched As Integer ' RAW 12/03/2003 : ISS2893 : added
        ' KB PN 7129
        Dim v_cCurrencyDifference As Double

        ' raw 12/03/2003 : iss2893 : added

        result = gPMConstants.PMEReturnCode.PMTrue


        If m_oAllocationDetail Is Nothing Then


            m_oAllocationDetail = New bACTAllocationdetail.Form
            m_lReturn = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' RAW 12/03/2003 : ISS2893 : moved to within if test
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If



        m_lReturn = m_oAllocationDetail.GetDetails(vAllocationId:=v_lAllocationId, vAllocationDetailID:=v_lAllocationDetailId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' RAW 12/03/2003 : ISS2893 : added
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_oAllocationDetail.Details.Count <> 1 Then
            ' RAW 12/03/2003 : ISS2893 : added
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' RAW 12/03/2003 : ISS2893 : get OS amounts instead of Orig

        m_lReturn = m_oAllocationDetail.GetNext(vOsBaseAmount:=cOsBaseAmount, vOsCcyAmount:=cOsCcyAmount)

        ' RAW 12/03/2003 : ISS2893 : added
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If



        'eck020103 also include write off
        ' RAW 12/03/2003 : ISS2893 : replaced OS amounts with NewOS amounts
        cNewOSBaseAmount = cOsBaseAmount - v_cAllocBaseAmount - v_cWriteOffAmount - v_cCurrencyDifference
        cNewOSCcyAmount = cOsCcyAmount - v_cAllocCcyAmount - v_cCurrencyWriteOff
        'DJM 06/12/2002 : Use the allocation amounts not total matched ever amounts.
        'eck 020103 Pass through write off details

        ' RAW 12/03/2003 : ISS2893 : added
        If cNewOSBaseAmount = 0 Then
            lFullyMatched = 1
        Else
            lFullyMatched = 0
        End If

        ' RAW 12/03/2003 : ISS2893 : saving NewOS bal instead of OS bal and added fullymatched

        m_lReturn = m_oAllocationDetail.EditUpdate(1, vNewOsBaseAmount:=cNewOSBaseAmount, vNewOsCcyAmount:=cNewOSCcyAmount, vAllocBaseAmount:=v_cAllocBaseAmount, vAllocCcyAmount:=v_cAllocCcyAmount, vWriteOffReasonID:=v_lWriteOffReasonID, vWriteOffAmount:=v_cWriteOffAmount, vFullyMatched:=lFullyMatched, vOrigBaseAmount:=cOsBaseAmount, vOrigCcyAmount:=cOsCcyAmount)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oAllocationDetail.Update()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

    End Function


    ' RAW 12/03/2003 : ISS2893 : added
    ' ***************************************************************** '
    ' Name: UpdateCashTransDetail (Private)
    '
    ' Description: Updates TransDetail record
    '
    ' ***************************************************************** '

    Private Function UpdateCashTransDetail(ByRef v_lTransdetailid As Integer, ByRef v_lFullyMatched As Object) As Integer


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue


        If m_oTransDetail Is Nothing Then


            m_oTransDetail = New bACTTransdetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        With m_oTransDetail

            ' get the details from database

            m_lReturn = .GetDetails(vTransdetailID:=v_lTransdetailid, vOSAmounts:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oTransDetail.Details.Count <> 1 Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


            ' update object properties

            m_lReturn = .EditUpdate(1, vTransdetailID:=v_lTransdetailid, vFullyMatched:=v_lFullyMatched)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' save details back to database

            m_lReturn = .Update

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


        End With

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: GetPaymentStatusCode
    '
    ' Description: Gets the payment status code from the id
    '
    ' SW 29/01/2003 Created
    '
    ' ***************************************************************** '
    Private Function GetPaymentStatusCode(ByVal v_lCashListItemID As Integer, ByVal v_lStatusID As Integer, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="statusid", vValue:=CStr(v_lStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPaymentStatusCodeSQL, sSQLName:=ACGetPaymentStatusCodeName, bStoredProcedure:=True, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        r_sCode = gPMFunctions.ToSafeString(vResultArray(0, 0))

        Return result

    End Function




    Private Function CreateAllocationManual() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_oAllocationManual = New bACTAllocationManual.Business
        m_lReturn = m_oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New System.Exception(Constants.vbObjectError.ToString() + ", , Failed to create object, bACTAllocationManual.Business")
        End If

        Return result

    End Function

    Public Function GetTransactionsForAllocatedCashListItem(ByVal lAccountID As Integer, ByVal lInsuranceFileCnt As Integer,
                                                            ByVal sDocumentRef As String, ByRef r_vResultArray As Object,
                                                            ByVal v_bUseDocumentRef As Boolean) As Integer

        Return GetTransactionsForAllocatedCashListItem(lAccountID:=lAccountID, lInsuranceFileCnt:=lInsuranceFileCnt,
                                                            sDocumentRef:=sDocumentRef, r_vResultArray:=r_vResultArray,
                                                             lTransDetailID:=0, lCashListItemId:=0, v_bUseDocumentRef:=v_bUseDocumentRef)
    End Function

    Public Function GetTransactionsForAllocatedCashListItem(ByVal lAccountID As Integer, ByVal lInsuranceFileCnt As Integer,
                                                            ByVal sDocumentRef As String, ByRef r_vResultArray As Object,
                                                            ByVal lTransDetailID As Integer, ByVal lCashListItemId As Integer, ByVal v_bUseDocumentRef As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetTransactionsForAllocatedCashListItem
        ' PURPOSE:
        ' AUTHOR: Danny Davis
        ' DATE: 31 January 2005, 11:49:44
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("account_id", CStr(lAccountID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                .Parameters.Add("insurance_file_cnt", CStr(lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                If v_bUseDocumentRef Then
                    .Parameters.Add("document_ref", sDocumentRef, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                End If
                If lTransDetailID <> 0 Then
                    .Parameters.Add("transdetail_id", CStr(lTransDetailID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                Else
                    .Parameters.Add("transdetail_id", CStr(m_lCashListTransactionID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If
                If lCashListItemId <> 0 Then
                    .Parameters.Add("cashlistitem_id", CStr(lCashListItemId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                End If
                m_lReturn = .SQLSelect("spu_ACT_Select_DebtForAllocatedCashListItem", "Select_DebtForAllocatedCashListItem", True, , r_vResultArray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception(m_lReturn.ToString() + ", GetTransactionsForAllocatedCashListItem, SQLSelect failed.")
                End If
            End With


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionsForAllocatedCashListItem", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

            End Select

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPreferredDocumentDate
    '
    ' Description: If suspended trans to be released need document date of payment
    '
    ' ***************************************************************** '
    Private Function GetPreferredDocumentDate(ByVal v_lCashListItemID As Integer, ByRef r_vPreferredDocumentDate As String) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("cashlistitem_id", CStr(v_lCashListItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("document_date", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMDate)
            m_lReturn = .SQLSelect("spu_ACT_Get_Preferred_Document_Date", "Get Preferred Document Date", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call spu_ACT_Get_Preferred_Document_Date", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPreferredDocumentDate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            End If


            If Convert.IsDBNull(.Parameters.Item("document_date").Value) Or Informations.IsNothing(.Parameters.Item("document_date").Value) Then
                r_vPreferredDocumentDate = Nothing
            Else
                r_vPreferredDocumentDate = .Parameters.Item("document_date").Value
            End If
        End With

        Return result

    End Function

    Private Function GetDaysDelay(ByRef iCashListTypeID As Integer, ByRef lBankAccountID As Integer, ByRef lMediaTypeID As Integer) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetDaysDelay
        ' PURPOSE: Returns the days delay for a particular media type on a Bank Account
        ' AUTHOR: Danny Davis
        ' DATE: 02 October 2006, 16:01:20
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        Dim vResults As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        If iCashListTypeID = gACTLibrary.ACTCashListTypeReceipts Then

            m_lReturn = m_oBankAccount.SelectBankAccountDelay(lBankAccountID:=lBankAccountID, lMediaTypeID:=lMediaTypeID, r_vBankAccountDelay:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", " + +", Error running bACTCashListPost.Automated.GetDaysDelay")
            ElseIf Informations.IsArray(vResults) Then
                'We only support one record per bank account so pick the first one

                result = gPMFunctions.ToSafeInteger(CInt(vResults(2, 0)))
            Else
                'Payments are not currently supported
                result = 0
            End If
        Else
            'Payments are not currently supported
            result = 0
        End If


        Return result


    End Function

    ' ***************************************************************** '
    ' Name: GetPostingStatusForCashListItem
    '
    ' Parameters: CashListItemID
    '
    ' Description:
    '
    ' History:
    '           Created : Gautam Poddar : Date : 11 June 2009
    ' ***************************************************************** '
    Private Function GetPostingStatusForCashListItem(ByVal v_lCashListItemID As Integer, ByRef r_bIsPosted As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPostingStatusForCashListItem"




        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("cashlistitem_id", CStr(v_lCashListItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'developer guide no.85
            .Parameters.Add("IsPosted", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = .SQLSelect(sSQL:=ACGetPostingStatusForCashListItemSQL, sSQLName:=ACGetPostingStatusForCashListItemName, bStoredProcedure:=ACGetPostingStatusForCashListItemStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetPostingStatusForCashListItemName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If Convert.IsDBNull(.Parameters.Item("IsPosted").Value) Or Informations.IsNothing(.Parameters.Item("IsPosted").Value) Then

                r_bIsPosted = Nothing
            Else
                r_bIsPosted = gPMFunctions.ToSafeBoolean(.Parameters.Item("IsPosted").Value)
            End If
        End With


        Return result
    End Function

    Private Function AllocateSplitReceipt(ByVal v_iLeadAccountID As Integer, ByVal iLeadTransDetailID As Integer, ByVal vTransactions(,) As Object) As Integer
        Dim lResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim vKeyArray(,) As Object
            Dim oAllocationManual As bACTAllocationManual.Business
            Dim vAllocationtrans(0) As Object



            oAllocationManual = New bACTAllocationManual.Business
            m_lReturn = oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For lRow As Integer = 0 To vTransactions.GetUpperBound(1)

                vAllocationtrans(0) = CStr(vTransactions(0, lRow)) & "|" & CStr(vTransactions(1, lRow))

                vKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 3}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = v_iLeadAccountID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(iLeadTransDetailID) & "|" & CStr(-Math.Abs(CDbl(vTransactions(1, lRow))))

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationtrans

                With oAllocationManual

                    m_lReturn = .SetKeys(vKeyArray:=vKeyArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Start()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Error Msg goes here
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                End With
            Next

            oAllocationManual.Dispose()
            oAllocationManual = Nothing

        Catch ex As Exception
            ' Error Msg gors here
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try


        Return lResult

    End Function
    ''' <summary>
    ''' AllocateTPInstalment
    ''' </summary>
    ''' <param name="v_iLeadAccountID"></param>
    ''' <param name="iLeadTransDetailID"></param>
    ''' <param name="vTransactions"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AllocateTPInstalment(ByVal v_iLeadAccountID As Integer, ByVal iLeadTransDetailID As Integer, ByVal vTransactions(,) As Object) As Integer
        Dim lResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try
            Dim vKeyArray(,) As Object
            Dim oAllocationManual As bACTAllocationManual.Business
            Dim vAllocationtrans(0) As Object



            oAllocationManual = New bACTAllocationManual.Business
            m_lReturn = oAllocationManual.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For lRow As Integer = 0 To vTransactions.GetUpperBound(1)
                'SRP
                vAllocationtrans(0) = CStr(vTransactions(0, lRow)) & "|" & CStr(vTransactions(1, lRow))

                vKeyArray = Array.CreateInstance(GetType(Object), New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue - gPMConstants.PMENavLetGetKeyColPosition.PMKeyName + 1, 3}, New Integer() {gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0})


                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAccountID
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
                'IND
                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = v_iLeadAccountID

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = CStr(iLeadTransDetailID) & "|" & CStr(-(CDbl(vTransactions(1, lRow))))

                vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vAllocationtrans

                With oAllocationManual

                    m_lReturn = .SetKeys(vKeyArray:=vKeyArray)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    m_lReturn = .Start()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'Error Msg goes here
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                End With
            Next

            oAllocationManual.Dispose()
            oAllocationManual = Nothing

        Catch ex As Exception
            ' Error Msg gors here
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try


        Return lResult

    End Function

    ''' <summary>
    ''' GetTransDetailTypeID
    ''' </summary>
    ''' <param name="sTransdetailTypeCode"></param>
    ''' <param name="r_nTransdetailTypeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetTransDetailTypeID(
            ByVal sTransdetailTypeCode As String,
            ByRef r_nTransdetailTypeID As Integer) As Integer

        Dim oTransDetailTypeId As Object(,) = Nothing
        Dim nResult As Integer = 0

        Const kGetTransDetailTypeIDStored As Boolean = False
        Const kGetTransDetailTypeIDName As String = "GetTransDetailTypeID"
        Const kGetTransDetailTypeIDSQL As String = "SELECT transdetail_type_id from transdetail_type where code = {code}"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                .Parameters.Add("code", CStr(sTransdetailTypeCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                m_lReturn = .SQLSelect(sSQL:=kGetTransDetailTypeIDSQL, sSQLName:=kGetTransDetailTypeIDName, bStoredProcedure:=kGetTransDetailTypeIDStored, vResultArray:=oTransDetailTypeId)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    RaiseError("GetTransDetailTypeID", "GetTransDetailTypeID Failed to get TransdetailTypeID - bACTImportSiriusTrans.Form", Constants.vbObjectError)
                End If
            End With

            If Informations.IsArray(oTransDetailTypeId) Then
                r_nTransdetailTypeID = NullToLong(oTransDetailTypeId(0, 0))
            Else
                r_nTransdetailTypeID = 1 'Default to Journal
            End If

            Return nResult
        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            Return nResult
        End Try
    End Function


    ''' <summary>
    ''' AddReInsurerPaymentReciept
    ''' </summary>
    ''' <param name="nCompanyID"></param>
    ''' <param name="nTaxBandID"></param>
    ''' <param name="nCurrencyID"></param>
    ''' <param name="crAmount"></param>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nRIPartyCnt"></param>
    ''' <param name="sDocumentRef"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddReInsurerPaymentReciept(ByVal nCompanyID As Integer,
                                                ByVal nTaxBandID As Integer,
                                                ByVal nCurrencyID As Integer,
                                                ByVal crAmount As Double,
                                                ByVal nInsuranceFileCnt As Integer,
                                                ByVal nRIPartyCnt As Integer,
                                                ByVal sDocumentRef As String) As Integer


        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("nCompanyID", nCompanyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            .Parameters.Add("nTaxBandID", nTaxBandID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            .Parameters.Add("nCurrencyID", nCurrencyID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            .Parameters.Add("crAmount", crAmount, PMEParameterDirection.PMParamInput, PMEDataType.PMDouble)
            .Parameters.Add("nInsuranceFileCnt", nInsuranceFileCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            .Parameters.Add("nRIPartyCnt", nRIPartyCnt, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            .Parameters.Add("sDocumentRef", sDocumentRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
            m_lReturn = .SQLAction(sSQL:=kAddRIPaymentRecieptSQL,
                                   sSQLName:=kAddRIPaymentRecieptName,
                                   bStoredProcedure:=kAddRIPaymentRecieptStored)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kAddRIPaymentRecieptSQL + " failed.")
            End If
        End With

        Return PMEReturnCode.PMTrue
    End Function

    ''' <summary>
    ''' GetAccountIdFormTaxBandId
    ''' </summary>
    ''' <param name="nTaxBandId"></param>
    ''' <param name="v_rAccountId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetAccountIdFormTaxBandId(ByVal nTaxBandId As Integer,
                                               ByRef v_rAccountId As Integer) As Integer


        Dim oResultArray As Object(,) = Nothing

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("nTaxBandID", nTaxBandId, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetAccountIdFormTaxBandIDSQL,
                                               sSQLName:=kGetAccountIdFormTaxBandIDName,
                                               bStoredProcedure:=kGetAccountIdFormTaxBandIDStored,
                                               vResultArray:=oResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception(kGetAccountIdFormTaxBandIDSQL + " failed.")
            End If
            If Informations.IsArray(oResultArray) Then
                v_rAccountId = oResultArray(0, 0)
            End If
        End With
        Return PMEReturnCode.PMTrue
    End Function

    ''' <summary>
    ''' GetParentNodeIdForTax
    ''' </summary>
    ''' <param name="r_nParentNodeID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetParentNodeIdForTax(ByRef r_nParentNodeID As Integer) As Integer

        Dim oResultArray(,) As Object = Nothing

        Try
            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .SQLSelect(sSQL:=kGetParentNodeIdForTaxSQL,
                                       sSQLName:=kGetParentNodeIdForTaxName,
                                       bStoredProcedure:=kGetParentNodeIdForTaxStored,
                                       lNumberRecords:=gPMConstants.PMAllRecords,
                                       vResultArray:=oResultArray)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kGetParentNodeIdForTaxSQL + " failed.")
                End If
            End With

            If Informations.IsArray(oResultArray) Then
                r_nParentNodeID = ToSafeLong(oResultArray(0, 0))
            Else
                Return PMEReturnCode.PMNotFound
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function
    ''' <summary>
    ''' GetFinancerAccountID
    ''' </summary>
    ''' <param name="r_nFinancerAccountId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetFinancerAccountID(ByVal v_sPlanRef As String, ByRef r_nFinancerAccountId As Integer) As Integer

        Dim oResultArray(,) As Object = Nothing

        Try
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("PlanRef", v_sPlanRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                m_lReturn = .SQLSelect(sSQL:=kGetFinancerAccountIdSQL,
                                       sSQLName:=kGetFinancerAccountIdName,
                                       bStoredProcedure:=kGetFinancerAccountIdStored,
                                       lNumberRecords:=gPMConstants.PMAllRecords,
                                       vResultArray:=oResultArray)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kGetFinancerAccountIdSQL + " failed.")
                End If
            End With

            If Informations.IsArray(oResultArray) Then
                r_nFinancerAccountId = ToSafeLong(oResultArray(0, 0))
            Else
                Return PMEReturnCode.PMNotFound
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function
    ''' <summary>
    ''' GetPolicyTransDetail
    ''' </summary>
    ''' <param name="v_sPlanRef"></param>
    ''' <param name="v_nTPTransdetailID"></param>
    ''' <param name="v_dbaseAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPolicyTransDetail(ByVal v_sPlanRef As String, ByRef v_nTPTransdetailID As Integer, ByRef v_dbaseAmount As Double, ByRef v_nPremiumFinanceVersion As Integer) As Integer

        Dim oResultArray(,) As Object = Nothing

        Try
            With m_oDatabase
                .Parameters.Clear()
                .Parameters.Add("PlanRef", v_sPlanRef, PMEParameterDirection.PMParamInput, PMEDataType.PMString)
                m_lReturn = .SQLSelect(sSQL:=kGetPolicyTransdetailSQL,
                                       sSQLName:=kGetPolicyTransdetailName,
                                       bStoredProcedure:=kGetPolicyTransdetailStored,
                                       lNumberRecords:=gPMConstants.PMAllRecords,
                                       vResultArray:=oResultArray)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Throw New Exception(kGetPolicyTransdetailSQL + " failed.")
                End If
            End With

            If Informations.IsArray(oResultArray) Then
                v_nTPTransdetailID = ToSafeInteger(oResultArray(0, 0))
                v_dbaseAmount = ToSafeDouble(oResultArray(1, 0))
                v_nPremiumFinanceVersion = ToSafeInteger(oResultArray(2, 0))
            Else
                Return PMEReturnCode.PMNotFound
            End If

            Return PMEReturnCode.PMTrue
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return PMEReturnCode.PMError
        End Try
    End Function
    ''' <summary>
    ''' GetPlanTransactionOutstanding
    ''' </summary>
    ''' <param name="v_nPlanTrancationId"></param>
    ''' <param name="r_dOutstandingAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
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
    ''' GetPostingStatusForCashList
    ''' </summary>
    ''' <param name="nCashListID"></param>
    ''' <param name="r_bIsPosted"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetPostingStatusForCashList(ByVal nCashListID As Integer,
                                                 ByRef r_bIsPosted As Boolean) As Integer



        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("nCashlistID", nCashListID, PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            .Parameters.Add("nIsPosted", DBNull.Value, PMEParameterDirection.PMParamOutput, PMEDataType.PMInteger)

            m_lReturn = .SQLSelect(sSQL:=kGetPostingStatusForCashListSQL,
                                    sSQLName:=kGetPostingStatusForCashListName,
                                    bStoredProcedure:=kGetPostingStatusForCashListStored)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New System.Exception("")
            End If

            If (.Parameters.Item("nIsPosted").Value) Is Nothing Then
                r_bIsPosted = False
            Else
                r_bIsPosted = ToSafeBoolean(.Parameters.Item("nIsPosted").Value)
            End If
        End With

        Return PMEReturnCode.PMTrue

    End Function

    ''' <summary>
    ''' Check if an account exists
    ''' </summary>
    ''' <param name="sShortCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DoAccountExists(ByVal sShortCode As String) As Integer
        Dim oResultArray(,) As Object = Nothing

        With m_oDatabase
            .Parameters.Clear()
            m_lReturn = .Parameters.Add(
                        sName:="short_code",
                        vValue:=sShortCode,
                        iDirection:=PMEParameterDirection.PMParamInput,
                        iDataType:=PMEDataType.PMString)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to add paramter short_code")
            End If

            m_lReturn = .SQLSelect(sSQL:=kDoAccountExistsSQL,
                                   sSQLName:=kDoAccountExistsName,
                                   bStoredProcedure:=kDoAccountExistsStored,
                                   lNumberRecords:=gPMConstants.PMAllRecords,
                                   vResultArray:=oResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Throw New Exception("Failed to add paramter short_code")
            End If
        End With

        If Informations.IsArray(oResultArray) Then
            Return PMEReturnCode.PMTrue
        Else
            Return PMEReturnCode.PMNotFound
        End If

    End Function



End Class
