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
    ' Date: 24th October 1996
    '
    ' Description: Creatable Business class which contains all the
    '              methods, business rules required to manipulate
    '              a PMSystem.
    '
    ' Edit History:
    ' RFC270398 - Get Licences In Use method added.
    ' RFC250398 - Changed to use Sirius Architecture DSN
    ' RFC250398 - Product Family Property Get Added.
    ' RFC080399 - Update Licence Limit added.
    ' DAK040100 - Changes to licence control.
    ' DAK240100 - System name is not case sensitive - convert to upper
    '             before calculating licence key.
    ' DAK110400 - Licence key should no longer involve the system name
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

    ' Collection of PMSystems (Private)
    Private m_oPMSystems As BPMSYSTEM.PMSystems

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer (Private)
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lError As gPMConstants.PMEReturnCode
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    ' RFC250398 - Product Family Property Get Added.
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusArchitecture
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
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
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

            ' Have we a valid Database Object Reference?

            If (Not Informations.IsNothing(vDatabase)) And (Informations.IsReference(vDatabase)) Then
                ' Yes, so use it.
                m_oDatabase = vDatabase

                ' Do NOT Close Database in Terminate() method
                m_bCloseDatabase = False
            Else
                ' NO, Create new instance of the database object
                m_oDatabase = New dPMDAO.Database()

                ' Open the Database
                ' RDC 27062002 use Comp Serv to open database
                '        m_lError& = m_oDatabase.OpenDatabase(vDSN:=PMSiriusArchitectureDSN)
                m_lError = CType(gPMComponentServices.NewDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusArchitecture, r_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Close Database in Terminate() method
                m_bCloseDatabase = True
            End If

            m_oPMSystems = New BPMSYSTEM.PMSystems()

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
    ' Name: GetValidSystem
    '
    ' Description: Finds the valid System record for the specified
    '              Product Code, checks the Licence Key and returns
    '              the Valid System Record.
    '
    ' DAK110400 - Licence key should no longer involve the system name
    ' PN23693 - adding sAD_OU_Path,sAD_OU_Domain
    ' ***************************************************************** '
    Public Function GetValidSystem(ByRef sProductCode As String, ByRef iSystemID As Integer, ByRef iProductID As Integer, ByRef sSystemName As String, ByRef iDefaultSourceID As Integer, ByRef iHomeCountryID As Integer, ByRef iCurrencyID As Integer, ByRef iLanguageID As Integer, ByRef iLicenceLimit As Integer, ByRef sLicenceKey As String, ByRef iLogLevel As Integer, ByRef iPoolSize As Integer, ByRef vTimestamp As Object, Optional ByRef sAD_OU_Path As String = "", Optional ByRef sAD_OU_Domain As String = "") As Integer

        Dim result As Integer = 0
        Dim sNewLicenceKey As String = ""
        Dim lRecordCount As Integer
        'DAK040100
        Dim sICCS As String = ""
        ' RDC 25102002 for those systems that have a WTS Session ID
        Dim sSystemOnly As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the System Name of the Computer
            m_lError = CType(gPMFunctions.GetSystemName(sSystemName), gPMConstants.PMEReturnCode)

            sSystemOnly = sSystemName

            ' RDC strip WTS SID suffix if it exists
            m_lError = CType(gPMFunctions.GetSystemNameNoSID(sSystemOnly), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the SystemName parameter (INPUT)
            ' RDC 25102002 updated with NoSID system name
            m_lError = m_oDatabase.Parameters.Add(sName:="system_name", vValue:=CStr(sSystemOnly), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Product Code parameter (INPUT)
            m_lError = m_oDatabase.Parameters.Add(sName:="product_code", vValue:=CStr(sProductCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetValidSystemSQL, sSQLName:=ACGetValidSystemName, bStoredProcedure:=ACGetValidSystemStored, lNumberRecords:=0)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' How many records do we have ?

            If lRecordCount <> 1 Then

                ' NOT one, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' RDC 25102002 updated with NoSID system name
            'developer guide no. 111 (Guide)
            m_lError = CType(GetPropertiesFromDB(0, iSystemID, iProductID, sSystemOnly, iDefaultSourceID, iHomeCountryID, iCurrencyID, iLanguageID, iLicenceLimit, sLicenceKey, iLogLevel, iPoolSize, vTimestamp, sAD_OU_Path, sAD_OU_Domain), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK040100
            m_lError = CType(GetICCS(sICCS), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

           Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get a valid system record", vApp:=ACApp, vClass:=ACClass, vMethod:="GetValidSystem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetLicencesInUse
    '
    ' Description: Gets the number of licences in use already.
    ' ***************************************************************** '
    Public Function GetLicencesInUse(ByRef r_iLicencesInUse As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn, lRecordCount As Integer
        Dim vLicencesInUse As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetNoLicencesInUseSQL, sSQLName:=ACGetNoLicencesInUseName, bStoredProcedure:=ACGetNoLicencesInUseStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' How many records do we have ?
            If lRecordCount <> 1 Then

                ' There should be one record, return PMFalse
                Return gPMConstants.PMEReturnCode.PMFalse

            Else

                ' Return the number of licences in use

                'developer guide no. 111 (guide)
                vLicencesInUse = m_oDatabase.Records.Item(0).Fields()("user_count")

                ' Check for Nulls

                If Convert.IsDBNull(vLicencesInUse) Or Informations.IsNothing(vLicencesInUse) Then
                    r_iLicencesInUse = 0
                Else

                    r_iLicencesInUse = CInt(vLicencesInUse)
                End If

            End If

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get a valid system record", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLicencesInUse", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds a single PMSystem directly into the database.
    '              Note: The PMSystem will NOT be added to the collection.
    '
    ' ***************************************************************** '
    Public Function Add(ByRef iSystemID As Integer, ByRef iProductID As Integer, ByRef sSystemName As String, ByRef iDefaultSourceID As Integer, ByRef iHomeCountryID As Integer, ByRef iCurrencyID As Integer, ByRef iLanguageID As Integer, ByRef iLicenceLimit As Integer, ByRef sLicenceKey As String, ByRef iLogLevel As Integer, ByRef iPoolSize As Integer, ByRef vTimestamp As Object) As Integer


        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.PMSystem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new PMSystem
            oPMSystem = New BPMSYSTEM.PMSystem()

            ' Populate PMSystem Attributes
            m_lError = CType(SetProperties(oPMSystem, gPMConstants.PMEComponentAction.PMAdd, iSystemID, iProductID, sSystemName, iDefaultSourceID, iHomeCountryID, iCurrencyID, iLanguageID, iLicenceLimit, sLicenceKey, iLogLevel, iPoolSize, vTimestamp), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the PMSystem to the Database
            m_lError = CType(AddItem(oPMSystem), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the ID of the PMSystem Added
            iSystemID = oPMSystem.SystemID

            oPMSystem = Nothing

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
    ' Description: Gets the required PMSystems and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef vMessageID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oPMSystem As BPMSYSTEM.PMSystem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Check that we do not have any outstanding changes.
            m_lError = CType(Cancel(), gPMConstants.PMEReturnCode)

            ' If changes are outstanding, exit.
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lError
            End If

            ' Clear the Collection
            m_oPMSystems.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' If the supplied keys are not valid, exit

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vMessageID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : MessageID=" & CStr(vMessageID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                Return result

            End If

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the InsuranceFileID parameter (INPUT)

            m_lError = m_oDatabase.Parameters.Add(sName:="message_id", vValue:=CStr(vMessageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lError = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=0)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

                    ' Create New PMSystem
                    oPMSystem = New BPMSYSTEM.PMSystem()

                    m_lError = CType(SetPropertiesFromDB(oPMSystem:=oPMSystem, lRecordNumber:=lSub), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Add PMSystem to collection
                    If m_oPMSystems.Count = 0 Then
                        m_oPMSystems.Add(Nothing)
                    End If
                    m_lError = CType(m_oPMSystems.Add(oNewPMSystem:=oPMSystem), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oPMSystem = Nothing

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
    ' Description: Gets the required PMSystems and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef iSystemID As Integer = 0, Optional ByRef iProductID As Integer = 0, Optional ByRef sSystemName As String = "", Optional ByRef iDefaultSourceID As Integer = 0, Optional ByRef iHomeCountryID As Integer = 0, Optional ByRef iCurrencyID As Integer = 0, Optional ByRef iLanguageID As Integer = 0, Optional ByRef iLicenceLimit As Integer = 0, Optional ByRef sLicenceKey As String = "", Optional ByRef iLogLevel As Integer = 0, Optional ByRef iPoolSize As Integer = 0, Optional ByRef vTimestamp As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.PMSystem
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oPMSystems.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                result = gPMConstants.PMEReturnCode.PMEOF
            End If

            oPMSystem = m_oPMSystems.Item(m_lCurrentRecord)

            ' Get the PMSystem Property Values
            m_lError = CType(GetProperties(oPMSystem, iStatus, iSystemID, iProductID, sSystemName, iDefaultSourceID, iHomeCountryID, iCurrencyID, iLanguageID, iLicenceLimit, sLicenceKey, iLogLevel, iPoolSize, vTimestamp), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMSystem = Nothing

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
    ' Description: Adds the supplied PMSystem into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, ByRef iSystemID As Integer, ByRef iProductID As Integer, ByRef sSystemName As String, ByRef iDefaultSourceID As Integer, ByRef iHomeCountryID As Integer, ByRef iCurrencyID As Integer, ByRef iLanguageID As Integer, ByRef iLicenceLimit As Integer, ByRef sLicenceKey As String, ByRef iLogLevel As Integer, ByRef iPoolSize As Integer, ByRef vTimestamp As Object) As Integer


        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.PMSystem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oPMSystems.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new PMSystem
            oPMSystem = New BPMSYSTEM.PMSystem()

            ' Populate PMSystem Attributes
            m_lError = CType(SetProperties(oPMSystem, gPMConstants.PMEComponentAction.PMAdd, iSystemID, iProductID, sSystemName, iDefaultSourceID, iHomeCountryID, iCurrencyID, iLanguageID, iLicenceLimit, sLicenceKey, iLogLevel, iPoolSize, vTimestamp), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PMSystem to collection
            If m_oPMSystems.Count = 0 Then
                m_oPMSystems.Add(Nothing)
            End If
            m_lError = CType(m_oPMSystems.Add(oNewPMSystem:=oPMSystem), gPMConstants.PMEReturnCode)
            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oPMSystem = Nothing

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
    ' Description: Validates that this action is valid on the PMSystem
    '              specified and updates the PMSystem with the new values.
    '
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vSystemID As Object = Nothing, Optional ByRef vProductID As Object = Nothing, Optional ByRef vSystemName As Object = Nothing, Optional ByRef vDefaultSourceID As Object = Nothing, Optional ByRef vHomeCountryID As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vLicenceLimit As Object = Nothing, Optional ByRef vLicenceKey As Object = Nothing, Optional ByRef vLogLevel As Object = Nothing, Optional ByRef vPoolSize As Object = Nothing, Optional ByRef vTimestamp As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.PMSystem
        Dim iStatus As gPMConstants.PMEComponentAction

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMSystems.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oPMSystem = m_oPMSystems.Item(lRow)

            ' Check the Status of the PMSystem

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oPMSystem.DatabaseStatus
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

            ' Update PMSystem Attributes

            m_lError = CType(SetProperties(oPMSystem, iStatus, vSystemID:=CInt(vSystemID), vProductID:=CInt(vProductID), vSystemName:=CStr(vSystemName), vDefaultSourceID:=CInt(vDefaultSourceID), vHomeCountryID:=CInt(vHomeCountryID), vCurrencyID:=CInt(vCurrencyID), vLanguageID:=CInt(vLanguageID), vLicenceLimit:=CInt(vLicenceLimit), vLicenceKey:=CStr(vLicenceKey), vLogLevel:=CInt(vLogLevel), vPoolSize:=CInt(vPoolSize), vTimestamp:=vTimestamp), gPMConstants.PMEReturnCode)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Release reference to PMSystem
            oPMSystem = Nothing

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
    ' Description: Validate that the specified PMSystem can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oPMSystem As BPMSYSTEM.PMSystem

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oPMSystems.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oPMSystem = m_oPMSystems.Item(lRow)

            ' Check the Status of the PMSystem

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oPMSystem.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oPMSystem.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oPMSystem.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to PMSystem
            oPMSystem = Nothing

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

            If Not (m_oPMSystems Is Nothing) Then
                ' Loop round Collection
                For lSub As Integer = 1 To m_oPMSystems.Count()
                    Select Case m_oPMSystems.Item(lSub).DatabaseStatus
                        Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                            ' Do nothing
                        Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                            result = gPMConstants.PMEReturnCode.PMDataChanged
                            Exit For
                    End Select
                Next lSub
            End If

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
        Dim oPMSystem As BPMSYSTEM.PMSystem
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub As Integer = 1 To m_oPMSystems.Count()
                oPMSystem = m_oPMSystems.Item(lSub)


                Select Case oPMSystem.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lError = CType(AddItem(oPMSystem), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lError = CType(UpdateItem(oPMSystem), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lError = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lError = CType(DeleteItem(oPMSystem), gPMConstants.PMEReturnCode)
                        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oPMSystem = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lError = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Set all items in the collection to PMView
                    For lSub As Integer = 1 To m_oPMSystems.Count()
                        m_oPMSystems.Item(lSub).DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    Next lSub

                Else

                    m_lError = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
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

    'RFC080399 - Update Licence Limit added.
    ' ***************************************************************** '
    ' Name: UpdateLicenceLimit
    '
    ' Description: Updates the PMSystem entry for this system with
    '              the new LicenceLimit/Key if they match.
    '
    ' DAK110400 - Licence key should no longer involve the system name
    ' ***************************************************************** '
    Public Function UpdateLicenceLimit(ByVal v_sProductCode As String, ByVal v_iNewLicenceLimit As Integer, ByVal v_sNewLicenceKey As String) As Integer

        Dim result As Integer = 0

        'Todo: Initilizing the 2 variable, as they are giving error at run time
        Dim sTrueLicenceKey As String = ""
        Dim sSystemName As String = ""
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRecordsAffected As Integer
        'DAK040100
        Dim sICCS As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            v_sProductCode = v_sProductCode.Trim()
            v_sNewLicenceKey = v_sNewLicenceKey.Trim()

            'DAK040100 - GetSystemName moved from below
            ' Get the System Name
            lReturn = CType(gPMFunctions.GetSystemNameNoSID(sSystemName), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DAK040100
            ' Get ICCS
            lReturn = CType(GetICCS(sICCS), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Generate What the Licence Limit Should be for this no of users.
            'DAK240100
            lReturn = CType(GenLicenceKey(sLicenceKey:=sTrueLicenceKey, sProductCode:=v_sProductCode, sICCS:=sICCS, iLicenceLimit:=v_iNewLicenceLimit), gPMConstants.PMEReturnCode)

            '    lReturn& = GenLicenceKey( _
            'sLicenceKey:=sTrueLicenceKey, _
            'sSystemName:=UCase$(Trim$(sSystemName)), _
            'sICCS:=sICCS, _
            'iLicenceLimit:=v_iNewLicenceLimit)

            ' If the Supplied Licence Key is NOT correct EXIT
            If v_sNewLicenceKey <> sTrueLicenceKey Then
                Return gPMConstants.PMEReturnCode.PMInvalidRequest
            End If

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="licence_limit", vValue:=CStr(v_iNewLicenceLimit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.Parameters.Add(sName:="licence_key", vValue:=v_sNewLicenceKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.Parameters.Add(sName:="system_name", vValue:=sSystemName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateLimitSQL, sSQLName:=ACUpdateLimitSQL, bStoredProcedure:=ACUpdateLimitStored, lRecordsAffected:=lRecordsAffected)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lRecordsAffected < 1 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLicenceLimitFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLicenceLimit", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function AddItem(ByRef oPMSystem As BPMSYSTEM.PMSystem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lError = CType(AddInputParam(oPMSystem:=oPMSystem), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add PMSystemID as an OUTPUT param for an insert
        m_lError = m_oDatabase.Parameters.Add(sName:="system_id", vValue:=CStr(oPMSystem.SystemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oPMSystem.SystemID = m_oDatabase.Parameters.Item("system_id").Value

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
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
    Private Function UpdateItem(ByRef oPMSystem As BPMSYSTEM.PMSystem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lError = CType(AddInputParam(oPMSystem:=oPMSystem), gPMConstants.PMEReturnCode)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add PMSystemID as an INPUT param for an update
        m_lError = m_oDatabase.Parameters.Add(sName:="system_id", vValue:=CStr(oPMSystem.SystemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
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
    Private Function DeleteItem(ByRef oPMSystem As BPMSYSTEM.PMSystem) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the InsuranceFileID INPUT parameter
        m_lError = m_oDatabase.Parameters.Add(sName:="system_id", vValue:=CStr(oPMSystem.SystemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' If record wasn't deleted, error
        If lRecordsAffected > 0 Then
            ' Deleted, No action required
        Else
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Private)
    '
    ' Description: Sets the supplied PMSystem properties from a database
    '              record.
    ' ***************************************************************** '
    Private Function SetPropertiesFromDB(ByRef oPMSystem As BPMSYSTEM.PMSystem, ByRef lRecordNumber As Integer) As Integer

        Dim result As Integer = 0
        'developer guide no. 112(Guide)
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oPMSystem

            .SystemID = oFields("system_id")
            .ProductID = oFields("product_id")
            .SystemName = oFields("system_name")
            .DefaultSourceID = oFields("default_source_id")
            .HomeCountryID = oFields("home_country_id")
            .CurrencyID = oFields("currency_id")
            .LanguageID = oFields("language_id")
            .LicenceLimit = oFields("licence_limit")
            .LicenceKey = oFields("licence_key")
            .LogLevel = oFields("log_level")
            .PoolSize = oFields("pool_size")

            'developer guide no. 24(Guide)
            .Timestamp = oFields("timestamp")

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetPropertiesFromDB (Private)
    '
    ' Description: Gets the PMSystem properties from a database
    '              record.
    ' PN23693 - adding sAD_OU_Path,sAD_OU_Domain
    ' ***************************************************************** '
    Private Function GetPropertiesFromDB(ByRef lRecordNumber As Integer, ByRef iSystemID As Integer, ByRef iProductID As Integer, ByRef sSystemName As String, ByRef iDefaultSourceID As Integer, ByRef iHomeCountryID As Integer, ByRef iCurrencyID As Integer, ByRef iLanguageID As Integer, ByRef iLicenceLimit As Integer, ByRef sLicenceKey As String, ByRef iLogLevel As Integer, ByRef iPoolSize As Integer, ByRef vTimestamp As Object, ByRef sAD_OU_Path As String, ByRef sAD_OU_Domain As String) As Integer

        Dim result As Integer = 0
        'developer guide no. 112(Guide)
        Dim oFields As DataRow



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Get Parameters from DB record

        iSystemID = oFields("system_id")
        iProductID = oFields("product_id")
        sSystemName = oFields("system_name")
        iDefaultSourceID = oFields("default_source_id")
        iHomeCountryID = oFields("home_country_id")
        iCurrencyID = oFields("currency_id")
        iLanguageID = oFields("language_id")
        iLicenceLimit = oFields("licence_limit")
        sLicenceKey = oFields("licence_key")
        iLogLevel = oFields("log_level")
        iPoolSize = oFields("pool_size")

        vTimestamp = oFields("timestamp")

        sAD_OU_Path = If(Convert.IsDBNull(oFields("AD_OU_Path")) Or Informations.IsNothing(oFields("AD_OU_Path")), "", oFields("AD_OU_Path"))

        sAD_OU_Domain = If(Convert.IsDBNull(oFields("AD_OU_Domain")) Or Informations.IsNothing(oFields("AD_OU_Domain")), "", oFields("AD_OU_Domain"))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Private)
    '
    ' Description: Sets the supplied PMSystem property values.
    '
    ' ***************************************************************** '
    Private Function SetProperties(ByRef oPMSystem As BPMSYSTEM.PMSystem, ByRef iStatus As Integer, Optional ByRef vSystemID As Integer = 0, Optional ByRef vProductID As Integer = 0, Optional ByRef vSystemName As String = "", Optional ByRef vDefaultSourceID As Integer = 0, Optional ByRef vHomeCountryID As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vLanguageID As Integer = 0, Optional ByRef vLicenceLimit As Integer = 0, Optional ByRef vLicenceKey As String = "", Optional ByRef vLogLevel As Integer = 0, Optional ByRef vPoolSize As Integer = 0, Optional ByRef vTimestamp As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Set Property values.
        With oPMSystem


            If Not Informations.IsNothing(vSystemID) Then
                If .SystemID <> vSystemID Then
                    .SystemID = vSystemID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vProductID) Then
                If .ProductID <> vProductID Then
                    .ProductID = vProductID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vSystemName) Then
                If .SystemName <> vSystemName Then
                    .SystemName = vSystemName
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDefaultSourceID) Then
                If .DefaultSourceID <> vDefaultSourceID Then
                    .DefaultSourceID = vDefaultSourceID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vHomeCountryID) Then
                If .HomeCountryID <> vHomeCountryID Then
                    .HomeCountryID = vHomeCountryID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCurrencyID) Then
                If .CurrencyID <> vCurrencyID Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vLanguageID) Then
                If .LanguageID <> vLanguageID Then
                    .LanguageID = vLanguageID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vLicenceLimit) Then
                If .LicenceLimit <> vLicenceLimit Then
                    .LicenceLimit = vLicenceLimit
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vLicenceKey) Then
                If .LicenceKey <> vLicenceKey Then
                    .LicenceKey = vLicenceKey
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vLogLevel) Then
                If .LogLevel <> vLogLevel Then
                    .LogLevel = vLogLevel
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vPoolSize) Then
                If .PoolSize <> vPoolSize Then
                    .PoolSize = vPoolSize
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vTimestamp) Then


                If Not .Timestamp.Equals(vTimestamp) Then


                    'developer guide no. 24(Guide)
                    .Timestamp = vTimestamp
                    bDataChanged = True
                End If
            End If

            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Private)
    '
    ' Description: Returns the supplied PMSystem property values.
    '
    ' ***************************************************************** '
    Private Function GetProperties(ByRef oPMSystem As BPMSYSTEM.PMSystem, ByRef iStatus As Integer, ByRef iSystemID As Integer, ByRef iProductID As Integer, ByRef sSystemName As String, ByRef iDefaultSourceID As Integer, ByRef iHomeCountryID As Integer, ByRef iCurrencyID As Integer, ByRef iLanguageID As Integer, ByRef iLicenceLimit As Integer, ByRef sLicenceKey As String, ByRef iLogLevel As Integer, ByRef iPoolSize As Integer, ByRef vTimestamp As Object) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oPMSystem

            iSystemID = .SystemID
            iProductID = .ProductID
            sSystemName = .SystemName
            iDefaultSourceID = .DefaultSourceID
            iHomeCountryID = .HomeCountryID
            iCurrencyID = .CurrencyID
            iLanguageID = .LanguageID
            iLicenceLimit = .LicenceLimit
            sLicenceKey = .LicenceKey
            iLogLevel = .LogLevel
            iPoolSize = .PoolSize


            vTimestamp = .Timestamp

            iStatus = .DatabaseStatus

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
    Private Function AddInputParam(ByRef oPMSystem As BPMSYSTEM.PMSystem) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lError = .Parameters.Add(sName:="product_id", vValue:=CStr(oPMSystem.ProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="system_name", vValue:=oPMSystem.SystemName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="default_source_id", vValue:=CStr(oPMSystem.DefaultSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="home_country_id", vValue:=CStr(oPMSystem.HomeCountryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="currency_id", vValue:=CStr(oPMSystem.CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="language_id", vValue:=CStr(oPMSystem.LanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="licence_limit", vValue:=CStr(oPMSystem.LicenceLimit), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="licence_key", vValue:=oPMSystem.LicenceKey, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="log_level", vValue:=CStr(oPMSystem.LogLevel), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lError = .Parameters.Add(sName:="pool_size", vValue:=CStr(oPMSystem.PoolSize), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

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
            m_lError = m_oDatabase.SQLBeginTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
            m_lError = m_oDatabase.SQLCommitTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
            m_lError = m_oDatabase.SQLRollbackTrans()

            If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

    ' ***************************************************************** '
    ' Name: GenLicenceKey
    '
    ' Description: Encrypts the system name, ICCS code and
    '              licence limit to generate the licence key.
    '
    ' DAK110400 - Licence key should no longer involve the system name
    ' ***************************************************************** '
    Private Function GenLicenceKey(ByRef sLicenceKey As String, ByRef sProductCode As String, ByRef sICCS As String, ByRef iLicenceLimit As Integer) As Integer
        'Private Function GenLicenceKey(sLicenceKey As String, _
        'sSystemName As String, _
        'sICCS As String, _
        'iLicenceLimit As Integer) As Long
        Dim result As Integer = 0
        Dim lErrorValue As Integer
        'DAK240100
        Dim sLicence As String = ""




        result = gPMConstants.PMEReturnCode.PMTrue

        If iLicenceLimit = 0 Then
            sLicenceKey = ""
            Return result
        End If

        sLicence = CStr(iLicenceLimit) &
                   sICCS &
                   sProductCode &
                   Strings.ChrW(19).ToString() &
                   Strings.ChrW(8).ToString() &
                   Strings.ChrW(63).ToString() &
                       iLicenceLimit

        '    sLicence = CStr(iLicenceLimit) & _
        'sICCS & _
        'UCase$(Trim$(sSystemName)) & _
        'Chr$(19) & _
        'Chr$(8) & _
        'Chr$(63) & _
        'CStr(iLicenceLimit)

        lErrorValue = bPMFunc.LicenceEncrypt(sLicence:=sLicence, sLicenceKey:=sLicenceKey)

        ' Check for any errors
        If lErrorValue <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Failed to Encrypt Licence Key.
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetICCS
    '
    ' Description:
    '
    ' History: 04/01/2000 DAK - Created.
    '
    ' ***************************************************************** '
    Public Function GetICCS(ByRef sICCS As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the InsuranceFileID parameter (INPUT)
        m_lError = m_oDatabase.Parameters.Add(sName:="ICCS", vValue:=sICCS, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lError = m_oDatabase.SQLAction(sSQL:=ACGetICCSSQL, sSQLName:=ACGetICCSName, bStoredProcedure:=ACGetICCSStored)

        If m_lError <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        sICCS = m_oDatabase.Parameters.Item("ICCS").Value

        Return result

    End Function

    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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


    Public Function GetTransactionsExist(ByRef r_bTransactionsExist As Boolean) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add return parameter

            'developer guide no. 85(Guide) 
            lReturn = m_oDatabase.Parameters.Add("TransactionsExist", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMBoolean)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLAction(sSQL:=ACGetTransactionsExistSQL, sSQLName:=ACGetTransactionsExistName, bStoredProcedure:=ACGetTransactionsExistStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get returned parameter
            r_bTransactionsExist = m_oDatabase.Parameters.Item("TransactionsExist").Value

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get a valid system record", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTransactionsExist", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
