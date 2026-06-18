Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'Developer Guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Recoveries_NET.Recoveries")> _
Public NotInheritable Class Recoveries
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMCoinsuranceRecoveries
    '
    ' Date: {TodaysDate}
    '
    ' Description: Describes the CLMCoinsuranceRecoveries attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/12/2003
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_oDatabase As dPMDAO.Database

    Private m_sTransactionType As String = ""
    ' ************************************************


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMCoinsuranceRecoveries"

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMCoinsuranceRecoveries As dCLMCoinsuranceRecoveries.Data

    ' Error Code
    Private m_lReturn As Integer

    Private m_sPartyName As String = ""

    ' Primary Keys to work with
    Private m_lPartyID As Integer
    Private m_lClaimID As Integer
    Private m_lInsuranceFileCnt As Integer

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property DatabaseStatus() As Integer
        Get

            Return m_iDatabaseStatus

        End Get
        Set(ByVal Value As Integer)

            m_iDatabaseStatus = Value

        End Set
    End Property

    Public Property PartyID() As Integer
        Get

            Return m_lPartyID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyID = Value

        End Set
    End Property

    Public Property PartyName() As String
        Get

            Return m_sPartyName

        End Get
        Set(ByVal Value As String)

            m_sPartyName = Value

        End Set
    End Property

    Public Property ClaimID() As Integer
        Get

            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            m_lClaimID = Value

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
            m_dCLMCoinsuranceRecoveries = New dCLMCoinsuranceRecoveries.Data()

            m_lReturn = m_dCLMCoinsuranceRecoveries.Initialise(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, v_vDatabase)


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
        If Not Me.disposedValue Then
            Me.disposedValue = True
            If disposing Then
                If m_dCLMCoinsuranceRecoveries IsNot Nothing Then
                    m_dCLMCoinsuranceRecoveries.Dispose()
                    m_dCLMCoinsuranceRecoveries = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CLMCoinsuranceRecoveries.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults




            'developer guide no.98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyID:=vPartyID, vPartyName:=vPartyName, vShare:=vShare, vShareValue:=vShareValue)

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
    ' Description: Sets the supplied CLMCoinsuranceRecoveries property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'developer guide no.98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyID:=vPartyID, vPartyName:=vPartyName, vShare:=vShare, vShareValue:=vShareValue)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vPartyID:=vPartyID, vPartyName:=vPartyName, vShare:=vShare, vShareValue:=vShareValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dCLMCoinsuranceRecoveries


                If Not Information.IsNothing(vPartyID) Then
                    If .PartyID <> vPartyID Then
                        .PartyID = vPartyID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vPartyName) Then
                    If .PartyName <> vPartyName Then
                        .PartyName = vPartyName
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vShare) Then
                    If .Share <> vShare Then
                        .Share = vShare
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vShareValue) Then
                    If .ShareValue <> vShareValue Then
                        .ShareValue = vShareValue
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied CLMCoinsuranceRecoveries property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dCLMCoinsuranceRecoveries
                'developer guide  no.118
                'start change

                vPartyID = .PartyID

                vPartyName = .PartyName

                vShare = .Share

                vShareValue = .ShareValue

                'end change
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
    Public Function SelectItem(ByVal icount As Integer) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMCoinsuranceRecoveries

                ' Set Data object primary key
                .ClaimID = ClaimID
                .InsuranceFileCnt = InsuranceFileCnt

                ' Select a record from the database
                m_lReturn = .GetDetails(vArray)

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

            With m_dCLMCoinsuranceRecoveries

                ' Add a record to the database from the object
                .ClaimID = ClaimID
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMCoinsuranceRecoveries Added


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

            With m_dCLMCoinsuranceRecoveries

                ' Set Data object primary key
                .PartyID = PartyID

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

            With m_dCLMCoinsuranceRecoveries

                ' Set Data object primary key
                .PartyID = m_lPartyID

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

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a CLMCoinsuranceRecoveries.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no.44
        'start
        If (Information.IsNothing(vPartyID)) OrElse (vPartyID.Equals(0)) OrElse (bDefaultAll) Then
            vPartyID = 0
        End If



        If (Information.IsNothing(vPartyName)) OrElse (vPartyName.Equals(0)) OrElse (bDefaultAll) Then
            vPartyName = 0
        End If



        If (Information.IsNothing(vShare)) OrElse (vShare.Equals(0)) OrElse (bDefaultAll) Then
            vShare = 0
        End If



        If (Information.IsNothing(vShareValue)) OrElse (vShareValue.Equals(0)) OrElse (bDefaultAll) Then
            vShareValue = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the CLMCoinsuranceRecoveries for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyID As Object = Nothing, Optional ByRef vPartyName As Object = Nothing, Optional ByRef vShare As Object = Nothing, Optional ByRef vShareValue As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate


        If Not Information.IsNothing(vPartyID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vPartyName) Then

            If Object.Equals(vPartyName, Nothing) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Information.IsNothing(vShare) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If Information.IsNothing(vShareValue) Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result

    End Function


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



    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Reads the Base Details from the Database .
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMCoinsuranceRecoveries

                ' Set Data object primary key
                .ClaimID = ClaimID
                .InsuranceFileCnt = InsuranceFileCnt
                .TransactionType = m_sTransactionType

                ' Select a record from the database
                m_lReturn = .GetDetails(r_vResultArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class