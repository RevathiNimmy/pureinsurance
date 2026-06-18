Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Diagnostics
Imports System.Globalization
Imports System.IO
'developer guide no. 129
Imports SharedFiles
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              User Defined Data Maintenance.
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
    Public Shared iCache As ICacheManager
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)

    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Database Class (Private)
    Private m_oArcDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseArcDatabase As Boolean

    'developer guide no. 33
    Private m_vDataDictionary As Object

    Private m_vScreenHeader As Object

    Private m_vScreenDetails As Object

    'developer guide no. 33
    Private m_vChildScreenDetails As Object

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lScreenId As Integer
    Private m_sScreenCode As String = ""
    Private m_sNewScreenCode As String = ""
    Private m_sNewScreenDescription As String = ""

    Private m_lGISDataModelId As Integer
    Private m_sGISDataModelCode As String = ""
    Private m_lGISObjectId As Integer

    Private m_sRulePath As String = ""

    Private m_lScreenType As Integer

    Private _CompiledRules As Boolean

    Public Property ScreenId() As Integer
        Get
            Return m_lScreenId
        End Get
        Set(ByVal Value As Integer)
            m_lScreenId = Value
        End Set
    End Property

    Public Property ScreenCode() As String
        Get
            Return m_sScreenCode
        End Get
        Set(ByVal Value As String)
            m_sScreenCode = Value
        End Set
    End Property

    ' RAW 08/07/2003 : CQ1596 : added
    Public Property ScreenType() As Integer
        Get
            Return m_lScreenType
        End Get
        Set(ByVal Value As Integer)
            m_lScreenType = Value
        End Set
    End Property
    ' RAW 08/07/2003 : CQ1596 : end

    Public Property SourceId() As Integer
        Get
            Return m_iSourceID
        End Get
        Set(ByVal Value As Integer)
            m_iSourceID = Value
        End Set
    End Property

    Public Property GISObjectId() As Integer
        Get
            Return m_lGISObjectId
        End Get
        Set(ByVal Value As Integer)
            m_lGISObjectId = Value
        End Set
    End Property

    Public Property GISDataModelId() As Integer
        Get
            Return m_lGISDataModelId
        End Get
        Set(ByVal Value As Integer)
            m_lGISDataModelId = Value
        End Set
    End Property

    Public Property GISDataModelCode() As String
        Get
            Return m_sGISDataModelCode
        End Get
        Set(ByVal Value As String)
            m_sGISDataModelCode = Value
        End Set
    End Property

    Public Property NewScreenCode() As String
        Get
            Return m_sNewScreenCode
        End Get
        Set(ByVal Value As String)
            m_sNewScreenCode = Value
        End Set
    End Property

    Public Property NewScreenDescription() As String
        Get
            Return m_sNewScreenDescription
        End Get
        Set(ByVal Value As String)
            m_sNewScreenDescription = Value
        End Set
    End Property

    Public Property RulePath() As String
        Get
            If m_sRulePath = "" Then
                m_lReturn = CType(GetRulePath(r_sRulePath:=m_sRulePath), gPMConstants.PMEReturnCode)
            End If
            Return m_sRulePath
        End Get
        Set(ByVal Value As String)
            m_sRulePath = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public ReadOnly Property CompiledRules As Boolean
        Get
            Return _CompiledRules
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get an instance to the architecture database
            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oArcDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bCloseArcDatabase = True


            ' Set Username and Password

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
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
                    m_oDatabase = Nothing
                End If
                m_vDataDictionary = Nothing
                m_vScreenHeader = Nothing
                m_vScreenDetails = Nothing

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




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the data dictionary and screen layout
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetDetails(ByRef r_vDataDictionary As Object, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object, ByRef r_vChildScreenDetails As Object) As Integer

        Dim result As Integer = 0
        Dim lDataModelId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no. 12
            r_vDataDictionary = Nothing
            r_vScreenHeader = Nothing
            r_vScreenDetails = Nothing
            r_vChildScreenDetails = Nothing


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            If m_lGISObjectId = 0 Then
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                lDataModelId = m_lGISDataModelId

                If lDataModelId = 0 Then
                    'Default it...
                    lDataModelId = 1
                End If

                'Add the screen
                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_data_model_id", vValue:=CStr(lDataModelId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDataDictionarySQL, sSQLName:=ACGetDataDictionaryName, bStoredProcedure:=ACGetDataDictionaryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vDataDictionary)
            Else
                ' Clear the Database Parameters Collection
                m_oDatabase.Parameters.Clear()

                'Add the screen
                m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_object_id", vValue:=CStr(m_lGISObjectId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSpecificDataDictionarySQL, sSQLName:=ACGetSpecificDataDictionaryName, bStoredProcedure:=ACGetSpecificDataDictionaryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vDataDictionary)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(m_vDataDictionary) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_vDataDictionary = m_vDataDictionary

            'Now get the screen header

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the screen
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=PBDatabaseConsts.ACGetAllScreenHeaderSQL, sSQLName:=PBDatabaseConsts.ACGetAllScreenHeaderName, bStoredProcedure:=PBDatabaseConsts.ACGetAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vScreenHeader, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'read in the script columns
            If Information.IsArray(m_vScreenHeader) Then
                ReadScriptColumn(m_oDatabase, m_lScreenId, m_vScreenHeader, PBDatabaseConsts.ACHScriptDefaults, ACNScriptDefaults)
                ReadScriptColumn(m_oDatabase, m_lScreenId, m_vScreenHeader, PBDatabaseConsts.ACHScriptDynamicLogic, ACNScriptDynamicLogic)
            End If

            r_vScreenHeader = m_vScreenHeader

            'Now get the screen details

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the screen
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllScreenDetailsSQL, sSQLName:=ACGetAllScreenDetailsName, bStoredProcedure:=ACGetAllScreenDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vScreenDetails, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            r_vScreenDetails = m_vScreenDetails

            'Now get the child screen details

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the screen
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllChildScreenDetailsSQL, sSQLName:=ACGetAllChildScreenDetailsName, bStoredProcedure:=ACGetAllChildScreenDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=m_vChildScreenDetails, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_vChildScreenDetails = m_vChildScreenDetails

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the data dictionary and screen values
    '
    ' ***************************************************************** '
    Public Function GetNext(ByRef r_vDataDictionary As String, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object, ByRef r_vChildScreenDetails As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vDataDictionary = m_vDataDictionary


            r_vScreenHeader = m_vScreenHeader


            r_vScreenDetails = m_vScreenDetails
            r_vChildScreenDetails = m_vChildScreenDetails

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Sets the arrays ready for updating
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef r_vDataDictionary As String, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vDataDictionary = r_vDataDictionary


            m_vScreenHeader = r_vScreenHeader


            m_vScreenDetails = r_vScreenDetails

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Redoes the data
    '
    ' ***************************************************************** '
    'developer guide no. 17
    Public Function Update(ByRef r_vDataDictionary As Object, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vDataDictionary = r_vDataDictionary


            m_vScreenHeader = r_vScreenHeader


            m_vScreenDetails = r_vScreenDetails

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            'First we delete the screen details, foreign keys, don't you know
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(m_lScreenId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteScreenDetailsSQL, sSQLName:=ACDeleteScreenDetailsName, bStoredProcedure:=ACDeleteScreenDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            'Now we add the screen
            m_lReturn = CType(AddScreenHeader(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If
            
            m_lReturn = CType(AddScreenDetails(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckCode (Public)
    '
    ' Description: Redoes the data
    '
    ' ***************************************************************** '
    Public Function CheckCode(ByVal v_lScreenId As Integer, ByVal v_sCode As String, ByRef r_bOK As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(v_lScreenId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=v_sCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckScreenHeaderCodeSQL, sSQLName:=ACCheckScreenHeaderCodeName, bStoredProcedure:=ACCheckScreenHeaderCodeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(vArray) Then
                r_bOK = True
            End If

            vArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyScreen
    '
    ' Description:
    '
    ' History: 14/07/2000 Tomo - Created.
    ' RAW 08/07/2003 : CQ1596 : major revamp : added params v_vOldScreenID, r_vNewScreenID, v_vNewParentScreenID
    '                           calls itself recursively for processing children
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function CopyScreen(Optional ByVal v_vScreenType As Object = Nothing, Optional ByVal v_vOldScreenID As Object = Nothing, Optional ByRef r_vNewScreenID As Object = Nothing, Optional ByVal v_vNewParentScreenID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Static iCallDepth As Integer

        Dim vChildArray, vResultArray(,) As Object
        'Dim lNewScreenId As Long
        'Dim lNewDetailScreenId As Long
        Dim lCaption, lThisScreenType, lThisOldScreenID, lThisNewScreenID, lChildOldScreenID, lChildNewScreenID As Integer
        'developer guide no.101
        Dim vOldScreenCode As Object
        Dim vNewScreenCode As Object
        Dim vNewScreenDesc As Object
        Dim vNewScreenDescCaptionId As Object
        Dim lChildScreenType As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RAW 08/07/2003 : CQ1596 : added

            ' It is important that whenever leaving this function that this count is reduced by 1
            ' NEVER EXIT FUNCTION DIRECTLY - ALWAYS GOTO FINALLY
            iCallDepth += 1


            ' Do we have enough info to work with for the type of screen that we are processing?
            ' =========================================================================

            ' Check Screen Type
            ' use the parameter value if present otherwise use the module variable - but only if this is the first call

            If Information.IsNothing(v_vScreenType) Then
                If iCallDepth = 1 Then
                    lThisScreenType = m_lScreenType
                Else
                    lThisScreenType = 0
                End If
            Else

                lThisScreenType = CInt(v_vScreenType)
            End If


            ' Check Old Screen ID
            ' use the parameter value if present otherwise use the module variable - but only if this is the first call

            If Information.IsNothing(v_vOldScreenID) Then
                If iCallDepth = 1 Then
                    lThisOldScreenID = m_lScreenId
                Else
                    lThisOldScreenID = 0
                End If
            Else

                lThisOldScreenID = CInt(v_vOldScreenID)
            End If

            If lThisOldScreenID > 0 Then
            Else
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot copy screen without a screen to copy from", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen")
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            ' Check new parent screen id

            Select Case lThisScreenType
                Case GISDataModelType.GISOTAssociatedClient, GISDataModelType.GISOTDisclosure, GISDataModelType.GISOTPeril
                    ' these screens do not have a direct single parent but are children nonetheless
                    v_vNewParentScreenID = -1
            End Select


            If Information.IsNothing(v_vNewParentScreenID) Then

                v_vNewParentScreenID = Nothing
            End If


            ' Check New Screen Code
            ' Only use the module variable if this is the first call, otherwise the value will be set by the sp
            If iCallDepth = 1 Then
                If NewScreenCode = "" Then
                    ' we cannot copy this screen without a new code
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot copy screen without a code for the screen to copy to", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                Else
                    vNewScreenCode = NewScreenCode
                End If
            Else

                vNewScreenCode = Nothing
            End If


            ' Check new Screen description
            ' Only use the module variable if this is the first call, otherwise the value will be set by the sp
            If iCallDepth = 1 Then
                If NewScreenDescription = "" Then
                    ' we cannot copy this screen without a new desc
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Cannot copy screen without a description for the screen to copy to", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                Else
                    vNewScreenDesc = NewScreenDescription

                    m_lReturn = CType(GetCaptionID(v_sCaption:=NewScreenDescription, r_lCaptionID:=lCaption), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        Return result
                    End If

                    vNewScreenDescCaptionId = CStr(lCaption)
                End If
            Else

                vNewScreenDesc = Nothing

                vNewScreenDescCaptionId = Nothing
            End If
            ' RAW 08/07/2003 : CQ1596 : end


            ' Get the child screen ids
            ' ==================================================================

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' RAW 08/07/2003 : CQ1596 : pass lThisOldScreenID instead of m_lScreenId
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(lThisOldScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetScreenChildrenSQL, sSQLName:=ACGetScreenChildrenName, bStoredProcedure:=ACGetScreenChildrenStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vChildArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            ' start a database transaction
            ' ===================================================================
            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)


            'Copy this screen
            ' ====================================================================

            m_oDatabase.Parameters.Clear()

            ' this is the screen that we are copying
            ' RAW 08/07/2003 : CQ1596 : rename param, pass lThisOldScreenID instead of m_lScreenId, change from IO to input
            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_GIS_screen_id", vValue:=CStr(lThisOldScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' this is the new parent that this screen is being copied to (may be null)
            ' RAW 08/07/2003 : CQ1596 : rename param, pass v_vNewParentScreenID instead of null
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_parent_id", vValue:=CStr(v_vNewParentScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' this is the new code that will be used for the new screen (may be null in which case it will be set/ returned by sp)
            ' RAW 08/07/2003 : CQ1596 : rename param, pass vNewScreenCode instead of NewScreenCode property
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_code", vValue:=vNewScreenCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' this is the caption for the new description that will be used for the new screen (may be null in which case it will be set by sp)
            ' RAW 08/07/2003 : CQ1596 : rename param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_caption_id", vValue:=vNewScreenDescCaptionId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' this is the new description that will be used for the new screen (may be null in which case it will be set by sp)
            ' RAW 08/07/2003 : CQ1596 : rename param, pass vNewScreenDesc instead of NewScreenDesc property
            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_description", vValue:=vNewScreenDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' Execute SQL Statement
            ' RAW 08/07/2003 : CQ1596 : replace SQLAction with SQLSelect and return a result array containing details of the new screen
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCopyScreenSQL, sSQLName:=ACCopyScreenName, bStoredProcedure:=ACCopyScreenStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' get details from the sp

            lThisNewScreenID = CInt(vResultArray(0, 0))

            vOldScreenCode = CStr(vResultArray(1, 0))

            vNewScreenCode = CStr(vResultArray(2, 0))

            vNewScreenDesc = CStr(vResultArray(3, 0))

            vNewScreenDescCaptionId = CStr(vResultArray(4, 0))


            'Copy the screen contents (controls)
            ' =========================================================================
            m_oDatabase.Parameters.Clear()

            ' this is the screen that we are copying from
            ' RAW 08/07/2003 : CQ1596 : pass lThisOldScreenID instead of m_lScreenId
            m_lReturn = m_oDatabase.Parameters.Add(sName:="OLD_GIS_screen_id", vValue:=CStr(lThisOldScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' this is the screen that we are copying to
            m_lReturn = m_oDatabase.Parameters.Add(sName:="NEW_GIS_screen_id", vValue:=CStr(lThisNewScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyScreenDetailSQL, sSQLName:=ACCopyScreenDetailName, bStoredProcedure:=ACCopyScreenDetailStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return result
            End If


            ' Now copy each child screen
            ' ==================================================================

            If Information.IsArray(vChildArray) Then

                For lTemp As Integer = vChildArray.GetLowerBound(1) To vChildArray.GetUpperBound(1)

                    ' RAW 08/07/2003 : CQ1596 : added recursive call to CopyScreen for each child screen to replace existing call to sp

                    lChildOldScreenID = CInt(vChildArray(0, lTemp))


                    Dim dbNumericTemp As Double
                    lChildScreenType = IIf(Double.TryParse(CStr(vChildArray(1, lTemp)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp), CInt(vChildArray(1, lTemp)), 0)



                    Select Case lChildScreenType
                        Case GISDataModelType.GISOTAssociatedClient, GISDataModelType.GISOTDisclosure, GISDataModelType.GISOTPeril
                            ' do not copy screen

                        Case Else
                            m_lReturn = CType(CopyScreen(v_vScreenType:=lChildScreenType, v_vOldScreenID:=lChildOldScreenID, r_vNewScreenID:=lChildNewScreenID, v_vNewParentScreenID:=lThisNewScreenID), gPMConstants.PMEReturnCode)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If


                            'update the child screen id for the child object in its parent's screen definition
                            ' =================================================================================
                            m_oDatabase.Parameters.Clear()

                            ' this is the parent of the child - ie this screen just added
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(lThisNewScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If

                            ' this is the entry for the old child that is to be replaced
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="OLD_child_screen_id", vValue:=CStr(lChildOldScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If

                            ' this is the new child id
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="NEW_child_screen_id", vValue:=CStr(lChildNewScreenID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If

                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetChildSQL, sSQLName:=ACSetChildName, bStoredProcedure:=ACSetChildStored)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMFalse
                                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                                Return result
                            End If
                    End Select
                    ' RAW 08/07/2003 : CQ1596 : end

                Next lTemp
            End If


            r_vNewScreenID = lThisNewScreenID


            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' RAW 08/07/2003 : CQ1596 : added arguments
            m_lReturn = CType(CopyScripts(v_sOldScreenCode:=vOldScreenCode, v_sNewScreenCode:=vNewScreenCode), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' RAW 08/07/2003 : CQ1596 : added
            iCallDepth -= 1

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyScreen Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyScreen", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

            ' RAW 08/07/2003 : CQ1596 : added
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            Return result


            ' RAW 08/07/2003 : CQ1596 : added
        Finally
            iCallDepth -= 1


        End Try
        Return result
    End Function


    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetCaptionID
    '
    ' Description: Calls the sp_pm_caption_id_return stored procedure
    '              to either get or create a caption_id
    '
    ' ***************************************************************** '
    Private Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionID As Integer) As Integer

        Dim result As Integer = 0


        ' m_iLanguageID

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oArcDatabase.Parameters.Clear()

        ' Add the parameters
        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_lCaptionID), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Perform the stored procedure
        m_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACSQLCaptionReturn, sSQLName:=ACSQLCaptionReturnName, bStoredProcedure:=ACSQLCaptionReturnStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the returned caption_id
        r_lCaptionID = m_oArcDatabase.Parameters.Item("caption_id").Value

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddScreenHeader
    '
    ' Description:
    '
    ' History: 23/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddScreenHeader() As Integer

        Dim result As Integer = 0
        Dim lCaption As Integer
        Dim sSQL, sSQL2 As String



        result = gPMConstants.PMEReturnCode.PMTrue

        'What if it's zero?  Then we're adding a new one...

        'Always get the new caption - the description may have changed...

        'developer guide no. 98
        m_lReturn = CType(GetCaptionID(v_sCaption:=m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0), r_lCaptionID:=lCaption), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_vScreenHeader(PBDatabaseConsts.ACHCaptionId, 0) = lCaption


        If m_lScreenId > 0 Then
            sSQL2 = ACUpdateScreenHeaderName
            sSQL = "spu_GIS_Screen_upd_ex"
            'developer guide no. 
            bPMAddParameter.AddParameterLite(m_oDatabase, "GIS_screen_id", m_lScreenId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

        Else
            sSQL2 = ACInsertScreenHeaderName
            sSQL = ACInsertScreenHeaderSQL
            'developer guide no. 
            bPMAddParameter.AddParameterLite(m_oDatabase, "GIS_screen_id", m_lScreenId, gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMLong, True)
        End If
        sSQL2 = ACInsertScreenHeaderName

        'developer guide no. 
        If m_vScreenHeader(PBDatabaseConsts.ACHGISDataModelId, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "GIS_data_model_id", m_vScreenHeader(PBDatabaseConsts.ACHGISDataModelId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "GIS_data_model_id", CInt(m_vScreenHeader(PBDatabaseConsts.ACHGISDataModelId, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If
        'developer guide no. 
        If m_vScreenHeader(PBDatabaseConsts.ACHCaptionId, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "caption_id", m_vScreenHeader(PBDatabaseConsts.ACHCaptionId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "caption_id", CInt(m_vScreenHeader(PBDatabaseConsts.ACHCaptionId, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If

        'developer guide no. 
        If m_vScreenHeader(PBDatabaseConsts.ACHCode, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", m_vScreenHeader(PBDatabaseConsts.ACHCode, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "code", CStr(m_vScreenHeader(PBDatabaseConsts.ACHCode, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        End If

        'developer guide no. 
        If m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "description", CStr(m_vScreenHeader(PBDatabaseConsts.ACHDescription, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHIsDeleted, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", m_vScreenHeader(PBDatabaseConsts.ACHIsDeleted, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", CInt(m_vScreenHeader(PBDatabaseConsts.ACHIsDeleted, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If

        'developer guide no. 
        If m_vScreenHeader(PBDatabaseConsts.ACHEffectiveDate, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", CDate(m_vScreenHeader(PBDatabaseConsts.ACHEffectiveDate, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", CDate(m_vScreenHeader(PBDatabaseConsts.ACHEffectiveDate, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHParentId, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "parent_id", m_vScreenHeader(PBDatabaseConsts.ACHParentId, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        Else
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, m_lReturn, "parent_id", CInt(m_vScreenHeader(PBDatabaseConsts.ACHParentId, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHIsMaintainable, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_maintainable", m_vScreenHeader(PBDatabaseConsts.ACHIsMaintainable, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_maintainable", CInt(m_vScreenHeader(PBDatabaseConsts.ACHIsMaintainable, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If

        bPMAddParameter.AddParameterLite(m_oDatabase, "script_defaults", Convert.ToString(m_vScreenHeader(PBDatabaseConsts.ACHScriptDefaults, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)



        'developer guide no. 
        If m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "script_dynamic_logic", m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "script_dynamic_logic", CStr(m_vScreenHeader(PBDatabaseConsts.ACHScriptDynamicLogic, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        End If


        If m_vScreenHeader(PBDatabaseConsts.ACHScreenType, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_type", m_vScreenHeader(PBDatabaseConsts.ACHScreenType, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_type", CInt(m_vScreenHeader(PBDatabaseConsts.ACHScreenType, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHScreenHeight, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_height", m_vScreenHeader(PBDatabaseConsts.ACHScreenHeight, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_height", CInt(m_vScreenHeader(PBDatabaseConsts.ACHScreenHeight, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHScreenWidth, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_width", m_vScreenHeader(PBDatabaseConsts.ACHScreenWidth, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_width", CInt(m_vScreenHeader(PBDatabaseConsts.ACHScreenWidth, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHEnableCompiledRule, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_rule_set_type_id", m_vScreenHeader(PBDatabaseConsts.ACHEnableCompiledRule, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_rule_set_type_id", CInt(m_vScreenHeader(PBDatabaseConsts.ACHEnableCompiledRule, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyDefaults, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "file_name_Defaults", m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyDefaults, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "file_name_Defaults", CStr(m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyDefaults, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        End If

        If m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyValidation, 0) Is DBNull.Value Then
            bPMAddParameter.AddParameterLite(m_oDatabase, "file_name_Validation", m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyValidation, 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        Else
            bPMAddParameter.AddParameterLite(m_oDatabase, "file_name_Validation", CStr(m_vScreenHeader(PBDatabaseConsts.ACHCompiledRuleAssemblyValidation, 0)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL, sSQL2, True)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lScreenId = m_oDatabase.Parameters.Item("GIS_screen_id").Value

        'developer guide no. 65
        m_vScreenHeader(PBDatabaseConsts.ACHScreenId, 0) = m_lScreenId



        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: AddScreenDetails
    '
    ' Description:
    '
    ' History: 23/06/2000 Tomo - Created.
    '
    ' ***************************************************************** '
    Private Function AddScreenDetails() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not Information.IsArray(m_vScreenDetails) Then
            Return result
        End If


        For lCount As Integer = m_vScreenDetails.GetLowerBound(1) To m_vScreenDetails.GetUpperBound(1)
            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            sSQL = ACInsertScreenDetailsSQL


            'developer guide no. 65
            m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount) = m_lScreenId



            'developer guide no.  replace AddParameter with AddParameterLite 

            bPMAddParameter.AddParameterLite(m_oDatabase, "GIS_screen_id", m_vScreenDetails(PBDatabaseConsts.ACDGISScreenId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "screen_detail_cnt", m_vScreenDetails(PBDatabaseConsts.ACDScreenDetailCnt, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_object_id", m_vScreenDetails(PBDatabaseConsts.ACDGISObjectId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_property_id", m_vScreenDetails(PBDatabaseConsts.ACDGISPropertyId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_frame", (m_vScreenDetails(PBDatabaseConsts.ACDIsFrame, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "tab_number", (m_vScreenDetails(PBDatabaseConsts.ACDTabNumber, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "caption", m_vScreenDetails(PBDatabaseConsts.ACDCaption, lCount).ToString(), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "item_top", m_vScreenDetails(PBDatabaseConsts.ACDTop, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "item_left", m_vScreenDetails(PBDatabaseConsts.ACDLeft, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "item_height", m_vScreenDetails(PBDatabaseConsts.ACDHeight, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "item_width", m_vScreenDetails(PBDatabaseConsts.ACDWidth, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "column_width", m_vScreenDetails(PBDatabaseConsts.ACDColumnWidth, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "pre_quote_requirement", m_vScreenDetails(PBDatabaseConsts.ACDPreQuoteRequirement, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "post_quote_requirement", m_vScreenDetails(PBDatabaseConsts.ACDPostQuoteRequirement, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "purchase_requirement", m_vScreenDetails(PBDatabaseConsts.ACDPurchaseRequirement, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "parent_id", m_vScreenDetails(PBDatabaseConsts.ACDParentId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "help_text", m_vScreenDetails(PBDatabaseConsts.ACDHelpText, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "default_object_id", m_vScreenDetails(PBDatabaseConsts.ACDDefaultObjectId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "default_property_id", m_vScreenDetails(PBDatabaseConsts.ACDDefaultPropertyId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_valuation", m_vScreenDetails(PBDatabaseConsts.ACDIsValuation, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_rate_and_premium", m_vScreenDetails(PBDatabaseConsts.ACDIsRateAndPremium, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "child_screen_id", m_vScreenDetails(PBDatabaseConsts.ACDChildScreenId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "PMFormat", m_vScreenDetails(PBDatabaseConsts.ACDPMFormat, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "column_position", m_vScreenDetails(PBDatabaseConsts.ACDColumnPosition, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            If m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) Is Nothing Then
                m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount) = System.DBNull.Value
            End If
            bPMAddParameter.AddParameterLite(m_oDatabase, "tab_set_index", (m_vScreenDetails(PBDatabaseConsts.ACDTabSetIndex, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            bPMAddParameter.AddParameterLite(m_oDatabase, "data_model_type", CInt(IIf(m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount).ToString() = "", 0, m_vScreenDetails(PBDatabaseConsts.ACDDataModelType, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:=ACInsertScreenDetailsName, bStoredProcedure:=ACInsertScreenDetailsStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next lCount

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyScripts
    '
    ' Description:
    '
    ' History: 22/02/2001 Tomo - Created.
    ' RAW 08/07/2003 : CQ1596 : added params v_sOldScreenCode , v_sNewScreenCode to replace module variables
    '
    ' ***************************************************************** '
    Private Function CopyScripts(ByVal v_sOldScreenCode As String, ByVal v_sNewScreenCode As String) As Integer

        Dim result As Integer = 0
        Dim sRulePath, sDir As String



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(GetRulePath(r_sRulePath:=sRulePath), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sDir = ""
        sDir = FileSystem.Dir(sRulePath & v_sOldScreenCode.Trim() & "Val.Rul", FileAttribute.Normal)

        If sDir <> "" Then
            File.Copy(sRulePath & v_sOldScreenCode.Trim() & "Val.Rul", sRulePath & v_sNewScreenCode.Trim() & "Val.Rul")
        End If

        sDir = ""
        sDir = FileSystem.Dir(sRulePath & v_sOldScreenCode.Trim() & "Def.Rul", FileAttribute.Normal)

        If sDir <> "" Then
            File.Copy(sRulePath & v_sOldScreenCode.Trim() & "Def.Rul", sRulePath & v_sNewScreenCode.Trim() & "Def.Rul")
        End If

        Return result

    End Function

    Private Function GetRulePath(ByRef r_sRulePath As String) As Integer

        Dim result As Integer = 0
        Dim sSubKey As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        sSubKey = "GIS\" & m_sGISDataModelCode

        'Get it from the server, as that's where we're going to hold _all_ the
        'GIS registry settings (except list management, of course)
        '   m_lReturn = GetPMRegSetting(v_lPMERegSettingRoot:=pmeRSRLocalMachine, _
        'v_lPMEProductFamily:=pmePFSiriusSolutions, _
        'v_lPMERegsettinglevel:=pmeRSLCommon, _
        'v_sSettingName:="RulePath", _
        'r_sSettingValue:=r_sRulePath, _
        'v_sSubKey:=sSubKey)

        m_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", r_sSettingValue:=r_sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

        If r_sRulePath = "" Then
            result = gPMConstants.PMEReturnCode.PMFalse
        Else
            If Not r_sRulePath.EndsWith("\") Then
                r_sRulePath = r_sRulePath & "\"
            End If
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
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    '
    ' Name: GetChildScreens
    '
    ' Description:
    '
    ' History: 21/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function GetChildScreens() As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetChildScreens")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetChildScreens")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetChildScreens")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetChildScreens Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetChildScreens", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetScreensByObjectType
    '
    ' Description: Gets the screen data for a given object type
    '
    ' History: 21/01/2003 CLG - Created.
    '
    ' ***************************************************************** '
    Public Function GetScreensByObjectType(ByVal v_lScreenType As Integer, ByVal v_lDataModelId As Integer, ByRef r_vArray(,) As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetScreensByObjectType")

        Try

            Dim sSQL, sSQL2 As String
            Dim iRetval As gPMConstants.PMEReturnCode

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            sSQL = ACSQLGetScreensByObjecTypeSQL
            sSQL2 = ACSQLGetScreensByObjecTypeName
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetval, "screen_type", v_lScreenType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameter(m_oDatabase, sSQL, iRetval, "gis_data_model_id", v_lDataModelId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            If iRetval <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=sSQL2, bStoredProcedure:=ACSQLGetScreensByObjecTypeStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vArray, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If




            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetScreensByObjectType")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetScreensByObjectType")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetScreensByObjectType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetScreensByObjectType", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetDataModelTypeId
    '
    ' Description: get Data Model type ID
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetDataModelTypeId(ByVal v_sGISDataModelCode As String, ByRef r_lDataModelTypeId As Integer) As Integer

        Dim result As Integer = 0

        Dim vArray(,) As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters
        'developer guide no. cint removed
        bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_sGISDataModelCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetDataModelDetailsSQL, sSQLName:=kGetDataModelDetailsName, bStoredProcedure:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vArray, bKeepNulls:=True)

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Or Not Information.IsArray(vArray) Then
            r_lDataModelTypeId = 0
            Return result
        ElseIf (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If CStr(vArray(0, 0)).Trim().ToUpper() = "CASE" Then
            r_lDataModelTypeId = GISDataModelType.GISDMTypeCase
        End If

        Return result
    End Function
    'Start(Sriram P)CacheBug
       Public Function UpdateCache(ByVal v_lScreenId As Integer) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCache"
        Try
            Catch_Renamed = True


            result = gPMConstants.PMEReturnCode.PMTrue


            ' RAM20040511 : Code changes related to Caching
            ' Const SIRIUS_CACHE_KEYS As String = "SIRIUS_CACHE_KEYS"
            'developer guide no. 12
            '  Dim oCache As Hashtable
            Dim sKey As String = ""
            'Dim vDataDictionary As Object
            'Dim vScreenHeader As Object
            'Dim vScreenDetails As Object
            'Dim vChildScreenDetails As Object
            'Dim vKeyArray As Object
            '' Default values
            ''developer guide no. 12
            'Dim r_vScreenHeader(,) As Object
            'Dim r_vScreenDetails(,) As Object
            'Dim r_vChildScreenDetails(,) As Object
            'Dim r_vDataDictionary() As Object

            Dim sCachePath As String = ""
            Dim sCacheFilename As String = ""
            'developer guide no. 12
            'r_vScreenHeader = ""
            'r_vScreenDetails = ""
            'r_vChildScreenDetails = ""
            ' Create the Cache Object
            'developer guide no. 12
            '  oCache = New Hashtable()

            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception
            End Try

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(sCachePath, 1) <> "\" Then
                sCachePath += "\"
            End If

            m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeySystemOptionCacheFileName, r_sSettingValue:=sCacheFilename)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            'get the screen details for each data model type
            For lTemp As Integer = GISDataModelType.GISDMTypeRisk To GISDataModelType.GISDMTypeRisk 'TBD

                sKey = "KEY_DATA_MODEL_TYPE_" & StringsHelper.Format(lTemp, "00000") & "_GIS_SCREEN_" & StringsHelper.Format(v_lScreenId, "00000") & "_DATA_DICTIONARY"

                If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                    iCache.Remove(sKey)
                End If

                'ReDim r_vDataDictionary(lTemp)

                '' r_vDataDictionary(lTemp) = ""
                '' Create key for the input parameters
                '' eg. KEY_KEY_DATA_MODEL_TYPE_00001_GIS_SCREEN_00026_DATA_DICTIONARY :
                ''   means : The data is the data Dictionary for data Model Type 1 and GIS Screen ID 26


                '' Clear it before we get it from cache
                'vDataDictionary = Nothing

                '' Get from the Cache by the Key, if available


                ''developer guide no. 12
                'vDataDictionary = oCache.Item(sKey)




                '' Not in the CACHE, therefore we need to hit the database to get the value


                '' The data is not cached so, use the usual way to fetch the data

                '' Clear the Database Parameters Collection
                'm_oDatabase.Parameters.Clear()

                'm_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(v_lScreenId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


                'm_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_type_id", vValue:=CStr(lTemp), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError(kMethodName, "Failed to add the input parameters")

                '    ' Cleanup Memory
                '    If oCache Is Nothing Then
                '    Else
                '        oCache = Nothing
                '    End If
                '    Return result
                'End If

                '' Execute SQL Statement
                'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateDataDictionarySQL, sSQLName:=ACUpdateDataDictionaryName, bStoredProcedure:=ACUpdateDataDictionaryStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vDataDictionary(lTemp))

                'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '    gPMFunctions.RaiseError(kMethodName, "Failed to get the values from database")
                '    ' Cleanup Memory
                '    If oCache Is Nothing Then
                '    Else
                '        oCache = Nothing
                '    End If
                '    Return result
                'End If

                '' Do we have any records ?
                'If lTemp = GISDataModelType.GISDMTypeRisk And Not Information.IsArray(r_vDataDictionary) Then
                '    ' No Records, return PMNotFound
                '    result = gPMConstants.PMEReturnCode.PMNotFound

                '    ' Cleanup Memory
                '    If oCache Is Nothing Then
                '    Else
                '        oCache = Nothing
                '    End If
                '    Return result
                'End If

                '' We got the values, so put them in the CACHE

                '' Add them to the Cache
                'oCache.Add(sKey, r_vDataDictionary(lTemp))

                '' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                '' Sirius Cache Controller


                'vKeyArray = oCache.Item(SIRIUS_CACHE_KEYS)

                'If Object.Equals(vKeyArray, Nothing) Then
                '    ReDim vKeyArray(0)
                'Else

                '    ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
                'End If


                'vKeyArray(vKeyArray.GetUpperBound(0)) = sKey
                '' Remove the existing keys first
                'oCache.Remove(SIRIUS_CACHE_KEYS)
                '' Add the updated one
                'oCache.Add(SIRIUS_CACHE_KEYS, vKeyArray)

            Next

            'Now get the screen header

            ' Create key for the input parameters
            ' eg. KEY_GIS_SCREEN_00026_HEADER_DETAILS :  means : The data is the HEADER_DETAILS for the GIS Screen ID 26
            sKey = "KEY_GIS_SCREEN_" & StringsHelper.Format(v_lScreenId, "00000") & "_HEADER_DETAILS"

            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                iCache.Remove(sKey)
            End If

            'vScreenHeader = oCache.Item(sKey)

            '' Not in the CACHE, therefore we need to hit the database to get the value


            '' Clear the Database Parameters Collection
            'm_oDatabase.Parameters.Clear()

            ''Add the screen
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(v_lScreenId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "Failed to add the input parameters")
            '    ' Cleanup Memory
            '    If oCache Is Nothing Then
            '    Else
            '        oCache = Nothing
            '    End If
            '    Return result
            'End If

            '' Execute SQL Statement
            'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateAllScreenHeaderSQL, sSQLName:=ACUpdateAllScreenHeaderName, bStoredProcedure:=ACUpdateAllScreenHeaderStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScreenHeader, bKeepNulls:=True)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "Failed to get the values from database")
            '    ' Cleanup Memory
            '    If oCache Is Nothing Then
            '    Else
            '        oCache = Nothing
            '    End If
            '    Return result
            'End If

            '' Add them to the Cache
            'oCache.Add(sKey, r_vScreenHeader)

            '' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
            '' Sirius Cache Controller


            'vKeyArray = oCache.Item(SIRIUS_CACHE_KEYS)

            'If Object.Equals(vKeyArray, Nothing) Then
            '    ReDim vKeyArray(0)
            'Else

            '    ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
            'End If


            'vKeyArray(vKeyArray.GetUpperBound(0)) = sKey
            '' Remove the existing keys first
            ''developer guide no. 12
            'oCache.Remove(SIRIUS_CACHE_KEYS)
            ''' Add the updated one
            'oCache.Add(SIRIUS_CACHE_KEYS, vKeyArray)


            'Now get the screen details

            ' Create key for the input parameters
            ' eg. KEY_GIS_SCREEN_00026_DETAILS :  means : The data is the DETAILS for the GIS Screen ID 26
            sKey = "KEY_GIS_SCREEN_" & StringsHelper.Format(v_lScreenId, "00000") & "_DETAILS"

            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                iCache.Remove(sKey)
            End If

            ''developer guide no. 12
            'vScreenDetails = oCache.Item(sKey)

            '' Not in the CACHE, therefore we need to hit the database to get the value


            '' Clear the Database Parameters Collection
            'm_oDatabase.Parameters.Clear()

            ''Add the screen
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(v_lScreenId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "Failed to add the input parameters")
            '    ' Cleanup Memory
            '    'developer guide no. 12
            '    If oCache Is Nothing Then
            '    Else
            '        oCache = Nothing
            '    End If
            '    Return result
            'End If

            '' Execute SQL Statement
            'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateAllScreenDetailsSQL, sSQLName:=ACUpdateAllScreenDetailsName, bStoredProcedure:=ACUpdateAllScreenDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vScreenDetails, bKeepNulls:=True)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "Failed to get the values from database")
            '    ' Cleanup Memory
            '    If oCache Is Nothing Then
            '    Else
            '        oCache = Nothing
            '    End If
            '    Return result
            'End If

            '' Add them to the Cache
            'oCache.Add(sKey, r_vScreenDetails)

            '' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
            '' Sirius Cache Controller


            'vKeyArray = oCache.Item(SIRIUS_CACHE_KEYS)

            'If Object.Equals(vKeyArray, Nothing) Then
            '    ReDim vKeyArray(0)
            'Else

            '    ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
            'End If


            'vKeyArray(vKeyArray.GetUpperBound(0)) = sKey
            '' Remove the existing keys first
            'oCache.Remove(SIRIUS_CACHE_KEYS)
            ''' Add the updated one
            'oCache.Add(SIRIUS_CACHE_KEYS, vKeyArray)


            'Now get the child screen details

            ' Create key for the input parameters
            ' eg. KEY_GIS_SCREEN_00026_CHILD_SCREEN_DETAILS :  means : The data is the CHILD SCREEN DETAILS for the GIS Screen ID 26
            sKey = "KEY_GIS_SCREEN_" & StringsHelper.Format(v_lScreenId, "00000") & "_CHILD_SCREEN_DETAILS"


            ' Get from the Cache by the Key, if available
            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                iCache.Remove(sKey)
            End If

            'vChildScreenDetails = oCache.Item(sKey)

            '' Not in the CACHE, therefore we need to hit the database to get the value


            '' Clear the Database Parameters Collection
            'm_oDatabase.Parameters.Clear()

            ''Add the screen
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_screen_id", vValue:=CStr(v_lScreenId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "Failed to add the input parameters")
            '    ' Cleanup Memory
            '    'developer guide no. 12
            '    If oCache Is Nothing Then
            '    Else
            '        oCache = Nothing
            '    End If
            '    Return result
            'End If

            '' Execute SQL Statement
            'm_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateAllChildScreenDetailsSQL, sSQLName:=ACUpdateAllChildScreenDetailsName, bStoredProcedure:=ACUpdateAllChildScreenDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vChildScreenDetails, bKeepNulls:=True)

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    gPMFunctions.RaiseError(kMethodName, "Failed to get the values from database")
            '    ' Cleanup Memory
            '    If oCache Is Nothing Then
            '    Else
            '        oCache = Nothing
            '    End If
            '    Return result
            'End If

            '' Add them to the Cache
            'oCache.Add(sKey, r_vChildScreenDetails)

            '' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
            '' Sirius Cache Controller


            'vKeyArray = oCache.Item(SIRIUS_CACHE_KEYS)


            'If Object.Equals(vKeyArray, Nothing) Then
            '    ReDim vKeyArray(0)
            'Else

            '    ReDim Preserve vKeyArray(vKeyArray.GetUpperBound(0) + 1)
            'End If


            'vKeyArray(vKeyArray.GetUpperBound(0)) = sKey
            '' Remove the existing keys first
            'oCache.Remove(SIRIUS_CACHE_KEYS)
            ''' Add the updated one
            'oCache.Add(SIRIUS_CACHE_KEYS, vKeyArray)


            '' Cleanup Memory
            'If oCache Is Nothing Then
            'Else
            '    oCache = Nothing
            'End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then


                ' DO Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

            End If
Finally_Renamed:
        End Try
    End Function
    'End(Sriram P)CacheBug
End Class
