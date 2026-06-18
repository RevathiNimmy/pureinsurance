Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared

Friend NotInheritable Class SIRPartyPC
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyPC
    '
    ' Date: 12/10/1998
    '
    ' Description: Describes the SIRPartyPC attributes.
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
    Private Const ACClass As String = "SIRPartyPC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyPC As dSIRPartyPC.SIRPartyPC ' was dSIRPartyPC.SIRPartyPC

    ' Instance of the Core SIRClaim object
    'Private m_bSIRParty As bSIRParty.Business
    Private m_bSIRParty As bSIRParty.Business

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer

    Private m_bEvent As Boolean

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

    Public Property FromEvent() As Boolean
        Get

            Return m_bEvent

        End Get
        Set(ByVal Value As Boolean)

            m_bEvent = Value

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
            m_dSIRPartyPC = New dSIRPartyPC.SIRPartyPC()

            m_lReturn = m_dSIRPartyPC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

            ' Create Core Business Object
            '    Set m_bSIRParty = New bSIRParty.Business




            m_bSIRParty = New bSIRParty.Business
            m_lReturn = m_bSIRParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)


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
                If m_dSIRPartyPC IsNot Nothing Then
                    m_dSIRPartyPC.Dispose()
                End If
                m_dSIRPartyPC = Nothing
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
    ' Description: Returns the Default Values for the SIRPartyPC.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no. 98 (Guide)
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=(vPartyCnt), vPartyTitleCode:=(vPartyTitleCode), vForename:=(vForename), vInitials:=(vInitials), vEmploymentStatusCode:=(vEmploymentStatusCode), vEmployerCnt:=(vEmploymentStatusCode), vEmployerBusiness:=(vEmployerBusiness), vSecondaryEmploymentStatusC:=(vSecondaryEmploymentStatusC), vSecondaryEmployerBusiness:=(vSecondaryEmployerBusiness), vMaritalStatusCode:=(vMaritalStatusCode), vNumberOfChildren:=(vNumberOfChildren), vNationalityId:=vNationalityId, vCountryOfOriginCode:=(vCountryOfOriginCode), vMailshot:=(vMailshot), vIsPetOwner:=(vIsPetOwner), vAccommodationTypeCode:=(vAccommodationTypeCode), vIsFeeClient:=(vIsFeeClient)), gPMConstants.PMEReturnCode)


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
    ' Description: Sets the supplied SIRPartyPC property values.
    '
    ' ***************************************************************** '
    'developer guide no. 101(Guide)
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyTitleCode As String = "", Optional ByRef vForename As String = "", Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = 0, Optional ByRef vEmployerBusiness As String = "", Optional ByRef vSecondaryEmploymentStatusC As String = "", Optional ByRef vSecondaryEmployerBusiness As String = "", Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vPartyLifestyleId As Integer = 0, Optional ByRef vPartyLifestyleName As String = "", Optional ByRef vCategory As Integer = 0, Optional ByRef vDateOfBirth As Date = #12/30/1899#, Optional ByRef vGender As String = "", Optional ByRef vOccupationCode As String = "", Optional ByRef vSecondaryOccupationCode As String = "", Optional ByRef vIsSmoker As Integer = 0, Optional ByRef vSalutation As String = "", Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters

                'developer guide no. 98(Guide)
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyTitleCode:=vPartyTitleCode, vForename:=vForename, vInitials:=(vInitials), vEmploymentStatusCode:=(vEmploymentStatusCode), vEmployerCnt:=vEmployerCnt, vEmployerBusiness:=vEmployerBusiness, vSecondaryEmploymentStatusC:=vSecondaryEmploymentStatusC, vSecondaryEmployerBusiness:=vSecondaryEmployerBusiness, vMaritalStatusCode:=(vMaritalStatusCode), vNumberOfChildren:=(vNumberOfChildren), vNationalityId:=vNationalityId, vCountryOfOriginCode:=(vCountryOfOriginCode), vMailshot:=(vMailshot), vIsPetOwner:=(vIsPetOwner), vAccommodationTypeCode:=(vAccommodationTypeCode), vPartyLifestyleId:=vPartyLifestyleId, vPartyLifestyleName:=vPartyLifestyleName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGender:=vGender, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker, vSalutation:=vSalutation, vSource:=(vSource), vTPSind:=(vTPSind), vEMPSind:=(vEMPSind), vTPPassword:=(vTPPassword), vIsFeeClient:=vIsFeeClient), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on DefaultParameters", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vPartyTitleCode:=vPartyTitleCode, vForename:=vForename, vInitials:=vInitials, vEmploymentStatusCode:=vEmploymentStatusCode, vEmployerCnt:=vEmployerCnt, vEmployerBusiness:=vEmployerBusiness, vSecondaryEmploymentStatusC:=vSecondaryEmploymentStatusC, vSecondaryEmployerBusiness:=vSecondaryEmployerBusiness, vMaritalStatusCode:=vMaritalStatusCode, vNumberOfChildren:=vNumberOfChildren, vNationalityId:=vNationalityId, vCountryOfOriginCode:=vCountryOfOriginCode, vMailshot:=vMailshot, vIsPetOwner:=vIsPetOwner, vAccommodationTypeCode:=vAccommodationTypeCode, vSalutation:=vSalutation), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed on Validate", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyPC



                'developer guide no. 115(Guide)
                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Informations.IsNothing(vPartyTitleCode)) AndAlso (Not String.IsNullOrEmpty(vPartyTitleCode)) Then
                    .PartyTitleCode = vPartyTitleCode
                End If



                If (Not Informations.IsNothing(vForename)) AndAlso (Not String.IsNullOrEmpty(vForename)) Then
                    .Forename = vForename
                End If



                If (Not Informations.IsNothing(vInitials)) AndAlso (Not Object.Equals(vInitials, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .Initials = vInitials
                End If



                If (Not Informations.IsNothing(vEmploymentStatusCode)) AndAlso (Not Object.Equals(vEmploymentStatusCode, Nothing)) Then


                    ' developer guide no. 24(Guide)
                    .EmploymentStatusCode = vEmploymentStatusCode
                End If



                If (Not Informations.IsNothing(vEmployerCnt)) AndAlso (Not vEmployerCnt.Equals(0)) Then
                    .EmployerCnt = vEmployerCnt
                End If



                If (Not Informations.IsNothing(vEmployerBusiness)) AndAlso (Not String.IsNullOrEmpty(vEmployerBusiness)) Then
                    .EmployerBusiness = vEmployerBusiness
                End If



                If (Not Informations.IsNothing(vSecondaryEmploymentStatusC)) AndAlso (Not String.IsNullOrEmpty(vSecondaryEmploymentStatusC)) Then
                    .SecondaryEmploymentStatusC = vSecondaryEmploymentStatusC
                End If



                If (Not Informations.IsNothing(vSecondaryEmployerBusiness)) AndAlso (Not String.IsNullOrEmpty(vSecondaryEmployerBusiness)) Then
                    .SecondaryEmployerBusiness = vSecondaryEmployerBusiness
                End If



                If (Not Informations.IsNothing(vMaritalStatusCode)) AndAlso (Not Object.Equals(vMaritalStatusCode, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .MaritalStatusCode = vMaritalStatusCode
                End If



                If (Not Informations.IsNothing(vNumberOfChildren)) AndAlso (Not Object.Equals(vNumberOfChildren, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .NumberOfChildren = vNumberOfChildren
                End If



                If (Not Informations.IsNothing(vNationalityId)) AndAlso (Not Object.Equals(vNationalityId, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .NationalityId = vNationalityId
                End If



                If (Not Informations.IsNothing(vCountryOfOriginCode)) AndAlso (Not Object.Equals(vCountryOfOriginCode, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .CountryOfOriginCode = vCountryOfOriginCode
                End If



                If (Not Informations.IsNothing(vMailshot)) AndAlso (Not Object.Equals(vMailshot, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .Mailshot = vMailshot
                End If



                If (Not Informations.IsNothing(vIsPetOwner)) AndAlso (Not Object.Equals(vIsPetOwner, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .IsPetOwner = vIsPetOwner
                End If



                If (Not Informations.IsNothing(vAccommodationTypeCode)) AndAlso (Not Object.Equals(vAccommodationTypeCode, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .AccommodationTypeCode = vAccommodationTypeCode
                End If



                If (Not Informations.IsNothing(vPartyLifestyleId)) AndAlso (Not vPartyLifestyleId.Equals(0)) Then
                    .PartyLifestyleID = vPartyLifestyleId
                End If



                If (Not Informations.IsNothing(vPartyLifestyleName)) AndAlso (Not String.IsNullOrEmpty(vPartyLifestyleName)) Then
                    .PartyLifestyleName = vPartyLifestyleName
                End If



                If (Not Informations.IsNothing(vCategory)) AndAlso (Not vCategory.Equals(0)) Then
                    .Category = vCategory
                End If



                If (Not Informations.IsNothing(vDateOfBirth)) AndAlso (Not vDateOfBirth.Equals(DateTime.FromOADate(0))) Then
                    .DateOfBirth = vDateOfBirth
                End If



                If (Not Informations.IsNothing(vGender)) AndAlso (Not String.IsNullOrEmpty(vGender)) Then
                    .Gender = vGender
                End If



                If (Not Informations.IsNothing(vOccupationCode)) AndAlso (Not String.IsNullOrEmpty(vOccupationCode)) Then
                    .OccupationCode = vOccupationCode
                End If



                If (Not Informations.IsNothing(vSecondaryOccupationCode)) AndAlso (Not String.IsNullOrEmpty(vSecondaryOccupationCode)) Then
                    .SecondaryOccupationCode = vSecondaryOccupationCode
                End If




                If ((Not Informations.IsNothing(vIsSmoker)) AndAlso (Not vIsSmoker.Equals(0))) AndAlso (Not (Convert.IsDBNull(vIsSmoker) Or Informations.IsNothing(vIsSmoker))) Then
                    .IsSmoker = vIsSmoker
                End If

                ' CTAF 270701


                If (Not Informations.IsNothing(vSalutation)) AndAlso (Not String.IsNullOrEmpty(vSalutation)) Then
                    .Salutation = vSalutation
                End If




                If (Not Informations.IsNothing(vSource)) AndAlso (Not Object.Equals(vSource, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .Source = vSource
                End If



                If (Not Informations.IsNothing(vTPSind)) AndAlso (Not Object.Equals(vTPSind, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .TPSind = vTPSind
                End If



                If (Not Informations.IsNothing(vEMPSind)) AndAlso (Not Object.Equals(vEMPSind, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .EMPSind = vEMPSind
                End If



                If (Not Informations.IsNothing(vTPPassword)) AndAlso (Not Object.Equals(vTPPassword, Nothing)) Then


                    'developer guide no. 24(Guide)
                    .TPPassword = vTPPassword
                End If



                'developer guide no. 115(Guide)
                If (Not Informations.IsNothing(vIsFeeClient)) AndAlso (Not vIsFeeClient.Equals(0) AndAlso (Not vIsFeeClient Is DBNull.Value)) Then
                    .IsFeeClient = vIsFeeClient
                End If

                ' If we have changed one of the properties, update the status
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
    ' Description: Returns the supplied SIRPartyPC property values.
    '
    ' ***************************************************************** '
    'developer guide no. 101(Guide)
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vGender As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyPC


                'developer guide no. 143(Guide)
                'start
                vPartyCnt = .PartyCnt


                If Convert.IsDBNull(.PartyTitleCode) Or Informations.IsNothing(.PartyTitleCode) Then
                    vPartyTitleCode = ""
                Else
                    vPartyTitleCode = .PartyTitleCode
                End If


                vForename = .Forename

                If Convert.IsDBNull(.Initials) Or Informations.IsNothing(.Initials) Then
                    vInitials = ""
                Else
                    vInitials = .Initials
                End If

                If Convert.IsDBNull(.EmploymentStatusCode) Or Informations.IsNothing(.EmploymentStatusCode) Then
                    vEmploymentStatusCode = ""
                Else
                    vEmploymentStatusCode = .EmploymentStatusCode
                End If

                If Convert.IsDBNull(.EmployerCnt) Or Informations.IsNothing(.EmployerCnt) Then
                    vEmployerCnt = ""
                Else
                    vEmployerCnt = CStr(.EmployerCnt)
                End If

                If Convert.IsDBNull(.EmployerBusiness) Or Informations.IsNothing(.EmployerBusiness) Then
                    vEmployerBusiness = ""
                Else
                    vEmployerBusiness = .EmployerBusiness
                End If

                If Convert.IsDBNull(.SecondaryEmploymentStatusC) Or Informations.IsNothing(.SecondaryEmploymentStatusC) Then
                    vSecondaryEmploymentStatusC = ""
                Else
                    vSecondaryEmploymentStatusC = .SecondaryEmploymentStatusC
                End If

                If Convert.IsDBNull(.SecondaryEmployerBusiness) Or Informations.IsNothing(.SecondaryEmployerBusiness) Then
                    vSecondaryEmployerBusiness = ""
                Else
                    vSecondaryEmployerBusiness = .SecondaryEmployerBusiness
                End If

                If Convert.IsDBNull(.MaritalStatusCode) Or Informations.IsNothing(.MaritalStatusCode) Then
                    vMaritalStatusCode = ""
                Else
                    vMaritalStatusCode = .MaritalStatusCode
                End If

                If Convert.IsDBNull(.NumberOfChildren) Or Informations.IsNothing(.NumberOfChildren) Then
                    vNumberOfChildren = 0
                Else
                    vNumberOfChildren = .NumberOfChildren
                End If


                'DC 28/09/00 set to Null if nothing set
                'TF110802 - Not Null - empty string
                If Convert.IsDBNull(.NationalityId) Or Informations.IsNothing(.NationalityId) Then
                    vNationalityId = Nothing
                Else
                    vNationalityId = .NationalityId
                End If

                If Convert.IsDBNull(.CountryOfOriginCode) Or Informations.IsNothing(.CountryOfOriginCode) Then
                    vCountryOfOriginCode = ""
                Else
                    vCountryOfOriginCode = .CountryOfOriginCode
                End If


                If Convert.IsDBNull(.Mailshot) Or Informations.IsNothing(.Mailshot) Then
                    vMailshot = 0
                Else
                    vMailshot = .Mailshot
                End If

                If Convert.IsDBNull(.IsPetOwner) Or Informations.IsNothing(.IsPetOwner) Then
                    vIsPetOwner = 0
                Else
                    vIsPetOwner = .IsPetOwner
                End If

                If Convert.IsDBNull(.AccommodationTypeCode) Or Informations.IsNothing(.AccommodationTypeCode) Then
                    vAccommodationTypeCode = ""
                Else
                    vAccommodationTypeCode = .AccommodationTypeCode
                End If

                If Convert.IsDBNull(.Salutation) Or Informations.IsNothing(.Salutation) Then
                    vSalutation = ""
                Else
                    vSalutation = .Salutation
                End If

                If Convert.IsDBNull(.Source) Or Informations.IsNothing(.Source) Then
                    vSource = ""
                Else
                    vSource = .Source
                End If

                If Convert.IsDBNull(.TPSind) Or Informations.IsNothing(.TPSind) Then
                    vTPSind = 0
                Else
                    vTPSind = .TPSind
                End If

                If Convert.IsDBNull(.EMPSind) Or Informations.IsNothing(.EMPSind) Then
                    vEMPSind = 0
                Else
                    vEMPSind = .EMPSind
                End If

                If Convert.IsDBNull(.TPPassword) Or Informations.IsNothing(.TPPassword) Then
                    vTPPassword = ""
                Else
                    vTPPassword = .TPPassword
                End If

                vIsFeeClient = .IsFeeClient

                iStatus = m_iDatabaseStatus
                'end

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyPC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                'And if we're coming from events
                .FromEvent = FromEvent

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End With

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

            With m_dSIRPartyPC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the record on the database from the object
                m_lReturn = .AddInsuredLifestyle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Retain the Primary Key of the SIRPartyPC Added
                PartyCnt = .PartyCnt

            End With

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

            With m_dSIRPartyPC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Delete()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

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

            With m_dSIRPartyPC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Update the record on the database from the object
                ' TF210700 - Dealt with in Party.Services()
                ' TF250700 - Not any more its not (only for PartyPC)!
                m_lReturn = .UpdateInsuredLifestyle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CopyToEvent (Public)
    '
    ' Description: Makes a copy of the party to the event table.
    '
    ' ***************************************************************** '
    Public Function CopyToEvent(ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_dSIRPartyPC.CopyPartyToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRPartyPC.CopyPartyPCToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyToEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyToEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyPC.
    '
    ' ***************************************************************** '
    'developer guide no. 101(Guide)
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = 0, Optional ByRef vPartyTitleCode As String = "", Optional ByRef vForename As String = "", Optional ByRef vInitials As String = "", Optional ByRef vEmploymentStatusCode As String = "", Optional ByRef vEmployerCnt As Object = 0, Optional ByRef vEmployerBusiness As String = "", Optional ByRef vSecondaryEmploymentStatusC As String = "", Optional ByRef vSecondaryEmployerBusiness As String = "", Optional ByRef vMaritalStatusCode As String = "", Optional ByRef vNumberOfChildren As Object = 0, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As String = "", Optional ByRef vMailshot As Object = 0, Optional ByRef vIsPetOwner As Object = 0, Optional ByRef vAccommodationTypeCode As String = "", Optional ByRef vPartyLifestyleId As Object = 0, Optional ByRef vPartyLifestyleName As String = "", Optional ByRef vCategory As Object = 0, Optional ByRef vDateOfBirth As String = "", Optional ByRef vGender As String = "", Optional ByRef vOccupationCode As String = "", Optional ByRef vSecondaryOccupationCode As String = "", Optional ByRef vIsSmoker As Object = 0, Optional ByRef vSalutation As String = "", Optional ByRef vSource As String = "", Optional ByRef vTPSind As Object = 0, Optional ByRef vEMPSind As Object = 0, Optional ByRef vTPPassword As String = "", Optional ByRef vIsFeeClient As Object = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no. 44 
        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vPartyTitleCode)) OrElse (String.IsNullOrEmpty(vPartyTitleCode)) Or (bDefaultAll) Then
            vPartyTitleCode = ""
        End If



        If (Informations.IsNothing(vForename)) OrElse (String.IsNullOrEmpty(vForename)) Or (bDefaultAll) Then
            vForename = ""
        End If



        If (Informations.IsNothing(vInitials)) OrElse (String.IsNullOrEmpty(vInitials)) Or (bDefaultAll) Then
            vInitials = ""
        End If



        If (Informations.IsNothing(vEmployerCnt)) OrElse (vEmployerCnt.Equals(0)) Or (bDefaultAll) Then
            vEmploymentStatusCode = ""
        End If




        If (Informations.IsNothing(vEmployerBusiness)) OrElse (String.IsNullOrEmpty(vEmployerBusiness)) Or (bDefaultAll) Then
            vEmployerBusiness = ""
        End If



        If (Informations.IsNothing(vSecondaryEmploymentStatusC)) OrElse (String.IsNullOrEmpty(vSecondaryEmploymentStatusC)) Or (bDefaultAll) Then
            vSecondaryEmploymentStatusC = ""
        End If



        If (Informations.IsNothing(vSecondaryEmployerBusiness)) OrElse (String.IsNullOrEmpty(vSecondaryEmployerBusiness)) Or (bDefaultAll) Then
            vSecondaryEmployerBusiness = ""
        End If



        If (Informations.IsNothing(vMaritalStatusCode)) OrElse (String.IsNullOrEmpty(vMaritalStatusCode)) Or (bDefaultAll) Then
            vMaritalStatusCode = ""
        End If



        If (Informations.IsNothing(vNumberOfChildren)) OrElse (vNumberOfChildren.Equals(0)) Or (bDefaultAll) Then
            vNumberOfChildren = 0
        End If

        'DC 03/08/00 was no check just setting to 1
        'DC 27/09/00 set to Null if missing


        If (Informations.IsNothing(vNationalityId)) OrElse (Object.Equals(vNationalityId, Nothing)) Or (bDefaultAll) Then


            vNationalityId = DBNull.Value
        End If



        If (Informations.IsNothing(vCountryOfOriginCode)) OrElse (String.IsNullOrEmpty(vCountryOfOriginCode)) Or (bDefaultAll) Then
            vCountryOfOriginCode = ""
        End If



        If (Informations.IsNothing(vMailshot)) OrElse (vMailshot.Equals(0)) Or (bDefaultAll) Then
            vMailshot = 0
        End If



        If (Informations.IsNothing(vIsPetOwner)) OrElse (vIsPetOwner.Equals(0)) Or (bDefaultAll) Then
            vIsPetOwner = 0
        End If



        If (Informations.IsNothing(vAccommodationTypeCode)) OrElse (String.IsNullOrEmpty(vAccommodationTypeCode)) Or (bDefaultAll) Then
            vAccommodationTypeCode = ""
        End If



        If (Informations.IsNothing(vPartyLifestyleId)) OrElse (vPartyLifestyleId.Equals(0)) Or (bDefaultAll) Then
            vPartyLifestyleId = 1
        End If



        If (Informations.IsNothing(vPartyLifestyleName)) OrElse (String.IsNullOrEmpty(vPartyLifestyleName)) Or (bDefaultAll) Then
            vPartyLifestyleName = ""
        End If



        If (Informations.IsNothing(vCategory)) OrElse (vCategory.Equals(0)) Or (bDefaultAll) Then
            vCategory = 1
        End If



        If (Informations.IsNothing(vDateOfBirth)) OrElse (String.IsNullOrEmpty(vDateOfBirth)) Or (bDefaultAll) Then
            'DC 20/10/00 chnaged to Null (which is 29/12/1899)
            'vDateOfBirth = Now
            vDateOfBirth = "29/12/1899"
        End If



        If (Informations.IsNothing(vGender)) OrElse (String.IsNullOrEmpty(vGender)) Or (bDefaultAll) Then
            vGender = ""
        End If



        If (Informations.IsNothing(vOccupationCode)) OrElse (String.IsNullOrEmpty(vOccupationCode)) Or (bDefaultAll) Then
            vOccupationCode = ""
        End If



        If (Informations.IsNothing(vSecondaryOccupationCode)) OrElse (String.IsNullOrEmpty(vSecondaryOccupationCode)) Or (bDefaultAll) Then
            vSecondaryOccupationCode = ""
        End If



        If (Informations.IsNothing(vIsSmoker)) OrElse (vIsSmoker.Equals(0)) Or (bDefaultAll) Then
            vIsSmoker = 0
        End If

        ' CTAF 260701


        If (Informations.IsNothing(vSalutation)) OrElse (String.IsNullOrEmpty(vSalutation)) Or (bDefaultAll) Then
            vSalutation = ""
        End If




        If (Informations.IsNothing(vSource)) OrElse (String.IsNullOrEmpty(vSource)) Or (bDefaultAll) Then
            vSource = ""
        End If



        If (Informations.IsNothing(vTPSind)) OrElse (vTPSind.Equals(0)) Or (bDefaultAll) Then
            vTPSind = 0
        End If



        If (Informations.IsNothing(vEMPSind)) OrElse (vEMPSind.Equals(0)) Or (bDefaultAll) Then
            vEMPSind = 0
        End If



        If (Informations.IsNothing(vTPPassword)) OrElse (String.IsNullOrEmpty(vTPPassword)) Or (bDefaultAll) Then
            vTPPassword = ""
        End If



        If Informations.IsNothing(vIsFeeClient) OrElse vIsFeeClient.Equals(0) Or bDefaultAll Then
            vIsFeeClient = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyPC for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTitleCode As Object = Nothing, Optional ByRef vForename As Object = Nothing, Optional ByRef vInitials As Object = Nothing, Optional ByRef vEmploymentStatusCode As Object = Nothing, Optional ByRef vEmployerCnt As Object = Nothing, Optional ByRef vEmployerBusiness As Object = Nothing, Optional ByRef vSecondaryEmploymentStatusC As Object = Nothing, Optional ByRef vSecondaryEmployerBusiness As Object = Nothing, Optional ByRef vMaritalStatusCode As Object = Nothing, Optional ByRef vNumberOfChildren As Object = Nothing, Optional ByRef vNationalityId As Object = Nothing, Optional ByRef vCountryOfOriginCode As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vIsPetOwner As Object = Nothing, Optional ByRef vAccommodationTypeCode As Object = Nothing, Optional ByRef vSalutation As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="vPartyCnt must be numeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If


        If Not Informations.IsNothing(vEmployerCnt) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vEmployerCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="vEmployerCnt must be numeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If


        If Not Informations.IsNothing(vNumberOfChildren) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vNumberOfChildren), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="vNumberOfChildren must be numeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If

        'DC 27/09/00 no need to check if Null

        If Not (Convert.IsDBNull(vNationalityId) Or Informations.IsNothing(vNationalityId)) Then

            If Not Informations.IsNothing(vNationalityId) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vNationalityId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="vNationalityId must be numeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
        End If


        If Not Informations.IsNothing(vMailshot) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vMailshot), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="vMailshot must be numeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If
        End If


        If Not Informations.IsNothing(vIsPetOwner) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vIsPetOwner), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="vIsPetOwner must be numeric.", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

