Option Strict Off
Option Explicit On
Imports System.Data
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    'Implements SSP.S4I.Interfaces.IBusiness
    '*******************************************************************************
    ' Class Name: PMLookup
    '
    ' Date: 10 January 1997
    '
    ' Description: Creatable PMLookup class containing all the
    '              methods required to manipulate Lookup lists
    '              required by interface applications.
    '
    ' Edit History: 100197 - Created
    '
    '   TF200198 - GetCodeFromID() added
    '
    '   RAW 15/07/2003 : CQ258 : added extra sql call to include deleted lookup entries
    '
    '   PW170105 - PN18127 - don't sort selections from Posting_Type table.
    '*******************************************************************************

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
    Private Const ACClass As String = "PMLookup"

    Private Const ACSPReturnParamName As String = "return_value"
    Private Const ACSPErrorValue As Integer = -100

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode

    ' Public property to set correct database
    Private m_lPMLookupProductFamily As Integer
    ' PRIVATE Data Members (End)

    '<ThreadStatic()> _
    'Public Shared cache As Hashtable 'new shared (static) member (public so shared across all bPMLookup instances so no need to hit the db)
    Public Shared iCache As ICacheManager
    Private m_sCachePath As String
    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
        End Get
    End Property

    Public Property PMLookupProductFamily() As Integer
        Get
            Return m_lPMLookupProductFamily
        End Get

        Set(ByVal Value As Integer)

            Dim lReturn As Integer

            Try

                lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=Value, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=m_oDatabase)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp & "." & ACClass & "." & "PMLookupProductFamily" + ", " + "Unable to Check Database for Product Family: " & Value)
                    Exit Property
                End If

                ' All OK, so set the Lookup Family Property
                m_lPMLookupProductFamily = Value

            Catch excep As System.Exception

                Throw New System.Exception(excep.Source + ", " + excep.Message)

            End Try

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
    ' Edit History  :
    ' RAM20040308   : Code changes related to caching of UW Or Agenty Hidden Option
    '                   and store it in the module level variable
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

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

            ' Check that we have the right Database for our
            ' product Family
            lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_iUserID = iUserID

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            'todo
            'Try
            '    iCache = CacheFactory.GetCacheManager("PureCache")
            'Catch ex As Exception

            'End Try

            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sCachePath.EndsWith("\") = False Then
                m_sCachePath += "\"
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)
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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Selects all captions ns for the tes the
    '              Base Details.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef dtEffectiveDate As Date, ByRef vResultArray(,) As Object) As Integer
        Return GetLookupValues(iLookupType, vTableArray, iLanguageID, dtEffectiveDate, vResultArray, "")
    End Function

    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef dtEffectiveDate As Date, ByRef vResultArray(,) As Object, ByVal sSortColumnName As String) As Integer

        Dim result As Integer = 0
        Dim vLookupItems(,) As Object
        Dim iDimensions As Integer
        Dim sTableName As String = ""

        Dim vLookupKey As Object
        Dim lNumOfItems, lStartPosition As Integer
        Dim sWhereClause As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Check that the Table Name array is two dimensional
            ' Each row should contain a table name and a lookup item to retrieve (Blank for All)

            ' This statement will cause a subscript out of range error if
            ' the Table Array is NOT a two dimensional array.
            iDimensions = vTableArray.GetUpperBound(0)

            If iDimensions < 1 Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Lookup Table Name array (vTableArray). Must be two dimensional.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Reset Result Array
            vResultArray = Nothing

            ' Set Start Position to beginning
            lStartPosition = 0

            ' For each Lookup Table in the array
            For lTableRow As Integer = vTableArray.GetLowerBound(1) To vTableArray.GetUpperBound(1)

                ' Get the Lookup Table Name
                sTableName = CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lTableRow)).Trim()

                If sTableName = "" Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Lookup Table Name Must be supplied at position " & lTableRow, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Lookup Key

                ' Get the Lookup Key
                If ((vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lTableRow)) Is Nothing) Then
                    vLookupKey = Nothing
                Else
                    'removed trim because in some cases the value is coming integer 
                    vLookupKey = vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lTableRow)
                End If

                'get the where clause
                If vTableArray.GetUpperBound(0) >= gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause Then

                    sWhereClause = Convert.ToString(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupWhereClause, lTableRow)).Trim()
                End If

                ' Check that we have a key if we are looking up a single item
                If (iLookupType = gPMConstants.PMELookupType.PMLookupSingle) And (CStr(vLookupKey) = "") Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Reset Lookup Items
                vLookupItems = Nothing

                ' Are we doing a single lookup
                If iLookupType <> gPMConstants.PMELookupType.PMLookupSingle Then

                    ' No, so get all Lookup captions for this tabl
                    m_lError = CType(SelectCaptions(iLookupType:=iLookupType, sTable:=sTableName, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vLookupItems, sWhereClause:=sWhereClause, sSortColumnName:=sSortColumnName), gPMConstants.PMEReturnCode)

                Else
                    ' Yes, so get the specific Lookup caption for this key
                    m_lError = CType(SelectCaptions(iLookupType:=iLookupType, sTable:=sTableName, iLanguageID:=iLanguageID, vResultArray:=vLookupItems, dtEffectiveDate:=dtEffectiveDate, vID:=vLookupKey, sWhereClause:="", sSortColumnName:=sSortColumnName), gPMConstants.PMEReturnCode)

                End If

                ' Check Return Code from select
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Were any LookupItems returned
                If Not (vLookupItems Is Nothing) Then
                    lNumOfItems = vLookupItems.GetUpperBound(1) + 1
                Else
                    lNumOfItems = 0
                End If

                ' Set Start Position and Number of Items in Table Array
                vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupStartPos, lTableRow) = lStartPosition
                vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupNumOfItems, lTableRow) = lNumOfItems

                ' If we have any Items
                If lNumOfItems > 0 Then

                    ' Is this the first set of lookup items
                    If Not (vResultArray Is Nothing) Then

                        ' No, Add the lookup items to the end of the results array
                        ' Resize the Results array

                        ReDim Preserve vResultArray(vResultArray.GetUpperBound(0), vResultArray.GetUpperBound(1) + lNumOfItems)

                        ' Add the Lookup Items to the end of the results array

                        For lItemRow As Integer = vLookupItems.GetLowerBound(1) To vLookupItems.GetUpperBound(1)
                            'vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lItemRow)
                            'vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lItemRow)
                            'vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lItemRow)
                            'vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, lItemRow)

                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lItemRow)
                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lItemRow)
                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lItemRow)
                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, lItemRow)
                            lStartPosition += 1
                        Next lItemRow

                    Else
                        ' Yes, we can simply assign the results
                        vResultArray = vLookupItems
                        lStartPosition += lNumOfItems
                    End If

                End If

            Next lTableRow

            ' Reset Lookup Items
            vLookupItems = Nothing

            Return result

        Catch exc As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Nothing, vErrDesc:=exc.Message, excep:=exc)
            Return result

        End Try

    End Function

    ' ***************************************************************** '
    ' Name: GetEffectiveIDFromCode (Public)
    '
    ' Description: Gets the ID of the effective record, based on the
    '              supplied Code, Tablename and EffectiveDate.
    '
    ' ***************************************************************** '
    Public Function GetEffectiveIDFromCode(ByVal v_sTableName As String, ByVal v_sCode As String, ByVal v_dtEffectiveDate As Date, ByRef r_lID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturnStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' CTAF 181200 - Re-ordered the adding of parameters to be the same
            '               as the SP

            ' Add the return status parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:=ACSPReturnParamName, vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Tablename parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="tablename", vValue:=CStr(v_sTableName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Code parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_sCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Effective Date parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ID parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(r_lID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACGetFromCodeSQL, sSQLName:=ACGetFromCodeName, bStoredProcedure:=ACGetFromCodeStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return Status parameter
            lReturnStatus = m_oDatabase.Parameters.Item(ACSPReturnParamName).Value

            ' Check for Error from Stored Procedure
            If lReturnStatus = ACSPErrorValue Then
                r_lID = 0
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the ID Value NULL
            With m_oDatabase.Parameters.Item("id")

                If Convert.IsDBNull(.Value) Or (.Value Is Nothing) Then
                    ' Yes , Return zero
                    r_lID = 0
                Else
                    ' No, Return the ID value
                    r_lID = .Value
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveIDFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveIDFromCode", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetEffectiveIDFromID (Public)
    '
    ' Description: Gets the ID of the effective record, based on the
    '              supplied ID, Tablename and EffectiveDate.
    '
    ' ***************************************************************** '
    Public Function GetEffectiveIDFromID(ByVal v_sTableName As String, ByVal v_dtEffectiveDate As Date, ByRef r_lID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturnStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' CTAF 181200 - Re-ordered the adding of parameters to be the same
            '               as the SP

            ' Add the return status parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:=ACSPReturnParamName, vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Tablename parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="tablename", vValue:=CStr(v_sTableName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Effective Date parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ID parameter (INPUT/OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(r_lID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACGetFromIDSQL, sSQLName:=ACGetFromIDName, bStoredProcedure:=ACGetFromIDStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return Status parameter
            lReturnStatus = m_oDatabase.Parameters.Item(ACSPReturnParamName).Value

            ' Check for Error from Stored Procedure
            If lReturnStatus = ACSPErrorValue Then
                r_lID = 0
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Is the ID Value NULL
            With m_oDatabase.Parameters.Item("id")

                If Convert.IsDBNull(.Value) Or (.Value Is Nothing) Then
                    ' Yes , Return zero
                    r_lID = 0
                Else
                    ' No, Return the ID value
                    r_lID = .Value
                End If

            End With

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveIDFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveIDFromID", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCodeFromID (Public)
    '
    ' Description: Gets the Code of the effective record, based on the
    '              supplied Tablename and ID.
    '
    ' Edit History: TF200198 - Created
    ' ***************************************************************** '
    Public Function GetCodeFromID(ByVal v_sTableName As String, ByVal v_lID As Integer, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the Tablename parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="tablename", vValue:=CStr(v_sTableName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ID parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(v_lID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Code parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(r_sCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLAction(sSQL:=ACGetCodeFromIDSQL, sSQLName:=ACGetCodeFromIDName, bStoredProcedure:=ACGetCodeFromIDStored)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the code
            r_sCode = m_oDatabase.Parameters.Item("code").Value

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCodeFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeFromID", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name          : SelectCaptions (Public)
    '
    ' Description   : If No ID is supplied, gets all of the captions for
    '                   the Lookup Table.
    '                 If an ID is supplied, gets only the requested lookup
    '                 item from the Lookup Table.
    ' RAW 25/07/2003 : CQ258 : added lookuptype param
    ' RAM20040304   : Code changes related to caching of UW Or Agenty Hidden Option
    '                   code moved from here to the Initialise method
    ' RAM20040305   : Code changes related to caching of Lookups
    ' RAM20040323   : Code changes related to the lookups id are returned as long before, now
    '                   they are retured as string. we need to cater for these situations.
    ' RAM20040422   : Code changes related to CQ5226 (To cater dates in all regional settings)
    ' RAM20040511   : Code changes are done to utilize caching when using the 'WHERE CLAUSE'
    ' RAM20040512   : Code changes related to CQ5665 (To cater for Dates in all formats)
    '                 Notes : But if any of the machine have customise date format,
    '                           then it will fail, at that time we trap the error and
    '                           log  more details in the sirius log
    ' TR 17/05/2004 : CQ5734 - Added a catch in the error handler for more complex filters.
    '                 If using the given filter against the cached data fails, we now
    '                 attempt once only, to get the filtered data from the database.
    '                 This will support the complex dynamic filters allowable in Product Builder
    ' ***************************************************************** '    
    Private Function SelectCaptions(ByRef iLookupType As Integer, ByRef sTable As String,
                                      ByRef iLanguageID As Integer,
                                      ByRef dtEffectiveDate As Date,
                                      ByRef vResultArray(,) As Object,
                                      Optional ByRef vID As Object = Nothing,
                                   Optional ByRef sWhereClause As String = "",
                                   Optional ByVal sSortColumnName As String = "") As Integer
        Dim Err_SelectCaptions As Boolean = False
        Dim result As Integer = 0
        Dim sKey As String = ""
        Dim sFilter As String = ""
        Dim lReturn As Integer = 0
        Dim oADORecordset As DataSet = Nothing
        Dim vFieldNameArray() As Object
        Dim bApplyingFilter As Boolean
        Dim bAfterApplyingFilter As Boolean
        Dim strErrorMessage As String = ""
        Dim bUDLVersioningSearch As Boolean
        Dim vResults(,) As Object = Nothing
        Dim sContent(1) As String
        Dim sCacheFilename As String = ""
        Dim sFilePath As String = ""
        Try
            Err_SelectCaptions = True

            result = gPMConstants.PMEReturnCode.PMTrue

            bUDLVersioningSearch = False
            If sTable.Substring(0, 4).ToLower = "udl_" Then
                If (sWhereClause.IndexOf("Effective_date")) > 0 Then
                    bUDLVersioningSearch = True
                End If
            End If


            '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040305 : Code changes related to Caching - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''

            ' Check if the table is already cached in our Cache

            ' Create key for the input parameters
            ' eg. KEY_LOOKUP_00026_<sTable> :  means : Language ID 26 <sTable> means supplied Table Name
            'sKey = "KEY_LOOKUP_" & StringsHelper.Format(iLanguageID, "00000") & "_" & sTable.ToUpper()
            sKey = String.Format("KEY_LOOKUP_{0:00000}_{1}", iLanguageID, sTable.ToUpper())
            sCacheFilename = sKey
            If Not (vID Is Nothing) Then
                If CInt(vID) <> 0 Then
                    sKey += "_parent = '" & vID.ToString.Trim & "'"
                End If
            End If

            ' Check the iLookupType Parameter
            If iLookupType <> gPMConstants.PMELookupType.PMLookupAllWithDeleted Then
                sKey += "_is_deleted_0"
            End If

            If Not bUDLVersioningSearch Then
                Dim dateTime As DateTime = Nothing
                If DateTime.TryParse(dtEffectiveDate, dateTime) Then

                    sKey += "_effective_date_" & dtEffectiveDate.AddDays(1).ToString("yyyyMMdd")
                End If
            End If
            ' If a Where clause has been supplied then add the where clause to the filter criteria
            If sWhereClause.Trim().Length > 0 Then
                sKey += sWhereClause.Trim().Replace("=", "_").Replace("(", "").Replace(")", "").Replace("<", "").Replace(">", "")
            End If

            'If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
            '    vResults = iCache.GetData(sKey)
            '    vResultArray = vResults
            'End If

            If (vResults Is Nothing) Then


                lReturn = SelectLookupTable(iLanguageID, sTable, oADORecordset)
                ' Check how many rows of data we have, if the record count is > 0, then return the data array
                If oADORecordset.Tables(0).Rows.Count > 0 Then

                    ' Based on the input paramter apply filter
                    sFilter = ""
                    ' Filter by ID (if supplied return the data even if it is deleted (i.e isdeleted = 1)
                    ' we need to return by just one row based on the ID

                    If Not (vID Is Nothing) Then
                        ' Check that we have a key if we are looking up a single item and see if we have
                        If (iLookupType = gPMConstants.PMELookupType.PMLookupSingle) And CStr(vID) = "" Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Invalid Data. Lookup ID Missing. sTable : " & sTable, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectCaptions")

                            result = gPMConstants.PMEReturnCode.PMFalse

                            ' Close the record set
                            oADORecordset = Nothing
                            Return result
                        Else
                            ' Build the Filter
                            ' RAM20040323 : Now the ID is stored as String
                            sFilter = sTable & "_ID = '" & vID.ToString.Trim() & "'"
                        End If
                    Else
                        ' Check the iLookupType Parameter
                        If iLookupType <> gPMConstants.PMELookupType.PMLookupAllWithDeleted Then
                            If sFilter.Length > 0 Then sFilter = sFilter & " AND "
                            ' RAM20040323 : Now the is_deleted is stroed as String
                            sFilter = sFilter & "is_deleted = '0'"
                        End If

                        If Not bUDLVersioningSearch Then
                            Dim dateTime As DateTime = Nothing
                            If DateTime.TryParse(dtEffectiveDate, dateTime) Then
                                If sFilter.Length > 0 Then sFilter = sFilter & " AND "
                                sFilter = sFilter & "effective_date  <  #" & dtEffectiveDate.AddDays(1).ToString("yyyy/MM/dd") & "#"
                            End If
                        End If
                        ' If a Where clause has been supplied then add the where clause to the filter criteria
                        If sWhereClause.Trim().Length > 0 Then
                            If sFilter.Length > 0 Then sFilter = sFilter & " AND "
                            sFilter = sFilter & sWhereClause.Trim()
                        End If
                    End If

                    If sFilter.Length > 0 Then
                        bApplyingFilter = True  ' RAM200405123 : CQ5665 - Debug purpose
                        ' Filter the recordset for the supplied filter
                        Try
                            oADORecordset.Tables(0).DefaultView.RowFilter = sFilter
                            If sSortColumnName <> "" AndAlso oADORecordset.Tables(0).Columns.Contains(sSortColumnName) Then
                                oADORecordset.Tables(0).DefaultView.Sort = sSortColumnName
                            End If

                        Catch ex As Exception
                            'TR - Add a seperate error handler section to catch complex filters and
                            'try a direct call to the database ONCE ONLY
                            'Select Case Information.Err().Number
                            '    Case 5
                            '        'TR - Have we already tried to use the alternative solution
                            '        If lComplexFilterAttemptCount = 0 Then
                            '            'TR - First increment the counter as we only want to try this once(?)
                            '            lComplexFilterAttemptCount = lComplexFilterAttemptCount + 1

                            '            'TR - Do a select from the database when the clause is more complex
                            '            SelectCaptionsFromDatabase(iLookupType:=iLookupType, sTable:=sTable,
                            '                iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate,
                            '                vResultArray:=vResultArray, vID:=vID, sWhereClause:=sWhereClause)

                            '            Return gPMConstants.PMEReturnCode.PMTrue
                            '        End If

                            '    Case Else
                            '        'Continue as before
                            'End Select
                        End Try

                        bAfterApplyingFilter = True ' RAM200405123 : CQ5665 - Debug purpose
                    End If

                    'Create Field Array (This should match the column names as in the
                    ' ACSelectLookupTableToCacheSQL SQL (PMLookupSQL.bas)
                    ReDim vFieldNameArray(3)


                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID) = sTable & "_ID"
                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption) = "caption"
                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode) = "code"
                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted) = "is_deleted"
                    ' Check how many rows of data we have, if the record count is > 0 after filter, then return the data array
                    If oADORecordset.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                        ' Return the Results back into an Array, without the effective date column
                        ''developer guide no.  modified the below line			          
                        'vResultArray = oADORecordset.GetRows(ADODB.GetRowsOptionEnum.adGetRowsRest, ADODB.BookmarkEnum.adBookmarkCurrent, vFieldNameArray)
                        Dim indx As Integer
                        Dim intRowCount As Integer
                        Dim dtTable As DataTable = oADORecordset.Tables(0).DefaultView.ToTable
                        intRowCount = oADORecordset.Tables(0).DefaultView.ToTable.Rows.Count
                        ReDim vResultArray(3, intRowCount - 1)
                        For indx = 0 To intRowCount - 1
                            With dtTable
                                vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, indx) = .Rows(indx)(gPMConstants.PMELookupOutArrayColPos.PMLookupID)
                                vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, indx) = .Rows(indx)(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption)
                                vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, indx) = .Rows(indx)(gPMConstants.PMELookupOutArrayColPos.PMLookupCode)
                                vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted, indx) = .Rows(indx)(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted)
                            End With
                        Next
                    End If

                End If
                ' Add them to the Cache
                sFilePath = m_sCachePath + sCacheFilename + ".xml"

                If Not FileExists(sFilePath) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sFilePath)
                    fileIO.Close()
                    File.WriteAllLines(sFilePath, sContent)
                End If
                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller
                'If Not iCache Is Nothing Then
                '    'iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
                '    iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sFilePath))
                'End If

            End If

            ' Close the record set
            oADORecordset = Nothing

            Return result
        Catch excep As System.Exception
            If Not Err_SelectCaptions Then
                Throw excep
            End If

