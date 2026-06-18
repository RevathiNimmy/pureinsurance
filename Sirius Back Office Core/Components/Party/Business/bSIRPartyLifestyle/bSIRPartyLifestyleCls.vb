Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyLifestyle
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyLifestyle
    '
    ' Date: 20/07/2000
    '
    ' Description: Describes the SIRPartyLifestyle attributes.
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
    Private Const ACClass As String = "SIRPartyLifestyle"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyLifestyle As dSIRPartyLifestyle.SIRPartyLifestyle ' was dSIRPartyLifestyle.SIRPartyLifestyle

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
    Private m_lPartyLifestyleID As Integer
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

    Public Property PartyLifestyleID() As Integer
        Get

            Return m_lPartyLifestyleID

        End Get
        Set(ByVal Value As Integer)

            m_lPartyLifestyleID = Value

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
            m_dSIRPartyLifestyle = New dSIRPartyLifestyle.SIRPartyLifestyle()
            '    Set m_dSIRPartyLifestyle = New dSIRPartyLifestyle.SIRPartyLifestyle

            m_lReturn = m_dSIRPartyLifestyle.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRPartyLifestyle IsNot Nothing Then
                    m_dSIRPartyLifestyle.Dispose()
                End If
                m_dSIRPartyLifestyle = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyLifestyle.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults









            'developer guide no. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPartyLifestyleID:=vPartyLifestyleID, vName:=vName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGenderCode:=vGenderCode, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker)

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
    ' Description: Sets the supplied SIRPartyLifestyle property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyLifestyleID As Integer = 0, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters

                'developer guide no. 98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyLifestyleID:=vPartyLifestyleID, vName:=vName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGenderCode:=vGenderCode, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vPartyCnt:=vPartyCnt, vPartyLifestyleID:=vPartyLifestyleID, vName:=vName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGenderCode:=vGenderCode, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyLifestyle



                If (Not Informations.IsNothing(vPartyCnt)) And (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Informations.IsNothing(vPartyLifestyleID)) And (Not vPartyLifestyleID.Equals(0)) Then
                    .PartyLifestyleID = vPartyLifestyleID
                End If




                If (Not Informations.IsNothing(vName)) And (Not Object.Equals(vName, Nothing)) Then


                    'developer guide no. 24
                    .Name = vName
                End If



                If (Not Informations.IsNothing(vCategory)) And (Not Object.Equals(vCategory, Nothing)) Then


                    'developer guide no. 24
                    .Category = vCategory
                End If



                If (Not Informations.IsNothing(vDateOfBirth)) And (Not Object.Equals(vDateOfBirth, Nothing)) Then


                    'developer guide no. 24
                    .DateOfBirth = vDateOfBirth
                End If



                If (Not Informations.IsNothing(vGenderCode)) And (Not Object.Equals(vGenderCode, Nothing)) Then


                    'developer guide no. 24
                    .GenderCode = vGenderCode
                End If



                If (Not Informations.IsNothing(vOccupationCode)) And (Not Object.Equals(vOccupationCode, Nothing)) Then


                    'developer guide no. 24
                    .OccupationCode = vOccupationCode
                End If



                If (Not Informations.IsNothing(vSecondaryOccupationCode)) And (Not Object.Equals(vSecondaryOccupationCode, Nothing)) Then


                    'developer guide no. 24
                    .SecondaryOccupationCode = vSecondaryOccupationCode
                End If



                If (Not Informations.IsNothing(vIsSmoker)) And (Not Object.Equals(vIsSmoker, Nothing)) Then


                    'developer guide no. 24
                    .IsSmoker = vIsSmoker
                End If


                ' If we have changed one of the properties, update the status
                'If (bDataChanged = True) Then
                m_iDatabaseStatus = iStatus
                'End If

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
    ' Description: Returns the supplied SIRPartyLifestyle property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyLifestyleID As Integer = 0, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyLifestyle


                vPartyCnt = .PartyCnt


                vPartyLifestyleID = .PartyLifestyleID


                vName = .Name


                vCategory = .Category


                vDateOfBirth = .DateOfBirth


                vGenderCode = .GenderCode


                vOccupationCode = .OccupationCode


                If Not (Convert.IsDBNull(.SecondaryOccupationCode) Or Informations.IsNothing(.SecondaryOccupationCode)) Then


                    vSecondaryOccupationCode = .SecondaryOccupationCode
                End If


                vIsSmoker = .IsSmoker


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

            With m_dSIRPartyLifestyle

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyLifestyleID = PartyLifestyleID

                ' Select a record from the database
                m_lReturn = .SelectSingle()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' TF200700 - OK to return Not Found
                    If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                        Return gPMConstants.PMEReturnCode.PMNotFound
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
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

            With m_dSIRPartyLifestyle

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyLifestyle Added
                PartyCnt = .PartyCnt
                PartyLifestyleID = .PartyLifestyleID

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

            With m_dSIRPartyLifestyle

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyLifestyleID = PartyLifestyleID

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

            With m_dSIRPartyLifestyle

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyLifestyleID = PartyLifestyleID

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
    ' Description: Sets the Default Values for a SIRPartyLifestyle.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As String = "", Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As String = "", Optional ByRef vGenderCode As String = "", Optional ByRef vOccupationCode As String = "", Optional ByRef vSecondaryOccupationCode As String = "", Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vPartyLifestyleID)) OrElse (vPartyLifestyleID.Equals(0)) OrElse (bDefaultAll) Then
            vPartyLifestyleID = 0
        End If



        If (Informations.IsNothing(vName)) OrElse (String.IsNullOrEmpty(vName)) OrElse (bDefaultAll) Then
            vName = ""
        End If



        If (Informations.IsNothing(vCategory)) OrElse (vCategory.Equals(0)) OrElse (bDefaultAll) Then
            vCategory = 0
        End If



        If (Informations.IsNothing(vDateOfBirth)) OrElse (String.IsNullOrEmpty(vDateOfBirth)) OrElse (bDefaultAll) Then
            'DC 20/10/00 set to Null (which is 29/12/1899)
            'vDateOfBirth = Now
            vDateOfBirth = "29/12/1899"
        End If



        If (Informations.IsNothing(vGenderCode)) OrElse (String.IsNullOrEmpty(vGenderCode)) OrElse (bDefaultAll) Then
            vGenderCode = ""
        End If



        If (Informations.IsNothing(vOccupationCode)) OrElse (String.IsNullOrEmpty(vOccupationCode)) OrElse (bDefaultAll) Then
            vOccupationCode = ""
        End If



        If (Informations.IsNothing(vSecondaryOccupationCode)) OrElse (String.IsNullOrEmpty(vSecondaryOccupationCode)) OrElse (bDefaultAll) Then
            vSecondaryOccupationCode = ""
        End If



        If (Informations.IsNothing(vIsSmoker)) OrElse (vIsSmoker.Equals(0)) OrElse (bDefaultAll) Then
            vIsSmoker = 0
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyLifestyle for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

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


        If Not Informations.IsNothing(vPartyLifestyleID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPartyLifestyleID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCategory) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vCategory), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDateOfBirth) Then
            If Not Informations.IsDate(vDateOfBirth) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsSmoker) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vIsSmoker), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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

