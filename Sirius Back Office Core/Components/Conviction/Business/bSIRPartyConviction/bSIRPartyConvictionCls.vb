Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class SIRPartyConviction
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyConviction
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIRPartyConviction attributes.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "bSIRPartyConviction"

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

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyConviction As dSIRPartyConviction.SIRPartyConviction

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    Private m_lPartyConvictionID As Integer
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""

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
    Public Property PartyConvictionID() As Integer
        Get
            Return m_lPartyConvictionID
        End Get
        Set(ByVal Value As Integer)
            m_lPartyConvictionID = Value
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

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Create instance of data class
            m_dSIRPartyConviction = New dSIRPartyConviction.SIRPartyConviction()
            'New dSIRPartyConviction.SIRPartyConviction

            m_lReturn = m_dSIRPartyConviction.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRPartyConviction IsNot Nothing Then
                    m_dSIRPartyConviction.Dispose()
                End If
                m_dSIRPartyConviction = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyConviction.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults














            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID, vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SIRPartyConviction property values.
    '
    ' ***************************************************************** '
    'developer guide no. 33
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing, Optional vUniqueId As String = "", Optional vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters



                'developer guide no.98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID, vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vPartyCnt:=vPartyCnt, vPartyConvictionID:=vPartyConvictionID, vCode:=vCode, vConvictionDate:=vConvictionDate, vDescription:=vDescription, vFineAmt:=vFineAmt, vSentenceCode:=vSentenceCode, vSentenceDescription:=vSentenceDescription, vSentenceDuration:=vSentenceDuration, vSentenceDurationQualifier:=vSentenceDurationQualifier, vSentenceEffectiveDate:=vSentenceEffectiveDate, vStatusCode:=vStatusCode, vAlcoholLevel:=vAlcoholLevel, vAlcoholMeasurementMethod:=vAlcoholMeasurementMethod, vDrivingLicencePenaltyPoints:=vDrivingLicencePenaltyPoints)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False
            'EK 21/10/99 Change set up so thta nulls are not written
            ' Set Property values.
            With m_dSIRPartyConviction



                If (Not Information.IsNothing(vPartyCnt)) And (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Information.IsNothing(vPartyConvictionID)) And (Not vPartyConvictionID.Equals(0)) Then
                    .PartyConvictionID = vPartyConvictionID
                End If



                If (Not Information.IsNothing(vCode)) And (Not Object.Equals(vCode, Nothing)) Then


                    'developer guide no. 24
                    .Code = vCode
                End If



                If (Not Information.IsNothing(vConvictionDate)) And (Not Object.Equals(vConvictionDate, Nothing)) Then


                    'developer guide no. 24
                    .ConvictionDate = vConvictionDate
                End If



                If (Not Information.IsNothing(vDescription)) And (Not Object.Equals(vDescription, Nothing)) Then


                    'developer guide no. 24
                    .Description = vDescription
                End If



                If (Not Information.IsNothing(vFineAmt)) And (Not Object.Equals(vFineAmt, Nothing)) Then


                    'developer guide no. 24
                    .FineAmt = vFineAmt
                End If



                If (Not Information.IsNothing(vSentenceCode)) And (Not Object.Equals(vSentenceCode, Nothing)) Then


                    'developer guide no. 24
                    .SentenceCode = vSentenceCode
                End If



                If (Not Information.IsNothing(vSentenceDescription)) And (Not Object.Equals(vSentenceDescription, Nothing)) Then


                    'developer guide no. 24
                    .SentenceDescription = vSentenceDescription
                End If



                If (Not Information.IsNothing(vSentenceDuration)) And (Not Object.Equals(vSentenceDuration, Nothing)) Then


                    'developer guide no. 24
                    .SentenceDuration = vSentenceDuration
                End If



                If (Not Information.IsNothing(vSentenceDurationQualifier)) And (Not Object.Equals(vSentenceDurationQualifier, Nothing)) Then


                    'developer guide no. 24
                    .SentenceDurationQualifier = vSentenceDurationQualifier
                End If



                If (Not Information.IsNothing(vSentenceEffectiveDate)) And (Not Object.Equals(vSentenceEffectiveDate, Nothing)) Then


                    'developer guide no. 24
                    .SentenceEffectiveDate = vSentenceEffectiveDate
                End If



                If (Not Information.IsNothing(vStatusCode)) And (Not Object.Equals(vStatusCode, Nothing)) Then


                    'developer guide no. 24
                    .StatusCode = vStatusCode
                End If



                If (Not Information.IsNothing(vAlcoholLevel)) And (Not Object.Equals(vAlcoholLevel, Nothing)) Then


                    'developer guide no. 24
                    .AlcoholLevel = vAlcoholLevel
                End If



                If (Not Information.IsNothing(vAlcoholMeasurementMethod)) And (Not Object.Equals(vAlcoholMeasurementMethod, Nothing)) Then


                    'developer guide no. 24
                    .AlcoholMeasurementMethod = vAlcoholMeasurementMethod
                End If



                If (Not Information.IsNothing(vDrivingLicencePenaltyPoints)) And (Not Object.Equals(vDrivingLicencePenaltyPoints, Nothing)) Then


                    'developer guide no. 24
                    .DrivingLicencePenaltyPoints = vDrivingLicencePenaltyPoints
                End If

                If Not String.IsNullOrEmpty(vUniqueId) Then
                    .UniqueId = vUniqueId
                    .ScreenHierarchy = vScreenHierarchy
                End If
                ' If we have changed one of the properties, update the status
                m_iDatabaseStatus = iStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRPartyConviction property values.
    '
    ' ***************************************************************** '
    'Developer Guie no 101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyConviction

                'developer guide no. 143
                'Starts

                vPartyCnt = .PartyCnt


                vPartyConvictionID = .PartyConvictionID

                vCode = .Code


                vConvictionDate = .ConvictionDate


                vDescription = .Description


                vFineAmt = .FineAmt



                vSentenceCode = .SentenceCode


                vSentenceDescription = .SentenceDescription

                vSentenceDuration = .SentenceDuration


                vSentenceDurationQualifier = .SentenceDurationQualifier

                vSentenceEffectiveDate = .SentenceEffectiveDate


                vStatusCode = .StatusCode

                vAlcoholLevel = .AlcoholLevel


                vAlcoholMeasurementMethod = .AlcoholMeasurementMethod


                vDrivingLicencePenaltyPoints = .DrivingLicencePenaltyPoints

                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyConviction

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyConvictionID = PartyConvictionID

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyConviction

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyConviction Added
                PartyCnt = .PartyCnt
                PartyConvictionID = .PartyConvictionID

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyConviction

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyConvictionID = PartyConvictionID
                .UniqueId = m_sUniqueId
                .ScreenHierarchy = m_sScreenHierarchy

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dSIRPartyConviction

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyConvictionID = PartyConvictionID

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyConviction.
    '
    ' ***************************************************************** '
    'developer guide no. 33
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'Developer Guie no 151
        'Starts
        ' {* USER DEFINED CODE (Begin) *}


        If (Information.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Information.IsNothing(vPartyConvictionID)) OrElse (vPartyConvictionID.Equals(0)) OrElse (bDefaultAll) Then
            vPartyConvictionID = 0
        End If



        If (Information.IsNothing(vCode)) OrElse (String.IsNullOrEmpty(vCode)) OrElse (bDefaultAll) Then
            vCode = ""
        End If



        If (Information.IsNothing(vConvictionDate)) OrElse (String.IsNullOrEmpty(vConvictionDate)) OrElse (bDefaultAll) Then
            vConvictionDate = ""
        End If



        If (Information.IsNothing(vDescription)) OrElse (String.IsNullOrEmpty(vDescription)) OrElse (bDefaultAll) Then
            vDescription = ""
        End If



        If (Information.IsNothing(vFineAmt)) OrElse (vFineAmt.Equals(0)) OrElse (bDefaultAll) Then
            vFineAmt = 0
        End If



        If (Information.IsNothing(vSentenceCode)) OrElse (String.IsNullOrEmpty(vSentenceCode)) OrElse (bDefaultAll) Then
            vSentenceCode = ""
        End If



        If (Information.IsNothing(vSentenceDescription)) OrElse (String.IsNullOrEmpty(vSentenceDescription)) OrElse (bDefaultAll) Then
            vSentenceDescription = ""
        End If



        If (Information.IsNothing(vSentenceDuration)) OrElse (vSentenceDuration.Equals(0)) OrElse (bDefaultAll) Then
            vSentenceDuration = 0
        End If



        If (Information.IsNothing(vSentenceDurationQualifier)) OrElse (String.IsNullOrEmpty(vSentenceDurationQualifier)) OrElse (bDefaultAll) Then
            vSentenceDurationQualifier = ""
        End If



        If (Information.IsNothing(vSentenceEffectiveDate)) OrElse (String.IsNullOrEmpty(vSentenceEffectiveDate)) OrElse (bDefaultAll) Then
            vSentenceEffectiveDate = ""
        End If



        If (Information.IsNothing(vStatusCode)) OrElse (String.IsNullOrEmpty(vStatusCode)) OrElse (bDefaultAll) Then
            vStatusCode = ""
        End If



        If (Information.IsNothing(vAlcoholLevel)) OrElse (vAlcoholLevel.Equals(0)) OrElse (bDefaultAll) Then
            vAlcoholLevel = 0
        End If



        If (Information.IsNothing(vAlcoholMeasurementMethod)) OrElse (String.IsNullOrEmpty(vAlcoholMeasurementMethod)) OrElse (bDefaultAll) Then
            vAlcoholMeasurementMethod = ""
        End If



        If (Information.IsNothing(vDrivingLicencePenaltyPoints)) OrElse (vDrivingLicencePenaltyPoints.Equals(0)) OrElse (bDefaultAll) Then
            vDrivingLicencePenaltyPoints = 0
        End If

        'Ends


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyConviction for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyConvictionID As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vConvictionDate As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vFineAmt As Object = Nothing, Optional ByRef vSentenceCode As Object = Nothing, Optional ByRef vSentenceDescription As Object = Nothing, Optional ByRef vSentenceDuration As Object = Nothing, Optional ByRef vSentenceDurationQualifier As Object = Nothing, Optional ByRef vSentenceEffectiveDate As Object = Nothing, Optional ByRef vStatusCode As Object = Nothing, Optional ByRef vAlcoholLevel As Object = Nothing, Optional ByRef vAlcoholMeasurementMethod As Object = Nothing, Optional ByRef vDrivingLicencePenaltyPoints As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lVarRow As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Information.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vPartyConvictionID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPartyConvictionID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vFineAmt) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vFineAmt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vSentenceDuration) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vSentenceDuration), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vAlcoholLevel) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vAlcoholLevel), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vDrivingLicencePenaltyPoints) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vDrivingLicencePenaltyPoints), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
        'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

