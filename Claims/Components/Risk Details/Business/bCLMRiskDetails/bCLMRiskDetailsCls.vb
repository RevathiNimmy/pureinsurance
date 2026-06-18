Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class RiskDetails
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMRiskDetails
    '
    ' Date: {TodaysDate}
    '
    ' Description: Describes the CLMRiskDetails attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMRiskDetails"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)

    ' ************************************************
    ' Added to replace global variables 24/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************

    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMRiskDetails As dCLMRiskDetails.CLMRiskDetails

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    '***PrimaryKeys***
    Private m_lClaimID As Integer
    Private m_lRiskTypeID As Integer
    Private m_lRiskDataDefnID As Integer

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
    '***PrimaryKeyLetGets***


    Public Property ClaimId() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

        End Set
    End Property



    Public Property RiskTypeID() As Integer
        Get

            Return m_lRiskTypeID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskTypeID = Value

        End Set
    End Property




    Public Property RiskDataDefnID() As Integer
        Get

            Return m_lRiskDataDefnID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskDataDefnID = Value

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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            ' Create instance of data class
            m_dCLMRiskDetails = New dCLMRiskDetails.CLMRiskDetails()

            m_lReturn = m_dCLMRiskDetails.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
                If m_dCLMRiskDetails IsNot Nothing Then
                    m_dCLMRiskDetails.Dispose()

                End If
                m_dCLMRiskDetails = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CLMRiskDetails.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vClaimNumber:=vClaimNumber, vPerilDescription:=vPerilDescription, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied CLMRiskDetails property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'developer guide no.98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vClaimNumber:=vClaimNumber, vPerilDescription:=vPerilDescription, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vProgressStatusID:=vProgressStatusID, vClaimStatusID:=vClaimStatusID, vClaimDescription:=vClaimDescription, vPrimaryCauseID:=vPrimaryCauseID, vSecondaryCauseID:=vSecondaryCauseID, vPerilTypeID:=vPerilTypeID, vClaimNumber:=vClaimNumber, vPerilDescription:=vPerilDescription, vSumInsured:=vSumInsured, vCurrentReserve:=vCurrentReserve, vComments:=vComments)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dCLMRiskDetails


                If Not Informations.IsNothing(vProgressStatusID) Then
                    If .ProgressStatusID <> vProgressStatusID Then
                        .ProgressStatusID = vProgressStatusID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vClaimStatusID) Then
                    If .ClaimStatusID <> vClaimStatusID Then
                        .ClaimStatusID = vClaimStatusID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vClaimDescription) Then
                    If .ClaimDescription <> vClaimDescription Then
                        .ClaimDescription = vClaimDescription
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vPrimaryCauseID) Then
                    If .PrimaryCauseID <> vPrimaryCauseID Then
                        .PrimaryCauseID = vPrimaryCauseID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vSecondaryCauseID) Then
                    If .SecondaryCauseID <> vSecondaryCauseID Then
                        .SecondaryCauseID = vSecondaryCauseID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vPerilTypeID) Then
                    If .PerilTypeID <> vPerilTypeID Then
                        .PerilTypeID = vPerilTypeID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vPerilDescription) Then
                    If .PerilDescription <> vPerilDescription Then
                        .PerilDescription = vPerilDescription
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vClaimNumber) Then
                    If .ClaimNumber <> vClaimNumber Then
                        .ClaimNumber = vClaimNumber
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vSumInsured) Then
                    If .SumInsured <> vSumInsured Then
                        .SumInsured = vSumInsured
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vCurrentReserve) Then
                    If .CurrentReserve <> vCurrentReserve Then
                        .CurrentReserve = vCurrentReserve
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vComments) Then
                    If .Comments <> vComments Then
                        .Comments = vComments
                        bDataChanged = True
                    End If
                End If


                ' If we have changed one of the properties, update the status
                If bDataChanged Then
                    m_iDatabaseStatus = iStatus
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied CLMRiskDetails property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dCLMRiskDetails

                'developer guide no.118
                'If Not Informations.IsNothing(vProgressStatusID) Then
                vProgressStatusID = .ProgressStatusID
                'End If


                'If Not Informations.IsNothing(vClaimStatusID) Then
                vClaimStatusID = .ClaimStatusID
                'End If


                'If Not Informations.IsNothing(vClaimDescription) Then
                vClaimDescription = .ClaimDescription
                'End If


                'If Not Informations.IsNothing(vPrimaryCauseID) Then
                vPrimaryCauseID = .PrimaryCauseID
                'End If


                'If Not Informations.IsNothing(vSecondaryCauseID) Then
                vSecondaryCauseID = .SecondaryCauseID
                'End If


                'If Not Informations.IsNothing(vPerilTypeID) Then
                vPerilTypeID = .PerilTypeID
                'End If


                'If Not Informations.IsNothing(vPerilDescription) Then
                vPerilDescription = .PerilDescription
                'End If


                'If Not Informations.IsNothing(vClaimNumber) Then
                vClaimNumber = .ClaimNumber
                'End If


                'If Not Informations.IsNothing(vSumInsured) Then
                vSumInsured = .SumInsured
                'End If


                'If Not Informations.IsNothing(vCurrentReserve) Then
                vCurrentReserve = .CurrentReserve
                'End If


                'If Not Informations.IsNothing(vComments) Then
                vComments = .Comments
                'End If

                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectItem (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function SelectItem(ByRef iIndex As Integer) As Integer

        Dim result As Integer = 0
        Try


            '    With m_dCLMRiskDetails
            '        Select Case iIndex
            '        Case g_iClaim
            '            ' Set Data object primary key
            '            .PerilTypeID = pe
            '            ' Select a record from the database
            '            m_lReturn& = .SelectSingle(iIndex)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                SelectItem = PMFalse
            '                Exit Function
            '            End If
            '        Case g_iRiskType
            '            ' Set Data object primary key
            '            .RiskTypeID = RiskTypeID
            '            ' Select a record from the database
            '            m_lReturn& = .SelectSingle(iIndex)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                SelectItem = PMFalse
            '                Exit Function
            '            End If
            '        Case g_iRiskDataDefn
            '            ' Set Data object primary key
            '            .RiskDataDefnID = RiskDataDefnID
            '            ' Select a record from the database
            '            m_lReturn& = .SelectSingle(iIndex)
            '
            '            If (m_lReturn& <> PMTrue) Then
            '                SelectItem = PMFalse
            '                Exit Function
            '            End If
            '        End Select
            '    End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_dCLMRiskDetails

                ' Add a record to the database from the object
                m_lReturn = .Add(1)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMRiskDetails Added

                'ClaimID = .ClaimID
                'RiskTypeID = .RiskTypeID
                'RiskDataDefnID = .RiskDataDefnID

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            '    With m_dCLMRiskDetails
            '
            '        ' Set Data object primary key
            '        .ClaimID = ClaimID
            '        .RiskTypeID = RiskTypeID
            '        .RiskDataDefnID = RiskDataDefnID
            '
            '        ' Update the record on the database from the object
            '        m_lReturn& = .Delete()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            DeleteItem = PMFalse
            '            Exit Function
            '        End If
            '
            '    End With

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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


            '    With m_dCLMRiskDetails
            '
            '        ' Set Data object primary key
            '        .ClaimID = ClaimID
            '        .RiskTypeID = RiskTypeID
            '        .RiskDataDefnID = RiskDataDefnID
            '
            '        ' Update the record on the database from the object
            '        m_lReturn& = .Update()
            '
            '        If (m_lReturn& <> PMTrue) Then
            '            UpdateItem = PMFalse
            '            Exit Function
            '        End If
            '
            '    End With
            '
            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a CLMRiskDetails.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}


        'developer guide no.118
        If (Informations.IsNothing(vProgressStatusID)) OrElse (vProgressStatusID.Equals(0)) OrElse (bDefaultAll) Then
            vProgressStatusID = 0
        End If



        If (Informations.IsNothing(vClaimStatusID)) OrElse (vClaimStatusID.Equals(0)) OrElse (bDefaultAll) Then
            vClaimStatusID = 0
        End If



        If (Informations.IsNothing(vClaimDescription)) OrElse (vClaimDescription.Equals(0)) OrElse (bDefaultAll) Then
            vClaimDescription = 0
        End If



        If (Informations.IsNothing(vPrimaryCauseID)) OrElse (vPrimaryCauseID.Equals(0)) OrElse (bDefaultAll) Then
            vPrimaryCauseID = 0
        End If



        If (Informations.IsNothing(vSecondaryCauseID)) OrElse (vSecondaryCauseID.Equals(0)) OrElse (bDefaultAll) Then
            vSecondaryCauseID = 0
        End If



        If (Informations.IsNothing(vPerilTypeID)) OrElse (vPerilTypeID.Equals(0)) OrElse (bDefaultAll) Then
            vPerilTypeID = 0
        End If



        If (Informations.IsNothing(vPerilDescription)) OrElse (vPerilDescription.Equals(0)) OrElse (bDefaultAll) Then
            vPerilDescription = 0
        End If



        If (Informations.IsNothing(vClaimNumber)) OrElse (vClaimNumber.Equals(0)) OrElse (bDefaultAll) Then
            vClaimNumber = 0
        End If



        If (Informations.IsNothing(vSumInsured)) OrElse (vSumInsured.Equals(0)) OrElse (bDefaultAll) Then
            vSumInsured = 0
        End If



        If (Informations.IsNothing(vCurrentReserve)) OrElse (vCurrentReserve.Equals(0)) OrElse (bDefaultAll) Then
            vCurrentReserve = 0
        End If



        If (Informations.IsNothing(vComments)) OrElse (vComments.Equals(0)) OrElse (bDefaultAll) Then
            vComments = 0
        End If
        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the CLMRiskDetails for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vProgressStatusID As Object = Nothing, Optional ByRef vClaimStatusID As Object = Nothing, Optional ByRef vClaimDescription As Object = Nothing, Optional ByRef vPrimaryCauseID As Object = Nothing, Optional ByRef vSecondaryCauseID As Object = Nothing, Optional ByRef vClaimNumber As Object = Nothing, Optional ByRef vPerilTypeID As Object = Nothing, Optional ByRef vPerilDescription As Object = Nothing, Optional ByRef vSumInsured As Object = Nothing, Optional ByRef vCurrentReserve As Object = Nothing, Optional ByRef vComments As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vProgressStatusID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vProgressStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vClaimStatusID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vClaimStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vClaimDescription) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Not Informations.IsNothing(vPrimaryCauseID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vPrimaryCauseID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vSecondaryCauseID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vSecondaryCauseID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPerilTypeID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vPerilTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vPerilDescription) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Not Informations.IsNothing(vClaimNumber) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Not Informations.IsNothing(vSumInsured) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vSumInsured), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCurrentReserve) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vCurrentReserve), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vComments) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        ' {* USER DEFINED CODE (End) *}

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
        '
        ' Log Error Message
        'bPMFunc.LogMessage("", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Class_Initialize", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class

