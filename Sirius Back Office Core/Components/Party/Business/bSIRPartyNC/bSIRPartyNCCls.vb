Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
Friend NotInheritable Class SIRPartyNC
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyNC
    '
    ' Date: 25/06/1999
    '
    ' Description: Describes the SIRPartyNC attributes.
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
    Private Const ACClass As String = "SIRPartyNC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyNC As dSIRPartyNC.SIRPartyNC ' was dSIRPartyNC.SIRPartyNC

    ' Instance of the Core SIRClaim object
    'Private m_bSIRParty As bSIRParty.Business
    Private m_bSIRParty As bSIRParty.Business

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    ' PRIVATE Data Members (End)

    'RJG 09/06/2000 - User ID for call to Login.
    Private m_sUserID As String = ""

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
    Public ReadOnly Property bSIRParty() As Object
        Get

            Return m_bSIRParty

        End Get
    End Property


    Public Property UserID() As String
        Get
            Return m_sUserID
        End Get
        Set(ByVal Value As String)
            m_sUserID = Value
        End Set
    End Property

    Public Function Login() As Integer
        ' ***************************************************************** '
        ' Name: Login (Public)
        '
        ' Description: Reads the PartyCnt from the Database given a UserID.
        '
        ' ***************************************************************** '
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRPartyNC

                ' Set Data object primary key
                .UserID = UserID

                ' Select a record from the database
                ' NIIT changed as per VB Code
                'm_lReturn = .result()
                m_lReturn = result
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                PartyCnt = .PartyCnt

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Login Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Login", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
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
            m_dSIRPartyNC = New dSIRPartyNC.SIRPartyNC()
            '    Set m_dSIRPartyNC = New dSIRPartyNC.SIRPartyNC

            m_lReturn = m_dSIRPartyNC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRPartyNC IsNot Nothing Then
                    m_dSIRPartyNC.Dispose()
                End If
                m_dSIRPartyNC = Nothing
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
    ' Description: Returns the Default Values for the SIRPartyNC.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            'Add UserID and Curr Ins renewal date as parameters



            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserID:=vUserID, vCurrInsRenewalDate:=vCurrInsRenewalDate), gPMConstants.PMEReturnCode)

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
    ' Description: Sets the supplied SIRPartyNC property values.
    '
    ' ***************************************************************** '
    'eck060600 Pass date of birth
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyLifeStyleID As Integer = 0, Optional ByRef vDateOfBirth As Date = #12/30/1899#, Optional ByRef vPassword As String = "", Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserID As String = "", Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'RJG 09/06/2000 - Added UserID and Curr Ins Renewal date as new params

                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vDateOfBirth:=vDateOfBirth, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserID:=vUserID, vCurrInsRenewalDate:=vCurrInsRenewalDate), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTPIntroducerCode:=vTPIntroducerCode, vTPUserCode:=vTPUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserID:=vUserID, vCurrInsRenewalDate:=vCurrInsRenewalDate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyNC



                If (Not Informations.IsNothing(vPartyCnt)) And (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If
                'eck060600


                If (Not Informations.IsNothing(vPartyLifeStyleID)) And (Not vPartyLifeStyleID.Equals(0)) Then
                    .PartyLifestyleID = vPartyLifeStyleID
                End If



                If (Not Informations.IsNothing(vDateOfBirth)) And (Not vDateOfBirth.Equals(DateTime.FromOADate(0))) Then
                    .DateOfBirth = vDateOfBirth
                End If



                If (Not Informations.IsNothing(vPassword)) And (Not String.IsNullOrEmpty(vPassword)) Then
                    .Password = vPassword
                End If




                If (Not Informations.IsNothing(vMothersMaidenName)) And (Not Object.Equals(vMothersMaidenName, Nothing)) Then


                    'Developer Guide No.24
                    '.set_MothersMaidenName(vMothersMaidenName)
                    .MothersMaidenName = vMothersMaidenName

                End If



                If (Not Informations.IsNothing(vTPIntroducerCode)) And (Not Object.Equals(vTPIntroducerCode, Nothing)) Then


                    'Developer Guide No.24
                    '.set_TPIntroducerCode(vTPIntroducerCode)
                    .TPIntroducerCode = vTPIntroducerCode
                End If



                If (Not Informations.IsNothing(vTPUserCode)) And (Not Object.Equals(vTPUserCode, Nothing)) Then


                    'Developer Guide no.24
                    '.set_TPUserCode(vTPUserCode)
                    .TPUserCode = vTPUserCode

                End If



                If (Not Informations.IsNothing(vMemorableDate)) And (Not Object.Equals(vMemorableDate, Nothing)) Then


                    'Developer Guide No.24
                    '.set_MemorableDate(vMemorableDate)
                    .MemorableDate = vMemorableDate

                End If



                If (Not Informations.IsNothing(vAQuestion)) And (Not Object.Equals(vAQuestion, Nothing)) Then


                    'Developer Guide No.
                    '.set_AQuestion(vAQuestion)
                    .AQuestion = vAQuestion

                End If



                If (Not Informations.IsNothing(vTheAnswer)) And (Not Object.Equals(vTheAnswer, Nothing)) Then


                    'Developer guide No. 24
                    '.set_TheAnswer(vTheAnswer)
                    .TheAnswer = vTheAnswer
                End If

                'RJG 09/06/2000 - Set properties for UserID and Curr Ins Renewal Date


                If (Not Informations.IsNothing(vUserID)) And (Not String.IsNullOrEmpty(vUserID)) Then
                    .UserID = vUserID
                End If



                If (Not Informations.IsNothing(vCurrInsRenewalDate)) And (Not Object.Equals(vCurrInsRenewalDate, Nothing)) Then


                    'Developer Guide  No. 24
                    '.set_CurrInsRenewalDate(vCurrInsRenewalDate)
                    .CurrInsRenewalDate = vCurrInsRenewalDate
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
    ' Description: Returns the supplied SIRPartyNC property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPassword As String = "", Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vDateOfBirth As Date = #12/30/1899#, Optional ByRef vUserID As String = "", Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyNC


                'developer guide no. 118
                'If Not Informations.IsNothing(vPartyCnt) Then
                vPartyCnt = .PartyCnt
                'End If


                'If Not Informations.IsNothing(vPassword) Then
                vPassword = .Password
                'End If


                'If Not Informations.IsNothing(vMothersMaidenName) Then


                vMothersMaidenName = .MothersMaidenName
                'End If


                'If Not Informations.IsNothing(vTPIntroducerCode) Then


                vTPIntroducerCode = .TPIntroducerCode
                'End If


                'If Not Informations.IsNothing(vTPUserCode) Then


                vTPUserCode = .TPUserCode
                'End If


                'If Not Informations.IsNothing(vMemorableDate) Then


                vMemorableDate = .MemorableDate
                'End If


                'If Not Informations.IsNothing(vAQuestion) Then


                vAQuestion = .AQuestion
                'End If


                'If Not Informations.IsNothing(vTheAnswer) Then


                vTheAnswer = .TheAnswer
                'End If


                'If Not Informations.IsNothing(vDateOfBirth) Then
                vDateOfBirth = .DateOfBirth
                'End If

                'RJG 09/06/2000 - Assign values to UserID and Curr Ins Renewal date

                'If Not Informations.IsNothing(vUserID) Then
                vUserID = .UserID
                'End If


                'If Not Informations.IsNothing(vCurrInsRenewalDate) Then


                vCurrInsRenewalDate = .CurrInsRenewalDate
                'End If

                iStatus = m_iDatabaseStatus

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

            With m_dSIRPartyNC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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

            With m_dSIRPartyNC

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'eck060600
                ' Update the record on the database from the object
                m_lReturn = .AddInsuredLifestyle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyNC Added
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

            With m_dSIRPartyNC

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

            With m_dSIRPartyNC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Update the record on the database from the object
                m_lReturn = .Update()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'eck060600
                ' Update the record on the database from the object
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
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyNC.
    '
    ' ***************************************************************** '
    'eck060600
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifeStyleID As Object = Nothing, Optional ByRef vDateOfBirth As Date = #12/30/1899#, Optional ByRef vPassword As String = "", Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserID As String = "", Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPartyCnt)) Or (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vPartyLifeStyleID)) Or (vPartyLifeStyleID.Equals(0)) Or (bDefaultAll) Then
            vPartyLifeStyleID = 1
        End If



        If (Informations.IsNothing(vDateOfBirth)) Or (vDateOfBirth.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateOfBirth = DateTime.Now
        End If



        If (Informations.IsNothing(vPassword)) Or (String.IsNullOrEmpty(vPassword)) Or (bDefaultAll) Then
            vPassword = ""
        End If

        'RJG 08/06/2000 - Set mothers maiden name to null by default


        If (Informations.IsNothing(vMothersMaidenName)) Or (Object.Equals(vMothersMaidenName, Nothing)) Or (bDefaultAll) Then


            vMothersMaidenName = DBNull.Value
        End If

        'RJG 08/06/2000 - Set TP Introducer Code to null by default


        If (Informations.IsNothing(vTPIntroducerCode)) Or (Object.Equals(vTPIntroducerCode, Nothing)) Or (bDefaultAll) Then


            vTPIntroducerCode = DBNull.Value
        End If

        'RJG 08/06/2000 - Set TPUser code to null by default


        If (Informations.IsNothing(vTPUserCode)) Or (Object.Equals(vTPUserCode, Nothing)) Or (bDefaultAll) Then


            vTPUserCode = DBNull.Value
        End If

        'RJG 08/06/2000 - Set memorable date to null by default


        If (Informations.IsNothing(vMemorableDate)) Or (Object.Equals(vMemorableDate, Nothing)) Or (bDefaultAll) Then


            vMemorableDate = DBNull.Value
        End If

        'RJG 08/06/2000 - Set A Question to null by default


        If (Informations.IsNothing(vAQuestion)) Or (Object.Equals(vAQuestion, Nothing)) Or (bDefaultAll) Then


            vAQuestion = DBNull.Value
        End If

        'RJG 08/06/2000 - Set The Answer to null by default


        If (Informations.IsNothing(vTheAnswer)) Or (Object.Equals(vTheAnswer, Nothing)) Or (bDefaultAll) Then


            vTheAnswer = DBNull.Value
        End If

        'RJG 09/06/2000 - Set Default values for new Userid and Curr Ins renewal date


        If (Informations.IsNothing(vUserID)) Or (String.IsNullOrEmpty(vUserID)) Or (bDefaultAll) Then
            vUserID = ""
        End If



        If (Informations.IsNothing(vCurrInsRenewalDate)) Or (Object.Equals(vCurrInsRenewalDate, Nothing)) Or (bDefaultAll) Then


            vCurrInsRenewalDate = DBNull.Value
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyNC for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTPIntroducerCode As Object = Nothing, Optional ByRef vTPUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserID As Object = Nothing, Optional ByRef vCurrInsRenewalDate As Object = Nothing) As Integer

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


        'RJG 08/06/2000 - Check if the date field is null or not.

        If Not Informations.IsNothing(vMemorableDate) Then

            If Not (Convert.IsDBNull(vMemorableDate) Or Informations.IsNothing(vMemorableDate)) Then
                If Not Informations.IsDate(vMemorableDate) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'RJG 09/06/2000 - Check if the date field is null or not.

        If Not Informations.IsNothing(vCurrInsRenewalDate) Then

            If Not (Convert.IsDBNull(vCurrInsRenewalDate) Or Informations.IsNothing(vCurrInsRenewalDate)) Then
                If Not Informations.IsDate(vCurrInsRenewalDate) Then
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

