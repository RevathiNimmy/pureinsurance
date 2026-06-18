Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
Friend NotInheritable Class SIRPartyNetData
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRPartyNetData
    '
    ' Date: 20/07/2000
    '
    ' Description: Describes the SIRPartyNetData attributes.
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
    Private Const ACClass As String = "SIRPartyNetData"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dSIRPartyNetData As dSIRPartyNetData.SIRPartyNetData ' was dSIRPartyNetData.SIRPartyNetData

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lPartyCnt As Integer
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
            m_dSIRPartyNetData = New dSIRPartyNetData.SIRPartyNetData()
            '    Set m_dSIRPartyNetData = New dSIRPartyNetData.SIRPartyNetData

            m_lReturn = m_dSIRPartyNetData.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=vDatabase)

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
                If m_dSIRPartyNetData IsNot Nothing Then
                    m_dSIRPartyNetData.Dispose()
                End If
                m_dSIRPartyNetData = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SIRPartyNetData.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTpIntroducerCode As Object = Nothing, Optional ByRef vTpUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserid As Object = Nothing, Optional ByRef vCurrentInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults
            'developer guide no. 98
            m_lReturn = DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vPartyCnt:=vPartyCnt, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTpIntroducerCode:=vTpIntroducerCode, vTpUserCode:=vTpUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserid:=vUserid, vCurrentInsRenewalDate:=vCurrentInsRenewalDate)

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
    ' Description: Sets the supplied SIRPartyNetData property values.
    '
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPassword As String = "", Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTpIntroducerCode As Object = Nothing, Optional ByRef vTpUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserid As Object = Nothing, Optional ByRef vCurrentInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                'developer guide no. 98
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTpIntroducerCode:=vTpIntroducerCode, vTpUserCode:=vTpUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserid:=vUserid, vCurrentInsRenewalDate:=vCurrentInsRenewalDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = Validate(vPartyCnt:=vPartyCnt, vPassword:=vPassword, vMothersMaidenName:=vMothersMaidenName, vTpIntroducerCode:=vTpIntroducerCode, vTpUserCode:=vTpUserCode, vMemorableDate:=vMemorableDate, vAQuestion:=vAQuestion, vTheAnswer:=vTheAnswer, vUserid:=vUserid, vCurrentInsRenewalDate:=vCurrentInsRenewalDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSIRPartyNetData



                If (Not Informations.IsNothing(vPartyCnt)) And (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Informations.IsNothing(vPassword)) And (Not String.IsNullOrEmpty(vPassword)) Then
                    .Password = vPassword
                End If



                If (Not Informations.IsNothing(vMothersMaidenName)) And (Not Object.Equals(vMothersMaidenName, Nothing)) Then


                    'developer guide no. 24
                    .MothersMaidenName = vMothersMaidenName
                End If



                If (Not Informations.IsNothing(vTpIntroducerCode)) And (Not Object.Equals(vTpIntroducerCode, Nothing)) Then


                    'developer guide no. 24
                    .TpIntroducerCode = vTpIntroducerCode
                End If



                If (Not Informations.IsNothing(vTpUserCode)) And (Not Object.Equals(vTpUserCode, Nothing)) Then


                    'developer guide no. 24
                    .TpUserCode = vTpUserCode
                End If



                If (Not Informations.IsNothing(vMemorableDate)) And (Not Object.Equals(vMemorableDate, Nothing)) Then


                    'developer guide no. 24
                    .MemorableDate = vMemorableDate
                End If



                If (Not Informations.IsNothing(vAQuestion)) And (Not Object.Equals(vAQuestion, Nothing)) Then


                    'developer guide no. 24
                    .AQuestion = vAQuestion
                End If



                If (Not Informations.IsNothing(vTheAnswer)) And (Not Object.Equals(vTheAnswer, Nothing)) Then


                    'developer guide no. 24
                    .TheAnswer = vTheAnswer
                End If



                If (Not Informations.IsNothing(vUserid)) And (Not Object.Equals(vUserid, Nothing)) Then


                    'developer guide no. 24
                    .Userid = vUserid
                End If



                If (Not Informations.IsNothing(vCurrentInsRenewalDate)) And (Not Object.Equals(vCurrentInsRenewalDate, Nothing)) Then


                    'developer guide no. 24
                    .CurrentInsRenewalDate = vCurrentInsRenewalDate
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
    ' Description: Returns the supplied SIRPartyNetData property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Integer = 0, Optional ByRef vPassword As String = "", Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTpIntroducerCode As Object = Nothing, Optional ByRef vTpUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserid As Object = Nothing, Optional ByRef vCurrentInsRenewalDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dSIRPartyNetData


                'developer guide no.118
                vPartyCnt = .PartyCnt


                'developer guide no.118
                vPassword = .Password


                'developer guide no.118
                vMothersMaidenName = .MothersMaidenName


                'developer guide no.118
                vTpIntroducerCode = .TpIntroducerCode

                'developer guide no.118
                vTpUserCode = .TpUserCode

                'developer guide no.118
                vMemorableDate = .MemorableDate


                'developer guide no.118
                vAQuestion = .AQuestion


                'developer guide no.118
                vTheAnswer = .TheAnswer

                'developer guide no.118
                vUserid = .Userid


                'developer guide no.118
                vCurrentInsRenewalDate = .CurrentInsRenewalDate

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

            With m_dSIRPartyNetData

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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

            With m_dSIRPartyNetData

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the SIRPartyNetData Added
                PartyCnt = .PartyCnt

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

            With m_dSIRPartyNetData

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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

            With m_dSIRPartyNetData

                ' Set Data object primary key
                .PartyCnt = PartyCnt

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
    ' Description: Sets the Default Values for a SIRPartyNetData.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As String = "", Optional ByRef vMothersMaidenName As String = "", Optional ByRef vTpIntroducerCode As String = "", Optional ByRef vTpUserCode As String = "", Optional ByRef vMemorableDate As Date = #12/30/1899#, Optional ByRef vAQuestion As String = "", Optional ByRef vTheAnswer As String = "", Optional ByRef vUserid As String = "", Optional ByRef vCurrentInsRenewalDate As Date = #12/30/1899#) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vPartyCnt)) Or (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Informations.IsNothing(vPassword)) Or (String.IsNullOrEmpty(vPassword)) Or (bDefaultAll) Then
            vPassword = ""
        End If



        If (Informations.IsNothing(vMothersMaidenName)) Or (String.IsNullOrEmpty(vMothersMaidenName)) Or (bDefaultAll) Then
            vMothersMaidenName = ""
        End If



        If (Informations.IsNothing(vTpIntroducerCode)) Or (String.IsNullOrEmpty(vTpIntroducerCode)) Or (bDefaultAll) Then
            vTpIntroducerCode = ""
        End If



        If (Informations.IsNothing(vTpUserCode)) Or (String.IsNullOrEmpty(vTpUserCode)) Or (bDefaultAll) Then
            vTpUserCode = ""
        End If



        If (Informations.IsNothing(vMemorableDate)) Or (vMemorableDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vMemorableDate = DateTime.Now
        End If



        If (Informations.IsNothing(vAQuestion)) Or (String.IsNullOrEmpty(vAQuestion)) Or (bDefaultAll) Then
            vAQuestion = ""
        End If



        If (Informations.IsNothing(vTheAnswer)) Or (String.IsNullOrEmpty(vTheAnswer)) Or (bDefaultAll) Then
            vTheAnswer = ""
        End If



        If (Informations.IsNothing(vUserid)) Or (String.IsNullOrEmpty(vUserid)) Or (bDefaultAll) Then
            vUserid = ""
        End If



        If (Informations.IsNothing(vCurrentInsRenewalDate)) Or (vCurrentInsRenewalDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vCurrentInsRenewalDate = DateTime.Now
        End If


        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SIRPartyNetData for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPassword As Object = Nothing, Optional ByRef vMothersMaidenName As Object = Nothing, Optional ByRef vTpIntroducerCode As Object = Nothing, Optional ByRef vTpUserCode As Object = Nothing, Optional ByRef vMemorableDate As Object = Nothing, Optional ByRef vAQuestion As Object = Nothing, Optional ByRef vTheAnswer As Object = Nothing, Optional ByRef vUserid As Object = Nothing, Optional ByRef vCurrentInsRenewalDate As Object = Nothing) As Integer

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


        If Not Informations.IsNothing(vMemorableDate) Then
            If Not Informations.IsDate(vMemorableDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vCurrentInsRenewalDate) Then
            If Not Informations.IsDate(vCurrentInsRenewalDate) Then
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

