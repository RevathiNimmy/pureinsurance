Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Form_NET.Form")> _
Public NotInheritable Class Form
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ************************************************
    ' Added to replace global variables 27/11/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************


    ' ***************************************************************** '
    ' Class Name: FindCashList
    '
    ' Date: 3rd September 1997
    '
    ' Description: Creatable class used by the FindCashList interface.
    '
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' Constants for use in SQL
    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTypeOfBusiness As New FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Component Sub Type
    Private m_sSubType As New FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PRIVATE Data Members (End)

    'Select CashList from full query
    ' RAM20040514 : Added MaxRowsToFetch Parameter
    Private Const ACFindCashListItemStored As Boolean = True
    Private Const ACFindReceiptCashListItemName As String = "SelectReceiptQuery"
    'developer guide no. 39
    Private Const ACFindReceiptCashListItemSQL As String = "spu_ACT_Do_FindReceipt"

    'added payment maintenance sw 06-11-2002
    ' RAM20040514 : Added MaxRowsToFetch Parameter
    Private Const ACFindPaymentCashListItemSQL As String = "spu_ACT_Do_FindPayment"
    Private Const ACFindPaymentCashListItemName As String = "SelectPaymentQuery"

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

    Public ReadOnly Property TypeOfBusiness() As String
        Get

            Return m_sTypeOfBusiness.Value

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

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


            If Not Information.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness.Value = CStr(vTypeOfBusiness)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values as defined by vTableArray.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing

            ' Get the Lookup items from the Business Component

            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTableArray, iLanguageID:=iLanguageID, dtEffectiveDate:=m_dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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

            ' Get Reference to Database
            'Set m_oDatabase = GetOrionDatabase( _
            'lOpenStatus:=m_lReturn, _
            'bCloseDatabase:=m_bCloseDatabase, _
            'vDatabase:=vDatabase)

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            Return result

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    '****************************************************************** '
    ' Name: SearchReceiptDetails (Public)
    '
    ' Description: Selects CashLists according to the query by given
    '              parameters
    ' Edit History  :
    ' RAM20040514   : Performance Enhancement changes. We pass the number of
    '                   records to fetch as a parameter, so that SQL server
    '                   can fetch those many rows only.
    '****************************************************************** '
    Public Function SearchReceiptDetails(ByRef r_lNumberOfRecords As Integer, ByRef r_vResultArray(,) As Object, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vReceiptTypeId As Object = Nothing, Optional ByVal v_vAccount As Object = Nothing, Optional ByVal v_vMediaReference As Object = Nothing, Optional ByVal v_vTheirReference As Object = Nothing, Optional ByVal v_vAmount As Object = Nothing, Optional ByVal v_vBatchNumber As Object = Nothing, Optional ByVal v_vMediaTypeId As Object = Nothing, Optional ByVal v_vReceiptNumber As Object = Nothing, Optional ByVal v_vBatchReference As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the start_date parameter (INPUT)

            If Information.IsNothing(v_vStartDate) Then


                v_vStartDate = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="start_date", vValue:=CStr(v_vStartDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the end_date parameter (INPUT)

            If Information.IsNothing(v_vEndDate) Then


                v_vEndDate = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="end_date", vValue:=CStr(v_vEndDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the cashlistitem_receipt_type_id parameter (INPUT)

            If Information.IsNothing(v_vReceiptTypeId) Then


                v_vReceiptTypeId = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="cashlistitem_receipt_type_id", vValue:=CStr(v_vReceiptTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the account_name parameter (INPUT)

            If Information.IsNothing(v_vAccount) Then


                v_vAccount = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="account_name", vValue:=CStr(v_vAccount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the media_reference parameter (INPUT)

            If Information.IsNothing(v_vMediaReference) Then


                v_vMediaReference = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="media_reference", vValue:=CStr(v_vMediaReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the their_reference parameter (INPUT)

            If Information.IsNothing(v_vTheirReference) Then


                v_vTheirReference = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="their_reference", vValue:=CStr(v_vTheirReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the amount parameter (INPUT)

            If Information.IsNothing(v_vAmount) Then


                v_vAmount = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="amount", vValue:=CStr(v_vAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the batch_number parameter (INPUT)

            If Information.IsNothing(v_vBatchNumber) Then


                v_vBatchNumber = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="batch_number", vValue:=CStr(v_vBatchNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the mediatype_id parameter (INPUT)

            If Information.IsNothing(v_vMediaTypeId) Then


                v_vMediaTypeId = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="mediatype_id", vValue:=CStr(v_vMediaTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the receipt_number parameter (INPUT)

            If Information.IsNothing(v_vReceiptNumber) Then


                v_vReceiptNumber = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="receipt_number", vValue:=CStr(v_vReceiptNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            ' Add the batch_reference parameter (INPUT)

            If Information.IsNothing(v_vBatchReference) Then


                v_vBatchReference = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="batch_ref", vValue:=CStr(v_vBatchReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            If m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040514 : Performance Enhancement Changes. - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If m_oDatabase.Parameters.Add(sName:="MaxRowsToFetch", vValue:=CStr(r_lNumberOfRecords), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchReceiptDetails", "m_oDatabase.Parameters.Add Failed.")
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACFindReceiptCashListItemSQL, sSQLName:=ACFindReceiptCashListItemName, bStoredProcedure:=ACFindCashListItemStored, vResultArray:=r_vResultArray)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040514 : Performance Enhancement Changes. - END
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchReceiptDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return CheckResults(r_vResultArray)
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchReceiptDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchReceiptDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        End Try

    End Function


    '****************************************************************** '
    ' Name: SearchReceiptDetails (Public)
    '
    ' Description: Selects CashLists according to the query by given
    '              parameters
    ' Edit History  :
    ' RAM20040514   : Performance Enhancement changes. We pass the number of
    '                   records to fetch as a parameter, so that SQL server
    '                   can fetch those many rows only.
    '****************************************************************** '

    Public Function SearchPaymentDetails(ByRef r_lNumberOfRecords As Object, ByRef r_vResultArray(,) As Object, Optional ByVal v_vPayeeName As Object = Nothing, Optional ByVal v_vAccountID As Object = Nothing, Optional ByVal v_vPaymentTypeID As Object = Nothing, Optional ByVal v_vPaymentMediaTypeID As Object = Nothing, Optional ByVal v_vChequeEFTNo As Object = Nothing, Optional ByVal v_vPaymentStatusID As Object = Nothing, Optional ByVal v_vAmount As Object = Nothing, Optional ByVal v_vBatchNumber As Object = Nothing, Optional ByVal v_vBatchReference As Object = Nothing, Optional ByVal v_vBranchID As Object = Nothing, Optional ByVal v_vBankAccountID As Object = Nothing, Optional ByVal v_vClientCode As Object = Nothing, Optional ByVal v_vClientAccountNumber As Object = Nothing, Optional ByVal v_vPolicyClaimNumber As Object = Nothing, Optional ByVal v_vMediaFrom As Object = Nothing, Optional ByVal v_vMediaTo As Object = Nothing, Optional ByVal v_vAmountFrom As Object = Nothing, Optional ByVal v_vAmountTo As Object = Nothing, Optional ByVal v_vStartDate As Object = Nothing, Optional ByVal v_vEndDate As Object = Nothing, Optional ByVal v_vShowOnlyOutStanding As Object = Nothing, Optional ByVal v_vUserID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the payee_name parameter (INPUT)

            If Information.IsNothing(v_vPayeeName) Or (v_vPayeeName = "") Then

                'developer guide no. 101
                v_vPayeeName = DBNull.Value
            End If
            If m_oDatabase.Parameters.Add(sName:="payee_name", vValue:=v_vPayeeName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the end_date parameter (INPUT)

            If Information.IsNothing(v_vAccountID) Then
                v_vAccountID = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="account_ID", vValue:=v_vAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the cashlistitem_receipt_type_id parameter (INPUT)

            If Information.IsNothing(v_vPaymentTypeID) Or (v_vPaymentTypeID = -1) Then
                v_vPaymentTypeID = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="payment_type_id", vValue:=v_vPaymentTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the account_name parameter (INPUT)

            If Information.IsNothing(v_vPaymentMediaTypeID) Or (v_vPaymentMediaTypeID = -1) Then

                v_vPaymentMediaTypeID = DBNull.Value

            End If
            If m_oDatabase.Parameters.Add(sName:="payment_media_type_id", vValue:=v_vPaymentMediaTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the media_reference parameter (INPUT)

            If Information.IsNothing(v_vChequeEFTNo) Then

                v_vChequeEFTNo = DBNull.Value

            End If

            If m_oDatabase.Parameters.Add(sName:="media_reference", vValue:=v_vChequeEFTNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the their_reference parameter (INPUT)

            If Information.IsNothing(v_vPaymentStatusID) Or (v_vPaymentStatusID = -1) Then

                v_vPaymentStatusID = DBNull.Value
            End If
            If m_oDatabase.Parameters.Add(sName:="payment_status_id", vValue:=v_vPaymentStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the amount parameter (INPUT)

            If Information.IsNothing(v_vAmount) Then


                v_vAmount = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="amount", vValue:=v_vAmount, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the batch_number parameter (INPUT)

            If Information.IsNothing(v_vBatchNumber) Then
                v_vBatchNumber = DBNull.Value
            End If

            If m_oDatabase.Parameters.Add(sName:="batch_number", vValue:=v_vBatchNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add the batch_number parameter (INPUT)

            If Information.IsNothing(v_vBatchReference) Or (v_vBatchReference = "") Then

                v_vBatchReference = DBNull.Value
            End If
            If m_oDatabase.Parameters.Add(sName:="batch_ref", vValue:=v_vBatchReference, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            If m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Add Additional Parameters for Payment Maintenance
            ' Branch ID

            If Information.IsNothing(v_vBranchID) Or (v_vBranchID = -1) Then

                v_vBranchID = DBNull.Value
            End If
            If m_oDatabase.Parameters.Add(sName:="branch", vValue:=v_vBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Bank ID

            If Information.IsNothing(v_vBankAccountID) Or (v_vBankAccountID = -1) Then

                v_vBankAccountID = DBNull.Value
            End If
            If m_oDatabase.Parameters.Add(sName:="bankaccountid", vValue:=v_vBankAccountID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Client Code

            If (Information.IsNothing(v_vClientCode)) Or (v_vClientCode = "") Then

                v_vClientCode = DBNull.Value
            End If
            ' Add the clientcode parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="clientcode", vValue:=v_vClientCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Client Account Number

            If (Information.IsNothing(v_vClientAccountNumber)) Or (v_vClientAccountNumber = "") Then

                v_vClientAccountNumber = DBNull.Value
            End If
            ' Add the client_account_number parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="client_account_number", vValue:=v_vClientAccountNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Policy/ClaimNumber

            If (Information.IsNothing(v_vPolicyClaimNumber)) Or (v_vPolicyClaimNumber = "") Then

                v_vPolicyClaimNumber = DBNull.Value
            End If
            ' Add the policy_claim_number parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="policy_claim_number", vValue:=v_vPolicyClaimNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If
            ' Media From
            If (Information.IsNothing(v_vMediaFrom)) Or (v_vMediaFrom = "") Then

                v_vMediaFrom = DBNull.Value
            End If
            ' Add the media_from parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="media_from", vValue:=v_vMediaFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Media To

            If (Information.IsNothing(v_vMediaTo)) Or (v_vMediaTo = "") Then

                v_vMediaTo = DBNull.Value
            End If

            ' Add the media_to parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="media_to", vValue:=v_vMediaTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Amount From

            If (Information.IsNothing(v_vAmountFrom)) Or (v_vAmountFrom = 0) Then

                v_vAmountFrom = DBNull.Value
            End If
            ' Add the amount_from parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="amount_from", vValue:=v_vAmountFrom, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Amount To

            If (Information.IsNothing(v_vAmountTo)) Or (v_vAmountTo = 0) Then

                v_vAmountTo = DBNull.Value
            End If
            ' Add the amount_to parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="amount_to", vValue:=v_vAmountTo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' StartDate

            If Information.IsNothing(v_vStartDate) OrElse v_vStartDate = #12/30/1899# Then

                v_vStartDate = DBNull.Value
            Else
                If StringsHelper.ToDoubleSafe(v_vStartDate) = 0 Then

                    v_vStartDate = DBNull.Value
                Else
                    Dim TempDate As Date
                    v_vStartDate = IIf(DateTime.TryParse(v_vStartDate, TempDate), TempDate.ToString("yyyy.MM.dd HH:mm:ss"), v_vStartDate)
                End If
            End If

            'Add the StartDate parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="date_from", vValue:=v_vStartDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' EndDate

            If Information.IsNothing(v_vEndDate) OrElse v_vEndDate = #12/30/1899# Then

                v_vEndDate = DBNull.Value
            Else
                If StringsHelper.ToDoubleSafe(v_vEndDate) = 0 Then

                    v_vEndDate = DBNull.Value
                Else
                    Dim TempDate2 As Date
                    v_vEndDate = IIf(DateTime.TryParse(v_vEndDate, TempDate2), TempDate2.ToString("yyyy.MM.dd HH:mm:ss"), v_vEndDate)
                End If
            End If

            'Add the EndDate parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="date_to", vValue:=v_vEndDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            ' Show Only Outstanding
            If Information.IsNothing(v_vShowOnlyOutStanding) Then
                v_vShowOnlyOutStanding = DBNull.Value
            End If

            ' Add the showonlyoutstanding parameter (INPUT)
            If m_oDatabase.Parameters.Add(sName:="showonlyoutstanding", vValue:=v_vShowOnlyOutStanding, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            '''''''''''''''''''''End Additional Parameters

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040514 : Performance Enhancement Changes. - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            If m_oDatabase.Parameters.Add(sName:="MaxRowsToFetch", vValue:=r_lNumberOfRecords, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError("SearchPaymentDetails", "Parameters.Add Failed.")
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACFindPaymentCashListItemSQL, sSQLName:=ACFindPaymentCashListItemName, bStoredProcedure:=ACFindCashListItemStored, vResultArray:=r_vResultArray)

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040514 : Performance Enhancement Changes. - END
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchPaymentDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return CheckResults(r_vResultArray)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            MessageBox.Show(Information.Err().Description, Application.ProductName)
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchPaymentDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchPaymentDetails", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
        End Try
    End Function

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: CheckResults (Private)
    '
    ' Description: Checks the result array after a query
    '              If records found returns PMTrue
    '              If no records found returns PMNotFound
    '
    ' ***************************************************************** '
    Private Function CheckResults(ByRef vResultArray As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' If NO records were found return PMNotFound
        If Not Information.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

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
        ' Error Section.
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