TryComplexFilterSelect:
            If Err_SelectCaptions Then
                ' Error Section.
                result = gPMConstants.PMEReturnCode.PMError

                ' CQ5665 : To Trap Error Message
                strErrorMessage = "Select Captions Failed."
                strErrorMessage = strErrorMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Cache key : " & sKey
                strErrorMessage = strErrorMessage & Strings.ChrW(13) & Strings.ChrW(10) & "sFilter   : " & sFilter

                If bApplyingFilter And (Not bAfterApplyingFilter) Then
                    strErrorMessage = strErrorMessage & Strings.ChrW(13) & Strings.ChrW(10) & "While trying to apply Filter to the Record."
                End If

                If bAfterApplyingFilter Then
                    strErrorMessage = strErrorMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Filter Applied Successfully."
                    strErrorMessage = strErrorMessage & Strings.ChrW(13) & Strings.ChrW(10) & "But oADORecordset.GetRows Failed."
                End If

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=strErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectCaptions", vErrNo:=Nothing, vErrDesc:=excep.Message, excep:=excep)
                Return result
            End If
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name          : SelectCaptionsFromDatabase (Private)
    '
    ' Description   : If No ID is supplied, gets all of the captions for
    '                   the Lookup Table.
    '                 If an ID is supplied, gets only the requested lookup
    '                 item from the Lookup Table.
    ' Edit History  :
    ' RAM20040305   : Created
    ' RAM20040305   : Added code related to the get Underwriting / Agency
    ' ***************************************************************** '
    Private Function SelectCaptionsFromDatabase(ByRef iLookupType As Integer, ByRef sTable As String, ByRef iLanguageID As Integer, ByRef dtEffectiveDate As Date, ByRef vResultArray(,) As Object, Optional ByRef vID As Object = Nothing, Optional ByRef sWhereClause As String = "") As Integer

        Dim result As Integer = 0
        Dim sSQL, sSQLMode As String
        Dim bUDLVersioningSearch As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue

        '  order for underwriting, non-ordered for broking - default to broking
        sSQLMode = ""

        bUDLVersioningSearch = False
        If sTable.Substring(0, 4).ToLower = "udl_" Then
            If (sWhereClause.IndexOf("Effective_date")) > 0 Then
                bUDLVersioningSearch = True
            End If
        End If

        sSQLMode = " ORDER BY 2" 'always Underwriting now so removed if condition

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Table Name Parameter (INPUT)
        m_lError = m_oDatabase.Parameters.Add(sName:="Table_Name", vValue:=CStr(sTable), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMTableName)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the LanguageID Parameter (INPUT)
        m_lError = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Is ID Supplied

        If Not (vID Is Nothing) Then

            ' Yes , Add ID Parameter

            m_lError = m_oDatabase.Parameters.Add(sName:="ID", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACSelectByIDSQL, sSQLName:=ACSelectByIDName, bStoredProcedure:=ACSelectByIDStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Else

            If bUDLVersioningSearch Then
                sSQL = ACSelectAllUDLSQL
            Else
                sSQL = ACSelectAllSQL
            End If

            If sTable.ToUpper() = "GIS_DATA_MODEL" Then
                sSQL = sSQL.Replace(" WHERE", " LEFT JOIN hidden_options ON hidden_options.option_number = " &
                       "tn.product_option WHERE (ISNULL(hidden_options.value,'0')='1' OR " &
                       "tn.product_option IS NULL) AND")
            End If

            ' RAW 25/07/2003 : CQ258 : added
            If iLookupType <> gPMConstants.PMELookupType.PMLookupAllWithDeleted Or sTable.ToLower() = "lapsed_reason" Then
                sSQL = sSQL & " AND tn.is_deleted = 0"
            End If
            ' RAW 25/07/2003 : CQ258 : end

            If sWhereClause <> "" Then
                sSQL = sSQL & " AND " & sWhereClause
            End If
            sSQL = sSQL & " " & sSQLMode
            If Not bUDLVersioningSearch Then
                ' No , Add Effective Parameter
                m_lError = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=CStr(dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACSelectAllName, bStoredProcedure:=ACSelectAllStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : SelectLookupTableInToXML (Public)
    '
    ' Description   : Function to Return All the records including deleted
    '                   into an XML Stream
    ' Edit History  :
    ' RAM20040305   : Created
    ' ***************************************************************** '
    Private Function SelectLookupTable(ByVal v_iLanguageID As Integer, ByVal v_sTable As String, ByRef r_vLookupDataset As DataSet) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""
        Dim sKey As String = String.Empty
        Dim sFilePath As String = ""
        Dim sCacheFilename As String = ""
        Dim sContent(1) As String
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a New Database Connection
            lReturn = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, m_oDatabase), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create a new Instance of Database.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectLookupTableInToXML", vErrNo:=Nothing, vErrDesc:=Information.Err().Description)
                Return lReturn
            End If
            sKey = String.Format("KEY_LOOKUP_{0:00000}_{1}", CStr(v_iLanguageID), v_sTable.ToUpper())
            sCacheFilename = sKey
            ' Make-up the SQL Script to fetch the Lookups
            sSQL = ACSelectLookupTableToCacheSQL
            ' Replace the Table Name with the supplied one
            sSQL = sSQL.Replace("{Table_Name}", v_sTable)
            ' Replace the Language ID
            sSQL = sSQL.Replace("{Language_ID}", CStr(v_iLanguageID))
            sSQL = sSQL.Replace("{Is_Underwriting}", CStr(1))

            'If Not iCache Is Nothing AndAlso iCache.Contains(sKey) AndAlso Not String.IsNullOrEmpty(Convert.ToString(iCache.GetData(sKey))) Then
            '    r_vLookupDataset = iCache.GetData(sKey)
            'Else

            'End If
            If r_vLookupDataset Is Nothing Then
                ' Create a new ADO Recordset
                r_vLookupDataset = New DataSet()

                ' Execute the SQL
                lReturn = m_oDatabase.BatchSQLSelect(sSQL, r_vLookupDataset)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Lookup data for Lookup Table [" & v_sTable & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectLookupTableInToXML", vErrNo:=Nothing, vErrDesc:='Information.Err().Description)
                    Return lReturn
                End If
                ' Add them to the Cache
                sFilePath = m_sCachePath + sCacheFilename + ".xml"

                If Not FileExists(sFilePath) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sFilePath)
                    fileIO.Close()
                    File.WriteAllLines(sFilePath, sContent)
                End If
                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller
                'If Not iCache Is Nothing Then
                '    'iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
                '    iCache.Add(sKey, r_vLookupDataset, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sFilePath))
                'End If
            End If

            ' Add them to the Cache
            sFilePath = m_sCachePath + sCacheFilename + ".xml"

            If Not FileExists(sFilePath) Then
                Dim fileIO As FileStream
                fileIO = File.Create(sFilePath)
                fileIO.Close()
                File.WriteAllLines(sFilePath, sContent)
            End If
            ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
            ' Sirius Cache Controller
            'If Not iCache Is Nothing Then
            '    'iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(m_sCachePath + m_sCacheFilename))
            '    iCache.Add(sKey, r_vLookupDataset, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sFilePath))
            'End If

            Return result
        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectLookupTableInToXML Failed. Table Name : " & v_sTable, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectLookupTableInToXML", vErrNo:=Nothing, vErrDesc:=excep.Message)
            Return result

        End Try
    End Function

    Public Function GetCache() As String


        'Try

        '    Dim _cache As Cache = DirectCast(GetType(CacheManager).GetField("realCache", _
        '                            System.Reflection.BindingFlags.Instance Or System.Reflection.BindingFlags.NonPublic).GetValue(iCache), Cache)
        '    For Each item As DictionaryEntry In _cache.CurrentCacheState

        '        Debug.Print(item.Key)

        '    Next

        '    Return iCache.ToString

        'Catch ex As Exception

        '    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to load cache", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Nothing, vErrDesc:=ex.Message, excep:=ex)
        '    Return Nothing

        'Finally

        'End Try
        Return Nothing
    End Function

    Public Function ClearCache() As Integer

        'Const kMethodName As String = "ClearCache"
        Dim result As Integer = 0
        ''no key passed - clear the whole cache        

        'Try
        '    result = gPMConstants.PMEReturnCode.PMTrue
        '    iCache.Flush()
        '    Return result

        'Catch ex As Exception

        '    result = gPMConstants.PMEReturnCode.PMError
        '    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to clear cache", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Nothing, vErrDesc:=ex.Message, excep:=ex)
        '    Return result

        'Finally
        'End Try
        Return result
    End Function

    Public Function ClearCache(ByVal sKey As String) As Integer


        Dim result As Integer = 0

        'Try
        '    result = gPMConstants.PMEReturnCode.PMTrue
        '    iCache.Remove(sKey)
        '    Return result

        'Catch ex As Exception

        '    result = gPMConstants.PMEReturnCode.PMError
        '    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to remove item from cache. Key: " + sKey, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Nothing, vErrDesc:=ex.Message, excep:=ex)
        '    Return result

        'Finally

        'End Try
        Return result
    End Function

End Class
