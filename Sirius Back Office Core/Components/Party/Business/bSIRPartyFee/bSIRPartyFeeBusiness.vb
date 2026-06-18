Option Strict Off
Option Explicit On
' developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 05/05/1999
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a PMBFee.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 11/02/2004
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
    Private m_vNewArray(,) As Object

    ' bSIRParty.Init Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Instance of the Core  object
    'Private m_bSIRPartyFeeBusiness As bSIRPartyFee.Business

    ' PM Lookup Business Component (Private)
    Private m_oLookup As bPMLookup.Business

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

    'Lookup value for Loading in Edit Mode
    Private m_lRiskGroupID As Integer
    ' Primary Key to work with
    Private m_lPartyCnt As Integer

    Private m_sUnderwritingOrAgency As String = ""

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

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

    Public Property PartyCnt() As Integer
        Get

            Return m_lPartyCnt

        End Get
        Set(ByVal Value As Integer)

            m_lPartyCnt = Value

        End Set
    End Property

    Public Property RiskGroupID() As Integer
        Get

            Return m_lRiskGroupID

        End Get
        Set(ByVal Value As Integer)

            m_lRiskGroupID = Value

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
                End If
                m_oLookup = Nothing
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
    ' Name: GetDetails
    '
    ' Description: Get details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetDetails(ByRef vRiskGroup(,) As Object, ByRef vDiscounts(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get from DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskGroupsSQL, sSQLName:=ACGetRiskGRoupsName, bStoredProcedure:=ACGetRiskGroupsStored, lNumberRecords:=0, vResultArray:=vRiskGroup)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'TO DELETE DISCOUNT RELATED FETCH MK 991001
            'Get from DB
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetDiscountDetailsSQL, sSQLName:=ACGetDiscountDetailsName, bStoredProcedure:=ACGetDiscountDetailsStored, lNumberRecords:=0, vResultArray:=vDiscounts)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' PUBLIC Methods (End)


    ' PRIVATE Methods (Begin)

    ' PRIVATE Methods (End)


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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' 09/08/2000 PSA

    ' ***************************************************************** '
    ' Name: GetFeeDetailsByRisk
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetFeeDetailsByRisk(ByVal iLanguageID As Integer, ByVal sCode As String, ByRef r_vFeesArray As Object) As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="language_id", vValue:=CStr(iLanguageID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="code", vValue:=sCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetFeeByRiskCodeSQL, sSQLName:=ACGetFeeByRiskCodeName, bStoredProcedure:=ACGetFeeByRiskCodeStored, lNumberRecords:=0, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data
            If Informations.IsArray(vResultArray) Then


                r_vFeesArray = vResultArray
            Else
                r_vFeesArray = Nothing
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetFeeDetailsByRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeDetailsByRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'Added 990928 MK
    ' ***************************************************************** '
    ' Name: GetFeeAmount
    '
    ' Description: Get other details from DB, for related parties.
    '
    ' ***************************************************************** '
    Public Function GetFeeAmount(ByRef vPartyCnt As Integer, ByRef vRiskGroup As Integer, ByRef vPercentage As Double, ByRef vAmount As Decimal, ByRef vCommissionPercentage As Double, ByRef vCommissionAmount As Decimal) As Integer


        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(vPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Group_Id", vValue:=CStr(vRiskGroup), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPercentageDetailsSQL, sSQLName:=ACGetPercentageDetailsName, bStoredProcedure:=ACGetPercentageDetailsStored, lNumberRecords:=0, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'return the data
            If Informations.IsArray(vResultArray) Then

                vPercentage = CDbl(vResultArray(3, 0))

                vAmount = CDec(vResultArray(4, 0))

                vCommissionPercentage = CDbl(vResultArray(5, 0))

                vCommissionAmount = CDec(vResultArray(6, 0))
            Else
                vPercentage = 0
                vAmount = 0
                vCommissionPercentage = 0
                vCommissionAmount = 0
            End If

            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetFeeAmount Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetFeeAmount", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '----------------
    'Added MK
    ' ***************************************************************** '
    ' Name: GetLookupValues (Public)
    '
    ' Description: Gets the Lookup values for a SIRParty.
    '
    '
    ' ***************************************************************** '
    'developer guide no. 101
    Public Function GetLookupValues(Optional ByRef iLookupType As Integer = 0, Optional ByRef vTableArray As Object = Nothing, Optional ByRef iLanguageID As Object = Nothing, Optional ByRef vResultArray As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim dtEffectiveDate As Date

        ' {* USER DEFINED CODE (Begin) *}
        Dim vTabArray(3, 0) As Object
        'Dim vPartyTypeID As Variant
        ' {* USER DEFINED CODE (End) *}

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Reset Result Array
            'developer guide no. 146
            vResultArray = Nothing
            ' Reset Table Array

            'developer guide no. 146
            vTableArray = Nothing

            ' {* USER DEFINED CODE (Begin) *}

            ' Setup Lookup Table Names

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupTableName, 0) = "Risk_group"

            ' {* USER DEFINED CODE (End) *}

            ' Do we have any records
            ' No, we can only lookup all
            iLookupType = gPMConstants.PMELookupType.PMLookupAll

            ' Do not supply a key

            vTabArray(gPMConstants.PMELookupInArrayColPos.PMLookupKey, 0) = m_lRiskGroupID


            ' Default Effective Date to current date/time
            dtEffectiveDate = DateTime.Now

            ' Release SIRParty reference
            'Set oSIRParty = Nothing

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
    ' Name: DisplayLookupDetails
    '
    ' Description: Displays all of the lookup details using the lookup
    '              values/details.
    '
    ' ***************************************************************** '
    Public Function DisplayLookupDetails() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the lookup values.

            m_lReturn = CType(GetLookupValues(), gPMConstants.PMEReturnCode)

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get all of the lookup details.

            ' {* USER DEFINED CODE (Begin) *}
            'SP090998

            Return result

        Catch excep As System.Exception



            ' Error Section
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the lookup details", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayLookupDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'MK 991008
    ' *****************************************************************
    ' Name: UpdateFeeDetails(Public)
    '
    ' Method to Update list view details of PartyEX Fee Details
    '
    ' *****************************************************************
    Public Function UpdateFeeDetails(ByVal v_vPartyCnt As Object, ByRef v_vFeeDetails(,) As Object) As Integer


        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_vNewArray = v_vFeeDetails.Clone

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Delete old data
            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(v_vPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteFeeAmountsSQL, sSQLName:=ACDeleteFeeAmountsName, bStoredProcedure:=ACDeleteFeeAmountsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add new Fee Details for the PartyEX passed
            If (True) And (Informations.IsArray(v_vFeeDetails)) Then

                For i As Integer = v_vFeeDetails.GetLowerBound(1) To v_vFeeDetails.GetUpperBound(1)

                    If CStr(v_vFeeDetails(1, i)) <> "" Then

                        m_oDatabase.Parameters.Clear()


                        m_lReturn = m_oDatabase.Parameters.Add(sName:="party_cnt", vValue:=CStr(CInt(v_vPartyCnt)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_group_id", vValue:=CStr(CInt(v_vFeeDetails(0, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="fee_percentage", vValue:=CStr(CDbl(v_vFeeDetails(2, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="fee_amount", vValue:=CStr(CDbl(v_vFeeDetails(3, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_percentage", vValue:=CStr(CDbl(v_vFeeDetails(4, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_amount", vValue:=CStr(CDbl(v_vFeeDetails(5, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="display_on_quotes", vValue:=CStr(CInt(v_vFeeDetails(6, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                        If CStr(v_vFeeDetails(7, i)) = "" Or CStr(v_vFeeDetails(7, i)) = "0" Then

                            ' developer guide no. 85
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Else

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id", vValue:=CStr(CInt(v_vFeeDetails(7, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        End If
                        'Datasure - use tax group not rates

                        If CStr(v_vFeeDetails(8, i)) = "" Or CStr(v_vFeeDetails(8, i)) = "0" Then

                            ' developer guide no. 85
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="tax_group_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Else

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="tax_group_id", vValue:=CStr(CInt(v_vFeeDetails(8, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        End If

                        If CStr(v_vFeeDetails(9, i)) = "" Or CStr(v_vFeeDetails(9, i)) = "0" Then

                            ' developer guide no. 85
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="extra_scheme_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Else

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="extra_scheme_id", vValue:=CStr(CInt(v_vFeeDetails(9, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        End If

                        m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=CStr(CInt(v_vFeeDetails(11, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                        'Datasure - commission tax group

                        If CStr(v_vFeeDetails(13, i)) = "" Or CStr(v_vFeeDetails(13, i)) = "0" Then

                            ' developer guide no. 85
                            m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_tax_group_id", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        Else

                            m_lReturn = m_oDatabase.Parameters.Add(sName:="commission_tax_group_id", vValue:=CStr(CInt(v_vFeeDetails(8, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                        End If


                        m_lReturn = m_oDatabase.Parameters.Add(sName:="FSA_Type_Of_Sale_Id", vValue:=CStr(CInt(v_vFeeDetails(14, i))), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

                        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddFeeAmountsSQL, sSQLName:=ACAddFeeAmountsName, bStoredProcedure:=ACAddFeeAmountsStored)
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)

                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                Next i
            End If


            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateFeeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateFeeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '' ***************************************************************** '
    ' Name: UpdatePartyEX (Public)
    '
    ' Method to handle update of Party EX (PartyDetails)
    '
    ' ***************************************************************** '
    Public Function UpdatePartyEX(ByVal v_vPartyCnt As Object, ByVal v_vPartyTypeID As Object, ByVal v_vCurrencyID As Object, ByVal v_vShortName As Object, ByVal v_vName As Object, ByVal v_vPaymentMethodCode As Object, Optional ByVal v_vResolvedName As String = "", Optional ByVal v_vUniqueId As String = "", Optional ByVal v_vScreenHeirarchy As String = "") As Integer

        Dim result As Integer = 0

        Dim sSQL As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(BeginTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Build SQL





            If (Not Informations.IsNothing(v_vPartyTypeID)) Then
                sSQL = "UPDATE party SET " &
                    "party_type_id = " & CStr(CInt(v_vPartyTypeID)) &
                    ", shortname = '" & CStr(v_vShortName) & "'" &
                    ", name = '" & CStr(v_vName) & "'" &
                    ", currency_id = " & CStr(CInt(v_vCurrencyID)) &
                    ", payment_method_code = '" & CStr(v_vPaymentMethodCode) & "'" &
                    ", ScreenHierarchy = '" & CStr(v_vScreenHeirarchy) & "'" &
                    ", UserId = '" & CStr(m_iUserID) & "'" &
                    ", UniqueId = '" & CStr(v_vUniqueId) & "'"
            Else
                sSQL = "UPDATE party SET " &
                       "party_type_id = " & CStr(CInt(v_vPartyTypeID)) &
                       ", shortname = '" & CStr(v_vShortName) &
                       "', name = '" & CStr(v_vName) &
                       "', currency_id = " & CStr(CInt(v_vCurrencyID)) &
                       ", payment_method_code = '" & CStr(v_vPaymentMethodCode) & "'"
            End If

            If v_vResolvedName <> "" Then
                sSQL = sSQL & ", resolved_name = '" & v_vResolvedName & "'"
            End If


            sSQL = sSQL & " WHERE party_cnt = " & CStr(CInt(v_vPartyCnt))

            ' Execute statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UPDPARTYEX", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(CommitTrans(), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            m_lReturn = CType(RollbackTrans(), gPMConstants.PMEReturnCode)


            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePartyEX  Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePartyEX ", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
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

    ' ***************************************************************** '
    ' Name: DirectAdd (Public)
    '
    ' Description: Adds a single SIRPartyEX directly into the database.
    '
    ' ***************************************************************** '
    Public Function DirectAdd(Optional ByRef vPartyCnt As Object = Nothing, Optional ByRef vPartyTypeID As Object = Nothing, Optional ByRef vPartyStructureID As Object = Nothing, Optional ByRef vSourceID As Object = Nothing, Optional ByRef vPartyID As Object = Nothing, Optional ByRef vShortname As Object = Nothing, Optional ByRef vName As Object = Nothing, Optional ByRef vResolvedName As Object = Nothing, Optional ByRef vPaymentMethodCode As Object = Nothing, Optional ByRef vCurrencyId As Object = Nothing, Optional ByRef vLanguageID As Object = Nothing, Optional ByRef vCreatedByID As Object = Nothing, Optional ByRef vDateCreated As Object = Nothing, Optional ByVal vUniqueId As Object = Nothing, Optional ByVal vScreenHeirarchy As Object = Nothing) As Integer

        'Dim oSIRParty As bSIRParty.Business
        Dim result As Integer = 0
        Dim oSIRParty As Object 'bSIRParty.Business

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'EK 16/10/99
            ' Create a new SIRParty
            '    Set oSIRParty = New bSIRParty.Business
            '    m_lReturn& = oSIRParty.Initialise( _
            'sUsername:=m_sUsername, _
            'sPassword:=m_sPassword, _
            'iUserID:=m_iUserID, _
            'iSourceID:=m_iSourceID, _
            'iLanguageID:=m_iLanguageID, _
            'iCurrencyID:=m_iCurrencyID, _
            'iLogLevel:=m_iLogLevel, _
            'sCallingAppName:=ACApp)



            ' oSIRParty = New bSIRParty.Business

            'm_oInsurance = New bSIRInsuranceFile.Services
            oSIRParty = Nothing
            If oSIRParty Is Nothing Then
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=oSIRParty, v_sClassName:="bSIRParty.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bSIRParty.Business"
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            m_lReturn = oSIRParty.Initialise(sUsername:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSIRParty.DirectAdd(vPartyCnt:=vPartyCnt, vPartyTypeID:=vPartyTypeID, vPartyStructureID:=vPartyStructureID, vSourceID:=vSourceID, vPartyID:=vPartyID, vShortname:=vShortname, vName:=vName, vResolvedName:=vResolvedName, vCurrencyId:=vCurrencyId, vLanguageID:=vLanguageID, vCreatedByID:=vCreatedByID, vDateCreated:=vDateCreated, vPaymentMethodCode:=vPaymentMethodCode, vUniqueId:=vUniqueId, vScreenHeirarchy:=vScreenHeirarchy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                oSIRParty = Nothing
                Return result
            End If

            ' Retrieve Primary Key of new Core record


            vPartyCnt = oSIRParty.PartyCnt

            ' {* USER DEFINED CODE (Begin) *}
            ' Terminate Core business Component
            'EK 16/10/99

            oSIRParty.Dispose()
            ' {* USER DEFINED CODE (End) *}

            oSIRParty = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="DirectAdd Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="DirectAdd", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    '---------------------------------------------------------------------------------------
    ' Procedure : GetExtraSchemeDetails
    ' DateTime  : 10 Oct 03 11:55
    ' Author    : AMB
    ' Purpose   : Returns a list of the Extra Schemes
    '---------------------------------------------------------------------------------------
    '
    Public Function GetExtraSchemeDetails(ByRef r_vExtraSchemes(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetExtraSchemeDetailsSQL, sSQLName:=ACGetExtraSchemeDetailsName, bStoredProcedure:=ACGetExtraSchemeDetailsStored, vResultArray:=r_vExtraSchemes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExtraSchemeDetails")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If Not Informations.IsArray(r_vExtraSchemes) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            Else
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

        Catch
        End Try



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetExtraSchemeDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetExtraSchemeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UnderwritingOrAgency
    '
    ' Description:  Finds out if Underwriting or Agency business
    '
    ' ***************************************************************** '
    Private Function getUnderwritingOrAgency() As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(bPMFunc.getUnderwritingOrAgency(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, m_sCallingAppName, r_vUnderwriting:=m_sUnderwritingOrAgency), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getUnderwritingOrAgency Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetUnderwritingOrAgency")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function
End Class
