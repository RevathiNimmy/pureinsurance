Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
'Developer Guide No: 129
Imports SharedFiles

Friend NotInheritable Class CLMInfoChklst
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMInfoChklst
    ' Date: 06/10/1998
    ' Description: Describes the CLMInfoChklst attributes.
    ' Edit History:
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMInfoChklst"

    Private m_sUsername As String = ""
    ' Update Status
    Private m_iDatabaseStatus As Integer
    ' Instance of Data component
    Private m_dCLMInfoChklst As dCLMInfoChklst.CLMInfoChklst
    ' Error Code
    Private m_lReturn As Integer
    ' Primary Keys to work with
    Private m_lClmExpServId As Integer
    Private m_oDatabase As dPMDAO.Database


    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property
    Public Property ClmExpServId() As Integer
        Get
            Return m_lClmExpServId
        End Get
        Set(ByVal Value As Integer)
            m_lClmExpServId = Value
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
            m_dCLMInfoChklst = New dCLMInfoChklst.CLMInfoChklst()

            'TR - Set local variables
            m_sUsername = sUsername

            m_lReturn = m_dCLMInfoChklst.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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
            If disposing Then
                If m_dCLMInfoChklst IsNot Nothing Then
                    m_dCLMInfoChklst.Dispose()

                End If
                m_dCLMInfoChklst = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied CLMInfoChklst property values.
    '
    ' JMK 25/05/2001 additional optional parameters:- vInfoStatus; vUnderwritingOrAgency
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing, Optional ByRef vInfoStatus As Object = Nothing, Optional ByRef vUnderwritingOrAgency As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If (iStatus = gPMConstants.PMEComponentAction.PMAdd) Or (iStatus = gPMConstants.PMEComponentAction.PMEdit And vInfoStatus) Then
                ' Default Any Missing Parameters



                'developer guide no.98
                m_lReturn = DefaultParameters(bDefaultAll:=True, vClmExpServId:=vClmExpServId, vClaim_Id:=vClaim_Id, vExpServId:=vExpServId, vPrtyClmId:=vPrtyClmId, vServTypeId:=vServTypeId, vService:=vService, vDescription:=vDescription, vReference:=vReference, vContact:=vContact, vDateReq:=vDateReq, vDateCrit:=vDateCrit, vDateRecv:=vDateRecv)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dCLMInfoChklst


                If Not Information.IsNothing(vClmExpServId) Then
                    If .ClmExpServId <> vClmExpServId Then
                        .ClmExpServId = vClmExpServId
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vClaim_Id) Then
                    If .Claim_Id <> vClaim_Id Then
                        .Claim_Id = vClaim_Id
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vExpServId) Then
                    If .ExpServId <> vExpServId Then
                        .ExpServId = vExpServId
                        bDataChanged = True
                    End If
                End If

                'DC011104 PN16151 maybe cheating but if not set to anything set to zero
                'If vPrtyClmId = StringsHelper.ToDoubleSafe("") Then
                If Convert.ToString(vPrtyClmId) = "" Then
                    vPrtyClmId = 0
                End If


                If Not Information.IsNothing(vPrtyClmId) Then
                    If .PrtyClmId <> vPrtyClmId Then
                        .PrtyClmId = vPrtyClmId
                        bDataChanged = True
                    End If
                End If
                'DC011104 -end



                If (Not Information.IsNothing(vServTypeId)) AndAlso (Not vServTypeId.Equals(0)) Then
                    .ServTypeId = vServTypeId
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vService)) AndAlso (Not String.IsNullOrEmpty(vService)) Then
                    .Service = vService
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vDescription)) AndAlso (Not String.IsNullOrEmpty(vDescription)) Then
                    .Description = vDescription
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vReference)) AndAlso (Not String.IsNullOrEmpty(vReference)) Then
                    .Reference = vReference
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vContact)) AndAlso (Not String.IsNullOrEmpty(vContact)) Then
                    .Contact = vContact
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vDateReq)) AndAlso (Not Object.Equals(vDateReq, Nothing)) Then



                    'Developer Guide No: 24
                    .DateReq = vDateReq
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vDateCrit)) AndAlso (Not Object.Equals(vDateCrit, Nothing)) Then



                    'Developer Guide No: 24
                    .DateCrit = vDateCrit
                    bDataChanged = True
                End If




                If (Not Information.IsNothing(vDateRecv)) AndAlso (Not Object.Equals(vDateRecv, Nothing)) Then



                    'Developer Guide No: 24
                    .DateRecv = vDateRecv
                    bDataChanged = True
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
    ' Description: Returns the supplied CLMInfoChklst property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dCLMInfoChklst
                'developer guide no.118
                'start

                vClmExpServId = .ClmExpServId


                If Convert.IsDBNull(.Claim_Id) Or IsNothing(.Claim_Id) Then
                    vClaim_Id = ""
                Else
                    vClaim_Id = CStr(.Claim_Id)
                End If



                If Convert.IsDBNull(.ExpServId) Or IsNothing(.ExpServId) Then
                    vExpServId = ""
                Else
                    vExpServId = CStr(.ExpServId)
                End If


                If Convert.IsDBNull(.PrtyClmId) Or IsNothing(.PrtyClmId) Then
                    vPrtyClmId = ""
                Else
                    vPrtyClmId = CStr(.PrtyClmId)
                End If


                If Convert.IsDBNull(.ServTypeId) Or IsNothing(.ServTypeId) Then
                    vServTypeId = ""
                Else
                    vServTypeId = CStr(.ServTypeId)
                End If


                If Convert.IsDBNull(.Service) Or IsNothing(.Service) Then
                    vService = ""
                Else
                    vService = .Service
                End If

                If Convert.IsDBNull(.Description) Or IsNothing(.Description) Then
                    vDescription = ""
                Else
                    vDescription = .Description
                End If


                If Convert.IsDBNull(.Reference) Or IsNothing(.Reference) Then
                    vReference = ""
                Else
                    vReference = .Reference
                End If


                If Convert.IsDBNull(.Contact) Or IsNothing(.Contact) Then
                    vContact = ""
                Else
                    vContact = .Contact
                End If

                If Convert.IsDBNull(.DateReq) Or IsNothing(.DateReq) Then
                    vDateReq = ""
                Else

                    vDateReq = .DateReq
                End If


                If Convert.IsDBNull(.DateCrit) Or IsNothing(.DateCrit) Then
                    vDateCrit = ""
                Else

                    vDateCrit = .DateCrit
                End If


                If Convert.IsDBNull(.DateRecv) Or IsNothing(.DateRecv) Then
                    vDateRecv = ""
                Else

                    vDateRecv = .DateRecv
                End If

                'end

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
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMInfoChklst

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMInfoChklst Added
                ClmExpServId = .ClmExpServId

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

            With m_dCLMInfoChklst

                ' Set Data object primary key
                .ClmExpServId = ClmExpServId

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

            With m_dCLMInfoChklst

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
    ' Description: Sets the Default Values for a CLMInfoChklst.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vClmExpServId As Object = Nothing, Optional ByRef vClaim_Id As Object = Nothing, Optional ByRef vExpServId As Object = Nothing, Optional ByRef vPrtyClmId As Object = Nothing, Optional ByRef vServTypeId As Object = Nothing, Optional ByRef vService As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vReference As Object = Nothing, Optional ByRef vContact As Object = Nothing, Optional ByRef vDateReq As Object = Nothing, Optional ByRef vDateCrit As Object = Nothing, Optional ByRef vDateRecv As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.44
        'start


        If (Information.IsNothing(vClmExpServId)) OrElse (vClmExpServId.Equals(0)) Then
            vClmExpServId = 0
        End If



        If (Information.IsNothing(vExpServId)) OrElse (vExpServId.Equals(0)) Then
            vExpServId = 0
        End If



        If (Information.IsNothing(vClaim_Id)) OrElse (vClaim_Id.Equals(0)) Then
            vClaim_Id = 0
        End If



        If (Information.IsNothing(vPrtyClmId)) OrElse (vPrtyClmId.Equals(0)) Then
            vPrtyClmId = 0
        End If



        If (Information.IsNothing(vServTypeId)) OrElse (vServTypeId.Equals(0)) Then
            vServTypeId = 0
        End If
        'end


        If (Information.IsNothing(vService)) Or (String.IsNullOrEmpty(vService)) Then
            vService = ""
        End If



        If (Information.IsNothing(vDescription)) Or (String.IsNullOrEmpty(vDescription)) Then
            vDescription = ""
        End If



        If (Information.IsNothing(vReference)) Or (String.IsNullOrEmpty(vReference)) Then
            vReference = ""
        End If



        If (Information.IsNothing(vContact)) Or (String.IsNullOrEmpty(vContact)) Then
            vContact = ""
        End If



        If (Information.IsNothing(vDateReq)) Or (String.IsNullOrEmpty(vDateReq)) Then
            vDateReq = ""
        End If



        If (Information.IsNothing(vDateCrit)) Or (String.IsNullOrEmpty(vDateCrit)) Then
            vDateCrit = ""
        End If



        If (Information.IsNothing(vDateRecv)) Or (String.IsNullOrEmpty(vDateRecv)) Then
            vDateRecv = ""
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

