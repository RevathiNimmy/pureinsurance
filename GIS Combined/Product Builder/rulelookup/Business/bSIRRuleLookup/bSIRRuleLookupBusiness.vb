Option Strict Off
Option Explicit On
'Developer Guide No 129
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 20/07/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRProduct.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/09/2003
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

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Calling Application Name

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private lPMAuthorityLevel As Integer


    ' Primary Keys to work with
    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)

            ' Set the calling application name.
            m_sCallingAppName = Value

        End Set
    End Property

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now
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
            If disposing Then
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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetDataModelCode
    '
    ' Description:
    '
    ' History: 10/05/2001 AJM - Created.
    '
    ' ***************************************************************** '
    Public Function GetDataModelCode(ByVal v_lDataModelId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(v_lDataModelId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDataModelCodeSQL, sSQLName:=ACGetDataModelCodeName, bStoredProcedure:=ACGetDataModelCodeStored, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDataModelCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDataModelCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetNextLineKey
    '
    ' Desc : get next available line key
    '
    ' ***************************************************************** '
    Public Function GetNextLineKey(ByVal v_lLookupKey As Integer, ByRef r_lLineKey As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(v_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.SQLSelect(sSQL:=ACGetNextLineKeySQL, sSQLName:=ACGetNextLineKeyName, bStoredProcedure:=ACGetNextLineKeyStored, vResultArray:=vResultArray, bKeepNulls:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then

                r_lLineKey = CInt(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNextLineKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNextLineKey", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllLookupData
    '
    ' Desc : get all lookup header
    '
    ' ***************************************************************** '
    Public Function GetAllLookupData(ByVal v_lLookupKey As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(v_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACGetAllLookupDataSQL, sSQLName:=ACGetAllLookupDataName, bStoredProcedure:=ACGetAllLookupDataStored, lNumberRecords:=-1, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllLookupData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllLookupData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetAllLookupHeader
    '
    ' Desc : get all lookup header
    '
    ' ***************************************************************** '
    Public Function GetAllLookupHeader(ByVal v_lDataModelId As Object, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'AJM 09/05/01 - select by data model
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(v_lDataModelId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            Return m_oDatabase.SQLSelect(sSQL:=ACGetAllLookupHeaderSQL, sSQLName:=ACGetAllLookupHeaderName, bStoredProcedure:=ACGetAllLookupHeaderStored, lNumberRecords:=-1, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllLookupHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllLookupHeader", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateLookupData
    '
    ' Description: update details from lookup_data and lookup_header
    '
    ' ***************************************************************** '
    Public Function UpdateLookupData(ByVal v_iTask As Integer, ByVal v_lLookupKey As Integer, ByVal v_lLineKey As Integer, ByVal v_vKeyLevel As Object, ByVal v_vValue As Object, ByVal v_vType As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(v_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="line_key", vValue:=CStr(v_lLineKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="key_level", vValue:=CStr(v_vKeyLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="value", vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="type", vValue:=CStr(v_vType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddLookupDataSQL, sSQLName:=ACAddLookupDataName, bStoredProcedure:=ACAddLookupDataStored)

            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdLookupDataSQL, sSQLName:=ACUpdLookupDataName, bStoredProcedure:=ACUpdLookupDataStored)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLookupData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLookupData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateLookupHeader
    '
    ' Description: update lookup_header table
    '
    ' ***************************************************************** '
    Public Function UpdateLookupHeader(ByVal v_iTask As Integer, ByVal v_lInsurerPanelMemberKey As Integer, ByVal v_lSchemeNumber As Integer, ByRef r_lLookupKey As Integer, ByVal v_vLookupName As Object, ByVal v_vEffectiveDate As Object, ByVal v_dtModifyDate As Date, ByVal v_vStatus As Object, ByVal v_vDefinition As Object, ByVal v_vConstant As Object, ByVal v_vDefault As Object, ByVal v_lDataModelId As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_panel_member_key", vValue:=CStr(v_lInsurerPanelMemberKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="scheme_number", vValue:=CStr(v_lSchemeNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(r_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(r_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_name", vValue:=CStr(v_vLookupName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_vEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'Developer Guide no. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_date", vValue:=v_dtModifyDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(v_vStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="definition", vValue:=CStr(v_vDefinition), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="valid_constants", vValue:=CStr(v_vConstant), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="default_value", vValue:=CStr(v_vDefault), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AJM 09/05/01 - add data model id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(v_lDataModelId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'are we adding or updating
            If v_iTask = gPMConstants.PMEComponentAction.PMAdd Then



                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddLookupHeaderSQL, sSQLName:=ACAddLookupHeaderName, bStoredProcedure:=ACAddLookupHeaderStored)

                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    r_lLookupKey = m_oDatabase.Parameters.Item("lookup_key").Value
                End If

            Else
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdLookupHeaderSQL, sSQLName:=ACUpdLookupHeaderName, bStoredProcedure:=ACUpdLookupHeaderStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLookupData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLookupData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**********************************************************************************
    ' Name : DeleteLookupData
    '
    ' Desc :  delete lookup header and all associate lookup data
    '
    '***********************************************************************************
    Public Function DeleteLookupData(ByVal v_lLookupKey As Integer, ByVal v_lLineKey As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(v_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="line_key", vValue:=CStr(v_lLineKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.SQLAction(sSQL:=ACDelLookupDataSQL, sSQLName:=ACDelLookupDataName, bStoredProcedure:=ACDelLookupDataStored) <> gPMConstants.PMEReturnCode.PMTrue Then


                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteLookupData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLookupData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**********************************************************************************
    ' Name : DeleteLookupHeader
    '
    ' Desc :  delete lookup header and all associate lookup data
    '
    '***********************************************************************************
    Public Function DeleteLookupHeader(ByVal v_lDataModelId As Object, ByVal v_lLookupKey As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If BeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(v_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AJM 09/05/01

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(v_lDataModelId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.SQLAction(sSQL:=ACDelLookupHeaderSQL, sSQLName:=ACDelLookupHeaderName, bStoredProcedure:=ACDelLookupHeaderStored) <> gPMConstants.PMEReturnCode.PMTrue Then


                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            Return CommitTrans()

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteLookupHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLookupHeader", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '**********************************************************************************
    ' Name : CreateLookupFile
    '
    ' Desc :  create flat file of the lookup data (path is defined in the registry)
    '
    '***********************************************************************************
    Public Function CreateLookupFile(ByVal v_vDataModelCode As String) As Integer

        Dim result As Integer = 0
        Dim oMaint As bGISLookupManager.Maintain
        Dim m_lReturn As gPMConstants.PMEReturnCode
        Dim sDataModelCode As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            oMaint = New bGISLookupManager.Maintain()

            If oMaint Is Nothing Then

                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to start LookupManager object", vApp:="TestData", vClass:="Interface", vMethod:="CreateLookupFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            '    Set oDatabase = CreateObject("dPMDAO.Database")
            '
            '    If oDatabase Is Nothing Then
            '
            '        ' Log Error.
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogOnError, _
            ''            sMsg:="Failed to start Database object", _
            ''            vApp:="TestData", _
            ''            vClass:="Interface", _
            ''            vMethod:="CreateLookupFile", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '
            '         CreateLookupFile = PMFalse
            '        Exit Function
            '
            '    End If
            '
            '    m_lReturn = oDatabase.OpenDatabase(vUsername:="SIRIUS", _
            ''                                             vPassword:="$1r1u5", _
            ''                                             vdsn:="GIS")
            '
            '    If m_lReturn <> PMTrue Then
            '        CreateLookupFile = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = CType(oMaint.initialise(m_oDatabase), gPMConstants.PMEReturnCode)

            'NO NEED TO CHECK FOR FAILURE SINCE oMaint.Initialise(oDatabase) DOES NOT SET ITS RETURNING VALUE
            '    If m_lReturn <> PMTrue Then
            '        CreateLookupFile = PMFalse
            '        Exit Function
            '    End If

            'Default
            sDataModelCode = v_vDataModelCode.TrimEnd()

            m_lReturn = oMaint.BuildLookupCacheFiles(sDataModelCode, "1", CType(True, gPMConstants.PMEReturnCode))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oMaint = Nothing

            '    m_lReturn = oDatabase.CloseDatabase
            '    Set oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create Lookup flat file", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateLookupFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Public Methods (End)

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

    ' private Methods (End)


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






    ' ***************************************************************** '
    '
    ' Name: IsUniqueLookupName
    '
    ' Description: bResult = false if record already exists.
    '
    ' History: 19/11/2002 CJR - Created.
    '
    ' ***************************************************************** '
    Public Function IsUniqueLookupName(ByRef bResult As Boolean, ByRef sLookupName As String, Optional ByRef lLookupKey As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try
            bResult = False

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_name", vValue:=sLookupName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            If lLookupKey > 0 Then 'implies edit mode


                m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACLookupNameUniqueEditSQL, sSQLName:=ACLookupNameUniqueEditName, bStoredProcedure:=ACLookupNameUniqueEditStored, lNumberRecords:=1, vResultArray:=vResultArray)
            Else
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACLookupNameUniqueAddSQL, sSQLName:=ACLookupNameUniqueAddName, bStoredProcedure:=ACLookupNameUniqueAddStored, lNumberRecords:=1, vResultArray:=vResultArray)
            End If

            'Check if the reference exists
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            bResult = Not Informations.IsArray(vResultArray)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsUniqueLookupName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsUniqueLookupName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function
    Public Function GetLookupHeader(ByVal v_lDataModelCode As Object, ByRef r_vResultArray(,) As Object, ByVal v_sLookupName As String, ByVal v_lInsurerPanelMemberKey As Integer, ByVal v_lSchemeNumber As Integer, ByVal v_dEffectiveDate As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'AJM 09/05/01 - select by data model
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_panel_member_key", vValue:=CStr(v_lInsurerPanelMemberKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="scheme_number", vValue:=CStr(v_lSchemeNumber), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=CStr(v_lDataModelCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_name", vValue:=CStr(v_sLookupName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If v_dEffectiveDate <> "" Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACGetSingleLookupHeaderSQL, sSQLName:=ACGetSingleLookupHeaderName, bStoredProcedure:=ACGetSingleLookupHeaderStored, lNumberRecords:=-1, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleLookupHeader Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllLookupHeader", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    Public Function GetLookupData(ByVal v_lLookupKey As Integer, Optional ByVal v_vKeyLevel As Object = Nothing, Optional ByRef r_vResultArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lookup_key", vValue:=CStr(v_lLookupKey), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If v_vKeyLevel IsNot Nothing Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="key_level", vValue:=CStr(v_vKeyLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACSingleAllLookupDataSQL, sSQLName:=ACSingleAllLookupDataName, bStoredProcedure:=ACSingleAllLookupDataStored, lNumberRecords:=-1, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSingleLookupData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllLookupData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

