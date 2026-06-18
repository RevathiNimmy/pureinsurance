Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'developer guide no. 129 (guide)
Friend NotInheritable Class SirFreeFormText
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SirFreeFormText
    '
    ' Date: 08/09/1998
    '
    ' Description: Describes the SirFreeFormText attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "SirFreeFormText"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    Private m_dSirFreeFormText As Object

    Private m_vSirFreeFormTextID As Object


    ' Instance of Data component
    'Private m_oComServs As sPMServerCS.PMServerBusinessCS

    Private m_oDatabase As dPMDAO.Database
    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode
    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    ' PRIVATE Data Members (End)
    Public Property SirFreeFormTextID() As Object
        Get
            Return m_vSirFreeFormTextID
        End Get
        Set(ByVal Value As Object)


            m_vSirFreeFormTextID = Value
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
    Public Property dSirFreeFormText() As Object
        Get
            Return m_dSirFreeFormText
        End Get
        Set(ByVal Value As Object)
            m_dSirFreeFormText = Value
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
    Public Function Initialise(ByRef sUserName As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing, Optional ByRef lKeyFldValue As Integer = 0, Optional ByRef sEntName As String = "", Optional ByRef sTxtType As String = "") As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""
        Dim ex As Exception = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Initialisation Code.
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUserName

            ' Create instance of data class
            '    Set m_oComServs = New sPMServerCS.PMServerBusinessCS

            If (sEntName.ToLower().Trim() = "claim") And (sTxtType.ToLower().Trim() = "private") Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIRClaimPrivateText.ClaimPrivateText")

                result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = sMessage + "Failed to create dSIRClaimPrivatetext;"
                End If

            ElseIf ((sEntName.ToLower().Trim() = "claim") And (sTxtType.ToLower().Trim() = "public")) Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIRClaimPublicText.ClaimPublicText")

                result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = sMessage + "Failed to create dSIRClaimPublicText;"
                End If

            ElseIf ((sEntName.ToLower().Trim() = "policy") And (sTxtType.ToLower().Trim() = "private")) Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIRInsFilePrivateText.InsFilePrivateT")

                result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = sMessage + "Failed to create dSIRInsFilePrivateText;"
                End If

            ElseIf ((sEntName.ToLower().Trim() = "policy") And (sTxtType.ToLower().Trim() = "public")) Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIRInsFilePublicText.InsFilePublicText")

                result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = sMessage + "Failed to create dSIRInsFilePublictext;"
                End If

            ElseIf ((sEntName.ToLower().Trim() = "party") And (sTxtType.ToLower().Trim() = "private")) Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIRPartyPrivateText.PartyPrivateText")

                'result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                'If result <> gPMConstants.PMEReturnCode.PMTrue Then
                '    sMessage = sMessage + "Failed to create dSIRPartyPrivatetext;"
                'End If

            ElseIf ((sEntName.ToLower().Trim() = "party") And (sTxtType.ToLower().Trim() = "public")) Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIRPartyPublicText.PartyPublicText")

                result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = sMessage + "Failed to create dSIRPartyPublictext;"
                End If
            ElseIf ((sEntName.ToLower().Trim() = "event") And (sTxtType.ToLower().Trim() = "public")) Then

                m_dSirFreeFormText = gPMFunctions.CreateLateBoundObject("dSIREventPublicText.EventPublicText")

                result = m_dSirFreeFormText.Initialise(sUserName:=ToSafeString(sUserName), sPassword:=ToSafeString(sPassword), iUserID:=ToSafeInteger(iUserID), iSourceID:=ToSafeInteger(iSourceID), iLanguageID:=ToSafeInteger(iLanguageID), iCurrencyID:=ToSafeInteger(iCurrencyID), iLogLevel:=ToSafeInteger(iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=DirectCast(m_oDatabase, dPMDAO.Database))

                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = sMessage + "Failed to create dSIREventPublicText;"
                End If
            End If
            If String.IsNullOrEmpty(sMessage) > 0 Then
                ex = New Exception(sMessage)
                Throw ex
            End If

            If sEntName.ToLower().Trim() = "party" Then

                m_dSirFreeFormText.PartyCnt = lKeyFldValue

            ElseIf (sEntName.ToLower().Trim() = "policy") Then

                m_dSirFreeFormText.InsuranceFileCnt = lKeyFldValue

            ElseIf (sEntName.ToLower().Trim() = "claim") Then

                m_dSirFreeFormText.ClaimCnt = lKeyFldValue

            ElseIf (sEntName.ToLower().Trim() = "event") Then

                m_dSirFreeFormText.EventCnt = lKeyFldValue
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ToSafeString(ACApp), vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            If disposing Then
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the SirFreeFormText.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vInsFilePrivateTextID As Object = Nothing, Optional ByRef vTextLine As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults



            'developer guide no.98
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vInsuranceFileCnt:=vInsuranceFileCnt, vInsFilePrivateTextID:=vInsFilePrivateTextID, vTextLine:=vTextLine), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Error.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaults Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaults", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied SirFreeFormText property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vInsFilePrivateTextID As Object = Nothing, Optional ByRef vTextLine As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vInsuranceFileCnt:=vInsuranceFileCnt, vInsFilePrivateTextID:=vInsFilePrivateTextID, vTextLine:=vTextLine), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            m_lReturn = CType(Validate(vInsuranceFileCnt:=vInsuranceFileCnt, vInsFilePrivateTextID:=vInsFilePrivateTextID, vTextLine:=vTextLine), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dSirFreeFormText


                If Not Informations.IsNothing(vInsuranceFileCnt) Then

                    .FileCnt = vInsuranceFileCnt
                    bDataChanged = True
                End If


                If Not Informations.IsNothing(vInsFilePrivateTextID) Then

                    .TextID = vInsFilePrivateTextID
                    bDataChanged = True
                End If


                If Not Informations.IsNothing(vTextLine) Then

                    .TextLine = vTextLine
                    bDataChanged = True
                End If

                ' If we have changed one of the properties, update the status
                If bDataChanged Then
                    m_iDatabaseStatus = iStatus
                End If

            End With

            Return result

        Catch excep As System.Exception



            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied SirFreeFormText property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vInsFilePrivateTextID As Object = Nothing, Optional ByRef vTextLine As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With dSirFreeFormText


                If Not Informations.IsNothing(vInsuranceFileCnt) Then

                    vInsuranceFileCnt = .FileCnt
                End If


                If Not Informations.IsNothing(vInsFilePrivateTextID) Then

                    vInsFilePrivateTextID = .TextID
                End If


                If Not Informations.IsNothing(vTextLine) Then

                    vTextLine = .TextLine
                End If

                iStatus = m_iDatabaseStatus

            End With

            Return result

        Catch excep As System.Exception



            ' Error.
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

            ' Select a record from the database

            m_lReturn = m_dSirFreeFormText.SelectSingle()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            ' Add a record to the database from the object

            m_lReturn = m_dSirFreeFormText.Add()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            ' Update the record on the database from the object

            m_lReturn = m_dSirFreeFormText.Delete()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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

            ' Update the record on the database from the object

            m_lReturn = m_dSirFreeFormText.Update()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
    ' Description: Sets the Default Values for a SirFreeFormText.
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vInsFilePrivateTextID As Object = Nothing, Optional ByRef vTextLine As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        'developer guide no.44
        'start
        If (Informations.IsNothing(vInsuranceFileCnt)) OrElse (vInsuranceFileCnt.Equals(0)) Or (bDefaultAll) Then
            vInsuranceFileCnt = 0
        End If



        If (Informations.IsNothing(vInsFilePrivateTextID)) OrElse (vInsFilePrivateTextID.Equals(0)) Or (bDefaultAll) Then
            vInsFilePrivateTextID = 0
        End If



        If (Informations.IsNothing(vTextLine)) OrElse (String.IsNullOrEmpty(vTextLine)) Or (bDefaultAll) Then
            vTextLine = ""
        End If
        'end

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the SirFreeFormText for Consistency.
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vInsuranceFileCnt As Object = Nothing, Optional ByRef vInsFilePrivateTextID As Object = Nothing, Optional ByRef vTextLine As Object = Nothing) As Integer

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


        If Not Informations.IsNothing(vInsFilePrivateTextID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vInsFilePrivateTextID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
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
        ' Error.
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