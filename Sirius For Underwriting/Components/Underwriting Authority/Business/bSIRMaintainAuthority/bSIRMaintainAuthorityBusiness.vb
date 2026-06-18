Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide No. 129
Imports SharedFiles
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
    ' Added to replace global variables 22/12/2003
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

    ' Database Class (Private)
    Private m_oArcDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseArcDatabase As Boolean

    Private m_vDataDictionary As String = ""

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lGISDataModelId As Integer

    Private m_lStatus As Integer

    Private Const ACAdd As Integer = 1
    Private Const ACDelete As Integer = 2

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' Rule Properties
    Private m_lGISObjectId As Integer
    Private m_lGISPropertyId As Integer
    Private m_sDescription As String = ""
    Private m_sCode As String = ""
    Private m_iIsDeleted As Integer
    Private m_lCaptionId As Integer

    Public Property CaptionId() As Integer
        Get
            Return m_lCaptionId
        End Get
        Set(ByVal Value As Integer)
            m_lCaptionId = Value
        End Set
    End Property


    Public Property IsDeleted() As Integer
        Get
            Return m_iIsDeleted
        End Get
        Set(ByVal Value As Integer)
            m_iIsDeleted = Value
        End Set
    End Property

    Public Property Code() As String
        Get
            Return m_sCode
        End Get
        Set(ByVal Value As String)
            m_sCode = Value
        End Set
    End Property


    Public Property Description() As String
        Get
            Return m_sDescription
        End Get
        Set(ByVal Value As String)
            m_sDescription = Value
        End Set
    End Property


    Public Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property


    Public Property GISPropertyId() As Integer
        Get
            Return m_lGISPropertyId
        End Get
        Set(ByVal Value As Integer)
            m_lGISPropertyId = Value
        End Set
    End Property



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

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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
                m_vDataDictionary = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
                If m_bCloseArcDatabase Then

                    m_oArcDatabase.CloseDatabase()

                    m_oArcDatabase = Nothing

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
    Public Function GetDetails(ByRef r_vDataDictionary As String) As Integer

        Dim result As Integer = 0
        Dim lDataModelId As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_vDataDictionary = ""

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
    Public Function GetNext(ByRef r_vDataDictionary As Object, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object, ByRef r_vChildScreenDetails As Object) As Integer

        '    On Error GoTo Err_GetNext
        '
        '    GetNext = PMTrue
        '
        '    r_vDataDictionary = m_vDataDictionary
        '    r_vScreenHeader = m_vScreenHeader
        '    r_vScreenDetails = m_vScreenDetails
        '    r_vChildScreenDetails = m_vChildScreenDetails

        Dim result As Integer = 0
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Sets the arrays ready for updating
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef r_vDataDictionary As Object, ByRef r_vScreenHeader As Object, ByRef r_vScreenDetails As Object) As Integer

        Dim result As Integer = 0
        Try


            '    m_vDataDictionary = r_vDataDictionary
            '    m_vScreenHeader = r_vScreenHeader
            '    m_vScreenDetails = r_vScreenDetails

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    '' Name: Update (Public)
    ''
    '' Description: Updates all data
    ''
    '' ***************************************************************** '
    'Public Function Update() As Long
    '
    '    On Error GoTo Err_Update
    '
    '    Update = PMTrue
    '
    '    If (m_iTask = PMAdd) Then
    '
    '        m_lReturn = AddCaption(m_sDescription, m_lCaptionId)
    '        If (m_lReturn& <> PMTrue) Then
    '            Update = PMFalse
    '            Exit Function
    '        End If
    '
    '    End If
    '
    '    m_lReturn = BeginTrans()
    '
    '    'First we delete the screen details, foreign keys, don't you know
    '    ' Clear the Database Parameters Collection
    '    m_oDatabase.Parameters.Clear
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_property_id", _
    ''                                           vValue:=m_lGISPropertyId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_object_id", _
    ''                                           vValue:=m_lGISObjectId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="code", _
    ''                                           vValue:=m_sCode, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMString)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="description", _
    ''                                           vValue:=m_sDescription, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMString)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="is_deleted", _
    ''                                           vValue:=m_iIsDeleted, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMInteger)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", _
    ''                                           vValue:=m_dtEffectiveDate, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMDate)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="failure_consequence_id", _
    ''                                           vValue:=m_lFailureConsequenceId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="lower_bound", _
    ''                                           vValue:=m_lLowerBound, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="upper_bound", _
    ''                                           vValue:=m_lUpperBound, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = m_oDatabase.Parameters.Add(sName:="caption_id", _
    ''                                           vValue:=m_lCaptionId, _
    ''                                           iDirection:=PMParamInput, _
    ''                                           iDataType:=PMLong)
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    If (m_iTask = PMAdd) Then
    '
    '        m_lReturn = m_oDatabase.Parameters.Add(sName:="authority_rule_id", _
    ''                                               vValue:=m_lRuleId, _
    ''                                               iDirection:=PMParamOutput, _
    ''                                               iDataType:=PMLong)
    '
    '        If (m_lReturn& <> PMTrue) Then
    '            Update = PMFalse
    '            m_lReturn = RollbackTrans()
    '            Exit Function
    '        End If
    '
    '        ' Execute SQL Statement
    '        m_lReturn& = m_oDatabase.SQLAction( _
    ''            sSQL:=ACInsertAuthorityRuleSQL, _
    ''            sSQLName:=ACInsertAuthorityRuleName, _
    ''            bStoredProcedure:=ACInsertAuthorityRuleStored)
    '
    '        If (m_lReturn& <> PMTrue) Then
    '            Update = PMFalse
    '            m_lReturn = RollbackTrans()
    '            Exit Function
    '        End If
    '
    '        m_lRuleId = m_oDatabase.Parameters.Item("authority_rule_id").Value
    '
    '
    '    Else
    '
    '        m_lReturn = m_oDatabase.Parameters.Add(sName:="authority_rule_id", _
    ''                                               vValue:=m_lRuleId, _
    ''                                               iDirection:=PMParamInput, _
    ''                                               iDataType:=PMLong)
    '
    '        If (m_lReturn& <> PMTrue) Then
    '            Update = PMFalse
    '            m_lReturn = RollbackTrans()
    '            Exit Function
    '        End If
    '
    '        ' Execute SQL Statement
    '        m_lReturn& = m_oDatabase.SQLAction( _
    ''            sSQL:=ACUpdateAuthorityRuleSQL, _
    ''            sSQLName:=ACUpdateAuthorityRuleName, _
    ''            bStoredProcedure:=ACUpdateAuthorityRuleStored)
    '
    '        If (m_lReturn& <> PMTrue) Then
    '            Update = PMFalse
    '            m_lReturn = RollbackTrans()
    '            Exit Function
    '        End If
    '
    '    End If
    '
    '    m_lReturn = UpdateInclusions()
    '
    '    If (m_lReturn& <> PMTrue) Then
    '        Update = PMFalse
    '        m_lReturn = RollbackTrans()
    '        Exit Function
    '    End If
    '
    '    m_lReturn = CommitTrans()
    '
    '    Exit Function
    '
    'Err_Update:
    '
    '    Update = PMError
    '    m_lReturn = RollbackTrans()
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="Update Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="Update", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function
    '

    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: GetCaptionID
    '
    ' Description: Calls the spu_pm_caption_id_return stored procedure
    '              to either get or create a caption_id
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetCaptionID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    ' m_iLanguageID
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Clear the parameters
    'm_oArcDatabase.Parameters.Clear()
    '
    ' Add the parameters
    'm_lReturn = m_oArcDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = m_oArcDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(r_lCaptionId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Perform the stored procedure
    'm_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACSQLCaptionReturn, sSQLName:=ACSQLCaptionReturnName, bStoredProcedure:=ACSQLCaptionReturnStored)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    ' Get the returned caption_id
    'r_lCaptionId = m_oArcDatabase.Parameters.Item("caption_id").Value
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function


    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLBeginTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CommitTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLCommitTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RollbackTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lReturn = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
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
    ' Name: GetLookupValues
    '
    ' Description: Retrieves array of standard PM Lookup values.
    '
    ' History: 01/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef v_iLanguageId As Integer, ByRef v_dtEffectiveDate As Date, ByRef v_sTableName As String, ByRef r_vLookupArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the language id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(v_iLanguageId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the effective date
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the table name
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAllSQL, sSQLName:=ACSelectAllName, bStoredProcedure:=ACSelectAllStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vLookupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vLookupArray) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetGISUserDefDetail
    '
    ' Description:
    '
    ' History: 01/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetGISUserDefDetail(ByRef v_lGISUserDefHeaderId As Integer, ByRef r_vLookupArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the table name
            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_user_def_header_id", vValue:=CStr(v_lGISUserDefHeaderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectGISUserDefDetailSQL, sSQLName:=ACSelectGISUserDefDetailName, bStoredProcedure:=ACSelectGISUserDefDetailStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vLookupArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vLookupArray) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISUserDefDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISUserDefDetail", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: AddCaption
    '
    ' Description: Inserts caption into Architecture database and returns
    '               new Id.
    '
    ' History: 06/11/2000 RWH - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddCaption) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddCaption(ByRef v_sCaption As String, ByRef r_lCaptionId As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim oArchDatabase As dPMDAO.Database
    '
    'Const sADD_PM_CAPTION As String = "{call spu_pm_caption_id_return (?,?,?)}"
    'Const sADD_PM_CAPTION_NAME As String = "AddPMCaption"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '
    'm_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oArchDatabase), gPMConstants.PMEReturnCode)
    '
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'oArchDatabase.Parameters.Clear()
    '
    'm_lReturn = oArchDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(m_iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'm_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oArchDatabase.Parameters.Add(sName:="caption", vValue:=v_sCaption, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oArchDatabase.Parameters.Add(sName:="caption_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'm_lReturn = oArchDatabase.SQLAction(sSQL:=sADD_PM_CAPTION, sSQLName:=sADD_PM_CAPTION_NAME, bStoredProcedure:=True)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '        m_lReturn = RollbackTrans()
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Get returned caption_id to pass into AddScheme proc.
    'r_lCaptionId = oArchDatabase.Parameters.Item("caption_id").Value
    'oArchDatabase.CloseDatabase()
    'oArchDatabase = Nothing
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddCaption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCaption", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    '' ***************************************************************** '
    ''
    '' Name: SetProperties
    ''
    '' Description: Set all rule properties within business object.
    ''
    '' History: 06/11/2000 RWH - Created.
    ''
    '' ***************************************************************** '
    'Public Function SetProperties(Optional ByVal lRuleId As Long, _
    ''                                Optional ByVal lGISObjectId As Long, _
    ''                                Optional ByVal lGISPropertyId As Long, _
    ''                                Optional ByVal sDescription As String, _
    ''                                Optional ByVal sCode As String, _
    ''                                Optional ByVal dtEffectiveDate As Date, _
    ''                                Optional ByVal vInclusions As Variant) As Long
    '
    '    On Error GoTo Err_SetProperties
    '
    '    SetProperties = PMTrue
    '
    '    If (Not (IsMissing(lRuleId))) Then
    '        m_lRuleId = lRuleId
    '    End If
    '
    '    If (Not (IsMissing(lFailureConsequence))) Then
    '        m_lFailureConsequenceId = lFailureConsequence
    '    End If
    '
    '    If (Not (IsMissing(lGISObjectId))) Then
    '        m_lGISObjectId = lGISObjectId
    '    End If
    '
    '    If (Not (IsMissing(lGISPropertyId))) Then
    '        m_lGISPropertyId = lGISPropertyId
    '    End If
    '
    '    If (Not (IsMissing(lLowerBound))) Then
    '        m_lLowerBound = lLowerBound
    '    End If
    '
    '    If (Not (IsMissing(lUpperBound))) Then
    '        m_lUpperBound = lUpperBound
    '    End If
    '
    '    If (Not (IsMissing(sDescription))) Then
    '        m_sDescription = sDescription
    '    End If
    '
    '    If (Not (IsMissing(sCode))) Then
    '        m_sCode = sCode
    '    End If
    '
    '    If (Not (IsMissing(dtEffectiveDate))) Then
    '        m_dtEffectiveDate = dtEffectiveDate
    '    End If
    '
    '    If (Not (IsMissing(vInclusions))) Then
    '        m_vInclusions = vInclusions
    '    End If
    '
    '    Exit Function
    '
    'Err_SetProperties:
    '
    '    SetProperties = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="SetProperties Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="SetProperties", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '
    'End Function


    ' ***************************************************************** '
    '
    ' Name: GetAuthorityLevelTypes
    '
    ' Description: Retrieves all Authority Level Types on the system.
    '
    ' History: 20/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAuthorityLevelTypes(ByRef r_vAuthorityLevels(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAuthorityLevelsSQL, sSQLName:=ACSelectAuthorityLevelsName, bStoredProcedure:=ACSelectAuthorityLevelsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAuthorityLevels)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vAuthorityLevels) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAuthorityLevelTypes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAuthorityLevelTypes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetPMUsers
    '
    ' Description: Retrieve all PMUsers on the system
    '
    ' History: 20/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetPMUsers(ByRef r_vPMUsers(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oArchDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=oArchDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            oArchDatabase.Parameters.Clear()

            'Add the effective date
            'Developer Guide No. 40
            m_lReturn = oArchDatabase.Parameters.Add(sName:="Effective_Date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = oArchDatabase.SQLSelect(sSQL:=ACSelectPMUsersSQL, sSQLName:=ACSelectPMUsersName, bStoredProcedure:=ACSelectPMUsersStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vPMUsers)

            oArchDatabase.CloseDatabase()
            oArchDatabase = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vPMUsers) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMUsers Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMUsers", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetProducts
    '
    ' Description: Retrieve all products on the system.
    '
    ' History: 20/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetProducts(ByRef r_vProducts(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectProductsSQL, sSQLName:=ACSelectProductsName, bStoredProcedure:=ACSelectProductsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vProducts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vProducts) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProducts Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProducts", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetAuthorityLevelsForUSer
    '
    ' Description: Get all authority levels associated with a user.
    '
    ' History: 20/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function GetAuthorityLevelsForUser(ByVal v_lUserId As Integer, ByRef r_vUserAuthorityLevels(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the user id.
            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectUserAuthorityLevelsSQL, sSQLName:=ACSelectUserAuthorityLevelsName, bStoredProcedure:=ACSelectUserAuthorityLevelsStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vUserAuthorityLevels)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any records ?
            If Not Information.IsArray(r_vUserAuthorityLevels) Then
                ' No Records, return PMNotFound
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAuthorityLevelsForUSer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAuthorityLevelsForUSer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Update
    '
    ' Description:
    '
    ' History: 21/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function Update(ByVal v_lUserId As Integer, ByVal v_vUserDetails As Object, ByVal v_vActionArray() As Object, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (Not Information.IsArray(v_vUserDetails)) Or (Not Information.IsArray(v_vActionArray)) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For iCount As Integer = 0 To v_vUserDetails.GetUpperBound(1)

                If Conversion.Val(CStr(v_vActionArray(iCount))) <> 0 Then

                    ' Clear the Database Parameters Collection
                    m_oDatabase.Parameters.Clear()
                    Dim sScreenHierarchy As String = ""
                    'Add the product id.

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(v_vUserDetails(ACUserAuthProductId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Add the user id.
                    m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(v_lUserId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'Add the user id.

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="authority_level_type_id", vValue:=CStr(v_vUserDetails(ACUserAuthLevelTypeId, iCount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If Not String.IsNullOrEmpty(v_sScreenHierarchy) Then
                        sScreenHierarchy = v_sScreenHierarchy & $"\({CStr(v_vUserDetails(ACUserAuthProductDesc, iCount)).Trim()})"
                    End If

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    Select Case (Conversion.Val(CStr(v_vActionArray(iCount))))
                        Case ACActionAdd
                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddUserAuthorityLevelSQL, sSQLName:=ACAddUserAuthorityLevelName, bStoredProcedure:=ACAddUserAuthorityLevelStored)

                        Case ACActionUpdate
                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateUserAuthorityLevelSQL, sSQLName:=ACUpdateUserAuthorityLevelName, bStoredProcedure:=ACUpdateUserAuthorityLevelStored)

                        Case ACActionDelete
                            ' Execute SQL Statement
                            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteUserAuthorityLevelSQL, sSQLName:=ACDeleteUserAuthorityLevelName, bStoredProcedure:=ACDeleteUserAuthorityLevelStored)

                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
            Next iCount

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Delete
    '
    ' Description:
    '
    ' History: 21/12/2000 RWH - Created.
    '
    ' ***************************************************************** '
    Public Function Delete(ByVal v_lUserId As Integer, ByVal v_lProductId As Integer, ByVal v_lAuthorityLevelTypeId As Integer) As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
