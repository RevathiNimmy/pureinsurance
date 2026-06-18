Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'developer guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
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
    ' RAW 04/09/2003 : CQ258 : added extra sql call to include deleted lookup entries
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 26/11/2003
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
    Private m_lAccumulationLevel As Integer
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property AccumulationLevel() As Integer
        Get
            Return m_lAccumulationLevel
        End Get
        Set(ByVal Value As Integer)
            m_lAccumulationLevel = Value
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

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
        Dim lAccumulationLevel As Integer
        Dim vLookupKey As String = ""
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

            ' Reset Result Array
            vResultArray = Nothing

            'vResultArray = ""

            ' Set Start Position to beginning
            lStartPosition = 0

            ' For each Lookup Table in the array
            For lTableRow As Integer = vTableArray.GetLowerBound(1) To vTableArray.GetUpperBound(1)

                ' Get the Lookup Table Name

                lAccumulationLevel = CInt(CStr(vTableArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, lTableRow)).Trim())

                If lAccumulationLevel = 0 Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Accumulation Level Must be supplied at position " & lTableRow, vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues")
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

                ' Are we doing a single lookup
                If iLookupType <> gPMConstants.PMELookupType.PMLookupSingle Then

                    ' No, so get all Lookup captions for this table
                    ' RAW 04/09/2003 : CQ258 : added lookuptype
                    m_lError = CType(SelectCaptions(iLookupType:=iLookupType, lAccumulationLevel:=lAccumulationLevel, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vLookupItems), gPMConstants.PMEReturnCode)

                Else

                    ' Yes, so get the specific Lookup caption for this key
                    ' RAW 04/09/2003 : CQ258 : added lookuptype
                    m_lError = CType(SelectCaptions(iLookupType:=iLookupType, lAccumulationLevel:=lAccumulationLevel, iLanguageID:=iLanguageID, vResultArray:=vLookupItems, dtEffectiveDate:=dtEffectiveDate, vID:=vLookupKey), gPMConstants.PMEReturnCode)

                End If

                ' Check Return Code from select
                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Were any LookupItems returned
                If Information.IsArray(vLookupItems) Then

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
                    If Information.IsArray(vResultArray) Then

                        ' No, Add the lookup items to the end of the results array

                        ' Resize the Results array

                        ReDim Preserve vResultArray(vResultArray.GetUpperBound(0), vResultArray.GetUpperBound(1) + lNumOfItems)

                        ' Add the Lookup Items to the end of the results array

                        For lItemRow As Integer = vLookupItems.GetLowerBound(1) To vLookupItems.GetUpperBound(1)


                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupID, lItemRow)


                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCaption, lItemRow)


                            vResultArray(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lStartPosition) = vLookupItems(gPMConstants.PMELookupOutArrayColPos.PMLookupCode, lItemRow)
                            ' RAW 04/09/2003 : CQ258 : added


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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

            Return result



            Return result

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
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


                If Convert.IsDBNull(.Value) Or IsNothing(.Value) Then
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveIDFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveIDFromCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


                If Convert.IsDBNull(.Value) Or IsNothing(.Value) Then
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetEffectiveIDFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetEffectiveIDFromID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCodeFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCodeFromID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: SelectCaptions (Public)
    '
    ' Description: If No ID is supplied, gets all of the captions for
    '              the Lookup Table.
    '              If an ID is supplied, gets only the requested lookup
    '              item from the Lookup Table.
    ' ***************************************************************** '
    ' RAW 04/09/2003 : CQ258 : added lookuptype param
    ' ***************************************************************** '
    Private Function SelectCaptions(ByRef iLookupType As Integer, ByRef lAccumulationLevel As Integer, ByRef iLanguageID As Integer, ByRef dtEffectiveDate As Date, ByRef vResultArray(,) As Object, Optional ByRef vID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Table Name Parameter (INPUT)
        m_lError = m_oDatabase.Parameters.Add(sName:="accumulation_level", vValue:=CStr(lAccumulationLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the LanguageID Parameter (INPUT)
        m_lError = m_oDatabase.Parameters.Add(sName:="Language_ID", vValue:=CStr(iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Is ID Supplied

        If Not Information.IsNothing(vID) Then

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


            ' No , Add Effective Parameter
            m_lError = m_oDatabase.Parameters.Add(sName:="Effective_Date", vValue:=CStr(dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' RAW 04/09/2003 : CQ258 : added
            If iLookupType = gPMConstants.PMELookupType.PMLookupAllWithDeleted Then

                m_lError = m_oDatabase.Parameters.Add(sName:="v_iIncludeDeleted", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lError = m_oDatabase.Parameters.Add(sName:="v_iIncludeDeleted", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 04/09/2003 : CQ258 : end

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACSelectAllSQL, sSQLName:=ACSelectAllName, bStoredProcedure:=ACSelectAllStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    'UPGRADE_NOTE: (7001) The following declaration (BeginTrans) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
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
        ' Error.
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

End Class
