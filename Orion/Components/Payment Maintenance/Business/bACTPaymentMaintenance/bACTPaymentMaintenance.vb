Option Strict Off
Option Explicit On
Imports SSP.Shared
'developer guide no. 129
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable


    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    Private Const ACClass As String = "Business"

    Private m_oDatabase As dPMDAO.Database

    Private m_bCloseDatabase As Boolean

    Private m_lError As gPMConstants.PMEReturnCode
    ' Task
    Private m_iTask As Integer

    ' Navigate
    Private m_lNavigate As Integer

    ' Process Mode
    Private m_lProcessMode As Integer

    ' Type of Business
    Private m_sTypeOfBusiness As New StringsHelper.FixedLengthString(10)

    ' Effective
    Private m_dtEffectiveDate As Date

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Component Sub Type
    Private m_sSubType As New StringsHelper.FixedLengthString(20)
    ' Variable Data Business Component (Private)

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business
    ' PRIVATE Data Members (End)

    'Currency Convertor Object
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

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

    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTypeOfBusiness As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

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


            If Not Informations.IsNothing(vTypeOfBusiness) Then

                m_sTypeOfBusiness.Value = CStr(vTypeOfBusiness)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetProcessModes", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetLookupValues", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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

            ' Set User ID
            m_iUserID = iUserID


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business Object passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion

            ' Currency Convert - added for front office receipting.

            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="Initialise", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
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
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                m_oLookup = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


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
        If Not Informations.IsArray(vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If

        Return result
    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        Try

            ' Class Initialise


        Catch ex As Exception

            ' Error Section.

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally



        End Try
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '****************************************************************** '
    ' Name: GetUserReverseAllocation (Public)
    '
    ' Description: Gets Payment Status
    '
    '****************************************************************** '
    Public Function GetUserReverseAllocation(ByVal v_UserID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserReverseAllocation"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_UserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter user_id")
                Return result
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACPaymentMaintenanceGetUserReverseAllocationQuerySQL, sSQLName:=ACPaymentMaintenanceGetUserReverseAllocationQueryName, bStoredProcedure:=ACPaymentMaintenanceGetUserReverseAllocationQueryStored, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Return result
            End If

            result = CheckResults(vResultArray)


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="GetUserReverseAllocation", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' Gets Payment Status
    ''' </summary>
    ''' <param name="v_lCashlistitem_id"></param>
    ''' <param name="v_dtReversed_date"></param>
    ''' <param name="v_iCashlistitem_reverse_pmuser_id"></param>
    ''' <param name="v_lCashlistitem_reverse_reason_id"></param>
    ''' <param name="v_lcashlistitem_reversal_transdetail_id"></param>
    ''' <param name="nIsReceiptReversal"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetCashListItemFlags(ByVal v_lCashlistitem_id As Integer, ByVal v_dtReversed_date As Date,
                                         ByVal v_iCashlistitem_reverse_pmuser_id As Integer,
                                         ByVal v_lCashlistitem_reverse_reason_id As Integer,
                                         ByVal v_lcashlistitem_reversal_transdetail_id As Long,
                                         Optional ByVal nIsReceiptReversal As Integer = 0) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "SetCashListItemFlags"


        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(v_lCashlistitem_id),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter cashlistitem_id")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="reversed_date", vValue:=v_dtReversed_date,
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter reversed_date")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_reverse_pmuser_id",
                                                   vValue:=CStr(v_iCashlistitem_reverse_pmuser_id),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter cashlistitem_reverse_pmuser_id")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_reverse_reason_id",
                                                   vValue:=CStr(v_lCashlistitem_reverse_reason_id),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter cashlistitem_reverse_reason_id")
                Return nResult
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cashlistitem_reversal_transdetail_id",
                                                   vValue:=CStr(v_lcashlistitem_reversal_transdetail_id),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter cashlistitem_reversal_transdetail_id")
                Return nResult
            End If
            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nIsReceiptReversal",
                vValue:=nIsReceiptReversal,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, "Failed to add parameter cashlistitem_reversal_transdetail_id")
                Return nResult
            End If
            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACPaymentMaintenanceUpdateCashListItemQuerySQL,
                                             sSQLName:=ACPaymentMaintenanceUpdateCashListItemQueryName,
                                             bStoredProcedure:=ACPaymentMaintenanceUpdateCashListItemQueryStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Raise Error
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Return nResult
            End If



        Catch ex As Exception
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="SetCashListItemFlags", r_lFunctionReturn:=nResult, v_sUsername:=m_sUsername, excep:=ex)

        Finally

        End Try
        Return nResult
    End Function


    '****************************************************************** '
    ' Name: FillCancelPaymentGrid (Public)
    '
    ' Description: Gets data for Cancel Payment List
    '
    '****************************************************************** '
    Public Function FillCancelPaymentGrid(ByVal v_lTransDetailID As Long, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "FillCancelPaymentGrid"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lTransDetailId",
                                                   vValue:=CStr(v_lTransDetailID),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Raise Error.
                gPMFunctions.RaiseError(kMethodName, "Failed to add parameter lTransDetailId")
                Return result
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(
                sSQL:=ACPaymentMaintenanceGetCancelPaymentListDataQuerySQL,
                sSQLName:=ACPaymentMaintenanceGetCancelPaymentListDataQueryName,
                bStoredProcedure:=ACPaymentMaintenanceGetCancelPaymentListDataQueryStored,
                vResultArray:=vResultArray)


            If (m_lError <> gPMConstants.PMEReturnCode.PMTrue) Then
                ' Raise Error.
                RaiseError(kMethodName, "m_oDatabase.SQLSelect Failed")
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="lTransDetailId",
                    vValue:=v_lTransDetailID,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Raise Error.
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter lTransDetailId")
                    Return result
                End If


                m_lReturn = m_oDatabase.Parameters.Add(sName:="iCancelPayment",
                                                       vValue:=CStr(1),
                                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                       iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                    ' Raise Error.
                    gPMFunctions.RaiseError(kMethodName, "Failed to add parameter iCancelPayment")
                    Return result
                End If


                'Execute SQL Statement
                m_lError = m_oDatabase.SQLSelect(sSQL:=ACPaymentMaintenanceGetCancelPaymentListDataQuerySQL,
                                                 sSQLName:=ACPaymentMaintenanceGetCancelPaymentListDataQueryName,
                                                 bStoredProcedure:=ACPaymentMaintenanceGetCancelPaymentListDataQueryStored,
                                                 vResultArray:=vResultArray)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Raise Error.
                    gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect Failed")
                    Return result
                End If
            End If
            result = CheckResults(vResultArray)


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="FillCancelPaymentGrid", r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    '****************************************************************** '
    ' Name: GetEventTypeId (Public)
    '
    ' Description: Gets Payment Status
    '
    '****************************************************************** '
    Public Function GetEventTypeId(ByVal v_sEventCode As String, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetEventTypeId"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="eventcode", vValue:=v_sEventCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter eventcode")
                Return result
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACPaymentMaintenanceGetEventTypeIdQuerySQL, sSQLName:=ACPaymentMaintenanceGetEventTypeIdQueryName, bStoredProcedure:=ACPaymentMaintenanceGetEventTypeIdQueryStored, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Return result
            End If


            result = CheckResults(vResultArray)


        Catch ex As Exception

            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
    '****************************************************************** '
    ' Name: GetArrayForMediaTypeValidation (Public)
    '
    ' Description: Gets Array for specified media type validation
    '
    '****************************************************************** '
    Public Function GetArrayForMediaTypeValidationId(ByVal v_iMediaTypeValidationID As Integer, ByRef v_vMediaTypeArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetArrayForMediaTypeValidationId"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="mediatype_validation_id", vValue:=CStr(v_iMediaTypeValidationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Add parameter mediatype_validation_id")
                Return result
            End If

            'Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACMediTypeForValidationIDQuerySQL, sSQLName:=ACMediTypeForValidationIDQueryName, bStoredProcedure:=ACMediTypeForValidationIDQueryStored, vResultArray:=v_vMediaTypeArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                'Raise Error
                gPMFunctions.RaiseError(kMethodName, "m_oDatabase.SQLSelect failed")
                Return result
            End If



        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Error Section.
            bPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, v_sUsername:=m_sUsername, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
