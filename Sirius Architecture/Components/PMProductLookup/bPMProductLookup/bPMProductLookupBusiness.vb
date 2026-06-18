Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 4th October 1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMProductLookup.
    '
    ' Edit History:
    ' DAK211299 - Allow for retrieving all tables
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

    ' Collection of PMProductLookups (Private)
    Private m_oPMProductLookups As bPMProductLookup.PMProductLookups

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As Integer

    ' Current Task Record Pointer (Private)
    Private m_lTaskCurrentRecord As Integer

    ' Response (Private)
    ' RDC 13062002 gPMLibraries replaced by gPM* BAS modules
    'Private m_lReturn As gPMLibraries.PMEReturnCode
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Caption lookup
    Private m_oCaption As bPMCaption.Business
    'Private m_oCaption As bPMCaption.Business

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture

        End Get
    End Property


    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            If Value < 0 Then
                m_lCurrentRecord = 0
            ElseIf (Value > m_oPMProductLookups.Count()) Then
                m_lCurrentRecord = m_oPMProductLookups.Count()
            Else
                m_lCurrentRecord = Value
            End If

        End Set
    End Property


    Public Property TaskCurrentRecord() As Integer
        Get

            Return m_lTaskCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            If Value < 0 Then
                m_lTaskCurrentRecord = 0
            ElseIf (Value > m_oPMProductLookups.Count()) Then
                m_lTaskCurrentRecord = m_oPMProductLookups.Count()
            Else
                m_lTaskCurrentRecord = Value
            End If

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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        ' RDC 13062002 CompServ replaced with BAS module
        'Dim oComponentServices As PMServerBusinessCS
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


            ' Initialisation Code.

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            '    Set oComponentServices = New PMServerBusinessCS


            If Informations.IsNothing(vDatabase) Then
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase)
                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                '        lReturn = oComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, _
                'r_bNewInstanceCreated:=m_bCloseDatabase, _
                'r_oCheckedDatabase:=m_oDatabase, _
                'v_vDatabase:=vDatabase)

                lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            End If

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't pass a database in, otherwise we'd corrupt the existing one...
            '    lReturn = oComponentServices.CreateBusinessObject(r_oObject:=m_oCaption, _
            'v_sClassName:="bPMCaption.Business", _
            'v_sCallingAppName:=ACApp, _
            'v_sUserName:=m_sUsername, _
            'v_sPassword:=m_sPassword, _
            'v_iUserID:=m_iUserID, _
            'v_iSourceID:=m_iSourceID, _
            'v_iLanguageId:=m_iLanguageID, _
            'v_iCurrencyID:=m_iCurrencyID, _
            'v_iLogLevel:=m_iLogLevel)

            m_oCaption = New bPMCaption.Business
            lReturn = m_oCaption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                '        Set oComponentServices = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    Set oComponentServices = Nothing

            'create empty users collection
            m_oPMProductLookups = New bPMProductLookup.PMProductLookups()
            ' Set Current Record to zero
            m_lCurrentRecord = 0

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
            Me.disposedValue = True
            If disposing Then
                If m_oCaption IsNot Nothing Then
                    m_oCaption.Dispose()
                    m_oCaption = Nothing
                End If
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a single PMProductLookup directly into the database.
    '              Note: The PMProductLookup will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function Add(ByRef lPMProductId As Integer, ByRef sTableName As String, ByRef iPrivilegeLevel As Integer, ByRef sLinkedCaption As String, ByRef sLinkedObjectName As String, ByRef sLinkedClassName As String, ByRef iIsGenericMaintenance As Integer) As Integer


        Dim result As Integer = 0
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMProductLookup
            oPMProductLookup = New bPMProductLookup.PMProductLookup()

            ' Populate PMProductLookup Attributes
            m_lReturn = CType(SetProperties(oPMProductLookup:=oPMProductLookup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMProductId:=lPMProductId, vTableName:=sTableName, vPrivilegeLevel:=iPrivilegeLevel, vLinkedCaption:=sLinkedCaption, vLinkedObjectName:=sLinkedObjectName, vLinkedClassName:=sLinkedClassName, vIsGenericMaintenance:=iIsGenericMaintenance), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Add the PMProductLookup to the Database
            m_lReturn = CType(AddItem(oPMProductLookup), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            oPMProductLookup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required PMProductLookups and populate the
    ' Collection for the PMProductId and Table Name if supplied,
    ' else returns all of the groups
    '
    ' ***************************************************************** '
    'DAK211299
    Public Function GetDetails(Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.

            m_lReturn = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Clear the Collection
            m_oPMProductLookups.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'DAK211299

            If Informations.IsNothing(vPMProductId) Then

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetEverySQL, sSQLName:=ACGetEveryName, bStoredProcedure:=ACGetEveryStored, lNumberRecords:=0)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

            Else
                ' Add the UserId parameter (INPUT)
                'DAK211299

                m_lReturn = m_oDatabase.Parameters.Add(sName:="pmproduct_id", vValue:=CStr(CInt(vPMProductId)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                'if the table name was supplied

                If Informations.IsNothing(vTableName) Then
                    ' select all of the PMProductLookup records for a product
                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                Else

                    ' Add the table name parameter (INPUT)

                    ' CTAF 151200 - Changed DataType from PMDate to PMString

                    m_lReturn = m_oDatabase.Parameters.Add(sName:="table_name", vValue:=CStr(vTableName), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New PMProductLookup
                    oPMProductLookup = New bPMProductLookup.PMProductLookup()
                    'developer guide no.162
                    m_lReturn = CType(SetPropertiesFromDB(oPMProductLookup:=oPMProductLookup, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    ' Add PMProductLookup to collection
                    m_lReturn = CType(m_oPMProductLookups.Add(oNewPMProductLookup:=oPMProductLookup), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    oPMProductLookup = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required PMProductLookup tables and populates
    '              the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMProductLookups.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                Return gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMProductLookup = m_oPMProductLookups.Item(m_lCurrentRecord - 1)

            ' Get the PMProductLookup Property Values







            'developer guide no.98
            m_lReturn = CType(GetProperties(oPMProductLookup:=oPMProductLookup, vStatus:=iStatus, vPMProductId:=vPMProductId, vTableName:=vTableName, vPrivilegeLevel:=vPrivilegeLevel, vLinkedCaption:=vLinkedCaption, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vIsGenericMaintenance:=vIsGenericMaintenance), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            oPMProductLookup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied PMProductLookup into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lPMProductId As Integer, ByRef sTableName As String, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup
        Dim sKey As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sKey = CStr(lPMProductId) & sTableName
            oPMProductLookup = m_oPMProductLookups.Item(CInt(sKey))
            If Not (oPMProductLookup Is Nothing) Then
                oPMProductLookup = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMProductLookup
            oPMProductLookup = New bPMProductLookup.PMProductLookup()

            ' Populate PMProductLookup Attributes





            'developer guide no.98
            m_lReturn = CType(SetProperties(oPMProductLookup:=oPMProductLookup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMProductId:=lPMProductId, vTableName:=sTableName, vPrivilegeLevel:=vPrivilegeLevel, vLinkedCaption:=vLinkedCaption, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vIsGenericMaintenance:=vIsGenericMaintenance), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Add PMProductLookup to collection
            m_lReturn = CType(m_oPMProductLookups.Add(oNewPMProductLookup:=oPMProductLookup), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            oPMProductLookup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the PMProductLookup
    '              specified and updates the PMProductLookup with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lPMProductId As Integer, ByRef sTableName As String, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup
        Dim iStatus As Integer
        Dim sKey As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get a reference to the row to Edit
            sKey = CStr(lPMProductId) & sTableName

            oPMProductLookup = m_oPMProductLookups.Item(CInt(sKey))
            If oPMProductLookup Is Nothing Then
                Throw New Exception()
            End If

            ' Check the Status of the PMProductLookup

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMProductLookup.DatabaseStatus
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

            ' Update PMProductLookup Attributes





            'developer guide no.98
            m_lReturn = CType(SetProperties(oPMProductLookup:=oPMProductLookup, iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPMProductId:=lPMProductId, vTableName:=sTableName, vPrivilegeLevel:=vPrivilegeLevel, vLinkedCaption:=vLinkedCaption, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vIsGenericMaintenance:=vIsGenericMaintenance), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Release reference to PMProductLookup
            oPMProductLookup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified PMProductLookup can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lPMProductId As Integer, ByVal sTableName As String) As Integer
        Dim result As Integer = 0
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup
        Dim sKey As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sKey = CStr(lPMProductId) & sTableName

            ' Get a reference to the lookup
            oPMProductLookup = m_oPMProductLookups.Item(CInt(sKey))
            If oPMProductLookup Is Nothing Then
                Throw New Exception()
            End If

            ' Check the Status of the PMProductLookup

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMProductLookup.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMProductLookup.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                'set database status to update
                oPMProductLookup.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to PMProductLookup
            oPMProductLookup = Nothing

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            For lSub As Integer = 0 To m_oPMProductLookups.Count() - 1
                Select Case m_oPMProductLookups.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
        Dim oPMProductLookup As bPMProductLookup.PMProductLookup
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 0 To m_oPMProductLookups.Count() - 1
                oPMProductLookup = m_oPMProductLookups.Item(lSub)


                Select Case oPMProductLookup.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception()
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(AddItem(oPMProductLookup), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception()
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(UpdateItem(oPMProductLookup), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete
                        m_lReturn = CType(DeleteItem(oPMProductLookup), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Throw New Exception()
                        End If

                End Select

            Next lSub

            ' Release last reference
            oPMProductLookup = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 0 To m_oPMProductLookups.Count() - 1
                        m_oPMProductLookups.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Next lSub

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oPMProductLookup:=oPMProductLookup), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oPMProductLookup:=oPMProductLookup), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' Add key fields as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmproduct_id", vValue:=CStr(oPMProductLookup.PMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="table_name", vValue:=oPMProductLookup.TableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' If the record was NOT UpdateItemd reselect it to see if the data
        ' has been changed or the record deleted

        If lRecordsAffected > 0 Then

            ' UpdatedItem, No action required

        Else

            result = gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup) As Integer
        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add key fields as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="pmproduct_id", vValue:=CStr(oPMProductLookup.PMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="table_name", vValue:=oPMProductLookup.TableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Throw New Exception()
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMProductLookup properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'Developer Guide No. 112 (Guide)
        Dim oFields As DataRow
        Dim sLinkedCaption As String = ""
        Dim lLinkedCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMProductLookup

            .PMProductID = oFields("pmproduct_id")

            .TableName = oFields("lookup_table_name")

            .PrivilegeLevel = oFields("edit_privilege_level")


            If Convert.IsDBNull(oFields("linked_caption_id")) Or Informations.IsNothing(oFields("linked_caption_id")) Then
                lLinkedCaptionID = 0
            Else
                lLinkedCaptionID = oFields("linked_caption_id")
            End If

            If lLinkedCaptionID <> 0 Then


                m_lReturn = m_oCaption.GetCaptionDesc(v_lCaptionId:=lLinkedCaptionID, r_sCaption:=sLinkedCaption)
                .LinkedCaption = sLinkedCaption

                .LinkedObjectName = oFields("linked_object_name")

                .LinkedClassName = oFields("linked_class_name")

            End If

            .IsGenericMaintenance = oFields("is_generic_maintenance")

        End With

        oFields = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMProductLookup property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function SetProperties(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup, ByRef iStatus As Integer, Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim sDescription As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        'decide whether call is an add or an edit
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            'decide whether all of the mandatory values have been supplied
            m_lReturn = CType(MandatoryParameterCheck(vPMProductId:=vPMProductId, vTableName:=vTableName, vPrivilegeLevel:=vPrivilegeLevel, vLinkedCaption:=vLinkedCaption, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vIsGenericMaintenance:=vIsGenericMaintenance), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            'supply defaults for any missing parameters
            m_lReturn = CType(DefaultMissingParameters(vPMProductId:=vPMProductId, vTableName:=vTableName, vPrivilegeLevel:=vPrivilegeLevel, vLinkedCaption:=vLinkedCaption, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vIsGenericMaintenance:=vIsGenericMaintenance), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

        End If

        'check whether the values in the parameters are valid
        m_lReturn = CType(ValidateParameters(vPMProductId:=vPMProductId, vTableName:=vTableName, vPrivilegeLevel:=vPrivilegeLevel, vLinkedCaption:=vLinkedCaption, vLinkedObjectName:=vLinkedObjectName, vLinkedClassName:=vLinkedClassName, vIsGenericMaintenance:=vIsGenericMaintenance), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        ' Set Property values.
        With oPMProductLookup

            If Not Informations.IsNothing(vPMProductId) Then
                .PMProductID = vPMProductId
            End If

            If Not Informations.IsNothing(vTableName) Then
                .TableName = vTableName
            End If

            If Not Informations.IsNothing(vPrivilegeLevel) Then
                .PrivilegeLevel = vPrivilegeLevel
            End If

            If Not Informations.IsNothing(vLinkedCaption) Then
                .LinkedCaption = vLinkedCaption
            End If

            If Not Informations.IsNothing(vLinkedObjectName) Then
                .LinkedObjectName = vLinkedObjectName
            End If

            If Not Informations.IsNothing(vLinkedClassName) Then
                .LinkedClassName = vLinkedClassName
            End If

            If Not Informations.IsNothing(vIsGenericMaintenance) Then
                .IsGenericMaintenance = vIsGenericMaintenance
            End If

            .DatabaseStatus = iStatus

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMProductLookup property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function GetProperties(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup, ByRef vStatus As Object, Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMProductLookup


            If Not Informations.IsNothing(vPMProductId) Then
                vPMProductId = .PMProductID
            End If


            If Not Informations.IsNothing(vTableName) Then
                vTableName = .TableName
            End If


            If Not Informations.IsNothing(vPrivilegeLevel) Then
                vPrivilegeLevel = .PrivilegeLevel
            End If


            If Not Informations.IsNothing(vLinkedCaption) Then
                vLinkedCaption = .LinkedCaption
            End If


            If Not Informations.IsNothing(vLinkedObjectName) Then
                vLinkedObjectName = .LinkedObjectName
            End If


            If Not Informations.IsNothing(vLinkedClassName) Then
                vLinkedClassName = .LinkedClassName
            End If


            If Not Informations.IsNothing(vIsGenericMaintenance) Then
                vIsGenericMaintenance = .IsGenericMaintenance
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam(ByRef oPMProductLookup As bPMProductLookup.PMProductLookup) As Integer
        Dim result As Integer = 0
        Dim sLinkedCaption As String = ""
        Dim lLinkedCaptionID As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="pmproduct_id", vValue:=CStr(oPMProductLookup.PMProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            m_lReturn = .Parameters.Add(sName:="table_name", vValue:=oPMProductLookup.TableName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            m_lReturn = .Parameters.Add(sName:="privilege_level", vValue:=CStr(oPMProductLookup.PrivilegeLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            If oPMProductLookup.LinkedCaption = "" Then


                m_lReturn = .Parameters.Add(sName:="linked_caption_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If


                m_lReturn = .Parameters.Add(sName:="linked_object_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If


                m_lReturn = .Parameters.Add(sName:="linked_class_name", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

            Else

                sLinkedCaption = oPMProductLookup.LinkedCaption


                m_lReturn = m_oCaption.GetCaptionID(v_sCaption:=sLinkedCaption, r_lCaptionId:=lLinkedCaptionID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                m_lReturn = .Parameters.Add(sName:="linked_caption_id", vValue:=CStr(lLinkedCaptionID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                m_lReturn = .Parameters.Add(sName:="linked_object_name", vValue:=oPMProductLookup.LinkedObjectName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

                m_lReturn = .Parameters.Add(sName:="linked_class_name", vValue:=oPMProductLookup.LinkedClassName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Throw New Exception()
                End If

            End If

            m_lReturn = .Parameters.Add(sName:="is_generic_maintenance", vValue:=CStr(oPMProductLookup.IsGenericMaintenance), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: ValidateParameters (Private)
    '
    ' Description: Checks that all paramaters are valid for the datatype
    '
    ' ***************************************************************** '
    Private Function ValidateParameters(Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Informations.IsNothing(vPMProductId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPMProductId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateParameters vPMProductLookupId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateParameters", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If


        If Not Informations.IsNothing(vTableName) Then

        End If


        If Not Informations.IsNothing(vPrivilegeLevel) Then

        End If


        If Not Informations.IsNothing(vLinkedCaption) Then

        End If


        If Not Informations.IsNothing(vLinkedObjectName) Then

        End If


        If Not Informations.IsNothing(vLinkedClassName) Then

        End If


        If Not Informations.IsNothing(vIsGenericMaintenance) Then

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DefaultMissingProperties (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied and sets defaults for the non mandatory ones
    '
    ' ***************************************************************** '
    Private Function DefaultMissingParameters(Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer

        Dim result As Integer = 0



        'set defaults for any properties which have not been supplied

        Return gPMConstants.PMEReturnCode.PMTrue

    End Function

    ' ***************************************************************** '
    ' Name: MandatoryParameterCheck (Private)
    '
    ' Description: Checks that all mandatory paramaters have been
    '               supplied
    '
    ' ***************************************************************** '

    Private Function MandatoryParameterCheck(Optional ByRef vPMProductId As Object = Nothing, Optional ByRef vTableName As Object = Nothing, Optional ByRef vPrivilegeLevel As Object = Nothing, Optional ByRef vLinkedCaption As Object = Nothing, Optional ByRef vLinkedObjectName As Object = Nothing, Optional ByRef vLinkedClassName As Object = Nothing, Optional ByRef vIsGenericMaintenance As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'check that all mandatory parameters have been supplied

        If Informations.IsNothing(vPMProductId) Or Informations.IsNothing(vTableName) Or Informations.IsNothing(vPrivilegeLevel) Or Informations.IsNothing(vIsGenericMaintenance) Then

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Mandatory Property Was Not Supplied", vApp:=ACApp, vClass:=ACClass, vMethod:="MandatoryParameterCheck", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return gPMConstants.PMEReturnCode.PMFalse
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
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
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
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
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
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception



            ' Error.
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

End Class
