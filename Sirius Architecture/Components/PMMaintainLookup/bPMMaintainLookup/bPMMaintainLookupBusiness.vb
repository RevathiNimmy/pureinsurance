Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Text
Imports System.IO
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 05/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PMBLifeStyle.
    '
    ' Edit History:
    ' DAK011299 - Store procedure spu_PM_Get_Columns changed to set offset
    '             value.
    ' DAK220200 - Add more SQL Server 7.0 data types
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 18/09/2003
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

    ' Database Class (Private)
    Private m_oArcDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Close Database Flag (Private)
    Private m_bCloseArcDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date
    'Start(Sriram P)CacheBug
    Dim m_sUnderwritingOrAgency As String = ""
    'End(Sriram P)CacheBug
    Private lPMAuthorityLevel As Integer

    'SD 08/08/2002 varchar field array added
    Private m_vVarcharFields(,) As Object
    'SD 09/008/2002
    Public m_bStringTruncated As Boolean

    ' SD 21/08/2002 We should only need to query varchar fields for a table once.
    Private m_bVarcharFieldsReturned As Boolean
    Private sRulePath As String = ""
    Private sGISDataModelCode As String = ""

    Public ReadOnly Property FieldsTruncated() As Boolean
        Get
            Return m_bStringTruncated
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public Property GISDataModelCode() As String
        Get
            Return sGISDataModelCode
        End Get
        Set(ByVal Value As String)
            sGISDataModelCode = Value
        End Set
    End Property

    Public Property RulePath() As String
        Get
            If sRulePath = "" Then
                m_lReturn = CType(GetRulePath(sRulePath:=sRulePath), gPMConstants.PMEReturnCode)
            End If
            Return sRulePath
        End Get
        Set(ByVal Value As String)
            sRulePath = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oCServices As sPMServerCS.PMServerBusinessCS
        'Start(Sriram P)CacheBug
        Dim result As Integer = 0
        Dim sUnderwritingOrAgency As String = ""
        'End(Sriram P)CacheBug

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


            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' New instance of server component services
            '    Set oCServices = New sPMServerCS.PMServerBusinessCS

            ' Get a database instance
            'Set m_oDatabase = New dPMDAO.Database

            ' Get an instance to the broking database
            '    m_lReturn& = oCServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            ''        v_lPMProductFamily:=PMProductFamily, _
            ''        r_bNewInstanceCreated:=m_bCloseDatabase, _
            ''        r_oCheckedDatabase:=m_oDatabase, _
            ''        v_vDatabase:=vDatabase)
            '    If (m_lReturn& <> PMTrue) Then
            '        Initialise = PMFalse
            '        Exit Function
            '    End If

            ' Get an instance to the architecture database
            '    m_lReturn& = oCServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=pmePFSiriusArchitecture, _
            'r_oDatabase:=m_oArcDatabase)
            m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oArcDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Start(Sriram P)CacheBug

            m_lReturn = bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=sUnderwritingOrAgency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set it to the Module level variable
            m_sUnderwritingOrAgency = sUnderwritingOrAgency.Trim().ToUpper()
            'End(Sriram P)CacheBug

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCaptionID
    '
    ' Description: Calls the spu_pm_caption_id_return stored procedure
    '              to either get or create a caption_id
    '
    ' ***************************************************************** '
    Public Function GetCaptionID(ByVal v_sCaption As String, ByRef r_lCaptionID As Integer) As Integer

        Dim result As Integer = 0
        Try

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

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptionID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptionID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: ConnectDatabase
    '
    ' Description: This is called manually after Initialise, because
    '              we need to open any of the pm databases
    '
    ' ***************************************************************** '
    Public Function ConnectDatabase(ByVal v_lProductFamily As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' RDC 13062002 CompServ replaced with BAS module
            'Dim oComponentServices As PMServerBusinessCS

            ' Use component services to open an instance of the database

            '    Set oComponentServices = New PMServerBusinessCS

            ' Close the database if we already have a connection open
            If m_bCloseDatabase Then

                m_lReturn = m_oDatabase.CloseDatabase()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDatabase = Nothing

            End If

            ' Get a new connection to the database from component services
            '    m_lReturn& = oComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=v_lProductFamily, _
            'r_oDatabase:=m_oDatabase)
            m_lReturn = gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=v_lProductFamily, r_oDatabase:=m_oDatabase)

            '    Set oComponentServices = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_bCloseDatabase = True

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConnectDatabase Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConnectDatabase", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description:
    '
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

    ' ******************************************************************** '
    ' Name: GetColumns
    '
    ' Description: Gets the column names and details for the passed table
    '
    ' ******************************************************************** '
    Public Function GetColumns(ByVal v_sTableName As String, ByVal v_sProductCode As String, ByRef r_vColumns(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oArcDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="table_name", vValue:=v_sTableName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the product code
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="product_code", vValue:=v_sProductCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oArcDatabase.SQLSelect(sSQL:=ACSQLGetColumns, sSQLName:=ACSQLGetColumnsName, bStoredProcedure:=ACSQLGetColumnsStored, vResultArray:=r_vColumns)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check that some data was returned
            If Not Information.IsArray(r_vColumns) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'SD 21/08/2002 START - This change introduced as some lookup details are also
            ' lookups, and will repeat the method call. We must only run this call the first time
            If m_bVarcharFieldsReturned <> TriState.True Then

                'SD 08/08/2002 Get length for varchar columns and place it in modular object
                ' Clear the parameters
                m_oArcDatabase.Parameters.Clear()

                ' Add the table name parameter
                m_lReturn = m_oArcDatabase.Parameters.Add(sName:="table_name", vValue:=v_sTableName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oArcDatabase.SQLSelect(sSQL:=ACSQLGetVarcharFields, sSQLName:=ACSQLGetVarcharFieldsName, bStoredProcedure:=ACSQLGetVarcharFieldsStored, vResultArray:=m_vVarcharFields)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_bVarcharFieldsReturned = TriState.True

            End If

            'reset the variables
            m_bStringTruncated = TriState.False

            'SD 21/08/2002 END

            ' CF 310899 - Begin

            ' Update the Offset field so that it has a NULL/NON-NULL value
            ' instead of some meaning-less number (who's idea was this anyway? :)

            ' The following is because "offset" does not correctly report
            ' a nullable field, as first thought.
            ' Rather than chage loads of code, the offset value is changed
            ' to +/- depending on the "NULLABLE" field returned by spu_columns
            ' stored procedure

            'DAK011299 - This is all done in the Stored procedure now
            ' Exec the SQL
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''                    sSQL:=ACSQLSPColumns & v_sTableName$, _
            ''                    sSQLName:=ACSQLSPColumnsName, _
            ''                    bStoredProcedure:=ACSQLSPColumnsStored, _
            ''                    vResultArray:=vResultArray)
            '    If (m_lReturn& <> PMTrue) Then
            '        GetColumns = PMFalse
            '        Exit Function
            '    End If

            ' Merge the results back in
            '    For iLoop1% = LBound(vResultArray, 2) To UBound(vResultArray, 2)
            '        For iLoop2% = LBound(r_vColumns, 2) To UBound(r_vColumns, 2)
            '            If (vResultArray(3, iLoop1%) = r_vColumns(ACColumnName, iLoop2%)) Then
            '                If (vResultArray(10, iLoop1%) = 0) Then
            '                    r_vColumns(ACColumnOffset, iLoop2) = 1
            '                Else
            '                    r_vColumns(ACColumnOffset, iLoop2) = -1
            '                End If
            '                Exit For
            '            End If
            '        Next iLoop2%
            '    Next iLoop1%

            ' CF 310899 - End

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetKeyColumns
    '
    ' Description: Gets the column's that are key columns
    '
    ' ***************************************************************** '
    Public Function GetKeyColumns(ByVal v_sTableName As String, ByVal v_sProductCode As String, ByRef r_vKeyColumns(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oArcDatabase.Parameters.Clear()

            ' Add the table name parameter
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="table_name", vValue:=v_sTableName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the product code parameter
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="product_code", vValue:=v_sProductCode, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oArcDatabase.SQLSelect(sSQL:=ACSQLGetKeyColumns, sSQLName:=ACSQLGetKeyColumnsName, bStoredProcedure:=ACSQLGetKeyColumnsStored, vResultArray:=r_vKeyColumns)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetKeyColumns Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetKeyColumns", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Gets all the data for the passed table/columns
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByVal v_sTableName As String, ByVal v_sColumns As String, ByRef r_vData(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Construct the SQL
            sSQL = "SELECT " & v_sColumns & " FROM " & v_sTableName

            ' Exception for GIS_Data_Model which is filtered if the product option is set
            If v_sTableName.ToUpper() = "GIS_DATA_MODEL" Then
                sSQL = sSQL & " LEFT JOIN hidden_options ON hidden_options.option_number = " &
                       "GIS_Data_Model.product_option WHERE (ISNULL(hidden_options.value,'0')='1' OR " &
                       "GIS_Data_Model.product_option IS NULL)"
            End If

            If UCase(v_sTableName) Like "UDL_*" Then
                sSQL = sSQL & " Where UDL_version = (select max(udl_version) from " & v_sTableName & ")"
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetDetails", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check data returned
            If Not Information.IsArray(r_vData) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ''' <summary>
    ''' Add Default Bank Name
    ''' </summary>
    ''' <param name="v_sTableName"> table Name</param>
    ''' <param name="v_vColumns">Column Name</param>
    ''' <param name="r_vValues">Value</param>
    ''' <returns>Result</returns>
    ''' <remarks></remarks>
    Public Function Add(ByVal v_sTableName As String, ByVal v_vColumns(,) As Object, ByRef r_vValues() As Object, Optional ByVal v_sUniqueId As String = "") As Integer

        Dim nResult As Integer = 0
        Dim sSQL As String = ""
        Dim sValues As New StringBuilder
        Dim sColumns As New StringBuilder
        Dim oResultArray(,) As Object
        Dim oResultIdentity As Object
        Dim nLbound As Integer

        Try

            nResult = PMEReturnCode.PMTrue
            ' Get the next ID
            ' RDC 18112002 added ISNULL function

            sSQL = "SELECT ISNULL(MAX(" & CStr(v_vColumns(1, 0)) & ") + 1, 1) FROM " & v_sTableName

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectMax", bStoredProcedure:=False, vResultArray:=oResultArray)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed select max id from " & CStr(v_vColumns(1, 0)), vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(oResultArray) Then
                Return PMEReturnCode.PMNotFound
            End If

            ' PN37982-Get identity column property value from the table
            sSQL = "SELECT IDENT_CURRENT ('" & v_sTableName & "')"

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="SelectIdentity", bStoredProcedure:=False, vResultArray:=oResultIdentity)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed Get Identity Column", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return PMEReturnCode.PMFalse
            End If

            If Not Information.IsArray(oResultIdentity) Then
                Return PMEReturnCode.PMNotFound
            End If

            ' RDC 18112002 added ISNULL function to 'next next key' query
            ' Set the ID


            r_vValues(0) = CInt(oResultArray(0, 0))

            sValues = New StringBuilder("")
            sColumns = New StringBuilder("")

            ' PN37982- Check for Identity property in the table.
            ' If found then start loop by +1
            If gPMFunctions.ToSafeInteger(oResultIdentity(0, 0)) > 0 Then
                nLbound = v_vColumns.GetLowerBound(1) + 1
            Else
                nLbound = v_vColumns.GetLowerBound(1)
            End If

            ' Construct the list of columns
            For nLoop1 As Integer = nLbound To v_vColumns.GetUpperBound(1) - 1

                If LCase(v_vColumns(1, nLoop1)) = "pie_guid" OrElse LCase(v_vColumns(1, nLoop1) = "pie_last_updated") Then
                    Continue For
                End If

                If sColumns.Length = 0 Then


                    sColumns.Append(CStr(v_vColumns(1, nLoop1)))
                    ' Add question marks for the values and let dPMDAO sort them out
                    sValues.Append("{" & CStr(v_vColumns(1, nLoop1)) & "}")
                Else
                    sColumns.Append(", ")
                    sValues.Append(", ")
                    sColumns.Append(CStr(v_vColumns(1, nLoop1)))
                    ' Add question marks for the values and let dPMDAO sort them out
                    sValues.Append("{" & CStr(v_vColumns(1, nLoop1)) & "}")
                End If

            Next nLoop1

            m_lReturn = AddAuditTrailForLookups(v_sTableName, r_vValues, v_vColumns, v_sUniqueId, True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If gPMFunctions.ToSafeInteger(oResultIdentity(0, 0)) = 0 AndAlso (v_sTableName.ToLower() = "document_template_group" OrElse v_sTableName.ToLower() = "document_template_sub_group") Then
                sSQL = "SET IDENTITY_INSERT " & v_sTableName & " ON"
                sSQL = sSQL + vbCrLf
                sSQL = sSQL + "INSERT INTO " & v_sTableName & " (" & sColumns.ToString() & ") " &
                   "VALUES (" & sValues.ToString() & ")"
                sSQL = sSQL + vbCrLf
                sSQL = sSQL + "SET IDENTITY_INSERT " & v_sTableName & " OFF"
            Else

                ' Construct the SQL
                sSQL = "INSERT INTO " & v_sTableName & " (" & sColumns.ToString() & ") " &
                   "VALUES (" & sValues.ToString() & ")"
            End If
            m_lReturn = AddParameters(r_vValues, v_vColumns)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Add", bStoredProcedure:=False)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to add record to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return PMEReturnCode.PMFalse
            End If
            If LCase(v_sTableName) Like "udl_*" Then
                m_oDatabase.Parameters.Clear()
                m_lReturn = m_oDatabase.Parameters.Add("table", v_sTableName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_UDL_Version_upd", sSQLName:="spu_UDL_Version_upd", bStoredProcedure:=True)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to add record to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return PMEReturnCode.PMFalse
                End If

            End If


            Return nResult

        Catch excep As System.Exception

            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Update Default Bank Name
    ''' </summary>
    ''' <param name="v_sTableName">Table Name</param>
    ''' <param name="v_vColumns">Column Name</param>
    ''' <param name="v_vValues">Value</param>
    ''' <returns>Result</returns>
    ''' <remarks></remarks>
    Public Function Update(ByVal v_sTableName As String, ByVal v_vColumns(,) As Object, ByVal v_vValues() As Object, Optional ByVal v_sUniqueId As String = "") As Integer

        Dim nResult As Integer = 0
        Dim sValues As String = ""
        Dim sSQL1 As New StringBuilder
        Dim sSQL2 As New StringBuilder
        Dim sColumns As String = ""
        Dim nCaptionID As Integer
        Dim sCaptionDesc As String = ""

        Try

            nResult = PMEReturnCode.PMTrue

            sValues = ""
            sColumns = ""

            nCaptionID = 0
            sCaptionDesc = ""

            m_lReturn = AddAuditTrailForLookups(v_sTableName, v_vValues, v_vColumns, v_sUniqueId, False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' We have the +1's because we dont update the identity column
            sSQL2 = New StringBuilder("UPDATE " & v_sTableName & " SET ")
            sSQL1 = New StringBuilder("")
            ' Construct the list of columns
            For nLoop1 As Integer = v_vColumns.GetLowerBound(1) + 1 To v_vColumns.GetUpperBound(1) - 1


                If CStr(v_vColumns(1, nLoop1)).ToLower() = "caption_id" Then
                    nCaptionID = CInt(v_vValues(nLoop1))
                End If


                If CStr(v_vColumns(1, nLoop1)).ToLower() = "description" Then
                    sCaptionDesc = CStr(v_vValues(nLoop1))
                End If

                If LCase(v_vColumns(1, nLoop1)) = "pie_guid" OrElse LCase(v_vColumns(1, nLoop1) = "pie_last_updated") Then
                    Continue For
                End If

                If CStr(v_vColumns(1, nLoop1)) = "0" Then
                    If sSQL1.ToString() = "" Then
                        sSQL1.Append(CStr(v_vColumns(1, nLoop1)) & " = {" & CStr(v_vColumns(1, nLoop1)) & "}")
                    Else
                        sSQL1.Append(", ")
                        sSQL1.Append("[" & CStr(v_vColumns(1, nLoop1)) & "] = {" & CStr(v_vColumns(1, nLoop1)) & "}")
                    End If
                Else
                    If sSQL1.ToString() = "" Then
                        sSQL1.Append(CStr(v_vColumns(1, nLoop1)) & " = {" & CStr(v_vColumns(1, nLoop1)) & "}")
                    Else
                        sSQL1.Append(", ")
                        sSQL1.Append("[" & CStr(v_vColumns(1, nLoop1)) & "] = {" & CStr(v_vColumns(1, nLoop1)) & "}")
                    End If
                End If

            Next nLoop1

            ' Add the where clause


            sSQL1.Append(" WHERE " & CStr(v_vColumns(1, 0)) & " = {" & CStr(v_vColumns(1, 0)) & "}")


            m_lReturn = AddParameters(v_vValues, v_vColumns)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            sSQL2.Append(sSQL1)
            ' Perform the SQL
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL2.ToString(), sSQLName:="Update", bStoredProcedure:=False)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update " & v_sTableName & " record", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return PMEReturnCode.PMFalse
            End If

            'PSL 28/04/2003 Issue3343
            'Apostrophes must be dealt with in the caption insert as well
            m_lReturn = bPMFunc.ValidateSQL(sCaptionDesc)

            ' RDC 18112002 update caption
            If nCaptionID > 0 And sCaptionDesc <> "" Then
                sSQL1 = New StringBuilder("UPDATE pmcaption SET caption = '" & sCaptionDesc & "' WHERE caption_id = " & CStr(nCaptionID))

                m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL1.ToString(), sSQLName:="UpdateLookupCaption", bStoredProcedure:=False)

                If m_lReturn <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to update pmcaption record", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return PMEReturnCode.PMFalse
                End If

            End If

            If LCase(v_sTableName) Like "udl_*" Then
                'Update the Version
                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add("table", v_sTableName, PMEParameterDirection.PMParamInput, PMEDataType.PMString)

                m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_UDL_Version_upd", sSQLName:="spu_UDL_Version_upd", bStoredProcedure:=True)
                If m_lReturn <> PMEReturnCode.PMTrue Then
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed to add record to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return PMEReturnCode.PMFalse
                End If

            End If


            Return nResult

        Catch excep As System.Exception
            nResult = PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUserAuthority
    '
    ' Description: Gets the User Groups that the current user is a
    '              Supervisor of.
    '
    ' ***************************************************************** '
    Public Function GetUserAuthority(ByRef r_bIsAdministrator As Boolean, ByRef r_vSupervisedGroups As Object) As Integer

        Dim result As Integer = 0
        Dim oUserGroup As bPMUserGroup.Utilities
        ' RDC 13062002 CompServ repalced with BAS module
        'Dim oCS As sPMServerCS.PMServerBusinessCS

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the User Group Component to do this work.
            '    Set oCS = New sPMServerCS.PMServerBusinessCS
            '    m_lReturn = oCS.CreateBusinessObject( _
            'r_oObject:=oUserGroup, _
            'v_sClassName:="bPMUserGroup.Utilities", _
            'v_sCallingAppName:=m_sCallingAppName, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel, _
            'v_oDatabase:=m_oDatabase)

            oUserGroup = New bPMUserGroup.Utilities
            m_lReturn = oUserGroup.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            '    Set oCS = Nothing
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Is the User an Administrator

            m_lReturn = oUserGroup.IsUserAdministrator(v_iUserID:=m_iUserID, r_bIsAdministrator:=r_bIsAdministrator)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Get the Groups they Supervise

            m_lReturn = oUserGroup.GetGroupsSupervisedByUser(v_iUserID:=m_iUserID, r_vSupervisedGroups:=r_vSupervisedGroups)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn

                oUserGroup.Dispose()
                oUserGroup = Nothing
                Return result
            End If

            ' Terminate

            oUserGroup.Dispose()
            oUserGroup = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetUserAuthorityFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUserAuthority", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AddParameters
    '
    ' Description:
    '
    ' History: 28/02/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Private Function AddParameters(ByVal v_vValues() As Object, ByVal v_vColumns(,) As Object) As Integer
        Dim result As Integer = 0
        Dim vValue As Object
        Dim iDataType As gPMConstants.PMEDataType
        'DAK080500
        Dim sValue As String = ""
        'SD 08/08/2002
        Dim sVarcharField As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        For iLoop1 As Integer = v_vValues.GetLowerBound(0) To v_vValues.GetUpperBound(0)

            vValue = DBNull.Value

            If CStr(v_vValues(iLoop1)).Trim() <> "" Then

                Select Case v_vColumns(0, iLoop1)
                    ' String
                    'DAK220200 - add text, nchar, ntext and nvarchar
                    'SD 08/08/2002  - remove varchar and nvarchar
                    Case "char", "text", "nchar", "ntext"
                        iDataType = gPMConstants.PMEDataType.PMString
                        'DAK080500 - check for apostrophe in text fields

                        sValue = CStr(v_vValues(iLoop1)).Trim()
                        m_lReturn = bPMFunc.ValidateSQL(sValue)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        vValue = sValue

                        'SD 08/08/2002  - Check if the field needs truncation for varchar and nvarchar only
                    Case "varchar", "nvarchar"
                        iDataType = gPMConstants.PMEDataType.PMString

                        sValue = CStr(v_vValues(iLoop1)).Trim()

                        sVarcharField = CStr(v_vColumns(1, iLoop1)).Trim()
                        m_lReturn = TruncateString(sValue, sVarcharField)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        vValue = sValue

                        ' Date
                        'DAK220200 - Add smalldatetime
                    Case "datetime", "smalldatetime"
                        iDataType = gPMConstants.PMEDataType.PMDate

                        vValue = CDate(v_vValues(iLoop1))

                        ' Tiny Int
                        'DAK220200 - Add bit
                    Case "tinyint"
                        iDataType = gPMConstants.PMEDataType.PMInteger

                        vValue = CInt(v_vValues(iLoop1))
                    Case "bit"
                        iDataType = gPMConstants.PMEDataType.PMInteger

                        If CStr(v_vValues(iLoop1)).ToLower() = "true" Or CStr(v_vValues(iLoop1)).ToLower() = "false" Then

                            vValue = IIf(CStr(v_vValues(iLoop1)) = "True", 1, 0)
                        Else

                            vValue = CInt(v_vValues(iLoop1))
                        End If

                    Case "int"
                        iDataType = gPMConstants.PMEDataType.PMLong

                        vValue = CInt(v_vValues(iLoop1))

                    Case "smallint"
                        iDataType = gPMConstants.PMEDataType.PMInteger

                        vValue = CInt(v_vValues(iLoop1))

                        'DAK220200 - add numeric
                    Case "decimal", "numeric"
                        iDataType = gPMConstants.PMEDataType.PMDecimal

                        vValue = CDec(v_vValues(iLoop1))

                        'DAK220200 - add float and real
                    Case "float", "real"
                        iDataType = gPMConstants.PMEDataType.PMDouble

                        vValue = CDbl(v_vValues(iLoop1))

                        'DAK220200 - add money, smallmoney
                    Case "money", "smallmoney"
                        iDataType = gPMConstants.PMEDataType.PMCurrency

                        vValue = CDec(v_vValues(iLoop1))

                        'DAK220200 - add binary, image, timestamp, uniqueidentifier, varbinary
                    Case "binary", "image", "timestamp", "uniqueidentifier", "varbinary"
                        iDataType = gPMConstants.PMEDataType.PMBinary

                        vValue = v_vValues(iLoop1)

                    Case Else
                        iDataType = gPMConstants.PMEDataType.PMLong

                        vValue = CInt(v_vValues(iLoop1))

                End Select

            End If

            ' Add the database parameter
            'If vValue Is DBNull.Value Then
            '    vValue = ""
            'End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:=ToSafeString(v_vColumns(1, iLoop1)), vValue:=(vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=iDataType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter. Value : " & CStr(vValue), vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Next iLoop1

        Return result

    End Function

    Public Function AddAuditTrailForLookups(ByVal v_sTableName As String, ByVal v_vValues() As Object, ByVal v_vColumns(,) As Object, ByVal v_sUniqueId As String, ByVal bAddlookupData As Boolean) As Integer


        Dim result As Integer = 0

        Const kMethodName As String = "AddAuditTrailForLookups"
        Dim vValue As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim v_dtLookupData As New DataTable("Lookup")

            ' Define columns
            v_dtLookupData.Columns.Add("Column_name", GetType(String))
            v_dtLookupData.Columns.Add("Column_DisplayName", GetType(String))
            v_dtLookupData.Columns.Add("Column_Value", GetType(String))

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            For iLoop1 As Integer = v_vValues.GetLowerBound(0) To v_vValues.GetUpperBound(0)
                vValue = DBNull.Value

                If CStr(v_vValues(iLoop1)).Trim() <> "" Then
                    Select Case v_vColumns(0, iLoop1)
                        Case "tinyint"
                            vValue = CInt(v_vValues(iLoop1)).ToString
                        Case "datetime", "smalldatetime"
                            vValue = CDate(v_vValues(iLoop1)).ToString("MMM d yyyy h:mmtt", System.Globalization.CultureInfo.InvariantCulture)
                        Case "bit"
                            If CStr(v_vValues(iLoop1)).ToLower() = "true" Or CStr(v_vValues(iLoop1)).ToLower() = "false" Then

                                vValue = IIf(CStr(v_vValues(iLoop1)) = "True", 1, 0)
                            Else

                                vValue = CInt(v_vValues(iLoop1))
                            End If
                        Case Else
                            vValue = v_vValues(iLoop1).ToString
                    End Select
                End If
                If v_vColumns(1, iLoop1).ToString.Trim.Length > 1 Then
                    v_dtLookupData.Rows.Add(v_vColumns(1, iLoop1).ToString, ReformatText(v_vColumns(1, iLoop1).ToString), vValue)
                End If
            Next iLoop1
            Dim sFieldDescription As String = ReformatText(v_sTableName) + " (" + v_vValues(2).ToString + ")"

            m_oDatabase.Parameters.Clear()
            Using cmd As New SqlCommand("spu_create_audit_trail_for_lookups")
                cmd.Parameters.AddWithValue("@TableName", v_sTableName)
                cmd.Parameters.AddWithValue("@TableData", v_dtLookupData)
                cmd.Parameters.AddWithValue("@KeyFieldDescription", sFieldDescription)
                cmd.Parameters.AddWithValue("@UniqueId", v_sUniqueId)
                cmd.Parameters.AddWithValue("@UserId", m_iUserID)
                cmd.Parameters.AddWithValue("@bAddlookupData", bAddlookupData)
                cmd.CommandType = CommandType.StoredProcedure

                'Execute SQL Statement
                m_lReturn = m_oDatabase.ExecuteNonQuery(cmd)

            End Using

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMTrue
            Return result

        End Try
        Return result
    End Function

    'SD 09/08/2002 Extra processing for varchar type strings
    ' ***************************************************************** '
    ' Name: TruncateString
    '
    ' Description: Truncates the first parameter input string if it is in
    ' a modular array of varchars based on the length of that varchar
    ' (name of field type is the second parameter). Also validates sql
    '
    ' ***************************************************************** '
    '
    Private Function TruncateString(ByRef r_sString As String, ByRef r_sFieldName As String) As Integer

        Dim result As Integer = 0
        Dim sFieldName As String
        Dim iFieldLength As Integer



        iFieldLength = 255
        'If varchars or nvarchars are present, then the vVarcharFields array
        'can not be empty
        If Information.IsArray(m_vVarcharFields) Then
            result = gPMConstants.PMEReturnCode.PMTrue
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'loop thru m_vVarcharFields array
        For iLoop As Integer = m_vVarcharFields.GetLowerBound(1) To m_vVarcharFields.GetUpperBound(1)

            'when the field name matches the input parameter, get field length
            sFieldName = CStr(m_vVarcharFields(0, iLoop)).Trim()
            If String.Compare(sFieldName, r_sFieldName) = 0 Then
                iFieldLength = (CInt(m_vVarcharFields(1, iLoop)))
                Exit For
            End If

        Next iLoop

        'SD 12/08/2002 The following code is redundant because the ADO connection
        ' automatically validates inserts to tables where the values include the "'"
        ' character. Note that this code can be resurrected for non ADO connection
        ' but it has the following characteristic: If the last character within the
        ' truncated length of an insert string is a single char, it is deleted.

        '    'Validate the SQL expression
        '    m_lReturn = ValidateSQL(r_sString)
        '    If m_lReturn <> PMTrue Then
        '        TruncateString = PMFalse
        '        Exit Function
        '    End If
        '
        '    'Truncate the string to the specified length
        '    If Len(r_sString) > iFieldLength Then
        '        r_sString = Left(r_sString, iFieldLength)
        '        m_bStringTruncated = vbTrue
        '
        '        'check if last character is a single "'" field
        '        If StrComp(Right(r_sString, 1), sSingleQuote) = 0 Then
        '            'if number of single quotes adjacent is an odd number,
        '            'truncate an extra character
        '            For iLoop = Len(r_sString) To 1 Step -1
        '                If StrComp((Mid(r_sString, iLoop, 1)), sSingleQuote) = 0 Then
        '                    iCount = iCount + 1
        '                Else
        '                    Exit For
        '                End If
        '            Next
        '            If iCount Mod 2 <> 0 Then
        '                r_sString = Left(r_sString, iFieldLength - 1)
        '            End If
        '        End If
        '
        '    End If

        'Truncate the string to the specified length
        If r_sString.Length > iFieldLength Then
            r_sString = r_sString.Substring(0, iFieldLength)
            m_bStringTruncated = TriState.True
        End If

        'Validate the SQL expression
        m_lReturn = bPMFunc.ValidateSQL(r_sString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: GetViewForColumn
    '
    ' Description: Returns the name a a View if the passed in column is
    '              of the format <Existing view>_id.
    '
    ' History: 22/01/2003 CJR - Created.
    '
    ' ***************************************************************** '
    Public Function GetViewForColumn(ByRef v_sColName As String, ByRef r_sView As String) As Integer

        Dim result As Integer = 0
        Try

            Dim vArray(,) As Object
            Dim sViewName As String = ""

            'Strip _ID off
            sViewName = v_sColName.Substring(0, v_sColName.Length - 3)
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oArcDatabase.Parameters.Clear()

            ' Add the parameters
            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="ViewName", vValue:=sViewName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Perform the stored procedure
            m_lReturn = m_oArcDatabase.SQLSelect(sSQL:=ACSQLGetViews, sSQLName:=ACSQLGetViewsName, bStoredProcedure:=ACSQLGetViewsStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                r_sView = CStr(vArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetViewForColumn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetViewForColumn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CheckLinkedDataMandatory
    '
    ' Description: Calls the spu_Check_Lookup_Linked_Data_Mandatory stored
    '              procedure to get the value of LinkedDataMandatory flag.
    '
    ' ***************************************************************** '
    Public Function CheckLinkedDataMandatory(ByVal v_sTableName As String) As Integer
        Dim result As Integer = 0
        Dim bLinkedDataMandatory As Boolean
        Try

            ' m_iLanguageID

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters
            m_oArcDatabase.Parameters.Clear()

            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="lookup_table_name", vValue:=v_sTableName, idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oArcDatabase.Parameters.Add(sName:="linked_data_mandatory", vValue:=CStr(bLinkedDataMandatory), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the stored procedure
            m_lReturn = m_oArcDatabase.SQLAction(sSQL:=ACSQLCheckLinkedDataMandatory, sSQLName:=ACSQLCheckLinkedDataMandatoryName, bStoredProcedure:=ACSQLCheckLinkedDataMandatoryStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the returned caption_id
            bLinkedDataMandatory = m_oArcDatabase.Parameters.Item("linked_data_mandatory").Value



            If Not bLinkedDataMandatory Then
                Return gPMConstants.PMEReturnCode.PMFalse
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckLinkedDataMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckLinkedDataMandatory", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    Public Function UpdateProductEvents(ByVal v_sTableName As String, ByVal v_lEventId As Integer) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_oDatabase.Parameters.Clear()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Perform the SQL

            Select Case v_sTableName.ToLower()
                Case "mta_event_description"

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="mta_event_description_id", vValue:=CStr(v_lEventId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSQLProductMTAEventAdd, sSQLName:=ACSQLProductMTAEventAddName, bStoredProcedure:=ACSQLProductMTAEventAddStored)


                Case "claim_event_description"

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_event_description_id", vValue:=CStr(v_lEventId), idirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSQLProductClaimEventAdd, sSQLName:=ACSQLProductClaimEventAddName, bStoredProcedure:=ACSQLProductClaimEventAddStored)

            End Select

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateProductEvents record to database.", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProductEvents", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateProductEvents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateProductEvents", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    Public Function UpdateCache(ByVal v_sTableName As String) As Integer
        Dim result As Integer = 0
        Dim sKey As String = ""
        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' eg. KEY_LOOKUP_00026_<sTable> :  means : Language ID 26 <sTable> means supplied Table Name
            sKey = "KEY_LOOKUP_" & StringsHelper.Format(m_iLanguageID, "00000") & "_" & v_sTableName.ToUpper() & ".xml"

            ClearCache(sKey)
            Return result
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ClearCache", r_lFunctionReturn:=result, excep:=ex)
        End Try
    End Function


    Public Function ClearCache(Optional ByVal v_sKey As String = Nothing) As Integer

        Dim result As Integer = 0
        Dim sKey As String, sCachePath As String
        Dim sFileNames() As String
        Dim i As Integer, iReturm As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            iReturm = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=sCachePath)

            If iReturm <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Right(sCachePath, 1) <> "\" Then
                sCachePath += "\"
            End If

            sFileNames = Directory.GetFiles(sCachePath)
            ' if nothing is passed, clear all the cache
            If Information.IsNothing(v_sKey) Then
                ' No Key is suppled, so delete all
                'we need a threadlock on this
                'oCache.Clear()
                For i = 0 To sFileNames.GetUpperBound(0)
                    File.Delete(sFileNames(i))
                Next
            Else
                sKey = CStr(v_sKey)
                File.Delete(sCachePath & sKey)
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ClearCache", r_lFunctionReturn:=result, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' This method wil import the datamodel with dynamic script generated from other database
    ''' to copy that datamodel 
    ''' </summary>
    ''' <param name="sFileName"></param>
    ''' <param name="o_sDataModelCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ImportMarketPlaceDataModel(ByVal sFileName As String, ByRef o_sDataModelCode As String) As Integer

        Const kMethodName As String = "ImportMarketPlaceDataModel"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try
            nReturn = gPMConstants.PMEReturnCode.PMTrue

            Dim srFileReader As StreamReader
            srFileReader = New StreamReader(sFileName)

            'read the first line for data model code 
            Dim sFirstLine As String = String.Empty
            If srFileReader.Peek() >= 0 Then
                sFirstLine = srFileReader.ReadLine
            End If
            'Read from File to create dynamic sql
            Dim sSql As StringBuilder = New StringBuilder("")
            While srFileReader.Peek() >= 0
                sSql.AppendLine(srFileReader.ReadLine.ToString)
            End While
            srFileReader.Close()

            m_oDatabase.Parameters.Clear()

            m_oDatabase.SQLBeginTrans()
            ' Perform the SQL operation         
            nReturn = m_oDatabase.SQLAction(sSQL:=sSql.ToString, sSQLName:="Import Market Place Data Model", bStoredProcedure:=False)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLRollbackTrans()
                Return nReturn
            ElseIf nReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_oDatabase.SQLCommitTrans()

                If Not String.IsNullOrEmpty(sFirstLine) Then

                    o_sDataModelCode = Mid(sFirstLine, 3, Len(sFirstLine))

                    m_oDatabase.Parameters.Clear()

                    nReturn = m_oDatabase.Parameters.Add(sName:="sDataModelCode", vValue:=o_sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="")
                        Return nReturn
                    End If

                    nReturn = m_oDatabase.Parameters.Add(sName:="bIsMPDataModel", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="")
                        Return nReturn
                    End If

                    nReturn = m_oDatabase.Parameters.Add(sName:="bIsImportedMPDataModel", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="")
                        Return nReturn
                    End If

                    nReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateMPDataModelSQL, sSQLName:=ACUpdateMPDataModelName, bStoredProcedure:=ACUpdateMPDataModelStored)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACUpdateMPDataModelSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="")
                        Return nReturn
                    End If
                End If
            End If
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="", excep:=ex)
            Return nReturn
        Finally
            'clear all objects here
        End Try
        Return nReturn
    End Function

    ''' <summary>
    ''' This method will export data model dynamic script file at pure\datasets\datamodelcode_script\datamodelcode_script.sql
    ''' which can be run on other database to copy same data model on that database
    ''' </summary>
    ''' <param name="sDataModelCode"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ExportDataModelScript(ByVal sDataModelCode As String) As Integer
        Const kMethodName As String = "ExportDataModelScript"
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMFalse

        Try
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="sDataModelCode", vValue:=sDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="")
                Return nReturn
            End If

            Dim dtResults As DataTable
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACExportMPDataModelSQL, sSQLName:=ACExportMPDataModelName, bStoredProcedure:=ACExportMPDataModelStored, oRecordset:=dtResults)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACExportMPDataModelSQL & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:="")
                Return nReturn
            Else
                If dtResults IsNot Nothing AndAlso dtResults.Rows IsNot Nothing AndAlso dtResults.Rows.Count > 0 Then
                    Dim sPath As String = Strings.Left(My.Application.Info.DirectoryPath, InStr(My.Application.Info.DirectoryPath, "Application") - 1) & "Datasets\" & sDataModelCode & "_Script"

                    If Not Directory.Exists(sPath) Then
                        Directory.CreateDirectory(sPath)
                    End If

                    Dim sFileName As String = Strings.Left(My.Application.Info.DirectoryPath, InStr(My.Application.Info.DirectoryPath, "Application") - 1) & "Datasets\" & sDataModelCode & "_Script\" & sDataModelCode & "_Script.sql"
                    Dim fsDataModel As StreamWriter = New StreamWriter(sFileName)

                    For Each trRow As DataRow In dtResults.Rows
                        fsDataModel.WriteLine(trRow(0).ToString)
                    Next

                    fsDataModel.Close()
                End If
            End If
            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ExportDataModelScript Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nReturn
        Finally
            'clear all objects here
        End Try

    End Function
    ''' <summary>
    ''' Validate that the combination of values for Bank Account Default maintenance
    ''' </summary>
    ''' <param name="v_nID">Account Id </param>
    ''' <param name="v_oValues">Value</param>
    ''' <param name="v_oColumns">Coumn Name</param>
    ''' <param name="r_sMessage">Massage</param>
    ''' <returns>Result</returns>
    ''' <remarks></remarks>
    Public Function ValidateBankAccountDetailsForReceiptType(ByVal v_nID As Integer, ByVal v_oValues As Object,
                                                             ByVal v_oColumns As Object, ByRef r_sMessage As String) As Integer

        Try

            ValidateBankAccountDetailsForReceiptType = PMEReturnCode.PMTrue

            Dim nCount As Integer
            Dim nSourceID As Integer
            Dim nCashListTypeID As Integer
            Dim nMediaTypeID As Integer
            Dim nProductID As Integer
            Dim bIsCashListItemTypeReceipts As Boolean
            Dim sCode As String
            Dim sDescription As String
            Dim nIs_deleted As Integer

            For nCount = LBound(v_oColumns, 2) To UBound(v_oColumns, 2)
                If UCase(v_oColumns(1, nCount)) = UCase("source_id") Then
                    nSourceID = v_oValues(nCount)
                End If
                If UCase(v_oColumns(1, nCount)) = UCase("cashlisttype_id") Then
                    nCashListTypeID = v_oValues(nCount)
                End If
                If UCase(v_oColumns(1, nCount)) = UCase("mediatype_id") Then
                    If Len(v_oValues(nCount)) > 0 Then
                        nMediaTypeID = v_oValues(nCount)
                    End If
                End If
                If UCase(v_oColumns(1, nCount)) = UCase("product_id") Then
                    If Len(v_oValues(nCount)) > 0 Then
                        nProductID = v_oValues(nCount)
                    End If
                End If
                If UCase(v_oColumns(1, nCount)) = UCase("code") Then
                    sCode = v_oValues(nCount)
                End If
                If UCase(v_oColumns(1, nCount)) = UCase("description") Then
                    sDescription = v_oValues(nCount)
                End If
                If UCase(v_oColumns(1, nCount)) = UCase("is_deleted") Then
                    nIs_deleted = v_oValues(nCount)
                End If
            Next

            Dim sSQL As String
            Dim oData As Object
            sSQL = "SELECT description FROM CashListType where CashListType_id = " & nCashListTypeID
            m_lReturn = m_oDatabase.SQLSelect(
                            sSQL:=sSQL,
                            sSQLName:="GetCashListType",
                            bStoredProcedure:=False,
                            lNumberRecords:=gPMConstants.PMAllRecords,
                            vResultArray:=oData)
            If IsArray(oData) Then
                If UCase$(oData(0, 0) & "") = "RECEIPTS" Then
                    bIsCashListItemTypeReceipts = True
                End If
            End If

            Dim sTableName As String
            Dim sColumns As String
            sColumns = "source_id, cashlisttype_id, mediatype_id, product_id"

            sTableName = "BankAccount_Default" & vbCrLf
            sTableName = sTableName & "WHERE 1 = 1 " & vbCrLf
            sTableName = sTableName & "AND is_deleted = " & nIs_deleted & vbCrLf
            sTableName = sTableName & "AND source_id =  " & nSourceID & vbCrLf
            sTableName = sTableName & "AND cashlisttype_id =  " & nCashListTypeID & vbCrLf
            If bIsCashListItemTypeReceipts Then
                sTableName = sTableName & "AND mediatype_id =  " & nMediaTypeID & vbCrLf
                If nProductID = 0 Then
                    sTableName = sTableName & "AND (product_id is null or product_id = " & nProductID & ")" & vbCrLf
                Else
                    sTableName = sTableName & "AND product_id =  " & nProductID & vbCrLf
                End If
            End If
            If v_nID <> 0 Then
                sTableName = sTableName & "AND BankAccount_Default_id" & " <> " & v_nID & vbCrLf
            End If
            m_lReturn = GetDetails(v_sTableName:=sTableName,
                                                v_sColumns:=sColumns,
                                                r_vData:=oData)
            If m_lReturn <> PMEReturnCode.PMNotFound Then
                If bIsCashListItemTypeReceipts Then
                    r_sMessage = "Selected combination of Product ID / Source ID / Media Type ID already exists for Receipts (" & sCode & "/" & sDescription & ").  Please alter selection to unique combination within the system."
                Else
                    r_sMessage = "Selected combination of Branch / Cash List Type is invalid (" & sCode & "/" & sDescription & ").  Please alter selection to unique combination within the system."
                End If
                ValidateBankAccountDetailsForReceiptType = PMEReturnCode.PMFalse
            End If
        Catch excep As System.Exception
            ' Log Error Message
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="ValidateBankAccountDetailsForReceiptType", r_lFunctionReturn:=ValidateBankAccountDetailsForReceiptType, excep:=excep)
            ' If you want to rollback a transaction or something, do it here
        End Try

    End Function
    Public Function GetUDLVersion(ByVal v_sTableName As String, ByRef r_nId As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim vArray As Object
            Dim sViewName As String = ""
            Dim sSql As String = ""
            'Strip _ID off

            result = gPMConstants.PMEReturnCode.PMTrue

            sSql = "select Max(UDL_version) from " + v_sTableName


            ' Perform the stored procedure
            m_lReturn = m_oArcDatabase.SQLSelect(sSQL:=sSql, sSQLName:=ACSQLGetViewsName, bStoredProcedure:=False, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Information.IsArray(vArray) Then

                r_nId = CInt(vArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetViewForColumn Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetViewForColumn", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)

            Return result


        End Try
    End Function

    ''' <summary>
    ''' get path of rule file i.e. C:/Pure/Rules
    ''' </summary>
    ''' <param name="sRulePath"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRulePath(ByRef sRulePath As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim sSubKey As String = ""

        sSubKey = "GIS\" & sGISDataModelCode

        nResult = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:="RulePath", r_sSettingValue:=sRulePath, v_sSubKey:=sSubKey), gPMConstants.PMEReturnCode)

        If sRulePath = "" Then
            nResult = PMEReturnCode.PMFalse
        Else
            If Not sRulePath.EndsWith("\") Then
                sRulePath = sRulePath & "\"
            End If
        End If

        Return nResult

    End Function
End Class

