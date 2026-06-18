Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
'developer guide no. 129
Imports SharedFiles
Friend NotInheritable Class SIRTextFile
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRTextFile
    '
    ' Date: 07/05/1999
    '
    ' Description: Describes the SIRTextFile attributes.
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
    Private Const ACClass As String = "SIRTextFile"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRTextFile As dSIRTextFile.SIRTextFile

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lEntityTypeId As Integer
    Private m_lEntityCnt As Integer
    Private m_lSlotNumber As Integer
    Private m_lFileNumber As Integer
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

    Public Property EntityTypeId() As Integer
        Get

            Return m_lEntityTypeId

        End Get
        Set(ByVal Value As Integer)

            m_lEntityTypeId = Value

        End Set
    End Property

    Public Property EntityCnt() As Integer
        Get

            Return m_lEntityCnt

        End Get
        Set(ByVal Value As Integer)

            m_lEntityCnt = Value

        End Set
    End Property

    Public Property SlotNumber() As Integer
        Get

            Return m_lSlotNumber

        End Get
        Set(ByVal Value As Integer)

            m_lSlotNumber = Value

        End Set
    End Property

    Public Property FileNumber() As Integer
        Get

            Return m_lFileNumber

        End Get
        Set(ByVal Value As Integer)

            m_lFileNumber = Value

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
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Create instance of data class
            m_dSIRTextFile = New dSIRTextFile.SIRTextFile()
            'Set m_dSIRTextFile = New dSIRTextFile.SIRTextFile

            m_lReturn = m_dSIRTextFile.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRTextFile IsNot Nothing Then
                    m_dSIRTextFile.Dispose()
                    m_dSIRTextFile = Nothing
                End If
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRTextFile.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults




            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vEntityTypeId:=(vEntityTypeId), vEntityCnt:=(vEntityCnt), vSlotNumber:=(vSlotNumber), vFileNumber:=(vFileNumber))

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
    ' Description: Sets the supplied SIRTextFile property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vEntityTypeId As Integer = 0, Optional ByRef vEntityCnt As Integer = 0, Optional ByRef vSlotNumber As Integer = 0, Optional ByRef vFileNumber As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = DefaultParameters(bDefaultAll:=False, vEntityTypeId:=vEntityTypeId, vEntityCnt:=vEntityCnt, vSlotNumber:=vSlotNumber, vFileNumber:=vFileNumber)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vEntityTypeId:=vEntityTypeId, vEntityCnt:=vEntityCnt, vSlotNumber:=vSlotNumber, vFileNumber:=vFileNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRTextFile


                If Not Information.IsNothing(vEntityTypeId) Then

                    If .EntityTypeId.Equals(0) Or (.EntityTypeId <> vEntityTypeId) Then
                        .EntityTypeId = vEntityTypeId
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vEntityCnt) Then

                    If .EntityCnt.Equals(0) Or (.EntityCnt <> vEntityCnt) Then
                        .EntityCnt = vEntityCnt
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vSlotNumber) Then

                    If .SlotNumber.Equals(0) Or (.SlotNumber <> vSlotNumber) Then
                        .SlotNumber = vSlotNumber
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vFileNumber) Then

                    If .FileNumber.Equals(0) Or (.FileNumber <> vFileNumber) Then
                        .FileNumber = vFileNumber
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
    ' Description: Returns the supplied SIRTextFile property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vEntityTypeId As Integer = 0, Optional ByRef vEntityCnt As Integer = 0, Optional ByRef vSlotNumber As Integer = 0, Optional ByRef vFileNumber As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRTextFile

                'Developer Guide No. 118
                'If Not Information.IsNothing(vEntityTypeId) Then
                vEntityTypeId = .EntityTypeId
                'End If

                'Developer Guide No. 118
                'If Not Information.IsNothing(vEntityCnt) Then
                vEntityCnt = .EntityCnt
                ' End If

                'Developer Guide No. 118
                'If Not Information.IsNothing(vSlotNumber) Then
                vSlotNumber = .SlotNumber
                'End If

                'Developer Guide No. 118
                'If Not Information.IsNothing(vFileNumber) Then
                vFileNumber = .FileNumber
                'End If

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

            With m_dSIRTextFile

                ' Set Data object primary key
                .EntityTypeId = EntityTypeId
                .EntityCnt = EntityCnt
                .SlotNumber = SlotNumber

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

            With m_dSIRTextFile

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRTextFile Added
                FileNumber = .FileNumber

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

            With m_dSIRTextFile

                ' Set Data object primary key
                .EntityTypeId = EntityTypeId
                .EntityCnt = EntityCnt
                .SlotNumber = SlotNumber

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

            With m_dSIRTextFile

                ' Set Data object primary key
                .FileNumber = FileNumber

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
    ' Description: Sets the Default Values for a SIRTextFile.
    '
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        If (Information.IsNothing(vEntityTypeId)) Or (vEntityTypeId.Equals(0)) Or (bDefaultAll) Then
            vEntityTypeId = 0
        End If



        If (Information.IsNothing(vEntityCnt)) Or (vEntityCnt.Equals(0)) Or (bDefaultAll) Then
            vEntityCnt = 0
        End If



        If (Information.IsNothing(vSlotNumber)) Or (vSlotNumber.Equals(0)) Or (bDefaultAll) Then
            vSlotNumber = 0
        End If



        If (Information.IsNothing(vFileNumber)) Or (vFileNumber.Equals(0)) Or (bDefaultAll) Then
            vFileNumber = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRTextFile for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vEntityTypeId As Object = Nothing, Optional ByRef vEntityCnt As Object = Nothing, Optional ByRef vSlotNumber As Object = Nothing, Optional ByRef vFileNumber As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}


        If Not Information.IsNothing(vEntityTypeId) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vEntityTypeId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vEntityCnt) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vEntityCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vSlotNumber) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vSlotNumber), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Information.IsNothing(vFileNumber) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vFileNumber), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
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