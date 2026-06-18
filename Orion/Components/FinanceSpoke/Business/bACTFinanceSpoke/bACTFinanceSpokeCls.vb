Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Data.SqlClient
Imports System.Diagnostics
'Developer Guide no.129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("Business_NET.Business")> _
Public NotInheritable Class Business
    Implements IDisposable
    Implements SSP.S4I.Interfaces.IBusiness

    ' ***************************************************************** '
    ' Edit History:
    ' RAW 21/11/2002 : PS005 : Add export for party loyalty scheme
    ' ***************************************************************** '

    ' ************************************************
    ' Added to replace global variables 10/12/2003
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

    ' Error Code (Private)
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    ' Account Transaction Posting Variables.
    Private m_oAllocateManual As Object
    Private m_oDocumentPost As bACTDocumentPost.Form
    Private m_oAutoNumber As bACTAutoNumber.Business
    Private m_oTransDetail As Object
    Private m_oCurrencyConvert As bACTCurrencyConvert.Form

    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    'developer guide no. 39
    Private Const ACCreateBatchRecordSQL As String = "spu_ACT_Add_Batch"
    Private Const ACCreateBatchRecordName As String = "AddNewBatchRecord"

    'developer guide no. 39
    Private Const ACUpdateBatchRefSQL As String = "spu_ACT_Update_Batch_Ref"
    Private Const ACUpdateBatchRefName As String = "UpdateBatchRef"

    Public ReadOnly Property PMProductFamily() As Integer
        Get

            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions

        End Get
    End Property
    ''' <summary>
    ''' Description: Produces an export of transactions to go to the staging area. Create to overload for Batch process. 
    ''' </summary>
    ''' <param name="v_sInterfaceCode"></param>
    ''' <param name="r_sBatchRef"></param>
    ''' <param name="r_sStatusCode"></param>
    ''' <param name="r_sMessage"></param>
    ''' <param name="r_sHeaderXML"></param>
    ''' <param name="r_vHeaderData"></param>
    ''' <param name="r_vDetailData"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Export(ByVal v_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, _
                           ByRef r_sMessage As String, ByRef r_sHeaderXML As String, _
                           ByRef r_vHeaderData As Object, ByRef r_vDetailData As Object) As Integer

        Dim nResult As Integer = gPMConstants.PMEReturnCode.PMTrue

        nResult = Export(v_sInterfaceCode:=v_sInterfaceCode, r_sBatchRef:=r_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_sHeaderXML:=r_sHeaderXML, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData, bCreateBatch:=False)
        'If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
        '    nResult = gPMConstants.PMEReturnCode.PMFalse
        '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Export Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
        'End If
        Return nResult
    End Function
    ' ***************************************************************** '
    '
    ' Name: Export
    '
    ' Description: Produces an export of transactions to go to the
    '              staging area.
    '
    ' History: 23/10/2002 SJP - Created.
    '          28/10/2002 SJP - Amended as per tech spec 208 version 0.09
    '          11/11/2002 PWC - Amended for Front Office Receipting
    '          14/01/2003 PW  - Add Credit Control Export (PS218)
    '          18/03/2003 SW  - Changed BatchRef to ByRef as this is
    '                           returned from Sirius for exports
    '          DD 24/09/2003: Put in code to smooth COM+ upgrade
    ' ***************************************************************** '


    Public Function Export(ByVal v_sInterfaceCode As String, ByRef r_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData As Object, ByRef r_vDetailData As Object, ByVal bCreateBatch As Boolean) As Integer

        Dim result As Integer = 0
        Dim oExportAutoBank As ExportAutoBank
        Dim oExport3rdPartyCollect As Export3rdPartyCollect
        Dim oExportExtractTrans As ExportExtractTrans
        Dim oExportLoyaltyScheme As ExportLoyaltyScheme ' RAW 21/11/2002 : PS005 : Added
        Dim oPaymentRun As ExportPaymentRun
        Dim oExportSweepBalances As ExportSweepBalances
        Dim oExportStaleCheques As ExportStaleCheques
        Dim oExportChequeReminder As ExportChequeReminder
        Dim oExportCreditBalance As ExportCreditBalance
        Dim oExportRecurring As ExportRecurring
        Dim oExportOneOffReceipts As ExportOneOffReceipts

        ' PW140302 (PS218)
        Dim oExportCreditControl As ExportCreditControl
        'sw 01/04/2003
        Dim oExportGeneralLedger As ExportGeneralLedger
        Dim oExportChaseCycle As ExportChaseCycle

        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".Export")

            'Initialise return
            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case v_sInterfaceCode.ToUpper()
                Case gHUBSpokeConstants.ksICAutoBank
                    oExportAutoBank = New ExportAutoBank()
                    With oExportAutoBank
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportAutoBank.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportAutoBank = Nothing
                            Return result
                        End If

                        'Do the Export processing
                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound

                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportAutoBank.Start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportAutoBank = Nothing
                            Return result
                        End If

                        oExportAutoBank = Nothing
                        'Return to Caller
                        Return result
                    End With

                Case gHUBSpokeConstants.ksIC3rdPartyCollect
                    oExport3rdPartyCollect = New Export3rdPartyCollect()
                    With oExport3rdPartyCollect
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExport3rdPartyCollect.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExport3rdPartyCollect = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_vHeaderData:=r_vHeaderData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExport3rdPartyCollect.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExport3rdPartyCollect = Nothing
                            Return result
                        End If

                    End With

                    oExport3rdPartyCollect = Nothing
                    'Return to Caller
                    Return result

                Case gHUBSpokeConstants.ksICExtractTrans
                    oExportExtractTrans = New ExportExtractTrans()
                    With oExportExtractTrans
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportExtractTrans.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportExtractTrans = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportExtractTrans.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportExtractTrans = Nothing
                            Return result
                        End If

                    End With

                    oExportExtractTrans = Nothing
                    'Return to Caller
                    Return result

                    ' RAW 21/11/2002 : PS005 : Added
                Case gHUBSpokeConstants.ksICPartyLoyaltyScheme
                    oExportLoyaltyScheme = New ExportLoyaltyScheme()
                    With oExportLoyaltyScheme
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportLoyaltyScheme.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportLoyaltyScheme = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportLoyaltyScheme.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportLoyaltyScheme = Nothing
                            Return result
                        End If

                    End With

                    oExportLoyaltyScheme = Nothing
                    'Return to Caller
                    Return result

                    ' PW140103 (PS218) - add Chase Cycle export
                Case gHUBSpokeConstants.ksICChaseCycle
                    oExportChaseCycle = New ExportChaseCycle()
                    With oExportChaseCycle
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportChaseCycle.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportChaseCycle = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportChaseCycle.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportChaseCycle = Nothing
                            Return result
                        End If

                        oExportChaseCycle = Nothing
                    End With
                    'Return to Caller
                    Return result

                    ' PW140103 (PS218) - add credit control export
                Case gHUBSpokeConstants.ksICCreditControl
                    oExportCreditControl = New ExportCreditControl()
                    With oExportCreditControl
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportCreditControl.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportCreditControl = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData, bCreateBatch:=bCreateBatch), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportCreditControl.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportCreditControl = Nothing
                            Return result
                        End If

                        oExportCreditControl = Nothing
                        'Return to Caller
                        Return result
                    End With

                Case gHUBSpokeConstants.ksICRejections
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'RKC 29/11/2002
                    'Set Return Status Code and Message for Batch Export
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_INCORRECT_EXPORT_INTERFACE
                    Return result

                Case gHUBSpokeConstants.ksICPaymentRun
                    oPaymentRun = New ExportPaymentRun()
                    With oPaymentRun
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oPaymentRun.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oPaymentRun = Nothing
                            Return result
                        End If

                        'Do the Export processing


                        m_lReturn = CType(.Start(v_sBatchRef:=r_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oPaymentRun.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oPaymentRun = Nothing
                            Return result
                        End If

                    End With

                    oPaymentRun = Nothing
                    'Return to Caller
                    Return result

                Case gHUBSpokeConstants.ksICSweepBalances
                    oExportSweepBalances = New ExportSweepBalances()
                    With oExportSweepBalances
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportSweepBalances.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportSweepBalances = Nothing
                            Return result
                        End If

                        'Do the Export processing
                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage), gPMConstants.PMEReturnCode)


                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportSweepBalances.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportSweepBalances = Nothing
                            Return result
                        End If

                    End With

                    oExportSweepBalances = Nothing
                    'Return to Caller
                    Return result

                    'sw 07/02/2003
                Case gHUBSpokeConstants.ksICStaleCheques
                    oExportStaleCheques = New ExportStaleCheques()
                    With oExportStaleCheques
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportStaleCheques.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportStaleCheques = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, v_vHeaderData:=r_vHeaderData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportSweepBalances.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportStaleCheques = Nothing
                            Return result
                        End If

                    End With

                    oExportStaleCheques = Nothing
                    'Return to Caller
                    Return result

                    'sw 07/02/2003
                Case gHUBSpokeConstants.ksICChequeReminder
                    oExportChequeReminder = New ExportChequeReminder()
                    With oExportChequeReminder
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportChequeReminder.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportChequeReminder = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, v_vHeaderData:=r_vHeaderData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportChequeReminder.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportChequeReminder = Nothing
                            Return result
                        End If

                    End With

                    oExportChequeReminder = Nothing
                    'Return to Caller
                    Return result

                    'DD 20/02/2003 (TS220)
                Case gHUBSpokeConstants.ksICCreditBalance
                    oExportCreditBalance = New ExportCreditBalance()
                    With oExportCreditBalance
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportCreditBalance.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportCreditBalance = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, v_vHeaderData:=r_vHeaderData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportCreditBalance.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportCreditBalance = Nothing
                            Return result
                        End If

                    End With

                    oExportCreditBalance = Nothing
                    'Return to Caller
                    Return result


                Case gHUBSpokeConstants.ksICRecurring
                    oExportRecurring = New ExportRecurring()
                    With oExportRecurring
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportRecurring.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportRecurring = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        result = .Start(r_sInterfaceCode:=v_sInterfaceCode, r_sBatchRef:=r_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_sHeaderXML:=r_sHeaderXML, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData)

                        ' RAM20040408 : Code changes related to CQ4807. Return the status
                        '                   to the calling application, rather than setting
                        '                   it here and log error
                        '                If m_lReturn = PMNotFound Then
                        '                    Export = PMNotFound
                        '                ElseIf m_lReturn <> PMTrue Then
                        '                    Export = PMFalse
                        '                    ' RAW 24/02/2004 : CQ4106 : added
                        '                    LogMessage m_sUsername, iType:=PMLogError, _
                        ''                        sMsg:="oExportRecurring.start call failed - " & r_sMessage, _
                        ''                        vApp:=ACApp, vClass:=ACClass, _
                        ''                        vMethod:="Export"
                        '                    Set oExportRecurring = Nothing
                        '                    Exit Function
                        '                End If

                    End With

                    oExportRecurring = Nothing
                    'Return to Caller
                    Return result

                Case gHUBSpokeConstants.ksICOneOff
                    oExportOneOffReceipts = New ExportOneOffReceipts()
                    With oExportOneOffReceipts
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportOneOffReceipts.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportOneOffReceipts = Nothing
                            Return result
                        End If

                        'Do the Export processing

                        m_lReturn = CType(.Start(r_sInterfaceCode:=v_sInterfaceCode, r_sBatchRef:=r_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_sHeaderXML:=r_sHeaderXML, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportOneOffReceipts.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportOneOffReceipts = Nothing
                            Return result
                        End If

                    End With

                    oExportOneOffReceipts = Nothing
                    'Return to Caller
                    Return result

                Case gHUBSpokeConstants.ksICGeneralLedger

                    oExportGeneralLedger = New ExportGeneralLedger()

                    With oExportGeneralLedger
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportGeneralLedger.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportGeneralLedger = Nothing
                            Return result
                        End If

                        m_lReturn = CType(.Start(v_sInterfaceCode:=v_sInterfaceCode, v_sBatchRef:=r_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, v_sHeaderXML:=r_sHeaderXML, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData), gPMConstants.PMEReturnCode)

                        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                            result = gPMConstants.PMEReturnCode.PMNotFound
                        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oExportGeneralLedger.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Export")
                            oExportGeneralLedger = Nothing
                            Return result
                        End If

                    End With

                    oExportGeneralLedger = Nothing
                    Return result

            End Select

            'RKC 25/11/2002
            'Set Return Status Code and Message for Batch Import
            'We Have a Succesful Export
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".Export")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Export Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Export", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RKC 25/11/2002
            'Set Return Status Code and Message for Batch Export
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
            Return result
            '? Why Resume Here



            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ReExport
    '
    ' Description: Run when staging area Batch needs to be re-run
    '
    ' History: 23/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function ReExport(ByVal v_sInterfaceCode As String, ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByRef r_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object) As Integer

        Dim result As Integer = 0
        Dim lBatchID As Integer
        Dim dTotalAmount As Double

        Dim sSQLWhere, sSQLUpdate, sSQLSelectFields As String

        Dim vBatchId, vResults As Object
        Dim vDetailData(,) As Object
        Dim vXMLObject As Object

        Dim oReExportRecurring As ExportRecurring
        Dim oReExportOneOff As ExportOneOffReceipts

        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".ReExport")

            ' Initialise Variables
            result = gPMConstants.PMEReturnCode.PMTrue
            dTotalAmount = 0
            lBatchID = 0

            sSQLSelectFields = "SELECT batch_id, xml_object"

            m_lReturn = CType(GetBatchRecord(v_sBatchRef:=v_sBatchRef, r_vBatchResults:=vBatchId, v_sSelectFields:=sSQLSelectFields), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' RAW 24/02/2004 : CQ4106 : added
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetBatchRecord call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                'RKC 25/11/2002
                'Set Return Status Code and Message for Batch Export
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
                Return result
            End If

            'Batch Id contained in first column of results array

            lBatchID = CInt(vBatchId(0, 0))
            'XML Object contained in second column of results array


            vXMLObject = vBatchId(1, 0)

            'Retrieve all relevant records against the retrieved batch id value.
            Select Case v_sInterfaceCode.ToUpper()
                Case gHUBSpokeConstants.ksICRecurring
                    sSQLWhere = "WHERE [PFI].batch_id = " & lBatchID
                    oReExportRecurring = New ExportRecurring()

                    With oReExportRecurring

                        'Pass through the DB Connection
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oReExportRecurring.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                            oReExportRecurring = Nothing
                            Return result
                        End If

                        m_lReturn = CType(.RetrieveRecords(v_sWhereClause:=sSQLWhere, r_vResults:=vResults, v_bGroupRecords:=True), gPMConstants.PMEReturnCode)

                        ' RAW 24/02/2004 : CQ4106 : added
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oReExportRecurring.RetrieveRecords call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                        End If


                    End With

                    'SMJB CQ2515 11/09/03: Moved inside the if as the array bounds should be different for ONEOFF
                    'redeclare the detail array to the size defined by the interface type code and the number of records

                    vDetailData = Array.CreateInstance(GetType(Object), New Integer() {gHUBSpokeConstants.eddPFPlanTransactionID - gHUBSpokeConstants.eddDetailId + 1, vResults.GetUpperBound(conRows - 1) + 1}, New Integer() {gHUBSpokeConstants.eddDetailId, 0})

                Case gHUBSpokeConstants.ksICOneOff
                    sSQLWhere = "WHERE [CLI].batch_id = " & lBatchID
                    oReExportOneOff = New ExportOneOffReceipts()

                    With oReExportOneOff

                        'Pass through the DB Connection
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oReExportOneOff.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                            oReExportOneOff = Nothing
                            Return result
                        End If

                        m_lReturn = CType(.RetrieveRecords(v_sWhereClause:=sSQLWhere, r_vResults:=vResults, v_bGroupRecords:=False), gPMConstants.PMEReturnCode)

                        ' RAW 24/02/2004 : CQ4106 : added
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oReExportOneOff.RetrieveRecords call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                        End If
                    End With
                    'SMJB CQ2515 11/09/03: Redeclare the detail array to the size defined by
                    'the interface type code and the number of records

                    vDetailData = Array.CreateInstance(GetType(Object), New Integer() {gHUBSpokeConstants.eddAgent - gHUBSpokeConstants.eddDetailId + 1, vResults.GetUpperBound(conRows - 1) + 1}, New Integer() {gHUBSpokeConstants.eddDetailId, 0})

                Case gHUBSpokeConstants.ksICRejections
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'RKC 29/11/2002
                    'Set Return Status Code and Message for Batch Export
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_INCORRECT_EXPORT_INTERFACE
                    Return result

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'RKC 29/11/2002
                    'Set Return Status Code and Message for Batch Export
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_UNKNOWN_INTERFACE
                    Return result
            End Select


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                'RKC 25/11/2002
                'Set Return Status Code and Message for Batch Export
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
                Return result
            End If



            For iLoop As Integer = 0 To vResults.GetUpperBound(conRows - 1)

                m_lReturn = CType(PopulateDetailArray(v_sInterfaceCode:=v_sInterfaceCode, r_vDetailData:=vDetailData, v_vResults:=vResults, v_iElementNumber:=iLoop), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Debug.WriteLine(VB6.TabLayout("Error while populating detail array", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Fatal Error"))
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="PopulateDetailArray call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                    'RKC 25/11/2002
                    'Set Return Status Code and Message for Batch Export
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
                    Return result
                End If

                If v_sInterfaceCode.ToUpper() = gHUBSpokeConstants.ksICRecurring.ToUpper() Then

                    dTotalAmount += CDbl(vResults(rcpPFIInstalmentAmount, iLoop))
                ElseIf v_sInterfaceCode.ToUpper() = gHUBSpokeConstants.ksICOneOff.ToUpper() Then

                    dTotalAmount += CDbl(vResults(oopCashListItemAmount, iLoop))
                End If
            Next

            'Set the Batch re-export date value for the record.
            sSQLUpdate = "UPDATE Batch" & Strings.Chr(13) & Strings.Chr(10)
            sSQLUpdate = sSQLUpdate & "SET reexport_date = '" & DateTime.Today.ToString("yyyy/MM/dd") & "'" & Strings.Chr(13) & Strings.Chr(10)
            sSQLUpdate = sSQLUpdate & "WHERE Batch_Id = " & CStr(lBatchID)

            m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQLUpdate, sSQLName:="Batch Update", bStoredProcedure:=False)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Debug.WriteLine(VB6.TabLayout("Error while updating reexport date in Batch table", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Fatal Error"))
                result = gPMConstants.PMEReturnCode.PMFalse
                ' RAW 24/02/2004 : CQ4106 : added
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update reexport date in batch table", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport")
                'RKC 25/11/2002
                'Set Return Status Code and Message for Batch Export
                r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
                r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
                Return result
            End If

            'redeclare the detail data array so that is can hold the return values
            r_vDetailData = ArraysHelper.RedimPreserve(Of Object())(r_vDetailData, New Integer() {conValue - conName + 1}, New Integer() {conName})

            'Pass back the completed detail data array

            r_vDetailData(conValue) = vDetailData

            'Update Batch Amount in Header Data Array

            r_vHeaderData(conValue)(ehdBatchAmount) = dTotalAmount

            'Set the r_sHeaderXML to the xml_object from the batch record.
            '  r_sHeaderXML = vXMLObject

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".ReExport")

            'RKC 25/11/2002
            'Set Return Status Code and Message for Batch Import
            'We Have a Succesful Import
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_COMPLETE
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".ReExport")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReExport Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReExport", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RKC 25/11/2002
            'Set Return Status Code and Message for Batch Export
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_EXPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_EXPORT_FAILED
            Return result




            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Import
    '
    ' Description: Process the rejections or closes the batch
    '
    ' History: 23/10/2002 SJP - Created.
    '
    ' ***************************************************************** '
    Public Function Import(ByVal v_sInterfaceCode As String, ByVal v_sBatchRef As String, ByRef r_sStatusCode As String, ByRef r_sMessage As String, ByVal v_sHeaderXML As String, ByRef r_vHeaderData() As Object, ByRef r_vDetailData() As Object) As Integer

        Dim result As Integer = 0
        Dim sSQLSelectFields, sBatchInterfaceCode As String
        Dim lBatchID, lInterfaceId As Integer
        Dim vBatchId As Object
        Dim oRejections As ImportRejections
        Dim oElecReceipt As ImportElecReceipting
        Dim oImportBankStatement As ImportBankStatement
        Dim oImportCloseBatch As ImportCloseBatch

        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".Import")

            ' Initialise Variables
            result = gPMConstants.PMEReturnCode.PMTrue
            lBatchID = 0
            lInterfaceId = 0


            sBatchInterfaceCode = ""

            '****************************************************************************************************************************************
            ' Get the Batch ID record for supplied batch ref and also store the transaction type stored in interface id
            ' FOR REJECTIONS AND CLOSE BATCH
            '****************************************************************************************************************************************

            Const conBatchId As Integer = 0

            Const conInterfaceCode As Integer = 1
            Select Case v_sInterfaceCode.ToUpper()
                Case gHUBSpokeConstants.ksICRejections
                    sSQLSelectFields = "SELECT batch_id, interface_code"

                Case gHUBSpokeConstants.ksICElectronicReceipting, gHUBSpokeConstants.ksICBankStatement, gHUBSpokeConstants.ksICCloseBatch

                Case gHUBSpokeConstants.ksIC3rdPartyCollect, gHUBSpokeConstants.ksICAutoBank, gHUBSpokeConstants.ksICExtractTrans, gHUBSpokeConstants.ksICOneOff, gHUBSpokeConstants.ksICPartyLoyaltyScheme, gHUBSpokeConstants.ksICRecurring
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'RKC 29/11/2002
                    'Set Return Status Code and Message for Batch Export
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_INCORRECT_IMPORT_INTERFACE
                    Return result

                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                    'RKC 29/11/2002
                    'Set Return Status Code and Message for Batch Export
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_UNKNOWN_INTERFACE
                    Return result
            End Select


            If v_sInterfaceCode = gHUBSpokeConstants.ksICRejections Then

                m_lReturn = CType(GetBatchRecord(v_sBatchRef:=CStr(r_vHeaderData(conValue)(ACREJBatchRef)), r_vBatchResults:=vBatchId, v_sSelectFields:=sSQLSelectFields), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetBatchRecord call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                    'RKC 25/11/2002
                    'Set Return Status Code and Message for Batch Import
                    r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
                    r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
                    Return result
                End If

                'Batch Id contained in first column of results array

                lBatchID = CInt(vBatchId(conBatchId, 0))


                sBatchInterfaceCode = CStr(vBatchId(conInterfaceCode, 0))

            End If

            '****************************************************************************************************************************************
            ' END OF Get the Batch ID record for supplied batch ref and also store the transaction type stored in interface id
            '****************************************************************************************************************************************

            Select Case v_sInterfaceCode.ToUpper()
                Case gHUBSpokeConstants.ksICRejections

                    oRejections = New ImportRejections()

                    With oRejections
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oRejections.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oRejections = Nothing
                            Return result
                        End If

                        'Do the Export processing
                        If .Start(r_vDetailData:=r_vDetailData, r_vHeaderData:=r_vHeaderData, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_sBatchInterfaceCode:=sBatchInterfaceCode, r_lBatchID:=lBatchID, r_sInterfaceCode:=v_sInterfaceCode, r_sBatchRef:=v_sBatchRef) <> gPMConstants.PMEReturnCode.PMTrue Then

                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oRejections.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oRejections = Nothing
                            Return result
                        End If
                    End With

                    oRejections = Nothing
                    'Return to Caller
                    Return result


                Case gHUBSpokeConstants.ksICElectronicReceipting

                    oElecReceipt = New ImportElecReceipting()

                    With oElecReceipt
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oElecReceipt.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oElecReceipt = Nothing
                            Return result
                        End If

                        'Do the Export processing
                        If .Start(CInt(v_sBatchRef), r_vHeaderData, r_vDetailData, v_sHeaderXML, r_sStatusCode, r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then

                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oElecReceipt.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oElecReceipt = Nothing
                            Return result
                        End If
                    End With

                    oElecReceipt = Nothing

                    'Return to Caller
                    Return result

                Case gHUBSpokeConstants.ksICBankStatement

                    oImportBankStatement = New ImportBankStatement()
                    With oImportBankStatement
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oImportBankStatement.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oImportBankStatement = Nothing
                            Return result
                        End If

                        'Do the Export processing
                        If .Start(r_sInterfaceCode:=v_sInterfaceCode, r_sBatchRef:=v_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage, r_sHeaderXML:=v_sHeaderXML, r_vHeaderData:=r_vHeaderData, r_vDetailData:=r_vDetailData) <> gPMConstants.PMEReturnCode.PMTrue Then

                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oImportBankStatement.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oImportBankStatement = Nothing
                            Return result
                        End If
                    End With

                    oImportBankStatement = Nothing

                    'Return to Caller
                    Return result

                Case gHUBSpokeConstants.ksICCloseBatch


                    '**********
                    'thsi need to be changed to work off a batch ref SIR passed in though the XML
                    '**********


                    oImportCloseBatch = New ImportCloseBatch()
                    With oImportCloseBatch
                        'Set required helper objects
                        .Business = Me
                        .Database = m_oDatabase

                        'Pass through the login information
                        m_lReturn = CType(.PassThroughLogin(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, sCallingAppName:=m_sCallingAppName, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel), gPMConstants.PMEReturnCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oImportCloseBatch.PassThroughLogin call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oImportCloseBatch = Nothing
                            Return result
                        End If


                        v_sBatchRef = CStr(r_vHeaderData(conValue)(0))

                        'Do the Export processing
                        If .Start(r_sInterfaceCode:=v_sInterfaceCode, r_sBatchRef:=v_sBatchRef, r_sStatusCode:=r_sStatusCode, r_sMessage:=r_sMessage) <> gPMConstants.PMEReturnCode.PMTrue Then

                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="oImportCloseBatch.start call failed - " & r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="Import")
                            oImportCloseBatch = Nothing
                            Return result
                        End If
                    End With

                    oImportCloseBatch = Nothing

                    'Return to Caller
                    Return result

            End Select

            'RKC 25/11/2002
            'Set Return Status Code and Message for Batch Import
            'We Have a Succesful Import
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_COMPLETE
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_COMPLETE

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".Import")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Import Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Import", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            'RKC 25/11/2002
            'Set Return Status Code and Message for Batch Import
            r_sStatusCode = gHUBSpokeConstants.k_STATUS_SIRIUS_BATCH_IMPORT_FAILED
            r_sMessage = gHUBSpokeConstants.k_MESSAGE_SIRIUS_BATCH_IMPORT_FAILED
            Return result




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
        'gPMFunctions.LogMessagePopup(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Class_Initialize Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Class_Initialize", vErrNo:=CStr(Information.Err().Number), vErrDesc:=excep.Message)
        '
        'Exit Sub
        '
        'End Try

    End Sub

    Protected Overrides Sub Finalize()
        Dispose(False)
    End Sub

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

            m_sUsername = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_iSourceID = iSourceID
            m_iLanguageID = iLanguageID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel
            m_sCallingAppName = sCallingAppName

            ' RAW 21/11/2002 : PS005 : Moved to here so that the global variables that
            ' are used within CheckDatabase are set before the function is called

            m_lReturn = CType(gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' RAW 21/11/2002 : PS005 : End

            m_lProcessMode = gPMConstants.PMEProcessMode.PMProcessModeGeneric
            m_sTransactionType = gPMConstants.PMTransactionTypeGeneric
            m_dtEffectiveDate = DateTime.Now


            m_oCurrencyConvert = New bACTCurrencyConvert.Form
            m_lReturn = m_oCurrencyConvert.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

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
            Me.disposedValue = True
            If disposing Then
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()

                End If
                m_oDatabase = Nothing
                If m_oAllocateManual IsNot Nothing Then
                    m_oAllocateManual.Dispose()
                    m_oAllocateManual = Nothing
                End If
                If m_oDocumentPost IsNot Nothing Then
                    m_oDocumentPost.Dispose()
                    m_oDocumentPost = Nothing
                End If
                If m_oAutoNumber IsNot Nothing Then
                    m_oAutoNumber.Dispose()
                    m_oAutoNumber = Nothing
                End If
                If m_oTransDetail IsNot Nothing Then
                    m_oTransDetail.Dispose()
                    m_oTransDetail = Nothing
                End If

            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: PopulateDetailArray
    '
    ' Description: Populates array using results retrieved from database
    '
    ' ***************************************************************** '
    Friend Function PopulateDetailArray(ByVal v_sInterfaceCode As String, ByRef r_vDetailData(,) As Object, ByVal v_vResults(,) As Object, ByVal v_iElementNumber As Integer) As Integer

        Dim result As Integer = 0
        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PopulateDetailArray")

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sInterfaceCode.ToUpper() = gHUBSpokeConstants.ksICRecurring Then

                'RKC 26/11/2002
                '0 Add the Incremental Record Count

                r_vDetailData(gHUBSpokeConstants.eddDetailId, v_iElementNumber) = v_iElementNumber
                'Elements to be filled in array, all others are to be null for now
                '3 (eddClientCode) Client Code


                r_vDetailData(gHUBSpokeConstants.eddClientCode, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPartyShortName, v_iElementNumber))).Trim()

                If Strings.Len(CStr(v_vResults(rcpPFPremiumFinanceClientName, v_iElementNumber))) = 0 Then
                    '4 (eddClientName) Company Name


                    r_vDetailData(gHUBSpokeConstants.eddClientName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceCompanyName, v_iElementNumber))).Trim()
                Else
                    '4 (eddClientName) Client Name


                    r_vDetailData(gHUBSpokeConstants.eddClientName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceClientName, v_iElementNumber))).Trim()
                End If
                '5 (eddAccountName) Account Name


                r_vDetailData(gHUBSpokeConstants.eddAccountName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceBankAccountName, v_iElementNumber))).Trim()
                '6 (eddAmount) Amount
                'first round to 2dp


                If Not (Convert.IsDBNull(v_vResults(rcpPFIInstalmentAmount, v_iElementNumber)) Or IsNothing(v_vResults(rcpPFIInstalmentAmount, v_iElementNumber))) And CStr(v_vResults(rcpPFIInstalmentAmount, v_iElementNumber)) <> "" Then


                    v_vResults(rcpPFIInstalmentAmount, v_iElementNumber) = Math.Round(CDbl(v_vResults(rcpPFIInstalmentAmount, v_iElementNumber)), 2)
                End If



                r_vDetailData(gHUBSpokeConstants.eddAmount, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFIInstalmentAmount, v_iElementNumber))).Trim()

                'sw issue 3009 18/03/2003, include bank detail in detail XML
                '8 bank account no


                r_vDetailData(gHUBSpokeConstants.eddBankAccountNumber, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceBankAccountNo, v_iElementNumber))).Trim()
                '9 bank sort code


                r_vDetailData(gHUBSpokeConstants.eddBankSortCode, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceBankSortCode, v_iElementNumber))).Trim()
                '10 bank name


                r_vDetailData(gHUBSpokeConstants.eddBankName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceBankName, v_iElementNumber))).Trim()
                'end sw 3009

                '11 (eddCreditCardName) Credit Card Name


                r_vDetailData(gHUBSpokeConstants.eddCreditCardName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFPremiumFinanceBankAccountName, v_iElementNumber))).Trim()
                '12 (eddCreditCardNumber) Credit Card Number


                r_vDetailData(gHUBSpokeConstants.eddCreditCardNumber, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFCCNumber, v_iElementNumber))).Trim()
                '13 (eddCreditCardExpiry) Credit Card Expiry Date


                r_vDetailData(gHUBSpokeConstants.eddCreditCardExpiry, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFCCExpiryDate, v_iElementNumber))).Trim()
                '14 (eddCreditCardStart) Credit Card Start Date


                r_vDetailData(gHUBSpokeConstants.eddCreditCardStart, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFCCStartDate, v_iElementNumber))).Trim()
                '15 (eddCreditCardIssue) Credit Card Issue Number


                r_vDetailData(gHUBSpokeConstants.eddCreditCardIssue, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFCCIssueNumber, v_iElementNumber))).Trim()
                '16 (eddCreditCardPin) Credit Card Pin


                r_vDetailData(gHUBSpokeConstants.eddCreditCardPin, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFCCPIN, v_iElementNumber))).Trim()

                '17 (eddTransactionID) Transaction Id


                r_vDetailData(gHUBSpokeConstants.eddTransactionID, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentsGroupID, v_iElementNumber))).Trim()
                '18 (eddAlternativeIdentifier) Alternative Identifier


                r_vDetailData(gHUBSpokeConstants.eddAlternativeIdentifier, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPartyAlternativeIdentifier, v_iElementNumber))).Trim()
                '19 (eddAgent) Agent


                r_vDetailData(gHUBSpokeConstants.eddAgent, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPartyAgentCnt, v_iElementNumber))).Trim()
                'RKC 26/11/2002
                '1 Add the Record Status If Got This Far Assume Success

                r_vDetailData(gHUBSpokeConstants.eddRecordStatus, v_iElementNumber) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS
                'RKC 26/11/2002
                '2 Add the Record Status If Got This Far Assume Success

                r_vDetailData(gHUBSpokeConstants.eddRecordMessage, v_iElementNumber) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS
                'DD 19/06/2003: Added for bSIRPFExport Support


                r_vDetailData(gHUBSpokeConstants.eddPFInstalmentID, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentsId, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddPFInstalmentStatusID, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentStatusID, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddPFInstalmentStatusDescription, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentStatusDescription, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddPFInstalmentTransStatusID, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentTransStatusID, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddPFAutoGeneratedPlanRef, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFAutoGeneratedPlanRef, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddPFInstalmentNo, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentNo, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddPFInstalmentDueDate, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(rcpPFInstalmentDueDate, v_iElementNumber))).Trim()

            ElseIf v_sInterfaceCode.ToUpper() = gHUBSpokeConstants.ksICOneOff Then

                'Elements to be filled in array, all others are to be null for now
                'RKC 26/11/2002
                '0 Add the Incremental Record Count

                r_vDetailData(gHUBSpokeConstants.eddDetailId, v_iElementNumber) = v_iElementNumber
                '3 (eddClientCode) Client Code


                r_vDetailData(gHUBSpokeConstants.eddClientCode, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopPartyShortName, v_iElementNumber))).Trim()
                '4 (eddClientName) Client Name


                r_vDetailData(gHUBSpokeConstants.eddClientName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopPartyName, v_iElementNumber))).Trim()
                '5 (eddAccountName) Account Name


                r_vDetailData(gHUBSpokeConstants.eddAccountName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCashListItemPaymentName, v_iElementNumber))).Trim()
                '6 (eddAmount) Amount
                'first round to 2dp


                If Not (Convert.IsDBNull(v_vResults(oopCashListItemAmount, v_iElementNumber)) Or IsNothing(v_vResults(oopCashListItemAmount, v_iElementNumber))) And CStr(v_vResults(oopCashListItemAmount, v_iElementNumber)) <> "" Then


                    v_vResults(oopCashListItemAmount, v_iElementNumber) = Math.Round(CDbl(v_vResults(oopCashListItemAmount, v_iElementNumber)), 2)
                End If


                r_vDetailData(gHUBSpokeConstants.eddAmount, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCashListItemAmount, v_iElementNumber))).Trim()

                '11 (eddCreditCardName) Credit Card Name


                r_vDetailData(gHUBSpokeConstants.eddCreditCardName, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCashListItemPaymentName, v_iElementNumber))).Trim()

                'START SW ISSUE 2866 13/03/2003
                'for some reason these details had been omitted


                r_vDetailData(gHUBSpokeConstants.eddCreditCardNumber, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCCNumber, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddCreditCardExpiry, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCCExpiryDate, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddCreditCardStart, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCCStartDate, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddCreditCardIssue, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCCIssue, v_iElementNumber))).Trim()


                r_vDetailData(gHUBSpokeConstants.eddCreditCardPin, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCCPin, v_iElementNumber))).Trim()
                'END SW ISSUE 2866 13/03/2003

                '17 (eddTransactionID) Transaction Id


                r_vDetailData(gHUBSpokeConstants.eddTransactionID, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopCashListItemTransDetailId, v_iElementNumber))).Trim()
                '18 (eddAlternativeIdentifier) Alternative Identifier


                r_vDetailData(gHUBSpokeConstants.eddAlternativeIdentifier, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopPartyAlternativeIdentifier, v_iElementNumber))).Trim()
                '19 (eddAgent) Agent


                r_vDetailData(gHUBSpokeConstants.eddAgent, v_iElementNumber) = gPMFunctions.NullToString(CStr(v_vResults(oopPartyAgentCnt, v_iElementNumber))).Trim()
                'RKC 26/11/2002
                '1 Add the Record Status If Got This Far Assume Success

                r_vDetailData(gHUBSpokeConstants.eddRecordStatus, v_iElementNumber) = gHUBSpokeConstants.k_STATUS_SIRIUS_RECORD_EXPORT_SUCCESS
                'RKC 26/11/2002
                '2 Add the Record Status If Got This Far Assume Success

                r_vDetailData(gHUBSpokeConstants.eddRecordMessage, v_iElementNumber) = gHUBSpokeConstants.k_MESSAGE_SIRIUS_RECORD_EXPORT_SUCCESS
            End If


            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PopulateDetailArray")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".PopulateDetailArray")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PopulateDetailArray Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PopulateDetailArray", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RollbackTrans (Private)
    '
    ' Description: Rollback a Transaction (Undo changes to DB).
    '
    ' ***************************************************************** '
    Friend Function RollbackTrans() As Integer

        Dim result As Integer = 0
        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".RollbackTrans")

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Begin the Transaction
            m_lReturn = m_oDatabase.SQLRollbackTrans()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".RollbackTrans")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".RollbackTrans")

            ' Error.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RollbackTrans Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RollbackTrans", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: FindImportRecord
    '
    ' Description: Retrieves Id value for either PFInstalments or
    '              CashListItem either against a batch id or against
    '              selection criteria contained in a detail array
    '
    ' ***************************************************************** '
    Friend Function FindImportRecord(ByVal v_sInterfaceCode As String, ByRef r_vIDField(,) As Object, ByRef r_iRowsReturned As Integer, Optional ByVal v_lBatchID As Integer = 0, Optional ByVal v_vDetailData As Array = Nothing, Optional ByVal v_sSelectFields As String = "", Optional ByVal v_vRow As Object = Nothing) As gPMConstants.PMEReturnCode

        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim iLoop As Integer
        Dim sSQLSearch As String = ""
        Dim vIDField(,) As Object
        Dim vResults(,) As Object
        Dim lRow As Integer

        Try



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".FindImportRecord")

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_sSelectFields.Length = 0 Then
                Select Case v_sInterfaceCode
                    Case gHUBSpokeConstants.ksICRecurring
                        sSQLSearch = "SELECT DISTINCT [PFI].pfinstalments_id" & Strings.Chr(13) & Strings.Chr(10)
                        sSQLSearch = sSQLSearch & "FROM PFInstalments[PFI]" & Strings.Chr(13) & Strings.Chr(10)
                    Case gHUBSpokeConstants.ksICOneOff
                        sSQLSearch = "SELECT DISTINCT [CLI].cashlistitem_id, "
                        sSQLSearch = sSQLSearch & "[CLD].collection_account_id, "
                        sSQLSearch = sSQLSearch & "[BKA].account_id, "
                        sSQLSearch = sSQLSearch & "[CLI].mediatype_id, "
                        sSQLSearch = sSQLSearch & "[CLI].amount, "
                        sSQLSearch = sSQLSearch & "[CLI].account_id 'client_account_id', "
                        sSQLSearch = sSQLSearch & "[CLD].suspense_account_id , "
                        sSQLSearch = sSQLSearch & "[CLI].transdetail_id" & Strings.Chr(13) & Strings.Chr(10)
                        sSQLSearch = sSQLSearch & "FROM CashListItem[CLI]" & Strings.Chr(13) & Strings.Chr(10)
                End Select
            Else
                Select Case v_sInterfaceCode
                    Case gHUBSpokeConstants.ksICRecurring
                        sSQLSearch = v_sSelectFields & Strings.Chr(13) & Strings.Chr(10)
                        sSQLSearch = sSQLSearch & "FROM PFInstalments[PFI]" & Strings.Chr(13) & Strings.Chr(10)
                    Case gHUBSpokeConstants.ksICOneOff
                        sSQLSearch = v_sSelectFields & Strings.Chr(13) & Strings.Chr(10)
                        sSQLSearch = sSQLSearch & "FROM CashListItem[CLI]" & Strings.Chr(13) & Strings.Chr(10)
                End Select
            End If


            'join tables
            Select Case v_sInterfaceCode
                Case gHUBSpokeConstants.ksICRecurring
                    'sw 27/01/2003 Changed this part of the query to remove reference to plan table and correct the join from PFP to PFI
                    'DD 19/11/2003: Added joins to the Scheme Bank Account
                    sSQLSearch = sSQLSearch & "LEFT JOIN PFPremiumFinance[PFP] ON [PFP].pfprem_finance_cnt = [PFI].pfprem_finance_cnt" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "AND [PFP].pfprem_finance_version = [PFI].pfprem_finance_version" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "LEFT JOIN Party[PTY] ON [PTY].party_cnt = [PFP].clientid" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "LEFT JOIN PFScheme[PFS] ON [PFS].CompanyNo=[PFP].CompanyNo" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "AND [PFS].SchemeNo=[PFP].SchemeNo" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "AND [PFS].SchemeVersion=[PFP].SchemeVersion" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "LEFT JOIN BankAccount [BA] ON [BA].bankaccount_id=[PFS].bankaccount_id" & Strings.Chr(13) & Strings.Chr(10)

                Case gHUBSpokeConstants.ksICOneOff
                    sSQLSearch = sSQLSearch & "LEFT JOIN Account[ACC] ON [ACC].account_id = [CLI].account_id" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "LEFT JOIN Party[PTY] ON [PTY].party_cnt = [ACC].account_key" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "LEFT JOIN CashList[CLT] ON [CLT].cashlist_id = [CLI].cashlist_id" & Strings.Chr(13) & Strings.Chr(10)
                    sSQLSearch = sSQLSearch & "LEFT JOIN CashList_Drawer[CLD] ON [CLD].cashlist_drawer_id = [CLT].cashlist_drawer_id" & Strings.Chr(13) & Strings.Chr(10)

                    'sw 01/04/2003 join bank account to cashlist not cashdrawer otherwise we loose none cashdrawer records
                    sSQLSearch = sSQLSearch & "LEFT JOIN BankAccount[BKA] ON [BKA].bankaccount_id = [CLT].bankaccount_id" & Strings.Chr(13) & Strings.Chr(10)

            End Select


            If Not Information.IsNothing(v_vDetailData) Then


                If Not Information.IsNothing(v_vRow) Then

                    lRow = CInt(v_vRow)
                End If

                Select Case v_sInterfaceCode
                    Case gHUBSpokeConstants.ksICRecurring
                        'sw 16/01/2003, v_vDetailData is multidimensional for Rejections, check whether v_vrow has been passed

                        If Not Information.IsNothing(v_vRow) Then

                            sSQLSearch = sSQLSearch & "WHERE ([PFI].group_id = " & CStr(v_vDetailData(iddTransactionId, lRow)) & ")" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR (UPPER(RTRIM([PTY].shortname)) = '" & CStr(v_vDetailData(iddClientCode, lRow)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND (RTRIM([BA].bank_account_no) = '" & CStr(v_vDetailData(iddAccountNumber, lRow)) & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR [PFP].cc_number = '" & CStr(v_vDetailData(iddCreditCardNumber, lRow)) & "')" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND [PFI].amount = " & CStr(v_vDetailData(iddAmount, lRow)) & ")" & Strings.Chr(13) & Strings.Chr(10)
                            sSQLSearch = sSQLSearch & "AND [PFI].Status = 2" & Strings.Chr(13) & Strings.Chr(10)
                        Else

                            sSQLSearch = sSQLSearch & "WHERE ([PFI].group_id = " & CStr(v_vDetailData(iddTransactionId)) & ")" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR (UPPER(RTRIM([PTY].shortname)) = '" & CStr(v_vDetailData(iddClientCode, lRow)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND (RTRIM([BA].bank_account_no) = '" & CStr(v_vDetailData(iddAccountNumber, lRow)) & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR [PFP].cc_number = '" & CStr(v_vDetailData(iddCreditCardNumber)) & "')" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND [PFI].amount = " & CStr(v_vDetailData(iddAmount)) & ")"
                            sSQLSearch = sSQLSearch & "AND [PFI].Status = 2" & Strings.Chr(13) & Strings.Chr(10)
                        End If

                    Case gHUBSpokeConstants.ksICOneOff
                        'sw 16/01/2003, v_vDetailData is multidimensional for Rejections, check whether v_vrow has been passed

                        If Not Information.IsNothing(v_vRow) Then

                            sSQLSearch = sSQLSearch & "WHERE ([CLI].transdetail_id = " & CStr(v_vDetailData(iddTransactionId, lRow)) & ")" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR (UPPER([PTY].shortname) = '" & CStr(v_vDetailData(iddClientCode, lRow)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND (UPPER([CLI].payment_name) = '" & CStr(v_vDetailData(iddAccountName, lRow)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR [CLI].cc_number = '" & CStr(v_vDetailData(iddCreditCardNumber, lRow)) & "')" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND [CLI].amount = " & CStr(v_vDetailData(iddAmount, lRow)) & ")"
                        Else

                            sSQLSearch = sSQLSearch & "WHERE ([CLI].transdetail_id = " & CStr(v_vDetailData(iddTransactionId)) & ")" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR (UPPER([PTY].shortname) = '" & CStr(v_vDetailData(iddClientCode)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND (UPPER([CLI].payment_name) = '" & CStr(v_vDetailData(iddAccountName)).ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "OR [CLI].cc_number = '" & CStr(v_vDetailData(iddCreditCardNumber)) & "')" & Strings.Chr(13) & Strings.Chr(10)

                            sSQLSearch = sSQLSearch & "AND [CLI].amount = " & CStr(v_vDetailData(iddAmount)) & ")"
                        End If
                End Select
            ElseIf Not False Then
                Select Case v_sInterfaceCode
                    Case gHUBSpokeConstants.ksICRecurring
                        sSQLSearch = sSQLSearch & "WHERE [PFI].batch_id = " & CStr(v_lBatchID) & Strings.Chr(13) & Strings.Chr(10)
                    Case gHUBSpokeConstants.ksICOneOff
                        sSQLSearch = sSQLSearch & "WHERE [CLI].batch_id = " & CStr(v_lBatchID) & Strings.Chr(13) & Strings.Chr(10)
                End Select
            Else
                'No search criteria supplied so will return all records for either CashListItem or PFInstalments.
            End If

            If sSQLSearch.Length > 0 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Installment-Payment Search", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMError
                ElseIf (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And Not (Information.IsArray(vResults)) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No results returned from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="FindImportRecord")
                    Return result
                End If
            End If


            r_iRowsReturned = vResults.GetUpperBound(conRows - 1) + 1

            If v_sSelectFields.Length = 0 Then

                m_lReturn = CType(gPMFunctions.FlipArray(vResults), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="FlipArray call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindImportRecord")
                    Return result
                End If

                Select Case v_sInterfaceCode
                    Case gHUBSpokeConstants.ksICRecurring
                        vIDField = ArraysHelper.RedimPreserve(Of Object(,))(vIDField, New Integer() {conIDFieldValue - conIDFieldName + 1, 1}, New Integer() {conIDFieldName, 0})

                        vIDField(conIDFieldName, 0) = clnPFInstalmentsID


                        vIDField(conIDFieldValue, 0) = vResults(0, 0)
                    Case gHUBSpokeConstants.ksICOneOff
                        vIDField = ArraysHelper.RedimPreserve(Of Object(,))(vIDField, New Integer() {conIDFieldValue - conIDFieldName + 1, 8}, New Integer() {conIDFieldName, 0})

                        vIDField(conIDFieldName, 0) = clnCashListItemID

                        vIDField(conIDFieldName, 1) = clnCollectionAccountID

                        vIDField(conIDFieldName, 2) = clnAccountID

                        vIDField(conIDFieldName, 3) = clnMediaTypeID

                        vIDField(conIDFieldName, 4) = clnAmount
                        'sw for some reason client account and suspense were missing 20/01/2003

                        vIDField(conIDFieldName, 5) = clnClientAccountId

                        vIDField(conIDFieldName, 6) = clnSuspenseAccountId
                        'sw added in transdetail id 04/04/2003

                        vIDField(conIDFieldName, 7) = clnTransDetailID


                        vIDField(conIDFieldValue, 0) = vResults(0, 0)


                        vIDField(conIDFieldValue, 1) = vResults(0, 1)


                        vIDField(conIDFieldValue, 2) = vResults(0, 2)


                        vIDField(conIDFieldValue, 3) = vResults(0, 3)


                        vIDField(conIDFieldValue, 4) = vResults(0, 4)


                        vIDField(conIDFieldValue, 5) = vResults(0, 5)


                        vIDField(conIDFieldValue, 6) = vResults(0, 6)


                        vIDField(conIDFieldValue, 7) = vResults(0, 7)
                End Select
                'Pass back the completed array holding column names and corresponding values for the row found.
                r_vIDField = vIDField
            Else

                r_vIDField = vResults
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".FindImportRecord")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".FindImportRecord")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FindImportRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="FindImportRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetIDValueFromCode
    '
    ' Description: Retrieves Id value for any specified table given a
    '              code (or vice versa)
    '
    ' ***************************************************************** '
    Friend Function GetIDValueFromCode(ByVal v_sTableName As String, ByVal v_bGettingCode As Boolean, Optional ByRef r_sCode As String = "", Optional ByRef r_lID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sSQLSearch As String = ""
        Dim vResults(,) As Object

        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetIDValueFromCode")

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_bGettingCode Then
                sSQLSearch = "SELECT code FROM " & v_sTableName & Strings.Chr(13) & Strings.Chr(10)
                sSQLSearch = sSQLSearch & "WHERE " & v_sTableName & "_id = " & CStr(r_lID)
            Else
                sSQLSearch = "SELECT " & v_sTableName & "_id FROM " & v_sTableName & Strings.Chr(13) & Strings.Chr(10)
                sSQLSearch = sSQLSearch & "WHERE UPPER(code) = '" & r_sCode.ToUpper() & "'"
            End If

            If sSQLSearch.Length > 0 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Installment-Payment Search", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And Not (Information.IsArray(vResults)) Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No results returned from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIDValueFromCode")
                    Return result
                ElseIf (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And (Information.IsArray(vResults)) Then

                    If vResults.GetUpperBound(conRows - 1) > 0 Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="More than one row returned from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIDValueFromCode")
                        Return result
                    End If
                End If
            End If

            If v_bGettingCode Then

                r_sCode = CStr(vResults(0, 0))
            Else

                r_lID = CInt(vResults(0, 0))
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetIDValueFromCode")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".GetIDValueFromCode")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetIDValueFromCode Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetIDValueFromCode", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: PostTransaction
    '
    ' Description: Posts a balancing set of transactions
    '
    '              NB At the moment, the transaction detail ids obtained
    '              during each posting are not passed back to the calling
    '              function, as they are not required there at present.
    '
    ' 11/12/2002 - PWC - Added 2 optional parameters to allow transactionid(s) to be
    '                    passed back to the caller.
    '                    NOTE - it is the callers responsibility to ensure that
    '                    if an array is passed it is large enough
    '
    ' DD 27/08/2003: Added handling of different document type
    ' SMJB 15/10/03: Changed default document type to Journal
    ' ***************************************************************** '
    'developer Guide no 33
    Friend Function PostTransaction(ByVal v_vCreditAccount As Object, ByVal v_vDebitAccount As Object, ByVal v_sComment As String, Optional ByVal v_dTotalAmount As Decimal = 0, Optional ByRef r_vNewCreditTransDetailId As Object = Nothing, Optional ByRef r_vNewDebitTransDetailId As Object = Nothing, Optional ByVal v_sDocumentRangeCode As String = gACTLibrary.ACTAutoNumberRangeCodeRvj) As Integer

        Dim result As Integer = 0
        Dim lNumberRangeID, lNumber As Integer
        Dim iDocSeq As Integer

        Dim lCreditTransDetailId, lDebitTransDetailId As Integer

        ' Document paramters
        Dim sDocumentRef As String = ""
        Dim dtDocumentDate As Date
        Dim sComment As String = ""

        Dim lDocumentType As Integer
        Dim sGroupCode As String = ""
        Dim iBaseCurrencyID As Integer

        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".PostTransaction")

            result = gPMConstants.PMEReturnCode.PMTrue

            dtDocumentDate = DateTime.Now

            ' Get an instance of DocumentPost
            If m_oDocumentPost Is Nothing Then

                m_oDocumentPost = New bACTDocumentPost.Form
                m_lReturn = m_oDocumentPost.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="failed to create object bACTDocumentPost.Form", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                    Return result
                End If
            End If

            ' Get an instance of auto number if needed
            If m_oAutoNumber Is Nothing Then

                m_oAutoNumber = New bACTAutoNumber.Business
                m_lReturn = m_oAutoNumber.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="failed to create object bACTAutoNumber.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                    Return result
                End If
            End If

            Select Case v_sDocumentRangeCode
                Case gACTLibrary.ACTAutoNumberRangeCodeIcr
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef38
                    lDocumentType = gACTLibrary.ACTDocTypeInstalmentCredit
                Case Else
                    sGroupCode = gACTLibrary.ACTAutoNumberGroupCodeDocumentRef1
                    lDocumentType = gACTLibrary.ACTDocTypeJournal
            End Select

            With m_oAutoNumber
                ' Get the number range

                m_lReturn = .GetNumberRange(v_sGroupCode:=gACTLibrary.ACTAutoNumberGroupCodeDocumentRef38, v_sRangeCode:=v_sDocumentRangeCode, r_lNumberRangeID:=lNumberRangeID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oAutoNumber.GetNumberRange call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                    Return result
                End If

                ' Generate the next number
                'Start(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber

                m_lReturn = .GenerateDocumentReferenceNumber(v_lNumberRangeID:=lNumberRangeID, v_iUserID:=m_iUserID, v_iCompanyID:=m_iSourceID, r_sDocumentRef:=sDocumentRef, v_sRangeCode:=v_sDocumentRangeCode)
                'End(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
                    'Note:- the method GenerateNumber is changed to GenerateDocumentReferenceNumber
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oAutoNumber.GenerateDocumentReferenceNumber call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                    Return result
                End If
            End With

            ' Format the number
            '(Arul Stephen)-(Tech Spec -WPR78_Unique_Document_Reference_Number)
            sDocumentRef = v_sDocumentRangeCode & sDocumentRef
            ' Generate document

            With m_oDocumentPost

                m_lReturn = .AddDocument(v_lDocumentTypeId:=lDocumentType, v_sDocumentRef:=sDocumentRef, v_dtDocumentDate:=dtDocumentDate, v_sComment:=v_sComment)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddDocument call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                    Return result
                End If

                iDocSeq = 1

                ' Get the Company's base currency

                m_lReturn = m_oCurrencyConvert.GetBaseCurrency(v_lCompanyID:=m_iSourceID, r_iBaseCurrencyID:=iBaseCurrencyID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Information.IsArray(v_vCreditAccount) Then

                    For iLoop As Integer = 0 To v_vCreditAccount.GetUpperBound(conRows - 1)


                        m_lReturn = .AddAdjustmentTransaction(v_lAccountId:=v_vCreditAccount(conAccountId, iLoop), v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=(CDbl(v_vCreditAccount(conAmount, iLoop)) * -1), v_cCurrencyAmount:=(CDbl(v_vCreditAccount(conAmount, iLoop)) * -1), v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=lCreditTransDetailId, v_vDocumentSequence:=iDocSeq, v_vComment:=v_sComment, v_vAccountingDate:=dtDocumentDate)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddAdjustmentTransaction call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                            Return result
                        End If
                        iDocSeq += 1

                        'Return the Id of the new transaction

                        If Not Information.IsNothing(r_vNewCreditTransDetailId) Then

                            r_vNewCreditTransDetailId(iLoop) = lCreditTransDetailId
                        End If
                    Next iLoop
                Else

                    m_lReturn = .AddAdjustmentTransaction(v_lAccountId:=v_vCreditAccount, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=(v_dTotalAmount * -1), v_cCurrencyAmount:=(v_dTotalAmount * -1), v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=lCreditTransDetailId, v_vDocumentSequence:=iDocSeq, v_vComment:=v_sComment, v_vAccountingDate:=dtDocumentDate)

                    iDocSeq += 1

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddAdjustmentTransaction call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                        Return result
                    End If

                    'Return the Id of the new transaction

                    If Not Information.IsNothing(r_vNewCreditTransDetailId) Then
                        r_vNewCreditTransDetailId = lCreditTransDetailId
                    End If
                End If

                If Information.IsArray(v_vDebitAccount) Then

                    For iLoop As Integer = 0 To v_vDebitAccount.GetUpperBound(conRows - 1)

                        m_lReturn = .AddAdjustmentTransaction(v_lAccountId:=v_vDebitAccount(conAccountId, iLoop), v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_vDebitAccount(conAmount, iLoop), v_cCurrencyAmount:=v_vDebitAccount(conAmount, iLoop), v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=lDebitTransDetailId, v_vDocumentSequence:=iDocSeq, v_vComment:=v_sComment, v_vAccountingDate:=dtDocumentDate)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            ' RAW 24/02/2004 : CQ4106 : added
                            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddAdjustmentTransaction call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                            Return result
                        End If
                        iDocSeq += 1

                        'Return the Id of the new transaction

                        If Not Information.IsNothing(r_vNewDebitTransDetailId) Then

                            r_vNewDebitTransDetailId(iLoop) = lDebitTransDetailId
                        End If
                    Next iLoop
                Else

                    m_lReturn = .AddAdjustmentTransaction(v_lAccountId:=v_vDebitAccount, v_iCurrencyID:=iBaseCurrencyID, v_cAmount:=v_dTotalAmount, v_cCurrencyAmount:=v_dTotalAmount, v_vdCurrencyBaseXRate:=1, r_vTransDetailId:=lDebitTransDetailId, v_vDocumentSequence:=iDocSeq, v_vComment:=v_sComment, v_vAccountingDate:=dtDocumentDate)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' RAW 24/02/2004 : CQ4106 : added
                        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="m_oDocumentPost.AddAdjustmentTransaction call failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction")
                        Return result
                    End If

                    'Return the Id of the new transaction

                    If Not Information.IsNothing(r_vNewDebitTransDetailId) Then
                        r_vNewDebitTransDetailId = lDebitTransDetailId
                    End If
                End If
            End With

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".PostTransaction")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".PostTransaction")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PostTransaction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PostTransaction", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetBatchRecord
    '
    ' Description: Retrieves Batch Record Id for supplied Batch Ref
    '
    ' ***************************************************************** '
    Friend Function GetBatchRecord(ByVal v_sBatchRef As String, ByRef r_vBatchResults As Object, ByVal v_sSelectFields As String) As Integer

        Dim result As Integer = 0
        Dim sSQLSearch As String = ""
        Dim vResults(,) As Object

        Try

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & conDot & ACClass & ".GetBatchRecord")

            result = gPMConstants.PMEReturnCode.PMTrue

            'Retrieve Batch Id value and any other values from the batch record with the supplied batch ref (v_sBatchRef) value.
            sSQLSearch = v_sSelectFields & " FROM Batch WHERE UPPER(batch_ref) = '" & v_sBatchRef.ToUpper() & "'" & Strings.Chr(13) & Strings.Chr(10)

            If sSQLSearch.Length > 0 Then
                m_lReturn = m_oDatabase.SQLSelect(sSQL:=sSQLSearch, sSQLName:="Batch Id Search", bStoredProcedure:=False, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords, vResultArray:=vResults)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Debug.WriteLine(VB6.TabLayout("Error(s) during retrieval of batch Id value", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Fatal Error"))
                    Return gPMConstants.PMEReturnCode.PMFalse
                ElseIf (m_lReturn = gPMConstants.PMEReturnCode.PMTrue) And Not Information.IsArray(vResults) Then
                    Debug.WriteLine(VB6.TabLayout("Search returned no results.  Cannot proceed further", MsgBoxStyle.Critical + MsgBoxStyle.OkOnly, "Error"))
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' RAW 24/02/2004 : CQ4106 : added
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="No results returned from DB", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchRecord")
                    Return result
                End If
            End If

            'Pass back the results found


            r_vBatchResults = vResults

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & conDot & ACClass & ".GetBatchRecord")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & conDot & ACClass & ".GetBatchRecord")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetBatchRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetBatchRecord", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: CreateBatchRecord
    ' PURPOSE:
    ' AUTHOR: Steve Watton  -  creates a new record in the batch table
    ' DATE: 13/01/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function CreateBatchRecord(Optional ByRef r_lBatchID As Integer = 0, Optional ByVal v_lBatchStatusID As Integer = 0, Optional ByVal v_lCompanyID As Integer = 0, Optional ByVal v_lUserID As Integer = 0, Optional ByVal v_sBatchRef As String = "", Optional ByVal v_dtCreatedDate As Date = #12/30/1899#, Optional ByVal v_dtAuthorisedDate As Date = #12/30/1899#, Optional ByVal v_dtAccountingDate As Date = #12/30/1899#, Optional ByVal v_sComment As String = "", Optional ByVal v_lBatchTypeID As Integer = 0, Optional ByVal v_lBatchSourceID As Integer = 0, Optional ByVal v_sXML As String = "", Optional ByVal v_dtExportDate As Date = #12/30/1899#, Optional ByVal v_dtReExportDate As Date = #12/30/1899#, Optional ByVal v_lMediaTypeID As Integer = 0, Optional ByVal v_cTotalAmount As Decimal = 0, Optional ByVal v_lTotalTransactions As Integer = 0, Optional ByVal v_dtImportedDate As Date = #12/30/1899#, Optional ByVal v_cRejectAmount As Decimal = 0, Optional ByVal v_lRejectTransactions As Integer = 0, Optional ByVal v_dtClosedDate As Date = #12/30/1899#, Optional ByVal v_sInterfaceCode As String = "", Optional ByVal v_iAutoClose As Integer = 0) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "CreateBatchRecord"



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(r_lBatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        'Developer Guide no.85
        'Start
        If m_oDatabase.Parameters.Add(sName:="batchstatus_id", vValue:=IIf(v_lBatchStatusID = 0, DBNull.Value, CStr(v_lBatchStatusID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="company_id", vValue:=IIf(v_lCompanyID = 0, DBNull.Value, CStr(v_lCompanyID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="user_id", vValue:=IIf(v_lUserID = 0, DBNull.Value, CStr(v_lUserID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If m_oDatabase.Parameters.Add(sName:="batch_ref", vValue:=v_sBatchRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="created_date", vValue:=IIf(v_dtCreatedDate = CDate("00:00:00"), DBNull.Value, v_dtCreatedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="authorised_date", vValue:=IIf(v_dtAuthorisedDate = CDate("00:00:00"), DBNull.Value, v_dtAuthorisedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="accounting_date", vValue:=IIf(v_dtAccountingDate = CDate("00:00:00"), DBNull.Value, v_dtAccountingDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="comment", vValue:=IIf(v_sComment = "", DBNull.Value, v_sComment), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="batch_type_id", vValue:=IIf(v_lBatchTypeID = 0, DBNull.Value, CStr(v_lBatchTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="batch_source_id", vValue:=IIf(v_lBatchSourceID = 0, DBNull.Value, CStr(v_lBatchSourceID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="xml_object", vValue:=IIf(v_sXML = "", DBNull.Value, v_sXML), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="exportdate", vValue:=IIf(v_dtExportDate = CDate("00:00:00"), DBNull.Value, v_dtExportDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="reexportdate", vValue:=IIf(v_dtReExportDate = CDate("00:00:00"), DBNull.Value, v_dtReExportDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        'sw 14-04-2003 Changed this from mediavalidationid to mediaid.
        'table and sp changed accordingly

        If m_oDatabase.Parameters.Add(sName:="mediatypeid", vValue:=IIf(v_lMediaTypeID = 0, DBNull.Value, CStr(v_lMediaTypeID)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="totalamount", vValue:=IIf(Conversion.Val(CStr(v_cTotalAmount)) = 0, DBNull.Value, CStr(v_cTotalAmount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="totaltransactions", vValue:=IIf(v_lTotalTransactions = 0, DBNull.Value, CStr(v_lTotalTransactions)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="importeddate", vValue:=IIf(v_dtImportedDate = CDate("00:00:00"), DBNull.Value, v_dtImportedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="rejectamount", vValue:=IIf(Conversion.Val(CStr(v_cRejectAmount)) = 0, DBNull.Value, CStr(v_cRejectAmount)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMCurrency) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="rejecttransactions", vValue:=IIf(v_lRejectTransactions = 0, DBNull.Value, CStr(v_lRejectTransactions)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result


        If m_oDatabase.Parameters.Add(sName:="closeddate", vValue:=IIf(v_dtClosedDate = CDate("00:00:00"), DBNull.Value, v_dtClosedDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate) <> gPMConstants.PMEReturnCode.PMTrue Then Return result



        If m_oDatabase.Parameters.Add(sName:="interfacecode", vValue:=IIf(v_sInterfaceCode = "", DBNull.Value, v_sInterfaceCode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result
        'End the modification
        If m_oDatabase.Parameters.Add(sName:="autoclose", vValue:=CStr(v_iAutoClose), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACCreateBatchRecordSQL, sSQLName:=ACCreateBatchRecordName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        r_lBatchID = gPMFunctions.NullToLong(m_oDatabase.Parameters.Item("batch_id").Value)

        result = gPMConstants.PMEReturnCode.PMTrue

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
        Resume


        Select Case Information.Err().Number
            Case Constants.vbObjectError
                ' Log internal failure.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source)

                result = gPMConstants.PMEReturnCode.PMFalse

                GoTo Finally_Renamed

            Case Else
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                result = gPMConstants.PMEReturnCode.PMError

                GoTo Finally_Renamed

        End Select

Finally_Renamed:

        Return result

    End Function

    ' ---------------------------------------------------------------------------
    ' PROCEDURE NAME: UpdateBatchRef
    ' PURPOSE:
    ' AUTHOR: Steve Watton  -  Updates the Batch Ref with the value passed
    ' DATE: 18/03/2003
    ' RETURNS: PMTrue for success
    ' ---------------------------------------------------------------------------

    Public Function UpdateBatchRef(ByVal v_lBatchID As Integer, ByVal v_sBatchRef As String) As Integer

        Dim result As Integer = 0
        Const ACMethod As String = "UpdateBatchRef"



        result = gPMConstants.PMEReturnCode.PMFalse

        ' Clear the Database Parameters Collection
        m_oDatabase.Parameters.Clear()

        If m_oDatabase.Parameters.Add(sName:="batch_id", vValue:=CStr(v_lBatchID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        If m_oDatabase.Parameters.Add(sName:="batch_ref", vValue:=v_sBatchRef, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        ' Execute SQL Statement
        If m_oDatabase.SQLAction(sSQL:=ACUpdateBatchRefSQL, sSQLName:=ACUpdateBatchRefName, bStoredProcedure:=True) <> gPMConstants.PMEReturnCode.PMTrue Then Return result

        result = gPMConstants.PMEReturnCode.PMTrue

        GoTo Finally_Renamed

        '----------------------------------------------------------------------------------------
        'Only for Debugging, the code will never execute this line
        '----------------------------------------------------------------------------------------
        Resume


        Select Case Information.Err().Number
            Case Constants.vbObjectError
                ' Log internal failure.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=Information.Err().Description, vApp:=ACApp, vClass:=ACClass, vMethod:=Information.Err().Source)

                result = gPMConstants.PMEReturnCode.PMFalse

                GoTo Finally_Renamed

            Case Else
                ' Log Error.
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACMethod & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=ACMethod, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                result = gPMConstants.PMEReturnCode.PMError

                GoTo Finally_Renamed

        End Select

Finally_Renamed:

        Return result

    End Function
End Class
