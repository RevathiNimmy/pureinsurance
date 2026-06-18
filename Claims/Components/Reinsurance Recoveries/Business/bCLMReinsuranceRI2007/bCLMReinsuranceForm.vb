Option Strict Off
Option Explicit On
'Developer Guide no. 129
Imports SSP.Shared
Imports SSP.Shared.StringsHelper
<System.Runtime.InteropServices.ProgId("Form_NET.Form")>
Public NotInheritable Class Form
    Implements IDisposable

    ' ***************************************************************** '
    ' Creatable Form class which contains all the methods, business rules
    ' required for processing Reinsurance.
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
    Private Const ACClass As String = "Form"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)

    ' Primary Keys to work with
    Private m_lClaimID As Integer

    ' Flag to indicate that we are closing this claim
    Private m_bBalanceAndCloseClaim As Boolean

    ' Collection of reinsurance
    Private m_oReinsurances As bCLMReinsuranceRI2007.Reinsurances

    Private m_lRIArrangement_id As Integer

    'Gaurav
    Private m_vDeletedRIArrangementIds() As Object
    Private m_vParticipantsArray(,) As Object

    Private m_iRIArrangementVersion As Integer
    Private m_cTotalSumInsured As Decimal
    Private m_bOpenClaimNoTrans As Boolean
    'Arul Stephen
    Private m_bRecovery As Boolean
    Private m_lRecovery As Integer
    Private m_iIsCreated As Integer
    Public WriteOnly Property Recovery() As Boolean
        Set(ByVal Value As Boolean)
            m_bRecovery = Value
        End Set
    End Property
    Public WriteOnly Property ActualRecovery() As Integer
        Set(ByVal Value As Integer)
            m_lRecovery = Value
        End Set
    End Property

    'End Arul Stephen

    Public WriteOnly Property BrokerParticipantArray() As Object
        Set(ByVal Value As Object)
            m_vParticipantsArray = Value
        End Set
    End Property


    Public Property DeletedRIArrangementIds() As Object
        Get
            Return m_vDeletedRIArrangementIds.Clone()
        End Get
        Set(ByVal Value As Object)
            m_vDeletedRIArrangementIds = Value
        End Set
    End Property


    Public Property RIArrangementId() As Integer
        Get
            Return m_lRIArrangement_id
        End Get
        Set(ByVal Value As Integer)
            m_lRIArrangement_id = Value
        End Set
    End Property

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public ReadOnly Property EffectiveDate() As Date
        Get
            Return m_dtEffectiveDate
        End Get
    End Property

    Public Property BalanceAndCloseClaim() As Boolean
        Get
            Return m_bBalanceAndCloseClaim
        End Get
        Set(ByVal Value As Boolean)
            m_bBalanceAndCloseClaim = Value
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

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_lNavigate
        End Get
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_lProcessMode
        End Get
    End Property

    Public ReadOnly Property RecordCount() As Integer
        Get
            ' Return Number in Collection
            Return m_oReinsurances.Count()
        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            ' Return the Steps Status
            Return m_sStepStatus.Value
        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(value As Integer)
            m_iTask = value
        End Set
    End Property

    ' E007 Changes
    Public Property IsCreated() As Integer
        Get
            Return m_iIsCreated
        End Get
        Set(value As Integer)
            m_iIsCreated = value
        End Set
    End Property


    Public ReadOnly Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
    End Property
    Public ReadOnly Property TotalSumInsured() As Decimal
        Get
            Return m_cTotalSumInsured
        End Get
    End Property

    Public ReadOnly Property RIArrangementVersion() As Integer
        Get
            Return m_iRIArrangementVersion
        End Get
    End Property




    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '

    Public Function ApplyReinsurance(ByRef r_bApplyReinsurance As Boolean) As Integer
        ' Retained to preserve class interface
        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue
        r_bApplyReinsurance = True
        Return result
    End Function


    ' ***************************************************************** '
    ' This will create or refresh an entire reinsurance arrangement for
    ' the given risk, including the original reinsurance
    ' ***************************************************************** '
    Public Function CalculateRI() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CalculateRI"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Don't calculate if this is view mode
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                ' Add parameters
                bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                bPMAddParameter.AddParameterLite(m_oDatabase, "is_balance_and_close", Math.Abs(CInt(m_bBalanceAndCloseClaim)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                bPMAddParameter.AddParameterLite(m_oDatabase, "bOpenClaimNoTrans", Math.Abs(CInt(m_bOpenClaimNoTrans)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                'Arul Stephen
                If m_bRecovery Then
                    bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery", m_lRecovery, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                End If
                'End Arul Stephen
                bPMAddParameter.AddParameterLite(m_oDatabase, "is_created", CInt(m_iIsCreated), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                ' Execute SQL Statement to create new RI
                lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRISQL, sSQLName:=ACCopyRIName, bStoredProcedure:=ACCopyRIStored)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACCopyRISQL)
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
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

        Const kMethodName As String = "Cancel"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Loop round Collection
            For Each oReinsurance As Reinsurance In m_oReinsurances
                Select Case oReinsurance.DatabaseStatus
                    Case gPMConstants.PMEComponentAction.PMView, gPMConstants.PMEComponentAction.PMDummyDelete
                        ' Do nothing
                    Case gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEComponentAction.PMDelete
                        result = gPMConstants.PMEReturnCode.PMDataChanged
                        Exit For
                End Select
            Next oReinsurance

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Delete the  claim
    ' ***************************************************************** '
    Public Function DeleteClaim() As Integer

        Dim result As Integer = 0
        Dim lStatus As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DeleteClaim"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "status", lStatus, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            ' Execute sql
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteClaimSQL, sSQLName:=ACDeleteClaimName, bStoredProcedure:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to delete  claim")
            End If

            ' Store status
            lStatus = m_oDatabase.Parameters.Item("status").Value

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Update ri band details
    ' ***************************************************************** '
    Public Function EditUpdate(ByVal lRIBandID As Integer, ByRef vRILines(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vOldRI As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get appropriate band
        Dim oReinsurance As bCLMReinsuranceRI2007.Reinsurance = Nothing
        Try
            oReinsurance = m_oReinsurances.Item("RI" & lRIBandID)

        Catch
        End Try


        If oReinsurance Is Nothing Then
            gPMFunctions.RaiseError("Set oReinsurance = m_oReinsurances.Item", "Unable to locate reinsurance for band " & lRIBandID)
        End If

        ' Check if the values have changed
        If oReinsurance.DatabaseStatus <> gPMConstants.PMEComponentAction.PMEdit Then
            ' Check we have an array
            If Informations.IsArray(vRILines) And Informations.IsArray(oReinsurance.RILines) Then
                ' If dimensions have changed then we have added fac so set edited
                If vRILines.GetUpperBound(1) <> oReinsurance.RILines.GetUpperBound(1) Then
                    oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
                Else
                    ' Cache array

                    vOldRI = (oReinsurance.RILines).Clone()

                    ' We need to walk the 2 arrays to check
                    For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                        ' Check for changes to the editable fields only!!




                        If Not vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisReserve, lCount).Equals(vOldRI(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisReserve, lCount)) Or Not vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisPayment, lCount).Equals(vOldRI(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisPayment, lCount)) Then

                            oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
                            Exit For
                        End If
                    Next
                End If
            Else
                oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit
            End If
        End If

        ' Store array
        oReinsurance.RILines = vRILines

        ' Release reference
        oReinsurance = Nothing
        Return result
    End Function


    ' ***************************************************************** '
    ' Gets the reinsurance details for the supplied band
    ' ***************************************************************** '
    'Arul Stephen -New Optional parameter "cRecoveredToDate" has been added in the method.
    Public Function GetBandValues(ByVal lRIBandID As Integer, ByRef cSumInsured As Decimal, ByRef cReserveToDate As Decimal, ByRef cPaymentToDate As Decimal, ByRef cThisReserve As Decimal, ByRef cThisPayment As Decimal, ByRef lRIModelID As Integer, ByRef lCatastropheCodeID As Integer, ByRef lXolClmModelID As Integer, ByRef cXolClmLimit As Decimal, ByRef lXolCatModelID As Integer, ByRef cXolCatLimit As Decimal, ByRef lXolCatReinstatements As Integer, ByRef vRILines(,) As Object, Optional ByRef cRecoveredToDate As Decimal = 0, Optional ByRef lXOLRIModelId As Integer = 0, Optional ByRef dIncurredToDate As Decimal = 0) As Integer


        Dim result As Integer = 0



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get appropriate band
        Dim oReinsurance As bCLMReinsuranceRI2007.Reinsurance = Nothing
        Try
            oReinsurance = m_oReinsurances.Item("RI" & lRIBandID)

        Catch
        End Try


        If oReinsurance Is Nothing Then
            gPMFunctions.RaiseError("Set oReinsurance = m_oReinsurances.Item", "Unable to locate reinsurance for band " & lRIBandID)
        End If

        ' Set return values
        cSumInsured = oReinsurance.SumInsured
        cReserveToDate = oReinsurance.ReserveToDate
        cPaymentToDate = oReinsurance.PaymentToDate
        cThisReserve = oReinsurance.ThisReserve
        cThisPayment = oReinsurance.ThisPayment
        lRIModelID = oReinsurance.RIModelID
        vRILines = (oReinsurance.RILines).Clone()
        ' XOL
        lXolClmModelID = oReinsurance.XolClmModelID
        cXolClmLimit = oReinsurance.XolClmLimit
        lXolCatModelID = oReinsurance.XolCatModelID
        cXolCatLimit = oReinsurance.XolCatLimit
        lXolCatReinstatements = oReinsurance.XolCatReinstatements
        cRecoveredToDate = oReinsurance.RecoveredToDate
        lXOLRIModelId = oReinsurance.XOLRIModelId
        dIncurredToDate = oReinsurance.IncurredToDate
        oReinsurance = Nothing


        Return result
    End Function

    ' ***************************************************************** '
    ' Returns TRUE if the associated risk is 'Reinsurance Deferred'
    ' ***************************************************************** '
    Public Function GetClaimRiskStatus(ByVal v_lClaimId As Integer, ByRef r_bIsDeferred As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetClaimRiskStatus"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_bIsDeferred = False

            ' Add Parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimRiskStatusSQL, sSQLName:=ACGetClaimRiskStatusName, bStoredProcedure:=ACGetClaimRiskStatusStored, vResultArray:=vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to check deferred reinsurance status")
            End If

            ' Check the status returned
            If Informations.IsArray(vArray) Then
                'developer guide no.101
                If gPMFunctions.NullToString(vArray(1, 0)).Trim().ToUpper() = "RIDEFERRED" Then
                    r_bIsDeferred = True
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Gets reinsurance arrangement details
    ' ***************************************************************** '
    Public Function GetDetails() As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Recalculate the RI to ensure any new perils are taken into account
            m_lReturn = CType(CalculateRI(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Collection
            m_oReinsurances.Clear()

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "mode", m_iTask, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            'Arul Stephen
            If m_bRecovery Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery", Math.Abs(CInt(m_bRecovery)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            'End Arul Stephen
            ' Get RI Arrangement details
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementSQL, sSQLName:=ACSelectRIArrangementName, bStoredProcedure:=ACSelectRIArrangementStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementSQL)
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                result = gPMConstants.PMEReturnCode.PMNotFound
            Else
                ' Yes, load them into the collection

                m_lRIArrangement_id = CInt(vResultArray(MainModule.RIArrangementEnum.DBCRIArrangementID, 0))

                m_iRIArrangementVersion = gPMFunctions.ToSafeInteger(CDbl(vResultArray(MainModule.RIArrangementEnum.DBCRIRIArrangementVersion, 0)))


                lReturn = CType(StoreRIArrangement(vResultArray), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("StoreRIArrangement", "Unable to store reinsurance arrangement details")
                End If
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Find out if Claim was previously Info Only
    ' ***************************************************************** '
    Public Function GetInfoOnlyStatus(ByRef bInfoStatus As Boolean) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the Claim Id parameter (INPUT)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetInfoOnlyStatusSQL, sSQLName:=ACGetInfoOnlyStatusName, bStoredProcedure:=ACGetInfoOnlyStatusStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDatabase.SQLSelect failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInfoOnlyStatus")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            'either PMTrue or PMNotFound
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                bInfoStatus = False
            Else

                bInfoStatus = CBool(vResultArray(0, 0))
            End If

            Return result

        Catch excep As System.Exception




            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetInfoOnlyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClientAndPolicyID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ''PN 61908 (start)
    ' ***************************************************************** '
    ' Find out transaction type of claim
    ' ***************************************************************** '
    Public Function GetClaimTransType(ByRef vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add the Claim Id parameter (INPUT)
            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetClaimTransTypeSQL, sSQLName:=ACGetClaimTransType, bStoredProcedure:=ACGetClaimTransTypeStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACGetClaimTransTypeSQL)
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vResultArray) Then
                ' No Records, return PMFalse
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=CStr(result), r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function
    ''PN 61908 (end)

    '***********************************************************************
    ' Get the original claim ID from  table
    '***********************************************************************
    Public Function GetOriginalClaimID(ByVal v_lClaimId As Integer, ByRef r_lOriginalClaimID As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", v_lClaimId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute sql
            result = m_oDatabase.SQLSelect(sSQL:=ACGetOriginalClaimIDSQL, sSQLName:=ACGetOriginalClaimIDName, bStoredProcedure:=True, vResultArray:=vResultArray)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Check for results
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Get claim

            r_lOriginalClaimID = CInt(vResultArray(0, 0))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetOriginalClaimID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetOriginalClaimID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Gets reinsurance arrangement details
    ' ***************************************************************** '
    Public Function GetRIBands(ByRef vBands(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetRIBands"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Get RI Arrangement details
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementBandsSQL, sSQLName:=ACSelectRIArrangementBandsName, bStoredProcedure:=ACSelectRIArrangementBandsStored, vResultArray:=vBands)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLAction", "Unable to run: " & ACSelectRIArrangementBandsSQL)
            End If

            ' Do we have any records ?
            If Not Informations.IsArray(vBands) Then
                ' No Records, return PMFalse
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Retrieve details from treaty
    ' ***************************************************************** '
    Public Function GetTreatyInfo(ByVal lTreatyId As Integer, ByRef sCode As String, ByRef sAgreementCode As String, ByRef bIsRetained As Boolean) As Integer

        Dim result As Integer = 0
        Dim vArray(,) As Object = Nothing

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyInfo"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the Database Parameters Collection
            bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", lTreatyId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Execute SQL Statement
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTreatySQL, sSQLName:=ACSelectTreatyName, bStoredProcedure:=ACSelectTreatyStored, vResultArray:=vArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to select treaty details")
            End If

            If Informations.IsArray(vArray) Then

                sCode = CStr(vArray(1, 0))

                sAgreementCode = CStr(vArray(6, 0))

                bIsRetained = gPMFunctions.ToSafeBoolean(CStr(vArray(12, 0)))
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Entry point for any initialisation code for this object.
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

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

            ' Initialisation Code.

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            ' Set the ProcessMode to Generic
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric

            ' Set the Type Of Business to New Business
            m_sTransactionType = gPMConstants.PMTypeOfBusinessNB

            ' Set the Effective Date to NOW
            m_dtEffectiveDate = DateTime.Now

            ' Create Reinsurances Collection
            m_oReinsurances = New bCLMReinsuranceRI2007.Reinsurances()

            ' Initialise the Status settings
            m_sProcessStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sMapStatus.Value = gPMConstants.PMNavStatusUnknown
            m_sStepStatus.Value = gPMConstants.PMNavStatusUnknown

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function


    ' ***************************************************************** '
    ' Set the optional process modes.
    ' ***************************************************************** '
    Public Function SetProcessModes(Optional ByRef vTask As Object = Nothing, Optional ByRef vNavigate As Object = Nothing, Optional ByRef vProcessMode As Object = Nothing, Optional ByRef vTransactionType As Object = Nothing, Optional ByRef vEffectiveDate As Object = Nothing, Optional ByRef bOpenClaimNoTrans As Object = Nothing) As Integer

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

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Informations.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Informations.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If


            If Not Informations.IsNothing(bOpenClaimNoTrans) Then

                m_bOpenClaimNoTrans = CBool(bOpenClaimNoTrans)
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
    ' Set the Process, Map and Step status.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception



            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Entry point for any termination code for this object.
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                m_oReinsurances = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' When cancelling from a claim roadmap, the  table data needs to
    ' be deleted (for underwriting) and with claimsbuilder, additional
    ' GIS-related data will also need to be deleted...
    ' ***************************************************************** '
    Public Function TidyUpAfterCancel(ByVal v_lClaimId As Integer) As Integer

        Dim result As Integer = 0
        Dim oBusiness As bCLMRiskDetails.Business
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "TidyUpAfterCancel"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get claim risk business object

            oBusiness = New bCLMRiskDetails.Business
            lReturn = oBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "Failed to get instance of bCLMRiskDetails.Business")
            End If

            ' Set process modes

            lReturn = oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oBusiness.SetProcessModes", "Failed to set process modes on bCLMRiskDetails.Business")
            End If

            ' Tidy up claim

            lReturn = oBusiness.TidyUpAfterCancel(v_lClaimId:=v_lClaimId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oBusiness.TidyUpAfterCancel", "Failed to tidy up  claim")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            oBusiness = Nothing

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Loops round the collection, doing any required Adds, Deletes or Updates.
    ' ***************************************************************** '
    Public Function Update() As Integer

        Dim result As Integer = 0
        Dim bInTransaction As Boolean
        Dim vRILines(,) As Object
        Dim m_iProcessId As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lRiArrangementid As Integer
        Dim IsReserveChanged As Boolean

        Const kMethodName As String = "Update"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            Select Case m_sTransactionType
                Case "NB", "MTA", "REN", "MTC"
                    m_iProcessId = 1
                Case "C_CO", "C_CP", "C_CR"
                    m_iProcessId = 2
            End Select
            ' If we haven't already started a transaction start one.
            If Not bInTransaction Then
                lReturn = m_oDatabase.SQLBeginTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans", "Unable to begin transaction")
                End If
                bInTransaction = True
            End If

            If Informations.IsArray(m_vDeletedRIArrangementIds) Then
                For lCount As Integer = m_vDeletedRIArrangementIds.GetLowerBound(0) To m_vDeletedRIArrangementIds.GetUpperBound(0)
                    m_lReturn = CType(DeleteRILines(gPMFunctions.ToSafeLong(m_vDeletedRIArrangementIds(lCount))), gPMConstants.PMEReturnCode)
                Next
            End If

            ' Loop round Collection
            For Each oReinsurance As bCLMReinsuranceRI2007.Reinsurance In m_oReinsurances
                If oReinsurance IsNot Nothing Then
                    lRiArrangementid = oReinsurance.ArrangementID
                    ' If this line is marked as updated, update it
                    If oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMEdit Then
                        ' Write modified flag to arrangement
                        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", oReinsurance.ArrangementID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "is_modified", 1, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRIArrangementSQL, sSQLName:=ACUpdateRIArrangementName, bStoredProcedure:=ACUpdateRIArrangementStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement")
                        End If

                        ' Cache array and walk lines

                        vRILines = (oReinsurance.RILines).Clone()

                        For lCount As Integer = vRILines.GetLowerBound(1) To vRILines.GetUpperBound(1)
                            ' Check line status


                            If gPMFunctions.ToSafeLong(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LineID, lCount)) > 0 Or Convert.ToString(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, lCount)) = "FX" Then
                                ' Add parameters
                                bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LineID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisShare, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007AgreementCode, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "this_reserve", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisReserve, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "this_payment", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisPayment, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "Grouping", gPMFunctions.ToSafeLong(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007GroupingID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007SumInsured, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "Trans_type", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                                ' Update this line
                                lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdateRIArrangementLineSQL, sSQLName:=ACUpdateRIArrangementLineName, bStoredProcedure:=ACUpdateRIArrangementLineStored)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
                                End If

                            Else
                                ' Add parameters

                                'Developer guide no.85
                                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_line_id", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong, True)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", oReinsurance.ArrangementID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "type", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "treaty_id", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007TreatyID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "party_cnt", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007PartyCnt, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "xol_arrangement_id", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007XOLID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "default_share_percent", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007DefaultShare, lCount), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "this_share_percent", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisShare, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "agreement_code", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007AgreementCode, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "priority", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Priority, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "number_of_lines", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Lines, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "line_limit", gPMFunctions.ToSafeCurrency(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL200UpperLimit, lCount), 0), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007SumInsured, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "reserve", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ReserveToDate, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "payment", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007PaymentToDate, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "this_reserve", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisReserve, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "this_payment", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisPayment, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "lower_limit", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LowerLimit, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

                                bPMAddParameter.AddParameterLite(m_oDatabase, "retained", gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Retained, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "participation_percent", 0.0#, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                                bPMAddParameter.AddParameterLite(m_oDatabase, "Trans_type", m_sTransactionType, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                                ' Insert this line
                                lReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertRIArrangementLineSQL, sSQLName:=ACInsertRIArrangementLineName, bStoredProcedure:=ACInsertRIArrangementLineStored)
                                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to insert ri arrangement lines")
                                End If

                                ' Store new ID

                                vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LineID, lCount) = m_oDatabase.Parameters.Item("ri_arrangement_line_id").Value

                                'Add broker participants if any attached to Fac Prop


                                If CStr(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, lCount)) = "F" And Informations.IsArray(m_vParticipantsArray) And CDbl(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LineID, lCount)) > 0 Then
                                    For iRow As Integer = 0 To m_vParticipantsArray.GetUpperBound(0)

                                        If gPMFunctions.ToSafeLong(m_vParticipantsArray(iRow, ACIBrokerAssociationPartyCnt)) = gPMFunctions.ToSafeDouble(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007PartyCnt, lCount)) Then
                                            'Add broker

                                            bPMAddParameter.AddParameterLite(m_oDatabase, "Ri_arrangement_line_id", vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LineID, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                                            bPMAddParameter.AddParameterLite(m_oDatabase, "PartyCnt", m_vParticipantsArray(iRow, ACIBrokerPartyCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, False)
                                            bPMAddParameter.AddParameterLite(m_oDatabase, "Part_percent", m_vParticipantsArray(iRow, ACIBrokerParticipant_percent) * 100, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble, False)
                                            bPMAddParameter.AddParameterLite(m_oDatabase, "ProcessId", m_iProcessId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, False)

                                            lReturn = m_oDatabase.SQLAction(sSQL:=ACAddBrokerParticipantsSQL, sSQLName:=ACAddBrokerParticipantsName, bStoredProcedure:=ACAddBrokerParticipantsStored)
                                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to insert Ri Broker Lines")
                                            End If
                                        End If
                                    Next
                                End If
                            End If


                            If CDbl(vRILines(MainModule.RI2007ArrangementLineEnum.DBCRIL2007ThisReserve, lCount)) > 0 Then
                                IsReserveChanged = True
                            End If

                        Next
                        'Change in RI Map - Update RI Arrangement Version
                        If m_sTransactionType = "C_CR" And Not IsReserveChanged Then
                            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", lRiArrangementid, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRIArrangementVersionSQL, sSQLName:=ACUpdRIArrangementVersionName, bStoredProcedure:=ACUpdRIArrangementVersionStored)
                            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to Update RI Arrangement Version.")
                            End If

                        End If

                        ' Store array back (we may have new id's) and set db status

                        oReinsurance.RILines = vRILines
                        oReinsurance.DatabaseStatus = gPMConstants.PMEComponentAction.PMView
                    End If
                End If
            Next oReinsurance

            ' Commit transaction
            If bInTransaction Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans", "Unable to commit transaction")
                End If
                bInTransaction = False
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' Rollback any transactions
            If bInTransaction Then
                lReturn = m_oDatabase.SQLRollbackTrans()
                bInTransaction = False
            End If

        Finally
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: ValidateBands
    '
    ' Description: Check bands for 100% allocation
    '
    ' History:
    '   23/06/2003 Peter Finney - Created
    '
    ' Notes:
    '   r_lValid returns band status
    '       0 - Bands all okay
    '       1 - Reserve not 100% allocated
    '       2 - Payment not 100% allocated
    '   r_lBand returns number of invalid band if found
    ' ***************************************************************** '
    Public Function ValidateBands(ByRef r_lValid As Integer, ByRef r_lBand As Integer) As Integer

        Dim result As Integer = 0
        Try


            ' Default to true
            result = gPMConstants.PMEReturnCode.PMTrue
            r_lValid = 0

            ' Process each band
            For Each oReinsurance As bCLMReinsuranceRI2007.Reinsurance In m_oReinsurances
                ' Check for minor rounding
                oReinsurance.Round()

                ' Check band reserve against allocated reserve (no rounding issues)
                If oReinsurance.TotalThisReserve <> oReinsurance.ThisReserve Then
                    r_lValid = 1
                    r_lBand = oReinsurance.RIBand
                    Exit For
                End If

                ' Check band payment against allocated payment (no rounding issues)
                If oReinsurance.TotalThisPayment <> oReinsurance.ThisPayment Then
                    r_lValid = 2
                    r_lBand = oReinsurance.RIBand
                    Exit For
                End If
            Next oReinsurance

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateBands Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateBands", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Groups reinsurance lines by RI Band and adds them to the collection.
    ' ***************************************************************** '
    Private Function StoreRIArrangement(ByVal vArrangements(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vRILines(,) As Object = Nothing
        Dim oReinsurance As bCLMReinsuranceRI2007.Reinsurance


        Dim lReturn As gPMConstants.PMEReturnCode




        result = gPMConstants.PMEReturnCode.PMTrue

        ' For each record
        For lCount As Integer = vArrangements.GetLowerBound(1) To vArrangements.GetUpperBound(1)
            ' Create, set ri band and add to collection
            oReinsurance = New Reinsurance()

            oReinsurance.ArrangementID = CInt(vArrangements(MainModule.RIArrangementEnum.DBCRIArrangementID, lCount))

            oReinsurance.RIBand = CInt(vArrangements(MainModule.RIArrangementEnum.DBCRIBandID, lCount))

            oReinsurance.RIModelID = gPMFunctions.ToSafeInteger(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIModelID, lCount)), 0)

            oReinsurance.SumInsured = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRISumInsured, lCount)))

            oReinsurance.ReserveToDate = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIReserveToDate, lCount)))

            oReinsurance.PaymentToDate = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIPaymentToDate, lCount)))

            oReinsurance.ThisReserve = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIThisReserve, lCount)))

            oReinsurance.ThisPayment = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIThisPayment, lCount)))

            oReinsurance.CatastropheCodeID = gPMFunctions.ToSafeLong(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRICatastropheCodeID, lCount)))

            ' Store xol triggers

            oReinsurance.XolClmModelID = gPMFunctions.ToSafeLong(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIXolClmModelID, lCount)))



            oReinsurance.XolClmLimit = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIXolClmLimit, lCount)))

            oReinsurance.XolCatModelID = gPMFunctions.ToSafeLong(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIXolCatModelID, lCount)))

            oReinsurance.XolCatLimit = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIXolCatLimit, lCount)))

            oReinsurance.XolCatReinstatements = gPMFunctions.ToSafeLong(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIXolCatReinstatements, lCount)))

            'Start-(Arul Stephen)-(Bug Fixing-PN58889)

            oReinsurance.RecoveredToDate = gPMFunctions.ToSafeCurrency(CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIRecoveredToDate, lCount)))
            'End-(Arul Stephen)-(Bug Fixing-PN58889)
            ' Get RI Lines
            bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", m_lClaimID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            bPMAddParameter.AddParameterLite(m_oDatabase, "ri_arrangement_id", CDbl(vArrangements(MainModule.RIArrangementEnum.DBCRIArrangementID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "mode", m_iTask, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            'Arul Stephen
            If m_bRecovery Then
                bPMAddParameter.AddParameterLite(m_oDatabase, "Recovery", Math.Abs(CInt(m_bRecovery)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            End If
            'End Arul Stephen
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRIArrangementLineRI2007SQL, sSQLName:=ACSelectRIArrangementLineRI2007Name, bStoredProcedure:=ACSelectRIArrangementLineRI2007Stored, vResultArray:=vRILines)


            m_cTotalSumInsured = CDec(vArrangements(MainModule.RIArrangementEnum.DBCRISumInsured, 0))


            lReturn = CType(SortRIArray2007(vRILines), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to get ri arrangement lines")
            End If

            ' Store ri lines
            oReinsurance.RILines = vRILines
            oReinsurance.XOLRIModelId = ToSafeLong(vArrangements(MainModule.RIArrangementEnum.DBCRIXOLRIModelId, lCount))
            oReinsurance.IncurredToDate = ToSafeCurrency(vArrangements(MainModule.RIArrangementEnum.DBCRIIncurredToDate, lCount))

            ' Add new reinsurance arrangement to collection
            If m_oReinsurances.count = 0 Then
                m_oReinsurances.Add(Nothing)
            End If
            m_oReinsurances.Add(oReinsurance)
        Next lCount


        Return result

    End Function


    ' ***************************************************************** '
    '                           CLASS EVENTS
    ' ***************************************************************** '
    Public Sub New()
        MyBase.New()
        Exit Sub
    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub






    '*******************************************************************
    'Function              :  SortRIArray2007
    'Description           :  Sort the RI Lines.
    '                      :  Sort order based on RI line Type
    '                         i.e F(Fac Prop), FX(FAC XOl), T( Treaty Prop), TX(Treaty XOL), R(Retained)
    '*********************************************************************
    Private Function SortRIArray2007(ByRef vArrayResult(,) As Object) As Integer

        Dim result As Integer = 0
        Dim vTemp As Object




        result = gPMConstants.PMEReturnCode.PMTrue

        For iCnt1 As Integer = 0 To vArrayResult.GetUpperBound(1) - 1
            For iCnt As Integer = 0 To vArrayResult.GetUpperBound(1) - iCnt1 - 1


                If IsSortingRequired(CStr(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, iCnt)), CStr(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, iCnt + 1))) Then
                    For innerCnt As Integer = 0 To vArrayResult.GetUpperBound(0)

                        vTemp = vArrayResult(innerCnt, iCnt)


                        vArrayResult(innerCnt, iCnt) = vArrayResult(innerCnt, iCnt + 1)

                        vArrayResult(innerCnt, iCnt + 1) = vTemp
                    Next
                End If
            Next
        Next

        'Shorting Rows for same Type of RI Layers
        For iCnt1 As Integer = 0 To vArrayResult.GetUpperBound(1) - 1
            'developer guide no.248
            If gPMFunctions.ToSafeString(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, iCnt1)) = "TX" Or gPMFunctions.ToSafeString(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, iCnt1)) = "FX" Then
                For iCnt As Integer = iCnt1 + 1 To vArrayResult.GetUpperBound(1) - 1
                    'developer guide no.248
                    If gPMFunctions.ToSafeString(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, iCnt)) = gPMFunctions.ToSafeString(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007Type, iCnt1)) And gPMFunctions.ToSafeCurrency(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LowerLimit, iCnt)) < gPMFunctions.ToSafeCurrency(vArrayResult(MainModule.RI2007ArrangementLineEnum.DBCRIL2007LowerLimit, iCnt1)) Then
                        For innerCnt As Integer = 0 To vArrayResult.GetUpperBound(0)

                            vTemp = vArrayResult(innerCnt, iCnt)


                            vArrayResult(innerCnt, iCnt) = vArrayResult(innerCnt, iCnt1)

                            vArrayResult(innerCnt, iCnt1) = vTemp
                        Next
                    End If
                Next
            End If
        Next

        Return result
    End Function

    '*************************************************************************************
    '   Function        :        IsSortingRequired
    '   I/P             :        sRIType1,sRIType2  Denotes the RI Type
    '   Return          :        True/False
    '   Description     :        Returns True if sRIType1 priority is greater than sRIType2
    '                            Priority values are Hard Coded and calculated from index of vSortOrder.
    '   Example         :        Priority of 'T' is greater than 'F'
    '**************************************************************************************
    Private Function IsSortingRequired(ByVal sRIType1 As String, ByVal sRIType2 As String) As Boolean

        Dim iIndex1, iIndex2 As Integer

        Dim vSortOrder(6) As String
        vSortOrder(0) = "F"
        vSortOrder(1) = "FX"
        vSortOrder(2) = "TFS"
        vSortOrder(3) = "T"
        vSortOrder(4) = "TX"
        vSortOrder(5) = "TC"
        vSortOrder(6) = "R"

        For iCounter As Integer = vSortOrder.GetLowerBound(0) To vSortOrder.GetUpperBound(0)
            If vSortOrder(iCounter) = sRIType1 Then
                iIndex1 = iCounter
            End If
            If vSortOrder(iCounter) = sRIType2 Then
                iIndex2 = iCounter
            End If
        Next
        Return iIndex1 > iIndex2

    End Function


    Public Function DeleteRILines(ByVal lDeletedRILineIds As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "DeleteRILines"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bPMAddParameter.AddParameterLite(m_oDatabase, "Claim_ri_arrangement_line_id", lDeletedRILineIds, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)


            ' Delete this line
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteRIArrangementLineRI2007SQL, sSQLName:=ACDeleteRIArrangementLineRI2007Name, bStoredProcedure:=ACDeleteRIArrangementLineRI2007Stored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "Unable to update ri arrangement lines")
            End If
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Update(), excep:=ex)

        Finally
            '		Return result
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function
End Class
