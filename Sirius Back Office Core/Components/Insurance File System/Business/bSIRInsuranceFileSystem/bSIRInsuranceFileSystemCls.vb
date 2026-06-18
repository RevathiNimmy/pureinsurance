Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'Developer Guide No. 129
Friend NotInheritable Class SIRInsFileSys
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRInsuranceFileSystem
    '
    ' Date: 11/09/1998
    '
    ' Description: Describes the SIRInsuranceFileSystem attributes.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRInsuranceFileSystem"

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
    Private m_dSIRInsuranceFileSystem As dSIRInsuranceFileSystem.SIRInsFileSys

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lInsuranceFileCnt As Integer

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

    Public Property InsuranceFileCnt() As Integer
        Get

            Return m_lInsuranceFileCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFileCnt = Value

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
            m_dSIRInsuranceFileSystem = New dSIRInsuranceFileSystem.SIRInsFileSys()

            m_lReturn = m_dSIRInsuranceFileSystem.Initialise(sUserName:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRInsuranceFileSystem IsNot Nothing Then
                    m_dSIRInsuranceFileSystem.Dispose()
                    m_dSIRInsuranceFileSystem = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRInsuranceFileSystem.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vEndorsementCount As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastTransDate As Object = Nothing, Optional ByRef vLastTransTypeID As Object = Nothing, Optional ByRef vLastTransDescription As Object = Nothing, Optional ByRef vLastTransDebitCredit As Object = Nothing, Optional ByRef vLastTransDocumentRef As Object = Nothing, Optional ByRef vLastTransCoverStartDate As Object = Nothing, Optional ByRef vLastTransExpiryDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults




            'Developer Guide No 17. 
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vInsuranceFileCnt:=vInsuranceFileCnt, vEndorsementCount:=vEndorsementCount, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vLastTransDate:=vLastTransDate, vLastTransTypeID:=vLastTransTypeID, vLastTransDescription:=vLastTransDescription, vLastTransDebitCredit:=vLastTransDebitCredit, vLastTransDocumentRef:=vLastTransDocumentRef, vLastTransCoverStartDate:=vLastTransCoverStartDate, vLastTransExpiryDate:=vLastTransExpiryDate)

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
    ' Description: Sets the supplied SIRInsuranceFileSystem property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vInsuranceFileCnt As Integer = 0, Optional ByRef vEndorsementCount As Integer = 0, Optional ByRef vCreatedByID As Integer = 0, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastTransDate As Object = Nothing, Optional ByRef vLastTransTypeID As Object = Nothing, Optional ByRef vLastTransDescription As Object = Nothing, Optional ByRef vLastTransDebitCredit As Object = Nothing, Optional ByRef vLastTransDocumentRef As Object = Nothing, Optional ByRef vLastTransCoverStartDate As Object = Nothing, Optional ByRef vLastTransExpiryDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = DefaultParameters(bDefaultAll:=False, vInsuranceFileCnt:=vInsuranceFileCnt, vEndorsementCount:=vEndorsementCount, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vLastTransDate:=vLastTransDate, vLastTransTypeID:=vLastTransTypeID, vLastTransDescription:=vLastTransDescription, vLastTransDebitCredit:=vLastTransDebitCredit, vLastTransDocumentRef:=vLastTransDocumentRef, vLastTransCoverStartDate:=vLastTransCoverStartDate, vLastTransExpiryDate:=vLastTransExpiryDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vInsuranceFileCnt:=vInsuranceFileCnt, vEndorsementCount:=vEndorsementCount, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vLastTransDate:=vLastTransDate, vLastTransTypeID:=vLastTransTypeID, vLastTransDescription:=vLastTransDescription, vLastTransDebitCredit:=vLastTransDebitCredit, vLastTransDocumentRef:=vLastTransDocumentRef, vLastTransCoverStartDate:=vLastTransCoverStartDate, vLastTransExpiryDate:=vLastTransExpiryDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRInsuranceFileSystem



                If (Not Informations.IsNothing(vInsuranceFileCnt)) And (Not vInsuranceFileCnt.Equals(0)) Then
                    .InsuranceFileCnt = vInsuranceFileCnt
                End If



                If (Not Informations.IsNothing(vEndorsementCount)) And (Not vEndorsementCount.Equals(0)) Then
                    .EndorsementCount = vEndorsementCount
                End If



                If (Not Informations.IsNothing(vCreatedByID)) And (Not vCreatedByID.Equals(0)) Then
                    .CreatedByID = vCreatedByID
                End If



                If (Not Informations.IsNothing(vDateCreated)) And (Not vDateCreated.Equals(DateTime.FromOADate(0))) Then
                    .DateCreated = vDateCreated
                End If



                If (Not Informations.IsNothing(vModifiedByID)) And (Not Object.Equals(vModifiedByID, Nothing)) Then


                    .ModifiedByID = vModifiedByID
                End If



                If (Not Informations.IsNothing(vLastModified)) And (Not Object.Equals(vLastModified, Nothing)) Then


                    .LastModified = vLastModified
                End If



                If (Not Informations.IsNothing(vLastTransDate)) And (Not Object.Equals(vLastTransDate, Nothing)) Then


                    .LastTransDate = vLastTransDate
                End If



                If (Not Informations.IsNothing(vLastTransTypeID)) And (Not Object.Equals(vLastTransTypeID, Nothing)) Then


                    .LastTransTypeID = vLastTransTypeID
                End If



                If (Not Informations.IsNothing(vLastTransDescription)) And (Not Object.Equals(vLastTransDescription, Nothing)) Then


                    .LastTransDescription = vLastTransDescription
                End If



                If (Not Informations.IsNothing(vLastTransDebitCredit)) And (Not Object.Equals(vLastTransDebitCredit, Nothing)) Then



                    .LastTransDebitCredit = vLastTransDebitCredit
                End If



                If (Not Informations.IsNothing(vLastTransDocumentRef)) And (Not Object.Equals(vLastTransDocumentRef, Nothing)) Then


                    .LastTransDocumentRef = vLastTransDocumentRef
                End If



                If (Not Informations.IsNothing(vLastTransCoverStartDate)) And (Not Object.Equals(vLastTransCoverStartDate, Nothing)) Then


                    .LastTransCoverStartDate = vLastTransCoverStartDate
                End If



                If (Not Informations.IsNothing(vLastTransExpiryDate)) And (Not Object.Equals(vLastTransExpiryDate, Nothing)) Then


                    .LastTransExpiryDate = vLastTransExpiryDate
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
    ' Description: Returns the supplied SIRInsuranceFileSystem property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vEndorsementCount As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastTransDate As Object = Nothing, Optional ByRef vLastTransTypeID As Object = Nothing, Optional ByRef vLastTransDescription As Object = Nothing, Optional ByRef vLastTransDebitCredit As Object = Nothing, Optional ByRef vLastTransDocumentRef As Object = Nothing, Optional ByRef vLastTransCoverStartDate As Object = Nothing, Optional ByRef vLastTransExpiryDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRInsuranceFileSystem

                'developer guide no.118
                'If Not Informations.IsNothing(vInsuranceFileCnt) Then
                vInsuranceFileCnt = .InsuranceFileCnt
                'End If


                'If Not Informations.IsNothing(vEndorsementCount) Then
                vEndorsementCount = .EndorsementCount
                'End If


                'If Not Informations.IsNothing(vCreatedByID) Then
                vCreatedByID = .CreatedByID
                'End If


                'If Not Informations.IsNothing(vDateCreated) Then
                vDateCreated = .DateCreated
                'End If


                'If Not Informations.IsNothing(vModifiedByID) Then


                vModifiedByID = .ModifiedByID
                'End If


                'If Not Informations.IsNothing(vLastModified) Then


                vLastModified = .LastModified
                'End If


                'If Not Informations.IsNothing(vLastTransDate) Then


                vLastTransDate = .LastTransDate
                'End If


                'If Not Informations.IsNothing(vLastTransTypeID) Then


                vLastTransTypeID = .LastTransTypeID
                'End If


                'If Not Informations.IsNothing(vLastTransDescription) Then


                vLastTransDescription = .LastTransDescription
                'End If


                'If Not Informations.IsNothing(vLastTransDebitCredit) Then


                vLastTransDebitCredit = .LastTransDebitCredit
                'End If


                'If Not Informations.IsNothing(vLastTransDocumentRef) Then


                vLastTransDocumentRef = .LastTransDocumentRef
                'End If


                'If Not Informations.IsNothing(vLastTransCoverStartDate) Then


                vLastTransCoverStartDate = .LastTransCoverStartDate
                'End If


                'If Not Informations.IsNothing(vLastTransExpiryDate) Then


                vLastTransExpiryDate = .LastTransExpiryDate
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

            With m_dSIRInsuranceFileSystem

                ' Set Data object primary key
                .InsuranceFileCnt = InsuranceFileCnt

                'And if we're coming from events
                .FromEvent = FromEvent

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

            With m_dSIRInsuranceFileSystem

                .InsuranceFileCnt = InsuranceFileCnt

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRInsuranceFileSystem Added
                InsuranceFileCnt = .InsuranceFileCnt

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

            With m_dSIRInsuranceFileSystem

                ' Set Data object primary key
                .InsuranceFileCnt = InsuranceFileCnt

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

            With m_dSIRInsuranceFileSystem

                ' Set Data object primary key
                .InsuranceFileCnt = InsuranceFileCnt


                .LastTransDate = DateTime.Now

                .ModifiedByID = m_iUserID

                .LastModified = DateTime.Now

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
    ' Description: Sets the Default Values for a SIRInsuranceFileSystem.
    '
    ' ***************************************************************** '
    'Developer Guide No 17. 
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vEndorsementCount As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastTransDate As Object = Nothing, Optional ByRef vLastTransTypeID As Object = Nothing, Optional ByRef vLastTransDescription As Object = Nothing, Optional ByRef vLastTransDebitCredit As Object = Nothing, Optional ByRef vLastTransDocumentRef As Object = Nothing, Optional ByRef vLastTransCoverStartDate As Object = Nothing, Optional ByRef vLastTransExpiryDate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vInsuranceFileCnt)) Or (vInsuranceFileCnt.Equals(0)) Or (bDefaultAll) Then
            vInsuranceFileCnt = 0
        End If



        If (Informations.IsNothing(vEndorsementCount)) Or (vEndorsementCount.Equals(0)) Or (bDefaultAll) Then
            vEndorsementCount = 0
        End If



        If (Informations.IsNothing(vCreatedByID)) Or (vCreatedByID.Equals(0)) Or (bDefaultAll) Then
            vCreatedByID = m_iUserID
        End If



        If (Informations.IsNothing(vDateCreated)) Or (vDateCreated.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If



        If (Informations.IsNothing(vModifiedByID)) Or (Object.Equals(vModifiedByID, Nothing)) Or (bDefaultAll) Then


            vModifiedByID = DBNull.Value
        End If



        If (Informations.IsNothing(vLastModified)) Or (Object.Equals(vLastModified, Nothing)) Or (bDefaultAll) Then


            vLastModified = DBNull.Value
        End If



        If (Informations.IsNothing(vLastTransDate)) Or (Object.Equals(vLastTransDate, Nothing)) Or (bDefaultAll) Then


            vLastTransDate = DBNull.Value
        End If



        If (Informations.IsNothing(vLastTransTypeID)) Or (Object.Equals(vLastTransTypeID, Nothing)) Or (bDefaultAll) Then


            vLastTransTypeID = DBNull.Value
        End If



        If vLastTransDescription = "Renewals" Then
            vLastTransDescription = ""
        Else
            If (Information.IsNothing(vLastTransDescription)) Or (Object.Equals(vLastTransDescription, Nothing)) Or (bDefaultAll) Then
                vLastTransDescription = DBNull.Value
            End If
        End If



        If (Informations.IsNothing(vLastTransDebitCredit)) Or (Object.Equals(vLastTransDebitCredit, Nothing)) Or (bDefaultAll) Then


            vLastTransDebitCredit = DBNull.Value
        End If



        If (Informations.IsNothing(vLastTransDocumentRef)) Or (Object.Equals(vLastTransDocumentRef, Nothing)) Or (bDefaultAll) Then


            vLastTransDocumentRef = DBNull.Value
        End If



        If (Informations.IsNothing(vLastTransCoverStartDate)) Or (Object.Equals(vLastTransCoverStartDate, Nothing)) Or (bDefaultAll) Then


            vLastTransCoverStartDate = DBNull.Value
        End If



        If (Informations.IsNothing(vLastTransExpiryDate)) Or (Object.Equals(vLastTransExpiryDate, Nothing)) Or (bDefaultAll) Then


            vLastTransExpiryDate = DBNull.Value
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRInsuranceFileSystem for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vEndorsementCount As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vLastTransDate As Object = Nothing, Optional ByRef vLastTransTypeID As Object = Nothing, Optional ByRef vLastTransDescription As Object = Nothing, Optional ByRef vLastTransDebitCredit As Object = Nothing, Optional ByRef vLastTransDocumentRef As Object = Nothing, Optional ByRef vLastTransCoverStartDate As Object = Nothing, Optional ByRef vLastTransExpiryDate As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vInsuranceFileCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vInsuranceFileCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEndorsementCount) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vEndorsementCount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCreatedByID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vCreatedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDateCreated) Then
            If Not Informations.IsDate(vDateCreated) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vModifiedByID) Then

            If Not (Convert.IsDBNull(vModifiedByID) Or Informations.IsNothing(vModifiedByID)) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vModifiedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vLastModified) Then

            If Not (Convert.IsDBNull(vLastModified) Or Informations.IsNothing(vLastModified)) Then
                If Not Informations.IsDate(vLastModified) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vLastTransDate) Then

            If Not (Convert.IsDBNull(vLastTransDate) Or Informations.IsNothing(vLastTransDate)) Then
                If Not Informations.IsDate(vLastTransDate) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vLastTransTypeID) Then

            If Not (Convert.IsDBNull(vLastTransTypeID) Or Informations.IsNothing(vLastTransTypeID)) Then

                Dim dbNumericTemp5 As Double
                If Not Double.TryParse(CStr(vLastTransTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vLastTransCoverStartDate) Then

            If Not (Convert.IsDBNull(vLastTransCoverStartDate) Or Informations.IsNothing(vLastTransCoverStartDate)) Then
                If Not Informations.IsDate(vLastTransCoverStartDate) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vLastTransExpiryDate) Then

            If Not (Convert.IsDBNull(vLastTransExpiryDate) Or Informations.IsNothing(vLastTransExpiryDate)) Then
                If Not Informations.IsDate(vLastTransExpiryDate) Then
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

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Informations: http://www.vbtonet.com/ewis/ewi7001.aspx
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

