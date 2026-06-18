Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Globalization
Imports SharedFiles
Friend NotInheritable Class Policy
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Policy
    '
    ' Date: 28/04/1999
    '
    ' Description: Interfaces with the dSIRProspectPolicy data object.
    '
    ' Edit History: 280499 - CTAF - Initial Version
    '
    ' ***************************************************************** '


    Private Const ACClass As String = "Policy"

    ' Reference to data object
    Private m_oPolicy As dSIRProspectPolicy.SIRProspectPolicy

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Private variables
    Private m_vPartyCnt As Object
    'developer Guie no 17
    Private m_vPolicyID As Object
    Private m_vPolicyTypeID As Object
    Private m_vRenewalDate As Object
    Private m_vNoOfTimesQuoted As Object
    Private m_vTargetPremium As Object
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


    ' Public Properties (BEGIN)

    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)


            m_vPartyCnt = Value
        End Set
    End Property

    Public Property PolicyID() As Object
        Get
            Return m_vPolicyID
        End Get
        Set(ByVal Value As Object)

            m_vPolicyID = Value
        End Set
    End Property

    Public Property PolicyTypeID() As Object
        Get
            Return m_vPolicyTypeID
        End Get
        Set(ByVal Value As Object)


            m_vPolicyTypeID = Value
        End Set
    End Property

    Public Property RenewalDate() As Object
        Get
            Return m_vRenewalDate
        End Get
        Set(ByVal Value As Object)


            m_vRenewalDate = Value
        End Set
    End Property

    Public Property NoOfTimesQuoted() As Object
        Get
            Return m_vNoOfTimesQuoted
        End Get
        Set(ByVal Value As Object)


            m_vNoOfTimesQuoted = Value
        End Set
    End Property

    Public Property TargetPremium() As Object
        Get
            Return m_vTargetPremium
        End Get
        Set(ByVal Value As Object)


            m_vTargetPremium = Value
        End Set
    End Property

    ' Public Properties (END)

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
            m_oPolicy = New dSIRProspectPolicy.SIRProspectPolicy()

            ' Initialise the data object
            m_lReturn = m_oPolicy.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)

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
                If m_oPolicy IsNot Nothing Then
                    m_oPolicy.Dispose()
                    m_oPolicy = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ************************************************************************** '
    ' Name: GetDetails
    '
    ' Description: Gets the required SIRPartyPolicy and populate the Collection
    '
    ' ************************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'Developer Guie No 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = m_oPolicy.SelectSingle(vLockMode:=CInt(vLockMode))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Read the properties off the data object



            'Developer Guie no 98
            m_lReturn = CType(GetProperties(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
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
    'Developer Guie No 101
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the properties


            'Developer Guie No 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the item
            m_lReturn = CType(AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the new key


            vPartyCnt = m_vPartyCnt
            vPolicyID = m_vPolicyID

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
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the record to delete


            'Developer Guie no 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Then delete it
            m_lReturn = CType(DeleteItem(), gPMConstants.PMEReturnCode)
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
    Public Function EditAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' As collections aren't supported in this object, then
            ' emulate an EditAdd by calling DirectAdd

            'Developoer Guie no 98
            'm_lReturn = CType(DirectAdd(vPartyCnt:=vPartyCnt, vPolicyID:=CInt(vPolicyID), vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
            m_lReturn = CType(DirectAdd(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
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
    '
    ' ***************************************************************** '
    Public Function EditUpdate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the properties



            'developer Guie no 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
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
    '
    ' ***************************************************************** '
    Public Function EditDelete(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' As collections aren't supported in this object, then
            ' emulate an EditDelete by calling DirectDelete
            m_lReturn = CType(DirectDelete(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID), gPMConstants.PMEReturnCode)
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
    ' Description:
    '
    '
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the data
            m_lReturn = m_oPolicy.Update()
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
    ' Name: SetProperties
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'Developer Guie No 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If in add mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Then default any missing parameters



                'developer Guie No 98
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Validate
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vPolicyID:=vPolicyID, vPolicyTypeID:=vPolicyTypeID, vRenewalDate:=vRenewalDate, vNoOfTimesQuoted:=vNoOfTimesQuoted, vTargetPremium:=vTargetPremium), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the properties on the data object
            With m_oPolicy



                'Developer Guie No 115
                If (Not Information.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                'Developer Guie No 115
                If (Not Information.IsNothing(vPolicyID)) AndAlso (Not vPolicyID.Equals(0)) Then
                    .PolicyID = vPolicyID
                End If



                'Developer Guie No 115
                If (Not Information.IsNothing(vPolicyTypeID)) AndAlso (Not vPolicyTypeID.Equals(0)) Then
                    .PolicyTypeID = vPolicyTypeID
                End If



                'Developer Guie No 115
                If (Not Information.IsNothing(vRenewalDate)) AndAlso (Not Object.Equals(vRenewalDate, Nothing)) Then


                    'Developer Guide No. 24 
                    .RenewalDate = vRenewalDate
                End If



                'Developer Guie No 115
                If (Not Information.IsNothing(vNoOfTimesQuoted)) AndAlso (Not Object.Equals(vNoOfTimesQuoted, Nothing)) Then


                    'Developer Guide No. 24 
                    .NoOfTimesQuoted = vNoOfTimesQuoted
                End If



                'Developer Guie No 115
                If (Not Information.IsNothing(vTargetPremium)) AndAlso (Not Object.Equals(vTargetPremium, Nothing)) Then


                    'Developer Guide No. 24 (Guide)
                    .TargetPremium = vTargetPremium
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
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetProperties(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oPolicy
                'Developer guie no 118
                vPartyCnt = .PartyCnt




                vPolicyID = .PolicyID




                vPolicyTypeID = .PolicyTypeID






                vRenewalDate = .RenewalDate






                vNoOfTimesQuoted = .NoOfTimesQuoted






                vTargetPremium = .TargetPremium


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
    ' Name: DefaultParameters
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'Developer Guie no 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'Developer Guide No. 151
        If (Information.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If

        If (Information.IsNothing(vPolicyID)) OrElse (vPolicyID.Equals(0)) OrElse (bDefaultAll) Then
            vPolicyID = 0
        End If

        If (Information.IsNothing(vPolicyTypeID)) OrElse (vPolicyTypeID.Equals(0)) OrElse (bDefaultAll) Then
            vPolicyTypeID = 0
        End If

        If (Information.IsNothing(vRenewalDate)) OrElse (vRenewalDate.Equals(0)) OrElse (bDefaultAll) Then
            vRenewalDate = 0
        End If

        If (Information.IsNothing(vNoOfTimesQuoted)) OrElse (vNoOfTimesQuoted.Equals(0)) OrElse (bDefaultAll) Then
            vNoOfTimesQuoted = 0
        End If

        If (Information.IsNothing(vTargetPremium)) OrElse (vTargetPremium.Equals(0)) OrElse (bDefaultAll) Then
            vTargetPremium = 0
        End If
        'Ends
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPolicyID As Object = Nothing, Optional ByRef vPolicyTypeID As Object = Nothing, Optional ByRef vRenewalDate As Object = Nothing, Optional ByRef vNoOfTimesQuoted As Object = Nothing, Optional ByRef vTargetPremium As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' PartyCnt

            If Not Information.IsNothing(vPartyCnt) Then

                Dim dbNumericTemp As Double
                If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' PolicyID

            If Not Information.IsNothing(vPolicyID) Then

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vPolicyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Policy TypeID

            If Not Information.IsNothing(vPolicyTypeID) Then

                Dim dbNumericTemp3 As Double
                If Not Double.TryParse(CStr(vPolicyTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Renewal Date

            If Not Information.IsNothing(vRenewalDate) Then
                If Not Information.IsDate(vRenewalDate) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' No Of Times Quoted

            If Not Information.IsNothing(vNoOfTimesQuoted) Then

                Dim dbNumericTemp4 As Double
                If Not Double.TryParse(CStr(vNoOfTimesQuoted), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Target Premium

            If Not Information.IsNothing(vTargetPremium) Then

                Dim dbNumericTemp5 As Double
                If Not Double.TryParse(CStr(vTargetPremium), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oPolicy

                ' Call the data object to add, using the parameters
                m_lReturn = .Add()

                ' Save the new ID's
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_vPolicyID = .PolicyID
                End If

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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

            With m_oPolicy

                ' delete it.
                m_lReturn = .Delete()

            End With

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPoliciesForParty
    '
    ' Description: Gets all the policies that match party_cnt
    '
    ' ***************************************************************** '
    Public Function GetPoliciesForParty(ByVal v_vPartyCnt As Object, ByRef r_vPolicies As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pass through to the data object
            m_lReturn = m_oPolicy.GetPoliciesForParty(v_vPartyCnt:=v_vPartyCnt, r_vPolicies:=r_vPolicies)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesForParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPolicyDetails
    '
    ' Description: Gets the details for the passed party_cnt and policy_id
    '
    ' ***************************************************************** '
    Public Function GetPolicyDetails(ByVal v_vPartyCnt As Object, ByVal v_vPolicyID As Object, ByRef r_vPolicyTypeID As Object, ByRef r_vRenewalDate As Object, ByRef r_vNoOfTimesQuoted As Object, ByRef r_vTargetPremium As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' pass through to the data object
            m_lReturn = m_oPolicy.GetPolicyDetails(v_vPartyCnt:=v_vPartyCnt, v_vPolicyID:=v_vPolicyID, r_vPolicyTypeID:=r_vPolicyTypeID, r_vRenewalDate:=r_vRenewalDate, r_vNoOfTimesQuoted:=r_vNoOfTimesQuoted, r_vTargetPremium:=r_vTargetPremium)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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