Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

Friend NotInheritable Class SIRContact
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRContact
    '
    ' Date: 06/10/1998
    '
    ' Description: Describes the SIRContact attributes.
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
    Private Const ACClass As String = "SIRContact"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRContact As dSIRContact.SIRContact

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lContactCnt As Integer
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

    Public Property ContactCnt() As Integer
        Get

            Return m_lContactCnt

        End Get
        Set(ByVal Value As Integer)

            m_lContactCnt = Value

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
            m_dSIRContact = New dSIRContact.SIRContact()

            m_lReturn = m_dSIRContact.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRContact IsNot Nothing Then
                    m_dSIRContact.Dispose()
                End If
                m_dSIRContact = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRContact.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vContactCnt As Object = Nothing, Optional ByRef vContactTypeID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vContactID As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vAreaCode As Object = Nothing, Optional ByRef vNumber As Object = Nothing, Optional ByRef vExtension As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults













            'developer guide no. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vContactCnt:=vContactCnt, vContactTypeID:=vContactTypeID, vSourceID:=vSourceID, vContactID:=vContactID, vCountryID:=vCountryID, vDescription:=vDescription, vAreaCode:=vAreaCode, vNumber:=vNumber, vExtension:=vExtension, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified)

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
    ' Description: Sets the supplied SIRContact property values.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vContactCnt As Object = Nothing, Optional ByRef vContactTypeID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vContactID As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vAreaCode As Object = Nothing, Optional ByRef vNumber As Object = Nothing, Optional ByRef vExtension As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByVal sUniqueId As String = "", Optional ByVal sScreenHeirarchy As String = "") As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters




                'developer guide no. 98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vContactCnt:=vContactCnt, vContactTypeID:=vContactTypeID, vSourceID:=vSourceID, vContactID:=vContactID, vCountryID:=vCountryID, vDescription:=vDescription, vAreaCode:=vAreaCode, vNumber:=vNumber, vExtension:=vExtension, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            'm_lReturn = Validate(vContactCnt:=vContactCnt, vContactTypeID:=vContactTypeID, vSourceID:=vSourceID, vContactID:=vContactID, vCountryID:=vCountryID, vDescription:=vDescription, vAreaCode:=vAreaCode, vNumber:=vNumber, vExtension:=vExtension, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRContact


                If Not Informations.IsNothing(vContactCnt) Then
                    If .ContactCnt <> vContactCnt Then
                        .ContactCnt = vContactCnt
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vContactTypeID) Then
                    If .ContactTypeID <> vContactTypeID Then
                        .ContactTypeID = vContactTypeID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vSourceID) Then
                    If .SourceID <> vSourceID Then
                        .SourceID = vSourceID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vContactID) Then
                    If .ContactID <> vContactID Then
                        .ContactID = vContactID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vCountryID) Then
                    If .CountryID <> vCountryID Then
                        .CountryID = vCountryID
                        bDataChanged = True
                    End If
                End If



                If (Not Informations.IsNothing(vDescription)) And (Not Object.Equals(vDescription, Nothing)) Then


                    'developer guide no. 24
                    .Description = vDescription

                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vAreaCode)) And (Not Object.Equals(vAreaCode, Nothing)) Then


                    'developer guide no. 24
                    .AreaCode = vAreaCode

                    bDataChanged = True
                End If


                If Not Informations.IsNothing(vNumber) Then
                    If .Number.Trim() <> vNumber.Trim() Then
                        .Number = vNumber
                        bDataChanged = True
                    End If
                End If



                If (Not Informations.IsNothing(vExtension)) And (Not Object.Equals(vExtension, Nothing)) Then


                    'developer guide no. 24
                    .Extension = vExtension

                    bDataChanged = True
                End If


                If Not Informations.IsNothing(vCreatedByID) Then
                    If .CreatedByID <> vCreatedByID Then
                        .CreatedByID = vCreatedByID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vDateCreated) Then
                    If .DateCreated <> vDateCreated Then
                        .DateCreated = vDateCreated
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vModifiedByID) Then
                    If .ModifiedByID <> vModifiedByID Then
                        .ModifiedByID = vModifiedByID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vLastModified) Then


                    If Not .LastModified Is Nothing AndAlso Not .LastModified.Equals(vLastModified) Then


                        'developer guide no. 24
                        .LastModified = vLastModified

                        bDataChanged = True
                    End If
                End If

                If Not String.IsNullOrEmpty(sUniqueId) Then
                    .UniqueId = sUniqueId
                    .ScreenHeirarchy = sScreenHeirarchy
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRContact property values.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vContactCnt As Object = Nothing, Optional ByRef vContactTypeID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vContactID As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vAreaCode As Object = Nothing, Optional ByRef vNumber As Object = Nothing, Optional ByRef vExtension As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRContact


                'developer guide no. 118
                vContactCnt = .ContactCnt


                vContactTypeID = .ContactTypeID


                vSourceID = .SourceID


                vContactID = .ContactID


                vCountryID = .CountryID



                If Convert.IsDBNull(.Description) Or Informations.IsNothing(.Description) Then
                    vDescription = ""
                Else

                    vDescription = .Description
                End If



                If Convert.IsDBNull(.AreaCode) Or Informations.IsNothing(.AreaCode) Then
                    vAreaCode = ""
                Else

                    vAreaCode = .AreaCode
                End If

                vNumber = .Number



                If Convert.IsDBNull(.Extension) Or Informations.IsNothing(.Extension) Then
                    vExtension = ""
                Else

                    vExtension = .Extension
                End If


                vCreatedByID = .CreatedByID


                vDateCreated = .DateCreated


                vModifiedByID = .ModifiedByID




                vLastModified = .LastModified

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

            With m_dSIRContact

                ' Set Data object primary key
                .ContactCnt = ContactCnt

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

            With m_dSIRContact

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRContact Added
                ContactCnt = .ContactCnt

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

            With m_dSIRContact

                ' Set Data object primary key
                .ContactCnt = ContactCnt

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

            With m_dSIRContact

                ' Set Data object primary key
                .ContactCnt = ContactCnt

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
    ' Description: Sets the Default Values for a SIRContact.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vContactCnt As Object = Nothing, Optional ByRef vContactTypeID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vContactID As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vAreaCode As Object = Nothing, Optional ByRef vNumber As Object = Nothing, Optional ByRef vExtension As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no. 44
        If (Informations.IsNothing(vContactCnt)) OrElse (vContactCnt.Equals(0)) Or (bDefaultAll) Then
            vContactCnt = 0
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vContactTypeID)) OrElse (vContactTypeID.Equals(0)) Or (bDefaultAll) Then
            vContactTypeID = 0
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vSourceID)) OrElse (vSourceID.Equals(0)) Or (bDefaultAll) Then
            vSourceID = m_iSourceID
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vContactID)) OrElse (vContactID.Equals(0)) Or (bDefaultAll) Then
            vContactID = 0
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vCountryID)) OrElse (vCountryID.Equals(0)) Or (bDefaultAll) Then
            vCountryID = m_iLanguageID
        End If



        If (Informations.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Or (bDefaultAll) Then
            vDescription = ""
        End If



        If (Informations.IsNothing(vAreaCode)) Or (String.IsNullOrEmpty(vAreaCode)) Or (bDefaultAll) Then
            vAreaCode = ""
        End If



        If (Informations.IsNothing(vNumber)) Or (String.IsNullOrEmpty(vNumber)) Or (bDefaultAll) Then
            vNumber = ""
        End If



        If (Informations.IsNothing(vExtension)) Or (String.IsNullOrEmpty(vExtension)) Or (bDefaultAll) Then
            vExtension = ""
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vCreatedByID)) OrElse (vCreatedByID.Equals(0)) Or (bDefaultAll) Then
            vCreatedByID = m_iUserID
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vDateCreated)) OrElse (vDateCreated.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vModifiedByID)) OrElse (vModifiedByID.Equals(0)) Or (bDefaultAll) Then
            vModifiedByID = m_iUserID
        End If



        'developer guide no. 44
        If (Informations.IsNothing(vLastModified)) OrElse (vLastModified.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vLastModified = DateTime.Now
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRContact for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vContactCnt As Object = Nothing, Optional ByRef vContactTypeID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vContactID As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vAreaCode As Object = Nothing, Optional ByRef vNumber As Object = Nothing, Optional ByRef vExtension As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Informations.IsNothing(vContactCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vContactCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vContactTypeID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vContactTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vSourceID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vContactID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vContactID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCountryID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vCountryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCreatedByID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vCreatedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDateCreated) Then
            If Not Informations.IsDate(vDateCreated) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vModifiedByID) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vModifiedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLastModified) Then
            If Not Informations.IsDate(vLastModified) Then
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

