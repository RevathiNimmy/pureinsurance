Option Strict Off
Option Explicit On
Imports System.Data
Imports System.IO
Imports Microsoft.Practices.EnterpriseLibrary.Caching
Imports Microsoft.Practices.EnterpriseLibrary.Caching.Expirations
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMLookup
    '
    ' Date: 10 January 1997
    '
    ' Description: Creatable PMLookup class containing all the
    '              methods required to manipulate Lookup lists
    '              required by interface applications.
    '
    ' Edit History: 100197 - Created
    '               TF200198 - GetCodeFromID() added
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
    Private Const ACClass As String = "PMLookup"

    Private Const ACSPReturnParamName As String = "eturn_value"
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
    Private m_sGISDataModelCode As String = ""
    ' PRIVATE Data Members (End)
    Public Shared iCache As ICacheManager
    Private m_sCachePath As String

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    Public Property GISDataModelCode() As String
        Get

            Return m_sGISDataModelCode

        End Get
        Set(ByVal Value As String)

            Dim lReturn As gPMConstants.PMEReturnCode

            Try

                If Value = m_sGISDataModelCode Then
                    Exit Property
                End If

                lReturn = CType(GISSharedConstants.CheckGISDSN(v_sDataModelCode:=Value, r_oDatabase:=m_oDatabase, r_bNew:=m_bCloseDatabase), gPMConstants.PMEReturnCode)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New System.Exception((gPMConstants.PMEReturnCode.PMFalse).ToString() + ", " + ACApp & "." & ACClass & "." & "GISDataModelCode" + ", " + "Unable to Check Database for Product Family: " & Value)
                    Exit Property
                End If

                ' All OK, so set the Data Model Property
                m_sGISDataModelCode = Value

            Catch excep As System.Exception



                Throw New System.Exception(Informations.Err().Number.ToString() + ", " + excep.Source + ", " + excep.Message)

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
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Dim lReturn As Integer

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
            m_iUserID = iUserID

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040713 : Use the passed in Database object
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040713 : END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            'Caching

            Try
                iCache = CacheFactory.GetCacheManager("PureCache")
            Catch ex As Exception
            End Try

            lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine,
                                                   v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture,
                                                   v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer,
                                                   v_sSettingName:=gPMConstants.PMRegKeyCachePath, r_sSettingValue:=m_sCachePath)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_sCachePath.Substring(m_sCachePath.Length - 1) <> "\" Then
                m_sCachePath += "\"
            End If

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
    ' Edit History  :
    ' RAM20040716   : Close the database connection, only if this class creates the dPMDAO.
    '                   since, we changed the Initialise method to use the passed in dPMDAO..
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                If m_oDatabase IsNot Nothing Then
                    If m_bCloseDatabase Then
                        m_oDatabase.CloseDatabase()
                        m_oDatabase = Nothing
                    End If
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Selects all captions for the
    '              Base Details.
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef dtEffectiveDate As Date, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vLookupItems(,) As Object
        Dim iDimensions As Integer
        Dim lTable As Integer
        Dim sTableName As String = ""
        Dim vLookupKey As Object
        Dim lNumOfItems, lStartPosition As Integer
        result = gPMConstants.PMEReturnCode.PMTrue

        ' Unset On Error handling

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

            ' Reset On Error handling

            ' Reset Result Array
            vResultArray = Nothing

            'vResultArray = ""

            ' Set Start Position to beginning
            lStartPosition = 0

            ' For each Lookup Table in the array
            For lTableRow As Integer = vTableArray.GetLowerBound(1) To vTableArray.GetUpperBound(1)

                ' Get the Lookup Table Name
                sTableName = CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lTableRow)).Trim()
                lTable = ToSafeInteger(CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lTableRow)).Trim())
                sTableName = CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lTableRow)).Trim()

                If lTable = 0 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Lookup Table Name Must be supplied at position " & lTableRow, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Lookup Key

                vLookupKey = CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, lTableRow)).Trim()

                ' Check that we have a key if we are looking up a single item
                If (iLookupType = gPMConstants.PMELookupType.PMLookupSingle) And (vLookupKey = "") Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key Must be supplied at position " & lTableRow, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Reset Lookup Items
                vLookupItems = Nothing

                'vLookupItems = ""

                ' Are we doing a single lookup
                If iLookupType <> gPMConstants.PMELookupType.PMLookupSingle Then

                    ' No, so get all Lookup captions for this table

                    m_lError = CType(SelectCaptions(lTable:=lTable, iLanguageID:=iLanguageID,
                                                    dtEffectiveDate:=dtEffectiveDate,
                                                    vResultArray:=vLookupItems,
                                                    vID:=vLookupKey,
                                                    sTable:=sTableName), gPMConstants.PMEReturnCode)
                Else
                    ' RAM20040521 : If we need a single lookup, go and get it based on the
                    '               ID, the passing of Lookuptype param will trigger the
                    '               select captions to fetch it based on the Key Supplied.
                    ' Yes, so get the specific Lookup caption for this key

                    m_lError = CType(SelectCaptions(lTable:=lTable, iLanguageID:=iLanguageID,
                                                    vResultArray:=vLookupItems,
                                                    dtEffectiveDate:=dtEffectiveDate,
                                                    vID:=vLookupKey,
                                                    iLookupType:=gPMConstants.PMELookupType.PMLookupSingle,
                                                    sTable:=sTableName), gPMConstants.PMEReturnCode)
                End If

                ' Check Return Code from select
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Were any LookupItems returned
                If Informations.IsArray(vLookupItems) Then

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
                    If Informations.IsArray(vResultArray) Then

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

