Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports SharedFiles
Friend NotInheritable Class Prospect
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Prospect
    '
    ' Date: 28/04/1999
    '
    ' Description: Interfaces with the dSIRProspect data object.
    '
    ' Edit History: 280499 - CTAF - Initial Version
    '
    ' ***************************************************************** '


    Private Const ACClass As String = "Prospect"

    ' Reference to data object
    Private m_oProspect As dSIRPartyProspect.SIRPartyProspect

    ' *** Private variables ***
    Private m_vPartyCnt As Object
    Private m_vAgentReference As Object
    Private m_vCurrentIntermediary As Object
    Private m_vProspectStatusID As Object
    Private m_vStrengthCodeID As Object
    Private m_vPreviousInsurerCnt As Object
    Private m_vPreviousBrokerCnt As Object
    ' Error Code
    Private m_lReturn As Integer
    ' ************************************************
    ' Added to replace global variables 10/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer


    ' ************* Public Properties (BEGIN) ************************* '
    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)


            m_vPartyCnt = Value
        End Set
    End Property

    Public Property AgentReference() As Object
        Get
            Return m_vAgentReference
        End Get
        Set(ByVal Value As Object)


            m_vAgentReference = Value
        End Set
    End Property

    Public Property CurrentIntermediary() As Object
        Get
            Return m_vCurrentIntermediary
        End Get
        Set(ByVal Value As Object)


            m_vCurrentIntermediary = Value
        End Set
    End Property

    Public Property ProspectStatusID() As Object
        Get
            Return m_vProspectStatusID
        End Get
        Set(ByVal Value As Object)


            m_vProspectStatusID = Value
        End Set
    End Property
    Public Property StrengthCodeID() As Object
        Get
            Return m_vStrengthCodeID
        End Get
        Set(ByVal Value As Object)


            m_vStrengthCodeID = Value
        End Set
    End Property
    Public Property PreviousInsurerCnt() As Object
        Get
            Return m_vPreviousInsurerCnt
        End Get
        Set(ByVal Value As Object)


            m_vPreviousInsurerCnt = Value
        End Set
    End Property
    Public Property PreviousBrokerCnt() As Object
        Get
            Return m_vPreviousBrokerCnt
        End Get
        Set(ByVal Value As Object)


            m_vPreviousBrokerCnt = Value
        End Set
    End Property

    ' ************* Public Properties (END) *************************** '

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' TODO - Change to late bound once data object is finalised
            ' Create a new instance of the data object
            m_oProspect = New dSIRPartyProspect.SIRPartyProspect()

            ' Initialise the data object
            m_lReturn = m_oProspect.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Terminate
    '
    ' Description:
    '
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
                If m_oProspect IsNot Nothing Then
                    m_oProspect.Dispose()
                End If
                m_oProspect = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Gets the required SIRProspect and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set to get the party count

            m_lReturn = SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=CInt(vPartyCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Select it into the objects properties


            m_lReturn = m_oProspect.SelectSingle(vLockMode:=CInt(vLockMode))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext
    '
    ' Description: Gets the properties from the data object
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the values from the properties





            'Developer Guide No. 98
            m_lReturn = GetProperties(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectAdd
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the properties





            'Developer Guide No. 98
            m_lReturn = SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the item
            m_lReturn = AddItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Don't do this, as it's alway 0...
            ' Return the party count
            'vPartyCnt = m_vPartyCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete
    '
    ' Description: Deletes the given partycnt from the database.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the record to delete

            m_lReturn = SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=CInt(vPartyCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Then delete it
            m_lReturn = DeleteItem()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function EditAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' As collections aren't supported in this object, then
            ' emulate an EditDelete by calling DirectDelete
            m_lReturn = DirectAdd(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function EditUpdate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the properties





            'Developer Guide No. 98
            m_lReturn = SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByRef vPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' As collections aren't supported in this object, then
            ' emulate an EditDelete by calling DirectDelete
            m_lReturn = DirectDelete(vPartyCnt:=vPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update
    '
    ' Description: Updates the database.
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oProspect.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DefaultParameters
    '
    ' Description:
    '
    ' ***************************************************************** '
    'Developer Guide No. 101 (Guide)
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As String = "", Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        'Developer Guide No. 44 (Guide)
        If (Information.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
            vPartyCnt = 0
        End If



        If (Information.IsNothing(vAgentReference)) Or (String.IsNullOrEmpty(vAgentReference)) Or (bDefaultAll) Then
            vAgentReference = ""
        End If



        'Developer Guide No. 44 (Guide)
        If (Information.IsNothing(vCurrentIntermediary)) OrElse (vCurrentIntermediary.Equals(0)) Or (bDefaultAll) Then
            vCurrentIntermediary = 0
        End If



        'Developer Guide No. 44 (Guide)
        If (Information.IsNothing(vProspectStatusID)) OrElse (vProspectStatusID.Equals(0)) Or (bDefaultAll) Then
            vProspectStatusID = 0
        End If



        'Developer Guide No. 44 (Guide)
        If (Information.IsNothing(vStrengthCodeID)) OrElse (vStrengthCodeID.Equals(0)) Or (bDefaultAll) Then
            vStrengthCodeID = 0
        End If



        'Developer Guide No. 44 (Guide)
        If (Information.IsNothing(vPreviousInsurerCnt)) OrElse (vPreviousInsurerCnt.Equals(0)) Or (bDefaultAll) Then
            vPreviousInsurerCnt = 0
        End If



        'Developer Guide No. 44 (Guide)
        If (Information.IsNothing(vPreviousBrokerCnt)) OrElse (vPreviousBrokerCnt.Equals(0)) Or (bDefaultAll) Then
            vPreviousBrokerCnt = 0
        End If


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate
    '
    ' Description: Validates the paramters
    '
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' PartyCnt

        If Not Information.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Agent Reference

        If Not Information.IsNothing(vAgentReference) Then

            If Object.Equals(vAgentReference, Nothing) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Current Intermediary

        If Not Information.IsNothing(vCurrentIntermediary) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vCurrentIntermediary), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Prospect Status ID

        If Not Information.IsNothing(vProspectStatusID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vProspectStatusID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Strength Code ID

        If Not Information.IsNothing(vStrengthCodeID) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vStrengthCodeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Previous Insurer Cnt

        If Not Information.IsNothing(vPreviousInsurerCnt) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vPreviousInsurerCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Previous Broker ID

        If Not Information.IsNothing(vPreviousBrokerCnt) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vPreviousBrokerCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties
    '
    ' Description: Sets the supplied SIRProspect property values.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101 (Guide)
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Default all the parameters if we're adding
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then



                'Developer Guide No. 98)
                m_lReturn = DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate the parameters
            m_lReturn = Validate(vPartyCnt:=vPartyCnt, vAgentReference:=vAgentReference, vCurrentIntermediary:=vCurrentIntermediary, vProspectStatusID:=vProspectStatusID, vStrengthCodeID:=vStrengthCodeID, vPreviousInsurerCnt:=vPreviousInsurerCnt, vPreviousBrokerCnt:=vPreviousBrokerCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the data object's properties now

            With m_oProspect



                'Developer Guide No 115
                If (Not Information.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                'Developer Guide No 115
                If (Not Information.IsNothing(vAgentReference)) AndAlso (Not Object.Equals(vAgentReference, Nothing)) Then


                    'Developer Guide No. 24 (Guide)
                    AgentReference = vAgentReference
                End If



                'Developer Guide No 115
                If (Not Information.IsNothing(vCurrentIntermediary)) AndAlso (Not Object.Equals(vCurrentIntermediary, Nothing)) Then


                    'Developer Guide No. 24 (Guide)
                    .CurrentIntermediary = vCurrentIntermediary
                End If



                'Developer Guide No 115
                If (Not Information.IsNothing(vProspectStatusID)) AndAlso (Not vProspectStatusID.Equals(0)) Then
                    .ProspectStatusID = vProspectStatusID
                End If



                'If (Not Information.IsNothing(vStrengthCodeID)) And (Not vStrengthCodeID.Equals(0)) Then
                If (Not Information.IsNothing(vStrengthCodeID)) AndAlso (Not vStrengthCodeID.Equals(0)) Then
                    .StrengthCodeID = vStrengthCodeID
                End If


                'Developer Guide No 115
                If (Not Information.IsNothing(vPreviousInsurerCnt)) AndAlso (Not vPreviousInsurerCnt.Equals(0)) Then
                    .PreviousInsurerCnt = vPreviousInsurerCnt
                End If


                'Developer Guide No 115
                If (Not Information.IsNothing(vPreviousBrokerCnt)) AndAlso (Not vPreviousBrokerCnt.Equals(0)) Then
                    .PreviousBrokerCnt = vPreviousBrokerCnt
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
    ' Name: GetProperties
    '
    ' Description: Returns the supplied SIRProspect property values.
    '
    ' ***************************************************************** '
    'Developer Guide No. 101 (Guide)
    Public Function GetProperties(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vAgentReference As Object = Nothing, Optional ByRef vCurrentIntermediary As Object = Nothing, Optional ByRef vProspectStatusID As Object = Nothing, Optional ByRef vStrengthCodeID As Object = Nothing, Optional ByRef vPreviousInsurerCnt As Object = Nothing, Optional ByRef vPreviousBrokerCnt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oProspect


                'If Not Information.IsNothing(vPartyCnt) Then
                '    vPartyCnt = .PartyCnt
                'End If


                'If Not Information.IsNothing(vAgentReference) Then


                '    vAgentReference = .AgentReference
                'End If


                'If Not Information.IsNothing(vCurrentIntermediary) Then


                '    vCurrentIntermediary = .CurrentIntermediary
                'End If


                'If Not Information.IsNothing(vProspectStatusID) Then
                '    vProspectStatusID = .ProspectStatusID
                'End If

                'If Not Information.IsNothing(vStrengthCodeID) Then
                '    vStrengthCodeID = .StrengthCodeID
                'End If

                'If Not Information.IsNothing(vPreviousInsurerCnt) Then
                '    vPreviousInsurerCnt = .PreviousInsurerCnt
                'End If

                'If Not Information.IsNothing(vPreviousBrokerCnt) Then
                '    vPreviousBrokerCnt = .PreviousBrokerCnt
                'End If
                vPartyCnt = .PartyCnt
                vAgentReference = .AgentReference
                vCurrentIntermediary = .CurrentIntermediary
                vProspectStatusID = .ProspectStatusID
                vStrengthCodeID = .StrengthCodeID
                vPreviousInsurerCnt = .PreviousInsurerCnt
                vPreviousBrokerCnt = .PreviousBrokerCnt
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
    ' Name: AddItem
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oProspect

                ' Add using the objects properties as values
                m_lReturn = .Add()

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oProspect

                ' Set the party count to be deleted...
                '.PartyCnt = m_vPartyCnt

                ' and delete it.
                m_lReturn = .Delete()

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Sub New()
        MyBase.New()

        'UPGRADE_NOTE: (7001) The following code block (empty try-catch) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
        'Try 
        '
        ' Class Initialise
        '
        'Catch excep As System.Exception
        '
        '
        '
        '
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

End Class