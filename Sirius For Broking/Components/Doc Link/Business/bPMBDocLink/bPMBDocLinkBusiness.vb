Option Strict Off
Option Explicit On
'developer guide no.129
Imports System.Globalization
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date:  01/02/2001
    '
    ' Created By: Ajit Kumar
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PMBDocLink.
    '
    ' Edit History:
    '   26/06/2002 SJP - Merged from Carole Nash into Broking
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

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
    Private m_lSettingID As Integer

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
    ' Name: GetDocLink (Public)
    '
    ' Description: Gets the Document links from database table.
    '
    ' History: 04/02/2002 AK - Created.
    '
    ' ***************************************************************** '

    Public Function GetDocLink(ByRef r_vDocLink As Object) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetAllDocLinkSQL, sSQLName:=ACGetAllDocLinkName, bStoredProcedure:=ACGetAllDocLinkStored, vResultArray:=vResultarray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vDocLink = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRiskGroup (Public)
    '
    ' Description: Gets the Risk Groups from database table.
    '
    ' History: 04/02/2002 AK - Created.
    '
    ' ***************************************************************** '

    Public Function GetRiskGroup(ByRef r_vRiskGroup As Object) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetRiskGroupSQL, sSQLName:=ACGetRiskGroupName, bStoredProcedure:=ACGetRiskGroupStored, vResultArray:=vResultarray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vRiskGroup = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskGroup Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskGroup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetGISScheme (Public)
    '
    ' Description: Gets the Risk Groups from database table.
    '
    ' History: 13/03/2002 AK - Created.
    '
    ' ***************************************************************** '

    Public Function GetGISScheme(ByRef r_vGISScheme As Object) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                ' TF190303 - Merged AMJ 5/11/02 get all schemes
                m_lReturn = .SQLSelect(sSQL:=ACGetGISSchemeSQL, sSQLName:=ACGetGISSchemeName, bStoredProcedure:=ACGetGISSchemeStored, vResultArray:=vResultarray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vGISScheme = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetGISScheme Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetGISScheme", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProcessType (Public)
    '
    ' Description: Gets the Process Types from database table.
    '
    ' History: 04/02/2002 AK - Created.
    '
    ' ***************************************************************** '

    Public Function GetProcessType(ByRef r_vProcessType As Object, Optional ByVal v_iFunctionalArea As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="FunctionalArea", vValue:=CStr(v_iFunctionalArea), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetProcessType", " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetProcessSQL, sSQLName:=ACGetProcessName, bStoredProcedure:=ACGetProcessStored, vResultArray:=vResultarray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vProcessType = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProcessType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProcessType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    'DC040702 -Start
    ' ***************************************************************** '
    ' Name: GetAgent (Public)
    '
    ' Description: Gets the Agents from database table.
    '
    ' History: 04/07/2002 DC - Created.
    '
    ' ***************************************************************** '

    Public Function GetAgent(ByRef r_vAgent As Object) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                ' TF190303 - Merged AMJ 5/11/02 get all agents
                m_lReturn = .SQLSelect(sSQL:=ACGetAgentSQL, sSQLName:=ACGetAgentName, bStoredProcedure:=ACGetAgentStored, vResultArray:=vResultarray, lNumberRecords:=gPMConstants.PMAllRecords)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vAgent = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If

            End With


            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    'DC040702 -End


    ' ***************************************************************** '
    ' Name: GetDocType (Public)
    '
    ' Description: Gets the Document Types from database table.
    '
    ' History: 04/02/2002 AK - Created.
    '
    ' ***************************************************************** '

    Public Function GetDocType(ByRef r_vDocType As Object) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetDocTypeSQL, sSQLName:=ACGetDocTypeName, bStoredProcedure:=ACGetDocTypeStored, vResultArray:=vResultarray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vDocType = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If


            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDocTemp (Public)
    '
    ' Description: Gets the Document Templates from database table.
    '
    ' History: 04/02/2002 AK - Created.
    '
    ' ***************************************************************** '

    Public Function GetDocTemp(ByRef r_vDocTemp As Object) As Integer
        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetDocTempSQL, sSQLName:=ACGetDocTempName, bStoredProcedure:=ACGetDocTempStored, vResultArray:=vResultarray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Do we have any results?
                If Informations.IsArray(vResultarray) Then
                    ' Yes, so assign them


                    r_vDocTemp = vResultarray
                Else
                    ' No, so return Not Found
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If


            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemp Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemp", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateDocLink (Public)
    '
    ' Description: updates all the document links in the database.
    '
    ' History: 04/02/2002 AK - Created.
    ' RAM20040225   : Ref. PN Issue 7408 Changes
    ' ***************************************************************** '
    Public Function UpdateDocLink(ByVal v_vDocLink(,) As Object) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            BeginTrans()

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Execute SQL Statement to delete all existing rows
                m_lReturn = .SQLAction(sSQL:=ACDelDocLinkSQL, sSQLName:=ACDelDocLinkName, bStoredProcedure:=ACDelDocLinkStored)

                'now we will recreate all the doc-link records, using the passed array

                For iNum As Integer = 0 To v_vDocLink.GetUpperBound(1)

                    If CDbl(v_vDocLink(ACArrayIsDeleted, iNum)) <> 1 Then
                        ' Clear the Database Parameters Collection
                        .Parameters.Clear()

                        'Risk Group

                        m_lReturn = .Parameters.Add(sName:="GIS_Scheme_ID", vValue:=CStr(v_vDocLink(ACArraylGISSchemeID, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Process_Type

                        m_lReturn = .Parameters.Add(sName:="Process_Type_ID", vValue:=CStr(v_vDocLink(ACArraylProcessID, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'DC030702 agent parameter
                        'Agent

                        Dim dbNumericTemp As Double
                        If Double.TryParse(CStr(v_vDocLink(ACArraylAgentID, iNum)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                            m_lReturn = .Parameters.Add(sName:="Agent_ID", vValue:=CStr(v_vDocLink(ACArraylAgentID, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Else

                            'Developer Guide No 85
                            m_lReturn = .Parameters.Add(sName:="Agent_ID", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'Document_Type

                        m_lReturn = .Parameters.Add(sName:="Document_Type_ID", vValue:=CStr(v_vDocLink(ACArraylDocumentTypeID, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            RollbackTrans()
                            Return result
                        End If

                        'Document_Template
                        ' CJB 300802 Change frompassing Document Template Desccription to the
                        ' sp to passing the unique Document Template ID to prevent uniqueness
                        ' related errors that occurred if there was a template with a non-unique
                        ' description

                        m_lReturn = .Parameters.Add(sName:="Document_Template_ID", vValue:=CStr(v_vDocLink(ACArrayDocTempID, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        'DC190702 -start

                        m_lReturn = .Parameters.Add(sName:="Spool_Document", vValue:=CStr(v_vDocLink(ACArraySpoolDocument, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                        'DC190702 -end

                        'JES 260303 PN 3270 Scheme_Ver
                        ' Scheme_Ver

                        m_lReturn = .Parameters.Add(sName:="Scheme_Ver", vValue:=CStr(v_vDocLink(ACArrayDocSchemeVer, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' RAM20040225 : PN Issue 7408 Changes

                        m_lReturn = .Parameters.Add(sName:="Auto_Archive_Document", vValue:=CStr(v_vDocLink(ACArrayAutoArchiveDocument, iNum)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Execute SQL Statement
                        m_lReturn = .SQLAction(sSQL:=ACInsDocLinkSQL, sSQLName:=ACInsDocLinkName, bStoredProcedure:=ACInsDocLinkStored)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RollbackTrans()
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If


                    End If
                Next

            End With

            CommitTrans()

            Return result

        Catch excep As System.Exception



            RollbackTrans()

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateDocLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateDocLink", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

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



    ' ***************************************************************** '
    '
    ' Name: GetRiskGroups
    '
    ' Description: Gets the risks groups from the database
    '
    ' History: 04/02/2002 AK - Created.
    '
    ' ***************************************************************** '
    Public Function GetRiskGroups(ByRef r_vRiskGroups As Object) As Integer

        Dim result As Integer = 0
        Dim vResultarray(,) As Object = Nothing
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskGroupSQL, sSQLName:=ACGetRiskGroupName, bStoredProcedure:=ACGetRiskGroupStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have any results?
            If Informations.IsArray(vResultarray) Then
                ' Yes, so assign them


                r_vRiskGroups = vResultarray
            Else
                ' No, so return Not Found
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskGroups Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskGroups", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'AK - 050302 - function to get document template for given process/riskcode
    'DC 210802 -added agent
    'CJB 300802 - renamed back to GetDocTemplate from GetDocTemp since GetDocTemp already exists
    '             also changed the associated iPMBDocProduction to call the changed name
    'Developer Guide No. 101
    Public Function GetDocTemplate(ByVal v_lSchemeID As Integer, ByVal v_lAgentCnt As Integer, ByVal v_sProcessTypeCode As String, ByRef v_vDocumentArray(,) As Object) As Integer
        Dim result As Integer = 0
        'Developer Guide No. 101
        Dim vResultarray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            ' Clear any parameters
            With m_oDatabase

                .Parameters.Clear()

                'Risk Group
                m_lReturn = .Parameters.Add(sName:="GIS_Scheme_Id", vValue:=CStr(v_lSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'DC210802 -start -added agent parameter
                'Agent
                m_lReturn = .Parameters.Add(sName:="Agent_Cnt", vValue:=CStr(v_lAgentCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'DC210802 -end

                'Process Type
                m_lReturn = .Parameters.Add(sName:="Process_Type_Code", vValue:=v_sProcessTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '        'Document Type
                '        m_lReturn& = .Parameters.Add( _
                ''              sName:="Document_Type_Code", _
                ''              vValue:=v_sDocumentTypeCode, _
                ''              idirection:=PMParamInput, _
                ''              idatatype:=PMString)
                '        If (m_lReturn& <> PMTrue) Then
                '            GetDocTemplate = PMFalse
                '            Exit Function
                '        End If

                ' Perform the SQL
                m_lReturn = .SQLSelect(sSQL:=ACDocTemplateGetSQL, sSQLName:=ACDocTemplateGetName, bStoredProcedure:=ACDocTemplateGetStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultarray)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            ' Do we have any results?
            If Informations.IsArray(vResultarray) Then
                ' Yes, so assign them
                '        v_lDocumentTemplateId = Val(vResultArray(0, 0))
                '        v_lDocumentTypeId = Val(vResultArray(1, 0))
                v_vDocumentArray = vResultarray
            Else
                ' No, so return Not Found
                '        v_lDocumentTemplateId = 0
                '        v_lDocumentTypeId = 0
                v_vDocumentArray = Nothing
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocTemplate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocTemplate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyType
    '
    ' Description: Gets the party_type from the party_cnt
    '
    ' SJP(CMG) 26-02-2003
    ' ***************************************************************** '
    Public Function GetPartyType(ByRef r_vPartytype(,) As Object, ByVal v_lPartyCnt As String) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Modifying the inline query to make it compatible with SQL server 2005
            sSQL = "SELECT pt.Code, p.resolved_name "
            sSQL = sSQL & "FROM Party p LEFT OUTER JOIN Party_Type pt ON p.party_type_id = pt.party_type_id "
            sSQL = sSQL & "WHERE p.Party_cnt = " & v_lPartyCnt

            With m_oDatabase
                .Parameters.Clear()
                m_lReturn = .SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyType", bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vPartytype)
            End With


            Return m_lReturn

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsuranceFolder
    '
    ' Description: Gets the insurance folder cnt from the supplied
    '              insurance file cnt.
    '
    ' SJP(CMG) 26-02-2003
    ' ***************************************************************** '
    Public Function GetInsuranceFolder(ByRef v_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQLSearch As String = ""
        Dim vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQLSearch = "SELECT insurance_folder_cnt FROM insurance_file WHERE insurance_file_cnt = " & v_lInsuranceFileCnt

            With m_oDatabase
                .Parameters.Clear()

                m_lReturn = .SQLSelect(sSQL:=sSQLSearch, sSQLName:="GetInsuranceFolder", bStoredProcedure:=False, vResultArray:=vResults)
            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResults) Then

                r_lInsuranceFolderCnt = CInt(vResults(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInsuranceFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsuranceFolder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function
    ' ***************************************************************** '
    ' Name: AddDocLink
    '
    ' Description: This function Add/Update the records of table pmb_doc_link
    '
    ' Task: Renewal Printing
    '
    ' Author: Pankaj dt.03.03.2008
    'developer guide no. 101
    Public Function AddDocLink(ByRef r_lDocLinkId As Object, ByVal v_lGisSchemeId As Object, ByVal v_lProcessID As Object, ByVal v_lDocumentTypeID As Object, ByVal v_lDocumentTemplateID As Object, ByVal v_iSpoolDocument As Object, ByVal v_lProcessTypesDocsId As Object, ByVal v_iFunctionalArea As Object, ByVal v_lProductID As Object, ByVal v_lSourceID As Object, ByVal v_iIsClient As Object, ByVal v_iIsAgent As Object, ByVal v_iIsOffice As Object, ByVal v_iProductionOrder As Object, ByVal v_iIsBO As Object, ByVal v_iIsSAM As Object, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddDocLink"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            BeginTrans()
            Dim lSourceID As Object
            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            'Doc_link_ID add incase of > 0

            If r_lDocLinkId > 0 Then
                'developer guide no. 89
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PMB_Doc_Link_Id", vValue:=r_lDocLinkId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="PMB_Doc_Link_Id", vValue:=r_lDocLinkId, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="GIS_Scheme_Id", vValue:=v_lGisSchemeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Process_Type_Id", vValue:=v_lProcessID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Type_Id", vValue:=v_lDocumentTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Template_Id", vValue:=v_lDocumentTemplateID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="spool_document", vValue:=v_iSpoolDocument, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="process_types_docs_id", vValue:=v_lProcessTypesDocsId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="functional_area", vValue:=v_iFunctionalArea, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If v_lSourceID = 0 Then

                lSourceID = DBNull.Value
            Else
                lSourceID = v_lSourceID
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=lSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_client", vValue:=v_iIsClient, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_agent", vValue:=v_iIsAgent, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_office", vValue:=v_iIsOffice, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="production_order", vValue:=v_iProductionOrder, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="generate_through_BO", vValue:=v_iIsBO, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="generate_through_SAM", vValue:=v_iIsSAM, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If r_lDocLinkId = 0 Then
                If v_lSourceID >= 0 Then
                    'Case Add When Doc Link Id is equal to zero
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACAddDocLinkSQL, sSQLName:=ACAddDocLinkName, bStoredProcedure:=ACAddDocLinkStored)
                End If
            Else
                'Case Update When Doc Link Id is not equal to zero
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateDocLinkSQL, sSQLName:=ACUpdateDocLinkName, bStoredProcedure:=ACUpdateDocLinkStored)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            CommitTrans()

            If r_lDocLinkId = 0 Then
                r_lDocLinkId = m_oDatabase.Parameters.Item("PMB_Doc_Link_Id").Value
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()
        Finally

            '        Return result

            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: DeleteDocLink
    '
    ' Description: This function deletes record from table pmb_doc_link
    '
    ' Task: Renewal Printing
    '
    ' Author: Pankaj dt.03.03.2008

    Public Function DeleteDocLink(ByVal v_lDocLinkId As Integer, Optional ByVal v_sUniqueId As String = "", Optional ByVal v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteDocLink"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lDocLinkId = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            BeginTrans()

            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PMB_Doc_Link_Id", vValue:=CStr(v_lDocLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=v_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=v_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACDeleteDocLinkSQL, sSQLName:=ACDeleteDocLinkName, bStoredProcedure:=ACDeleteDocLinkStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            CommitTrans()


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            RollbackTrans()
        Finally

            '        Return result

            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetSFIDocLink
    '
    ' Description: This function Gets records from table pmb_doc_link
    '
    ' Task: Renewal Printing
    '
    ' Author: Pankaj dt.05.03.2008

    Public Function GetSFIDocLink(ByVal v_iFunctionalArea As Integer, ByVal v_lProductID As Integer, ByVal v_iProcessTypeID As Integer, ByVal v_lSourceID As Integer, ByVal v_lDocumentTemplateID As Integer, ByRef r_vResultarray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSFIDocLink"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="functional_area", vValue:=CStr(v_iFunctionalArea), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_Id", vValue:=CStr(v_lProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Process_Type_Id", vValue:=CStr(v_iProcessTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Document_Template_Id", vValue:=CStr(v_lDocumentTemplateID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDocLinkSQL, sSQLName:=ACGetDocLinkName, bStoredProcedure:=ACGetDocLinkStored, vResultArray:=r_vResultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetSFIDocLink
    '
    ' Description: This function Gets records from table pmb_doc_link
    '
    ' Task: Renewal Printing
    '
    ' Author: Pankaj dt.05.03.2008

    Public Function GetSFIDocumentTemplates(ByVal v_iFunctionalArea As Integer, ByVal v_lProductID As Integer, ByVal v_iProcessTypeID As Integer, ByVal v_lSourceID As Integer, ByRef r_vResultarray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSFIDocumentTemplates"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="functional_area", vValue:=v_iFunctionalArea, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_Id", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Process_Type_Id", vValue:=v_iProcessTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_lSourceID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSFIDocumnetTemplatesSQL, sSQLName:=ACGetSFIDocumnetTemplatesName, bStoredProcedure:=ACGetSFIDocumnetTemplatesStored, vResultArray:=r_vResultarray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: GetSFIDocLink
    '
    ' Description: This function Gets records from table pmb_doc_link
    '
    ' Task: Renewal Printing
    '
    ' Author: Pankaj dt.05.03.2008

    Public Function GetSFIDocumentTemplatesForProcessType(ByVal v_iFunctionalArea As Integer, ByVal v_lInsurance_File_Cnt As Integer, ByVal v_lProcessType_Docs_ID As Integer, ByVal v_lProcess_Type_Code As String, ByVal v_dtEffectiveDate As Date, ByRef r_vResultarray(,) As Object, Optional ByVal v_bCalledFromSAM As Boolean = False) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSFIDocumentTemplatesForProcessType"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="functional_area", vValue:=CStr(v_iFunctionalArea), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsurance_File_Cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="process_types_docs_id", vValue:=CStr(v_lProcessType_Docs_ID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="process_type_code", vValue:=v_lProcess_Type_Code, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            'Developer Guide No. 40
            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=v_dtEffectiveDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="called_from_SAM", vValue:=CStr(If(v_bCalledFromSAM, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetSFIDocumnetTemplatesForProcessTypeSQL, sSQLName:=ACGetSFIDocumnetTemplatesForProcessTypeName, bStoredProcedure:=ACGetSFIDocumnetTemplatesForProcessTypeStored, vResultArray:=r_vResultarray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetLookUpList(ByVal v_sTableName As String, ByRef r_vResultarray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sKeyFieldName, sDescriptionFieldName As String

        Const kMethodName As String = "GetLookUpList"

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            sKeyFieldName = v_sTableName & "_id"
            sDescriptionFieldName = "description"

            ' Clear any parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyIdFieldName", vValue:=sKeyFieldName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="DescFieldName", vValue:=sDescriptionFieldName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TableName", vValue:=v_sTableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLookUpListSQL, sSQLName:=ACGetLookUpListName, bStoredProcedure:=ACGetLookUpListStored, vResultArray:=r_vResultarray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    Public Function GetProcessTypeDocuments(ByRef r_vResultarray(,) As Object, Optional ByVal v_iFunctionalArea As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetProcessTypeDocuments"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="FunctionalArea", vValue:=CStr(v_iFunctionalArea), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetDocumentType", " Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetProcessTypeDocsSQL, sSQLName:=ACGetProcessTypeDocsName, bStoredProcedure:=ACGetProcessTypeDocsStored, vResultArray:=r_vResultarray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("GetProcessTypeDocuments", ACGetProcessTypeDocsSQL & "Failed")
                End If

            End With


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
End Class
