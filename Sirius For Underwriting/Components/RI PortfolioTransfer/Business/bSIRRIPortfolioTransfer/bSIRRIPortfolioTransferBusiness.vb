Option Strict Off
Option Explicit On
Imports System.Data
Imports SSP.Shared

'developer guide no. 129
<System.Runtime.InteropServices.ProgId("Business_NET.Business")>
Public NotInheritable Class Business
    Implements IDisposable

    ' ***************************************************************** '
    ' Class Name: Business
    '
    ' Date: 07/07/2004
    '
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

    ' Need for ProcessSinglePolicy()
    Private m_oRenSelection As bSIRRenSelection.Business
    Private m_oInsuranceFile As bSIRInsuranceFile.Services
    Private m_oReinsurance As Object
    Private m_oRiskData As bSIRRiskData.Business
    Private m_oPerilAllocation As bSirPerilAllocation.Business
    Private m_oControlTrans As bControlTrans.Automated

    Private m_oTax As bSIRRITax.Business

    Private m_vValue As Object
    Private m_bIsRI2007Enabled As Boolean
    Private m_vClaimsArray(,) As Object = Nothing
    Private m_oReinsuranceClaim As Object
    Private m_bSkipPostings As Boolean

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

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
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


            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lCurrentRecord = 0
            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now

            ' Create relevant business objects to do ProcessSinglePolicy()
            m_lReturn = CType(CreateBusinessObjects(), gPMConstants.PMEReturnCode)
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
    ' Description: Entry point for any termination code for this object.
    ' ***************************************************************** '
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


    ' ***************************************************************** '
    ' Name: SetProcessModes (Standard Method)
    ' Description: Set the optional process modes.
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
        ' Log Error Message
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   GetPoliciesPortfolioTransfer
    ' AUTHOR:      Alix Bergeret
    ' DATE:        07/07/2004
    ' DESCRIPTION: -
    ' ---------------------------------------------------------------------------
    Public Function GetPoliciesPortfolioTransfer(ByVal v_lProductID As Integer, ByVal v_dtTransferDate As Date, ByVal v_nBranchID As Integer, ByRef r_vPolicyArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try


            m_oDatabase.Parameters.Clear()

            ' Add parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_lProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            m_lReturn = m_oDatabase.Parameters.Add(sName:="transfer_date", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=v_nBranchID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If
            ' Get policies

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetPoliciesPortfolioTransferSQL, sSQLName:=ACGetPoliciesPortfolioTransferName, bStoredProcedure:=ACGetPoliciesPortfolioTransferStored, vResultArray:=r_vPolicyArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesPortfolioTransfer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesPortfolioTransfer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE:   GetRiskStatus
    ' AUTHOR:      AMB
    ' DATE:        24-Sep-03
    ' DESCRIPTION: Get the 'risk_status' of a risk (doh)
    ' HISTORY:     Created for 1.8.6 Deferred RI development
    ' ---------------------------------------------------------------------------
    Public Function GetRiskStatus(ByVal v_lRiskCnt As Integer, ByRef r_lRiskStatusID As Integer, ByRef r_sRiskStatusCode As String) As Integer

        Dim result As Integer = 0
        Dim vDeferredRIArray(,) As Object = Nothing

        Try

            ' Set the params

            m_oDatabase.Parameters.Clear()


            m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get the risk status of the risk

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetRiskStatusSQL, sSQLName:=ACGetRiskStatusName, bStoredProcedure:=ACGetRiskStatusStored, vResultArray:=vDeferredRIArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            If Informations.IsArray(vDeferredRIArray) Then
                r_lRiskStatusID = gPMFunctions.NullToLong(vDeferredRIArray(0, 0))
                r_sRiskStatusCode = gPMFunctions.NullToString(vDeferredRIArray(1, 0))
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetRiskStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRiskStatus", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function

    ''' <summary>
    ''' ProcessSinglePolicy
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="dtTransferDate"></param>
    ''' <param name="dtStartDate"></param>
    ''' <param name="dtEndDate"></param>
    ''' <param name="dtInceptionDate"></param>
    ''' <param name="nProductId"></param>
    ''' <param name="sInsuranceFileType"></param>
    ''' <param name="bSkipPosting"></param>
    ''' <param name="r_sMessage"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessSinglePolicy(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByVal dtStartDate As Date, ByVal dtEndDate As Date, ByVal dtInceptionDate As Date, ByVal nProductId As Integer, ByVal sInsuranceFileType As String, ByVal bSkipPosting As Boolean, ByRef r_sMessage As String) As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim sTransactionType As String = ""

        Dim dProRataRate As Double
        Dim nIsValid As Integer
        Const kMethodName As String = "ProcessSinglePolicy"
        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            sTransactionType = "PT"

            ' Set transaction so we can roll back if something gone wrong
            nReturnValue = m_oDatabase.SQLBeginTrans()
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturnValue
            End If


            m_bSkipPostings = bSkipPosting

            If sInsuranceFileType = "QUOTE" Or sInsuranceFileType = "RENEWAL" Or sInsuranceFileType = "MTAQUOTE" Or sInsuranceFileType = "MTAQTETEMP" Or sInsuranceFileType = "MTAQREINS" Or sInsuranceFileType = "MTAQCAN" Then
                nReturnValue = RecalculateRIQuote(nInsuranceFileCnt, dtTransferDate, 1)
            Else

                nReturnValue = GetProRataRate(nProductId, dtStartDate, dtEndDate, dtTransferDate, dtEndDate, dProRataRate, dtInceptionDate)
                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed GetProRataRate "
                    RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                nReturnValue = RecalculateRI(nInsuranceFileCnt, dtTransferDate, dProRataRate, 1, nIsValid, bSkipPosting)

                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Or nIsValid <> 1 Then
                    r_sMessage = "Failed to Validate Reinsurance for " & nInsuranceFileCnt
                    RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                ' Process stats (error messages are dealt within CreateAndPostStats())
                If (sInsuranceFileType = "POLICY" Or sInsuranceFileType = "MTA PERM" Or sInsuranceFileType = "MTA TEMP") And bSkipPosting = False Then
                    If CreateAndPostStats(nInsuranceFileCnt:=nInsuranceFileCnt, sTransactionType:=sTransactionType, nPTInsuranceFileCnt:=nInsuranceFileCnt, bReversePT:=False, dtTransferDate:=dtTransferDate, r_sMessage:=r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to CreateAndPostStats for " & nInsuranceFileCnt
                        RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

            End If


            ' Save all transactions to database

            m_lReturn = m_oDatabase.SQLCommitTrans

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to commit SQLTransaction"
                RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception

            ' Roll back all transactions as one of the step has failed

            m_lReturn = m_oDatabase.SQLRollbackTrans
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to Rollback SQLTransaction"
            End If
            nReturnValue = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(r_sMessage = "", "Failed ProcessSinglePolicy()", r_sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSinglePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

            Return nReturnValue

        End Try

        Return nReturnValue

    End Function

    '***********************************************************************************************************************
    ' Name :    CreateBusinessObjects
    ' Desc :    Create required objects for ProcessSinglePolicy
    ' Author :  Alix Bergeret - 08/07/2004
    '***********************************************************************************************************************
    Private Function CreateBusinessObjects() As Integer

        Dim result As Integer = 0
        Dim sMessage As String = "" = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
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

            ' E007 Portfolio Transfer - Kuljeet Kaur
            m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:="",
                            v_sPassword:="", v_iUserID:=0,
                            v_iMainSourceID:=0, v_iLanguageID:=0,
                            v_iCurrencyID:=0, v_iLogLevel:=0,
                            v_sCallingAppName:="",
                            v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRI2007,
                            v_vBranch:=m_iSourceID,
                            r_vUnderwriting:=m_vValue)
            If m_vValue = "1" Then
                m_bIsRI2007Enabled = True
            Else
                m_bIsRI2007Enabled = False
            End If

            If m_bIsRI2007Enabled Then
                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsuranceRI2007.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            Else
                m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsurance, v_sClassName:="bSIRReinsurance.Form", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRReinsurance.Form"
                Throw New Exception()
            End If

            m_oTax = New bSIRRITax.Business
            If m_oTax.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRITax.business"
                Throw New Exception()
            End If

             m_oRiskData = New bSIRRiskData.Business
            If m_oRiskData.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase) <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRRiskData.Business"
                Throw New Exception()
            End If


            m_oPerilAllocation = New bSirPerilAllocation.Business
            m_lReturn = m_oPerilAllocation.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bSIRPerilAllocation.Business"
                Throw New Exception()
            End If


            m_oControlTrans = New bControlTrans.Automated
            m_lReturn = m_oControlTrans.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceId:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to create an instance of bControlTrans.Automated"
                Throw New Exception()
            End If

            m_lReturn = CType(gPMComponentServices.CreateBusinessObject(r_oObject:=m_oReinsuranceClaim,
                                                        v_sClassName:=If(m_bIsRI2007Enabled,
    "bCLMReinsuranceRI2007.Form", "bCLMReinsurance.Form"),
                                                        v_sCallingAppName:=ACApp,
                                                        v_sUsername:=m_sUsername,
                                                        v_sPassword:=m_sPassword,
                                                        v_iUserID:=m_iUserID,
                                                        v_iSourceID:=m_iSourceID,
                                                        v_iLanguageID:=m_iLanguageID,
                                                        v_iCurrencyID:=m_iCurrencyID,
                                                        v_iLogLevel:=m_iLogLevel,
                                                        v_oDatabase:=m_oDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_bIsRI2007Enabled = True Then
                    sMessage = "Failed to create an instance of bCLMReinsuranceRI2007.Form"
                Else
                    sMessage = "Failed to create an instance of bCLMReinsurance.Form"
                End If
                Throw New Exception()
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed CreateBusinessObjects", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateBusinessObjects", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            CloseBusinessObjects()

            Return result
        End Try
    End Function

    '***********************************************************************************************************************
    ' Name :    CloseBusinessObjects
    ' Desc :    Close objects open by CreateBusinessObjects()
    ' Author :  Alix Bergeret - 08/07/2004
    '***********************************************************************************************************************
    Private Sub CloseBusinessObjects()


        Try

            If Not (m_oRenSelection Is Nothing) Then

                m_oRenSelection.Dispose()
                m_oRenSelection = Nothing
            End If

            If Not (m_oInsuranceFile Is Nothing) Then

                m_oInsuranceFile.Dispose()
                m_oInsuranceFile = Nothing
            End If

            If Not (m_oReinsurance Is Nothing) Then

                m_oReinsurance.Dispose()
                m_oReinsurance = Nothing
            End If

            If Not (m_oRiskData Is Nothing) Then

                m_oRiskData.Dispose()
                m_oRiskData = Nothing
            End If

            If Not (m_oPerilAllocation Is Nothing) Then

                m_oPerilAllocation.Dispose()
                m_oPerilAllocation = Nothing
            End If

            If Not (m_oControlTrans Is Nothing) Then

                m_oControlTrans.Dispose()
                m_oControlTrans = Nothing
            End If
            If (Not (m_oReinsuranceClaim Is Nothing)) Then
                m_oReinsuranceClaim.Dispose()
                m_oReinsuranceClaim = Nothing
            End If


        Catch exc As System.Exception
            'NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try


    End Sub

    '***********************************************************************************************************************
    ' Name :    CopyAllRisk
    ' Desc :    Copy related risks details to new policy version
    ' Author :  Alix Bergeret - 08/07/2004
    '************************ Note on Risk **************************
    'if risk is marked for portfolio transfer then we need to
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
    'if commit policy flag is not set then rollback policy (will be done all calling function)
    '***********************************************************************************************************************
    Public Function CopyAllRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtPolicyStartDate As Date, ByVal v_sTransactionType As String, ByRef r_sMessage As String, ByVal v_dtTransferDate As Date, Optional v_bIgnoreError As Boolean = False) As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim vRiskArray As Object = Nothing
        Dim vGISPolicyLinkArray As Object = Nothing
        Dim lRiskCnt, lNewRiskCnt As Integer
        Dim sGISDataModelCode As String = ""
        'Dim lGISPolicyLinkID, lNewGisPolicyLinkID, lPolicyBinderID, lNewPolicyBinderID As Integer
        'Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lValidRIBand As Object
        Dim lReinsBand As Object=0 ' Not used - function parameter required
        ' Dim vRiskTax As Object
        Dim sDescription As String = ""
        Dim lRiskStatusID As Integer
        Dim sRiskStatusCode As String = ""
        Dim lCommitPolicy As gPMConstants.PMEReturnCode ' Set to true when one of the deferred risk is now on live model and everything is ok
        Dim bMarkedForTransfer As Boolean
        Dim lDeferredRIBandNewRisk As Integer ' Number of bands which are on deferred RI model
        Dim lTransactionStarted As gPMConstants.PMEReturnCode ' Set togPMConstants.PMEReturnCode.PMTrue when m_oDatabase.SQLBeginTrans() is called
        Dim sMessage As String = ""
        Dim lClaimsCount As Long
        Dim lInsuranceFileType As Long
        Dim lPreviousRiskCnt As Long
        Dim dProRataRate As Double
        Const ACFieldPosRiskID As Integer = 0
       
        Dim ex As New Exception
        Try

            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            r_sMessage = ""
            lValidRIBand = gPMConstants.PMEReturnCode.PMFalse
            lCommitPolicy = gPMConstants.PMEReturnCode.PMFalse
            lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

            ' Get risk associated with this policy

            If m_oRiskData.GetRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to get associated risks for this policy " & v_lInsuranceFileCnt
                Throw ex
            End If

            ' Check if we have any risks
            If Not Informations.IsArray(vRiskArray) Then
                Return lReturnValue
            End If

            ' Make sure we have the right transaction type

            If m_oRiskData.SetProcessModes(vTransactionType:=CStr(v_sTransactionType)) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode to RiskData object (bSIRRiskData.business)"
                Throw ex
            End If

            m_lReturn = GetPolicyType(v_lInsuranceFileCnt, lInsuranceFileType)
            ' Process each risk

            For lCount As Integer = 0 To vRiskArray.GetUpperBound(1)

                'If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                '    r_sMessage = "Failed SQLBeginTrans()"
                '    Throw ex
                'End If

                lTransactionStarted = gPMConstants.PMEReturnCode.PMTrue

                lRiskCnt = gPMFunctions.NullToLong(vRiskArray(ACFieldPosRiskID, lCount))

                ' Is Risk Marked For Portfolio Transfer?
                If IsRiskMarkedForPortfolioTransfer(v_lRiskCnt:=lRiskCnt, r_bResult:=bMarkedForTransfer, v_dtTransferDate:=CDate(v_dtTransferDate)) <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to check if risk is marked for portfolio transfer (Policy: " & v_lInsuranceFileCnt & " Risk: " & CStr(lRiskCnt) & ")"
                    Throw ex
                End If

                ' No need to copy risk, simply add a link in insurance_file_risk_link table
                If Not bMarkedForTransfer Then

                    ' Add new risk link

                    If m_oRiskData.AddRiskLink(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_sStatusFlag:="U") <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to create new insurance file risk link (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lRiskCnt) & ")"
                        Throw ex
                    End If

                    ' Move claims to new version of policy with same risks
                    If MoveClaimToNewRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lNewRiskCnt:=lRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to move claims to new version of policy  (old Policy: " & v_lInsuranceFileCnt & " old Risk: " & CStr(lRiskCnt) &
                                     " new policy: " & CStr(v_lNewInsuranceFileCnt) & " new risk: " & CStr(lNewRiskCnt) & ")"
                        Throw ex
                    End If

                    ' E007 Changes : Manipulate array
                    If Informations.IsArray(m_vClaimsArray) Then
                        For lClaimsCount = 0 To DirectCast(m_vClaimsArray, Object(,)).GetUpperBound(1)
                            If m_vClaimsArray(2, lClaimsCount) = lRiskCnt Then
                                m_vClaimsArray(3, lClaimsCount) = lNewRiskCnt
                            End If
                        Next
                    End If

                    ' Save changes

                    'If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    '    r_sMessage = "Failed SQLCommitTrans()"
                    '    Throw ex
                    'End If
                    lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                    ' Risk DOES need copying and processing
                Else

                    ' Copy risk details and reset status to unquoted
                    If lInsuranceFileType = 2 Then
                        If m_oRiskData.CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt, v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue, v_lCreateLinkType:=1) <> gPMConstants.PMEReturnCode.PMTrue Then

                            r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & CStr(v_lNewInsuranceFileCnt) & " Risk :)" & CStr(lRiskCnt) & ")"
                            Throw ex
                        End If
                    Else
                        m_lReturn = GetPreviousRiskCnt(v_lPreInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                  v_lRiskCnt:=lRiskCnt,
                                                  v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                  r_lPreviousRiskCnt:=lPreviousRiskCnt)

                        If CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                                                v_vRiskDetail:=vRiskArray,
                                                v_lPosNo:=lCount,
                                                r_lRiskCnt:=lNewRiskCnt,
                                                v_lResetStatus:=gPMConstants.PMEReturnCode.PMTrue,
                                                v_lCreateLinkType:=kInsFileRiskLinkTypeORIGINAL,
                                                v_lOldRiskCnt:=If(lPreviousRiskCnt = 0, lRiskCnt, lPreviousRiskCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to copy risk details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy " & v_lNewInsuranceFileCnt & " Risk :)" & lRiskCnt & ")"
                            Throw ex
                        End If
                    End If

                    '************************ copy gis data for this risk *****************************
                    ' Get get policy link for old policy

                    If m_oRiskData.GetGISPolicyLink(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRiskID:=lRiskCnt, r_vResultArray:=vGISPolicyLinkArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                        r_sMessage = "Failed to copy gis details to new policy version (old policy :" & v_lInsuranceFileCnt & " new policy: " & CStr(v_lNewInsuranceFileCnt) & " Risk: " & CStr(lRiskCnt) & ")"
                        Throw ex
                    End If



                    'This is where unedited risks are dealt with - just link to an existing Risk Data set
                    'm_lReturn = m_oRiskData.CopyGisPolicyLink(v_lPolicyLinkID:=vGISPolicyLinkArray(0, 0), v_lOldRiskID:=lRiskCnt, v_lNewRiskID:=lNewRiskCnt, v_lInsuranceFileCnt:=vGISPolicyLinkArray(1, 0))
                    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRiskData.CopyGisPolicyLink Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData")
                    '    r_sMessage = "m_oRiskData.CopyGisPolicyLink Failed"
                    '    RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
                    'End If

                    ' Note : rating section will only work when original risk on a policy is of type (2,5) and status of policy is (3,4,5,6)
                    ' Do peril allocation for new policy version
                    With m_oPerilAllocation

                        .InsuranceFileCnt = v_lNewInsuranceFileCnt

                        .InsuranceFolderCnt = v_lInsuranceFolderCnt

                        .RiskID = lNewRiskCnt

                        .TransactionType = v_sTransactionType
                    End With

                    ' Populate rating sections

                    If m_oPerilAllocation.PopulateRatingSections() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw ex
                    End If

                    dProRataRate = m_oPerilAllocation.ProRataRate

                    If CopyRatings(v_lInsuranceFileCnt, lRiskCnt, lNewRiskCnt, dProRataRate) <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to populate rating sections for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")"
                        Throw ex
                    End If



                    ' Update risk with premium and suminsured

                    If m_oPerilAllocation.UpdateRisk() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to update risk's premium and suminsured for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw ex
                    End If

                    '**************** Note on reinsurance *******************
                    ' Reinsurance will work out which RI model is relevant for this risk
                    ' If it can't find one it will return false

                    'need to tell reinsurance that we are doing Portfolio transfer

                    m_lReturn = m_oReinsurance.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMEdit, vTransactionType:=CStr(v_sTransactionType), vEffectiveDate:=CDate(v_dtTransferDate))

                    ' Get ready to do reinsurance (risk level)

                    m_oReinsurance.InsuranceFileCnt = v_lNewInsuranceFileCnt

                    m_oReinsurance.RiskId = lNewRiskCnt
                    If Not (lInsuranceFileType = 2 Or lPreviousRiskCnt = 0) Then
                        m_oReinsurance.CopyFACRiskCnt = lRiskCnt
                    Else
                        m_oReinsurance.CopyFACRiskCnt = 0
                    End If
                    ' Generate reinsurance for new policy version

                    If m_oReinsurance.CalculateRI() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to generate RI for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw ex
                    End If

                    ' Get reinsurance details (to fix roundings and validate)

                    If m_oReinsurance.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw ex
                    End If

                    ' Do we have valid reinsurance bands ie adds up to 100%

                    If m_oReinsurance.ValidateBands(r_lValid:=lValidRIBand, r_lBand:=lReinsBand) <> gPMConstants.PMEReturnCode.PMTrue Then
                        If v_bIgnoreError = False Then
                            r_sMessage = "Failed to validate RI bands for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & lNewRiskCnt & ")"
                        Else
                            Continue For
                        End If
                    End If

                    ' Save reinsurance details

                    If m_oReinsurance.Update() <> gPMConstants.PMEReturnCode.PMTrue Then
                        r_sMessage = "Failed to reinsurance details for new policy version (Policy: " & v_lNewInsuranceFileCnt & " Risk: " & CStr(lNewRiskCnt) & ")"
                        Throw ex
                    End If

                    ' Do we have valid bands ie adds up to 100%
                    If lValidRIBand = gPMConstants.PMEReturnCode.PMFalse Then
                        ' Change old risk status to quoted so it won't get pick up again

                        If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lRiskCnt, v_lRiskStatusID:=0, v_sRiskStatusCode:="QUOTED") <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to update old risk status to quoted (Risk: " & lRiskCnt & ")"
                            Throw ex
                        End If

                        ' NOTE: after peril allocation risk_status_id = 8 (pending resinsurance) if everything is ok
                        '       reinsurance (business side) shouldn't change this risk status so we are expecting pending reinsurance
                        ' Get risk status for new risk
                        If GetRiskStatus(v_lRiskCnt:=lNewRiskCnt, r_lRiskStatusID:=lRiskStatusID, r_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to get status for new risk (Risk: " & lNewRiskCnt & ")"
                            Throw ex
                        End If

                        If sRiskStatusCode.Trim().ToUpper() = "PENDINGRI" Then 'pending reinsurance
                            ' Somthing went wrong, set risk status to pending RI portfolio transfer
                            sRiskStatusCode = If(lDeferredRIBandNewRisk = 0, "QUOTED", "PENDINGRIP")

                            ' Update risk status

                            If m_oRiskData.UpdateRiskStatus(v_lRiskCnt:=lNewRiskCnt, v_lRiskStatusID:=0, v_sRiskStatusCode:=sRiskStatusCode) <> gPMConstants.PMEReturnCode.PMTrue Then
                                r_sMessage = "Failed to update new risk status to quoted (Risk: " & lNewRiskCnt & ")"
                                Throw ex
                            End If
                        Else
                            r_sMessage = "Peril Allocation Failed or more questions need answering (Risk: " & lNewRiskCnt & ")"
                            Throw ex
                        End If

                        ' Move claims to new risk
                        ' Maintain/payment of claim will sort out reinsurance when this claim is picked up
                        If MoveClaimToNewRisk(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lRiskCnt:=lRiskCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_lNewRiskCnt:=lNewRiskCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                            r_sMessage = "Failed to move claims to new risk  (old Policy: " & v_lInsuranceFileCnt & " old Risk: " & CStr(lRiskCnt) &
                                         " new policy: " & CStr(v_lNewInsuranceFileCnt) & " new risk: " & CStr(lNewRiskCnt) & ")"
                            Throw ex
                        End If

                        ' E007 Changes : Manipulate array
                        If Informations.IsArray(m_vClaimsArray) Then
                            For lClaimsCount = 0 To DirectCast(m_vClaimsArray, Object(,)).GetUpperBound(1)
                                If m_vClaimsArray(2, lClaimsCount) = lRiskCnt Then
                                    m_vClaimsArray(3, lClaimsCount) = lNewRiskCnt
                                End If
                            Next
                        End If

                        ' Save changes to risk

                        'If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                        '    r_sMessage = "Failed to commit risk changes SQLCommitTrans()"
                        '    Throw ex
                        'End If

                        lTransactionStarted = gPMConstants.PMEReturnCode.PMFalse

                        ' Set this flag to stop policy from rolling back
                        lCommitPolicy = gPMConstants.PMEReturnCode.PMTrue

                    Else
                        If v_bIgnoreError = False Then
                            r_sMessage = "Invalid RI Bands ie bands do not add up to 100% - old policy: " & v_lInsuranceFileCnt & " new policy: " & v_lNewInsuranceFileCnt & "RI Portfolio Transfer"
                            Throw ex
                        Else
                            Continue For
                        End If
                    End If
                End If
            Next lCount

        Catch

            ' Rollback risk changes
            If lTransactionStarted = gPMConstants.PMEReturnCode.PMTrue Then

                'm_lReturn = m_oDatabase.SQLRollbackTrans
            End If

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse

            ' Let calling function log this message
            If r_sMessage = "" Then
                r_sMessage = Informations.Err().Description
            End If

            r_sMessage = r_sMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Failed in CopyAllRisk()"

        End Try

        Return lReturnValue
    End Function
    '***********************************************************************************************************************
    ' Name :    IsRiskMarkedForPortfolioTransfer
    ' Desc :    Checks if risk is attached to a RI model marked for portfolio transfer
    ' Author :  Alix Bergeret - 08/07/2004
    '***********************************************************************************************************************
    Private Function IsRiskMarkedForPortfolioTransfer(ByVal v_lRiskCnt As Integer, ByVal v_dtTransferDate As Date, ByRef r_bResult As Boolean) As Integer

        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = "" = ""
        Dim vResultArray(,) As Object = Nothing


        lReturnValue = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add parameters

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="transfer_date", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Get data

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACIsRiskMarkedForPortfolioTransferSQL, sSQLName:=ACIsRiskMarkedForPortfolioTransferName, bStoredProcedure:=ACIsRiskMarkedForPortfolioTransferStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If

        ' Return result
        r_bResult = False
        If Informations.IsArray(vResultArray) Then

            If CStr(vResultArray(0, 0)) = "1" Then
                r_bResult = True
            End If
        End If

        Return lReturnValue

    End Function

    '***********************************************************************************************************************
    ' Name :    MoveClaimToNewRisk
    ' Desc :    Move all associate claims to new risk
    ' Author :  Alix Bergeret - 08/07/2004
    '***********************************************************************************************************************
    Private Function MoveClaimToNewRisk(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lNewRiskCnt As Integer, Optional ByVal v_lclaimCnt As Long = 0) As Integer

        Dim sMessage As String = "" = ""
        Dim lReturnValue As gPMConstants.PMEReturnCode

        lReturnValue = gPMConstants.PMEReturnCode.PMTrue
        sMessage = ""


        m_oDatabase.Parameters.Clear()


        m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=CInt(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add insurance file cnt param"
            Return lReturnValue
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="RiskCnt", vValue:=v_lRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add risk cnt param"
            Return lReturnValue
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsuranceFileCnt", vValue:=CInt(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add new insurance file cnt param"
            Return lReturnValue
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewRiskCnt", vValue:=v_lNewRiskCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed to add new risk cnt param"
            Return lReturnValue
        End If
        ' E007 Changes
        If v_lclaimCnt > 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="ClaimCnt", vValue:=v_lclaimCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add new risk cnt param"
                Return lReturnValue
            End If
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACMoveClaimToNewRiskSQL, sSQLName:=ACMoveClaimToNewRiskName, bStoredProcedure:=ACMoveClaimToNewRiskStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "Failed SQLAction - move claims to new risk"
            Return lReturnValue
        End If

        Return lReturnValue
    End Function

    '***********************************************************************************************************************
    ' Name :    CreateAndPostStats
    ' Desc :    Create and post stats to orion
    ' Author :  Thinh Nguyen 27/02/2004
    '***********************************************************************************************************************
    Public Function CreateAndPostStats(ByVal nInsuranceFileCnt As Integer, ByVal sTransactionType As String, Optional ByVal nPTInsuranceFileCnt As Integer = 0, Optional ByVal bReversePT As Boolean = False, Optional ByVal dtTransferDate As Date = #1/1/2000#, Optional ByRef r_sMessage As String = "") As Integer

        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim bIsCloned As Boolean
        Const kMethodName As String = "CreateAndPostStats"
        Try
            r_sMessage = ""
            nReturnValue = gPMConstants.PMEReturnCode.PMTrue
            m_lReturn = CheckRIOnClone(nInsuranceFileCnt, bIsCloned)
            If bIsCloned Then
                Return m_lReturn
            End If

            m_oControlTrans.InsuranceFileCnt = nInsuranceFileCnt
            m_oControlTrans.PTInsuranceFileCnt = nPTInsuranceFileCnt
            m_oControlTrans.ReversePT = False
            m_oControlTrans.IsPT = True
            m_oControlTrans.TransferDate = dtTransferDate

            m_lReturn = m_oControlTrans.SetProcessModes(vTask:=gPMConstants.PMEComponentAction.PMAdd, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=sTransactionType, vEffectiveDate:=CDate(m_dtEffectiveDate))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to set process mode for bControlTrans.Automated"
                RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If


            If m_oControlTrans.Start() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create and post stats"
                RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            nReturnValue = gPMConstants.PMEReturnCode.PMFalse

            ' Let calling function log this message
            If r_sMessage = "" Then
                r_sMessage = Informations.Err().Description
            End If

            r_sMessage = r_sMessage & Strings.ChrW(13) & Strings.ChrW(10) & "Failed in CreateAndPostStats()"

        End Try

        Return nReturnValue

    End Function

    ''' <summary>
    ''' Delete the Insurance_File_PT_RI_Usage record
    ''' </summary>
    ''' <param name="v_lInsFilePTRIUsageID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function DeleteInsFilePTRIUsage(ByVal v_lInsFilePTRIUsageID As Integer) As Integer

        Dim lReturnValue As PMEReturnCode
        Dim sMessage As String = ""
        Dim kMethodName As String = "DeleteInsFilePTRIUsage"

        Try

            sMessage = ""
            lReturnValue = PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_PT_RI_usage_id", vValue:=v_lInsFilePTRIUsageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                lReturnValue = PMEReturnCode.PMFalse
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            ' do the call to remove the record from Insurance_File_PT_RI_Usage
            m_lReturn = m_oDatabase.SQLAction(
                    sSQL:=ACDeleteInsFilePTRIUsageSQL,
                    sSQLName:=ACDeleteInsFilePTRIUsageName,
                    bStoredProcedure:=ACDeleteInsFilePTRIUsageStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                lReturnValue = PMEReturnCode.PMFalse
                sMessage = "Failed to delete insurance_file_pt_ri_usage"
                RaiseError(kMethodName, sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            lReturnValue = gPMConstants.PMEReturnCode.PMError
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed DeleteInsFilePTRIUsage() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="DeleteInsFilePTRIUsage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        End Try
        Return lReturnValue
    End Function
    ''' <summary>
    ''' GetPTRIPolicy
    ''' </summary>
    ''' <param name="r_vResultArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPTRIPolicy(ByRef r_vResultArray(,) As Object) As Integer
        Dim nReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""

        Try

            sMessage = ""
            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Get the risk status of the risk
            nReturnValue = m_oDatabase.SQLSelect(
                    sSQL:=ACGetPTRIPolicySQL,
                    sSQLName:=ACGetPTRIPolicyName,
                    bStoredProcedure:=ACGetPTRIPolicyStored,
                    vResultArray:=r_vResultArray)


            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to get policy for portfolio transfer"
                Throw New Exception(sMessage)
            End If

        Catch ex As Exception

            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed GetPTRIPolicy() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="GetPTRIPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        End Try
        Return nReturnValue
    End Function

    Public Function InsertInsFilePTRIUsage(ByVal v_lInsFileCnt As Long, ByVal v_dtTransferDate As Date) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter insurance_file_cnt"
                Throw New Exception(sMessage)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transferDate", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter transferDate"
                Throw New Exception(sMessage)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_PT_RI_usage_id", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                Throw New Exception(sMessage)
            End If

            m_lReturn = m_oDatabase.SQLAction(
                sSQL:=ACInsertInsFilePTRIUsageSQL,
                sSQLName:=ACInsertInsFilePTRIUsageName,
                bStoredProcedure:=ACInsertInsFilePTRIUsageStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                Throw New Exception(sMessage)
            End If

        Catch ex As Exception

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed InsertInsFilePTRIUsage() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="InsertInsFilePTRIUsage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return lReturnValue
        End Try
        Return lReturnValue
    End Function

    ''' <summary>
    ''' SetPTRIStatus
    ''' </summary>
    ''' <param name="v_lInsFilePTRIUsageID"></param>
    ''' <param name="v_lInsFileCnt"></param>
    ''' <param name="v_lPTRIStatusID"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetPTRIStatus(ByVal v_lInsFilePTRIUsageID As Long, ByVal v_lInsFileCnt As Long, ByVal v_lPTRIStatusID As Long) As Long
        Dim nReturnValue As PMEReturnCode
        Dim sMessage As String = ""
        Try

            sMessage = ""
            nReturnValue = PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="ins_file_PT_RI_usage_id", vValue:=v_lInsFilePTRIUsageID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nReturnValue = PMEReturnCode.PMFalse
                sMessage = "Failed to add parameter ins_file_PT_RI_usage_id"
                RaiseError("SetPTRIStatus", sMessage, PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=v_lInsFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nReturnValue = PMEReturnCode.PMFalse
                sMessage = "Failed to add parameter insurance_file_cnt"
                RaiseError("SetPTRIStatus", sMessage, PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="PT_RI_status_type_id", vValue:=v_lPTRIStatusID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                nReturnValue = PMEReturnCode.PMFalse
                sMessage = "Failed to add parameter PT_RI_status_type_id"
                RaiseError("SetPTRIStatus", sMessage, PMELogLevel.PMLogError)
            End If

            ' do the update call
            m_lReturn = m_oDatabase.SQLAction(
                    sSQL:=ACUpdPTRIStatusSQL,
                    sSQLName:=ACUpdPTRIStatusName,
                    bStoredProcedure:=ACUpdPTRIStatusStored)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                nReturnValue = PMEReturnCode.PMFalse
                sMessage = "Failed to SetPTRIStatus"
                RaiseError("SetPTRIStatus", sMessage, PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            nReturnValue = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed SetPTRIStatus() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="SetPTRIStatus", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        End Try
        Return nReturnValue
    End Function


    Public Function SetPolicyStatus(ByVal v_lInsuranceFileCnt As Long,
                                ByVal v_lInsuranceFileStatusID As Long,
                                Optional ByVal v_bStartTransaction As Boolean = True,
                                Optional ByRef r_sFailureMessage As String = "GGGGGRRRRRR") As Long


        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Dim sSQL As String

        Try
            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "UPDATE Insurance_File SET insurance_file_status_id = {InsuranceFileStatusID}" & vbCrLf
            sSQL = sSQL & "WHERE insurance_file_cnt = {InsuranceFileCnt}"

            If v_bStartTransaction Then
                If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to start transaction"
                    End If
                    Throw New Exception(r_sFailureMessage)
                End If
            End If

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileStatusID", vValue:=If(v_lInsuranceFileStatusID = 0, Nothing, v_lInsuranceFileStatusID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileStatusID)"
                End If

                Throw New Exception(r_sFailureMessage)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt", vValue:=If(v_lInsuranceFileStatusID = 0, Nothing, v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to add parameter to PMDAO (InsuranceFileCnt)"
                End If

                Throw New Exception(r_sFailureMessage)
            End If

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="Change Policy Status", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If r_sFailureMessage <> "GGGGGRRRRRR" Then
                    r_sFailureMessage = "Failed to update policy status"
                End If
                Throw New Exception(r_sFailureMessage)
            End If

            If v_bStartTransaction Then
                If m_oDatabase.SQLCommitTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
                    If r_sFailureMessage <> "GGGGGRRRRRR" Then
                        r_sFailureMessage = "Failed to commit transactions"
                    End If

                    Throw New Exception(r_sFailureMessage)
                End If
            End If



        Catch ex As Exception

            If r_sFailureMessage <> "GGGGGRRRRRR" Then
                r_sFailureMessage = "SetPolicyStatus() - Errored"
            End If
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(r_sFailureMessage = "", "Failed InsertInsFilePTRIUsage() ", r_sFailureMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="InsertInsFilePTRIUsage", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)



        Finally

            If v_bStartTransaction Then
                If lReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    m_lReturn = m_oDatabase.SQLRollbackTrans()
                End If
            End If


        End Try
        Return lReturnValue
    End Function

    Public Function RelinkRisk(ByVal v_lOldInsuranceFileCnt As Long, ByVal v_lNewInsuranceFileCnt As Long) As Long

        Dim sSQL As String
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            sSQL = "INSERT INTO insurance_file_risk_link (" & vbCrLf &
                   "insurance_file_cnt," & vbCrLf &
                   "risk_cnt," & vbCrLf &
                   "status_flag, " & vbCrLf &
                   "original_risk_cnt)" & vbCrLf &
                   "SELECT " & v_lNewInsuranceFileCnt & "," & vbCrLf &
                   "risk_cnt," & vbCrLf &
                   "'U'," & vbCrLf &
                   "null" & vbCrLf &
                   "FROM insurance_file_risk_link" & vbCrLf &
                   "WHERE insurance_file_cnt = " & v_lOldInsuranceFileCnt & vbCrLf &
                   "AND status_flag <> 'D'" & vbCrLf


            m_oDatabase.Parameters.Clear()

            If m_oDatabase.SQLAction(sSQL:=sSQL,
                                    sSQLName:="Link old risks to new policy version",
                                    bStoredProcedure:=False) <> gPMConstants.PMEReturnCode.PMTrue Then

                Throw New Exception
            End If


        Catch ex As Exception

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed RelinkRisk() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="RelinkRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        Finally



        End Try
        Return lReturnValue
    End Function

    Public Function DeletePolicy(ByVal v_lInsuranceFileCnt As Long) As Long
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Try
            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            If m_oDatabase.Parameters.Add(sName:="InsuranceFileCnt",
                                                   vValue:=CInt(v_lInsuranceFileCnt),
                                                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                   iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            DeletePolicy = m_oDatabase.SQLAction(sSQL:=ACDeletePolicySQL,
                                                sSQLName:=ACDeletePolicyName,
                                                bStoredProcedure:=ACDeletePolicyStored)


        Catch ex As Exception

            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed DeletePolicy() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="DeletePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

        End Try
        Return lReturnValue
    End Function

    Public Function CopyPolicyHeader(ByVal v_lInsuranceFileCnt As Long,
                                ByVal v_dTransferDate As Date,
                                ByRef r_lNewInsurancefileCnt As Long,
                                ByRef r_sMessage As String,
                                Optional ByRef r_lInsuranceFolderCnt As Long = 0,
                                Optional ByRef r_dtPolicyStartDate As Date = Nothing,
                                Optional ByVal v_sSetOldInsuranceFileStatus As String = "",
                                Optional ByVal v_sSetNewInsuranceFileStatus As String = "",
                                Optional ByVal v_sTransactionType As String = "") As Long


        Dim sDescription As String = ""         'this is not used. just need to call tax function
        Dim vInsuranceFileTax As Object = Nothing    'this is not used. just need to call tax function
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = ""
        Try

            sMessage = ""
            lReturnValue = gPMConstants.PMEReturnCode.PMTrue
            '****************** Create new version of policy ************************
            'assign current insurance file count to business object
            m_oInsuranceFile.InsuranceFileCnt = v_lInsuranceFileCnt

            ' get details of current policy
            If (m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue) Then
                r_sMessage = "Failed to get policy details for " & v_lInsuranceFileCnt
                Throw New Exception(r_sMessage)
            End If


            'set last transaction type if required
            If v_sTransactionType <> "" Then
                m_oInsuranceFile.LastTransType = v_sTransactionType
            End If


            m_oInsuranceFile.PolicyVersion = CLng(m_oInsuranceFile.PolicyVersion) + 1
            m_oInsuranceFile.CoverStartDate = v_dTransferDate

            ' Set old version to "replaced portfolio transfer"
            m_oInsuranceFile.InsuranceFileStatus = SIRInsFileStatusReplacedPT


            m_oInsuranceFile.EventDescription = "Policy copied for portfolio transfer"


            m_oInsuranceFile.UpdatePolicy()

            ' create new version of policy based on current policy details
            If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create new version of policy for " & v_lInsuranceFileCnt
                Throw New Exception(r_sMessage)
            End If

            '******************** Get new policy version details ******************************************
            r_lNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

            'do we need to return start date?
            r_dtPolicyStartDate = CDate(m_oInsuranceFile.CoverStartDate)


            'do we need to return insurance_folder_cnt?
            If Not Informations.IsNothing(r_lInsuranceFolderCnt) Then
                r_lInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt   'note folder will be the same as previous policy version
            End If

            '****************** Copy related data to new policy version ***********************
            'copy standard wording
            If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy standard wording to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy coinsurance
            If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy coinsurance details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy agent commission
            If m_oRenSelection.CopyAgentCommission(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy agent commission details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw New Exception(r_sMessage)
            End If

            'copy agents
            If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=v_lInsuranceFileCnt, v_lNewInsFileCnt:=r_lNewInsurancefileCnt) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy agent details to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw New Exception(r_sMessage)
            End If

            '************** Policy level reinsurance is not supported at present **************************
            '    'copy policy level reinsurance
            '    m_oReinsurance.InsuranceFileCnt = r_lNewInsurancefileCnt
            '    m_oReinsurance.RiskId = 0
            '
            '    If m_oReinsurance.GetDetails() <>gPMConstants.PMEReturnCode.PMTrue Then
            '        r_sMessage = "Failed to copy policy level reinsurance to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
            '        GoTo Err_ProcessSingleDefRIPolicy
            '    End If

            ' copy policy level tax
            ' Must set the Task in Tax as this is passed into stored procedures as Mode. If it is wrong the new tax records will not be created.
            If m_oTax.SetProcessModes(vTask:=PMConst.PMEdit) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to SetProcessModes() to Tax object"
                Throw New Exception(r_sMessage)
            End If

            m_oTax.InsuranceFileCnt = r_lNewInsurancefileCnt
            If m_oTax.GetInsuranceFileTax(r_vInsuranceFileTax:=vInsuranceFileTax, r_sDescription:=CStr(sDescription)) <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to copy policy level tax to new policy version (current policy :" & v_lInsuranceFileCnt & " new policy " & r_lNewInsurancefileCnt & " )"
                Throw New Exception(r_sMessage)
            End If


        Catch ex As Exception
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed CopyPolicyHeader() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="CopyPolicyHeader", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        Finally

        End Try
        Return lReturnValue
    End Function

    ' E007 Changes
    Private Function TransferClaimToNewRisk(ByVal v_lInsuranceFileCnt As Long,
                                        ByVal nClaimId As Integer,
                                        ByRef r_sMessage As String) As Integer


        Const kMethodName As String = "TransferClaimToNewRisk"

        Dim nReturnValue As Integer
        Dim oControlTransClaims As bControlTransClaims.Automated
        Dim nNewClaimId As Integer
        Dim nStatsFolderId As Integer
        Dim sMessage As String = ""
        Try
            sMessage = ""
            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            nReturnValue = CreateClaimVersionForPT(v_lInsuranceFileCnt, nClaimId, nNewClaimId, nStatsFolderId, 0)
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "CreateClaimVersionForPT method failed"
                RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            If nStatsFolderId > 0 Then

                oControlTransClaims = New bControlTransClaims.Automated
                nReturnValue = oControlTransClaims.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=m_sCallingAppName, vDatabase:=m_oDatabase)

                If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                    r_sMessage = "Failed to create and post stats"
                    RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If

                oControlTransClaims.ClaimID = nNewClaimId
                oControlTransClaims.DocumentTypeID = 41


                nReturnValue = oControlTransClaims.CreateTransactions(nStatsFolderId)
                If (nReturnValue <> gPMConstants.PMEReturnCode.PMTrue) Then
                    r_sMessage = "CreateTransactions failed"
                    RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            nReturnValue = FinaliseClaimDetails(v_lClaimId:=nNewClaimId, v_sClaimVersionDescription:="Portfolio Claim Adjustment")
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "FinaliseClaimDetails failed"
                RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

            nReturnValue = UpdateClaimIsDirtyFlag(v_lclaimCnt:=nNewClaimId, v_iIsDirty:=0)
            If (nReturnValue <> gPMConstants.PMEReturnCode.PMTrue) Then
                r_sMessage = "UpdateClaimIsDirtyFlag failed"
                RaiseError(kMethodName, r_sMessage, gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As System.Exception
            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed TransferClaimToNewRisk() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="TransferClaimToNewRisk", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
        Finally
            oControlTransClaims = Nothing
        End Try
        Return nReturnValue
    End Function

    Private Function ProcessClaimPerils(ByVal v_lclaimCnt As Long,
                                        ByVal v_iPrePortfolioTransfer As Integer,
                                        ByVal v_lInsuranceFileCnt As Long,
                                        ByVal v_iIsCreated As Integer) As Long


        'Const kMethodName As String = "ProcessClaimPerils"
        Dim lReturnValue As gPMConstants.PMEReturnCode
        Dim sMessage As String = "" = ""
        Dim r_vNetOfClaimPerils As New DataTable
        Dim iCountPerils As Integer
        Dim r_cThisRevision As Decimal
        Dim r_iStatsFolderCnt As Integer
        Dim oControlTransClaims As bControlTransClaims.Automated
        Dim bAlreadyCreated As Boolean

        Try

            lReturnValue = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetNetOfClaimPerils(v_lClaimId:=v_lclaimCnt,
                                            dtResult:=r_vNetOfClaimPerils)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception
            End If

            If r_vNetOfClaimPerils.Rows.Count > 0 Then
                For Each row As DataRow In r_vNetOfClaimPerils.Rows
                    r_cThisRevision = 0

                    m_lReturn = NetOfClaimPerils(v_lclaimCnt:=v_lclaimCnt,
                                                v_lClaimPerilId:=CInt(r_vNetOfClaimPerils.Rows(iCountPerils)(0).ToString),
                                                v_iPrePortfolioTransfer:=v_iPrePortfolioTransfer,
                                                r_cThisRevision:=r_cThisRevision)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                        Throw New Exception
                    End If

                    ' Create gross line
                    If r_cThisRevision <> 0 Then
                        If bAlreadyCreated = False Then
                            m_lReturn = AddClaimsStatsFolder(v_lInsuranceFileCnt:=v_lInsuranceFileCnt,
                                                        v_sDebitCredit:="D",
                                                        v_sDocumentComment:="Reserve for claim number " & v_lclaimCnt,
                                                        v_iTransactionTypeId:=1,
                                                        v_sTransactionTypeCode:="C_CR",
                                                        v_iUserID:=m_iUserID,
                                                        v_sUsername:=m_sUsername,
                                                        v_iClaimId:=v_lclaimCnt,
                                                        v_iDocumentTypeId:=41,
                                                        r_iStatsFolderCnt:=r_iStatsFolderCnt)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                Throw New Exception
                            End If
                            bAlreadyCreated = True
                        End If

                        ' Add Stats Details
                        m_lReturn = AddClaimsStatsDetail(v_iStatsFolderCnt:=r_iStatsFolderCnt,
                                                v_lClaimId:=v_lclaimCnt,
                                                v_iClaimPerilId:=CInt(r_vNetOfClaimPerils.Rows(iCountPerils)(0).ToString),
                                                v_sStatsDetailType:="GRS",
                                                v_iClassOfBusiness:=CInt(r_vNetOfClaimPerils.Rows(iCountPerils)(1).ToString),
                                                v_sClassOfBusinessCode:=r_vNetOfClaimPerils.Rows(iCountPerils)(2).ToString,
                                                v_iRIPartyCnt:=0,
                                                v_sCreditAccountCode:="CLMRES" & r_vNetOfClaimPerils.Rows(iCountPerils)(2).ToString,
                                                v_iRIPartyType:=0,
                                                v_dRISharePercent:=0,
                                                v_crTransactionAmount:=r_cThisRevision,
                                                v_iDocumentTypeId:=41)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                            Throw New Exception
                        End If
                    End If
                    iCountPerils = iCountPerils + 1
                Next

                ' Accounting Raised only for RI
                oControlTransClaims = New bControlTransClaims.Automated
                m_lReturn = oControlTransClaims.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'RaiseError "CreateBusinessObject", "Failed to Create Object bControlTransClaims.Automated"
                    Throw New Exception
                End If

                If r_iStatsFolderCnt > 0 Then

                    'Process Claim Reinsurance
                    m_lReturn = ProcessClaimReinsurance(v_lclaimCnt:=v_lclaimCnt, v_iIsCreated:=v_iIsCreated)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "ProcessClaimReinsurance", "Failed to Process Reinsurance for Claim Id " & v_lclaimCnt

                        Throw New Exception
                    End If

                    oControlTransClaims.ClaimID = v_lclaimCnt
                    oControlTransClaims.DocumentTypeID = 41
                    m_lReturn = oControlTransClaims.CreateStatsForCoinsReins(ToSafeLong(r_iStatsFolderCnt, 0))
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                        Throw New Exception
                    End If

                    m_lReturn = FinaliseStats(v_lClaimId:=v_lclaimCnt,
                                    v_lTransactionTypeId:=41,
                                    v_sTransactionTypeCode:="C_CR",
                                    v_lStatsFolderCnt:=r_iStatsFolderCnt,
                                    v_lStatsSuppressed:=0)
                    If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                        Throw New Exception
                    End If

                    'm_lReturn = oControlTransClaims.CreateTransactions(ToSafeLong(r_iStatsFolderCnt, 0))
                    'If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    '    Throw New Exception
                    'End If
                End If

                m_lReturn = FinaliseClaimDetails(v_lClaimId:=v_lclaimCnt, v_sClaimVersionDescription:="Portfolio Claim Adjustment")
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'RaiseError "FinaliseClaimDetails", "Failed to FinaliseClaim for Claim Id " & v_lclaimCnt
                    Throw New Exception
                End If

                m_lReturn = UpdateClaimIsDirtyFlag(v_lclaimCnt:=v_lclaimCnt, v_iIsDirty:=0)
                If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Then

                    Throw New Exception
                End If

            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            lReturnValue = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=If(sMessage = "", "Failed ProcessClaimPerils() ", sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessClaimPerils", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            Return lReturnValue
        Finally
            oControlTransClaims = Nothing




        End Try
        Return lReturnValue
    End Function

    Private Function NetOfClaimPerils(ByVal v_lclaimCnt As Long,
                                    ByVal v_lClaimPerilId As Long,
                                    ByVal v_iPrePortfolioTransfer As Integer,
                                    ByRef r_cThisRevision As Decimal) As Long


        Const kMethodName As String = "NetOfClaimPerils"



        NetOfClaimPerils = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()

            ' Add parameters
            m_lReturn = .Parameters.Add(sName:="claim_id", vValue:=v_lclaimCnt, iDirection:=PMConst.PMParamInput, iDataType:=PMConst.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to add parameter claim_id ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .Parameters.Add(sName:="claim_peril_id", vValue:=v_lClaimPerilId, iDirection:=PMConst.PMParamInput, iDataType:=PMConst.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to add parameter claim_peril_id ", gPMConstants.PMELogLevel.PMLogError)
            End If

            .Parameters.Add("PrePortfolioTransfer", v_iPrePortfolioTransfer, PMConst.PMParamInput, PMConst.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to add parameter PrePortfolioTransfer ", gPMConstants.PMELogLevel.PMLogError)
            End If

            .Parameters.Add("this_revision", 0, PMConst.PMParamOutput, PMConst.PMCurrency)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed to add parameter this_revision ", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = .SQLAction(
                    sSQL:=ACNetOfClaimPerilReserveSQL,
                    sSQLName:=ACGetClaimPerilsName,
                    bStoredProcedure:=ACGetClaimPerilsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed NetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)
            End If

            r_cThisRevision = ToSafeCurrency(.Parameters.Item("this_revision").Value, 0)
        End With


    End Function

    Private Function ProcessClaimReinsurance(ByVal v_lclaimCnt As Long, ByVal v_iIsCreated As Integer) As Long

        Dim sMessage As String = ""

        Const kMethodName As String = "ProcessClaimReinsurance"
        ProcessClaimReinsurance = gPMConstants.PMEReturnCode.PMTrue

        If v_iIsCreated = 1 Then
            m_oReinsuranceClaim.IsCreated = 1
        End If
        m_oReinsuranceClaim.ClaimId = v_lclaimCnt
        m_oReinsuranceClaim.Task = PMConst.PMEdit
        m_lReturn = m_oReinsuranceClaim.CalculateRI()

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "bCLMReinsurance.CalculateRI Failed for " & v_lclaimCnt
            RaiseError(kMethodName, "Failed NetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)
        End If

    End Function

    Private Function GetAllClaimsOnPolicy(ByVal v_lInsuranceFileCnt As Long, ByRef r_vResultArray(,) As Object) As Long

        Const kMethodName As String = "GetAllClaimsOnPolicy"



        GetAllClaimsOnPolicy = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                                vValue:=CInt(v_lInsuranceFileCnt),
                                                iDirection:=PMConst.PMParamInput,
                                                iDataType:=PMConst.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed NetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Get policies
        m_lReturn = m_oDatabase.SQLSelect(
                sSQL:=ACGetAllClaimsOnRiskSQL,
                sSQLName:=ACGetAllClaimsOnRiskName,
                bStoredProcedure:=ACGetAllClaimsOnRiskStored,
                vResultArray:=r_vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed GetAllClaimsOnPolicy ", gPMConstants.PMELogLevel.PMLogError)

        End If


    End Function

    Private Function ClaimReversingPrePolicyTransfer(ByVal v_lInsuranceFileCnt As Long, ByVal nClaimId As Integer, ByRef nNewClaimId As Integer) As Integer

        Const kMethodName As String = "ClaimReversingPrePolicyTransfer"
        'Const ACFieldPosRiskID As Long = 0

        Dim oControlTransClaims As bControlTransClaims.Automated
        Dim nStatsFolderId As Integer
        Dim nReturn As Integer



        ClaimReversingPrePolicyTransfer = gPMConstants.PMEReturnCode.PMTrue
        nReturn = CreateClaimVersionForPT(v_lInsuranceFileCnt, nClaimId, nNewClaimId, nStatsFolderId, 1)
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed CreateClaimVersionForPT ", gPMConstants.PMELogLevel.PMLogError)
        End If

        If nStatsFolderId > 0 Then
            ' Create Object bCLMFindClaim
            oControlTransClaims = New bControlTransClaims.Automated
            nReturn = oControlTransClaims.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CreateBusinessObject bControlTransClaims.Automated ", gPMConstants.PMELogLevel.PMLogError)

            End If
            oControlTransClaims.ClaimID = nNewClaimId
            oControlTransClaims.DocumentTypeID = 41


            nReturn = oControlTransClaims.CreateTransactions(nStatsFolderId)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
                RaiseError(kMethodName, "Failed CreateTransactions ", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        nReturn = FinaliseClaimDetails(v_lClaimId:=CInt(nNewClaimId), v_sClaimVersionDescription:="Portfolio Claim Adjustment")
        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed FinaliseClaimDetails ", gPMConstants.PMELogLevel.PMLogError)
        End If

        nReturn = UpdateClaimIsDirtyFlag(v_lclaimCnt:=CInt(nNewClaimId), v_iIsDirty:=0)
        If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) Then
            RaiseError(kMethodName, "Failed UpdateClaimIsDirtyFlag ", gPMConstants.PMELogLevel.PMLogError)
        End If



        oControlTransClaims = Nothing
        Return nReturn
    End Function

    Private Function AddClaimsStatsFolder(ByVal v_lInsuranceFileCnt As Long,
                                            ByVal v_sDebitCredit As String,
                                            ByVal v_sDocumentComment As String,
                                            ByVal v_iTransactionTypeId As Integer,
                                            ByVal v_sTransactionTypeCode As String,
                                            ByVal v_iUserID As Integer,
                                            ByVal v_sUsername As String,
                                            ByVal v_iClaimId As Integer,
                                            ByVal v_iDocumentTypeId As Integer,
                                            ByRef r_iStatsFolderCnt As Integer) As Long



        Const kMethodName As String = "AddClaimsStatsFolder"



        AddClaimsStatsFolder = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt",
                                             vValue:=CInt(v_lInsuranceFileCnt),
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="debit_credit",
                                             vValue:=v_sDebitCredit,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="document_comment",
                                             vValue:=v_sDocumentComment,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id",
                                             vValue:=v_iTransactionTypeId,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code",
                                             vValue:=CStr(v_sTransactionTypeCode),
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id",
                                             vValue:=v_iUserID,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="user_name",
                                             vValue:=v_sUsername,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                             vValue:=v_iClaimId,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id",
                                             vValue:=v_iDocumentTypeId,
                                             iDirection:=PMConst.PMParamInput,
                                             iDataType:=PMConst.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                             vValue:=0,
                                             iDirection:=PMConst.PMParamOutput,
                                             iDataType:=PMConst.PMInteger)


        m_lReturn = m_oDatabase.SQLAction(
            sSQL:=ACAddStatsFolderClaimsSQL,
            sSQLName:=ACAddStatsFolderClaimsName,
            bStoredProcedure:=ACAddStatsFolderClaimsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed AddClaimsStatsFolder ", gPMConstants.PMELogLevel.PMLogError)

        End If

        ' Output parameter
        r_iStatsFolderCnt = ToSafeInteger(m_oDatabase.Parameters.Item("stats_folder_cnt").Value, 0)



    End Function

    Private Function GetNetOfClaimPerils(ByVal v_lClaimId As Long,
                                        ByRef dtResult As DataTable) As Long

        Const kMethodName As String = "GetNetOfClaimPerils"

        GetNetOfClaimPerils = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' Add parameters
        bPMAddParameter.AddParameterLite(m_oDatabase, "claim_id", CLng(v_lClaimId), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

        ' Get policies
        m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetNetOfClaimPerilSQL, sSQLName:=ACGetNetOfClaimPerilName, bStoredProcedure:=ACGetNetOfClaimPerilStored, oRecordset:=dtResult)


        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed GetNetOfClaimPerils ", gPMConstants.PMELogLevel.PMLogError)

        End If



    End Function

    Private Function AddClaimsStatsDetail(ByVal v_iStatsFolderCnt As Integer,
                                            ByVal v_lClaimId As Long,
                                            ByVal v_iClaimPerilId As Integer,
                                            ByVal v_sStatsDetailType As String,
                                            ByVal v_iClassOfBusiness As Integer,
                                            ByVal v_sClassOfBusinessCode As String,
                                            ByVal v_iRIPartyCnt As Integer,
                                            ByVal v_sCreditAccountCode As String,
                                            ByVal v_iRIPartyType As Integer,
                                            ByVal v_dRISharePercent As Double,
                                            ByVal v_crTransactionAmount As Decimal,
                                            ByVal v_iDocumentTypeId As Integer) As Long



        Const kMethodName As String = "AddClaimsStatsDetail"




        AddClaimsStatsDetail = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                             vValue:=v_iStatsFolderCnt,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                             vValue:=CInt(v_lClaimId),
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="peril_id",
                                             vValue:=v_iClaimPerilId,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_detail_type",
                                             vValue:=v_sStatsDetailType,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_id",
                                             vValue:=v_iClassOfBusiness,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="class_of_business_code",
                                             vValue:=v_sClassOfBusinessCode,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMString)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_party_cnt",
                                             vValue:=v_iRIPartyCnt,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_shortname",
                                             vValue:=v_sCreditAccountCode,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_party_type",
                                             vValue:=v_iRIPartyType,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="ri_share_percent",
                                             vValue:=v_dRISharePercent,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMDouble)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_amount",
                                             vValue:=v_crTransactionAmount,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMCurrency)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="documenttype_id",
                                             vValue:=v_iDocumentTypeId,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.SQLAction(
            sSQL:=ACAddStatsDetailsClaimsSQL,
            sSQLName:=ACAddStatsDetailsClaimsName,
            bStoredProcedure:=ACAddStatsDetailsClaimsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed AddClaimsStatsDetail ", gPMConstants.PMELogLevel.PMLogError)

        End If


    End Function

    Private Function FinaliseStats(ByVal v_lClaimId As Integer,
                                    ByVal v_lTransactionTypeId As Integer,
                                    ByVal v_sTransactionTypeCode As String,
                                    ByVal v_lStatsFolderCnt As Integer,
                                    ByVal v_lStatsSuppressed As Integer) As Long



        Const kMethodName As String = "FinaliseStats"



        FinaliseStats = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        ' Add parameters

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                             vValue:=CInt(v_lClaimId),
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMLong)


        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_id",
                                             vValue:=v_lTransactionTypeId,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="transaction_type_code",
                                             vValue:=CStr(v_sTransactionTypeCode),
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMString)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="stats_folder_cnt",
                                             vValue:=v_lStatsFolderCnt,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="bstatssuppressed",
                                             vValue:=v_lStatsSuppressed,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.SQLAction(
            sSQL:=ACUpdateClaimFinaliseStatsSQL,
            sSQLName:=ACUpdateClaimFinaliseStatsName,
            bStoredProcedure:=ACUpdateClaimFinaliseStatsStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed FinaliseStats ", gPMConstants.PMELogLevel.PMLogError)

        End If


    End Function

    Private Function UpdateClaimIsDirtyFlag(ByVal v_lclaimCnt As Long,
                                    ByVal v_iIsDirty As Integer) As Long


        Const kMethodName As String = "UpdateClaimIsDirtyFlag"



        UpdateClaimIsDirtyFlag = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()
        ' Add parameters

        m_lReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                             vValue:=v_lclaimCnt,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMLong)

        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_dirty",
                                             vValue:=v_iIsDirty,
                                             iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                             iDataType:=gPMConstants.PMEDataType.PMInteger)


        m_lReturn = m_oDatabase.SQLAction(
                    sSQL:=ACUpdateClaimStatusSQL,
                    sSQLName:=ACUpdateClaimStatusName,
                    bStoredProcedure:=ACUpdateClaimStatusStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "Failed UpdateClaimIsDirtyFlag ", gPMConstants.PMELogLevel.PMLogError)

        End If
    End Function

    Public Function FinaliseClaimDetails(
                        ByVal v_lClaimId As Long,
                        ByVal v_sClaimVersionDescription As String) As Long


        Dim nReturn As Integer
        Try


            m_oDatabase.Parameters.Clear()

            ' Add Required Stored Procedure Parameters
            nReturn = m_oDatabase.Parameters.Add(sName:="claim_id",
                                                 vValue:=CInt(v_lClaimId),
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturn
            End If


            nReturn = m_oDatabase.Parameters.Add(sName:="claim_version_description",
                                                 vValue:=v_sClaimVersionDescription,
                                                 iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                                 iDataType:=gPMConstants.PMEDataType.PMString)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturn
            End If

            ' Execute Action Query
            nReturn = m_oDatabase.SQLAction(
                                    sSQL:=kFinaliseClaimDetailsSQL,
                                    sSQLName:=kFinaliseClaimDetailsName,
                                    bStoredProcedure:=True)

            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nReturn
            End If
        Catch ex As System.Exception
            nReturn = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FinaliseClaimDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FinaliseClaimDetails", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
        Return nReturn
    End Function


    Public Function GetPolicyType(
            ByVal v_lInsuranceFileCnt As Long,
            ByRef r_lInsuranceFileTypeId As Long) As Long


        Dim dtResult As New DataTable
        Const kMethodName As String = "GetPolicyType"

        Try

            ' set the params
            m_oDatabase.Parameters.Clear()

            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_File_Cnt", CInt(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetInsuranceRefSQL, sSQLName:=ACGetInsuranceRefName, bStoredProcedure:=ACGetInsuranceRefStored, oRecordset:=dtResult)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

            End If


            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lInsuranceFileTypeId = CLng(dtResult.Rows(0)(1).ToString)
            Else
                GetPolicyType = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
            End If

        Catch excep As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPolicyType", r_lFunctionReturn:=GetPolicyType, excep:=excep)
        End Try
        Return m_lReturn
    End Function

    Public Function GetPreviousRiskCnt(
                        ByVal v_lPreInsuranceFileCnt As Long,
                        ByVal v_lRiskCnt As Long,
                        ByVal v_lInsuranceFileCnt As Long,
                        ByRef r_lPreviousRiskCnt As Long) As Long


        Dim dtResult As New DataTable
        Const kMethodName As String = "GetPreviousRiskCnt"
        Try

            GetPreviousRiskCnt = gPMConstants.PMEReturnCode.PMTrue

            r_lPreviousRiskCnt = 0


            'Send the new file in
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "Insurance_file_cnt", CLng(v_lPreInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "Risk_cnt", CLng(v_lRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "processed_Insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Execute the SP
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetPreviousRiskCntForTransferSQL, sSQLName:=ACGetPreviousRiskCntForTransferName, bStoredProcedure:=True, oRecordset:=dtResult)


            'Determine the result
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)
            ElseIf dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_lPreviousRiskCnt = CLng(dtResult.Rows(0)(0).ToString)
            End If

            Exit Function

        Catch excep As System.Exception

            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetPreviousRiskCnt", r_lFunctionReturn:=GetPreviousRiskCnt, excep:=excep)

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
    Public Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Long,
                            ByVal v_vRiskDetail(,) As Object,
                            ByVal v_lPosNo As Long,
                            ByRef r_lRiskCnt As Long,
                            Optional ByVal v_lResetStatus As Long = 0,
                            Optional ByVal v_lCreateLinkType As Long = 0,
                            Optional ByVal v_bAutoCancellation As Boolean = False,
                            Optional v_sRiskMergeStatus As String = "",
                            Optional v_lOldRiskCnt As Long = 0) As Long


        Dim lRiskCnt As Long

        Dim lOldRiskCnt As Long
        Dim lNewRiskCnt As Long
        Dim dtResult As New DataTable
        Dim iIsAutoReinsured As Integer
        Dim vArray As Object
        Const kMethodName As String = "CopyRisk"
        Try

            CopyRisk = gPMConstants.PMEReturnCode.PMTrue

            lOldRiskCnt = v_vRiskDetail(0, v_lPosNo)

            'Tomo030801
            'This bit's here because we need to reset the auto reinsured flag to that
            'from the risk type.
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_vRiskDetail(4, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)



            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAutoReinsuredSQL, sSQLName:=ACGetAutoReinsuredName, bStoredProcedure:=ACGetAutoReinsuredStored, oRecordset:=dtResult)



            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

            End If


            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                iIsAutoReinsured = CInt(dtResult.Rows(0)(0))
            Else
                iIsAutoReinsured = 0
            End If

            v_vRiskDetail(20, v_lPosNo) = iIsAutoReinsured

            vArray = Nothing

            'Tomo030801 - End

            m_oDatabase.Parameters.Clear()


            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_cnt", 0, gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMLong)

            'TN20010719 - start
            If v_lResetStatus = gPMConstants.PMEReturnCode.PMTrue Then
                'reset status to UnQuoted
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", 4, gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

                'TN20010719 - end
            Else
                bPMAddParameter.AddParameterLite(m_oDatabase, "risk_status_id", v_vRiskDetail(1, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            End If

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_folder_cnt", v_vRiskDetail(2, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "accumulation_id", v_vRiskDetail(3, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_type_id", v_vRiskDetail(4, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "description", v_vRiskDetail(5, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sequence_number", v_vRiskDetail(6, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "sum_insured_requested", v_vRiskDetail(7, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)

            bPMAddParameter.AddParameterLite(m_oDatabase, "inception_date", v_vRiskDetail(8, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "expiry_date", v_vRiskDetail(9, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "is_not_index_linked", v_vRiskDetail(10, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_accumulated", v_vRiskDetail(11, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)



            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_reason_id", v_vRiskDetail(12, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_date", v_vRiskDetail(13, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDate)

            bPMAddParameter.AddParameterLite(m_oDatabase, "lapsed_description", v_vRiskDetail(14, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            bPMAddParameter.AddParameterLite(m_oDatabase, "var_data_ref", v_vRiskDetail(15, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "total_sum_insured", v_vRiskDetail(16, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)


            bPMAddParameter.AddParameterLite(m_oDatabase, "total_annual_premium", v_vRiskDetail(17, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)


            bPMAddParameter.AddParameterLite(m_oDatabase, "total_this_premium", v_vRiskDetail(18, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_ri_at_risk_level", v_vRiskDetail(19, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_auto_reinsured", v_vRiskDetail(20, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            bPMAddParameter.AddParameterLite(m_oDatabase, "gis_screen_id", v_vRiskDetail(21, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            bPMAddParameter.AddParameterLite(m_oDatabase, "eml_percentage", v_vRiskDetail(22, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)


            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_number", v_vRiskDetail(23, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            ' PW311002
            bPMAddParameter.AddParameterLite(m_oDatabase, "variation_number", v_vRiskDetail(24, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_risk_selected", v_vRiskDetail(25, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            bPMAddParameter.AddParameterLite(m_oDatabase, "coverage", v_vRiskDetail(26, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            bPMAddParameter.AddParameterLite(m_oDatabase, "insured_item", v_vRiskDetail(27, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            bPMAddParameter.AddParameterLite(m_oDatabase, "extensions", v_vRiskDetail(28, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)


            bPMAddParameter.AddParameterLite(m_oDatabase, "premium_this_year", v_vRiskDetail(31, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMDouble)


            bPMAddParameter.AddParameterLite(m_oDatabase, "is_mandatory_risk", v_vRiskDetail(36, v_lPosNo), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMInteger)


            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACAddRiskSQL, sSQLName:=ACAddRiskName, bStoredProcedure:=ACAddRiskStored, oRecordset:=dtResult)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

            End If

            lRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

            If v_bAutoCancellation = False Then

                'RWH(20/11/2000) Add link record into Insurance_file_risk_link.
                If (lRiskCnt <> 0) Then

                    ' Determine whether or not to create a link and
                    ' if so what kind of link
                    Select Case v_lCreateLinkType

                        Case 0 ' standard - original and renewed risk cnt not populated
                            m_lReturn = m_oRiskData.AddRiskLink(
                                        CInt(v_lNewInsuranceFileCnt),
                                        CInt(lRiskCnt),
                                        "C", 0, 0)

                        Case 1 ' populate original_risk_cnt
                            m_lReturn = m_oRiskData.AddRiskLink(
                                CInt(v_lNewInsuranceFileCnt),
                                CInt(lRiskCnt),
                                "C",
                                CInt(v_lOldRiskCnt), 0)

                        Case 2 ' populate renewed_risk_cnt
                            m_lReturn = m_oRiskData.AddRiskLink(
                                CInt(v_lNewInsuranceFileCnt),
                                CInt(lRiskCnt),
                                "C",
                                0,
                                CInt(v_lOldRiskCnt))

                        Case Else
                            ' do nothing
                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

                    End If

                    r_lRiskCnt = lRiskCnt

                    m_lReturn = m_oRiskData.CopyRatingSection(v_lOldRiskCnt:=lOldRiskCnt,
                                                  v_lNewRiskCnt:=lRiskCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

                    End If
                End If

            Else

                ' delete the original insurance file risk link
                m_lReturn = m_oRiskData.DeleteInsuranceFileRiskLink(
                    v_lInsuranceFileCnt:=v_lNewInsuranceFileCnt,
                    v_lRiskCnt:=v_lOldRiskCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

                End If

                ' add new
                m_lReturn = m_oRiskData.AddRiskLink(
                    v_lNewInsuranceFileCnt,
                    lRiskCnt,
                    If(v_sRiskMergeStatus = "DP", "D", "C"),
                    v_lOldRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed GetPolicyType ", gPMConstants.PMELogLevel.PMLogError)

                End If

                r_lRiskCnt = lRiskCnt
                lNewRiskCnt = lRiskCnt

                m_lReturn = m_oRiskData.CopyRiskExtras(v_lOldRiskCnt:=v_lOldRiskCnt,
                                          v_lNewRiskCnt:=lNewRiskCnt)

                'sj 20/12/2002 - end
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "Failed CopyRisk ", gPMConstants.PMELogLevel.PMLogError)

                End If

            End If

        Catch excep As System.Exception
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CopyRisk", r_lFunctionReturn:=CopyRisk, excep:=excep)
        End Try

    End Function


    Public Function CopyRatings(v_lInsuranceFileCnt As Long, r_lOriginalRiskCnt As Long, r_lRiskCnt As Long, dProRataRate As Double)
        Dim j As Integer
        Dim m_dProrataRate As Double
        Dim dtResult As New DataTable

        Const kMethodName As String = "CopyRatings"

        Try
            CopyRatings = gPMConstants.PMEReturnCode.PMTrue

            'All unedited Risks go through without any pro-rata
            m_dProrataRate = dProRataRate

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt",
                                       vValue:=r_lRiskCnt,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelPerilSQL,
                                              sSQLName:=ACDelPerilName,
                                              bStoredProcedure:=ACDelPerilStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

            End If
            'Del Rating Sections
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="Risk_Cnt",
                                       vValue:=r_lRiskCnt,
                                       iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                                       iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACDelRatingSectionSQL,
                                              sSQLName:=ACDelRatingSectionName,
                                              bStoredProcedure:=ACDelRatingSectionStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

            End If
            'Get Original Rating Sections
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            bPMAddParameter.AddParameterLite(m_oDatabase, "risk_id", CLng(r_lOriginalRiskCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            'Fetch the records
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACSelectRatingSectionSQL, sSQLName:=ACSelectRatingSectionName, bStoredProcedure:=ACSelectRatingSectionStored, oRecordset:=dtResult)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then

                For Each row As DataRow In dtResult.Rows
                    m_lReturn = CopyRatingSectionsAndPerils(dtResult, 1, 0, v_lInsuranceFileCnt, r_lRiskCnt, j, m_dProrataRate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "Failed CopyRatings ", gPMConstants.PMELogLevel.PMLogError)

                    End If
                    j = j + 1
                Next
            End If


        Catch excep As System.Exception

            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CopyRatings", r_lFunctionReturn:=CopyRatings, excep:=excep)

        End Try

    End Function


    Public Function CopyRatingSectionsAndPerils(ByVal dtResult As DataTable, ByVal i_ThisPremiumSign As Integer, ByVal i_OriginalFlag As Integer, ByVal m_lInsuranceFileCnt As Long, ByVal m_lRiskCnt As Long, iIndex As Integer, Optional dProrata As Double = 0) As Long

        Dim m_lPolicyRatingSectionTypeId As Long
        Dim m_cThisPremium As Decimal
        Dim m_lInsuranceFileNoOfDp As Long
        Dim cAnnualPremium As Decimal
        Const kMethodName As String = "CopyRatingSectionsAndPerils"


        Try

            CopyRatingSectionsAndPerils = gPMConstants.PMEReturnCode.PMTrue

            m_lPolicyRatingSectionTypeId = -1
            If CDec(dtResult.Rows(iIndex)(6).ToString) = 0 Then
                cAnnualPremium = CDec(dtResult.Rows(iIndex)(5).ToString)
            Else
                cAnnualPremium = CDec(dtResult.Rows(iIndex)(6).ToString)
            End If

            If Informations.IsNothing(dProrata) Then
                m_cThisPremium = i_ThisPremiumSign * cAnnualPremium
            Else
                m_cThisPremium = i_ThisPremiumSign * (cAnnualPremium * dProrata)
            End If
            m_lInsuranceFileNoOfDp = 2

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="rating_section_type_id",
                vValue:=CLng(dtResult.Rows(iIndex)(10).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="policy_section_type_id",
                vValue:=m_lPolicyRatingSectionTypeId,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="insurance_file_cnt",
                vValue:=m_lInsuranceFileCnt,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="risk_id",
                vValue:=m_lRiskCnt,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="sum_insured",
                vValue:=ToSafeCurrency(dtResult.Rows(iIndex)(4).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="annual_rate",
                vValue:=ToSafeCurrency(dtResult.Rows(iIndex)(3).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="annual_premium",
                vValue:=ToSafeCurrency(dtResult.Rows(iIndex)(6).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="this_premium",
                vValue:=m_cThisPremium,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="rate_type_id",
                vValue:=CLng(dtResult.Rows(iIndex)(12).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="insurance_file_no_of_dp",
                vValue:=m_lInsuranceFileNoOfDp,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="original_flag",
                vValue:=i_OriginalFlag,
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMLong)

            If dtResult.Rows(iIndex)(14).ToString = "" Then
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="currency_id",
                    vValue:=Nothing,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(
                  sName:="currency_id",
                  vValue:=CInt(dtResult.Rows(iIndex)(14).ToString),
                  iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                  iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If (dtResult.Rows(iIndex)(15).ToString) = "" Then
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="country_id",
                    vValue:=Nothing,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="country_id",
                    vValue:=CInt(dtResult.Rows(iIndex)(15).ToString),
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If

            If (dtResult.Rows(iIndex)(16).ToString) = "" Then
                m_lReturn = m_oDatabase.Parameters.Add(
                    sName:="state_id",
                    vValue:=Nothing,
                    iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                    iDataType:=gPMConstants.PMEDataType.PMInteger)
            Else
                m_lReturn = m_oDatabase.Parameters.Add(
                   sName:="state_id",
                   vValue:=CInt(dtResult.Rows(iIndex)(16).ToString),
                   iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                   iDataType:=gPMConstants.PMEDataType.PMInteger)
            End If
            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="is_amended",
                vValue:=CInt(dtResult.Rows(iIndex)(17).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="calculated_premium",
                vValue:=ToSafeCurrency(CDbl(dtResult.Rows(iIndex)(18).ToString), 0),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMCurrency)

            m_lReturn = m_oDatabase.Parameters.Add(
                sName:="override_reason",
                vValue:=CStr(dtResult.Rows(iIndex)(19).ToString),
                iDirection:=gPMConstants.PMEParameterDirection.PMParamInput,
                iDataType:=gPMConstants.PMEDataType.PMString)


            ' Add Section & Perils
            m_lReturn = m_oDatabase.SQLAction(
                sSQL:=ACAddSectionAndPerilsSQL,
                sSQLName:=ACAddSectionAndPerilsName,
                bStoredProcedure:=ACAddSectionAndPerilsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed CopyRatingSectionsAndPerils ", gPMConstants.PMELogLevel.PMLogError)

            End If

        Catch excep As System.Exception

            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="CopyRatingSectionsAndPerils", r_lFunctionReturn:=CopyRatingSectionsAndPerils, excep:=excep)

        End Try
    End Function




    Public Function GetAllRiskStatus(
            ByVal v_lInsuranceFileCnt As Long,
            ByRef r_bIsRisksQuoted As Boolean) As Integer


        Dim dtResult As New DataTable
        Dim nReturn As Integer
        Const kMethodName As String = "GetAllRiskStatus"

        Try

            GetAllRiskStatus = gPMConstants.PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            ' Add Parameter
            bPMAddParameter.AddParameterLite(m_oDatabase, "insurance_file_cnt", CLng(v_lInsuranceFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            ' get the risk status of the risk
            nReturn = m_oDatabase.ExecuteDataTable(sSQL:=ACGetAllRiskStatusSQL, sSQLName:=ACGetAllRiskStatusName, bStoredProcedure:=ACGetAllRiskStatusStored, oRecordset:=dtResult)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed GetAllRiskStatus ", gPMConstants.PMELogLevel.PMLogError)
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_bIsRisksQuoted = False
            Else
                r_bIsRisksQuoted = True
            End If


        Catch excep As System.Exception

            nReturn = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:="GetAllRiskStatus", r_lFunctionReturn:=GetAllRiskStatus, excep:=excep)

        End Try
        Return nReturn
    End Function
#Region "RI Year END"
    ''' <summary>
    ''' pro rata rate = number of days (this policy) divides number of days in this year
    ''' </summary>
    ''' <param name="nProductID"></param>
    ''' <param name="dtOldStartDate"></param>
    ''' <param name="dtOldEndDate"></param>
    ''' <param name="dtStartDate"></param>
    ''' <param name="dtEndDate"></param>
    ''' <param name="o_dProRataRate"></param>
    ''' <param name="o_dtInceptionDate"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetProRataRate(ByVal nProductID As Integer, ByVal dtOldStartDate As Date, ByVal dtOldEndDate As Date, ByVal dtStartDate As Date, ByVal dtEndDate As Date, ByRef o_dProRataRate As Double, Optional ByRef o_dtInceptionDate As Date = #12/30/1899#) As Integer

        Dim nResult As Integer
        Dim oResultArray(,) As Object = Nothing

        Try

            nResult = PMEReturnCode.PMTrue

            'Check whether its a TMP
            m_oDatabase.Parameters.Clear()
            m_lReturn = m_oDatabase.Parameters.Add("product_id", CStr(nProductID), PMEParameterDirection.PMParamInput, PMEDataType.PMInteger)
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetTMPStatusSQL, sSQLName:=kGetTMPStatusName, bStoredProcedure:=False, vResultArray:=oResultArray, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If Informations.IsArray(oResultArray) Then
                Dim bIsTrueMonthlyPolicy As Boolean = False
                bIsTrueMonthlyPolicy = If(ToSafeInteger(oResultArray(1, 0)) = 1, 1, 0)
                Dim dtDate As Date = Nothing

                If bIsTrueMonthlyPolicy Then
                    dtDate = dtStartDate
                    Dim dProrata As Double
                    dProrata = 0
                    Dim nMonthCount As Integer
                    nMonthCount = Informations.DateDiff("m", dtStartDate, dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                    Dim nTotalDaysInMonth As Integer = 0

                    Do While dtDate <= dtEndDate
                        Select Case dtDate.Month
                            Case 1, 3, 5, 7, 8, 10, 12
                                nTotalDaysInMonth = 31
                            Case Else
                                nTotalDaysInMonth = Informations.DateDiff("d", dtDate, dtDate.AddMonths(1), FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                        End Select

                        Dim dtTempDate As Date = Nothing
                        Dim dtLastDateofMonth As Date = Nothing
                        dtLastDateofMonth = CDate(If(DateTime.TryParse(CStr(nTotalDaysInMonth) & "/" & CStr(dtDate.Month) & "/" & CStr(dtDate.Year), dtTempDate), dtTempDate.ToString("dd/MM/yyyy"), CStr(nTotalDaysInMonth) & "/" & CStr(dtDate.Month) & "/" & CStr(dtDate.Year)))
                        Dim nPolicyDays As Integer = 0

                        If dtDate.Month = dtEndDate.Month Then
                            ''PN 61501
                            nPolicyDays = Informations.DateDiff("d", dtDate, dtEndDate, FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
                        Else
                            nPolicyDays = Informations.DateDiff("d", dtDate, dtLastDateofMonth, FirstDayOfWeek.Sunday, FirstWeekOfYear.FirstJan1) + 1
                        End If

                        dProrata += (nPolicyDays / nTotalDaysInMonth)
                        If nMonthCount > 0 Then
                            If dtDate.Month < 12 Then
                                Dim dtTempDate2 As Date = Nothing
                                dtDate = CDate(If(DateTime.TryParse("01/" & dtDate.Month + 1 & "/" & CStr(dtDate.Year), dtTempDate2), dtTempDate2.ToString("dd/MM/yyyy"), "01/" & dtDate.Month + 1 & "/" & CStr(dtDate.Year)))
                            Else
                                Dim dtTempDate3 As Date = Nothing
                                dtDate = CDate(If(DateTime.TryParse("01/01/" & dtDate.Year + 1, dtTempDate3), dtTempDate3.ToString("dd/MM/yyyy"), "01/01/" & dtDate.Year + 1))
                            End If
                        Else : Exit Do
                        End If
                    Loop

                    o_dProRataRate = dProrata
                    Return nResult
                Else
                    Dim dtTempStartDate As Date
                    dtTempStartDate = CDate(dtStartDate).Date
                    Dim dtInceptionDate As Date
                    dtInceptionDate = o_dtInceptionDate
                    Dim dtTempEndDate As Date
                    dtTempEndDate = dtEndDate
                    Dim dtTmpDate As Date
                    dtTmpDate = o_dtInceptionDate

                    Dim nBaseLength As Integer = 0
                    nBaseLength = 365

                    If (Month(dtInceptionDate) = 2 AndAlso Day(dtInceptionDate) = 29) OrElse (Month(dtTempEndDate) = 2 AndAlso Day(dtTempEndDate) = 29) Then
                        nBaseLength = 366
                    Else
                        Do While CDate(dtTmpDate) < CDate(dtTempEndDate)
                            If (Day(dtTmpDate) = 1 AndAlso Month(dtTmpDate) <> 2) OrElse (Year(dtTmpDate) Mod 4 <> 0) Then
                                dtTmpDate = Informations.DateAdd("m", 1, dtTmpDate)
                            Else
                                dtTmpDate = Informations.DateAdd("d", 1, dtTmpDate)
                            End If

                            If Month(dtTmpDate) = 2 AndAlso Day(dtTmpDate) = 29 Then
                                nBaseLength = 366
                                Exit Do
                            End If
                        Loop
                    End If

                    Dim nPeriodLength As Integer
                    nPeriodLength = Informations.DateDiff("d", dtTempStartDate, dtTempEndDate) + 1
                    Dim dProRataRate As Double
                    dProRataRate = nPeriodLength / nBaseLength
                    o_dProRataRate = dProRataRate
                End If
            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetProRataRate Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProRataRate", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' RecalculateRI = recalculates reinsurance for portfolio transfer
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="dtTransferDate"></param>
    ''' <param name="dProRataRate"></param>
    ''' <param name="nIsPT"></param>
    ''' <param name="r_nIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Overloads Function RecalculateRI(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByVal dProRataRate As Double, ByVal nIsPT As Integer,
                                  ByRef r_nIsValid As Integer) As Integer

        m_lReturn = RecalculateRI(nInsuranceFileCnt, dtTransferDate, dProRataRate, nIsPT, r_nIsValid, False)
        Return m_lReturn

    End Function
    ''' <summary>
    ''' RecalculateRI = recalculates reinsurance for portfolio transfer
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="dtTransferDate"></param>
    ''' <param name="dProRataRate"></param>
    ''' <param name="nIsPT"></param>
    ''' <param name="r_nIsValid"></param>
    ''' <param name="bIsForAmend"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Overloads Function RecalculateRI(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByVal dProRataRate As Double, ByVal nIsPT As Integer,
                                  ByRef r_nIsValid As Integer, ByVal bIsForAmend As Boolean) As Integer

        Dim dtDetails As DataTable = Nothing
        Dim nResult As Integer

        Try
            nResult = PMEReturnCode.PMTrue
            If bIsForAmend = True Then
                m_oDatabase.Parameters.Clear()

                m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

                nResult = m_oDatabase.ExecuteDataTable(sSQL:="spu_get_policy_portfolio_log", sSQLName:="select policy for amend", bStoredProcedure:=True, oRecordset:=dtDetails)
                If nResult <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If
            End If
            'Policy is already amended once then no need to recalculate again
            If Not (dtDetails IsNot Nothing AndAlso dtDetails.Rows.Count > 0) Then
                ' set the params
                m_oDatabase.Parameters.Clear()

                m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

                m_oDatabase.Parameters.Add(sName:="dtTransfer_date", vValue:=dtTransferDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)

                m_oDatabase.Parameters.Add(sName:="dPro_rata_rate", vValue:=dProRataRate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDouble)

                m_oDatabase.Parameters.Add(sName:="nIs_PT", vValue:=nIsPT, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

                m_oDatabase.Parameters.Add(sName:="nIs_valid", vValue:=r_nIsValid, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMInteger)

                If bIsForAmend = True Then
                    m_oDatabase.Parameters.Add(sName:="nIs_Amend", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
                End If

                If m_bSkipPostings = True Then
                    m_oDatabase.Parameters.Add(sName:="nSkip_posting", vValue:=1, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)
                End If

                ' get the risk status of the risk
                nResult = m_oDatabase.SQLAction(sSQL:=kRecalculateRISQL, sSQLName:=kRecalculateRIName, bStoredProcedure:=kRecalculateRIStored)

                If nResult <> PMEReturnCode.PMTrue Then
                    Return PMEReturnCode.PMFalse
                End If

                r_nIsValid = NullToInteger(m_oDatabase.Parameters.Item("nIs_valid").Value)
            End If

            Return nResult

        Catch ex As Exception

            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="RecalculateRI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateRI", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)

            Return nResult
        End Try

    End Function
    ''' <summary>
    ''' RecalculateRIQuote = recalculates reinsurance for quotes in portfolio transfer
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="dtTransferDate"></param>
    ''' <param name="o_nIsValid"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function RecalculateRIQuote(ByVal nInsuranceFileCnt As Integer, ByVal dtTransferDate As Date, ByRef o_nIsValid As Integer) As Integer

        Dim nResult As Integer
        Try

            nResult = PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dtTransfer_date", vValue:=dtTransferDate, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nIs_valid", vValue:=o_nIsValid, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_recalculate_RI_Quote", sSQLName:=kRecalculateRIName, bStoredProcedure:=kRecalculateRIStored)

            If nResult <> PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            o_nIsValid = NullToInteger(m_oDatabase.Parameters.Item("nIs_valid").Value)

            Return nResult

        Catch ex As System.Exception
            nResult = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="RecalculateRIQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RecalculateRIQuote", vErrNo:=Informations.Err().Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' GetProductAndBranchDetails = returns product and branch details 
    ''' </summary>
    ''' <param name="dtProductDetails"></param>
    ''' <param name="dtBranchDetails"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetProductAndBranchDetails(ByRef dtProductDetails As DataTable, ByRef dtBranchDetails As DataTable) As Integer

        Dim nResult As Integer
        Dim nReturn As Integer
        Try

            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetProduct", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetBranch", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            If m_oDatabase.ExecuteDataTable(sSQL:="spu_get_all_product_and_branch_details", sSQLName:="select product", bStoredProcedure:=True, oRecordset:=dtProductDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            m_oDatabase.Parameters.Clear()
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetProduct", vValue:=0, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            nReturn = m_oDatabase.Parameters.Add(sName:="nGetBranch", vValue:=1, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If nReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If m_oDatabase.ExecuteDataTable(sSQL:="spu_get_all_product_and_branch_details", sSQLName:="select branch", bStoredProcedure:=True, oRecordset:=dtBranchDetails) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            Return nResult

        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetProductAndBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetProductAndBranchDetails", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try

    End Function
    ''' <summary>
    ''' GetClaimsPortfolioTransfer = returns claims for portfolio transfer
    ''' </summary>
    ''' <param name="nProductID"></param>
    ''' <param name="nBranchID"></param>
    ''' <param name="dtTransferDate"></param>
    ''' <param name="r_oClaimsArray"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetClaimsPortfolioTransfer(
                ByVal nProductID As Integer,
                ByVal nBranchID As Integer,
                ByVal v_dtTransferDate As Date,
                ByRef r_oClaimsArray(,) As Object) As Integer


        Dim nResult As Integer

        Try
            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            ' Add parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="nProduct_id", vValue:=nProductID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nSource_id", vValue:=nBranchID, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="transfer_date", vValue:=v_dtTransferDate, iDirection:=PMEParameterDirection.PMParamInput, iDataType:=PMEDataType.PMDate)

            ' Get policies
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=kGetClaimsPortfolioTransferSQL, sSQLName:=kGetClaimsPortfolioTransferName, bStoredProcedure:=kGetClaimsPortfolioTransferStored, vResultArray:=r_oClaimsArray, lNumberRecords:=gPMConstants.PMAllRecords)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            Return nResult
        Catch ex As Exception
            nResult = PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="GetClaimsPortfolioTransfer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetClaimsPortfolioTransfer", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' CheckRIOnClone = check for new RI  model as cloned model
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="r_bIsCloned"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function CheckRIOnClone(ByVal nInsuranceFileCnt As Integer, ByRef r_bIsCloned As Boolean) As Integer

        Dim dtResult As DataTable = Nothing
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            ' get the risk status of the risk
            m_lReturn = m_oDatabase.ExecuteDataTable(sSQL:="spu_check_ri_on_cloned", sSQLName:="spu_check_ri_on_cloned", bStoredProcedure:=True, oRecordset:=dtResult)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            If dtResult IsNot Nothing AndAlso dtResult.Rows IsNot Nothing AndAlso dtResult.Rows.Count > 0 Then
                r_bIsCloned = True
            End If

            Return nResult
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            nResult = PMEReturnCode.PMFalse
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="CheckRIOnClone Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckRIOnClone", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' ProcessSingleClaim = process claim for portfolio transfer
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nClaimId"></param>
    ''' <param name="r_sMessage"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function ProcessSingleClaim(ByVal nInsuranceFileCnt As Integer, ByVal nClaimId As Integer,
                                       ByRef r_sMessage As String) As Integer

        Dim nNewClaimId As Integer
        Dim nResult As Integer

        Try
            nResult = PMEReturnCode.PMTrue

            m_lReturn = m_oDatabase.SQLBeginTrans

            If m_lReturn <> PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to Begin SQLTransaction"
                Throw New ApplicationException(r_sMessage)
            End If

            m_lReturn = ClaimReversingPrePolicyTransfer(v_lInsuranceFileCnt:=nInsuranceFileCnt, nClaimId:=nClaimId,
                                                        nNewClaimId:=nNewClaimId)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to run ClaimReversingPrePolicyTransfer " & nInsuranceFileCnt.ToString
                Throw New ApplicationException(r_sMessage)
            End If

            m_lReturn = TransferClaimToNewRisk(v_lInsuranceFileCnt:=nInsuranceFileCnt, nClaimId:=nNewClaimId,
                                               r_sMessage:=r_sMessage)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to run TransferClaimToNewRisk " & nInsuranceFileCnt.ToString
                Throw New ApplicationException(r_sMessage)
            End If

            ' Save all transactions to database
            m_lReturn = m_oDatabase.SQLCommitTrans
            If m_lReturn <> PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to commit SQLTransaction"
                Throw New ApplicationException(r_sMessage)
            End If

            Return nResult

        Catch ex As System.Exception
            ' Roll back all transactions as one of the step has failed
            m_lReturn = m_oDatabase.SQLRollbackTrans
            If m_lReturn <> PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to Rollback SQLTransaction"
            End If

            nResult = PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:=If(r_sMessage = "", "Failed ProcessSingleClaim()", r_sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSingleClaim", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
            Return nResult
        End Try
    End Function
    ''' <summary>
    ''' CreateClaimVersionForPT = process claim for portfolio transfer
    ''' </summary>
    ''' <param name="nInsuranceFileCnt"></param>
    ''' <param name="nClaimId"></param>
    ''' <param name="nNewClaimId"></param>
    ''' <param name="r_nStatsFolderCnt"></param>
    ''' <param name="nIsPreTransfer"></param>
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function CreateClaimVersionForPT(ByVal nInsuranceFileCnt As Integer, ByVal nClaimId As Integer,
                                            ByRef nNewClaimId As Integer, ByRef r_nStatsFolderCnt As Integer,
                                            ByVal nIsPreTransfer As Integer) As Integer

        Dim nResult As Integer

        Try
            nResult = PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nInsurance_file_cnt", vValue:=nInsuranceFileCnt,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nClaim_id", vValue:=nClaimId,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nPrePortfolioTransfer", vValue:=nIsPreTransfer,
                                                   iDirection:=PMEParameterDirection.PMParamInput,
                                                   iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nNew_claim_id", vValue:=Nothing,
                                                   iDirection:=PMEParameterDirection.PMParamOutput,
                                                   iDataType:=PMEDataType.PMInteger)

            m_lReturn = m_oDatabase.Parameters.Add(sName:="nStatsFolderCnt", vValue:=Nothing,
                                                   iDirection:=PMEParameterDirection.PMParamOutput,
                                                   iDataType:=PMEDataType.PMInteger)

            ' get the risk status of the risk
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_create_claim_portfolio_transfer_version",
                                              sSQLName:="CreateClaimVersionForPT", bStoredProcedure:=True)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            nNewClaimId = NullToLong(m_oDatabase.Parameters.Item("nNew_claim_id").Value)
            r_nStatsFolderCnt = NullToLong(m_oDatabase.Parameters.Item("nStatsFolderCnt").Value)

        Catch ex As Exception
            nResult = PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed CreateClaimVersionForPT()", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateClaimVersionForPT", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)

        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' GetPolicyListDetails = gives list of policies to amend
    ''' </summary>
    ''' <param name="r_oPoliciesDetails"></param>   
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetPolicyListDetails(ByRef r_oPoliciesDetails(,) As Object) As Integer
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue

            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.SQLSelect(sSQL:="spu_get_portfolio_policylist", sSQLName:="select policies", bStoredProcedure:=True, vResultArray:=r_oPoliciesDetails, lNumberRecords:=gPMConstants.PMAllRecords)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

        Catch ex As System.Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed GetPolicyListDetails()", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyListDetails", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' GetPortfolioTransferDate = gives transfer date for portfolio process
    ''' </summary>
    ''' <param name="r_dtTransferDate"></param>   
    ''' <returns>Integer</returns>
    ''' <remarks></remarks>
    Public Function GetPortfolioTransferDate(ByRef r_dtTransferDate As Date) As Integer
        Dim nResult As Integer
        Try
            nResult = PMEReturnCode.PMTrue
            ' set the params
            m_oDatabase.Parameters.Clear()

            m_lReturn = m_oDatabase.Parameters.Add(sName:="dtTransfer_date", vValue:=Nothing, iDirection:=PMEParameterDirection.PMParamOutput, iDataType:=PMEDataType.PMDate)
            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If
            ' get the risk status of the risk
            m_lReturn = m_oDatabase.SQLAction(sSQL:="spu_Get_Portfolio_Transfer_Date", sSQLName:="GetPortfolioTransferDate", bStoredProcedure:=True)

            If m_lReturn <> PMEReturnCode.PMTrue Then
                Return PMEReturnCode.PMFalse
            End If

            r_dtTransferDate = NullToDate(m_oDatabase.Parameters.Item("dtTransfer_date").Value)

        Catch ex As System.Exception
            ' DO Not Call any functions before here or the error will be lost
            ' Error.
            nResult = PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=PMELogLevel.PMLogOnError, sMsg:="Failed GetPortfolioTransferDate()", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPortfolioTransferDate", vErrNo:=Informations.Err.Number, vErrDesc:=ex.Message, excep:=ex)
        End Try
        Return nResult
    End Function
