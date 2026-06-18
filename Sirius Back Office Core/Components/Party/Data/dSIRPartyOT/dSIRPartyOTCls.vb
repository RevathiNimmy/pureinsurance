Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129 (guide)
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRPartyOT_NET.SIRPartyOT")> _
Public NotInheritable Class SIRPartyOT
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PartyOT
    '
    ' Date: 25/06/1999
    '
    ' Description: Describes the other party attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 06/02/2004
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
    Private Const ACClass As String = "SIRPartyOT"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Integer
    Private m_vLicenseTypeId As Object
    Private m_vLicenseNumber As Object
    Private m_vDateOfBirth As Object
    Private m_vGender As Object
    Private m_vPartyStatus As Object
    Private m_vReferenceNumber As Object
    Private m_vExternalId As Object
    Private m_vRegNumber As Object
    'S4B Claims Enhancements R&D 2005
    Private m_vDatePassedTest As Object
    Private m_vContactName As Object
    Private m_vContactTelephoneNumber As Object
    Private m_vInsurerName As Object
    Private m_vInsurerAddress1 As Object
    Private m_vInsurerAddress2 As Object
    Private m_vInsurerAddress3 As Object
    Private m_vInsurerAddress4 As Object
    Private m_vInsurerPostCode As Object
    Private m_vInsurerTelephoneNumber As Object
    Private m_vInsurerFaxNumber As Object
    Private m_vInsurerContactName As Object
    Private m_vInsurerEmail As Object
    Private m_vInsurerNotes As Object
    Private m_vCompanyNotes As Object
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""
    Private m_vExtraSupplierPartyDetails As Object

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


    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property

    Public Property LicenseTypeId() As Object
        Get
            Return m_vLicenseTypeId
        End Get
        Set(ByVal Value As Object)


            m_vLicenseTypeId = Value
        End Set
    End Property

    Public Property LicenseNumber() As Object
        Get
            Return m_vLicenseNumber
        End Get
        Set(ByVal Value As Object)


            m_vLicenseNumber = Value
        End Set
    End Property

    Public Property DateOfBirth() As Object
        Get
            Return m_vDateOfBirth
        End Get
        Set(ByVal Value As Object)


            m_vDateOfBirth = Value
        End Set
    End Property

    Public Property Gender() As Object
        Get
            Return m_vGender
        End Get
        Set(ByVal Value As Object)


            m_vGender = Value
        End Set
    End Property

    Public Property PartyStatus() As Object
        Get
            Return m_vPartyStatus
        End Get
        Set(ByVal Value As Object)


            m_vPartyStatus = Value
        End Set
    End Property

    Public Property ReferenceNumber() As Object
        Get
            Return m_vReferenceNumber
        End Get
        Set(ByVal Value As Object)


            m_vReferenceNumber = Value
        End Set
    End Property

    Public Property ExternalId() As Object
        Get
            Return m_vExternalId
        End Get
        Set(ByVal Value As Object)


            m_vExternalId = Value
        End Set
    End Property

    Public Property RegNumber() As Object
        Get
            Return m_vRegNumber
        End Get
        Set(ByVal Value As Object)


            m_vRegNumber = Value
        End Set
    End Property

    Public Property DatePassedTest() As Object
        Get
            Return m_vDatePassedTest
        End Get
        Set(ByVal Value As Object)


            m_vDatePassedTest = Value
        End Set
    End Property

    Public Property ContactName() As Object
        Get
            Return m_vContactName
        End Get
        Set(ByVal Value As Object)


            m_vContactName = Value
        End Set
    End Property

    Public Property ContactTelephoneNumber() As Object
        Get
            Return m_vContactTelephoneNumber
        End Get
        Set(ByVal Value As Object)


            m_vContactTelephoneNumber = Value
        End Set
    End Property

    Public Property InsurerName() As Object
        Get
            Return m_vInsurerName
        End Get
        Set(ByVal Value As Object)


            m_vInsurerName = Value
        End Set
    End Property

    Public Property InsurerAddress1() As Object
        Get
            Return m_vInsurerAddress1
        End Get
        Set(ByVal Value As Object)


            m_vInsurerAddress1 = Value
        End Set
    End Property

    Public Property InsurerAddress2() As Object
        Get
            Return m_vInsurerAddress2
        End Get
        Set(ByVal Value As Object)


            m_vInsurerAddress2 = Value
        End Set
    End Property

    Public Property InsurerAddress3() As Object
        Get
            Return m_vInsurerAddress3
        End Get
        Set(ByVal Value As Object)


            m_vInsurerAddress3 = Value
        End Set
    End Property

    Public Property InsurerAddress4() As Object
        Get
            Return m_vInsurerAddress4
        End Get
        Set(ByVal Value As Object)


            m_vInsurerAddress4 = Value
        End Set
    End Property

    Public Property InsurerPostCode() As Object
        Get
            Return m_vInsurerPostCode
        End Get
        Set(ByVal Value As Object)


            m_vInsurerPostCode = Value
        End Set
    End Property

    Public Property InsurerTelephoneNumber() As Object
        Get
            Return m_vInsurerTelephoneNumber
        End Get
        Set(ByVal Value As Object)


            m_vInsurerTelephoneNumber = Value
        End Set
    End Property

    Public Property InsurerFaxNumber() As Object
        Get
            Return m_vInsurerFaxNumber
        End Get
        Set(ByVal Value As Object)


            m_vInsurerFaxNumber = Value
        End Set
    End Property

    Public Property InsurerContactName() As Object
        Get
            Return m_vInsurerContactName
        End Get
        Set(ByVal Value As Object)


            m_vInsurerContactName = Value
        End Set
    End Property

    Public Property InsurerEmail() As Object
        Get
            Return m_vInsurerEmail
        End Get
        Set(ByVal Value As Object)


            m_vInsurerEmail = Value
        End Set
    End Property

    Public Property InsurerNotes() As Object
        Get
            Return m_vInsurerNotes
        End Get
        Set(ByVal Value As Object)


            m_vInsurerNotes = Value
        End Set
    End Property

    Public Property CompanyNotes() As Object
        Get
            Return m_vCompanyNotes
        End Get
        Set(ByVal Value As Object)


            m_vCompanyNotes = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property

    Public Property ExtraSupplierPartyDetails() As Object
        Get
            Return m_vExtraSupplierPartyDetails
        End Get
        Set(ByVal Value As Object)
            m_vExtraSupplierPartyDetails = Value
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

        'SD 05/08/2002
        'Dim oComponentServices As PMServerBusinessCS

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


            '    Set oComponentServices = New PMServerBusinessCS


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            '    Set oComponentServices = Nothing

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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

            With m_oDatabase


                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Get the Primary Key of the record inserted
                m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

                If (Not Informations.IsNothing(m_vExtraSupplierPartyDetails)) Then
                    m_lReturn = .Parameters.Add(sName:="is_TPA_settle_directly", vValue:=m_vExtraSupplierPartyDetails(3), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Else
                    m_lReturn = .Parameters.Add(sName:="is_TPA_settle_directly", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If


                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was updated OK
                If lRecordsAffected > 0 Then
                    ' Updated No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' If record wasn't deleted, error
                If lRecordsAffected > 0 Then
                    ' Deleted, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required other party
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer =0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Default to No Lock if not supplied or not numeric
                Dim dbNumericTemp As Double

                If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                    vLockMode = gPMConstants.PMELockMode.PMNoLock
                End If

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' How many records were selected
                lRecordCount = .Records.Count()

                ' Do we have any records ?
                If lRecordCount = 1 Then
                    ' Selected, No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMNotFound
                End If

                ' Set properties
                'developer guide no.161
                m_lReturn = CType(SetPropertiesFromDB(oFields:= .Records.Item(0).Fields()), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied other party properties from a database
    '              record.
    ' ***************************************************************** '
    'developer guide no. 112 (guide)
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                PartyCnt = oFields("party_cnt")

                If Convert.IsDBNull(oFields("license_type_id")) Or Informations.IsNothing(oFields("license_type_id")) Then

                    LicenseTypeId = Nothing
                Else

                    LicenseTypeId = oFields("license_type_id")
                End If

                If Convert.IsDBNull(oFields("license_number")) Or Informations.IsNothing(oFields("license_number")) Then

                    LicenseNumber = Nothing
                Else

                    LicenseNumber = oFields("license_number")
                End If

                If Convert.IsDBNull(oFields("date_of_birth")) Or Informations.IsNothing(oFields("date_of_birth")) Then

                    DateOfBirth = Nothing
                Else

                    DateOfBirth = oFields("date_of_birth")
                End If

                If Convert.IsDBNull(oFields("gender")) Or Informations.IsNothing(oFields("gender")) Then

                    Gender = Nothing
                Else

                    Gender = oFields("gender")
                End If

                If Convert.IsDBNull(oFields("party_status")) Or Informations.IsNothing(oFields("party_status")) Then

                    PartyStatus = Nothing
                Else

                    PartyStatus = oFields("party_status")
                End If

                If Convert.IsDBNull(oFields("reference_number")) Or Informations.IsNothing(oFields("reference_number")) Then

                    ReferenceNumber = Nothing
                Else

                    ReferenceNumber = oFields("reference_number")
                End If

                If Convert.IsDBNull(oFields("external_id")) Or Informations.IsNothing(oFields("external_id")) Then

                    ExternalId = Nothing
                Else

                    ExternalId = oFields("external_id")
                End If

                If Convert.IsDBNull(oFields("reg_number")) Or Informations.IsNothing(oFields("reg_number")) Then

                    RegNumber = Nothing
                Else

                    RegNumber = oFields("reg_number")
                End If

                'S4B Claims Enhancements R&D 2005


                DatePassedTest = If(Convert.IsDBNull(oFields("date_passed_test")) Or Informations.IsNothing(oFields("date_passed_test")), DBNull.Value, oFields("date_passed_test"))


                ContactName = If(Convert.IsDBNull(oFields("contact_name")) Or Informations.IsNothing(oFields("contact_name")), DBNull.Value, oFields("contact_name"))


                ContactTelephoneNumber = If(Convert.IsDBNull(oFields("contact_telephone_number")) Or Informations.IsNothing(oFields("contact_telephone_number")), DBNull.Value, oFields("contact_telephone_number"))


                InsurerName = If(Convert.IsDBNull(oFields("insurer_name")) Or Informations.IsNothing(oFields("insurer_name")), DBNull.Value, oFields("insurer_name"))


                InsurerAddress1 = If(Convert.IsDBNull(oFields("insurer_address1")) Or Informations.IsNothing(oFields("insurer_address1")), DBNull.Value, oFields("insurer_address1"))


                InsurerAddress2 = If(Convert.IsDBNull(oFields("insurer_address2")) Or Informations.IsNothing(oFields("insurer_address2")), DBNull.Value, oFields("insurer_address2"))


                InsurerAddress3 = If(Convert.IsDBNull(oFields("insurer_address3")) Or Informations.IsNothing(oFields("insurer_address3")), DBNull.Value, oFields("insurer_address3"))


                InsurerAddress4 = If(Convert.IsDBNull(oFields("insurer_address4")) Or Informations.IsNothing(oFields("insurer_address4")), DBNull.Value, oFields("insurer_address4"))


                InsurerPostCode = If(Convert.IsDBNull(oFields("insurer_postcode")) Or Informations.IsNothing(oFields("insurer_postcode")), DBNull.Value, oFields("insurer_postcode"))


                InsurerTelephoneNumber = If(Convert.IsDBNull(oFields("insurer_telephone_number")) Or Informations.IsNothing(oFields("insurer_telephone_number")), DBNull.Value, oFields("insurer_telephone_number"))


                InsurerFaxNumber = If(Convert.IsDBNull(oFields("insurer_fax_number")) Or Informations.IsNothing(oFields("insurer_fax_number")), DBNull.Value, oFields("insurer_fax_number"))


                InsurerContactName = If(Convert.IsDBNull(oFields("insurer_contact_name")) Or Informations.IsNothing(oFields("insurer_contact_name")), DBNull.Value, oFields("insurer_contact_name"))


                InsurerEmail = If(Convert.IsDBNull(oFields("insurer_email")) Or Informations.IsNothing(oFields("insurer_email")), DBNull.Value, oFields("insurer_email"))


                InsurerNotes = If(Convert.IsDBNull(oFields("insurer_notes")) Or Informations.IsNothing(oFields("insurer_notes")), DBNull.Value, oFields("insurer_notes"))


                CompanyNotes = If(Convert.IsDBNull(oFields("company_notes")) Or Informations.IsNothing(oFields("company_notes")), DBNull.Value, oFields("company_notes"))

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        m_lReturn = m_oDatabase.Parameters.Add(sName:="license_type_id", vValue:=LicenseTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="license_number", vValue:=LicenseNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="date_of_birth", vValue:=DateOfBirth, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gender", vValue:=Gender, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_status", vValue:=PartyStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="reference_number", vValue:=ReferenceNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="external_id", vValue:=ExternalId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="reg_number", vValue:=RegNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'S4B Claim Enhancements R&D 2005
        m_lReturn = m_oDatabase.Parameters.Add(sName:="date_passed_test", vValue:=DatePassedTest, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_name", vValue:=ContactName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="contact_telephone_number", vValue:=ContactTelephoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_name", vValue:=InsurerName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_address1", vValue:=InsurerAddress1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_address2", vValue:=InsurerAddress2, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_address3", vValue:=InsurerAddress3, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_address4", vValue:=InsurerAddress4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_postcode", vValue:=InsurerPostCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_telephone_number", vValue:=InsurerTelephoneNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_fax_number", vValue:=InsurerFaxNumber, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_contact_name", vValue:=InsurerContactName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_email", vValue:=InsurerEmail, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_notes", vValue:=InsurerNotes, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="company_notes", vValue:=CompanyNotes, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If m_vExtraSupplierPartyDetails IsNot Nothing AndAlso Informations.IsArray(m_vExtraSupplierPartyDetails) AndAlso UBound(m_vExtraSupplierPartyDetails) >= 2 Then

            m_lReturn = m_oDatabase.Parameters.Add(sName:="active_indicator", vValue:=m_vExtraSupplierPartyDetails(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="after_hours_indicator", vValue:=m_vExtraSupplierPartyDetails(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMBoolean)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="priority_indicator", vValue:=m_vExtraSupplierPartyDetails(2), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

        End If
        m_lReturn = m_oDatabase.Parameters.Add(sName:="UserId", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="UniqueId", vValue:=m_sUniqueId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ScreenHierarchy", vValue:=m_sScreenHierarchy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AddKeyOutputParam) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AddKeyOutputParam() As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'With m_oDatabase
    '
    'm_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(0), idirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '
    'End With
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddKeyOutputParam Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddKeyOutputParam", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            PartyCnt = .Parameters.Item("party_cnt").Value

        End With

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        'Catch excep As System.Exception
        '
        '
        '
        ' Error.
        '
        ' Log Error Message
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

