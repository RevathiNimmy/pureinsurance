Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("CLMSalvageRecovery_NET.CLMSalvageRecovery")>
Public NotInheritable Class CLMSalvageRecovery
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: SIRContact
    '
    ' Date: 24/08/2000
    '
    ' Description: Describes the SIRContact attributes.
    '
    ' Edit History:
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
    Private Const ACClass As String = "CLMSalvageRecovery"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Database Class
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag
    Private m_bCloseDatabase As Boolean

    ' DataBase Attributes-Recovery Table
    Private m_lRecoveryID As Integer '- Receipt Table also
    Private m_lRecoveryTypeID As Integer
    Private m_lPerilID As Integer '- Receipt,Payment Table also
    Private m_lCurrencyID As Integer '-Payment Table also
    Private m_cinitialReserve As Decimal
    Private m_cRevisedReserve As Decimal
    Private m_cReceivedToDate As Decimal
    Private m_lRevisionCount As Integer
    ' DataBase Attributes-Receipt Table
    Private m_lReceiptID As Integer
    Private m_lPartyClaimID As Integer '-Payment Table also
    Private m_cReceiptAmount As Decimal
    Private m_dtDateofReceipt As Object

    ' DataBase Attributes-Payment Table
    Private m_lPaymentID As Integer
    Private m_lClaimID As Integer
    Private m_cPaymentAmount As Decimal
    Private m_dtDateofPayment As Object
    Private m_sComments As String = ""

    'Database Attribute For Identifying the Table
    Private m_lTable As Integer

    ' Function Return Code
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_cTaxAmount As Decimal
    Private m_dReceiptToLossRate As Decimal

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public Property TaxAmount() As Decimal
        Get
            Return m_cTaxAmount
        End Get
        Set(ByVal Value As Decimal)
            m_cTaxAmount = Value
        End Set
    End Property

    Public Property ReceiptToLossRate() As Double
        Get
            Return m_dReceiptToLossRate
        End Get
        Set(ByVal Value As Double)
            m_dReceiptToLossRate = Value
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property Database() As dPMDAO.Database
        Set(ByVal Value As dPMDAO.Database)

            m_oDatabase = Value

        End Set
    End Property


    Public Property RecoveryID() As Integer
        Get

            'Return Recovery Id
            Return m_lRecoveryID

        End Get
        Set(ByVal Value As Integer)

            'Set RecoveryId
            m_lRecoveryID = Value

        End Set
    End Property

    Public Property PerilID() As Integer
        Get

            'Return PerilId
            Return m_lPerilID

        End Get
        Set(ByVal Value As Integer)

            'Set PerilID
            m_lPerilID = Value

        End Set
    End Property


    Public Property initialReserve() As Decimal
        Get

            'Return initial Reserve
            Return m_cinitialReserve

        End Get
        Set(ByVal Value As Decimal)

            'Set initial Reserve
            m_cinitialReserve = Value

        End Set
    End Property

    Public Property RevisedReserve() As Decimal
        Get

            'Return Revised Reserve
            Return m_cRevisedReserve

        End Get
        Set(ByVal Value As Decimal)

            'Set Revised Reserve
            m_cRevisedReserve = Value

        End Set
    End Property
    Public Property ReceivedToDate() As Decimal
        Get

            'Return ReceivedToDate
            Return m_cReceivedToDate

        End Get
        Set(ByVal Value As Decimal)

            'Set ReceivedToDate
            m_cReceivedToDate = Value

        End Set
    End Property
    Public Property CurrencyID() As Integer
        Get

            'Return CurrencyID
            Return m_lCurrencyID

        End Get
        Set(ByVal Value As Integer)

            'Set CurrencyID
            m_lCurrencyID = Value

        End Set
    End Property

    Public Property RecoveryTypeID() As Integer
        Get
            'Return RecoveryTypeID
            Return m_lRecoveryTypeID

        End Get
        Set(ByVal Value As Integer)

            'Set RecoveryTypeID
            m_lRecoveryTypeID = Value

        End Set
    End Property
    Public Property RevisionCount() As Integer
        Get
            'Return RevisionCount
            Return m_lRevisionCount

        End Get
        Set(ByVal Value As Integer)

            'Set RevisionCount
            m_lRevisionCount = Value

        End Set
    End Property
    'Properties for Receipt Table
    Public Property ReceiptID() As Integer
        Get

            'Return ReceiptID
            Return m_lReceiptID

        End Get
        Set(ByVal Value As Integer)

            'Set ReceiptID
            m_lReceiptID = Value

        End Set
    End Property
    Public Property PartyClaimID() As Integer
        Get

            'Return PartyClaimID
            Return m_lPartyClaimID

        End Get
        Set(ByVal Value As Integer)

            'Set PartyClaimID
            m_lPartyClaimID = Value

        End Set
    End Property
    Public Property ReceiptAmount() As Decimal
        Get

            'Set ReceiptAmount
            Return m_cReceiptAmount

        End Get
        Set(ByVal Value As Decimal)

            'Return ReceiptAmount
            m_cReceiptAmount = Value

        End Set
    End Property
    Public Property DateofReceipt() As Object
        Get

            'Return DateofReceipt
            Return m_dtDateofReceipt

        End Get
        Set(ByVal Value As Object)

            'Set DateofReceipt


            m_dtDateofReceipt = Value

        End Set
    End Property

    '-Properties For Payment Table
    Public Property PaymentID() As Integer
        Get

            'Return PaymentID
            Return m_lPaymentID

        End Get
        Set(ByVal Value As Integer)

            'Set PaymentID
            m_lPaymentID = Value

        End Set
    End Property
    Public Property ClaimID() As Integer
        Get

            'Return ClaimId
            Return m_lClaimID

        End Get
        Set(ByVal Value As Integer)

            'Set ClaimId
            m_lClaimID = Value

        End Set
    End Property
    Public Property PaymentAmount() As Decimal
        Get

            'Return PaymentAmount
            Return m_cPaymentAmount

        End Get
        Set(ByVal Value As Decimal)

            'Set PaymentAmount
            m_cPaymentAmount = Value

        End Set
    End Property
    Public Property DateofPayment() As Object
        Get

            'Return DateofPayment
            Return m_dtDateofPayment

        End Get
        Set(ByVal Value As Object)

            'Set DateofPayment


            m_dtDateofPayment = Value

        End Set
    End Property
    Public Property Comments() As String
        Get

            'Return Comments
            Return m_sComments
        End Get
        Set(ByVal Value As String)

            'Set Comments
            m_sComments = Value

        End Set
    End Property
    '-Property for Accessing Seperate Database Table
    Public Property Table() As Integer
        Get

            'Return Table Constant
            Return m_lTable

        End Get
        Set(ByVal Value As Integer)
            'Set the Table Constant
            m_lTable = Value

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




            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: Add (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Add() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as OUTPUT parameters
            m_lReturn = CType(AddKeyOutputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Select Case Table
                Case ACRecovery

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the Primary Key of the record inserted
                    m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                Case ACReceipt

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQLReceipt, sSQLName:=ACAddNameReceipt, bStoredProcedure:=ACAddStoredReceipt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the Primary Key of the record inserted
                    m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case ACPayment


                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQLPayment, sSQLName:=ACAddNamePayment, bStoredProcedure:=ACAddStoredPayment)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Get the Primary Key of the record inserted
                    m_lReturn = CType(GetNewPrimaryKeyID(), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

            End Select


            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Add Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Add", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Update (Public)
    '
    ' Description: Updates a single record in the database from
    '              the Base Details.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            m_lRevisionCount += 1

            ' Add the required INPUT parameters
            m_lReturn = CType(AddInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Select Case Table
                Case ACRecovery

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQL, sSQLName:=ACUpdateName, bStoredProcedure:=ACUpdateStored, lRecordsAffected:=lRecordsAffected)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Check to see that the record was updated OK
                    If lRecordsAffected > 0 Then
                        ' Updated No action required
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case ACReceipt

                    ' Execute SQL Statement
                    m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateSQLReceipt, sSQLName:=ACUpdateNameReceipt, bStoredProcedure:=ACUpdateStoredReceipt, lRecordsAffected:=lRecordsAffected)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Check to see that the record was updated OK
                    If lRecordsAffected > 0 Then
                        ' Updated No action required
                    Else
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                Case ACPayment

            End Select

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Update Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Update", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: Delete (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    ' ***************************************************************** '
    Public Function Delete() As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteSQL, sSQLName:=ACDeleteName, bStoredProcedure:=ACDeleteStored, lRecordsAffected:=lRecordsAffected)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If record wasn't deleted, error
            If lRecordsAffected > 0 Then
                ' Deleted, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Delete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Delete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SelectSingle (Public)
    '
    ' Description: Selects the required SIRContact
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Public Function SelectSingle(Optional ByRef vLockMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    With m_oDatabase

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = 0
            End If

            ' Add PrimaryKey as INPUT parameters
            m_lReturn = CType(AddKeyInputParam(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSQL, sSQLName:=ACSelectSingleName, bStoredProcedure:=ACSelectSingleStored, bKeepNulls:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?
            If lRecordCount = 1 Then
                ' Selected, No action required
            Else
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set properties
            m_lReturn = CType(SetPropertiesFromDB(oFields:=m_oDatabase.Records.Item(1).Fields()), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            '    End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingle Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingle", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SetPropertiesFromDB (Public)
    '
    ' Description: Sets the supplied SIRContact properties from a database
    '              record.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    'Public Function SetPropertiesFromDB(ByRef oFields As ADODB.Fields) As Integer
    Public Function SetPropertiesFromDB(ByRef oFields As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Populate Base Details

            With oFields

                RecoveryID = gPMFunctions.NullToLong(oFields("Recovery_id"))
                PerilID = gPMFunctions.NullToLong(oFields("Claim_Peril_id"))

                RecoveryTypeID = gPMFunctions.NullToLong(oFields("Recovery_Type_id"))


                If Convert.IsDBNull(oFields("Initial_reserve")) Or Informations.IsNothing(oFields("Initial_reserve")) Then

                    initialReserve = Nothing
                Else
                    initialReserve = oFields("Initial_Reserve")
                End If


                If Convert.IsDBNull(oFields("Revised_reserve")) Or Informations.IsNothing(oFields("Revised_reserve")) Then
                    RevisedReserve = 0
                Else
                    RevisedReserve = oFields("Revised_reserve")
                End If



                If Convert.IsDBNull(oFields("Currency_id")) Or Informations.IsNothing(oFields("Currency_id")) Then
                    CurrencyID = 0
                Else
                    CurrencyID = oFields("Currency_id")
                End If


                If Convert.IsDBNull(oFields("Received_To_Date")) Or Informations.IsNothing(oFields("Received_To_Date")) Then
                    ReceivedToDate = 0
                Else
                    ReceivedToDate = oFields("Received_To_Date")
                End If


                If Convert.IsDBNull(oFields("Revision_count")) Or Informations.IsNothing(oFields("Revision_count")) Then
                    RevisionCount = 0
                Else
                    RevisionCount = oFields("Revision_count")
                End If


                If Convert.IsDBNull(oFields("tax_amount")) Or Informations.IsNothing(oFields("tax_amount")) Then
                    TaxAmount = 0
                Else
                    TaxAmount = oFields("tax_amount")
                End If

            End With

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetPropertiesFromDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPropertiesFromDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' PUBLIC Methods (End)

    ' PRIVATE Methods (Begin)
    ' ***************************************************************** '
    ' Name: AddInputParam (Private)
    '
    ' Description: Adds all of the NON-KEY INPUT parameters
    '              required for an Insert or Update.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function AddInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '   With m_oDatabase


        Select Case Table
            Case ACRecovery

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Peril_id", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Recovery_Type_id", vValue:=CStr(RecoveryTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Received_To_Date", vValue:=CStr(ReceivedToDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                m_lReturn = m_oDatabase.Parameters.Add(sName:="initial_reserve", vValue:=CStr(initialReserve), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Revised_reserve", vValue:=CStr(RevisedReserve), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Revision_Count", vValue:=CStr(RevisionCount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Alix - 16/05/2003 - Tax on claim
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Tax_Amount", vValue:=CStr(TaxAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


            Case ACReceipt

                '1
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Peril_id", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '2
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Recovery_id", vValue:=CStr(RecoveryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '3
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Recovery_Type_id", vValue:=CStr(RecoveryTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '4
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '5
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '6
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Claim_id", vValue:=CStr(PartyClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '7
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Amount", vValue:=CStr(ReceiptAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '8
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_of_Receipt", vValue:=Informations.FormatDateTime(DateofReceipt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '9
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comments", vValue:=Comments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Tax_Amount", vValue:=CStr(TaxAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="receipt_loss_xrate", vValue:=CStr(ReceiptToLossRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Case ACPayment
                '1
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Peril_id", vValue:=CStr(PerilID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '2
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Recovery_id", vValue:=CStr(RecoveryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '3
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Recovery_Type_id", vValue:=CStr(RecoveryTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                '4
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Claim_id", vValue:=CStr(ClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '5
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Currency_id", vValue:=CStr(CurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '6
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Party_Claim_id", vValue:=CStr(PartyClaimID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '7
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Amount", vValue:=CStr(PaymentAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '8
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Date_of_Payment", vValue:=Informations.FormatDateTime(DateofPayment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                '9
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Comments", vValue:=Comments, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
        End Select

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyInputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY INPUT parameters
    '              required for a Select, Delete or Update.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function AddKeyInputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '   With m_oDatabase


        Select Case Table
            Case ACRecovery

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Recovery_id", vValue:=CStr(RecoveryID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Case ACReceipt
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Receipt_id", vValue:=CStr(ReceiptID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Case ACPayment
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Payment_id", vValue:=CStr(PaymentID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End Select
        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddKeyOutputParam (Private)
    '
    ' Description: Adds all of the PRIMARY KEY OUTPUT parameters
    '              required for an Add.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function AddKeyOutputParam() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '    With m_oDatabase


        Select Case Table
            Case ACRecovery

                m_lReturn = m_oDatabase.Parameters.Add(sName:="recovery_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            Case ACReceipt

                m_lReturn = m_oDatabase.Parameters.Add(sName:="receipt_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Case ACPayment

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Payment_id", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        End Select



        '    End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetNewPrimaryKeyID (Private)
    '
    ' Description: Returns the new PRIMARY KEY values from an Add.
    '
    ' Date :24/08/2000
    '
    ' Edit History :Pandu
    ' ***************************************************************** '
    Private Function GetNewPrimaryKeyID() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        '   With m_oDatabase


        Select Case Table
            Case ACRecovery

                RecoveryID = m_oDatabase.Parameters.Item("recovery_id").Value

            Case ACReceipt

                ReceiptID = m_oDatabase.Parameters.Item("receipt_id").Value

            Case ACPayment

                PaymentID = m_oDatabase.Parameters.Item("Payment_id").Value

        End Select

        '    End With

        Return result

    End Function
    ' PRIVATE Methods (End)


    Public Sub New()
        MyBase.New()

        ' Class Initialise


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

