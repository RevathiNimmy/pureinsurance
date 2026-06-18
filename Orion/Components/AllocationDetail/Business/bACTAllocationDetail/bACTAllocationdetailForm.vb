Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Globalization
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Form
    '
    ' Date: 22/01/1998
    '
    ' Description: Creatable Form class which contains all the
    '              methods, Form rules required to manipulate
    '              a Allocationdetail.
    '
    ' Edit History: TF191198 - amendments for EMU database changes
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 09/12/2003
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
    Private Const ACClass As String = "Form"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)


    ' PRIVATE Data Members (Begin)

    ' Collection of Allocationdetails (Private)
    Private m_oAllocationdetails As Allocationdetails

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
    Private m_lAllocationID As Integer

    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFOrion
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
                Case Is > m_oAllocationdetails.Count()
                    m_lCurrentRecord = m_oAllocationdetails.Count()
                Case Else
                    m_lCurrentRecord = Value
            End Select

        End Set
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get

            ' Return Numner in Collection
            Return m_oAllocationdetails.Count()

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


    Public Property AllocationID() As Integer
        Get
            Return m_lAllocationID
        End Get
        Set(ByVal Value As Integer)
            m_lAllocationID = Value
        End Set
    End Property

    Public ReadOnly Property Details() As Allocationdetails
        Get
            Return m_oAllocationdetails
        End Get
    End Property

    ' PUBLIC Property Procedures (End)

    ' PRIVATE Property Procedures (Begin)
    ' PRIVATE Property Procedures (End)

    ' PUBLIC Methods (Begin)

    ' ***************************************************************** '
    ' Name: SetWriteOff
    '
    ' Description: Sets the write off details for an allocationdetail
    '
    ' ***************************************************************** '
    Public Function SetWriteOff(ByVal v_lAllocationDetailID As Integer, ByVal v_cWriteOffAmount As Decimal, ByVal v_lWriteOffReasonID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim cWriteOffAmount As Decimal

        Dim vWriteOffReasonID As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    ' Get the old value
            '    sSQL$ = "SELECT write_off_amount FROM AllocationDetail " & _
            ''            "WHERE AllocationDetail_id = " & CStr(v_lAllocationDetailID)
            '
            '    m_lReturn& = m_oDatabase.SQLSelect( _
            ''        sSQL:=sSQL$, _
            ''        sSQLName:="GetWriteOffAmount", _
            ''        bStoredProcedure:=False, _
            ''        vResultArray:=vResultArray, _
            ''        lNumberRecords:=1)
            '    If (m_lReturn& <> PMTrue) Then
            '        SetWriteOff = PMFalse
            '        Exit Function
            '    End If
            '
            '    If (IsArray(vResultArray) = True) Then
            '        cWriteOffAmount = vResultArray(0, 0)
            '    Else
            '        cWriteOffAmount = 0
            '    End If

            ' Sum the two amounts
            '    cWriteOffAmount = cWriteOffAmount + v_cWriteOffAmount
            cWriteOffAmount = v_cWriteOffAmount

            If v_lWriteOffReasonID = 0 Then
                vWriteOffReasonID = "Null"
            Else
                vWriteOffReasonID = CStr(v_lWriteOffReasonID)
            End If

            ' Update the write off reason id and amount
            sSQL = "UPDATE AllocationDetail SET " & _
                   "write_off_reason_id = " & vWriteOffReasonID & ", " & _
                   "write_off_amount = " & CStr(cWriteOffAmount) & " " & _
                   "WHERE AllocationDetail_ID = " & CStr(v_lAllocationDetailID)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetWriteOffReason", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetWriteOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetWriteOff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    '''''''''Round Off

    Public Function SetRoundOff(ByVal v_lTransDetailId As Integer, ByVal v_cRoundOffAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim cRoundOffAmount As Decimal


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cRoundOffAmount = v_cRoundOffAmount

            ' Update the write off reason id and amount
            sSQL = "UPDATE AllocationDetail SET " & _
                   "round_off_amount = " & CStr(cRoundOffAmount) & " " & _
                   "WHERE transdetail_ID = " & CStr(v_lTransDetailId)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetRoundOffReason", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetRoundOff Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetRoundOff", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'EK 100100
    ' ***************************************************************** '
    ' Name: SetLossGain
    '
    ' Description: Sets the write off details for an allocationdetail
    '
    ' ***************************************************************** '
    Public Function SetLossGain(ByVal v_lAllocationDetailID As Integer, ByVal v_cLossGainAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim cLossGainAmount As Decimal


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cLossGainAmount = v_cLossGainAmount

            ' Update the write off reason id and amount
            sSQL = "UPDATE AllocationDetail SET " & _
                   "loss_gain_amount = " & CStr(cLossGainAmount) & " " & _
                   "WHERE AllocationDetail_ID = " & CStr(v_lAllocationDetailID)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="SetLossGainReason", bStoredProcedure:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetLossGain Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetLossGain", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

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


            ' Set database


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Initialisation Code.

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Set the ProcessMode etc.
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create Allocationdetails Collection
            m_oAllocationdetails = New Allocationdetails()

            ' Create PM Lookup Business Object
            m_oLookup = New bPMLookup.Business()

            ' Initialise PM Lookup Business passing our Database Reference.
            m_lReturn = m_oLookup.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFOrion


            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=sUsername, sPassword:=sPassword, iUserID:=iUserID, iSourceID:=iSourceID, iLanguageID:=iLanguageID, iCurrencyID:=iCurrencyID, iLogLevel:=iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oAllocationdetails.CurrencyConvert = m_oCurrencyConvert

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
                If m_oCurrencyConvert IsNot Nothing Then
                    m_oCurrencyConvert.Dispose()
                    m_oCurrencyConvert = Nothing
                End If
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                m_oAllocationdetails = Nothing
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
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a Allocationdetail.
    '
    '
    ' ***************************************************************** '
    Public Function GetLookupValues(ByRef iLookupType As Integer, ByRef vTableArray(,) As Object, ByRef iLanguageID As Integer, ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim oAllocationdetail As Allocationdetail = Nothing
        Dim dtEffectiveDate As Date

        Const CDocumentType As Integer = 1

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray As Array = Array.CreateInstance(GetType(Object), New Integer() {4, CDocumentType - CDocumentType + 1}, New Integer() {0, CDocumentType})
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            vResultArray = Nothing
            ' Reset Table Array

            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, CDocumentType) = gACTLibrary.ACTLookupDocumentType

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            If m_lCurrentRecord < 1 Then
                ' No, we can only lookup all
                iLookupType = gPMConstants.PMELookupType.PMLookupAll
            Else
                ' Yes get current record
                oAllocationdetail = m_oAllocationdetails.Item(m_lCurrentRecord)
            End If

            Select Case iLookupType
                Case gPMConstants.PMELookupType.PMLookupAll

                    ' Do not supply a key

                    vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CDocumentType) = ""
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

                Case gPMConstants.PMELookupType.PMLookupAllEffective

                    ' Use keys and effective date from current record
                    ' Note: The keys are not used for the select, but are used by
                    '       the iterface program to set the list index.
                    With oAllocationdetail

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CDocumentType) = .DocumenttypeID
                        dtEffectiveDate = DateTime.Now
                        ' {* USER DEFINED CODE (End) *}

                    End With

                Case gPMConstants.PMELookupType.PMLookupSingle

                    ' Set keys from current record
                    With oAllocationdetail

                        ' {* USER DEFINED CODE (Begin) *}

                        vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, CDocumentType) = .DocumenttypeID
                        ' {* USER DEFINED CODE (End) *}

                    End With
                    ' Default Effective Date to current date/time
                    dtEffectiveDate = DateTime.Now

            End Select

            ' Release Allocationdetail reference
            oAllocationdetail = Nothing

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

    ''' <summary>
    ''' DirectAdd
    ''' </summary>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <param name="r_crAllocAccountAmount"></param>
    ''' <param name="r_crAllocSystemAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DirectAdd(Optional ByRef vAllocationDetailID As Object = Nothing, _
                              Optional ByRef vCashlistitemID As Object = Nothing, _
                              Optional ByRef vAllocationId As Object = Nothing, _
                              Optional ByRef vOriginalCurrency As Object = Nothing, _
                              Optional ByRef vTransdetailID As Object = Nothing, _
                              Optional ByRef vDocumenttypeID As Object = Nothing, _
                              Optional ByRef vAccountingDate As Object = Nothing, _
                              Optional ByRef vDocumentRef As Object = Nothing, _
                              Optional ByRef vOriginalDate As Object = Nothing, _
                              Optional ByRef vAllocateToBase As Object = Nothing, _
                              Optional ByRef vOrigBaseAmount As Object = Nothing, _
                              Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, _
                              Optional ByRef vOrigCcyAmount As Object = Nothing, _
                              Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, _
                              Optional ByRef vOrigXrate As Object = Nothing, _
                              Optional ByRef vEffectiveXrate As Object = Nothing, _
                              Optional ByRef vOsBaseAmount As Object = Nothing, _
                              Optional ByRef vOsCcyAmount As Object = Nothing, _
                              Optional ByRef vAllocBaseAmount As Object = Nothing, _
                              Optional ByRef vAllocCcyAmount As Object = Nothing, _
                              Optional ByRef vFullyMatched As Object = Nothing, _
                              Optional ByRef vWriteOffReasonID As Object = Nothing, _
                              Optional ByRef vWriteOffAmount As Object = Nothing, _
                              Optional ByRef vNewOsBaseAmount As Object = Nothing, _
                              Optional ByRef vNewOsCcyAmount As Object = Nothing, _
                              Optional ByRef vLossGainAmount As Object = Nothing, _
                              Optional ByRef vIsPrimary As Object = Nothing, _
                              Optional ByRef vEuroCurrencyID As Object = Nothing, _
                              Optional ByRef vEuroAmount As Object = Nothing, _
                              Optional ByRef vEuroBaseXRate As Object = Nothing, _
                              Optional ByRef vEuroCcyXRate As Object = Nothing, _
                              Optional ByRef r_crAllocAccountAmount As Decimal = 0, _
                              Optional ByRef r_crAllocSystemAmount As Decimal = 0, _
                              Optional ByRef nTransdetailExId As Integer = 0) As Integer

        Dim nResult As Integer
        Dim oAllocationdetail As Allocationdetail

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Allocationdetail
            oAllocationdetail = New Allocationdetail()

            ' Populate Allocationdetail Attributes
            nResult = CType(SetProperties(oAllocationdetail, gPMConstants.PMEComponentAction.PMAdd, vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID, vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate, vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount, vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate, r_crAllocAccountAmount:=r_crAllocAccountAmount, r_crAllocSystemAmount:=r_crAllocSystemAmount, nTransdetailExID:=nTransdetailExId), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Add the Allocationdetail to the Database
            nResult = CType(AddItem(oAllocationdetail), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Return the ID of the Allocationdetail Added

            If Not Informations.IsNothing(vAllocationDetailID) Then
                vAllocationDetailID = oAllocationdetail.AllocationdetailID
            End If

            oAllocationdetail = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: DirectDelete (Public)
    '
    ' Description: Deletes a single Allocationdetail directly from the database.
    '        Note: The Allocationdetail will NOT be deleted from the collection.
    '
    ' ***************************************************************** '
    Public Function DirectDelete(Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As Object = Nothing, Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing, Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing, Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oAllocationdetail As Allocationdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create a new Allocationdetail
            oAllocationdetail = New Allocationdetail()

            ' Populate Allocationdetail Attributes






























            'm_lReturn = CType(SetProperties(oAllocationdetail, gPMConstants.PMEComponentAction.PMDelete, vAllocationDetailID:=CInt(vAllocationDetailID), vCashlistitemID:=CInt(vCashlistitemID), vAllocationId:=CInt(vAllocationId), vOriginalCurrency:=CInt(vOriginalCurrency), vTransdetailID:=CInt(vTransdetailID), vDocumenttypeID:=CInt(vDocumenttypeID), vAccountingDate:=vAccountingDate, vDocumentRef:=CStr(vDocumentRef), vOriginalDate:=vOriginalDate, vAllocateToBase:=CInt(vAllocateToBase), vOrigBaseAmount:=CDec(vOrigBaseAmount), vOrigBaseAmountUnrounded:=CByte(vOrigBaseAmountUnrounded), vOrigCcyAmount:=CDec(vOrigCcyAmount), vOrigCcyAmountUnrounded:=CByte(vOrigCcyAmountUnrounded), vOrigXrate:=CDbl(vOrigXrate), vEffectiveXrate:=CDbl(vEffectiveXrate), vOsBaseAmount:=CDec(vOsBaseAmount), vOsCcyAmount:=CDec(vOsCcyAmount), vAllocBaseAmount:=CDec(vAllocBaseAmount), vAllocCcyAmount:=CDec(vAllocCcyAmount), vFullyMatched:=CInt(vFullyMatched), vWriteOffReasonID:=CInt(vWriteOffReasonID), vWriteOffAmount:=CDec(vWriteOffAmount), vNewOsBaseAmount:=CDec(vNewOsBaseAmount), vNewOsCcyAmount:=CDec(vNewOsCcyAmount), vLossGainAmount:=CDec(vLossGainAmount), vIsPrimary:=CInt(vIsPrimary), vEuroCurrencyID:=CInt(vEuroCurrencyID), vEuroAmount:=CByte(vEuroAmount), vEuroBaseXRate:=CByte(vEuroBaseXRate), vEuroCcyXRate:=CByte(vEuroCcyXRate)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(SetProperties(oAllocationdetail, gPMConstants.PMEComponentAction.PMDelete, vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID, vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate, vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount, vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the Allocationdetail to the Database
            m_lReturn = CType(DeleteItem(oAllocationdetail), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            oAllocationdetail = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectDelete Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectDelete", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDefaults (Public)
    '
    ' Description: Returns the Default Values for the Allocationdetail.
    '
    ' ***************************************************************** '
    Public Function GetDefaults(Optional ByRef vSubType As Object = Nothing, Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As Object = Nothing, Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing, Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing, Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Defaults































            'Developer Guide No 98
            'm_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAllocationDetailID:=CByte(vAllocationDetailID), vCashlistitemID:=CByte(vCashlistitemID), vAllocationId:=CByte(vAllocationId), vOriginalCurrency:=CByte(vOriginalCurrency), vTransdetailID:=CByte(vTransdetailID), vDocumenttypeID:=CByte(vDocumenttypeID), vAccountingDate:=CDate(vAccountingDate), vDocumentRef:=CStr(vDocumentRef), vOriginalDate:=CDate(vOriginalDate), vAllocateToBase:=CByte(vAllocateToBase), vOrigBaseAmount:=CByte(vOrigBaseAmount), vOrigBaseAmountUnrounded:=CByte(vOrigBaseAmountUnrounded), vOrigCcyAmount:=CByte(vOrigCcyAmount), vOrigCcyAmountUnrounded:=CByte(vOrigCcyAmountUnrounded), vOrigXrate:=CByte(vOrigXrate), vEffectiveXrate:=CByte(vEffectiveXrate), vOsBaseAmount:=CByte(vOsBaseAmount), vOsCcyAmount:=CByte(vOsCcyAmount), vAllocBaseAmount:=CByte(vAllocBaseAmount), vAllocCcyAmount:=CByte(vAllocCcyAmount), vFullyMatched:=CByte(vFullyMatched), vWriteOffReasonID:=CByte(vWriteOffReasonID), vWriteOffAmount:=CByte(vWriteOffAmount), vNewOsBaseAmount:=CByte(vNewOsBaseAmount), vNewOsCcyAmount:=CByte(vNewOsCcyAmount), vLossGainAmount:=CByte(vLossGainAmount), vIsPrimary:=CByte(vIsPrimary), vEuroCurrencyID:=CByte(vEuroCurrencyID), vEuroAmount:=CByte(vEuroAmount), vEuroBaseXRate:=CByte(vEuroBaseXRate), vEuroCcyXRate:=CByte(vEuroCcyXRate)), gPMConstants.PMEReturnCode)
            m_lReturn = CType(DefaultParameters(bDefaultAll:=True, vSubType:=vSubType, vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID, vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate, vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount, vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate), gPMConstants.PMEReturnCode)

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
    ' Name: CheckID (Public)
    '
    ' Description: Checks to see if the supplied ID is a valid record.
    '
    ' ***************************************************************** '
    Public Function CheckID(ByRef vID As Object) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Add the ID parameter (INPUT)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="id", vValue:=CStr(vID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

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
    ' Name: GetCaptions (Public)
    '
    ' Description: Get the requested caption fields for a record.
    '
    ' ***************************************************************** '
    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object) As Integer
        Return GetCaptions(vID:=vID, vFieldArray:=vFieldArray, vResultArray:=vResultArray, vTable:=Nothing)
    End Function

    Public Function GetCaptions(ByRef vID As Object, ByRef vFieldArray() As Object, ByRef vResultArray() As Object, ByRef vTable As Object) As Integer

        Dim result As Integer = 0
        'Developer Guide No 21
        'Dim oFields As ADODB.Fields
        Dim oFields As ADODB.Fields

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Not Informations.IsArray(vFieldArray) Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogWarning, sMsg:="Parameter vFieldArray must be a Variant Array", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Do we have a Table name
            'BB If (IsMissing(vTable) = False) Then

            ' Is this our table
            '    If (Trim$(vTable) <> PMTableAllocationdetail) Then

            '        GetCaptions = PMInvalidRequest


            '       Exit Function

            '    End If

            'End If

            ' Get the Captions ourself

            ' Check that this record exists
            m_lReturn = CType(CheckID(vID:=vID), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Resize the Temporary Results Array
            Dim vResults(vFieldArray.GetUpperBound(0)) As Object

            ' Get a reference to the Fields returned
            oFields = m_oDatabase.Records.Item(1).Fields()

            With oFields

                ' For Each Field requested
                For lSub As Integer = vFieldArray.GetLowerBound(0) To vFieldArray.GetUpperBound(0)

                    'AK 230702 - check for null value

                    If Not (Convert.IsDBNull(oFields(vFieldArray(lSub))) Or Informations.IsNothing(oFields(vFieldArray(lSub)))) Then
                        ' Store the results in the Temporary results array

                        vResults(lSub) = oFields(vFieldArray(lSub))
                    Else
                        Select Case oFields(vFieldArray(lSub)).Type
                            'to do list
                            'Case DbType.String, DbType.String, DbType.String, adLongVarWChar, DbType.String, DbType.String, adWChar
                            Case DbType.String, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar, DbType.String, DbType.String, ADODB.DataTypeEnum.adLongVarWChar

                                vResults(lSub) = ""
                                'to do list
                                'Case DbType.Date, adDBDate
                            Case DbType.Date, ADODB.DataTypeEnum.adDBDate

                                vResults(lSub) = -1
                            Case Else

                                vResults(lSub) = 0
                        End Select
                    End If

                Next lSub

            End With

            ' Assign the results
            vResultArray = vResults

            ' Release the reference to the Fields
            oFields = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCaptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetCaptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetDetails (Public)
    '
    ' Description: Gets the required Allocationdetails and populate the Collection
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef vAllocationId As Object) As Integer
        Return GetDetails(vAllocationId:=vAllocationId, vAllocationDetailID:=Nothing, vLockMode:=0)
    End Function

    Public Function GetDetails(ByRef vAllocationId As Object, ByRef vAllocationDetailID As Object) As Integer
        Return GetDetails(vAllocationId:=vAllocationId, vAllocationDetailID:=vAllocationDetailID, vLockMode:=0)
    End Function

    Public Function GetDetails(ByRef vAllocationId As Object, ByRef vAllocationDetailID As Object, ByRef vLockMode As Integer) As Integer

        Dim result As Integer = 0
        Dim lRecordCount As Integer
        Dim oAllocationdetail As Allocationdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Collection
            m_oAllocationdetails.Clear()

            ' Set Current Record to zero
            m_lCurrentRecord = 0

            ' Clear the Database Parameters Collection
            m_oDatabase.Parameters.Clear()

            ' Default to No Lock if not supplied or not numeric
            Dim dbNumericTemp As Double

            If (Informations.IsNothing(vLockMode)) Or (Not Double.TryParse(CStr(vLockMode), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp)) Then
                vLockMode = gPMConstants.PMELockMode.PMNoLock
            End If

            ' Do we have a key

            If Not Informations.IsNothing(vAllocationDetailID) Then

                ' Yes, Is the key valid

                Dim dbNumericTemp2 As Double
                If Not Double.TryParse(CStr(vAllocationDetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then

                    result = gPMConstants.PMEReturnCode.PMFalse

                    ' Log Error Message

                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Key is not numeric : vAllocationdetailID =" & CStr(vAllocationDetailID), vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails")

                    Return result

                End If

                ' Add the AllocationdetailID parameter (INPUT)

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocationdetail_id", vValue:=CStr(vAllocationDetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDetailsSQL, sSQLName:=ACGetDetailsName, bStoredProcedure:=ACGetDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords)
                'AMJ 160802 changed to retrieve all records not the first 500
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            Else

                ' No Key, Get All Records for this allocation

                m_lReturn = m_oDatabase.Parameters.Add(sName:="allocation_id", vValue:=CStr(vAllocationId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'eck180102 Set lNumberRecords = -1 This should be All Records
                ' was 0 which is converted to 500
                ' Execute SQL Statement
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllDetailsSQL, sSQLName:=ACGetAllDetailsName, bStoredProcedure:=ACGetAllDetailsStored, lNumberRecords:=gPMConstants.PMAllRecords)
                'Tomo310102
                'ACGetAllDetailsSQL replaced ACGetDetailsSQL etc

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            ' How many records were selected
            lRecordCount = m_oDatabase.Records.Count()

            ' Do we have any records ?

            If lRecordCount < 1 Then

                ' No Records, return PMFalse
                Return gPMConstants.PMEReturnCode.PMNotFound

            Else

                ' Yes, load them into the collection

                For lSub As Integer = 1 To lRecordCount

                    ' Create New Allocationdetail
                    oAllocationdetail = New Allocationdetail()

                    m_lReturn = CType(SetPropertiesFromDB(oAllocationdetail:=oAllocationdetail, lRecordNumber:=lSub - 1), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    ' Add Allocationdetail to collection
                    If (m_oAllocationdetails.Count = 0) Then
                        m_oAllocationdetails.Add(Nothing)
                    End If
                    m_lReturn = CType(m_oAllocationdetails.Add(oNewAllocationdetail:=oAllocationdetail), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    oAllocationdetail = Nothing

                Next lSub

            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''' <summary>
    ''' Gets the required Allocationdetails and populate the Collectio
    ''' </summary>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <param name="r_crAllocAccountAmount"></param>
    ''' <param name="r_crAllocSystemAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetNext(Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing,
                            Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing,
                            Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing,
                            Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing,
                            Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As Object = Nothing,
                            Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing,
                            Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing,
                            Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing,
                            Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing,
                            Optional ByRef vAllocBaseAmount As Object = Nothing, Optional ByRef vAllocCcyAmount As Object = Nothing,
                            Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing,
                            Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing,
                            Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing,
                            Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing,
                            Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing,
                             Optional ByRef r_crAllocAccountAmount As Decimal = 0, Optional ByRef r_crAllocSystemAmount As Decimal = 0) As Integer

        Dim nResult As Integer
        Dim oAllocationdetail As Allocationdetail
        Dim nStatus As Integer

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Check to see that we are not at the end of the Collection
            If m_lCurrentRecord < m_oAllocationdetails.Count() Then
                ' Increment current record pointer
                m_lCurrentRecord += 1
            Else
                nResult = gPMConstants.PMEReturnCode.PMEOF
            End If

            oAllocationdetail = m_oAllocationdetails.Item(m_lCurrentRecord)

            ' Get the Allocationdetail Property Values
            m_lReturn = CType(GetProperties(oAllocationdetail, nStatus, vAllocationDetailID:=vAllocationDetailID,
                                            vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId,
                                            vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID,
                                            vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate,
                                            vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase,
                                            vOrigBaseAmount:=vOrigBaseAmount, vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded,
                                            vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded,
                                            vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount,
                                            vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount,
                                            vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount,
                                            vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount,
                                            vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount,
                                            vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate,
                                            r_crAllocAccountAmount:=r_crAllocAccountAmount, r_crAllocSystemAmount:=r_crAllocSystemAmount), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If

            oAllocationdetail = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetNext Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetNext", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Adds the supplied Allocationdetail into the Collection.After the Add, lKey should be equal to the number of items in the collection.
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <param name="r_crAllocAccountAmount"></param>
    ''' <param name="r_crAllocSystemAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditAdd(ByRef lRow As Integer, Optional ByRef vAllocationDetailID As Object = Nothing,
                            Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vAllocationId As Object = Nothing,
                            Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing,
                            Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing,
                            Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Object = Nothing,
                            Optional ByRef vAllocateToBase As Object = Nothing, Optional ByRef vOrigBaseAmount As Object = Nothing,
                            Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing,
                            Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, Optional ByRef vOrigXrate As Object = Nothing,
                            Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing,
                            Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing,
                            Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing,
                            Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing,
                            Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing,
                            Optional ByRef vLossGainAmount As Object = Nothing, Optional ByRef vIsPrimary As Object = Nothing,
                            Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing,
                            Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing,
                            Optional ByRef r_crAllocAccountAmount As Decimal = 0, Optional ByRef r_crAllocSystemAmount As Decimal = 0) As Integer

        Dim nResult As Integer
        Dim oAllocationdetail As Allocationdetail

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the number of items we have in the collection is the same as
            ' the Interface Form - 1 (because we havent added this one yet.)
            If m_oAllocationdetails.Count() <> (lRow - 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create a new Allocationdetail
            oAllocationdetail = New Allocationdetail()

            ' Populate Allocationdetail Attributes
            nResult = CType(SetProperties(oAllocationdetail, gPMConstants.PMEComponentAction.PMAdd, vAllocationDetailID:=vAllocationDetailID,
                                            vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency,
                                            vTransdetailID:=vTransdetailID, vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate,
                                            vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount,
                                            vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded,
                                            vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount,
                                            vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID,
                                            vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount,
                                            vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate,
                                             r_crAllocAccountAmount:=r_crAllocAccountAmount, r_crAllocSystemAmount:=r_crAllocSystemAmount), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                oAllocationdetail = Nothing
                Return nResult
            End If

            ' Add Allocationdetail to collection
            If (m_oAllocationdetails.Count = 0) Then
                m_oAllocationdetails.Add(Nothing)
            End If
            nResult = CType(m_oAllocationdetails.Add(oNewAllocationdetail:=oAllocationdetail), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                oAllocationdetail = Nothing
                Return nResult
            End If

            oAllocationdetail = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ''' <summary>
    ''' Validates that this action is valid on the Allocationdetail specified and updates the Allocationdetail with the new values.
    ''' </summary>
    ''' <param name="lRow"></param>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <param name="r_crAllocAccountAmount"></param>
    ''' <param name="r_crAllocSystemAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function EditUpdate(ByRef lRow As Integer, Optional ByRef vAllocationDetailID As Object = Nothing,
                               Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vAllocationId As Object = Nothing,
                               Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing,
                               Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing,
                               Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Object = Nothing,
                               Optional ByRef vAllocateToBase As Object = Nothing, Optional ByRef vOrigBaseAmount As Object = Nothing,
                               Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing,
                               Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, Optional ByRef vOrigXrate As Object = Nothing,
                               Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing,
                               Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing,
                               Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing,
                               Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing,
                               Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing,
                               Optional ByRef vLossGainAmount As Object = Nothing, Optional ByRef vIsPrimary As Object = Nothing,
                               Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing,
                               Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing,
                                Optional ByRef r_crAllocAccountAmount As Decimal = 0, Optional ByRef r_crAllocSystemAmount As Decimal = 0) As Integer

        Dim nResult As Integer
        Dim oAllocationdetail As Allocationdetail
        Dim nStatus As gPMConstants.PMEComponentAction

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAllocationdetails.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to Edit
            oAllocationdetail = m_oAllocationdetails.Item(lRow)

            ' Check the Status of the Allocationdetail

            Select Case oAllocationdetail.DatabaseStatus
                Case gPMConstants.PMEComponentAction.PMAdd
                    ' Leave Status as Add
                    nStatus = gPMConstants.PMEComponentAction.PMAdd
                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                    ' Error
                    nResult = gPMConstants.PMEReturnCode.PMFalse
                Case Else
                    ' Set Edit (Update) Status
                    nStatus = gPMConstants.PMEComponentAction.PMEdit
            End Select

            ' Update Allocationdetail Attributes

            nResult = CType(SetProperties(oAllocationdetail, nStatus, vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID,
                                            vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID,
                                            vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate, vDocumentRef:=vDocumentRef,
                                            vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount,
                                            vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded,
                                            vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount,
                                            vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched,
                                            vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount,
                                            vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID,
                                            vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate,
                                             r_crAllocAccountAmount:=r_crAllocAccountAmount, r_crAllocSystemAmount:=r_crAllocSystemAmount), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                oAllocationdetail = Nothing
                Return nResult
            End If

            ' Release reference to Allocationdetail
            oAllocationdetail = Nothing

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="EditUpdate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="EditUpdate", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: EditDelete (Public)
    '
    ' Description: Validate that the specified Allocationdetail can be deleted
    '              and mark accordingly.
    '
    ' ***************************************************************** '
    Public Function EditDelete(ByVal lRow As Integer) As Integer

        Dim result As Integer = 0
        Dim oAllocationdetail As Allocationdetail

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Validate that the row specified is valid. (i.e. Within the bounds of the Collection)
            If (lRow < 1) Or (lRow > m_oAllocationdetails.Count()) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a reference to the row to delete
            oAllocationdetail = m_oAllocationdetails.Item(lRow)

            ' Check the Status of the Allocationdetail

            'If status is Added (i.e. It is not in the database),
            'then set to Dummy Delete else set to Delete
            If oAllocationdetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMAdd Then
                oAllocationdetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMDummyDelete
            Else
                oAllocationdetail.DatabaseStatus = gPMConstants.PMEComponentAction.PMDelete
            End If

            ' Release reference to Allocationdetail
            oAllocationdetail = Nothing

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
    ' ***************************************************************** '
    Public Function Cancel() As Integer
        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For lSub As Integer = 1 To m_oAllocationdetails.Count()
                Select Case m_oAllocationdetails.Item(lSub).DatabaseStatus
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
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim lSub As Integer
        Dim oAllocationdetail As Allocationdetail
        Dim bTransStarted As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set the Transaction started flag to false
            bTransStarted = False

            ' Loop round Collection

            For lSub = 1 To m_oAllocationdetails.Count()
                oAllocationdetail = m_oAllocationdetails.Item(lSub)


                Select Case oAllocationdetail.DatabaseStatus
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
                        m_lReturn = CType(AddItem(oAllocationdetail), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(UpdateItem(oAllocationdetail), gPMConstants.PMEReturnCode)
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
                        m_lReturn = CType(DeleteItem(oAllocationdetail), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            Exit For
                        End If

                End Select

            Next lSub

            ' Release last reference
            oAllocationdetail = Nothing

            ' Check to see if we started a Transaction (i.e. Were any updates done)
            If bTransStarted Then

                ' Commit if OK, or Rollback any errors
                If result = gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    ' Refresh the Collection Items D/B Status
                    lSub = 1

                    ' For each item in the collection
                    Do While lSub <= m_oAllocationdetails.Count()

                        ' With the item
                        With m_oAllocationdetails.Item(lSub)


                            Select Case .DatabaseStatus
                                ' Delete or Dummy Delete
                                Case gPMConstants.PMEComponentAction.PMDelete, gPMConstants.PMEComponentAction.PMDummyDelete
                                    ' Remove from Collection
                                    m_oAllocationdetails.Delete(lSub)

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
    ' Name: AddItem (Public)
    '
    ' Description: Adds to the Database from the Base Details.
    '
    ' ***************************************************************** '
    Private Function AddItem(ByRef oAllocationdetail As Allocationdetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the required INPUT parameters
        m_lReturn = CType(AddInputParam(oAllocationdetail:=oAllocationdetail), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AllocationdetailID as an OUTPUT param for an insert
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocationdetail_id", vValue:=CStr(oAllocationdetail.AllocationdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute SQL Statement
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSQL, sSQLName:=ACAddName, bStoredProcedure:=ACAddStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the ID of the record inserted
        oAllocationdetail.AllocationdetailID = m_oDatabase.Parameters.Item("Allocationdetail_id").Value

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateItem (Public)
    '
    ' Description: UpdateItems a single record in the database from
    '              the Base Details.
    '
    ' ***************************************************************** '
    Private Function UpdateItem(ByRef oAllocationdetail As Allocationdetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the Input Parameters to the Parameteres collection
        m_lReturn = CType(AddInputParam(oAllocationdetail:=oAllocationdetail), gPMConstants.PMEReturnCode)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add AllocationdetailID as an INPUT param for an update
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocationdetail_id", vValue:=CStr(oAllocationdetail.AllocationdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

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

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: DeleteItem (Public)
    '
    ' Description: Deletes a single record from the database.
    '
    '
    ' ***************************************************************** '
    Private Function DeleteItem(ByRef oAllocationdetail As Allocationdetail) As Integer

        Dim result As Integer = 0
        Dim lRecordsAffected As Integer
        '  Const ACTKeyNameAllocationId As String = "allocation_id"




        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        ' Add the AllocationdetailID INPUT parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Allocationdetail_id", vValue:=CStr(oAllocationdetail.AllocationdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        'EK 230200
        '    ' Add the AllocationdetailID INPUT parameter
        '    m_lReturn& = m_oDatabase.Parameters.Add( _
        ''            sName:="Allocation_id", _
        ''            vValue:=oAllocationdetail.AllocationID, _
        ''            iDirection:=PMParamInput, _
        ''            iDataType:=PMLong)
        '
        '    If (m_lReturn& <> PMTrue) Then
        '        DeleteItem = PMFalse
        '        Exit Function
        '    End If

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
        'eck060900 Cannot delete here as the detail records  will not
        '          have actually gone
        ''EK 230200 Check whether Allocation needs to go too !
        ''   If no allocation details left
        '     ' Execute SQL Statement
        '    m_lReturn& = m_oDatabase.SQLAction( _
        ''        sSQL:=ACSelectSQL, _
        ''        sSQLName:=ACSelectName, _
        ''        bStoredProcedure:=ACSelectStored, _
        ''        lRecordsAffected:=lRecordsAffected)
        '
        '    If (m_lReturn& <> PMTrue) Then
        '        DeleteItem = PMFalse
        '        Exit Function
        '    End If
        '    If lRecordsAffected < 0 Then
        '         m_lReturn& = m_oDatabase.SQLAction( _
        ''            sSQL:=ACDeleteAllocationSQL, _
        ''            sSQLName:=ACDeleteAllocationName, _
        ''            bStoredProcedure:=ACDeleteAllocationStored, _
        ''            lRecordsAffected:=lRecordsAffected)
        '
        '        If (m_lReturn& <> PMTrue) Then
        '            DeleteItem = PMFalse
        '            Exit Function
        '        End If
        '         ' Create a new instance of component services
        '
        '        ' Update the temp storage value
        '        m_lReturn& = gPMComponentServices.UpdateUserProperty( _
        ''            v_sUserName:=m_sUsername, _
        ''            v_sPropertyName:=ACTKeyNameAllocationId, _
        ''            v_vPropertyValue:=0)
        '
        '        ' Remove the instance
        '
        '     End If
        Return result

    End Function

    ''' <summary>
    ''' Sets the supplied Allocationdetail properties from a database   record.
    ''' </summary>
    ''' <param name="oAllocationdetail"></param>
    ''' <param name="lRecordNumber"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetPropertiesFromDB(ByRef oAllocationdetail As Allocationdetail, ByRef lRecordNumber As Integer) As Integer

        Dim nResult As Integer
        Dim oFields As DataRow



        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Set oFields to refer to Field Collection for Record 1
        oFields = m_oDatabase.Records.Item(lRecordNumber).Fields()

        ' Populate Base Details

        With oAllocationdetail

            .AllocationdetailID = oFields("allocationdetail_id")

            If Convert.IsDBNull(oFields("cashlistitem_id")) Or Informations.IsNothing(oFields("cashlistitem_id")) Then
                .CashlistitemID = 0
            Else
                .CashlistitemID = oFields("cashlistitem_id")
            End If

            If Convert.IsDBNull(oFields("allocation_id")) Or Informations.IsNothing(oFields("allocation_id")) Then
                .AllocationID = 0
            Else
                .AllocationID = oFields("allocation_id")
            End If

            If Convert.IsDBNull(oFields("original_currency")) Or Informations.IsNothing(oFields("original_currency")) Then
                .OriginalCurrency = 0
            Else
                .OriginalCurrency = oFields("original_currency")
            End If

            If Convert.IsDBNull(oFields("transdetail_id")) Or Informations.IsNothing(oFields("transdetail_id")) Then
                .TransdetailID = 0
            Else
                .TransdetailID = oFields("transdetail_id")
            End If

            If Convert.IsDBNull(oFields("documenttype_id")) Or Informations.IsNothing(oFields("documenttype_id")) Then
                .DocumenttypeID = 0
            Else
                .DocumenttypeID = oFields("documenttype_id")
            End If

            If Convert.IsDBNull(oFields("accounting_date")) Or Informations.IsNothing(oFields("accounting_date")) Then
                .AccountingDate = #12/30/1899#
            Else
                .AccountingDate = oFields("accounting_date")
            End If

            If Convert.IsDBNull(oFields("document_ref")) Or Informations.IsNothing(oFields("document_ref")) Then
                .DocumentRef = ""
            Else
                .DocumentRef = oFields("document_ref")
            End If

            If Convert.IsDBNull(oFields("original_date")) Or Informations.IsNothing(oFields("original_date")) Then
                .OriginalDate = #12/30/1899#
            Else
                .OriginalDate = oFields("original_date")
            End If

            If Convert.IsDBNull(oFields("allocate_to_base")) Or Informations.IsNothing(oFields("allocate_to_base")) Then
                .AllocateToBase = 0
            Else
                .AllocateToBase = oFields("allocate_to_base")
            End If

            If Convert.IsDBNull(oFields("orig_base_amount")) Or Informations.IsNothing(oFields("orig_base_amount")) Then
                .OrigBaseAmount = 0
            Else
                .OrigBaseAmount = oFields("orig_base_amount")
            End If

            If Convert.IsDBNull(oFields("orig_base_amount_unrounded")) Or Informations.IsNothing(oFields("orig_base_amount_unrounded")) Then
                .OrigBaseAmountUnrounded = 0
            Else
                .OrigBaseAmountUnrounded = oFields("orig_base_amount_unrounded")
            End If

            If Convert.IsDBNull(oFields("orig_ccy_amount")) Or Informations.IsNothing(oFields("orig_ccy_amount")) Then
                .OrigCcyAmount = 0
            Else
                .OrigCcyAmount = oFields("orig_ccy_amount")
            End If

            If Convert.IsDBNull(oFields("orig_ccy_amount_unrounded")) Or Informations.IsNothing(oFields("orig_ccy_amount_unrounded")) Then
                .OrigCcyAmountUnrounded = 0
            Else
                .OrigCcyAmountUnrounded = oFields("orig_ccy_amount_unrounded")
            End If

            If Convert.IsDBNull(oFields("orig_xrate")) Or Informations.IsNothing(oFields("orig_xrate")) Then
                .OrigXrate = 0
            Else
                .OrigXrate = oFields("orig_xrate")
            End If

            If Convert.IsDBNull(oFields("effective_xrate")) Or Informations.IsNothing(oFields("effective_xrate")) Then
                .EffectiveXrate = 0
            Else
                .EffectiveXrate = oFields("effective_xrate")
            End If

            If Convert.IsDBNull(oFields("os_base_amount")) Or Informations.IsNothing(oFields("os_base_amount")) Then
                .OsBaseAmount = 0
            Else
                .OsBaseAmount = oFields("os_base_amount")
            End If

            If Convert.IsDBNull(oFields("os_ccy_amount")) Or Informations.IsNothing(oFields("os_ccy_amount")) Then
                .OsCcyAmount = 0
            Else
                .OsCcyAmount = oFields("os_ccy_amount")
            End If

            If Convert.IsDBNull(oFields("alloc_base_amount")) Or Informations.IsNothing(oFields("alloc_base_amount")) Then
                .AllocBaseAmount = 0
            Else
                .AllocBaseAmount = oFields("alloc_base_amount")
            End If

            If Convert.IsDBNull(oFields("alloc_ccy_amount")) Or Informations.IsNothing(oFields("alloc_ccy_amount")) Then
                .AllocCcyAmount = 0
            Else
                .AllocCcyAmount = oFields("alloc_ccy_amount")
            End If

            If Convert.IsDBNull(oFields("fully_matched")) Or Informations.IsNothing(oFields("fully_matched")) Then
                .FullyMatched = 0
            Else
                .FullyMatched = oFields("fully_matched")
            End If

            'write_off_reason_id
            If Convert.IsDBNull(oFields("write_off_reason_id")) Or Informations.IsNothing(oFields("write_off_reason_id")) Then
                .WriteOffReasonID = 0
            Else
                .WriteOffReasonID = oFields("write_off_reason_id")
            End If

            If Convert.IsDBNull(oFields("write_off_amount")) Or Informations.IsNothing(oFields("write_off_amount")) Then
                .WriteOffAmount = 0
            Else
                .WriteOffAmount = oFields("write_off_amount")
            End If

            If Convert.IsDBNull(oFields("new_os_ccy_amount")) Or Informations.IsNothing(oFields("new_os_ccy_amount")) Then
                .NewOsCcyAmount = 0
            Else
                .NewOsCcyAmount = oFields("new_os_ccy_amount")
            End If

            If Convert.IsDBNull(oFields("new_os_base_amount")) Or Informations.IsNothing(oFields("new_os_base_amount")) Then
                .NewOsBaseAmount = 0
            Else
                .NewOsBaseAmount = oFields("new_os_base_amount")
            End If

            If Convert.IsDBNull(oFields("loss_gain_amount")) Or Informations.IsNothing(oFields("loss_gain_amount")) Then
                .LossGainAmount = 0
            Else
                .LossGainAmount = oFields("loss_gain_amount")
            End If

            If Convert.IsDBNull(oFields("is_primary")) Or Informations.IsNothing(oFields("is_primary")) Then
                .IsPrimary = 0
            Else
                .IsPrimary = oFields("is_primary")
            End If

            If Convert.IsDBNull(oFields("euro_currency_id")) Or Informations.IsNothing(oFields("euro_currency_id")) Then
                .EuroCurrencyID = 0
            Else
                .EuroCurrencyID = oFields("euro_currency_id")
            End If

            If Convert.IsDBNull(oFields("euro_amount")) Or Informations.IsNothing(oFields("euro_amount")) Then
                .EuroAmount = 0
            Else
                .EuroAmount = oFields("euro_amount")
            End If

            If Convert.IsDBNull(oFields("euro_base_xrate")) Or Informations.IsNothing(oFields("euro_base_xrate")) Then
                .EuroBaseXRate = 0
            Else
                .EuroBaseXRate = oFields("euro_base_xrate")
            End If

            If Convert.IsDBNull(oFields("euro_ccy_xrate")) Or Informations.IsNothing(oFields("euro_ccy_xrate")) Then
                .EuroCcyXRate = 0
            Else
                .EuroCcyXRate = oFields("euro_ccy_xrate")
            End If

            .DatabaseStatus = gPMConstants.PMEComponentAction.PMView

            If Convert.IsDBNull(oFields("alloc_account_amount")) Or Informations.IsNothing(oFields("alloc_account_amount")) Then
                .AllocAccountAmount = 0
            Else
                .AllocAccountAmount = oFields("alloc_account_amount")
            End If

            If Convert.IsDBNull(oFields("alloc_system_amount")) Or Informations.IsNothing(oFields("alloc_system_amount")) Then
                .AllocSystemAmount = 0
            Else
                .AllocSystemAmount = oFields("alloc_system_amount")
            End If

            If Convert.IsDBNull(oFields("transdetail_id")) Or Informations.IsNothing(oFields("transdetail_id")) Then
                .AllocSystemAmount = 0
            Else
                .AllocSystemAmount = oFields("transdetail_id")
            End If


        End With

        Return nResult

    End Function

    ''' <summary>
    ''' SetProperties
    ''' </summary>
    ''' <param name="oAllocationdetail"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <param name="r_crAllocAccountAmount"></param>
    ''' <param name="r_crAllocSystemAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetProperties(ByRef oAllocationdetail As Allocationdetail, ByRef iStatus As Integer,
                                Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing,
                                Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing,
                                Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing,
                                Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing,
                                Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As gPMConstants.PMEReturnCode = 0,
                                Optional ByRef vOrigBaseAmount As Decimal = 0, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing,
                                Optional ByRef vOrigCcyAmount As Decimal = 0, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing,
                                Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing,
                                Optional ByRef vOsBaseAmount As Decimal = 0, Optional ByRef vOsCcyAmount As Decimal = 0,
                                Optional ByRef vAllocBaseAmount As Decimal = 0, Optional ByRef vAllocCcyAmount As Decimal = 0,
                                Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing,
                                Optional ByRef vWriteOffAmount As Decimal = 0, Optional ByRef vNewOsBaseAmount As Decimal = 0,
                                Optional ByRef vNewOsCcyAmount As Decimal = 0, Optional ByRef vLossGainAmount As Decimal = 0,
                                Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing,
                                Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing,
                                Optional ByRef r_crAllocAccountAmount As Decimal = 0, Optional ByRef r_crAllocSystemAmount As Decimal = 0,
                                Optional ByRef nTransdetailExID As Integer = 0) As Integer


        Dim nResult As Integer
        Dim bDataChanged As Boolean



        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' If Add Mode
        If iStatus = gPMConstants.PMEComponentAction.PMAdd Then

            ' Check that All Mandatory Parameters have been supplied
            nResult = CType(CheckMandatory(vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID, vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate, vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount, vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Default Any Missing Parameters
            nResult = CType(DefaultParameters(bDefaultAll:=False, vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID,
                                              vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID,
                                              vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=CDate(vAccountingDate), vDocumentRef:=vDocumentRef,
                                              vOriginalDate:=CDate(vOriginalDate), vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount,
                                              vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount,
                                              vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate,
                                              vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount,
                                              vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID,
                                              vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount,
                                              vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID,
                                              vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate,
                                              crAllocAccountAmount:=r_crAllocAccountAmount, crAllocSystemAmount:=r_crAllocSystemAmount, nTransdetailExID:=nTransdetailExID), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

        End If

        ' Validate Parameters
        nResult = CType(Validate(vAllocationDetailID:=vAllocationDetailID, vCashlistitemID:=vCashlistitemID, vAllocationId:=vAllocationId, vOriginalCurrency:=vOriginalCurrency, vTransdetailID:=vTransdetailID, vDocumenttypeID:=vDocumenttypeID, vAccountingDate:=vAccountingDate, vDocumentRef:=vDocumentRef, vOriginalDate:=vOriginalDate, vAllocateToBase:=vAllocateToBase, vOrigBaseAmount:=vOrigBaseAmount, vOrigBaseAmountUnrounded:=vOrigBaseAmountUnrounded, vOrigCcyAmount:=vOrigCcyAmount, vOrigCcyAmountUnrounded:=vOrigCcyAmountUnrounded, vOrigXrate:=vOrigXrate, vEffectiveXrate:=vEffectiveXrate, vOsBaseAmount:=vOsBaseAmount, vOsCcyAmount:=vOsCcyAmount, vAllocBaseAmount:=vAllocBaseAmount, vAllocCcyAmount:=vAllocCcyAmount, vFullyMatched:=vFullyMatched, vWriteOffReasonID:=vWriteOffReasonID, vWriteOffAmount:=vWriteOffAmount, vNewOsBaseAmount:=vNewOsBaseAmount, vNewOsCcyAmount:=vNewOsCcyAmount, vLossGainAmount:=vLossGainAmount, vIsPrimary:=vIsPrimary, vEuroCurrencyID:=vEuroCurrencyID, vEuroAmount:=vEuroAmount, vEuroBaseXRate:=vEuroBaseXRate, vEuroCcyXRate:=vEuroCcyXRate), gPMConstants.PMEReturnCode)

        If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
            Return nResult
        End If

        ' Set Data Changed Flag to False
        bDataChanged = False

        ' Set Property values.
        With oAllocationdetail

            If Not Informations.IsNothing(vAllocationDetailID) Then
                If .AllocationdetailID <> vAllocationDetailID Then
                    .AllocationdetailID = vAllocationDetailID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vCashlistitemID) Then
                If .CashlistitemID <> vCashlistitemID Then
                    .CashlistitemID = vCashlistitemID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAllocationId) Then
                If .AllocationID <> vAllocationId Then
                    .AllocationID = vAllocationId
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOriginalCurrency) Then
                If .OriginalCurrency <> vOriginalCurrency Then
                    .OriginalCurrency = vOriginalCurrency
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vTransdetailID) Then
                If .TransdetailID <> vTransdetailID Then
                    .TransdetailID = vTransdetailID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDocumenttypeID) Then
                If .DocumenttypeID <> vDocumenttypeID Then
                    .DocumenttypeID = vDocumenttypeID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAccountingDate) Then

                If .AccountingDate <> CDate(vAccountingDate) Then

                    .AccountingDate = CDate(vAccountingDate)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vDocumentRef) Then
                If .DocumentRef.Trim() <> vDocumentRef.Trim() Then
                    .DocumentRef = vDocumentRef
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOriginalDate) Then

                If .OriginalDate <> CDate(vOriginalDate) Then

                    .OriginalDate = CDate(vOriginalDate)
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAllocateToBase) Then
                If .AllocateToBase <> vAllocateToBase Then
                    .AllocateToBase = vAllocateToBase
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOrigBaseAmount) Then
                If .OrigBaseAmount <> vOrigBaseAmount Then
                    .OrigBaseAmount = vOrigBaseAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOrigBaseAmountUnrounded) Then
                If (.OrigBaseAmountUnrounded <> vOrigBaseAmountUnrounded) OrElse Informations.IsNothing(.OrigBaseAmountUnrounded) Then
                    .OrigBaseAmountUnrounded = vOrigBaseAmountUnrounded
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOrigCcyAmount) Then
                If .OrigCcyAmount <> vOrigCcyAmount Then
                    .OrigCcyAmount = vOrigCcyAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOrigCcyAmountUnrounded) Then

                If (.OrigCcyAmountUnrounded <> vOrigCcyAmountUnrounded) OrElse Informations.IsNothing(.OrigCcyAmountUnrounded) Then
                    .OrigCcyAmountUnrounded = vOrigCcyAmountUnrounded
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOrigXrate) Then
                If Convert.ToDecimal(.OrigXrate) <> Convert.ToDecimal(vOrigXrate) Then
                    .OrigXrate = vOrigXrate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEffectiveXrate) Then
                If Convert.ToDecimal(.EffectiveXrate) <> Convert.ToDecimal(vEffectiveXrate) Then
                    .EffectiveXrate = vEffectiveXrate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOsBaseAmount) Then
                If .OsBaseAmount <> vOsBaseAmount Then
                    .OsBaseAmount = vOsBaseAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vOsCcyAmount) Then
                If .OsCcyAmount <> vOsCcyAmount Then
                    .OsCcyAmount = vOsCcyAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAllocBaseAmount) Then
                If .AllocBaseAmount <> vAllocBaseAmount Then
                    .AllocBaseAmount = vAllocBaseAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vAllocCcyAmount) Then
                If .AllocCcyAmount <> vAllocCcyAmount Then
                    .AllocCcyAmount = vAllocCcyAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vFullyMatched) Then
                If .FullyMatched <> vFullyMatched Then
                    .FullyMatched = vFullyMatched
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vWriteOffReasonID) Then
                If .WriteOffReasonID <> vWriteOffReasonID Then
                    .WriteOffReasonID = vWriteOffReasonID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vWriteOffAmount) Then
                If .WriteOffAmount <> vWriteOffAmount Then
                    .WriteOffAmount = vWriteOffAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vNewOsCcyAmount) Then
                If .NewOsCcyAmount <> vNewOsCcyAmount Then
                    .NewOsCcyAmount = vNewOsCcyAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vNewOsBaseAmount) Then
                If .NewOsBaseAmount <> vNewOsBaseAmount Then
                    .NewOsBaseAmount = vNewOsBaseAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vLossGainAmount) Then
                If .LossGainAmount <> vLossGainAmount Then
                    .LossGainAmount = vLossGainAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vIsPrimary) Then
                If .IsPrimary <> vIsPrimary Then
                    .IsPrimary = vIsPrimary
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroCurrencyID) Then
                If .EuroCurrencyID <> vEuroCurrencyID Then
                    .EuroCurrencyID = vEuroCurrencyID
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroAmount) Then
                If (.EuroAmount <> vEuroAmount) OrElse Informations.IsNothing(.EuroAmount) Then
                    .EuroAmount = vEuroAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroBaseXRate) Then

                If (.EuroBaseXRate <> vEuroBaseXRate) OrElse (Informations.IsNothing(.EuroBaseXRate)) Then
                    .EuroBaseXRate = vEuroBaseXRate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(vEuroCcyXRate) Then

                If (.EuroCcyXRate <> vEuroCcyXRate) OrElse (Informations.IsNothing(.EuroCcyXRate)) Then
                    .EuroCcyXRate = vEuroCcyXRate
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(r_crAllocAccountAmount) Then
                If (.AllocAccountAmount <> r_crAllocAccountAmount) Then
                    .AllocAccountAmount = r_crAllocAccountAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(r_crAllocSystemAmount) Then
                If (.AllocSystemAmount <> r_crAllocSystemAmount) Then
                    .AllocSystemAmount = r_crAllocSystemAmount
                    bDataChanged = True
                End If
            End If

            If Not Informations.IsNothing(nTransdetailExID) Then
                If (.TransdetailExID <> nTransdetailExID) Then
                    .TransdetailExID = nTransdetailExID
                    bDataChanged = True
                End If
            End If

            ' If we have changed one of the properties, update the status
            If bDataChanged Then
                .DatabaseStatus = iStatus
            End If

        End With

        Return nResult

    End Function

    ''' <summary>
    ''' Returns the supplied Allocationdetail property values.
    ''' </summary>
    ''' <param name="oAllocationdetail"></param>
    ''' <param name="iStatus"></param>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <param name="r_crAllocAccountAmount"></param>
    ''' <param name="r_crAllocSystemAmount"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function GetProperties(ByRef oAllocationdetail As Allocationdetail, ByRef iStatus As Integer,
                                   Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing,
                                   Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing,
                                   Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing,
                                   Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing,
                                   Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As Object = Nothing,
                                   Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing,
                                   Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing,
                                   Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing,
                                   Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing,
                                   Optional ByRef vAllocBaseAmount As Object = Nothing, Optional ByRef vAllocCcyAmount As Object = Nothing,
                                   Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing,
                                   Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing,
                                   Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing,
                                   Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing,
                                   Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing,
                                   Optional ByRef r_crAllocAccountAmount As Decimal = 0, Optional ByRef r_crAllocSystemAmount As Decimal = 0) As Integer


        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' Set Property values.
        With oAllocationdetail

            vAllocationDetailID = .AllocationdetailID
            vCashlistitemID = .CashlistitemID
            vAllocationId = .AllocationID
            vOriginalCurrency = .OriginalCurrency
            vTransdetailID = .TransdetailID
            vDocumenttypeID = .DocumenttypeID
            vAccountingDate = .AccountingDate
            vDocumentRef = .DocumentRef
            vOriginalDate = .OriginalDate
            vAllocateToBase = .AllocateToBase
            vOrigBaseAmount = .OrigBaseAmount
            vOrigBaseAmountUnrounded = .OrigBaseAmountUnrounded
            vOrigCcyAmount = .OrigCcyAmount
            vOrigCcyAmountUnrounded = .OrigCcyAmountUnrounded
            vOrigXrate = .OrigXrate
            vEffectiveXrate = .EffectiveXrate
            vOsBaseAmount = .OsBaseAmount
            vOsCcyAmount = .OsCcyAmount
            vAllocBaseAmount = .AllocBaseAmount
            vAllocCcyAmount = .AllocCcyAmount
            vFullyMatched = .FullyMatched
            vWriteOffReasonID = .WriteOffReasonID
            vWriteOffAmount = .WriteOffAmount
            vNewOsCcyAmount = .NewOsCcyAmount
            vNewOsBaseAmount = .NewOsBaseAmount
            vLossGainAmount = .LossGainAmount
            vIsPrimary = .IsPrimary
            vEuroCurrencyID = .EuroCurrencyID
            vEuroAmount = .EuroAmount
            vEuroBaseXRate = .EuroBaseXRate
            vEuroCcyXRate = .EuroCcyXRate
            iStatus = .DatabaseStatus
            r_crAllocAccountAmount = .AllocAccountAmount
            r_crAllocSystemAmount = .AllocSystemAmount

        End With

        Return nResult

    End Function

    ''' <summary>
    ''' Adds all of the INPUT parameters required for an Insert or Update.
    ''' </summary>
    ''' <param name="oAllocationdetail"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddInputParam(ByRef oAllocationdetail As Allocationdetail) As Integer

        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            If oAllocationdetail.CashlistitemID < 1 Then

                nResult = .Parameters.Add(sName:="cashlistitem_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                nResult = .Parameters.Add(sName:="cashlistitem_id", vValue:=CStr(oAllocationdetail.CashlistitemID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If oAllocationdetail.AllocationID < 1 Then

                nResult = .Parameters.Add(sName:="allocation_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                nResult = .Parameters.Add(sName:="allocation_id", vValue:=CStr(oAllocationdetail.AllocationID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="original_currency", vValue:=CStr(oAllocationdetail.OriginalCurrency), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If oAllocationdetail.TransdetailID < 1 Then

                nResult = .Parameters.Add(sName:="transdetail_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                nResult = .Parameters.Add(sName:="transdetail_id", vValue:=CStr(oAllocationdetail.TransdetailID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If oAllocationdetail.DocumenttypeID < 1 Then

                nResult = .Parameters.Add(sName:="documenttype_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                nResult = .Parameters.Add(sName:="documenttype_id", vValue:=CStr(oAllocationdetail.DocumenttypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="accounting_date", vValue:=oAllocationdetail.AccountingDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="document_ref", vValue:=oAllocationdetail.DocumentRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = .Parameters.Add(sName:="original_date", vValue:=oAllocationdetail.OriginalDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="allocate_to_base", vValue:=CStr(oAllocationdetail.AllocateToBase), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="orig_base_amount", vValue:=CStr(oAllocationdetail.OrigBaseAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="orig_base_amount_unrounded", vValue:=CStr(oAllocationdetail.OrigBaseAmountUnrounded), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="orig_ccy_amount", vValue:=CStr(oAllocationdetail.OrigCcyAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="orig_ccy_amount_unrounded", vValue:=CStr(oAllocationdetail.OrigCcyAmountUnrounded), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="orig_xrate", vValue:=CStr(oAllocationdetail.OrigXrate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="effective_xrate", vValue:=CStr(oAllocationdetail.EffectiveXrate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="os_base_amount", vValue:=CStr(oAllocationdetail.OsBaseAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="os_ccy_amount", vValue:=CStr(oAllocationdetail.OsCcyAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="alloc_base_amount", vValue:=CStr(oAllocationdetail.AllocBaseAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="alloc_ccy_amount", vValue:=CStr(oAllocationdetail.AllocCcyAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="fully_matched", vValue:=CStr(oAllocationdetail.FullyMatched), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If oAllocationdetail.WriteOffReasonID = 0 Then

                nResult = .Parameters.Add(sName:="write_off_reason_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                nResult = .Parameters.Add(sName:="write_off_reason_id", vValue:=CStr(oAllocationdetail.WriteOffReasonID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            If oAllocationdetail.WriteOffAmount = 0 Then

                nResult = .Parameters.Add(sName:="write_off_amount", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            Else
                nResult = .Parameters.Add(sName:="write_off_amount", vValue:=CStr(oAllocationdetail.WriteOffAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)
            End If

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="new_os_ccy_amount", vValue:=CStr(oAllocationdetail.NewOsCcyAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="new_os_base_amount", vValue:=CStr(oAllocationdetail.NewOsBaseAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="loss_gain_amount", vValue:=CStr(oAllocationdetail.LossGainAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="is_primary", vValue:=CStr(oAllocationdetail.IsPrimary), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="euro_currency_id", vValue:=CStr(oAllocationdetail.EuroCurrencyID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="euro_amount", vValue:=CStr(oAllocationdetail.EuroAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="euro_base_xrate", vValue:=CStr(oAllocationdetail.EuroBaseXRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="euro_ccy_xrate", vValue:=CStr(oAllocationdetail.EuroCcyXRate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="crAlloc_account_amount", vValue:=CStr(oAllocationdetail.AllocAccountAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            nResult = .Parameters.Add(sName:="crAlloc_system_amount", vValue:=CStr(oAllocationdetail.AllocSystemAmount), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If
            nResult = .Parameters.Add(sName:="transdetailex_id", vValue:=CStr(oAllocationdetail.TransdetailExID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

        End With

        Return nResult

    End Function
    ''' <summary>
    ''' default parameters
    ''' </summary>
    ''' <param name="bDefaultAll"></param>
    ''' <param name="vSubType"></param>
    ''' <param name="vAllocationDetailID"></param>
    ''' <param name="vCashlistitemID"></param>
    ''' <param name="vAllocationId"></param>
    ''' <param name="vOriginalCurrency"></param>
    ''' <param name="vTransdetailID"></param>
    ''' <param name="vDocumenttypeID"></param>
    ''' <param name="vAccountingDate"></param>
    ''' <param name="vDocumentRef"></param>
    ''' <param name="vOriginalDate"></param>
    ''' <param name="vAllocateToBase"></param>
    ''' <param name="vOrigBaseAmount"></param>
    ''' <param name="vOrigBaseAmountUnrounded"></param>
    ''' <param name="vOrigCcyAmount"></param>
    ''' <param name="vOrigCcyAmountUnrounded"></param>
    ''' <param name="vOrigXrate"></param>
    ''' <param name="vEffectiveXrate"></param>
    ''' <param name="vOsBaseAmount"></param>
    ''' <param name="vOsCcyAmount"></param>
    ''' <param name="vAllocBaseAmount"></param>
    ''' <param name="vAllocCcyAmount"></param>
    ''' <param name="vFullyMatched"></param>
    ''' <param name="vWriteOffReasonID"></param>
    ''' <param name="vWriteOffAmount"></param>
    ''' <param name="vNewOsBaseAmount"></param>
    ''' <param name="vNewOsCcyAmount"></param>
    ''' <param name="vLossGainAmount"></param>
    ''' <param name="vIsPrimary"></param>
    ''' <param name="vEuroCurrencyID"></param>
    ''' <param name="vEuroAmount"></param>
    ''' <param name="vEuroBaseXRate"></param>
    ''' <param name="vEuroCcyXRate"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DefaultParameters(ByRef bDefaultAll As Boolean, Optional ByRef vSubType As Object = Nothing, Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing,
                                       Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing,
                                       Optional ByRef vAccountingDate As Date = #12/30/1899#, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Date = #12/30/1899#, Optional ByRef vAllocateToBase As Object = Nothing,
                                       Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing,
                                       Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing,
                                       Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing,
                                       Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing,
                                       Optional ByRef crAllocAccountAmount As Decimal = 0, Optional ByRef crAllocSystemAmount As Decimal = 0, Optional ByRef nTransdetailExID As Integer = 0) As Integer


        Dim nResult As Integer


        nResult = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}

        If (Informations.IsNothing(vAllocationDetailID)) Or (vAllocationDetailID.Equals(0)) Or (bDefaultAll) Then
            vAllocationDetailID = 0
        End If

        If (Informations.IsNothing(vCashlistitemID)) OrElse Informations.IsNothing(vCashlistitemID) Or (bDefaultAll) Then
            vCashlistitemID = 0
        End If

        If (Informations.IsNothing(vAllocationId)) Or (vAllocationId.Equals(0)) Or (bDefaultAll) Then
            vAllocationId = 0
        End If

        If (Informations.IsNothing(vOriginalCurrency)) Or (vOriginalCurrency.Equals(0)) Or (bDefaultAll) Then
            vOriginalCurrency = 0
        End If

        If (Informations.IsNothing(vTransdetailID)) Or (vTransdetailID.Equals(0)) Or (bDefaultAll) Then
            vTransdetailID = 0
        End If

        If (Informations.IsNothing(vDocumenttypeID)) Or (vDocumenttypeID.Equals(0)) Or (bDefaultAll) Then
            vDocumenttypeID = 0
        End If

        If (Informations.IsNothing(vAccountingDate)) Or (vAccountingDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vAccountingDate = DateTime.Now
        End If

        If (Informations.IsNothing(vDocumentRef)) Or (String.IsNullOrEmpty(vDocumentRef)) Or (bDefaultAll) Then
            vDocumentRef = ""
        End If

        If (Informations.IsNothing(vOriginalDate)) Or (vOriginalDate.Equals(DateTime.FromOADate(0))) Or (bDefaultAll) Then
            vOriginalDate = DateTime.Now
        End If

        If (Informations.IsNothing(vAllocateToBase)) Or (vAllocateToBase.Equals(0)) Or (bDefaultAll) Then
            vAllocateToBase = 0
        End If

        If (Informations.IsNothing(vOrigBaseAmount)) Or (vOrigBaseAmount.Equals(0)) Or (bDefaultAll) Then
            vOrigBaseAmount = 0
        End If

        If (Informations.IsNothing(vOrigBaseAmountUnrounded)) Or (vOrigBaseAmountUnrounded.Equals(0)) Or (bDefaultAll) Then
            vOrigBaseAmountUnrounded = 0
        End If

        If (Informations.IsNothing(vOrigCcyAmount)) Or (vOrigCcyAmount.Equals(0)) Or (bDefaultAll) Then
            vOrigCcyAmount = 0
        End If

        If (Informations.IsNothing(vOrigCcyAmountUnrounded)) Or (vOrigCcyAmountUnrounded.Equals(0)) Or (bDefaultAll) Then
            vOrigCcyAmountUnrounded = 0
        End If

        If (Informations.IsNothing(vOrigXrate)) Or (vOrigXrate.Equals(0)) Or (bDefaultAll) Then
            vOrigXrate = 1
        End If

        If (Informations.IsNothing(vEffectiveXrate)) OrElse (vEffectiveXrate.Equals(0)) Or (bDefaultAll) Then
            vEffectiveXrate = 1
        End If

        If (Informations.IsNothing(vOsBaseAmount)) Or (vOsBaseAmount.Equals(0)) Or (bDefaultAll) Then
            vOsBaseAmount = 0
        End If

        If (Informations.IsNothing(vOsCcyAmount)) Or (vOsCcyAmount.Equals(0)) Or (bDefaultAll) Then
            vOsCcyAmount = 0
        End If

        If (Informations.IsNothing(vAllocBaseAmount)) Or (vAllocBaseAmount.Equals(0)) Or (bDefaultAll) Then
            vAllocBaseAmount = 0
        End If

        If (Informations.IsNothing(vAllocCcyAmount)) Or (vAllocCcyAmount.Equals(0)) Or (bDefaultAll) Then
            vAllocCcyAmount = 0
        End If

        If (Informations.IsNothing(vFullyMatched)) Or (vFullyMatched.Equals(0)) Or (bDefaultAll) Then
            vFullyMatched = 0
        End If
        If (Informations.IsNothing(vWriteOffReasonID)) OrElse (vWriteOffReasonID.Equals(0)) Or (bDefaultAll) Then
            vWriteOffReasonID = 0
        End If

        If (Informations.IsNothing(vWriteOffAmount)) Or (vWriteOffAmount.Equals(0)) Or (bDefaultAll) Then
            vWriteOffAmount = 0
        End If

        If (Informations.IsNothing(vNewOsCcyAmount)) Or (vNewOsCcyAmount.Equals(0)) Or (bDefaultAll) Then
            vNewOsCcyAmount = 0
        End If

        If (Informations.IsNothing(vNewOsBaseAmount)) Or (vNewOsBaseAmount.Equals(0)) Or (bDefaultAll) Then
            vNewOsBaseAmount = 0
        End If

        If (Informations.IsNothing(vLossGainAmount)) Or (vLossGainAmount.Equals(0)) Or (bDefaultAll) Then
            vLossGainAmount = 0
        End If

        If (Informations.IsNothing(vIsPrimary)) Or (vIsPrimary.Equals(0)) Or (bDefaultAll) Then
            vIsPrimary = 0
        End If

        If (Informations.IsNothing(vEuroCurrencyID)) OrElse Informations.IsNothing(vEuroCurrencyID) Or (bDefaultAll) Then
            vEuroCurrencyID = 0
        End If

        If (Informations.IsNothing(vEuroAmount)) OrElse Informations.IsNothing(vEuroAmount) Or (bDefaultAll) Then
            vEuroAmount = 0
        End If

        If (Informations.IsNothing(vEuroBaseXRate)) OrElse Informations.IsNothing(vEuroBaseXRate) Or (bDefaultAll) Then
            vEuroBaseXRate = 1
        End If

        If (Informations.IsNothing(vEuroCcyXRate)) OrElse Informations.IsNothing(vEuroCcyXRate) Or (bDefaultAll) Then
            vEuroCcyXRate = 1
        End If

        If (Informations.IsNothing(crAllocAccountAmount)) OrElse Informations.IsNothing(crAllocAccountAmount) Or (bDefaultAll) Then
            crAllocAccountAmount = 0
        End If

        If (Informations.IsNothing(crAllocSystemAmount)) OrElse Informations.IsNothing(crAllocSystemAmount) Or (bDefaultAll) Then
            crAllocSystemAmount = 0
        End If

        If (Informations.IsNothing(nTransdetailExID)) OrElse Informations.IsNothing(nTransdetailExID) Or (bDefaultAll) Then
            nTransdetailExID = 0
        End If

        ' {* USER DEFINED CODE (End) *}

        Return nResult

    End Function

    ' ***************************************************************** '
    ' Name: CheckMandatory (Public)
    '
    ' Description: Sets the Default Values for a Allocationdetail.
    '
    ' ***************************************************************** '
    Private Function CheckMandatory(Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As Object = Nothing, Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing, Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing, Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        ' {* USER DEFINED CODE (Begin) *}



        If (Informations.IsNothing(vAllocationDetailID)) Or (Object.Equals(vAllocationDetailID, Nothing)) Then
            Return gPMConstants.PMEReturnCode.PMMandatoryMissing
        End If

        ' {* USER DEFINED CODE (End) *}

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: Validate (Private)
    '
    ' Description: Checks the Allocationdetail for Consistency.
    '
    ' eck110102 : Add IsMissing logic
    '
    ' ***************************************************************** '
    Private Function Validate(Optional ByRef vAllocationDetailID As Object = Nothing, Optional ByRef vCashlistitemID As Object = Nothing, Optional ByRef vAllocationId As Object = Nothing, Optional ByRef vOriginalCurrency As Object = Nothing, Optional ByRef vTransdetailID As Object = Nothing, Optional ByRef vDocumenttypeID As Object = Nothing, Optional ByRef vAccountingDate As Object = Nothing, Optional ByRef vDocumentRef As Object = Nothing, Optional ByRef vOriginalDate As Object = Nothing, Optional ByRef vAllocateToBase As Object = Nothing, Optional ByRef vOrigBaseAmount As Object = Nothing, Optional ByRef vOrigBaseAmountUnrounded As Object = Nothing, Optional ByRef vOrigCcyAmount As Object = Nothing, Optional ByRef vOrigCcyAmountUnrounded As Object = Nothing, Optional ByRef vOrigXrate As Object = Nothing, Optional ByRef vEffectiveXrate As Object = Nothing, Optional ByRef vOsBaseAmount As Object = Nothing, Optional ByRef vOsCcyAmount As Object = Nothing, Optional ByRef vAllocBaseAmount As Object = Nothing, Optional ByRef vAllocCcyAmount As Object = Nothing, Optional ByRef vFullyMatched As Object = Nothing, Optional ByRef vWriteOffReasonID As Object = Nothing, Optional ByRef vWriteOffAmount As Object = Nothing, Optional ByRef vNewOsBaseAmount As Object = Nothing, Optional ByRef vNewOsCcyAmount As Object = Nothing, Optional ByRef vLossGainAmount As Object = Nothing, Optional ByRef vIsPrimary As Object = Nothing, Optional ByRef vEuroCurrencyID As Object = Nothing, Optional ByRef vEuroAmount As Object = Nothing, Optional ByRef vEuroBaseXRate As Object = Nothing, Optional ByRef vEuroCcyXRate As Object = Nothing) As Integer

        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Validate

        ' {* USER DEFINED CODE (Begin) *}

        If Not Informations.IsNothing(vAllocationDetailID) Then

            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(vAllocationDetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vCashlistitemID) Then

            Dim dbNumericTemp2 As Double
            If Not Double.TryParse(CStr(vCashlistitemID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp2) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        If Not Informations.IsNothing(vAllocationId) Then

            Dim dbNumericTemp3 As Double
            If Not Double.TryParse(CStr(vAllocationId), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp3) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOriginalCurrency) Then

            Dim dbNumericTemp4 As Double
            If Not Double.TryParse(CStr(vOriginalCurrency), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp4) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vTransdetailID) Then

            Dim dbNumericTemp5 As Double
            If Not Double.TryParse(CStr(vTransdetailID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp5) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vDocumenttypeID) Then

            Dim dbNumericTemp6 As Double
            If Not Double.TryParse(CStr(vDocumenttypeID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp6) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAccountingDate) Then
            If Not Informations.IsDate(vAccountingDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOriginalDate) Then
            If Not Informations.IsDate(vOriginalDate) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAllocateToBase) Then

            Dim dbNumericTemp7 As Double
            If Not Double.TryParse(CStr(vAllocateToBase), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp7) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOrigBaseAmount) Then

            Dim dbNumericTemp8 As Double
            If Not Double.TryParse(CStr(vOrigBaseAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp8) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOrigBaseAmountUnrounded) Then

            Dim dbNumericTemp9 As Double
            If Not Double.TryParse(CStr(vOrigBaseAmountUnrounded), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp9) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOrigCcyAmount) Then

            Dim dbNumericTemp10 As Double
            If Not Double.TryParse(CStr(vOrigCcyAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp10) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOrigCcyAmountUnrounded) Then

            Dim dbNumericTemp11 As Double
            If Not Double.TryParse(CStr(vOrigCcyAmountUnrounded), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp11) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOrigXrate) Then

            Dim dbNumericTemp12 As Double
            If Not Double.TryParse(CStr(vOrigXrate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp12) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEffectiveXrate) Then

            Dim dbNumericTemp13 As Double
            If Not Double.TryParse(CStr(vEffectiveXrate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp13) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOsBaseAmount) Then

            Dim dbNumericTemp14 As Double
            If Not Double.TryParse(CStr(vOsBaseAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp14) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vOsCcyAmount) Then

            Dim dbNumericTemp15 As Double
            If Not Double.TryParse(CStr(vOsCcyAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp15) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAllocBaseAmount) Then

            Dim dbNumericTemp16 As Double
            If Not Double.TryParse(CStr(vAllocBaseAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp16) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vAllocCcyAmount) Then

            Dim dbNumericTemp17 As Double
            If Not Double.TryParse(CStr(vAllocCcyAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp17) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vFullyMatched) Then

            Dim dbNumericTemp18 As Double
            If Not Double.TryParse(CStr(vFullyMatched), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp18) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vWriteOffAmount) Then

            Dim dbNumericTemp19 As Double
            If Not Double.TryParse(CStr(vWriteOffAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp19) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vWriteOffReasonID) Then

            Dim dbNumericTemp20 As Double
            If Not Double.TryParse(CStr(vWriteOffReasonID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp20) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vNewOsCcyAmount) Then

            Dim dbNumericTemp21 As Double
            If Not Double.TryParse(CStr(vNewOsCcyAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp21) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vNewOsBaseAmount) Then

            Dim dbNumericTemp22 As Double
            If Not Double.TryParse(CStr(vNewOsBaseAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp22) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vLossGainAmount) Then

            Dim dbNumericTemp23 As Double
            If Not Double.TryParse(CStr(vLossGainAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp23) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vIsPrimary) Then

            Dim dbNumericTemp24 As Double
            If Not Double.TryParse(CStr(vIsPrimary), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp24) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEuroCurrencyID) Then

            Dim dbNumericTemp25 As Double
            If Not Double.TryParse(CStr(vEuroCurrencyID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp25) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEuroAmount) Then

            Dim dbNumericTemp26 As Double
            If Not Double.TryParse(CStr(vEuroAmount), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp26) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEuroBaseXRate) Then

            Dim dbNumericTemp27 As Double
            If Not Double.TryParse(CStr(vEuroBaseXRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp27) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEuroBaseXRate) Then

            Dim dbNumericTemp28 As Double
            If Not Double.TryParse(CStr(vEuroBaseXRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp28) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If


        If Not Informations.IsNothing(vEuroCcyXRate) Then

            Dim dbNumericTemp29 As Double
            If Not Double.TryParse(CStr(vEuroCcyXRate), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp29) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' {* USER DEFINED CODE (End) *}


        Return result

    End Function

    ' ***************************************************************** '
    ' Name: BeginTrans (Private)
    '
    ' Description: Begins a Transaction.
    '
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
    ' PRIVATE Methods (End)

    Public Sub New()
        MyBase.New()

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

    Shared Sub New()
        MainModule.JustForInvokeMain()
    End Sub
End Class
