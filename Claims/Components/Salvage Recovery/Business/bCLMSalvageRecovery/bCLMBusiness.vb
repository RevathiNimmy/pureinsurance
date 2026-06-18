Option Strict Off
Option Explicit On
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24/08/2000
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMSalvageRecovery.
    '
    ' Edit History:Pandu
    ' SJP14062002 - getUnderWritingOrAgency and getUnderwritingType
    '               use new product options scheme
    ' ***************************************************************** '


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


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of CLMSalvageRecoverys (Private)
    Private m_oCLMSalvageRecoverys As bCLMSalvageRecovery.CLMSalvageRecoverys

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Primary Keys to work with
    Private m_lRecoveryId As Integer

    Private m_lClaimId As Integer

    Private m_sUnderwritingOrAgency As String = ""

    ' PM Lookup Business Component (Private)
    Private m_oLookup As BPMLOOKUP.Business

    'JMK 14/11/2001 - Underwriting hidden option
    Private m_sUnderwritingType As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public Property CurrentRecord() As Integer
        Get

            Return m_lCurrentRecord

        End Get
        Set(ByVal Value As Integer)

            Select Case Value
                Case Is < 1
                    m_lCurrentRecord = 0
                Case Is > m_oCLMSalvageRecoverys.Count()
                    m_lCurrentRecord = m_oCLMSalvageRecoverys.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Number in Collection
            Return m_oCLMSalvageRecoverys.Count()

        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get

            Return m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Return m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            Return m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            Return m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            Return m_dtEffectiveDate

        End Get
    End Property


    Public Property RecoveryId() As Integer
        Get

            Return m_lRecoveryId

        End Get
        Set(ByVal Value As Integer)

            m_lRecoveryId = Value

        End Set
    End Property


    Public Property ClaimID() As Integer
        Get

            Return m_lClaimId

        End Get
        Set(ByVal Value As Integer)

            m_lClaimId = Value

        End Set
    End Property

    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    ' JMK 14/11/2001 "A" for Underwriting Agency and "U" for Reinsurance
    Public ReadOnly Property UnderwritingType() As String
        Get

            If m_sUnderwritingType = "" Then
                m_lReturn = getUnderwritingType()
            End If

            Return m_sUnderwritingType

        End Get
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
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long





        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create CLMSalvageRecoverys Collection
            m_oCLMSalvageRecoverys = New bCLMSalvageRecovery.CLMSalvageRecoverys()

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                m_oCLMSalvageRecoverys = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    '
    ' Description: Set the optional process modes.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the process modes to the property members.


            If Not Informations.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Informations.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Informations.IsNothing(vProcessMode) Then

                m_lProcessMode = CInt(vProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single CLMSalvageRecovery directly into the database.
    '        Note: The CLMSalvageRecovery will NOT be added to the collection.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vinitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CLMSalvageRecovery
            oCLMSalvageRecovery = New bCLMSalvageRecovery.CLMSalvageRecovery()
            m_lReturn = CType(oCLMSalvageRecovery.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMSalvageRecovery Attributes
















            m_lReturn = CType(oCLMSalvageRecovery.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vRecoveryId:=CInt(vRecoveryId), vRecoveryTypeID:=CInt(vRecoveryTypeID), vPerilId:=CInt(vPerilId), vCurrencyID:=CInt(vCurrencyID), vinitialReserve:=CDec(vinitialReserve), vRevisedReserve:=CDec(vRevisedReserve), vReceivedToDate:=CDec(vReceivedToDate), vRevisionCount:=CInt(vRevisionCount), vReceiptId:=CInt(vReceiptId), vPartyClaimID:=CInt(vPartyClaimID), vReceiptAmount:=CDec(vReceiptAmount), vDateofReceipt:=vDateofReceipt, vPaymentId:=CInt(vPaymentId), vClaimID:=CInt(vClaimID), vPaymentAmount:=CDec(vPaymentAmount), vDateofPayment:=vDateofPayment, vComments:=CStr(vComments), vTable:=CInt(vTable)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            ' Add the CLMSalvageRecovery to the Database
            m_lReturn = CType(oCLMSalvageRecovery.AddItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            ' Retain the Primary Key of the CLMSalvageRecovery Added
            With oCLMSalvageRecovery
                RecoveryId = .RecoveryId
            End With


            oCLMSalvageRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single CLMSalvageRecovery directly from the database.
    '        Note: The CLMSalvageRecovery will NOT be deleted from the collection.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vRecoveryId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new CLMSalvageRecovery
            oCLMSalvageRecovery = New bCLMSalvageRecovery.CLMSalvageRecovery()
            m_lReturn = CType(oCLMSalvageRecovery.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Set CLMSalvageRecovery Primary Key

            m_lReturn = CType(oCLMSalvageRecovery.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMDelete, vRecoveryId:=CInt(vRecoveryId)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            ' Delete the CLMSalvageRecovery from the Database
            m_lReturn = CType(oCLMSalvageRecovery.DeleteItem(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            oCLMSalvageRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="recovery_id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckIDSQL, sSQLName:=ACCheckIDName, bStoredProcedure:=ACCheckIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required CLMSalvageRecoverys and populate the Collection
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetDetails(Optional ByRef vLockMode As Object = Nothing, Optional ByRef vPerilId As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oFields As ADODB.Fields
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Peril_id", vValue:=CStr(vPerilId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim alRecoveryId(lRecordCount) As Integer

            For lSub As Integer = 0 To lRecordCount - 1
                oFields = m_oDatabase.Records.Item(lSub + 1).Fields()
                alRecoveryId(lSub) = gPMFunctions.NullToLong(oFields("Recovery_Id"))
            Next

            ' Yes, load them into the collection
            For lSub As Integer = 0 To lRecordCount - 1

                ' Create New
                oCLMSalvageRecovery = New bCLMSalvageRecovery.CLMSalvageRecovery()
                m_lReturn = CType(oCLMSalvageRecovery.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

                ' Set component primary keys from current record
                With oCLMSalvageRecovery
                    .RecoveryId = alRecoveryId(lSub)

                    m_lReturn = CType(.SelectItem(), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End With

                ' Add CLMSalvageRecovery to collection
                If m_oCLMSalvageRecoverys.Count = 0 Then
                    m_oCLMSalvageRecoverys.Add(Nothing)
                End If
                m_lReturn = CType(m_oCLMSalvageRecoverys.Add(oNewCLMSalvageRecovery:=oCLMSalvageRecovery), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                oCLMSalvageRecovery = Nothing
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetNext (Public)
    '
    ' Description: Gets the required CLMSalvageRecoverys and populate the Collection
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetNext(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vinitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oCLMSalvageRecoverys.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
                'Else
                'GetNext = PMEOF
            End If

            oCLMSalvageRecovery = m_oCLMSalvageRecoverys.Item(m_lCurrentRecord)

            ' Get the CLMSalvageRecovery Property Values


















            m_lReturn = CType(oCLMSalvageRecovery.GetProperties(iStatus, vRecoveryId:=CInt(vRecoveryId), vRecoveryTypeID:=CStr(vRecoveryTypeID), vPerilId:=CStr(vPerilId), vCurrencyID:=CStr(vCurrencyID), vinitialReserve:=CStr(vinitialReserve), vRevisedReserve:=CStr(vRevisedReserve), vReceivedToDate:=CStr(vReceivedToDate), vRevisionCount:=CStr(vRevisionCount), vReceiptId:=CStr(vReceiptId), vPartyClaimID:=CStr(vPartyClaimID), vReceiptAmount:=CStr(vReceiptAmount), vDateofReceipt:=CStr(vDateofReceipt), vPaymentId:=CStr(vPaymentId), vClaimID:=CStr(vClaimID), vPaymentAmount:=CStr(vPaymentAmount), vDateofPayment:=CStr(vDateofPayment), vComments:=CStr(vComments), vTable:=CStr(vTable)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            oCLMSalvageRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditAdd (Public)
    '
    ' Description: Adds the supplied CLMSalvageRecovery into the Collection.
    '              After the Add, lKey should be equal to the number
    '              of items in the collection.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vinitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing, Optional ByRef vTaxAmount As Object = Nothing, Optional ByRef vReceiptToLossRate As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oCLMSalvageRecoverys.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new CLMSalvageRecovery
            oCLMSalvageRecovery = New bCLMSalvageRecovery.CLMSalvageRecovery()
            m_lReturn = CType(oCLMSalvageRecovery.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            ' Populate CLMSalvageRecovery Attributes


















            m_lReturn = CType(oCLMSalvageRecovery.SetProperties(iStatus:=gPMConstants.PMEComponentAction.PMAdd, vRecoveryId:=CInt(vRecoveryId), vRecoveryTypeID:=CInt(vRecoveryTypeID), vPerilId:=CInt(vPerilId), vCurrencyID:=CInt(vCurrencyID), vinitialReserve:=CDec(vinitialReserve), vRevisedReserve:=CDec(vRevisedReserve), vReceivedToDate:=CDec(vReceivedToDate), vRevisionCount:=CInt(vRevisionCount), vReceiptId:=CInt(vReceiptId), vPartyClaimID:=CInt(vPartyClaimID), vReceiptAmount:=CDec(vReceiptAmount), vDateofReceipt:=vDateofReceipt, vPaymentId:=CInt(vPaymentId), vClaimID:=CInt(vClaimID), vPaymentAmount:=CDec(vPaymentAmount), vDateofPayment:=vDateofPayment, vComments:=CStr(vComments), vTable:=CInt(vTable), vTaxAmount:=CDec(vTaxAmount), vReceiptToLossRate:=CDbl(vReceiptToLossRate)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            ' Add CLMSalvageRecovery to collection
            If m_oCLMSalvageRecoverys.Count = 0 Then
                m_oCLMSalvageRecoverys.Add(Nothing)
            End If
            m_lReturn = CType(m_oCLMSalvageRecoverys.Add(oNewCLMSalvageRecovery:=oCLMSalvageRecovery), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            oCLMSalvageRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditUpdate (Public)
    '
    ' Description: Validates that this action is valid on the CLMSalvageRecovery
    '              specified and updates the CLMSalvageRecovery with the new values.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vinitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing, Optional ByRef vTaxAmount As Object = Nothing, Optional ByRef vReceiptToLossRate As Object = Nothing) As gPMConstants.PMEReturnCode


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery
        Dim iStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCLMSalvageRecoverys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oCLMSalvageRecovery = m_oCLMSalvageRecoverys.Item(lRow)

            ' Check the Status of the CLMSalvageRecovery

            'If status is Add (i.e. It is not in the database),then leave status as Add
            'or If status is Delete, report an error
            'Otherwise set to Edit

            Select Case oCLMSalvageRecovery.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    iStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    result = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    iStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select


            ' Update CLMSalvageRecovery Attributes


















            m_lReturn = CType(oCLMSalvageRecovery.SetProperties(iStatus:=iStatus, vRecoveryId:=CInt(vRecoveryId), vRecoveryTypeID:=CInt(vRecoveryTypeID), vPerilId:=CInt(vPerilId), vCurrencyID:=CInt(vCurrencyID), vinitialReserve:=CDec(vinitialReserve), vRevisedReserve:=CDec(vRevisedReserve), vReceivedToDate:=CDec(vReceivedToDate), vRevisionCount:=CInt(vRevisionCount), vReceiptId:=CInt(vReceiptId), vPartyClaimID:=CInt(vPartyClaimID), vReceiptAmount:=CDec(vReceiptAmount), vDateofReceipt:=vDateofReceipt, vPaymentId:=CInt(vPaymentId), vClaimID:=CInt(vClaimID), vPaymentAmount:=CDec(vPaymentAmount), vDateofPayment:=vDateofPayment, vComments:=CStr(vComments), vTable:=CInt(vTable), vTaxAmount:=CDec(vTaxAmount), vReceiptToLossRate:=CDbl(vReceiptToLossRate)), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                oCLMSalvageRecovery = Nothing
                Return result
            End If

            ' Release reference to CLMSalvageRecovery
            oCLMSalvageRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified CLMSalvageRecovery can be deleted
    '              and mark accordingly.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oCLMSalvageRecovery As bCLMSalvageRecovery.CLMSalvageRecovery

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oCLMSalvageRecoverys.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oCLMSalvageRecovery = m_oCLMSalvageRecoverys.Item(lRow)

            ' Check the Status of the CLMSalvageRecovery

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oCLMSalvageRecovery.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oCLMSalvageRecovery.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oCLMSalvageRecovery.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to CLMSalvageRecovery
            oCLMSalvageRecovery = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Cancel (Public)
    '
    ' Description: Checks the Collection to see if Cancel is OK.
    '              i.e. Do we need any Adding, Deleting or Updating.
    '
    '              Returns PMTrue if all items are clean
    '                      (PMView or PMDummyDelete)
    '              Otherwise returns PMDataChanged.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oCLMSalvageRecoverys.Count()
                Select Case m_oCLMSalvageRecoverys.Item(lSub).DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next lSub

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Cancel Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Cancel", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Loops round the collection, doing any required
    '              Adds, Deletes or Updates.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oCLMSalvageRecovery As New bCLMSalvageRecovery.CLMSalvageRecovery
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection



            For lSub = 1 To m_oCLMSalvageRecoverys.Count()
                oCLMSalvageRecovery = m_oCLMSalvageRecoverys.Item(lSub)


                Select Case oCLMSalvageRecovery.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing

                    Case gPMConstants.PMEComponentAction.PMAdd

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Add Item
                        m_lReturn = CType(oCLMSalvageRecovery.AddItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMEdit

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Update Item
                        m_lReturn = CType(oCLMSalvageRecovery.UpdateItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                    Case gPMConstants.PMEComponentAction.PMDelete

                        ' If we haven't already started a transaction start one.
                        If Not bTransStarted Then
                            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Return gPMConstants.PMEReturnCode.PMFalse
                            End If
                            bTransStarted = True
                        End If

                        ' Delete Item
                        m_lReturn = CType(oCLMSalvageRecovery.DeleteItem(), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

                m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            Next lSub

            'Retain the Primary Key of the CLMSalvageRecovery
            With oCLMSalvageRecovery
                RecoveryId = .RecoveryId
            End With

            ' Release last reference
            oCLMSalvageRecovery = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    '            m_lReturn& = CommitTrans()
                    '            If (m_lReturn& <> PMTrue) Then
                    '                Update = PMFalse
                    '                Exit Function
                    '            End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oCLMSalvageRecoverys.Count()

                        ' With the item
                        With m_oCLMSalvageRecoverys.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oCLMSalvageRecoverys.Delete(lSub)

                                    ' Anything Else
                                Case Else
                                    ' Set Status to view
                                    .DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                                    lSub += 1

                            End Select

                        End With

                    Loop

                Else

                    m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="BeginTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="BeginTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CommitTrans (Private)
    '
    ' Description: Commits a Transaction (Saves changes to DB).
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CommitTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CommitTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Private)
    '
    ' Description: Check Mandatory parameters have been passed.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '

    'Private Function CheckMandatory(Optional ByRef vRecoveryId As Object = Nothing, Optional ByRef vRecoveryTypeID As Object = Nothing, Optional ByRef vPerilId As Object = Nothing, Optional ByRef vCurrencyID As Object = Nothing, Optional ByRef vinitialReserve As Object = Nothing, Optional ByRef vRevisedReserve As Object = Nothing, Optional ByRef vReceivedToDate As Object = Nothing, Optional ByRef vRevisionCount As Object = Nothing, Optional ByRef vReceiptId As Object = Nothing, Optional ByRef vPartyClaimID As Object = Nothing, Optional ByRef vReceiptAmount As Object = Nothing, Optional ByRef vDateofReceipt As Object = Nothing, Optional ByRef vPaymentId As Object = Nothing, Optional ByRef vClaimID As Object = Nothing, Optional ByRef vPaymentAmount As Object = Nothing, Optional ByRef vDateofPayment As Object = Nothing, Optional ByRef vComments As Object = Nothing, Optional ByRef vTable As Object = Nothing) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    '    ' {* USER DEFINED CODE (Begin) *}
    ''
    '    If (IsMissing(vReserveID) = True) _
    ''    Or (IsEmpty(vReserveID) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If
    ''
    '    If (IsMissing(vSourceID) = True) _
    ''    Or (IsEmpty(vSourceID) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If
    '


    'If (Informations.IsNothing(vPerilId)) Or (Object.Equals(vPerilId, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '


    'If (Informations.IsNothing(vRecoveryTypeID)) Or (Object.Equals(vRecoveryTypeID, Nothing)) Then
    'Return gPMConstants.PMEReturnCode.PMMandatoryMissing
    'End If
    '
    '    If (IsMissing(vNumber) = True) _
    ''    Or (IsEmpty(vNumber) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If
    ''
    '    If (IsMissing(vCreatedByID) = True) _
    ''    Or (IsEmpty(vCreatedByID) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If
    ''
    '    If (IsMissing(vDateCreated) = True) _
    ''    Or (IsEmpty(vDateCreated) = True) Then
    '        CheckMandatory = PMMandatoryMissing
    '        Exit Function
    '    End If
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
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMandatory Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMandatory", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a OpenClaim.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function GetLookupValues(ByVal iLookupType As Integer, ByRef vTableArray(,) As Object, ByVal iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTabArray(3, 1) As Object
        Dim dtEffectiveDate As Date
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'vResultArray = ""
            ' Reset Table Array

            vTableArray = Nothing


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Currency"

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 1) = "Coinsurance_Treatment"


            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = ""

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 1) = ""


            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Get the Lookup items
            m_lReturn = m_oLookup.GetLookupValues(iLookupType:=iLookupType, vTableArray:=vTabArray, iLanguageID:=iLanguageID, dtEffectiveDate:=dtEffectiveDate, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the Table Array

            vTableArray = vTabArray

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupValues Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupValues", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         GetCoinsuranceRecoveries (Public)
    ' Description:  Gets the CoinsuranceRecoveries for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/08/2000
    ' Author:       Pandu
    ' ***************************************************************** '
    Public Function GetCoinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetCoInsurerDetailsSQL, sSQLName:=ACGetCoInsurerDetailsName, bStoredProcedure:=ACGetCoInsurerDetailsStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCoinsuranceRecoveries Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCoinsuranceRecoveries", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetReinsuranceRecoveries (Public)
    ' Description:  Gets the ReinsuranceRecoveries for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/08/2000
    ' Author:       Pandu
    ' ***************************************************************** '
    Public Function GetReinsuranceRecoveries(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetReInsurerDetailsSQL, sSQLName:=ACGetReInsurerDetailsName, bStoredProcedure:=ACGetReInsurerDetailsStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsuranceRecoveries")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetReinsuranceRecoveries Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReinsuranceRecoveries", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         GetPerilDetails (Public)
    ' Description:  Gets the GetPerilDetails for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/08/2000
    ' Author:       Pandu
    ' ***************************************************************** '
    Public Function GetPerilDetails(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPerilDetailsSQL, sSQLName:=ACGetPerilDetailsName, bStoredProcedure:=ACGetPerilDetailsStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPerilDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPerilDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Name:         GetDefaultCurrencyID (Public)
    ' Description:  Gets the the Default CurrencyID for the given Claim Id
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/08/2000
    ' Author:       Pandu
    ' ***************************************************************** '
    Public Function GetDefaultCurrencyID(ByRef r_vResultArray(,) As Object, ByVal v_lClaim_Id As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCurrencyID")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDefaultCurrencyIDSQL, sSQLName:=ACGetDefaultCurrencyIDName, bStoredProcedure:=ACGetDefaultCurrencyIDStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCurrencyID")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDefaultCurrencyID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDefaultCurrencyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name:         GetSalvageRecoveryType (Public)
    ' Description:  Gets the SalvageRecoveryTypes
    ' Returns    :  r_vResultArray: (2-dimensional Array) containing
    '
    ' Date:         20/08/2000
    ' Author:       Pandu
    ' ***************************************************************** '
    Public Function GetSalvageRecoveryType(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSalvageRecoveryType")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSalvageRecoveryTypesSQL, sSQLName:=ACGetCoInsurerDetailsName, bStoredProcedure:=ACGetCoInsurerDetailsStored, vResultArray:=r_vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSalvageRecoveryType")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If


            ' If NO record was found return Not Found
            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If


            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSalvageRecoveryType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSalvageRecoveryType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CheckRecoveryTypeID (Public)
    '
    ' Description: Checks to see if the supplied RecoveryTypeID is a valid record.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function CheckRecoveryTypeID(ByRef vRecoveryTypeID As Object, ByRef vPerilId As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="recovery_type_id", vValue:=CStr(vRecoveryTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)



            m_lReturn = m_oDatabase.Parameters.Add(sName:="Peril_id", vValue:=CStr(vPerilId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckRecoverytypeIDSQL, sSQLName:=ACCheckRecoverytypeIDName, bStoredProcedure:=ACCheckRecoverytypeIDStored, lNumberRecords:=0)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount < 1 Then
                ' No record found
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=" CheckRecoveryTypeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRecoveryTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_nOptionValue As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim m_oSystemOption As New bSIROptions.Business
            Dim sOptionValue As String = ""

            If m_oSystemOption Is Nothing Then
                m_oSystemOption = New bSIROptions.Business()

                m_lReturn = m_oSystemOption.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to initialise the system option object", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                    Return result
                End If
            End If

            m_lReturn = m_oSystemOption.GetOption(iOptionNumber:=v_iOptionNumber, sValue:=sOptionValue)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add the event", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            r_nOptionValue = CInt(sOptionValue)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name : GetPartyName
    '
    ' Desc : get name using party count
    '
    ' Hist : 20/03/2001 Created - Tinny
    '********************************************************************************
    Public Function GetPartyName(ByVal v_lPartyCnt As Integer, ByVal v_sFieldName As String, ByRef r_sResult As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT " & v_sFieldName & " FROM Party WHERE party_cnt = {party_cnt}"


            result = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(v_lPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetPartyName", bStoredProcedure:=False, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'pass back name

            r_sResult = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name : GetClassOfBusinss
    '
    ' Desc : get class of business code for peril type
    '
    ' Hist : 30/03/2001 Created - Tinny
    '        04/07/2001 RWH - Updated to return Id as well.
    '
    '********************************************************************************
    Public Function GetClassOfBusinss(ByVal v_lPerilTypeID As Integer, ByRef r_lId As Integer, ByRef r_sCode As String) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT per.class_of_business_id, cob.code" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM peril_type per, class_of_business cob" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE per.peril_type_id = {peril_type_id}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "AND per.class_of_business_id = cob.class_of_business_id"

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="peril_type_id", vValue:=CStr(v_lPerilTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetClassOfBusiness", bStoredProcedure:=False, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            'pass back results.

            r_lId = CInt(vResultArray(0, 0))

            r_sCode = CStr(vResultArray(1, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClassOfBusinss Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClassOfBusinss", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' Created By:  'RWH(09/11/2000) For Claims Numbering.
    '
    ' SJP14062002 - getUnderWritingOrAgency uses new product options scheme
    ' ***************************************************************** '
    Public Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Try


            Return bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingOrAgency)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnderwritingOrAgencyFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnderwritingOrAgency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: DeleteWorkClaim
    '
    ' Description:
    '
    ' History: 08/05/2001 Tomo - Created.
    '
    ' ***************************************************************** '
    Public Function DeleteWorkClaim() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="work_claim_id", vValue:=CStr(m_lClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=CStr(lStatus), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLBeginTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteWorkClaimSQL, sSQLName:=ACDeleteWorkClaimName, bStoredProcedure:=ACDeleteWorkClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            lStatus = m_oDatabase.Parameters.Item("status").Value

            If lStatus <> 0 Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLCommitTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DeleteWorkClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteWorkClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '************************************************************
    ' Name : GetInfoOnlyFlag
    '
    ' Desc : get info only flag from work table
    '
    ' Hist : 04 June 2001 Tinny - Created
    '************************************************************
    Public Function GetInfoOnlyFlag(ByVal v_lClaimID As Integer, ByRef r_bStatus As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_bStatus = False

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetInfoOnlyFlagSQL, sSQLName:=ACGetInfoOnlyFlagName, bStoredProcedure:=ACGetInfoOnlyFlagStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Dim auxVar As Object = vResultArray(0, 0)


            If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                r_bStatus = CBool(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyFlag Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyFlag", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function



    '***********************************************************************
    ' Name : GetOriginalClaimID
    '
    ' Desc : get the original claim ID from work table
    '
    ' Hist : 15 June 2001 Tinny - Created
    '***********************************************************************
    Public Function GetOriginalClaimID(ByVal v_lClaimID As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalClaimIDSQL, sSQLName:=ACGetOriginalClaimIDName, bStoredProcedure:=ACGetOriginalClaimIDStored, vResultArray:=vResultArray)


            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lOriginalClaimID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInfoOnlyStatus
    '
    ' Description:  Find out if Claim was previously Info Only
    '
    ' Created By:  Jude Killip 25/05/2001
    '
    ' ***************************************************************** '
    Public Function GetInfoOnlyStatus(ByVal v_lClaim_Id As Integer, ByRef r_bInfoStatus As Boolean) As Integer

        Dim result As Integer = 0
        Dim l_vResultArray(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            'Add the Claim Id parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(v_lClaim_Id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oParameters.Add failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInfoOnlyStatusSQL, sSQLName:=ACGetInfoOnlyStatusName, bStoredProcedure:=ACGetInfoOnlyStatusStored, vResultArray:=l_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'either PMTrue or PMNotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                r_bInfoStatus = False
            Else

                r_bInfoStatus = CBool(l_vResultArray(0, 0))
            End If

            Return m_lReturn

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClientAgentID
    '
    ' Description:  get client and agent details for this claim
    '
    ' Created By:  TN 23/08/2001
    '
    ' ***************************************************************** '
    Public Function GetClientAgentID(ByVal v_lWorkClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT  Client_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "Insurer_name," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "(" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT party_cnt FROM party WHERE shortname = Client_short_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ")," & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "CASE WHEN insurer_short_name <> '' THEN (" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SELECT party_cnt FROM party WHERE shortname = Insurer_short_name" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ")" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "ELSE NULL" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "END," & Strings.ChrW(13) & Strings.ChrW(10)
            ' Alix - 13/02/2004 - Also return product ID
            sSQL = sSQL & "(SELECT product_id FROM insurance_file WHERE insurance_file_cnt = policy_id)" & Strings.ChrW(13) & Strings.ChrW(10)
            ' /Alix
            sSQL = sSQL & "From work_claim" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE claim_id = {claim_id}"

            m_oDatabase.Parameters.Clear()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=CStr(v_lWorkClaimId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If



            Return m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetClientAgentDetails", bStoredProcedure:=False, vResultArray:=r_vResultArray)

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientAgentID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAgentID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result




            Return result
        End Try
    End Function

    'RWH(05/10/01) We need to be able to close a claim from salvage/recovery.
    Public Function CloseClaim(ByVal v_lClaimID As Integer) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLAction(ACCloseClaimSQL, ACCloseClaimName, ACCloseClaimStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CloseClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CloseClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    '                            Added a functionality. On pressing OK, in Salvage screen,
    '                             when the Sum(CurrentReserve) = 0 then a message will come up
    '                             asking the User wether the Claim can be closed.
    '                             If the reply is YES the Claim status is set to closed.

    Public Function GetCurrentReserveRecovery(ByVal v_lClaimID As Integer, ByRef r_vDataArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimID", vValue:=CStr(v_lClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the stored procedure.
            m_lReturn = m_oDatabase.SQLSelect(ACGetCurrentReserveRecoverySQL, ACGetCurrentReserveRecoveryName, ACGetCurrentReserveRecoveryStored, , r_vDataArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentReserveRecovery Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentReserveRecovery", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingType
    '
    ' Description:  Finds out Underwriting type - U or A
    '               For labelling: A - Insurer. U - Reinsurer
    '
    ' JMK 14/11/2001    Created
    ' SJP14062002 - getUnderwritingType uses new product options scheme
    ' ***************************************************************** '
    Private Function getUnderwritingType() As Integer

        Dim result As Integer = 0



        Return bPMFunc.getUnderwritingType(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, m_sUnderwritingType)

    End Function

    '********************************************************************************
    ' Name:         GetTaxTypesTaxBands
    ' Author:       Alix Bergeret
    ' Date:         15/05/2003
    ' Description:  Returns list of tax types and associated tax bands
    '********************************************************************************
    Public Function GetTaxTypesTaxBands(ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Get records
            result = m_oDatabase.SQLSelect(sSQL:=ACGetTaxTypesBandsSQL, sSQLName:=ACGetTaxTypesBandsName, bStoredProcedure:=ACGetTaxTypesBandsStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Following stored procedure failed to run:" & ACGetTaxTypesBandsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxTypesTaxBands", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaxTypesTaxBands Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaxTypesTaxBands", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name:         TidyUpAfterCancel
    '
    ' Parameters:   v_lWorkClaimId      - Work claim id
    '
    ' Description:  When cancelling from a claim roadmap, the work table
    '               data needs to be deleted (for underwriting) and with
    '               claimsbuilder, additional GIS-related data will also
    '               need to be deleted...
    '
    ' History:
    '               Created : RVH   23/12/2004
    ' ***************************************************************** '
    Public Function TidyUpAfterCancel(ByVal v_lWorkClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "TidyUpAfterCancel"

        Try

            Dim oBusiness As bCLMRiskDetails.Business

            result = gPMConstants.PMEReturnCode.PMTrue


            oBusiness = New bCLMRiskDetails.Business
            m_lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to create the business component
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to get an instance of bCLMRiskDetails.Business business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

                Return result
            End If


            m_lReturn = oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to set process modes
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed when calling SetProcessModes on bCLMRiskDetails.Business business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

                Return result
            End If


            m_lReturn = oBusiness.TidyUpAfterCancel(v_lClaimID:=v_lWorkClaimId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to call tidy up routine
                result = gPMConstants.PMEReturnCode.PMFalse
                Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
                oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
                gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed when calling TidyUpAfterCancel on bCLMRiskDetails.Business business object", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description), oDicParms:=oDict)

                Return result
            End If

            oBusiness = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            '******************************
            ' Log Error.
            Dim oDict As New System.Collections.Generic.Dictionary(Of String, Object)
            oDict.Add("v_lWorkClaimId", v_lWorkClaimId)
            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=excep, oDicParms:=oDict)
            '******************************

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: CreateEvent
    ' Desc: Adds an entry into the Event Log
    '
    ' Hist: 10/01/2006 - A.Robinson : Function Created.

    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lEventTypeId As Integer, ByVal v_sDescription As String, ByVal v_ClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURE_NAME As String = "CreateEvent"
        Dim oEvent As New bSIREvent.Business
        Try


            Dim sSQL As String = ""
            Dim lInsuranceFileCnt, lInsuranceFolderCnt, lPartyCnt As Integer
            Dim vResults(,) As Object = Nothing

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT C.policy_id FROM claim C WHERE C.claim_id = " & v_ClaimId

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Policy Id for Claim", bStoredProcedure:=False, vResultArray:=vResults)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to get Policy Id for Claim", gPMConstants.PMELogLevel.PMLogError)
            End If

            lInsuranceFileCnt = gPMFunctions.ToSafeLong(vResults(0, 0))

            m_lReturn = CType(GetClientPolicyDetails(v_lInsuranceFileCnt:=lInsuranceFileCnt, r_lPartyCnt:=lPartyCnt, r_lInsuranceFolderCnt:=lInsuranceFolderCnt), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to get Client details for Policy", gPMConstants.PMELogLevel.PMLogError)
            End If


            oEvent = New bSIREvent.Business
            m_lReturn = oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to create business object bSIREvent.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oEvent.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=v_ClaimId, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=v_sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to add event to Event Log", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_PROCEDURE_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURE_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If Not (oEvent Is Nothing) Then

                oEvent.Dispose()
                oEvent = Nothing
            End If


        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetClientPolicyDetails
    '
    ' Description:
    '
    ' History: 02/10/2002 sj - Created.
    ' RAM20021022 : Added Party Short Name Parameter
    '               Note : Updated spu_get_client_policy_details stored Procedure too
    '               (NRMA Changes - Sirius Process No 126)
    ' ***************************************************************** '
    Public Function GetClientPolicyDetails(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_lPartyCnt As Integer = 0, Optional ByRef r_sPartyShortName As String = "", Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_sInsuranceRef As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object = Nothing

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add Failed for insurance_file_cnt " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClientPolicyDetailsSQL, sSQLName:=ACGetClientPolicyDetailsName, bStoredProcedure:=ACGetClientPolicyDetailsStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect Failed calling " & ACGetClientPolicyDetailsSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails")
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lPartyCnt = CInt(vResultArray(0, 0))

            r_sPartyShortName = CStr(vResultArray(1, 0)) ' RAM20021022 : Added Party Short Name too

            r_lInsuranceFolderCnt = CInt(vResultArray(2, 0))

            r_sInsuranceRef = CStr(vResultArray(3, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClientPolicyDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientPolicyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class

