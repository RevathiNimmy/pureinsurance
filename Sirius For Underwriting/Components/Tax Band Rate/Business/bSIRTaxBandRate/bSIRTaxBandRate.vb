Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System

'Developer Guide No.: 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness
    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 03/01/2001
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a TaxBandRate.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

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
    Private m_lPMAuthorityLevel As Integer

    ' Primary Keys to work with
    Private m_lTaxBandId As Integer



    Public WriteOnly Property PMAuthorityLevel() As Integer
        Set(ByVal Value As Integer)
            Value = m_lPMAuthorityLevel
        End Set
    End Property

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property TaxBandId() As Integer
        Get
            Return m_lTaxBandId
        End Get
        Set(ByVal Value As Integer)
            m_lTaxBandId = Value
        End Set
    End Property


    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long Implements SSP.S4I.Interfaces.IBusiness.Initialise


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



            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set Username and Password
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

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
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
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


            If Not Information.IsNothing(vTask) Then

                m_iTask = CInt(vTask)
            End If


            If Not Information.IsNothing(vNavigate) Then

                m_lNavigate = CInt(vNavigate)
            End If


            If Not Information.IsNothing(vProcessMode) Then

                m_lProcessMode = CType(CInt(vProcessMode), gPMConstants.PMEProcessMode)
            End If


            If Not Information.IsNothing(vTransactionType) Then

                m_sTransactionType = CStr(vTransactionType)
            End If


            If Not Information.IsNothing(vEffectiveDate) Then

                m_dtEffectiveDate = CDate(vEffectiveDate)
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetProcessModes", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    Public Function GetTaxBandRate(ByRef r_vTaxBandRate(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetTaxBandRate"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_id", m_lTaxBandId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call procedure
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectTaxBandRateSQL, sSQLName:=ACSelectTaxBandRateName, bStoredProcedure:=ACSelectTaxBandRateStored, vResultArray:=r_vTaxBandRate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACSelectTaxBandRateSQL)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '		Return result

            ' This is for debugging only
            '		Resume 

            '		Return result
        End Try
        Return result
    End Function


    Public Function Update(ByVal v_vTaxBandRate(,) As Object, Optional v_sUniqueId As String = "", Optional v_sScreenHierarchy As String = "") As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Update"
        Dim lReturn As gPMConstants.PMEReturnCode

        Dim bTransaction As Boolean

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin a transaction for this process
            lReturn = m_oDatabase.SQLBeginTrans()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLBeginTrans()", "Begin Transaction Failed")
            End If

            ' We have an open transaction
            bTransaction = True

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_id", m_lTaxBandId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
            bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
            bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            ' Delete current tax bands
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteTaxBandRateSQL, sSQLName:=ACDeleteTaxBandRateName, bStoredProcedure:=ACDeleteTaxBandRateStored)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACDeleteTaxBandRateSQL)
            End If

            ' Only add new records if we have a valid array
            If Information.IsArray(v_vTaxBandRate) Then
                ' Process all the tax rates
                For lCount As Integer = v_vTaxBandRate.GetLowerBound(1) To v_vTaxBandRate.GetUpperBound(1)
                    ' Check for valid code

                    If Strings.Len(CStr(v_vTaxBandRate(ACRCode, lCount))) Then
                        ' Add parameters
                        bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_id", m_lTaxBandId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "tax_band_rate_id", v_vTaxBandRate(ACRTaxBandRateId, lCount), gPMConstants.PMEParameterDirection.PMParamInputOutput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "code", v_vTaxBandRate(ACRCode, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "caption_id", v_vTaxBandRate(ACRCaptionId, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_vTaxBandRate(ACRDescription, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "effective_date", CDate(v_vTaxBandRate(ACREffectiveDate, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "is_deleted", v_vTaxBandRate(ACRIsDeleted, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "is_value", v_vTaxBandRate(ACRIsValue, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "rate", v_vTaxBandRate(ACRRate, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "calc_basis", v_vTaxBandRate(ACRCalcBasis, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "basis_value", v_vTaxBandRate(ACRSumInsuredValue, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMCurrency)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "NB", v_vTaxBandRate(ACRNB, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "AMTA", v_vTaxBandRate(ACRAMTA, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "RMTA", v_vTaxBandRate(ACRRMTA, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "CANC", v_vTaxBandRate(ACRCANC, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "REN", v_vTaxBandRate(ACRREN, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                        bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_rounded", Math.Abs(CDbl(v_vTaxBandRate(ACRSumInsuredRounded, lCount))), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "currency_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRCurrencyID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "allow_tax_credit", v_vTaxBandRate(ACRAllowTaxCredit, lCount), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "country_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRCountryID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "state_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRStateID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "class_of_business_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRCOBID, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTRI", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTRI, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTRIC", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTRIC, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTAC", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTAC, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTF", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTF, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTCP", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTCP, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTCS", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTCS, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTCR", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTCR, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTIC", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTIC, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTI", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTI, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "MTA_Threshold_Date", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRMTAThresholdDate, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "Is_Passed_To_Insurer", gPMFunctions.BlankToZero(CType(v_vTaxBandRate(ACRIsPassedToInsurer, lCount), Integer)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

                        ' 020506 Datasure
                        If v_vTaxBandRate(ACRTTE, lCount) = "" Then
                            v_vTaxBandRate(ACRTTE, lCount) = 0
                        End If

                        If v_vTaxBandRate(ACRUseForBackdatedNB, lCount).ToString = "" Then
                            v_vTaxBandRate(ACRUseForBackdatedNB, lCount) = 0
                        End If

                        If v_vTaxBandRate(ACRUseForRefundWhenExpired, lCount).ToString = "" Then
                            v_vTaxBandRate(ACRUseForRefundWhenExpired, lCount) = 0
                        End If

                        bPMAddParameter.AddParameterLite(m_oDatabase, "TTE", gPMFunctions.BlankToZero(v_vTaxBandRate(ACRTTE, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_group_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRRiskGroupId, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "risk_code_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRRiskCodeId, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "COB_rating_section_id", gPMFunctions.BlankToNull(v_vTaxBandRate(ACRCOBRatingSectionId, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "use_for_refund_when_expired", BlankToZero(v_vTaxBandRate(ACRUseForRefundWhenExpired, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "use_for_backdated_nb", BlankToZero(v_vTaxBandRate(ACRUseForBackdatedNB, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "nTTRIPR", BlankToZero(v_vTaxBandRate(kACRIsRIPaymentsRecoveries, lCount)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "userid", m_iUserID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "UniqueId", v_sUniqueId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)
                        bPMAddParameter.AddParameterLite(m_oDatabase, "ScreenHierarchy", v_sScreenHierarchy, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

                        lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertTaxBandRateSQL, sSQLName:=ACInsertTaxBandRateName, bStoredProcedure:=ACInsertTaxBandRateStored)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACInsertTaxBandRateSQL)
                        End If
                    End If
                Next lCount
            End If

            ' If we have a transaction commit it
            If bTransaction Then
                lReturn = m_oDatabase.SQLCommitTrans()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLCommitTrans()", "Commit Transaction Failed")
                End If

                bTransaction = False
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            If bTransaction Then
                m_oDatabase.SQLRollbackTrans()
                bTransaction = False
            End If

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '		Return result

            ' This is for debugging only
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function



    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' 020506 Datasure
    Public Function GetCOBRatingSectionsForRisk(ByVal v_lRiskCodeID As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetCOBRatingSectionsForRisk"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameters
            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_Code_id", v_lRiskCodeID, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong, True)

            ' Call procedure
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectCOBRatingSectionsForRiskSQL, sSQLName:=ACSelectCOBRatingSectionsForRiskName, bStoredProcedure:=ACSelectCOBRatingSectionsForRiskStored, vResultArray:=r_vResults)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect()", "Command = " & ACSelectCOBRatingSectionsForRiskSQL)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
            '		Return result

            ' This is for debugging only
            '		Resume 


            '		Return result
        End Try
        Return result
    End Function
End Class
