Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

'Developer Guide No: 129
Imports SharedFiles

Friend NotInheritable Class CLMPerilRsrvType
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMPerilTypeReserveType
    '
    ' Date: 30/09/2000
    '
    ' Description: Describes the CLMPerilTypeReserveType attributes.
    '
    ' Edit History: DG
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMPerilTypeReserveType"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMPerilTypeReserveType As dCLMPerilReserveType.PerilReserveType

    ' Error Code
    Private m_lReturn As Integer
    ' ************************************************
    ' Added to replace global variables 18/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    ' ************************************************

    ' Primary Keys to work with
    Private m_lPerilTypeReserveType As Integer
    Private m_lMainReserve As Integer

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

    Public Property PerilTypeReserveType() As Integer
        Get

            Return m_lPerilTypeReserveType

        End Get
        Set(ByVal Value As Integer)

            m_lPerilTypeReserveType = Value

        End Set
    End Property

    Public Property MainReserve() As Integer
        Get

            Return m_lMainReserve

        End Get
        Set(ByVal Value As Integer)

            m_lMainReserve = Value

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
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_sCallingAppName = v_sCallingAppName
            m_iLanguageID = v_iLanguageID
            m_iSourceID = v_iSourceID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel

            ' Create instance of data class
            m_dCLMPerilTypeReserveType = New dCLMPerilReserveType.PerilReserveType()

            m_lReturn = m_dCLMPerilTypeReserveType.Initialise(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, v_vDatabase)

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
                If m_dCLMPerilTypeReserveType IsNot Nothing Then
                    m_dCLMPerilTypeReserveType.Dispose()

                End If
                m_dCLMPerilTypeReserveType = Nothing
            End If
        End If
		Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CLMPerilTypeReserveType.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vPerilTypeReserveType As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer

        ''    On Error GoTo Err_GetDefaults
        ''
        ''    GetDefaults = PMTrue
        '''    DefaultParameters(bDefaultAll As Boolean, _
        ''''    Optional vPerilTypeReserveType As Variant, Optional vDescription As Variant, _
        ''''    Optional vMandatory As Variant, _
        ''''    Optional vName As Variant) As Long
        ''
        ''    ' Get the Defaults
        ''    m_lReturn& = DefaultParameters(bDefaultAll:=True, _
        '''        vPerilTypeReserveType:=vPerilTypeReserveType, vDescription:=vDescription, vMandatory:=vMandatory, _
        '''        vCurrencyID:=vCurrencyID, vName:=vName, vName:=vName, _
        '''        vName:=vName, vRevisedReserve:=vRevisedReserve, vReceivedToDate:=vReceivedToDate, _
        '''        vRevisionCount:=vRevisionCount, vReceiptId:=vReceiptId, vPartyClaimID:=vPartyClaimID, _
        '''        vReceiptAmount:=vReceiptAmount, vDateofReceipt:=vDateofReceipt, vPaymentId:=vPaymentId, _
        '''        vClaimID:=vClaimID, vPaymentAmount:=vPaymentAmount, vDateofPayment:=vDateofPayment, _
        '''        vComments:=vComments, vTable:=vTable)
        ''
        ''    If (m_lReturn& <> PMTrue) Then
        ''        GetDefaults = PMFalse
        ''    End If
        ''
        ''    Exit Function
        ''
        ''
        ''Err_GetDefaults:
        ''
        ''    GetDefaults = PMError
        ''
        ''    ' Log Error Message
        ''    LogMessage m_sUsername, _
        '''        iType:=PMLogOnError, _
        '''        sMsg:="GetDefaults Failed", _
        '''        vApp:=ACApp, _
        '''        vClass:=ACClass, _
        '''        vMethod:="GetDefaults", _
        '''        vErrNo:=Err.Number, _
        '''        vErrDesc:=Err.Description
        ''
        ''    Exit Function

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied CLMPerilTypeReserveType property values.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vPerilTypeReserveTypeId As Object = Nothing, Optional ByRef vReserveTypeId As Object = Nothing, Optional ByRef vPerilTypeId As Object = Nothing, Optional ByRef vMainReserve As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Or iStatus = gPMConstants.PMEComponentAction.PMEdit Then

                ' Default Any Missing Parameters
                m_lReturn = DefaultParameters(bDefaultAll:=True, vPerilTypeReserveTypeId:=vPerilTypeReserveTypeId, vReserveTypeId:=vReserveTypeId, vPerilTypeId:=vPerilTypeId, vMainReserve:=vMainReserve)

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
            With m_dCLMPerilTypeReserveType


                If Not Information.IsNothing(vPerilTypeReserveTypeId) Then
                    If .PerilTypeReserveTypeID <> vPerilTypeReserveTypeId Then
                        .PerilTypeReserveTypeID = vPerilTypeReserveTypeId
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vPerilTypeId) Then
                    If .PerilTypeID <> vPerilTypeId Then
                        .PerilTypeID = vPerilTypeId
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vReserveTypeId) Then
                    If .ReserveTypeId <> vReserveTypeId Then
                        .ReserveTypeId = vReserveTypeId
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vMainReserve) Then
                    If .MainReserve <> vMainReserve Then
                        .MainReserve = vMainReserve
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
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMPerilTypeReserveType

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMPerilTypeReserveType Added
                PerilTypeReserveType = .PerilTypeReserveTypeID

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

            With m_dCLMPerilTypeReserveType

                ' Set Data object primary key
                .PerilTypeReserveTypeID = PerilTypeReserveType

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

            With m_dCLMPerilTypeReserveType

                ' Set Data object primary key
                .PerilTypeReserveTypeID = PerilTypeReserveType


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
    ' Description: Sets the Default Values for a CLMPerilTypeReserveType.
    '
    ' ***************************************************************** '
    'developer guide no.101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vPerilTypeReserveTypeId As Object = Nothing, Optional ByRef vReserveTypeId As Object = Nothing, Optional ByRef vPerilTypeId As Object = Nothing, Optional ByRef vMainReserve As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}


        'developer guide no.118
        If (Information.IsNothing(vPerilTypeReserveTypeId)) OrElse (vPerilTypeReserveTypeId.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vPerilTypeReserveTypeId = 0
        End If



        If (Information.IsNothing(vReserveTypeId)) OrElse (vReserveTypeId.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vReserveTypeId = 0
        End If



        If (Information.IsNothing(vMainReserve)) OrElse (vMainReserve.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vMainReserve = 0
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

End Class

