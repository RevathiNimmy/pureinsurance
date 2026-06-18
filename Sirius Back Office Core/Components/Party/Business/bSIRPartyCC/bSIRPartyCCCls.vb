Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
Friend NotInheritable Class SIRPartyCC
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyCC
    '
    ' Date: 12/10/1998
    '
    ' Description: Describes the SIRPartyCC attributes.
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
    Private Const ACClass As String = "SIRPartyCC"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyCC As dSIRPartyCC.SIRPartyCC ' was dSIRPartyCC.SIRPartyCC

    ' Instance of the Core SIRClaim object
    'Private m_bSIRParty As bSIRParty.Business
    Private m_bSIRParty As bSIRParty.Business

    ' Error Code
    Private m_lReturn As Integer

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
            m_dSIRPartyCC = New dSIRPartyCC.SIRPartyCC()
            '    Set m_dSIRPartyCC = New dSIRPartyCC.SIRPartyCC

            m_lReturn = m_dSIRPartyCC.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)



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
                If m_dSIRPartyCC IsNot Nothing Then
                    m_dSIRPartyCC.Dispose()
                End If
                m_dSIRPartyCC = Nothing
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
    ' Description: Returns the Default Values for the SIRPartyCC.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults












            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vCompanyReg:=vCompanyReg, vTradingSinceDate:=vTradingSinceDate, vPartyBusinessId:=vPartyBusinessId, vLocation:=vLocation, vNoOfOffices:=vNoOfOffices, vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear, vTradeCode:=vTradeCode, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vSICCodeId:=vSICCodeId, vIsFeeClient:=vIsFeeClient), gPMConstants.PMEReturnCode)

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
    ' Description: Sets the supplied SIRPartyCC property values.
    '
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    ' ***************************************************************** '
    'developer guide no. 213
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters



                'developer guide no. 213

                m_lReturn = DefaultParameters(bDefaultAll:=False,
           vPartyCnt:=vPartyCnt,
           vCompanyReg:=vCompanyReg,
           vTradingSinceDate:=vTradingSinceDate,
           vPartyBusinessId:=vPartyBusinessId,
           vLocation:=vLocation, vNoOfOffices:=vNoOfOffices,
           vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear,
           vTradeCode:=vTradeCode, vWageRoll:=vWageRoll,
           vTurnover:=vTurnover,
           vSICCodeId:=vSICCodeId, vSalutation:=vSalutation,
           vTradeID:=vTradeID,
           vTPSind:=vTPSind, vSource:=vSource,
           vMailshot:=vMailshot, vEMPSind:=vEMPSind,
           vTPPassword:=vTPPassword, vIsFeeClient:=vIsFeeClient)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            'DC 28/06/00 Added Correspondence Type Id
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vCompanyReg:=vCompanyReg, vTradingSinceDate:=vTradingSinceDate, vPartyBusinessId:=vPartyBusinessId, vLocation:=vLocation, vNoOfOffices:=vNoOfOffices, vNoOfEmployees:=vNoOfEmployees, vFinancialYear:=vFinancialYear, vTradeCode:=vTradeCode, vWageRoll:=vWageRoll, vTurnover:=vTurnover, vSICCodeId:=vSICCodeId, vSalutation:=vSalutation, vTradeID:=vTradeID, vTPSind:=vTPSind, vSource:=vSource, vMailshot:=vMailshot, vEMPSind:=vEMPSind, vTPPassword:=vTPPassword), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyCC



                .PartyCnt = vPartyCnt
                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If




                .CompanyReg = vCompanyReg
                If (Not Informations.IsNothing(vCompanyReg)) AndAlso (Not Object.Equals(vCompanyReg, Nothing)) Then


                    'developer guide no.24
                    .CompanyReg = vCompanyReg
                End If



                .TradingSinceDate = vTradingSinceDate
                If (Not Informations.IsNothing(vTradingSinceDate)) AndAlso (Not Object.Equals(vTradingSinceDate, Nothing)) Then


                    'developer guide no. 24
                    .TradingSinceDate = vTradingSinceDate
                End If



                .PartyBusinessId = vPartyBusinessId
                If (Not Informations.IsNothing(vPartyBusinessId)) AndAlso (Not Object.Equals(vPartyBusinessId, Nothing)) Then


                    'developer guide no. 24
                    .PartyBusinessId = vPartyBusinessId
                End If



                .Location = vLocation
                If (Not Informations.IsNothing(vLocation)) AndAlso (Not Object.Equals(vLocation, Nothing)) Then


                    ' developer guide no. 24
                    .Location = vLocation
                End If



                .NoOfOffices = vNoOfOffices
                If (Not Informations.IsNothing(vNoOfOffices)) AndAlso (Not Object.Equals(vNoOfOffices, Nothing)) Then


                    'developer guide no. 24
                    .NoOfOffices = vNoOfOffices
                End If



                .NoOfEmployees = vNoOfEmployees
                If (Not Informations.IsNothing(vNoOfEmployees)) AndAlso (Not Object.Equals(vNoOfEmployees, Nothing)) Then


                    'developer guide no. 
                    .NoOfEmployees = vNoOfEmployees
                End If



                .FinancialYear = vFinancialYear
                If (Not Informations.IsNothing(vFinancialYear)) AndAlso (Not Object.Equals(vFinancialYear, Nothing)) Then


                    'developer guide no. 24
                    .FinancialYear = vFinancialYear
                End If



                .TradeCode = vTradeCode
                If (Not Informations.IsNothing(vTradeCode)) AndAlso (Not Object.Equals(vTradeCode, Nothing)) Then


                    'developer guide no. 24
                    .TradeCode = vTradeCode
                End If



                .WageRoll = vWageRoll
                If (Not Informations.IsNothing(vWageRoll)) AndAlso (Not Object.Equals(vWageRoll, Nothing)) Then


                    'developer guide no. 24
                    .WageRoll = vWageRoll
                End If



                .Turnover = vTurnover
                If (Not Informations.IsNothing(vTurnover)) AndAlso (Not Object.Equals(vTurnover, Nothing)) Then


                    'developer guide no. 24
                    .Turnover = vTurnover
                End If



                .SICCodeId = vSICCodeId
                If (Not Informations.IsNothing(vSICCodeId)) AndAlso (Not Object.Equals(vSICCodeId, Nothing)) Then


                    'developer guide no. 24
                    .SICCodeId = vSICCodeId
                End If

                ' CTAF 250601


                .Salutation = vSalutation
                If (Not Informations.IsNothing(vSalutation)) AndAlso (Not String.IsNullOrEmpty(vSalutation)) Then
                    'developer guide no. 24
                    .Salutation = vSalutation
                End If

                ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade_id


                .TradeID = vTradeID
                If (Not Informations.IsNothing(vTradeID)) AndAlso (Not vTradeID.Equals(0)) Then
                    .TradeID = vTradeID
                End If

                'DD 24/10/2003


                .TPSind = vTPSind
                If (Not Informations.IsNothing(vTPSind)) AndAlso (Not vTPSind.Equals(0)) Then
                    .TPSind = vTPSind
                End If



                .Source = vSource
                If (Not Informations.IsNothing(vSource)) AndAlso (Not String.IsNullOrEmpty(vSource)) Then
                    .Source = vSource
                End If



                .Mailshot = vMailshot
                If (Not Informations.IsNothing(vMailshot)) AndAlso (Not vMailshot.Equals(0)) Then
                    .Mailshot = vMailshot
                End If



                .EMPSind = vEMPSind
                If (Not Informations.IsNothing(vEMPSind)) AndAlso (Not vEMPSind.Equals(0)) Then
                    .EMPSind = vEMPSind
                End If



                .TPPassword = vTPPassword
                If (Not Informations.IsNothing(vTPPassword)) AndAlso (Not String.IsNullOrEmpty(vTPPassword)) Then
                    .TPPassword = vTPPassword
                End If



                .IsFeeClient = vIsFeeClient
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
    ' Description: Returns the supplied SIRPartyCC property values.
    '
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    ' ***************************************************************** '
    'developer guide no. 213
    Public Function GetProperties(ByRef iStatus As Object, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyCC


                If Not Informations.IsNothing(vPartyCnt) Then
                    vPartyCnt = .PartyCnt
                End If


                If Not Informations.IsNothing(vCompanyReg) Then

                    If Not (Convert.IsDBNull(.CompanyReg) Or Informations.IsNothing(.CompanyReg)) Then


                        vCompanyReg = .CompanyReg
                    End If
                End If

                'MSS200901 - Add null check from UW

                If Not Informations.IsNothing(vTradingSinceDate) Then

                    If Not (Convert.IsDBNull(.TradingSinceDate) Or Informations.IsNothing(.TradingSinceDate)) Then


                        vTradingSinceDate = .TradingSinceDate
                    End If


                    vTradingSinceDate = .TradingSinceDate
                End If
                'MSS200901 - Merge End


                If Not Informations.IsNothing(vPartyBusinessId) Then


                    vPartyBusinessId = .PartyBusinessId
                End If

                If Not Informations.IsNothing(vLocation) Then


                    vLocation = .Location
                End If

                If Not Informations.IsNothing(vNoOfOffices) Then


                    vNoOfOffices = .NoOfOffices
                End If

                If Not Informations.IsNothing(vNoOfEmployees) Then


                    vNoOfEmployees = .NoOfEmployees
                End If

                'MSS200901 - Add null check from UW

                If Not Informations.IsNothing(vFinancialYear) Then
                    'RWH(17/05/01) Do Null check on dates.

                    If Not (Convert.IsDBNull(.FinancialYear) Or Informations.IsNothing(.FinancialYear)) Then


                        vFinancialYear = .FinancialYear
                    End If


                    vFinancialYear = .FinancialYear
                End If
                'MSS200901 - Merge End


                If Not Informations.IsNothing(vTradeCode) Then


                    vTradeCode = .TradeCode
                End If

                If Not Informations.IsNothing(vWageRoll) Then


                    vWageRoll = .WageRoll
                End If

                If Not Informations.IsNothing(vTurnover) Then


                    vTurnover = .Turnover
                End If

                If Not Informations.IsNothing(vSICCodeId) Then


                    vSICCodeId = .SICCodeId
                End If

                ' CTAF 250701

                If Convert.IsDBNull(.Salutation) OrElse Informations.IsNothing(.Salutation) Then
                    vSalutation = ""
                Else
                    vSalutation = .Salutation
                End If

                ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade_id

                If Not Informations.IsNothing(vTradeID) Then
                    vTradeID = .TradeID
                End If

                'DD 24/10/2003

                If Not Informations.IsNothing(vTPSind) Then
                    vTPSind = .TPSind
                End If

                If Not Informations.IsNothing(vSource) Then
                    vSource = .Source
                End If

                If Not Informations.IsNothing(vMailshot) Then
                    vMailshot = .Mailshot
                End If

                If Not Informations.IsNothing(vEMPSind) Then
                    vEMPSind = .EMPSind
                End If

                If Not Informations.IsNothing(vTPPassword) Then
                    vTPPassword = .TPPassword
                End If

                If Not Informations.IsNothing(vIsFeeClient) Then
                    vIsFeeClient = .IsFeeClient
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

            With m_dSIRPartyCC

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

            With m_dSIRPartyCC

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyCC Added
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

            With m_dSIRPartyCC

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

            With m_dSIRPartyCC

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

            m_lReturn = m_dSIRPartyCC.CopyPartyToEvent(v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_dSIRPartyCC.CopyPartyCCToEvent(v_lEventCnt)

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
    ' Description: Sets the Default Values for a SIRPartyCC.
    '
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    ' ***************************************************************** '
    'developer guide no. 213
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = #12/30/1899#, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Date = #12/30/1899#, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing, Optional ByRef vIsFeeClient As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vCompanyReg)) OrElse (String.IsNullOrEmpty(vCompanyReg)) Or (bDefaultAll) Then
            vCompanyReg = ""
        End If



        If (Informations.IsNothing(vTradingSinceDate)) OrElse (vTradingSinceDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            'DC 08/09/00 reset to todays date, otherwise cannot edit corporate client
            vTradingSinceDate = DateTime.Today
        End If



        If (Informations.IsNothing(vPartyBusinessId)) OrElse (String.IsNullOrEmpty(vPartyBusinessId)) Or (bDefaultAll) Then
            '        vPartyBusinessId = 0
            vPartyBusinessId = ""
        End If



        If (Informations.IsNothing(vLocation)) OrElse (vLocation.Equals(0)) Or (bDefaultAll) Then
            vLocation = 0
        End If



        If (Informations.IsNothing(vNoOfOffices)) OrElse (vNoOfOffices.Equals(0)) Or (bDefaultAll) Then
            vNoOfOffices = 0
        End If



        If (Informations.IsNothing(vNoOfEmployees)) OrElse (vNoOfEmployees.Equals(0)) Or (bDefaultAll) Then
            vNoOfEmployees = 0
        End If



        If (Informations.IsNothing(vFinancialYear)) OrElse (vFinancialYear.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vFinancialYear = DateTime.Now
        End If



        If (Informations.IsNothing(vTradeCode)) OrElse (String.IsNullOrEmpty(vTradeCode)) Or (bDefaultAll) Then
            vTradeCode = ""
        End If

        ' CF 070799


        If (Informations.IsNothing(vWageRoll)) OrElse (vWageRoll.Equals(0)) Or (bDefaultAll) Then
            vWageRoll = 0
        End If

        ' CF 070799


        If (Informations.IsNothing(vTurnover)) OrElse (vTurnover.Equals(0)) Or (bDefaultAll) Then
            vTurnover = 0
        End If



        If (Informations.IsNothing(vSICCodeId)) OrElse (Object.Equals(vSICCodeId, Nothing)) Or (bDefaultAll) Then


            vSICCodeId = DBNull.Value
        End If

        ' CTAF 250701


        If (Informations.IsNothing(vSalutation)) OrElse (String.IsNullOrEmpty(vSalutation)) Or (bDefaultAll) Then
            vSalutation = ""
        End If

        ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - trade id


        If (Informations.IsNothing(vTradeID)) OrElse (Object.Equals(vTradeID, Nothing)) Or (bDefaultAll) Then


            vTradeID = DBNull.Value
        End If

        'DD 24/10/2003


        If (Informations.IsNothing(vTPSind)) OrElse (vTPSind.Equals(0)) Or (bDefaultAll) Then
            vTPSind = 0
        End If



        If (Informations.IsNothing(vSource)) OrElse (String.IsNullOrEmpty(vSource)) Or (bDefaultAll) Then
            vSource = ""
        End If



        If (Informations.IsNothing(vMailshot)) OrElse (vMailshot.Equals(0)) Or (bDefaultAll) Then
            vMailshot = 0
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
    ' Description: Checks the SIRPartyCC for Consistency.
    '
    ' AMB 21-Oct-03: 1.8.6 Folgate EL0037 development - added trade_id
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vCompanyReg As Object = Nothing, Optional ByRef vTradingSinceDate As Object = Nothing, Optional ByRef vPartyBusinessId As Object = Nothing, Optional ByRef vLocation As Object = Nothing, Optional ByRef vNoOfOffices As Object = Nothing, Optional ByRef vNoOfEmployees As Object = Nothing, Optional ByRef vFinancialYear As Object = Nothing, Optional ByRef vTradeCode As Object = Nothing, Optional ByRef vWageRoll As Object = Nothing, Optional ByRef vTurnover As Object = Nothing, Optional ByRef vSICCodeId As Object = Nothing, Optional ByRef vSalutation As Object = Nothing, Optional ByRef vTradeID As Object = Nothing, Optional ByRef vTPSind As Object = Nothing, Optional ByRef vSource As Object = Nothing, Optional ByRef vMailshot As Object = Nothing, Optional ByRef vEMPSind As Object = Nothing, Optional ByRef vTPPassword As Object = Nothing) As Integer

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
