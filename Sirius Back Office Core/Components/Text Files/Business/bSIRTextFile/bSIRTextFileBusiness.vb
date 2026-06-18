Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports System.IO
'developer guide no. 129
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
    '              a SIRTextFile.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 27/11/2003
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

    ' Collection of SIRTextFiles (Private)
    Private m_oSIRTextFiles As bSIRTextFile.SIRTextFiles

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
    Private m_lEntityTypeId As Integer
    Private m_lEntityCnt As Integer
    Private m_lSlotNumber As Integer
    Private m_lFileNumber As Integer

    Private m_sServer As String = "" 'CT 20/12/00 holds server base document path read from registry

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    Private m_oEvent As bSIREvent.Business
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

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oSIRTextFiles.Count()
                    m_lCurrentRecord = m_oSIRTextFiles.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oSIRTextFiles.Count()

        End Get
    End Property

    Public Property EntityTypeId() As Integer
        Get

            Return m_lEntityTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lEntityTypeId = Value

        End Set
    End Property

    Public Property EntityCnt() As Integer
        Get

            Return m_lEntityCnt

        End Get
        Set(ByVal Value As Integer)

            m_lEntityCnt = Value

        End Set
    End Property

    Public Property SlotNumber() As Integer
        Get

            Return m_lSlotNumber

        End Get
        Set(ByVal Value As Integer)

            m_lSlotNumber = Value

        End Set
    End Property

    Public Property FileNumber() As Integer
        Get

            Return m_lFileNumber

        End Get
        Set(ByVal Value As Integer)

            m_lFileNumber = Value

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

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create SIRTextFiles Collection
            m_oSIRTextFiles = New bSIRTextFile.SIRTextFiles()

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions


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
            Me.disposedValue = True
            If disposing Then
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                m_oSIRTextFiles = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRTextFile.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray As Object, ByRef iLanguageID As Integer, ByRef vResultArray As Object) As Integer

        'Dim oSIRTextFile As bSIRTextFile.SIRTextFile
        'Dim dtEffectiveDate As Date
        '
        '' {* USER DEFINED CODE (Begin) *}
        'Dim vTabArray(3, 1) As Variant
        'Dim vRepeatTypeID As Variant
        'Dim vEventTypeID As Variant
        '' {* USER DEFINED CODE (End) *}
        '
        '    On Error GoTo Err_GetLookupValues
        '
        '    GetLookupValues = PMTrue
        '
        '    ' Reset Result Array
        '    vResultArray = ""
        '    ' Reset Table Array
        '    vTableArray = ""
        '
        '    ' {* USER DEFINED CODE (Begin) *}
        '
        '    ' Setup Lookup Table Names
        '    vTabArray(PMLookupTableName, 0) = PMLookupEventRepeatType
        '    vTabArray(PMLookupTableName, 1) = PMLookupEventType
        '
        '    ' {* USER DEFINED CODE (End) *}
        '
        '    ' Do we have any records
        '    If (m_lCurrentRecord& < 1) Then
        '        ' No, we can only lookup all
        '        iLookupType = PMLookupAll
        '    Else
        '        ' Yes get current record
        '        Set oSIRTextFile = m_oSIRTextFiles.Item(m_lCurrentRecord&)
        '    End If
        '
        '    Select Case iLookupType%
        '      Case PMLookupAll
        '
        '        ' Do not supply a key
        '        vTabArray(PMLookupKey, 0) = ""
        '        vTabArray(PMLookupKey, 1) = ""
        '
        '      Case PMLookupAllEffective
        '
        '        ' Use keys and effective date from current record
        '        ' Note: The keys are not used for the select, but are used by
        '        '       the iterface program to set the list index.
        '        With oSIRTextFile
        '
        '            ' {* USER DEFINED CODE (Begin) *}
        '            m_lReturn& = .GetProperties(iStatus:=PMView)
        '
        '            vTabArray(PMLookupKey, 0) = vRepeatTypeID
        '            vTabArray(PMLookupKey, 1) = vEventTypeID
        '            ' {* USER DEFINED CODE (End) *}
        '
        '        End With
        '
        '      Case PMLookupSingle
        '
        '        ' Set keys from current record
        '        With oSIRTextFile
        '
        '            ' {* USER DEFINED CODE (Begin) *}
        '            m_lReturn& = .GetProperties(iStatus:=PMView)
        '
        '            vTabArray(PMLookupKey, 0) = vRepeatTypeID
        '            vTabArray(PMLookupKey, 1) = vEventTypeID
        '            ' {* USER DEFINED CODE (End) *}
        '
        '        End With
        '
        '    End Select
        '
        '    ' Default Effective Date to current date/time
        '    dtEffectiveDate = Now
        '
        '    ' Release SIRTextFile reference
        '    Set oSIRTextFile = Nothing
        '
        '    ' Get the Lookup items
        '    m_lReturn& = m_oLookup.GetLookupValues( _
        ''        iLookupType:=iLookupType, _
        ''        vTableArray:=vTabArray, _
        ''        iLanguageID:=iLanguageID, _
        ''        dtEffectiveDate:=dtEffectiveDate, _
        ''        vResultArray:=vResultArray)
        '
        '    If (m_lReturn& <> PMTrue) Then
        '        GetLookupValues = PMFalse
        '        Exit Function
        '    End If
        '
        '    ' Return the Table Array
        '    vTableArray = vTabArray

        Dim result As Integer = 0
        Return result




        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRTextFile directly into the database.
    '        Note: The SIRTextFile will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile
        Dim dtDateTime As Date

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRTextFile
            oSIRTextFile = New bSIRTextFile.SIRTextFile()
            m_lReturn = CType(oSIRTextFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            'We're passing in a date, from the PC.  Really we want the date (and time) on the server.
            dtDateTime = DateTime.Now

            ' Populate SIRTextFile Attributes



            m_lReturn = CType(oSIRTextFile.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vEntityTypeId:=vEntityTypeId, vEntityCnt:=vEntityCnt, vSlotNumber:=vSlotNumber, vFileNumber:=vFileNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRTextFile = Nothing
                Return result
            End If

            ' Add the SIRTextFile to the Database
            m_lReturn = CType(oSIRTextFile.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRTextFile = Nothing
                Return result
            End If

            ' Retain the Primary Key of the SIRTextFile Added
            With oSIRTextFile
                FileNumber = .FileNumber
            End With

            vFileNumber = FileNumber

            ' {* USER DEFINED CODE (Begin) *}
            ' {* USER DEFINED CODE (End) *}

            oSIRTextFile = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single SIRTextFile directly from the database.
    '        Note: The SIRTextFile will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new SIRTextFile
            oSIRTextFile = New bSIRTextFile.SIRTextFile()
            m_lReturn = CType(oSIRTextFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set SIRTextFile Primary Key

            m_lReturn = CType(oSIRTextFile.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vEntityTypeId:=vEntityTypeId, vEntityCnt:=vEntityCnt, vSlotNumber:=vSlotNumber, vFileNumber:=vFileNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRTextFile = Nothing
                Return result
            End If

            ' Delete the SIRTextFile from the Database
            m_lReturn = CType(oSIRTextFile.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRTextFile = Nothing
                Return result
            End If

            oSIRTextFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As gPMConstants.PMEReturnCode) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "id", vID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required SIRTextFiles and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0, Optional ByRef vEntityTypeId As Integer = 0, Optional ByRef vEntityCnt As Integer = 0, Optional ByRef vSlotNumber As Integer = 0, Optional ByRef vFileNumber As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        'Developer Guide No. 21
        Dim oFields As DataRow
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile
        Dim sSQL As String = ""
        Dim bDone As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oSIRTextFiles.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Check for Valid Primary Key
            Dim dbNumericTemp2 As Double

            If (Not Information.IsNothing(vEntityTypeId)) And (Not Double.TryParse(CStr(vEntityTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vEntityTypeId=" & vEntityTypeId, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If

            ' Check for Valid parameters
            Dim dbNumericTemp3 As Double

            If (Not Information.IsNothing(vEntityCnt)) And (Not Double.TryParse(CStr(vEntityCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vEntityCnt=" & vEntityCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If

            ' Check for Valid parameters
            Dim dbNumericTemp4 As Double

            If (Not Information.IsNothing(vSlotNumber)) And (Not Double.TryParse(CStr(vSlotNumber), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4)) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vSlotNumber=" & vSlotNumber, vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result
            End If


            If Not Information.IsNothing(vSlotNumber) Then

                ' Create New SIRTextFile
                oSIRTextFile = New bSIRTextFile.SIRTextFile()
                m_lReturn = CType(oSIRTextFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys
                With oSIRTextFile
                    .EntityTypeId = vEntityTypeId
                    .EntityCnt = vEntityCnt
                    .SlotNumber = vSlotNumber

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add SIRTextFile to collection
                m_lReturn = CType(m_oSIRTextFiles.Add(oNewSIRTextFile:=oSIRTextFile), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oSIRTextFile = Nothing

            Else

                ' No Key, Get All Records for the parameters passed
                sSQL = "SELECT slot_number FROM text_file"
                bDone = False


                If Not Information.IsNothing(vEntityTypeId) Then
                    If bDone Then
                        sSQL = sSQL & " AND "
                    Else
                        sSQL = sSQL & " WHERE "
                        bDone = True
                    End If

                    sSQL = sSQL & "entity_type_id = " & CStr(vEntityTypeId)
                End If


                If Not Information.IsNothing(vEntityCnt) Then
                    If bDone Then
                        sSQL = sSQL & " AND "
                    Else
                        sSQL = sSQL & " WHERE "
                        bDone = True
                    End If

                    sSQL = sSQL & "entity_cnt = " & CStr(vEntityCnt)
                End If

                sSQL = sSQL & " ORDER BY slot_number"

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = m_oDatabase.Records.Count()

                ' Do we have any records ?
                If lRecordCount < 1 Then
                    ' No Records, return PMFalse
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Yes, load them into the collection
                For lSub As Integer = 1 To lRecordCount

                    ' Create New
                    oSIRTextFile = New bSIRTextFile.SIRTextFile()
                    'Developer Guide No. 9
                    m_lReturn = oSIRTextFile.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

                    ' Set oFields to refer to one Record
                    'Developer Guide No. 111
                    oFields = m_oDatabase.Records.Item(lSub - 1).Fields()

                    ' Set component primary keys from current record
                    With oSIRTextFile
                        .EntityTypeId = gPMFunctions.NullToLong(oFields("entity_type_id"))
                        .EntityCnt = gPMFunctions.NullToLong(oFields("entity_cnt"))
                        .SlotNumber = gPMFunctions.NullToLong(oFields("slot_number"))
                        .FileNumber = gPMFunctions.NullToLong(oFields("file_number"))

                        m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If
                    End With

                    ' Add SIRTextFile to collection
                    m_lReturn = CType(m_oSIRTextFiles.Add(oNewSIRTextFile:=oSIRTextFile), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oSIRTextFile = Nothing
                Next lSub
            End If

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
    ' Description: Gets the required SIRTextFiles and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oSIRTextFiles.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oSIRTextFile = m_oSIRTextFiles.Item(m_lCurrentRecord)

            ' Get the SIRTextFile Property Values

            m_lReturn = CType(oSIRTextFile.GetProperties(iStatus, vEntityTypeId:=vEntityTypeId, vEntityCnt:=vEntityCnt, vSlotNumber:=vSlotNumber, vFileNumber:=vFileNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oSIRTextFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied SIRTextFile into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oSIRTextFiles.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new SIRTextFile
            oSIRTextFile = New bSIRTextFile.SIRTextFile()
            m_lReturn = CType(oSIRTextFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate SIRTextFile Attributes

            m_lReturn = CType(oSIRTextFile.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vEntityTypeId:=vEntityTypeId, vEntityCnt:=vEntityCnt, vSlotNumber:=vSlotNumber, vFileNumber:=vFileNumber), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRTextFile = Nothing
                Return result
            End If

            ' Add SIRTextFile to collection
            m_lReturn = CType(m_oSIRTextFiles.Add(oNewSIRTextFile:=oSIRTextFile), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRTextFile = Nothing
                Return result
            End If

            oSIRTextFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the SIRTextFile
    '              specified and updates the SIRTextFile with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRTextFiles.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oSIRTextFile = m_oSIRTextFiles.Item(lRow)

            ' Check the Status of the SIRTextFile

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oSIRTextFile.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update SIRTextFile Attributes

            m_lReturn = CType(oSIRTextFile.SetProperties(iStatus:=iStatus, vEntityTypeId:=CInt(vEntityTypeId), vEntityCnt:=CInt(vEntityCnt), vSlotNumber:=CInt(vSlotNumber), vFileNumber:=CInt(vFileNumber)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oSIRTextFile = Nothing
                Return result
            End If

            ' Release reference to SIRTextFile
            oSIRTextFile = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified SIRTextFile can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oSIRTextFiles.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oSIRTextFile = m_oSIRTextFiles.Item(lRow)

            ' Check the Status of the SIRTextFile

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oSIRTextFile.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oSIRTextFile.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oSIRTextFile.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to SIRTextFile
            oSIRTextFile = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oSIRTextFiles.Count()
                Select Case m_oSIRTextFiles.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oSIRTextFile As bSIRTextFile.SIRTextFile
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oSIRTextFiles.Count()
                oSIRTextFile = m_oSIRTextFiles.Item(lSub)


                Select Case oSIRTextFile.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(oSIRTextFile.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oSIRTextFile.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oSIRTextFile.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Retain the Primary Key of the SIRTextFile
            With oSIRTextFile
                FileNumber = .FileNumber
            End With

            ' Release last reference
            oSIRTextFile = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oSIRTextFiles.Count()

                        ' With the item
                        With m_oSIRTextFiles.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oSIRTextFiles.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            'KN (CMG) 04/11/2002
            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SearchAll (Public)
    '
    ' Description: Gets everything as an array
    '
    ' ***************************************************************** '

    Public Function SearchAll(ByRef r_vResultArray(,) As Object, ByVal v_lEntityTypeId As Integer, ByVal v_lEntityCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Restructuring the order of code blocks for better readability

            'policy text
            If v_lEntityTypeId = PMBConst.PMBPolicyTextFile Then

                'MKW050603 PN4121 1.6.9 to 1.8.6 Catchup - START

                'DJM 02/06/2003 : Select all text files for this policy (Insurance_Folder).
                sSQL = ""
                sSQL = sSQL & " SELECT tfd.slot_number," & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " tfd.description," & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " (" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    SELECT MAX(tf.file_number)" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    FROM text_file tf" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    JOIN insurance_file fi2" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    ON fi2.insurance_file_cnt = tf.entity_cnt" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    WHERE fi2.insurance_folder_cnt = fi.insurance_folder_cnt" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND tf.slot_number = tfd.slot_number" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & "    AND tf.entity_type_id = tfd.entity_type_id" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " )," & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " tfd.source_id" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " FROM text_file_description tfd" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " JOIN insurance_file fi" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " ON fi.source_id = tfd.source_id" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " WHERE tfd.entity_type_id = " & CStr(v_lEntityTypeId) & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " AND fi.insurance_file_cnt = " & CStr(v_lEntityCnt) & Strings.Chr(13) & Strings.Chr(10)

                'These comments were present before restructuring.

                'sSQL = sSQL & "insurance_file i " & vbCrLf

                'sSQL = sSQL & "WHERE tf.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''           "AND tfd.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''           "AND i.insurance_file_cnt = " & v_lEntityCnt & vbCrLf & _
                ''           "AND tf.entity_cnt =* i.insurance_file_cnt " & vbCrLf & _
                ''           "AND tfd.source_id = i.source_id " & vbCrLf & _
                ''           "AND tf.slot_number =* tfd.slot_number" & vbCrLf & _
                ''           "ORDER BY tfd.slot_number"

                'MKW050603 PN4121 1.6.9 to 1.8.6 Catchup - END
            Else
                sSQL = ""

                'CT 24/11/00  bring back source specific text files not the global one
                '    sSQL = "SELECT tfd.slot_number," & vbCrLf & _
                ''                  "tfd.description," & vbCrLf & _
                ''                  "tf.file_number" & vbCrLf

                sSQL = " SELECT tfd.slot_number," & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " tfd.description," & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " tf.file_number," & Strings.Chr(13) & Strings.Chr(10) & _
                       " tfd.source_id" & Strings.Chr(13) & Strings.Chr(10)

                'Modifying the inline query to make it compatible with SQL server 2005

                sSQL = sSQL & " FROM text_file_description tfd" & Strings.Chr(13) & Strings.Chr(10)

                'inner join corresponding tables according to the if condition

                'client text
                If v_lEntityTypeId = PMBConst.PMBClientTextFile Then
                    sSQL = sSQL & " INNER JOIN party p" & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & " ON tfd.source_id = p.source_id" & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & " AND tfd.entity_type_id = " & CStr(v_lEntityTypeId) & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & " AND p.party_cnt = " & CStr(v_lEntityCnt) & Strings.Chr(13) & Strings.Chr(10)

                    'claim text
                ElseIf v_lEntityTypeId = PMBConst.PMBClaimTextFile Then
                    sSQL = sSQL & " INNER JOIN claim c" & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & " ON tfd.source_id =" & Strings.Chr(13) & Strings.Chr(10)

                    'DC020201 change from event_insurance_file to insurance_file
                    sSQL = sSQL & "     (SELECT i.source_id" & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & "     FROM insurance_file i" & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & "     WHERE i.insurance_file_cnt = c.policy_id )" & Strings.Chr(13) & Strings.Chr(10)

                    sSQL = sSQL & " AND tfd.entity_type_id = " & CStr(v_lEntityTypeId) & Strings.Chr(13) & Strings.Chr(10)
                    sSQL = sSQL & " AND c.claim_id =" & CStr(v_lEntityCnt) & Strings.Chr(13) & Strings.Chr(10)
                End If

                sSQL = sSQL & " LEFT OUTER JOIN text_file tf" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " ON tf.slot_number = tfd.slot_number" & Strings.Chr(13) & Strings.Chr(10)
                sSQL = sSQL & " AND tf.entity_type_id = tfd.entity_type_id" & Strings.Chr(13) & Strings.Chr(10)

                'add specific conditions based on the v_lEntityTypeId field

                'client text
                If v_lEntityTypeId = PMBConst.PMBClientTextFile Then
                    sSQL = sSQL & " AND tf.entity_cnt = p.party_cnt" & Strings.Chr(13) & Strings.Chr(10)

                    'claim text
                ElseIf v_lEntityTypeId = PMBConst.PMBClaimTextFile Then
                    sSQL = sSQL & " AND tf.entity_cnt = c.claim_id" & Strings.Chr(13) & Strings.Chr(10)
                End If

                'These comments were present before restructuring.

                'CT 24/11/00  bring back source specific text files not the global one
                '    sSQL = sSQL & "FROM text_file tf," & vbCrLf & _
                ''                   "text_file_description tfd" & vbCrLf

                'DC 170101 added claim and party, insurance_file and event_insurance_file

                'CT 24/11/00  bring back source specific text files not the global one
                '    sSQL = sSQL & "WHERE tf.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''                  "AND tfd.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''                  "AND tfd.source_id = " & g_iSourceID & vbCrLf & _
                ''                  "AND tf.entity_cnt = " & v_lEntityCnt & vbCrLf & _
                ''                  "AND tf.slot_number =* tfd.slot_number" & vbCrLf & _
                ''                  "ORDER BY tfd.slot_number"

                'DC 17/01/01 wrong cathy ! to just tie down to insurance_file
                '    sSQL = sSQL & "WHERE tf.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''                  "AND tfd.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''                  "AND i.insurance_file_cnt = " & v_lEntityCnt & vbCrLf & _
                ''                  "AND tfd.source_id = i.source_id " & vbCrLf & _
                ''                  "AND tf.entity_cnt = " & v_lEntityCnt & vbCrLf & _
                ''                  "AND tf.slot_number =* tfd.slot_number" & vbCrLf & _
                ''                  "ORDER BY tfd.slot_number"

                'DC 19/01/01 event_insurance_file not required here
                'sSQL = sSQL & "claim c, event_insurance_file ei " & vbCrLf

                'DC 19/01/01 modified this slightly
                '        sSQL = sSQL & "WHERE tf.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''                  "AND tfd.entity_type_id = " & v_lEntityTypeId & vbCrLf & _
                ''                  "AND c.claim_id = " & v_lEntityCnt & vbCrLf & _
                ''                  "AND ei.insurance_file_cnt = c.policy_id " & vbCrLf & _
                ''                  "AND tf.entity_cnt =* ei.insurance_file_cnt " & vbCrLf & _
                ''                  "AND tfd.source_id = ei.source_id " & vbCrLf & _
                ''                  "AND tf.entity_cnt = " & v_lEntityCnt & vbCrLf & _
                ''                  "AND tf.slot_number =* tfd.slot_number" & vbCrLf & _
                ''                  "ORDER BY tfd.slot_number"

            End If

            'moving order by outside since it is common to all the above conditions
            sSQL = sSQL & " ORDER BY tfd.slot_number" & Strings.Chr(13) & Strings.Chr(10)

            'DC 170101

            ' Execute SQL Statement

            'CT 24/11/00 changed as it sets up SQL string but them ignores it and uses a stored procedure
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL, _
            ''        sSQLName:=ACGetAllDetailsName, _
            ''        bStoredProcedure:=ACGetAllDetailsStored, _
            ''        lNumberRecords:=0, _
            ''        vResultArray:=r_vResultArray)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=False, lNumberRecords:=0, vResultArray:=r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SearchAll Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SearchAll", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DJM 25/02/2004 : Changed to use stored procedure and return if template is editable.
    Public Function GetDocument(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimCnt As Integer, ByVal v_lRiskCodeId As Integer, ByVal v_lRiskGroupId As Integer, ByVal v_lSlotNumber As Integer, ByVal v_lSourceID As Integer, Optional ByRef r_lDocumentTemplateId As Integer = 0, Optional ByRef r_lDocumentTypeId As Integer = 0, Optional ByRef r_bEditable As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lEntityTypeId As gPMConstants.PMEReturnCode
        Dim vArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set default values
            r_lDocumentTypeId = 0
            r_lDocumentTemplateId = 0
            r_bEditable = True

            'Determine type of text file (party, policy or claim, oh my)
            'MKW PN15584 Moved ClaimCnt to first as maintain claim passes in
            '   InsFileCnt aswell.
            If v_lClaimCnt > 0 Then
                lEntityTypeId = PMBConst.PMBClaimTextFile
            ElseIf (v_lInsuranceFileCnt > 0) Then
                lEntityTypeId = PMBConst.PMBPolicyTextFile
            Else
                lEntityTypeId = gPMConstants.PMEReturnCode.PMTrue
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "DocumentTypeId", lEntityTypeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            If v_lRiskCodeId > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCodeId", v_lRiskCodeId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCodeId", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            If v_lRiskGroupId > 0 Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "RiskGroupId", v_lRiskGroupId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            Else

                bPMAddParameter.AddParameterLite(m_oDatabase, "RiskGroupId", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "SlotNumber", v_lSlotNumber, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "SourceID", v_lSourceID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTextFileTemplateSQL, sSQLName:=ACTextFileTemplateName, bStoredProcedure:=ACTextFileTemplateStored, vResultArray:=vArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'If rows have been returned then return the details of the first row.
            If Information.IsArray(vArray) Then
                r_lDocumentTypeId = lEntityTypeId

                r_lDocumentTemplateId = CInt(vArray(0, 0))

                r_bEditable = (CStr(vArray(1, 0)) = "1")
                vArray = Nothing
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDocument Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDocument", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (CheckMandatory) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckMandatory(Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    '
    ' {* USER DEFINED CODE (Begin) *}
    '
    '
    ' {* USER DEFINED CODE (End) *}
    '
    'Return gPMConstants.PMEReturnCode.PMTrue
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
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
    ' Name: GetDetailsForBranchMove
    '
    ' Description:
    ' CT 20/12/00 - created
    ' ***************************************************************** '
    Public Function GetDetailsForBranchMove(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = ""
            sSQL = "SELECT slot_number, file_number "
            sSQL = sSQL & "FROM text_file "
            sSQL = sSQL & " WHERE entity_cnt = " & CStr(EntityCnt)
            sSQL = sSQL & " AND entity_type_id = " & CStr(EntityTypeId)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetBranchMoveDetails", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there are no textfiles meeting criteria, then return notfound
            If Not Information.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get text file details for this client or policy", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetailsForBranchMove", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function
    ' ***************************************************************** '
    ' Name: MoveTextFilesBranch
    '
    ' Description: Moves all documents for 1 client or policy from one branch
    '              directory on the server to another.
    ' CT 20/12/00 - created
    ' ***************************************************************** '
    Public Function MoveTextFilesBranch(ByRef lOldSource As Integer, ByRef lNewSource As Integer) As Integer
        Dim result As Integer = 0
        Dim vTextFiles(,) As Object = Nothing
        Dim sOldServerDir As String = String.Empty
        Dim sNewServerDir As String = String.Empty

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetDetailsForBranchMove(vResultArray:=vTextFiles), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' return true as we just didn't find any text files to copy
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMTrue
            End If


            For i As Integer = vTextFiles.GetLowerBound(1) To vTextFiles.GetUpperBound(1)

                SlotNumber = CInt(vTextFiles(0, i))

                FileNumber = CInt(vTextFiles(1, i))

                m_lReturn = CType(GetTextFilePath(lEntityTypeId:=EntityTypeId, lSourceID:=lOldSource, sPath:=sOldServerDir), gPMConstants.PMEReturnCode)


                m_lReturn = CType(GetAndCreateTextFilePath(lEntityTypeId:=EntityTypeId, lSourceID:=lNewSource, sPath:=sNewServerDir), gPMConstants.PMEReturnCode)
            Next i





            Return result

        Catch excep As System.Exception





            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to move the text files for this client or policy", vApp:=ACApp, vClass:=ACClass, vMethod:="MoveTextFilesBranch", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetServer
    '
    ' Description:
    '
    ' History: CT - 20/12/00 - Created from one by Tom in ipmbtextfile.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (GetServer) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GetServer() As Integer
    '
    'Dim result As Integer = 0
    'Dim sServer As String = ""
    '
    'Dim eRegSettingRoot As gPMConstants.PMERegSettingRoot
    'Dim eRegSettingLevel As gPMConstants.PMERegSettingLevel
    'Dim eProductFamily As gPMConstants.PMEProductFamily
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If m_sServer.Trim() > "" Then
    'Return result
    'End If
    '
    'eRegSettingRoot = gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine
    'eProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    'eRegSettingLevel = gPMConstants.PMERegSettingLevel.pmeRSLClient
    '
    'sServer = ""
    '
    'm_lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=eRegSettingRoot, v_lPMEProductFamily:=eProductFamily, v_lPMERegSettingLevel:=eRegSettingLevel, v_sSettingName:="DocServer", r_sSettingValue:=sServer), gPMConstants.PMEReturnCode)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Unable to get Server from Registry.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
    'Return gPMConstants.PMEReturnCode.PMFalse
    'Else
    'm_sServer = sServer
    'End If
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetServer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetServer", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: GetTextFilePath
    '
    ' Description:
    '
    ' History: 'CT 20/12/00.
    '
    ' ***************************************************************** '
    Private Function GetTextFilePath(ByRef lEntityTypeId As Integer, ByRef lSourceID As Integer, ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim sText As String
        Dim lTemp, lTemp2 As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lFileNumber = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lTemp = m_lFileNumber \ 500

        lTemp2 = m_lFileNumber - (lTemp * 500)

        sText = "Client Text Files"

        If EntityTypeId = 2 Then
            sText = "Policy Text Files"
        End If

        sPath = m_sServer & "\" & sText & "\Slot " & CStr(SlotNumber) & "\" & StringsHelper.Format(lTemp, "000") & StringsHelper.Format(lTemp2, "000") & ".zip"

        Return result

    End Function



    ' ***************************************************************** '
    '
    ' Name: GetAndCreateTextFilePath
    '
    ' Description:
    '
    ' History: 'CT 20/12/00.
    '
    ' ***************************************************************** '
    Private Function GetAndCreateTextFilePath(ByRef lEntityTypeId As Integer, ByRef lSourceID As Integer, ByRef sPath As String) As Integer

        Dim result As Integer = 0
        Dim sTemp, sText As String
        Dim lTemp, lTemp2 As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        If m_lFileNumber = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lTemp = m_lFileNumber \ 500

        lTemp2 = m_lFileNumber - (lTemp * 500)

        sText = "Client Text Files"

        If EntityTypeId = 2 Then
            sText = "Policy Text Files"
        End If

        sPath = m_sServer

        'Make sure the directory's there
        sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(sPath)
        End If

        sPath = sPath & "\" & sText

        'Make sure the directory's there
        sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(sPath)
        End If


        '    sPath = sPath _
        ''        & "\Company " & lSourceID
        'Make sure the directory's there
        sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(sPath)
        End If

        sPath = sPath & "\Slot " & CStr(m_lSlotNumber)

        'Make sure the directory's there
        sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(sPath)
        End If

        sPath = sPath & "\" & StringsHelper.Format(lTemp, "000")

        'Make sure the directory's there
        sTemp = FileSystem.Dir(sPath, FileAttribute.Directory)
        If sTemp = "" Then
            Directory.CreateDirectory(sPath)
        End If

        sPath = sPath & "\" & StringsHelper.Format(lTemp2, "000") & ".zip"

        Return result

    End Function

    'DJM 24/10/2003
    Public Function CopyTextFiles(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "oldinsurance_file_cnt", v_lOldInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "newinsurance_file_cnt", v_lNewInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACTextFilesCopySQL, sSQLName:=ACTextFilesCopyName, bStoredProcedure:=ACTextFilesCopyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyTextFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'DJM 24/10/2003
    Public Function GetTextFiles(ByVal v_lInsuranceFileCnt As Integer, ByRef vTextFiles(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "entity_type_id", 2, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "entity_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACTextFilesSelAllSQL, sSQLName:=ACTextFilesSelAllName, bStoredProcedure:=ACTextFilesSelAllStored, lNumberRecords:=0, vResultArray:=vTextFiles)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTextFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTextFiles", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: CreateEvent (Private)
    '
    ' Description: Create an event record.
    '
    ' Passed event description
    ' ***************************************************************** '
    Public Function CreateEvent(ByRef r_lEventCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_vInsuranceFolderCnt As Object, ByVal v_vInsuranceFileCnt As Object, ByVal v_vClaimCnt As Object, ByVal v_vDocumentCnt As Object, ByVal v_vOldAddressCnt As Object, ByVal v_vNewAddressCnt As Object, ByVal v_vCampaignId As Object, ByVal v_vDocumentTypeId As Object, ByVal v_vReportTypeId As Object, ByVal v_lEventTypeId As Integer, ByVal v_dtEventDate As Date, ByVal v_sDescription As String, Optional ByVal v_lFSAComplaintFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oEvent Is Nothing Then
                m_oEvent = New bSIREvent.Business()

                m_lReturn = m_oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the event object", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If
            'eck100101 added description
            If r_lEventCnt = 0 Then

                m_lReturn = m_oEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserid:=m_iUserID, vEventDate:=v_dtEventDate, vDescription:=v_sDescription, vFSAComplaintFolderCnt:=IIf(v_lFSAComplaintFolderCnt = 0, DBNull.Value, v_lFSAComplaintFolderCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            Else

                'TODO need to be checked with SSP
                'm_lReturn = m_oEvent.DirectUpdate(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate, vFSAComplaintFolderCnt:=IIf(v_lFSAComplaintFolderCnt = 0, DBNull.Value, v_lFSAComplaintFolderCnt))
                m_lReturn = m_oEvent.DirectUpdate(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentTypeId, vReportType:=v_vReportTypeId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=v_dtEventDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

