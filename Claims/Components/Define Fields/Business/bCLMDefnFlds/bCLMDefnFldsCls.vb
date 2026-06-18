Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

'Developer Guide No: 129
Imports SharedFiles

Friend NotInheritable Class CLMDefnFlds
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMDefnFlds
    '
    ' Date: 06/10/1998
    '
    ' Description: Describes the CLMDefnFlds attributes.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMDefnFlds"

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMDefnFlds As dCLMDefnFlds.CLMDefnFlds

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lDataDefnID As Integer
    ' PRIVATE Data Members (End)

    ' ************************************************
    ' Added to replace global variables
    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
    Private m_oDatabase As dPMDAO.Database
    ' ************************************************

    ' PUBLIC Property Procedures (Begin)
    Public Property DatabaseStatus() As Integer
        Get
            Return m_iDatabaseStatus
        End Get
        Set(ByVal Value As Integer)
            m_iDatabaseStatus = Value
        End Set
    End Property
    Public Property DataDefnID() As Integer
        Get
            Return m_lDataDefnID
        End Get
        Set(ByVal Value As Integer)
            m_lDataDefnID = Value
        End Set
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal v_sUsername As String, ByVal v_sPassword As String, ByVal v_iUserID As Integer, ByVal v_iSourceID As Integer, ByVal v_iLanguageID As Integer, ByVal v_iCurrencyID As Integer, ByVal v_iLogLevel As Integer, ByVal v_sCallingAppName As String, Optional ByVal v_vDatabase As dPMDAO.Database = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sUsername = v_sUsername
            m_sPassword = v_sPassword
            m_iUserID = v_iUserID
            m_sCallingAppName = v_sCallingAppName
            m_iLanguageID = v_iLanguageID
            m_iSourceID = v_iSourceID
            m_iCurrencyID = v_iCurrencyID
            m_iLogLevel = v_iLogLevel
            m_oDatabase = v_vDatabase

            ' Create instance of data class
            m_dCLMDefnFlds = New dCLMDefnFlds.CLMDefnFlds()

            m_lReturn = m_dCLMDefnFlds.Initialise(v_sUsername, v_sPassword, v_iUserID, v_iSourceID, v_iLanguageID, v_iCurrencyID, v_iLogLevel, v_sCallingAppName, v_vDatabase)

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
                If m_dCLMDefnFlds IsNot Nothing Then
                    m_dCLMDefnFlds.Dispose()

                End If
                m_dCLMDefnFlds = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CLMDefnFlds.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vIntialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer

    End Function

    ' ***************************************************************** '
    ' Name: SetProperties (Public)
    '
    ' Description: Sets the supplied CLMDefnFlds property values.
    '
    ' ***************************************************************** '
    'Developer Guide No 101
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vTabID As Object = Nothing, Optional ByRef vTabCaption As Object = Nothing, Optional ByRef vMode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Or iStatus = gPMConstants.PMEComponentAction.PMEdit Then

                ' Default Any Missing Parameters

                'Developer Guide NO 98
                m_lReturn = DefaultParameters(bDefaultAll:=True, vDataDefnID:=vDataDefnID, vDescription:=vDescription, vMandatory:=vMandatory, vCaption:=vCaption, vRiskTypeID:=vRiskTypeID, vType:=vType, vDispOrd:=vDispOrd, vReadOnly:=vReadOnly, vClmPrtyTypeID:=vClmPrtyTypeID, vClmLookupID:=vClmLookupID, vMode:=vMode)


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
            With m_dCLMDefnFlds


                If Not Information.IsNothing(vDataDefnID) Then
                    If .DataDefnID <> vDataDefnID Then
                        .DataDefnID = vDataDefnID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vDescription) Then
                    If .Description <> vDescription Then
                        .Description = vDescription
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vMandatory) Then
                    If .Mandatory <> vMandatory Then
                        .Mandatory = vMandatory
                        bDataChanged = True
                    End If
                End If




                If (Not Information.IsNothing(vCaption)) And (Not String.IsNullOrEmpty(vCaption)) Then
                    .Caption = vCaption
                    bDataChanged = True
                End If



                If Not Information.IsNothing(vRiskTypeID) Then
                    If .RiskTypeID <> vRiskTypeID Then
                        .RiskTypeID = vRiskTypeID
                        bDataChanged = True
                    End If
                End If


                If Not Information.IsNothing(vType) Then
                    If .Typee <> vType Then
                        .Typee = vType
                        bDataChanged = True
                    End If
                End If



                If (Not Information.IsNothing(vDispOrd)) And (Not Object.Equals(vDispOrd, Nothing)) Then



                    'Developer Guide No: 24
                    .DispOrd = vDispOrd

                    bDataChanged = True
                End If


                If Not Information.IsNothing(vReadOnly) Then
                    If .ReadOnly_Renamed <> vReadOnly Then
                        .ReadOnly_Renamed = vReadOnly
                        bDataChanged = True
                    End If
                End If



                If (Not Information.IsNothing(vClmPrtyTypeID)) And (Not Object.Equals(vClmPrtyTypeID, Nothing)) Then



                    'Developer Guide No: 24
                    .ClmPrtyTypeID = vClmPrtyTypeID

                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vClmLookupID)) And (Not Object.Equals(vClmLookupID, Nothing)) Then



                    'Developer Guide No: 24
                    .ClmLookupID = vClmLookupID

                    bDataChanged = True
                End If



                'Developer Guide No. 115
                If (Not Information.IsNothing(vTabID)) AndAlso (Not vTabID.Equals(0)) Then
                    .TabID = vTabID
                    bDataChanged = True
                End If



                If (Not Information.IsNothing(vTabCaption)) And (Not String.IsNullOrEmpty(vTabCaption)) Then
                    .TabCaption = vTabCaption
                    bDataChanged = True
                End If


                If Not Information.IsNothing(vMode) Then
                    If .Mode <> vMode Then
                        .Mode = vMode
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
    ' Description: Returns the supplied CLMDefnFlds property values.
    '
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer

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

            With m_dCLMDefnFlds

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMDefnFlds Added
                DataDefnID = .DataDefnID

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

            With m_dCLMDefnFlds

                ' Set Data object primary key
                .DataDefnID = DataDefnID

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

            With m_dCLMDefnFlds

                ' Set Data object primary key
                .DataDefnID = DataDefnID

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
    ' Description: Sets the Default Values for a CLMDefnFlds.
    '
    ' ***************************************************************** '
    'Developer Guie No 101
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRiskTypeID As Object = Nothing, Optional ByRef vType As Object = Nothing, Optional ByRef vDispOrd As Object = Nothing, Optional ByRef vReadOnly As Object = Nothing, Optional ByRef vClmPrtyTypeID As Object = Nothing, Optional ByRef vClmLookupID As Object = Nothing, Optional ByRef vMode As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        'Developer Guide No 151
        'Starts


        If (Information.IsNothing(vDataDefnID)) OrElse (vDataDefnID.Equals(0)) Then
            vDataDefnID = 0
        End If



        If (Information.IsNothing(vMandatory)) OrElse (vMandatory.Equals(0)) Then
            vMandatory = 0
        End If



        If (Information.IsNothing(vDescription)) OrElse (String.IsNullOrEmpty(vDescription)) Then
            vDescription = ""
        End If



        If (Information.IsNothing(vCaption)) OrElse (String.IsNullOrEmpty(vCaption)) Then
            vCaption = ""
        End If



        If (Information.IsNothing(vRiskTypeID)) OrElse (vRiskTypeID.Equals(0)) Then
            vRiskTypeID = 0
        End If



        If (Information.IsNothing(vType)) OrElse (vType.Equals(0)) Then
            vType = 0
        End If



        If (Information.IsNothing(vDispOrd)) OrElse (vDispOrd.Equals(0)) Then
            vDispOrd = 0
        End If



        If (Information.IsNothing(vReadOnly)) OrElse (vReadOnly.Equals(0)) Then
            vReadOnly = 0
        End If




        If (Information.IsNothing(vClmPrtyTypeID)) OrElse (vClmPrtyTypeID.Equals(0)) Then
            'ToDo
            'vClmPrtyTypeID = 0
            vClmPrtyTypeID = Nothing
        End If



        If (Information.IsNothing(vClmLookupID)) OrElse (vClmLookupID.Equals(0)) Then

            'ToDo
            'vClmLookupID = 0
            vClmLookupID = Nothing
        End If



        If (Information.IsNothing(vMode)) OrElse (vMode.Equals(0)) Then
            vMode = 0
        End If
        'Ends

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the CLMDefnFlds for Consistency.
    '
    ' ***************************************************************** '

    'Private Function Validate(Optional ByRef vDataDefnID As Object = Nothing, Optional ByRef vDescription As Object = Nothing, Optional ByRef vMandatory As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vCaption As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer
    'End Function

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