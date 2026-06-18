Option Strict Off
Option Explicit On
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyAGG
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyAGG
    '
    ' Date: 08/07/02
    '
    ' Description: Describes the SIRPartyAGG attributes.
    '              Created from bSIRPartyAG.
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
    Private Const ACClass As String = "SIRPartyAGG"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyAGG As dSIRPartyAGG.SIRPartyAGG ' was dSIRPartyAGG.SIRPartyAGG

    ' Instance of the Core SIRClaim object
    'Private m_bSIRParty As bSIRParty.Business
    Private m_bSIRParty As bSIRParty.Business

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
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


            ' Create instance of data class
            m_dSIRPartyAGG = New dSIRPartyAGG.SIRPartyAGG()
            '    Set m_dSIRPartyAGG = New dSIRPartyAGG.SIRPartyAGG

            m_lReturn = m_dSIRPartyAGG.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRPartyAGG IsNot Nothing Then
                    m_dSIRPartyAGG.Dispose()

                End If
                m_dSIRPartyAGG = Nothing
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
    ' Description: Returns the Default Values for the SIRPartyAGG.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'Developer Guide No. 98
            'm_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=CByte(vPartyCnt))
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt)

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
    ' Description: Sets the supplied SIRPartyAGG property values.
    '
    ' ***************************************************************** '
    'developer guide no.98
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vGroupActive As Object = Nothing, Optional ByVal vUniqueId As String = "", Optional ByVal vScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vGroupActive:=vGroupActive)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vPartyCnt:=vPartyCnt, vGroupActive:=vGroupActive)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyAGG


                'developer guide no.115
                If (Not Informations.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If

                If Not String.IsNullOrEmpty(vUniqueId) Then
                    .UniqueId = vUniqueId
                    .ScreenHierarchy = vScreenHierarchy
                End If

                '        If ((IsMissing(vPartyAgentOriginID) = False) _
                ''        And (IsEmpty(vPartyAgentOriginID) = False)) Then
                '            .PartyAgentOriginID = vPartyAgentOriginID
                '        End If
                '
                '        If ((IsMissing(vIsBranch) = False) _
                ''        And (IsEmpty(vIsBranch) = False)) Then
                '            .IsBranch = vIsBranch
                '        End If
                '


                If (Not Informations.IsNothing(vGroupActive)) Then
                    .PartyGroupActive = vGroupActive
                End If

                '        If ((IsMissing(vIsHeadOffice) = False) _
                ''        And (IsEmpty(vIsHeadOffice) = False)) Then
                '            .IsHeadOffice = vIsHeadOffice
                '        End If
                '
                '        If ((IsMissing(vAgencyAgreementDate) = False) _
                ''        And (IsEmpty(vAgencyAgreementDate) = False)) Then
                '            .AgencyAgreementDate = vAgencyAgreementDate
                '        End If
                '
                '        If ((IsMissing(vAgencyNextReviewDate) = False) _
                ''        And (IsEmpty(vAgencyNextReviewDate) = False)) Then
                '            .AgencyNextReviewDate = vAgencyNextReviewDate
                '        End If
                '
                '        If ((IsMissing(vAgencyAccountNumber) = False) _
                ''        And (IsEmpty(vAgencyAccountNumber) = False)) Then
                '            .AgencyAccountNumber = vAgencyAccountNumber
                '        End If
                '
                '        If ((IsMissing(vDefaultCommissionPercent) = False) _
                ''        And (IsEmpty(vDefaultCommissionPercent) = False)) Then
                '            .DefaultCommissionPercent = vDefaultCommissionPercent
                '        End If
                '
                '        If ((IsMissing(vTradingName) = False) _
                ''        And (IsEmpty(vTradingName) = False)) Then
                '            .TradingName = vTradingName
                '        End If
                '
                '        If ((IsMissing(vBinderIndicator) = False) _
                ''        And (IsEmpty(vBinderIndicator) = False)) Then
                '            .BinderIndicator = vBinderIndicator
                '        End If
                '
                '        If ((IsMissing(vReportIndicator) = False) _
                ''        And (IsEmpty(vReportIndicator) = False)) Then
                '            .ReportIndicator = vReportIndicator
                '        End If
                '
                '        If ((IsMissing(vWitholdingTax) = False) _
                ''        And (IsEmpty(vWitholdingTax) = False)) Then
                '            .WitholdingTax = vWitholdingTax
                '        End If

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
    ' Description: Returns the supplied SIRPartyAGG property values.
    '
    ' ***************************************************************** '
    'developer guide no.98
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vGroupActive As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyAGG


                'Developer Guide No. 118
                'If Not Informations.IsNothing(vPartyCnt) Then
                vPartyCnt = .PartyCnt
                'End If

                ' CF 050799

                'Developer Guide No. 118
                'If Not Informations.IsNothing(vGroupActive) Then
                vGroupActive = .PartyGroupActive
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

            With m_dSIRPartyAGG

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

            With m_dSIRPartyAGG

                ' Set Data object primary key
                .PartyCnt = PartyCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyAGG Added
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

            With m_dSIRPartyAGG

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

            With m_dSIRPartyAGG

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
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a SIRPartyAGG.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    'Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Byte = 0, Optional ByRef vGroupActive As Byte = 0) As Integer
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vGroupActive As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}


        'developer guide no.44
        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If
        'EK 27/9/99 set defaults

        'developer guide no.44
        If (Informations.IsNothing(vGroupActive)) OrElse (bDefaultAll) Then
            vGroupActive = 1
        End If

        '    If ((IsMissing(vPartyAgentOriginID) = True) _
        ''    Or (IsEmpty(vPartyAgentOriginID) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vPartyAgentOriginID = 1
        '    End If
        '
        '    If ((IsMissing(vIsBranch) = True) _
        ''    Or (IsEmpty(vIsBranch) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vIsBranch = 1
        '    End If
        '
        '    If ((IsMissing(vIsHeadOffice) = True) _
        ''    Or (IsEmpty(vIsHeadOffice) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vIsHeadOffice = 0
        '    End If
        '
        '    If ((IsMissing(vAgencyAgreementDate) = True) _
        ''    Or (IsEmpty(vAgencyAgreementDate) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vAgencyAgreementDate = Now
        '    End If
        '
        '    If ((IsMissing(vAgencyNextReviewDate) = True) _
        ''    Or (IsEmpty(vAgencyNextReviewDate) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vAgencyNextReviewDate = Now
        '    End If
        '
        '    If ((IsMissing(vDefaultCommissionPercent) = True) _
        ''    Or (IsEmpty(vDefaultCommissionPercent) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vDefaultCommissionPercent = 10
        '    End If
        '
        '    If ((IsMissing(vWitholdingTax) = True) _
        ''    Or (IsEmpty(vWitholdingTax) = True) _
        ''    Or (bDefaultAll = True)) Then
        '        vWitholdingTax = 0
        '    End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyAGG for Consistency.

    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vGroupActive As Object = Nothing) As Integer

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

        '    If (IsMissing(vPartyAgentOriginID) = False) Then
        '        If (IsNumeric(vPartyAgentOriginID) = False) Then
        '            Validate = PMFalse
        '            Exit Function
        '        End If
        '    End If
        '
        '    If (IsMissing(vAgencyAgreementDate) = False) Then
        '        If (IsDate(vAgencyAgreementDate) = False) Then
        '            Validate = PMFalse
        '            Exit Function
        '        End If
        '    End If
        '
        '    If (IsMissing(vAgencyNextReviewDate) = False) Then
        '        If (IsDate(vAgencyNextReviewDate) = False) Then
        '            Validate = PMFalse
        '            Exit Function
        '        End If
        '    End If

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