Err_GetLookupValues:

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result


        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
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

            ' Add the ID parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(r_lID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the return status parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:=ACSPReturnParamName, vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Effective Date parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Code parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="code", vValue:=CStr(v_sCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Tablename parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="tablename", vValue:=CStr(v_sTableName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

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


                If Convert.IsDBNull(.Value) Or Informations.IsNothing(.Value) Then
                    ' Yes , Return zero
                    r_lID = 0
                Else
                    ' No, Return the ID value
                    r_lID = .Value
                End If

            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveIDFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveIDFromCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            ' Add the ID parameter (INPUT/OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(r_lID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInputOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the return status parameter (OUTPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:=ACSPReturnParamName, vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamReturnValue, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Effective Date parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=CStr(v_dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Tablename parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="tablename", vValue:=CStr(v_sTableName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

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


                If Convert.IsDBNull(.Value) Or Informations.IsNothing(.Value) Then
                    ' Yes , Return zero
                    r_lID = 0
                Else
                    ' No, Return the ID value
                    r_lID = .Value
                End If

            End With

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveIDFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveIDFromID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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




            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCodeFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeFromID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '

    'Private Function BeginTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lError = m_oDatabase.SQLBeginTrans()
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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

    'Private Function CommitTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lError = m_oDatabase.SQLCommitTrans()
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
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

    'Private Function RollbackTrans() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Begin the Transaction
    'm_lError = m_oDatabase.SQLRollbackTrans()
    '
    'If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    ' Error.
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
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
        ' Error.
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
    ' Name: SelectCaptions (Public)
    '
    ' Description: If No ID is supplied, gets all of the captions for
    '              the Lookup Table.
    '              If an ID is supplied, gets only the requested lookup
    '              item from the Lookup Table.
    ' Edit History  :
    ' RAM20040308   : Code changes related to caching of GIS Lookups
    ' RAM20040422   : Code changes related to CQ5226 (To cater dates in all regional settings)
    ' RAM20040512   : Code changes related to CQ5665 (To cater for Dates in all formats)
    '                 Notes : But if any of the machine have customise date format,
    '                           then it will fail, at that time we trap the error and
    '                           log  more details in the sirius log
    ' RAM20040521   : Code changed to pass in the LookupType as well
    ' ***************************************************************** '
    Private Function SelectCaptions(ByRef lTable As Integer, ByRef iLanguageID As Integer,
                                      ByRef dtEffectiveDate As Date,
                                      ByRef vResultArray(,) As Object,
                                      Optional ByRef vID As Object = Nothing,
                                      Optional ByRef iLookupType As Object = Nothing,
                                      Optional ByRef sTable As String = "") As Integer


        Dim result As Integer = 0
        Dim sKey As String = ""
        Dim vLookupsXML As String = "" ' 64 K Limit ????
        Dim sFilter As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim oADORecordset As DataSet
        Dim vFieldNameArray() As Object
        Dim dtDateToCompare As Date
        Dim bApplyingFilter As Boolean
        Dim bAfterApplyingFilter As Boolean
        Dim strErrorMessage As String = ""
        Dim vResults(,) As Object = Nothing
        Dim sContent(1) As String
        Dim sCacheFilename As String = ""
        Dim sFilePath As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040305 : Code changes related to Caching - START
            '''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' Check if the table is already cached in our Cache

            ' Create key for the input parameters
            ' eg. KEY_GIS_LOOKUP_00026_00018 :  means : Language ID 26,  Table ID 18
            sKey = String.Format("KEY_LOOKUP_{0:00000}_{1}", iLanguageID, sTable.ToUpper())
            sCacheFilename = sKey
            If Informations.IsDate(dtEffectiveDate) Then
                sKey = sKey & "_" & dtEffectiveDate.AddDays(1).ToString("yyyy/MM/dd").ToString
            End If
            If Not Informations.IsNothing(vID) Then
                If ToSafeInteger(vID) <> 0 Then
                    sKey += "_parent = '" & CStr(vID) & "'"
                End If
            End If

            If Not iCache Is Nothing AndAlso iCache.Contains(sKey) Then
                vResults = iCache.GetData(sKey)
                vResultArray = vResults
            End If

            If Informations.IsNothing(vResults) Then
                ' The table is not cached so, use the usual way to fetch the data
                lReturn = SelectLookupsInToXML(iLanguageID, lTable, vLookupsXML)
                If (String.IsNullOrEmpty(vLookupsXML)) Then
                    Return lReturn
                End If

                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                Else
                    ' Log Error Message
                    Return lReturn
                End If


                ' We have the lookups in the in XML Format Need to load them back in the reocord set first            
                ' Create a New RecordSet
                oADORecordset = New DataSet()
                Dim xDoc As System.Xml.XmlDocument = New Xml.XmlDocument()
                xDoc.LoadXml(vLookupsXML)
                Dim sReader As New System.IO.StringReader(xDoc.InnerXml)
                oADORecordset.ReadXml(sReader)

                ' Check how many rows of data we have, if the record count is > 0, then return the data array
                If oADORecordset.Tables(0).Rows.Count > 0 Then

                    ' Based on the input paramter apply filter
                    sFilter = ""

                    ' Set the default filter
                    If Informations.IsDate(dtEffectiveDate) Then
                        ' Do we need to use timestamp ????
                        'sFilter = "effective_date <= #" & Format(dtEffectiveDate, "MM/DD/YYYY") & "#"

                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' RAM20040422 : CQ5226 - To make sure that we cater for the default setting of the
                        '               busines server, Assumes that dPMDAO, and this component is running
                        '               in the same business server.
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' Add one day to the effective date, ignore time stamp
                        dtDateToCompare = CDate(dtEffectiveDate.AddDays(1).ToString("yyyy/MM/dd"))
                        ' RAM20040512 : Changed from Long date to short date, since long date may contain, day
                        '               which Ado doesn't like when used in the filter clause
                        'developer guide no. 
                        sFilter = sFilter & "effective_date  <  #" & DateTime.Parse(dtDateToCompare).ToString("yyyy/MM/dd") & "#"
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                        ' RAM20040422 : CQ5226 - END
                        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    End If

                    ' Filter by ID, if supplied ID is valid ie. <> 0
                    ' By default 0 Will be passed in
                    ' This will override the effective date filter

                    If Not Informations.IsNothing(vID) Then
                        ' RAM20040521 : The id from Cached may come as a string, So, convert it into
                        '               long and compare it.

                        If ToSafeInteger(vID) <> 0 Then
                            ' Build the Filter for the ID, this will override the original effective date
                            ' RAM20040323 : Now the id ID is stroed as Strings in the CACHE

                            sFilter = "[parent] = '" & CStr(vID) & "'"
                        End If
                    End If

                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20040521 : Code changes to cater to get description for unique id
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' This will override all the above filer

                    If Not Informations.IsNothing(iLookupType) Then
                        ' We want a single lookup description, the ID passed in here is the
                        ' unique in the whole GIS User Def Detail table

                        If ToSafeInteger(vID) <> 0 Then

                            sFilter = "GIS_user_def_detail_id = '" & CStr(vID) & "'"
                        End If
                    End If
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
                    ' RAM20040521 : Code changes to cater to get description for unique id
                    '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

                    If sFilter.Length > 0 Then
                        bApplyingFilter = True ' RAM200405123 : CQ5665 - Debug purpose
                        ' Filter the recordset for the supplied filter
                        oADORecordset.Tables(0).DefaultView.RowFilter = sFilter
                        bAfterApplyingFilter = True ' RAM200405123 : CQ5665 - Debug purpose
                    End If

                    'Create Field Array (This should match the column names as in the
                    ' ACSelectLookupTableToCacheSQL SQL (PMLookupSQL.bas)
                    ReDim vFieldNameArray(3)

                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID) = "GIS_user_def_detail_id"

                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption) = "caption"

                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode) = "code"

                    vFieldNameArray(gPMConstants.PMELookupOutArrayColPos.PMLookupIsDeleted) = "is_deleted"

                    ' Check how many rows of data we have after applying the filter, if the record count is > 0,
                    ' then return the data array
                    If oADORecordset.Tables(0).DefaultView.ToTable.Rows.Count > 0 Then
                        ' Return the Results back into an Array, without the effective date column
                        'developer guide no. 31 of Guide
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

                ' Add them to the Cache
                If Not File.Exists(sFilePath) Then
                    Dim fileIO As FileStream
                    fileIO = File.Create(sFilePath)
                    fileIO.Close()
                    File.WriteAllLines(sFilePath, sContent)
                End If
                ' Add the key to the SIRIUS_CACHE_KEYS   Cache Array, to be used by
                ' Sirius Cache Controller
                If Not iCache Is Nothing Then
                    iCache.Add(sKey, vResultArray, CacheItemPriority.NotRemovable, Nothing, New FileDependency(sFilePath))
                End If
            End If

            ' Close the record set
            oADORecordset = Nothing

            ' Clear the cache


            Return result

        Catch excep As System.Exception



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

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=strErrorMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function





    ' ***************************************************************** '
    ' Name          : SelectLookupsInToXML (Public)
    '
    ' Description   : Function to Return All the GIS Lookups for the supplied
    '                   GIS_user_def_header_id including deleted into an XML Stream
    '
    ' Notes         : This function is added  to provide caching
    '
    ' Edit History  :
    ' RAM20040308   : Created
    ' ***************************************************************** '
    Private Function SelectLookupsInToXML(ByVal v_iLanguageID As Integer, ByVal v_lGIS_User_Def_Header_ID As Integer, ByRef r_vLookupXML As String) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sSQL As String = ""
        Dim oADORecordset As DataSet




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Make-up the SQL Script to fetch the Lookups
        sSQL = ACSelectGISUserDefDetailToCacheSQL
        ' Replace the Table Name with the supplied one
        sSQL = sSQL.Replace("{table}", CStr(v_lGIS_User_Def_Header_ID))
        ' Replace the Language ID
        sSQL = sSQL.Replace("{Language_ID}", CStr(v_iLanguageID))

        ' Create a new ADO Recordset
        oADORecordset = New DataSet()

        ' Execute the SQL
        lReturn = m_oDatabase.BatchSQLSelect(sSQL, oADORecordset)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Lookup data for GIS Lookup Table [" & v_lGIS_User_Def_Header_ID & "]", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectLookupsInToXML", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return lReturn
        End If

        ' Save the Recordset into the Stream as XML
        'developer guide no. 30 of Guide
        r_vLookupXML = ""
        If (oADORecordset.Tables.Count > 0) Then
            If (oADORecordset.Tables(0).Rows.Count > 0) Then
                r_vLookupXML = oADORecordset.GetXml()
            End If
        End If
        oADORecordset = Nothing

        Return result

    End Function
End Class

