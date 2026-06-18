Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Globalization
'developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("SIRAddress_NET.SIRAddress")> _
Public NotInheritable Class SIRAddress
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRAddress
    '
    ' Date: 21/10/1998
    '
    ' Description: Describes the SIRAddress attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
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
    Private Const ACClass As String = "SIRAddress"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lAddressCnt As Integer
    Private m_lAddressID As Integer
    Private m_sAddress1 As String = "" 'MK 991018 * 60
    Private m_vAddress2 As Object
    Private m_vAddress3 As Object
    Private m_vAddress4 As Object
    Private m_sPostalCode As String = "" 'MK 991018 * 20
    Private m_iCountryID As Integer
    Private m_iCreatedByID As Integer
    Private m_dtDateCreated As Date
    'developer guide no.101
    Private m_vModifiedByID As Object
    Private m_vLastModified As Object
    'DJM 07/05/2002 : New Property added
    Private m_iAddressUsageTypeID As Integer
    'AR20050404 - PN15664
    Private m_bUpdateGlobalAddress As Boolean

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    Public Property AddressCnt() As Integer
        Get

            Return m_lAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lAddressCnt = Value

        End Set
    End Property

    Public Property SourceID() As Integer
        Get

            Return m_iSourceID

        End Get
        Set(ByVal Value As Integer)

            m_iSourceID = Value

        End Set
    End Property

    Public Property AddressID() As Integer
        Get

            Return m_lAddressID

        End Get
        Set(ByVal Value As Integer)

            m_lAddressID = Value

        End Set
    End Property

    Public Property Address1() As String
        Get

            Return m_sAddress1

        End Get
        Set(ByVal Value As String)

            m_sAddress1 = Value

        End Set
    End Property

    Public Property Address2() As Object
        Get

            Return m_vAddress2

        End Get
        Set(ByVal Value As Object)



            m_vAddress2 = Value

        End Set
    End Property

    Public Property Address3() As Object
        Get

            Return m_vAddress3

        End Get
        Set(ByVal Value As Object)



            m_vAddress3 = Value

        End Set
    End Property

    Public Property Address4() As Object
        Get

            Return m_vAddress4

        End Get
        Set(ByVal Value As Object)



            m_vAddress4 = Value

        End Set
    End Property

    Public Property PostalCode() As String
        Get

            Return m_sPostalCode

        End Get
        Set(ByVal Value As String)

            m_sPostalCode = Value

        End Set
    End Property

    Public Property CountryID() As Integer
        Get

            Return m_iCountryID

        End Get
        Set(ByVal Value As Integer)

            m_iCountryID = Value

        End Set
    End Property

    Public Property CreatedByID() As Integer
        Get

            Return m_iCreatedByID

        End Get
        Set(ByVal Value As Integer)

            m_iCreatedByID = Value

        End Set
    End Property

    Public Property DateCreated() As Date
        Get

            Return m_dtDateCreated

        End Get
        Set(ByVal Value As Date)

            m_dtDateCreated = Value

        End Set
    End Property
    'developer guide no.101
    Public Property ModifiedByID() As Object
        Get

            Return m_vModifiedByID

        End Get
        Set(ByVal Value As Object)


            m_vModifiedByID = Value

        End Set
    End Property

    Public Property LastModified() As Object
        Get

            Return m_vLastModified

        End Get
        Set(ByVal Value As Object)



            m_vLastModified = Value

        End Set
    End Property

    'DJM 07/05/2002 : New property added
    Public Property AddressUsageTypeID() As Integer
        Get

            Return m_iAddressUsageTypeID

        End Get
        Set(ByVal Value As Integer)

            m_iAddressUsageTypeID = Value

        End Set
    End Property

    'AR20050404 - PN15664
    Public WriteOnly Property UpdateGlobalAddress() As Boolean
        Set(ByVal Value As Boolean)

            m_bUpdateGlobalAddress = Value

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
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_sCallingAppName = v_sCallingAppName
            m_iLanguageID = v_iLanguageID
            m_iSourceID = v_iSourceID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel
            m_oDatabase = v_vDatabase


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=v_vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as OUTPUT parameters
            m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Primary Key of the record inserted
            m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check to see that the record was updated OK
            If lRecordsAffected > 0 Then
                ' Updated No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRAddress
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As gPMConstants.PMELockMode = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Information.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount = 1 Then
                ' Selected, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set properties
            'developer guide no.111
            m_lReturn = CType(SetPropertiesFromDB(oFields:=m_oDatabase.Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIRAddress properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guide no.21
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                AddressCnt = oFields("address_cnt")
                SourceID = oFields("source_id")
                AddressID = oFields("address_id")
                Address1 = oFields("address1")

                If Convert.IsDBNull(oFields("address2")) Or IsNothing(oFields("address2")) Then

                    Address2 = Nothing
                Else

                    Address2 = oFields("address2")
                End If

                If Convert.IsDBNull(oFields("address3")) Or IsNothing(oFields("address3")) Then

                    Address3 = Nothing
                Else

                    Address3 = oFields("address3")
                End If

                If Convert.IsDBNull(oFields("address4")) Or IsNothing(oFields("address4")) Then

                    Address4 = Nothing
                Else

                    Address4 = oFields("address4")
                End If
                PostalCode = oFields("postal_code")
                CountryID = oFields("country_id")
                CreatedByID = oFields("created_by_id")
                DateCreated = oFields("date_created")

                If Convert.IsDBNull(oFields("modified_by_id")) Or IsNothing(oFields("modified_by_id")) Then

                    ModifiedByID = Nothing
                Else
                    ModifiedByID = oFields("modified_by_id")
                End If

                If Convert.IsDBNull(oFields("last_modified")) Or IsNothing(oFields("last_modified")) Then

                    LastModified = Nothing
                Else

                    LastModified = oFields("last_modified")
                End If

                'DJM 07/05/2002 : Fill new property from DB

                If Convert.IsDBNull(oFields("address_usage_type_id")) Or IsNothing(oFields("address_usage_type_id")) Then
                    m_iAddressUsageTypeID = 4 'Default to correspondence
                Else
                    m_iAddressUsageTypeID = oFields("address_usage_type_id")
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '    With m_oDatabase

        m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(SourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address_id", vValue:=CStr(AddressID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address1", vValue:=Address1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address2", vValue:=Address2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address3", vValue:=Address3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address4", vValue:=Address4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="postal_code", vValue:=PostalCode.ToUpper(), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="country_id", vValue:=CStr(CountryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="created_by_id", vValue:=CStr(CreatedByID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'developer guide no.40
        m_lReturn = m_oDatabase.Parameters.Add(sName:="date_created", vValue:=DateCreated, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If ModifiedByID < 1 Then

            'Developer Guide no 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_by_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="modified_by_id", vValue:=CStr(ModifiedByID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="last_modified", vValue:=LastModified, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'DJM 01/05/2002 : Included new property
        m_lReturn = m_oDatabase.Parameters.Add(sName:="address_usage_type_id", vValue:=CStr(m_iAddressUsageTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'AR20050404 - PN15664
        m_lReturn = m_oDatabase.Parameters.Add(sName:="update_address", vValue:=CStr(IIf(m_bUpdateGlobalAddress, 1, 0)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '    With m_oDatabase

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address_cnt", vValue:=CStr(AddressCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '    With m_oDatabase

        m_lReturn = m_oDatabase.Parameters.Add(sName:="address_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '    With m_oDatabase


        If Not (Convert.IsDBNull(m_oDatabase.Parameters.Item("address_cnt").Value) Or IsNothing(m_oDatabase.Parameters.Item("address_cnt").Value)) Then

            AddressCnt = m_oDatabase.Parameters.Item("address_cnt").Value
        Else
            'TO delete remark AddressCnt = 0
        End If

        '    End With

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise


        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: Check (Public)
    '
    ' Description: Checks to the Database from the Base Details.
    '
    ' Added 991018 MK
    ' ***************************************************************** '
    Public Function Check() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as OUTPUT parameters

            ' To delete remark 991018 MK
            m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCheckSQL, sSQLName:=ACCheckName, bStoredProcedure:=ACCheckStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Primary Key of the record inserted
            m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Check Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Check", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class