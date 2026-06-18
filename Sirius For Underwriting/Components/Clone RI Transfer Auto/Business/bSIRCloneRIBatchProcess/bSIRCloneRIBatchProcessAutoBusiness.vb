Option Strict Off
Option Explicit On
'refer Developer Guide No. 129
Imports System.Data
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 14/03/2011
    '
    ' Description: Creatable Business class which contains all the
    '              methods, Business rules required to manipulate
    '              a SIRRenewal.
    '
    ' Edit History:
    ' ***************************************************************** '


    ' ************************************************
    ' Added to replace global variables 14/03/2011
    Private m_sUsername As String

    Private m_sPassword As String

    Private m_iUserID As Short

    Private m_sCallingAppName As String
    Private m_iSourceID As Short
    Private m_iLanguageID As Short
    Private m_iCurrencyID As Short
    Private m_iLogLevel As Short
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
    Private m_iTask As Short
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String
    Private m_dtEffectiveDate As Date
    Private m_oRenSelection As bSIRRenSelection.Business
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_oReinsurance As Object
    Private m_oTax As bSIRRITax.Business
    Private m_oRiskData As bSIRRiskData.Business
    Private m_oPerilAllocation As bSirPerilAllocation.Business
    Private m_oAgentCommission As bSirAgentCommission.Business
    Private m_oControlTrans As bControlTrans.Automated
    Private m_bIsRI2007Enabled As Boolean

    ' PRIVATE Data Members (End)

    ' PUBLIC Property Procedures (Begin)

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            PMProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property

    Public ReadOnly Property Task() As Short
        Get

            Task = m_iTask

        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get

            Navigate = m_lNavigate

        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get

            ProcessMode = m_lProcessMode

        End Get
    End Property

    Public ReadOnly Property TransactionType() As String
        Get

            TransactionType = m_sTransactionType

        End Get
    End Property

    Public ReadOnly Property EffectiveDate() As Date
        Get

            EffectiveDate = m_dtEffectiveDate

        End Get
    End Property

    Public WriteOnly Property RI2007Enabled() As Boolean
        Set(ByVal Value As Boolean)

            m_bIsRI2007Enabled = Value

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

            'create relevant business objects to do ProcessSingleDefRIPolicy()
            m_lReturn = CType(CreateClonedRIBusinessObject(), gPMConstants.PMEReturnCode)

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

    'UPGRADE_NOTE: Class_Initialize was upgraded to Class_Initialize_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'
    Private Sub Class_Initialize_Renamed()


    End Sub

    Public Sub New()
        MyBase.New()



    End Sub
    'UPGRADE_NOTE: Class_Terminate was upgraded to Class_Terminate_Renamed. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="A9E4979A-37FA-4718-9994-97DD76ED70A7"'

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub


    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   UpdateInsFileClonedRIUsage
    ' AUTHOR:
    ' DATE:
    ' ---------------------------------------------------------------------------
    Public Function UpdateInsFileClonedRIUsage(ByVal v_lInsFileClonedRIUsageID As Integer, ByVal v_lInsFileCnt As Integer, ByVal v_lClonedRIStatusID As Integer) As Integer

        Const kMethodName As String = "UpdateInsFileClonedRIUsage"

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_cloned_RI_usage_id", vValue:=v_lInsFileClonedRIUsageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cloned_RI_status_type_id", vValue:=v_lClonedRIStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' do the update call
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdCloneRIPoliciesSQL, sSQLName:=ACUpdCloneRIPoliciesName, bStoredProcedure:=ACUpdCloneRIPoliciesStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateInsFileClonedRIUsage = m_lReturn
                Exit Function
            End If

            UpdateInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMTrue

        Catch exec As System.Exception
            UpdateInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMTrue
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateInsFileClonedRIUsage, excep:=exec)

            ' If you want to rollback a transaction or something, do it here

        End Try

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   UpdateRiskRIModel
    ' DESCRIPTION: Update the risk's RI_Model if a new one has come into effect
    ' ---------------------------------------------------------------------------
    Public Function UpdateRiskRIModel(ByVal v_lInsFileCnt As Integer, ByVal v_lRiskCnt As Integer) As Integer

        Const kMethodName As String = "UpdateRiskRIModel"
        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateRiskRIModel = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateRiskRIModel = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' do the call to insert the record into Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateRiskRIModelSQL, sSQLName:=ACUpdateRiskRIModelName, bStoredProcedure:=ACUpdateRiskRIModelStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateRiskRIModel = m_lReturn
                Exit Function
            End If

            UpdateRiskRIModel = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            UpdateRiskRIModel = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateRiskRIModel, excep:=ex)

        End Try

    End Function
    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   GetRiskStatus
    ' AUTHOR:
    ' DATE:
    ' DESCRIPTION: Get the 'risk_status' of a risk (doh)
    ' ---------------------------------------------------------------------------
    Public Function GetRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer

        Dim dtResult As New DataTable

        Const kMethodName As String = "GetRiskStatus"

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", CLng(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' get the risk status of the risk
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetRiskStatusSQL, sSQLName:=ACGetRiskStatusName, bStoredProcedure:=ACGetRiskStatusStored, oRecordset:=dtResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetRiskStatus = m_lReturn
                Exit Function
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lRiskStatusID = CInt(dtResult.Rows(0)(0).ToString)
                r_sRiskStatusCode = CStr(dtResult.Rows(0)(1).ToString)
            Else
                GetRiskStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            GetRiskStatus = gPMConstants.PMEReturnCode.PMTrue

        Catch exec As System.Exception

            GetRiskStatus = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetRiskStatus, excep:=exec)
        End Try

    End Function

    Private Function CreateClonedRIBusinessObject() As Integer

        Const kMethodName As String = "CreateClonedRIBusinessObject"
        Dim sMessage As String
        Dim vValue As Object = Nothing
        Try

            CreateClonedRIBusinessObject = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""


            m_oRenSelection = New bSIRRenSelection.Business
            m_lReturn = m_oRenSelection.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRenSelection.Business"
                Throw New Exception()
            End If


            m_oInsuranceFile = New bSIRInsuranceFile.Services
            m_lReturn = m_oInsuranceFile.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRInsuranceFile.Services"
                Throw New Exception()
            End If

            ' Check the product option
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=ACApp, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableRI2007, v_vBranch:=m_iSourceID, r_vUnderwriting:=vValue)

            'UPGRADE_WARNING: Couldn't resolve default property of object vValue. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            If vValue = "1" Then
                m_bIsRI2007Enabled = True
            Else
                m_bIsRI2007Enabled = False
            End If



            If m_bIsRI2007Enabled = True Then
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            Else
                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRReinsurance.Form"
                Throw New Exception()
            End If


            m_oTax = New bSIRRITax.Business
            m_lReturn = m_oTax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRITax.Business"
                Throw New Exception()
            End If

            'changes done in name bSIRRiskData.business to bSIRRiskData.Business due to dll issue

            m_oRiskData = New bSIRRiskData.Business
            m_lReturn = m_oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRiskData.Business"
                Throw New Exception()
            End If

            'changes done in name bSIRPerilAllocation.Business to bSirPerilAllocation.Business due to dll issue

            m_oPerilAllocation = New bSirPerilAllocation.Business
            m_lReturn = m_oPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSirPerilAllocation.Business"
                Throw New Exception()
            End If

            'changes done in name bSIRAgentCommission.Business to bSirAgentCommission.Business due to dll issue

            m_oAgentCommission = New bSirAgentCommission.Business
            m_lReturn = m_oAgentCommission.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRAgentCommission.Business"
                Throw New Exception()
            End If



            m_oControlTrans = New bControlTrans.Automated
            m_lReturn = m_oControlTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bControlTrans.Automated"
                Throw New Exception()
            End If

        Catch exec As System.Exception
            CreateClonedRIBusinessObject = gPMConstants.PMEReturnCode.PMFalse
            Call CloseCloneRIBusinessObject()
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CreateClonedRIBusinessObject, excep:=exec)

        End Try

    End Function
    ''' <summary>
    ''' process clone on singe policy
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="sInsuranceFileType"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ProcessSingleClonedRIPolicy(ByVal nInsuranceFileCnt As Integer, ByVal sInsuranceFileType As String) As Integer
        Const kMethodName As String = "ProcessSingleClonedRIPolicy"

        Dim sMessage As String = ""

        Dim sTransactionType As String
        Dim nReturn As Integer
        Dim nIsValid As Integer
        Try

            ProcessSingleClonedRIPolicy = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""

            sTransactionType = "DRI"

            'set transaction so we can roll back if something gone wrong
            If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                ProcessSingleClonedRIPolicy = gPMConstants.PMEReturnCode.PMFalse
                sMessage = "Failed to begin SQLTransaction"
                RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If (sInsuranceFileType = "QUOTE" Or sInsuranceFileType = "RENEWAL" Or sInsuranceFileType = "MTAQUOTE" Or m_oInsuranceFile.InsuranceFileType = "MTAQTETEMP" Or sInsuranceFileType = "MTAQREINS") Then
                m_lReturn = RecalculateRIQuote(nInsuranceFileCnt:=nInsuranceFileCnt,
                                      nIsValid:=nIsValid)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to recalculate RI for quote"
                    RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                m_lReturn = RecalculateRI(nInsuranceFileCnt:=nInsuranceFileCnt, r_nIsValid:=nIsValid)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to recalculate RI for " & nInsuranceFileCnt
                    RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                ElseIf nIsValid = 0 Then
                    sMessage = "Failed to Validate Reinsurance For " & nInsuranceFileCnt
                    RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                'process stats (error messages are dealt within CreateAndPostStats())
                If Not (sInsuranceFileType = "QUOTE" Or sInsuranceFileType = "RENEWAL" Or sInsuranceFileType = "MTAQUOTE" Or m_oInsuranceFile.InsuranceFileType = "MTAQTETEMP" Or sInsuranceFileType = "MTAQREINS") Then
                    If CreateAndPostStats(nInsuranceFileCnt:=nInsuranceFileCnt, nClonedInsuranceFileCnt:=nInsuranceFileCnt, bReverseCloned:=sInsuranceFileType <> "POLICY", sTransactionType:=sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to create stats"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    ' clear up any entries in the cloned ri usage table that are no longer applicable
                    ' as they have now been processed by the batch utility...
                    nReturn = DeleteInsFileClonedRIUsage(nInsuranceFileCnt:=nInsuranceFileCnt)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to Delete Cloned RI Usage"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

            End If
            'save all transactions to database
            m_lReturn = m_oDatabase.SQLCommitTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to commit SQLTransaction"
                RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch excep As System.Exception

            'roll back all transactions as one of the step has failed
            m_lReturn = m_oDatabase.SQLRollbackTrans
            ProcessSingleClonedRIPolicy = gPMConstants.PMEReturnCode.PMFalse

            ' bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessSingleClonedRIPolicy)

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed ProcessSinglePolicy()", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleClonedRIPolicy", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=excep)


        End Try

    End Function

    Public Function CopyPolicyHeader(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lNewInsurancefileCnt As Integer, ByRef r_sMessage As String, Optional ByRef r_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_dtPolicyStartDate As Date = #12:00:00 AM#, Optional ByVal v_sSetOldInsuranceFileStatus As String = "", Optional ByVal v_sSetNewInsuranceFileStatus As String = "", Optional ByVal v_sTransactionType As String = "") As Integer
        Const kMethodName As String = "CopyPolicyHeader"

        Dim sDescription As String = " " 'this is not used. just need to call tax function
        Dim vInsuranceFileTax As Object = Nothing  'this is not used. just need to call tax function

        Try

            CopyPolicyHeader = gPMConstants.PMEReturnCode.PMTrue

            '****************** Create new version of policy ************************
            'assign current insurance file count to business object
            m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt

            ' get details of current policy
            If (m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed to get policy details for " & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            If (m_oInsuranceFile.InsuranceFileType = "QUOTE" Or m_oInsuranceFile.InsuranceFileType = "RENEWAL") Then
                m_oInsuranceFile.EventDescription = "Policy copied for cloned reinsurance processing"
                m_oInsuranceFile.UpdatePolicy()
                Exit Function
            End If

            ' change old policy version status if required
            If v_sSetOldInsuranceFileStatus <> "" Then
                m_oInsuranceFile.InsuranceFileStatus = v_sSetOldInsuranceFileStatus
            End If

            m_oInsuranceFile.UpdatePolicy()

            'change new policy version status if require otherwise default it to NULL (Live)
            If m_oInsuranceFile.InsuranceFileStatus <> "CAN" Then
                If v_sSetNewInsuranceFileStatus <> "" Then
                    m_oInsuranceFile.InsuranceFileStatus = v_sSetNewInsuranceFileStatus
                Else
                    m_oInsuranceFile.InsuranceFileStatus = System.DBNull.Value 'set policy status to live
                End If
            End If
            'set last transaction type if required
            If v_sTransactionType <> "" Then
                m_oInsuranceFile.LastTransType = v_sTransactionType
            End If

            m_oInsuranceFile.EventDescription = "Policy copied for cloned reinsurance processing"
            m_oInsuranceFile.PolicyVersion = CInt(m_oInsuranceFile.PolicyVersion) + 1

            ' create new version of policy based on current policy details
            If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to create new version of policy for " & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            '******************** Get new policy version details ******************************************
            r_lNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

            'do we need to return start date?
            If Not Informations.IsNothing(r_dtPolicyStartDate) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object m_oInsuranceFile.CoverStartDate. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_dtPolicyStartDate = CDate(m_oInsuranceFile.CoverStartDate)
            End If

            'do we need to return insurance_folder_cnt?
            If Not Informations.IsNothing(r_lInsuranceFolderCnt) Then
                r_lInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt 'note folder will be the same as previous policy version
            End If

            '****************** Copy related data to new policy version ***********************
            'copy standard wording
            If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to copy standard wording to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )", gPMConstants.PMELogLevel.PMLogError)
            End If

            'copy coinsurance
            If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to copy coinsurance details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )", gPMConstants.PMELogLevel.PMLogError)
            End If

            'copy agent commission
            If m_oRenSelection.CopyAgentCommission(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to copy agent commission details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )", gPMConstants.PMELogLevel.PMLogError)
            End If

            'copy agents
            If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to copy agent details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )", gPMConstants.PMELogLevel.PMLogError)
            End If

            '************** Policy level reinsurance is not supported at present **************************
            '    'copy policy level reinsurance
            '    m_oReinsurance.InsuranceFileCnt = r_lNewInsurancefileCnt
            '    m_oReinsurance.RiskId = 0
            '
            '    If m_oReinsurance.GetDetails() <> PMTrue Then
            '        r_sMessage = "Failed to copy policy level reinsurance to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
            '        GoTo Err_ProcessSingleDefRIPolicy
            '    End If

            ' copy policy level tax
            ' Must set the Task in Tax as this is passed into stored procedures as Mode. If it is wrong the new tax records will not be created.
            If m_oTax.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to SetProcessModes() to Tax object", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_oTax.InsuranceFileCnt = r_lNewInsurancefileCnt
            If m_oTax.GetInsuranceFileTax(r_vInsuranceFileTax:=vInsuranceFileTax, r_sDescription:=sDescription) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to copy policy level tax to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CopyPolicyHeader, excep:=ex)

        Finally


        End Try
    End Function

    Public Function CopyClonedRIRisks(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtPolicyStartDate As Date, ByVal v_sTransactionType As String, ByRef r_sMessage As String, Optional ByRef v_bIgnoreError As Boolean = False) As Integer
        Const kMethodName As String = "CopyClonedRIRisks"

        Dim vRiskArray(,) As Object = Nothing
        Dim vGISPolicyLinkArray(,) As Object = Nothing
        Dim lCount As Integer
        Dim lRiskCnt As Integer
        Dim lNewRiskCnt As Integer
        Dim sGISDataModelCode As String
        Dim lGISPolicyLinkID As Integer
        Dim lNewGisPolicyLinkID As Integer
        Dim lPolicyBinderID As Integer
        Dim lNewPolicyBinderID As Integer
        Dim sXMLDataSetDef As String = ""
        Dim sXMLDataSet As String = ""
        Dim lValidRIBand As Object
        Dim lReinsBand As Object 'not used - function required
        Dim vRiskTax As Object = Nothing
        Dim sDescription As String = ""
        Dim lRiskStatusID As Integer
        Dim sRiskStatusCode As String = ""
        Dim lCommitPolicy As Integer 'set to true when one of the deferred risk is now on live model and everything is ok
        Dim lClonedRIBand As Integer 'number of bands which are on cloned RI model
        Dim lClonedRIBandNewRisk As Integer 'number of bands which are on cloned RI model
        Dim lTransactionStarted As Integer 'set to pmtrue when m_oDatabase.SQLBeginTrans() is called
        Dim vResultArray(,) As Object
        Dim sInsuranceRef As String
        Dim lInsuranceFileType As Integer
        Dim lPreviousRiskCnt As Integer

        Const ACFieldPosRiskID As Integer = 0
        Const ACFieldPosGisScreenID As Integer = 21
        'Unused variable
        ' Const ACFieldPosIsRICloned As Integer = 7

        Try

            CopyClonedRIRisks = gPMConstants.PMEReturnCode.PMTrue
            r_sMessage = ""
            lValidRIBand = gPMConstants.PMEReturnCode.PMFalse
            lCommitPolicy = gPMConstants.PMEReturnCode.PMFalse
            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

            If m_oRiskData.GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to get associated risks for this policy " & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'do we have any risks
            If Not Informations.IsArray(vRiskArray) Then
                Exit Function
            End If

            'make sure we have the right transaction type
            If m_oRiskData.SetProcessModes(vTransactionType:=v_sTransactionType) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to set process mode to RiskData object (bSIRRiskData.business)", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = GetPolicyType(v_lInsuranceFileCnt, lInsuranceFileType)


            'process each risk
            For lCount = 0 To vRiskArray.GetUpperBound(1)
                '        If m_oDatabase.SQLBeginTrans() <> PMTrue Then
                '            RaiseError kMethodName, "Failed SQLBeginTrans()", PMLogError
                '        End If

                lTransactionStarted = gPMConstants.PMEReturnCode.PMTrue

                lRiskCnt = NullToLong(vRiskArray(ACFieldPosRiskID, lCount))

                'how many bands are on Cloned ri model for this risk
                If GetRiskClonedRIBand(v_lRiskCnt:=lRiskCnt, r_lClonedRIBand:=lClonedRIBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to get RI model for old risk (Policy: " & v_lInsuranceFileCnt & " Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                End If

                'either reinsurance is not apply or this risk is on a live RI model
                'we just need to create the insurance file link and move on to process next risk
                If lClonedRIBand = 0 Then

                    'add new risk link
                    If m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_sStatusFlag:="U") <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to create new insurance file risk link (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'move claims to new version of policy with same risks
                    If MoveClaimToNewRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lNewRiskCnt:=lRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to move claims to new version of policy  (old Policy: " & v_lInsuranceFileCnt & " old Risk: " & lRiskCnt & " new policy: " & v_lNewInsuranceFileCnt & " new risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse
                Else 'risk is on Cloned ri model


                    '************** we are on a Cloned RI model ***********************
                    'copy risk details and reset status to unquoted
                    If lInsuranceFileType = 2 Then
                        If m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=kInsFileRiskLinkTypeORIGINAL) <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & v_lNewInsuranceFileCnt & " Risk :)" & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else
                        m_lReturn = GetPreviousRiskCnt(v_lPreInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lPreviousRiskCnt:=lPreviousRiskCnt)


                        If CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=kInsFileRiskLinkTypeORIGINAL, v_lOldRiskCnt:=If(lPreviousRiskCnt = 0, lRiskCnt, lPreviousRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & v_lNewInsuranceFileCnt & " Risk :)" & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                        End If

                    End If
                    '************************ copy gis data for this risk *****************************
                    'get get policy link for old policy
                    If m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=lRiskCnt, r_vResultArray:=vGISPolicyLinkArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to copy gis details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy: " & v_lNewInsuranceFileCnt & " Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'do we have any gis data
                    lGISPolicyLinkID = NullToLong(vGISPolicyLinkArray(0, 0))
                    sGISDataModelCode = Trim(NullToString(vGISPolicyLinkArray(4, 0)))

                    If lGISPolicyLinkID = 0 Then
                        RaiseError(kMethodName, "We have no gis data for policy version (old policy :" & v_lInsuranceFileCnt & " Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'load gis detail for old risk (Note: gis object store folder_cnt in file_cnt field)
                    If m_oRenSelection.GIS_LoadFromDB(v_sGisDataModelCode:=sGISDataModelCode, r_vInsuranceFileCnt:=v_lInsuranceFolderCnt, r_vPolicyLinkID:=lGISPolicyLinkID, r_vRiskID:=lRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to load gis data (policy folder :" & v_lInsuranceFolderCnt & " Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'copy gis data to new risk (Note: gis object store folder_cnt in file_cnt field)
                    If m_oRenSelection.CopyDataSet(v_sDataModelCode:=sGISDataModelCode, r_lNewGISPolicyLinkId:=lNewGisPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet, v_vOldGISPolicyLinkId:=lGISPolicyLinkID, v_vOldInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vOldRiskID:=lRiskCnt, v_vNewInsuranceFileCnt:=v_lInsuranceFolderCnt, v_vNewRiskID:=lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to copy gis data (policy folder :" & v_lInsuranceFolderCnt & "Old Risk: " & lRiskCnt & " New Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Initialise the Data Set with the Object/Properties
                    If m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed load data from xml (policy folder :" & v_lInsuranceFolderCnt & "Old Risk: " & lRiskCnt & " New Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'save gis data for new risk
                    If m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to save gis data for new risk (policy folder :" & v_lInsuranceFolderCnt & "Old Risk: " & lRiskCnt & " New Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'get policy binder for old policy version
                    If m_oRenSelection.GetPolicyBinderID(v_sDataModelCode:=sGISDataModelCode, v_lGISPolicyLinkId:=lGISPolicyLinkID, r_lPolicyBinderID:=lPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to get policy binder for old policy version (gis policy link: " & lGISPolicyLinkID & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'get policy binder for new policy version
                    If m_oRenSelection.GetPolicyBinderID(v_sDataModelCode:=sGISDataModelCode, v_lGISPolicyLinkId:=lNewGisPolicyLinkID, r_lPolicyBinderID:=lNewPolicyBinderID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to get policy binder for new policy version (gis policy link: " & lNewGisPolicyLinkID & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'copy standard wording
                    If m_oRenSelection.CopyRiskStandardWordings(v_lOldPolicyBinderID:=lPolicyBinderID, v_lNewPolicyBinderID:=lNewPolicyBinderID, v_sDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to copy standard wording (Old Policy Binder: " & lPolicyBinderID & " New Policy Binder: " & lNewPolicyBinderID & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'do we need to do index linking
                    If Not Informations.IsDBNull(vRiskArray(ACFieldPosGisScreenID, lCount)) Then
                        'Note: the risk_id and v_lInsuranceFileCnt are not being used so it doesn't matter what we pass in
                        'this is because further up the code we already load up gis info
                        If m_oRenSelection.GISIndexLink(v_lInsuranceFileCnt:=0, v_lRiskID:=0, v_vGisScreenID:=vRiskArray(ACFieldPosGisScreenID, lCount), v_dtEffectiveDate:=v_dtPolicyStartDate, v_sGisDataModelCode:=sGISDataModelCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed index linking for policy: " & v_lNewInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If

                    'copy gis related sum insured
                    If m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=lGISPolicyLinkID, v_lNewPolicyLinkID:=lNewGisPolicyLinkID) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to copy gis related sum insured (Old policy link:" & lGISPolicyLinkID & " New policy link:" & lNewGisPolicyLinkID & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Note : rating section will only work when original risk on a policy is of type (2,5) and status of policy is (3,4,5)
                    'do peril allocation for new policy version
                    With m_oPerilAllocation
                        .InsuranceFileCnt = v_lNewInsuranceFileCnt
                        .InsuranceFolderCnt = v_lInsuranceFolderCnt
                        .RiskID = lNewRiskCnt
                        .TransactionType = v_sTransactionType
                    End With

                    'populate rating sections
                    If m_oPerilAllocation.PopulateRatingSections() <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to populate rating sections for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'update risk with premium and suminsured
                    If m_oPerilAllocation.UpdateRisk() <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to update risk's premium and suminsured for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'do risk tax
                    m_oTax.RiskCnt = lNewRiskCnt
                    If m_oTax.GetRiskTax(r_vRiskTax:=vRiskTax, r_sDescription:=sDescription) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to do risk tax for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If CopyRatings(v_lInsuranceFileCnt, lRiskCnt, lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to populate rating sections for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If



                    '**************** Note on reinsurance *******************
                    'reinsurance will work out which RI model is relevant for this risk
                    'if it can't find one it will return false

                    ' ensure that the m_oReinsurance.Task property is not set to PMView
                    ' as the CalculateRI wont be run.
                    If lInsuranceFileType = 2 Then
                        Call m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="NB")
                    Else
                        Call m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:="MTA")
                    End If

                    'get ready to do reinsurance (risk level)

                    m_oReinsurance.InsuranceFileCnt = v_lNewInsuranceFileCnt
                    m_oReinsurance.RiskId = lNewRiskCnt
                    If Not (lInsuranceFileType = 2 Or lPreviousRiskCnt = 0) Then
                        m_oReinsurance.CopyFACRiskCnt = lRiskCnt
                    Else
                        m_oReinsurance.CopyFACRiskCnt = 0
                    End If
                    'generate reinsurance for new policy version
                    If m_oReinsurance.CalculateRI() <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to generate RI for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'Note : load and save reinsurance details to fix any roundings
                    'get reinsurance details
                    If m_oReinsurance.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to reinsurance details for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'save new reinsurance details
                    If m_oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to reinsurance details for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    'do we have valid reinsurance bands ie adds up to 100%
                    If m_oReinsurance.ValidateBands(r_lValid:=lValidRIBand, r_lBand:=lReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                        If v_bIgnoreError = False Then
                            RaiseError(kMethodName, "Failed to validate RI bands for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                        Else
                            GoTo NextLoop
                        End If
                    End If





                    'do we have valid bands ie adds up to 100%
                    If lValidRIBand = 0 Then

                        'Note: for now we just check to see if any of the original Cloned bands been moved onto live band.
                        ' in the future we'll have to check for any changes in the bands
                        ' this is a big job and involve changes in reinsurance as well

                        'how many band are on Cloned ri model for new risk
                        If GetRiskClonedRIBand(v_lRiskCnt:=lNewRiskCnt, r_lClonedRIBand:=lClonedRIBandNewRisk) <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to get RI model for old risk (Policy: " & v_lInsuranceFileCnt & " Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                        End If

                        'has any of the Cloned band on this new risk moved to live ri model
                        If lClonedRIBandNewRisk <> lClonedRIBand Then
                            'change old risk status to quoted so it won't get pick up again
                            If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lRiskCnt, v_lRiskStatusID:=0, v_sRiskStatusCode:="QUOTED") <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "Failed to update old risk status to quoted (Risk: " & lRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            'NOTE: after peril allocation risk_status_id = 8 (pending resinsurance) if everything is ok
                            '      reinsurance (business side) shouldn't change this risk status so we are expecting pending reinsurance
                            'get risk status for new risk
                            If GetRiskStatus(v_lRiskCnt:=lNewRiskCnt, r_lRiskStatusID:=lRiskStatusID, r_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "Failed to get status for new risk (Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            If (Trim(sRiskStatusCode)).ToUpper = "PENDINGRI" Then 'pending reinsurance
                                'if we still have Cloned band on this risk then set it to RICLONED
                                sRiskStatusCode = If(lClonedRIBandNewRisk = 0, "QUOTED", "RICLONED")

                                'update risk status
                                If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lNewRiskCnt, v_lRiskStatusID:=0, v_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                                    RaiseError(kMethodName, "Failed to update new risk status to quoted (Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                                End If
                            Else
                                RaiseError(kMethodName, "Peril Allocation Failed or more questions need answering (Risk: " & lNewRiskCnt & ")", gPMConstants.PMELogLevel.PMLogError)
                            End If

                            ' Do error handling

                            'save changes to risk
                            '                    If m_oDatabase.SQLCommitTrans() <> PMTrue Then
                            '                        RaiseError kMethodName, "Failed to commit risk changes SQLCommitTrans()", PMLogError
                            '                    End If

                            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                            'set this flag to stop policy from rolling back
                            lCommitPolicy = gPMConstants.PMEReturnCode.PMTrue
                        Else 'none of the bands move to live model
                            'rollback changes on risk
                            '                    If m_oDatabase.SQLRollbackTrans() <> PMTrue Then
                            '                        RaiseError kMethodName, "Failed SQLRollbackTrans()", PMLogError
                            '                    End If

                            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                        End If 'lClonedRIBandNewRisk <> lClonedRIBand
                    Else 'bands are not add up to 100%
                        If v_bIgnoreError = False Then
                            RaiseError(kMethodName, "Invalid RI Bands ie bands do not add up to 100%", gPMConstants.PMELogLevel.PMLogError)
                        Else
                            GoTo NextLoop
                        End If
                    End If 'lValidRIBand = 0
                End If 'lClonedRIBand = 0

                m_lReturn = InsertClaimClonedRIUsage(v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lOldRiskCnt:=lRiskCnt, v_lNewRiskCnt:=lNewRiskCnt)


                m_lReturn = UpdateRIArrangementClonedStatus(v_lRisk_cnt:=lRiskCnt, v_iCloned:=2)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to UpdateRIArrangementClonedStatus", gPMConstants.PMELogLevel.PMLogError)
                End If

NextLoop:

            Next lCount

            If v_bIgnoreError Then
                Exit Function
            End If

            'rollback policy if none of new risks are on live ri model

            If lCommitPolicy <> gPMConstants.PMEReturnCode.PMTrue Then

                m_oDatabase.Parameters.Clear()

                m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to add Parameter InsuranceFileCnt", gPMConstants.PMELogLevel.PMLogError)
                    Exit Function
                End If

                vResultArray = Nothing
                sInsuranceRef = ""
                m_lReturn = m_oDatabase.SQLAction(sSQL:=CStr(ACInsertInsFileClonedRIUsageStored), sSQLName:=ACInsertInsFileClonedRIUsageName, bStoredProcedure:=ACInsertInsFileClonedRIUsageStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to get Insurance Ref: SQLSelect", gPMConstants.PMELogLevel.PMLogError)
                End If

                If Informations.IsArray(vResultArray) Then
                    sInsuranceRef = Trim(vResultArray(0, 0))
                End If

                RaiseError(kMethodName, "All risks are still on Cloned model" & " Insurance Ref : " & sInsuranceRef, gPMConstants.PMELogLevel.PMLogError)
                ' MsgBox("All risks are still on Cloned model" & " Insurance Ref : " & sInsuranceRef)
            End If
            CopyClonedRIRisks = gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception
            'rollback risk changes
            If lTransactionStarted = gPMConstants.PMEReturnCode.PMTrue Then
                'm_lReturn = m_oDatabase.SQLRollbackTrans
            End If

            CopyClonedRIRisks = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CopyClonedRIRisks, excep:=excep)

            ' If you want to rollback a transaction or something, do it here

        End Try

    End Function

    '***********************************************************************************************************************
    ' Name : CloseDefRIBusinessObject
    '
    ' Desc : close objects open by CreateDefRIBusinessObject()
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Private Sub CloseCloneRIBusinessObject()


        If (Not (m_oRenSelection Is Nothing)) Then
            m_oRenSelection.Dispose()
            m_oRenSelection = Nothing
        End If

        If (Not (m_oInsuranceFile Is Nothing)) Then
            m_oInsuranceFile.Dispose()
            m_oInsuranceFile = Nothing
        End If

        If (Not (m_oReinsurance Is Nothing)) Then
            m_oReinsurance.Dispose()
            m_oReinsurance = Nothing
        End If

        If (Not (m_oTax Is Nothing)) Then
            m_oTax.Dispose()
            m_oTax = Nothing
        End If

        If (Not (m_oRiskData Is Nothing)) Then
            m_oRiskData.Dispose()
            m_oRiskData = Nothing
        End If

        If (Not (m_oPerilAllocation Is Nothing)) Then
            m_oPerilAllocation.Dispose()
            m_oPerilAllocation = Nothing
        End If

        If (Not (m_oAgentCommission Is Nothing)) Then
            m_oAgentCommission.Dispose()
            m_oAgentCommission = Nothing
        End If

        If (Not (m_oControlTrans Is Nothing)) Then
            m_oControlTrans.Dispose()
            m_oControlTrans = Nothing
        End If


    End Sub

    '***********************************************************************************************************************
    ' Name : CopyDefRIRisks
    '
    ' Desc : copy related risks details to new policy version
    '
    ' Author : Thinh Nguyen 27/02/2004
    ' Note : if all new version of risks are still on deferred then rollback policy

    '************************ Note on Risk **************************
    'if risk has deferred RI model then we need to
    'begin transaction (risk)
    ' 1. copy risk
    ' 2. rerate (populate rating sections)
    ' 3. apply reinsurance
    ' 4. apply tax - now that risk has been successfully processed
    ' 5. if valid bands
    'if new risk is on live RI model (ie one of the bands is now on live model)
    '5.1 change old risk to quoted so it won't be pick up next time
    '5.2 change new risk to quoted if current status is pending reinsurance
    '5.3 move claims to new risk
    '5.4 if everything is ok then commit risk changes
    '5.5 set commit policy flag to true
    'else risk still on deferred model
    'rollback transaction (risk)
    '5.1 relink risk to new policy version
    '5.2 move claims to new policy version same risk
    'else
    '1. create a new link in insurance_file_risk_link with status_flag = "U" (unchanged)
    '2. move claims to new version of policy (same risk)
    'endif
    'if commit policy flag is not set then rollback policy (will be done on calling function)
    '***********************************************************************************************************************

    '***********************************************************************************************************************
    ' Name : MoveClaimToNewRisk
    '
    ' Desc : move all associate claims to new risk
    '
    ' Author : Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Private Function MoveClaimToNewRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer



        Dim sMessage As String
        Dim lReturnValue As Integer


        MoveClaimToNewRisk = gPMConstants.PMEReturnCode.PMTrue
        lReturnValue = gPMConstants.PMEReturnCode.PMTrue
        sMessage = ""

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add insurance file cnt param"
            MoveClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskCnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add risk cnt param"
            MoveClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=v_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add new insurance file cnt param"
            MoveClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=v_lNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add new risk cnt param"
            MoveClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACMoveClaimToNewRiskSQL, sSQLName:=ACMoveClaimToNewRiskName, bStoredProcedure:=ACMoveClaimToNewRiskStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed SQLAction - move claims to new risk"
            MoveClaimToNewRisk = gPMConstants.PMEReturnCode.PMFalse
            Exit Function
        End If

    End Function
    ''' <summary>
    ''' create postings for clone rework
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nClonedInsuranceFileCnt"></param>
    ''' <param name="bReverseCloned"></param>
    ''' <param name="sTransactionType"></param>
    ''' <param name="r_sMessage"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function CreateAndPostStats(ByVal nInsuranceFileCnt As Integer, Optional ByVal nClonedInsuranceFileCnt As Integer = 0, Optional ByVal bReverseCloned As Boolean = False, Optional ByVal sTransactionType As String = "", Optional ByRef r_sMessage As String = "") As Integer
        Const kMethodName As String = "CreateAndPostStats"
        Try
            r_sMessage = ""
            CreateAndPostStats = gPMConstants.PMEReturnCode.PMTrue

            m_oControlTrans.InsuranceFileCnt = nInsuranceFileCnt
            m_oControlTrans.ClonedInsuranceFileCnt = nClonedInsuranceFileCnt
            m_oControlTrans.ReverseCloned = bReverseCloned
            m_oControlTrans.IsCloned = True
            m_lReturn = m_oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=sTransactionType, vEffectiveDate:=m_dtEffectiveDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode for bControlTrans.Automated"
                CreateAndPostStats = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oControlTrans.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                r_sMessage = "Failed to create and post stats"
                Return gPMConstants.PMEReturnCode.PMFalse

            End If
            '    sSQL = "update insurance_file_clone_log  set status_id =3 " & vbCrLf
            '    sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"
            '
            '     m_oDatabase.Parameters.Clear
            '
            '     m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", _
            '                                            vValue:=v_nInsuranceFileCnt, _
            '                                            idirection:=PMParamInput, _
            '                                            iDataType:=PMLong)
            '
            '     m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Status", bStoredProcedure:=False)
            '    If m_lReturn <> PMTrue Then
            '        r_sMessage = "Failed to update insurance_file_clone_log "
            '        CreateAndPostStats = PMFalse
            '        GoTo Finally
            '    End If

        Catch excep As System.Exception
            CreateAndPostStats = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=CreateAndPostStats, excep:=excep)
        End Try

    End Function

    Public Function GetClonedRIPolicy(ByRef r_vResultArray(,) As Object) As Integer

        Const kMethodName As String = "GetClonedRIPolicy"

        Try

            GetClonedRIPolicy = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            GetClonedRIPolicy = m_oDatabase.SQLSelect(sSQL:=ACSelClonedRIPolicyForAmendSQL, sSQLName:=ACSelClonedRIPolicyForAmendName, bStoredProcedure:=ACSelClonedRIPolicyForAmendStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray)

        Catch ex As System.Exception
            GetClonedRIPolicy = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetClonedRIPolicy, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function

    '***********************************************************************************************************************
    ' Name : RelinkRisk
    '
    ' Desc : link risks from old policy version to new policy version (insurance_file_risk_link)
    '
    '***********************************************************************************************************************
    Public Function RelinkRisk(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer
        Const kMethodName As String = "RelinkRisk"
        Dim sSQL As String

        Try

            RelinkRisk = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "INSERT INTO insurance_file_risk_link (" & vbCrLf & "insurance_file_cnt," & vbCrLf & "risk_cnt," & vbCrLf & "status_flag, " & vbCrLf & "original_risk_cnt)" & vbCrLf & "SELECT " & v_lNewInsuranceFileCnt & "," & vbCrLf & "risk_cnt," & vbCrLf & "'U'," & vbCrLf & "null" & vbCrLf & "FROM insurance_file_risk_link" & vbCrLf & "WHERE insurance_file_cnt = " & v_lOldInsuranceFileCnt & vbCrLf & "AND status_flag <> 'D'" & vbCrLf


            m_oDatabase.Parameters.Clear()

            If m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Link old risks to new policy version", bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                RelinkRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch ex As System.Exception

            RelinkRisk = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RelinkRisk, excep:=ex)

        End Try

    End Function

    '***********************************************************************************************************************
    ' Name : DeletePolicy
    '
    ' Desc : delete this policy version and all dependant data
    '        if one of the step failed the whole thing will rollback
    ' Author : Thinh Nguyen 20/09/2004
    '***********************************************************************************************************************
    Public Function DeletePolicy(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Const kMethodName As String = "DeletePolicy"
        Try

            DeletePolicy = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                DeletePolicy = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            DeletePolicy = m_oDatabase.SQLAction(sSQL:=ACDeletePolicySQL, sSQLName:=ACDeletePolicyName, bStoredProcedure:=ACDeletePolicyStored)


        Catch ex As System.Exception

            DeletePolicy = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DeletePolicy, excep:=ex)

        End Try

    End Function

    '***********************************************************************************************************************
    ' Name : SetPolicyStatus
    ' Desc : change policy status
    '***********************************************************************************************************************
    Public Function SetPolicyStatus(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lInsuranceFileStatusID As Integer, Optional ByVal v_bStartTransaction As Boolean = True, Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Integer
        Dim sSQL As String

        Try

            SetPolicyStatus = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Insurance_File SET insurance_file_status_id = {InsuranceFileStatusID}" & vbCrLf
            sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"

            If v_bStartTransaction Then
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If

                    SetPolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileStatusID", vValue:=If(v_lInsuranceFileStatusID = 0, System.DBNull.Value, v_lInsuranceFileStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileStatusID)"
                End If

                SetPolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileCnt)"
                End If

                SetPolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Policy Status", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update policy status"
                End If

                SetPolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If v_bStartTransaction Then
                If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    SetPolicyStatus = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

        Catch ex As System.Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = "SetPolicyStatus() - Errored"
            End If

            SetPolicyStatus = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get renewals which await notice print", vApp:=ACApp, vClass:=ACClass, vMethod:="SetPolicyStatus", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)

        Finally

            If v_bStartTransaction Then
                If SetPolicyStatus <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                End If
            End If
        End Try

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
    ' ***************************************************************** '
    Private Function AddInputParameter(ByVal v_sName As String, ByVal v_vValue As Object, ByVal v_iType As Short) As Integer

        Const kMethodName As String = "AddInputParameter"


        AddInputParameter = gPMConstants.PMEReturnCode.PMTrue

        ' Add Parameter to database object
        If m_oDatabase.Parameters.Add(sName:=v_sName, vValue:=v_vValue, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=v_iType) <> gPMConstants.PMEReturnCode.PMTrue Then

            ' Log Error.
            AddInputParameter = gPMConstants.PMEReturnCode.PMFalse

            LogMessageToFile(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed to add parameter name:" & v_sName & ", values :" & v_vValue & ", type:" & v_iType, vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, excep:=New Exception(Informations.Err.Description))

        End If

    End Function
    ''' <summary>
    ''' create postings for clone rework
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function DeleteInsFileClonedRIUsage(ByVal nInsuranceFileCnt As Integer) As Integer

        Const kMethodName As String = "DeleteInsFileClonedRIUsage"

        Dim nReturn As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            AddInputParameter(v_sName:="insurance_file_cnt", v_vValue:=nInsuranceFileCnt, v_iType:=gPMConstants.PMEDataType.PMLong)

            ' Execute Action Query
            nReturn = m_oDatabase.SQLAction(sSQL:=ACDeleteInsFileClonedRIUsageSQL, sSQLName:=ACDeleteInsFileClonedRIUsageName, bStoredProcedure:=ACDeleteInsFileClonedRIUsageStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(ACDeleteInsFileClonedRIUsageSQL, "Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=DeleteInsFileClonedRIUsage, excep:=ex)
            Return nResult
            ' If you want to rollback a transaction or something, do it here

        End Try

    End Function

    Private Function GetRiskClonedRIBand(ByVal v_lRiskCnt As Integer, ByRef r_lClonedRIBand As Integer) As Integer
        Const kMethodName As String = "GetRiskClonedRIBand"

        Dim dtResult As New DataTable



        GetRiskClonedRIBand = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        bPMAddParameter.AddParameterLite(m_oDatabase, "RiskCnt", CLng(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


        m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetRiskClonedRIBandSQL, sSQLName:=ACGetRiskClonedRIBandName, bStoredProcedure:=ACGetRiskClonedRIBandStored, oRecordset:=dtResult)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed to get risk RI model", gPMConstants.PMELogLevel.PMLogError)
        End If

        If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
            r_lClonedRIBand = NullToLong(CInt(dtResult.Rows(0)(0).ToString))
        End If



    End Function

    Public Function InsertInsFileClonedRIUsage(ByVal v_lInsFileCnt As Integer) As Integer
        Const kMethodName As String = "InsertInsFileClonedRIUsage"
        Try

            InsertInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMTrue

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_cloned_RI_usage_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' do the call to insert the record into Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertInsFileClonedRIUsageSQL, sSQLName:=ACInsertInsFileClonedRIUsageName, bStoredProcedure:=ACInsertInsFileClonedRIUsageStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            InsertInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As System.Exception
            InsertInsFileClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=InsertInsFileClonedRIUsage, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        End Try


    End Function

    Public Function InsertClaimClonedRIUsage(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer
        Const kMethodName As String = "InsertClaimClonedRIUsage"
        Try

            InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMTrue

            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=v_lOldInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=v_lNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="old_risk_cnt", vValue:=v_lOldRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="new_risk_cnt", vValue:=v_lNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_cloned_RI_usage_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' do the call to insert the record into Insurance_File_Deferred_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACInsertInsClaimCloneRIUsageSQL, sSQLName:=ACInsertInsClaimCloneRIUsageName, bStoredProcedure:=ACInsertInsClaimCloneRIUsageStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As System.Exception
            InsertClaimClonedRIUsage = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=InsertClaimClonedRIUsage, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try


    End Function

    ' This will call the stored procedure spu_Risks_Cloned_RI_Status_Sel
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_nProductId"></param>
    ''' <param name="v_nBranchId"></param>
    ''' <param name="r_vClonedRIPoliciesArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClonedRIPolicies(ByVal v_nProductId As Integer, ByVal v_nBranchId As Integer, ByRef r_vClonedRIPoliciesArray(,) As Object) As Integer
        Const kMethodName As String = "GetClonedRIPolicies"

        Dim nReturn As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_nProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_nBranchId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=ACSelClonedRIPoliciesSQL, sSQLName:=ACSelClonedRIPoliciesName, bStoredProcedure:=ACSelClonedRIPoliciesStored, vResultArray:=r_vClonedRIPoliciesArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACSelClonedRIPoliciesSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetClonedRIPolicies, excep:=ex)
            Return nResult
            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function

    Public Function UpdateRIArrangementClonedStatus(ByVal v_lRisk_cnt As Integer, ByVal v_iCloned As Short, Optional ByRef v_lInsuranceFileCnt As Integer = 0) As Integer
        Const kMethodName As String = "UpdateRIArrangementClonedStatus"

        Dim lReturn As Integer
        Try

            UpdateRIArrangementClonedStatus = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRisk_cnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateRIArrangementClonedStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'UPGRADE_NOTE: IsMissing() was changed to IsNothing(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="8AE1CB93-37AB-439A-A4FF-BE3B6760BB23"'
            If Not Informations.IsNothing(v_lInsuranceFileCnt) Then

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    UpdateRIArrangementClonedStatus = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="cloned", vValue:=v_iCloned, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                UpdateRIArrangementClonedStatus = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' do the update call
            lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdRIArrangementClonedStatusSQL, sSQLName:=ACUpdRIArrangementClonedStatusName, bStoredProcedure:=ACUpdRIArrangementClonedStatusStored)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACUpdRIArrangementClonedStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateRIArrangementClonedStatus, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="v_nProductId"></param>
    ''' <param name="v_nBranchId"></param>
    ''' <param name="r_vClonedRIClaimsArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetClonedRIClaims(ByVal v_nProductId As Integer, ByVal v_nBranchId As Integer, ByRef r_vClonedRIClaimsArray(,) As Object) As Integer
        Const kMethodName As String = "GetClonedRIClaims"

        Dim nReturn As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_nProductId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_nBranchId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=ACSelClonedRIClaimsSQL, sSQLName:=ACSelClonedRIClaimsName, bStoredProcedure:=ACSelClonedRIClaimsStored, vResultArray:=r_vClonedRIClaimsArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACSelClonedRIClaimsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetClonedRIClaims, excep:=ex)
            Return nResult
            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function
    ''' <summary>
    ''' process clone on claim for a policy
    ''' </summary>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <param name="nNewRiskCnt"></param>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nOldRiskCnt"></param>
    ''' <param name="bIsRI2007Enabled"></param>
    ''' <param name="nBaseClaimId"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ProcessSingleClonedRIClaim(ByVal nNewInsuranceFileCnt As Integer, ByVal nNewRiskCnt As Integer, ByVal nOldInsuranceFileCnt As Integer, ByVal nOldRiskCnt As Integer, ByVal bIsRI2007Enabled As Boolean, ByVal nBaseClaimId As Integer) As Integer

        Const kMethodName As String = "ProcessSingleClonedRIClaim"

        Dim nReturn As Integer
        Dim r_vClonedRIClaimVersionsArray(,) As Object = Nothing
        Dim iLoopy As Integer
        Dim nClaimId As Integer
        Dim obCLMReinsurance As bCLMReinsurance.Form
        Dim obCLMReinsuranceRI2007 As bCLMReinsuranceRI2007.Form
        Dim sTransactionTypeCode As String
        Dim sClaimVersionDescription As String
        Dim sMessage As String
        Dim nResult As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Need to pass New_Insurance_file_cnt and New_Risk_cnt
            nReturn = GetClonedRIClaimVersions(nBaseClaimId:=nBaseClaimId, nNewInsuranceFileCnt:=nNewInsuranceFileCnt, nNewRiskCnt:=nNewRiskCnt, nOldInsuranceFileCnt:=nOldInsuranceFileCnt, nOldRiskCnt:=nOldRiskCnt, r_vClonedRIClaimVersionsArray:=r_vClonedRIClaimVersionsArray)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to Get Cloned RI Claim Versions", gPMConstants.PMELogLevel.PMLogError)
            End If
            If Informations.IsArray(r_vClonedRIClaimVersionsArray) Then
                'set transaction so we can roll back if something gone wrong
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    nReturn = gPMConstants.PMEReturnCode.PMFalse
                    RaiseError(kMethodName, "Failed to begin SQLTransaction", gPMConstants.PMELogLevel.PMLogError)
                End If


                obCLMReinsurance = New bCLMReinsurance.Form
                nReturn = obCLMReinsurance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("gPMComponentServices.CreateBusinessObject", "Failed to get instance of bCLMReinsurance.Form")
                End If


                obCLMReinsuranceRI2007 = New bCLMReinsuranceRI2007.Form
                nReturn = obCLMReinsuranceRI2007.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("gPMComponentServices.CreateBusinessObject", "Failed to get instance of bCLMReinsurance.Form")
                End If

                ' For each claim versions
                For iLoopy = r_vClonedRIClaimVersionsArray.GetLowerBound(1) To r_vClonedRIClaimVersionsArray.GetUpperBound(1)
                    ' Delete the existing RI by calling spu_Claim_RI_Arrangement_Line_Del_RI2007
                    ' Delete RI_Arrangement as well
                    nClaimId = ToSafeLong(r_vClonedRIClaimVersionsArray(MainModule.ClaimVersionsEnum.DBClaimID, iLoopy), 0)
                    sTransactionTypeCode = ToSafeString(r_vClonedRIClaimVersionsArray(MainModule.ClaimVersionsEnum.DBTransactionCode, iLoopy), "")
                    nReturn = ClaimRIArrangementArchive(nClaimId:=nClaimId)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to archive claim ri arrangement lines"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    If sTransactionTypeCode = "C_CO" Then
                        nReturn = ClaimRIArrangementDel(nClaimId:=nClaimId)
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to Delete Claims RI Arrangement", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                    ' CalculateRI Again
                    ' Put product option check to call normal or ri2007 business component
                    If bIsRI2007Enabled = True Then
                        ' CalculateRI.bClmReinsurance (nClaimId)
                        obCLMReinsuranceRI2007.ClaimID = nClaimId
                        obCLMReinsuranceRI2007.BalanceAndCloseClaim = False
                        If sTransactionTypeCode = "C_SA" Then
                            obCLMReinsuranceRI2007.ActualRecovery = 1
                            obCLMReinsuranceRI2007.Recovery = True
                        ElseIf sTransactionTypeCode = "C_RV" Then
                            obCLMReinsuranceRI2007.ActualRecovery = 0
                            obCLMReinsuranceRI2007.Recovery = True
                        Else
                            obCLMReinsuranceRI2007.Recovery = False
                            obCLMReinsuranceRI2007.ActualRecovery = 0
                        End If

                        obCLMReinsuranceRI2007.Task = gPMConstants.PMEComponentAction.PMEdit

                        nReturn = obCLMReinsuranceRI2007.CalculateRI()
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to CalculateRI", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    Else
                        obCLMReinsurance.ClaimID = nClaimId
                        obCLMReinsurance.Task = gPMConstants.PMEComponentAction.PMEdit
                        nReturn = obCLMReinsurance.CalculateRI()
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            RaiseError(kMethodName, "Failed to CalculateRI", gPMConstants.PMELogLevel.PMLogError)
                        End If
                    End If
                    sClaimVersionDescription = "Claim Clone Batch Processed"
                    If sTransactionTypeCode = "C_SA" Then
                        sClaimVersionDescription = sClaimVersionDescription & " on Salvage Recovery "
                    ElseIf sTransactionTypeCode = "C_RV" Then
                        sClaimVersionDescription = sClaimVersionDescription & " on Third Party Recovery "
                    End If
                    nReturn = FinaliseClaimDetails(nClaimId:=nClaimId, sClaimVersionDescription:=sClaimVersionDescription)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to run FinaliseClaimDetails"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    nReturn = ReverseClaimTransactions(nClaimId:=nClaimId, v_sTransactionTypeCode:=sTransactionTypeCode)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to Reverse Claim Transactions"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    nReturn = RaiseClaimTransaction(nClaimId:=nClaimId, sTransactionTypeCode:=sTransactionTypeCode)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to Raise Claim Transactions"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If
                    'End If

                    ' When all is well then update claim status flag for table ClaimClonedRIUsage
                    nReturn = UpdateClaimClonedRIUsage(nNewInsuranceFileCnt:=nNewInsuranceFileCnt, nStatus:=2)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to Update Claim Cloned status"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If

                    nReturn = UpdateClaimRIStatus(nClaimId:=nClaimId, nStatus:=2)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        sMessage = "Failed to Update Claim Cloned status"
                        RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next

                'save all transactions to database
                nReturn = m_oDatabase.SQLCommitTrans
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed to commit SQLTransaction", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            Return nResult
        Catch ex As System.Exception

            'roll back all transactions as one of the step has failed
            nReturn = m_oDatabase.SQLRollbackTrans
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessSingleClonedRIClaim, excep:=ex)
            Return nResult
        Finally

            obCLMReinsurance = Nothing
            obCLMReinsuranceRI2007 = Nothing
        End Try
    End Function
    ''' <summary>
    ''' create postings for clone rework
    ''' </summary>
    ''' <param name="nNewInsuranceFileCnt"></param>
    ''' <param name="nNewRiskCnt"></param>
    ''' <param name="nOldInsuranceFileCnt"></param>
    ''' <param name="nOldRiskCnt"></param>
    ''' <param name="nBaseClaimId"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetClonedRIClaimVersions(ByVal nBaseClaimId As Integer, ByVal nNewInsuranceFileCnt As Integer, ByVal nNewRiskCnt As Integer, ByVal nOldInsuranceFileCnt As Integer, ByVal nOldRiskCnt As Integer, ByRef r_vClonedRIClaimVersionsArray(,) As Object) As Integer
        Const kMethodName As String = "GetClonedRIClaimVersions"

        Dim nReturn As Integer
        Try

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="BaseClaimId", vValue:=nBaseClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=nNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=nOldInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="OldRiskCnt", vValue:=nOldRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            ' Execute selection Query
            nReturn = m_oDatabase.SQLSelect(sSQL:=ACSelClonedRIClaimVersionsSQL, sSQLName:=ACSelClonedRIClaimVersionsName, bStoredProcedure:=ACSelClonedRIClaimVersionsStored, vResultArray:=r_vClonedRIClaimVersionsArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACSelClonedRIClaimVersionsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nReturn
        Catch ex As System.Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetClonedRIClaimVersions, excep:=ex)
            Return nReturn
            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function
    '''' <summary>
    '''' delete claim ri arrangement
    '''' </summary>
    '''' <param name="v_nClaimId"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ClaimRIArrangementDel(ByVal nClaimId As Integer) As Integer
        Const kMethodName As String = "ClaimRIArrangementDel"

        Dim nReturn As Integer
        Try

            ClaimRIArrangementDel = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute selection Query
            nReturn = m_oDatabase.SQLAction(sSQL:=ACClaimRIArrangementDelSQL, sSQLName:=ACClaimRIArrangementDelName, bStoredProcedure:=ACClaimRIArrangementDelStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACClaimRIArrangementDelSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nReturn
        Catch ex As System.Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ClaimRIArrangementDel, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
            Return nReturn
        End Try
    End Function
    ''' <summary>
    ''' this method generates reversal of stats
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="v_sTransactionTypeCode"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ReverseClaimTransactions(ByVal nClaimId As Integer, ByVal v_sTransactionTypeCode As String) As Integer
        Const kMethodName As String = "ReverseClaimTransactions"

        Dim nStatsFolderCnt As Integer
        Dim sMessage As String
        Dim nReturn As Integer
        Dim dtResult As New DataTable
        Dim bThisRevesionPresent As Boolean
        Try

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="nIgnoreDocRef", vValue:=CStr(1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Execute selection Query
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:="spu_CLM_Get_Stats_Folder_For_Claim", sSQLName:="spu_CLM_Get_Stats_Folder_For_Claim", bStoredProcedure:=True, oRecordset:=dtResult)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to Get Stats Folder from spu_CLM_Get_Stats_Folder_For_Claim"
                RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then

                nReturn = CreateStatsFolder(r_nStatsFolderCnt:=nStatsFolderCnt, sTransactionTypeCode:=v_sTransactionTypeCode, nClaimId:=nClaimId)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to Create Stats Folder"
                    RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                nReturn = ReverseClaimStats(nClaimId:=nClaimId, nStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:=v_sTransactionTypeCode, r_bThisRevesionPresent:=bThisRevesionPresent)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to Reverse Claim Stats"
                    RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                nReturn = RaiseClaimTransaction(nClaimId:=nClaimId, sTransactionTypeCode:=v_sTransactionTypeCode, bIsCloneReversal:=True, nStatsFolderCnt:=nStatsFolderCnt)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    sMessage = "Failed to Raise Claim Transactions"
                    RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                If bThisRevesionPresent Then

                    m_lReturn = CreateStatsFolder(r_nStatsFolderCnt:=nStatsFolderCnt, sTransactionTypeCode:="C_CR", nClaimId:=nClaimId)

                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "CreateStatsFolder Payment Ref Check Failed", PMELogLevel.PMLogError)
                    End If

                    m_lReturn = ReverseClaimStats(nClaimId:=nClaimId, nStatsFolderCnt:=nStatsFolderCnt, v_sTransactionTypeCode:="C_CR", r_bThisRevesionPresent:=bThisRevesionPresent)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to Delete Claim Stats", PMELogLevel.PMLogError)
                    End If

                    m_lReturn = RaiseClaimTransaction(nClaimId:=nClaimId, sTransactionTypeCode:="C_CR", bIsCloneReversal:=True, nStatsFolderCnt:=nStatsFolderCnt)
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed to Raise Claim Transactions", PMELogLevel.PMLogError)
                    End If

                End If
            End If

            Return nReturn

        Catch ex As System.Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ReverseClaimTransactions, excep:=ex)
            Return nReturn
            ' If you want to rollback a transaction or something, do it here
        End Try
    End Function
    ''' <summary>
    ''' this method raise stats
    ''' </summary>
    ''' <param name="nNewInsuranceFileCnt"></param>
    '''  <param name="nStatus"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function UpdateClaimClonedRIUsage(ByVal nNewInsuranceFileCnt As Integer, ByVal nStatus As Short) As Integer
        Const kMethodName As String = "UpdateClaimClonedRIUsage"
        Dim nResult As Integer
        Dim nReturn As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=nNewInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=nStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute selection Query
            nReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateClaimClonedRIUsageSQL, sSQLName:=ACUpdateClaimClonedRIUsageName, bStoredProcedure:=ACUpdateClaimClonedRIUsageStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACUpdateClaimClonedRIUsageSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdateClaimClonedRIUsage, excep:=ex)
            Return nResult
            ' If you want to rollback a transaction or something, do it here
        End Try
    End Function
    ''' <summary>
    ''' this method raise stats
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="sTransactionTypeCode"></param>
    '''  <param name="bIsCloneReversal"></param>
    '''  <param name="nStatsFolderCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RaiseClaimTransaction(ByVal nClaimId As Integer, Optional ByVal sTransactionTypeCode As String = "", Optional ByVal bIsCloneReversal As Boolean = False, Optional ByVal nStatsFolderCnt As Integer = 0) As Integer
        Const kMethodName As String = "RaiseClaimTransaction"

        Dim obCLMChangeClaimStatus As bCLMChangeClaimStatus.Business = Nothing
        Dim nReturn As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            'Get an instance of bACTDocumentReversal

            obCLMChangeClaimStatus = New bCLMChangeClaimStatus.Business
            nReturn = obCLMChangeClaimStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = obCLMChangeClaimStatus.SetProcessModes(vTransactionType:=sTransactionTypeCode)

            If bIsCloneReversal = True Then
                obCLMChangeClaimStatus.IsCloneReversal = True
                nReturn = DirectCast(obCLMChangeClaimStatus.RaiseClonedTransactions(nClaimId:=nClaimId, nStatsFolderCnt:=nStatsFolderCnt), Integer)
            Else
                obCLMChangeClaimStatus.IsCloned = 1
                nReturn = DirectCast(obCLMChangeClaimStatus.RaiseTransactions(v_lClaimId:=nClaimId), Integer)
            End If
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue And nReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=RaiseClaimTransaction, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            obCLMChangeClaimStatus.Dispose()
            obCLMChangeClaimStatus = Nothing

        End Try
        Return nResult
    End Function

    Public Function GetHiddenOption(ByRef r_sValue As String, Optional ByVal v_iBranchID As Integer = 1, Optional ByVal v_lOptionNo As Integer = 1) As Integer

        Dim vResultArray(,) As Object = Nothing

        Const kMethodName As String = "GetHiddenOption"



        Try

            GetHiddenOption = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="BranchID", vValue:=v_iBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                GetHiddenOption = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If m_oDatabase.Parameters.Add(sName:="OptionNo", vValue:=v_lOptionNo, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then

                GetHiddenOption = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If m_oDatabase.SQLSelect(sSQL:=ACGetHiddenOptionSQL, sSQLName:=ACGetHiddenOptionName, bStoredProcedure:=ACGetHiddenOptionStored, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                GetHiddenOption = gPMConstants.PMEReturnCode.PMFalse
                Exit Function

            End If


            If Informations.IsArray(vResultArray) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vResultArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                r_sValue = vResultArray(0, 0)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetHiddenOption, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally


        End Try
    End Function




    Public Function GetPolicyType(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lInsuranceFileTypeId As Integer) As Integer

        Dim dtResult As New DataTable

        Const kMethodName As String = "GetPolicyType"

        Try

            GetPolicyType = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetInsuranceRefSQL, sSQLName:=ACGetInsuranceRefName, bStoredProcedure:=ACGetInsuranceRefStored, oRecordset:=dtResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetPolicyType = m_lReturn
                Exit Function
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lInsuranceFileTypeId = CInt(dtResult.Rows(0)(1).ToString)
            Else
                GetPolicyType = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            GetPolicyType = gPMConstants.PMEReturnCode.PMTrue
        Catch excep As System.Exception
            GetPolicyType = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetPolicyType, excep:=excep)

        End Try

    End Function


    Public Function GetRisk(ByVal v_lRiskCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Try

            GetRisk = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            GetRisk = m_oDatabase.SQLSelect(sSQL:="spu_risk_sel", sSQLName:="spu_risk_sel", bStoredProcedure:=True, vResultArray:=r_vResultArray, bKeepNulls:=True)

            Exit Function

        Catch ex As Exception

            GetRisk = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
        Finally

        End Try
    End Function

    Public Function GetPreviousRiskCnt(ByVal v_lPreInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_lPreviousRiskCnt As Integer) As Integer

        Dim dtResult As New DataTable

        Try

            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMTrue

            r_lPreviousRiskCnt = 0
            'Send the new file in
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", CLng(v_lPreInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_cnt", CLng(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "processed_Insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute the SP
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetPreviousRiskCntForTransferSQL, sSQLName:=ACGetPreviousRiskCntForTransferName, bStoredProcedure:=True, oRecordset:=dtResult)

            'Determine the result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMFalse
            ElseIf dtResult IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lPreviousRiskCnt = ToSafeInteger(dtResult.Rows(0)(0).ToString)
            End If

            Exit Function

        Catch excep As System.Exception
            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPreviousRiskCnt", r_lFunctionReturn:=GetPreviousRiskCnt, excep:=excep)

            Exit Function
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: CopyRisk (Private)
    '
    ' Desc: copy risk details from OldInsuranceFileCnt to NewInsuranceFileCnt
    '
    ' Changes: RWH(20/11/2000) Updated for new Risk database structure
    '           which uses Insurance_file_risk_link table rather than
    '           storing insurance_file_cnt on the Risk table.
    ' ***************************************************************** '
    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer, Optional ByVal v_lResetStatus As Integer = 0, Optional ByVal v_lCreateLinkType As Integer = 0, Optional ByVal v_bAutoCancellation As Boolean = False, Optional ByRef v_sRiskMergeStatus As String = "", Optional ByRef v_lOldRiskCnt As Integer = 0) As Integer

        Dim lRiskCnt As Integer

        Dim lOldRiskCnt As Integer
        Dim lNewRiskCnt As Integer

        Dim iIsAutoReinsured As Short
        Dim vArray(,) As Object = Nothing

        Try

            CopyRisk = gPMConstants.PMEReturnCode.PMTrue

            'UPGRADE_WARNING: Couldn't resolve default property of object v_vRiskDetail(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            lOldRiskCnt = v_vRiskDetail(0, v_lPosNo)

            'Tomo030801
            'This bit's here because we need to reset the auto reinsured flag to that
            'from the risk type.
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=v_vRiskDetail(4, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAutoReinsuredSQL, sSQLName:=ACGetAutoReinsuredName, bStoredProcedure:=ACGetAutoReinsuredStored, vResultArray:=vArray)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            If Informations.IsArray(vArray) Then
                'UPGRADE_WARNING: Couldn't resolve default property of object vArray(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
                iIsAutoReinsured = vArray(0, 0)
            Else
                iIsAutoReinsured = 0
            End If

            'UPGRADE_WARNING: Couldn't resolve default property of object v_vRiskDetail(). Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            v_vRiskDetail(20, v_lPosNo) = iIsAutoReinsured

            'UPGRADE_WARNING: Couldn't resolve default property of object vArray. Click for more: 'ms-help://MS.VSCC.v80/dv_commoner/local/redirect.htm?keyword="6A50421D-15FE-4896-8A1B-2EC21E9037B2"'
            vArray = Nothing

            'Tomo030801 - End

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            'TN20010719 - start
            If v_lResetStatus = gPMConstants.PMEReturnCode.PMTrue Then
                'reset status to UnQuoted
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=4, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
                'TN20010719 - end
            Else
                m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=v_vRiskDetail(1, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=v_vRiskDetail(2, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="accumulation_id", vValue:=v_vRiskDetail(3, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=v_vRiskDetail(4, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=v_vRiskDetail(5, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_number", vValue:=v_vRiskDetail(6, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured_requested", vValue:=v_vRiskDetail(7, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_date", vValue:=v_vRiskDetail(8, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=v_vRiskDetail(9, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_not_index_linked", vValue:=v_vRiskDetail(10, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_accumulated", vValue:=v_vRiskDetail(11, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_reason_id", vValue:=v_vRiskDetail(12, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_date", vValue:=v_vRiskDetail(13, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_description", vValue:=v_vRiskDetail(14, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="var_data_ref", vValue:=v_vRiskDetail(15, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_sum_insured", vValue:=v_vRiskDetail(16, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_annual_premium", vValue:=v_vRiskDetail(17, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="total_this_premium", vValue:=v_vRiskDetail(18, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_ri_at_risk_level", vValue:=v_vRiskDetail(19, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_auto_reinsured", vValue:=v_vRiskDetail(20, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=v_vRiskDetail(21, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="eml_percentage", vValue:=v_vRiskDetail(22, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' PW311002
            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_number", vValue:=v_vRiskDetail(23, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' PW021202
            m_lReturn = m_oDatabase.Parameters.Add(sName:="variation_number", vValue:=v_vRiskDetail(24, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' PW021202
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_risk_selected", vValue:=v_vRiskDetail(25, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' PW311002
            m_lReturn = m_oDatabase.Parameters.Add(sName:="coverage", vValue:=v_vRiskDetail(26, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' PW311002
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insured_item", vValue:=v_vRiskDetail(27, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' PW311002
            m_lReturn = m_oDatabase.Parameters.Add(sName:="extensions", vValue:=v_vRiskDetail(28, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="premium_this_year", vValue:=v_vRiskDetail(31, v_lPosNo), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            ' Sasria
            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_mandatory_risk", vValue:=ToSafeInteger(v_vRiskDetail(36, v_lPosNo), 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskSQL, sSQLName:=ACAddRiskName, bStoredProcedure:=ACAddRiskStored)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

            lRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

            If v_bAutoCancellation = False Then

                'RWH(20/11/2000) Add link record into Insurance_file_risk_link.
                If (lRiskCnt <> 0) Then

                    ' Determine whether or not to create a link and
                    ' if so what kind of link
                    Select Case v_lCreateLinkType

                        Case 0 ' standard - original and renewed risk cnt not populated
                            m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", 0, 0)

                        Case 1 ' populate original_risk_cnt
                            m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", v_lOldRiskCnt, 0)

                        Case 2 ' populate renewed_risk_cnt
                            m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C", 0, v_lOldRiskCnt)

                        Case Else
                            ' do nothing
                    End Select

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If

                    r_lRiskCnt = lRiskCnt

                    m_lReturn = m_oRiskData.CopyRatingSection(v_lOldRiskCnt:=lOldRiskCnt, v_lNewRiskCnt:=lRiskCnt)

                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                        Exit Function
                    End If
                End If

            Else

                ' delete the original insurance file risk link
                m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=v_lOldRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                ' add new
                m_lReturn = m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, If(v_sRiskMergeStatus = "DP", "D", "C"), v_lOldRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

                r_lRiskCnt = lRiskCnt
                lNewRiskCnt = lRiskCnt

                m_lReturn = m_oRiskData.CopyRiskExtras(v_lOldRiskCnt:=v_lOldRiskCnt, v_lNewRiskCnt:=lNewRiskCnt)

                'sj 20/12/2002 - end
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                    CopyRisk = gPMConstants.PMEReturnCode.PMFalse
                    Exit Function
                End If

            End If

            Exit Function

        Catch ex As Exception

            CopyRisk = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRisk", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)



        End Try
    End Function

    Public Function CopyRatings(ByRef v_lInsuranceFileCnt As Integer, ByRef r_lOriginalRiskCnt As Integer, ByRef r_lRiskCnt As Integer) As Object

        Dim r_vResultArray2(,) As Object = Nothing
        Dim lCnt1 As Integer
        Dim lCnt2 As Integer
        Dim j As Short
        Dim m_dProrataRate As Double


        Try

            CopyRatings = gPMConstants.PMEReturnCode.PMTrue

            'All unedited Risks go through without any pro-rata
            m_dProrataRate = 1

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=r_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPerilSQL, sSQLName:=ACDelPerilName, bStoredProcedure:=ACDelPerilStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                'Exit Function
                Return CopyRatings

            End If


            'Del Rating Sections
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=r_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelRatingSectionSQL, sSQLName:=ACDelRatingSectionName, bStoredProcedure:=ACDelRatingSectionStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                Return CopyRatings

            End If

            'Get Original Rating Sections
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=r_lOriginalRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            'Fetch the records
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRatingSectionSQL, sSQLName:=ACSelectRatingSectionName, bStoredProcedure:=ACSelectRatingSectionStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray2)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                Return CopyRatings
            End If

            If Informations.IsArray(r_vResultArray2) Then

                lCnt1 = r_vResultArray2.GetLowerBound(1)
                lCnt2 = r_vResultArray2.GetUpperBound(1)

                For j = lCnt1 To lCnt2
                    m_lReturn = CopyRatingSectionsAndPerils(r_vResultArray2, 1, 0, v_lInsuranceFileCnt, r_lRiskCnt, j, m_dProrataRate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        CopyRatings = gPMConstants.PMEReturnCode.PMFalse
                        Return CopyRatings
                    End If
                Next

            End If
            Return CopyRatings

        Catch ex As Exception

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatings Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatings", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMError

        End Try

    End Function

    Public Function CopyRatingSectionsAndPerils(ByVal r_vResultArray(,) As Object, ByVal i_ThisPremiumSign As Short, ByVal i_OriginalFlag As Short, ByVal m_lInsuranceFileCnt As Integer, ByVal m_lRiskCnt As Integer, ByRef iIndex As Short, Optional ByRef dProrata As Double = 0) As Integer

        Dim m_lPolicyRatingSectionTypeId As Integer
        Dim m_cThisPremium As Decimal
        Dim m_lInsuranceFileNoOfDp As Integer

        Try

            CopyRatingSectionsAndPerils = gPMConstants.PMEReturnCode.PMTrue

            m_lPolicyRatingSectionTypeId = -1
            If Informations.IsNothing(dProrata) Then
                m_cThisPremium = i_ThisPremiumSign * r_vResultArray(5, iIndex)
            Else
                m_cThisPremium = i_ThisPremiumSign * (r_vResultArray(5, iIndex) * dProrata)
            End If
            m_lInsuranceFileNoOfDp = 2

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="rating_section_type_id", vValue:=r_vResultArray(10, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="policy_section_type_id", vValue:=m_lPolicyRatingSectionTypeId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=m_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=m_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured", vValue:=r_vResultArray(4, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_rate", vValue:=r_vResultArray(3, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="annual_premium", vValue:=r_vResultArray(6, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="this_premium", vValue:=m_cThisPremium, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="rate_type_id", vValue:=r_vResultArray(12, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_no_of_dp", vValue:=m_lInsuranceFileNoOfDp, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_flag", vValue:=i_OriginalFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="currency_id", vValue:=If(r_vResultArray(14, iIndex) = "", System.DBNull.Value, r_vResultArray(14, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="country_id", vValue:=If(r_vResultArray(15, iIndex) = "", System.DBNull.Value, r_vResultArray(15, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="state_id", vValue:=If(r_vResultArray(16, iIndex) = "", System.DBNull.Value, r_vResultArray(16, iIndex)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="is_amended", vValue:=r_vResultArray(17, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="calculated_premium", vValue:=ToSafeCurrency(r_vResultArray(18, iIndex), 0), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="override_reason", vValue:=r_vResultArray(19, iIndex), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)


            ' Add Section & Perils
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddSectionAndPerilsSQL, sSQLName:=ACAddSectionAndPerilsName, bStoredProcedure:=ACAddSectionAndPerilsStored)


            Exit Function

        Catch ex As Exception
            CopyRatingSectionsAndPerils = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatingSectionsAndPerils Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatingSectionsAndPerils", vErrNo:=Informations.Err.Number, vErrDesc:=Informations.Err.Description, excep:=ex)
        End Try
    End Function




    Public Function GetAllRiskStatus(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bIsRisksQuoted As Boolean) As Integer

        Dim dtResult As New DataTable

        Const kMethodName As String = "GetAllRiskStatus"

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            ' get the risk status of the risk
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAllRiskStatusSQL, sSQLName:=ACGetAllRiskStatusName, bStoredProcedure:=ACGetAllRiskStatusStored, oRecordset:=dtResult)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GetAllRiskStatus = m_lReturn
                Exit Function
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_bIsRisksQuoted = False
            Else
                r_bIsRisksQuoted = True
            End If

            GetAllRiskStatus = gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=GetAllRiskStatus, excep:=ex)

        Finally


        End Try
    End Function
    ''' <summary>
    ''' delete claim ri arrangement
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="sClaimVersionDescription"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function FinaliseClaimDetails(ByVal nClaimId As Integer, ByVal sClaimVersionDescription As String) As Integer

        Dim oChangeClaimStatus As bCLMChangeClaimStatus.Business
        Dim nReturn As Integer
        Dim nResult As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue









            oChangeClaimStatus = New bCLMChangeClaimStatus.Business
            nReturn = oChangeClaimStatus.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'RaiseError "CreateBusinessObject", "Failed to Create Object bControlTransClaims.Automated"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = oChangeClaimStatus.FinaliseClaimDetails(v_lClaimId:=nClaimId,
                                                              v_sClaimVersionDescription:=sClaimVersionDescription)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FinaliseClaimDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FinaliseClaimDetails", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally
            oChangeClaimStatus = Nothing
        End Try
        Return nResult
    End Function


    ''' <summary>
    ''' this method update claim status
    ''' </summary>
    ''' <param name="nClaimId"></param>
    '''  <param name="nStatus"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function UpdateClaimRIStatus(ByVal nClaimId As Integer, ByVal nStatus As Short) As Integer
        Const kMethodName As String = "UpdateClaimRIStatus"

        Dim nResult As gPMConstants.PMEReturnCode
        Dim nReturn As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="ClaimId", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="status", vValue:=nStatus, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute selection Query
            nReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateClaimRIStatusSQL, sSQLName:=ACUpdateClaimRIStatusName, bStoredProcedure:=ACUpdateClaimRIStatusStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACUpdateClaimRIStatusSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            Return nResult
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateClaimRIStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateClaimRIStatus", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
            ' If you want to rollback a transaction or something, do it here

        End Try
    End Function

    ''' <summary>
    ''' this method reverses stats details for claim clone reversal
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <param name="nStatsFolderCnt"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ReverseClaimStats(ByVal nClaimId As Integer, ByVal nStatsFolderCnt As Integer, Optional ByVal v_sTransactionTypeCode As String = "", Optional ByRef r_bThisRevesionPresent As Boolean = False) As Integer
        Const kMethodName As String = "ReverseClaimStats"
        Dim nResult As Integer
        Dim nReturn As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            nReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="New_Stats_Folder_Cnt", vValue:=nStatsFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=v_sTransactionTypeCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="ThisRevesionPresent", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute selection Query
            nReturn = m_oDatabase.SQLAction(sSQL:=ACReverseClaimStatsSQL, sSQLName:=ACReverseClaimStatsName, bStoredProcedure:=ACReverseClaimStatsStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACReverseClaimStatsSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_bThisRevesionPresent = ToSafeBoolean(m_oDatabase.Parameters.Item("ThisRevesionPresent").Value)
        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReverseClaimStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReverseClaimStats", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nResult

    End Function


    ''' <summary>
    ''' calculate RI for single policy
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_nIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RecalculateRI(ByVal nInsuranceFileCnt As Integer, ByRef r_nIsValid As Integer) As Integer
        Dim nReturn As Integer
        Try
            ' set the params
            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMLong)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="nIs_valid", vValue:=1,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' get the risk status of the risk
            nReturn = m_oDatabase.SQLAction(
                sSQL:=kRecalculateRISQL,
                sSQLName:=kRecalculateRIName,
                bStoredProcedure:=kRecalculateRIStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_nIsValid = Convert.ToInt32(m_oDatabase.Parameters.Item("nIs_valid").Value)


        Catch ex As System.Exception

            nReturn = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecalculateRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateRI", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nReturn
    End Function

    ''' <summary>
    ''' GetProductAndBranchDetails = returns product and branch details 
    ''' </summary>
    ''' <param name="r_dtProductDetails"></param>
    ''' <param name="r_dtBranchDetails"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetProductAndBranchDetails(ByRef r_dtProductDetails As DataTable, ByRef r_dtBranchDetails As DataTable) As Integer


        Dim nResult As Integer
        Dim nReturn As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetProduct", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetBranch", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If m_oDatabase.ExecuteDataTable(sSQL:="spu_get_all_product_and_branch_details", sSQLName:="select product", bStoredProcedure:=True, oRecordset:=r_dtProductDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetProduct", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetBranch", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oDatabase.ExecuteDataTable(sSQL:="spu_get_all_product_and_branch_details", sSQLName:="select branch", bStoredProcedure:=True, oRecordset:=r_dtBranchDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetProductAndBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductAndBranchDetails", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_nIsInValid"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function IsValidInsuranceFileToAmend( _
            ByVal nInsuranceFileCnt As Integer, _
            ByRef r_nIsInValid As Integer) As Integer

        Dim nResult As Integer
        Dim nReturn As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="nIs_InValid", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' get the risk status of the risk
            nReturn = m_oDatabase.SQLAction( _
                    sSQL:="spu_Check_Valid_Insurance_File_For_Clone", _
                    sSQLName:="spu_Check_Valid_Insurance_File_For_Clone", _
                    bStoredProcedure:=True)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_nIsInValid = Convert.ToInt32(m_oDatabase.Parameters.Item("nIs_InValid").Value)



        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            nResult = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="IsValidInsuranceFileToAmend Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="IsValidInsuranceFileToAmend", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
        Return nResult

    End Function
    ''' <summary>
    ''' this method generates stats folder
    ''' </summary>
    ''' <param name="sTransactionTypeCode"></param>
    ''' <param name="nClaimId"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function CreateStatsFolder(ByVal sTransactionTypeCode As String,
                                      ByVal nClaimId As Integer,
                                      ByRef r_nStatsFolderCnt As Integer) As Integer

        Dim nResult As Integer
        Dim nReturn As Integer
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt", vValue:=0,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code", vValue:=sTransactionTypeCode,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMString)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=m_iUserID,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                CreateStatsFolder = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="claim_id", vValue:=nClaimId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute Add Stats Folder SQL Statement
            nReturn = m_oDatabase.SQLAction(sSQL:="spu_clm_add_stats_folder",
                                            sSQLName:="AddStatsFolderClaims",
                                            bStoredProcedure:=True)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the Cnt of the record inserted
            r_nStatsFolderCnt = m_oDatabase.Parameters.Item("stats_folder_cnt").Value


            If (r_nStatsFolderCnt < 1) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateStatsFolder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateStatsFolder", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)


        End Try
        Return nResult
    End Function
    ''' <summary>
    ''' update in archive tables 
    ''' </summary>
    ''' <param name="nClaimId"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ClaimRIArrangementArchive(ByVal nClaimId As Integer) As Integer
        Const kMethodName As String = "ClaimRIArrangementArchive"

        Dim nReturn As Long
        Try

            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nClaim_id", vValue:=nClaimId,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' Execute selection Query
            nReturn = m_oDatabase.SQLAction(sSQL:=kClaimRIArrangementArchiveSQL,
                                            sSQLName:=kClaimRIArrangementArchiveName,
                                            bStoredProcedure:=kClaimRIArrangementArchiveStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, ACClaimRIArrangementDelSQL & " Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ClaimRiArrangementArchive Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ClaimRiArrangementArchive", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nReturn
    End Function
    ''' <summary>
    ''' calculate RI for single quote
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RecalculateRIQuote(
                                       ByVal nInsuranceFileCnt As Integer,
                                       ByRef nIsValid As Integer) As Integer

        Dim nResult As Integer
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nInsurance_file_cnt",
                vValue:=nInsuranceFileCnt,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="nIs_valid",
                vValue:=1,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            ' get the risk status of the risk
            m_lReturn = m_oDatabase.SQLAction(
                sSQL:=kRecalculateRIQuoteSQL,
                sSQLName:=kRecalculateRIQuoteName,
                bStoredProcedure:=kRecalculateRIQuoteStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            nIsValid = Convert.ToInt16(m_oDatabase.Parameters.Item("nIs_valid").Value)



        Catch ex As System.Exception
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RecalculateRIQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateRIQuote", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)


        End Try
        Return nResult
    End Function

End Class
