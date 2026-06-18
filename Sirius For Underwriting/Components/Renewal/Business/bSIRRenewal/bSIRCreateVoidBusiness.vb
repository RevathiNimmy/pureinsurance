Option Strict Off
Option Explicit On
Imports System.Data
Imports SSP.Shared


<System.Runtime.InteropServices.ProgId("Business_NET.VoidBusiness")>
Public NotInheritable Class VoidBusiness
    Implements IDisposable


    Private m_sUsername As String = ""

    Private m_sPassword As String = ""

    Private m_iUserID As Integer

    Private m_sCallingAppName As String = ""
    Private m_nSourceID As Integer
    Private m_nLanguageID As Integer
    Private m_nCurrencyID As Integer
    Private m_nLogLevel As Integer
    ' ************************************************

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "VoidBusiness"

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Current Record Pointer
    Private m_nCurrentRecord As Integer

    ' Error Code (Private)
    Private m_nReturn As gPMConstants.PMEReturnCode

    ' Process Mode Properties
    Private m_nTask As Integer
    Private m_nNavigate As Integer
    Private m_nProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Need for ProcessSinglePolicy()
    Private m_oRenSelection As Object
    Private m_oInsuranceFile As Object
    Private m_oReinsurance As Object
    Private m_oRiskData As Object
    Private m_oPerilAllocation As Object
    Private m_oControlTrans As Object
    Private m_oAllocationPost As Object
    Private m_oAllocationManual As Object

    Private m_oTax As Object

    Private m_oValue As Object
    Private m_bIsRI2007Enabled As Boolean
    Private m_oReinsuranceClaim As Object

    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public ReadOnly Property Task() As Integer
        Get
            Return m_nTask
        End Get
    End Property

    Public ReadOnly Property Navigate() As Integer
        Get
            Return m_nNavigate
        End Get
    End Property

    Public ReadOnly Property ProcessMode() As Integer
        Get
            Return m_nProcessMode
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
    Private disposedValue As Boolean
    Public Sub Dispose() Implements IDisposable.Dispose
        Dispose(True)
        GC.SuppressFinalize(Me)
    End Sub

    Protected Sub Dispose(disposing As Boolean)
        If Not Me.disposedValue Then
            If disposing Then
                CloseBusinessObjects()
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                    m_oDatabase = Nothing
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub

    ''' <summary>
    ''' Entry point for any initialisation code for this
    '''              object.
    ''' </summary>
    ''' <param name="sUsername"></param>
    ''' <param name="sPassword"></param>
    ''' <param name="iUserID"></param>
    ''' <param name="iSourceID"></param>
    ''' <param name="iLanguageID"></param>
    ''' <param name="iCurrencyID"></param>
    ''' <param name="iLogLevel"></param>
    ''' <param name="sCallingAppName"></param>
    ''' <param name="bStandAlone"></param>
    ''' <param name="vDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long

        Dim nResult As Integer = 0
        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_nLanguageID = iLanguageID
            m_nSourceID = iSourceID
            m_nCurrencyID = iCurrencyID
            m_nLogLevel = iLogLevel

            nResult = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_nSourceID, m_nLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nCurrentRecord = 0
            m_nProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create relevant business objects to do ProcessSinglePolicy()
            nResult = CType(CreateBusinessObjects(), gPMConstants.PMEReturnCode)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult

        End Try
    End Function

    Private Sub CloseBusinessObjects()

        Try

            If Not (m_oRenSelection Is Nothing) Then

                m_nReturn = m_oRenSelection.Dispose()
                m_oRenSelection = Nothing
            End If

            If Not (m_oInsuranceFile Is Nothing) Then

                m_nReturn = m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

            If Not (m_oReinsurance Is Nothing) Then

                m_nReturn = m_oReinsurance.Dispose()
                m_oReinsurance = Nothing
            End If

            If Not (m_oRiskData Is Nothing) Then

                m_nReturn = m_oRiskData.Dispose()
                m_oRiskData = Nothing
            End If

            If Not (m_oPerilAllocation Is Nothing) Then

                m_nReturn = m_oPerilAllocation.Dispose()
                m_oPerilAllocation = Nothing
            End If

            If Not (m_oControlTrans Is Nothing) Then

                m_nReturn = m_oControlTrans.Dispose()
                m_oControlTrans = Nothing
            End If
            If (Not (m_oReinsuranceClaim Is Nothing)) Then
                m_nReturn = m_oReinsuranceClaim.Dispose()
                m_oReinsuranceClaim = Nothing
            End If

            If (Not (m_oAllocationPost Is Nothing)) Then
                m_nReturn = m_oAllocationPost.Dispose()
                m_oAllocationPost = Nothing
            End If

        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Function CreateBusinessObjects() As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            sMessage = ""
            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oRenSelection, v_sClassName:="bSIRRenSelection.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRenSelection.Business"
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oInsuranceFile, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRInsuranceFile.Services"
                Throw New Exception(sMessage)
            End If

            ' E007 Portfolio Transfer - Kuljeet Kaur
            m_nReturn = bPMFunc.getProductOptionValue(v_sUsername:="",
                            v_sPassword:="", v_iUserID:=0,
                            v_iMainSourceID:=0, v_iLanguageID:=0,
                            v_iCurrencyID:=0, v_iLogLevel:=0,
                            v_sCallingAppName:="",
                            v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRI2007,
                            v_vBranch:=m_nSourceID,
                            r_vUnderwriting:=m_oValue)
            If m_oValue = "1" Then
                m_bIsRI2007Enabled = True
            Else
                m_bIsRI2007Enabled = False
            End If

            If m_bIsRI2007Enabled Then
                m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            End If

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRReinsurance.Form"
                Throw New Exception(sMessage)
            End If

            If gPMComponentServices.CreateBusinessObject(r_oObject:=m_oTax, v_sClassName:="bSIRRITax.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRITax.business"
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oRiskData, v_sClassName:="bSIRRiskData.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                sMessage = "Failed to create an instance of bSIRRiskData.Business"
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oPerilAllocation, v_sClassName:="bSirPerilAllocation.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRPerilAllocation.Business"
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oControlTrans, v_sClassName:="bControlTrans.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bControlTrans.Automated"
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsuranceClaim,
                                                        v_sClassName:=IIf(m_bIsRI2007Enabled, "bCLMReinsuranceRI2007.Form", "bCLMReinsurance.Form"),
                                                        v_sCallingAppName:=ACApp,
                                                        v_sUsername:=m_sUsername,
                                                        v_sPassword:=m_sPassword,
                                                        v_iUserID:=m_iUserID,
                                                        v_iSourceID:=m_nSourceID,
                                                        v_iLanguageID:=m_nLanguageID,
                                                        v_iCurrencyID:=m_nCurrencyID,
                                                        v_iLogLevel:=m_nLogLevel,
                                                        v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_bIsRI2007Enabled = True Then
                    sMessage = "Failed to create an instance of bCLMReinsuranceRI2007.Form"
                Else
                    sMessage = "Failed to create an instance of bCLMReinsurance.Form"
                End If
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationPost, v_sClassName:="bACTAllocationPost.Automated", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bACTAllocationPost.Automated"
                Throw New Exception(sMessage)
            End If

            m_nReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oAllocationManual, v_sClassName:="bACTAllocationManual.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_nSourceID, v_iLanguageID:=m_nLanguageID, v_iCurrencyID:=m_nCurrencyID, v_iLogLevel:=m_nLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)


            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bACTAllocationManual.Business"
                Throw New Exception(sMessage)
            End If


            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=IIf(sMessage = "", "Failed CreateBusinessObjects", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjects", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            CloseBusinessObjects()

            Return result
        End Try
    End Function

    Private Function DeleteQuote(ByVal v_lInsuranceFileCnt As Integer, ByVal v_nInsuranceFolderCnt As Integer) As Integer
        Dim v_vResults As Object = Nothing
        m_nReturn = gPMConstants.PMEReturnCode.PMTrue
        Try
            ' Clear Down Database Parameters
            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=v_nInsuranceFolderCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute selection Query
            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetQuotePolicyVersionsSQL, sSQLName:=ACGetQuotePolicyVersionsName, bStoredProcedure:=ACGetQuotePolicyVersionsStored, vResultArray:=v_vResults, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to DeleteQuote", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                m_nReturn = gPMConstants.PMEReturnCode.PMError
            End If

            If Informations.IsArray(v_vResults) Then
                For iCount As Integer = 0 To v_vResults.GetUpperBound(1)
                    If (ToSafeString(v_vResults(3, iCount)) <> "RENEWAL") Then
                        m_nReturn = m_oRenSelection.DeletePolicy(ToSafeInteger(v_vResults(0, iCount)))
                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to DeletePolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            m_nReturn = gPMConstants.PMEReturnCode.PMError
                        End If
                    Else
                        m_nReturn = m_oRenSelection.DeleteRenewal(ToSafeInteger(v_vResults(0, iCount)))
                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to DeletePolicy", vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            m_nReturn = gPMConstants.PMEReturnCode.PMError
                        End If
                    End If

                Next
            End If
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to DeleteQuote", vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            m_nReturn = gPMConstants.PMEReturnCode.PMError
        End Try
        Return m_nReturn
    End Function
    ''' <summary>
    ''' Create a void version 
    ''' </summary>
    ''' <param name="v_nInsuranceFileCnt"></param>
    ''' <param name="r_sMessage"></param>
    ''' <returns></returns>
    Public Function ProcessVoidVersion(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nInsuranceFolderCnt As Integer, ByRef r_sMessage As String, ByRef r_nNewInsuranceFileCnt As Integer) As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim v_nNewInsuranceFileCnt As Integer
        Dim v_iInsuranceFileTypeId As Integer
        Dim sTransactionType As String
        Dim dtPolicyStartDate As Date
        Dim iContinue As Integer = 0
        Try
            nReturnValue = gPMConstants.PMEReturnCode.PMTrue
            nReturnValue = CheckPolicyForVoid(v_nInsuranceFileCnt, iContinue)
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                r_nNewInsuranceFileCnt = 0
                r_sMessage = "Failed to get the policy version for void "
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPolicyForVoid", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If iContinue = 1 Then
                nReturnValue = DeleteQuote(v_nInsuranceFileCnt, v_nInsuranceFolderCnt)
                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_nNewInsuranceFileCnt = 0
                    r_sMessage = "Failed to delete MTA quote"
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Set transaction so we can roll back if something gone wrong
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                    r_sMessage = "Failed to begin SQLTransaction"
                    Return nReturnValue
                End If

                m_oInsuranceFile.InsuranceFileCnt = v_nInsuranceFileCnt
                If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                    r_sMessage = "Failed to get policy details for " & v_nInsuranceFileCnt
                    Return nReturnValue
                End If

                v_iInsuranceFileTypeId = ToSafeInteger(m_oInsuranceFile.InsuranceFileTypeId)
                sTransactionType = ToSafeString(m_oInsuranceFile.LastTransType)
                dtPolicyStartDate = ToSafeDate(m_oInsuranceFile.CoverStartDate)

                nReturnValue = CreatePolicy(v_nInsuranceFileCnt, v_nNewInsuranceFileCnt)

                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_nNewInsuranceFileCnt = 0
                    r_sMessage = "Failed to create void the policy"
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    nReturnValue = m_oDatabase.SQLRollbackTrans
                    Return PMEReturnCode.PMFalse
                End If

                r_nNewInsuranceFileCnt = v_nNewInsuranceFileCnt

                If r_nNewInsuranceFileCnt > 0 Then
                    nReturnValue = CopyAllRisk(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, v_nNewInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                                           v_nInsuranceFolderCnt:=v_nInsuranceFolderCnt, v_dtPolicyStartDate:=dtPolicyStartDate,
                                           v_sTransactionType:=sTransactionType, r_sMessage:=r_sMessage)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                        r_sMessage = "Failed to copy risk details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(v_nNewInsuranceFileCnt) & " )"
                        nReturnValue = m_oDatabase.SQLRollbackTrans
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAllRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If


                    nReturnValue = CreateAndPostStats(v_lNewInsuranceFileCnt:=v_nNewInsuranceFileCnt, v_sTransactionType:=sTransactionType,
                                   v_lOldInsuranceFileCnt:=v_nInsuranceFileCnt, r_sMessage:=r_sMessage)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                        nReturnValue = m_oDatabase.SQLRollbackTrans
                        r_sMessage = "Failed to create stats for policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(v_nNewInsuranceFileCnt) & " )"
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAndPostStats", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    'nReturnValue = ContinueVoidProcess(v_nInsuranceFileCnt, r_sMessage, r_nNewInsuranceFileCnt, sTransactionType)

                    'If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    '    r_nNewInsuranceFileCnt = 0
                    '    r_sMessage = "Failed to copy risk or stats"
                    '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    '    nReturnValue = m_oDatabase.SQLRollbackTrans
                    '    Return PMEReturnCode.PMFalse
                    'End If

                    nReturnValue = UpdatePolicyStatus(v_nInsuranceFileCnt,sTransactionType)

                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_nNewInsuranceFileCnt = 0
                        r_sMessage = "Failed to update policy status"
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        nReturnValue = m_oDatabase.SQLRollbackTrans
                        Return PMEReturnCode.PMFalse
                    End If

                    r_sMessage = ""
                    nReturnValue = m_oDatabase.SQLCommitTrans
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to commit SQLTransaction"
                        nReturnValue = m_oDatabase.SQLRollbackTrans
                        Return PMEReturnCode.PMFalse
                    End If

                    Dim iContinueReversal As Integer = 1
                    nReturnValue = CheckInstalmentBusiness(v_linsurance_file_cnt:=v_nInsuranceFileCnt, iContinueReversal)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to check whether it is an instalment business."
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If iContinueReversal = 1 Then

                        If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
                            r_sMessage = "Failed to begin SQLTransaction"
                            Return nReturnValue
                        End If


                        nReturnValue = ReverseAllocation(v_lInsuranceFileCnt:=v_nInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_nNewInsuranceFileCnt)
                        If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to reverse the allocation"
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                            nReturnValue = m_oDatabase.SQLRollbackTrans
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        nReturnValue = m_oDatabase.SQLCommitTrans
                        If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to commit SQLTransaction"
                            nReturnValue = m_oDatabase.SQLRollbackTrans
                            Return PMEReturnCode.PMFalse
                        End If
                    End If
                Else
                    r_nNewInsuranceFileCnt = 0
                    r_sMessage = "Failed to void policy."
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    nReturnValue = m_oDatabase.SQLRollbackTrans
                    Return PMEReturnCode.PMFalse
                End If
            End If
        Catch ex As Exception
            nReturnValue = m_oDatabase.SQLRollbackTrans
            m_oRenSelection.DeletePolicy(CInt(r_nNewInsuranceFileCnt))
            ResetPolicyStatus(v_nInsuranceFileCnt, v_iInsuranceFileTypeId)
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=IIf(r_sMessage = "", "Failed ProcessVoidVersion()", r_sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessVoidVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try

        Return nReturnValue

    End Function

    'Private Function ContinueVoidProcess(ByVal v_nInsuranceFileCnt As Integer, ByRef r_sMessage As String, ByVal v_nNewInsurancefileCnt As Integer, ByVal v_sTransactionType As String) As Integer
    '    Dim nReturnValue As gPMConstants.PMEReturnCode
    '    ' Dim nNewInsurancefileCnt As Integer
    '    Dim nInsuranceFolderCnt As Integer
    '    Dim dtPolicyStartDate As Date
    '    ' Dim sTransactionType As String = ""
    '    Dim sInsuranceRef As String
    '    Dim nMaxPolicyVersion As Integer

    '    Try
    '        nReturnValue = gPMConstants.PMEReturnCode.PMTrue

    '        ''****************** Create new version of policy ************************
    '        '' Assign current insurance file count to business object
    '        'm_oInsuranceFile.InsuranceFileCnt = v_nInsuranceFileCnt

    '        '' Get details of current policy
    '        'If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        '    r_sMessage = "Failed to get policy details for " & v_nInsuranceFileCnt
    '        '    Return nReturnValue
    '        'End If
    '        'sTransactionType = m_oInsuranceFile.LastTransType

    '        '' Set old version to "Void replaced for MTA/MTR" 
    '        '' Set old version to "Void renewal replaced for Renewal" 
    '        'm_oInsuranceFile.InsuranceFileStatus = gSIRLibrary.SIRInsFileStatusVoid
    '        'm_oInsuranceFile.OriginalInsuranceFileTypeID = m_oInsuranceFile.InsuranceFileTypeId
    '        'If m_oInsuranceFile.InsuranceFileType = "POLICY" Then
    '        '    m_oInsuranceFile.InsuranceFileType = "VOIDRENREP"
    '        'Else
    '        '    m_oInsuranceFile.InsuranceFileType = "VOIDREP"
    '        'End If

    '        'm_oInsuranceFile.EventDescription = ToSafeString(m_oInsuranceFile.EventDescription) + " and voided"
    '        'm_oInsuranceFile.UpdatePolicy()
    '        'm_oInsuranceFile.MakeEvent()
    '        'm_oInsuranceFile.ThisPremium = m_oInsuranceFile.ThisPremium * -1
    '        'm_oInsuranceFile.NetPremium = m_oInsuranceFile.NetPremium * -1
    '        'm_oInsuranceFile.EventDescription = "Void counter version of policy version " & m_oInsuranceFile.PolicyVersion

    '        'nReturnValue = GetMaxPolicyVersion(v_nInsuranceFileCnt, nMaxPolicyVersion)
    '        'If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    r_sMessage = "Failed to get Max Policy Version for InsuranceFile " & v_nInsuranceFileCnt
    '        '    Return nReturnValue
    '        'End If

    '        'm_oInsuranceFile.PolicyVersion = nMaxPolicyVersion + 1

    '        'm_oInsuranceFile.InsuranceFileStatus = gSIRLibrary.SIRInsFileStatusVoid
    '        'm_oInsuranceFile.InsuranceFileType = "VOID"

    '        '' create new version of policy based on current policy details
    '        'If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        '    r_sMessage = "Failed to create new version of policy for " & v_nInsuranceFileCnt
    '        '    Return nReturnValue
    '        'End If
    '        'm_oInsuranceFile.MakeEvent()

    '        ''******************** Get new policy version details ******************************************

    '        'nNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

    '        'nInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt 'note folder will be the same as previous policy version

    '        'dtPolicyStartDate = m_oInsuranceFile.CoverStartDate

    '        'sInsuranceRef = m_oInsuranceFile.InsuranceRef

    '        ''****************** Copy related data to new policy version ***********************
    '        '' Copy standard wording

    '        'If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_nInsuranceFileCnt, v_lNewInsuranceFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        '    r_sMessage = "Failed to copy standard wording to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '        '    Return nReturnValue
    '        'End If

    '        '' Copy coinsurance

    '        'If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=v_nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        '    r_sMessage = "Failed to copy coinsurance details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '        '    Return nReturnValue
    '        'End If

    '        '' Copy agent commission

    '        'If CopyAgentCommission(v_lCurrentInsFileCnt:=v_nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        '    r_sMessage = "Failed to copy agent commission details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '        '    Return nReturnValue
    '        'End If

    '        '' Copy agents
    '        'If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=v_nInsuranceFileCnt, v_lNewInsFileCnt:=nNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
    '        '    nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        '    r_sMessage = "Failed to copy agent details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '        '    Return nReturnValue
    '        'End If

    '        ' Copy risk details (error messages are dealt within CopyAllRisk())
    '        If CopyAllRisk(v_nInsuranceFileCnt:=v_nInsuranceFileCnt, v_nNewInsuranceFileCnt:=v_nNewInsurancefileCnt, v_nInsuranceFolderCnt:=nInsuranceFolderCnt, v_dtPolicyStartDate:=dtPolicyStartDate, v_sTransactionType:=v_sTransactionType, r_sMessage:=r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '            r_sMessage = "Failed to copy risk details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(v_nNewInsurancefileCnt) & " )"
    '            Return nReturnValue
    '        End If

    '        ' Process stats (error messages are dealt within CreateAndPostStats())
    '        If CreateAndPostStats(v_lNewInsuranceFileCnt:=v_nNewInsurancefileCnt, v_sTransactionType:=v_sTransactionType, v_lOldInsuranceFileCnt:=v_nInsuranceFileCnt, r_sMessage:=r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '            r_sMessage = "Failed to create stats for policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(v_nNewInsurancefileCnt) & " )"
    '            Return nReturnValue
    '        End If

    '    Catch ex As Exception
    '        nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=IIf(r_sMessage = "", "Failed StartVoidProcess()", r_sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="StartVoidProcess", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
    '    End Try

    '    Return nReturnValue
    'End Function

    'Public Function CopyAgentCommission(ByVal v_lCurrentInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer) As Integer

    '    Dim result As Integer = 0
    '    Try

    '        result = gPMConstants.PMEReturnCode.PMTrue

    '        m_oDatabase.Parameters.Clear()
    '        m_nReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lCurrentInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        m_nReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CStr(v_lNewInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        m_nReturn = m_oDatabase.SQLAction(sSQL:=ACVoidCopyAgentCommissionSQL, sSQLName:=ACVoidCopyAgentCommissionName, bStoredProcedure:=ACVoidCopyAgentCommissionStored)
    '        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return gPMConstants.PMEReturnCode.PMFalse
    '        End If

    '        Return result

    '    Catch excep As System.Exception
    '        result = gPMConstants.PMEReturnCode.PMError
    '        ' Log Error Message
    '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyAgentCommission Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAgentCommission", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)
    '        Return result
    '    End Try
    'End Function

    'Private Function GetMaxPolicyVersion(ByVal v_nInsuranceFileCnt As Integer,
    '                                ByRef r_nMaxPolicyVersion As Integer) As Integer
    '    Try
    '        m_nReturn = gPMConstants.PMEReturnCode.PMTrue

    '        With m_oDatabase
    '            .Parameters.Clear()
    '            ' Add parameters
    '            m_nReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=v_nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '            If (m_nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
    '                Return m_nReturn
    '            End If
    '            m_nReturn = .Parameters.Add(sName:="max_version_no", vValue:=r_nMaxPolicyVersion, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
    '            If (m_nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
    '                Return m_nReturn
    '            End If
    '            m_nReturn = .SQLAction(sSQL:=kGetMaxPolicyVersionSQL, sSQLName:=kGetMaxPolicyVersionName, bStoredProcedure:=kGetMaxPolicyVersionStored)

    '            If (m_nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
    '                Return m_nReturn
    '            End If

    '            r_nMaxPolicyVersion = ToSafeCurrency(.Parameters.Item("max_version_no").Value, 0)
    '        End With

    '    Catch ex As Exception
    '        m_nReturn = gPMConstants.PMEReturnCode.PMError
    '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetMaxPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetMaxPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
    '    End Try
    '    Return m_nReturn
    'End Function

    Public Function CopyAllRisk(ByVal v_nInsuranceFileCnt As Integer, ByVal v_nNewInsuranceFileCnt As Integer, ByVal v_nInsuranceFolderCnt As Integer, ByVal v_dtPolicyStartDate As Date,
                                ByVal v_sTransactionType As String, ByRef r_sMessage As String) As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim oRiskArray(,) As Object = Nothing
        Dim oGISPolicyLinkArray As Object = Nothing
        Dim nRiskCnt As Integer
        Dim nNewRiskCnt As Integer
        Dim sGISDataModelCode As String = ""
        Dim nGISPolicyLinkID As Integer
        Dim nNewGisPolicyLinkID As Integer
        Dim nPolicyBinderID As Integer
        Dim nNewPolicyBinderID As Integer
        Dim sXMLDataSetDef As String = ""
        Dim sXMLDataSet As String = ""
        Dim nValidRIBand As Object
        Dim nReinsBand As Object = 0  ' Not used - function parameter required
        Dim sDescription As String = ""
        Dim nRiskStatusID As Integer
        Dim sRiskStatusCode As String = ""
        Dim nCommitPolicy As gPMConstants.PMEReturnCode ' Set to true when one of the deferred risk is now on live model and everything is ok
        Dim nDeferredRIBandNewRisk As Integer ' Number of bands which are on deferred RI model
        Dim nTransactionStarted As gPMConstants.PMEReturnCode ' Set to pmtrue when m_oDatabase.SQLBeginTrans() is called
        Dim dProRataRate As Double
        Const kFieldPosRiskID As Integer = 0
        Const kFieldPosGisScreenID As Integer = 21

        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue
            r_sMessage = ""
            nValidRIBand = gPMConstants.PMEReturnCode.PMFalse
            nCommitPolicy = gPMConstants.PMEReturnCode.PMFalse
            nTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

            ' Get risk associated with this policy

            If GetRisk(v_lInsuranceFileCnt:=v_nInsuranceFileCnt, r_vResultArray:=oRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to get associated risks for this policy " & v_nInsuranceFileCnt
                Return PMEReturnCode.PMFalse
            End If

            ' Check if we have any risks
            If Not Informations.IsArray(oRiskArray) Then
                Return nReturnValue
            End If

            ' Make sure we have the right transaction type

            If m_oRiskData.SetProcessModes(vTransactionType:=CStr(v_sTransactionType)) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode to RiskData object (bSIRRiskData.business)"
                Return PMEReturnCode.PMFalse
            End If

            ' m_nReturn = GetPolicyType(v_nInsuranceFileCnt, nInsuranceFileType)
            ' Process each risk

            For iCount As Integer = 0 To oRiskArray.GetUpperBound(1)

                nTransactionStarted = gPMConstants.PMEReturnCode.PMTrue
                nRiskCnt = gPMFunctions.NullToLong(oRiskArray(kFieldPosRiskID, iCount))
                'm_nReturn = GetPreviousRiskCnt(v_nPreInsuranceFileCnt:=v_nNewInsuranceFileCnt,
                '                                  v_nRiskCnt:=nRiskCnt,
                '                                  v_nInsuranceFileCnt:=v_nInsuranceFileCnt,
                '                                  r_nPreviousRiskCnt:=nPreviousRiskCnt)

                If CopyRisk(v_nNewInsuranceFileCnt:=v_nNewInsuranceFileCnt, v_oRiskDetail:=oRiskArray, v_nPosNo:=iCount, r_nRiskCnt:=nNewRiskCnt,
                            v_nResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_nCreateLinkType:=1, v_nOldRiskCnt:=nRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_nInsuranceFileCnt & " new policy " & v_nNewInsuranceFileCnt & " Risk :)" & nRiskCnt & ")"
                    Return PMEReturnCode.PMFalse
                End If
                ' End If

                '************************ copy gis data for this risk *****************************
                ' Get get policy link for old policy
                If m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=CInt(v_nInsuranceFolderCnt), v_lRiskID:=CInt(nRiskCnt), r_vResultArray:=oGISPolicyLinkArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to copy gis details to new policy version (old policy :" & v_nInsuranceFileCnt & " new policy: " & CStr(v_nNewInsuranceFileCnt) & " Risk: " & CStr(nRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Do we have any gis data
                nGISPolicyLinkID = gPMFunctions.NullToLong(oGISPolicyLinkArray(0, 0))
                sGISDataModelCode = gPMFunctions.NullToString(oGISPolicyLinkArray(4, 0)).Trim()

                If nGISPolicyLinkID = 0 Then
                    r_sMessage = "We have no gis data for policy version (old policy :" & v_nInsuranceFileCnt & " Risk: " & CStr(nRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Load gis detail for old risk (Note: gis object store folder_cnt in file_cnt field)
                If m_oRenSelection.GIS_LoadFromDB(v_sGisDataModelCode:=CStr(sGISDataModelCode), r_vInsuranceFileCnt:=CInt(v_nInsuranceFolderCnt), r_vPolicyLinkID:=CInt(nGISPolicyLinkID), r_vRiskID:=CInt(nRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to load gis data (policy folder :" & v_nInsuranceFolderCnt & " Risk: " & CStr(nRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Copy gis data to new risk (Note: gis object store folder_cnt in file_cnt field)
                If m_oRenSelection.CopyDataSet(v_sDataModelCode:=CStr(sGISDataModelCode), r_lNewGISPolicyLinkId:=CInt(nNewGisPolicyLinkID), r_sXMLDataSetDef:=CStr(sXMLDataSetDef), r_sXMLDataSet:=CStr(sXMLDataSet), v_vOldGISPolicyLinkId:=CInt(nGISPolicyLinkID), v_vOldInsuranceFileCnt:=CInt(v_nInsuranceFolderCnt), v_vOldRiskID:=CInt(nRiskCnt), v_vNewInsuranceFileCnt:=CInt(v_nInsuranceFolderCnt), v_vNewRiskID:=CInt(nNewRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to copy gis data (policy folder :" & v_nInsuranceFolderCnt & "Old Risk: " & CStr(nRiskCnt) & " New Risk: " & CStr(nNewRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Initialise the Data Set with the Object/Properties
                If m_oRenSelection.LoadFromXML(v_sXMLDataSetDef:=CStr(sXMLDataSetDef), v_sXMLDataSet:=CStr(sXMLDataSet)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed load data from xml (policy folder :" & v_nInsuranceFolderCnt & "Old Risk: " & CStr(nRiskCnt) & " New Risk: " & CStr(nNewRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Save gis data for new risk
                If m_oRenSelection.GIS_SaveToDB(v_sGisDataModelCode:=CStr(sGISDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to save gis data for new risk (policy folder :" & v_nInsuranceFolderCnt & "Old Risk: " & CStr(nRiskCnt) & " New Risk: " & CStr(nNewRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Get policy binder for old policy version
                If m_oRenSelection.GetPolicyBinderID(v_sDataModelCode:=CStr(sGISDataModelCode), v_lGISPolicyLinkId:=CInt(nGISPolicyLinkID), r_lPolicyBinderID:=CInt(nPolicyBinderID)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to get policy binder for old policy version (gis policy link: " & CStr(nGISPolicyLinkID) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Get policy binder for new policy version
                If m_oRenSelection.GetPolicyBinderID(v_sDataModelCode:=CStr(sGISDataModelCode), v_lGISPolicyLinkId:=CInt(nNewGisPolicyLinkID), r_lPolicyBinderID:=CInt(nNewPolicyBinderID)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to get policy binder for new policy version (gis policy link: " & CStr(nNewGisPolicyLinkID) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Copy standard wording
                If m_oRenSelection.CopyRiskStandardWordings(v_lOldPolicyBinderID:=CInt(nPolicyBinderID), v_lNewPolicyBinderID:=CInt(nNewPolicyBinderID), v_sDataModelCode:=CStr(sGISDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to copy standard wording (Old Policy Binder: " & nPolicyBinderID & " New Policy Binder: " & CStr(nNewPolicyBinderID) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Do we need to do index linking
                Dim auxVar As Object = oRiskArray(kFieldPosGisScreenID, iCount)

                If Not (Convert.IsDBNull(auxVar) Or IsNothing(auxVar)) Then
                    'Note: the risk_id and v_nInsuranceFileCnt are not being used so it doesn't matter what we pass in
                    'this is because further up the code we already load up gis info

                    If m_oRenSelection.GISIndexLink(v_lInsuranceFileCnt:=0, v_lRiskID:=0, v_vGisScreenID:=oRiskArray(kFieldPosGisScreenID, iCount), v_dtEffectiveDate:=CDate(v_dtPolicyStartDate), v_sGisDataModelCode:=CStr(sGISDataModelCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed index linking for policy: " & v_nNewInsuranceFileCnt
                        Return PMEReturnCode.PMFalse
                    End If
                End If

                ' Copy gis related sum insured
                If m_oRiskData.CopyRSASumInsured(v_lOldPolicyLinkID:=CInt(nGISPolicyLinkID), v_lNewPolicyLinkID:=CInt(nNewGisPolicyLinkID)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to copy gis related sum insured (Old policy link:" & nGISPolicyLinkID & " New policy link:" & CStr(nNewGisPolicyLinkID) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Note : rating section will only work when original risk on a policy is of type (2,5) and status of policy is (3,4,5,6)
                ' Do peril allocation for new policy version
                With m_oPerilAllocation

                    .InsuranceFileCnt = v_nNewInsuranceFileCnt
                    .InsuranceFolderCnt = v_nInsuranceFolderCnt
                    .RiskId = nNewRiskCnt
                    .TransactionType = v_sTransactionType
                End With

                ' Populate rating sections

                If m_oPerilAllocation.PopulateRatingSections() <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                dProRataRate = m_oPerilAllocation.ProRataRate

                If CopyPerilAndRating(v_nInsuranceFileCnt, nRiskCnt, nNewRiskCnt, dProRataRate, v_nNewInsuranceFileCnt) <> PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & nNewRiskCnt & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Update risk with premium and suminsured
               ' If m_oPerilAllocation.UpdateRisk() <> gPMConstants.PMEReturnCode.PMTrue Then
                '    r_sMessage = "Failed to update risk's premium and suminsured for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                '    Return PMEReturnCode.PMFalse
               ' End If

                '**************** Note on reinsurance *******************
                ' Reinsurance will work out which RI model is relevant for this risk
                ' If it can't find one it will return false

                'need to tell reinsurance that we are doing Portfolio transfer
                ' v_sTransactionType = "MTA"

                m_nReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vTransactionType:=CStr(v_sTransactionType), vEffectiveDate:=CDate(v_dtPolicyStartDate))

                ' Get ready to do reinsurance (risk level)

                m_oReinsurance.InsuranceFileCnt = v_nNewInsuranceFileCnt

                m_oReinsurance.RiskId = nNewRiskCnt

                ' Generate reinsurance for new policy version

                If m_oReinsurance.CalculateRI <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to generate RI for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Get reinsurance details (to fix roundings and validate)
                m_nReturn = m_oReinsurance.GetDetails()
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue AndAlso m_nReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    r_sMessage = "Failed to generate RI for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If m_oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Do we have valid reinsurance bands ie adds up to 100%

                If m_oReinsurance.ValidateBands(r_lValid:=nValidRIBand, r_lBand:=nReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to validate RI bands for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & nNewRiskCnt & ")"
                    Return PMEReturnCode.PMFalse
                End If

                ' Save reinsurance details
                'If m_oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                '    r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                '    Return PMEReturnCode.PMFalse
                'End If

                If CopyRiskTaxFee(nRiskCnt, v_nInsuranceFileCnt, nNewRiskCnt, v_nNewInsuranceFileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to apply risk taxes details for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                'If m_oRenSelection.ApplyRiskFee(nNewRiskCnt, v_nNewInsuranceFileCnt, r_sMessage, True, v_nInsuranceFileCnt, nPreviousRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                '    r_sMessage = "Failed to apply risk taxes details for new policy version (Policy: " & v_nNewInsuranceFileCnt & " Risk: " & CStr(nNewRiskCnt) & ")"
                '    Return gPMConstants.PMEReturnCode.PMFalse
                'End If

                ' Do we have valid bands ie adds up to 100%
                If nValidRIBand = gPMConstants.PMEReturnCode.PMFalse Then
                    ' Change old risk status to quoted so it won't get pick up again

                    If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=CInt(nRiskCnt), v_lRiskStatusID:=0, v_sRiskStatusCode:="QUOTED") <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to update old risk status to quoted (Risk: " & nRiskCnt & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' NOTE: after peril allocation risk_status_id = 8 (pending resinsurance) if everything is ok
                    '       reinsurance (business side) shouldn't change this risk status so we are expecting pending reinsurance
                    ' Get risk status for new risk
                    If GetRiskStatus(v_lRiskCnt:=nNewRiskCnt, r_lRiskStatusID:=nRiskStatusID, r_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to get status for new risk (Risk: " & nNewRiskCnt & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    If sRiskStatusCode.Trim().ToUpper() = "PENDINGRI" Then 'pending reinsurance
                        ' Somthing went wrong, set risk status to pending RI portfolio transfer
                        sRiskStatusCode = IIf(nDeferredRIBandNewRisk = 0, "QUOTED", "PENDINGRIP")

                        ' Update risk status

                        If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=CInt(nNewRiskCnt), v_lRiskStatusID:=0, v_sRiskStatusCode:=CStr(sRiskStatusCode)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to update new risk status to quoted (Risk: " & nNewRiskCnt & ")"
                            Return PMEReturnCode.PMFalse
                        End If
                    Else
                        r_sMessage = "Peril Allocation Failed or more questions need answering (Risk: " & nNewRiskCnt & ")"
                        Return PMEReturnCode.PMFalse
                    End If

                    ' Set this flag to stop policy from rolling back
                    nCommitPolicy = gPMConstants.PMEReturnCode.PMTrue

                End If
                'End If

            Next iCount

        Catch ex As Exception

            nReturnValue = gPMConstants.PMEReturnCode.PMFalse

            ' Let calling function log this message
            If r_sMessage = "" Then
                r_sMessage = Informations.Err().Description
            End If

            r_sMessage = r_sMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Failed in CopyAllRisk()"
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyAllRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAllRisk",
                               vErrNo:=Informations.Err().Number, vErrDesc:=r_sMessage & ex.Message, excep:=ex)
            Return nReturnValue

        End Try
        Return nReturnValue

    End Function

    Public Function CreateAndPostStats(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_sTransactionType As String, Optional ByVal v_lOldInsuranceFileCnt As Long = 0, Optional ByRef r_sMessage As String = "") As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode
        lReturnValue = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oControlTrans.InsuranceFileCnt = v_lNewInsuranceFileCnt
            m_oControlTrans.OriginalInsuranceFileCnt = v_lOldInsuranceFileCnt
            ' m_oControlTrans.PTInsuranceFileCnt = v_lOldInsuranceFileCnt
            m_oControlTrans.ReverseVoid = True

            m_nReturn = m_oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=CInt(m_nNavigate), vProcessMode:=CInt(m_nProcessMode), vTransactionType:=CStr(v_sTransactionType), vEffectiveDate:=CDate(m_dtEffectiveDate))

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode for bControlTrans.Automated"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_oControlTrans.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create and post stats"
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch ex As Exception

            lReturnValue = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateAndPostStats Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateAndPostStats",
                               vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        End Try

        Return lReturnValue

    End Function

    Public Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        Dim result As Integer = 0

        Try
            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_oDatabase.SQLSelect(sSQL:=ACGetVoidRiskSQL, sSQLName:=ACGetVoidRiskName, bStoredProcedure:=ACCGetVoidRiskStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRisk Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    Public Function CopyRisk(ByVal v_nNewInsuranceFileCnt As Integer,
                            ByVal v_oRiskDetail As Object,
                            ByVal v_nPosNo As Integer,
                            ByRef r_nRiskCnt As Integer,
                            Optional ByVal v_nResetStatus As Integer = 0,
                            Optional ByVal v_nCreateLinkType As Integer = 0,
                            Optional ByVal v_bAutoCancellation As Boolean = False,
                            Optional v_sRiskMergeStatus As String = "",
                            Optional v_nOldRiskCnt As Integer = 0) As Integer

        Dim nRiskCnt As Integer
        Dim nOldRiskCnt As Integer
        Dim nNewRiskCnt As Integer
        Dim dtResult As New DataTable
        Dim nIsAutoReinsured As Integer
        Dim oArray As Object
        Const kMethodName As String = "CopyRisk"
        Try

            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            nOldRiskCnt = v_oRiskDetail(0, v_nPosNo)

            'Tomo030801
            'This bit's here because we need to reset the auto reinsured flag to that
            'from the risk type.
            m_oDatabase.Parameters.Clear()
            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_oRiskDetail(4, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAutoReinsuredSQL, sSQLName:=ACGetAutoReinsuredName, bStoredProcedure:=ACGetAutoReinsuredStored, oRecordset:=dtResult)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                nIsAutoReinsured = CInt(dtResult.Rows(0)(0))
            Else
                nIsAutoReinsured = 0
            End If

            v_oRiskDetail(20, v_nPosNo) = nIsAutoReinsured

            oArray = ""

            m_oDatabase.Parameters.Clear()
            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'TN20010719 - start
            If v_nResetStatus = gPMConstants.PMEReturnCode.PMTrue Then
                'reset status to UnQuoted
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", 4, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                'TN20010719 - end
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", v_oRiskDetail(1, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_cnt", v_oRiskDetail(2, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "accumulation_id", v_oRiskDetail(3, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_oRiskDetail(4, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_oRiskDetail(5, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_number", v_oRiskDetail(6, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_requested", v_oRiskDetail(7, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "inception_date", v_oRiskDetail(8, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_oRiskDetail(9, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_not_index_linked", v_oRiskDetail(10, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_accumulated", v_oRiskDetail(11, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_reason_id", v_oRiskDetail(12, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_date", v_oRiskDetail(13, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_description", v_oRiskDetail(14, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "var_data_ref", v_oRiskDetail(15, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_sum_insured", v_oRiskDetail(16, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_annual_premium", v_oRiskDetail(17, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_this_premium", ToSafeDouble(v_oRiskDetail(18, v_nPosNo) * -1), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_ri_at_risk_level", v_oRiskDetail(19, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_auto_reinsured", v_oRiskDetail(20, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_screen_id", v_oRiskDetail(21, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "eml_percentage", v_oRiskDetail(22, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_number", v_oRiskDetail(23, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            ' PW311002
            bPMAddParameter.AddParameterLite(m_oDatabase, "variation_number", v_oRiskDetail(24, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_risk_selected", v_oRiskDetail(25, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            bPMAddParameter.AddParameterLite(m_oDatabase, "coverage", v_oRiskDetail(26, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "insured_item", v_oRiskDetail(27, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "extensions", v_oRiskDetail(28, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_this_year", v_oRiskDetail(31, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_mandatory_risk", v_oRiskDetail(36, v_nPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACAddVoidRiskSQL, sSQLName:=ACAddVoidRiskName, bStoredProcedure:=ACAddVoidRiskStored, oRecordset:=dtResult)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRisk ", gPMConstants.PMELogLevel.PMLogError)
            End If

            nRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

            'RWH(20/11/2000) Add link record into Insurance_file_risk_link.
            If (nRiskCnt <> 0) Then

                ' Determine whether or not to create a link and
                ' if so what kind of link
                Select Case v_nCreateLinkType

                    Case 0 ' standard - original and renewed risk cnt not populated
                        m_nReturn = m_oRiskData.AddRiskLink(CInt(v_nNewInsuranceFileCnt), CInt(nRiskCnt), "C", 0, 0)

                    Case 1 ' populate original_risk_cnt
                        m_nReturn = m_oRiskData.AddRiskLink(CInt(v_nNewInsuranceFileCnt), CInt(nRiskCnt), "C", CInt(v_nOldRiskCnt), 0)

                    Case 2 ' populate renewed_risk_cnt
                        m_nReturn = m_oRiskData.AddRiskLink(CInt(v_nNewInsuranceFileCnt), CInt(nRiskCnt), "C", 0, CInt(v_nOldRiskCnt))

                    Case Else
                        ' do nothing
                End Select

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed CopyRisk ", gPMConstants.PMELogLevel.PMLogError)
                End If

                r_nRiskCnt = nRiskCnt

                m_nReturn = CopyRatingSectionForVoidTransaction(v_lOldRiskCnt:=nOldRiskCnt, v_lNewRiskCnt:=nRiskCnt)

                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed CopyRisk ", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="CopyRisk",
                r_lFunctionReturn:=CopyRisk, excep:=excep)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
        Return m_nReturn
    End Function

    Public Function CopyRatingSectionForVoidTransaction(ByVal v_lOldRiskCnt As Integer, ByVal v_lNewRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="OldRiskCnt", vValue:=CStr(v_lOldRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=CStr(v_lNewRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRatingSectionForVoidSQL, sSQLName:=ACCopyRatingSectionForVoidName, bStoredProcedure:=ACCopyRatingSectionForVoidStored)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRatingSection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRatingSection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    Public Function CopyRiskTaxFee(ByVal v_lSourceRiskCnt As Long, ByVal v_lSourceInsuranceFileCnt As Long, ByVal v_lNewRiskCnt As Long, ByVal v_lNewInsuranceFileCnt As Long) As Integer
        Const kMethodName As String = "CopyRiskTax"

        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue

        Try

            AddParameterLite(m_oDatabase, "risk_cnt", v_lNewRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger, True)
            AddParameterLite(m_oDatabase, "insurance_file_cnt", v_lNewInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "source_risk_cnt", v_lSourceRiskCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)
            AddParameterLite(m_oDatabase, "source_insurance_file_cnt", v_lSourceInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)

            nReturn = m_oDatabase.SQLAction(sSQL:=ACCopyRiskTaxSQL, sSQLName:=ACCopyRiskTaxName, bStoredProcedure:=ACCopyRiskTaxStored)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "CopyRiskTax Failed", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyRiskTax Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskTax", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function GetRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer

        Dim oDeferredRIArray(,) As Object = Nothing
        Try

            ' Set the params
            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Get the risk status of the risk
            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskStatusSQL, sSQLName:=ACGetRiskStatusName, bStoredProcedure:=ACGetRiskStatusStored, vResultArray:=oDeferredRIArray)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_nReturn
            End If

            If Informations.IsArray(oDeferredRIArray) Then
                r_lRiskStatusID = gPMFunctions.NullToLong(oDeferredRIArray(0, 0))
                r_sRiskStatusCode = gPMFunctions.NullToString(oDeferredRIArray(1, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Public Function CopyPerilAndRating(ByVal nInsuranceFileCnt As Integer, ByVal nOriginalRiskCnt As Integer, ByVal nRiskCnt As Integer, ByVal dProRataRate As Double, Optional ByVal nNew_InsuranceFileCnt As Integer = 0, Optional ByVal nInsuranceFileTypeId As Integer = 0) As Integer

        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            'delete peril 
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_nReturn = m_oDatabase.SQLAction(sSQL:=kDelPerilForDeletedRiskSQL, sSQLName:=kDelPerilForDeletedRiskName, bStoredProcedure:=kDelPerilForDeletedRiskStored)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                Return m_nReturn
            End If

            ''Del Rating Sections
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt", vValue:=nRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_nReturn = m_oDatabase.SQLAction(sSQL:=kDelRatingSectionForDeletedRiskSQL, sSQLName:=kDelRatingSectionForDeletedRiskName, bStoredProcedure:=kDelRatingSectionForDeletedRiskStored)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
                Return m_nReturn
            End If

            CopyRatingSectionForVoidTransaction(nOriginalRiskCnt, nRiskCnt)

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="OldRiskCnt", vValue:=ToSafeInteger(nOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=ToSafeInteger(nRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.SQLAction(sSQL:=kAddSectionAndPerilsSQL, sSQLName:=kAddSectionAndPerilsName, bStoredProcedure:=kAddSectionAndPerilsStored)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

        Catch excep As System.Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyPerilAndRating Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPerilAndRating Failed", excep:=excep)
        Finally
        End Try
        Return m_nReturn
    End Function

    Private Function GetAllocation(ByVal v_iVoidTransactionLogId As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Try
            m_oDatabase.Parameters.Clear()
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            If m_oDatabase.Parameters.Add("reverse_transaction_log_id", ToSafeInteger(v_iVoidTransactionLogId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllocationSQL, sSQLName:=ACGetAllocationName, bStoredProcedure:=ACGetAllocationStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Private Function GetTransDetailID(ByVal v_lAllocationID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByRef r_lAccountID As Integer, ByRef r_lTransDetailID As Integer, ByRef r_vResultArray As Object) As Integer
        Dim nReturn As Integer = 0
        Dim vResultArray As Object = Nothing
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="allocation_id", vValue:=v_lAllocationID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If m_oDatabase.Parameters.Add("insurance_file_cnt", v_lInsuranceFileCnt, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetTransDetailIDSQL, sSQLName:=ACGetTransDetailIDName, bStoredProcedure:=ACGetTransDetailIDStored, vResultArray:=vResultArray)

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
            End If
            r_vResultArray = vResultArray

        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
        Return m_nReturn

    End Function

    Private Function GetRemainingAllocationDetail(ByVal v_lAllocationID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lTransaction_log_id As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim nReturn As Integer = 0

        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_nReturn = m_oDatabase.Parameters.Add(sName:="allocation_id", vValue:=v_lAllocationID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oDatabase.Parameters.Add("insurance_file_cnt", v_lInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oDatabase.Parameters.Add("reverse_transaction_log_id", v_lTransaction_log_id, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' m_nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAllocationDetailSQL, sSQLName:=ACGetAllocatedDetailName, bStoredProcedure:=ACGetAllocatedDetailStored, oRecordset:=dtResult)
            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACGetAllocationDetailSQL, sSQLName:=ACGetAllocationDetailName, bStoredProcedure:=ACGetAllocationDetailStored, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=r_vResultArray, bKeepNulls:=True)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_nReturn = gPMConstants.PMEReturnCode.PMFalse
            End If
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
        Return m_nReturn
    End Function

    Private Function CheckInstalmentBusiness(ByVal v_linsurance_file_cnt As Integer, ByRef r_iContinue As Integer) As Integer
        Dim vResultArray As Object
        Try
            r_iContinue = 1
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(v_linsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckInstalmentIsCollectedSQL, sSQLName:=ACCheckInstalmentIsCollectedName, bStoredProcedure:=ACCheckInstalmentIsCollectedStored, vResultArray:=vResultArray)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                r_iContinue = 0
            End If

            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckInstalmentBusiness Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckInstalmentBusiness", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function ReverseAllocation(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer) As Integer
        Dim v_TransdetailArray As Object = Nothing
        Dim v_AllocationArray As Object = Nothing
        Dim sMessage As String = ""
        Dim iAllocationid As Integer
        Dim oRemainingAllocationDetailArray As Object = Nothing
        Dim vMatchTrans As Object() = Nothing
        Dim lAccountID As Integer
        Dim sTransdetailKey As String = ""
        Dim lRevTransdetailID As Integer
        Dim iMatchCounter As Integer = 0
        Dim iVoidTransactionLogId As Integer = 0
        Dim sParentDocumentRef As String = ""
        ' log in transaction log 

        m_nReturn = GetTransDetailID(iAllocationid, v_lInsuranceFileCnt, lAccountID, lRevTransdetailID, v_TransdetailArray)
        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If Informations.IsArray(v_TransdetailArray) Then
            For iCount As Integer = 0 To v_TransdetailArray.GetUpperBound(1)
                If iCount = 0 Then
                    m_nReturn = AddReversalLog(v_lInsuranceFileCnt, iVoidTransactionLogId)
                    If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                lRevTransdetailID = ToSafeInteger(v_TransdetailArray(0, iCount))
                m_nReturn = m_oAllocationPost.ReverseAllocation(v_lTransDetailID:=CInt(lRevTransdetailID), r_lAccountID:=CInt(lAccountID), iVoidTransactionLogID:=CInt(iVoidTransactionLogId))
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    ' Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Next
        End If

        m_nReturn = GetAllocation(iVoidTransactionLogId, v_AllocationArray)
        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        If Informations.IsArray(v_AllocationArray) Then
            For iCount As Integer = 0 To v_AllocationArray.GetUpperBound(1)
                iAllocationid = v_AllocationArray(0, iCount)
                lAccountID = v_AllocationArray(1, iCount)
                m_nReturn = GetRemainingAllocationDetail(iAllocationid, v_lInsuranceFileCnt, iVoidTransactionLogId, oRemainingAllocationDetailArray)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(oRemainingAllocationDetailArray) And lAccountID > 0 Then
                    ReDim vMatchTrans(oRemainingAllocationDetailArray.GetUpperBound(1) - 1)
                    For iCnt As Integer = 0 To oRemainingAllocationDetailArray.GetUpperBound(1)
                        If ToSafeInteger(oRemainingAllocationDetailArray(4, iCnt)) = 1 Then
                            sTransdetailKey = ToSafeString(oRemainingAllocationDetailArray(3, iCnt))
                            sParentDocumentRef = ToSafeString(oRemainingAllocationDetailArray(1, iCnt))
                        Else
                            vMatchTrans(iMatchCounter) = ToSafeString(oRemainingAllocationDetailArray(3, iCnt))
                            iMatchCounter += 1
                        End If
                    Next
                    If Informations.IsArray(vMatchTrans) Then
                        m_nReturn = UpdateAllocation(vMatchTrans, lAccountID, sTransdetailKey)
                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        m_nReturn = UpdateLogDetail(iVoidTransactionLogId, iAllocationid,sParentDocumentRef)
                        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLogDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLogDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        End If
                    End If
                End If
            Next
        End If
        m_nReturn = Allocation(v_lInsuranceFileCnt, v_lNewInsuranceFileCnt)
        If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetAllocationDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetAllocationDetail", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return m_nReturn
    End Function

    Public Function UpdateLogDetail(ByVal v_iVoidTransactionLogId As Integer, ByVal v_iAllocationid As Integer, ByVal v_sParentDocRef As String) As Integer
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="reverse_transaction_log_id", vValue:=ToSafeInteger(v_iVoidTransactionLogId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="allocationid", vValue:=v_iAllocationid, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If String.IsNullOrEmpty(v_sParentDocRef) Then
                m_nReturn = m_oDatabase.Parameters.Add(sName:="parentDocRef", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            Else
                m_nReturn = m_oDatabase.Parameters.Add(sName:="parentDocRef", vValue:=v_sParentDocRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            End If

            m_nReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateVoidReversalDetailSQL, sSQLName:=ACCUpdateVoidReversalDetailName, bStoredProcedure:=ACCUpdateVoidReversalDetailStored)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLogDetail Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLogDetail", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function AddReversalLog(ByVal v_linsurance_file_cnt As Integer, ByRef iVoidTransactionLogId As Integer) As Integer
        Dim vResultArray As Object
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(v_linsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=IIf(ToSafeInteger(m_iUserID) > 0, ToSafeInteger(m_iUserID), 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACInsertVoidReversalSQL, sSQLName:=ACInsertVoidReversalName, bStoredProcedure:=ACInsertVoidReversalStored, vResultArray:=vResultArray)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                iVoidTransactionLogId = ToSafeInteger(vResultArray(0, 0))
            End If

            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddReversalLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddReversalLog", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function UpdateAllocation(ByVal oAllocation As Object, ByVal iAccountKey As Integer, ByVal sTransDetailKey As String) As Integer
        m_nReturn = gPMConstants.PMEReturnCode.PMTrue
        Try
            Dim vKeys(0 To 1, 7) As Object
            vKeys(0, 0) = "account_id"
            vKeys(1, 0) = iAccountKey

            'TransdetailKey
            vKeys(0, 1) = "trans_detail_id"
            vKeys(1, 1) = sTransDetailKey

            ' AllocationTransdetailKeys
            vKeys(0, 2) = "trans_detail_ids"
            vKeys(1, 2) = oAllocation

            vKeys(0, 3) = "writeoff_reason_id"
            vKeys(1, 3) = 0

            vKeys(0, 4) = "writeoff_amount"
            vKeys(1, 4) = 0

            vKeys(0, 5) = "currency_difference"
            vKeys(1, 5) = 0

            vKeys(0, 6) = "cashlistitem_id"
            vKeys(1, 6) = 0

            'TransdetailExKey
            vKeys(0, 7) = "trans_detail_ex_id"
            vKeys(1, 7) = 0

            m_nReturn = m_oAllocationManual.SetProcessModes(vTask:=2)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ACTAllocationManual.Business.SetProcessModes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oAllocationManual.SetKeys(vKeyArray:=DirectCast(vKeys, Object(,)))
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ACTAllocationManual.Business.SetKeys Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_nReturn = m_oAllocationManual.Start()
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ACTAllocationManual.Business.Start Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAllocation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAllocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
        Return m_nReturn
    End Function

    Public Function Allocation(ByVal v_lCancelledInsuranceFileCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim vKeys As Object

        Try
            Dim vResultArray(,) As Object = Nothing
            result = gPMConstants.PMEReturnCode.PMTrue


            With m_oDatabase
                .Parameters.Clear()

                ' Add Cancelled Insurance File Count Parameter
                m_nReturn = .Parameters.Add(sName:="insurance_file_cnt_1", vValue:=gPMFunctions.ToSafeInteger(v_lCancelledInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_nReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocation Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllocationAgainstPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Add Insurance File Count Parameter
                m_nReturn = .Parameters.Add(sName:="insurance_file_cnt_2", vValue:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_nReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocation Failed to add Parameter to Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllocationAgainstPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                ' Execute Stored Procedure
                m_nReturn = .SQLSelect(sSQL:="spu_get_trans_detail_for_insurance_file", sSQLName:="TransDetailSP", bStoredProcedure:=True, vResultArray:=vResultArray)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_nReturn
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Allocation Failed to process the Stored Proc", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckAllocationAgainstPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End With

            ' Check result array
            If Not Informations.IsArray(vResultArray) Then
                ' Returning true when no entry found as it a valid case for zero value MTA 
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            Dim n As Integer = vResultArray.GetLength(1)

            ' Loop through each account returned
            For i As Integer = 0 To n - 1
                Dim accountId As String = gPMFunctions.ToSafeString(vResultArray(0, i))

                ' Call SP to get transaction details for this account
                Dim vTransDetails(,) As Object = Nothing
                m_oDatabase.Parameters.Clear()
                m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt_1", vValue:=gPMFunctions.ToSafeString(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then Return m_nReturn

                m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt_2", vValue:=gPMFunctions.ToSafeString(v_lCancelledInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then Return m_nReturn

                m_nReturn = m_oDatabase.Parameters.Add("account_id", accountId, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then Return m_nReturn

                m_nReturn = m_oDatabase.SQLSelect("spu_get_trans_detail_for_account_id", "TransDetailSP", True, vResultArray:=vTransDetails)
                If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get transaction details", vApp:=ACApp, vClass:=ACClass, vMethod:="Allocation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                End If


                ' Process transaction details in pairs (positive + negative of same amount)
                If Not Informations.IsArray(vTransDetails) Then
                    Continue For
                End If

                Dim transCount As Integer = vTransDetails.GetLength(1) ' <-- FIXED
                Dim processed(transCount - 1) As Boolean

                For j As Integer = 0 To transCount - 1
                    If processed(j) Then Continue For

                    Dim amount1 As Decimal = CDec(vTransDetails(0, j))
                    Dim transID1 As String = gPMFunctions.ToSafeString(vTransDetails(2, j))


                    For k As Integer = j + 1 To transCount - 1
                        If processed(k) Then Continue For

                        Dim amount2 As Decimal = CDec(vTransDetails(0, k))

                        ' Matching condition: same magnitude, opposite sign
                        If Math.Abs(amount1) = Math.Abs(amount2) Then

                            Dim transID2 As String = gPMFunctions.ToSafeString(vTransDetails(2, k))


                            ' Initialise vKeys BEFORE using it
                            ReDim vKeys(1, 2)

                            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 1) = PMNavKeyConst.ACTKeyNameTransDetailID
                            vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyName, 2) = PMNavKeyConst.ACTKeyNameTransDetailIDs
                            Dim vMatchTrans(0) As String
                            ' Assign key values correctly
                            If amount1 > 0 Then

                                vMatchTrans(0) = transID1 & "|" & amount1
                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = transID2 & "|" & amount2
                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans
                            Else

                                vMatchTrans(0) = transID2 & "|" & amount2
                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 1) = transID1 & "|" & amount1
                                vKeys(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2) = vMatchTrans
                            End If

                            ' Set modes + keys
                            m_nReturn = m_oAllocationManual.SetProcessModes(gPMConstants.PMEComponentAction.PMEdit)
                            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to start Allocation Manual.",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Allocation",
                                   vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            End If

                            m_nReturn = m_oAllocationManual.SetKeys(vKeys)
                            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to start Allocation Manual.",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Allocation",
                                   vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            End If

                            ' Start allocation
                            m_nReturn = m_oAllocationManual.Start()
                            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                bPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError,
                                   sMsg:="Failed to start Allocation Manual.",
                                   vApp:=ACApp, vClass:=ACClass, vMethod:="Allocation",
                                   vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                            End If

                            processed(j) = True
                            processed(k) = True
                            Exit For
                        End If
                    Next
                Next
            Next

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch ex As Exception
            Return gPMConstants.PMEReturnCode.PMError
        End Try
    End Function

    Public Function CreatePolicy(ByVal v_lOldinsurance_file_cnt As Integer, ByRef v_lNewinsurance_file_cnt As Integer) As Integer
        Dim vResultArray As Object = Nothing
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=ToSafeInteger(v_lOldinsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=IIf(ToSafeInteger(m_iUserID) > 0, ToSafeInteger(m_iUserID), 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=ToSafeInteger(v_lNewinsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACCreateVoidPolicySQL, sSQLName:=ACCreateVoidPolicyName, bStoredProcedure:=ACCreateVoidPolicyStored, vResultArray:=vResultArray)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            v_lNewinsurance_file_cnt = ToSafeInteger(m_oDatabase.Parameters.Item("new_insurance_file_cnt").Value, 0)
            'If Informations.IsArray(vResultArray) Then
            '    v_lNewinsurance_file_cnt = ToSafeInteger(vResultArray(0, 0))
            'End If

            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function


    Public Function UpdatePolicyStatus(ByVal v_linsurance_file_cnt As Integer, ByVal v_sTransType As String) As Integer
        Dim vResultArray As Object = Nothing
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(v_linsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=IIf(ToSafeInteger(m_iUserID) > 0, ToSafeInteger(m_iUserID), 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="trans_type", vValue:=ToSafeString(v_sTransType), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACUpdPolicyStatusSQL, sSQLName:=ACUpdPolicyStatusName, bStoredProcedure:=ACUpdPolicyStatusStored, vResultArray:=vResultArray)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function ResetPolicyStatus(ByVal v_linsurance_file_cnt As Integer, ByVal v_iInsuranceFileTypeId As Integer) As Integer
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_type_id", vValue:=ToSafeInteger(v_iInsuranceFileTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(v_linsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            m_nReturn = m_oDatabase.SQLAction(sSQL:=ACResetPolicyStatusSQL, sSQLName:=ACResetPolicyStatusName, bStoredProcedure:=ACResetPolicyStatusStored)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ResetPolicyStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ResetPolicyStatus", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

    Public Function CheckPolicyForVoid(ByVal v_linsurance_file_cnt As Integer, ByRef v_iContinue As Integer) As Integer
        Dim vResultArray As Object = Nothing
        Try
            m_nReturn = gPMConstants.PMEReturnCode.PMTrue
            m_oDatabase.Parameters.Clear()
            m_nReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=IIf(ToSafeInteger(m_iUserID) > 0, ToSafeInteger(m_iUserID), 1), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            m_nReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=ToSafeInteger(v_linsurance_file_cnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            m_nReturn = m_oDatabase.SQLSelect(sSQL:=ACCheckPolicyForVoidSQL, sSQLName:=ACCheckPolicyForVoidName, bStoredProcedure:=ACCheckPolicyForVoidStored, vResultArray:=vResultArray)
            If m_nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(vResultArray) Then
                v_iContinue = ToSafeInteger(vResultArray(0, 0))
            End If

            Return m_nReturn
        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckPolicyForVoid Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckPolicyForVoid", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return gPMConstants.PMEReturnCode.PMFalse
        End Try
    End Function

End Class


