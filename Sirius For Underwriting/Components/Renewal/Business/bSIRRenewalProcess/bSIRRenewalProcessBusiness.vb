Option Strict Off
Option Explicit On
Imports System.Data
Imports System.Text
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business

    Implements IDisposable
    ' ************************************************
    ' Added to replace global variables 26/09/2003
    ' Username.
    Private m_sUsername As String = ""

    ' Password.
    Private m_sPassword As String = ""

    ' User ID
    Private m_iUserID As Integer

    ' Calling Application
    Private m_sCallingAppName As String = ""
    ' Source ID
    Private m_iSourceID As Integer
    ' Language ID
    Private m_iLanguageID As Integer
    ' Currency ID
    Private m_iCurrencyID As Integer
    ' LogLevel
    Private m_iLogLevel As Integer
    ' ************************************************
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Business"

    ' PUBLIC Data Members (Begin)
    ' PUBLIC Data Members (End)
    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_lCurrentRecord As Integer

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_bDontDeleteScheme As Boolean
    Private m_oDataSet As cGISDataSetControl.Application
    Private m_iTransactionID As Integer
    Private m_bIsMigratedPolicy As Boolean
    Dim sOptionValue As String = ""
    Dim bChaseCycleEnabled As Boolean
    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

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
    'PN 74070
    Public ReadOnly Property DontDeleteScheme() As Boolean
        Get
            Return m_bDontDeleteScheme
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
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As Object = Nothing) As Integer


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' Set Username and Password
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            m_lReturn = CreateRequiredObject()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
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
    ' ***************************************************************** '
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub


    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                CloseRequiredObject()

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

    ' ***************************************************************** '
    'start database transaction
    ' ***************************************************************** '
    Public Function BeginTransaction() As Integer

        Return m_oDatabase.SQLBeginTrans()

    End Function

    ' ***************************************************************** '
    'save transaction to database
    ' ***************************************************************** '
    Public Function CommitTransaction() As Integer
        Return m_oDatabase.SQLCommitTrans()
    End Function

    ' ***************************************************************** '
    'rollback database transaction
    ' ***************************************************************** '
    Public Function RollBackTransaction() As Integer
        Return m_oDatabase.SQLRollbackTrans()
    End Function

    ' ***************************************************************** '
    ' instantiate required objects for Renewal Process to run
    ' ***************************************************************** '
    Private Function CreateRequiredObject() As Integer

        Dim result As Integer = 0
        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oInsuranceFile = New bSIRInsuranceFile.Services
            If m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

            End If


        Finally
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                CloseRequiredObject()
            End If
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' close down objects required by Renewal Process
    ' ***************************************************************** '
    Private Sub CloseRequiredObject()


        If Not (m_oInsuranceFile Is Nothing) Then

            m_oInsuranceFile.Dispose()
            m_oInsuranceFile = Nothing
        End If


    End Sub

    ' ***************************************************************** '
    ' create an instance of specified object
    ' ***************************************************************** '
    Private Function CreateBusinessObject(ByRef r_oObject As Object, ByVal v_sClassName As String) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        result = gPMComponentServices.CreateBusinessObject(r_oObject:=r_oObject, v_sClassName:=v_sClassName, v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)




        Return result

    End Function

    '*********************************************************************************************************************
    ' update renewal status with renewal status type id = specified value
    ' if specified value = 2 (Awaiting Renewal notice Print) then is_invite_printed will be set to zero
    '*********************************************************************************************************************
    Public Function SetRenewalStatusTypeID(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalStatusTypeID As Integer, Optional ByVal v_lIsInvitePrinted As Integer = 0, Optional ByVal sCreditControlEnabled As String = "") As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="RenewalInsuranceFileCnt", vValue:=CStr(v_lRenewalInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If m_oDatabase.Parameters.Add(sName:="RenewalStatusTypeID", vValue:=CStr(v_lRenewalStatusTypeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If m_oDatabase.Parameters.Add(sName:="IsInvitePrinted", vValue:=CStr(v_lIsInvitePrinted), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            If m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalStatusTypeIDSQL, sSQLName:=ACUpdateRenewalStatusTypeIDName, bStoredProcedure:=ACUpdateRenewalStatusTypeIDStored) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If sCreditControlEnabled = "1" Then


                m_oDatabase.Parameters.Clear()

                If m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                            vValue:=v_lRenewalInsuranceFileCnt,
                                                            iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                            iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                    SetRenewalStatusTypeID = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function

                End If
                If m_oDatabase.Parameters.Add(sName:="business_type",
                                                        vValue:="REN WTG UPDATE",
                                                        iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                        iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    'm_lReturn& = RollbackTrans()
                    'UpdateRenewalStatus = gPMConstants.PMEReturnCode.PMFalse
                    SetRenewalStatusTypeID = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                If m_oDatabase.SQLAction(sSQL:=ACAddCreditControlItemInsFileSQL,
                        sSQLName:=ACAddCreditControlItemInsFileName,
                        bStoredProcedure:=ACAddCreditControlItemInsFileStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                    SetRenewalStatusTypeID = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set renewal status type id for renewal insurance file count :" & v_lRenewalInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SetRenewalStatusTypeID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
            End If



        End Try
        Return result
    End Function

    '****************************************************************************************
    ' set r_lResult = PMTrue if this version of policy is quoted
    '****************************************************************************************
    Public Function IsQuoted(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lResult As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            r_lResult = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = m_oDatabase.SQLSelect(sSQL:=ACIsQuotedSQL, sSQLName:=ACIsQuotedName, bStoredProcedure:=ACIsQuotedStored, vResultArray:=vResultArray, bKeepNulls:=True)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If


            If CInt(vResultArray(0, 0)) = 0 Then
                r_lResult = gPMConstants.PMEReturnCode.PMTrue
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if policy is quoted - Insurance_file_cnt : " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="IsQuoted", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    '****************************************************************************
    'Check to see if this policy has an instalment plan attached
    '****************************************************************************
    Public Function IsInstalment(ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelIsInstalmentSQL, sSQLName:=ACSelIsInstalmentName, bStoredProcedure:=ACSelIsInstalmentStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to check if this policy has an instalment plan attached - Insurance_File_Cnt: " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="IsInstalment", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    '*****************************************************************************
    ' Create quote plan for renewal version if current version of policy is on instalment
    '*****************************************************************************
    Public Function CreateInstalmentQuote(ByVal v_lOriginalInsuranceFileCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_vPlanArray(,) As Object, ByRef r_sFailureMessage As String) As Integer

        Dim result As Integer = 0
        Dim lPremiumFinanceCnt, lPremiumFinanceVer As Integer
        Dim lpfschemenoorg, lpfschemeversionorg, lpfschemenoren, lpfschemeversionren, lpfpremfinancecntren, lpfpremfinanceversionren As Integer
        Dim vPreviousSchemeDetails(,) As Object = Nothing
        Dim m_oPremiumFinance As bSIRPremiumFinance.Business = Nothing

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            'PN 74070
            m_bDontDeleteScheme = False

            m_oPremiumFinance = New bSIRPremiumFinance.Business
            If m_oPremiumFinance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = gPMConstants.PMEReturnCode.PMFalse
                Return m_lReturn
            End If

            'UPGRADE_TODO: (1067) Member GetPreviousPlanSelectedFromInsuranceFile is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oPremiumFinance.GetPreviousPlanSelectedFromInsuranceFile(v_lInsuranceFileCnt:=v_lOriginalInsuranceFileCnt, r_vPreviousSchemeDetails:=vPreviousSchemeDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateInstalmentQuote", "Failed to get Previous Scheme Details")
            End If
            If Informations.IsArray(vPreviousSchemeDetails) Then
                lpfschemenoorg = gPMFunctions.ToSafeLong(vPreviousSchemeDetails(3, 0))
                lpfschemeversionorg = gPMFunctions.ToSafeLong(vPreviousSchemeDetails(4, 0))
            End If
            vPreviousSchemeDetails = Nothing
            'UPGRADE_TODO: (1067) Member GetPreviousPlanSelectedFromInsuranceFile is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
            m_lReturn = m_oPremiumFinance.GetPreviousPlanSelectedFromInsuranceFile(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, r_vPreviousSchemeDetails:=vPreviousSchemeDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreateInstalmentQuote", "Failed to get Previous Scheme Details")
            End If
            If Informations.IsArray(vPreviousSchemeDetails) Then
                lpfpremfinancecntren = gPMFunctions.ToSafeLong(vPreviousSchemeDetails(0, 0))
                lpfpremfinanceversionren = gPMFunctions.ToSafeLong(vPreviousSchemeDetails(1, 0))
                lpfschemenoren = gPMFunctions.ToSafeLong(vPreviousSchemeDetails(3, 0))
                lpfschemeversionren = gPMFunctions.ToSafeLong(vPreviousSchemeDetails(4, 0))
            End If

            ' Donot delete/insert plan if already changed during renewal amendment
            If lpfschemenoorg = lpfschemenoren And lpfschemeversionorg = lpfschemeversionren Then
                'Delete the original quote and create a new one
                'UPGRADE_TODO: (1067) Member DeletePlanForOneInsFile is not defined in type Variant. More Information: http://www.vbtonet.com/ewis/ewi1067.aspx
                m_lReturn = m_oPremiumFinance.DeletePlanForOneInsFile(v_lRenewalInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to delete quote plan for renewal version Policy ID " & v_lRenewalInsuranceFileCnt
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'create quote plan for renewal version

                m_lReturn = m_oPremiumFinance.CopyInstalmentPlanForRenewals(v_lOriginalInsuranceFileCnt:=v_lOriginalInsuranceFileCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lPartyCnt:=v_lPartyCnt, r_lPremiumFinanceCnt:=lPremiumFinanceCnt, r_lPremiumFinanceVer:=lPremiumFinanceVer)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to create quote plan for renewal version Policy ID " & v_lRenewalInsuranceFileCnt
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                ' Don't delete but update status Or just mark the status to PFStatusIndSaved
                m_bDontDeleteScheme = True
            End If

            m_lReturn = m_oPremiumFinance.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, r_vPFPremiumFinance:=r_vPlanArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to get instalment quote details for Policy ID " & v_lRenewalInsuranceFileCnt
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            result = gPMConstants.PMEReturnCode.PMTrue
            'if we get here then everything is cool

            Return result

        Catch excep As System.Exception

            r_sFailureMessage = excep.Message
            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instalment quote", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateInstalmentQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            If Not (m_oPremiumFinance Is Nothing) Then
                m_oPremiumFinance.Dispose()
                m_oPremiumFinance = Nothing
            End If

        End Try

        Return result
    End Function

    '****************************************************************************
    'Accepting renewal version and make it live
    '****************************************************************************
    'developer guide no. 101
    Public Function AcceptRenewal(ByVal v_lOldInsuranceFileCnt As Object, ByVal v_lNewInsuranceFileCnt As Object, ByVal v_lRenewalStatusCnt As Object, Optional ByRef v_sNewPolicyRef As Object = Nothing, Optional ByRef v_dNewStartDate As Date = #12/29/1899#, Optional ByRef v_dNewExpiryDate As Date = #12/29/1899#, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR", Optional ByRef v_lAccountId As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sInsFileStatusIdReplaced As String = String.Empty
        Dim sInsFileTypeIdPolicy As String = String.Empty
        Dim lIsMidNightRenewal As Integer
        Dim vEnablePayNowOptions As Object = Nothing
        Dim vTempResultArray(,) As Object = Nothing
        Dim m_sAgentType As String = ""
        Dim m_sPaymentMethod As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get insurance_file_status_id for status (REPLACED)
            m_lReturn = GetValueFromTable(v_sTableName:="Insurance_File_Status", v_vReturnColumn:="insurance_file_status_id", v_sKeyColumn:="Code", v_sKeyValue:="REP", v_iDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=sInsFileStatusIdReplaced)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to get insurance file status id for (REPLACED)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'get details for policy live version

            m_oInsuranceFile.InsuranceFileCnt = v_lOldInsuranceFileCnt


            If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to get policy details for live version, insurance file count : " & v_lOldInsuranceFileCnt
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'if live version has status of REPLACED then we can't accept renewal

            If m_oInsuranceFile.InsuranceFileStatusID = CInt(sInsFileStatusIdReplaced) Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Live version status is set to REPLACED, insurance file count : " & v_lOldInsuranceFileCnt
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'PN 33588 ----------------------------Start

            Dim vAltRefMandatory As Object = Nothing
            Dim vAltRefForEachTrans As Object = Nothing
            Dim sAltRefNewPolicy As String = ""
            If gPMFunctions.ToSafeLong(m_oInsuranceFile.LeadAgentCnt, 0) <> 0 Then



                m_lReturn = GetValueFromTable("party_agent", "alternate_reference_mandatory", "party_cnt", m_oInsuranceFile.LeadAgentCnt, gPMConstants.PMEDataType.PMLong, vAltRefMandatory)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = GetValueFromTable("party_agent", "alternate_reference_for_each_transaction", "party_cnt", m_oInsuranceFile.LeadAgentCnt, gPMConstants.PMEDataType.PMLong, vAltRefForEachTrans)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = GetValueFromTable("insurance_file", "alternate_reference", "insurance_file_cnt", CStr(v_lNewInsuranceFileCnt), gPMConstants.PMEDataType.PMLong, sAltRefNewPolicy)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If gPMFunctions.ToSafeBoolean(vAltRefMandatory, 0) And (gPMFunctions.ToSafeBoolean(vAltRefForEachTrans, 0) And gPMFunctions.ToSafeString(sAltRefNewPolicy) = "") Then
                    r_sFailureMessage = "The Alternate Reference must be entered for this renewal policy." & Strings.ChrW(13) & Strings.ChrW(10) &
                                        "You must amend the renewal before the renewal can be accepted."
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            'PN 33588 ----------------------------End

            'set status of live version to (REPLACED)

            m_oInsuranceFile.InsuranceFileStatusID = CInt(sInsFileStatusIdReplaced)


            m_lReturn = m_oInsuranceFile.UpdatePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to set status of live version to REPLACED, insurance file count :" & v_lOldInsuranceFileCnt
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'get insurance_file_type_id for insurance file type (POLICY)
            m_lReturn = GetValueFromTable(v_sTableName:="Insurance_File_Type", v_vReturnColumn:="insurance_file_type_id", v_sKeyColumn:="Code", v_sKeyValue:="POLICY", v_iDataType:=gPMConstants.PMEDataType.PMString, r_vResult:=sInsFileTypeIdPolicy)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to get insurance file type id for type POLICY"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'get details for renewal version of policy

            m_oInsuranceFile.InsuranceFileCnt = v_lNewInsuranceFileCnt


            If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to get policy details for renewal version, insurance file count : " & v_lNewInsuranceFileCnt
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'change renewal version to type (POLICY) and status to (LIVE)


            m_oInsuranceFile.InsuranceFileStatusID = Nothing

            m_oInsuranceFile.InsuranceFileTypeID = CInt(sInsFileTypeIdPolicy)

            'change insurance_ref and cover period if required
            If v_sNewPolicyRef <> "" Then

                m_oInsuranceFile.InsuranceRef = v_sNewPolicyRef
            End If

            If Informations.DateDiff("d", v_dNewStartDate, #12/29/1899#, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1) <> 0 Then
                m_oInsuranceFile.CoverStartDate = v_dNewStartDate
            End If

            If Informations.DateDiff("d", v_dNewExpiryDate, #12/29/1899#, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1) <> 0 Then

                m_oInsuranceFile.ExpiryDate = v_dNewExpiryDate

                If IsMidnightRenewal(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt, r_lIsMidNightRenewal:=lIsMidNightRenewal) <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                'renewal date is one more then expiry date in the case of midnight renewal
                If lIsMidNightRenewal = 1 Then

                    m_oInsuranceFile.RenewalDate = v_dNewExpiryDate.AddDays(1)
                Else

                    m_oInsuranceFile.RenewalDate = v_dNewExpiryDate
                End If

            End If

            'make renewal version LIVE

            m_lReturn = m_oInsuranceFile.UpdatePolicy

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to make renewal version LIVE, insurance file count :" & v_lNewInsuranceFileCnt
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Changes done by Krishna Nand PN: 70509 Dated: 31/03/2010
            m_lReturn = UpdateCurrencyToInsuranceFile()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'End of Changes

            'update insurance file system so we know that renewal has been done to this policy
            m_lReturn = UpdateInsuranceFileSystem(v_lNewInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update insurance file system, insurance file count :" & v_lNewInsuranceFileCnt
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Need to remove Last Print Run before deleting Renewal_Status table.
            If DeleteLastPrintRun(v_lUserID:=m_iUserID, v_lRenewalStatusCnt:=v_lRenewalStatusCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete last print run, renewal status count : " & v_lRenewalStatusCnt
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'delete record from of renewal status table
            If DeleteRenewalStatus(v_lRenewalStatusCnt:=v_lRenewalStatusCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete from renewal status, renewal status count :" & v_lRenewalStatusCnt
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'increament renewal count in Insurance_Folder
            If UpdateRenewalCount(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then

                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to increament renewal count in insurance folder, insurance file count : " & v_lNewInsuranceFileCnt
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Process risks and update policy premium
            If ProcessUpdatePolicy(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = CType(bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnablePayNowOptions, v_vBranch:=m_iSourceID, r_vUnderwriting:=vEnablePayNowOptions), gPMConstants.PMEReturnCode)
            If CStr(vEnablePayNowOptions) <> "1" Then
                ' If m_lLeadAgentCnt > 0 Then
                m_lReturn = GetAgentType(v_lNewInsuranceFileCnt, vTempResultArray)
                'End If
                If Informations.IsArray(vTempResultArray) Then
                    m_sAgentType = CStr(vTempResultArray(0, 0)).Trim()

                    If m_sAgentType = "Intermed" Then
                        m_lReturn = GetPaymentMethod(v_lNewInsuranceFileCnt, vTempResultArray)
                        If Informations.IsArray(vTempResultArray) Then
                            m_sPaymentMethod = CStr(vTempResultArray(0, 0)).Trim()
                            If m_sPaymentMethod <> "" Or CStr(m_sPaymentMethod).ToUpper() <> "PAYNOW" Then
                                m_lReturn = GetTransAccountId(v_lOldInsuranceFileCnt, vTempResultArray)
                                If Informations.IsArray(vTempResultArray) Then
                                    v_lAccountId = CInt(vTempResultArray(0, 0))
                                End If
                            End If
                        End If
                    End If
                End If
            End If

            Dim oaPFPlan(,) As Object = Nothing
            m_lReturn = GetPFPlanForInsFile(v_lNewInsuranceFileCnt, oaPFPlan)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sFailureMessage = "Failed to check If PF paln is attached to Insurance file"
                Return m_lReturn
            End If
            'If PF plan exists for insurance file
            If oaPFPlan IsNot Nothing AndAlso Informations.IsArray(oaPFPlan) AndAlso oaPFPlan.Length > 0 Then
                'Get the payment method type
                Dim oaPaymentMethod(,) As Object = Nothing
                m_lReturn = GetPaymentMethod(v_lNewInsuranceFileCnt, oaPaymentMethod)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sFailureMessage = "Failed to get payment method for insurance file"
                    Return m_lReturn
                End If
                If oaPaymentMethod IsNot Nothing AndAlso Informations.IsArray(oaPaymentMethod) Then
                    m_sPaymentMethod = ToSafeString(oaPaymentMethod(0, 0)).Trim().ToUpper()
                End If
                'If payment method is other than instalment then delete the attached plan and create log
                If m_sPaymentMethod <> "INSTALMENTS" AndAlso m_sPaymentMethod <> "INSTALMENT" AndAlso m_sPaymentMethod <> "PREMIUMFINANCE" AndAlso
                    m_sPaymentMethod <> "DIRECT DEBIT" AndAlso m_sPaymentMethod <> "CREDIT CARD" Then
                    Dim obSIRPremiumFinance As bSIRPremiumFinance.Business = New bSIRPremiumFinance.Business
                    m_lReturn = obSIRPremiumFinance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to initilize bSIRPremiumFinance.Business"
                        Return m_lReturn
                    End If
                    m_lReturn = obSIRPremiumFinance.DeletePlanForOneInsFile(v_lNewInsuranceFileCnt)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sFailureMessage = "Failed to Delete PF plan for insurance file"
                        Return m_lReturn
                    End If
                    m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=m_oInsuranceFile.InsuredCnt, v_vInsuranceFolderCnt:=m_oInsuranceFile.InsuranceFolderCnt, v_vInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                            v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value,
                                            v_vCampaignId:=DBNull.Value, v_vDocumentType:=DBNull.Value, v_vReportType:=DBNull.Value, v_vEventType:=PMBConst.PMBEventPolChange,
                                            v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:="Instalment plan deleted")
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        If r_sFailureMessage <> "GGGGGRRRRRR" Then
                            r_sFailureMessage = "Failed to create event"
                        End If
                        Return m_lReturn
                    End If
                    obSIRPremiumFinance.Dispose()
                    obSIRPremiumFinance = Nothing
                End If
            End If
        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AcceptRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AcceptRenewal", excep:=ex)


        Finally

        End Try
        Return result
    End Function

    '****************************************************************************
    ' Get values from specified columns (v_vReturnColumn)
    ' if v_sKeyColumn and v_sKeyValue is passed in then use it as criteria otherwise
    ' whole table is selected
    ' if v_vReturnColumn is not an array then single value is returned
    ' Note: v_lColumnType = PMLong, PMString etc
    '
    '****************************************************************************
    'developer guide no. 101
    Public Function GetValueFromTable(ByVal v_sTableName As Object, ByVal v_vReturnColumn As Object, ByVal v_sKeyColumn As Object, ByVal v_sKeyValue As Object, ByVal v_iDataType As Object, ByRef r_vResult As Object) As Integer

        Dim result As Integer = 0
        Dim sSQL As New StringBuilder
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = New StringBuilder("SELECT DISTINCT ")

            If Informations.IsArray(v_vReturnColumn) Then


                For lCount As Integer = 0 To v_vReturnColumn.GetUpperBound(0)

                    sSQL.Append(CStr(v_vReturnColumn(lCount)) & ",")
                Next

                'get rid of last comma
                sSQL = New StringBuilder(sSQL.ToString().Substring(0, sSQL.ToString().Length - 1))

            Else

                sSQL.Append(CStr(v_vReturnColumn))
            End If

            sSQL.Append(Strings.ChrW(13) & Strings.ChrW(10) & "FROM " & v_sTableName & Strings.ChrW(13) & Strings.ChrW(10))

            If v_sKeyColumn <> "" Then
                sSQL.Append("WHERE " & v_sKeyColumn & " = {KeyValue}")

                m_oDatabase.Parameters.Clear()

                Select Case v_iDataType
                    Case gPMConstants.PMEDataType.PMString
                        'developer guide no. 98
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMLong
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)
                    Case gPMConstants.PMEDataType.PMInteger
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDouble
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMDate
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMBoolean
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                    Case gPMConstants.PMEDataType.PMCurrency
                        m_lReturn = m_oDatabase.Parameters.Add(sName:="KeyValue", vValue:=v_sKeyValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iDataType)

                End Select

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL.ToString(), sSQLName:="Get single value from table", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResultArray, bKeepNulls:=False)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'are we returning an array or a single value?
            If Informations.IsArray(v_vReturnColumn) Then


                r_vResult = vResultArray
            Else
                If Informations.IsArray(vResultArray) Then


                    r_vResult = vResultArray(0, 0)
                Else
                    result = gPMConstants.PMEReturnCode.PMNotFound
                End If
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get value from " & v_sTableName & "." & v_sKeyColumn & " = " & v_sKeyValue, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    '*********************************************************************************
    ' update insurance file system with renewal event
    '*********************************************************************************
    'developer guide no. 101
    Public Function UpdateInsuranceFileSystem(ByRef v_lInsuranceFileCnt As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim vResultArray1(,) As Object = Nothing
        Dim lTransTypeId As Integer
        Dim sTransTypeDescription As String = ""

        Dim vOldTransTypeArray(,) As Object = Nothing
        Dim sOldTransTypeDescription As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get the old transtype description
            sSQL = "SELECT last_trans_type_id, last_trans_description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM insurance_file_system ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifs.insurance_file_cnt = " & v_lInsuranceFileCnt

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetOldTransTypeDescription", bStoredProcedure:=False, vResultArray:=vOldTransTypeArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Informations.IsArray(vOldTransTypeArray) Then

                sOldTransTypeDescription = gPMFunctions.ToSafeString(CStr(vOldTransTypeArray(1, 0)))
            Else
                sOldTransTypeDescription = ""
            End If

            'Get the required transaction details first.
            sSQL = "SELECT transaction_type_id, description" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "FROM Transaction_Type" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE LTRIM(RTRIM(code)) = 'REN'"

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionTypeDetails", bStoredProcedure:=False, vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If


            lTransTypeId = CInt(vResultArray(0, 0))

            sSQL = "Select last_trans_description from Insurance_File_System Where insurance_file_cnt = " & CStr(v_lInsuranceFileCnt)

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL,
                                                sSQLName:="SelectLastTransDescription",
                                                bStoredProcedure:=False,
                                                vResultArray:=vResultArray1)

            If Information.IsArray(vResultArray1) Then
                sTransTypeDescription = vResultArray1(0, 0)
            End If
            If sTransTypeDescription = "" Then
                sTransTypeDescription = Replace(CStr(vResultArray(1, 0)), "'", "''")
            End If

            sSQL = "UPDATE Insurance_File_System" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "SET modified_by_id = " & m_iUserID & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ", last_modified = {last_modified}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_date = {last_trans_date}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & ",last_trans_type_id = " & lTransTypeId & Strings.ChrW(13) & Strings.ChrW(10)
            If sOldTransTypeDescription = "" Then
                sSQL = sSQL & ",last_trans_description = '" & sTransTypeDescription & "'" & Strings.ChrW(13) & Strings.ChrW(10)
            End If
            sSQL = sSQL & "FROM insurance_file_system ifs" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE ifs.insurance_file_cnt = " & v_lInsuranceFileCnt

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_modified", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="last_trans_date", vValue:=DateTime.Now, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateInsuranceFileSystem", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update Insurance_File_System with renewal event", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateInsuranceFileSystem", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    ' ********************************************************************************
    ' Description: delete last print run or just the ones link to v_lRenewalStatusCnt
    ' ********************************************************************************
    Public Function DeleteLastPrintRun(ByVal v_lUserID As Integer, Optional ByVal v_lRenewalStatusCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "DELETE Last_Print_Run " & Strings.ChrW(13) & Strings.ChrW(10)


            m_oDatabase.Parameters.Clear()


            If v_lRenewalStatusCnt <> 0 Then
                sSQL = sSQL & "Where renewal_status_cnt = {RenewalStatusCnt}"

                m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalStatusCnt", vValue:=CStr(v_lRenewalStatusCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                sSQL = sSQL & "WHERE UserID = {UserID}"

                m_lReturn = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=CStr(v_lUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteLastPrintRun", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete Last_print_Run for renewal status ID : " & v_lRenewalStatusCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteLastPrintRun", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    '************************************************************************
    'Delete record from renewal status table
    '************************************************************************
    'developer guide no. 101
    Public Function DeleteRenewalStatus(ByVal v_lRenewalStatusCnt As Object) As Integer
        Dim result As Integer = 0
        Dim sSQL As String = ""


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Now set the Renewal status to that supplied

            sSQL = "DELETE Renewal_status " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE renewal_status_cnt = " & v_lRenewalStatusCnt & Strings.ChrW(13) & Strings.ChrW(10)

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="DeleteRenewalStatus", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete record from Renewal_Status table Renewal_Status_Cnt :" & v_lRenewalStatusCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally



        End Try
        Return result
    End Function

    Public Function RollBackPolicyToPreviousStatus(ByVal v_lProductId As Integer, ByVal v_lRenewalStatusTypeID As Integer, ByVal v_lInsuranceHolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_vLeadAgentCnt As String, ByVal v_lRenewalInsuranceFileCnt As Integer, Optional ByVal v_lBrokerXferStatusTypeID As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            result = m_oDatabase.SQLBeginTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            If v_vLeadAgentCnt = "" Then

                v_vLeadAgentCnt = Nothing
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="renewal_status_type_id", vValue:=v_lRenewalStatusTypeID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="insurance_holder_cnt", vValue:=v_lInsuranceHolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If
            If Informations.IsNothing(v_vLeadAgentCnt) OrElse v_vLeadAgentCnt = "" Then
                result = m_oDatabase.Parameters.Add(sName:="lead_agent_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            Else
                result = m_oDatabase.Parameters.Add(sName:="lead_agent_cnt", vValue:=v_vLeadAgentCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            End If

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="created_by_id", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="date_created", vValue:=DateTime.Today, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            'Developer Guide No. 85
            result = m_oDatabase.Parameters.Add(sName:="critical_date", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=v_lRenewalInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="is_invite_printed", vValue:=gPMConstants.PMEReturnCode.PMFalse, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            'Developer Guide No. 85
            result = m_oDatabase.Parameters.Add(sName:="BrokerXferStatusTypeID", vValue:=If(v_lBrokerXferStatusTypeID = 0, DBNull.Value, CStr(v_lBrokerXferStatusTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACRollBackPolicyToPreviousStateSQL, sSQLName:=ACRollBackPolicyToPreviousStateName, bStoredProcedure:=ACRollBackPolicyToPreviousStateStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLCommitTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to reset policy to previous state", vApp:=ACApp, vClass:=ACClass, vMethod:="RollBackPolicyToPreviousStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally


        End Try
        Return result
    End Function



    '*****************************************************************************************
    'Increament renewal count in Insurance_Folder table
    '*****************************************************************************************
    Public Function UpdateRenewalCount(Optional ByVal v_lInsuranceFileCnt As Integer = 0, Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFileCnt = 0 And v_lInsuranceFolderCnt = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Insurance File Count And Insurance Folder Count Are Zero", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalCount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If

            result = m_oDatabase.SQLBeginTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACUpdRenewalCountSQL, sSQLName:=ACUpdRenewalCountName, bStoredProcedure:=ACUpdRenewalCountStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If

            result = m_oDatabase.SQLCommitTrans()

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = m_oDatabase.SQLRollbackTrans()
                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to increament renewal count", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalCount", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally


        End Try
        Return result
    End Function

    ' *****************************************************************
    'Create an event in Event_Log
    ' *****************************************************************
    'developer guide no. 101
    Public Function CreateEvent(Optional ByVal v_vEventCnt As Object = Nothing, Optional ByVal v_vPartyCnt As Object = Nothing, Optional ByVal v_vInsuranceFolderCnt As Object = Nothing, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByVal v_vClaimCnt As Object = Nothing, Optional ByVal v_vDocumentCnt As Object = Nothing, Optional ByVal v_vOldAddressCnt As Object = Nothing, Optional ByVal v_vNewAddressCnt As Object = Nothing, Optional ByVal v_vCampaignId As Object = Nothing, Optional ByVal v_vDocumentType As Object = Nothing, Optional ByVal v_vReportType As Object = Nothing, Optional ByVal v_vEventType As Object = Nothing, Optional ByVal v_vUserId As Object = Nothing, Optional ByVal v_vEventDate As Date = #12/30/1899#, Optional ByVal v_vDescription As Object = Nothing) As Integer


        Dim result As Integer = 0
        Dim oEvent As bSIREvent.Business = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            If Informations.IsNothing(v_vEventCnt) Then
                v_vEventCnt = 0
            End If

            oEvent = New bSIREvent.Business
            If oEvent.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If Informations.IsNothing(v_vPartyCnt) Then


                v_vPartyCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vInsuranceFolderCnt) Then


                v_vInsuranceFolderCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vInsuranceFileCnt) Then


                v_vInsuranceFileCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vClaimCnt) Then


                v_vClaimCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vDocumentCnt) Then


                v_vDocumentCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vOldAddressCnt) Then


                v_vOldAddressCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vNewAddressCnt) Then


                v_vNewAddressCnt = DBNull.Value
            End If


            If Informations.IsNothing(v_vCampaignId) Then


                v_vCampaignId = DBNull.Value
            End If



            If Informations.IsNothing(v_vDocumentType) Then


                v_vDocumentType = DBNull.Value
            End If


            If Informations.IsNothing(v_vReportType) Then


                v_vReportType = DBNull.Value
            End If


            If Informations.IsNothing(v_vEventType) Then


                v_vEventType = DBNull.Value
            End If


            If Informations.IsNothing(v_vUserId) Then
                v_vUserId = m_iUserID
            End If


            If Informations.IsNothing(v_vEventDate) Then
                v_vEventDate = DateTime.Today
            End If


            If Informations.IsNothing(v_vDescription) Then


                v_vDescription = DBNull.Value
            End If

            'create an event in event_log
            oEvent.m_oDatabase = m_oDatabase
            result = oEvent.DirectAdd(vEventCnt:=v_vEventCnt, vPartyCnt:=v_vPartyCnt, vInsuranceFolderCnt:=v_vInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vClaimCnt:=v_vClaimCnt, vDocumentCnt:=v_vDocumentCnt, vOldAddressCnt:=v_vOldAddressCnt, vNewAddressCnt:=v_vNewAddressCnt, vCampaignId:=v_vCampaignId, vDocumentType:=v_vDocumentType, vReportType:=v_vReportType, vEventType:=v_vEventType, vUserId:=v_vUserId, vEventDate:=v_vEventDate, vDescription:=v_vDescription)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add event.", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create event", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEvent", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally
            If Not (oEvent Is Nothing) Then
                oEvent.Dispose()
                oEvent = Nothing
            End If


        End Try
        Return result
    End Function

    '************************************************************************************
    'create a lock for specified key and value
    '************************************************************************************
    'developer guide no. 101
    Public Function LockKey(ByVal v_sKeyName As Object, ByVal v_lKeyValue As Object, ByVal v_lUserID As Object, ByRef r_sLockedBy As Object) As Integer
        Dim oLock As bpmlock.User = Nothing
        Dim result As Integer = 0
        Try

            oLock = New bpmlock.User
            If oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            result = oLock.LockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID, sCurrentlyLockedBy:=r_sLockedBy, v_bOtherUserOnly:=False)


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally
            If Not (oLock Is Nothing) Then
                oLock.Dispose()
                oLock = Nothing
            End If



        End Try
        Return result
    End Function

    '************************************************************************************
    'unlock specified key
    '************************************************************************************
    Public Function UnLockKey(ByVal v_sKeyName As String, ByVal v_lKeyValue As Integer, ByVal v_lUserID As Integer) As Integer
        Dim oLock As bpmlock.User = Nothing
        Dim result As Integer = 0
        Try

            oLock = New bpmlock.User
            If oLock.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            result = oLock.UnLockKey(sKeyName:=v_sKeyName, vKeyValue:=v_lKeyValue, iUserID:=v_lUserID)


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock KeyName: " & v_sKeyName & " KeyValue: " & CStr(v_lKeyValue), vApp:=ACApp, vClass:=ACClass, vMethod:="LockKey", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally
            If Not (oLock Is Nothing) Then
                oLock.Dispose()
                oLock = Nothing
            End If


        End Try
        Return result
    End Function
    ''' <summary>
    ''' DeleteRenewal
    ''' </summary>
    ''' <param name="v_lRenewalInsuranceFileCnt"></param>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lRenewalStatusCnt"></param>
    ''' <param name="v_lInsuranceFolderCnt"></param>
    ''' <param name="v_lPartyCnt"></param>
    ''' <param name="v_sEventDesc"></param>
    ''' <param name="v_bStartTransaction"></param>
    ''' <param name="r_sFailureMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteRenewal(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRenewalStatusCnt As Integer, Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lPartyCnt As Integer = 0, Optional ByVal v_sEventDesc As String = "", Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer
        Dim oRenewalSelection As bSIRRenSelection.Business = Nothing
        Dim nResult As Integer = 0
        Dim dtRenPolDetails As New DataTable
        Try


            nResult = gPMConstants.PMEReturnCode.PMTrue

            If v_bStartTransaction Then
                If BeginTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If

                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
            End If

            oRenewalSelection = New bSIRRenSelection.Business
            If oRenewalSelection.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Throw New ApplicationException("CreateBusinessObject Failed to craete bSIRRenSelection.Business object. ")
            End If

            'delete work tasks
            m_lReturn = oRenewalSelection.DeleteWorkTask(v_sKeyName:="InsuranceFileKey", v_sKeyValue:=v_lRenewalInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete work task"
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'delete renewal version of the policy associates 
            m_lReturn = DeleteRenewalPolicyAssociates(v_lRenewalInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete Policy Associates"
                End If

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_lReturn = GetRenewalPolicyDetails(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, dtResult:=dtRenPolDetails)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                DeleteRenewal = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            'delete renewal version of the policy
            If Not (dtRenPolDetails IsNot Nothing AndAlso dtRenPolDetails.Rows.Count > 0) Then
                m_lReturn = oRenewalSelection.DeletePolicy(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete renewal version of policy"
                End If

                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'Remove any records in 'Last_Print_Run' linked to our Renewal_Status record.
            m_lReturn = DeleteLastPrintRun(v_lUserID:=m_iUserID, v_lRenewalStatusCnt:=v_lRenewalStatusCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete last print run records"
                End If

                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            m_lReturn = DeleteRenewalStatus(v_lRenewalStatusCnt:=v_lRenewalStatusCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to delete renewal status record"
                End If

                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If

            'change live policy status from (in renewal) back to (live)
            m_lReturn = SetPolicyStatus(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lInsuranceFileStatusID:=0, v_bStartTransaction:=False, r_sFailureMessage:=r_sFailureMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            'Parralel PN 52231
            'Update  policy last Modified date
            m_lReturn = SetModifiedDate(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_bStartTransaction:=False, r_sFailureMessage:=r_sFailureMessage)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
                Return nResult
            End If
            'do we need to create an event
            If v_lInsuranceFolderCnt <> 0 And v_lPartyCnt <> 0 Then
                Dim vEventType As Integer
                If v_sEventDesc = "Delete Renewal - " Then
                    vEventType = PMBConst.PMBEventRenewal
                Else
                    vEventType = 5
                End If
                m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vEventType:=vEventType, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:=v_sEventDesc)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to create event"
                    End If

                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
            End If

            If v_bStartTransaction Then
                If CommitTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    nResult = gPMConstants.PMEReturnCode.PMFalse
                    Return nResult
                End If
            End If

            Return nResult
        Catch excep As System.Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = Informations.Err().Description
            End If

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal - Renewal Status Count: " & v_lRenewalStatusCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        Finally

            If v_bStartTransaction Then
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollBackTransaction()
                End If
            End If
            If Not (oRenewalSelection Is Nothing) Then
                oRenewalSelection.Dispose()
                oRenewalSelection = Nothing
            End If

        End Try
        Return nResult
    End Function

    '*****************************************************************************
    ' 1. get policy version which is in renewal and renewal version of policy and renewal status count
    ' using v_lInsuranceFileCnt (any version of insurance file count will do)
    ' 2. delete renewal version of policy change status of policy in renewal back to live and delete all related details
    '*****************************************************************************
    Public Function DeletePolicyFromRenewal(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lInsuranceFileCnt, lRenewalInsuranceFileCnt, lRenewalStatusCnt, lInsuranceFolderCnt, lPartyCnt As Integer
        Dim sInsuranceRef As String = ""
        Dim lCnt As Integer
        Dim bFound As Boolean

        result = gPMConstants.PMEReturnCode.PMTrue

        Try
            sSQL = "spu_Get_Insurance_File_Details"
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameters to PMDAO (InsuranceFileCnt)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="Get Renewal details", bStoredProcedure:=True, vResultArray:=vResultArray, bKeepNulls:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to renewal policy details"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            bFound = False
            If Not Informations.IsArray(vResultArray) Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Cannot find renewal policy details"
                End If

                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            ElseIf vResultArray.GetLowerBound(1) > 0 Then
                ' search for passed iFileCnt
                For lCnt = 0 To vResultArray.GetUpperBound(1)
                    If v_lInsuranceFileCnt = CLng(vResultArray(2, lCnt)) Then
                        lInsuranceFileCnt = CLng(vResultArray(0, lCnt))
                        lRenewalStatusCnt = CLng(vResultArray(1, lCnt))
                        lRenewalInsuranceFileCnt = CLng(vResultArray(2, lCnt))
                        lInsuranceFolderCnt = CLng(vResultArray(3, lCnt))
                        lPartyCnt = CLng(vResultArray(4, lCnt))
                        sInsuranceRef = ToSafeString(vResultArray(5, lCnt))
                        bFound = True
                        Exit For
                    End If
                Next lCnt
            End If

            If bFound = False Then
                lInsuranceFileCnt = CInt(vResultArray(0, 0))

                lRenewalStatusCnt = CInt(vResultArray(1, 0))

                lRenewalInsuranceFileCnt = CInt(vResultArray(2, 0))

                lInsuranceFolderCnt = CInt(vResultArray(3, 0))

                lPartyCnt = CInt(vResultArray(4, 0))

                sInsuranceRef = gPMFunctions.ToSafeString(CStr(vResultArray(5, 0)))
            End If
            result = DeleteRenewal(v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lInsuranceFileCnt:=lInsuranceFileCnt, v_lRenewalStatusCnt:=lRenewalStatusCnt, v_bStartTransaction:=v_bStartTransaction, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_sEventDesc:="Delete Renewal - " & sInsuranceRef, r_sFailureMessage:=r_sFailureMessage)


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to delete policy from renewal - insurance_file_cnt : " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicyFromRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally

        End Try
        Return result
    End Function

    '*********************************************************************************
    'delete policy from renewal and lapse all versions of policy
    '*********************************************************************************
    Public Function LapseRenewal(ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRenewalStatusCnt As Integer, ByVal v_lLapseReasonID As Integer, ByVal v_sLapseReasonDesc As String, ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer


        Dim result As Integer = 0
        Dim sDescription As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            result = RunLapseRule(v_lInsuranceFileCnt, v_lInsuranceFolderCnt)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to Lapse the renewal."
                End If
                Return result
            End If
            If v_bStartTransaction Then
                m_lReturn = BeginTransaction()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_lReturn = DeleteRenewal(v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRenewalStatusCnt:=v_lRenewalStatusCnt, r_sFailureMessage:=r_sFailureMessage)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFolderCnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add param InsuranceFolderCnt"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateLapsePolicySQL, sSQLName:=ACUpdateLapsePolicyName, bStoredProcedure:=ACUpdateLapsePolicyStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to set status to lapsed for policy folder (" & v_lInsuranceFolderCnt & ")"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'Add the event
            sDescription = "Lapsed renewal (" & v_sLapseReasonDesc & ")"
            Dim vEventType As Integer
            If sDescription = "Lapsed renewal (" Then
                vEventType = PMBConst.PMBEventRenewal
            Else
                vEventType = 5
            End If
            m_lReturn = CreateEvent(v_vEventCnt:=0, v_vPartyCnt:=v_lPartyCnt, v_vInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentType:=DBNull.Value, v_vReportType:=DBNull.Value, v_vEventType:=vEventType, v_vUserId:=m_iUserID, v_vEventDate:=DateTime.Today, v_vDescription:=sDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to create event"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            'record that the live policy is now lapsed
            m_lReturn = LapsePolicy(v_lInsuranceFileCnt, v_lLapseReasonID, v_sLapseReasonDesc, v_lLivePolicy:=gPMConstants.PMEReturnCode.PMTrue)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update Policy Reason and Description"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            ' if Chase Cycle enabled
            m_lReturn = CType(GetSystemOptionLite(v_iOptionNumber:=kSystemOptionChaseCycleEnabled, r_sOptionValue:=sOptionValue, v_iSourceID:=1), gPMConstants.PMEReturnCode)

            If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                bChaseCycleEnabled = (sOptionValue = "1")
            End If

            If bChaseCycleEnabled Then
                ' Add a Chase Cycle item
                ' NB: if this policy is later moved onto instalments the Chase Cycle item
                ' will be deleted by the instalment process.
                m_sTransactionType = "RENLAP"
                m_lReturn = AddChaseCycleItem(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sBusinessType:=m_sTransactionType)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add Chase Cycle Item", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
                m_sTransactionType = Nothing
            End If
            'PN4538 - Start
            If CheckAndUpdateCommonRenewalDate(v_lInsuranceFileCnt:=v_lInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            'PN4538 - End

            If v_bStartTransaction Then
                m_lReturn = CommitTransaction()

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If


        Catch ex As Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = Informations.Err().Description
            End If

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lapse policy - insurance file cnt : " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="LapseRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)


        Finally

            If v_bStartTransaction Then
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollBackTransaction()
                End If
            End If



        End Try
        Return result
    End Function
    ''' <summary>
    ''' LapsePolicy
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_lLapseId"></param>
    ''' <param name="v_sLapseDesc"></param>
    ''' <param name="v_lLivePolicy"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function LapsePolicy(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLapseId As Integer, ByVal v_sLapseDesc As String, ByVal v_lLivePolicy As Integer) As Integer
        Const kMethodName As String = "LapsePolicy"
        Dim nResult As Integer = 0
        Dim sSQL As String = ""


        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Update the policy to be lapsed.

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PolicyCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LapseId", vValue:=v_lLapseId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter LapseId", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LivePolicy", vValue:=v_lLivePolicy, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter LivePolicy", gPMConstants.PMELogLevel.PMLogError)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LapseDesc", vValue:=v_sLapseDesc, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter LapseDesc", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_bIsMigratedPolicy = True Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="IsMigratedPolicy", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                'Check for any error
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(ACClass, kMethodName & " Fails to Add parameter IsMigratedPolicy", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdRenewalPolicyDetailsSQL,
                                                     sSQLName:=ACUpdRenewalPolicyDetailsName,
                                                     bStoredProcedure:=ACUpdRenewalPolicyDetailsStored)


            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Get Renewal Policy Details", gPMConstants.PMELogLevel.PMLogError)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception



            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set policy status to lapsed - insurance file count:" & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="LapsePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    '***************************************************************************************
    'get policies in renewal at the moment which await amendment/acceptance
    'v_lRenewalType = 0=all, 1=Amend, 2=Accept, 3=Invite
    '
    '***************************************************************************************
    Public Function GetRenewalPolicy(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByVal v_dRenewalDate As Date, ByVal v_lProductID As Integer, ByVal v_lBranchID As Integer, ByVal v_lRenewalType As Integer, ByVal v_lLeadAgentCnt As Integer, ByVal v_lAgentcode As Integer, ByRef r_vResult(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=If(v_lInsuranceFileCnt = 0, DBNull.Value, v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceRef", vValue:=If(v_sInsuranceRef = "", DBNull.Value, v_sInsuranceRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If v_sInsuranceRef <> "" Or v_lInsuranceFileCnt <> 0 Then

                'developer guide no. 85
                m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalDate", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalDate", vValue:=v_dRenewalDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ProductID", vValue:=If(v_lProductID = 0, DBNull.Value, v_lProductID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="BranchID", vValue:=If(v_lBranchID = 0, DBNull.Value, v_lBranchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalType", vValue:=v_lRenewalType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="LeadAgentCnt", vValue:=If(v_lLeadAgentCnt = 0, DBNull.Value, v_lLeadAgentCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="AgentCode", vValue:=If(v_lAgentcode = 0, DBNull.Value, v_lAgentcode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="UserID", vValue:=m_iUserID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelRenewalSQL, sSQLName:=ACSelRenewalName, bStoredProcedure:=ACSelRenewalStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResult, bKeepNulls:=False)

        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policy in renewal", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    '****************************************************************
    ' update policy status
    '****************************************************************
    Public Function SetPolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFileStatusID As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Insurance_File SET insurance_file_status_id = {InsuranceFileStatusID}" & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"

            If v_bStartTransaction Then
                If BeginTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_oDatabase.Parameters.Clear()


            'developer guide no. 85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileStatusID", vValue:=If(v_lInsuranceFileStatusID = 0, DBNull.Value, CStr(v_lInsuranceFileStatusID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileStatusID)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileCnt)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Policy Status", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update policy status"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If v_bStartTransaction Then
                If CommitTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If



        Catch ex As Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = "SetPolicyStatus() - Errored"
            End If

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get renewals which await notice print", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If v_bStartTransaction Then
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollBackTransaction()
                End If
            End If

        End Try
        Return result
    End Function

    '*****************************************************************
    ' add to last print run table
    '*****************************************************************
    Public Function AddLastPrintRun(ByVal v_lRenewalStatusCnt As Integer, ByVal v_iUserID As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="renewal_status_cnt", vValue:=CStr(v_lRenewalStatusCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result

            End If

            If m_oDatabase.Parameters.Add(sName:="UserId", vValue:=CStr(v_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result

            End If

            If m_oDatabase.SQLAction(sSQL:=ACAddLastPrintRunSQL, sSQLName:=ACAddLastPrintRunName, bStoredProcedure:=ACAddLastPrintRunStored) <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If



        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddLastPrintRun Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddLastPrintRun", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result
    End Function


    '*****************************************************************
    ' get renewal status for this policy
    'r_vResultArray(0) - renewal_status_type_id
    'r_vResultArray(1) - code
    'r_vResultArray(2) - description
    '*****************************************************************
    Public Function GetPolicyRenewalStatus(ByVal v_lRenewalStatusCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalStatusCnt", vValue:=CStr(v_lRenewalStatusCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add Renewal Status Count Param"
                Throw New Exception()
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPolicyRenewalStatusSQL, sSQLName:=ACGetPolicyRenewalStatusName, bStoredProcedure:=ACGetPolicyRenewalStatusStored, vResultArray:=r_vResultArray, bKeepNulls:=False)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            If sMessage = "" Then
                sMessage = "Failed to get policy renewal status"
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    '*****************************************************************
    'get plan details for this policy version
    '*****************************************************************
    Public Function GetSingleFinancePlanFromInsFileCnt(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vPFPremiumFinance As Object) As Integer
        Dim oPremiumFinance As bSIRPremiumFinance.Business = Nothing
        Dim result As Integer = 0
        Try

            oPremiumFinance = New bSIRPremiumFinance.Business
            If oPremiumFinance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If
            result = oPremiumFinance.GetSingleFinancePlanFromInsFileCnt(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vPFPremiumFinance:=r_vPFPremiumFinance)
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instalment plan for Insurance File Count " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRenewalStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        Finally
            If Not (oPremiumFinance Is Nothing) Then
                oPremiumFinance.Dispose()
                oPremiumFinance = Nothing
            End If
        End Try

    End Function

    '*****************************************************************************
    'is the flag midnight renewal set for this policy
    '*****************************************************************************
    Private Function IsMidnightRenewal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lIsMidNightRenewal As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing


        result = gPMConstants.PMEReturnCode.PMTrue
        r_lIsMidNightRenewal = gPMConstants.PMEReturnCode.PMFalse

        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            Return result
        End If

        If m_oDatabase.SQLSelect(sSQL:=ACSelIsMidNightRenewalSQL, sSQLName:=ACSelIsMidNightRenewalName, bStoredProcedure:=ACSelIsMidNightRenewalStored, vResultArray:=vResultArray, bKeepNulls:=True) <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse
            Return result

        End If

        If Informations.IsArray(vResultArray) Then
            r_lIsMidNightRenewal = gPMFunctions.NullToLong(vResultArray(0, 0))
        Else
            result = gPMConstants.PMEReturnCode.PMNotFound
        End If


        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetAllUserBranches
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : MEvans : 17-03-2005 : PN19562
    ' ***************************************************************** '
    Public Function GetAllUserBranches(ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAllUserBranches"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()


            ' Add Required Stored Procedure Parameters
            m_lReturn = AddInputParameter(v_sName:="user_id", v_vValue:=m_iUserID, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kGetAllUserBranchesSQL, sSQLName:=kGetAllUserBranchesName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kGetAllUserBranchesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object
        If v_vValue Is DBNull.Value Then
            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)
        Else
            lReturn = m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=CStr(v_vValue), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType)
        End If

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            result = gPMConstants.PMEReturnCode.PMFalse


            gPMFunctions.LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFunctionName & " Failed to add parameter name:" & v_sName &
                                  ", values :" & CStr(v_vValue) & ", type:" & CStr(v_iType), vApp:=ACApp, vClass:=ACClass, vMethod:=sFunctionName)

        End If

        Return result

    End Function



    '***********************************************************************************************************************
    ' during renewal amendment user can change policy details so we need to update renewal status table with these info
    '***********************************************************************************************************************
    Public Function UpdateRenewalStatus(ByVal v_lRenewalStatusCnt As Integer, ByRef r_sMessage As String) As Integer

        Dim result As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            result = m_oDatabase.Parameters.Add(sName:="RenewalStatusCnt", vValue:=CStr(v_lRenewalStatusCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to add param RenewalStatusCnt - UpdateRenewalStatus()"
                Return result
            End If

            result = m_oDatabase.SQLAction(sSQL:=ACUpdateRenewalStatusSQL, sSQLName:=ACUpdateRenewalStatusName, bStoredProcedure:=ACUpdateRenewalStatusStored)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to execute proc to update Renewal Status table"
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            r_sMessage = Informations.Err().Description

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalStatus()", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally


        End Try
        Return result

    End Function

    '****************************************************************************
    'transfer renewal version of policy to new broker and reset Renewal_Status.renewal_status_type_id to original value
    ' this will be stored in Renewal_Status.broker_xfer_status_type_id by renewal selection
    '****************************************************************************
    'developer guide no. 101
    Public Function TransferBroker(ByVal v_lRenewalInsuranceFileCnt As Object, ByVal v_lTransferToPartyCnt As Object) As Integer

        Dim result As Integer = 0
        Dim sFailMsg As String = ""

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="RenewalInsuranceFileCnt", vValue:=v_lRenewalInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add RenewalInsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If

            If v_lTransferToPartyCnt = "" OrElse v_lTransferToPartyCnt Is Nothing OrElse v_lTransferToPartyCnt = 0 Then
                v_lTransferToPartyCnt = DBNull.Value
            End If
            'developer guide no. 85
            'm_lReturn = m_oDatabase.Parameters.Add(sName:="TransferToPartyCnt", vValue:=If(v_lTransferToPartyCnt = 0 OrElse v_lTransferToPartyCnt = "", DBNull.Value, v_lTransferToPartyCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="TransferToPartyCnt", vValue:=v_lTransferToPartyCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add TransferToPartyCnt param"
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateTransferBrokerSQL, sSQLName:=ACUpdateTransferBrokerName, bStoredProcedure:=ACUpdateTransferBrokerStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to update database with new broker"
                Throw New Exception(sFailMsg)
            End If


        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            If sFailMsg = "" Then
                sFailMsg = "Failed to transfer broker"
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="GetValueFromTable", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function

    '***************************************************************************************
    'delete the agent_commission link for this policy version if its now a direct business
    '****************************************************************************************
    Public Function DeleteAgentcommission(ByVal v_lInsuranceFileCnt As Integer, Optional ByRef r_sFailMessage As String = "") As Integer

        Dim result As Integer = 0
        Dim sFailMsg As String = ""

        Try

            m_oDatabase.Parameters.Clear()
            result = gPMConstants.PMEReturnCode.PMTrue
            r_sFailMessage = ""

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to add InsuranceFileCnt param"
                Throw New Exception(sFailMsg)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelAgentCommissionSQL, sSQLName:=ACDelAgentCommissionName, bStoredProcedure:=ACDelAgentCommissionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to remove agent commission link for policy_id " & v_lInsuranceFileCnt
                Throw New Exception(sFailMsg)
            End If


        Catch ex As Exception

            result = gPMConstants.PMEReturnCode.PMError

            If sFailMsg = "" Then
                sFailMsg = "Failed to delete record from Agent_Commission for policy_id " & v_lInsuranceFileCnt
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteAgentcommission", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally
            If Not False Then
                r_sFailMessage = sFailMsg
            End If



        End Try
        Return result
    End Function


    Public Function ValidateAcceptTMPIsValidAction(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sInsuranceRef As String, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ValidateAcceptTMPIsValidAction"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' insurance file cnt
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' insurance ref
            m_lReturn = AddInputParameter(v_sName:="insurance_ref", v_vValue:=v_sInsuranceRef, v_iType:=gPMConstants.PMEDataType.PMString)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(sSQL:=kValidateAcceptTMPIsValidActionSQL, sSQLName:=kValidateAcceptTMPIsValidActionName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError(kMethodName, kValidateAcceptTMPIsValidActionSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)

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
    '
    ' Name: GetAgents
    '
    ' Description:
    '
    ' ***************************************************************** '
    Public Function GetAgents(ByRef r_vAgentArray(,) As Object, Optional ByRef v_lSourceID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lSourceID = 0 Then 'get all Agents
                sSQL = " SELECT PA.party_cnt, P.ShortName"
                sSQL = sSQL & " FROM Party_Agent PA"
                sSQL = sSQL & " Join Party P ON PA.Party_Cnt=P.Party_cnt"
                sSQL = sSQL & " ORDER BY P.ShortName"

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetAllAgents", bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vAgentArray)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else
                'get Agents specific to selected Branch

                m_oDatabase.Parameters.Clear()


                ' Add the branch parameter
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Branchid", vValue:=CStr(v_lSourceID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Call the stored procedure
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetBranchAgentsSQL, sSQLName:=ACGetBranchAgentsName, bStoredProcedure:=ACGetBranchAgentsStored, vResultArray:=r_vAgentArray, bKeepNulls:=True)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAgents Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAgents", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    'Get the AccountID for Agent/Client
    Public Function GetPolicyGrossTotal(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "GetPolicyGrossTotal"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("insurance_file_cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If
            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectPolicyGrossTotalSQL, sSQLName:=ACSelectPolicyGrossTotalName, bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError(kMethodName, ACSelectPolicyGrossTotalSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPolicyGrossTotal", r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    'Get the Agent Type for A Insurance File
    Public Function GetAgentType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "GetAgentType"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAgentCodeForInsuranceFileCntSQL, sSQLName:=ACSelectAgentCodeForInsuranceFileCntName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACSelectAgentCodeForInsuranceFileCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAgentType", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    Public Function GetAgentCommission(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "GetAgentCommission"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("Insurance_File_Cnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_sir_agent_commission_sel", sSQLName:="spu_sir_agent_commission_sel", bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "spu_sir_agent_commission_sel" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAgentCommission", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: GetProdPrintOptions
    '
    ' Parameters:
    '
    ' Description: Used to retrieve product risk options to determine
    '              whether to produce documents or not. Following are the
    '              documents it used to look for
    '                  1)produce_schedule
    '                  2)produce_certificate
    '                  3)produce_debit_note
    '
    ' History:
    '
    ' ***************************************************************** '
    Public Function GetProdPrintOptions(ByVal lproduct_id As Integer, ByRef vPrintOptions(,) As Object) As Integer

        Dim result As Integer = 0
        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=CStr(lproduct_id), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetProductPrintOptionsSQL, sSQLName:=ACGetProductPrintOptionsName, bStoredProcedure:=False, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vPrintOptions)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        Return result
    End Function

    Public Function DeleteCreditControlItem(ByRef v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DeleteCreditControlItem"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            lReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteCreditControlItemSQL, sSQLName:=ACDeleteCreditControlItemName, bStoredProcedure:=ACDeleteCreditControlItemStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACDeleteCreditControlItemName & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Public Function GetPaymentMethod(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "GetPaymentMethod"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("insurancefilecnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:="spu_GetPaymentMethod", sSQLName:="spu_GetPaymentMethod", bStoredProcedure:=True, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "spu_GetPaymentMethod" & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPaymentMethod", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    'Changes done by Krishna Nand PN: 70509 Dated: 31/03/2010
    Public Function UpdateCurrencyToInsuranceFile() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateCurrencyToInsuranceFile"

        Dim sFailMsg As String = ""

        Dim oCurrencyConvert As bACTCurrencyConvert.Form
        Dim lAccountID, lSourceID As Integer
        Dim iTransactionCurrencyID As Integer
        Dim cTransactionAmount As Decimal
        Dim iBaseCurrencyID As Integer
        Dim cBaseCurrentAmount As Decimal
        Dim iAccountCurrencyID As Integer
        Dim cAccountCurrentAmount As Decimal
        Dim iSystemCurrencyID As Integer
        Dim cSystemCurrentAmount As Decimal
        Dim dTransToBaseExchangeRate As Double
        Dim dtEffectiveDateOfExchange As Date
        Dim dAccountToBaseExchangeRate, dSystemToBaseExchangeRate As Double
        Dim iRateOverrideReasonID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            oCurrencyConvert = New bACTCurrencyConvert.Form
            If oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            iTransactionCurrencyID = m_oInsuranceFile.CurrencyID

            lSourceID = m_oInsuranceFile.SourceID

            If Convert.IsDBNull(dtEffectiveDateOfExchange) Or Informations.IsNothing(dtEffectiveDateOfExchange) Or dtEffectiveDateOfExchange = CDate("00:00:00") Then
                dtEffectiveDateOfExchange = DateTime.Today
            End If


            m_lReturn = oCurrencyConvert.DoCurrencyConversion(v_lAccountID:=lAccountID, v_lCompanyId:=lSourceID, v_iCurrencyID:=iTransactionCurrencyID, v_cCurrencyAmountUnrounded:=cTransactionAmount, r_iBaseCurrencyID:=iBaseCurrencyID, r_cBaseAmount:=cBaseCurrentAmount, r_iAccountCurrencyID:=iAccountCurrencyID, r_cAccountAmount:=cAccountCurrentAmount, r_iSystemCurrencyID:=iSystemCurrencyID, r_cSystemAmount:=cSystemCurrentAmount, r_dCurrencyBaseXrate:=dTransToBaseExchangeRate, r_dtCurrencyBaseDate:=dtEffectiveDateOfExchange, r_dAccountBaseXrate:=dAccountToBaseExchangeRate, r_dtAccountBaseDate:=dtEffectiveDateOfExchange, r_dSystemBaseXrate:=dSystemToBaseExchangeRate, r_dtSystemBaseDate:=dtEffectiveDateOfExchange)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
                Throw New Exception
            End If

            'A zero here means that the exchange rates have not been set up
            If dTransToBaseExchangeRate = 0 Or dSystemToBaseExchangeRate = 0 Then
                result = gPMConstants.PMEReturnCode.PMFalse

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
                Throw New Exception
            End If



            m_lReturn = oCurrencyConvert.UpdateInsuranceFile(v_lInsuranceFileCnt:=m_oInsuranceFile.InsuranceFileCnt, v_dCurrencyBaseXrate:=dTransToBaseExchangeRate, v_dtCurrencyBaseDate:=dtEffectiveDateOfExchange, v_dAccountBaseXrate:=0, v_dtAccountBaseDate:=#12/30/1899#, v_dSystemBaseXrate:=dSystemToBaseExchangeRate, v_dtSystemBaseDate:=dtEffectiveDateOfExchange, v_lRateOverrideReasonID:=iRateOverrideReasonID, v_iBaseCurrencyID:=iBaseCurrencyID, v_iAccountCurrencyID:=0)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sFailMsg = "Failed to update Insurance File Data"

                oCurrencyConvert.Dispose()
                oCurrencyConvert = Nothing
                Throw New Exception(sFailMsg)
            End If


            oCurrencyConvert.Dispose()
            oCurrencyConvert = Nothing

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            If sFailMsg = "" Then
                sFailMsg = "Failed to Update Currency "
            End If

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sFailMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

        End Try
        Return result
    End Function
    'End of Changes
    'End of Changes

    ' Update Lapsed Last Modified Date
    ' Parralel PN 52231
    Public Function SetModifiedDate(ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE insurance_file_system  SET last_modified ='" & DateTime.Now.ToString("yyyy-MM-dd HH:MM:ss") & "' " & Strings.ChrW(13) & Strings.ChrW(10)
            sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"

            If v_bStartTransaction Then
                If BeginTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If

                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileCnt)"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Policy Modified Date ", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update policy modified date"
                End If

                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            If v_bStartTransaction Then
                If CommitTransaction() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If



        Catch ex As Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = "SetModifiedDate() - Errored"
            End If

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get renewals which await notice print", vApp:=ACApp, vClass:=ACClass, vMethod:="SetModifiedDate", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally

            If v_bStartTransaction Then
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = RollBackTransaction()
                End If
            End If


        End Try
        Return result
    End Function

    Public Function GetTransAccountId(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResults(,) As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "GetTransAccountId"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Throw New Exception
            End If

            lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransAccountIdForInsuranceFileCntSQL, sSQLName:=ACGetTransAccountIdForInsuranceFileCntName, bStoredProcedure:=False, vResultArray:=r_vResults, lNumberRecords:=gPMConstants.PMAllRecords)


            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, ACGetTransAccountIdForInsuranceFileCntSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetTransAccountId", r_lFunctionReturn:=result, excep:=ex)
        Finally



        End Try
        Return result
    End Function

    'Start - Written Status.doc
    Public Function WriteRenewal(ByVal v_lOldInsuranceFileCnt As Long,
                                    ByVal v_lNewInsuranceFileCnt As Long,
                                    ByVal v_lRenewalStatusCnt As Long,
                                    Optional ByVal v_sNewPolicyRef As String = "",
                                    Optional ByVal v_dNewStartDate As Date = #12/29/1899#,
                                    Optional ByVal v_dNewExpiryDate As Date = #12/29/1899#,
                                    Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Long

        Const kMethodName As String = "WriteRenewal"
        Dim sInsFileTypeIdPolicy As String = String.Empty
        Dim lIsMidNightRenewal As Long
        Try
            WriteRenewal = gPMConstants.PMEReturnCode.PMTrue

            'get insurance_file_type_id for insurance file type (POLICY)
            m_lReturn = GetValueFromTable(v_sTableName:="Insurance_File_Type",
                                        v_vReturnColumn:="insurance_file_type_id",
                                        v_sKeyColumn:="Code",
                                        v_sKeyValue:="WRITTEN",
                                        v_iDataType:=gPMConstants.PMEDataType.PMString,
                                        r_vResult:=sInsFileTypeIdPolicy)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to get insurance file type id for type POLICY"
                End If
                RaiseError(kMethodName, "GetValueFromTable Failed", gPMConstants.PMEReturnCode.PMError)
            End If

            'get details for renewal version of policy
            m_oInsuranceFile.InsuranceFileCnt = v_lNewInsuranceFileCnt

            m_lReturn = m_oInsuranceFile.GetDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to get policy details for renewal version, insurance file count : " & v_lNewInsuranceFileCnt
                End If
                RaiseError(kMethodName, "GetDetails Failed", gPMConstants.PMEReturnCode.PMError)
            End If

            'change renewal version to type (POLICY) and status to (LIVE)
            m_oInsuranceFile.InsuranceFileStatusID = Nothing
            m_oInsuranceFile.InsuranceFileTypeID = ToSafeLong(sInsFileTypeIdPolicy)

            'change insurance_ref and cover period if required
            If v_sNewPolicyRef <> "" Then
                m_oInsuranceFile.InsuranceRef = v_sNewPolicyRef
            End If

            If Informations.DateDiff("d", v_dNewStartDate, #12/29/1899#, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1) <> 0 Then
                m_oInsuranceFile.CoverStartDate = v_dNewStartDate
            End If

            If Informations.DateDiff("d", v_dNewExpiryDate, #12/29/1899#, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1) <> 0 Then
                m_oInsuranceFile.ExpiryDate = v_dNewExpiryDate
                m_lReturn = IsMidnightRenewal(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                r_lIsMidNightRenewal:=lIsMidNightRenewal)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "IsMidnightRenewal Failed", gPMConstants.PMEReturnCode.PMError)
                End If

                'renewal date is one more then expiry date in the case of midnight renewal
                If lIsMidNightRenewal = 1 Then
                    m_oInsuranceFile.RenewalDate = Informations.DateAdd("d", 1, v_dNewExpiryDate)
                Else
                    m_oInsuranceFile.RenewalDate = v_dNewExpiryDate
                End If
            End If

            'make renewal version WRITTEN
            m_lReturn = m_oInsuranceFile.UpdatePolicy()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update insurance file system, insurance file count :" & v_lNewInsuranceFileCnt
                End If
                RaiseError(kMethodName, "UpdatePolicy Failed", gPMConstants.PMEReturnCode.PMError)
            End If

            'delete record from of renewal status table
            m_lReturn = SetRenewalStatusTypeID(v_lRenewalInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                        v_lRenewalStatusTypeID:=PMBRenewalStatusTypeWrittenAwaitUpdate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to change renewal status, renewal status count :" & v_lRenewalStatusCnt
                End If
                RaiseError(kMethodName, "SetRenewalStatusTypeID Failed", gPMConstants.PMEReturnCode.PMError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=WriteRenewal, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Function
    'End  Written Status.doc
    'Start  Written Status.doc
    Public Function IsWrittenUsed(Optional ByVal v_lProductId As Long = 0) As Long

        Const kMethodName As String = "IsWrittenUsed"

        Dim vWrittenStatus(,) As Object = Nothing

        Try
            IsWrittenUsed = gPMConstants.PMEReturnCode.PMTrue



            ' AddParameterLite is missing

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_Id",
                                                vValue:=v_lProductId,
                                                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Adding the Parameter v_lProductId failed", gPMConstants.PMEReturnCode.PMError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetWrittenStatusUsedSQL,
                                                sSQLName:=ACGetWrittenStatusUsedName,
                                                bStoredProcedure:=ACGetWrittenStatusUsedStored,
                                                vResultArray:=vWrittenStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "IsWrittenUsed failed ", gPMConstants.PMEReturnCode.PMError)
            End If

            If Informations.IsArray(vWrittenStatus) Then
                IsWrittenUsed = gPMConstants.PMEReturnCode.PMNotFound
            Else
                IsWrittenUsed = gPMConstants.PMEReturnCode.PMTrue
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=IsWrittenUsed, excep:=ex)
            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Function
    'End -  Written Status.doc

    'PN4538 - Start
    Private Function CheckAndUpdateCommonRenewalDate(ByVal v_lInsuranceFileCnt As Long) As Long

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateCommonRenewalDateSQL,
                                sSQLName:=ACUpdateCommonRenewalDateName,
                                bStoredProcedure:=ACUpdateCommonRenewalDateStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result


    End Function
    'PN4538 - End

    ' ***************************************************************** '
    ' Name: RunLapseRule
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function RunLapseRule(ByVal iInsuranceFileCnt As Integer, ByVal iInsuranceFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim oRiskData As bSIRRiskData.Business, iReturn As Integer
        Dim oRiskArray(,) As Object = Nothing
        Dim oGisPolicyLinkArray(,) As Object = Nothing
        Try

            Const ACRiskPosCnt As Integer = 0
            Dim iTransactionType, iQuoteType As Integer

            Dim sXMLDataSetDef As String = String.Empty
            Dim sXMLDataSet As String = String.Empty
            Dim oGIS As bGIS.Application
            result = gPMConstants.PMEReturnCode.PMTrue
            oRiskData = New bSIRRiskData.Business
            m_lReturn = CType(oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If oRiskData.GetRisk(v_lInsuranceFileCnt:=iInsuranceFileCnt, r_vResultArray:=oRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            For lCount As Integer = 0 To oRiskArray.GetUpperBound(1)

                m_lReturn = oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=iInsuranceFolderCnt, v_lRiskID:=oRiskArray(ACRiskPosCnt, lCount), r_vResultArray:=oGisPolicyLinkArray)


                'do we have any data
                Dim obj As Object = oGisPolicyLinkArray(0, 0)

                If Not (Convert.IsDBNull(obj) Or Informations.IsNothing(obj)) Then
                    'Make sure GIS object present.
                    m_oDataSet = New cGISDataSetControl.Application()
                    oGIS = New bGIS.Application
                    iReturn = oGIS.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                    Dim sDataModelCode As String = CStr(oGisPolicyLinkArray(4, 0)).Trim()
                    Dim iRiskID As Integer = oRiskArray(0, lCount)
                    Dim iPolicyLinkId As Integer = oGisPolicyLinkArray(0, 0)

                    iReturn = oGIS.LoadFromDB(r_sXMLDataSetDef:=sXMLDataSetDef,
                                              r_sXMLDataset:=sXMLDataSet,
                                              v_sGisDataModelCode:=sDataModelCode,
                                              r_vInsuranceFileCnt:=iInsuranceFileCnt,
                                              r_vPolicyLinkID:=iPolicyLinkId)

                    ' Load Data as XML
                    iReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    m_lReturn = GetLapsedRenewalTranType()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    iTransactionType = m_iTransactionID ' for Renewal Lapse

                    'run renewal script
                    EncodeTransactionScreenAndType(r_lEncoded:=iQuoteType, r_lTransactionType:=iTransactionType, r_lGISScreenId:=0, r_lQuoteType:=PBCQemQuoteTypeRenewalLapse)
                    'PBCQemQuoteTypeRenewalLapse Declared in SharedFiles
                    m_lReturn = GIS_NBQuote(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim(),
                                                  v_lQuoteType:=iQuoteType,
                                                  r_sXMLDataSet:=sXMLDataSet,
                                                  r_sXMLDataSetDef:=sXMLDataSetDef)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = GIS_SaveToDB(v_sGisDataModelCode:=CStr(oGisPolicyLinkArray(4, 0)).Trim())
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If

            Next

        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return result
        Finally
            oRiskData = Nothing
            oRiskArray = Nothing
            oGisPolicyLinkArray = Nothing
        End Try
        Return result
    End Function
    Public Function GIS_NBQuote(ByRef v_sGisDataModelCode As String, ByRef v_lQuoteType As Integer, ByRef r_sXMLDataSet As String, ByRef r_sXMLDataSetDef As String) As Integer


        Dim iresult As Integer = 0
        Dim iReturn As Integer
        Dim sDataModelCode As String = ""
        Dim oGIS As bGIS.Application

        Try
            'Make sure GIS object present.
            iresult = gPMConstants.PMEReturnCode.PMTrue
            oGIS = New bGIS.Application
            iReturn = oGIS.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RFC300300 - Clear all Quote Output that may already exist
            ' as there is no need to Pass it back across the network.
            iReturn = m_oDataSet.ClearAllQuoteOutput()
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            iReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oGIS.NBQuote(v_sGisDataModelCode:=v_sGisDataModelCode, v_lQuoteType:=v_lQuoteType, v_sGisBusinessTypeCode:="NB", v_dtEffectiveDate:=DateTime.Today, r_sXMLDataset:=r_sXMLDataSet)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the NBQuote Results
            iReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return iresult

        Catch excep As System.Exception

            iresult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_NBQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_NBQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return iresult
        Finally
            oGIS = Nothing
        End Try
    End Function


    Public Function GIS_SaveToDB(ByVal v_sGisDataModelCode As String) As Integer

        Dim iresult As Integer = 0
        Dim sXMLDataSetDef As String = String.Empty
        Dim sXMLDataSet As String = String.Empty
        Dim iReturn As Integer
        Dim oGIS As bGIS.Application

        Try
            'Make sure GIS object present.
            iresult = gPMConstants.PMEReturnCode.PMTrue
            oGIS = New bGIS.Application
            iReturn = oGIS.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Data as an XML String
            iReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Save it to the DataBase

            iReturn = oGIS.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Saved to DB Results
            iReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
            If iReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'destroy gis object
            If Not (oGIS Is Nothing) Then

                oGIS.Dispose()
                oGIS = Nothing
            End If

            Return iresult

        Catch excep As System.Exception

            iresult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GIS_SaveToDB Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GIS_SaveToDB", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return iresult
        Finally
            oGIS = Nothing
        End Try
    End Function
    ' Name: 
    '
    ' Description: Get TransactionType for lapsed renewal
    '*****************************************************************
    Public Function GetLapsedRenewalTranType() As Integer
        Dim iresult As Integer = 0
        Dim sSQL As String = ""
        Dim oResultArray(,) As Object = Nothing
        Dim sTransTypeDescription As String = ""

        Try
            iresult = gPMConstants.PMEReturnCode.PMTrue
            sSQL = "SELECT transaction_type_id, description FROM Transaction_Type WHERE LTRIM(RTRIM(code)) = 'RENLAP' "

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTransactionTypeDetails", bStoredProcedure:=False, vResultArray:=oResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(oResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            m_iTransactionID = gPMFunctions.ToSafeInteger(oResultArray(0, 0))

            sTransTypeDescription = gPMFunctions.ToSafeString(oResultArray(1, 0))

        Catch excep As System.Exception
            iresult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLapsedRenewalTranType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLapsedRenewalTranType", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return iresult
        End Try
        Return iresult
    End Function
    Private Function AddChaseCycleItem(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sBusinessType As String) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            result = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .Parameters.Add(sName:="business_type", vValue:=v_sBusinessType, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            result = .SQLAction(sSQL:=ACAddChaseCycleItemInsuranceFileSQL, sSQLName:=ACAddChaseCycleItemInsuranceFileName, bStoredProcedure:=ACAddChaseCycleItemInsuranceFileStored)

        End With

        Return result

    End Function
    Public Function GetSystemOptionLite(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, ByVal v_iSourceID As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSystemOptionLite"

        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lReturn = CType(bPMFunc.GetSystemOption(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_iOptionNumber:=v_iOptionNumber, r_sOptionValue:=r_sOptionValue, v_iSourceID:=v_iSourceID), gPMConstants.PMEReturnCode)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSystemOption Failed", gPMConstants.PMELogLevel.PMLogError)
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
    'Ashwani - (RFC_Enable_PrePayment_functionality)
    Public Function GetPrePaymentOptionValue(ByVal v_lproductid As Integer, ByRef r_Prepayment(,) As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetPrePaymentOptionValue"

        Dim lReturn As gPMConstants.PMEReturnCode


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            lReturn = CType(AddInputParameter(v_sName:="product_id", v_vValue:=v_lproductid, v_iType:=gPMConstants.PMEDataType.PMInteger), gPMConstants.PMEReturnCode)

            ' Execute selection Query
            lReturn = m_oDatabase.SQLSelect(sSQL:=kGetPrepaymentOPtionValSQL, sSQLName:=kGetPrepaymentOPtionVal, bStoredProcedure:=True, vResultArray:=r_Prepayment, lNumberRecords:=gPMConstants.PMAllRecords)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PrePaymentOptionValue Failed", gPMConstants.PMELogLevel.PMLogError)
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

    '  ''' <summary>
    '  ''' GetAnnivPriorVersionInsFileCnt
    ''  ''' </summary>
    '  '  ''' <param name="nFolderCnt"></param>
    '  '  ''' <param name="nPolicyCnt"></param>
    '  '  ''' <param name="r_vFileCntArray"></param>
    '  ''' <returns></returns>
    '  ''' <remarks></remarks>
    Public Function GetAnnivPriorVersionInsFileCnt(ByVal nFolderCnt As Integer, ByVal nPolicyCnt As Integer, ByRef r_oFileCntArray(,) As Object) As Integer

        Dim sSQL As String = ""
        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMFalse

            m_oDatabase.Parameters.Clear()

            nResult = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt",
                                                vValue:=nPolicyCnt,
                                                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMInteger)

            nResult = m_oDatabase.Parameters.Add(sName:="nInsuranceFolderCnt",
                                                vValue:=nFolderCnt,
                                                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            'parameters are added sucessfully
            nResult = m_oDatabase.SQLSelect(sSQL:=kGetAnnivPriorVersionInsFileCntSQL,
                                                    sSQLName:=kGetAnnivPriorVersionInsFileCntName,
                                                    bStoredProcedure:=True,
                                                    vResultArray:=r_oFileCntArray)

            Return nResult

        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAnnivPriorVersionInsFileCnt", r_lFunctionReturn:=GetAnnivPriorVersionInsFileCnt, excep:=ex)

            Return nResult

        End Try

    End Function
    ''' <summary>
    ''' GetRenewalPolicyDetails
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="dtResult"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetRenewalPolicyDetails(ByVal v_lInsuranceFileCnt As Long, Optional ByRef dtResult As DataTable = Nothing) As Integer

        Const kMethodName As String = "GetRenewalPolicyDetails"

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Add parameter
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Add parameter Insurance_File_Cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetRenewalPolicyDetailsSQL, bStoredProcedure:=ACGetRenewalPolicyDetailsStored, sSQLName:=ACGetRenewalPolicyDetailsName, oRecordset:=dtResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
            'Check for any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACClass, kMethodName & " Fails to Get Renewal Policy Details", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                m_bIsMigratedPolicy = True
            End If
        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetRenewalPolicyDetails, excep:=ex)
            nResult = gPMConstants.PMEReturnCode.PMError

        Finally

        End Try
        Return nResult
    End Function

    Private Function ProcessUpdatePolicy(v_lInsuranceFileCnt As Long) As Long
        Dim lReturn As Long
        Dim vRisks As Object = Nothing
        Dim m_oChangePolicyStatus As bSIRChangePolicyStatus.Business
        Const kMethodName As String = "ProcessUpdatePolicy"
        Dim nResult As Integer = 0


        nResult = gPMConstants.PMEReturnCode.PMTrue
        ProcessUpdatePolicy = gPMConstants.PMEReturnCode.PMTrue

        m_oChangePolicyStatus = New bSIRChangePolicyStatus.Business
        If m_oChangePolicyStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed to get instance of bSIRChangePolicyStatus.Business", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Get risks associated with this insurance file
        lReturn = m_oChangePolicyStatus.GetRisksByStatus(
                  v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                  r_vRisks:=vRisks)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "GetRisksByStatus Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            nResult = gPMConstants.PMEReturnCode.PMFalse
        End If

        If Informations.IsArray(vRisks) Then
            lReturn = m_oChangePolicyStatus.DeleteRisks(v_vrisks:=vRisks)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "DeleteRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                nResult = gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Re-jig the risk and variation numbers of the remaining
            '            risks on this policy
            lReturn = m_oChangePolicyStatus.RenumberRisks(
                      v_lInsuranceFileCnt:=v_lInsuranceFileCnt)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "RenumberRisks Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        End If

        Exit Function
        Return nResult
    End Function

    ''' <summary>
    ''' ValidateAcceptTMPAnniversaryIsValidAction
    ''' </summary>
    ''' <param name="v_lInsuranceFileCnt"></param>
    ''' <param name="v_dRenewalDate"></param>
    ''' <param name="r_vResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ValidateAcceptTMPAnniversaryIsValidAction(
                        ByVal v_lInsuranceFileCnt As Integer,
                        ByVal v_dRenewalDate As Date,
                        ByRef r_vResults As Object) As Integer

        Const kMethodName As String = "ValidateAcceptTMPAnniversaryIsValidAction"



        ValidateAcceptTMPAnniversaryIsValidAction = gPMConstants.PMEReturnCode.PMTrue
        Try


            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters

            ' insurance file cnt
            m_lReturn = AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=v_lInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' insurance ref
            m_lReturn = AddInputParameter(v_sName:="renewal_date", v_vValue:=v_dRenewalDate, v_iType:=gPMConstants.PMEDataType.PMDate)

            ' Execute selection Query
            If m_oDatabase.SQLSelect(
                                    sSQL:="spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action",
                                    sSQLName:="spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action",
                                    bStoredProcedure:=True,
                                    vResultArray:=r_vResults,
                                    lNumberRecords:=gPMConstants.PMAllRecords) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New ApplicationException("spu_SIR_Get_Is_Accept_TMP_Anniversary_Valid_Action" & " Failed")


            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(
                 v_sUsername:=m_sUsername,
                 v_sClass:=ACClass,
                 v_sMethod:=kMethodName,
                 r_lFunctionReturn:=ValidateAcceptTMPAnniversaryIsValidAction)

            ' If you want to rollback a transaction or something, do it here
        End Try
    End Function

    ''' <summary>
    ''' his method is used to Delete the policy associates of Renewal version Only
    ''' </summary>
    ''' <param name="nInsuranceRenewalFileCnt"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DeleteRenewalPolicyAssociates(ByRef nInsuranceRenewalFileCnt As Integer) As Integer
        Dim nReturn As gPMConstants.PMEReturnCode = PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        nReturn = m_oDatabase.Parameters.Add(sName:="nInsuranceFileCnt", vValue:=CStr(nInsuranceRenewalFileCnt), iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
        nReturn = m_oDatabase.SQLAction(sSQL:=kDeletepolicyAssociatesSQL, sSQLName:=kDeletepolicyAssociatesFileName, bStoredProcedure:=kDeletepolicyAssociatesStored)

        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Throw New ApplicationException(" DeleteRenewalPolicyAssociates Failed")
        End If

        Return nReturn

    End Function
    ''' <summary>
    ''' Get Premium Finance attached to the insurance file
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="aoResults"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPFPlanForInsFile(ByVal nInsuranceFileCnt As Integer, ByRef aoResults(,) As Object) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "GetPFPlanForInsFile"
        Try
            m_oDatabase.Parameters.Clear()
            m_oDatabase.Parameters.Add("financeplancnt", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add("financeplanversion", DBNull.Value, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            m_oDatabase.Parameters.Add("insurancefilecnt", nInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            nResult = m_oDatabase.SQLSelect(sSQL:=KSelectSinglePFForInsSQL, sSQLName:=KSelectSinglePFForInsName, bStoredProcedure:=KSelectSinglePFForInsStored, vResultArray:=aoResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, KSelectSinglePFForInsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            nResult = PMEReturnCode.PMFalse
        End Try
        Return nResult
    End Function

End Class
