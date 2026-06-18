Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Automated_NET.Automated")> _
Public NotInheritable Class Automated

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 03/04/2007
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""
    Private m_sCallingAppName As String = ""
    Private m_iUserID As Integer
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Automated"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

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

    Private m_dtAccountingDate As Date
    Private m_lAccountID As Integer
    Private m_lAllocationID As Integer
    Private m_vTransDetailIdArray As Array
    Private m_lAllocationTransType As Integer

    Private m_vCashListItemID As Integer
    Private m_vCashListID As Integer = 0

    Private m_iCompanyID As Integer

    Private m_bAbortTrans As Boolean

    Private m_oAllocation As bACTAllocation.Form
    Private m_oAllocationDetail As bACTAllocationDetail.Form
    Private m_oAllocationCalculate As bACTAllocationCalculate.Form
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form
    Private m_oDocument As bACTDocument.Form
    Private m_oTransDetail As bACTTransDetail.Form
    Private m_oCashList As bACTCashList.Form
    Private m_oCashListItem As bACTCashlistitem.Form

    Private m_lBatchID As Integer
    Private m_nAllocationBatchID As Integer = 0


    ' PUBLIC Property Procedures (Begin)
    Public Property AccountingDate() As Date
        Get
            Return m_dtAccountingDate
        End Get
        Set(ByVal Value As Date)
            m_dtAccountingDate = Value
        End Set
    End Property
    Public Property AccountId() As Integer
        Get
            Return m_lAccountID
        End Get
        Set(ByVal Value As Integer)
            m_lAccountID = Value
        End Set
    End Property
    Public Property AllocationId() As Integer
        Get
            Return m_lAllocationID
        End Get
        Set(ByVal Value As Integer)
            m_lAllocationID = Value
        End Set
    End Property

    Public Property AllocationBatchID() As Integer
        Get
            Return m_nAllocationBatchID
        End Get
        Set(ByVal Value As Integer)
            m_nAllocationBatchID = Value
        End Set
    End Property

    Public Property TransDetailIdArray() As Object
        Get
            Return m_vTransDetailIdArray
        End Get
        Set(ByVal Value As Object)
            m_vTransDetailIdArray = Value
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

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property AbortTrans() As Boolean
        Get

            Return m_bAbortTrans

        End Get
        Set(ByVal Value As Boolean)

            m_bAbortTrans = Value

        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
        End Get
    End Property
    'Get the allocated base amounts and the allocated currency amounts from the database
    '
    Public Function GetAllocationAmounts(ByVal v_lAllocationID As Integer, ByRef r_cAllocBaseAmount As Decimal, ByRef r_cAllocCcyAmount As Decimal) As Integer
        Dim result As Integer = 0
        Dim vArray(,) As Object
        Dim sSQL As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            lReturn = m_oDatabase.Parameters.Add("Allocation_id", CStr(v_lAllocationID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllocationTotalSQL, sSQLName:="Get Allocation Totals", bStoredProcedure:=True, vResultArray:=vArray)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(vArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    r_cAllocBaseAmount = CDec(vArray(0, 0))
                Else
                    r_cAllocBaseAmount = 0
                End If


                Dim dbNumericTemp2 As Double
                If Double.TryParse(CStr(vArray(1, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    r_cAllocCcyAmount = CDec(vArray(1, 0))
                Else
                    r_cAllocCcyAmount = 0
                End If
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Err_GetAllocationAmounts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Err_GetAllocationAmounts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserId As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyId As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer




        Dim result As Integer = 0
        Try


            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserId
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyId
            m_iLogLevel = iLogLevel


            ' Initialisation Code.


            ' Set Username and Password

            ' Set UserID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            'Set m_oDatabase = GetOrionDatabase(m_lReturn, m_bCloseDatabase, vDatabase)

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFOrion, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bAbortTrans = True

            'Set m_oAllocation = GetOrionBusiness(v_sClassName:="bACTAllocation.Form", v_vDatabase:=m_oDatabase)

            m_oAllocation = New bACTAllocation.Form
            m_lReturn = m_oAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oAllocationDetail = GetOrionBusiness(v_sClassName:="bACTAllocationDetail.Form", v_vDatabase:=m_oDatabase)

            m_oAllocationDetail = New bACTAllocationDetail.Form
            m_lReturn = m_oAllocationDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oCurrencyConvert = GetOrionBusiness(v_sClassName:="bACTCurrencyConvert.Form", v_vDatabase:=m_oDatabase)

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oDocument = GetOrionBusiness(v_sClassName:="bACTDocument.Form", v_vDatabase:=m_oDatabase)

            m_oDocument = New bACTDocument.Form
            m_lReturn = m_oDocument.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oCashList = GetOrionBusiness(v_sClassName:="bACTCashList.Form", v_vDatabase:=m_oDatabase)

            m_oCashList = New bACTCashList.Form
            m_lReturn = m_oCashList.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oCashListItem = GetOrionBusiness(v_sClassName:="bACTCashListItem.Form", v_vDatabase:=m_oDatabase)

            m_oCashListItem = New bACTCashlistitem.Form
            m_lReturn = m_oCashListItem.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oTransDetail = GetOrionBusiness(v_sClassName:="bACTTransDetail.Form", v_vDatabase:=m_oDatabase)

            m_oTransDetail = New bACTTransDetail.Form
            m_lReturn = m_oTransDetail.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set m_oAllocationCalculate = GetOrionBusiness(v_sClassName:="bACTAllocationCalculate.Form", v_vDatabase:=m_oDatabase)

            m_oAllocationCalculate = New bACTAllocationCalculate.Form
            m_lReturn = m_oAllocationCalculate.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserId:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyId:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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


            Return result

        Catch excep As System.Exception



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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
                m_oAllocation = Nothing
                m_oAllocationDetail = Nothing
                m_oAllocationCalculate = Nothing
                m_oCurrencyConvert = Nothing
                m_oDocument = Nothing
                m_oTransDetail = Nothing
                m_oCashList = Nothing
                m_oCashListItem = Nothing

            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetBatchDetails
    '
    ' Description: Gets the details using the m_lBatchID key
    '
    ' ***************************************************************** '
    Public Function GetBatchDetails(Optional ByVal lBatchID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vTempArray(,) As Object
        Dim oNavBatch As bPMNavBatch.Business


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not True Then
                lBatchID = m_lBatchID
            End If


            ' Get an instance of the batch component

            oNavBatch = New bPMNavBatch.Business
            m_lReturn = oNavBatch.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Set the batch to retrieve

            oNavBatch.BatchSetID = lBatchID

            ' Get the batch details - TransDetailsIds

            m_lReturn = oNavBatch.GetBatchSet(r_vBatchArray:=vTempArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TF110903 - Exit cleanly if no valid batch (eg. cancelled Navigator)
            'PN6707
            If Not Information.IsArray(vTempArray) Then

                oNavBatch.Dispose()
                oNavBatch = Nothing
                Return result
            End If


            m_vTransDetailIdArray = Array.CreateInstance(GetType(Object), New Integer() {vTempArray.GetUpperBound(1) - vTempArray.GetLowerBound(1) + 1}, New Integer() {vTempArray.GetLowerBound(1)})

            ' Convert the batch data to transdetailids array

            For iLoop1 As Integer = vTempArray.GetLowerBound(1) To vTempArray.GetUpperBound(1)

                m_vTransDetailIdArray(iLoop1) = vTempArray(2, iLoop1)
            Next iLoop1

            ' MOVED to after the navigator process is complete

            ' Remove the batch from the database
            '    m_lReturn& = oNavBatch.DeleteBatchSet(v_lBatchSetID:=m_lBatchID)
            '    If (m_lReturn& <> PMTrue) Then
            '        GetBatchDetails = PMFalse
            '    End If

            ' Terminate the object and remove its instance

            oNavBatch.Dispose()
            oNavBatch = Nothing
            ' Remove component services

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBatchDetailsFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
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
            If Not Information.IsArray(vKeyArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    ' Step through the key array.
            For lRow As Integer = vKeyArray.GetLowerBound(1) To vKeyArray.GetUpperBound(1)

                '        ' Assign the parameter member with the
                '        ' correct key array item.


                Select Case CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, lRow)).Trim()
                    ' CF031298 - Changed TransDetailIDs for BatchID
                    'Case ACTKeyNameTransDetailIDs
                    '    m_vTransDetailIdArray = vkeyarray(PMKeyValue, lRow&)
                    Case PMNavKeyConst.PMKeyNameBatchID

                        m_lBatchID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAccountID

                        m_lAccountID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAccountingDate

                        m_dtAccountingDate = CDate(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck040601 replace Cint with cLng
                    Case PMNavKeyConst.ACTKeyNameAllocationTransType

                        m_lAllocationTransType = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameAllocationId

                        m_lAllocationID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListId

                        m_vCashListID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                    Case PMNavKeyConst.ACTKeyNameCashListItemId

                        m_vCashListItemID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                        'eck100500
                    Case PMNavKeyConst.ACTKeyNameBranchID

                        m_iCompanyID = CInt(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, lRow))
                End Select

            Next lRow

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Initialise the key array with the number of
            ' keys needed to be returned.
            ' Note: Remember arrays are zero based.
            ReDim vKeyArray(1, 0)

            ' Assign the key array with the parameter members.

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 0) = PMNavKeyConst.ACTKeyNameAllocationId

            vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0) = m_lAllocationID

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeys", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Start (Public)
    '
    ' Description: Performs the Automated Action dependant on the Task
    '              Process Mode etc.
    ' ***************************************************************** '
    Public Function Start() As Integer


        Dim result As Integer = 0
        Try

            ' Get the batch ID and transdetailids
            m_lReturn = CType(GetBatchDetails(lBatchID:=m_lBatchID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.SQLBeginTrans()

            '    If (m_vCashListId <> 0) Then
            '        m_lReturn& = CreateAllocationForCashlist( _
            ''                        v_vCashListID:=m_vCashListId, _
            ''                        r_lAllocationId:=m_lAllocationId&)
            '
            '    Else
            '
            m_lReturn = CType(CreateAllocation(), gPMConstants.PMEReturnCode)
            '
            '    End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                m_oDatabase.SQLCommitTrans()
                result = gPMConstants.PMEReturnCode.PMTrue
            End If

            ' Save the allocation_id

            ' Create a new instance of component services

            ' Update the temp storage value
            m_lReturn = CType(gPMComponentServices.UpdateUserProperty(v_sUsername:=m_sUsername, v_sPropertyName:=PMNavKeyConst.ACTKeyNameAllocationId, v_vPropertyValue:=m_lAllocationID), gPMConstants.PMEReturnCode)

            ' Remove the instance

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Start", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    '''   Create an Allocation header if one does not already exist
    ''' then create one allocation detail per transaction id passed
    ''' in vTransIDArray
    ''' </summary>
    ''' <param name="v_dtAccountingDate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateAllocation(Optional ByVal v_dtAccountingDate As Date = #12/30/1899#) As Integer

        Dim nResult As Integer
        Dim nAllocationId As Integer
        Dim nCompanyId As Integer
        Dim nUserId As Integer
        Dim nAccountId As Integer
        Dim nTransDetailId As Integer
        Dim dtAccountingDate As Date
        Dim nAllocationStatusId As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If m_iCompanyID > 0 Then
                nCompanyId = m_iCompanyID
            Else
                nCompanyId = m_iSourceID
            End If
            nUserId = m_iUserID
            nAccountId = m_lAccountID

            If Information.IsNothing(v_dtAccountingDate) Then
                dtAccountingDate = m_dtAccountingDate
            Else
                dtAccountingDate = v_dtAccountingDate
            End If

            If dtAccountingDate = #12/30/1899# Then
                dtAccountingDate = DateTime.Now
            End If

            nAllocationStatusId = gACTLibrary.ACTAllocationStatusUnallocated

            nAllocationId = m_lAllocationID

            If m_nAllocationBatchID = 0 Then
                m_lReturn = CreateAllocationBatch(0, m_nAllocationBatchID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If
            End If

            If nAllocationId = 0 Then

                ' Create a new allocation header (DirectAdd to get the Id for the detail)

                m_lReturn = m_oAllocation.DirectAdd(vAllocationId:=nAllocationId, vCompanyID:=nCompanyId, vAccountID:=nAccountId, vUserID:=nUserId, _
                                                    vAllocationDate:=dtAccountingDate, vAllocationstatusId:=nAllocationStatusId, _
                                                    r_nAllocationBatchID:=m_nAllocationBatchID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                ' Save the value to pass back out
                m_lAllocationID = nAllocationId

            End If

            ' Read back the details for later

            m_lReturn = m_oAllocation.GetDetails(vAllocationId:=nAllocationId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' For each transaction id specified create an allocation detail

            If Not Information.IsArray(m_vTransDetailIdArray) Then
                Return nResult
            End If

            For lTransDetail As Integer = 0 To m_vTransDetailIdArray.GetUpperBound(0)

                ' Set if primary or secondary allocation
                If lTransDetail = 0 Then
                    m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation
                Else
                    m_lAllocationTransType = gACTLibrary.ACTSecondaryForAllocation
                End If

                nTransDetailId = CInt(m_vTransDetailIdArray(lTransDetail))

                ' Pass a cashlisteitem if we have one

                If m_vCashListItemID.Equals(0) Then
                    m_lReturn = CreateAllocationDetail(v_lTransDetailId:=nTransDetailId, v_lAllocationID:=nAllocationId)
                Else
                    m_lReturn = CreateAllocationDetail(v_lTransDetailId:=nTransDetailId, v_lAllocationID:=nAllocationId, v_vCashListItemId:=m_vCashListItemID)
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

            Next lTransDetail

            Return nResult

        Catch excep As System.Exception
            ' Error.
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAllocation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function
    'eck110102 Added optional return parameter for allocation detail ID
    Public Function CreateAllocationDetail(ByVal v_lTransDetailId As Object, _
                                           ByVal v_lAllocationID As Object, _
                                           Optional ByVal v_vCashListItemId As Object = Nothing, _
                                           Optional ByRef v_vAllocationDetailId As Integer = 0, _
                                           Optional ByVal crCashTaxAmount As Decimal = 0, _
                                           Optional ByVal bAllocatingCashTaxAmount As Boolean = False) As PMEReturnCode


        Dim nResult As PMEReturnCode = PMEReturnCode.PMFalse
        Dim lAllocationDetailId As Integer
        Dim iCompanyId, iCurrencyId As Integer
        Dim dtAccountingDate As Date
        Dim vdCurrencyBaseXRate As Object
        Dim cAllocBaseAmount, cAllocCcyAmount, cNewOsCcyAmount, cNewOsBaseAmount, cLossGainAmount As Decimal
        Dim iFullyMatched As Integer
        Dim iIsPrimary As PMEReturnCode
        Dim vDocumentTypeID, vDocumentRef, vOriginalDate As Object
        Dim vOrigBaseAmount, vOrigBaseAmountUnrounded As Object ' RAW 12/03/2003 : ISS2893 : added
        Dim vOrigCcyAmount, vOrigCcyAmountUnrounded As Object ' RAW 12/03/2003 : ISS2893 : added
        Dim vOrigXRate, vOSBaseAmount, vOSCcyAmount As Object
        Dim crAllocAccountAmount As Decimal
        Dim crAllocSystemAmount As Decimal
        Try

            nResult = PMEReturnCode.PMTrue
            dtAccountingDate = m_oAllocation.Details.Item(1).AllocationDate
            ' Get the Transaction specified
            m_lReturn = CType(GetTransDetail(v_lTransDetailId:=CInt(v_lTransDetailId)), PMEReturnCode)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFail
            End If


            With m_oTransDetail.Details.Item(1)
                ' And the document that owns the transaction
                m_lReturn = CType(GetDocument(v_lDocumentId:=.DocumentId), PMEReturnCode)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If

                iCurrencyId = .CurrencyID
                iCompanyId = .CompanyID
                'Get the effective exchange rate to use for this allocation

                m_lReturn = CType(GetXRateForCurrency(v_iCurrencyID:=iCurrencyId, _
                                                      v_iCompanyID:=iCompanyId, _
                                                      r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, _
                                                      v_dtAccountingDate:=dtAccountingDate), PMEReturnCode)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If



                m_lReturn = CType(GetAllocationAmounts(v_lAllocationID:=CInt(v_lAllocationID), _
                                                       r_cAllocBaseAmount:=cAllocBaseAmount, _
                                                       r_cAllocCcyAmount:=cAllocCcyAmount), PMEReturnCode)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If

                If bAllocatingCashTaxAmount = False And crCashTaxAmount <> 0 Then
                    cAllocBaseAmount = cAllocBaseAmount - crCashTaxAmount
                    cAllocCcyAmount = cAllocCcyAmount - (crCashTaxAmount / vdCurrencyBaseXRate)
                End If

                ' CTAF TODO 031298 - Uncomment when Currency objects work properly
                ' RAW 12/03/2003 : ISS2893 : reactivated
                m_lReturn = m_oAllocationCalculate.CalculateValues(v_iOriginalCurrency:=iCurrencyId, _
                                                                   v_lCompanyID:=iCompanyId, _
                                                                   v_iAllocateToBase:=PMEReturnCode.PMTrue, _
                                                                   v_vdOrigXrate:=.CurrencyBaseXrate, _
                                                                   v_vdEffectiveXrate:=.CurrencyBaseXrate, _
                                                                   v_cOsBaseAmount:=.OSBaseAmount, _
                                                                   v_cOsCcyAmount:=.OSCurrencyAmount, _
                                                                   r_cAllocBaseAmount:=cAllocBaseAmount, _
                                                                   r_cAllocCcyAmount:=cAllocCcyAmount, _
                                                                   r_cNewOsCcyAmount:=cNewOsCcyAmount, _
                                                                   r_cNewOsBaseAmount:=cNewOsBaseAmount, _
                                                                   r_cLossGainAmount:=cLossGainAmount, _
                                                                   r_iFullyMatched:=iFullyMatched)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If

                If m_lAllocationTransType = gACTLibrary.ACTPrimaryForAllocation Then
                    iIsPrimary = PMEReturnCode.PMTrue
                Else
                    iIsPrimary = PMEReturnCode.PMFalse
                End If

                ' Create the allocation detail that relates to this transaction
                vDocumentTypeID = m_oDocument.Details.Item(1).DocumenttypeID
                vDocumentRef = m_oDocument.Details.Item(1).DocumentRef
                vOriginalDate = m_oDocument.Details.Item(1).DocumentDate
                vOrigBaseAmount = .Amount
                vOrigBaseAmountUnrounded = .BaseAmountUnrounded ' RAW 12/03/2003 : ISS2893 : added
                vOrigCcyAmount = .CurrencyAmount
                vOrigCcyAmountUnrounded = .CurrencyAmountUnrounded ' RAW 12/03/2003 : ISS2893 : added
                'EK 100100
                'EK Fix 010200
                '        vOrigXRate = .OrigXRate
                vOrigXRate = .CurrencyBaseXrate
                vOSBaseAmount = .OSBaseAmount
                vOSCcyAmount = .OSCurrencyAmount
                crAllocAccountAmount = cAllocBaseAmount / .AccountBaseXrate
                crAllocSystemAmount = cAllocBaseAmount / .SystemBaseXrate

                ' RAW 12/03/2003 : ISS2893 : added unrounded arguments
                m_lReturn = m_oAllocationDetail.DirectAdd(vAllocationId:=v_lAllocationID, _
                                                          vAllocationDetailID:=lAllocationDetailId, _
                                                          vCashListItemId:=v_vCashListItemId, _
                                                          vOriginalCurrency:=iCurrencyId, _
                                                          vTransDetailID:=v_lTransDetailId, _
                                                          vDocumentTypeID:=vDocumentTypeID, _
                                                          vAccountingDate:=dtAccountingDate, _
                                                          vDocumentRef:=vDocumentRef, _
                                                          vOriginalDate:=vOriginalDate, _
                                                          vAllocateToBase:=PMEReturnCode.PMFalse, _
                                                          vOrigBaseAmount:=vOrigBaseAmount, _
                                                          vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, _
                                                          vOrigCcyAmount:=vOrigCcyAmount, _
                                                          vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, _
                                                          vOrigXRate:=vOrigXRate, _
                                                          vEffectiveXRate:=vdCurrencyBaseXRate, _
                                                          vOSBaseAmount:=vOSBaseAmount, _
                                                          vOSCcyAmount:=vOSCcyAmount, _
                                                          vAllocBaseAmount:=cAllocBaseAmount, _
                                                          vAllocCCyAmount:=cAllocCcyAmount, _
                                                          vFullyMatched:=iFullyMatched, _
                                                          vWriteOffAmount:=0, _
                                                          vNewOsCcyAmount:=cNewOsCcyAmount, _
                                                          vNewOsBaseAmount:=cNewOsBaseAmount, _
                                                          vLossGainAmount:=cLossGainAmount, _
                                                          vIsPrimary:=iIsPrimary, _
                                                          r_crAllocAccountAmount:=crAllocAccountAmount, _
                                                          r_crAllocSystemAmount:=crAllocSystemAmount)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFail
                End If
                'eck110102 Return allocationdetail id to passed parameter
                If Not Information.IsNothing(v_vAllocationDetailId) Then
                    v_vAllocationDetailId = lAllocationDetailId
                End If
            End With

            Return nResult
        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CreateAllocationDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAllocationDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function
    ' Write a new allocation Header & Detail for the Specified CashListItem
    Public Function CreateAllocationForCashlist(ByVal v_vCashListID As Integer, ByRef r_lAllocationId As Integer, Optional ByRef v_vCashListItemId() As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lAllocationId, lAllocationDetailId As Integer
        Dim iCompanyId, iUserId As Integer
        Dim lAccountId As Integer
        Dim iCurrencyId As Integer
        Dim dtAccountingDate As Date
        Dim vdCurrencyBaseXRate As Object
        'EK 1001001

        '	Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign

        Dim cAllocBaseAmount, cAllocCcyAmount, cNewOsCcyAmount, cNewOsBaseAmount, cLossGainAmount As Decimal
        Dim iFullyMatched As Integer


        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            If Information.IsNothing(v_vCashListItemId) Then
                ReDim v_vCashListItemId(0)
            End If

            ' Load up the collections with the cashlists
            m_lReturn = CType(GetCashListDetails(v_vCashListID, v_vCashListItemId), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Get the Transaction that this item was posted to
            m_lReturn = CType(GetTransDetail(v_lTransDetailId:=CInt(m_vTransDetailIdArray(0))), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Setup defaults & values from related records

            iCompanyId = m_oCashList.Details.Item(1).CompanyID
            iUserId = m_iUserID

            lAccountId = m_oCashListItem.Details.Item(1).AccountId

            dtAccountingDate = m_oCashList.Details.Item(1).ListDate

            iCurrencyId = m_oCashList.Details.Item(1).CurrencyID

            ' Create a new allocation header (DirectAdd to get the Id for the detail)

            If m_lAllocationID = 0 Then


                m_lReturn = m_oAllocation.DirectAdd(vAllocationId:=lAllocationId, vCompanyID:=iCompanyId, vAccountID:=lAccountId, vUserID:=iUserId, vAllocationDate:=dtAccountingDate, vAllocationstatusId:=gACTLibrary.ACTAllocationStatusUnallocated)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                m_lAllocationID = lAllocationId

            End If

            ' Read back the details for later


            m_lReturn = m_oAllocation.GetDetails(vAllocationId:=m_lAllocationID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFail
            End If

            ' Create the allocation detail that relates to this Cash Item

            For iLoop1 As Integer = m_vTransDetailIdArray.GetLowerBound(0) To m_vTransDetailIdArray.GetUpperBound(0)

                m_lReturn = CType(GetTransDetail(v_lTransDetailId:=CInt(m_vTransDetailIdArray(iLoop1))), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If



                m_lReturn = CType(GetXRateForCurrency(v_iCurrencyID:=m_oTransDetail.Details.Item(1).CurrencyID, v_iCompanyID:=iCompanyId, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                'eck110102 And the document that owns the transaction

                m_lReturn = CType(GetDocument(v_lDocumentId:=m_oTransDetail.Details.Item(1).DocumentId), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                ' RAW 12/03/2003 : ISS2893 : reactivated and moved


                m_lReturn = m_oAllocationCalculate.CalculateValues(v_iOriginalCurrency:=iCurrencyId, v_lCompanyID:=iCompanyId, v_iAllocateToBase:=gPMConstants.PMEReturnCode.PMFalse, v_vdOrigXrate:=m_oTransDetail.Details.Item(1).CurrencyBaseXrate, v_vdEffectiveXrate:=vdCurrencyBaseXRate, v_cOsBaseAmount:=m_oTransDetail.Details.Item(1).OSBaseAmount, v_cOsCcyAmount:=m_oTransDetail.Details.Item(1).OSCurrencyAmount, r_cAllocBaseAmount:=cAllocBaseAmount, r_cAllocCcyAmount:=cAllocCcyAmount, r_cNewOsCcyAmount:=cNewOsCcyAmount, r_cNewOsBaseAmount:=cNewOsBaseAmount, r_cLossGainAmount:=cLossGainAmount, r_iFullyMatched:=iFullyMatched)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If


                m_lReturn = m_oAllocationDetail.DirectAdd(vAllocationId:=m_lAllocationID, vAllocationDetailID:=lAllocationDetailId, vCashListItemId:=m_oCashListItem.Details.Item(1).CashListItemID, vOriginalCurrency:=m_oTransDetail.Details.Item(1).CurrencyID, vTransDetailID:=m_vTransDetailIdArray(iLoop1), vDocumentTypeID:=m_oDocument.Details.Item(1).DocumenttypeID, vAccountingDate:=dtAccountingDate, vDocumentRef:=m_oDocument.Details.Item(1).DocumentRef, vAllocateToBase:=gPMConstants.PMEReturnCode.PMFalse, vOriginalDate:=m_oDocument.Details.Item(1).DocumentDate, vOrigBaseAmount:=m_oTransDetail.Details.Item(1).Amount, vOrigBaseAmountUnrounded:=m_oTransDetail.Details.Item(1).BaseAmountUnrounded, vOrigCcyAmount:=m_oTransDetail.Details.Item(1).CurrencyAmount, vOrigCcyAmountUnrounded:=m_oTransDetail.Details.Item(1).CurrencyAmountUnrounded, vOrigXRate:=m_oTransDetail.Details.Item(1).CurrencyBaseXrate, vEffectiveXRate:=vdCurrencyBaseXRate, vEuroCurrencyId:=m_oTransDetail.Details.Item(1).EuroCurrencyId, vEuroAmount:=m_oTransDetail.Details.Item(1).EuroAmount, vEuroBaseXrate:=m_oTransDetail.Details.Item(1).EuroBaseXrate, vEuroCCyXrate:=m_oTransDetail.Details.Item(1).EuroCCyXrate, vOSBaseAmount:=m_oTransDetail.Details.Item(1).OSBaseAmount, vOSCcyAmount:=m_oTransDetail.Details.Item(1).OSCurrencyAmount, vAllocBaseAmount:=cAllocBaseAmount, vAllocCCyAmount:=cAllocCcyAmount, vFullyMatched:=iFullyMatched, vWriteOffAmount:=0, vNewOsCcyAmount:=cNewOsCcyAmount, vNewOsBaseAmount:=cNewOsBaseAmount, vLossGainAmount:=cLossGainAmount, vIsPrimary:=gPMConstants.PMEReturnCode.PMTrue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

            Next iLoop1

            r_lAllocationId = m_lAllocationID

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            '    Resume Next

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAllocationForCashlist Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAllocationForCashlist", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' Get the details of a particular document into the class
    Private Function GetDocument(ByVal v_lDocumentId As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oDocument.GetDetails(vDocumentID:=v_lDocumentId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function
    ' Get the details of a particular TransDetail into the class
    'eck110102 Make Public
    Public Function GetTransDetail(ByVal v_lTransDetailId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oTransDetail.GetDetails(vTransDetailID:=v_lTransDetailId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTransDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Private Function GetXRateForCurrency(ByVal v_iCurrencyID As Integer, ByVal v_iCompanyID As Integer, ByRef r_vdCurrencyBaseXRate As Object, ByVal v_dtAccountingDate As Date) As Integer

        Dim result As Integer = 0
        Dim cBaseAmount, cCurrencyAmount As Decimal



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Convert �1 to the other currency and return the rate
        ' unless it's the base and then return 1

        cBaseAmount = 1
        r_vdCurrencyBaseXRate = 0 'Blank rate so that it gets populated.


        m_lReturn = m_oCurrencyConvert.ConvertBaseToCurrency(lCurrencyID:=v_iCurrencyID, lCompanyID:=v_iCompanyID, cBaseAmount:=cBaseAmount, cCurrencyAmount:=cCurrencyAmount, vConversionDate:=v_dtAccountingDate, vConversionRate:=r_vdCurrencyBaseXRate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFail
        End If

        Return result

    End Function
    ' Get the details of a particular allocation & it's details into the class
    'eck110102 Made Public
    Public Function GetAllocation(ByVal v_lAllocationID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oAllocation.GetDetails(vAllocationId:=v_lAllocationID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            m_lReturn = m_oAllocationDetail.GetDetails(vAllocationId:=v_lAllocationID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Load the details into the CashList & CashListItem Collections
    Private Function GetCashListDetails(ByVal v_lCashListId As Integer, Optional ByRef v_vCashListItemId() As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oCashList.GetDetails(vCashListID:=v_lCashListId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' Now get the details


        m_lReturn = m_oCashListItem.GetDetails(vCashListItemId:=v_vCashListItemId, vCashListID:=v_lCashListId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result

    End Function

    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    ''' <summary>
    ''' CreateAllocationBatch
    ''' </summary>
    ''' <param name="v_nReversedAllocationBatchID"></param>
    ''' <param name="r_nAllocationBatchID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function CreateAllocationBatch(ByVal v_nReversedAllocationBatchID As Integer, _
            ByRef r_nAllocationBatchID As Integer) As Integer


        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        nResult = m_oDatabase.Parameters.Add("nAllocation_batch_id", CStr(r_nAllocationBatchID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return nResult
        End If

        If ToSafeLong(v_nReversedAllocationBatchID) > 0 Then
            m_oDatabase.Parameters.Add("nReversed_allocation_batch_id", CStr(v_nReversedAllocationBatchID), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)
        End If
        nResult = m_oDatabase.SQLAction(sSQL:=kAddAllocationBatchSQL, sSQLName:=kAddAllocationBatchName, bStoredProcedure:=kAddAllocationBatchStored)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return nResult
        Else
            r_nAllocationBatchID = ToSafeInteger(m_oDatabase.Parameters.Item("nAllocation_batch_id").Value)
        End If

        Return nResult

    End Function


End Class
