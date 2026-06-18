Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no.129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyOT
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyOT
    '
    ' Date: 08/10/1998
    '
    ' Description: Describes the SIRPartyOT attributes.
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
    Private Const ACClass As String = "SIRPartyOT"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyOT As dSIRPartyOT.SIRPartyOT ' was dSIRPartyOT.SIRPartyOT

    ' Instance of the Core SIRParty object
    Private m_bSIRParty As bSIRParty.Business

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lAddressCnt As Integer
    Private m_lPartyCnt As Integer

    Private m_vSupplierBusinesses As Object


    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

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

    Public Property AddressCnt() As Integer
        Get

            Return m_lAddressCnt

        End Get
        Set(ByVal Value As Integer)

            m_lAddressCnt = Value

        End Set
    End Property

    Public ReadOnly Property bSIRParty() As Object
        Get

            Return m_bSIRParty

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


            ' Create instance of data class
            m_dSIRPartyOT = New dSIRPartyOT.SIRPartyOT()
            '    Set m_dSIRPartyOT = New dSIRPartyOT.SIRPartyOT

            m_lReturn = m_dSIRPartyOT.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            ' Create Core Business Object
            '    Set m_bSIRParty = New bSIRParty.Business




            m_bSIRParty = New bSIRParty.Business
            m_lReturn = m_bSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)


            '    m_lReturn& = m_bSIRParty.Initialise( _
            'sUsername:=m_sUsername, _
            'sPassword:=m_sPassword, _
            'iUserID:=m_iUserID, _
            'iSourceID:=m_iSourceID, _
            'iLanguageID:=m_iLanguageID, _
            'iCurrencyID:=m_iCurrencyID, _
            'iLogLevel:=m_iLogLevel, _
            'sCallingAppName:=ACApp, _
            'vDatabase:=vDatabase)

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
            Me.disposedValue = True
            If disposing Then
                If m_bSIRParty IsNot Nothing Then
                    m_bSIRParty.Dispose()
                End If
                m_bSIRParty = Nothing
            End If

        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyOT.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vPartyStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vExternalId As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no.98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vPartyStatus:=vPartyStatus, vReferenceNumber:=vReferenceNumber, vExternalId:=vExternalId, vRegNumber:=vRegNumber, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIRPartyOT property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vPartyStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vExternalId As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing, Optional ByVal vExtraSupplierPartyDetails As Object = "", Optional ByVal vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vPartyStatus:=vPartyStatus, vReferenceNumber:=vReferenceNumber, vExternalId:=vExternalId, vRegNumber:=vRegNumber, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vLicenseTypeId:=vLicenseTypeId, vLicenseNumber:=vLicenseNumber, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vPartyStatus:=vPartyStatus, vReferenceNumber:=vReferenceNumber, vExternalId:=vExternalId, vRegNumber:=vRegNumber, vDatePassedTest:=vDatePassedTest, vContactName:=vContactName, vContactTelephoneNumber:=vContactTelephoneNumber, vInsurerName:=vInsurerName, vInsurerAddress1:=vInsurerAddress1, vInsurerAddress2:=vInsurerAddress2, vInsurerAddress3:=vInsurerAddress3, vInsurerAddress4:=vInsurerAddress4, vInsurerPostcode:=vInsurerPostcode, vInsurerTelephoneNumber:=vInsurerTelephoneNumber, vInsurerFaxNumber:=vInsurerFaxNumber, vInsurerContactName:=vInsurerContactName, vInsurerEmail:=vInsurerEmail, vInsurerNotes:=vInsurerNotes, vCompanyNotes:=vCompanyNotes), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyOT



                If (Not Informations.IsNothing(vPartyCnt)) And (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If




                If (Not Informations.IsNothing(vLicenseTypeId)) And (Not Object.Equals(vLicenseTypeId, Nothing)) Then


                    'developer guide no.24
                    .LicenseTypeId = vLicenseTypeId
                End If




                If (Not Informations.IsNothing(vLicenseNumber)) And (Not Object.Equals(vLicenseNumber, Nothing)) Then


                    'developer guide no.24
                    .LicenseNumber = vLicenseNumber
                End If



                If (Not Informations.IsNothing(vDateOfBirth)) And (Not Object.Equals(vDateOfBirth, Nothing)) Then


                    'developer guide no.24
                    .DateOfBirth = vDateOfBirth
                End If



                If (Not Informations.IsNothing(vGender)) And (Not Object.Equals(vGender, Nothing)) Then


                    'developer guide no.24
                    .Gender = vGender
                End If



                If (Not Informations.IsNothing(vPartyStatus)) And (Not Object.Equals(vPartyStatus, Nothing)) Then


                    'developer guide no.24
                    .PartyStatus = vPartyStatus
                End If



                If (Not Informations.IsNothing(vReferenceNumber)) And (Not Object.Equals(vReferenceNumber, Nothing)) Then


                    'developer guide no.24
                    .ReferenceNumber = vReferenceNumber
                End If



                If (Not Informations.IsNothing(vExternalId)) And (Not Object.Equals(vExternalId, Nothing)) Then


                    'developer guide no.24
                    .ExternalId = vExternalId
                End If



                If (Not Informations.IsNothing(vRegNumber)) And (Not Object.Equals(vRegNumber, Nothing)) Then


                    'developer guide no.24
                    .RegNumber = vRegNumber
                End If



                If (Not Informations.IsNothing(vDatePassedTest)) And (Not Object.Equals(vDatePassedTest, Nothing)) Then


                    'developer guide no.24
                    .DatePassedTest = vDatePassedTest
                End If



                If (Not Informations.IsNothing(vContactName)) And (Not Object.Equals(vContactName, Nothing)) Then


                    'developer guide no.24
                    .ContactName = vContactName
                End If



                If (Not Informations.IsNothing(vContactTelephoneNumber)) And (Not Object.Equals(vContactTelephoneNumber, Nothing)) Then


                    'developer guide no.24
                    .ContactTelephoneNumber = vContactTelephoneNumber
                End If



                If (Not Informations.IsNothing(vInsurerName)) And (Not Object.Equals(vInsurerName, Nothing)) Then


                    'developer guide no.24
                    .InsurerName = vInsurerName
                End If



                If (Not Informations.IsNothing(vInsurerAddress1)) And (Not Object.Equals(vInsurerAddress1, Nothing)) Then


                    'developer guide no.24
                    .InsurerAddress1 = vInsurerAddress1
                End If



                If (Not Informations.IsNothing(vInsurerAddress2)) And (Not Object.Equals(vInsurerAddress2, Nothing)) Then


                    'developer guide no.24
                    .InsurerAddress2 = vInsurerAddress2
                End If



                If (Not Informations.IsNothing(vInsurerAddress3)) And (Not Object.Equals(vInsurerAddress3, Nothing)) Then


                    'developer guide no.24
                    .InsurerAddress3 = vInsurerAddress3
                End If



                If (Not Informations.IsNothing(vInsurerAddress4)) And (Not Object.Equals(vInsurerAddress4, Nothing)) Then


                    'developer guide no.24
                    .InsurerAddress4 = vInsurerAddress4
                End If



                If (Not Informations.IsNothing(vInsurerPostcode)) And (Not Object.Equals(vInsurerPostcode, Nothing)) Then


                    'developer guide no.24
                    .InsurerPostCode = vInsurerPostcode
                End If



                If (Not Informations.IsNothing(vInsurerTelephoneNumber)) And (Not Object.Equals(vInsurerTelephoneNumber, Nothing)) Then


                    'developer guide no.24
                    .InsurerTelephoneNumber = vInsurerTelephoneNumber
                End If



                If (Not Informations.IsNothing(vInsurerFaxNumber)) And (Not Object.Equals(vInsurerFaxNumber, Nothing)) Then


                    'developer guide no.24
                    .InsurerFaxNumber = vInsurerFaxNumber
                End If



                If (Not Informations.IsNothing(vInsurerContactName)) And (Not Object.Equals(vInsurerContactName, Nothing)) Then


                    'developer guide no.24
                    .InsurerContactName = vInsurerContactName
                End If



                If (Not Informations.IsNothing(vInsurerEmail)) And (Not Object.Equals(vInsurerEmail, Nothing)) Then


                    'developer guide no.24
                    .InsurerEmail = vInsurerEmail
                End If



                If (Not Informations.IsNothing(vInsurerNotes)) And (Not Object.Equals(vInsurerNotes, Nothing)) Then


                    'developer guide no.24
                    .InsurerNotes = vInsurerNotes
                End If



                If (Not Informations.IsNothing(vCompanyNotes)) And (Not Object.Equals(vCompanyNotes, Nothing)) Then


                    'developer guide no.24
                    .CompanyNotes = vCompanyNotes
                End If

                If Not String.IsNullOrEmpty(vUniqueId) Then
                    .UniqueId = vUniqueId
                    .ScreenHierarchy = vScreenHierarchy
                End If

                If (Not Informations.IsNothing(vExtraSupplierPartyDetails)) And (Not Object.Equals(vExtraSupplierPartyDetails, Nothing)) Then


                    'developer guide no.24
                    .ExtraSupplierPartyDetails = vExtraSupplierPartyDetails
                End If

                m_iDatabaseStatus = iStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRPartyOT property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vPartyStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vExternalId As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyOT
                'developer guide no.118
                'start

                vPartyCnt = .PartyCnt


                vLicenseTypeId = .LicenseTypeId


                vLicenseNumber = .LicenseNumber


                vDateOfBirth = .DateOfBirth


                vGender = .Gender


                vPartyStatus = .PartyStatus


                vReferenceNumber = .ReferenceNumber


                vExternalId = .ExternalId


                vRegNumber = .RegNumber


                vDatePassedTest = .DatePassedTest


                vContactName = .ContactName


                vContactTelephoneNumber = .ContactTelephoneNumber


                vInsurerName = .InsurerName


                vInsurerAddress1 = .InsurerAddress1


                vInsurerAddress2 = .InsurerAddress2


                vInsurerAddress3 = .InsurerAddress3


                vInsurerAddress4 = .InsurerAddress4


                vInsurerPostcode = .InsurerPostCode


                vInsurerTelephoneNumber = .InsurerTelephoneNumber


                vInsurerFaxNumber = .InsurerFaxNumber


                vInsurerContactName = .InsurerContactName


                vInsurerEmail = .InsurerEmail


                vInsurerNotes = .InsurerNotes


                vCompanyNotes = .CompanyNotes

                'end
                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Data object primary key
            m_dSIRPartyOT.PartyCnt = PartyCnt

            ' Select a record from the database
            m_lReturn = m_dSIRPartyOT.SelectSingle()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Data object primary key
            m_dSIRPartyOT.PartyCnt = PartyCnt
            ' Add a record to the database from the object
            m_lReturn = m_dSIRPartyOT.Add()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Retain the Primary Key of the SIRPartyIN Added
            PartyCnt = m_dSIRPartyOT.PartyCnt

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Data object primary key
            m_dSIRPartyOT.PartyCnt = PartyCnt

            ' Update the record on the database from the object
            m_lReturn = m_dSIRPartyOT.Delete()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Data object primary key
            m_dSIRPartyOT.PartyCnt = PartyCnt

            ' Update the record on the database from the object
            m_lReturn = m_dSIRPartyOT.Update()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyOT.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vPartyStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vExternalId As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPartyCnt)) Or (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vLicenseTypeId)) Or (Object.Equals(vLicenseTypeId, Nothing)) Or (bDefaultAll) Then


            vLicenseTypeId = DBNull.Value
        End If



        If (Informations.IsNothing(vLicenseNumber)) Or (Object.Equals(vLicenseNumber, Nothing)) Or (bDefaultAll) Then


            vLicenseNumber = DBNull.Value
        End If



        If (Informations.IsNothing(vDateOfBirth)) Or (Object.Equals(vDateOfBirth, Nothing)) Or (bDefaultAll) Then


            vDateOfBirth = DBNull.Value
        End If



        If (Informations.IsNothing(vGender)) Or (Object.Equals(vGender, Nothing)) Or (bDefaultAll) Then


            vGender = DBNull.Value
        End If



        If (Informations.IsNothing(vPartyStatus)) Or (Object.Equals(vPartyStatus, Nothing)) Or (bDefaultAll) Then


            vPartyStatus = DBNull.Value
        End If



        If (Informations.IsNothing(vReferenceNumber)) Or (Object.Equals(vReferenceNumber, Nothing)) Or (bDefaultAll) Then


            vReferenceNumber = DBNull.Value
        End If



        If (Informations.IsNothing(vExternalId)) Or (Object.Equals(vExternalId, Nothing)) Or (bDefaultAll) Then


            vExternalId = DBNull.Value
        End If



        If (Informations.IsNothing(vRegNumber)) Or (Object.Equals(vRegNumber, Nothing)) Or (bDefaultAll) Then


            vRegNumber = DBNull.Value
        End If



        If Informations.IsNothing(vDatePassedTest) Or Object.Equals(vDatePassedTest, Nothing) Or bDefaultAll Then


            vDatePassedTest = DBNull.Value
        End If



        If Informations.IsNothing(vContactName) Or Object.Equals(vContactName, Nothing) Or bDefaultAll Then


            vContactName = DBNull.Value
        End If



        If Informations.IsNothing(vContactTelephoneNumber) Or Object.Equals(vContactTelephoneNumber, Nothing) Or bDefaultAll Then


            vContactTelephoneNumber = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerName) Or Object.Equals(vInsurerName, Nothing) Or bDefaultAll Then


            vInsurerName = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerAddress1) Or Object.Equals(vInsurerAddress1, Nothing) Or bDefaultAll Then


            vInsurerAddress1 = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerAddress2) Or Object.Equals(vInsurerAddress2, Nothing) Or bDefaultAll Then


            vInsurerAddress2 = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerAddress3) Or Object.Equals(vInsurerAddress3, Nothing) Or bDefaultAll Then


            vInsurerAddress3 = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerAddress4) Or Object.Equals(vInsurerAddress4, Nothing) Or bDefaultAll Then


            vInsurerAddress4 = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerPostcode) Or Object.Equals(vInsurerPostcode, Nothing) Or bDefaultAll Then


            vInsurerPostcode = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerTelephoneNumber) Or Object.Equals(vInsurerTelephoneNumber, Nothing) Or bDefaultAll Then


            vInsurerTelephoneNumber = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerFaxNumber) Or Object.Equals(vInsurerFaxNumber, Nothing) Or bDefaultAll Then


            vInsurerFaxNumber = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerContactName) Or Object.Equals(vInsurerContactName, Nothing) Or bDefaultAll Then


            vInsurerContactName = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerEmail) Or Object.Equals(vInsurerEmail, Nothing) Or bDefaultAll Then


            vInsurerEmail = DBNull.Value
        End If



        If Informations.IsNothing(vInsurerNotes) Or Object.Equals(vInsurerNotes, Nothing) Or bDefaultAll Then


            vInsurerNotes = DBNull.Value
        End If



        If Informations.IsNothing(vCompanyNotes) Or Object.Equals(vCompanyNotes, Nothing) Or bDefaultAll Then


            vCompanyNotes = DBNull.Value
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyOT for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vLicenseTypeId As Object = Nothing, Optional ByRef vLicenseNumber As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vPartyStatus As Object = Nothing, Optional ByRef vReferenceNumber As Object = Nothing, Optional ByRef vExternalId As Object = Nothing, Optional ByRef vRegNumber As Object = Nothing, Optional ByRef vDatePassedTest As Object = Nothing, Optional ByRef vContactName As Object = Nothing, Optional ByRef vContactTelephoneNumber As Object = Nothing, Optional ByRef vInsurerName As Object = Nothing, Optional ByRef vInsurerAddress1 As Object = Nothing, Optional ByRef vInsurerAddress2 As Object = Nothing, Optional ByRef vInsurerAddress3 As Object = Nothing, Optional ByRef vInsurerAddress4 As Object = Nothing, Optional ByRef vInsurerPostcode As Object = Nothing, Optional ByRef vInsurerTelephoneNumber As Object = Nothing, Optional ByRef vInsurerFaxNumber As Object = Nothing, Optional ByRef vInsurerContactName As Object = Nothing, Optional ByRef vInsurerEmail As Object = Nothing, Optional ByRef vInsurerNotes As Object = Nothing, Optional ByRef vCompanyNotes As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLicenseTypeId) Then

            If Not (Convert.IsDBNull(vLicenseTypeId) Or Informations.IsNothing(vLicenseTypeId)) Then

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vLicenseTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vDateOfBirth) Then

            If Not (Convert.IsDBNull(vDateOfBirth) Or Informations.IsNothing(vDateOfBirth)) Then
                If Not Informations.IsDate(vDateOfBirth) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vPartyStatus) Then

            If Not (Convert.IsDBNull(vPartyStatus) Or Informations.IsNothing(vPartyStatus)) Then

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vPartyStatus), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vExternalId) Then

            If Not (Convert.IsDBNull(vExternalId) Or Informations.IsNothing(vExternalId)) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vExternalId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vDatePassedTest) Then

            If Not (Convert.IsDBNull(vDatePassedTest) Or Informations.IsNothing(vDatePassedTest)) Then
                If Not Informations.IsDate(vDatePassedTest) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        ' {* USER DEFINED CODE (End) *}

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

