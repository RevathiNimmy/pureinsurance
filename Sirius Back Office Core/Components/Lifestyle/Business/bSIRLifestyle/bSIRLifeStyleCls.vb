Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no 129. 
Imports SharedFiles
Friend NotInheritable Class PMBLifeStyle
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: PMBLifeStyle
    '
    ' Date: 05/05/1999
    '
    ' Description: Describes the PMBLifeStyle attributes.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "PMBLifeStyle"

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
    Private m_dPMBLifeStyle As dSIRLifeStyle.PMBLifeStyle
    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode
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
            m_dPMBLifeStyle = New dSIRLifeStyle.PMBLifeStyle()
            m_lReturn = m_dPMBLifeStyle.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dPMBLifeStyle IsNot Nothing Then
                    m_dPMBLifeStyle.Dispose()
                End If
                m_dPMBLifeStyle = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the PMBLifeStyle.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            'developer guide no. 98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPartyLifestyleID:=vPartyLifestyleID, vName:=vName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGenderCode:=vGenderCode, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker), gPMConstants.PMEReturnCode)

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
    ' Description: Sets the supplied PMBLifeStyle property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPartyLifestyleID As Integer = 0, Optional ByRef vName As String = "", Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As String = "", Optional ByRef vOccupationCode As String = "", Optional ByRef vSecondaryOccupationCode As String = "", Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters



                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPartyLifestyleID:=vPartyLifestyleID, vName:=vName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGenderCode:=vGenderCode, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vPartyLifestyleID:=vPartyLifestyleID, vName:=vName, vCategory:=vCategory, vDateOfBirth:=vDateOfBirth, vGenderCode:=vGenderCode, vOccupationCode:=vOccupationCode, vSecondaryOccupationCode:=vSecondaryOccupationCode, vIsSmoker:=vIsSmoker), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dPMBLifeStyle


                If Not Information.IsNothing(vPartyCnt) Then
                    If .PartyCnt <> vPartyCnt Then
                        .PartyCnt = vPartyCnt
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vPartyLifestyleID) Then
                    If .PartyLifestyleID <> vPartyLifestyleID Then
                        .PartyLifestyleID = vPartyLifestyleID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vName) Then

                    'developer guide no. 44
                    If (Object.Equals(.Name, Nothing)) OrElse (.Name.Trim() <> vName.Trim()) Then

                        'developer guide no. 24(Guide)
                        .Name = vName
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vCategory) Then



                    'developer guide no. 44
                    If (Object.Equals(.Category, Nothing)) OrElse (Not .Category.Equals(vCategory)) Then


                        'developer guide no. 24(Guide)
                        .Category = vCategory
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vDateOfBirth) Then



                    'developer guide no. 44
                    If (Object.Equals(.DateOfBirth, Nothing)) OrElse (Not .DateOfBirth.Equals(vDateOfBirth)) Then


                        'developer guide no. 24(Guide)
                        .DateOfBirth = vDateOfBirth
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vGenderCode) Then

                    'developer guide no. 44
                    If (Object.Equals(.GenderCode, Nothing)) OrElse (.GenderCode.Trim() <> vGenderCode.Trim()) Then

                        'developer guide no. 24(Guide)
                        .GenderCode = vGenderCode
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vOccupationCode) Then

                    If (Object.Equals(.OccupationCode, Nothing)) OrElse (.OccupationCode.Trim() <> vOccupationCode.Trim()) Then

                        'developer guide no. 24(Guide)
                        .OccupationCode = vOccupationCode
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vSecondaryOccupationCode) Then

                    'developer guide no. 44
                    If (Object.Equals(.SecondaryOccupationCode, Nothing)) OrElse (.SecondaryOccupationCode.Trim() <> vSecondaryOccupationCode.Trim()) Then

                        'developer guide no. 24(Guide)
                        .SecondaryOccupationCode = vSecondaryOccupationCode
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vIsSmoker) Then



                    'developer guide no. 44
                    If (Object.Equals(.IsSmoker, Nothing)) OrElse (Not .IsSmoker.Equals(vIsSmoker)) Then


                        'developer guide no. 24(Guide)
                        .IsSmoker = vIsSmoker
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
    ' Description: Returns the supplied PMBLifeStyle property values.
    '
    ' ***************************************************************** '
    'developer guide no. 101(Guide)
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dPMBLifeStyle

                'developer guide no. 143(Guide)
                vPartyCnt = .PartyCnt



                'developer guide no. 143(Guide)
                vPartyLifestyleID = .PartyLifestyleID




                'developer guide no. 143(Guide)
                vName = .Name

                'developer guide no. 143(Guide)
                vCategory = .Category

                'developer guide no. 143(Guide)
                vDateOfBirth = .DateOfBirth

                'developer guide no. 143(Guide)
                vGenderCode = .GenderCode

                'developer guide no. 143(Guide)
                vOccupationCode = .OccupationCode
                'developer guide no. 143(Guide)
                vSecondaryOccupationCode = .SecondaryOccupationCode

                vIsSmoker = .IsSmoker

                'developer guide no. 143(Guide)
                vSecondaryOccupationCode = .SecondaryOccupationCode

                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, gPMConstants.PMELogLevel.PMLogOnError, "GetProperties Failed", ACApp, ACClass, "GetProperties", Information.Err().Number, excep.Message, excep:=excep)

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

            With m_dPMBLifeStyle

                ' Set Data object primary key
                .PartyCnt = PartyCnt
                .PartyLifestyleID = PartyLifestyleID

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

            With m_dPMBLifeStyle

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the PMBLifeStyle Added
                PartyCnt = .PartyCnt
                PartyLifestyleID = .PartyLifestyleID

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

            With m_dPMBLifeStyle

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

            With m_dPMBLifeStyle

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: DefaultParameters (Private)
    '
    ' Description: Sets the Default Values for a PMBLifeStyle.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As String = "", Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Date = #12/30/1899#, Optional ByRef vGenderCode As String = "", Optional ByRef vOccupationCode As String = "", Optional ByRef vSecondaryOccupationCode As String = "", Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no. 44
        If (Information.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        'developer guide no. 44
        If (Information.IsNothing(vPartyLifestyleID)) OrElse (vPartyLifestyleID.Equals(0)) Or (bDefaultAll) Then
            vPartyLifestyleID = 0
        End If



        'developer guide no. 44
        If (Information.IsNothing(vName)) OrElse (String.IsNullOrEmpty(vName)) Or (bDefaultAll) Then
            vName = ""
        End If



        'developer guide no. 44
        If (Information.IsNothing(vCategory)) OrElse (vCategory.Equals(0)) Or (bDefaultAll) Then
            vCategory = 0
        End If



        'developer guide no. 44
        If (Information.IsNothing(vDateOfBirth)) OrElse (vDateOfBirth.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vDateOfBirth = DateTime.Now
        End If



        'developer guide no. 44
        If (Information.IsNothing(vGenderCode)) OrElse (String.IsNullOrEmpty(vGenderCode)) Or (bDefaultAll) Then
            vGenderCode = ""
        End If



        'developer guide no. 44
        If (Information.IsNothing(vOccupationCode)) OrElse (String.IsNullOrEmpty(vOccupationCode)) Or (bDefaultAll) Then
            vOccupationCode = ""
        End If



        'developer guide no. 44
        If (Information.IsNothing(vSecondaryOccupationCode)) OrElse (String.IsNullOrEmpty(vSecondaryOccupationCode)) Or (bDefaultAll) Then
            vSecondaryOccupationCode = ""
        End If



        'developer guide no. 44
        If (Information.IsNothing(vIsSmoker)) OrElse (vIsSmoker.Equals(0)) Or (bDefaultAll) Then
            vIsSmoker = 0
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the PMBLifeStyle for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyLifestyleID As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vCategory As Object = Nothing, Optional ByRef vDateOfBirth As Object = Nothing, Optional ByRef vGenderCode As Object = Nothing, Optional ByRef vOccupationCode As Object = Nothing, Optional ByRef vSecondaryOccupationCode As Object = Nothing, Optional ByRef vIsSmoker As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Information.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vPartyLifestyleID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vPartyLifestyleID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vCategory) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vCategory), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vDateOfBirth) Then
            If Not Information.IsDate(vDateOfBirth) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vIsSmoker) Then

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

