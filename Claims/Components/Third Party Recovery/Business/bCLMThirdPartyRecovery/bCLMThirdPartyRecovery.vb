Option Strict Off
Option Explicit On
Imports SSP.Shared

Friend NotInheritable Class CLMThirdPartyRecovery
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: CLMThirdPartyRecovery
    '
    ' Date: 24/08/2000
    '
    ' Description: Describes the CLMThirdPartyRecovery attributes.
    '
    ' Edit History:Pandu
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "CLMThirdPartyRecovery"

    ' Update Status
    Private m_iDatabaseStatus As Integer

    ' Instance of Data component
    Private m_dCLMTPRecoveryRecovery As dCLMTPRecovery.CLMThirdPartyRecovery

    ' Error Code
    Private m_lReturn As Integer

    ' Primary Keys to work with
    Private m_lRecoveryId As Integer
    ' PRIVATE Data Members (End)

    ' ************************************************
    ' Added to replace global variables 19/12/2003
    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer
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

    Public Property RecoveryId() As Integer
        Get

            Return m_lRecoveryId

        End Get
        Set(ByVal Value As Integer)

            m_lRecoveryId = Value

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
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
            m_dCLMTPRecoveryRecovery = New dCLMTPRecovery.CLMThirdPartyRecovery()

            m_lReturn = m_dCLMTPRecoveryRecovery.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName)

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
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
                If m_dCLMTPRecoveryRecovery IsNot Nothing Then
                    m_dCLMTPRecoveryRecovery.Dispose()

                End If
                m_dCLMTPRecoveryRecovery = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the CLMThirdPartyRecovery.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vInitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults


















            m_lReturn = DefaultParameters(bDefaultAll:=True, vRecoveryId:=CByte(vRecoveryId), vRecoveryTypeID:=CByte(vRecoveryTypeID), vPerilId:=CByte(vPerilId), vCurrencyID:=CByte(vCurrencyID), vInitialReserve:=CByte(vInitialReserve), vRevisedReserve:=CByte(vRevisedReserve), vReceivedToDate:=CByte(vReceivedToDate), vRevisionCount:=CByte(vRevisionCount), vReceiptId:=CByte(vReceiptId), vPartyClaimID:=CByte(vPartyClaimID), vReceiptAmount:=CByte(vReceiptAmount), vDateofReceipt:=CStr(vDateofReceipt), vPaymentId:=CByte(vPaymentId), vClaimID:=CByte(vClaimID), vPaymentAmount:=CByte(vPaymentAmount), vDateofPayment:=CStr(vDateofPayment), vComments:=CStr(vComments), vTable:=CByte(vTable))

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
    ' Description: Sets the supplied CLMThirdPartyRecovery property values.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function SetProperties(ByRef iStatus As Integer, Optional ByRef vRecoveryId As Integer = 0, Optional ByRef vRecoveryTypeID As Integer = 0, Optional ByRef vPerilId As Integer = 0, Optional ByRef vCurrencyID As Integer = 0, Optional ByRef vInitialReserve As Decimal = 0, Optional ByRef vRevisedReserve As Decimal = 0, Optional ByRef vReceivedToDate As Decimal = 0, Optional ByRef vRevisionCount As Integer = 0, Optional ByRef vReceiptId As Integer = 0, Optional ByRef vPartyClaimID As Integer = 0, Optional ByRef vReceiptAmount As Decimal = 0, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Integer = 0, Optional ByRef vClaimID As Integer = 0, Optional ByRef vPaymentAmount As Decimal = 0, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As String = "", Optional ByRef vTable As Integer = 0, Optional ByRef vTaxAmount As Decimal = 0, Optional ByRef vReceiptToLossRate As Double = 0) As Integer

        Dim result As Integer = 0
        Dim bDataChanged As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' If Add Mode
            If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

                ' Default Any Missing Parameters


                m_lReturn = DefaultParameters(bDefaultAll:=True, vRecoveryId:=vRecoveryId, vRecoveryTypeID:=vRecoveryTypeID, vPerilId:=vPerilId, vCurrencyID:=vCurrencyID, vInitialReserve:=vInitialReserve, vRevisedReserve:=vRevisedReserve, vReceivedToDate:=vReceivedToDate, vRevisionCount:=vRevisionCount, vReceiptId:=vReceiptId, vPartyClaimID:=vPartyClaimID, vReceiptAmount:=vReceiptAmount, vDateofReceipt:=CStr(vDateofReceipt), vPaymentId:=vPaymentId, vClaimID:=vClaimID, vPaymentAmount:=vPaymentAmount, vDateofPayment:=CStr(vDateofPayment), vComments:=vComments, vTable:=vTable, vTaxAmount:=vTaxAmount, vReceiptToLossRate:=vReceiptToLossRate)


                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If

            End If

            ' Validate Parameters
            'm_lReturn& = Validate(vRecoveryCnt:=vRecoveryCnt, vReserveID:=vReserveID, _
            'vPerilID:=vPerilID, vRecoveryTypeID:=vRecoveryTypeID, _
            'vCurrencyID:=vCurrencyID, vInitialReserve:=vInitialReserve)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Set Data Changed Flag to False
            bDataChanged = False

            ' Set Property values.
            With m_dCLMTPRecoveryRecovery


                If Not Informations.IsNothing(vRecoveryId) Then
                    If .RecoveryID <> vRecoveryId Then
                        .RecoveryID = vRecoveryId
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vRecoveryTypeID) Then
                    If .RecoveryTypeID <> vRecoveryTypeID Then
                        .RecoveryTypeID = vRecoveryTypeID
                        bDataChanged = True
                    End If
                End If


                If Not Informations.IsNothing(vPerilId) Then
                    If .PerilID <> vPerilId Then
                        .PerilID = vPerilId
                        bDataChanged = True
                    End If
                End If




                If (Not Informations.IsNothing(vCurrencyID)) And (Not vCurrencyID.Equals(0)) Then
                    .CurrencyID = vCurrencyID
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vInitialReserve)) And (Not vInitialReserve.Equals(0)) Then
                    .InitialReserve = vInitialReserve
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vRevisedReserve)) And (Not vRevisedReserve.Equals(0)) Then
                    .RevisedReserve = vRevisedReserve
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vReceivedToDate)) And (Not vReceivedToDate.Equals(0)) Then
                    .ReceivedToDate += vReceivedToDate
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vRevisionCount)) And (Not vRevisionCount.Equals(0)) Then
                    .RevisionCount = vRevisionCount
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vReceiptId)) And (Not vReceiptId.Equals(0)) Then
                    .ReceiptID = vReceiptId
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vPartyClaimID)) And (Not vPartyClaimID.Equals(0)) Then
                    .PartyClaimID = vPartyClaimID
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vReceiptAmount)) And (Not vReceiptAmount.Equals(0)) Then
                    .ReceiptAmount = vReceiptAmount
                    bDataChanged = True
                End If




                If (Not Informations.IsNothing(vDateofReceipt)) And (Not Object.Equals(vDateofReceipt, Nothing)) Then


                    '.set_DateofReceipt(vDateofReceipt)
                    Dim DateofReceipt As Object
                    DateofReceipt = vDateofReceipt
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vPaymentId)) And (Not vPaymentId.Equals(0)) Then
                    .PaymentID = vPaymentId
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vClaimID)) And (Not vClaimID.Equals(0)) Then
                    .ClaimID = vClaimID
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vPaymentAmount)) And (Not vPaymentAmount.Equals(0)) Then
                    .PaymentAmount = vPaymentAmount
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vDateofPayment)) And (Not Object.Equals(vDateofPayment, Nothing)) Then


                    '.set_DateofPayment(vDateofPayment)
                    Dim DateofPayment As Object
                    DateofPayment = vDateofPayment
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vComments)) And (Not String.IsNullOrEmpty(vComments)) Then
                    .Comments = vComments
                    bDataChanged = True
                End If




                If (Not Informations.IsNothing(vTable)) And (Not vTable.Equals(0)) Then
                    .Table = vTable
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vTaxAmount)) And (Not vTaxAmount.Equals(0)) Then
                    .TaxAmount = vTaxAmount
                    bDataChanged = True
                End If



                If (Not Informations.IsNothing(vReceiptToLossRate)) And (Not vReceiptToLossRate.Equals(0)) Then
                    .ReceiptToLossRate = vReceiptToLossRate
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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetProperties (Public)
    '
    ' Description: Returns the supplied CLMThirdPartyRecovery property values.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetProperties(ByRef iStatus As Integer, Optional ByRef vRecoveryId As Integer = 0, Optional ByRef vRecoveryTypeID As String = "", Optional ByRef vPerilId As String = "", Optional ByRef vCurrencyID As String = "", Optional ByRef vInitialReserve As String = "", Optional ByRef vRevisedReserve As String = "", Optional ByRef vReceivedToDate As String = "", Optional ByRef vRevisionCount As String = "", Optional ByRef vReceiptId As String = "", Optional ByRef vPartyClaimID As String = "", Optional ByRef vReceiptAmount As String = "", Optional ByRef vDateofReceipt As String = "", Optional ByRef vPaymentId As String = "", Optional ByRef vClaimID As String = "", Optional ByRef vPaymentAmount As String = "", Optional ByRef vDateofPayment As String = "", Optional ByRef vComments As String = "", Optional ByRef vTable As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set Property values.
            With m_dCLMTPRecoveryRecovery


                If Not Informations.IsNothing(vRecoveryId) Then
                    vRecoveryId = .RecoveryID
                End If


                If Not Informations.IsNothing(vRecoveryTypeID) Then

                    If Convert.IsDBNull(.RecoveryTypeID) Or Informations.IsNothing(.RecoveryTypeID) Then
                        vRecoveryTypeID = ""
                    Else
                        vRecoveryTypeID = CStr(.RecoveryTypeID)
                    End If
                End If



                If Not Informations.IsNothing(vPerilId) Then

                    If Convert.IsDBNull(.PerilID) Or Informations.IsNothing(.PerilID) Then
                        vPerilId = ""
                    Else
                        vPerilId = CStr(.PerilID)
                    End If
                End If


                If Not Informations.IsNothing(vCurrencyID) Then

                    If Convert.IsDBNull(.CurrencyID) Or Informations.IsNothing(.CurrencyID) Then
                        vCurrencyID = ""
                    Else
                        vCurrencyID = CStr(.CurrencyID)
                    End If
                End If


                If Not Informations.IsNothing(vInitialReserve) Then

                    If Convert.IsDBNull(.InitialReserve) Or Informations.IsNothing(.InitialReserve) Then
                        vInitialReserve = ""
                    Else
                        vInitialReserve = CStr(.InitialReserve)
                    End If
                End If


                If Not Informations.IsNothing(vRevisedReserve) Then

                    If Convert.IsDBNull(.RevisedReserve) Or Informations.IsNothing(.RevisedReserve) Then
                        vRevisedReserve = ""
                    Else
                        vRevisedReserve = CStr(.RevisedReserve)
                    End If
                End If


                If Not Informations.IsNothing(vReceivedToDate) Then

                    If Convert.IsDBNull(.ReceivedToDate) Or Informations.IsNothing(.ReceivedToDate) Then
                        vReceivedToDate = ""
                    Else
                        vReceivedToDate = CStr(.ReceivedToDate)
                    End If
                End If


                If Not Informations.IsNothing(vRevisionCount) Then

                    If Convert.IsDBNull(.RevisionCount) Or Informations.IsNothing(.RevisionCount) Then
                        vRevisionCount = ""
                    Else
                        vRevisionCount = CStr(.RevisionCount)
                    End If
                End If


                If Not Informations.IsNothing(vReceiptId) Then

                    If Convert.IsDBNull(.ReceiptID) Or Informations.IsNothing(.ReceiptID) Then
                        vReceiptId = ""
                    Else
                        vReceiptId = CStr(.ReceiptID)
                    End If
                End If



                If Not Informations.IsNothing(vPartyClaimID) Then

                    If Convert.IsDBNull(.PartyClaimID) Or Informations.IsNothing(.PartyClaimID) Then
                        vPartyClaimID = ""
                    Else
                        vPartyClaimID = CStr(.PartyClaimID)
                    End If
                End If


                If Not Informations.IsNothing(vReceiptAmount) Then

                    If Convert.IsDBNull(.ReceiptAmount) Or Informations.IsNothing(.ReceiptAmount) Then
                        vReceiptAmount = ""
                    Else
                        vReceiptAmount = CStr(.ReceiptAmount)
                    End If
                End If


                If Not Informations.IsNothing(vDateofReceipt) Then

                    If Convert.IsDBNull(.DateofReceipt) Or Informations.IsNothing(.DateofReceipt) Then
                        vDateofReceipt = ""
                    Else

                        vDateofReceipt = .DateofReceipt
                    End If
                End If


                If Not Informations.IsNothing(vPaymentId) Then

                    If Convert.IsDBNull(.PaymentID) Or Informations.IsNothing(.PaymentID) Then
                        vPaymentId = ""
                    Else
                        vPaymentId = CStr(.PaymentID)
                    End If
                End If


                If Not Informations.IsNothing(vClaimID) Then

                    If Convert.IsDBNull(.ClaimID) Or Informations.IsNothing(.ClaimID) Then
                        vClaimID = ""
                    Else
                        vClaimID = CStr(.ClaimID)
                    End If
                End If


                If Not Informations.IsNothing(vPaymentAmount) Then

                    If Convert.IsDBNull(.PaymentAmount) Or Informations.IsNothing(.PaymentAmount) Then
                        vPaymentAmount = ""
                    Else
                        vPaymentAmount = CStr(.PaymentAmount)
                    End If
                End If


                If Not Informations.IsNothing(vDateofPayment) Then

                    If Convert.IsDBNull(.DateofPayment) Or Informations.IsNothing(.DateofPayment) Then
                        vDateofPayment = ""
                    Else

                        vDateofPayment = .DateofPayment
                    End If
                End If



                If Not Informations.IsNothing(vComments) Then

                    If Convert.IsDBNull(.Comments) Or Informations.IsNothing(.Comments) Then
                        vComments = ""
                    Else
                        vComments = .Comments
                    End If
                End If


                If Not Informations.IsNothing(vTable) Then

                    If Convert.IsDBNull(.Table) Or Informations.IsNothing(.Table) Then
                        vTable = ""
                    Else
                        vTable = CStr(.Table)
                    End If
                End If

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function SelectItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMTPRecoveryRecovery

                ' Set Data object primary key
                .RecoveryID = RecoveryId

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
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectItem Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectItem", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function AddItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMTPRecoveryRecovery

                ' Add a record to the database from the object
                m_lReturn = .Add()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Retain the Primary Key of the CLMThirdPartyRecovery Added
                RecoveryId = .RecoveryID

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function DeleteItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMTPRecoveryRecovery

                ' Set Data object primary key
                .RecoveryID = RecoveryId

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
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function UpdateItem() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_dCLMTPRecoveryRecovery

                ' Set Data object primary key
                .RecoveryID = RecoveryId


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
    ' Description: Sets the Default Values for a CLMThirdPartyRecovery.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vRecoveryId As Byte = 0, Optional ByRef vRecoveryTypeID As Byte = 0, Optional ByRef vPerilId As Byte = 0, Optional ByRef vCurrencyID As Byte = 0, Optional ByRef vInitialReserve As Byte = 0, Optional ByRef vRevisedReserve As Byte = 0, Optional ByRef vReceivedToDate As Byte = 0, Optional ByRef vRevisionCount As Byte = 0, Optional ByRef vReceiptId As Byte = 0, Optional ByRef vPartyClaimID As Byte = 0, Optional ByRef vReceiptAmount As Byte = 0, Optional ByRef vDateofReceipt As String = "", Optional ByRef vPaymentId As Byte = 0, Optional ByRef vClaimID As Byte = 0, Optional ByRef vPaymentAmount As Byte = 0, Optional ByRef vDateofPayment As String = "", Optional ByRef vComments As String = "", Optional ByRef vTable As Byte = 0, Optional ByRef vTaxAmount As Byte = 0, Optional ByRef vReceiptToLossRate As Byte = 0) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vRecoveryId)) Or (vRecoveryId.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vRecoveryId = 0
        End If



        If (Informations.IsNothing(vPerilId)) Or (vPerilId.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vPerilId = 0
        End If



        If (Informations.IsNothing(vRecoveryTypeID)) Or (vRecoveryTypeID.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vRecoveryTypeID = 0
        End If



        If (Informations.IsNothing(vCurrencyID)) Or (vCurrencyID.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vCurrencyID = 0
        End If



        If (Informations.IsNothing(vInitialReserve)) Or (vInitialReserve.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vInitialReserve = 0
        End If




        If (Informations.IsNothing(vRevisedReserve)) Or (vRevisedReserve.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vRevisedReserve = 0
        End If



        If (Informations.IsNothing(vReceivedToDate)) Or (vReceivedToDate.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vReceivedToDate = 0
        End If



        If (Informations.IsNothing(vRevisionCount)) Or (vRevisionCount.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vRevisionCount = 0
        End If



        If (Informations.IsNothing(vReceiptId)) Or (vReceiptId.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vReceiptId = 0
        End If



        If (Informations.IsNothing(vPartyClaimID)) Or (vPartyClaimID.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vPartyClaimID = 0
        End If



        If (Informations.IsNothing(vReceiptAmount)) Or (vReceiptAmount.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vReceiptAmount = 0
        End If



        If (Informations.IsNothing(vDateofReceipt)) Or (String.IsNullOrEmpty(vDateofReceipt)) Then 'Or (bDefaultAll = True)) Then
            vDateofReceipt = ""
        End If



        If (Informations.IsNothing(vPaymentId)) Or (vPaymentId.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vPaymentId = 0
        End If



        If (Informations.IsNothing(vClaimID)) Or (vClaimID.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vClaimID = 0
        End If




        If (Informations.IsNothing(vPaymentAmount)) Or (vPaymentAmount.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vPaymentAmount = 0
        End If




        If (Informations.IsNothing(vDateofPayment)) Or (String.IsNullOrEmpty(vDateofPayment)) Then 'Or (bDefaultAll = True)) Then
            vDateofPayment = ""
        End If



        If (Informations.IsNothing(vComments)) Or (String.IsNullOrEmpty(vComments)) Then 'Or (bDefaultAll = True)) Then
            vComments = ""
        End If



        If (Informations.IsNothing(vTable)) Or (vTable.Equals(0)) Then 'Or (bDefaultAll = True)) Then
            vTable = 0
        End If

        ' Alix - 21/05/2003


        If (Informations.IsNothing(vTaxAmount)) Or (vTaxAmount.Equals(0)) Then
            vTaxAmount = 0
        End If



        If (Informations.IsNothing(vReceiptToLossRate)) Or (vReceiptToLossRate.Equals(0)) Then
            vReceiptToLossRate = 1
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the CLMThirdPartyRecovery for Consistency.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '

    'Private Function Validate(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vInitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Dim lVarRow As Integer
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    ' Validate
    '
    '    If (IsMissing(vRecoveryCnt) = False) Then
    '        If (IsNumeric(vRecoveryCnt) = False) Then
    '            Validate = PMFalse
    '            Exit Function
    '        End If
    '    End If
    ''
    '    If (IsMissing(vReserveID) = False) Then
    '        If (IsNumeric(vReserveID) = False) Then
    '            Validate = PMFalse
    '            Exit Function
    '        End If
    '    End If
    ''
    ''
    '    If (IsMissing(vPerilId) = False) Then
    '        If (IsNumeric(vPerilId) = False) Then
    '            Validate = PMFalse
    '            Exit Function
    '        End If
    '    End If
    '

    'If Not Informations.IsNothing(vRecoveryTypeID) Then

    'Dim dbNumericTemp As Double
    'If Not Double.TryParse(CStr(vRecoveryTypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '
    '

    'If Not Informations.IsNothing(vCurrencyID) Then

    'Dim dbNumericTemp2 As Double
    'If Not Double.TryParse(CStr(vCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '

    'If Not Informations.IsNothing(vInitialReserve) Then

    'Dim dbNumericTemp3 As Double
    'If Not Double.TryParse(CStr(vInitialReserve), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
    'Return gPMConstants.PMEReturnCode.PMFalse
    'End If
    'End If
    '
    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Validate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Validate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
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

