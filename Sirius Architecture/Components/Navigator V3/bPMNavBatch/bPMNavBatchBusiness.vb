Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
'Developer Guide No 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 02/10/1997
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMNavBatch.
    '
    ' Edit History: CTAF 021298 - Removed PMFunc, PMConst
    '                             Reference gPMLibraries
    '                             Added product family property
    '                             Component Services check in Initialise
    '
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


    ' ************************************************
    ' Added to replace global variables 18/09/2003
    ' Username.

    ' Password.

    ' User ID

    ' Calling Application
    ' Source ID
    ' Language ID
    ' Currency ID
    ' LogLevel
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

    ' Error status member
    Private m_lReturn As Integer

    ' Batch members for processing
    Private m_lBatchSetID As Integer
    Private m_lBatchRecordID As Integer

    ' RDC 13062002 CompServ replaced with BAS module
    'Private m_oCServices As sPMServerCS.PMServerBusinessCS

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' CF021298 - Added Product Family
    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property

    Public Property BatchSetID() As Integer
        Get

            Return m_lBatchSetID

        End Get
        Set(ByVal Value As Integer)

            m_lBatchSetID = Value

        End Set
    End Property

    Public Property BatchRecordID() As Integer
        Get

            Return m_lBatchRecordID

        End Get
        Set(ByVal Value As Integer)

            m_lBatchRecordID = Value

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
    ' History:
    '    02/12/98 - CTAF - Added database checks using Component Services
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise

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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set user ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            ' Create an instance of component services
            '    Set m_oCServices = New sPMServerCS.PMServerBusinessCS

            ' Use component services to check the database
            '    m_lReturn& = m_oCServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, m_sUsername, m_iSourceID, m_iLanguageID, _
            'v_lPMProductFamily:=pmePFSiriusArchitecture, _
            'r_bNewInstanceCreated:=m_bCloseDatabase, _
            'r_oCheckedDatabase:=m_oDatabase, _
            'v_vDatabase:=vDatabase)

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Remove the object
            '    Set m_oCServices = Nothing

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
    ' Name: DeleteBatchSet
    '
    ' Description: Deletes the data from the PMNav_Batch_Key_Value table
    '              after the batch data has been retrieved.
    '
    ' ***************************************************************** '
    Public Function DeleteBatchSet(ByVal v_lBatchSetID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the pmnav_batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="pmnav_batch_set_id", vValue:=CStr(v_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteBatchSetSQL, sSQLName:=ACDeleteBatchSetName, bStoredProcedure:=ACDeleteBatchSetStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteBatchSetFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateBatchSet (Public)
    '
    ' Description: Creates a new Navigator Batch Set
    '
    ' ***************************************************************** '

    Public Function CreateBatchSet(ByVal v_sNavBatchCode As String, Optional ByVal v_vBatchArray(,) As Object = Nothing) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the batch_code parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="batch_code", vValue:=v_sNavBatchCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                ' Add the created_by_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="created_by_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                ' Add the batch_set_id parameter (OUTPUT)
                m_lReturn = .Parameters.Add(sName:="batch_set_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddBatchSetSQL, sSQLName:=ACAddBatchSetName, bStoredProcedure:=ACAddBatchSetStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Set BatchSetID property
                m_lBatchSetID = .Parameters.Item("batch_set_id").Value

            End With

            ' Add records if array passed
            If Information.IsArray(v_vBatchArray) Then

                m_lReturn = AddBatchRecord(v_vBatchArray, v_sNavBatchCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Attach Record to Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddBatchRecord (Public)
    '
    ' Description: Creates a new Record in the Navigator Batch Set
    '
    ' ***************************************************************** '
    'Public Function AddBatchRecord( _
    'ByVal v_lRecordID As Long, _
    'ByVal v_lKeyID As Long, _
    'ByVal v_vKeyValue As Variant) As Long
    Public Function AddBatchRecord(ByRef v_vBatchArray(,) As Object, ByRef v_sNavBatchCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim v_lKeyID As Integer
            Dim r_vResultArray, v_vKeyValue As Object

            ' here use SQL to select the key_ids for this batch...
            With m_oDatabase

                ' clear the db params collection
                .Parameters.Clear()

                ' Add the batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="nav_batch_code", vValue:=v_sNavBatchCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                'Tomo31012002 - Was PMLong

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACGetNavKeysSQL, sSQLName:=ACGetNavKeysName, bStoredProcedure:=ACGetNavKeysStored, vResultArray:=r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            If Not Information.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            For iCount As Integer = v_vBatchArray.GetLowerBound(1) To v_vBatchArray.GetUpperBound(1)

                For icount2 As Integer = r_vResultArray.GetLowerBound(1) To r_vResultArray.GetUpperBound(1)

                    ' set the value of the key_value


                    v_vKeyValue = v_vBatchArray(icount2, iCount)


                    If Strings.Len(CStr(v_vKeyValue)) = 0 Then
                        Exit For
                    End If

                    ' set the key_id
                    v_lKeyID = CInt(r_vResultArray(0, icount2))

                    With m_oDatabase

                        ' Clear the Database Parameters Collection
                        .Parameters.Clear()

                        ' Add the batch_set_id parameter (INPUT)
                        m_lReturn = .Parameters.Add(sName:="batch_set_id", vValue:=CStr(m_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        ' Add the batch_record_id parameter (INPUT)
                        m_lReturn = .Parameters.Add(sName:="batch_record_id", vValue:=(CStr(iCount + 1)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        ' Add the key_id parameter (INPUT)
                        m_lReturn = .Parameters.Add(sName:="key_id", vValue:=CStr(v_lKeyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        ' Add the key_value parameter (INPUT)
                        m_lReturn = .Parameters.Add(sName:="key_value", vValue:=CStr(v_vKeyValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                        ' Add the returned batch_record_id parameter (OUTPUT)
                        m_lReturn = .Parameters.Add(sName:="pmnav_batch_record_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        ' Execute SQL Statement
                        m_lReturn = .SQLAction(sSQL:=ACAddBatchRecordSQL, sSQLName:=ACAddBatchRecordName, bStoredProcedure:=ACAddBatchRecordStored)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Add Navigator Batch Record", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                            Return result
                        End If

                        'Set BatchRecordID property
                        m_lBatchRecordID = .Parameters.Item("pmnav_batch_record_id").Value

                    End With

                Next icount2
            Next iCount

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Create Navigator Batch Record", vApp:=ACApp, vClass:=ACClass, vMethod:="AddBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectNextBatchRecord (Public)
    '
    ' Description: Selects the next record in the Navigator Batch Set
    '
    ' ***************************************************************** '
    Public Function SelectNextBatchRecord(ByRef r_vBatchRecordArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                '        ' Add the batch_record_id parameter (OUTPUT)
                '        m_lReturn& = .Parameters.Add( _
                ''            sName:="pmnav_batch_record_id", _
                ''            vValue:=m_lBatchRecordID&, _
                ''            iDirection:=PMParamOutput, _
                ''            iDataType:=PMLong)

                ' Add the batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="pmnav_batch_set_id", vValue:=CStr(m_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectBatchRecordSQL, sSQLName:=ACSelectBatchRecordName, bStoredProcedure:=ACSelectBatchRecordStored, vResultArray:=r_vBatchRecordArray)

                ' Check for no more records in batch
                'If (CLng(m_oDatabase.Parameters.Item("pmnav_batch_record_id").Value) = -1) Then
                If Not Information.IsArray(r_vBatchRecordArray) Then
                    result = gPMConstants.PMEReturnCode.PMEOF
                    m_lBatchRecordID = 0
                    Return result
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Retrieve Next Navigator Batch Record", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectNextBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                'Set BatchRecordID property
                'm_lBatchRecordID& = CLng(.Parameters.Item("pmnav_batch_record_id").Value)

            End With


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Retrieve Next Navigator Batch Record", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectNextBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteBatchRecord (Public)
    '
    ' Description: Deletes the current record from the Navigator Batch Set
    '
    ' ***************************************************************** '
    Public Function UpdateBatchRecord() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="pmnav_batch_set_id", vValue:=CStr(m_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Add the batch_record_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="pmnav_batch_record_id", vValue:=CStr(m_lBatchRecordID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteBatchRecordSQL, sSQLName:=ACDeleteBatchRecordName, bStoredProcedure:=ACDeleteBatchRecordStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete the Navigator Batch Record", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

                ' Clear the current record id
                m_lBatchRecordID = 0

            End With


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Delete the Navigator Batch Record", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StartBatchSet (Public)
    '
    ' Description: Starts processing the Navigator Batch Set
    '
    ' ***************************************************************** '
    Public Function StartBatchSet() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="batch_set_id", vValue:=CStr(m_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Add the user_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACStartBatchSetSQL, sSQLName:=ACStartBatchSetName, bStoredProcedure:=ACStartBatchSetStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="StartBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Start Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="StartBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBatchSet (Public)
    '
    ' Description: Selects all records in the Navigator Batch Set
    '
    ' ***************************************************************** '
    Public Function GetBatchSet(ByRef r_vBatchArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="batch_set_id", vValue:=CStr(m_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                ' TF020701 - Use PMAllRecords
                '        m_lReturn& = .SQLSelect( _
                ''            sSQL:=ACGetBatchSetSQL, _
                ''            sSQLName:=ACGetBatchSetName, _
                ''            bStoredProcedure:=ACGetBatchSetStored, _
                ''            vResultArray:=r_vBatchArray)
                m_lReturn = .SQLSelect(sSQL:=ACGetBatchSetSQL, sSQLName:=ACGetBatchSetName, bStoredProcedure:=ACGetBatchSetStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vBatchArray)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Retrieve Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Retrieve Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: StopBatchSet (Public)
    '
    ' Description: Stops processing the Navigator Batch Set
    '
    ' ***************************************************************** '
    Public Function StopBatchSet() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add the batch_set_id parameter (INPUT)
                m_lReturn = .Parameters.Add(sName:="batch_set_id", vValue:=CStr(m_lBatchSetID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACStopBatchSetSQL, sSQLName:=ACStopBatchSetName, bStoredProcedure:=ACStopBatchSetStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Stop Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="StopBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return result
                End If

            End With


            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Stop Navigator Batch Set", vApp:=ACApp, vClass:=ACClass, vMethod:="StopBatchSet", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
