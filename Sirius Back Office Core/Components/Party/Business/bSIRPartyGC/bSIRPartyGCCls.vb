Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
Friend NotInheritable Class SIRPartyGC
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyGC
    '
    ' Date: 12/10/1998
    '
    ' Description: Describes the SIRPartyGC attributes.
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
    Private Const ACClass As String = "SIRPartyGC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyGC As dSIRPartyGC.SIRPartyGC ' dSIRPartyGC.SIRPartyGC

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

            m_dSIRPartyGC = New dSIRPartyGC.SIRPartyGC()
            '    Set m_dSIRPartyGC = New dSIRPartyGC.SIRPartyGC

            m_lReturn = m_dSIRPartyGC.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRPartyGC IsNot Nothing Then
                    m_dSIRPartyGC.Dispose()
                End If
                m_dSIRPartyGC = Nothing
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
    ' Description: Returns the Default Values for the SIRPartyGC.
    '
    ' ***************************************************************** '
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields.
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyGroupTypeID As Object = Nothing, Optional ByRef vIsRegisteredCharity As Object = Nothing, Optional ByRef vCharityNumber As Object = Nothing, Optional ByRef vNumberofMembers As Object = Nothing, Optional ByRef vTpsInd As Object = Nothing, Optional ByRef vEmpsInd As Object = Nothing, Optional ByRef vMailShot As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults


            'developer guide no. 84(guide)
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPartyGroupTypeID:=vPartyGroupTypeID, vIsRegisteredCharity:=vIsRegisteredCharity, vCharityNumber:=vCharityNumber, vNumberofMembers:=vNumberofMembers, vTpsInd:=vTpsInd, vEmpsInd:=vEmpsInd, vMailShot:=vMailShot, vTurnover:=vTurnover, vIsFeeClient:=vIsFeeClient), gPMConstants.PMEReturnCode)

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
    ' Description: Sets the supplied SIRPartyGC property values.
    '
    ' ***************************************************************** '
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields.
    'developer guide no. 101(guide)
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyGroupTypeID As Object = Nothing, Optional ByRef vCharityNumber As Object = Nothing, Optional ByRef vIsRegisteredCharity As Object = Nothing, Optional ByRef vNumberofMembers As Object = Nothing, Optional ByRef vTpsInd As Object = Nothing, Optional ByRef vEmpsInd As Object = Nothing, Optional ByRef vMailShot As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters

                'developer guide no. 84(guide)
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyGroupTypeID:=vPartyGroupTypeID, vIsRegisteredCharity:=vIsRegisteredCharity, vCharityNumber:=vCharityNumber, vNumberofMembers:=vNumberofMembers, vTpsInd:=vTpsInd, vEmpsInd:=vEmpsInd, vMailShot:=vMailShot, vTurnover:=vTurnover, vIsFeeClient:=vIsFeeClient), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vPartyGroupTypeID:=vPartyGroupTypeID, vIsRegisteredCharity:=vIsRegisteredCharity, vCharityNumber:=vCharityNumber, vNumberofMembers:=vNumberofMembers, vTpsInd:=vTpsInd, vEmpsInd:=vEmpsInd, vMailShot:=vMailShot, vTurnover:=vTurnover), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyGC



                'developer guide no. 142(latest guide)
                .PartyCnt = vPartyCnt
                ' developer guide no. 115(guide)
                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                'developer guide no. 142(latest guide)
                .PartyGroupTypeID = vPartyGroupTypeID
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vPartyGroupTypeID)) AndAlso (Not vPartyGroupTypeID.Equals(0)) Then
                    .PartyGroupTypeID = vPartyGroupTypeID
                End If



                'developer guide no. 142(latest guide)
                .IsRegisteredCharity = vIsRegisteredCharity
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vIsRegisteredCharity)) AndAlso (Not vIsRegisteredCharity.Equals(0)) Then
                    .IsRegisteredCharity = vIsRegisteredCharity
                End If



                'developer guide no. 142(latest guide)
                .TpsInd = vTpsInd
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vTpsInd)) AndAlso (Not vTpsInd.Equals(0)) Then
                    .TpsInd = vTpsInd
                End If



                'developer guide no. 142(latest guide)
                .EmpsInd = vEmpsInd
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vEmpsInd)) AndAlso (Not vEmpsInd.Equals(0)) Then
                    .EmpsInd = vEmpsInd
                End If



                'developer guide no. 142(latest guide)
                .Mailshot = vMailShot
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vMailShot)) AndAlso (Not vMailShot.Equals(0)) Then
                    .Mailshot = vMailShot
                End If



                'Modeveloper guide no. 142(latest guide)
                .CharityNumber = vCharityNumber
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vCharityNumber)) AndAlso (Not String.IsNullOrEmpty(vCharityNumber)) Then
                    .CharityNumber = vCharityNumber
                End If


                'developer guide no. 142(latest guide)
                .NumberOfMembers = vNumberofMembers
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vNumberofMembers)) AndAlso (Not vNumberofMembers.Equals(0)) Then
                    .NumberOfMembers = vNumberofMembers
                End If



                ' developer guide no. 142(latest guide)
                .Turnover = vTurnover
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vTurnover)) AndAlso (Not Object.Equals(vTurnover, Nothing)) Then


                    'developer guide no. 24(guide)
                    .Turnover = vTurnover
                End If



                'developer guide no. 142(latest guide)
                .IsFeeClient = vIsFeeClient
                'developer guide no. 115(guide)
                If (Not Informations.IsNothing(vIsFeeClient)) AndAlso (Not vIsFeeClient.Equals(0)) Then
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
    ' Description: Returns the supplied SIRPartyGC property values.
    '
    ' ***************************************************************** '
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields.
    'developer guide no. 101(guide)
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyGroupTypeID As Object = Nothing, Optional ByRef vIsRegisteredCharity As Object = Nothing, Optional ByRef vCharityNumber As Object = Nothing, Optional ByRef vNumberofMembers As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vTpsInd As Object = Nothing, Optional ByRef vEmpsInd As Object = Nothing, Optional ByRef vMailShot As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyGC


                'developer guide no. 143(latest guide)
                vPartyCnt = .PartyCnt
                If Not Informations.IsNothing(vPartyCnt) Then
                    'developer guide no. 229(latest guide)
                    vPartyCnt = .PartyCnt.ToString()
                End If


                'developer guide no. 143(latest guide)
                vPartyGroupTypeID = .PartyGroupTypeID
                If Not Informations.IsNothing(vPartyGroupTypeID) Then
                    'developer guide no. 229(latest guide)
                    vPartyGroupTypeID = .PartyGroupTypeID.ToString()
                End If


                'developer guide no. 143(latest guide)
                vIsRegisteredCharity = .IsRegisteredCharity
                If Not Informations.IsNothing(vIsRegisteredCharity) Then
                    'developer guide no. 229(latest guide)
                    vIsRegisteredCharity = .IsRegisteredCharity.ToString()
                End If


                'developer guide no. 143(latest guide)
                vMailShot = .Mailshot
                If Not Informations.IsNothing(vMailShot) Then
                    'developer guide no. 229(latest guide)
                    vMailShot = .Mailshot.ToString()
                End If


                'developer guide no. 143(latest guide)
                vTpsInd = .TpsInd
                If Not Informations.IsNothing(vTpsInd) Then
                    'developer guide no. 229(latest guide)
                    vTpsInd = .TpsInd.ToString()
                End If


                'developer guide no. 143(latest guide)
                vEmpsInd = .EmpsInd
                If Not Informations.IsNothing(vEmpsInd) Then
                    'developer guide no. 229(latest guide)
                    vEmpsInd = .EmpsInd.ToString()
                End If


                'developer guide no. 143(latest guide)
                vCharityNumber = .CharityNumber
                If Not Informations.IsNothing(vCharityNumber) Then

                    If Convert.IsDBNull(vCharityNumber) Or Informations.IsNothing(vCharityNumber) Then

                        vCharityNumber = Nothing
                    Else
                        'developer guide no. 229(latest guide)
                        vCharityNumber = .CharityNumber.ToString()
                    End If
                End If


                'developer guide no. 143(latest guide)
                vNumberofMembers = .NumberOfMembers
                If Not Informations.IsNothing(vNumberofMembers) Then

                    If Convert.IsDBNull(vNumberofMembers) Or Informations.IsNothing(vNumberofMembers) Then

                        vNumberofMembers = Nothing
                    Else
                        'developer guide no. 229(latest guide)
                        vNumberofMembers = .NumberOfMembers.ToString()
                    End If
                End If


                'developer guide no. 143(latest guide)
                vTurnover = .Turnover
                If Not Informations.IsNothing(vTurnover) Then


                    'developer guide no. 229(latest guide)
                    vTurnover = .Turnover.ToString()
                End If


                'developer guide no. 143(latest guide)
                vIsFeeClient = .IsFeeClient
                If Not Informations.IsNothing(vIsFeeClient) Then
                    'developer guide no. 229(latest guide)
                    vIsFeeClient = .IsFeeClient.ToString()
                End If

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

            With m_dSIRPartyGC

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

            With m_dSIRPartyGC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyGC Added
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

            With m_dSIRPartyGC

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

            With m_dSIRPartyGC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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

            m_lReturn = m_dSIRPartyGC.CopyPartyToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRPartyGC.CopyPartyGCToEvent(v_lEventCnt)

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
    ' Description: Sets the Default Values for a SIRPartyGC.
    '
    ' ***************************************************************** '
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields.
    'developer guide no. 101(guide)
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyGroupTypeID As Object = Nothing, Optional ByRef vIsRegisteredCharity As Object = Nothing, Optional ByRef vCharityNumber As Object = Nothing, Optional ByRef vNumberofMembers As Object = Nothing, Optional ByRef vTpsInd As Object = Nothing, Optional ByRef vEmpsInd As Object = Nothing, Optional ByRef vMailShot As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Developer Guie No 151


        'If (Informations.IsNothing(vPartyCnt)) Or (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
        '	vPartyCnt = 0
        'End If



        'If (Informations.IsNothing(vPartyGroupTypeID)) Or (vPartyGroupTypeID.Equals(0)) Or (bDefaultAll) Then
        '	vPartyGroupTypeID = 0
        'End If



        'If (Informations.IsNothing(vIsRegisteredCharity)) Or (String.IsNullOrEmpty(vIsRegisteredCharity)) Or (bDefaultAll) Then
        '	vIsRegisteredCharity = ""
        'End If



        'If (Informations.IsNothing(vMailShot)) Or (String.IsNullOrEmpty(vMailShot)) Or (bDefaultAll) Then
        '	vMailShot = ""
        'End If



        'If (Informations.IsNothing(vTpsInd)) Or (String.IsNullOrEmpty(vTpsInd)) Or (bDefaultAll) Then
        '	vTpsInd = ""
        'End If



        'If (Informations.IsNothing(vEmpsInd)) Or (String.IsNullOrEmpty(vEmpsInd)) Or (bDefaultAll) Then
        '	vEmpsInd = ""
        'End If



        'If (Informations.IsNothing(vCharityNumber)) Or (vCharityNumber.Equals(0)) Or (bDefaultAll) Then
        '	vCharityNumber = 0
        'End If



        'If (Informations.IsNothing(vNumberofMembers)) Or (vNumberofMembers.Equals(0)) Or (bDefaultAll) Then
        '	vNumberofMembers = 0
        'End If



        'If (Informations.IsNothing(vTurnover)) Or (vTurnover.Equals(0)) Or (bDefaultAll) Then
        '	vTurnover = 0
        'End If



        'If Informations.IsNothing(vIsFeeClient) Or vIsFeeClient.Equals(0) Or bDefaultAll Then
        '	vIsFeeClient = 0
        'End If

        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vPartyGroupTypeID)) OrElse (vPartyGroupTypeID.Equals(0)) OrElse (bDefaultAll) Then
            vPartyGroupTypeID = 0
        End If



        If (Informations.IsNothing(vIsRegisteredCharity)) OrElse (String.IsNullOrEmpty(vIsRegisteredCharity)) OrElse (bDefaultAll) Then
            vIsRegisteredCharity = ""
        End If



        If (Informations.IsNothing(vMailShot)) OrElse (String.IsNullOrEmpty(vMailShot)) OrElse (bDefaultAll) Then
            vMailShot = ""
        End If



        If (Informations.IsNothing(vTpsInd)) OrElse (String.IsNullOrEmpty(vTpsInd)) OrElse (bDefaultAll) Then
            vTpsInd = ""
        End If



        If (Informations.IsNothing(vEmpsInd)) OrElse (String.IsNullOrEmpty(vEmpsInd)) OrElse (bDefaultAll) Then
            vEmpsInd = ""
        End If



        If (Informations.IsNothing(vCharityNumber)) OrElse (vCharityNumber.Equals(0)) OrElse (bDefaultAll) Then
            vCharityNumber = 0
        End If



        If (Informations.IsNothing(vNumberofMembers)) OrElse (vNumberofMembers.Equals(0)) OrElse (bDefaultAll) Then
            vNumberofMembers = 0
        End If



        If (Informations.IsNothing(vTurnover)) OrElse (vTurnover.Equals(0)) OrElse (bDefaultAll) Then
            vTurnover = 0
        End If



        If Informations.IsNothing(vIsFeeClient) OrElse vIsFeeClient.Equals(0) OrElse bDefaultAll Then
            vIsFeeClient = 0
        End If

        'Ends

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyGC for Consistency.
    '
    ' ***************************************************************** '
    'mkw090204 PN10359. Add TPS, Emps and Mailshot fields. START
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyGroupTypeID As Object = Nothing, Optional ByRef vIsRegisteredCharity As Object = Nothing, Optional ByRef vCharityNumber As Object = Nothing, Optional ByRef vNumberofMembers As Object = Nothing, Optional ByRef vTpsInd As Object = Nothing, Optional ByRef vEmpsInd As Object = Nothing, Optional ByRef vMailShot As Object = Nothing, Optional ByRef vTurnover As Object = Nothing) As Integer

        Dim result As Integer = 0




        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Informations.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

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
