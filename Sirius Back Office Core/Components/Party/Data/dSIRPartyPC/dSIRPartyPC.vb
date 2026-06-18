Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("SIRPartyPC_NET.SIRPartyPC")>
Public NotInheritable Class SIRPartyPC
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyPC
    '
    ' Date: 04/09/1998
    '
    ' Description: Describes the SIRPartyPC attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 19/05/2004
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
    Private Const ACClass As String = "SIRPartyPC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes
    Private m_lPartyCnt As Integer
    Private m_vPartyTitleCode As String = ""

    'RKS PN13509
    'to avoid the trailing white spaces to save in database table
    'm_sForename changed from  (String * 60) to variable length
    Private m_sForename As String = ""
    'RKS PN13509

    Private m_vInitials As Object
    Private m_vEmploymentStatusCode As Object
    'Developer Guide No. 101
    Private m_vEmployerCnt As Object
    Private m_vEmployerBusiness As String = ""
    Private m_vSecondaryEmploymentStatusC As String = ""
    Private m_vSecondaryEmployerBusiness As String = ""
    Private m_vMaritalStatusCode As Object
    Private m_vNumberOfChildren As Object
    Private m_vNationalityId As Object
    Private m_vCountryOfOriginCode As Object
    Private m_vMailshot As Object 'Mail Preference Service
    Private m_vIsPetOwner As Object
    Private m_vAccommodationTypeCode As Object
    Private m_lPartyLifestyleID As Integer
    Private m_sPartyLifestyleName As String = ""
    Private m_lCategory As Integer
    Private m_dtDateOfBirth As Date
    Private m_sGender As String = ""
    Private m_sOccupationCode As String = ""
    Private m_sSecondaryOccupationCode As String = ""
    Private m_iIsSmoker As Integer
    Private m_vSalutation As String = ""

    Private m_vSource As Object
    Private m_vTPSind As Object 'Telephone Preference Service

    'DD 23/10/2003
    Private m_vEMPSind As Object 'E-Mail Preference Service
    Private m_vTPPassword As Object 'Third Party Password
    'Developer Guide No. 101
    Private m_vIsFeeClient As Object
    Private m_bEvent As Boolean

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

    Public Property PartyTitleCode() As String
        Get

            Return m_vPartyTitleCode

        End Get
        Set(ByVal Value As String)


            m_vPartyTitleCode = CStr(Value)

        End Set
    End Property

    Public Property Forename() As String
        Get

            Return m_sForename

        End Get
        Set(ByVal Value As String)

            m_sForename = Value

        End Set
    End Property

    Public Property Initials() As Object
        Get
            Return m_vInitials

        End Get
        Set(ByVal Value As Object)



            m_vInitials = Value

        End Set
    End Property

    Public Property EmploymentStatusCode() As Object
        Get

            Return m_vEmploymentStatusCode

        End Get
        Set(ByVal Value As Object)



            m_vEmploymentStatusCode = Value

        End Set
    End Property

    Public Property EmployerCnt() As Object
        Get

            Return m_vEmployerCnt

        End Get
        Set(ByVal Value As Object)
            m_vEmployerCnt = Value

        End Set
    End Property

    Public Property EmployerBusiness() As String
        Get

            Return m_vEmployerBusiness

        End Get
        Set(ByVal Value As String)


            m_vEmployerBusiness = CStr(Value)

        End Set
    End Property

    Public Property SecondaryEmploymentStatusC() As String
        Get

            Return m_vSecondaryEmploymentStatusC

        End Get
        Set(ByVal Value As String)


            m_vSecondaryEmploymentStatusC = CStr(Value)

        End Set
    End Property

    Public Property SecondaryEmployerBusiness() As String
        Get

            Return m_vSecondaryEmployerBusiness

        End Get
        Set(ByVal Value As String)


            m_vSecondaryEmployerBusiness = CStr(Value)

        End Set
    End Property

    Public Property MaritalStatusCode() As Object
        Get

            Return m_vMaritalStatusCode

        End Get
        Set(ByVal Value As Object)



            m_vMaritalStatusCode = Value

        End Set
    End Property

    Public Property NumberOfChildren() As Object
        Get

            Return m_vNumberOfChildren

        End Get
        Set(ByVal Value As Object)



            m_vNumberOfChildren = Value

        End Set
    End Property

    Public Property NationalityId() As Object
        Get

            Return m_vNationalityId

        End Get
        Set(ByVal Value As Object)



            m_vNationalityId = Value

        End Set
    End Property

    Public Property CountryOfOriginCode() As Object
        Get

            Return m_vCountryOfOriginCode

        End Get
        Set(ByVal Value As Object)



            m_vCountryOfOriginCode = Value

        End Set
    End Property

    Public Property Mailshot() As Object
        Get

            Return m_vMailshot

        End Get
        Set(ByVal Value As Object)



            m_vMailshot = Value

        End Set
    End Property

    Public Property IsPetOwner() As Object
        Get

            Return m_vIsPetOwner

        End Get
        Set(ByVal Value As Object)



            m_vIsPetOwner = Value

        End Set
    End Property

    Public Property AccommodationTypeCode() As Object
        Get

            Return m_vAccommodationTypeCode

        End Get
        Set(ByVal Value As Object)



            m_vAccommodationTypeCode = Value

        End Set
    End Property

    Public Property PartyLifestyleID() As Integer
        Get

            Return m_lPartyLifestyleID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyLifestyleID = Value

        End Set
    End Property

    Public Property PartyLifestyleName() As String
        Get

            Return m_sPartyLifestyleName

        End Get
        Set(ByVal Value As String)

            m_sPartyLifestyleName = Value

        End Set
    End Property

    Public Property Category() As Integer
        Get

            Return m_lCategory

        End Get
        Set(ByVal Value As Integer)

            m_lCategory = Value

        End Set
    End Property

    Public Property DateOfBirth() As Date
        Get

            Return m_dtDateOfBirth

        End Get
        Set(ByVal Value As Date)

            m_dtDateOfBirth = Value

        End Set
    End Property

    Public Property Gender() As String
        Get

            Return m_sGender

        End Get
        Set(ByVal Value As String)

            m_sGender = Value

        End Set
    End Property

    Public Property OccupationCode() As String
        Get

            Return m_sOccupationCode

        End Get
        Set(ByVal Value As String)

            m_sOccupationCode = Value

        End Set
    End Property

    Public Property SecondaryOccupationCode() As String
        Get

            Return m_sSecondaryOccupationCode

        End Get
        Set(ByVal Value As String)

            m_sSecondaryOccupationCode = Value

        End Set
    End Property

    Public Property IsSmoker() As Integer
        Get

            Return m_iIsSmoker

        End Get
        Set(ByVal Value As Integer)

            m_iIsSmoker = Value

        End Set
    End Property

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

        End Set
    End Property

    ' CTAF 250701
    Public Property Salutation() As String
        Get
            Return m_vSalutation
        End Get
        Set(ByVal Value As String)

            m_vSalutation = CStr(Value)
        End Set
    End Property



    Public Property Source() As Object
        Get
            Return m_vSource
        End Get
        Set(ByVal Value As Object)


            m_vSource = Value
        End Set
    End Property


    Public Property TPSind() As Object
        Get
            Return m_vTPSind
        End Get
        Set(ByVal Value As Object)


            m_vTPSind = Value
        End Set
    End Property



    Public Property EMPSind() As Object
        Get
            Return m_vEMPSind
        End Get
        Set(ByVal Value As Object)


            m_vEMPSind = Value
        End Set
    End Property


    Public Property TPPassword() As Object
        Get
            Return m_vTPPassword
        End Get
        Set(ByVal Value As Object)


            m_vTPPassword = Value
        End Set
    End Property


    'Developer Guide No. 101
    Public Property IsFeeClient() As Object
        Get
            Return m_vIsFeeClient
        End Get
        Set(ByVal Value As Object)
            m_vIsFeeClient = Value
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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


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
    ' Name: AddInsuredLifestyle (Public)
    '
    ' Description: InsuredLifestyles a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddInsuredLifestyle() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as OUTPUT parameters
                m_lReturn = CType(AddKeyOutputLifestyleParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputLifestyleParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACAddLifestyleSQL, sSQLName:=ACAddLifestyleName, bStoredProcedure:=ACAddLifestyleStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was InsuredLifestyled OK
                If lRecordsAffected > 0 Then
                    ' InsuredLifestyled No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsuredLifestyle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddInsuredLifestyle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: UpdateInsuredLifestyle (Public)
    '
    ' Description: InsuredLifestyles a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateInsuredLifestyle() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                ' Clear the Database Parameters Collection
                .Parameters.Clear()

                ' Add PrimaryKey as INPUT parameters
                m_lReturn = CType(AddKeyInputLifestyleParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Add the required INPUT parameters
                m_lReturn = CType(AddInputLifestyleParam(), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = .SQLAction(sSQL:=ACUpdateLifestyleSQL, sSQLName:=ACUpdateLifestyleName, bStoredProcedure:=ACUpdateLifestyleStored, lRecordsAffected:=lRecordsAffected)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Check to see that the record was InsuredLifestyled OK
                If lRecordsAffected > 0 Then
                    ' InsuredLifestyled No action required
                Else
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InsuredLifestyle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuredLifestyle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    ' Description: Selects the required SIRPartyPC
    '
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

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

                If FromEvent Then
                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleEventSQL, sSQLName:=ACSelectSingleEventName, bStoredProcedure:=ACSelectSingleEventStored, bKeepNulls:=True)
                Else
                    ' Execute SQL Statement
                    m_lReturn = .SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)
                End If

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
                'Developer Guide No. 111 (Guide)    
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
    ' Description: Sets the supplied SIRPartyPC properties from a database
    '              record.
    ' ***************************************************************** '
    'Developer Guide No. 112
    Public Function SetPropertiesFromDB(ByRef oFields As DataRow) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields
                'MODIFIED BY ECK 21/04/99
                PartyCnt = gPMFunctions.NullToLong(oFields("party_cnt"))
                PartyTitleCode = gPMFunctions.NullToString(oFields("party_title_code"))
                Forename = gPMFunctions.NullToString(oFields("forename"))

                If Convert.IsDBNull(oFields("initials")) Or Informations.IsNothing(oFields("initials")) Then

                    Initials = Nothing
                Else

                    Initials = oFields("initials")
                End If

                If Convert.IsDBNull(oFields("employment_status_code")) Or Informations.IsNothing(oFields("employment_status_code")) Then

                    EmploymentStatusCode = Nothing
                Else

                    EmploymentStatusCode = oFields("employment_status_code")
                End If

                If Convert.IsDBNull(oFields("employer_cnt")) Or Informations.IsNothing(oFields("employer_cnt")) Then

                    EmployerCnt = Nothing
                Else
                    EmployerCnt = oFields("employer_cnt")
                End If

                If Convert.IsDBNull(oFields("employer_business")) Or Informations.IsNothing(oFields("employer_business")) Then

                    EmployerBusiness = Nothing
                Else
                    EmployerBusiness = oFields("employer_business")
                End If

                If Convert.IsDBNull(oFields("secondary_employment_status_co")) Or Informations.IsNothing(oFields("secondary_employment_status_co")) Then

                    SecondaryEmploymentStatusC = Nothing
                Else
                    SecondaryEmploymentStatusC = oFields("secondary_employment_status_co")
                End If

                If Convert.IsDBNull(oFields("secondary_employer_business")) Or Informations.IsNothing(oFields("secondary_employer_business")) Then

                    SecondaryEmployerBusiness = Nothing
                Else
                    SecondaryEmployerBusiness = oFields("secondary_employer_business")
                End If

                If Convert.IsDBNull(oFields("marital_status_Code")) Or Informations.IsNothing(oFields("marital_status_Code")) Then

                    MaritalStatusCode = Nothing
                Else

                    MaritalStatusCode = oFields("marital_status_code")
                End If

                If Convert.IsDBNull(oFields("number_of_children")) Or Informations.IsNothing(oFields("number_of_children")) Then

                    NumberOfChildren = Nothing
                Else

                    NumberOfChildren = oFields("number_of_children")
                End If

                If Convert.IsDBNull(oFields("nationality_id")) Or Informations.IsNothing(oFields("nationality_id")) Then

                    NationalityId = Nothing
                Else

                    NationalityId = oFields("nationality_id")
                End If

                If Convert.IsDBNull(oFields("country_of_origin_code")) Or Informations.IsNothing(oFields("country_of_origin_code")) Then

                    CountryOfOriginCode = Nothing
                Else

                    CountryOfOriginCode = oFields("country_of_origin_code")
                End If

                If Convert.IsDBNull(oFields("mailshot")) Or Informations.IsNothing(oFields("mailshot")) Then

                    Mailshot = Nothing
                Else

                    Mailshot = oFields("mailshot")
                End If

                If Convert.IsDBNull(oFields("is_pet_owner")) Or Informations.IsNothing(oFields("is_pet_owner")) Then

                    IsPetOwner = Nothing
                Else

                    IsPetOwner = oFields("is_pet_owner")
                End If

                If Convert.IsDBNull(oFields("accommodation_type_code")) Or Informations.IsNothing(oFields("accommodation_type_code")) Then

                    AccommodationTypeCode = Nothing
                Else

                    AccommodationTypeCode = oFields("accommodation_type_code")
                End If

                ' CTAF 250701

                If Convert.IsDBNull(oFields("salutation")) Or Informations.IsNothing(oFields("salutation")) Then

                    Salutation = Nothing
                Else
                    Salutation = oFields("salutation")
                End If


                If Convert.IsDBNull(oFields("source")) Or Informations.IsNothing(oFields("source")) Then

                    Source = Nothing
                Else

                    Source = oFields("source")
                End If


                If Convert.IsDBNull(oFields("tpsind")) Or Informations.IsNothing(oFields("tpsind")) Then

                    TPSind = Nothing
                Else

                    TPSind = oFields("tpsind")
                End If

                'DD 23/10/2003

                If Convert.IsDBNull(oFields("empsind")) Or Informations.IsNothing(oFields("empsind")) Then

                    EMPSind = Nothing
                Else

                    EMPSind = oFields("empsind")
                End If


                If Convert.IsDBNull(oFields("tp_password")) Or Informations.IsNothing(oFields("tp_password")) Then

                    TPPassword = Nothing
                Else

                    TPPassword = oFields("tp_password")
                End If


                If Convert.IsDBNull(oFields("is_fee_client")) Or Informations.IsNothing(oFields("is_fee_client")) Then

                    IsFeeClient = Nothing
                Else
                    IsFeeClient = If(oFields("is_fee_client"), 1, 0)
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyPartyPCToEvent (Public)
    '
    ' Description: Makes a copy of the party PC on the event table.
    '
    ' ***************************************************************** '
    Public Function CopyPartyPCToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPartyPCToEventSQL, sSQLName:=ACCopyPartyPCToEventName, bStoredProcedure:=ACCopyPartyPCToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPartyPCToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPartyPCToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyPartyToEvent (Public)
    '
    ' Description: Makes a copy of the party on the event table.
    '
    ' ***************************************************************** '
    Public Function CopyPartyToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="event_cnt", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyPartyToEventSQL, sSQLName:=ACCopyPartyToEventName, bStoredProcedure:=ACCopyPartyToEventStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPartyToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPartyToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_title_code", vValue:=PartyTitleCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="forename", vValue:=Forename, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="initials", vValue:=Initials, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="employment_status_code", vValue:=EmploymentStatusCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (EmployerCnt < 1) Or Convert.IsDBNull(EmployerCnt) Or Informations.IsNothing(EmployerCnt) Then

                'Developer Guide No. 85 (Guide)
                m_lReturn = .Parameters.Add(sName:="employer_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="employer_cnt", vValue:=CStr(EmployerCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (EmployerBusiness = "") Or Convert.IsDBNull(EmployerBusiness) Or Informations.IsNothing(EmployerBusiness) Then

                'Developer Guide No. 85 (Guide)
                m_lReturn = .Parameters.Add(sName:="employer_business", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                m_lReturn = .Parameters.Add(sName:="employer_business", vValue:=EmployerBusiness, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (SecondaryEmploymentStatusC = "") Or Convert.IsDBNull(SecondaryEmploymentStatusC) Or Informations.IsNothing(SecondaryEmploymentStatusC) Then

                'Developer Guide No. 85 (Guide)
                m_lReturn = .Parameters.Add(sName:="secondary_employment_status_c", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="secondary_employment_status_c", vValue:=SecondaryEmploymentStatusC, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If (SecondaryEmployerBusiness = "") Or Convert.IsDBNull(SecondaryEmployerBusiness) Or Informations.IsNothing(SecondaryEmployerBusiness) Then

                'Developer Guide No. 85
                m_lReturn = .Parameters.Add(sName:="secondary_employer_business", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="secondary_employer_business", vValue:=SecondaryEmployerBusiness, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="marital_status_code", vValue:=MaritalStatusCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="number_of_children", vValue:=NumberOfChildren, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="nationality_id", vValue:=NationalityId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="country_of_origin_code", vValue:=CountryOfOriginCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .Parameters.Add(sName:="mailshot", vValue:=Mailshot, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_pet_owner", vValue:=IsPetOwner, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = .Parameters.Add(sName:="accommodation_type_code", vValue:=AccommodationTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' CTAF 250701

            If (Salutation = "") Or Convert.IsDBNull(Salutation) Or Informations.IsNothing(Salutation) Then

                'Developer Guide No. 85 (Guide)
                m_lReturn = .Parameters.Add(sName:="salutation", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_lReturn = .Parameters.Add(sName:="salutation", vValue:=Salutation, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="source", vValue:=Source, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="tpsind", vValue:=TPSind, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="empsind", vValue:=EMPSind, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="tp_password", vValue:=TPPassword, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_fee_client", vValue:=CStr(IsFeeClient), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddInputLifestyleParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' ***************************************************************** '
    Private Function AddInputLifestyleParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="name", vValue:=PartyLifestyleName, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="category", vValue:=CStr(Category), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'developer guide no. 40
            m_lReturn = .Parameters.Add(sName:="date_of_birth", vValue:=If(DateOfBirth = Date.MinValue, New Date(1899, 12, 29), DateOfBirth), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="gender_code", vValue:=Gender, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="occupation_code", vValue:=OccupationCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="secondary_occupation_code", vValue:=SecondaryOccupationCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .Parameters.Add(sName:="is_smoker", vValue:=CStr(IsSmoker), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        End With

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
    ' Name: AddKeyInputLifestyleParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyInputLifestyleParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="party_lifestyle_id", vValue:=CStr(PartyLifestyleID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputLifestyleParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY Output parameters
    '              required for a Select, Delete or Update.
    '
    ' ***************************************************************** '
    Private Function AddKeyOutputLifestyleParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            m_lReturn = .Parameters.Add(sName:="party_cnt", vValue:=CStr(PartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = .Parameters.Add(sName:="party_lifestyle_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
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

