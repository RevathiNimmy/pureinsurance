Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports System.IO
Imports System.Runtime.ExceptionServices
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 24/08/00
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a CLMPeril.
    '
    ' Edit History:
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 11/12/2003
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

    ' Database Class (Private)

    Private m_oDatabase As dPMDAO.Database

    ' Data Layer object declaration
    Private m_odCLMPeril As dCLMPeril.Data

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

    Private lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lPerilID As Integer
    Private m_lClaimID As Integer
    Private m_lPerilTypeID As Integer
    Private m_lpartycnt As Integer

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    Private m_oPolicyNumMaint As bSIRPolicyNumMaint.Business

    'RWH(07/02/2001)
    Private m_sUnderwritingOrAgency As String = ""

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)

            Value = Value

        End Set
    End Property

    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property

    Public Property PerilID() As Integer
        Get
            Return m_lPerilID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilID = Value
        End Set
    End Property

    Public Property PerilTypeID() As Integer
        Get
            Return m_lPerilTypeID
        End Get
        Set(ByVal Value As Integer)
            m_lPerilTypeID = Value
        End Set
    End Property

    Public Property Partycnt() As Integer
        Get
            Return m_lpartycnt
        End Get
        Set(ByVal Value As Integer)
            m_lpartycnt = Value
        End Set
    End Property

    'RWH(07/02/2001)
    Public ReadOnly Property UnderwritingOrAgency() As String
        Get

            If m_sUnderwritingOrAgency = "" Then
                m_lReturn = getUnderwritingOrAgency()
            End If

            Return m_sUnderwritingOrAgency

        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
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
            'm_sTransactionType = PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create PM Lookup Business Object
            m_oLookup = New BPMLOOKUP.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Create instance of data class
            m_odCLMPeril = New dCLMPeril.Data()

            m_lReturn = m_odCLMPeril.Initialise(sUsername:=m_sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

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
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
    ' ***************************************************************** '
    Public Function BeginTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLBeginTrans

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
    ' ***************************************************************** '
    Public Function CommitTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLCommitTrans

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
    ' ***************************************************************** '
    Public Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction

            m_lReturn = m_oDatabase.SQLRollbackTrans

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
        ''

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ***************************************************************** '
    ' Name: GetControls (Public)
    '
    ' Description: Gets the list of user defined controls for the particular
    '              PerilTypeID
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    Public Function GetControls(ByRef r_vControlsArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_odCLMPeril.PerilID = PerilID
            m_odCLMPeril.ClaimID = ClaimID
            m_odCLMPeril.PerilTypeID = PerilTypeID
            m_odCLMPeril.Partycnt = Partycnt

            m_lReturn = m_odCLMPeril.GetControls(r_vControlsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetControls", vApp:=ACApp, vClass:=ACClass, vMethod:="GetControls", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveType (Public)
    '
    ' Description: Gets all the types of Reserve's
    '
    ' Author: Ranjit R
    '
    ' ***************************************************************** '
    Public Function GetReserveType(ByRef r_vReserveTypeArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_odCLMPeril.GetReserveType(r_vReserveTypeArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetReserveType", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetReserveDetails(Public)
    '
    ' Description: Gets the details regarding a Reserve by passing the reserve_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetReserveDetails(ByVal v_vPolicyID As Object, ByVal v_vRiskID As Object, ByRef r_vReserveDetailsArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim r_sSiriusproduct As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetSiriusProduct(r_sSiriusproduct), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            m_lReturn = m_odCLMPeril.GetReserveDetails(v_vPolicyID, v_vRiskID, r_sSiriusproduct, r_vReserveDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetReserveDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetReserveDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function GetPaymentList(ByVal lClaimID As Integer, ByVal lReserveID As Integer, ByRef r_vPaymentList As Object) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: GetPaymentList
        ' PURPOSE: Returns a payment list array based on the Claim and optionally
        '          the Reserve.
        ' AUTHOR: Danny Davis
        ' DATE: 01 November 2004, 17:03:38
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "reserve_id", If(lReserveID = 0, DBNull.Value, lReserveID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect("spu_get_all_payments_for_claim", "Get All Payments for Claim", True, , r_vPaymentList)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Throw New System.Exception(m_lReturn.ToString() + ", GetPaymentList, SQL Select failed")
            End If


            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Informations.Err().Number
                Case Else
                    ' Log Error.
                    result = Informations.Err().Number

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentList", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            End Select

        Finally
            'Return result

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetPaymentDetails (Public)
    '
    ' Description: Gets the details regarding a Paymnet by passing the payment_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetPaymentDetails(ByRef r_vPaymentDetailsArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 210503 - added another parameter for PS 237
            m_lReturn = m_odCLMPeril.GetPaymentDetails(r_vPaymentDetailsArray, m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPaymentDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPaymentDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetPartyDetails (Public)
    '
    ' Description: Gets the details regarding a Party_id
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    Public Function GetPartyDetails(ByRef r_vPartyDetailsArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_odCLMPeril.GetPartyDetails(r_vPartyDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetPartyDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddParty(Public)
    '
    ' Description: Adds a Party to the database
    '
    ' Author: Ranjit R
    ' ***************************************************************** '
    Public Function AddParty(ByVal v_vPartyIDArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vPartyIDArray) Or Object.Equals(v_vPartyIDArray, Nothing) Then
                Throw New Exception()
            End If

            m_lReturn = m_odCLMPeril.AddParty(v_vPartyIDArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddParty", vApp:=ACApp, vClass:=ACClass, vMethod:="Add Party", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DeleteParty(Public)
    '
    ' Description: Deletes a Party
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function DeleteParty(ByVal v_vPartyIDArray As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vPartyIDArray) Or Object.Equals(v_vPartyIDArray, Nothing) Then
                Throw New Exception()
            End If

            m_lReturn = m_odCLMPeril.DeleteParty(v_vPartyIDArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to DeleteParty", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteParty", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateReserveDetails (Public)
    '
    ' Description: Updates the details for a reserve
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function UpdateReserveDetails(ByVal v_vReserveDetailsArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vReserveDetailsArray) Or Object.Equals(v_vReserveDetailsArray, Nothing) Then
                Throw New Exception()
            End If

            m_lReturn = m_odCLMPeril.UpdateReserveDetails(v_vReserveDetailsArray, m_sTransactionType)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateReserveDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateReserveDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateGeneral(Public)
    '
    ' Description: Updates the details for a user defined field
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function UpdateGeneral(ByVal v_vGeneralDetailsArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(v_vGeneralDetailsArray) Or v_vGeneralDetailsArray Is Nothing Then
                Throw New Exception()
            End If

            m_lReturn = m_odCLMPeril.UpdateGeneral(v_vGeneralDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to UpdateGeneral", vApp:=ACApp, vClass:=ACClass, vMethod:="Update General", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimLookup (Public)
    '
    ' Description: Gets the list of values for a given lookup table
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetClaimLookup(ByVal v_vclaimlookupid As Object, ByRef r_vLookupArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_odCLMPeril.GetClaimLookup(v_vclaimlookupid, r_vLookupArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetClaimLookup", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimLookup", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetRecoveryDetails (Public)
    '
    ' Description: Gets the details regarding a Paymnet by passing the Recovery_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function GetRecoveryDetails(ByVal v_vRecoveryType As Object, ByRef r_vRecoveryDetailsArray(,) As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            If Informations.IsDBNull(r_vRecoveryDetailsArray) Then r_vRecoveryDetailsArray = Nothing
            m_lReturn = m_odCLMPeril.GetRecoveryDetails(v_vRecoveryType, r_vRecoveryDetailsArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to GetRecoveryDetails", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRecoveryDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddComments (Public)
    '
    ' Description: Gets the details regarding a Paymnet by passing the Receipt_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function AddComments(ByVal v_vComments As String) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_odCLMPeril.AddComments(v_vComments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddComments", vApp:=ACApp, vClass:=ACClass, vMethod:="AddComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: AddCommentsUW (Public)
    '
    ' Description: Gets the details regarding a Paymnet by passing the Receipt_id
    '
    ' Author: Ranjit R

    ' ***************************************************************** '
    Public Function AddCommentsUW(ByVal v_vComments As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = m_odCLMPeril.AddCommentsUW(v_vComments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to AddCommentsUW", vApp:=ACApp, vClass:=ACClass, vMethod:="AddCommentsUW", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetSiriusProduct (Private)
    '
    ' Description: Gets the Sirius product type
    '
    ' Author: Ranjit R

    ' ***************************************************************** '

    Private Function GetSiriusProduct(ByRef r_sSiriusproduct As String) As Integer
        Dim result As Integer = 0
        Dim oBackOffice As bBackOfficeLink.bBOLink

        result = gPMConstants.PMEReturnCode.PMTrue
        r_sSiriusproduct = ""
        oBackOffice = New bBackOfficeLink.bBOLink()
        '***********CODE MODIFIED FOR CLIENT SERVER MODEL Author: Ranjit
        m_lReturn = oBackOffice.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        '***************************************************************
        If Not (oBackOffice Is Nothing) Then
            r_sSiriusproduct = oBackOffice.Sirius_Product
            oBackOffice.Dispose()
            oBackOffice = Nothing
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetComments (Public)
    '
    ' Description: Gets comments for a particular Claim Peril ID
    '
    ' Author: Ranjit R

    ' ***************************************************************** '

    Public Function GetComments(ByRef r_vComments As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            '    r_vComments = Null

            m_lReturn = m_odCLMPeril.GetComments(r_vComments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetComments Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetComments", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateEvent
    ' Desc: Adds an entry into the Event Log
    '
    ' Hist: 10/01/2006 - A.Robinson : Function Created.

    ' ***************************************************************** '
    Public Function CreateEvent(ByVal v_lEventTypeId As Integer, ByVal v_sDescription As String, Optional ByRef v_lEventCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURE_NAME As String = "CreateEvent"

        Dim oEvent As bSIREvent.Business = Nothing
        Dim sSQL As String = ""
        Dim lInsuranceFileCnt, lInsuranceFolderCnt, lPartyCnt As Integer
        Dim vResults(,) As Object = Nothing

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "SELECT C.policy_id FROM claim C WHERE C.claim_id = " & m_lClaimID

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

            m_lReturn = oEvent.DirectAdd(vPartyCnt:=lPartyCnt, vInsuranceFolderCnt:=lInsuranceFolderCnt, vInsuranceFileCnt:=lInsuranceFileCnt, vClaimCnt:=m_lClaimID, vEventType:=v_lEventTypeId, vUserId:=m_iUserID, vEventDate:=DateTime.Today, vDescription:=v_sDescription, vPerilId:=m_lPerilID, vEventCnt:=v_lEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(AC_PROCEDURE_NAME, "Failed to add event to Event Log", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return result

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_PROCEDURE_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURE_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If Not (oEvent Is Nothing) Then

                oEvent.Dispose()
                oEvent = Nothing
            End If

            '  Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPerilDetails
    ' Desc:
    '
    ' Hist: 10/01/2006 - A.Robinson : Function Created.

    ' ***************************************************************** '

    Public Function GetClaimPerilDetails(ByRef r_vClaimPerilDetails As Object) As Integer

        Dim result As Integer = 0
        Const AC_PROCEDURE_NAME As String = "GetClaimPerilDetails"
        Try

            m_odCLMPeril.PerilID = PerilID
            m_odCLMPeril.ClaimID = ClaimID
            m_odCLMPeril.PerilTypeID = PerilTypeID
            m_odCLMPeril.Partycnt = Partycnt

            Return m_odCLMPeril.GetClaimPerilDetails(r_vClaimPerilDetails)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=AC_PROCEDURE_NAME & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=AC_PROCEDURE_NAME, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetCommentsUW (Public)
    '
    ' Description: Gets comments for a particular Claim Peril ID
    '
    ' Author: Ranjit R

    ' ***************************************************************** '

    Public Function GetCommentsUW(ByRef r_vComments As Object) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue
            '    r_vComments = Null

            m_lReturn = m_odCLMPeril.GetCommentsUW(r_vComments)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCommentsUW Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCommentsUW", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetOption (Private)
    '
    ' Description: Get an option.
    '
    ' ***************************************************************** '
    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim m_oSystemOption As bSIROptions.Business = Nothing
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

            r_sOptionValue = CInt(sOptionValue)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetUnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' RWH(07/02/2001) (U/W hidden option)
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = m_odCLMPeril.getUnderwritingOrAgency(m_sUnderwritingOrAgency)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New Exception()
        End If

        Return result

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
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_odCLMPeril.GetPartyName(v_lPartyCnt:=v_lPartyCnt, v_sFieldName:=v_sFieldName, r_sResult:=r_sResult)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPartyName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPartyName", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetClassOfBusiness
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetClassOfBusiness(ByRef r_lId As Integer, ByRef r_sCode As String, Optional ByVal v_lPerilTypeID As Integer = 0, Optional ByVal v_lClaimPerilId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClassOfBusiness"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim vResults(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="peril_type_id", v_vValue:=v_lPerilTypeID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClassOfBusinessSQL, sSQLName:=kGetClassOfBusinessName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClassOfBusinessSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResults) Then

                r_lId = CInt(vResults(0, 0))

                r_sCode = CStr(vResults(1, 0))
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    '********************************************************************************
    ' Name : GetSystemOption
    '
    ' Desc : get system option value
    '
    ' Hist : 10/05/2001 Created - Tinny
    '********************************************************************************
    Public Function GetSystemOption(ByVal v_lOptionNumber As Integer, ByRef r_sReturn As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_odCLMPeril.GetSystemOption(v_lOptionNumber:=v_lOptionNumber, r_sReturn:=r_sReturn)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSystemOption Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSystemOption", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name : GetClaimNumber
    '
    ' Desc : get claim number
    '
    ' Hist : 05 June 2001 Created - Tinny
    '********************************************************************************
    Public Function GetClaimNumber(ByVal v_lClaimId As Integer, ByRef r_sClaimRef As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimNumberSQL, sSQLName:=ACGetClaimNumberName, bStoredProcedure:=ACGetClaimNumberStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_sClaimRef = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'sj 03/10/2002 - start
    '********************************************************************************
    ' Name : GetClaimNumberFromClaim
    '
    ' Desc : get claim number
    '
    '********************************************************************************
    Public Function GetClaimNumberFromClaim(ByVal v_lClaimId As Integer, ByRef r_sClaimRef As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimNumberFromClaimSQL, sSQLName:=ACGetClaimNumberFromClaimName, bStoredProcedure:=ACGetClaimNumberFromClaimStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_sClaimRef = CStr(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimNumberFromClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimNumberFromClaim", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'sj 03/10/2002 - end

    ' ***************************************************************** '
    ' Name: GetClaimClientAndAgent
    '
    ' Description: retrieve Client and Agent id and Name to pass into PaymentMethod
    '
    ' Date : 21/08/2001
    '
    ' History : Created - Jude Killip
    ' ***************************************************************** '
    Public Function GetClaimClientAndAgent(ByVal v_lClaimId As Integer, ByRef r_vClaimClientAndAgent As Object) As Integer
        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetClaimClientAndAgentSQL, sSQLName:=ACGetClaimClientAndAgentName, bStoredProcedure:=ACGetClaimClientAndAgentStored, vResultArray:=vResultArray)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            r_vClaimClientAndAgent = vResultArray

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimClientAndAgent Error", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimClientAndAgent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    'PS344
    Public Function GetTaskGroupCode(ByVal v_sTaskCode As String, ByRef r_sTaskGroupCode As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Return m_odCLMPeril.GetTaskGroupCode(v_sTaskCode:=v_sTaskCode, r_sTaskGroupCode:=r_sTaskGroupCode)

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetTaskGroupCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetTaskGroupCode", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
    Public Function GetOriginalClaimID(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

    '*************************************************************************
    ' Name          : GetRiskDetails
    '
    ' Description   : get revelant info to past on to iPMURisk
    '
    ' Note          : This method is similar to the bCLMRiskDetails.dll's
    '                   GetRiskDetails_U Method
    ' RAM20021024   : Created
    '*************************************************************************
    Public Function GetRiskDetails(ByVal v_lClaimId As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACSelRiskDetailSQL, sSQLName:=ACSelRiskDetailName, bStoredProcedure:=True, vResultArray:=r_vResultArray)

            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'AK 130503 - created for checking payment status
    '********************************************************************************
    ' Name : CheckReferredPayment
    '
    ' Desc : Check Referred Payment
    '
    '********************************************************************************
    Public Function CheckReferredPayment(ByVal v_lClaimId As Integer, ByRef r_bStatus As Boolean, Optional ByRef r_iNoofReferredPayments As Integer = 0, Optional ByRef r_cSumofReferredPayments As Decimal = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            r_bStatus = False

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACGetReferredPaymentSQL, sSQLName:=ACGetReferredPaymentName, bStoredProcedure:=ACGetReferredPaymentStored, vResultArray:=vResultArray, bKeepNulls:=True)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            If CDbl(vResultArray(0, 0)) <> 0 Then
                r_bStatus = True

                r_iNoofReferredPayments = gPMFunctions.ToSafeInteger(CDec(vResultArray(0, 0)))

                r_cSumofReferredPayments = gPMFunctions.ToSafeCurrency(CDec(vResultArray(1, 0)))
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckReferredPayment Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckReferredPayment", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '********************************************************************************
    ' Name:         GetTaxTypesTaxBands
    ' Author:       Alix Bergeret
    ' Date:         14/05/2003
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

    'TF011003 - PN7035 - Copied from bCLMRiskDetails to identify GII policies
    Public Function GetPolicyType(ByVal v_lPolicyId As Integer, ByRef r_sType As String) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            'Clear the Database Parameters Collection
            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="insurance_file_id", vValue:=v_lPolicyId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter insurance_file_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'Execute SQL Statement

                m_lReturn = .SQLSelect(sSQL:=ACGetPolicyTypeSQL, sSQLName:=ACGetPolicyTypeName, bStoredProcedure:=ACGetPolicyTypeStored, vResultArray:=vArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process " & ACGetPolicyTypeSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End With

            If Informations.IsArray(vArray) Then

                r_sType = CStr(vArray(0, 0)).Trim()
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '*************************************************************************
    ' Name: GetClaimCurrency
    '
    ' Description: Get currency ID and Description
    '
    ' History: RDC 03062004 created
    '*************************************************************************
    Public Function GetClaimCurrency(ByVal v_lClaimId As Integer, ByRef r_lCurrencyID As Integer, ByRef r_sCurrencyDesc As String) As Integer

        Dim result As Integer = 0
        Dim vResult(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            With m_oDatabase

                .Parameters.Clear()

                m_lReturn = .Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to add parameter claim_id", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCurrency")

                    Return result
                End If

                'Execute SQL Statement

                m_lReturn = .SQLSelect(sSQL:=ACGetClaimCurrencySQL, sSQLName:=ACGetClaimCurrencyName, bStoredProcedure:=ACGetClaimCurrencyStored, vResultArray:=vResult)

            End With

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or Not Informations.IsArray(vResult) Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process " & ACGetClaimCurrencySQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyType")

                Return result
            End If

            r_lCurrencyID = CInt(vResult(0, 0))

            r_sCurrencyDesc = CStr(vResult(1, 0))

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetClaimCurrency failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimCurrency", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RetrieveCurrenciesForBranch
    '
    ' Description: get currencies used by branch
    '
    ' History:
    '     RDC 09062004 created
    ' ***************************************************************** '
    Public Function RetrieveCurrenciesForBranch(ByRef iSourceID As Integer, ByRef vReturnArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            m_lReturn = CType(bPMFunc.GetBranchCurrencies(v_iSourceID:=iSourceID, v_oDatabase:=m_oDatabase, r_vReturnArray:=vReturnArray), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Informations.IsArray(vReturnArray) Then
                Return result
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            'Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RetrieveCurrenciesForBranch failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RetrieveCurrenciesForBranch", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RetrieveCurrenciesForClaimBranch
    '
    ' Parameters: n/a
    '
    ' Description: Returns all available currencies for the base
    '                branch associated with the specified claim id
    '
    ' History:
    '           Created : MEvans : 21-03-2005 : PN19650
    ' ***************************************************************** '
    Public Function RetrieveCurrenciesForClaimBranch(ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RetrieveCurrenciesForClaimBranch"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kRetrieveCurrenciesForClaimBranchSQL, sSQLName:=kRetrieveCurrenciesForClaimBranchName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kRetrieveCurrenciesForClaimBranchSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: AddInputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddInputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AddOutputParameter
    '
    ' Parameters: v_sName   : Parameter Name
    '             v_vValue  : Parameter Value
    '             v_iType   : Parameter DataType
    '
    ' Description: Adds an input parameter to the database parameters
    '
    ' History:
    '           Created : MEvans : 18-12-2002 : 202
    ' ***************************************************************** '
    Private Function AddOutputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Integer) As Integer

        Dim result As Integer = 0
        Const sFunctionName As String = "AddOutputParameter"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object

        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse

            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                      ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName, excep:=New Exception(Informations.Err().Description))

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: GetSafeHarbourDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetSafeHarbourDetails(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSafeHarbourDetails"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetSafeHarbourDetailsSQL, sSQLName:=kGetSafeHarbourDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetSafeHarbourDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentToDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetClaimPaymentToDetails(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentToDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentToDetailsSQL, sSQLName:=kGetClaimPaymentToDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentToDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name:  GetLookupsByEffectiveDate
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetLookupsByEffectiveDate(ByVal v_sTableName As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetLookupsByEffectiveDate"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="table", v_vValue:=v_sTableName, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetLookupsByEffectiveDateSQL, sSQLName:=kGetLookupsByEffectiveDateName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetLookupsByEffectiveDateSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetClaimPaymentDetails(ByVal v_lClaimId As Integer, ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentDetails"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentDetailsSQL, sSQLName:=kGetClaimPaymentDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetCoInsurerDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : E Knott : 11-2005 : Datasure
    ' ***************************************************************** '
    Public Function GetCoInsurerDetails(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCoInsurerDetails"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "InsuranceFileCnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            'DC070606 change the way coinsurers are handled / displayed for claims for datasure
            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimId", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetCoInsurerDetailsSQL, sSQLName:=kGetCoInsurerDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCoInsurerDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: GenerateClaimNumber
    '
    ' Description: Uses the PolicyNumMaint component to generate a claim
    '               number.
    '
    ' History:  10/11/2000 RWH  - Created.
    '           15/10/2001 JMK  - add 2 optional claim date parameters
    ' ***************************************************************** '
    Public Function GenerateClaimNumber(ByVal v_lBusinessType As Integer, ByVal v_iBranchId As Integer, ByVal v_lProductId As Integer, ByVal v_lAgentId As Integer, ByRef r_sGeneratedClaimNumber As String, ByVal v_sLossYear As String, ByVal v_sReportedYear As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oPolicyNumMaint Is Nothing Then
                m_oPolicyNumMaint = New bSIRPolicyNumMaint.Business()
            End If

            m_lReturn = Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' JMK 15/10/2001
            m_lReturn = m_oPolicyNumMaint.GeneratePolicyNumber(v_lBusinessType, v_iBranchId, v_lProductId, v_lAgentId, r_sGeneratedClaimNumber, v_sLossYear, v_sReportedYear)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GenerateClaimNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GenerateClaimNumber", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '***********************************************************************
    ' Name : GetOriginalClaimID
    '
    ' Desc : get the original claim ID from  table
    '
    ' Hist : 15 June 2001 Tinny - Created
    '***********************************************************************
    Public Function GetOriginalClaimNo(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=v_lClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

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

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimNo Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimNo", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdateCoInsurerDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : E Knott : 11-2005 : Datasure
    ' ***************************************************************** '
    'DC080606 change the way coinsurer details are obtained
    Public Function UpdateCoInsurerDetails(ByVal v_lReserveId As Integer, ByVal v_lClaimPerilId As Integer, ByVal r_vCoInsurers(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCoInsurerDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For lRow As Integer = 0 To r_vCoInsurers.GetUpperBound(1)

                ' Clear Down Database Parameters

                m_oDatabase.Parameters.Clear()

                bPMAddParameter.AddParameterLite(m_oDatabase, "ReserveId", v_lReserveId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimPerilId", v_lClaimPerilId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", CType(CInt(r_vCoInsurers(0, lRow)), gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)

                bPMAddParameter.AddParameterLite(m_oDatabase, "Percentage", CType(CDbl(r_vCoInsurers(3, lRow)), gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)

                ' Execute selection Query

                If m_oDatabase.SQLAction(sSQL:=kUpdateCoInsurerDetailsSQL, sSQLName:=kUpdateCoInsurerDetailsName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError(kMethodName, kUpdateCoInsurerDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

                End If

            Next lRow


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateReserveEntries
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 27-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function CreateReserveEntries(ByVal v_lClaimPerilId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateReserveEntries"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            '    m_oDatabase.Parameters.Clear
            '
            '    ' Add Required Stored Procedure Parameters
            '    m_lReturn = AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_lClaimPerilId, v_iType:=PMLong)
            '
            '    ' Execute Action Query
            '    If m_oDatabase.SQLAction( _
            ''        sSQL:=kCreateReserveEntriesSQL, _
            ''        sSQLName:=kCreateReserveEntriesName, _
            ''        bStoredProcedure:=True) <> PMTrue Then
            '
            '        RaiseError kMethodName, kCreateReserveEntriesSQL & " Failed", PMLogError
            '
            '    End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-08-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetClaimDetails(ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimDetailsSQL, sSQLName:=kGetClaimDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCurrentClaimPaymentReserveDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetCurrentClaimPaymentReserveDetails(ByVal v_lClaimPerilId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCurrentClaimPaymentReserveDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetCurrentClaimPaymentReserveDetailsSQL, sSQLName:=kGetCurrentClaimPaymentReserveDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCurrentClaimPaymentReserveDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetOtherPartyDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetOtherPartyDetails(ByVal v_lPartyCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetOtherPartyDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="party_cnt", v_vValue:=v_lPartyCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetOtherPartyDetailsSQL, sSQLName:=kGetOtherPartyDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetOtherPartyDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAccountDetailsByShortCode
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetAccountDetailsByShortCode(ByVal v_sShortCode As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAccountDetailsByShortCode"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="short_code", v_vValue:=v_sShortCode, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetAccountDetailsByShortCodeSQL, sSQLName:=kGetAccountDetailsByShortCodeName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetAccountDetailsByShortCodeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentItemDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 11-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetClaimPaymentItemDetails(ByVal v_lClaimPaymentId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentItemDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentItemDetailsSQL, sSQLName:=kGetClaimPaymentItemDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentItemDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTaxGroupDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-07-2005 : 360 - Taxes On Claims
    ' ***************************************************************** '
    Public Function GetTaxGroupDetails(ByVal v_vIsWithHoldingTax As Object, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaxGroupDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="is_withholding_tax", v_vValue:=v_vIsWithHoldingTax, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="transaction_type_code", v_vValue:=m_sTransactionType, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetTaxGroupDetailsSQL, sSQLName:=kGetTaxGroupDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, kGetTaxGroupDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CalculateTaxAmounts
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 23-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function CalculateTaxAmounts(ByVal v_lCompanyId As Integer, ByVal v_lTaxGroupId As Integer, ByVal v_sTranstype As String, ByVal v_lCurrencyId As Integer, ByVal v_lLossCurrencyId As Integer, ByVal v_crAmount As Decimal, ByRef r_crTaxCurrencyAmount As Decimal, ByRef r_crTaxLossAmount As Decimal, ByRef r_crTaxBaseAmount As Decimal, ByVal v_lClaimPerilId As Integer, ByVal v_lClaimPaymentId As Integer, ByVal v_lClaimReceiptId As Integer, ByVal v_lClaimPaymentItemId As Integer, ByVal v_lClaimReceiptItemId As Integer, ByVal v_lCalculateOnly As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateTaxAmounts"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="company_id", v_vValue:=v_lCompanyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_group_id", v_vValue:=v_lTaxGroupId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="transtype", v_vValue:=v_sTranstype, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_id", v_vValue:=v_lCurrencyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="loss_currency_id", v_vValue:=v_lLossCurrencyId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="amount", v_vValue:=v_crAmount, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="calculate_only", v_vValue:=v_lCalculateOnly, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            If v_lClaimPaymentId <> 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_lClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_lClaimReceiptId <> 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="claim_receipt_id", v_vValue:=v_lClaimReceiptId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_lClaimPaymentItemId <> 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_item_id", v_vValue:=v_lClaimPaymentItemId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            If v_lClaimReceiptItemId <> 0 Then
                m_lReturn = CType(AddInputParameter(v_sName:="claim_receipt_item_id", v_vValue:=v_lClaimReceiptItemId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            End If

            ' output params
            m_lReturn = CType(AddOutputParameter(v_sName:="tax_currency_amount", v_vValue:=r_crTaxCurrencyAmount, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddOutputParameter(v_sName:="tax_loss_amount", v_vValue:=r_crTaxLossAmount, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddOutputParameter(v_sName:="tax_base_amount", v_vValue:=r_crTaxBaseAmount, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kCalculateTaxAmountsSQL, sSQLName:=kCalculateTaxAmountsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kCalculateTaxAmountsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' get total tax amounts....

            r_crTaxBaseAmount = m_oDatabase.Parameters.Item("tax_base_amount").Value

            r_crTaxLossAmount = gPMFunctions.NullToDecimal(m_oDatabase.Parameters.Item("tax_loss_amount").Value)

            r_crTaxCurrencyAmount = m_oDatabase.Parameters.Item("tax_currency_amount").Value


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ExecuteAdvancedTaxScript
    '
    ' Parameters:
    '
    ' Description: Used by Claim Payment and Receipt Controls to run
    '               advanced tax scripts
    '
    ' History:
    '           Created : MEvans : 23-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function ExecuteAdvancedTaxScript(ByVal v_lTaxScriptMode As Integer, ByVal v_sTaxScriptName As String, ByVal v_vTaxParameters() As Object, ByRef r_vUpdatedTaxParameters() As Object,
                                             ByVal nTaxGroupID As Integer) As Integer

        Dim nResult As Integer = 0
        Const kMethodName As String = "CalculateTaxAmounts"

        Dim oTaxParameters As Object
        Dim sLoadedRule As String = String.Empty
        Dim sLoadedRuleContent As String = String.Empty
        Dim sScript As String = String.Empty
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sAssemblyClassName As String = ""
        Const kTaxScriptModePayment As Integer = 1
        '    Const kTaxArrayTaxGroupId As Integer = 0
        Dim nRuleType As Integer = 1

        nResult = gPMConstants.PMEReturnCode.PMTrue

        lReturn = GetRuleTypeAndFileValue(nTaxGroupID, nRuleType, sAssemblyClassName)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If nRuleType = 1 Then
            ' load the requested rule file
            lReturn = CType(LoadRule(v_sTaxScriptName:=v_sTaxScriptName, r_sLoadedRule:=sLoadedRule, r_sLoadedRuleContent:=sLoadedRuleContent), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "LoadRule Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        ' depending on the transaction type
        ' create the appropriate tax parameters object
        If v_lTaxScriptMode = kTaxScriptModePayment Then
            oTaxParameters = New cPaymentTaxParameters()
        Else
            oTaxParameters = New cReceiptTaxParameters()
        End If

        ' load the tax parameters object
        lReturn = oTaxParameters.ArrayToData(CType(v_vTaxParameters, Object))
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
        End If

        If nRuleType = 1 Then
            lReturn = ExecuteAdvancedTaxScriptRuleFile(sLoadedRuleContent, oTaxParameters, r_vUpdatedTaxParameters)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If
        Else
            lReturn = ExecuteAdvancedTaxScriptCompiledRules(sAssemblyClassName, oTaxParameters, r_vUpdatedTaxParameters)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If




        Return nResult
    End Function

    ' ***************************************************************** '
    ' Name: GetRulePath
    '
    ' Parameters: n/a
    '
    ' Description: Returns the default Rule path from the registry
    '
    ' History:
    '           Created : MEvans : 23-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetRulePath(ByRef r_sRulePath As String) As Integer
        Dim Catch_Renamed As Boolean = False

        Dim result As Integer = 0
        Const kMethodName As String = "CalculateTaxAmounts"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try
            Catch_Renamed = True

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get rule path from registry
            lReturn = CType(gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:="RulePath", r_sSettingValue:=r_sRulePath, v_sSubKey:=kDefaultRulePathSubKey), gPMConstants.PMEReturnCode)

            ' if no path has been returned the raise error
            If r_sRulePath = "" Then
                gPMFunctions.RaiseError(kMethodName, "GetPMRegSetting Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If Not r_sRulePath.EndsWith("\") Then
                r_sRulePath = r_sRulePath & "\"
            End If

            Return result

        Catch excep As System.Exception
            If Not Catch_Renamed Then
                Throw excep
            End If

            GoTo Finally_Renamed

            If Catch_Renamed Then

                ' Do Not Call any functions before here or the error will be lost
                bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=excep)

                ' If you want to rollback a transaction or something, do it here

            End If
Finally_Renamed:
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LoadRule
    '
    ' Parameters: n/a
    '
    ' Description: Returns the default Rule path from the registry
    '
    ' History:
    '           Created : MEvans : 23-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function LoadRule(ByVal v_sTaxScriptName As String, ByRef r_sLoadedRule As String, ByRef r_sLoadedRuleContent As String) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadRule"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim sFilePath As String = String.Empty
        Dim sFile As String = String.Empty
        Dim fso As Scripting.FileSystemObject
        Dim fsoFile As FileInfo
        '   Dim fsoTxtStream As Scripting.TextStream
        Dim sLeft As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' get the default rule path from the registry
            lReturn = CType(GetRulePath(sFilePath), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetRulePath Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the file name
            sLeft = v_sTaxScriptName.Substring(0, 1)
            If sLeft = "\" Or sLeft = "/" Then
                ' need to strip one character
                v_sTaxScriptName = v_sTaxScriptName.Substring(1)
            End If

            ' combine file path and name
            r_sLoadedRule = sFilePath & v_sTaxScriptName

            ' create file system object
            fso = New Scripting.FileSystemObject()

            ' if the specified rule file exists
            If File.Exists(r_sLoadedRule) Then
                ' open the rule file
                Dim lInputFileNum, lFileLength As Integer
                lInputFileNum = FileSystem.FreeFile()
                FileSystem.FileOpen(lInputFileNum, r_sLoadedRule, OpenMode.Input)
                lFileLength = FileSystem.LOF(lInputFileNum)
                ' save the contents of the rule file
                r_sLoadedRuleContent = FileSystem.InputString(lInputFileNum, lFileLength)
                FileSystem.FileClose(lInputFileNum)

            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

            'destroy references
            fso = Nothing
            fsoFile = Nothing
            '   fsoTxtStream = Nothing

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetTaxGroupTaxBandDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 30-08-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetTaxGroupTaxBandDetails(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaxGroupTaxBandDetails"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetTaxGroupTaxBandDetailsSQL, sSQLName:=kGetTaxGroupTaxBandDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetTaxGroupTaxBandDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ''' <summary>
    ''' SaveClaimPayment
    ''' </summary>
    ''' <param name="v_vClaimPayment"></param>
    ''' <param name="r_lClaimPaymentId"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveClaimPayment(ByVal v_vClaimPayment() As Object, ByRef r_lClaimPaymentId As Integer) As Integer

        Dim result As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SaveClaimPayment"

        Const kClaimPayment As Integer = 0
        Const kClaimid As Integer = 1
        Const kClaimPerilId As Integer = 2
        Const kPaymentDate As Integer = 3
        Const kAmount As Integer = 4
        Const kTaxAmount As Integer = 5
        Const kTaxAmountWHT As Integer = 6
        Const kPartyCnt As Integer = 7
        Const kComments As Integer = 8
        Const kIsReferred As Integer = 9
        Const kCreatedBy As Integer = 10
        Const kPayeeMediaTypeId As Integer = 11
        Const kPayeeName As Integer = 12
        Const kBankName As Integer = 13
        Const kBankSortCode As Integer = 14
        Const kBankAccountNo As Integer = 15
        Const kPayeeCountryId As Integer = 16
        Const kPayeeComments As Integer = 17
        Const kSequenceNo As Integer = 18
        Const kTreatyId As Integer = 19
        Const kClaimPaymentTo As Integer = 20
        Const kPaymentPartyTo As Integer = 21
        Const kInsuredDomiciled As Integer = 22
        Const kInsuredPercentage As Integer = 23
        Const kInsuredTaxNumber As Integer = 24
        Const kPayeeDomiciled As Integer = 25
        Const kPayeePercentage As Integer = 26
        Const kPayeeTaxNumber As Integer = 27
        Const kSafeHarbourId As Integer = 28
        Const kSafeHarbourPercentage As Integer = 29
        Const kIsTaxExempt As Integer = 30
        Const kIsWHTExempt As Integer = 31
        Const kIsSettlement As Integer = 32
        Const kDocumentId As Integer = 33
        'Const kIsLive As Integer = 34
        'Const kLiveClaimPaymentId As Integer = 35
        Const kMediaRef As Integer = 36
        Const kCurrencyId As Integer = 37
        Const kExcessAmount As Integer = 38
        Const kPayeeAddress1 As Integer = 39
        Const kPayeeAddress2 As Integer = 40
        Const kPayeeAddress3 As Integer = 41
        Const kPayeeAddress4 As Integer = 42
        Const kPayeePostalCode As Integer = 43
        Const kThirdPartyReference As Integer = 44
        Const kChequeDate As Integer = 45
        Const kBankPaymentTypeId As Integer = 46

        Const kOurReference As Integer = 47
        Const kIsExGratia As Integer = 48
        Const kBIC As Integer = 49
        Const kIBAN As Integer = 50


        Try
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddOutputParameter(v_sName:="claim_payment_id", v_vValue:=v_vClaimPayment(kClaimPayment), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="claim_id", v_vValue:=v_vClaimPayment(kClaimid), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_vClaimPayment(kClaimPerilId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="date_of_payment", v_vValue:=v_vClaimPayment(kPaymentDate), v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="amount", v_vValue:=v_vClaimPayment(kAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="tax_amount", v_vValue:=v_vClaimPayment(kTaxAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="tax_amount_wht", v_vValue:=v_vClaimPayment(kTaxAmountWHT), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="party_cnt", v_vValue:=v_vClaimPayment(kPartyCnt), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="comments", v_vValue:=v_vClaimPayment(kComments), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="is_referred", v_vValue:=v_vClaimPayment(kIsReferred), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="created_by", v_vValue:=v_vClaimPayment(kCreatedBy), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="payeemediatype", v_vValue:=v_vClaimPayment(kPayeeMediaTypeId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="payeename", v_vValue:=v_vClaimPayment(kPayeeName), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="payeebankname", v_vValue:=v_vClaimPayment(kBankName), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="payeesortcode", v_vValue:=v_vClaimPayment(kBankSortCode), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="payeeaccountno", v_vValue:=v_vClaimPayment(kBankAccountNo), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="payeecomments", v_vValue:=v_vClaimPayment(kPayeeComments), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="sequenceno", v_vValue:=v_vClaimPayment(kSequenceNo), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="treaty_id", v_vValue:=v_vClaimPayment(kTreatyId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="claim_payment_to_id", v_vValue:=v_vClaimPayment(kClaimPaymentTo), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="payment_party_to", v_vValue:=v_vClaimPayment(kPaymentPartyTo), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="insured_domiciled", v_vValue:=v_vClaimPayment(kInsuredDomiciled), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="insured_percentage", v_vValue:=v_vClaimPayment(kInsuredPercentage), v_iType:=gPMConstants.PMEDataType.PMDouble)
            AddInputParameter(v_sName:="insured_tax_number", v_vValue:=v_vClaimPayment(kInsuredTaxNumber), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="payee_domiciled", v_vValue:=v_vClaimPayment(kPayeeDomiciled), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="payee_percentage", v_vValue:=v_vClaimPayment(kPayeePercentage), v_iType:=gPMConstants.PMEDataType.PMDouble)
            AddInputParameter(v_sName:="payee_tax_number", v_vValue:=v_vClaimPayment(kPayeeTaxNumber), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="safe_harbour_id", v_vValue:=v_vClaimPayment(kSafeHarbourId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="safe_harbour_percentage", v_vValue:=v_vClaimPayment(kSafeHarbourPercentage), v_iType:=gPMConstants.PMEDataType.PMDouble)
            AddInputParameter(v_sName:="is_tax_exempt", v_vValue:=v_vClaimPayment(kIsTaxExempt), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="is_wht_exempt", v_vValue:=v_vClaimPayment(kIsWHTExempt), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="is_settlement", v_vValue:=v_vClaimPayment(kIsSettlement), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="document_id", v_vValue:=v_vClaimPayment(kDocumentId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="media_ref", v_vValue:=v_vClaimPayment(kMediaRef), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="currency_id", v_vValue:=v_vClaimPayment(kCurrencyId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="excess_amount", v_vValue:=v_vClaimPayment(kExcessAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency)

            AddInputParameter(v_sName:="PayeeAddress1", v_vValue:=v_vClaimPayment(kPayeeAddress1), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeAddress2", v_vValue:=v_vClaimPayment(kPayeeAddress2), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeAddress3", v_vValue:=v_vClaimPayment(kPayeeAddress3), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeAddress4", v_vValue:=v_vClaimPayment(kPayeeAddress4), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeePostalCode", v_vValue:=v_vClaimPayment(kPayeePostalCode), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="payeecountry", v_vValue:=v_vClaimPayment(kPayeeCountryId), v_iType:=gPMConstants.PMEDataType.PMLong)

            AddInputParameter(v_sName:="ThirdPartyReference", v_vValue:=v_vClaimPayment(kThirdPartyReference), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="cheque_date", v_vValue:=v_vClaimPayment(kChequeDate), v_iType:=gPMConstants.PMEDataType.PMDate)
            'Party Bank Details
            ''65665

            Dim dbNumericTemp As Double
            If Not Double.TryParse(Convert.ToString(v_vClaimPayment(kBankPaymentTypeId)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                v_vClaimPayment(kBankPaymentTypeId) = DBNull.Value
            End If
            AddInputParameter(v_sName:="party_bank_id", v_vValue:=v_vClaimPayment(kBankPaymentTypeId), v_iType:=gPMConstants.PMEDataType.PMInteger)

            AddInputParameter(v_sName:="our_ref", v_vValue:=v_vClaimPayment(kOurReference), v_iType:=gPMConstants.PMEDataType.PMString)

            AddInputParameter(v_sName:="is_ex_gratia ", v_vValue:=v_vClaimPayment(kIsExGratia), v_iType:=gPMConstants.PMEDataType.PMBoolean)
            AddInputParameter(v_sName:="sBusinessIdentifierCode", v_vValue:=v_vClaimPayment(kBIC), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="sInternationalBankAccountNumber", v_vValue:=v_vClaimPayment(kIBAN), v_iType:=gPMConstants.PMEDataType.PMString)
            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kSaveClaimPaymentSQL, sSQLName:=kSaveClaimPaymentName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kSaveClaimPaymentSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            Else

                ' return id...

                r_lClaimPaymentId = m_oDatabase.Parameters.Item("claim_payment_id").Value

            End If
            Return result
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
            result = PMEReturnCode.PMFalse
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: SaveClaimPaymentItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function SaveClaimPaymentItem(ByVal v_vClaimPaymentItem As Object, ByRef r_lClaimPaymentItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveClaimPaymentItem"

        Const kClaimPaymentItemId As Integer = 0
        Const kClaimPaymentId As Integer = 1
        Const kReserveId As Integer = 2
        Const kRecoveryId As Integer = 3
        Const kRecoveryTypeId As Integer = 4
        Const kCurrencyId As Integer = 5
        Const kTaxGroupId As Integer = 6
        Const kThisPayment As Integer = 7
        Const kTaxAmount As Integer = 8
        Const kTaxAmountWHT As Integer = 9
        Const kExchangeRateOverrideReasonId As Integer = 10
        Const kCurrencyBaseXrate As Integer = 11
        Const kCurrencyBaseDate As Integer = 12
        Const kAccountBaseXrate As Integer = 13
        Const kAccountBaseDate As Integer = 14
        Const kSystemBaseXrate As Integer = 15
        Const kSystemBaseDate As Integer = 16
        Const kPaymentToLossXRate As Integer = 17
        'Const kIsLive As Integer = 18
        'Const kLiveClaimPaymentId As Integer = 19
        'Const kLiveRecoveryId As Integer = 20
        'Const kLiveReserveId As Integer = 21
        'Const kLiveClaimPaymentItemId As Integer = 22
        Const kPaymentAdjustment As Integer = 23



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            m_lReturn = CType(AddOutputParameter(v_sName:="claim_payment_item_id", v_vValue:=v_vClaimPaymentItem(kClaimPaymentItemId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_vClaimPaymentItem(kClaimPaymentId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="reserve_id", v_vValue:=v_vClaimPaymentItem(kReserveId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="recovery_id", v_vValue:=v_vClaimPaymentItem(kRecoveryId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="recovery_type_id", v_vValue:=v_vClaimPaymentItem(kRecoveryTypeId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_id", v_vValue:=v_vClaimPaymentItem(kCurrencyId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_group_id", v_vValue:=v_vClaimPaymentItem(kTaxGroupId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="this_payment", v_vValue:=v_vClaimPaymentItem(kThisPayment), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_amount", v_vValue:=v_vClaimPaymentItem(kTaxAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_amount_wht", v_vValue:=v_vClaimPaymentItem(kTaxAmountWHT), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="exchange_rate_override_reason_id", v_vValue:=v_vClaimPaymentItem(kExchangeRateOverrideReasonId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_base_xrate", v_vValue:=v_vClaimPaymentItem(kCurrencyBaseXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_base_date", v_vValue:=v_vClaimPaymentItem(kCurrencyBaseDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="account_base_xrate", v_vValue:=v_vClaimPaymentItem(kAccountBaseXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="account_base_date", v_vValue:=v_vClaimPaymentItem(kAccountBaseDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="system_base_xrate", v_vValue:=v_vClaimPaymentItem(kSystemBaseXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="system_base_date", v_vValue:=v_vClaimPaymentItem(kSystemBaseDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="payment_loss_xrate", v_vValue:=v_vClaimPaymentItem(kPaymentToLossXRate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="payment_adjustment", v_vValue:=v_vClaimPaymentItem(kPaymentAdjustment), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kSaveClaimPaymentItemSQL, sSQLName:=kSaveClaimPaymentItemName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kSaveClaimPaymentItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                ' return id...

                r_lClaimPaymentItemId = m_oDatabase.Parameters.Item("claim_payment_item_id").Value
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SaveTaxCalculationItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function SaveTaxCalculationItem(ByVal v_vTaxCalculation As Object, ByRef r_lTaxCalculationCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveTaxCalculationItem"

        Const kTaxCalculationCnt As Integer = 0
        Const kClaimPerilId As Integer = 1
        Const kClaimPaymentId As Integer = 2
        Const kClaimReceiptId As Integer = 3
        Const kClaimPaymentItemId As Integer = 4
        Const kClaimReceiptItemId As Integer = 5
        Const kTaxBandId As Integer = 6
        Const kPremium As Integer = 7
        Const kPercentage As Integer = 8
        Const kValue As Integer = 9
        Const kIsValue As Integer = 10
        Const kCurrencyId As Integer = 11
        Const kClassOfBusinessId As Integer = 12
        Const kTaxGroupId As Integer = 13
        Const kSequence As Integer = 14
        Const kTransType As Integer = 15
        Const kIsManuallyChanged As Integer = 16



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            m_lReturn = CType(AddOutputParameter(v_sName:="tax_calculation_cnt", v_vValue:=v_vTaxCalculation(kTaxCalculationCnt), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_vTaxCalculation(kClaimPerilId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_vTaxCalculation(kClaimPaymentId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_receipt_id", v_vValue:=v_vTaxCalculation(kClaimReceiptId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_item_id", v_vValue:=v_vTaxCalculation(kClaimPaymentItemId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_receipt_item_id", v_vValue:=v_vTaxCalculation(kClaimReceiptItemId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_band_id", v_vValue:=v_vTaxCalculation(kTaxBandId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="premium", v_vValue:=v_vTaxCalculation(kPremium), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="percentage", v_vValue:=v_vTaxCalculation(kPercentage), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="value", v_vValue:=v_vTaxCalculation(kValue), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="is_value", v_vValue:=v_vTaxCalculation(kIsValue), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_id", v_vValue:=v_vTaxCalculation(kCurrencyId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="class_of_business_id", v_vValue:=v_vTaxCalculation(kClassOfBusinessId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_group_id", v_vValue:=v_vTaxCalculation(kTaxGroupId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="sequence", v_vValue:=v_vTaxCalculation(kSequence), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="transtype", v_vValue:=v_vTaxCalculation(kTransType), v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="is_manually_changed", v_vValue:=v_vTaxCalculation(kIsManuallyChanged), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kSaveTaxCalculationItemSQL, sSQLName:=kSaveTaxCalculationItemName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kSaveTaxCalculationItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            Else
                ' return id...

                r_lTaxCalculationCnt = m_oDatabase.Parameters.Item("tax_calculation_cnt").Value
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPaymentItemTax
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 06-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function GetClaimPaymentItemTax(ByVal v_lClaimPaymentItemId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPaymentItemTax"



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_payment_item_id", v_vValue:=v_lClaimPaymentItemId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentItemTaxSQL, sSQLName:=kGetClaimPaymentItemTaxName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimPaymentItemTaxSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateClaimPaymentItemReserve
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function UpdateClaimPaymentItemReserve(ByVal v_lReserveId As Integer, ByVal v_crThisRevision As Decimal, ByVal v_crThisPayment As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimPaymentItemReserve"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="reserve_id", v_vValue:=v_lReserveId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="this_revision", v_vValue:=v_crThisRevision, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="this_payment", v_vValue:=v_crThisPayment, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kUpdateClaimPaymentItemReserveSQL, sSQLName:=kUpdateClaimPaymentItemReserveName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateClaimPaymentItemReserveSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '***************************************************************** '
    'Name:  UpdateClaimPaymentItemReserve
    '
    'Parameters: n/a
    '
    'Description:
    '
    'History:
    '           Created : MEvans : 08-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function PostPaymentToOrion(ByVal v_lClaimPaymentId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByVal v_cPayAmount As Decimal, ByVal v_sCreditAccountCode As String, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer, ByVal v_bPostClaimTax As Boolean, Optional ByVal v_lPartyCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostPaymentToOrion"

        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lDebitAccountID, lCreditAccountID As Integer
        Dim sDebitAccountCode As String = ""
        Dim lStatsFolderCnt As Integer
        Dim oClaimTrans As bControlTransClaims.Automated
        Dim sCreditAccountCode As String = ""
        Dim vTaxAmountByTaxType(,) As Object = Nothing
        Dim llBound, lUBOund As Integer
        Dim sTaxTypeCode As String = ""
        Dim crTaxAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oClaimTrans = New bControlTransClaims.Automated
            lReturn = oClaimTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bControlTransClaims.Automated", gPMConstants.PMELogLevel.PMLogError)
            End If

            'A payment will debit the reserve account.
            sDebitAccountCode = "CLMRES" & v_sCOBCode.Trim()

            'get credit account id - use party count if we have it
            If v_lPartyCnt <> 0 Then

                lReturn = oClaimTrans.GetAccountID(r_lAccountID:=lCreditAccountID, v_lPartyCnt:=v_lPartyCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.GetAccountId Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If
            End If

            'data which goes in stats folder/detail and transaction detail

            oClaimTrans.DebitAccountID = lDebitAccountID

            oClaimTrans.CreditAccountID = lCreditAccountID

            oClaimTrans.TransactionTypeID = 27

            oClaimTrans.TransactionTypeCode = "C_CP" 'claim payment

            oClaimTrans.DocumentTypeID = 28 'Claim Payment

            oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

            oClaimTrans.ClaimID = v_lClaimId

            oClaimTrans.PerilID = v_lClaimPerilId

            oClaimTrans.DebitCredit = "C"

            oClaimTrans.DocumentComment = "Payment for claim number " & v_sClaimNumber

            oClaimTrans.TransactionAmount = v_cPayAmount

            'RWH(02/07/01) Need to create stats separately now for each record to
            'account for reins and coins.

            lReturn = oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:="C_CP")
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsFolder Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode.Trim(), v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=v_sCreditAccountCode.Trim(), v_lRIPartyType:=0, v_sglRISharePercent:=0)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get tax lines grouped by tax type for this payment
            lReturn = CType(GetClaimTaxAmountsByTaxType(v_vClaimPaymentId:=v_lClaimPaymentId, r_vResults:=vTaxAmountByTaxType), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentTaxAmountsByTaxType Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' process the tax rows..
            If Informations.IsArray(vTaxAmountByTaxType) Then

                llBound = vTaxAmountByTaxType.GetLowerBound(1)

                lUBOund = vTaxAmountByTaxType.GetUpperBound(1)

                For lTaxTypeItem As Integer = llBound To lUBOund

                    ' get the tax type details

                    sTaxTypeCode = CStr(vTaxAmountByTaxType(kTaxTypeArrayPosCode, lTaxTypeItem)).Trim()

                    crTaxAmount = CDec(vTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, lTaxTypeItem))

                    ' Insert stats details records for Tax (One gross line for each tax type)
                    If crTaxAmount <> 0 And v_bPostClaimTax Then

                        ' Pass tax amount

                        oClaimTrans.TransactionAmount = crTaxAmount

                        ' set tan / tag account code
                        sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                        ' Create stats for gross tax amount

                        lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAG", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode.Trim(), v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                Next

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oClaimTrans = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: PostReserveAdjustmentToOrion
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function PostReserveAdjustmentToOrion(ByVal v_crRevisionAmount As Decimal, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_sClaimNo As String, ByVal v_lPerilID As Integer, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostReserveAdjustmentToOrion"

        Dim lReturn As gPMConstants.PMEReturnCode

        Dim lTransactionTypeID As Integer
        Dim sTransactionTypeCode As String = ""
        Dim lCreditAccountID As Integer
        Dim sDebitAccountCode, sCreditAccountCode As String
        Dim lStatsFolderCnt As Integer
        Dim oClaimTrans As bControlTransClaims.Automated

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oClaimTrans = New bControlTransClaims.Automated
            lReturn = oClaimTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bControlTransClaims.Automated", gPMConstants.PMELogLevel.PMLogError)
            End If

            'initialising debit/credit account
            sDebitAccountCode = "CLMEXP" & v_sCOBCode.Trim()
            sCreditAccountCode = "CLMRES" & v_sCOBCode.Trim()

            ' this is a payment adjustment
            sTransactionTypeCode = "C_CR"
            lTransactionTypeID = 28

            If m_sUnderwritingOrAgency = "" Then
                lReturn = getUnderwritingOrAgency()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "getUnderwritingOrAgency Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'post to Orion only when reserves has been added/changed
            If v_crRevisionAmount <> 0 Then

                'data which goes in stats folder/detail and transaction detail

                oClaimTrans.TransactionTypeID = lTransactionTypeID

                oClaimTrans.TransactionTypeCode = sTransactionTypeCode

                oClaimTrans.DocumentTypeID = 41 'Claim Adjustment

                oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

                oClaimTrans.ClaimID = v_lClaimId

                oClaimTrans.PerilID = v_lPerilID

                oClaimTrans.DebitCredit = "D"

                oClaimTrans.DocumentComment = "Reserve Adjustment for Claim Number " & v_sClaimNo

                oClaimTrans.TransactionAmount = v_crRevisionAmount

                ' create stats folder for payment adjustment

                lReturn = oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=sTransactionTypeCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsFolder Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Create stats_detail for payment adjustment

                lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=lCreditAccountID, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oClaimTrans = Nothing

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    '***************************************************************** '
    'Name:  PostBrokingPaymentToOrion
    '
    'Parameters: n/a
    '
    'Description: New Version of Claim Document Posting For Broking
    '
    'History:
    '           Created : EKnott : xx-11-2005 :
    ' ***************************************************************** '
    Public Function PostBrokingPaymentToOrion(ByVal v_lClaimPaymentId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_cPayAmount As Decimal, ByVal v_sCreditAccountCode As String, ByVal v_bPostClaimTax As Boolean, ByVal v_lMediaType As Integer, ByVal v_sComments As String, ByVal v_lPartyCnt As Integer, ByRef r_lClientAccountId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostBrokingPaymentToOrion"

        Dim iCompanyId As Integer
        Dim dtAccountingDate As Date
        Dim lDocumentType As Integer
        Dim sGroupCode, sRangeCode As String
        Dim lNumberRangeID As Integer
        Dim sDocumentRef As String = ""
        Dim lDocumentID As Integer
        Dim vDrawerSubBranchId As Object = Nothing

        Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign

        Dim iCurrencyID As Integer
        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroCcyXrate As Object = Nothing
        Dim vdEuroBaseXrate As Object = Nothing
        Dim vdCurrencyAmountUnrounded As Double
        Dim vdBaseAmountUnrounded As Double
        Dim lAccountID, lTransDetailID As Integer
        Dim v_Details As Object = Nothing
        Dim sInsurer As String = ""

        Dim lPartyAccountID As Integer
        Dim oParty As bSIRParty.Business
        Dim oDocumentPost As bACTDocumentPost.Form
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim oInsuranceFile As bSIRInsuranceFile.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bACTDocumentPost.Form", gPMConstants.PMELogLevel.PMLogError)
            End If


            oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            oInsuranceFile = New bSIRInsuranceFile.Business
            m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bSIRInsuranceFile.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            oParty = New bSIRParty.Business
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            iCompanyId = m_iSourceID
            dtAccountingDate = DateTime.Now
            iCurrencyID = m_iCurrencyID
            cCurrencyAmount = v_cPayAmount

            ' Check if we are creating a credit or debit for the client account PN24371
            If cCurrencyAmount > 0 Then
                eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignCredit
            Else
                eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit
            End If

            lDocumentType = gACTLibrary.ACTDocTypeClaimPayment
            sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef28
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClp

            ' Get the number range

            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Number Range Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Generate the next number

            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = oPMAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=iCompanyId, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=sRangeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Number Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Format the number
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            'sDocumentRef = Format(lNumber, "00000000")
            sDocumentRef = sRangeCode & sDocumentRef

            m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeId:=lDocumentType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="Cash", r_vDocumentId:=lDocumentID, r_vDocSourceID:=iCompanyId, v_vBatchId:=0, r_vSubBranchId:=vDrawerSubBranchId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add Document Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=vdEuroCcyXrate, r_vEuroBaseXRate:=vdEuroBaseXrate, r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Base Amount from Currency Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oParty.GetAccountID(vPartyRef:=v_sCreditAccountCode, vAccountID:=lPartyAccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Party Account Id Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If
            r_lClientAccountId = lPartyAccountID
            cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
            cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)
            vdBaseAmountUnrounded = gACTLibrary.ACTSigned(vdBaseAmountUnrounded, eCreditOrDebit)
            vdCurrencyAmountUnrounded = gACTLibrary.ACTSigned(vdCurrencyAmountUnrounded, eCreditOrDebit)
            lTransDetailID = 0

            ' now create the credit transaction (to the client)

            m_lReturn = oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=1, v_lAccountId:=lPartyAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCCyXrate:=vdEuroCcyXrate, v_vComment:=v_sComments, v_vAccountingDate:=dtAccountingDate, v_vSpare:="", v_vSubBranchId:=vDrawerSubBranchId, v_vDocSourceID:=iCompanyId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add Transaction for Client Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            'THIS BIT WILL NEED TO CHANGE FOR COINSURERS
            ' get the party id for the insurer

            m_lReturn = oInsuranceFile.GetDetails(vInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Policy Details Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oInsuranceFile.GetNext(r_vFieldArray:=v_Details)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Next Policy Details Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            lAccountID = CInt(v_Details(9))

            ' find the shortcode for the insurer

            m_lReturn = oParty.GetDetails(vPartyCnt:=lAccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Insurer Details Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oParty.GetNext(vShortname:=sInsurer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Insurer Get Next Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oParty.GetAccountID(vPartyRef:=sInsurer, vAccountID:=lAccountID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Insurer Account Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            cBaseAmount *= -1
            cCurrencyAmount *= -1
            vdBaseAmountUnrounded *= -1
            vdCurrencyAmountUnrounded *= -1
            lTransDetailID = 0

            ' now create the debit transaction (from the insurer)

            m_lReturn = oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=2, v_lAccountId:=lAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCCyXrate:=vdEuroCcyXrate, v_vComment:=v_sComments, v_vAccountingDate:=dtAccountingDate, v_vSpare:="", v_vSubBranchId:=vDrawerSubBranchId, v_vDocSourceID:=iCompanyId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add Insurer Transaction Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oDocumentPost = Nothing
            oPMAutoNumber = Nothing
            oInsuranceFile = Nothing
            oParty = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '***************************************************************** '
    'Name:  PostBrokingReceiptToOrion
    '
    'Parameters: n/a
    '
    'Description: New Version of Claim Document Posting For Broking
    '
    'History:
    '           Created : A.Robinson : 27-01-2006
    '                       S4B Claim Enhancements R&D 2005
    ' ***************************************************************** '

    Public Function PostBrokingReceiptToOrion(ByVal v_lClaimReceiptId As Integer, ByVal v_lClaimId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_cReceiptAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_bPostClaimTax As Boolean, ByVal v_lMediaType As Integer, ByVal v_sComments As String, ByVal v_lPartyCnt As Integer, ByVal v_vInsurerDetails(,) As Object, ByRef r_lClientAccountId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PostBrokingReceiptToOrion"
        Const kCOL_INSURANCE_FILE_REFERENCE As Integer = 7
        Const kCOL_INSURER_DETAILS_SHORTNAME As Integer = 7
        Const kCOL_INSURER_DETAILS_PERCENTAGE As Integer = 4

        Dim iCompanyId As Integer
        Dim dtAccountingDate As Date
        Dim lDocumentType As Integer
        Dim sGroupCode, sRangeCode As String
        Dim lNumberRangeID As Integer
        Dim sDocumentRef As String = ""
        Dim lDocumentID As Integer
        Dim vDrawerSubBranchId As Object = Nothing
        Dim sInsuranceFileRef As String = ""

        Dim eCreditOrDebit As gACTLibrary.ACTEAccountSign

        Dim iCurrencyID As Integer
        Dim cBaseAmount, cCurrencyAmount As Decimal
        Dim vdCurrencyBaseXRate As Object = Nothing
        Dim lEuroCurrencyID As Integer
        Dim cEuroAmount As Decimal
        Dim vdEuroCcyXrate As Object = Nothing
        Dim vdEuroBaseXrate As Object = Nothing
        Dim vdCurrencyAmountUnrounded As Double
        Dim vdBaseAmountUnrounded As Double
        Dim lAccountID, lTransDetailID As Integer
        Dim v_Details As Object = Nothing
        Dim sInsurer As String = ""

        Dim cInsBaseAmount, cInsCurrencyAmount As Decimal
        Dim vdInsBaseAmountUnrounded As Double
        Dim vdInsCurrencyAmountUnrounded As Double
        Dim lPartyAccountID As Integer
        Dim oParty As bSIRParty.Business
        Dim oDocumentPost As bACTDocumentPost.Form
        Dim oPMAutoNumber As bACTAutoNumber.Business
        Dim oInsuranceFile As bSIRInsuranceFile.Business

        Dim lDocumentSequence As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oDocumentPost = New bACTDocumentPost.Form
            m_lReturn = oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bACTDocumentPost.Form", gPMConstants.PMELogLevel.PMLogError)
            End If


            oPMAutoNumber = New bACTAutoNumber.Business
            m_lReturn = oPMAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bACTAutoNumber.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            oInsuranceFile = New bSIRInsuranceFile.Business
            m_lReturn = oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bSIRInsuranceFile.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            oParty = New bSIRParty.Business
            m_lReturn = oParty.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bSIRParty.Business", gPMConstants.PMELogLevel.PMLogError)
            End If

            iCompanyId = m_iSourceID
            dtAccountingDate = DateTime.Now
            iCurrencyID = m_iCurrencyID
            cCurrencyAmount = v_cReceiptAmount

            ' Check if we are creating a credit or debit for the client account PN24371
            eCreditOrDebit = gACTLibrary.ACTEAccountSign.acteSignDebit

            lDocumentType = gACTLibrary.ACTDocTypeClaimReceipt
            sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef29
            sRangeCode = gACTLibrary.ACTAutoNumberRangeCodeClr

            ' Get the number range

            m_lReturn = oPMAutoNumber.GetNumberRange(v_sGroupCode:=sGroupCode, v_sRangeCode:=sRangeCode, r_lNumberRangeID:=lNumberRangeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Number Range Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Generate the next number

            'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

            m_lReturn = oPMAutoNumber.GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=iCompanyId, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=sRangeCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Number Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Format the number

            'sDocumentRef = Format(lNumber, "00000000")
            sDocumentRef = sRangeCode & sDocumentRef

            m_lReturn = oDocumentPost.AddDocument(v_lDocumentTypeId:=lDocumentType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtAccountingDate, v_sComment:="Cash", r_vDocumentId:=lDocumentID, r_vDocSourceID:=iCompanyId, v_vBatchId:=0, v_vClaimId:=v_lClaimId, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vSubBranchId:=vDrawerSubBranchId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add Document Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetBaseAmountFromCurrency(v_iCurrencyID:=iCurrencyID, r_cBaseAmount:=cBaseAmount, v_cCurrencyAmount:=cCurrencyAmount, r_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_dtAccountingDate:=dtAccountingDate, r_lEuro:=lEuroCurrencyID, r_cEuroAmount:=cEuroAmount, r_vEuroCCyXrate:=vdEuroCcyXrate, r_vEuroBaseXRate:=vdEuroBaseXrate, r_vCCyAmountUnrounded:=vdCurrencyAmountUnrounded, r_vBaseAmountUnrounded:=vdBaseAmountUnrounded)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Base Amount from Currency Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oParty.GetAccountID(vPartyRef:=v_sDebitAccountCode, vAccountID:=lPartyAccountID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Party AccountId Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oInsuranceFile.GetDetails(vInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Policy Details Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = oInsuranceFile.GetNext(r_vFieldArray:=v_Details)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Get Next Policy Details Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If
            sInsuranceFileRef = gPMFunctions.ToSafeString(v_Details(kCOL_INSURANCE_FILE_REFERENCE)).Trim()

            r_lClientAccountId = lPartyAccountID
            cBaseAmount = gACTLibrary.ACTSigned(cBaseAmount, eCreditOrDebit)
            cCurrencyAmount = gACTLibrary.ACTSigned(cCurrencyAmount, eCreditOrDebit)
            vdBaseAmountUnrounded = gACTLibrary.ACTSigned(vdBaseAmountUnrounded, eCreditOrDebit)
            vdCurrencyAmountUnrounded = gACTLibrary.ACTSigned(vdCurrencyAmountUnrounded, eCreditOrDebit)
            lDocumentSequence = 1
            lTransDetailID = 0

            ' now create the debit transaction

            m_lReturn = oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=lDocumentSequence, v_lAccountId:=lPartyAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cBaseAmount, v_vBaseAmountUnrounded:=vdBaseAmountUnrounded, v_cCurrencyAmount:=cCurrencyAmount, v_vCurrencyAmountUnrounded:=vdCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCCyXrate:=vdEuroCcyXrate, v_vComment:=v_sComments, v_vAccountingDate:=dtAccountingDate, v_vSpare:="", v_vSubBranchId:=vDrawerSubBranchId, v_vDocSourceID:=iCompanyId, v_vInsuranceRef:=sInsuranceFileRef, v_vClaimReference:=v_sClaimNumber)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Add Transaction for Client Failed ", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Credit the insurer account(s)
            If Informations.IsArray(v_vInsurerDetails) Then

                For lLoop As Integer = v_vInsurerDetails.GetLowerBound(1) To v_vInsurerDetails.GetUpperBound(1)

                    sInsurer = gPMFunctions.ToSafeString(v_vInsurerDetails(kCOL_INSURER_DETAILS_SHORTNAME, lLoop)).Trim()

                    m_lReturn = oParty.GetAccountID(vPartyRef:=sInsurer, vAccountID:=lAccountID)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Get Insurer Account Failed ", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    cInsBaseAmount = cBaseAmount * (-(gPMFunctions.ToSafeDouble(v_vInsurerDetails(kCOL_INSURER_DETAILS_PERCENTAGE, lLoop)) / 100))
                    cInsCurrencyAmount = cCurrencyAmount * (-(gPMFunctions.ToSafeDouble(v_vInsurerDetails(kCOL_INSURER_DETAILS_PERCENTAGE, lLoop)) / 100))
                    vdInsBaseAmountUnrounded = vdBaseAmountUnrounded * (-(gPMFunctions.ToSafeDouble(v_vInsurerDetails(kCOL_INSURER_DETAILS_PERCENTAGE, lLoop)) / 100))
                    vdInsCurrencyAmountUnrounded = vdCurrencyAmountUnrounded * (-(gPMFunctions.ToSafeDouble(v_vInsurerDetails(kCOL_INSURER_DETAILS_PERCENTAGE, lLoop)) / 100))

                    lDocumentSequence += 1
                    lTransDetailID = 0

                    ' now create the credit transaction (to the insurer)

                    m_lReturn = oDocumentPost.AddTransaction(r_vTransDetailId:=lTransDetailID, v_vDocumentSequence:=lDocumentSequence, v_lAccountId:=lAccountID, v_iCurrencyID:=iCurrencyID, v_cAmount:=cInsBaseAmount, v_vBaseAmountUnrounded:=vdInsBaseAmountUnrounded, v_cCurrencyAmount:=cInsCurrencyAmount, v_vCurrencyAmountUnrounded:=vdInsCurrencyAmountUnrounded, v_vdCurrencyBaseXRate:=vdCurrencyBaseXRate, v_vEuroCurrencyID:=lEuroCurrencyID, v_vEuroAmount:=cEuroAmount, v_vEuroBaseXrate:=vdEuroBaseXrate, v_vEuroCCyXrate:=vdEuroCcyXrate, v_vComment:=v_sComments, v_vAccountingDate:=dtAccountingDate, v_vSpare:="", v_vSubBranchId:=vDrawerSubBranchId, v_vDocSourceID:=iCompanyId, v_vInsuranceRef:=sInsuranceFileRef, v_vClaimReference:=v_sClaimNumber)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "Add Insurer Transaction Failed ", gPMConstants.PMELogLevel.PMLogError)
                    End If

                Next lLoop
            Else
                gPMFunctions.RaiseError(kMethodName, "No insurer details found", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            oDocumentPost = Nothing
            oPMAutoNumber = Nothing
            oInsuranceFile = Nothing
            oParty = Nothing


        End Try
        Return result
    End Function

    Private Function GetBaseAmountFromCurrency(ByVal v_iCurrencyID As Integer, ByRef r_cBaseAmount As Decimal, ByVal v_cCurrencyAmount As Decimal, ByRef r_vdCurrencyBaseXRate As Object, ByVal v_dtAccountingDate As Date, ByRef r_lEuro As Integer, ByRef r_cEuroAmount As Decimal, ByRef r_vEuroCCyXrate As Object, ByRef r_vEuroBaseXRate As Object, ByRef r_vCCyAmountUnrounded As Object, ByRef r_vBaseAmountUnrounded As Object) As Integer

        Dim result As Integer = 0
        Dim oCurrencyConvert As Object = Nothing
        Const kMethodName As String = "CreditClient"
        ' Const kErrorCode As Integer = Informations.vbObjectError
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            ' Calculate Base Amount if Currency

            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=oCurrencyConvert, v_sClassName:="bACTCurrencyConvert.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to create business object bACTCurrencyConvert.Form", gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_iCurrencyID <> gACTLibrary.CompanyBaseCurrency() Then

                m_lReturn = oCurrencyConvert.ConvertCurrencytoBase(lCurrencyId:=ToSafeInteger(v_iCurrencyID), cBaseAmount:=ToSafeDouble(r_cBaseAmount), cCurrencyAmount:=ToSafeDouble(v_cCurrencyAmount), vConversionDate:=ToSafeDate(v_dtAccountingDate), vRounded:=True, lEuro:=ToSafeInteger(r_lEuro), cEuroAmount:=ToSafeDouble(r_cEuroAmount), vEuroCCyXrate:=CType(r_vEuroCCyXrate, Object), vEuroBaseXrate:=CType(r_vEuroBaseXRate, Object), vCCyAmountUnRounded:=CType(r_vCCyAmountUnrounded, Object), vBaseAmountUnRounded:=CType(r_vBaseAmountUnrounded, Object))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFail
                End If

                r_vdCurrencyBaseXRate = oCurrencyConvert.ConversionRate

            Else
                ' Home currency transaction
                r_cBaseAmount = v_cCurrencyAmount
                r_vdCurrencyBaseXRate = 1
                r_lEuro = 0
                r_cEuroAmount = 0
                r_vEuroCCyXrate = 0
                r_vEuroBaseXRate = 0
                If r_vCCyAmountUnrounded = 0 Then
                    r_vCCyAmountUnrounded = v_cCurrencyAmount
                    r_vBaseAmountUnrounded = r_cBaseAmount
                Else
                    r_vBaseAmountUnrounded = r_vCCyAmountUnrounded
                End If
            End If

        Catch ex As Exception
            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do your tidy up here. i.e. Terminate and set = Nothing object referenes
            oCurrencyConvert = Nothing

        End Try
        Return result
    End Function
    ' ***************************************************************** '
    ' Name: GetClaimTaxAmountsByTaxType
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 12-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Private Function GetClaimTaxAmountsByTaxType(ByRef r_vResults(,) As Object, Optional ByVal v_vClaimPaymentId As Object = Nothing, Optional ByVal v_vClaimReceiptId As Object = Nothing, Optional ByVal v_lClaimReceiptItemId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimTaxAmountsByTaxType"


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear Down Database Parameters

        m_oDatabase.Parameters.Clear()

        ' Add Required Stored Procedure Parameters

        If Not Informations.IsNothing(v_vClaimPaymentId) Then
            AddInputParameter(v_sName:="claim_payment_id", v_vValue:=v_vClaimPaymentId, v_iType:=gPMConstants.PMEDataType.PMLong)
        End If

        If Not Informations.IsNothing(v_vClaimReceiptId) Then
            AddInputParameter(v_sName:="claim_receipt_id", v_vValue:=v_vClaimReceiptId, v_iType:=gPMConstants.PMEDataType.PMLong)
        End If

        If v_lClaimReceiptItemId > 0 Then
            AddInputParameter(v_sName:="claim_receipt_item_id", v_vValue:=v_lClaimReceiptItemId, v_iType:=gPMConstants.PMEDataType.PMLong)
        End If

        ' Execute selection Query

        If m_oDatabase.SQLSelect(sSQL:=kGetClaimTaxAmountsByTaxTypeSQL, sSQLName:=kGetClaimTaxAmountsByTaxTypeName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

            gPMFunctions.RaiseError(kMethodName, kGetClaimTaxAmountsByTaxTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

        End If

        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimPerilRecoveryDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetClaimPerilRecoveryDetails(ByVal v_lClaimPerilId As Integer, ByVal v_bIsSalvage As Boolean, ByRef r_vResults(,) As Object, Optional ByRef v_lClaimReceiptId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimPerilRecoveryDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            m_lReturn = CType(AddInputParameter(v_sName:="is_salvage", v_vValue:=v_bIsSalvage, v_iType:=gPMConstants.PMEDataType.PMBoolean), gPMConstants.PMEReturnCode)

            bPMAddParameter.AddParameterLite(m_oDatabase, "ClaimReceiptId", v_lClaimReceiptId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimPerilRecoveryDetailsSQL, sSQLName:=kGetClaimPerilRecoveryDetailsName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimPerilRecoveryDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SaveClaimReceipt
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function SaveClaimReceipt(ByVal v_vClaimReceiptDetails As Object, ByRef r_lClaimReceiptId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveClaimReceipt"

        ' Const kClaimReceiptId As Integer = 0
        Const kClaimid As Integer = 1
        Const kClaimPerilId As Integer = 2
        Const kDateOfReceipt As Integer = 3
        Const kPartyCnt As Integer = 4
        Const kAmount As Integer = 5
        Const kTaxAmount As Integer = 6
        Const kComments As Integer = 7
        Const kCreatedBy As Integer = 8
        Const kInsuredDomiciled As Integer = 9
        Const kInsuredPercentage As Integer = 10
        Const kInsuredTaxNumber As Integer = 11
        Const kReceivableTaxPercentage As Integer = 12
        Const kReceivableIsTaxExempt As Integer = 13
        Const kIsSettlement As Integer = 14
        Const kPayeeMediaTypeId As Integer = 15
        Const kPayeeName As Integer = 16
        Const kBankName As Integer = 17
        Const kBankSortCode As Integer = 18
        Const kBankAccountNo As Integer = 19
        Const kPayeeCountryId As Integer = 20
        Const kPayeeComments As Integer = 21
        Const kPayeeMediaRef As Integer = 22
        Const kDocumentId As Integer = 23
        Const kCurrencyId As Integer = 24
        'Const kIsLive As Integer = 25
        'Const kLiveClaimReceiptId As Integer = 26


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' add output params
            AddOutputParameter(v_sName:="claim_receipt_id", v_vValue:=0, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' add input params
            AddInputParameter(v_sName:="claim_id", v_vValue:=v_vClaimReceiptDetails(kClaimid), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="claim_peril_id", v_vValue:=v_vClaimReceiptDetails(kClaimPerilId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="date_of_receipt", v_vValue:=v_vClaimReceiptDetails(kDateOfReceipt), v_iType:=gPMConstants.PMEDataType.PMDate)
            AddInputParameter(v_sName:="party_cnt", v_vValue:=v_vClaimReceiptDetails(kPartyCnt), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="Amount", v_vValue:=v_vClaimReceiptDetails(kAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="tax_amount", v_vValue:=v_vClaimReceiptDetails(kTaxAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="comments", v_vValue:=v_vClaimReceiptDetails(kComments), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="created_by", v_vValue:=v_vClaimReceiptDetails(kCreatedBy), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="insured_domiciled", v_vValue:=v_vClaimReceiptDetails(kInsuredDomiciled), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="insured_percentage", v_vValue:=v_vClaimReceiptDetails(kInsuredPercentage), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="insured_tax_number", v_vValue:=v_vClaimReceiptDetails(kInsuredTaxNumber), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="receivable_tax_percentage", v_vValue:=v_vClaimReceiptDetails(kReceivableTaxPercentage), v_iType:=gPMConstants.PMEDataType.PMCurrency)
            AddInputParameter(v_sName:="is_tax_exempt", v_vValue:=v_vClaimReceiptDetails(kReceivableIsTaxExempt), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="is_settlement", v_vValue:=v_vClaimReceiptDetails(kIsSettlement), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="PayeeMediaType", v_vValue:=v_vClaimReceiptDetails(kPayeeMediaTypeId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="PayeeName", v_vValue:=v_vClaimReceiptDetails(kPayeeName), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeBankName", v_vValue:=v_vClaimReceiptDetails(kBankName), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeSortCode", v_vValue:=v_vClaimReceiptDetails(kBankSortCode), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeAccountNo", v_vValue:=v_vClaimReceiptDetails(kBankAccountNo), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeCountry", v_vValue:=v_vClaimReceiptDetails(kPayeeCountryId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="PayeeComments", v_vValue:=v_vClaimReceiptDetails(kPayeeComments), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="PayeeMediaRef", v_vValue:=v_vClaimReceiptDetails(kPayeeMediaRef), v_iType:=gPMConstants.PMEDataType.PMString)
            AddInputParameter(v_sName:="document_id", v_vValue:=v_vClaimReceiptDetails(kDocumentId), v_iType:=gPMConstants.PMEDataType.PMLong)
            AddInputParameter(v_sName:="currency_id", v_vValue:=v_vClaimReceiptDetails(kCurrencyId), v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kSaveClaimReceiptSQL, sSQLName:=kSaveClaimReceiptName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kSaveClaimReceiptSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            Else
                ' return id...

                r_lClaimReceiptId = m_oDatabase.Parameters.Item("claim_receipt_id").Value
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: SaveClaimReceiptItem
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 02-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function SaveClaimReceiptItem(ByVal v_vClaimReceiptItem As Object, ByRef r_lClaimReceiptItemId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SaveClaimReceiptItem"

        Const kClaimReceiptItemId As Integer = 0
        Const kClaimReceiptId As Integer = 1
        Const kRecoveryId As Integer = 2
        Const kReserveId As Integer = 3
        Const kRecoveryTypeId As Integer = 4
        Const kCurrencyId As Integer = 5
        Const kThisReceipt As Integer = 6
        Const kTaxGroupId As Integer = 7
        Const kTaxAmount As Integer = 8
        Const kExchangeRateOverrideReasonId As Integer = 10
        Const kCurrencyBaseXrate As Integer = 11
        Const kCurrencyBaseDate As Integer = 12
        Const kAccountBaseXrate As Integer = 13
        Const kAccountBaseDate As Integer = 14
        Const kSystemBaseXrate As Integer = 15
        Const kSystemBaseDate As Integer = 16
        Const kReceiptLossXrate As Integer = 17
        'Const kIsLive As Integer = 18
        'Const kLiveClaimReceiptItemId As Integer = 19
        'Const kLiveClaimReceiptId As Integer = 20
        'Const kLiveReserveId As Integer = 21
        'Const kLiveRecoveryId As Integer = 22



        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddOutputParameter(v_sName:="claim_receipt_item_id", v_vValue:=v_vClaimReceiptItem(kClaimReceiptItemId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="claim_receipt_id", v_vValue:=v_vClaimReceiptItem(kClaimReceiptId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="reserve_id", v_vValue:=v_vClaimReceiptItem(kReserveId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="recovery_id", v_vValue:=v_vClaimReceiptItem(kRecoveryId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="recovery_type_id", v_vValue:=v_vClaimReceiptItem(kRecoveryTypeId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_id", v_vValue:=v_vClaimReceiptItem(kCurrencyId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_group_id", v_vValue:=v_vClaimReceiptItem(kTaxGroupId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="this_receipt", v_vValue:=v_vClaimReceiptItem(kThisReceipt), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_amount", v_vValue:=v_vClaimReceiptItem(kTaxAmount), v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="exchange_rate_override_reason_id", v_vValue:=v_vClaimReceiptItem(kExchangeRateOverrideReasonId), v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_base_xrate", v_vValue:=v_vClaimReceiptItem(kCurrencyBaseXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="currency_base_date", v_vValue:=v_vClaimReceiptItem(kCurrencyBaseDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="account_base_xrate", v_vValue:=v_vClaimReceiptItem(kAccountBaseXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="account_base_date", v_vValue:=v_vClaimReceiptItem(kAccountBaseDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="system_base_xrate", v_vValue:=v_vClaimReceiptItem(kSystemBaseXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="system_base_date", v_vValue:=v_vClaimReceiptItem(kSystemBaseDate), v_iType:=gPMConstants.PMEDataType.PMDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="receipt_loss_xrate", v_vValue:=v_vClaimReceiptItem(kReceiptLossXrate), v_iType:=gPMConstants.PMEDataType.PMDouble), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kSaveClaimReceiptItemSQL, sSQLName:=kSaveClaimReceiptItemName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kSaveClaimReceiptItemSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            Else
                ' return id...

                r_lClaimReceiptItemId = m_oDatabase.Parameters.Item("claim_receipt_item_id").Value
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateClaimReceiptItemRecovery
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 08-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function UpdateClaimReceiptItemRecovery(ByVal v_lRecoveryId As Integer, ByVal v_crThisRevision As Decimal, ByVal v_crThisReceipt As Decimal, ByVal v_crTaxAmount As Decimal) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimReceiptItemRecovery"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="recovery_id", v_vValue:=v_lRecoveryId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="this_revision", v_vValue:=v_crThisRevision, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="this_receipt", v_vValue:=v_crThisReceipt, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="tax_amount", v_vValue:=v_crTaxAmount, v_iType:=gPMConstants.PMEDataType.PMCurrency), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kUpdateClaimReceiptItemRecoverySQL, sSQLName:=kUpdateClaimReceiptItemRecoveryName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateClaimReceiptItemRecoverySQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    '***************************************************************** '
    'Name:  UpdateClaimPaymentItemReserve
    '
    'Parameters: n/a
    '
    'Description:
    '
    'History:
    '           Created : MEvans : 20-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function PostReceiptToOrion(ByVal v_bIsSalvage As Integer, ByVal v_lClaimReceiptId As Integer, ByVal v_sClaimNumber As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lClaimId As Integer, ByVal v_lClaimPerilId As Integer, ByVal v_crGrossReceiptAmount As Decimal, ByVal v_crNetReceiptAmount As Decimal, ByVal v_crTaxAmount As Decimal, ByVal v_sDebitAccountCode As String, ByVal v_sCOBCode As String, ByVal v_lCOBId As Integer, ByVal v_bPostClaimTax As Boolean, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_lClaimReceiptItemId As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Form_Load"

        Dim oClaimTrans As bControlTransClaims.Automated = Nothing
        Dim sCreditAccountCode As String = ""
        Dim lCreditAccountID, lDebitAccountID, lStatsFolderCnt, lReturn As Integer
        Dim sDocumentComment, sTransactionType As String
        Dim vTaxAmountByTaxType(,) As Object = Nothing
        Dim llBound, lUBOund As Integer
        Dim sTaxTypeCode As String = ""
        Dim crTaxAmount As Decimal

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_bIsSalvage Then
                sDocumentComment = "Salvage for Claim Number :=" & v_sClaimNumber
                sTransactionType = "C_SA"
            Else
                sDocumentComment = "TP Recovery for Claim Number :=" & v_sClaimNumber
                sTransactionType = "C_RV"
            End If

            ' Create object to send to orion

            oClaimTrans = New bControlTransClaims.Automated
            lReturn = oClaimTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oObjectManager.GetInstance() Failed to get instance of bControlTransClaims.Automated", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' get the debit account id
            If v_lPartyCnt <> 0 Then

                lReturn = oClaimTrans.GetAccountID(r_lAccountID:=lDebitAccountID, v_lPartyCnt:=v_lPartyCnt)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.GetAccountId Failed to get account id")
                End If
            End If

            ' Data which goes in stats folder/detail and transaction detail

            oClaimTrans.DebitAccountID = lDebitAccountID

            oClaimTrans.CreditAccountID = lCreditAccountID

            oClaimTrans.TransactionTypeID = 28

            oClaimTrans.TransactionTypeCode = sTransactionType

            oClaimTrans.DocumentTypeID = 29 ' Claim receipt

            oClaimTrans.InsuranceFileCnt = v_lInsuranceFileCnt

            oClaimTrans.ClaimID = v_lClaimId

            oClaimTrans.PerilID = v_lClaimPerilId

            oClaimTrans.DebitCredit = "C"

            oClaimTrans.DocumentComment = sDocumentComment

            ' Transaction amount depends if we are posting taxes
            If v_bPostClaimTax Then
                ' Only post net, we will post taxes shortly

                oClaimTrans.TransactionAmount = v_crNetReceiptAmount
            Else
                ' Post everything to gross

                oClaimTrans.TransactionAmount = v_crGrossReceiptAmount
            End If

            ' Need to create stats separately now for each record to account for reins and coins.

            lReturn = oClaimTrans.CreateStatsFolder(r_lStatsFolderCnt:=lStatsFolderCnt, v_sTransactionTypeCode:=sTransactionType)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsFolder Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Create stats_detail for main payment.

            lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="GRS", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=v_sDebitAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed to create stats detail for GRS line")
            End If

            ' get tax lines grouped by tax type for this payment
            lReturn = GetClaimTaxAmountsByTaxType(v_vClaimReceiptId:=v_lClaimReceiptId, r_vResults:=vTaxAmountByTaxType, v_lClaimReceiptItemId:=v_lClaimReceiptItemId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetClaimPaymentTaxAmountsByTaxType Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            If v_bPostClaimTax Then

                ' Pass tax amount

                oClaimTrans.TransactionAmount = v_crTaxAmount

                ' Create stats for gross tax amount

                lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAG", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode.Trim(), v_lRIPartyCnt:=v_lPartyCnt, v_sRIShortName:=v_sDebitAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                ' process the tax rows..
                If Informations.IsArray(vTaxAmountByTaxType) Then

                    llBound = vTaxAmountByTaxType.GetLowerBound(1)

                    lUBOund = vTaxAmountByTaxType.GetUpperBound(1)

                    For lTaxTypeItem As Integer = llBound To lUBOund

                        ' get the tax type details

                        sTaxTypeCode = CStr(vTaxAmountByTaxType(kTaxTypeArrayPosCode, lTaxTypeItem)).Trim()

                        crTaxAmount = CDec(vTaxAmountByTaxType(kTaxTypeArrayPosTaxAmount, lTaxTypeItem))

                        ' Insert stats details records for Tax (One gross line for each tax type)
                        If crTaxAmount <> 0 Then

                            ' Pass tax amount

                            oClaimTrans.TransactionAmount = -crTaxAmount

                            ' set tan / tag account code
                            sCreditAccountCode = "NOTA" & sTaxTypeCode & "IN"

                            ' Create stats for TAN amount

                            lReturn = oClaimTrans.CreateStatsDetails(v_lStatsFolderCnt:=lStatsFolderCnt, v_sStatsDetailType:="TAN", v_lClassOfBusId:=v_lCOBId, v_sClassOfBusCode:=v_sCOBCode, v_lRIPartyCnt:=0, v_sRIShortName:=sCreditAccountCode, v_lRIPartyType:=0, v_sglRISharePercent:=0)

                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError(kMethodName, "bControlTransClaims.Automated.CreateStatsDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If

                        End If

                    Next

                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            If Not (oClaimTrans Is Nothing) Then

                oClaimTrans.Dispose()
                oClaimTrans = Nothing

            End If

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: CreateClaimReserves
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 26-09-2005 : 360 - Taxes on Claims
    ' ***************************************************************** '
    Public Function CreateClaimReserves(ByVal v_lClaimPerilId As Integer, ByVal v_lRiskId As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CreateClaimReserves"

        Dim sUnderwritingOrAgency As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            sUnderwritingOrAgency = UnderwritingOrAgency

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="perilid", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="siriusproduct", v_vValue:=sUnderwritingOrAgency, v_iType:=gPMConstants.PMEDataType.PMString), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="policyid", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)
            m_lReturn = CType(AddInputParameter(v_sName:="riskid", v_vValue:=v_lRiskId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kCreateClaimReservesSQL, sSQLName:=kCreateClaimReservesName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kCreateClaimReservesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetCoinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetCoinsurance(ByVal v_lClaimPerilId As Integer, ByVal v_bIsSalvage As Boolean, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCoinsurance"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "peril_id", v_lClaimPerilId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", CType(Math.Abs(CInt(v_bIsSalvage)), gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetCoinsuranceSQL, sSQLName:=kGetCoinsuranceName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetCoinsuranceSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetReinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetReinsurance(ByVal v_lClaimPerilId As Integer, ByVal v_bIsSalvage As Boolean, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReinsurance"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "peril_id", v_lClaimPerilId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "is_salvage", CType(Math.Abs(CInt(v_bIsSalvage)), gPMConstants.PMEReturnCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetReinsuranceSQL, sSQLName:=kGetReinsuranceName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetReinsuranceSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: UpdateClaimReinsurance
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function UpdateClaimReinsurance(ByVal v_lClaimPerilId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateClaimReinsurance"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="peril_id", v_vValue:=v_lClaimPerilId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute Action Query

            If m_oDatabase.SQLAction(sSQL:=kUpdateClaimReinsuranceSQL, sSQLName:=kUpdateClaimReinsuranceName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kUpdateClaimReinsuranceSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetMediaTypes
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : Date : Process ID
    ' ***************************************************************** '
    Public Function GetMediaTypes(ByRef r_vResults(,) As Object, Optional ByVal iPaymentsOnly As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetMediaTypes"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()
            m_lReturn = AddInputParameter(v_sName:="PaymentsOnly", v_vValue:=iPaymentsOnly, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetMediaTypesSQL, sSQLName:=kGetMediaTypesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetMediaTypesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetClaimBranchCurrencies
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-11-2005 : PN25734
    ' ***************************************************************** '
    Public Function GetClaimBranchCurrencies(ByVal v_lSourceID As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimBranchCurrencies"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="source_id", v_vValue:=v_lSourceID, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=kGetClaimBranchCurrenciesSQL, sSQLName:=kGetClaimBranchCurrenciesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetClaimBranchCurrenciesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetUserCanChangeReserves
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Rajesh Choudhary : 15 Jan 2007 : (RC) QBENZ001
    ' ***************************************************************** '
    Public Function GetUserCanChangeReserves(ByVal v_lUserID As Integer, ByRef r_bCanChangeReserves As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetUserCanChangeReserves"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=v_lUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="can_change_reserves", vValue:=r_bCanChangeReserves, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMBoolean)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserCanChangeReservesSQL, sSQLName:=ACGetUserCanChangeReservesName, bStoredProcedure:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres

            r_bCanChangeReserves = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("can_change_reserves").Value)

            m_oDatabase.Parameters.Clear()


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetClaimXOLineCount(ByVal v_lClaimId As Integer, ByRef r_bHaveXOLLines As Boolean) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimXOLineCount"


        Dim vResults(,) As Object = Nothing
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            m_lReturn = CType(AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong), gPMConstants.PMEReturnCode)

            ' Execute selection Query

            If m_oDatabase.SQLSelect(sSQL:=ACGetXOLCountSQL, sSQLName:=ACGetXOLCountName, bStoredProcedure:=True, vResultArray:=vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, ACGetXOLCountName & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            If Informations.IsArray(vResults) Then

                If gPMFunctions.ToSafeInteger(CDec(vResults(0, 0))) > 0 Then
                    r_bHaveXOLLines = True
                End If
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function GetReceiptList(ByVal lClaimID As Integer, ByVal vRecoveryType As gPMConstants.PMEReturnCode, ByRef lRecoveryID As Integer, ByRef r_vReceiptList As Object, ByVal nSalvageAndTPRecoveryReceipts As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReceiptList"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "recoverytype", If(gPMFunctions.ToSafeLong(vRecoveryType) <> 0 And gPMFunctions.ToSafeLong(vRecoveryType) <> 1, DBNull.Value, vRecoveryType), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "RecoveryID", If(gPMFunctions.ToSafeLong(lRecoveryID, 0) <> 0, lRecoveryID, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "nSalvageAndTPRecoveryReceipts", nSalvageAndTPRecoveryReceipts, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(ACGetAllReceiptsForClaimSQL, ACGetAllReceiptsForClaimName, ACGetAllReceiptsForClaimStoredProcedure, , r_vReceiptList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "Call to the procedure spu_get_all_receipts_for_claim failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
        'GoTo Finally_Renamed

        'Catch_Renamed:
        '        ' Log Error.
        '        bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

        '        GoTo Finally_Renamed

        'Finally_Renamed:
        Return result

    End Function

    Public Function GetReceiptList(ByVal lClaimID As Integer, ByVal vRecoveryType As gPMConstants.PMEReturnCode, ByRef lRecoveryID As Integer, ByRef r_vReceiptList As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetReceiptList"
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "recoverytype", If(gPMFunctions.ToSafeLong(vRecoveryType) <> 0 And gPMFunctions.ToSafeLong(vRecoveryType) <> 1, DBNull.Value, vRecoveryType), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "RecoveryID", If(gPMFunctions.ToSafeLong(lRecoveryID, 0) <> 0, lRecoveryID, DBNull.Value), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(ACGetAllReceiptsForClaimSQL, ACGetAllReceiptsForClaimName, ACGetAllReceiptsForClaimStoredProcedure, , r_vReceiptList)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "Call to the procedure spu_get_all_receipts_for_claim failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' Log Error.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally


        End Try
        Return result
    End Function

    Public Function GetClaimReceiptDetails(ByVal v_lclaim_Receipt_id As Integer, ByRef r_vResultArray(,) As Object, ByRef v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_Receipt_id", v_lclaim_Receipt_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLSelect(ACGetClaimGetReceiptDetailsSQL, ACGetClaimGetReceiptDetailsName, ACGetClaimGetReceiptDetailsStoredProcedure, , r_vResultArray)


        Catch ex As Exception
            ' Log Error.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally


        End Try
        Return result
    End Function

    Public Function GetClaimReceiptItemTaxDetails(ByVal v_lclaim_Receipt_Item_id As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptItemTaxDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_Receipt_Item_id", v_lclaim_Receipt_Item_id, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            m_lReturn = m_oDatabase.SQLSelect(ACGetClaimTaxCalculationForReceiptSQL, ACGetClaimTaxCalculationForReceiptName, ACGetClaimTaxCalculationForReceiptStoredProcedure, , r_vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "Call to the procedure spu_CLM_Tax_Calculation_Select_For_Receipt failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            ' Log Error.
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally


        End Try
        Return result
    End Function

    Public Function GetClaimReceiptItemDetails(ByVal v_lClaimReceiptId As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetClaimReceiptItemDetails"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_receipt_id", v_lClaimReceiptId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute selection Query

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimReceiptItemDetailsSQL, sSQLName:=ACGetClaimReceiptItemDetailsName, bStoredProcedure:=ACGetClaimReceiptItemDetailsStoredProcedure, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetClaimReceiptItemDetailsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result

            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' WPR022
    Public Function UpdateClaimTransactionType(
                        ByVal v_lClaimId As Long,
                       ByVal v_sTransactionType As String) As Long


        Const kMethodName As String = "UpdateClaimTransactionType"
        Dim iReturn As Integer = 0
        Dim vResults As Object = Nothing
        Try
            iReturn = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' claim id
            iReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_lClaimId, v_iType:=gPMConstants.PMEDataType.PMLong)

            If (iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & " failed to add Claim_ID parameter.", gPMConstants.PMELogLevel.PMLogError)
            End If

            'transaction_type
            iReturn = AddInputParameter(v_sName:="transaction_type", v_vValue:=v_sTransactionType, v_iType:=gPMConstants.PMEDataType.PMString)

            If (iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, kMethodName & " failed to add Transaction_Type parameter.", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Execute selection Query
            iReturn = m_oDatabase.SQLSelect(
                                    sSQL:=ACUpdateClaimTransactionTypeSQL,
                                    sSQLName:=ACUpdateClaimTransactionTypeName,
                                    bStoredProcedure:=True,
                                    vResultArray:=vResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords)
            If (iReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, ACUpdateClaimTransactionTypeSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            Return iReturn
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(
                v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:=kMethodName,
                r_lFunctionReturn:=iReturn,
                excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return iReturn
        Finally
            'set all variable to nothing
        End Try
    End Function
    ''' <summary>
    ''' Name: IsAccountExists (Public)
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsAccountExists(ByVal AccountCode As String, ByRef IsExists As Boolean) As Integer
        Dim result As Integer = 0
        Dim dtResult As New DataTable
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "short_code", AccountCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            result = m_oDatabase.ExecuteDataTable(sSQL:=ACGetClaimGetAccountIdFromShortCodeSQL, sSQLName:=ACGetClaimGetAccountIdFromShortCodeName, bStoredProcedure:=ACGetClaimGetAccountIdFromShortCodeStoredProcedure, oRecordset:=dtResult)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If dtResult.Rows.Count > 0 Then
                IsExists = True
            Else
                IsExists = False
            End If

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to IsAccountExists", vApp:=ACApp, vClass:=ACClass, vMethod:="IsAccountExists", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try

    End Function


    ''' <summary>
    ''' WPR 85 - To Get default bank account
    ''' </summary>
    ''' <param name="v_nSourceID"></param>
    ''' <param name="v_nMediaType"></param>
    ''' <param name="v_nProductId"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultBankAccount(v_nSourceID As Integer, v_nMediaType As Integer, v_nProductId As Integer, ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetDefaultBankAccount"
        Dim nResult As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            Dim sSql As String

            sSql = "select bankaccount_id from BankAccount_Default"
            sSql = sSql & " left join CashListType on BankAccount_Default.cashlisttype_id = CashListType.cashlisttype_id"
            sSql = sSql & " where BankAccount_Default.source_id=" & CStr(v_nSourceID)
            sSql = sSql & " and CashListType.description='Receipts'"
            sSql = sSql & " and BankAccount_Default.mediatype_id=" & CStr(v_nMediaType)
            sSql = sSql & " and BankAccount_Default.effective_date <= GetDate()"
            sSql = sSql & " and BankAccount_Default.is_deleted <> 1"

            If v_nProductId <> 0 Then
                sSql = sSql & " and BankAccount_Default.product_id=" & CStr(v_nProductId)
            Else
                sSql = sSql & " and BankAccount_Default.product_id IS NULL"
            End If


            If m_oDatabase.SQLSelect(
                                    sSQL:=sSql,
                                    sSQLName:="GetDefaultBankAccount",
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, sSql & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return nResult

        Catch excep As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName + " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult
        End Try
    End Function

    ''' <summary>
    ''' WPR 85 - Get currency for bank account
    ''' </summary>
    ''' <param name="v_nBankAccountId"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetCurrencyFromBankAccount(ByVal v_nBankAccountId As Integer, ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetCurrencyFromBankAccount"
        Dim nResult As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            Dim sSql As String

            sSql = "select currency_id from BankAccount where bankaccount_id = {bankaccount_id}"

            m_lReturn = m_oDatabase.Parameters.Add(sName:="bankaccount_id",
                                                        vValue:=v_nBankAccountId,
                                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                        iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_oDatabase.SQLSelect(
                                    sSQL:=sSql,
                                    sSQLName:="GetCurrencyFromBankAccount",
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, sSql & " Failed", gPMConstants.PMELogLevel.PMLogError)

            End If

            Return nResult

        Catch excep As Exception

            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName + " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try

    End Function

    ''' <summary>
    ''' WPR 85 -To get default cash list item reciept type
    ''' </summary>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetDefaultCashListItemReceiptType(ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetDefaultCashListItemReceiptType"
        Dim nResult As Integer

        Try

            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            Dim sSql As String

            sSql = "select cashlistitem_receipt_type_id, code from CashListItem_Receipt_Type"
            sSql = sSql & " where description='Claim Receipt'"
            sSql = sSql & " and effective_date <= GetDate()"
            sSql = sSql & " and is_deleted = 0"


            If m_oDatabase.SQLSelect(
                                    sSQL:=sSql,
                                    sSQLName:="GetDefaultCashListItemReceiptType",
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                RaiseError(kMethodName, sSql & " Failed", PMELogLevel.PMLogError)

            End If
            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try

    End Function

    ''' <summary>
    ''' WPR 85-  To get media type lookup items
    ''' </summary>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetMediaTypeLookUpDetails(ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetMediaTypeLookUpDetails"

        Dim nResult As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            Dim sSql As String

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            sSql = "Select MediaType_id,  description, code from MediaType where is_deleted = 0 and effective_date <= GetDate() and is_receipt = 1 order by description"

            If m_oDatabase.SQLSelect(
                                    sSQL:=sSql,
                                    sSQLName:="SelectMediaTypeLookup",
                                    bStoredProcedure:=False,
                                    vResultArray:=r_oResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> PMEReturnCode.PMTrue Then

                RaiseError(kMethodName & " Failed", PMELogLevel.PMLogError)

            End If
            Return nResult
        Catch excep As Exception

            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' To Get Claim Payment total
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClaimPaymentTotal(ByVal nClaimId As Integer, ByRef r_oResults(,) As Object) As Integer

        Const kMethodName As String = "GetClaimPaymentTotal"

        Dim nReturn As Integer

        Try

            nReturn = PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            AddInputParameter(v_sName:="nClaim_Id", v_vValue:=nClaimId, v_iType:=PMEDataType.PMInteger)
            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimPaymentTotalSQL, sSQLName:=kGetClaimPaymentTotalName, bStoredProcedure:=True, vResultArray:=r_oResults)

            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, kGetClaimPaymentTotalSQL & " Failed", PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            nReturn = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=kMethodName + " failed", vApp:=ACApp,
                               vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nReturn
    End Function
#Region "Private Functions"


    ''' <summary>
    ''' To execute the Rule File For ATS
    ''' </summary>
    ''' <param name="sLoadedRuleContent"></param>
    ''' <param name="oTaxParameters"></param>
    ''' <param name="r_vUpdatedTaxParameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
	<HandleProcessCorruptedStateExceptions>
    Private Function ExecuteAdvancedTaxScriptRuleFile(ByVal sLoadedRuleContent As String, ByVal oTaxParameters As Object, ByRef r_vUpdatedTaxParameters As Object) As Integer
        Dim oScriptControl As MSScriptControl.ScriptControl
        Dim sScript As String = String.Empty
        Dim nReturn As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMTrue

        ' create an instance of the script control
        oScriptControl = New MSScriptControl.ScriptControl()

        ' we have successfully create the script control
        If Not (oScriptControl Is Nothing) Then

            ' set the script controls properties
            oScriptControl.Language = "VBScript"
            oScriptControl.AllowUI = True

            ' ensure that "option explicit" is set in the vbscript
            If (sLoadedRuleContent.IndexOf("Option Explicit") + 1) = 0 Then
                sScript = "Option Explicit" & Strings.ChrW(13) & Strings.ChrW(10) & sLoadedRuleContent
            Else
                sScript = sLoadedRuleContent
            End If

            ' set up rule to run
            oScriptControl.AddCode(sScript)

            ' add tax parameters object
            Try
                oScriptControl.AddObject("TaxParameters", oTaxParameters)

                ' run start method
                oScriptControl.Run("Start")
            Catch
            End Try

            ' copy data back into the for passing back to interface
            nReturn = oTaxParameters.DataToArray(r_vUpdatedTaxParameters)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nReturn = gPMConstants.PMEReturnCode.PMFalse
            End If

        End If

        'destroy object references
        oScriptControl = Nothing
        oTaxParameters = Nothing

        Return nReturn
    End Function

    ''' <summary>
    ''' To execute the compiled Rules For ATS
    ''' </summary>
    ''' <param name="sAssemblyClassName"></param>
    ''' <param name="oTaxParameters"></param>
    ''' <param name="r_vUpdatedTaxParameters"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function ExecuteAdvancedTaxScriptCompiledRules(ByVal sAssemblyClassName As String, ByVal oTaxParameters As Object, ByRef r_vUpdatedTaxParameters As Object) As Integer

        Dim sScript As String = String.Empty
        Dim nReturn As gPMConstants.PMEReturnCode = PMEReturnCode.PMTrue
        Dim oRules As Object

        If sAssemblyClassName = "" Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' add tax parameters object
        oRules = CreateLateBoundObject_CompiledRules(sAssemblyClassName)
        If Not (oRules Is Nothing) Then
            oRules.TaxParameters = oTaxParameters
            oRules.Start()
            oTaxParameters = oRules.TaxParameters
        End If

        ' copy data back into the for passing back to interface
        nReturn = oTaxParameters.DataToArray(r_vUpdatedTaxParameters)
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nReturn = gPMConstants.PMEReturnCode.PMFalse
        End If

        oTaxParameters = Nothing

        Return nReturn
    End Function

    ''' <summary>
    ''' get filename and rule type from tax_group table
    ''' </summary>
    ''' <param name="nTaxGroupID"></param>
    ''' <param name="nRuleTypeID"></param>
    ''' <param name="sAssemblyClassName"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetRuleTypeAndFileValue(ByVal nTaxGroupID As Integer, ByRef nRuleTypeID As Integer, ByRef sAssemblyClassName As String) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Dim oArray(,) As Object = Nothing
        'Clear the Database Parameters Collection
        With m_oDatabase

            .Parameters.Clear()
            nResult = .Parameters.Add(sName:="tax_group_id", vValue:=nTaxGroupID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            'Execute SQL Statement
            nResult = .SQLSelect(sSQL:=ACGetRuleTypeAndFileValueSQL, sSQLName:=ACGetRuleTypeAndFileValueName, bStoredProcedure:=ACGetRuleTypeAndFileValueStored, vResultArray:=oArray)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to process " & ACGetRuleTypeAndFileValueSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRuleTypeAndFileValue")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        If Informations.IsArray(oArray) Then
            nRuleTypeID = ToSafeInteger(oArray(0, 0))
            sAssemblyClassName = ToSafeString(oArray(1, 0))
        End If

        Return nResult

    End Function
#End Region

    ''' <summary>
    ''' GetClaimRecoveries
    ''' </summary>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="r_oResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClaimRecoveries(ByVal nClaimPerilID As Integer, ByRef r_oResultArray(,) As Object) As Integer

        Const kMethodName As String = "GetClaimRecoveries"

        Dim nReturn As Integer = PMEReturnCode.PMTrue

        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nReturn = AddInputParameter(v_sName:="lPerilID", v_vValue:=nClaimPerilID, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect( _
                                    sSQL:=kGetClaimRecoveriesSQL, _
                                    sSQLName:=kGetClaimRecoveriesName, _
                                    bStoredProcedure:=True, _
                                    vResultArray:=r_oResultArray, _
                                    lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Call to " + kGetClaimRecoveriesSQL + " Failed.")
            End If

        Catch ex As Exception

            bPMFunc.LogError( _
                 v_sUsername:=m_sUsername, _
                 v_sClass:=ACClass, _
                 v_sMethod:=kMethodName, _
                 r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            nReturn = gPMConstants.PMEReturnCode.PMFalse

        End Try

        Return nReturn

    End Function
    ''' <summary>
    ''' SaveClaimRecoveries
    ''' </summary>
    ''' <param name="nClaimPerilID"></param>
    ''' <param name="nRecoveryTypeID"></param>
    ''' <param name="r_cReserveAmount"></param>
    ''' <param name="nRecoveryPartyTypeID"></param>
    ''' <param name="nRecoveryPartyCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveClaimRecoveries( _
                        ByVal nClaimPerilID As Integer, _
                        ByVal nRecoveryTypeID As Integer, _
                        ByRef r_cReserveAmount As Decimal, _
                        Optional ByVal nRecoveryPartyTypeID As Integer = 0, _
                        Optional ByVal nRecoveryPartyCnt As Integer = 0) As Integer

        Const kMethodName As String = "SaveClaimRecoveries"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nReturn = AddInputParameter(v_sName:="lClaimPerilID", v_vValue:=nClaimPerilID, v_iType:=gPMConstants.PMEDataType.PMInteger)
            nReturn = AddInputParameter(v_sName:="lRecoveryTypeID", v_vValue:=nRecoveryTypeID, v_iType:=gPMConstants.PMEDataType.PMInteger)
            nReturn = AddInputParameter(v_sName:="crReserveAmount", v_vValue:=r_cReserveAmount, v_iType:=gPMConstants.PMEDataType.PMCurrency)

            If nRecoveryPartyTypeID > 0 Then
                nReturn = AddInputParameter(v_sName:="lRecoveryPartyTypeID", v_vValue:=nRecoveryPartyTypeID, v_iType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If nRecoveryPartyCnt > 0 Then
                nReturn = AddInputParameter(v_sName:="nRecoveryPartyCnt", v_vValue:=nRecoveryPartyCnt, v_iType:=gPMConstants.PMEDataType.PMInteger)
            End If

            ' Execute Action Query
            If m_oDatabase.SQLAction( _
                sSQL:=kSaveClaimRecoverySQL, _
                sSQLName:=kSaveClaimRecoveryName, _
                bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New ApplicationException("Call to " + kSaveClaimRecoverySQL + " Failed.")
            End If

        Catch ex As Exception

            bPMFunc.LogError( _
                 v_sUsername:=m_sUsername, _
                 v_sClass:=ACClass, _
                 v_sMethod:=kMethodName, _
                 r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            nReturn = gPMConstants.PMEReturnCode.PMFalse

        End Try

        Return nReturn

    End Function
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nClaimID"></param>
    ''' <param name="nClaimPaymentID"></param>
    ''' <param name="nPayeeMediaType"></param>
    ''' <param name="smedia_ref"></param>
    ''' <param name="sPayeeName"></param>
    ''' <param name="sPayeeBankName"></param>
    ''' <param name="sPayeeSortCode"></param>
    ''' <param name="sPayeeAccountNo"></param>
    ''' <param name="nPayeeCountry"></param>
    ''' <param name="sPayeeComments"></param>
    ''' <param name="sPayeeAddress1"></param>
    ''' <param name="sPayeeAddress2"></param>
    ''' <param name="sPayeeAddress3"></param>
    ''' <param name="sPayeeAddress4"></param>
    ''' <param name="sPayeePostalCode"></param>
    ''' <param name="sThirdPartyReference"></param>
    ''' <param name="sOur_ref"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function UpdateThisClaimPaymentDetails(ByVal nClaimID As Integer,
                                                    ByVal nClaimPaymentID As Integer,
                                                    ByVal nPayeeMediaType As Integer,
                                                    ByVal smedia_ref As String,
                                                    ByVal sPayeeName As String,
                                                    ByVal sPayeeBankName As String,
                                                    ByVal sPayeeSortCode As String,
                                                    ByVal sPayeeAccountNo As String,
                                                    ByVal nPayeeCountry As Integer,
                                                    ByVal sPayeeComments As String,
                                                    ByVal sPayeeAddress1 As String,
                                                    ByVal sPayeeAddress2 As String,
                                                    ByVal sPayeeAddress3 As String,
                                                    ByVal sPayeeAddress4 As String,
                                                    ByVal sPayeePostalCode As String,
                                                    ByVal sThirdPartyReference As String,
                                                    ByVal sOur_ref As String) As Integer

        Const kMethodName As String = "UpdateThisClaimPaymentDetails"
        Dim nReturn As Integer = PMEReturnCode.PMTrue
        Try
            m_oDatabase.Parameters.Clear()
            bPMAddParameter.AddParameterLite(m_oDatabase, "nClaimID", nClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "nClaimPaymentID", nClaimPaymentID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            bPMAddParameter.AddParameterLite(m_oDatabase, "nPayeeMediaType", nPayeeMediaType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "smedia_ref", smedia_ref, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeName", sPayeeName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeBankName", sPayeeBankName, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeSortCode", sPayeeSortCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeAccountNo", sPayeeAccountNo, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "nPayeeCountry", nPayeeCountry, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeComments", sPayeeComments, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeAddress1", sPayeeAddress1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeAddress2", sPayeeAddress2, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeAddress3", sPayeeAddress3, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeeAddress4", sPayeeAddress4, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sPayeePostalCode", sPayeePostalCode, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sThirdPartyReference", sThirdPartyReference, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "sOur_ref", sOur_ref, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            If m_oDatabase.SQLAction(
                sSQL:=kUpdateThisClaimPaymentSQL,
                sSQLName:=kUpdateThisClaimPaymentName,
                bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New ApplicationException("Call to " + kUpdateThisClaimPaymentSQL + " Failed.")
            End If

            nReturn = PMEReturnCode.PMTrue

        Catch ex As Exception
            bPMFunc.LogError(
            v_sUsername:=m_sUsername,
            v_sClass:=ACClass,
            v_sMethod:=kMethodName,
            r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            nReturn = PMEReturnCode.PMFalse
        End Try
        Return nReturn
    End Function

    ''' <summary>
    ''' Description: Gets The old Currency Rate When claim is opened and current currency rate
    ''' </summary>
    ''' <param name="v_nClaimId"></param>
    ''' <param name="r_oResults"></param>
    ''' <returns></returns>
    Public Function GetCurrencyRatesToOverride(ByVal v_nClaimId As Long,
                                           ByRef r_oResults As Object) As Integer
        Const kMethodName As String = "GetCurrencyRatesToOverride"
        Dim nReturn As Integer

        Try

            nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_nClaimId, v_iType:=gPMConstants.PMEDataType.PMInteger)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(
                            sSQL:=kGetCurrencyRatesToOverrideSQL,
                            sSQLName:=kGetCurrencyRatesToOverrideName,
                            bStoredProcedure:=True,
                            vResultArray:=r_oResults,
                           lNumberRecords:=gPMConstants.PMAllRecords)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            bPMFunc.LogError(
        v_sUsername:=m_sUsername,
        v_sClass:=ACClass,
        v_sMethod:=kMethodName,
        r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            nReturn = PMEReturnCode.PMError

        End Try
        Return nReturn
    End Function
    ''' <summary>
    ''' Description: Override the Claim's Currency Rate to the currency rate When claim is opened
    ''' </summary>
    ''' <param name="v_nClaimId"></param>
    ''' <param name="r_oOverriddenCurrencyRate"></param>
    ''' <returns></returns>
    Public Function OverrideClaimCurrencyRate(ByVal v_nClaimId As Long,
                                ByRef r_oOverriddenCurrencyRate As Object) As Integer

        Const kMethodName As String = "OverrideClaimCurrencyRate"

        Dim nReturn As Integer

        Try
            nReturn = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            nReturn = AddInputParameter(v_sName:="claim_id", v_vValue:=v_nClaimId, v_iType:=gPMConstants.PMEDataType.PMInteger)

            nReturn = AddInputParameter(v_sName:="OverriddenCurrencyRate", v_vValue:=r_oOverriddenCurrencyRate, v_iType:=gPMConstants.PMEDataType.PMDouble)



            nReturn = m_oDatabase.SQLSelect(
                            sSQL:=kOverrideClaimCurrencyRateSQL,
                            sSQLName:=kOverrideClaimCurrencyRateName,
                            bStoredProcedure:=True,
                            lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            bPMFunc.LogError(
        v_sUsername:=m_sUsername,
        v_sClass:=ACClass,
        v_sMethod:=kMethodName,
        r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)
            nReturn = PMEReturnCode.PMError
        End Try

        Return nReturn

    End Function

    Public Function GetUsersReserveLimit(ByRef dReserveLimit As Decimal) As Integer
        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserName", vValue:=m_sUsername, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransactionType", vValue:=m_sTransactionType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PerilID", vValue:=m_lPerilID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ReserveLimit", vValue:=dReserveLimit, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMDecimal)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetUserReserveLimitSQL, sSQLName:=ACGetUserReserveLimitName, bStoredProcedure:=ACGetUserReserveLimitStoredProcedure)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the return parameteres
            dReserveLimit = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("ReserveLimit").Value)

            Return result

        Catch excep As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Get Users Reserve Limit", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUsersReserveLimit", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return result
        End Try
    End Function

End Class
