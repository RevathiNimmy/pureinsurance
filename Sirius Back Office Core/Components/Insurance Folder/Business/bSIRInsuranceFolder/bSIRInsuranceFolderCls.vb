Option Strict Off
Option Explicit On
Imports System.Globalization
'Developer Guide No. 129
Imports SSP.Shared

Friend NotInheritable Class SIRInsuranceFolder
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRInsuranceFolder
    '
    ' Date: 11/09/1998
    '
    ' Description: Describes the SIRInsuranceFolder attributes.
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
    Private Const ACClass As String = "SIRInsuranceFolder"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRInsuranceFolder As dSIRInsuranceFolder.SIRInsuranceFolder

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lInsuranceFolderCnt As Integer

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

    Public Property InsuranceFolderCnt() As Integer
        Get

            Return m_lInsuranceFolderCnt

        End Get
        Set(ByVal Value As Integer)

            m_lInsuranceFolderCnt = Value

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
            m_dSIRInsuranceFolder = New dSIRInsuranceFolder.SIRInsuranceFolder()

            m_lReturn = m_dSIRInsuranceFolder.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRInsuranceFolder IsNot Nothing Then
                    m_dSIRInsuranceFolder.Dispose()
                End If
                m_dSIRInsuranceFolder = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRInsuranceFolder.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFolderID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vInsuranceHolderCnt As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vInceptionDate As Object = Nothing, Optional ByRef vArcArchiveFolderID As Object = Nothing, Optional ByRef vQuoteInsuranceRef As Object = Nothing, Optional ByRef vNextInsuranceRef As Object = Nothing, Optional ByRef vLastInsuranceRef As Object = Nothing, Optional ByRef vRenewalCount As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults







            'Developer Guide No. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFolderID:=vInsuranceFolderID, vSourceID:=vSourceID, vInsuranceHolderCnt:=vInsuranceHolderCnt, vCode:=vCode, vDescription:=vDescription, vInceptionDate:=vInceptionDate, vArcArchiveFolderID:=vArcArchiveFolderID, vQuoteInsuranceRef:=vQuoteInsuranceRef, vNextInsuranceRef:=vNextInsuranceRef, vLastInsuranceRef:=vLastInsuranceRef, vRenewalCount:=vRenewalCount)

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
    ' Description: Sets the supplied SIRInsuranceFolder property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vInsuranceFolderCnt As Integer = 0, Optional ByRef vInsuranceFolderID As Integer = 0, Optional ByRef vSourceID As Integer = 0, Optional ByRef vInsuranceHolderCnt As Integer = 0, Optional ByRef vCode As String = "", Optional ByRef vDescription As Object = Nothing, Optional ByRef vInceptionDate As Object = Nothing, Optional ByRef vArcArchiveFolderID As Object = Nothing, Optional ByRef vQuoteInsuranceRef As Object = Nothing, Optional ByRef vNextInsuranceRef As Object = Nothing, Optional ByRef vLastInsuranceRef As Object = Nothing, Optional ByRef vRenewalCount As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters

                'Developer Guide No. 98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFolderID:=vInsuranceFolderID, vSourceID:=vSourceID, vInsuranceHolderCnt:=vInsuranceHolderCnt, vCode:=vCode, vDescription:=vDescription, vInceptionDate:=vInceptionDate, vArcArchiveFolderID:=vArcArchiveFolderID, vQuoteInsuranceRef:=vQuoteInsuranceRef, vNextInsuranceRef:=vNextInsuranceRef, vLastInsuranceRef:=vLastInsuranceRef, vRenewalCount:=vRenewalCount)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vInsuranceFolderCnt:=vInsuranceFolderCnt, vInsuranceFolderID:=vInsuranceFolderID, vSourceID:=vSourceID, vInsuranceHolderCnt:=vInsuranceHolderCnt, vCode:=vCode, vDescription:=vDescription, vInceptionDate:=vInceptionDate, vArcArchiveFolderID:=vArcArchiveFolderID, vQuoteInsuranceRef:=vQuoteInsuranceRef, vNextInsuranceRef:=vNextInsuranceRef, vLastInsuranceRef:=vLastInsuranceRef, vRenewalCount:=vRenewalCount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRInsuranceFolder



                If (Not Informations.IsNothing(vInsuranceFolderCnt)) And (Not vInsuranceFolderCnt.Equals(0)) Then
                    .InsuranceFolderCnt = vInsuranceFolderCnt
                End If



                If (Not Informations.IsNothing(vInsuranceFolderID)) And (Not vInsuranceFolderID.Equals(0)) Then
                    .InsuranceFolderID = vInsuranceFolderID
                End If



                If (Not Informations.IsNothing(vSourceID)) And (Not vSourceID.Equals(0)) Then
                    .SourceID = vSourceID
                End If



                If (Not Informations.IsNothing(vInsuranceHolderCnt)) And (Not vInsuranceHolderCnt.Equals(0)) Then
                    .InsuranceHolderCnt = vInsuranceHolderCnt
                End If



                If (Not Informations.IsNothing(vCode)) And (Not String.IsNullOrEmpty(vCode)) Then
                    .Code = vCode
                End If



                If (Not Informations.IsNothing(vDescription)) And (Not Object.Equals(vDescription, Nothing)) Then


                    ''Modified by Archana Tokas on 4/26/2010 4:47:08 PM changes as per requirement
                    '.set_Description(vDescription)
                    .Description = vDescription
                End If



                If (Not Informations.IsNothing(vInceptionDate)) And (Not Object.Equals(vInceptionDate, Nothing)) Then


                    ''Modified by Archana Tokas on 4/26/2010 4:47:08 PM changes as per requirement
                    '.set_InceptionDate(vInceptionDate)
                    .InceptionDate = vInceptionDate
                End If



                If (Not Informations.IsNothing(vArcArchiveFolderID)) And (Not Object.Equals(vArcArchiveFolderID, Nothing)) Then


                    ''Modified by Archana Tokas on 4/26/2010 4:47:08 PM changes as per requirement
                    '.set_ArcArchiveFolderID(vArcArchiveFolderID)
                    .ArcArchiveFolderID = vArcArchiveFolderID
                End If



                If (Not Informations.IsNothing(vQuoteInsuranceRef)) And (Not Object.Equals(vQuoteInsuranceRef, Nothing)) Then


                    'Modified by Archana Tokas on 4/26/2010 4:47:08 PM changes as per requirement
                    '.set_QuoteInsuranceRef(vQuoteInsuranceRef)
                    .QuoteInsuranceRef = vQuoteInsuranceRef
                End If



                If (Not Informations.IsNothing(vNextInsuranceRef)) And (Not Object.Equals(vNextInsuranceRef, Nothing)) Then


                    'Modified by Archana Tokas on 4/26/2010 4:47:08 PM changes as per requirement
                    '.set_NextInsuranceRef(vNextInsuranceRef)
                    .NextInsuranceRef = vNextInsuranceRef
                End If



                If (Not Informations.IsNothing(vLastInsuranceRef)) And (Not Object.Equals(vLastInsuranceRef, Nothing)) Then


                    'Modified by Archana Tokas on 4/26/2010 4:47:08 PM changes as per requirement
                    '.set_LastInsuranceRef(vLastInsuranceRef)
                    .LastInsuranceRef = vLastInsuranceRef
                End If



                If (Not Informations.IsNothing(vRenewalCount)) And (Not vRenewalCount.Equals(0)) Then
                    .RenewalCount = vRenewalCount
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
    ' Description: Returns the supplied SIRInsuranceFolder property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vInsuranceFolderCnt As Integer = 0, Optional ByRef vInsuranceFolderID As Integer = 0, Optional ByRef vSourceID As Integer = 0, Optional ByRef vInsuranceHolderCnt As Integer = 0, Optional ByRef vCode As String = "", Optional ByRef vDescription As Object = Nothing, Optional ByRef vInceptionDate As Object = Nothing, Optional ByRef vArcArchiveFolderID As Object = Nothing, Optional ByRef vQuoteInsuranceRef As Object = Nothing, Optional ByRef vNextInsuranceRef As Object = Nothing, Optional ByRef vLastInsuranceRef As Object = Nothing, Optional ByRef vRenewalCount As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRInsuranceFolder


                'If Not Informations.IsNothing(vInsuranceFolderCnt) Then 'Developer Guide No. 118
                vInsuranceFolderCnt = .InsuranceFolderCnt
                'End If


                'If Not Informations.IsNothing(vInsuranceFolderID) Then 'Developer Guide No. 118
                vInsuranceFolderID = .InsuranceFolderID
                'End If


                'If Not Informations.IsNothing(vSourceID) Then 'Developer Guide No. 118
                vSourceID = .SourceID
                'End If


                'If Not Informations.IsNothing(vInsuranceHolderCnt) Then 'Developer Guide No. 118
                vInsuranceHolderCnt = .InsuranceHolderCnt
                'End If


                'If Not Informations.IsNothing(vCode) Then 'Developer Guide No. 118
                vCode = .Code
                'End If


                'If Not Informations.IsNothing(vDescription) Then 'Developer Guide No. 118


                vDescription = .Description
                'End If


                'If Not Informations.IsNothing(vInceptionDate) Then 'Developer Guide No. 118


                vInceptionDate = .InceptionDate
                'End If


                'If Not Informations.IsNothing(vArcArchiveFolderID) Then 'Developer Guide No. 118


                vArcArchiveFolderID = .ArcArchiveFolderID
                'End If


                'If Not Informations.IsNothing(vQuoteInsuranceRef) Then 'Developer Guide No. 118


                vQuoteInsuranceRef = .QuoteInsuranceRef
                'End If


                'If Not Informations.IsNothing(vNextInsuranceRef) Then 'Developer Guide No. 118


                vNextInsuranceRef = .NextInsuranceRef
                'End If


                'If Not Informations.IsNothing(vLastInsuranceRef) Then 'Developer Guide No. 118


                vLastInsuranceRef = .LastInsuranceRef
                'End If


                'If Not Informations.IsNothing(vRenewalCount) Then 'Developer Guide No. 118
                vRenewalCount = .RenewalCount
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

            With m_dSIRInsuranceFolder

                ' Set Data object primary key
                .InsuranceFolderCnt = InsuranceFolderCnt

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

            With m_dSIRInsuranceFolder

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRInsuranceFolder Added
                InsuranceFolderCnt = .InsuranceFolderCnt

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

            With m_dSIRInsuranceFolder

                ' Set Data object primary key
                .InsuranceFolderCnt = InsuranceFolderCnt

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

            With m_dSIRInsuranceFolder

                ' Set Data object primary key
                .InsuranceFolderCnt = InsuranceFolderCnt

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
    ' Description: Sets the Default Values for a SIRInsuranceFolder.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFolderID As Object = Nothing, Optional ByRef vSourceID As Integer = 0, Optional ByRef vInsuranceHolderCnt As Object = 0, Optional ByRef vCode As String = "", Optional ByRef vDescription As Object = Nothing, Optional ByRef vInceptionDate As Date = #12/30/1899#, Optional ByRef vArcArchiveFolderID As Object = Nothing, Optional ByRef vQuoteInsuranceRef As Object = Nothing, Optional ByRef vNextInsuranceRef As Object = Nothing, Optional ByRef vLastInsuranceRef As Object = Nothing, Optional ByRef vRenewalCount As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vInsuranceFolderCnt)) Or (vInsuranceFolderCnt.Equals(0)) Or (bDefaultAll) Then
            vInsuranceFolderCnt = 0
        End If



        If (Informations.IsNothing(vInsuranceFolderID)) Or (vInsuranceFolderID.Equals(0)) Or (bDefaultAll) Then
            vInsuranceFolderID = 0
        End If



        If (Informations.IsNothing(vSourceID)) Or (vSourceID.Equals(0)) Or (bDefaultAll) Then
            vSourceID = m_iSourceID
        End If



        If (Informations.IsNothing(vInsuranceHolderCnt)) Or (vInsuranceHolderCnt.Equals(0)) Or (bDefaultAll) Then
            vInsuranceHolderCnt = 0
        End If



        If (Informations.IsNothing(vCode)) Or (String.IsNullOrEmpty(vCode)) Or (bDefaultAll) Then
            vCode = ""
        End If



        If (Informations.IsNothing(vDescription)) Or (Object.Equals(vDescription, Nothing)) Or (bDefaultAll) Then


            vDescription = DBNull.Value
        End If



        If (Informations.IsNothing(vInceptionDate)) Or (vInceptionDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vInceptionDate = DateTime.Now
        End If



        If (Informations.IsNothing(vArcArchiveFolderID)) Or (Object.Equals(vArcArchiveFolderID, Nothing)) Or (bDefaultAll) Then


            vArcArchiveFolderID = DBNull.Value
        End If



        If (Informations.IsNothing(vQuoteInsuranceRef)) Or (Object.Equals(vQuoteInsuranceRef, Nothing)) Or (bDefaultAll) Then


            vQuoteInsuranceRef = DBNull.Value
        End If



        If (Informations.IsNothing(vNextInsuranceRef)) Or (Object.Equals(vNextInsuranceRef, Nothing)) Or (bDefaultAll) Then


            vNextInsuranceRef = DBNull.Value
        End If



        If (Informations.IsNothing(vLastInsuranceRef)) Or (Object.Equals(vLastInsuranceRef, Nothing)) Or (bDefaultAll) Then


            vLastInsuranceRef = DBNull.Value
        End If



        If (Informations.IsNothing(vRenewalCount)) Or (vRenewalCount.Equals(0)) Or (bDefaultAll) Then
            vRenewalCount = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRInsuranceFolder for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vInsuranceFolderCnt As Object = Nothing, Optional ByRef vInsuranceFolderID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vInsuranceHolderCnt As Object = Nothing, Optional ByRef vCode As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vInceptionDate As Object = Nothing, Optional ByRef vArcArchiveFolderID As Object = Nothing, Optional ByRef vQuoteInsuranceRef As Object = Nothing, Optional ByRef vNextInsuranceRef As Object = Nothing, Optional ByRef vLastInsuranceRef As Object = Nothing, Optional ByRef vRenewalCount As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vInsuranceFolderCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vInsuranceFolderCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vInsuranceFolderID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vInsuranceFolderID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vSourceID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vInsuranceHolderCnt) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vInsuranceHolderCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vInceptionDate) Then

            If Not (Convert.IsDBNull(vInceptionDate) Or Informations.IsNothing(vInceptionDate)) Then
                If Not Informations.IsDate(vInceptionDate) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If


        If Not Informations.IsNothing(vArcArchiveFolderID) Then

            If Not (Convert.IsDBNull(vArcArchiveFolderID) Or Informations.IsNothing(vArcArchiveFolderID)) Then

                Dim dbNumericTemp5 As Double
                If Not Double.TryParse(CStr(vArcArchiveFolderID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
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

    ' ***************************************************************** '
    ' Name: DuplicatePolicyFix (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function DuplicatePolicyNumber() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dSIRInsuranceFolder

                ' Add a record to the database from the object
                m_lReturn = .DuplicatePolicyFix()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Fix duplicate policy number Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DuplicatePolicyNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

