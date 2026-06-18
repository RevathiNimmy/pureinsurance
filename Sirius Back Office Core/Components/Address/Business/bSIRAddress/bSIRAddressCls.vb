Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
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
    Private m_dSIRAddress As dSIRAddress.SIRAddress

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lAddressCnt As Integer
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create instance of data class
            m_dSIRAddress = New dSIRAddress.SIRAddress()

            m_lReturn = m_dSIRAddress.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRAddress IsNot Nothing Then
                    m_dSIRAddress.Dispose()
                End If
                m_dSIRAddress = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ''' <summary>
    ''' GetDefaults (Public)
    ''' Description: Returns the Default Values for the SIRAddress.
    ''' </summary>
    ''' <param name="vSubType"></param>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vSourceID"></param>
    ''' <param name="vAddressID"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vCreatedByID"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vModifiedByID"></param>
    ''' <param name="vLastModified"></param>
    ''' <param name="sAddress5"></param>
    ''' <param name="sAddress6"></param>
    ''' <param name="sAddress7"></param>
    ''' <param name="sAddress8"></param>
    ''' <param name="sAddress9"></param>
    ''' <param name="sAddress10"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing,
                                Optional ByRef vAddress1 As Object = Nothing,
                                Optional ByRef vAddress2 As Object = Nothing,
                                Optional ByRef vAddress3 As Object = Nothing,
                                Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing,
                                Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing,
                                Optional ByRef vDateCreated As Object = Nothing,
                                Optional ByRef vModifiedByID As Object = Nothing,
                                Optional ByRef vLastModified As Object = Nothing,
                                Optional ByRef sAddress5 As String = "",
                                Optional ByRef sAddress6 As String = "",
                                Optional ByRef sAddress7 As String = "",
                                Optional ByRef sAddress8 As String = "",
                                Optional ByRef sAddress9 As String = "",
                                Optional ByRef sAddress10 As String = "") As Integer


        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults

            'developer guide no. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAddressCnt:=vAddressCnt, vSourceID:=vSourceID, vAddressID:=vAddressID, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified,
                                          sAddress5:=sAddress5, sAddress6:=sAddress6, sAddress7:=sAddress7,
                                          sAddress8:=sAddress8, sAddress9:=sAddress9, sAddress10:=sAddress10)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    '''  Name: SetProperties (Public)
    ''' Description: Sets the supplied SIRAddress property values.
    ''' </summary>
    ''' <param name="iStatus"></param>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vSourceID"></param>
    ''' <param name="vAddressID"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vCreatedByID"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vModifiedByID"></param>
    ''' <param name="vLastModified"></param>
    ''' <param name="vExternalId"></param>
    ''' <param name="sAddress5"></param>
    ''' <param name="sAddress6"></param>
    ''' <param name="sAddress7"></param>
    ''' <param name="sAddress8"></param>
    ''' <param name="sAddress9"></param>
    ''' <param name="sAddress10"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetProperties(ByRef iStatus As Integer,
                                  Optional ByRef vAddressCnt As Object = Nothing,
                                  Optional ByRef vSourceID As Object = Nothing,
                                  Optional ByRef vAddressID As Object = Nothing,
                                  Optional ByRef vAddress1 As Object = Nothing,
                                  Optional ByRef vAddress2 As Object = Nothing,
                                  Optional ByRef vAddress3 As Object = Nothing,
                                  Optional ByRef vAddress4 As Object = Nothing,
                                  Optional ByRef vPostalCode As Object = Nothing,
                                  Optional ByRef vCountryID As Object = Nothing,
                                  Optional ByRef vCreatedByID As Object = Nothing,
                                  Optional ByRef vDateCreated As Object = Nothing,
                                  Optional ByRef vModifiedByID As Object = Nothing,
                                  Optional ByRef vLastModified As Object = Nothing,
                                  Optional ByRef vExternalId As Object = Nothing,
                                  Optional ByRef sAddress5 As String = "",
                                  Optional ByRef sAddress6 As String = "",
                                  Optional ByRef sAddress7 As String = "",
                                  Optional ByRef sAddress8 As String = "",
                                  Optional ByRef sAddress9 As String = "",
                                  Optional ByRef sAddress10 As String = "",
                                  Optional ByVal sUniqueId As String = "",
                                  Optional ByVal sScreenHeirarchy As String = "") As Integer


        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim bDataChanged As Boolean

        Try

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters



                'developer guide no. 98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vAddressCnt:=vAddressCnt, vSourceID:=vSourceID, vAddressID:=vAddressID,
                                              vAddress1:=vAddress1, vAddress2:=vAddress2,
                                              vAddress3:=vAddress3, vAddress4:=vAddress4,
                                              vPostalCode:=vPostalCode, vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vExternalId:=vExternalId,
                                              sAddress5:=sAddress5, sAddress6:=sAddress6, sAddress7:=sAddress7,
                                              sAddress8:=sAddress8, sAddress9:=sAddress9, sAddress10:=sAddress10)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vAddressCnt:=vAddressCnt, vSourceID:=vSourceID,
                                 vAddressID:=vAddressID, vAddress1:=vAddress1, vAddress2:=vAddress2, vAddress3:=vAddress3, vAddress4:=vAddress4, vPostalCode:=vPostalCode, vCountryID:=vCountryID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vModifiedByID:=vModifiedByID, vLastModified:=vLastModified, vExternalId:=vExternalId,
                                 vAddress5:=sAddress5, vAddress6:=sAddress6, vAddress7:=sAddress7,
                                 vAddress8:=sAddress8, vAddress9:=sAddress9, vAddress10:=sAddress10)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRAddress
                If Not Informations.IsNothing(vAddressCnt) Then
                    If .AddressCnt <> vAddressCnt Then
                        .AddressCnt = vAddressCnt
                        bDataChanged = True
                    End If
                End If
                If Not Informations.IsNothing(vSourceID) Then
                    If .SourceID <> vSourceID Then
                        .SourceID = vSourceID
                        bDataChanged = True
                    End If
                End If
                If Not Informations.IsNothing(vAddressID) Then
                    If .AddressID <> vAddressID Then
                        .AddressID = vAddressID
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vAddress1) Then
                    If .Address1.Trim() <> vAddress1.Trim() Then
                        .Address1 = vAddress1
                        bDataChanged = True
                    End If
                End If

                If (Not Informations.IsNothing(vAddress2)) And (Not Object.Equals(vAddress2, Nothing)) Then
                    'developer guide no. 24
                    .Address2 = vAddress2
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(vAddress3)) And (Not Object.Equals(vAddress3, Nothing)) Then
                    'developer guide no. 24
                    .Address3 = vAddress3
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(vAddress4)) And (Not Object.Equals(vAddress4, Nothing)) Then
                    'developer guide no. 24
                    .Address4 = vAddress4
                    bDataChanged = True
                End If
                If Not Informations.IsNothing(vPostalCode) Then
                    If .PostalCode.Trim() <> vPostalCode.Trim() Then
                        .PostalCode = vPostalCode
                        bDataChanged = True
                    End If
                End If
                If Not Informations.IsNothing(vCountryID) Then
                    If .CountryID <> vCountryID Then
                        .CountryID = vCountryID
                        bDataChanged = True
                    End If
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
                    'developer guide no. 44
                    If .LastModified Is Nothing OrElse Not .LastModified.Equals(vLastModified) Then

                        'developer guide no. 24
                        .LastModified = vLastModified
                        bDataChanged = True
                    End If
                End If

                If Not Informations.IsNothing(vExternalId) Then
                    If gPMFunctions.ToSafeString(.ExternalId) <> vExternalId Then

                        'developer guide no. 24
                        .ExternalId = vExternalId
                        bDataChanged = True
                    End If
                End If

                If (Not Informations.IsNothing(sAddress5)) And (Not Object.Equals(sAddress5, Nothing)) Then
                    .Address5 = sAddress5
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(sAddress6)) And (Not Object.Equals(sAddress6, Nothing)) Then
                    .Address6 = sAddress6
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(sAddress7)) And (Not Object.Equals(sAddress7, Nothing)) Then
                    .Address7 = sAddress7
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(sAddress8)) And (Not Object.Equals(sAddress8, Nothing)) Then
                    .Address8 = sAddress8
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(sAddress9)) And (Not Object.Equals(sAddress9, Nothing)) Then
                    .Address9 = sAddress9
                    bDataChanged = True
                End If

                If (Not Informations.IsNothing(sAddress10)) And (Not Object.Equals(sAddress10, Nothing)) Then
                    .Address10 = sAddress10
                    bDataChanged = True
                End If

                If Not String.IsNullOrEmpty(sUniqueId) Then
                    .UniqueId = sUniqueId
                    bDataChanged = True
                End If

                If Not String.IsNullOrEmpty(sScreenHeirarchy) Then
                    .ScreenHeirarchy = sScreenHeirarchy
                    bDataChanged = True
                End If

                ' If we have changed one of the properties, update the status
                If bDataChanged Then
                    m_iDatabaseStatus = iStatus
                End If

            End With

            Return nResult

        Catch excep As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    '''  Name: GetProperties (Public)
    ''' Description: Returns the supplied SIRAddress property values.
    ''' </summary>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vSourceID"></param>
    ''' <param name="vAddressID"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vCreatedByID"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vModifiedByID"></param>
    ''' <param name="vLastModified"></param>
    ''' <param name="vExternalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetProperties(ByRef nStatus As Object, Optional ByRef vAddressCnt As Object = Nothing,
                                  Optional ByRef vSourceID As Object = Nothing,
                                  Optional ByRef vAddressID As Object = Nothing,
                                  Optional ByRef vAddress1 As Object = Nothing,
                                  Optional ByRef vAddress2 As Object = Nothing,
                                  Optional ByRef vAddress3 As Object = Nothing,
                                  Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Date = #12/30/1899#, Optional ByRef vModifiedByID As Byte = 0, Optional ByRef vLastModified As Object = Nothing,
                                  Optional ByRef vExternalId As Object = Nothing,
                                  Optional ByRef sAddress5 As String = "",
                                  Optional ByRef sAddress6 As String = "",
                                  Optional ByRef sAddress7 As String = "",
                                  Optional ByRef sAddress8 As String = "",
                                  Optional ByRef sAddress9 As String = "",
                                  Optional ByRef sAddress10 As String = "") As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        Try


            ' Set Property values.
            With m_dSIRAddress

                'developer guide no.118
                vAddressCnt = .AddressCnt
                'End If

                'developer guide no.118
                vSourceID = .SourceID
                'End If

                'developer guide no.118
                vAddressID = .AddressID
                'End If
                'developer guide no.118
                vAddress1 = .Address1
                'End If

                'developer guide no.118

                If Convert.IsDBNull(.Address2) Or Informations.IsNothing(.Address2) Then
                    vAddress2 = ""
                Else
                    vAddress2 = .Address2
                End If
                'End If
                'developer guide no.118
                If Convert.IsDBNull(.Address3) Or Informations.IsNothing(.Address3) Then
                    vAddress3 = ""
                Else
                    vAddress3 = .Address3
                End If
                'End If

                'developer guide no.118

                If Convert.IsDBNull(.Address4) Or Informations.IsNothing(.Address4) Then
                    vAddress4 = ""
                Else

                    vAddress4 = .Address4
                End If
                'End If

                'developer guide no.118
                vPostalCode = .PostalCode
                'End If

                'developer guide no.118
                vCountryID = .CountryID
                'End If
                'developer guide no.118
                vCreatedByID = .CreatedByID
                'End If

                'developer guide no.118
                vDateCreated = .DateCreated
                'End If

                'developer guide no.118
                vModifiedByID = .ModifiedByID
                'End If

                'developer guide no.118
                vLastModified = .LastModified
                'End If

                'developer guide no.118
                vExternalId = .ExternalId
                'End If
                If Convert.IsDBNull(.Address5) Or Informations.IsNothing(.Address5) Then
                    sAddress5 = ""
                Else
                    sAddress5 = .Address5
                End If
                If Convert.IsDBNull(.Address6) Or Informations.IsNothing(.Address6) Then
                    sAddress6 = ""
                Else
                    sAddress6 = .Address6
                End If

                If Convert.IsDBNull(.Address7) Or Informations.IsNothing(.Address7) Then
                    sAddress7 = ""
                Else
                    sAddress7 = .Address7
                End If

                If Convert.IsDBNull(.Address8) Or Informations.IsNothing(.Address8) Then
                    sAddress8 = ""
                Else

                    sAddress8 = .Address8
                End If

                If Convert.IsDBNull(.Address9) Or Informations.IsNothing(.Address9) Then
                    sAddress9 = ""
                Else

                    sAddress9 = .Address9
                End If

                If Convert.IsDBNull(.Address10) Or Informations.IsNothing(.Address10) Then
                    sAddress10 = ""
                Else

                    sAddress10 = .Address10
                End If

                nStatus = m_iDatabaseStatus

            End With

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
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

            With m_dSIRAddress

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRAddress Added
                AddressCnt = .AddressCnt

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

            With m_dSIRAddress

                ' Set Data object primary key
                '  .AddressCnt = AddressCnt
                AddressCnt = .AddressCnt

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    ''' <summary>
    '''  Name: DefaultParameters (Private)
    ''' Description: Sets the Default Values for a SIRAddress.
    ''' </summary>
    ''' <param name="bDefaultAll"></param>
    ''' <param name="vSubType"></param>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vSourceID"></param>
    ''' <param name="vAddressID"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vCreatedByID"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vModifiedByID"></param>
    ''' <param name="vLastModified"></param>
    ''' <param name="vExternalId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vAddressID As Object = Nothing,
                                       Optional ByRef vAddress1 As Object = Nothing,
                                       Optional ByRef vAddress2 As Object = Nothing,
                                       Optional ByRef vAddress3 As Object = Nothing,
                                       Optional ByRef vAddress4 As Object = Nothing,
                                       Optional ByRef vPostalCode As Object = Nothing, Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing, Optional ByRef vLastModified As Object = Nothing,
                                       Optional ByRef vExternalId As Object = Nothing,
                                       Optional ByRef sAddress5 As String = "",
                                       Optional ByRef sAddress6 As String = "",
                                       Optional ByRef sAddress7 As String = "",
                                       Optional ByRef sAddress8 As String = "",
                                       Optional ByRef sAddress9 As String = "",
                                       Optional ByRef sAddress10 As String = "") As Integer


        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no. 44(Guide)
        If (Informations.IsNothing(vAddressCnt)) OrElse (vAddressCnt.Equals(0)) Or (bDefaultAll) Then
            vAddressCnt = 0
        End If

        'developer guide no. 44(Guide)
        If (Informations.IsNothing(vSourceID)) OrElse (vSourceID.Equals(0)) Or (bDefaultAll) Then
            vSourceID = m_iSourceID
        End If

        'developer guide no. 44(Guide)
        If (Informations.IsNothing(vAddressID)) OrElse (vAddressID.Equals(0)) Or (bDefaultAll) Then
            vAddressID = 0
        End If

        If (Informations.IsNothing(vAddress1)) Or (String.IsNullOrEmpty(vAddress1)) Or (bDefaultAll) Then
            vAddress1 = ""
        End If

        If (Informations.IsNothing(vAddress2)) Or (String.IsNullOrEmpty(vAddress2)) Or (bDefaultAll) Then
            vAddress2 = ""
        End If

        If (Informations.IsNothing(vAddress3)) Or (String.IsNullOrEmpty(vAddress3)) Or (bDefaultAll) Then
            vAddress3 = ""
        End If

        If (Informations.IsNothing(vAddress4)) Or (String.IsNullOrEmpty(vAddress4)) Or (bDefaultAll) Then
            vAddress4 = ""
        End If

        If (Informations.IsNothing(vPostalCode)) Or (String.IsNullOrEmpty(vPostalCode)) Or (bDefaultAll) Then
            vPostalCode = ""
        End If

        'developer guide no. 44(Guide)
        If (Informations.IsNothing(vCountryID)) OrElse (vCountryID.Equals(0)) Or (bDefaultAll) Then
            vCountryID = m_iLanguageID
        End If
        'developer guide no. 44(Guide)
        If (Informations.IsNothing(vCreatedByID)) OrElse (vCreatedByID.Equals(0)) Or (bDefaultAll) Then
            vCreatedByID = m_iUserID
        End If
        'developer guide no. 44(Guide)
        If (Informations.IsNothing(vDateCreated)) OrElse (vDateCreated.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateCreated = DateTime.Now
        End If

        'developer guide no. 44(guide)
        If (Informations.IsNothing(vModifiedByID)) OrElse (vModifiedByID.Equals(0)) Or (bDefaultAll) Then
            vModifiedByID = m_iUserID
        End If

        ' developer guide no. 44(Guide)
        If (Informations.IsNothing(vLastModified)) OrElse (vLastModified.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vLastModified = DateTime.Now
        End If
        If (Informations.IsNothing(sAddress5)) Or (String.IsNullOrEmpty(sAddress5)) Or (bDefaultAll) Then
            sAddress5 = ""
        End If

        If (Informations.IsNothing(sAddress6)) Or (String.IsNullOrEmpty(sAddress6)) Or (bDefaultAll) Then
            sAddress6 = ""
        End If

        If (Informations.IsNothing(sAddress7)) Or (String.IsNullOrEmpty(sAddress7)) Or (bDefaultAll) Then
            sAddress7 = ""
        End If

        If (Informations.IsNothing(sAddress8)) Or (String.IsNullOrEmpty(sAddress8)) Or (bDefaultAll) Then
            sAddress8 = ""
        End If

        If (Informations.IsNothing(sAddress9)) Or (String.IsNullOrEmpty(sAddress9)) Or (bDefaultAll) Then
            sAddress9 = ""
        End If

        If (Informations.IsNothing(sAddress10)) Or (String.IsNullOrEmpty(sAddress10)) Or (bDefaultAll) Then
            sAddress10 = ""
        End If

        Return nResult

    End Function

    ''' <summary>
    ''' Name: Validate (Private)
    ''' Description: Checks the SIRAddress for Consistency.
    ''' </summary>
    ''' <param name="vAddressCnt"></param>
    ''' <param name="vSourceID"></param>
    ''' <param name="vAddressID"></param>
    ''' <param name="vAddress1"></param>
    ''' <param name="vAddress2"></param>
    ''' <param name="vAddress3"></param>
    ''' <param name="vAddress4"></param>
    ''' <param name="vPostalCode"></param>
    ''' <param name="vCountryID"></param>
    ''' <param name="vCreatedByID"></param>
    ''' <param name="vDateCreated"></param>
    ''' <param name="vModifiedByID"></param>
    ''' <param name="vLastModified"></param>
    ''' <param name="vExternalId"></param>
    ''' <param name="vAddress5"></param>
    ''' <param name="vAddress6"></param>
    ''' <param name="vAddress7"></param>
    ''' <param name="vAddress8"></param>
    ''' <param name="vAddress9"></param>
    ''' <param name="vAddress10"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function Validate(Optional ByRef vAddressCnt As Object = Nothing, Optional ByRef vSourceID As Object = Nothing,
                              Optional ByRef vAddressID As Object = Nothing, Optional ByRef vAddress1 As Object = Nothing,
                              Optional ByRef vAddress2 As Object = Nothing, Optional ByRef vAddress3 As Object = Nothing,
                              Optional ByRef vAddress4 As Object = Nothing, Optional ByRef vPostalCode As Object = Nothing,
                              Optional ByRef vCountryID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing,
                              Optional ByRef vDateCreated As Object = Nothing, Optional ByRef vModifiedByID As Object = Nothing,
                              Optional ByRef vLastModified As Object = Nothing, Optional ByRef vExternalId As Object = Nothing,
                              Optional ByRef vAddress5 As Object = Nothing, Optional ByRef vAddress6 As Object = Nothing,
                              Optional ByRef vAddress7 As Object = Nothing, Optional ByRef vAddress8 As Object = Nothing,
                              Optional ByRef vAddress9 As Object = Nothing, Optional ByRef vAddress10 As Object = Nothing) As Integer

        Dim nResult As Integer = 0

        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}

        If Not Informations.IsNothing(vAddressCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vAddressCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vSourceID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vSourceID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAddressID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vAddressID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCountryID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vCountryID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCreatedByID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vCreatedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDateCreated) Then
            If Not Informations.IsDate(vDateCreated) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vModifiedByID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vModifiedByID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLastModified) Then
            If Not Informations.IsDate(vLastModified) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        ' {* USER DEFINED CODE (End) *}
        Return nResult


    End Function

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