#End Region

    ''' <summary>
    ''' GetPoliciesPortfolioTransferRI2007Off
    ''' </summary>
    ''' <param name="v_nProductID"></param>
    ''' <param name="v_dtTransferDate"></param>
    ''' <param name="r_oPolicyArray"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetPoliciesPortfolioTransferRI2007Off(ByVal v_nProductID As Integer, ByVal v_dtTransferDate As Date, ByRef r_oPolicyArray(,) As Object) As Integer

        Dim nResult As Integer = 0
        Try

            m_oDatabase.Parameters.Clear()

            ' Add parameters

            nResult = m_oDatabase.Parameters.Add(sName:="product_id", vValue:=v_nProductID, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If


            nResult = m_oDatabase.Parameters.Add(sName:="transfer_date", vValue:=CDate(v_dtTransferDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            ' Get policies

            nResult = m_oDatabase.SQLSelect(sSQL:=kGetPoliciesForPortfolioTransferRI2007DisabledSQL, sSQLName:=kGetPoliciesForPortfolioTransferRI2007DisabledName, bStoredProcedure:=kGetPoliciesForPortfolioTransferRI2007DisabledStored, vResultArray:=r_oPolicyArray)

            If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                Return nResult
            End If

            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception

            nResult = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPoliciesPortfolioTransfer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPoliciesPortfolioTransfer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return nResult
        End Try
    End Function
    ' ''' <summary>
    ' ''' ProcessSinglePolicy
    ' ''' </summary>
    ' ''' <param name="v_nInsuranceFileCnt"></param>
    ' ''' <param name="v_dtTransferDate"></param>
    ' ''' <param name="r_sMessage"></param>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    'Public Function ProcessSinglePolicyRI2007OFF(ByVal v_nInsuranceFileCnt As Integer, ByVal v_dtTransferDate As Date, ByRef r_sMessage As String) As Integer

    '    Dim nReturnValue As gPMConstants.PMEReturnCode
    '    Dim nNewInsurancefileCnt As Integer
    '    Dim nInsuranceFolderCnt As Integer
    '    Dim dtPolicyStartDate As Date
    '    Dim sDescription As String = ""
    '    Dim vInsuranceFileTax As Object
    '    Dim sTransactionType As String = ""
    '    Dim sMessage As String=""
    '    Dim sInsuranceRef As String

    '    Try

    '        nReturnValue = gPMConstants.PMEReturnCode.PMTrue

    '        sTransactionType = "PT"

    '        ' Set transaction so we can roll back if something gone wrong

    '        If m_oDatabase.SQLBeginTrans() <> gPMConstants.PMEReturnCode.PMTrue Then
    '            nReturnValue = gPMConstants.PMEReturnCode.PMFalse
    '            r_sMessage = "Failed to begin SQLTransaction"
    '            Return nReturnValue
    '        End If

    '        '****************** Create new version of policy ************************

    '        ' Assign current insurance file count to business object

    '        m_oInsuranceFile.InsuranceFileCnt = v_nInsuranceFileCnt

    '        ' Get details of current policy

    '        If m_oInsuranceFile.GetDetails() <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to get policy details for " & v_nInsuranceFileCnt
    '            Return nReturnValue
    '        End If

    '        ' Set old version to "replaced portfolio transfer"

    '        m_oInsuranceFile.InsuranceFileStatus = gSIRLibrary.SIRInsFileStatusReplacedPT

    '        m_oInsuranceFile.UpdatePolicy()

    '        m_oInsuranceFile.InsuranceFileStatus = Nothing   'set policy status to live

    '        m_oInsuranceFile.EventDescription = "Policy copied for portfolio transfer"

    '        m_oInsuranceFile.PolicyVersion += 1

    '        m_oInsuranceFile.LastTransType = sTransactionType

    '        m_oInsuranceFile.CoverStartDate = v_dtTransferDate

    '        ' E007 Changes : Reversal of claims
    '        m_vClaimsArray = Nothing
    '        m_lReturn = ClaimReversingPrePolicyTransfer(v_nInsuranceFileCnt:=cint(v_nInsuranceFileCnt))
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to run ClaimReversingPrePolicyTransfer " & v_nInsuranceFileCnt
    '            Return nReturnValue
    '        End If

    '        ' create new version of policy based on current policy details

    '        If m_oInsuranceFile.CreatePolicy() <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to create new version of policy for " & v_nInsuranceFileCnt
    '            Return nReturnValue
    '        End If

    '        '******************** Get new policy version details ******************************************

    '        nNewInsurancefileCnt = m_oInsuranceFile.InsuranceFileCnt

    '        nInsuranceFolderCnt = m_oInsuranceFile.InsuranceFolderCnt 'note folder will be the same as previous policy version

    '        dtPolicyStartDate = m_oInsuranceFile.CoverStartDate

    '        sInsuranceRef = m_oInsuranceFile.InsuranceRef

    '        '****************** Copy related data to new policy version ***********************
    '        ' Copy standard wording

    '        If m_oRenSelection.CopyPolicyStandardWordings(v_lOldInsuranceFileCnt:=cint(v_nInsuranceFileCnt), v_lNewInsuranceFileCnt:=cint(nNewInsurancefileCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to copy standard wording to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '            Return nReturnValue
    '        End If

    '        ' Copy coinsurance

    '        If m_oRenSelection.CopyCoinsurance(v_lCurrentInsFileCnt:=cint(v_nInsuranceFileCnt), v_lNewInsFileCnt:=cint(nNewInsurancefileCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to copy coinsurance details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '            Return nReturnValue
    '        End If

    '        ' Copy agent commission

    '        If m_oRenSelection.CopyAgentCommission(v_lCurrentInsFileCnt:=cint(v_nInsuranceFileCnt), v_lNewInsFileCnt:=cint(nNewInsurancefileCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to copy agent commission details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '            Return nReturnValue
    '        End If

    '        ' Copy agents

    '        If m_oRenSelection.CopyInsuranceFileAgent(v_lCurrentInsFileCnt:=cint(v_nInsuranceFileCnt), v_lNewInsFileCnt:=cint(nNewInsurancefileCnt)) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to copy agent details to new policy version (current policy :" & v_nInsuranceFileCnt & " new policy " & CStr(nNewInsurancefileCnt) & " )"
    '            Return nReturnValue
    '        End If

    '        ' Copy risk details (error messages are dealt within CopyAllRisk())
    '        If CopyAllRisk(v_nInsuranceFileCnt:=cint(v_nInsuranceFileCnt), v_lNewInsuranceFileCnt:=cint(nNewInsurancefileCnt), v_lInsuranceFolderCnt:=nInsuranceFolderCnt, v_dtPolicyStartDate:=dtPolicyStartDate, v_sTransactionType:=sTransactionType, r_sMessage:=r_sMessage, v_dtTransferDate:=cdate(v_dtTransferDate), v_sInsuranceRef:=sInsuranceRef) <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return nReturnValue
    '        End If

    '        ' Process stats (error messages are dealt within CreateAndPostStats())

    '        If m_oInsuranceFile.InsuranceFileType = "POLICY" Or m_oInsuranceFile.InsuranceFileType = "MTA PERM" Or m_oInsuranceFile.InsuranceFileType = "MTA TEMP" Then
    '            If CreateAndPostStats(v_nInsuranceFileCnt:=cint(nNewInsurancefileCnt), v_sTransactionType:=sTransactionType, v_lPTInsuranceFileCnt:=cint(v_nInsuranceFileCnt), v_bReversePT:=m_oInsuranceFile.InsuranceFileType <> "POLICY", r_sMessage:=r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then
    '                Return nReturnValue
    '            End If
    '        End If

    '        ' E007 Changes
    '        m_lReturn = TransferClaimToNewRisk(v_nInsuranceFileCnt:=cint(v_nInsuranceFileCnt), _
    '                                v_lNewInsuranceFileCnt:=cint(nNewInsurancefileCnt), _
    '                                r_sMessage:=sMessage)
    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            Return nReturnValue
    '        End If

    '        ' Save all transactions to database

    '        m_lReturn = m_oDatabase.SQLCommitTrans

    '        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '            r_sMessage = "Failed to commit SQLTransaction"
    '            Return nReturnValue
    '        End If
    '        Return nReturnValue
    '    Catch ex As Exception

    '        ' Roll back all transactions as one of the step has failed

    '        nReturnValue = m_oDatabase.SQLRollbackTrans
    '        nReturnValue = gPMConstants.PMEReturnCode.PMFalse

    '        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=if(r_sMessage = "", "Failed ProcessSinglePolicy()", r_sMessage), vApp:=ACApp, vClass:=ACClass, vMethod:="ProcessSinglePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)

    '    End Try

    '    Return nReturnValue

    'End Function
    Private Function ClaimReversingPrePolicyTransfer(ByVal v_nInsuranceFileCnt As Integer) As Integer

        Const kMethodName As String = "ClaimReversingPrePolicyTransfer"

        Dim nReturnValue As Integer
        Dim m_oFindClaim As Object = Nothing 
        Dim nNewClaimId As object=0
        Dim nClaimsCount As Integer

        Try

            nReturnValue = gPMConstants.PMEReturnCode.PMTrue

            ' Create Object bCLMFindClaim
            nReturnValue = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oFindClaim,
                                                                v_sClassName:="bCLMFindClaim.Business",
                                                                v_sCallingAppName:=ACApp,
                                                                v_sUsername:=m_sUsername,
                                                                v_sPassword:=m_sPassword,
                                                                v_iUserID:=m_iUserID,
                                                                v_iSourceID:=m_iSourceID,
                                                                v_iLanguageID:=m_iLanguageID,
                                                                v_iCurrencyID:=m_iCurrencyID,
                                                                v_iLogLevel:=m_iLogLevel,
                                                                v_oDatabase:=m_oDatabase)
            If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)

            End If

            If GetAllClaimsOnPolicy(v_lInsuranceFileCnt:=v_nInsuranceFileCnt,
                                    r_vResultArray:=m_vClaimsArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)

            End If

            ' Process all claims
            If Informations.IsArray(m_vClaimsArray) Then
                For nClaimsCount = 0 To DirectCast(m_vClaimsArray, Object(,)).GetUpperBound(1)
                    ' Copy the claim by passing MAX ClaimId of Base Claim
                    ' Question : Do we need to check Remaining Reservev before processing claims ?
                    nReturnValue = m_oFindClaim.SetProcessModes(vTransactionType:="C_CR")
                    nReturnValue = m_oFindClaim.ProcessCopyClaim(v_lClaimId:=m_vClaimsArray(1, nClaimsCount), r_lCopyClaimId:=nNewClaimId)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "ProcessCopyClaim", "Failed to Copy claim for Claim Id " & m_vClaimsArray(0, lClaimsCount)
                        RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)

                    End If
                    If nNewClaimId > 0 Then
                        m_vClaimsArray(4, nClaimsCount) = nNewClaimId
                    End If

                    ' No need to MoveClaimToNewRisk
                    'Process Claim Perils and Reserves
                    nReturnValue = ProcessClaimPerils(v_lclaimCnt:=nNewClaimId,
                                                    v_iPrePortfolioTransfer:=1,
                                                    v_lInsuranceFileCnt:=v_nInsuranceFileCnt,
                                                    v_iIsCreated:=0)
                    If nReturnValue <> gPMConstants.PMEReturnCode.PMTrue Then
                        'RaiseError "ProcessClaimPerils", "Failed to NetOf Perils for Claim" & m_vClaimsArray(0, lClaimsCount)

                        RaiseError(kMethodName, "Failed ClaimReversingPrePolicyTransfer ", gPMConstants.PMELogLevel.PMLogError)
                    End If
                Next
            End If

        Catch excep As System.Exception
            'DO Not Call any functions before here or the error will be lost
            Return gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogError(v_sUsername:=m_sUsername,
                v_sClass:=ACClass,
                v_sMethod:="GetAllClaimsOnPolicy",
                r_lFunctionReturn:=ClaimReversingPrePolicyTransfer)

        End Try
        Return nReturnValue
    End Function
End Class

