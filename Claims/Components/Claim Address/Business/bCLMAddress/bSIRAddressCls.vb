Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'Developer Guide no 129
Imports SharedFiles
Friend NotInheritable Class SIRAddress
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRAddress
    '
    ' Date: 08/10/1998
    '
    ' Description: Describes the SIRAddress attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SIRAddress"

    ' ************************************************
    ' Added to replace global variables
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_oDatabase As dPMDAO.Database
    ' ************************************************

    ' Update Status
    Private m_iDatabaseStatus As gPMConstants.PMEComponentAction
    ' Instance of Data component
    Private m_dSIRAddress As dCLMAddress.SIRAddress
    ' Error Code
    Private m_lReturn As Integer
    ' Primary Keys to work with
    Private m_lAddressCnt As Integer

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property
    Public Property AddressCnt() As Integer
        Get
            Return m_lAddressCnt
        End Get
        Set(ByVal Value As Integer)
            m_lAddressCnt = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_sCallingAppName = v_sCallingAppName
            m_iLanguageID = v_iLanguageID
            m_iSourceID = v_iSourceID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel
            m_oDatabase = v_vDatabase

            ' Create instance of data class
            m_dSIRAddress = New dCLMAddress.SIRAddress()

            m_lReturn = m_dSIRAddress.Initialise(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, v_vDatabase)

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
        Me.disposedValue = True
        If Not Me.disposedValue Then
            If disposing Then
                If m_dSIRAddress IsNot Nothing Then
                    m_dSIRAddress.Dispose()

                End If
                m_dSIRAddress = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRAddress.
    '
    ' ***************************************************************** '
    ' DJM 07/05/2002 : Added vAddressUsageTypeID as a parameter
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAddressCnt:=vAddressCnt, vSourceID:=vSourceID, vAddressID:=vAddressID, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified)

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
    ' Description: Sets the supplied SIRAddress property values.
    '
    ' ***************************************************************** '
    'DJM 01/05/2002 : Added vAddressUsageTypeID as a parameter
    'AR20050404 - PN15664 Add UpdateAddress parameter
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing, Optional ByRef vUpdateGlobalAddress As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'DJM 01/05/2002 : Added vAddressUsageTypeID as a parameter
                'AR20050404 - PN15664 Add UpdateAddress parameter

                'developer guide no.98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vAddressCnt:=vAddressCnt, vSourceID:=vSourceID, vAddressID:=vAddressID, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vAddressUsageTypeID:=vAddressUsageTypeID, vUpdateGlobalAddress:=vUpdateGlobalAddress)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            'DJM 01/05/2002 : Added vAddressUsageTypeID as a parameter
            m_lReturn = Validate(vAddressCnt:=vAddressCnt, vSourceID:=vSourceID, vAddressID:=vAddressID, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vAddressUsageTypeID:=vAddressUsageTypeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRAddress


                If Not Information.IsNothing(vAddressCnt) Then
                    If .AddressCnt <> vAddressCnt Then
                        .AddressCnt = vAddressCnt
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vSourceID) Then
                    If .SourceID <> vSourceID Then
                        .SourceID = vSourceID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vAddressID) Then
                    If .AddressID <> vAddressID Then
                        .AddressID = vAddressID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vAddress1) Then
                    If .Address1.Trim() <> vAddress1.Trim() Then
                        .Address1 = vAddress1
                        bDataChanged = True
                    End If
                End If



                If (Not Information.IsNothing(vAddress2)) And (Not Object.Equals(vAddress2, Nothing)) Then


                    'Developer Guide no 24
                    .Address2 = vAddress2
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vAddress3)) And (Not Object.Equals(vAddress3, Nothing)) Then


                    'Developer Guide no 24
                    .Address3 = vAddress3
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vAddress4)) And (Not Object.Equals(vAddress4, Nothing)) Then


                    'Developer Guide no 24
                    .Address4 = vAddress4
                    bDataChanged = True
                End If


                If Not Information.IsNothing(vPostalCode) Then
                    If .PostalCode.Trim() <> vPostalCode.Trim() Then
                        .PostalCode = vPostalCode
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vCountryID) Then
                    If .CountryID <> vCountryID Then
                        .CountryID = vCountryID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vCreatedByID) Then
                    If .CreatedByID <> vCreatedByID Then
                        .CreatedByID = vCreatedByID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vDateCreated) Then
                    If .DateCreated <> vDateCreated Then
                        .DateCreated = vDateCreated
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vModifiedByID) Then
                    If .ModifiedByID <> vModifiedByID Then
                        .ModifiedByID = vModifiedByID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vLastModified) Then


                    If Not .LastModified.Equals(vLastModified) Then


                        'Developer Guide no 24
                        .LastModified = vLastModified
                        bDataChanged = True
                    End If
                End If

                'DJM 07/05/2002 : Set data object with new property

                If Not Information.IsNothing(vAddressUsageTypeID) Then
                    If .AddressUsageTypeID <> vAddressUsageTypeID Then
                        .AddressUsageTypeID = vAddressUsageTypeID
                        bDataChanged = True
                    End If
                End If

                'AR20050404 - PN15664

                If Not Information.IsNothing(vUpdateGlobalAddress) Then
                    .UpdateGlobalAddress = vUpdateGlobalAddress
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SIRAddress property values.
    '
    ' ***************************************************************** '
    'DJM 07/05/2002 : Added vAddressUsageTypeID as a parameter
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRAddress
                'developer guide no.118
                'start


                vAddressCnt = .AddressCnt

                vSourceID = .SourceID

                vAddressID = .AddressID

                vAddress1 = .Address1

                If Convert.IsDBNull(.Address2) Or IsNothing(.Address2) Then
                    vAddress2 = ""
                Else

                    vAddress2 = .Address2
                End If

                If Convert.IsDBNull(.Address3) Or IsNothing(.Address3) Then
                    vAddress3 = ""
                Else

                    vAddress3 = .Address3
                End If


                If Convert.IsDBNull(.Address4) Or IsNothing(.Address4) Then
                    vAddress4 = ""
                Else

                    vAddress4 = .Address4
                End If

                vPostalCode = .PostalCode

                vCountryID = .CountryID

                vCreatedByID = .CreatedByID

                vDateCreated = .DateCreated

                vModifiedByID = .ModifiedByID

                vLastModified = .LastModified

                'DJM 07/05/2002 : Set new property from data object

                vAddressUsageTypeID = .AddressUsageTypeID
                'end
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

            With m_dSIRAddress

                ' Set Data object primary key
                .AddressCnt = AddressCnt

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

            With m_dSIRAddress

                'Check if the record exists so that it is not added

                m_lReturn = .Check()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                If .AddressCnt <> 0 Then
                    AddressCnt = .AddressCnt
                Else

                    ' Add a record to the database from the object
                    m_lReturn = .Add()

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Retain the Primary Key of the SIRAddress Added
                    AddressCnt = .AddressCnt
                End If

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

            With m_dSIRAddress

                ' Set Data object primary key
                .AddressCnt = AddressCnt

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

            With m_dSIRAddress

                ' Set Data object primary key
                .AddressCnt = AddressCnt

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
    ' Description: Sets the Default Values for a SIRAddress.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing, Optional ByRef vUpdateGlobalAddress As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Information.IsNothing(vAddressCnt)) Or (vAddressCnt.Equals(0)) Or (bDefaultAll) Then
            vAddressCnt = 0
        End If



        If (Information.IsNothing(vSourceID)) Or (vSourceID.Equals(0)) Or (bDefaultAll) Then
            vSourceID = m_iSourceID
        End If



        If (Information.IsNothing(vAddressID)) Or (vAddressID.Equals(0)) Or (bDefaultAll) Then
            vAddressID = 0
        End If



        If (Information.IsNothing(vAddress1)) Or (String.IsNullOrEmpty(vAddress1)) Or (bDefaultAll) Then
            vAddress1 = ""
        End If



        If (Information.IsNothing(vAddress2)) Or (String.IsNullOrEmpty(vAddress2)) Or (bDefaultAll) Then
            vAddress2 = ""
        End If



        If (Information.IsNothing(vAddress3)) Or (String.IsNullOrEmpty(vAddress3)) Or (bDefaultAll) Then
            vAddress3 = ""
        End If



        If (Information.IsNothing(vAddress4)) Or (String.IsNullOrEmpty(vAddress4)) Or (bDefaultAll) Then
            vAddress4 = ""
        End If



        If (Information.IsNothing(vPostalCode)) Or (String.IsNullOrEmpty(vPostalCode)) Or (bDefaultAll) Then
            vPostalCode = ""
        End If



        If (Information.IsNothing(vCountryID)) Or (vCountryID.Equals(0)) Or (bDefaultAll) Then
            vCountryID = m_iLanguageID
        End If



        If (Information.IsNothing(vCreatedByID)) Or (vCreatedByID.Equals(0)) Or (bDefaultAll) Then
            vCreatedByID = m_iUserID
        End If



        If (Information.IsNothing(vDateCreated)) Or (vDateCreated.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If



        If (Information.IsNothing(vModifiedByID)) Or (vModifiedByID.Equals(0)) Or (bDefaultAll) Then
            vModifiedByID = m_iUserID
        End If



        If (Information.IsNothing(vLastModified)) Or (vLastModified.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vLastModified = DateTime.Now
        End If



        If (Information.IsNothing(vUpdateGlobalAddress)) Or (vUpdateGlobalAddress.Equals(False)) Or (bDefaultAll) Then
            vUpdateGlobalAddress = False
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRAddress for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing, Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing, Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing, Optional ByRef vAddressUsageTypeID As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue


        If Not Information.IsNothing(vAddressCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vAddressCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vSourceID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vAddressID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vAddressID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vCountryID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vCountryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vCreatedByID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vCreatedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vDateCreated) Then
            If Not Information.IsDate(vDateCreated) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vModifiedByID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vModifiedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vLastModified) Then
            If Not Information.IsDate(vLastModified) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

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