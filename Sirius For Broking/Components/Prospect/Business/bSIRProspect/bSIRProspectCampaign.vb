Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.Data
Imports System.Data.SqlClient
Imports System.Globalization
Imports System.IO
Imports SharedFiles
Friend NotInheritable Class Campaign
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Prospect
    '
    ' Date: 28/04/1999
    '
    ' Description: Interfaces with the dSIRProspectCampaign data object.
    '
    ' Edit History: 280499 - CTAF - Initial Version
    '
    ' ***************************************************************** '


    Private Const ACClass As String = "Campaign"

    ' Reference to data object
    Private m_oCampaign As dSIRProspectCamp.SIRProspectCamp

    ' Error Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Private Variables
    Private m_vPartyCnt As Object
    'developer Guie no 17
    Private m_vRecordNo As Object
    Private m_vCampaignID As Object


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


    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)


            m_vPartyCnt = Value
        End Set
    End Property

    Public Property RecordNo() As Object
        Get
            Return m_vRecordNo
        End Get
        Set(ByVal Value As Object)

            m_vRecordNo = Value
        End Set
    End Property

    Public Property CampaignID() As Object
        Get
            Return m_vCampaignID
        End Get
        Set(ByVal Value As Object)


            m_vCampaignID = Value
        End Set
    End Property

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
            m_oCampaign = New dSIRProspectCamp.SIRProspectCamp()

            ' Initialise the data object
            m_lReturn = m_oCampaign.Initialise(sUserName:=sUserName, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=vDatabase)
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
                If m_oCampaign IsNot Nothing Then
                    m_oCampaign.Dispose()
                End If
                m_oCampaign = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDetails
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the properties on the database object


            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=CInt(vPartyCnt), vRecordNo:=CInt(vRecordNo)), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Select into the database object


            m_lReturn = m_oCampaign.SelectSingle(vLockMode:=CInt(vLockMode))
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
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the properties off the data object



            'Developer Guie No 98
            m_lReturn = CType(GetProperties(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
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
    '
    ' ***************************************************************** '
    'Developer guie No 101
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the properties


            'Developer Guie No 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the item
            m_lReturn = CType(AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the new keys


            vPartyCnt = m_vPartyCnt
            vRecordNo = m_vRecordNo

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
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the properties


            'Developer Guie no 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the item
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
    Public Function EditAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' As collections aren't supported in this object, then
            ' emulate an EditAdd by calling DirectAdd

            m_lReturn = CType(DirectAdd(vPartyCnt:=vPartyCnt, vRecordNo:=CInt(vRecordNo), vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
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
    Public Function EditUpdate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' set the properties of the data object



            'Developer Guie No 98
            m_lReturn = CType(SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMEdit, vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
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
    Public Function EditDelete(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' As collections aren't supported in this object, then
            ' emulate an EditDelete by calling DirectDelete
            m_lReturn = CType(DirectDelete(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo), gPMConstants.PMEReturnCode)
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
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_oCampaign.Update()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
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
    ' Name: AddItem
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oCampaign

                ' Call the data object to add, using the parameters
                m_lReturn = .Add()

                ' Save the new ID's
                If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    m_vRecordNo = .RecordNo
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

            With m_oCampaign

                ' Set the party count to be deleted...
                '.PartyCnt = PartyCnt
                '.RecordNo = RecordNo

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteItem", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue



        'If (Information.IsNothing(vPartyCnt)) Or (vPartyCnt.Equals(0)) Or (bDefaultAll) Then
        'Developer Guie No 151
        If (Information.IsNothing(vPartyCnt)) OrElse (vPartyCnt.Equals(0)) OrElse (bDefaultAll) Then
            vPartyCnt = 0
        End If



        'If (Information.IsNothing(vRecordNo)) Or (vRecordNo.Equals(0)) Or (bDefaultAll) Then
        'Developer Guie No 151
        If (Information.IsNothing(vRecordNo)) OrElse (vRecordNo.Equals(0)) OrElse (bDefaultAll) Then
            vRecordNo = 0
        End If



        'Developer Guie No 151
        If (Information.IsNothing(vCampaignID)) OrElse (vCampaignID.Equals(0)) OrElse (bDefaultAll) Then
            vCampaignID = 0
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'developer Guie no 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If in add mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Then default any missing parameters
                m_lReturn = CType(DefaultParameters(bDefaultAll:=False, vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' Validate any parameters
            m_lReturn = CType(Validate(vPartyCnt:=vPartyCnt, vRecordNo:=vRecordNo, vCampaignID:=vCampaignID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the properties on the data object
            With m_oCampaign
                'Developer Guie No 115
                If (Not Information.IsNothing(vPartyCnt)) AndAlso (Not vPartyCnt.Equals(0)) Then
                    .PartyCnt = vPartyCnt
                End If



                If (Not Information.IsNothing(vRecordNo)) AndAlso (Not vRecordNo.Equals(0)) Then
                    .RecordNo = vRecordNo
                End If



                If (Not Information.IsNothing(vCampaignID)) AndAlso (Not vCampaignID.Equals(0)) Then
                    .CampaignID = vCampaignID
                End If
                'Ends

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
    ' Name: Validate
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' PartyCnt

        If Not Information.IsNothing(vPartyCnt) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vPartyCnt), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' RecordNo

        If Not Information.IsNothing(vRecordNo) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vRecordNo), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' CampaignID

        If Not Information.IsNothing(vCampaignID) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vCampaignID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetProperties
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    'Developer Guie no 101
    Public Function GetProperties(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vRecordNo As Object = Nothing, Optional ByRef vCampaignID As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oCampaign
                'Developer guie no 118
                vPartyCnt = .PartyCnt




                vRecordNo = .RecordNo




                vCampaignID = .CampaignID

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
    ' Name: GetCampaignDetails
    '
    ' Description: Gets the campaign details for the passed party_cnt
    '              and record_no
    '
    ' ***************************************************************** '
    Public Function GetCampaignDetails(ByVal v_vPartyCnt As Object, ByVal v_vRecordNo As Object, ByVal v_vEffectiveDate As Date, ByRef r_vCampaignID As Object, ByRef r_vDescription As Object, ByRef r_vCampaignDate As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pass through to the data object for the details
            m_lReturn = m_oCampaign.GetCampaignDetails(v_vPartyCnt:=v_vPartyCnt, v_vRecordNo:=v_vRecordNo, v_vEffectiveDate:=v_vEffectiveDate, r_vCampaignID:=r_vCampaignID, r_vDescription:=r_vDescription, r_vCampaignDate:=r_vCampaignDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCampaignDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCampaignDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: GetRecordsForParty
    '
    ' Description: Gets all the records for a party_cnt
    '
    ' ***************************************************************** '
    Public Function GetRecordsForParty(ByVal v_vPartyCnt As Object, ByRef r_vRecords As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Call the data object and get the records for a party
            m_lReturn = m_oCampaign.GetRecordsForParty(v_vPartyCnt:=v_vPartyCnt, r_vRecords:=r_vRecords)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRecordsForParty Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecordsForParty", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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