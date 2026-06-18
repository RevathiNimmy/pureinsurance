Option Strict Off
Option Explicit On
Imports System.Text
Imports System.Xml
Imports SSP.Shared

<System.Runtime.InteropServices.ProgId("Renewals_NET.Renewals")>
Public NotInheritable Class Renewals
    Implements IDisposable

    Private m_sUsername As String = ""
    Private m_sPassword As String = ""
    Private m_iUserID As Integer
    Private m_sCallingAppName As String = ""
    Private m_iSourceID As Integer
    Private m_iLanguageID As Integer
    Private m_iCurrencyID As Integer
    Private m_iLogLevel As Integer

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "Renewals"
    ' Renewals Class (when creating objects)
    Private Const ACRenewalsClass As String = "Renewals"
    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database
    Private m_lReturn As Integer
    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean
    ' Data Set Definition
    Private m_oDataSet As cGISDataSetControl.Application
    ' GIS Scheme Business
    Private m_oGISSchemeBusiness As bGISSchemeBusiness.Business
    ' GIS Application
    Private m_oGisApplication As bGIS.Application
    ' GIIToSirPolicy
    Private m_oSirPolicy As Object

    Private _m_oDebugTimings As DebugTimings = Nothing
    Private Property m_oDebugTimings() As DebugTimings
        Get
            If _m_oDebugTimings Is Nothing Then
                _m_oDebugTimings = New DebugTimings()
            End If
            Return _m_oDebugTimings
        End Get
        Set(ByVal Value As DebugTimings)
            _m_oDebugTimings = Value
        End Set
    End Property

    ' TF311001 - Additional constants for Renewal Jobs
    ' Copied from JobConstants.bas
    Private Const AC_PreRenSelection As String = "Pre-Renewal Selection"
    Private Const AC_Selection As String = "Selection"
    Private Const AC_Quote_Broker As String = "Broker Led Quote"
    Private Const AC_ProcessEDI As String = "Process EDI"
    Private Const AC_Quote_Insurer As String = "Insurer Led Quote"
    Private Const AC_Invite As String = "Invite"
    Private Const AC_Reminder As String = "Reminder"
    Private Const AC_Complete As String = "Completion"
    Private Const AC_Confirm As String = "Confirm"

    ' And some more to associate with an integer value
    Private Const AC_PreRenSelectionID As Integer = 1
    Private Const AC_SelectionID As Integer = 2
    Private Const AC_Quote_BrokerID As Integer = 3
    Private Const AC_ProcessEDIID As Integer = 4
    Private Const AC_Quote_InsurerID As Integer = 5
    Private Const AC_InviteID As Integer = 6
    Private Const AC_ReminderID As Integer = 7
    Private Const AC_CompleteID As Integer = 8
    Private Const AC_ConfirmID As Integer = 9

    ' Message to write to log file.
    Private m_sRenTaskLog As String = ""

    ' Object to control Task Logging
    Private m_oTaskLog As Object
    Private m_oListManager As bGISListManager.InterfaceNoLogin
    Private m_oCommonLists As Hashtable

    ' PUBLIC Property Procedures (Begin)
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUserName As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceID As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long


        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel

            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUsername = sUserName
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            If Informations.IsNothing(vDatabase) Then
                m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase)
            Else

                m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' GIS Scheme Business

            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business
            m_lReturn = m_oGISSchemeBusiness.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bGISSchemeBusiness.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            m_oGisApplication = New bGIS.Application()

            m_lReturn = m_oGisApplication.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bGIS.Application", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Remove component services

            With m_oDebugTimings
                .UserId = m_iUserID
                .SaveAsFile = True
                .PrintToScreen = True
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
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
                If m_oGISSchemeBusiness IsNot Nothing Then
                    m_oGISSchemeBusiness.Dispose()
                    m_oGISSchemeBusiness = Nothing
                End If
                If m_oSirPolicy IsNot Nothing Then
                    m_oSirPolicy.Dispose()
                    m_oSirPolicy = Nothing
                End If
                If m_oDataSet IsNot Nothing Then
                    m_oDataSet.Dispose()
                    m_oDataSet = Nothing
                End If
                If m_oGisApplication IsNot Nothing Then
                    m_oGisApplication.Dispose()
                    m_oGisApplication = Nothing
                End If
                If m_oTaskLog IsNot Nothing Then
                    m_oTaskLog.Dispose()
                    m_oTaskLog = Nothing
                End If
                m_oListManager = Nothing
                m_oCommonLists = Nothing
                m_oDebugTimings = Nothing
                If m_bCloseDatabase Then
                    If m_bCloseDatabase Then
                        m_oDatabase.CloseDatabase()
                        m_oDatabase = Nothing
                    End If
                End If
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: PreRenSelection
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '          05/04/2001 CTAF - Added creation of BOM and calling
    '                            of it's methods.
    '
    ' ***************************************************************** '
    'AK 011101 - added another parameter insurance file cnt
    Public Function PreRenSelection(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer) As Integer
        Return PreRenSelection(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lGISDataModelID:=Nothing, v_sBusinessTypeCode:="", v_lInsuranceFileCnt:=0)
    End Function
    Public Function PreRenSelection(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByVal v_lGISDataModelID As Object, ByVal v_sBusinessTypeCode As String, ByVal v_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals

        'AK 021101
        Dim lSuspension As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - PreRenSelection - Start")

            m_oDebugTimings.StartBlock()

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_PreRenSelectionID, v_sProcessCode:=AC_PreRenSelection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - PreRenSelection - CreateBom")

            ' Did we create a BOM?
            If Not (oBom Is Nothing) Then

                ' Call PreRenSelection on the BOM
                'AK 011101 - this process was never being called, need to use it now

                m_lReturn = oBom.PreRenSelection(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), r_vSuspensionLevel:=lSuspension)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.PreRenSelection."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_PreRenSelectionID, v_sProcessCode:=AC_PreRenSelection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If

                ' Remove the instance of the BOM

                oBom.Dispose()
                oBom = Nothing

            End If

            m_oDebugTimings.PrintDebugMessage("bGis - PreRenSelection - oBOM.PreRenSelection")

            ' CTAF 050401 - Changed to use Component Services

            ' Create a SBO Link object

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "PreRenSelection")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                m_sRenTaskLog = "Failed to create bSiriusLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_PreRenSelectionID, v_sProcessCode:=AC_PreRenSelection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Remove component services

            m_oDebugTimings.PrintDebugMessage("bGis - PreRenSelection - Create oSBOLink")

            ' Call the PreRenSelected method on the Sirius Link Object
            'AK 021101 - added another parameter for passing InsuraceFileCnt here
            '            and also pass suspension level

            m_lReturn = oSBOLink.PreRenSelected(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSuspensionLevel:=lSuspension, v_lGisDataModelId:=v_lGISDataModelID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.PreRenSelected Failed", ACApp, ACClass, "PreRenSelection")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                m_sRenTaskLog = "Failed to process oSBOLink.PreRenSelection."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_PreRenSelectionID, v_sProcessCode:=AC_PreRenSelection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Terminate the link

            oSBOLink.Dispose()
            m_oDebugTimings.EndBlock("bGis - PreRenSelection - oSBOLink.PreRenSelected")

            m_oDebugTimings.EndBlock("bGis - PreRenSelection - Total Time")

            oSBOLink = Nothing

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_PreRenSelectionID, v_sProcessCode:=AC_PreRenSelection, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in PreRenSelection."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_PreRenSelectionID, v_sProcessCode:=AC_PreRenSelection, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenSelection
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenSelection(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByRef r_sGISDataModelCode As String) As Integer
        Return RenSelection(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, r_sGISDataModelCode:=r_sGISDataModelCode, v_sBusinessTypeCode:="")
    End Function

    Public Function RenSelection(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByRef r_sGISDataModelCode As String, ByVal v_sBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals
        Dim lRenewalInsuranceFileCnt, lProductID, lGISSchemeID, lIsInsurerLead, lPartyCnt As Integer
        Dim dtRenewalDate As Date
        Dim lInsuranceFileCnt As Integer
        Dim lGISDataModelID, lGISPolicyLinkID, lNewGisPolicyLinkID As Integer
        Dim sGisDataModelCode As String = ""
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lNewRiskCnt As Integer

        '30/10/2001 Thinh Nguyen add suspension level flag
        Dim vSuspensionLevel As Object
        Dim sQuoteRef As String = ""
        Dim oDataSet As Object = m_oDataSet

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - Start")

            m_oDebugTimings.StartBlock()

            '30/10/2001 default suspension level flag
            vSuspensionLevel = 0

            ' Create a SBO Link object


            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "RenSelection")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                m_sRenTaskLog = "Failed to create bSIRIUSlink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Remove component services

            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - Create Sirius Link Object")

            ' ***************************************************************** '
            ' Call the CreateRenPolicyVersion method on the Sirius Link Object
            ' ***************************************************************** '

            m_lReturn = oSBOLink.CreateRenPolicyVersion(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, r_lProductId:=lProductID, r_lGisSchemeId:=lGISSchemeID, r_lIsInsurerLead:=lIsInsurerLead, r_lPartyCnt:=lPartyCnt, r_dtRenewalDate:=dtRenewalDate, r_lInsuranceFileCnt:=lInsuranceFileCnt, r_lGISDataModelID:=lGISDataModelID, r_sGISDataModelCode:=r_sGISDataModelCode, r_lGISPolicyLinkID:=lGISPolicyLinkID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.PreRenSelected Failed", ACApp, ACClass, "RenSelection")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                m_sRenTaskLog = "Failed to process oSBOLink.PreRenSelected."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - oSBOLink.CreateRenPolicyVersion")

            'JSB 30/1/03 - added check to not run following for GII
            'RWH(09/01/2004) - trim Code before checking.
            v_sBusinessTypeCode = v_sBusinessTypeCode.Trim()
            If v_sBusinessTypeCode <> "GIIT" And v_sBusinessTypeCode <> "GIIH" And v_sBusinessTypeCode <> "GIIM" Then
                '---------------------------------------------------------------------------
                'RAM20020522 : Added the following code to copy Risk Data Asscociated
                '              with the old insurance file cnt to the new insurance file cnt
                '---------------------------------------------------------------------------
                m_oDebugTimings.PrintDebugMessage("bGis.Renewals - WhatIfQuote - CopyRiskData Started")

                m_lReturn = CopyRiskData(v_lOldInsuranceFileCnt:=lInsuranceFileCnt, v_lNewInsuranceFileCnt:=lRenewalInsuranceFileCnt, r_lNewRiskCnt:=lNewRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CopyRiskData Failed", ACApp, ACClass, "WhatIfQuote")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oSBOLink = Nothing
                    Return result
                End If
            Else
                'JSB 30/01/03 - set default value for riskcode
                lNewRiskCnt = 1
            End If
            'JSB 30/1/03

            m_oDebugTimings.PrintDebugMessage("bGis.Renewals - WhatIfQuote - CopyRiskData Finished")

            '---------------------------------------------------------------------------

            ' ***************************************************************** '
            ' Create the renewal version of the risk
            ' ***************************************************************** '
            sGisDataModelCode = r_sGISDataModelCode.Trim()

            m_lReturn = RenSelectionCopyDataSet(v_sDataModelCode:=sGisDataModelCode, v_lOldInsuranceFileCnt:=lInsuranceFileCnt, v_lNewInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lGISSchemeID:=lGISSchemeID, r_lNewGISPolicyLinkID:=lNewGisPolicyLinkID, v_lNewRiskID:=lNewRiskCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to make copy of risk data for " & lInsuranceFileCnt & " ", ACApp, ACClass, "RenSelection")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                m_sRenTaskLog = "Failed to process RenSelectionCopyDataSet."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - RenSelectionCopyDataSet")
            r_sGISDataModelCode = r_sGISDataModelCode.Trim()

            '-------------------------------------------------------------------------------
            'Modified by DD on 20/02/2002
            'Reason:    Update the Temporary Reference in the new copy
            '           Required otherwise confusion can occur between Renewals Quotes
            '           and the original New Business Policy

            ' Generate a new Quote Reference
            m_lReturn = m_oGisApplication.GenerateQuoteRef(v_lGISPolicyLinkID:=lNewGisPolicyLinkID, r_sQuoteRef:=sQuoteRef, v_sGisDataModelCode:=sGisDataModelCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear the Parameters
            m_oDatabase.Parameters.Clear()

            ' Policy Link ID Input Param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(lNewGisPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Quote Ref Input Param
            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref", vValue:=CStr(sQuoteRef), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Quote Ref Password Input Param - null = leave alone


            m_lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref_password", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Update the reference in the GIS_Policy_Link table
            m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateQteRefSQL, sSQLName:=ACUpdateQteRefName, bStoredProcedure:=ACUpdateQteRefStored)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If


            '-------------------------------------------------------------------------------

            ' Somebody set us up the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=r_sGISDataModelCode, v_sGisBusinessTypeCode:=v_sBusinessTypeCode, v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Check if a BOM was created
            If Not (oBom Is Nothing) Then

                '30/10/2001 Thinh Nguyen add suspension level
                ' AK 030801 - Call RenSelectionBefore on the BOM

                m_lReturn = oBom.RenSelectionBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet, v_sGisDataModelCode:=gPMFunctions.ToSafeString(sGisDataModelCode), v_lOldInsuranceFileCnt:=gPMFunctions.ToSafeInteger(lInsuranceFileCnt), r_vSuspensionLevel:=vSuspensionLevel)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to run oBOM.RenSelectionBefore for " & lInsuranceFileCnt & " ", ACApp, ACClass, "RenSelection")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenSelectionBefore."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDataSet = oDataSet

                v_sBusinessTypeCode = v_sBusinessTypeCode.Trim()
                If v_sBusinessTypeCode <> "GIIT" And v_sBusinessTypeCode <> "GIIH" And v_sBusinessTypeCode <> "GIIM" Then
                    ' Call RenSelection on the BOM
                    ' CJB 111104 PN16651 - Pass renewal date as rqd for index linking

                    m_lReturn = oBom.RenSelectionAfter(v_lOldGISPolicyLinkID:=gPMFunctions.ToSafeInteger(lGISPolicyLinkID), v_lNewGisPolicyLinkID:=gPMFunctions.ToSafeInteger(lNewGisPolicyLinkID), v_dtRenewalDate:=gPMFunctions.ToSafeDate(dtRenewalDate), r_oDataSet:=oDataSet, v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lGISSchemeID))
                Else

                    m_lReturn = oBom.RenSelectionAfter(v_lOldGISPolicyLinkID:=gPMFunctions.ToSafeInteger(lGISPolicyLinkID), v_lNewGisPolicyLinkID:=gPMFunctions.ToSafeInteger(lNewGisPolicyLinkID), r_oDataSet:=oDataSet, v_lGISSchemeID:=gPMFunctions.ToSafeInteger(lGISSchemeID))
                End If
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to run oBOM.RenSelectionAfter for " & lInsuranceFileCnt & " ", ACApp, ACClass, "RenSelection")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenSelectionAfter."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDataSet = oDataSet

                ' Destroy the BOM

                oBom.Dispose()

                oBom = Nothing

            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - oBom.RenSelectionAfter")

            'AK 030801 - Save the new dataset to Database now
            m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process m_oDataSet.ReturnAsXML."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - m_oDataSet.ReturnAsXML")

            m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=sGisDataModelCode, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to save to database.", ACApp, ACClass, "RenSelectionCopyDataSet")
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process m_oGisApplication.SaveToDB."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - m_oGisApplication.SaveToDB")

            ' ***************************************************************** '
            ' Update the control file and create the policy event
            ' ***************************************************************** '
            '30/10/2001 Thinh Nguyen add suspension level flag

            m_lReturn = oSBOLink.RenSelected(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_lGisSchemeId:=lGISSchemeID, v_lProductID:=lProductID, v_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=lGISSchemeID, v_dtRenewalDate:=dtRenewalDate, v_lGisDataModelId:=lGISDataModelID, v_lSuspensionLevel:=vSuspensionLevel)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to update control file and policy event", ACApp, ACClass, "RenSelection")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                m_sRenTaskLog = "Failed to process oSBOLink.RenSelected."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - oSBOLink.RenSelected")

            oSBOLink.Dispose()

            oSBOLink = Nothing

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
            m_oDebugTimings.EndBlock("bGis - RenSelection - CreateTaskLog")

            m_oDebugTimings.EndBlock("bGis - RenSelection - Total Time")

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in RenSelection."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewal
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenMtaAtRenewal(ByVal v_bTransactMtaRenewal As Boolean, ByVal v_lGISSchemeID As Integer, ByVal v_lGISPolicyLinkID As Integer, ByVal v_cOldAnnualPremium As Decimal, ByVal v_cNewAnnualPremium As Decimal, ByVal v_dtEffectiveDate As Date, ByVal v_sGisDataModelCode As String, ByRef r_bIsInRenewalCycle As Boolean, ByRef r_bResetRenewalRecord As Boolean, ByRef r_cNewRenewalPremiumIncIpt As Decimal, ByRef r_cOldRenewalPremiumIncIpt As Decimal) As Integer
        Dim result As Integer = 0
        Dim bUseXYZRule As Boolean
        Dim lInsuranceFolderCnt, lRenewalEDIAuditID, lPartyCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewal - Start")

            m_oDebugTimings.StartBlock()

            m_lReturn = RenMtaAtRenewalQuote(v_lGISPolicyLinkID:=v_lGISPolicyLinkID, v_cOldAnnualPremium:=v_cOldAnnualPremium, v_cNewAnnualPremium:=v_cNewAnnualPremium, v_dtEffectiveDate:=v_dtEffectiveDate, v_sGisDataModelCode:=v_sGisDataModelCode, r_bIsInRenewalCycle:=r_bIsInRenewalCycle, r_bResetRenewalRecord:=r_bResetRenewalRecord, r_cNewRenewalPremiumIncIpt:=r_cNewRenewalPremiumIncIpt, r_cOldRenewalPremiumIncIpt:=r_cOldRenewalPremiumIncIpt, r_bUseXYZRule:=bUseXYZRule, r_lInsuranceFolderCnt:=lInsuranceFolderCnt, r_lRenewalEdiAuditId:=lRenewalEDIAuditID, r_lPartyCnt:=lPartyCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewal")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewal - RenMtaAtRenewalQuote")

            If v_bTransactMtaRenewal And r_bResetRenewalRecord And r_bIsInRenewalCycle Then

                m_lReturn = RenMtaAtRenewalTransact(v_lGISPolicyLinkID:=v_lGISPolicyLinkID, v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lRenewalEdiAuditId:=lRenewalEDIAuditID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, r_bUseXYZRule:=bUseXYZRule, v_cOldAnnualPremium:=v_cOldAnnualPremium, v_cNewAnnualPremium:=v_cNewAnnualPremium, v_lPartyCnt:=lPartyCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewal")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            m_oDebugTimings.EndBlock("bGis - RenMtaAtRenewal - RenMtaAtRenewalTransact")

            m_oDebugTimings.EndBlock("bGis - RenMtaAtRenewal - Total Time")

            Return result

        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalQuote
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Private Function RenMtaAtRenewalQuote(ByVal v_lGISPolicyLinkID As Integer, ByVal v_cOldAnnualPremium As Decimal, ByVal v_cNewAnnualPremium As Decimal, ByVal v_dtEffectiveDate As Date, ByVal v_sGisDataModelCode As String, ByRef r_bIsInRenewalCycle As Boolean, ByRef r_bResetRenewalRecord As Boolean, ByRef r_cNewRenewalPremiumIncIpt As Object, ByRef r_cOldRenewalPremiumIncIpt As Object, ByRef r_bUseXYZRule As Boolean, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lRenewalEdiAuditId As Integer, ByRef r_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals

        Dim sRenewalStatusTypeCode As String = ""
        'Dim lRenewalEdiAuditId As Long
        Dim lIsInsurerLead, lRenewalAtMtaDayNum As Integer
        Dim dtXYZDate, dtRenewalDate As Date



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDebugTimings.StartBlock()
        m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalQuote - Start")

        m_oDebugTimings.StartBlock()


        ' Create a SBO Link object


        oSBOLink = New bSIRIUSLink.Renewals
        m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "RenMtaAtRenewalQuote")
            result = gPMConstants.PMEReturnCode.PMFalse
            oSBOLink = Nothing
            Return result
        End If


        ' Remove component services

        m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalQuote - Create Sirius Link Object")

        ' ***************************************************************** '
        ' Call the RenMtaAtRenewalQuote method on the Sirius Link Object
        ' ***************************************************************** '

        m_lReturn = oSBOLink.RenMtaAtRenewalQuote(v_lGisPolicyLinkId:=v_lGISPolicyLinkID, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_sRenewalStatusTypeCode:=sRenewalStatusTypeCode, r_lRenewalEdiAuditId:=r_lRenewalEdiAuditId, r_lIsInsurerLead:=lIsInsurerLead, r_lRenewalAtMtaDayNum:=lRenewalAtMtaDayNum, r_dtRenewalDate:=dtRenewalDate, r_lPartyCnt:=r_lPartyCnt)

        If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.RenMtaAtRenewalQuote Failed", ACApp, ACClass, "RenMtaAtRenewalQuote")
            result = gPMConstants.PMEReturnCode.PMFalse
            oSBOLink = Nothing
            Return result
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalQuote - oSBOLink.RenMtaAtRenewalQuote")

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Or sRenewalStatusTypeCode = PMRenewalStatusTypePreSelection Then
            'Nothing to do, policy is either pre-selected or not in the renewal cycle
            r_bIsInRenewalCycle = False
            r_bResetRenewalRecord = False
            r_bUseXYZRule = False
            Return result
        End If

        If sRenewalStatusTypeCode = PMRenewalStatusTypePolicyLapsed Or sRenewalStatusTypeCode = PMRenewalStatusTypePolicyRenewed Then
            'Nothing to do, policy has either renewed or lapsed
            r_bIsInRenewalCycle = False
            r_bResetRenewalRecord = False
            r_bUseXYZRule = False
            Return result
        End If

        If lIsInsurerLead = 0 Then
            ' If we are broker lead and have passed renewal pre selection
            r_bIsInRenewalCycle = True
            r_bResetRenewalRecord = True
            r_bUseXYZRule = False
            Return result
        End If

        If lIsInsurerLead = 1 And (sRenewalStatusTypeCode = PMRenewalStatusTypeRenewalSelected Or r_lRenewalEdiAuditId = 0) Then
            ' If we are insurer lead and have not yet received a quote
            r_bIsInRenewalCycle = True
            r_bResetRenewalRecord = True
            r_bUseXYZRule = False
            Return result
        End If

        dtXYZDate = dtRenewalDate.AddDays(-lRenewalAtMtaDayNum)
        'sj 19/09/2001 - start
        dtXYZDate = CDate(dtXYZDate.ToString("dd/MM/yyyy"))
        v_dtEffectiveDate = CDate(v_dtEffectiveDate.ToString("dd/MM/yyyy"))
        'sj 19/09/2001 - end

        If v_dtEffectiveDate < dtXYZDate Then
            ' We have not yet reached the point where the renewal record is
            ' recalculated
            r_bIsInRenewalCycle = True
            r_bResetRenewalRecord = True
            r_bUseXYZRule = False
            Return result
        End If

        ' Somebody set us up the BOM
        m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check if a BOM was created
        If Not (oBom Is Nothing) Then

            ' Call RenMtaAtRenewalQuote on the BOM to calculate the
            ' new renewal premium

            m_lReturn = oBom.RenMtaAtRenewalQuote(v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(r_lRenewalEdiAuditId), v_cOldAnnualPremium:=gPMFunctions.ToSafeDecimal(v_cOldAnnualPremium),
                                                  v_cNewAnnualPremium:=gPMFunctions.ToSafeDecimal(v_cNewAnnualPremium), r_cNewRenewalPremiumIncIpt:=r_cNewRenewalPremiumIncIpt,
                                                  r_cOldRenewalPremiumIncIpt:=r_cOldRenewalPremiumIncIpt)

            'SJD 16/9/2005 - If return is PMOK then Lapsed by Insurer
            If m_lReturn = gPMConstants.PMEReturnCode.PMOK Then

                oSBOLink.Dispose()
                oSBOLink = Nothing

                oBom.Dispose()
                oBom = Nothing
                r_bIsInRenewalCycle = False
                r_bResetRenewalRecord = False
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy the BOM

            oBom.Dispose()

            oBom = Nothing

        End If

        ' If we have got this far them we are recalculating the MTA premium
        r_bIsInRenewalCycle = True
        r_bResetRenewalRecord = True
        r_bUseXYZRule = True

        m_oDebugTimings.EndBlock("bGis - RenMtaAtRenewalQuote - oBom.RenMtaAtRenewalQuoteAfter")


        oSBOLink.Dispose()

        oSBOLink = Nothing

        m_oDebugTimings.EndBlock("bGis - RenMtaAtRenewalQuote - Total Time")

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalTransact
    '
    ' Description:
    '
    ' History: 11/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function RenMtaAtRenewalTransact(ByVal v_lGISPolicyLinkID As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal r_bUseXYZRule As Boolean, ByVal v_cOldAnnualPremium As Decimal, ByVal v_cNewAnnualPremium As Decimal, ByVal v_lPartyCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals
        Dim lRnlRecNo, cNewPremiumIncIpt, cNewPremium, cNewIptAmount As Object
        Dim sGisBusinessTypeCode As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDebugTimings.StartBlock()
        m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalTransact - Start")

        m_oDebugTimings.StartBlock()


        ' Create a SBO Link object


        oSBOLink = New bSIRIUSLink.Renewals
        m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "RenMtaAtRenewalTransact")
            result = gPMConstants.PMEReturnCode.PMFalse
            oSBOLink = Nothing
            Return result
        End If


        ' Remove component services

        m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalTransact - Create Sirius Link Object")

        If r_bUseXYZRule Then

            ' Somebody set us up the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalTransact - Create BOM")

            ' Check if a BOM was created
            If Not (oBom Is Nothing) Then

                ' Call RenMtaAtRenewalQuote on the BOM to calculate the
                ' new renewal premium

                m_lReturn = oBom.RenMtaAtRenewalTransact(v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), v_cOldAnnualPremium:=gPMFunctions.ToSafeDecimal(v_cOldAnnualPremium), v_cNewAnnualPremium:=gPMFunctions.ToSafeDecimal(v_cNewAnnualPremium),
                                                         r_lRnlRecNo:=lRnlRecNo, r_cNewPremiumIncIpt:=cNewPremiumIncIpt, r_cNewPremium:=cNewPremium, r_cNewIptAmount:=cNewIptAmount)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Get the business type code

                sGisBusinessTypeCode = oBom.GISBusinessTypeCode

                ' Destroy the BOM

                oBom.Dispose()

                oBom = Nothing
                m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalTransact - oBOM.RenMtaAtRenewalTransact")
            End If

            'Now call the QEM to update the premiums on the cobol renewal file

            m_lReturn = QEMRenMtaAtRenewal(v_lRnlRecNo:=lRnlRecNo, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_lGISPolicyLinkID:=v_lGISPolicyLinkID, v_lGISSchemeID:=v_lGISSchemeID, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=sGisBusinessTypeCode, v_cRenewalPremiumIncIpt:=cNewPremiumIncIpt, v_cRenewalPremium:=cNewPremium, v_cRenewalIpt:=cNewIptAmount)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "QEMRenMtaAtRenewal Failed", ACApp, ACClass, "RenMtaAtRenewalTransact")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                Return result
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenMtaAtRenewalTransact - Create BOM")

        End If


        ' ***************************************************************** '
        ' Call the RenMtaAtRenewalTransact method on the Sirius Link Object
        ' ***************************************************************** '
        'Change status on renewal control table to pre-renewal selected and set the
        'renewal edi audit id to null

        m_lReturn = oSBOLink.RenMtaAtRenewalTransact(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_bUseXYZRule:=r_bUseXYZRule, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.RenMtaAtRenewalTransact Failed", ACApp, ACClass, "RenMtaAtRenewalTransact")
            result = gPMConstants.PMEReturnCode.PMFalse
            oSBOLink = Nothing
            Return result
        End If

        m_oDebugTimings.EndBlock("bGis - RenMtaAtRenewalTransact - oSBOLink.RenMtaAtRenewalTransact")

        m_oDebugTimings.EndBlock("bGis - RenMtaAtRenewalTransact - Total Time")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotationInsurerLead
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenQuotationInsurerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String) As Integer
        Return RenQuotationInsurerLead(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_lGISSchemeID:=v_lGISSchemeID, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lProductID:=v_lProductID, v_dtRenewalDate:=v_dtRenewalDate, v_lPartyCnt:=v_lPartyCnt, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenQuotationInsurerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSet, sXMLDataSetDef, sXMLQuoteOutput As String
        Dim cPremium, cIPT As Object

        Dim oBom As Object
        Dim oSiriusLink As bSIRIUSLink.Renewals
        Dim oDataSet As Object = m_oDataSet

        Const LCRenewalsClass As String = "Renewals"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - LoadRisk")

            ' Set us up the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - CreateBOM")

            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenQuotationInsurerLeadBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenQuotationInsurerLeadBefore."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If

                m_oDataSet = oDataSet
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - oBom.RenQuotationInsurerLeadBefore")

            ' Process the QEM
            m_lReturn = QEMRenQuotationInsurerLead(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sXMLQuoteOutput:=sXMLQuoteOutput)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 291001 - log the message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to QEMRenQuotationInsurerLead for " & v_lInsuranceFolderCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process QEMRenQuotationInsurerLead."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - QEMRenQuotationInsurerLead")

            ' Sirius Link


            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' Remove component services

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result

                m_sRenTaskLog = "Failed to create bSiriusLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - Create SiriusLink")

            ' Call the BOM After method

            m_lReturn = oBom.RenQuotationInsurerLeadAfter(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet, r_cPremium:=cPremium, r_cIPT:=cIPT)

            ' SJD 12/9/2005 - If Return with PMOK then Quote Has been declined by Insurer
            If m_lReturn = gPMConstants.PMEReturnCode.PMOK Then


                oSiriusLink.Dispose()
                oSiriusLink = Nothing

                oBom.Dispose()
                oBom = Nothing
                Return gPMConstants.PMEReturnCode.PMTrue

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oBOM.RenQuotationInsurerLeadAfter."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDataSet = oDataSet

            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationInsurerLead - oBom.RenQuotationInsurerLeadAfter")

            ' Call RenQuotedInsurerLead

            m_lReturn = oSiriusLink.RenQuotedInsurerLead(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_cPremium:=cPremium, v_cIPT:=cIPT)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 291001 - log the message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call oSiriusLink.RenQuotedInsurerlead for " & v_lInsuranceFolderCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSiriusLink.RenQuotedInsurerlead."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.EndBlock("bGis - RenQuotationInsurerLead - oSiriusLink.RenQuotedInsurerlead")

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing


            oBom.Dispose()

            oBom = Nothing

            m_oDebugTimings.EndBlock("bGis - RenQuotationInsurerLead - Total Time")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationInsurerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in RenQuotationInsurerLed."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            Return result


        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LoadRisk
    '
    ' Description:
    '
    ' History: 27/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function LoadRisk(ByVal v_sGisDataModelCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_lInsuranceFileCnt As Integer) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".LoadRisk")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Load from DB
        m_lReturn = m_oGisApplication.LoadRiskFromDB(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataset:=r_sXMLDataset, r_sGISDataModelCode:=v_sGisDataModelCode, v_lInsuranceFileCnt:=r_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Create Data Set Control
        m_oDataSet = New cGISDataSetControl.Application()

        ' Load From XML
        m_lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=r_sXMLDataSetDef, v_sXMLDataSet:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".LoadRisk")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotationBrokerLead
    '
    ' Description:
    '
    ' History: 26/03/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenQuotationBrokerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer) As Integer
        Return RenQuotationBrokerLead(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenQuotationBrokerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSiriusLink As bSIRIUSLink.Renewals
        Dim cPremium, cIPT As Object

        Dim sXMLDataSet, sXMLDataSetDef, sXMLQuoteOutput As String

        ' Get these values in here
        Dim lGroupId As Integer
        Dim oDataSet As Object = m_oDataSet

        Const LCRenewalsClass As String = "Renewals"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - LoadRisk")

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - CreateBOM")

            ' Call RenQuotationBrokerLeadBefore
            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode

                'sGroupCode = oBOM.GroupCode

                'AK 251001  - need to call BrokerLeadBefore process here

                m_lReturn = oBom.RenQuotationBrokerLeadBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenQuotationBrokerLeadBefore."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If

                m_oDataSet = oDataSet

                lGroupId = oBom.GroupId

            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - oBom.RenQuotationBrokerLeadBefore")

            ' Call the QEM
            'TF151001 - Pass the current schemeID for lower function.
            m_lReturn = QEMRenQuotationBrokerLead(v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sXMLQuoteOutput:=sXMLQuoteOutput, v_lGroupId:=lGroupId, v_lGISSchemeID:=v_lGISSchemeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 291001 - log the message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call RenQuotationBrokerLead for " & v_lInsuranceFolderCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process QEMRenQuotationBrokerLead."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - QEMRenQuotationBrokerLead")

            ' Call RenQuotationBrokerLeadAfter
            If Not (oBom Is Nothing) Then
                'AK 021001 - added another parameter for this call

                m_lReturn = oBom.RenQuotationBrokerLeadAfter(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet, r_cPremium:=cPremium, r_cIPT:=cIPT, v_lSchemeID:=gPMFunctions.ToSafeInteger(v_lGISSchemeID))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenQuotationBrokerLeadAfter."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If

                m_oDataSet = oDataSet
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - oBom.RenQuotationBrokerLeadAfter")

            ' Call RenQuotedBrokerLead


            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' Remove component services

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                m_sRenTaskLog = "Failed to create bSiriusLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenQuotationBrokerLead - CreateSiriusLink")

            ' Call RenQuotedBrokerLead

            m_lReturn = oSiriusLink.RenQuotedBrokerLead(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_cPremium:=cPremium, v_cIPT:=cIPT)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)


                m_sRenTaskLog = "Failed to process oSiriusLink.RenQuotedBrokerLead."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.EndBlock("bGis - RenQuotationBrokerLead - oSiriusLink.RenQuotedBrokerlead")

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing

            If Not (oBom Is Nothing) Then

                oBom.Dispose()

                oBom = Nothing
            End If

            m_oDebugTimings.EndBlock("bGis - RenQuotationBrokerLead - Total Time")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in RenQuotationBrokerLed."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)

            Return result


        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: QEMRenInvitation
    '
    ' Description:
    '
    ' History: 14/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenInvitation(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, Optional ByVal v_bBatchRun As Boolean = False, Optional ByRef v_lPartyCnt As Integer = 0, Optional ByRef v_sAgentDoc As String = "", Optional ByRef r_bDocumentNotLinked As Object = False) As Integer

        'AMJ 071102 added v_bBatchRun for batch processing
        'AMJ 111102 added party count for doc production
        'KB 22012003 added agent doc  for doc production
        '                    selection by agent


        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".QEMRenInvitation")


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the name of the mapper
        ' Note to self: Change this to a constant
        ' Note to self: Helicopters cant fly upside down

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)
        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenInvitation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the function
        'IJR 2002-12-13 Start
        'bGIIVbToCobol has only 3 parameters
        If v_sGisDataModelCode.StartsWith("GII") Or v_sGisBusinessTypeCode.StartsWith("GII") Then

            m_lReturn = oQEM.RenInvitation(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet)

        Else
            ' pass on value for v_bBatchRun & v_sAgentDoc
            If sObject.ToUpper() = "BGISQEMCNC.RENEWALS" Then
                If Not v_bBatchRun Then

                    m_lReturn = oQEM.RenInvitation(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet, v_sAgentDoc:=gPMFunctions.ToSafeString(v_sAgentDoc), r_bDocumentNotLinked:=r_bDocumentNotLinked)
                Else

                    m_lReturn = oQEM.RenInvitation(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet, v_bBatchRun:=gPMFunctions.ToSafeBoolean(v_bBatchRun), v_sAgentDoc:=gPMFunctions.ToSafeString(v_sAgentDoc), r_bDocumentNotLinked:=r_bDocumentNotLinked)
                End If
            Else
                If Not v_bBatchRun Then

                    m_lReturn = oQEM.RenInvitation(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet, v_sAgentDoc:=gPMFunctions.ToSafeString(v_sAgentDoc))
                Else

                    m_lReturn = oQEM.RenInvitation(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet, v_bBatchRun:=gPMFunctions.ToSafeBoolean(v_bBatchRun), v_sAgentDoc:=gPMFunctions.ToSafeString(v_sAgentDoc))
                End If
            End If
        End If
        'IJR 2002-12-13 End

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDataSet = oDataSet

        ' Don't do anything with it?

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".QEMRenInvitation")

        Return result

Err_QEMRenInvitation:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenInvitation")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenInvitation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenInvitation", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function


    Private Function QEMRenConfDocsHoldingInsurer(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".QEMRenConfDocsHoldingInsurer")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the name of the mapper
        ' Note to self: Change this to a constant
        ' Note to self: Helicopters cant fly upside down

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)

        Catch
        End Try

        ' Reset the error trapping


        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenConfDocsHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=gPMFunctions.ToSafeString(ACApp), vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the function

        m_lReturn = oQEM.RenConfDocsHoldingInsurer(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDataSet = oDataSet

        ' Don't do anything with it?

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".QEMRenConfDocsHoldingInsurer")

        Return result

Err_QEMRenConfDocsHoldingInsurer:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenConfDocsHoldingInsurer")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenConfDocsHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenConfDocsHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: QEMRenMtaAtRenewal
    '
    ' Description:
    '
    ' History: 14/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenMtaAtRenewal(ByVal v_lRnlRecNo As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISPolicyLinkID As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_cRenewalPremiumIncIpt As Decimal, ByVal v_cRenewalPremium As Decimal, ByVal v_cRenewalIpt As Decimal) As Integer

        Dim result As Integer = 0
        Dim lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray(,) As Object


        result = gPMConstants.PMEReturnCode.PMTrue


        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=DateTime.Today)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)

        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenMtaAtRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID),
                                    iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the function

        m_lReturn = oQEM.RenMtaAtRenewal(v_lRnlRecNo:=gPMFunctions.ToSafeInteger(v_lRnlRecNo), v_cRenewalPremiumIncIpt:=gPMFunctions.ToSafeDecimal(v_cRenewalPremiumIncIpt), v_cRenewalPremium:=gPMFunctions.ToSafeDecimal(v_cRenewalPremium),
                                         v_cRenewalIpt:=gPMFunctions.ToSafeDecimal(v_cRenewalIpt), v_lGISPolicyLinkID:=gPMFunctions.ToSafeInteger(v_lGISPolicyLinkID), v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        Return result

Err_QEMRenMtaAtRenewal:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenMtaAtRenewal")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenMtaAtRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenMtaAtRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitationInsurerLed
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created
    '          21/08/2001 IJM - Changed to RenInvitationInsurerLed
    '          'sj 30/10/2001 - Offer alternate flag passed to Gis
    ' ***************************************************************** '
    Public Function RenInvitationInsurerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Return RenInvitationInsurerLed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:="", v_iOfferAlt:=0)
    End Function

    Public Function RenInvitationInsurerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_iOfferAlt As Integer) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals


        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - LoadRisk")

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - CreateBOM")

            ' BOM.RenInvitationInsurerLedBefore
            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenInvitationBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), v_iOfferAlt:=gPMFunctions.ToSafeInteger(v_iOfferAlt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenInvitationBefore."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - oBOM.RenInvitationInsurerLedBefore")

            ' QEM.RenInvitationInsurerLed
            m_lReturn = QEMRenInvitation(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process QEMRenInvitation."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - QEMRenInvitationInsurerLed")

            ' BOM.RenInvitationInsurerLedAfter
            If Not (oBom Is Nothing) Then

                m_lReturn = oBom.RenInvitationAfter()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenInvitationAfter."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - oBOM.RenInvitationInsurerLedAfter")

            ' New component services

            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create bSIRIUSLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationInsurerLed - Create SiriusLink")

            ' Remove component services

            ' Call Sirius Link to update the Control file and events

            m_lReturn = oSBOLink.RenInvited(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_vRenewalEDIAuditID:=v_lRenewalEdiAuditId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSBOLink.RenInvited."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.EndBlock("bGis - RenInvitationInsurerLed - oSBOLink.RenInvited")

            ' Clear up

            oSBOLink.Dispose()
            ' Remove the instance
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenInvitationInsurerLed - Total Time ")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitationInsurerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitationInsurerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in RenInvitationInsurerLed."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenReprintInvitationInsurerLed
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created
    '          21/08/2001 IJM - Changed to RenReprintInvitationInsurerLed
    '
    ' ***************************************************************** '
    Public Function RenReprintInvitationInsurerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Return RenReprintInvitationInsurerLed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:="", v_iOfferAlt:=0)
    End Function

    Public Function RenReprintInvitationInsurerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_iOfferAlt As Integer) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals


        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - LoadRisk")

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - CreateBOM")

            ' BOM.RenReprintInvitationInsurerLedBefore
            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenInvitationBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), v_iOfferAlt:=gPMFunctions.ToSafeInteger(v_iOfferAlt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - oBOM.RenReprintInvitationInsurerLedBefore")

            ' QEM.RenReprintInvitationInsurerLed
            m_lReturn = QEMRenInvitation(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - QEMRenReprintInvitationInsurerLed")

            ' BOM.RenReprintInvitationInsurerLedAfter
            If Not (oBom Is Nothing) Then

                m_lReturn = oBom.RenInvitationAfter()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - oBOM.RenReprintInvitationInsurerLedAfter")

            ' New component services

            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationInsurerLed - Create SiriusLink")

            ' Remove component services

            ' Call Sirius Link to update the events

            m_lReturn = oSBOLink.RenReprintInvitation(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_vRenewalEDIAuditID:=v_lRenewalEdiAuditId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.EndBlock("bGis - RenReprintInvitationInsurerLed - oSBOLink.RenInvited")

            ' Clear up

            oSBOLink.Dispose()
            ' Remove the instance
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenReprintInvitationInsurerLed - Total Time ")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintInvitationInsurerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintInvitationInsurerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: GetSTSCommonData
    '
    ' Description: gets the STS Common data
    '
    ' History: 31/03/2004 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function GetSTSCommonData(ByVal v_lInsuranceFileCnt As Integer, ByRef r_sCommonXML As String, ByRef r_oDOM As XmlDocument) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Dim oDOM As New XmlDocument

        Const ACGetSTSCommonDataSQL As String = "spu_STS_GetEDICommonData"
        Const ACGetSTSCommonDataStored As Boolean = True



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add the new insurance file parameter
        lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        ' Call the stored procedure to return the XML
        lReturn = m_oDatabase.SQLSelectForXML(sSQL:=ACGetSTSCommonDataSQL, bStoredProcedure:=ACGetSTSCommonDataStored, oXMLDOM:=r_oDOM)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Return the XML value
        r_sCommonXML = r_oDOM.InnerXml

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: CreateEDIXML
    '
    ' Description: Creates an EDI XML string
    '
    ' Inputs: XML Template
    '
    ' Outputs: New XML refined to the invite XML
    '
    ' History: 25/03/2004 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function CreateEDIXML(ByVal v_sInviteXML As String, ByVal v_sFullXML As String, ByVal v_sNewSchemaFilename As String, ByVal v_lInsuranceFileCnt As Integer, ByVal v_sGisDataModelCode As String, ByRef r_sNewXML As String, Optional ByVal v_bResendMessage As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim bFound As Boolean
        Dim sCommonXML As String = ""
        Dim lReturn As Integer
        Dim sDate As String = ""

        Dim oXMLA As New XmlDocument
        Dim oXMLB As New XmlDocument
        Dim oNodeList, oCommonNodeList As XmlNodeList
        Dim oNode As XmlNode
        Dim oTargetNodes As XmlNodeList
        Dim oAttribA, oAttribB As XmlAttribute
        Dim oNewDoc As New XmlDocument
        Dim oNewNode As XmlNode
        Dim oCommonDOM As New XmlDocument
        Dim oLookupProperties As Hashtable
        Dim sObjectName, sKey, sAbiCode As String
        Const LCPMLookup As String = "2"



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Try the thing thats most likely to fall over first, getting the back office data
        lReturn = GetSTSCommonData(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_sCommonXML:=sCommonXML, r_oDOM:=oCommonDOM)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Initilise list manager
        m_lReturn = InitialiseListManager(v_sDataModel:=v_sGisDataModelCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseListManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEDIXML")
            Return result
        End If

        'Get a list of all the object/properties which are gis lookups
        m_lReturn = GetLookupProperties(v_sGisDataModelCode:=v_sGisDataModelCode, r_oLookupProperties:=oLookupProperties)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLookupProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEDIXML")
            Return result
        End If

        'Replace the lookup descriptions in the common data with abi codes
        sObjectName = "STS_EDI_COMMON"

        oCommonNodeList = oCommonDOM.SelectNodes("STS_EDI_COMMON")
        For iLoop1 As Integer = oCommonNodeList.Count - 1 To 0 Step -1
            oNode = oCommonNodeList.Item(iLoop1)
            For Each oAttribB2 As XmlAttribute In oNode.Attributes
                oAttribB = oAttribB2
                'Check to see if this is a gis lookup
                sKey = BuildLookupPropertyKey(sObjectName, oAttribB.LocalName)
                If oLookupProperties.ContainsKey(sKey) Then
                    'Convert the description back to an Abi Code

                    m_lReturn = m_oListManager.GetABICodeFromDescription(v_sPropertyId:=oLookupProperties.Item(sKey), v_sDescription:=oAttribB.Value.Trim(), r_sABICode:=sAbiCode)
                    oAttribB.Value = sAbiCode
                End If
            Next oAttribB2
        Next iLoop1

        ' Load the schemas
        oXMLA.LoadXml(v_sInviteXML)
        oXMLB.LoadXml(v_sFullXML)

        ' Check for errors loading the XML

        oNodeList = oXMLB.SelectNodes("/DATA_SET/RISK_OBJECTS//*")

        If oNodeList.Count = 0 Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="FullXML is not a valid GIS DataSet : " & v_sFullXML, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEDIXML")
            Return result
        End If

        '**************************************************************************************
        'Set the missing/incorrect attributes on <DMC>_POLICY/STS_EDI_COMMON

        'if this is not re-send then because the last_edi_message_count_sent gets incremented later
        'on STS_EDI_COMMON.CON_EDI_NEW_VER_NO has the old value so we need to increment it here
        If Not v_bResendMessage Then
            oNode = oCommonDOM.SelectSingleNode("STS_EDI_COMMON")
            oNode.Attributes.GetNamedItem("CON_EDI_NEW_VER_NO").InnerText = CStr(CInt(oNode.Attributes.GetNamedItem("CON_EDI_NEW_VER_NO").InnerText) + 1)
        End If

        'Needs to be done before the following loop as the loop deletes some of the source attributes we are going to use
        oNode = oXMLB.SelectSingleNode("/DATA_SET/RISK_OBJECTS/" & v_sGisDataModelCode.ToUpper() & "_POLICY_BINDER/" & v_sGisDataModelCode.ToUpper() & "_POLICY")
        'Set <DMC>_POLICY.CON_EDI_LAST_RCV_EVENT from <DMC>_POLICY.CON_EDI_LAST_SND_EVENT
        oAttribA = oNode.Attributes.GetNamedItem("CON_EDI_LAST_SND_EVENT")
        If oAttribA Is Nothing Then
            'we are in trouble, create the attribute and default the value
            oAttribA = oXMLB.CreateAttribute("CON_EDI_LAST_RCV_EVENT")

            oNode.Attributes.SetNamedItem(oAttribA)
            oNode.Attributes.GetNamedItem("CON_EDI_LAST_SND_EVENT").InnerText = "Proposal"
        Else
        End If
        oAttribA = oNode.Attributes.GetNamedItem("CON_EDI_LAST_RCV_EVENT")
        If oAttribA Is Nothing Then
            oAttribA = oXMLB.CreateAttribute("CON_EDI_LAST_RCV_EVENT")

            oNode.Attributes.SetNamedItem(oAttribA)
        End If
        oNode.Attributes.GetNamedItem("CON_EDI_LAST_RCV_EVENT").InnerText = oNode.Attributes.GetNamedItem("CON_EDI_LAST_SND_EVENT").InnerText

        'Set <DMC>_POLICY.CON_EDI_NEW_VER_NO from STS_EDI_COMMON.CON_EDI_NEW_VER_NO
        oAttribA = oNode.Attributes.GetNamedItem("CON_EDI_NEW_VER_NO")
        If oAttribA Is Nothing Then
            oAttribA = oXMLB.CreateAttribute("CON_EDI_NEW_VER_NO")

            oNode.Attributes.SetNamedItem(oAttribA)
        End If
        oNode.Attributes.GetNamedItem("CON_EDI_NEW_VER_NO").InnerText = oCommonDOM.SelectSingleNode("STS_EDI_COMMON").Attributes.GetNamedItem("CON_EDI_NEW_VER_NO").InnerText

        'Set <DMC>_POLICY.CON_EDI_OLD_VER_NO from STS_EDI_COMMON.CON_EDI_OLD_VER_NO
        oAttribA = oNode.Attributes.GetNamedItem("CON_EDI_OLD_VER_NO")
        If oAttribA Is Nothing Then
            oAttribA = oXMLB.CreateAttribute("CON_EDI_OLD_VER_NO")

            oNode.Attributes.SetNamedItem(oAttribA)
        End If
        oNode.Attributes.GetNamedItem("CON_EDI_OLD_VER_NO").InnerText = oCommonDOM.SelectSingleNode("STS_EDI_COMMON").Attributes.GetNamedItem("CON_EDI_OLD_VER_NO").InnerText


        '**************************************************************************************
        For iLoop1 As Integer = oNodeList.Count - 1 To 0 Step -1

            oNode = oNodeList.Item(iLoop1)

            ' Find it?
            oTargetNodes = oXMLA.GetElementsByTagName(oNode.LocalName)
            sObjectName = oNode.LocalName

            ' 0 = its not in the target, so remove it
            If oTargetNodes.Count = 0 Then
                oNode.ParentNode.RemoveChild(oNode)
            Else
                ' otherwise we need to compare the attributes
                ' go through each in B and check its in A
                For Each oAttribB In oNode.Attributes
                    bFound = False
                    For Each oAttribA2 As XmlAttribute In oTargetNodes.Item(0).Attributes
                        oAttribA = oAttribA2
                        If oAttribA.LocalName = oAttribB.LocalName Then
                            bFound = True
                            If Informations.IsDate(oAttribB.Value) Then
                                sDate = oAttribB.Value.Trim()
                                oAttribB.Value = sDate.Replace(" "c, "T"c)
                            End If

                            'Check to see if this is a lookup
                            sKey = BuildLookupPropertyKey(sObjectName, oAttribB.LocalName)
                            If oLookupProperties.ContainsKey(sKey) Then

                                'Is this a UDL lookup

                                If CStr(oLookupProperties.Item(sKey & "|TYPE")) = LCPMLookup Then

                                    ' CJB 101104 Convert the id back to a code from the UDL


                                    m_lReturn = GetPMLookupCodeFromID(v_sLookupTable:=oLookupProperties.Item(sKey), v_sLookupId:=oAttribB.Value.Trim(), r_sLookupCode:=sAbiCode)

                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                        bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupCodeFromID Failed for " & CStr(oLookupProperties.Item(sKey)) & " with value " & oAttribB.Value.Trim(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEDIXML", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                        oAttribB.Value = ""
                                    Else
                                        oAttribB.Value = sAbiCode
                                    End If

                                Else
                                    'Treat as GIS lookups
                                    'Convert the description back to an Abi Code


                                    m_lReturn = m_oListManager.GetABICodeFromDescription(v_sPropertyId:=oLookupProperties.Item(sKey), v_sDescription:=oAttribB.Value.Trim(), r_sABICode:=sAbiCode)
                                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                                        bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oListManager.GetABICodeFromDescription Failed for " & CStr(oLookupProperties.Item(sKey)) & " with value " & oAttribB.Value.Trim(), vApp:=ACApp, vClass:=ACClass, vMethod:="CreateEDIXML", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                                        oAttribB.Value = ""
                                    Else
                                        oAttribB.Value = sAbiCode
                                    End If
                                End If
                            End If

                            Exit For
                        End If
                    Next oAttribA2
                    If Not bFound Then
                        ' remove it
                        oNode.Attributes.RemoveNamedItem(oAttribB.LocalName)
                    End If
                Next oAttribB
            End If
        Next

        oNode = oXMLB.SelectSingleNode("/DATA_SET/RISK_OBJECTS/*")

        ' Create a new element
        oNewNode = oNewDoc.CreateNode(XmlNodeType.Element, "STS", "")

        oNewDoc.AppendChild(oNewNode)

        oNewNode.AppendChild(oNode)

        ' create a new one
        oAttribA = oNewDoc.CreateAttribute("xmlns:xs")
        oAttribA.Value = "http://www.w3.org/2001/XMLSchema-instance"

        oNewNode.Attributes.SetNamedItem(oAttribA)

        oAttribA = oNewDoc.CreateAttribute("xs:noNamespaceSchemaLocation")
        oAttribA.Value = v_sNewSchemaFilename

        oNewNode.Attributes.SetNamedItem(oAttribA)

        oNewNode = oNewDoc.SelectSingleNode("/STS")
        oNewNode.AppendChild(oCommonDOM.FirstChild)

        ' Return the XML
        r_sNewXML = oNewDoc.InnerXml


        oXMLA = Nothing
        oXMLB = Nothing
        oNodeList = Nothing
        oNode = Nothing
        oTargetNodes = Nothing
        oAttribA = Nothing
        oAttribB = Nothing
        oNewDoc = Nothing
        oNewNode = Nothing
        oCommonDOM = Nothing
        oLookupProperties = Nothing
        oCommonNodeList = Nothing

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitationEDI
    '
    ' Description:
    '
    ' History: 01/04/2004 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitationEDI(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer) As Integer
        Return RenInvitationEDI(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_sGisBusinessTypeCode:="", v_bResendMessage:=False)
    End Function

    Public Function RenInvitationEDI(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer
        Return RenInvitationEDI(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_bResendMessage:=False)
    End Function

    Public Function RenInvitationEDI(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_bResendMessage As Boolean) As Integer

        Dim result As Integer = 0
        Dim sDef, sDS As String
        Dim oSBOLink As bSIRIUSLink.Renewals
        Dim oDOM As XmlDocument
        Dim sInviteXML, sTemplateXML, sTemplateFile, sFolder, sFilename, sNewSchemaFilename As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the registry setting for the xml folder
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:="XMLRenInviteFolder", r_sSettingValue:=sFolder, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sFolder = "") Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log the error in the task log
                m_sRenTaskLog = "Failed to find registry setting for XMLRenInviteFolder for datamodel " & v_sGisDataModelCode
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Get the registry setting for the template xml
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:="XMLRenInviteTemplate", r_sSettingValue:=sTemplateFile, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sTemplateFile = "") Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log the error in the task log
                m_sRenTaskLog = "Failed to find registry setting for XMLRenInviteTemplate for datamodel " & v_sGisDataModelCode
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Get the registry setting for the schema file name
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:="XMLRenInviteSchemaName", r_sSettingValue:=sNewSchemaFilename, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer)
            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (sTemplateFile = "") Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log the error in the task log
                m_sRenTaskLog = "Failed to find registry setting for XMLRenInviteSchemaName for datamodel " & v_sGisDataModelCode
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Load the Risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sDef, r_sXMLDataset:=sDS, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Load the Template XML

            oDOM = New XmlDocument()


            'oDOM.async = False
            Try
                oDOM.Load(sTemplateFile)

            Catch
            End Try



            'If oDOM.parseError.errorCode <> 0 Then
            '	result = gPMConstants.PMEReturnCode.PMFalse
            '	' Log the error in the task log

            '	m_sRenTaskLog = "Failed to load XML template (" & sTemplateFile & "). Reason = " & oDOM.parseError.Message
            '	m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt, AC_InviteID, AC_Invite, gPMConstants.PMEReturnCode.PMError, m_sRenTaskLog)
            '	Return result


            ' Grab the XML
            sTemplateXML = oDOM.InnerXml

            ' remove the DOM object
            oDOM = Nothing

            ' Create the XML
            m_lReturn = CreateEDIXML(v_sInviteXML:=sTemplateXML, v_sFullXML:=sDS, v_sNewSchemaFilename:=sNewSchemaFilename, v_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_sGisDataModelCode:=v_sGisDataModelCode, r_sNewXML:=sInviteXML, v_bResendMessage:=v_bResendMessage)

            ' Now save it to the XML file store
            oDOM = New XmlDocument()

            Try
                oDOM.LoadXml(sInviteXML)

            Catch
            End Try



            'If oDOM.parseError.errorCode <> 0 Then
            '	' error in the XML somehow
            '	result = gPMConstants.PMEReturnCode.PMFalse
            '	m_sRenTaskLog = "Failed to load XML to save: " & sInviteXML
            '	m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt, AC_InviteID, AC_Invite, gPMConstants.PMEReturnCode.PMError, m_sRenTaskLog)
            '	Return result


            If Not sFolder.EndsWith("\") Then
                sFolder = sFolder & "\"
            End If

            ' Construct a filename
            ' INVITE_<DATAMODELCODE>_<INSURANCEFOLDERCNT>_<TIME>.XML
            sFilename = sFolder & "INVITE_" & v_sGisDataModelCode & "_" & CStr(v_lInsuranceFolderCnt) & "_" & DateTime.Now.ToString("HH:mm:ss").Replace(":", "") & ".XML"

            oDOM.Save(sFilename)

            oDOM = Nothing

            ' Update the renewal status
            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create bSIRIUSLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationEDI - Create SiriusLink")

            ' Call Sirius Link to update the Control file and events

            m_lReturn = oSBOLink.RenInvitedEdi(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_bResendMessage:=v_bResendMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSBOLink.RenInvitedEDI."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.EndBlock("bGis - RenInvitationEDI - oSBOLink.RenInvitedEDI")


            oSBOLink.Dispose()
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenInvitationEDI - Total Time ")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=gPMFunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitationEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitationEDI", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitationBrokerLed
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created
    '          21/08/2001 IJM - Changed to RenInvitationBrokerLed
    '          15/01/2002 DD  - Added Call to QEM for document production
    '
    ' ***************************************************************** '
    Public Function RenInvitationBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer
        Return RenInvitationBrokerLed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lBatchRun:=gPMConstants.PMEReturnCode.PMFalse, v_sAgentDoc:="", v_lAgentCnt:=0, r_bDocumentNotLinked:=False)
    End Function

    Public Function RenInvitationBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String, ByRef r_bDocumentNotLinked As Boolean) As Integer
        Return RenInvitationBrokerLed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lBatchRun:=gPMConstants.PMEReturnCode.PMFalse, v_sAgentDoc:="", v_lAgentCnt:=0, r_bDocumentNotLinked:=r_bDocumentNotLinked)
    End Function

    Public Function RenInvitationBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Object, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String, ByVal v_lBatchRun As Integer, ByVal v_sAgentDoc As String, ByVal v_lAgentCnt As Integer, ByRef r_bDocumentNotLinked As Boolean) As Integer
        'AMJ 071102 added v_lBatchRun for batch processing
        'KB 22012003 added agent doc
        Dim result As Integer = 0
        Dim bBatchRun As Boolean

        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals

        Dim sBom As String = ""

        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AMJ071102
            bBatchRun = (v_lBatchRun) Or (v_lBatchRun = gPMConstants.PMEReturnCode.PMTrue)

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - LoadRisk")

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - CreateBOM")

            ' BOM.RenInvitationBrokerLedBefore
            If Not (oBom Is Nothing) Then

                'DD 10/05/2002: Retrieve Business Type from BOM

                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode

                m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegBOMRequired, r_sSettingValue:=sBom, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer)

                If sBom = "SFB" Then

                    oBom.fromBatchTrans = v_lBatchRun
                    If False Then

                        oBom.AgentCnt = 0
                    Else

                        oBom.AgentCnt = v_lAgentCnt
                    End If
                End If


                m_lReturn = oBom.RenInvitationBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=r_lRenewalInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenInvitationBefore."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - oBOM.RenInvitationBrokerLedBefore")

            ' QEM.RenInvitationBrokerLed
            m_lReturn = QEMRenInvitation(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_bBatchRun:=bBatchRun, v_lPartyCnt:=v_lPartyCnt, v_sAgentDoc:=v_sAgentDoc, r_bDocumentNotLinked:=r_bDocumentNotLinked)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process QEMRenInvitation."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - QEMRenInvitationBrokerLed")

            ' BOM.RenInvitationBrokerLedAfter
            If Not (oBom Is Nothing) Then

                m_lReturn = oBom.RenInvitationAfter()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenInvitationAfter."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - oBOM.RenInvitationBrokerLedAfter")

            ' New component services

            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create bSIRIUSLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitationBrokerLed - Create SiriusLink")

            ' Remove component services

            ' Call Sirius Link to update the Control file and events

            m_lReturn = oSBOLink.RenInvited(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_vRenewalEDIAuditID:=v_lRenewalEdiAuditId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSBOLink.RnInvited."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.EndBlock("bGis - RenInvitationBrokerLed - oSBOLink.RenInvited")

            ' Clear up

            oSBOLink.Dispose()
            ' Remove the instance
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenInvitationBrokerLed - Total Time ")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitationBrokerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitationBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in RenInvitationBrokerLed."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenMultipleInvitationBrokerLed
    '
    ' Description:  Processes the Invitations for an array of
    '               Renewal Insurance Folders returning an array of
    '               failures. This is used by iGISSellerTool to maximise
    '               performance from a web page.
    '
    ' History: 07/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenMultipleInvitationBrokerLed(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef v_vSelectedArray As Object, ByRef r_vFailedArray(,) As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim oRenewals, vControlRecord As Object
            Dim iFailures As Integer

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Business Object

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oRenewals, v_sClassName:="bSIRRenewalControl.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Perform the action for each Folder
            iFailures = 0

            For Each vFolder As Object In v_vSelectedArray

                m_lReturn = oRenewals.GetRenewalControl(vFolder, vControlRecord)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'General failure
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                m_lReturn = RenInvitationBrokerLed(v_lInsuranceFolderCnt:=bz(CByte(vControlRecord(0, 0))), v_lPartyCnt:=bz(CByte(vControlRecord(9, 0))), v_dtRenewalDate:=CDate(vControlRecord(8, 0)), v_lRiskCodeID:=bz(CByte(vControlRecord(10, 0))), v_lGISDataModelID:=bz(CByte(vControlRecord(11, 0))), v_sGisDataModelCode:=v_sDataModelCode, v_lGISSchemeID:=bz(CByte(vControlRecord(1, 0))), v_lProductID:=bz(CByte(vControlRecord(7, 0))), r_lRenewalInsuranceFileCnt:=bz(CByte(vControlRecord(5, 0))), v_lRenewalGISSchemeID:=bz(CByte(vControlRecord(4, 0))), v_lRenewalEdiAuditId:=bz(CByte(vControlRecord(12, 0))), v_sGisBusinessTypeCode:=v_sBusinessTypeCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Add the entry to the failure array
                    iFailures += 1
                    If Informations.IsArray(r_vFailedArray) Then
                        ReDim Preserve r_vFailedArray(1, iFailures - 1)
                    Else
                        ReDim r_vFailedArray(1, 0)
                    End If


                    r_vFailedArray(0, iFailures - 1) = vControlRecord(16, 0)

                    r_vFailedArray(1, iFailures - 1) = "Error: " & m_sRenTaskLog
                End If

            Next vFolder


            oRenewals.Dispose()
            oRenewals = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMultipleInvitationBrokerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMultipleInvitationBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenReprintInvitationBrokerLead
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created
    '          21/08/2001 IJM - Changed to RenReprintInvitationBrokerLead
    '
    ' ***************************************************************** '
    Public Function RenReprintInvitationBrokerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Return RenReprintInvitationBrokerLead(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenReprintInvitationBrokerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals


        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationBrokerLead - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationBrokerLead - LoadRisk")

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationBrokerLead - CreateBOM")

            ' BOM.RenReprintInvitationBrokerLeadBefore
            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenInvitationBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationBrokerLead - oBOM.RenReprintInvitationBrokerLeadBefore")

            ' BOM.RenReprintInvitationBrokerLeadAfter
            If Not (oBom Is Nothing) Then

                m_lReturn = oBom.RenInvitationAfter()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationBrokerLead - oBOM.RenReprintInvitationBrokerLeadAfter")

            ' New component services

            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintInvitationBrokerLead - Create SiriusLink")

            ' Remove component services

            ' Call Sirius Link to update the Control file and events

            m_lReturn = oSBOLink.RenReprintInvitation(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_vRenewalEDIAuditID:=v_lRenewalEdiAuditId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.EndBlock("bGis - RenReprintInvitationBrokerLead - oSBOLink.RenInvited")

            ' Clear up

            oSBOLink.Dispose()
            ' Remove the instance
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenReprintInvitationBrokerLead - Total Time ")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintInvitationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintInvitationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitePreferredQuotes
    '
    ' Description:
    '
    ' History: 11/09/2001 SJ - Created

    '
    ' ***************************************************************** '
    Public Function RenInvitePreferredQuotes(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer) As Integer
        Return RenInvitePreferredQuotes(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenInvitePreferredQuotes(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer


        Dim result As Integer = 0
        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals


        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitePreferredQuotes - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitePreferredQuotes - LoadRisk")

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitePreferredQuotes - CreateBOM")

            ' BOM.RenInvitePreferredQuotesBefore
            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenInvitePreferredQuotesBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitePreferredQuotes - oBOM.RenInvitePreferredQuotesBefore")

            ' New component services

            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenInvitePreferredQuotes - Create SiriusLink")

            ' Remove component services

            ' Call Sirius Link to update the Control file and events

            m_lReturn = oSBOLink.RenInvitePreferredQuotes(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.EndBlock("bGis - RenInvitePreferredQuotes - oSBOLink.RenInvited")

            ' Clear up

            oSBOLink.Dispose()
            ' Remove the instance
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenInvitePreferredQuotes - Total Time ")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitePreferredQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenConfDocsHoldingInsurer
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenConfDocsHoldingInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Return RenConfDocsHoldingInsurer(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, v_lProductID:=v_lProductID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenConfDocsHoldingInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByVal v_lProductID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Const LCRenewalsClass As String = "Renewals"

        Dim oBom As Object
        Dim oSBOLink As bSIRIUSLink.Renewals


        Dim sXMLDataSetDef, sXMLDataSet As String

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenConfDocsHoldingInsurer - Start")

            m_oDebugTimings.StartBlock()

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - CreateBom")

            If Not (oBom Is Nothing) Then

                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode
            Else
                Return result
            End If

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenConfDocsHoldingInsurer - LoadRisk")


            ' QEM.RenConfDocsHoldingInsurer
            m_lReturn = QEMRenConfDocsHoldingInsurer(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenConfDocsHoldingInsurer - QEMRenConfDocsHoldingInsurer")


            ' New component services

            ' Get an instance of Sirius Link

            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenConfDocsHoldingInsurer - Create SiriusLink")

            ' Remove component services

            ' Call Sirius Link to update the Control file and events

            m_lReturn = oSBOLink.RenConfDocsHoldingInsurer(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_vRenewalEDIAuditID:=v_lRenewalEdiAuditId)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.EndBlock("bGis - RenConfDocsHoldingInsurer - oSBOLink.RenInvited")

            ' Clear up

            oSBOLink.Dispose()
            ' Remove the instance
            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenConfDocsHoldingInsurer - Total Time ")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenConfDocsHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenConfDocsHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewal
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '          26/10/2001 SJ - Add IsWhatIfQ parameter
    '          22/11/01   AK - Added v_bAutoConfirm parameter to handle autoconfirm cases
    ' ***************************************************************** '
    Public Function ConfirmRenewal(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer
        Return ConfirmRenewal(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSchemeID:=v_lSchemeID, v_lPartyCnt:=v_lPartyCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_bIsWhatIfQ:=False, v_bAutoConfirm:=False)
    End Function

    Public Function ConfirmRenewal(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_bIsWhatIfQ As Boolean, ByVal v_bAutoConfirm As Boolean) As Integer

        'AK 140901 - do not need BOM to do this work, moved this code from bGISBOMGIIMotor

        'Dim oBOM As Object
        '
        '    ' Debug message
        '    Debug.Print Timer & ": Entering " & ACApp & "." & ACClass & ".GetPolicyRenewalVersion"
        '
        '    On Error GoTo Err_ConfirmRenewal
        '
        '    ConfirmRenewal = PMTrue
        '
        '    ' Somebody set us up the BOM
        '    m_lReturn& = CreateBOM(v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
        ''                           v_sGisBusinessTypeCode:=gpmfunctions.ToSafeString(v_sGisBusinessTypeCode), _
        ''                           v_sClassName:=ACRenewalsClass, _
        ''                           r_oBOM:=oBOM, _
        ''                           vDatabase:=directcast(m_oDatabase,dpmdao.database))
        '    If (m_lReturn& <> PMTrue) Then
        '        ' Make your time
        '        ConfirmRenewal = PMFalse
        '        Exit Function
        '    End If
        '
        '    If (oBOM Is Nothing) Then
        '        ' Uh oh
        '        ConfirmRenewal = PMFalse
        '        Exit Function
        '    End If
        '
        '    ' Call the BOM
        '    m_lReturn& = oBOM.ConfirmRenewal(v_lInsuranceFolderCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFolderCnt), _
        ''                                     v_lInsuranceFileCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFileCnt), _
        ''                                     v_lSchemeID:=gpmfunctions.ToSafeString(v_lSchemeID), _
        ''                                     v_lPartyCnt:=gpmfunctions.ToSafeInteger(v_lPartyCnt))
        '    If (m_lReturn& <> PMTrue) Then
        '        ConfirmRenewal = PMFalse
        '        Exit Function
        '    End If
        '
        '    ' Clear up the BOM
        '    m_lReturn& = oBOM.Terminate()
        '    If (m_lReturn& <> PMTrue) Then
        '        ' Like I care
        '    End If
        '
        '    Set oBOM = Nothing


        Dim result As Integer = 0

        Dim oSiriusLinkn As bSIRIUSLink.Renewals

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmRenewal")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Create a new component services

            ' New instance of bSIRIUSLink
            oSiriusLinkn = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLinkn.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRIUSLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If


            ' Call sirius link
            'sj 26/10/2001 - Add v_bIsWhatIfQ parameter
            'AK 22/11/2001 - added v_bAutoConfirm parameter

            m_lReturn = oSiriusLinkn.RenewalConfirmed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSchemeID:=v_lSchemeID, v_lPartyCnt:=v_lPartyCnt, v_bWhatIf:=v_bIsWhatIfQ, v_bAutoConfirm:=v_bAutoConfirm)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSiriusLink.RenewalConfirmed"
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ConfirmID, v_sProcessCode:=AC_Confirm, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            End If

            ' Terminate Sirius Link

            oSiriusLinkn.Dispose()

            oSiriusLinkn = Nothing
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ConfirmID, v_sProcessCode:=AC_Confirm, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in ConfirmRenewal."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ConfirmID, v_sProcessCode:=AC_Confirm, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewalBrokerLed
    '
    ' Description: Confirm the Renewal for Brokers (GeminiNet method)
    '
    ' History: TF281101 - Created from ConfirmRenewal.
    ' DD, 21/12/2001: Added new parameters for passing to BOM
    ' DD 11/03/2002: Removed call to RenewalTransact as it was calling
    '                BOM TransactAfter before anything had taken place.
    ' ***************************************************************** '
    Public Function ConfirmRenewalBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, Optional ByVal v_bIsWhatIfQ As Boolean = False, Optional ByVal v_bAutoConfirm As Boolean = False, Optional ByVal v_lPolicyBinderID As Object = Nothing, Optional ByVal v_lPolicyID As Object = Nothing, Optional ByVal v_lOldInsurerNo As Object = Nothing, Optional ByVal v_lNewInsurerNo As Object = Nothing, Optional ByVal v_lSchemeNo As Object = Nothing, Optional ByVal v_sCoverCode As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim oBom As Object
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lGISPolicyLinkID As Integer
        Dim vSchemeArray, vAdditionalDataArray As Object
        Dim oSiriusLink As bSIRIUSLink.Renewals
        Dim oDataSet As Object = m_oDataSet

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmRenewalBrokerLed")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the Data Set Definition
            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Create instance of bGISScheme
            m_oGISSchemeBusiness = New bGISSchemeBusiness.Business()


            m_lReturn = m_oGISSchemeBusiness.Initialise(m_sUsername, m_sPassword, m_iUserID, m_iSourceID, m_iLanguageID, m_iCurrencyID, m_iLogLevel, ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            ' Get the details of the Scheme that we are Transacting For
            lGISPolicyLinkID = m_oDataSet.PolicyLinkID()

            ' DD 12/03/2002
            ' Set the Chosen Scheme in GIS Policy Link
            m_lReturn = m_oGisApplication.UpdatePolicyLinkSchemeID(lGISPolicyLinkID, v_lSchemeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.UpdatePolicyLinkSchemeID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                Return result
            End If

            m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_lGisPolicyLinkID:=lGISPolicyLinkID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemeArray, v_lGISSchemeId:=v_lSchemeID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.GetSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                Return result
            End If

            '    m_lReturn = m_oGisApplication.RenewalTransact( _
            ''        v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
            ''        v_sGisBusinessTypeCode:=gpmfunctions.ToSafeString(v_sGisBusinessTypeCode), _
            ''        v_lGISSchemeID:=gpmfunctions.ToSafeString(v_lSchemeID), _
            ''        sXMLDataSet, _
            ''        v_vInsuranceFileCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFileCnt))
            '    If (m_lReturn <> PMTrue) Then
            '        ConfirmRenewalBrokerLed = m_lReturn
            '        Exit Function
            '    End If

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode.Trim(), v_sClassName:=ACRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Call RenConfirmedBrokerLed

            m_lReturn = oBom.RenewalConfirmedBrokerLed(gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), gPMFunctions.ToSafeInteger(v_lSchemeID),
                                                       gPMFunctions.ToSafeInteger(v_lPolicyBinderID), gPMFunctions.ToSafeInteger(v_lPolicyID), gPMFunctions.ToSafeInteger(v_lOldInsurerNo), gPMFunctions.ToSafeInteger(v_lNewInsurerNo), gPMFunctions.ToSafeInteger(v_lSchemeNo), gPMFunctions.ToSafeString(v_sCoverCode))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oBOM.RenewalConfirmedBrokerLed."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)

                oBom.Dispose()
                oBom = Nothing

                Return result
            End If

            'Reload the Risk - RenewalConfirmedBrokerLed has stored the new Policy No
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk 2."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            ' Call BOM's RenewalTransact method
            If Not (oBom Is Nothing) Then

                m_lReturn = oBom.RenewalTransactAfter(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode), r_oDataSet:=oDataSet, v_vSchemeArray:=vSchemeArray, r_vAdditionalDataArray:=vAdditionalDataArray, v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt))

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = m_lReturn
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oBOM.RenewalTransactAfter Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalTransact")
                    Return result
                End If
                m_oDataSet = oDataSet
            End If

            'TF301101 - Drop BOM if no longer required
            If Not False Then
                If Not v_bAutoConfirm Then

                    oBom.Dispose()
                    oBom = Nothing
                End If
            End If

            ' Create a new component services

            ' New instance of bSIRIUSLink
            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRIUSLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewalBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                m_sRenTaskLog = "Failed to create bSiriusLink.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If


            ' Call sirius link

            m_lReturn = oSiriusLink.RenewalConfirmedBrokerLed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSchemeID:=v_lSchemeID, v_lPartyCnt:=v_lPartyCnt, v_bWhatIf:=v_bIsWhatIfQ, v_bAutoConfirm:=v_bAutoConfirm)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSiriusLink.RenewalConfirmedBrokerLed."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)


                oSiriusLink.Dispose()
                oSiriusLink = Nothing
                Return result
            End If

            ' Terminate Sirius Link

            oSiriusLink.Dispose()

            oSiriusLink = Nothing

            ' AMJ 130203 this is done as part of the renewal transaction after
            'TF301101 - Now back to the BOM to Post to Accounts
            '    If (IsMissing(v_bAutoConfirm) = False) Then
            '        If (v_bAutoConfirm = True) Then
            '
            '            m_lReturn = oBOM.RenPostToAccounts( _
            ''                            v_lInsuranceFileCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFileCnt))
            '            If (m_lReturn& <> PMTrue) Then
            '                ConfirmRenewalBrokerLed = PMFalse
            '                m_lReturn = oBOM.Terminate
            '                Set oBOM = Nothing
            '                m_sRenTaskLog = "Failed to process oBOM.RenPostToAccounts."
            '                m_lReturn = CreateTaskLog( _
            ''                                v_lInsuranceFolderCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFolderCnt), _
            ''                                v_lProcessID:=AC_Quote_BrokerID, _
            ''                                v_sProcessCode:=AC_Quote_Broker, _
            ''                                v_lStatus:=PMError, _
            ''                                v_sMessage:=m_sRenTaskLog)
            '
            '                Exit Function
            '            End If
            '
            '            m_lReturn = oBOM.Terminate
            '            Set oBOM = Nothing
            '
            '        End If
            '    End If
            If Not (oBom Is Nothing) Then

                oBom.Dispose()
                oBom = Nothing


            End If

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ConfirmID, v_sProcessCode:=AC_Confirm, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmRenewalBrokerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmRenewalBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ConfirmLapse
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '          29/05/2001 SSL - Updated to confirm Lapse from ConfirmRenewal
    ' ***************************************************************** '
    Public Function ConfirmLapse(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim oBom As Object

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmLapse")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Somebody set us up the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Make your time
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oBom Is Nothing Then
                ' Uh oh
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the BOM

            m_lReturn = oBom.LapseConfirmed(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), v_lSchemeID:=gPMFunctions.ToSafeString(v_lSchemeID), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear up the BOM

            oBom.Dispose()
            oBom = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmLapse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ConfirmLapse", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: QEMRenCompLapse
    '
    ' Description:
    '
    ' History: 18/05/2001 AK - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenCompLapse(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sXMLQuoteOutput As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".QEMRenCompLapse")


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the name of the mapper

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)

        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Remember to pass in v_lRenewalEDIAuditID

        m_lReturn = oQEM.RenCompLapse(v_vSchemeArray:=vSchemesArray, r_oDataSet:=oDataSet)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            'Exit Function
        End If
        m_oDataSet = oDataSet

        ' Get the XML from the dataset
        '    m_lReturn& = m_oDataSet.ReturnAsXML( _
        ''                                r_sXMLDataSetDef:=gpmfunctions.ToSafeString(r_sXMLDataSetDef), _
        ''                                r_sXMLDataset)
        '    If (m_lReturn& <> PMTrue) Then
        '        QEMRenCompLapse = PMFalse
        '        Exit Function
        '    End If
        '
        '    ' Call the GIS to save the quote
        '    m_lReturn& = m_oGisApplication.SaveToDB( _
        ''                    v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
        ''                    r_sXMLDataset)
        '    If (m_lReturn& <> PMTrue) Then
        '        QEMRenCompLapse = PMFalse
        '        Exit Function
        '    End If
        '
        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".QEMRenCompLapse")

        Return result

Err_QEMRenCompLapse:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenCompLapse")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenCompLapse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenCompLapse", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompLapse
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '          130901     AK  - Added one more parameter to differentiate Alt Insurer -Lapses
    '                           from the genuine Lapse-Confirmed cases
    '          051101     SJ - Add extra parameter v_lOldInsuranceFileCnt
    ' ***************************************************************** '
    Public Function RenCompLapse(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String) As Integer
        Return RenCompLapse(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_lGISSchemeID:=v_lGISSchemeID, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lProductID:=v_lProductID, v_dtRenewalDate:=v_dtRenewalDate, v_lPartyCnt:=v_lPartyCnt, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sRenewalStatusCode:=PMRenewalStatusTypePolicyLapseConfirmed, v_lOldInsuranceFileCnt:=0)
    End Function

    Public Function RenCompLapse(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sRenewalStatusCode As String) As Integer
        Return RenCompLapse(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_lGISSchemeID:=v_lGISSchemeID, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lProductID:=v_lProductID, v_dtRenewalDate:=v_dtRenewalDate, v_lPartyCnt:=v_lPartyCnt, v_lRiskCodeID:=v_lRiskCodeID, v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sRenewalStatusCode:=v_sRenewalStatusCode, v_lOldInsuranceFileCnt:=0)
    End Function

    Public Function RenCompLapse(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_sRenewalStatusCode As String, ByVal v_lOldInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSet, sXMLDataSetDef, sXMLQuoteOutput As String

        Dim oBom As Object
        Dim oSiriusLink As bSIRIUSLink.Renewals
        Dim oDataSet As Object = m_oDataSet

        Const LCRenewalsClass As String = "Renewals"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Load the risk
            'sj 05/11/2001 - start
            '    m_lReturn& = LoadRisk( _
            ''                    v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
            ''                    sXMLDataSetDef, _
            ''                    sXMLDataSet, _
            ''                    r_lRenewalInsuranceFileCnt)
            '    If (m_lReturn& <> PMTrue) Then
            '        RenCompLapse = PMFalse
            '        Exit Function
            '    End If

            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=v_lOldInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 05/11/2001 - end

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode
                'AK 270601 - changed the parameters

                m_lReturn = oBom.RenCompLapseBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                ' Process the QEM
                m_lReturn = QEMRenCompLapse(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sXMLQuoteOutput:=sXMLQuoteOutput)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'AK 291001 - log the message
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call QEMRenCompLapse for " & v_lInsuranceFolderCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapse", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oBom.RenCompLapseAfter(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDataSet = oDataSet
            End If

            ' Sirius Link


            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' Remove component services

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'AK 130901
            'sj 05/11/2001 Add OldInsuranceFileCnt parameter
            If v_sRenewalStatusCode.Trim() = PMRenewalStatusTypePolicyLapseConfirmed Then
                ' Call RenCompLapsed

                m_lReturn = oSiriusLink.RenCompLapsed(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt)
            Else

                ' Call RenCompLapsedAlternateInsurer
                'sj 05/11/2001 Add OldInsuranceFileCnt parameter

                m_lReturn = oSiriusLink.RenCompLapsedAlternateInsurer(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing

            Return result

        Catch excep As System.Exception


            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompLapse Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapse", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: QEMRenCompHoldingInsurer
    '
    ' Description:
    '
    ' History: 18/05/2001 AK - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenCompHoldingInsurer(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sXMLQuoteOutput As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer
        Dim oQEM As Object
        Dim sObject As String = ""
        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".QEMRenCompHoldingInsurer")


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the name of the mapper

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)

        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Remember to pass in v_lRenewalEDIAuditID
        'sj 06/09/2001 - start
        '    m_lReturn& = oQEM.RenCompHoldingInsurer(v_vSchemeArray:=vSchemesArray, r_oDataSet:=DirectCast(m_oDataSet, cGISDataSetControl.Application))

        m_lReturn = oQEM.RenCompHoldingInsurer(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet)
        'sj 06/09/2001 - end
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_oDataSet = oDataSet

        ' Get the XML from the dataset
        m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the GIS to save the quote
        m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".QEMRenCompHoldingInsurer")

        Return result

Err_QEMRenCompHoldingInsurer:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenCompHoldingInsurer")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenCompHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenCompHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompHoldingInsurer
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '          21/05/2001  AK - Added the actual functionality
    '
    ' ***************************************************************** '
    Public Function RenCompHoldingInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, Optional ByVal v_sGisBusinessTypeCode As String = "", Optional ByVal v_lOldInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSet, sXMLDataSetDef, sXMLQuoteOutput As String

        Dim oSiriusLink As bSIRIUSLink.Renewals
        Dim oBom As Object
        Dim oDataSet As Object = m_oDataSet

        Const LCRenewalsClass As String = "Renewals"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process LoadRisk."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - LoadRisk")

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to Create instance of oBOM."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - CreateBom")

            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenCompHoldingInsurerBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenCompHoldingInsurerBefore."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - oBOM.RenCompHoldingInsurerBefore")

                ' Process the QEM
                m_lReturn = QEMRenCompHoldingInsurer(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sXMLQuoteOutput:=sXMLQuoteOutput)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process QEMRenCompHoldingInsurer."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - QEMRenCompHoldingInsurer")


                m_lReturn = oBom.RenCompHoldingInsurerAfter(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenCompHoldingInsurerAfter."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDataSet = oDataSet
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - oBOM.RenCompHoldingInsurerAfter")

            End If
            oSiriusLink = New bSIRIUSLink.Renewals
            ' Sirius Link

            ' m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSiriusLink, v_sClassName:="bSiriusLink.Renewals", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))

            ' Remove component services

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = gPMConstants.PMEReturnCode.PMFalse
            '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            '    m_sRenTaskLog = "Failed to create instance of bSiriusLink.Renewals."
            '    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
            '    Return result
            'End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompHoldingInsurer - Create SiriusLink")

            ' Call RenQuotedInsurerLead
            'AK 170502 - added another parameter to pass old insurance file cnt

            m_lReturn = oSiriusLink.RenCompletedHoldingInsurer(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID,
                                                               v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID,
                                                               v_dtRenewalDate:=gPMFunctions.ToSafeInteger(v_dtRenewalDate), v_lGisDataModelId:=v_lGISDataModelID, v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSiriusLink.RenCompletedHoldingInsurer."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.EndBlock("bGis - RenCompHoldingInsurer - oSiriusLink.RenCompletedHoldingInsurer")

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenCompHoldingInsurer - Total Time")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompAlternateInsurer
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompAlternateInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGisBusinessTypeId As Integer, ByRef r_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim sXMLDataSet, sGisBusinessTypeCode, sXMLDataSetDef As String
            Dim oSiriusLink As bSIRIUSLink.Renewals
            Dim oBom As Object
            Dim oDataSet As Object = m_oDataSet

            Const LCRenewalsClass As String = "Renewals"


            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompAlternateInsurer - Start")

            m_oDebugTimings.StartBlock()

            ' Create new folder, file, copying risk and quotation
            '    m_lReturn = CreateAlternatePolicy( _
            ''                    v_lOldInsuranceFileCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFolderCnt), _
            ''                    v_lPartyCnt:=gpmfunctions.ToSafeInteger(v_lPartyCnt), _
            ''                    v_sPolicyRef:="", _
            ''                    v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
            ''                    v_lGisBusinessTypeId:=gpmfunctions.ToSafeInteger(v_lGisBusinessTypeId), _
            ''                    v_lGISSchemeID:=gpmfunctions.ToSafeInteger(v_lGISSchemeID), _
            ''                    v_lQuoteBinderId:=0, _
            ''                    r_lInsuranceFolderCnt:=lNewInsuranceFolderCnt, _
            ''                    r_lInsuranceFileCnt:=gpmfunctions.ToSafeInteger(r_lNewInsuranceFileCnt), _
            ''                    r_lPolicyLinkID:=gpmfunctions.tosafeinteger(lNewPolicyLinkID), _
            ''                    r_lQuoteBinderId:=lNewQuoteBinderId)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '        RenCompAlternateInsurer = PMFalse
            '        Exit Function
            '    End If

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenCompAlternateInsurer - CreateBom")

            If Not (oBom Is Nothing) Then


                sGisBusinessTypeCode = oBom.GISBusinessTypeCode


                m_lReturn = oBom.RenCompAlternateInsurerBefore()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompAlternateInsurer - oBOM.RenCompAlternateInsurerBefore")

                ' Process the QEM (new business EDI)
                '        m_lReturn& = QEMRenCompAlternateInsurer( _
                ''                        v_lInsuranceFileCnt:=gpmfunctions.ToSafeInteger(r_lNewInsuranceFileCnt), _
                ''                        v_lPolicyLinkID:=gpmfunctions.tosafeinteger(lNewPolicyLinkID), _
                ''                        v_lRenewalEdiAuditId:=gpmfunctions.ToSafeInteger(v_lRenewalEdiAuditId), _
                ''                        v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
                ''                        v_sGisBusinessTypeCode:=sGisBusinessTypeCode)
                '
                '        If (m_lReturn& <> PMTrue) Then
                '            RenCompAlternateInsurer = PMFalse
                '            Exit Function
                '        End If
                '        m_oDebugTimings.PrintDebugMessage ("bGis - RenCompAlternateInsurer - QEMRenCompAlternateInsurer")

                ' Load the risk (current insurer)
                m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompAlternateInsurer - LoadRisk")

                '        'Lapse the current policy EDI
                '        m_lReturn = QEMRenCompLapse( _
                ''                    v_lRenewalEdiAuditId:=gpmfunctions.ToSafeInteger(v_lRenewalEdiAuditId), _
                ''                    v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
                ''                    v_sGisBusinessTypeCode:=sGisBusinessTypeCode, _
                ''                    sXMLDataSetDef, _
                ''                    sXMLDataSet, _
                ''                    r_sXMLQuoteOutput:=sXMLQuoteOutput)
                '
                '        If (m_lReturn& <> PMTrue) Then
                '            RenCompAlternateInsurer = PMFalse
                '            Exit Function
                '        End If
                '

                m_lReturn = oBom.RenCompAlternateInsurerAfter(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                m_oDataSet = oDataSet
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompAlternateInsurer - oBOM.RenCompAlternateInsurerAfter")

            End If

            ' Sirius Link
            oSiriusLink = New bSIRIUSLink.Renewals
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSiriusLink, v_sClassName:="bSiriusLink.Renewals", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))

            '' Remove component services

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = gPMConstants.PMEReturnCode.PMFalse
            '    ' Log Error Message
            '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            '    Return result
            'End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompAlternateInsurer - Create SiriusLink")

            ' Call Update Renewal Control

            m_lReturn = oSiriusLink.NewInsuranceFile(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            m_lReturn = oSiriusLink.RenCompletedAlternateInsurer(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGISDataModelID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.EndBlock("bGis - RenCompAlternateInsurer - oSiriusLink.RenCompletedHoldingInsurer")

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenCompAlternateInsurer - Total Time")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompAlternateInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompAlternateInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreatePolicy
    '
    ' Description:
    '
    ' History: 17/07/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Public Function CreatePolicy(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sPolicyRef As String, ByVal v_sGisDataModelCode As String, ByVal v_lGisBusinessTypeId As Integer, ByVal v_lGISSchemeID As Integer, ByRef r_lInsuranceFolderCnt As Object, ByRef r_lInsuranceFileCnt As Object, ByRef r_lPolicyLinkID As Integer, ByRef r_lQuoteBinderId As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim oSiriusLink As bSIRIUSLink.Renewals
            Dim sGisDataModelCode As String
            Dim lReturnValue As Object
            Dim sBOMRequired As String = ""

            m_oDebugTimings.StartBlock()

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Generate a unique policy reference
            m_lReturn = GeneratePolicyNumber(v_lGISSchemeID:=v_lGISSchemeID, r_sPolicyNo:=v_sPolicyRef)

            'TODO: This may not be needed, or may be modified

            ' Create GII to Sirius Policy Object
            ' TF230102 - Only if BOM required
            m_lReturn = GISSharedConstants.GetRegSettingFromDataBusModel(v_sDataModelCode:=v_sGisDataModelCode, v_sSettingName:=GISSharedConstants.GISRegBOMRequired, r_sSettingValue:=sBOMRequired, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer)

            If (sBOMRequired = "1") Or (sBOMRequired = "Y") Then

                m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oSirPolicy, v_sClassName:="bGIIToSirPolicy.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bGIIToSirPolicy.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Create Policy", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

                m_lReturn = m_oSirPolicy.CreateInsuranceFolder(v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_sPolicyRef:=gPMFunctions.ToSafeString(v_sPolicyRef), v_sTransactionType:="RN D", v_lGeminiBusinessType:=gPMFunctions.ToSafeInteger(v_lGisBusinessTypeId), v_iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lInsuranceFolderCnt:=r_lInsuranceFolderCnt, r_lReturnValue:=lReturnValue)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If

            sGisDataModelCode = v_sGisDataModelCode.Trim()

            m_lReturn = RenSelectionCopyDataSet(v_sDataModelCode:=sGisDataModelCode, v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lNewInsuranceFileCnt:=r_lInsuranceFileCnt, v_lGISSchemeID:=v_lGISSchemeID, r_lNewGISPolicyLinkID:=r_lPolicyLinkID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to make copy of risk data for " & r_lInsuranceFileCnt, ACApp, ACClass, "CreatePolicy")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenSelection - RenSelectionCopyDataSet")

            ' Sirius Link


            oSiriusLink = New bSIRIUSLink.Renewals
            m_lReturn = oSiriusLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            ' Remove component services

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing

            m_oDebugTimings.EndBlock("bGis - CreatePolicy - Total Time")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompWhatIf
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompWhatIf() As Integer

        Dim result As Integer = 0
        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompWhatIf Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompWhatIf", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenSelectionCopyDataSet
    '
    ' Description:
    '
    ' History: 02/04/2001 SJ - Created.
    '
    '          27/05/02  AK - added RiskId paramater
    ' ***************************************************************** '
    Private Function RenSelectionCopyDataSet(ByVal v_sDataModelCode As String, ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lGISSchemeID As Integer, ByRef r_lNewGISPolicyLinkID As Integer, Optional ByVal v_lNewRiskID As Integer = 1) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String

        'AK 030801 - Removed all the references to local oGisApplication, rather use module level object

        'Dim oGisApplication As New bGIS.Application



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDebugTimings.StartBlock()

        ' CTAF 170401 - Can't do anything here if we dont have a
        '               data model to work with
        If v_sDataModelCode = "" Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Need a valid DataModelCode.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelectionCopyDataSet", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        '    m_lReturn = oGisApplication.Initialise( _
        ''        sUsername:=gpmfunctions.ToSafeString(m_sUsername), _
        ''        sPassword:=gpmfunctions.ToSafeString(m_sPassword), _
        ''        iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), _
        ''        iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), _
        ''        iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), _
        ''        iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), _
        ''        iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), _
        ''        sCallingAppName:=ACApp)

        '    If (m_lReturn <> PMTrue) Then
        '        LogMessageFile _
        ''            PMLogOnError, "Failed to initialise bGis.Application", _
        ''            ACApp, _
        ''            ACClass, _
        ''            "RenSelectionCopyDataSet"
        '        RenSelectionCopyDataSet = PMFalse
        '        Set oGisApplication = Nothing
        '        Exit Function
        '    End If
        m_oDebugTimings.PrintDebugMessage("bGis - RenSelectionCopyDataSet - Initialise")

        'AK 270502 - pass new risk Id, so that policy link can be populated
        m_lReturn = m_oGisApplication.CopyDataSet(v_sDataModelCode:=v_sDataModelCode, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_vOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vNewRiskID:=v_lNewRiskID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to copy dataset", ACApp, ACClass, "RenSelectionCopyDataSet")
            '        Set oGisApplication = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - RenSelectionCopyDataSet - oGisApplication.CopyDataSet")

        'Clear the quote output (Temporary fix)
        ' Create Data Set Control
        m_oDataSet = New cGISDataSetControl.Application()

        m_lReturn = m_oDataSet.LoadFromXML(v_sXMLDataSetDef:=sXMLDataSetDef, v_sXMLDataSet:=sXMLDataSet)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '        Set oGisApplication = Nothing
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - RenSelectionCopyDataSet - m_oDataSet.LoadFromXML")

        m_lReturn = m_oDataSet.ClearAllQuoteOutput()
        m_oDebugTimings.PrintDebugMessage("bGis - RenSelectionCopyDataSet - ClearAllQuoteOutpu")

        '    m_lReturn = m_oDataSet.ReturnAsXML( _
        ''        sXMLDataSetDef, _
        ''        sXMLDataSet)
        '    If m_lReturn <> PMTrue Then
        '        RenSelectionCopyDataSet = PMFalse
        ''        Set oGisApplication = Nothing
        '        Exit Function
        '    End If
        m_oDebugTimings.PrintDebugMessage("bGis - RenSelectionCopyDataSet - m_oDataSet.ReturnAsXML")

        'AK 030801 - Save it at the end of Selection Process
        '    ' Save it back to the database
        '    m_lReturn& = m_oGisApplication.SaveToDB( _
        ''                    v_sGisDataModelCode:=v_sDataModelCode, _
        ''                    sXMLDataSet)
        '    If (m_lReturn& <> PMTrue) Then
        '        LogMessageFile _
        ''            PMLogOnError, "Failed to save to database.", _
        ''            ACApp, _
        ''            ACClass, _
        ''            "RenSelectionCopyDataSet"
        '        RenSelectionCopyDataSet = PMFalse
        ''        Set oGisApplication = Nothing
        '        Exit Function
        '    End If

        m_oDebugTimings.PrintDebugMessage("bGis - RenSelectionCopyDataSet - oGisApplication.SaveToDB")

        ' Update the scheme ID on the policy_link
        m_lReturn = m_oGisApplication.UpdatePolicyLinkSchemeID(v_lGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_lGISSchemeID:=v_lGISSchemeID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = UpdatePolicySchemesSel(v_lGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_lGISSchemeID:=v_lGISSchemeID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        'DC 12/11/2002 -start -copy agents from old to new
        m_lReturn = CopyAgentsFromOldToNewInsFile(v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        'DC 12/11/2002 -end

        '    m_lReturn = oGisApplication.Terminate()
        '    Set oGisApplication = Nothing

        m_oDebugTimings.EndBlock("bGis - RenSelectionCopyDataSet - UpdatePolicyLinkSchemeID")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CopyQuote
    '
    ' Description:
    '
    ' History: 30/07/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Private Function CopyQuote(ByVal v_lNewGisPolicyLinkID As Integer, ByVal v_lOldQuoteBinderId As Integer, ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lOldGISSchemeID As Integer, ByRef v_lGisBusinessTypeId As Integer, ByRef r_lNewQuoteBinderId As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' New Quote Binder Id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewQuoteBinderId", vValue:=CStr(r_lNewQuoteBinderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add NewQuoteBinderId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' New GisPolicyLinkId
        m_lReturn = m_oDatabase.Parameters.Add(sName:="NewGisPolicyLinkId", vValue:=CStr(v_lNewGisPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add NewGisPolicyLinkId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' New OldQuoteBinderId
        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsuranceFileCnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add InsuranceFileCnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' New OldGisSchemeId
        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldGisSchemeId", vValue:=CStr(v_lOldGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add OldGisSchemeId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Old quote binder id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="OldQuoteBinderId", vValue:=CStr(v_lOldQuoteBinderId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add OldQuoteBinderId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Gis Business Type Id
        m_lReturn = m_oDatabase.Parameters.Add(sName:="GisBusinessTypeId", vValue:=CStr(v_lGisBusinessTypeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add GisBusinessTypeId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyQuoteSQL, sSQLName:=ACCopyQuoteName, bStoredProcedure:=ACCopyQuoteStored)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyQuote", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        r_lNewQuoteBinderId = m_oDatabase.Parameters.Item("NewQuoteBinderId").Value

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name:  RenQuotationInsurerLead
    '
    ' Description:
    '
    ' History: 02/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenQuotationInsurerLead(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sXMLQuoteOutput As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        'Debug.Print Timer & ": Entering " & ACApp & "." & ACClass & ".QEMRenQuotationInsurerLead"


        m_oDebugTimings.StartBlock()
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - Start")

        m_oDebugTimings.StartBlock()

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' ClearAllQuoteOutput
        m_lReturn = m_oDataSet.ClearAllQuoteOutput()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - ClearAllQuoteOutput")

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - GetSchemeIDFromLink")

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - GetCurrentSchemeID")

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=DateTime.Today)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - GetSchemes")

        ' Get the name of the mapper

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)

        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - CreateQEM")

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - oQEM.Initialise")

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - oQEM.InitialiseEngine")

        ' Remember to pass in v_lRenewalEDIAuditID

        m_lReturn = oQEM.RenQuotationInsurerLead(v_vSchemeArray:=vSchemesArray, v_lRenewalEdiAuditId:=gPMFunctions.ToSafeInteger(v_lRenewalEdiAuditId), r_oDataSet:=oDataSet)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            'Exit Function
        End If
        m_oDataSet = oDataSet
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - RenQuotationInsurerLead")

        ' Get the XML from the dataset
        m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationInsurerLead - m_oDataSet.ReturnAsXML")

        ' Call the GIS to save the quote
        m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.EndBlock("bGis - QEM RenQuotationInsurerLead - m_oGISApplication.SaveToDB")

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing


        m_oDebugTimings.EndBlock("bGis - QEM RenQuotationInsurerLead - End")

        ' Debug message
        'Debug.Print Timer & ": Exiting " & ACApp & "." & ACClass & ".QEMRenQuotationInsurerLead"

        Return result

Err_QEMRenQuotationInsurerLead:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenQuotationInsurerLead")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenQuotationInsurerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function


    Private Function QEMRenQuotationBrokerLead(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sXMLQuoteOutput As String, ByVal v_lGroupId As Integer, ByVal v_lGISSchemeID As Object) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lSchemePolicyLinkId As Integer
        Dim sObject As String = ""
        Dim sGisDataModelCode As String = ""
        Dim vAllSchemesArray, vQEMSchemeArray As Object
        Dim sCurrentQEM As String = ""
        Dim sNextQEM As New StringBuilder
        Dim sXMLDataSetDef As String = ""
        Dim lQEMSchemeRow As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDebugTimings.StartBlock()
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - Start")


        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' ClearAllQuoteOutput
        m_lReturn = m_oDataSet.ClearAllQuoteOutput()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - ClearAllQuoteOutput")

        'Load all the schemes from the renewal group


        m_lReturn = LoadRenewalGroupSchemes(v_lPolicyLinkID:=lPolicyLinkID, v_lGroupId:=v_lGroupId, v_lGISSchemeID:=v_lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            lSchemePolicyLinkId = -1
        Else
            lSchemePolicyLinkId = lPolicyLinkID
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - LoadRenewalGroupSchemes")

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vAllSchemesArray, v_lGisPolicyLinkID:=lSchemePolicyLinkId, v_lGISSchemeId:=-1, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - GetSchemes")

        If lSchemePolicyLinkId = -1 Then
            'We need to save the schemes if there is no renewal group
            'TF 201101 - use AllSchemeArray instead



            m_lReturn = SaveSelectedSchemes(v_vResultArray:=vAllSchemesArray, v_lGISSchemeID:=v_lGISSchemeID, v_lPolicyLinkID:=lPolicyLinkID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationBrokerLead")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - SaveSelectedSchemes")

        ' Get the Data Model Code
        sGisDataModelCode = m_oDataSet.GISDataModelCode

        ' Get the First Quote  Engine Mapper From the List

        sCurrentQEM = CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, 0)).Trim().ToUpper()
        sCurrentQEM = sCurrentQEM & ".Renewals"

        ' For each Scheme

        For lRow As Integer = vAllSchemesArray.GetLowerBound(1) To vAllSchemesArray.GetUpperBound(1)

            ' Get the Quote Engine Mapper

            sNextQEM = New StringBuilder(CStr(vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, lRow)).Trim().ToUpper())
            sNextQEM.Append(".Renewals")

            ' Is the QEM the same as the last one
            If sCurrentQEM <> sNextQEM.ToString() Then



                m_lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMSchemeArray:=vQEMSchemeArray, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return m_lReturn
                End If



                vQEMSchemeArray = Nothing ' reset
                sCurrentQEM = sNextQEM.ToString()
            End If


            ' Yes so add it to the List of Schemes

            If Informations.IsArray(vQEMSchemeArray) Then

                ReDim Preserve vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, vQEMSchemeArray.GetUpperBound(1) + 1)
            Else
                ReDim vQEMSchemeArray(GISSharedConstants.GISQEMSchArraySize, 0)
            End If


            lQEMSchemeRow = vQEMSchemeArray.GetUpperBound(1)

            ' Move the Scheme Details Across



            vQEMSchemeArray(GISSharedConstants.GISQEMSchObjectName, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchObjectName, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchClassName, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchClassName, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchSchemeNo, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchSchemeNo, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchSchemeVer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchSchemeVer, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchFilename, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchFilename, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchQMInsurerRef, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchQMInsurerRef, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchPolarisInsurerNo, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchType, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchType, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchVariant, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchVariant, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchCommPerc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchCommPerc, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchID, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchID, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchDesc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchDesc, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchAbi81Insurer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAbi81Insurer, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAbi1EdiDirectory, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchAgencyCode, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchAgencyCode, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchEdiMailBox, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchEdiMailBox, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchInsurerDesc, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchInsurerDesc, lRow)


            vQEMSchemeArray(GISSharedConstants.GISQEMSchDictVer, lQEMSchemeRow) = vAllSchemesArray(GISSharedConstants.GISQEMSchDictVer, lRow)

        Next lRow

        If Informations.IsArray(vQEMSchemeArray) Then



            m_lReturn = CallQEMToQuote(v_sQEMName:=sCurrentQEM, v_vQEMSchemeArray:=vQEMSchemeArray, v_sGisDataModelCode:=sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lGISSchemeID:=v_lGISSchemeID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - CallQemToQuote")

        ' Convert the quote to an XML string
        m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - ReturnAsXML")

        ' Call the GIS to save the quote
        m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - QEM RenQuotationBrokerLead - SaveToDB")

        ' Terminate the mapper
        '    m_lReturn& = oQEM.Terminate()
        '    If (m_lReturn& <> PMTrue) Then
        '    End If
        '
        '    ' Clear it up
        '    Set oQEM = Nothing
        m_oDebugTimings.EndBlock("bGis - QEM RenQuotationBrokerLead - End")

        Return result

    End Function

    Private Function QEMRenQuotationInsurerLeadRebroke(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String, ByRef r_sXMLQuoteOutput As String, ByVal v_lGroupId As Integer, ByVal v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lSchemePolicyLinkId As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        'Dim sXMLDataSetDef As String
        'Dim sXMLDataset As String


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' ClearAllQuoteOutput
        m_lReturn = m_oDataSet.ClearAllQuoteOutput()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemeIDFromLink
        '    m_lReturn& = m_oGISApplication.GetSchemeIDFromLink( _
        ''                    v_lGISPolicyLinkID:=gpmfunctions.tosafeinteger(lPolicyLinkID), _
        ''                    r_lGisSchemeId:=lGISSchemeID)
        '    If (m_lReturn& <> PMTrue) Then
        '        QEMRenQuotationInsurerLeadRebroke = PMFalse
        '        Exit Function
        '    End If

        '    ' GetCurrentSchemeID
        '    m_lReturn& = m_oGISApplication.GetCurrentSchemeID( _
        ''                    v_lOldGISSchemeID:=gpmfunctions.ToSafeInteger(lGISSchemeID), _
        ''                    v_dtEffectiveDate:=Date, _
        ''                    r_lNewGISSchemeID:=lNewGISSchemeID, _
        ''                    v_sGisBusinessTypeCode:=gpmfunctions.ToSafeString(v_sGisBusinessTypeCode))
        '    If (m_lReturn& <> PMTrue) Then
        '        QEMRenQuotationInsurerLeadRebroke = PMFalse
        '        Exit Function
        '    End If

        m_lReturn = LoadRenewalGroupSchemes(v_lPolicyLinkID:=lPolicyLinkID, v_lGroupId:=v_lGroupId, v_lGISSchemeID:=v_lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            lSchemePolicyLinkId = -1
        Else
            lSchemePolicyLinkId = lPolicyLinkID
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lSchemePolicyLinkId, v_lGISSchemeId:=-1, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If lSchemePolicyLinkId = -1 Then
            'We need to save the schemes if there is no renewal group

            m_lReturn = SaveSelectedSchemes(v_vResultArray:=vSchemesArray, v_lGISSchemeID:=v_lGISSchemeID, v_lPolicyLinkID:=lPolicyLinkID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SaveSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationInsurerLeadRebroke")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Get the name of the mapper

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject) ' + "," + sObject.Substring(0, sObject.LastIndexOf(".")))).FullName, sObject).Unwrap()

        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationInsurerLeadRebroke", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the function

        m_lReturn = oQEM.RenQuotationInsurerLeadRebroke(v_vSchemeArray:=vSchemesArray, r_oDataSet:=oDataSet, v_lGISSchemeID:=gPMFunctions.ToSafeInteger(v_lGISSchemeID))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_oDataSet = oDataSet
        ' Convert the quote to an XML string
        m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=r_sXMLDataSetDef, r_sXMLDataSet:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the GIS to save the quote
        m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataset:=r_sXMLDataset)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing



        Return result

Err_QEMRenQuotationInsurerLeadRebroke:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenQuotationInsurerLeadRebroke Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenQuotationInsurerLeadRebroke", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: RenGetInsurerQuoteOptions
    '
    ' Description:  Takes a comma list of schemes and returns an array with
    '               the Web grid layout design. If more than one scheme is
    '               passed then the grid determines whether columns are
    '               shown or hidden.
    '
    ' History: 29/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenGetInsurerQuoteOptions(ByVal v_vSchemes As Object, ByVal v_sCoverCode As Object, ByRef r_vGridLayout As Object) As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Execute the SQL
            m_oDatabase.Parameters.Clear()

            m_oDatabase.Parameters.Add("Schemes", CStr(v_vSchemes), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_oDatabase.Parameters.Add("CoverCode", CStr(v_sCoverCode), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            m_lReturn = m_oDatabase.SQLSelect("spu_GIS_Insurer_GetQuoteOptions", "Get Insurer Options", True, , r_vGridLayout)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenGetInsurerQuoteOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenGetInsurerQuoteOptions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenGetPassword
    '
    ' Description:  Asks the BOM for the associated decrypted password
    '               for the Insurance File.
    '
    ' History:  27/11/2001 DD - Created.
    '           DD, 18/12/2001: Moved to the BOM to make the call generic.
    '
    ' ***************************************************************** '
    Public Function RenGetPassword(ByVal v_sGisDataModelCode As Object, ByVal v_sGisBusinessTypeCode As Object, ByVal v_lInsuranceFileCnt As Object, ByRef r_sUnencryptedPassword As Object) As Integer

        Dim result As Integer = 0
        Dim oBom As Object

        Try

            ' Set up the BOM



            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)

            If (m_lReturn <> gPMConstants.PMEReturnCode.PMTrue) Or (oBom Is Nothing) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            result = oBom.RenGetPassword(v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFileCnt), r_sUnencryptedPassword:=r_sUnencryptedPassword)

            ' Clear up the BOM

            oBom.Dispose()
            oBom = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenGetPassword Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenGetPassword", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPolicyRenewalVersion
    '
    ' Description:  Gets the latest version of the Policy record to be
    '               used for the Renewal process.
    '
    ' History: 21/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyRenewalVersion(ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Dim oBom As Object

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetPolicyRenewalVersion")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Somebody set us up the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=ACRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Make your time
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If oBom Is Nothing Then
                ' Uh oh
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the BOM

            m_lReturn = oBom.GetPolicyRenewalVersions(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), r_vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Clear up the BOM

            oBom.Dispose()
            oBom = Nothing

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetPolicyRenewalVersion")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetPolicyRenewalVersion")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyRenewalVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRenewalVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: LoadRenewalGroupSchemes
    '
    ' Description:
    '
    ' History: 21/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function LoadRenewalGroupSchemes(ByVal v_lPolicyLinkID As Integer, ByVal v_lGroupId As Integer, ByVal v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim oSchemeGroupMember As bGISSchemeBusiness.SchemeGroupMember
        Dim vResultArray, vParameterArray As Object
        Dim lUboundSchemes As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        oSchemeGroupMember = m_oGISSchemeBusiness.SchemeGroupMember

        ReDim vParameterArray(1, 0)

        vParameterArray(0, 0) = "gis_scheme_group_id"

        vParameterArray(1, 0) = v_lGroupId

        m_lReturn = m_oGISSchemeBusiness.DeleteAllSelectedSchemes(v_lPolicyLinkID:=v_lPolicyLinkID)

        'Get a list of all the schemes in the renewal group

        m_lReturn = oSchemeGroupMember.GetList(v_lListType:=1, r_vMembersArray:=vResultArray, v_vParameterValues:=vParameterArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSchemeGroupMember.GetList Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRenewalGroupSchemes")
            Return result

        End If

        oSchemeGroupMember = Nothing

        If Not Informations.IsArray(vResultArray) Then
            'No Renewal Group Set Up
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If


        lUboundSchemes = vResultArray.GetUpperBound(1)

        Dim lSelectedSchemes(lUboundSchemes) As Integer
        'bFound = False
        For lCnt As Integer = 0 To lUboundSchemes

            lSelectedSchemes(lCnt) = CInt(vResultArray(1, lCnt))
            'If (lSelectedSchemes(lCnt) = v_lGISSchemeID) Then
            '    bFound = True

        Next lCnt

        ' 25/01/2002 - DD: Cannot do the following as the current scheme
        '                  needs to be chosen on merit. It could have (probably has)
        '                  expired and must not be forceably added for quoting.
        '                  This will cause a new version of the scheme to generate
        '                  a duplicate and different quote
        '    TF151001 - Ensure current scheme is included in list.
        '    If (bFound = False) Then
        '        ReDim Preserve lSelectedSchemes(lUboundSchemes + 1)
        '        lSelectedSchemes(lUboundSchemes + 1) = v_lGISSchemeID
        '    End If

        'Save the selected schemes to the gis_policy_schemes_sel table
        m_lReturn = m_oGISSchemeBusiness.SaveSelectedSchemes(v_lPolicyLinkID:=v_lPolicyLinkID, r_lSelectedSchemes:=lSelectedSchemes)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.SaveSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoadRenewalGroupSchemes")
            Return result

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: RenReminder
    '
    ' Description: For Printing Reminder letters
    '
    ' History: 26/06/2001 AK  - Created.
    '          09/09/2002 CJB - Pass new optional parameter, SchemeID,
    '                           to support CNIC.
    '          AMJ 040203 added batch parameter
    '
    ' ***************************************************************** '
    Public Function RenReminder(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer, ByVal v_sGisDataModelCode As String) As Integer
        Return RenReminder(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lRenInsFileCnt:=v_lRenInsFileCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_vGISSchemeID:=Nothing, v_lBatchRun:=gPMConstants.PMEReturnCode.PMFalse)
    End Function

    Public Function RenReminder(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_vGISSchemeID As Object) As Integer
        Return RenReminder(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lRenInsFileCnt:=v_lRenInsFileCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_vGISSchemeID:=v_vGISSchemeID, v_lBatchRun:=gPMConstants.PMEReturnCode.PMFalse)
    End Function

    Public Function RenReminder(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lBatchRun As Integer) As Integer
        Return RenReminder(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lRenInsFileCnt:=v_lRenInsFileCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_vGISSchemeID:=Nothing, v_lBatchRun:=v_lBatchRun)
    End Function

    Public Function RenReminder(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenInsFileCnt As Integer, ByVal v_sGisDataModelCode As String, Optional ByVal v_vGISSchemeID As Object = Nothing, Optional ByVal v_lBatchRun As Integer = gPMConstants.PMEReturnCode.PMFalse) As Integer


        Dim result As Integer = 0
        Dim oBom As Object
        Dim bBatchRun As Boolean

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenReminder - Start")

            m_oDebugTimings.StartBlock()


            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ReminderID, v_sProcessCode:=AC_Reminder, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReminder - CreateBOM")

            ' Call BOM.RenReminder
            If Not (oBom Is Nothing) Then

                'v_sGisBusinessTypeCode = oBOM.GisBusinessTypeCode

                'IJR 2003-04-14 Start
                bBatchRun = v_lBatchRun = gPMConstants.PMEReturnCode.PMTrue
                'IJR 2003-04-14 End

                ' CJB 090902 If we've been passed a scheme id parameter then pass it
                ' on to the solution specific BOM RenReminder function...

                If Informations.IsNothing(v_vGISSchemeID) Then

                    m_lReturn = oBom.RenReminder(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(v_lRenInsFileCnt), v_bBatchRun:=gPMFunctions.ToSafeBoolean(bBatchRun))
                Else

                    m_lReturn = oBom.RenReminder(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(v_lRenInsFileCnt), v_lGISSchemeID:=v_vGISSchemeID, v_bBatchRun:=gPMFunctions.ToSafeBoolean(bBatchRun))
                End If

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenReminder."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ReminderID, v_sProcessCode:=AC_Reminder, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReminder - oBOM.RenReminder")

            m_oDebugTimings.EndBlock("bGis - RenReminder - Total Time ")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ReminderID, v_sProcessCode:=AC_Reminder, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReminder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReminder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            m_sRenTaskLog = "Error encountered in RenReminder."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_ReminderID, v_sProcessCode:=AC_Reminder, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenReprintConfirm
    '
    ' Description: Enables to reprint confirmation documents
    '
    ' History: 23/07/2001 SSL - Created.
    ' DD 05/03/2002: Changed r_lRenewalInsuranceFileCnt to ByVal
    '
    ' ***************************************************************** '
    Public Function RenReprintConfirm(ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Return RenReprintConfirm(v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenReprintConfirm(ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer



        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim oBom As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintConfirm - Start")

            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenResendEDI - LoadRisk")

            ' Create the BOM
            ' DD 05/03/2002: Corrected - passed in BusinessTypeCode
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintConfirm - CreateBOM")

            ' BOM.RenReprintInvitationInsurerLedBefore
            If Not (oBom Is Nothing) Then

                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' QEM.RenInvitation - Print Invite documents
            m_lReturn = QEMRenReprintConfirm(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 291001 - log the message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call RenReprintConfirm for InsuranceFile " & r_lRenewalInsuranceFileCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintConfirm", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenReprintConfirm - QEMRenInvitation")

            m_oDebugTimings.EndBlock("bGis - RenReprintConfirm - Total Time ")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintConfirm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintConfirm", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: QEMRenReprintConfirm
    '
    ' Description:
    '
    ' History: 23/07/2001 SSL - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenReprintConfirm(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".QEMRenReprintConfirm")


        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the name of the mapper
        ' Note to self: Change this to a constant
        ' Note to self: Helicopters cant fly upside down

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject)

        Catch
        End Try

        ' Reset the error trapping


        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenReprintConfirm", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the function

        m_lReturn = oQEM.RenReprintConfirm(v_vSchemeArray:=vSchemesArray, r_oDataSet:=oDataSet)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDataSet = oDataSet

        ' Don't do anything with it?

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".QEMRenReprintConfirm")

        Return result

Err_QEMRenReprintConfirm:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenReprintConfirm")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenReprintConfirm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenReprintConfirm", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenResendEDI
    '
    ' Description: Enables to reprint EDI (send messages)
    '
    ' History: 20/07/2001 SSL - Created.
    '
    ' ***************************************************************** '
    Public Function RenResendEDI(ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Return RenResendEDI(v_lGISDataModelID:=v_lGISDataModelID, v_sGisDataModelCode:=v_sGisDataModelCode, r_lRenewalInsuranceFileCnt:=r_lRenewalInsuranceFileCnt, v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisBusinessTypeCode:="")
    End Function

    Public Function RenResendEDI(ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByRef r_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisBusinessTypeCode As String) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim oBom As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenResendEDI - Start")
            m_oDebugTimings.StartBlock()

            ' Load the risk
            m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=r_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenResendEDI - LoadRisk")

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - Renresendedi - CreateBOM")

            ' BOM.RenReprintInvitationInsurerLedBefore
            If Not (oBom Is Nothing) Then

                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode
            Else
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' QEM.RenInvitation - Print Invite documents
            m_lReturn = QEMRenResendEDI(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 291001 - log the message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to run QEMRenResendEDI for InsuranceFile " & r_lRenewalInsuranceFileCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapse", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenResendEDI - QEMRenInvitation")

            m_oDebugTimings.EndBlock("bGis - RenResendEDI - Total Time ")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenResendEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenResendEDI", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: QEMRenResendEDI
    '
    ' Description:
    '
    ' History: 20/07/2001 SSL - Created.
    '
    ' ***************************************************************** '
    Private Function QEMRenResendEDI(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_sXMLDataSetDef As String, ByRef r_sXMLDataset As String) As Integer

        Dim result As Integer = 0
        Dim lPolicyLinkID, lGISSchemeID, lNewGISSchemeID As Integer

        Dim oQEM As Object

        Dim sObject As String = ""

        Dim vSchemesArray As Object
        Dim oDataSet As Object = m_oDataSet

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".QEMRenResendEDI")

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get Policy Link ID
        lPolicyLinkID = m_oDataSet.PolicyLinkID()

        ' GetSchemeIDFromLink
        m_lReturn = m_oGisApplication.GetSchemeIDFromLink(v_lGISPolicyLinkID:=lPolicyLinkID, r_lGisSchemeId:=lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetCurrentSchemeID
        m_lReturn = m_oGisApplication.GetCurrentSchemeID(v_lOldGISSchemeID:=lGISSchemeID, v_dtEffectiveDate:=DateTime.Today, r_lNewGISSchemeID:=lNewGISSchemeID, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' GetSchemes
        m_lReturn = m_oGISSchemeBusiness.GetSchemes(v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sGisDataModelCode:=v_sGisDataModelCode, r_vSchemesArray:=vSchemesArray, v_lGisPolicyLinkID:=lPolicyLinkID, v_lGISSchemeId:=lNewGISSchemeID, v_dtEffectiveDate:=CDate(DateTime.Today.ToString("MM-dd-yyyy")))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the name of the mapper
        ' Note to self: Change this to a constant
        ' Note to self: Helicopters cant fly upside down

        sObject = CStr(vSchemesArray(0, 0)).Trim() & ".Renewals"

        ' Disable error checking for a tad
        Try

            ' Create the QEM
            oQEM = gPMFunctions.CreateLateBoundObject(sObject) ' + "," + sObject.Substring(0, sObject.LastIndexOf(".")))).FullName, sObject).Unwrap()

        Catch
        End Try

        ' Reset the error trapping

        ' Check it was created
        If oQEM Is Nothing Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of QEM : " & sObject, vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenResendEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Initialise it

        Dim oDatabase As Object = m_oDatabase
        m_lReturn = oQEM.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=ACApp, vDatabase:=oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDatabase = oDatabase

        ' Initialise the engine

        m_lReturn = oQEM.InitialiseEngine(v_sGisDataModelCode:=gPMFunctions.ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the function

        m_lReturn = oQEM.RenResendEDI(v_vSchemeArray:=vSchemesArray, r_oDataSet:=oDataSet)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDataSet = oDataSet

        ' Don't do anything with it?

        ' Terminate the mapper

        oQEM.Dispose()
        ' Clear it up
        oQEM = Nothing

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".QEMRenResendEDI")

        Return result

Err_QEMRenResendEDI:

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".QEMRenResendEDI")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="QEMRenResendEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="QEMRenResendEDI", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    '' ***************************************************************** '
    ''
    '' Name: RenReprintInvite
    ''
    '' Description: Enables to reprint Invitation form
    ''
    '' History: 20/07/2001 SSL - Created.
    ''
    '' ***************************************************************** '
    'Public Function RenReprintInvite(ByVal v_lGISDataModelID As Long, _
    ''                              ByVal v_sGisDataModelCode As String, _
    ''                              ByRef r_lRenewalInsuranceFileCnt As Long, _
    ''                              ByVal v_lRenewalEdiAuditId As Long, _
    ''                     Optional ByVal v_sGisBusinessTypeCode As String = "") As Long
    '
    '
    '
    '    Dim sXMLDataSetDef As String
    '    Dim sXMLDataset As String
    '
    '    On Error GoTo Err_RenReprintInvite
    '
    '    RenReprintInvite = PMTrue
    '
    '    m_oDebugTimings.StartBlock
    '    m_oDebugTimings.PrintDebugMessage ("bGis - RenReprintInvite - Start")
    '
    '    m_oDebugTimings.StartBlock
    '
    '    ' Load the risk
    '    m_lReturn& = LoadRisk( _
    ''                    v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
    ''                    sXMLDataSetDef, _
    ''                    sXMLDataSet, _
    ''                    r_lRenewalInsuranceFileCnt)
    '    If (m_lReturn& <> PMTrue) Then
    '        RenReprintInvite = PMFalse
    '        Exit Function
    '    End If
    '    m_oDebugTimings.PrintDebugMessage ("bGis - RenReprintInvite - LoadRisk")
    '
    '    ' QEM.RenInvitation - Print Invite documents
    '    m_lReturn& = QEMRenInvitation( _
    ''                    v_lRenewalEdiAuditId:=gpmfunctions.ToSafeInteger(v_lRenewalEdiAuditId), _
    ''                    v_sGisDataModelCode:=gpmfunctions.ToSafeString(v_sGisDataModelCode), _
    ''                    v_sGisBusinessTypeCode:=gpmfunctions.ToSafeString(v_sGisBusinessTypeCode), _
    ''                    sXMLDataSetDef, _
    ''                    sXMLDataSet)
    '    If (m_lReturn& <> PMTrue) Then
    '        RenReprintInvite = PMFalse
    '        Exit Function
    '    End If
    '    m_oDebugTimings.PrintDebugMessage ("bGis - RenReprintInvite - QEMRenInvitation")
    '
    '    m_oDebugTimings.EndBlock ("bGis - RenReprintInvite - Total Time ")
    '
    '    Exit Function
    '
    'Err_RenReprintInvite:
    '
    '    RenReprintInvite = PMError
    '
    '    ' Log Error Message
    '    LogMessage m_sUsername, _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="RenReprintInvite Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="RenReprintInvite", _
    ''        vErrNo:=Informations.Err.Number, _
    ''        vErrDesc:=Informations.Err.Description
    '
    '    Exit Function
    '


    ' ***************************************************************** '
    ' Name: GeneratePolicyNumber (Private)
    '
    ' Description:
    ' ***************************************************************** '
    Private Function GeneratePolicyNumber(ByRef v_lGISSchemeID As Integer, ByRef r_sPolicyNo As Object) As Integer
        Dim result As Integer = 0



        Const ACRangeError As Integer = 1
        Const ACFormatError As Integer = 2

        Dim m_oPolicyNumber As Object
        Dim iError As Object

        result = gPMConstants.PMEReturnCode.PMTrue

        'm_oPolicyNumber = New bGIIGetPolicyNumber.Business()
        m_oPolicyNumber = New Object
        Dim oDatabase As Object = m_oDatabase
        m_lReturn = m_oPolicyNumber.Initialise(sUserName:=gPMFunctions.ToSafeString(m_sUsername), sPassword:=gPMFunctions.ToSafeString(m_sPassword), iUserID:=gPMFunctions.ToSafeInteger(m_iUserID), iSourceID:=gPMFunctions.ToSafeInteger(m_iSourceID), iLanguageID:=gPMFunctions.ToSafeInteger(m_iLanguageID), iCurrencyID:=gPMFunctions.ToSafeInteger(m_iCurrencyID), iLogLevel:=gPMFunctions.ToSafeInteger(m_iLogLevel), sCallingAppName:=gPMFunctions.ToSafeString(m_sCallingAppName), vDatabase:=oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            m_oPolicyNumber = Nothing
            Return result
        End If
        m_oDatabase = oDatabase

        m_lReturn = m_oPolicyNumber.GetNextPolicyNumber(v_lGISSchemeID:=gPMFunctions.ToSafeInteger(v_lGISSchemeID), v_lGisInsurerId:=0, r_sPolicyNumber:=r_sPolicyNo, r_iError:=iError)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Select Case iError
            Case ACRangeError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error getting policy range", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber", vErrNo:=0, vErrDesc:="")

                Return gPMConstants.PMEReturnCode.PMFalse

            Case ACFormatError

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error formatting policy number", vApp:=ACApp, vClass:=ACClass, vMethod:="GeneratePolicyNumber", vErrNo:=0, vErrDesc:="")

                Return gPMConstants.PMEReturnCode.PMFalse

        End Select

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRiskID
    '
    ' Description:
    '
    ' History: 09/05/2002 CTAF - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (UpdateRiskID) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function UpdateRiskID(ByVal v_lGISPolicyLinkID As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Dim sSQL As String = ""
    '
    'Try 
    '
    ' This is VERY temporary and only to be used until investigation
    ' has found out why the risk_id wasnt being populated...
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'sSQL = "UPDATE gis_policy_link " &  _
    '       "SET risk_id = (" &  _
    '       "SELECT max(risk_cnt) " &  _
    '       "FROM insurance_file_risk_link " &  _
    '       "WHERE insurance_file_cnt = {insurance_file_cnt})" &  _
    '       "WHERE gis_policy_link_id = {gis_policy_link_id}"
    '
    ' Clear params
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    '
    'm_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    '
    ' Call SQL
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdateRiskID", bStoredProcedure:=False)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'result = gPMConstants.PMEReturnCode.PMFalse
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SQL Failed : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskID", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    'Return result

    '
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRiskID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRiskID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result





    ' ***************************************************************** '
    '
    ' Name: WhatIfQuote
    '
    ' Description:
    '
    ' History: 24/07/2001 IJM - Created.
    '          RAM20020522    - Code added to copy risk data from old
    '                           insurance file to new insurance file
    '                           using GIS Stored Procedures
    ' ***************************************************************** '
    Public Function WhatIfQuote(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_lNewGISPolicyLinkID As Integer) As Integer
        Return WhatIfQuote(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_lNewInsuranceFileCnt:=0, v_bCreateDataSet:=True, v_cThisPremium:=0, v_cNetPremium:=0, v_cTaxAmount:=0)
    End Function

    Public Function WhatIfQuote(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_lNewGISPolicyLinkID As Integer, ByVal v_cThisPremium As Decimal, ByVal v_cNetPremium As Decimal, ByVal v_cTaxAmount As Decimal) As Integer
        Return WhatIfQuote(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_sGisDataModelCode:=v_sGisDataModelCode, v_lGISSchemeID:=v_lGISSchemeID, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_lNewInsuranceFileCnt:=0, v_bCreateDataSet:=True, v_cThisPremium:=v_cThisPremium, v_cNetPremium:=v_cNetPremium, v_cTaxAmount:=v_cTaxAmount)
    End Function

    Public Function WhatIfQuote(ByVal v_lInsuranceFileCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGISSchemeID As Integer, ByRef r_lNewGISPolicyLinkID As Integer, ByRef r_lNewInsuranceFileCnt As Integer, ByVal v_bCreateDataSet As Boolean, ByVal v_cThisPremium As Decimal, ByVal v_cNetPremium As Decimal, ByVal v_cTaxAmount As Decimal) As Integer

        Dim result As Integer = 0
        Dim oSBOLink As bSIRIUSLink.Renewals
        Dim lNewInsuranceFileCnt As Integer
        Dim sGisDataModelCode As String
        Dim lNewRiskCnt As Integer 'RAM20020522 : Variable to store new RiskCnt

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - WhatIfQuote - Start")

            m_oDebugTimings.StartBlock()

            ' Create a SBO Link object


            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "WhatIfQuote")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                Return result
            End If

            ' Remove component services

            m_oDebugTimings.PrintDebugMessage("bGis - WhatIfQuote - Create Sirius Link Object")

            ' ***************************************************************** '
            ' Call the CreateRenPolicyVersion method on the Sirius Link Object
            ' ***************************************************************** '
            'AK 140901 - mark this policy version as RenewalWhatIf

            m_lReturn = oSBOLink.CopyInsuranceFile(v_lOldPolicyKey:=v_lInsuranceFileCnt, r_lNewPolicyKey:=lNewInsuranceFileCnt, v_InsFileType:=ACPMInsFileTypeWhatIf, v_cThisPremium:=v_cThisPremium, v_cNetPremium:=v_cNetPremium, v_cTaxAmount:=v_cTaxAmount)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "oSBOLink.WhatIfQuote Failed", ACApp, ACClass, "WhatIfQuote")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                Return result
            End If

            'SJ 23/04/2004 - start
            r_lNewInsuranceFileCnt = lNewInsuranceFileCnt
            'SJ 23/04/2004 - end

            'IJR 2003-07-02 Start
            If v_sGisDataModelCode <> "GIITruck" And v_sGisDataModelCode <> "GIIHouse" And v_sGisDataModelCode <> "GIIMotor" Then
                'IJR 2003-07-02 End
                '---------------------------------------------------------------------------
                'RAM20020522 : Added the following code to copy Risk Data Asscociated
                '              with the old insurance file cnt to the new insurance file cnt
                '---------------------------------------------------------------------------
                m_oDebugTimings.PrintDebugMessage("bGis.Renewals - WhatIfQuote - CopyRiskData Started")

                m_lReturn = CopyRiskData(v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, r_lNewRiskCnt:=lNewRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "CopyRiskData Failed", ACApp, ACClass, "WhatIfQuote")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oSBOLink = Nothing
                    Return result
                End If

                m_oDebugTimings.PrintDebugMessage("bGis.Renewals - WhatIfQuote - CopyRiskData Finished")

                '---------------------------------------------------------------------------
                'IJR 2003-07-02 Start
            End If
            'IJR 2003-07-02 End

            m_oDebugTimings.PrintDebugMessage("bGis - WhatIfQuote - oSBOLink.CreateRenPolicyVersion")

            ' ***************************************************************** '
            ' Create the renewal version of the risk
            ' ***************************************************************** '
            sGisDataModelCode = v_sGisDataModelCode.Trim()

            'SJ 23/04/2004 - start
            If Not v_bCreateDataSet And r_lNewGISPolicyLinkID <> 0 Then
                'When called in this mode we have allready created a dataset and we just need
                'to update the gis_policy_link record with the new insurance_file_cnt, risk_cnt and gis_scheme_id
                m_lReturn = UpdatePolicyLink(v_lPolicyLinkID:=r_lNewGISPolicyLinkID, v_lInsuranceFileCnt:=lNewInsuranceFileCnt, v_lRiskCnt:=lNewRiskCnt, v_lGISSchemeID:=v_lGISSchemeID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed update policy link for " & lNewInsuranceFileCnt, ACApp, ACClass, "WhatIfQuote")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oSBOLink = Nothing
                    Return result
                End If
                'Create an entry in the gis_policy_schemes_sel table
                m_lReturn = UpdatePolicySchemesSel(v_lGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_lGISSchemeID:=v_lGISSchemeID)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, " UpdatePolicySchemesSel Failed ", ACApp, ACClass, "WhatIfQuote")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oSBOLink = Nothing
                    Return result
                End If
                'SJ 23/04/2004 - end
            Else
                ' Copy risk
                ' RAM20020522 : Added the v_lNewRiskID Optional Parameter
                m_lReturn = WhatIfQuoteCopyDataSet(v_sDataModelCode:=sGisDataModelCode, v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lNewInsuranceFileCnt:=lNewInsuranceFileCnt, v_lGISSchemeID:=v_lGISSchemeID, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_lNewRiskID:=lNewRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to make copy of risk data for " & v_lInsuranceFileCnt, ACApp, ACClass, "WhatIfQuote")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    oSBOLink = Nothing
                    Return result
                End If
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - WhatIfQuote - WhatIfQuoteCopyDataSet")


            oSBOLink.Dispose()

            oSBOLink = Nothing

            m_oDebugTimings.EndBlock("bGis - WhatIfQuote - Total Time")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WhatIfQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: WhatIfQuoteCopyDataSet
    '
    ' Description:
    '
    ' History: 13/08/2001 IJM - Created.
    '          RAM20020522    - Optional Paramater v_lNewRiskID added
    ' ***************************************************************** '
    Private Function WhatIfQuoteCopyDataSet(ByVal v_sDataModelCode As String, ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lGISSchemeID As Integer, ByRef r_lNewGISPolicyLinkID As Integer, Optional ByRef r_sXMLDataSetDef As String = "", Optional ByRef r_sXMLDataset As String = "", Optional ByVal v_vCopyQuotes As Boolean = False, Optional ByVal v_lNewRiskID As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim sXMLDataSetDef, sXMLDataSet As String
        Dim lNewRiskID As Integer 'RAM20020522 - Variable to hold NewRiskID

        'AK 030801 - Removed all the references to local oGisApplication, rather use module level object
        'sj 311001 - Remove all quote and deleted objects before copying the quote



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDebugTimings.StartBlock()
        m_oDebugTimings.PrintDebugMessage("bGis - WhatIfQuoteCopyDataSet - start")
        m_oDebugTimings.StartBlock()

        ' CTAF 170401 - Can't do anything here if we dont have a
        '               data model to work with
        If v_sDataModelCode = "" Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Need a valid DataModelCode.", vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuoteCopyDataSet", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        'RAM20020522 : If the NewRiskID is passed, we use it, else default value 1

        If Informations.IsNothing(v_lNewRiskID) Then
            lNewRiskID = 1
        Else
            lNewRiskID = v_lNewRiskID
        End If

        m_lReturn = m_oGisApplication.CopyDataSet(v_sDataModelCode:=v_sDataModelCode, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, v_vOldInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_vNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vCopyQuotes:=v_vCopyQuotes, v_vNewRiskID:=lNewRiskID)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to copy dataset", ACApp, ACClass, "WhatIfQuoteCopyDataSet")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If
        m_oDebugTimings.PrintDebugMessage("bGis - WhatIfQuoteCopyDataSet - m_oGisApplication.CopyDataSet")

        r_sXMLDataSetDef = sXMLDataSetDef
        r_sXMLDataset = sXMLDataSet

        ' Update the scheme ID on the policy_link
        m_lReturn = m_oGisApplication.UpdatePolicyLinkSchemeID(v_lGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_lGISSchemeID:=v_lGISSchemeID)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Save it back to the database
        m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=v_sDataModelCode, sXMLDataSet)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to save to database.", ACApp, ACClass, "WhatIfQuoteCopyDataSet")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_oDebugTimings.EndBlock("bGis - WhatIfQuoteCopyDataSet - UpdatePolicyLinkSchemeID")

        m_oDebugTimings.EndBlock("bGis - WhatIfQuoteCopyDataSet - End")

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: RemoveDeletedAndQuoteObjects
    '
    ' Description:
    '
    ' History: 31/10/2001 sj - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RemoveDeletedAndQuoteObjects) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RemoveDeletedAndQuoteObjects(ByRef r_sXMLDataset As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Dim lStartPosition, lEndPosition As Integer
    '
    ' Find start of deleted objects
    'lStartPosition = (r_sXMLDataset.IndexOf("<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """>") + 1)
    '
    ' If we found deleted objects
    'If lStartPosition > 0 Then
    '
    'lEndPosition = (r_sXMLDataset.IndexOf("</DELETED_OBJECTS>") + 1)
    '
    ' If we found delimiter
    'If lEndPosition > 0 Then
    '
    ' Copy first bit of message + empty deleted objects + end bit of original message
    'r_sXMLDataset = gPMFunctions.Mid(r_sXMLDataset, 1, lStartPosition - 1) &  _
    '                "<DELETED_OBJECTS OI=""" & "DELETED_OBJECTS" & """/>" &  _
    '                Mid(r_sXMLDataset, lEndPosition + ("</DELETED_OBJECTS>").Length, r_sXMLDataset.Length)


    '
    ' Find start of quote objects
    'lStartPosition = (r_sXMLDataset.IndexOf("<QUOTES NextQuoteNumber") + 1)
    '
    ' If we found QUOTE objects
    'If lStartPosition > 0 Then
    '
    'lEndPosition = (r_sXMLDataset.IndexOf("</QUOTES>") + 1)
    '
    ' If we found delimiter
    'If lEndPosition > 0 Then
    '
    'x = "<QUOTES NextQuoteNumber=""" & "16" & """ OI=" & """QUOTES" & """>"
    ' Copy first bit of message + empty deleted objects + end bit of original message
    'r_sXMLDataset = gPMFunctions.Mid(r_sXMLDataset, 1, lStartPosition - 1) &  _
    '                "<QUOTES NextQuoteNumber=""" & "1" & """ OI=" & """QUOTES" & """>" &  _
    '                Mid(r_sXMLDataset, lEndPosition, r_sXMLDataset.Length)


    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RemoveDeletedAndQuoteObjects Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RemoveDeletedAndQuoteObjects", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '



    ' ***************************************************************** '
    '
    ' Name: LapseExistingPolicy
    '
    ' Description:
    '
    ' History: 20/08/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Public Function LapseExistingPolicy(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer
        Return LapseExistingPolicy(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_bLoadRisk:=False)
    End Function

    Public Function LapseExistingPolicy(ByVal v_lRenewalEdiAuditId As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_bLoadRisk As Boolean) As Integer

        Dim result As Integer = 0
        Const LCRenewalsClass As String = "Renewals"

        Dim sXMLDataSetDef, sXMLDataSet, sXMLQuoteOutput, sDef, sDS As String
        Dim oBom As Object
        Dim oDataSet As Object = m_oDataSet

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_bLoadRisk Then
                m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sDef, r_sXMLDataset:=sDS, r_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not (oBom Is Nothing) Then


                v_sGisBusinessTypeCode = oBom.GISBusinessTypeCode

                m_lReturn = oBom.RenCompLapseBefore(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=gPMFunctions.ToSafeInteger(v_lPartyCnt), v_lRenInsFileCnt:=gPMFunctions.ToSafeInteger(v_lRenewalInsuranceFileCnt))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Process the QEM
                m_lReturn = QEMRenCompLapse(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_sXMLQuoteOutput:=sXMLQuoteOutput)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call QEMRenCompLapse for " & v_lInsuranceFolderCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapse")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                m_lReturn = oBom.RenCompLapseAfter(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=gPMFunctions.ToSafeInteger(v_lRenewalInsuranceFileCnt), r_oDataSet:=oDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_oDataSet = oDataSet
            End If

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapseExistingPolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseExistingPolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    '
    ' Name: RenCompletion
    '
    ' Description:
    '
    ' History: 21/08/01 - AK - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompletion(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lGISSchemeID As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lProductID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lPartyCnt As Integer, ByVal v_lRiskCodeID As Integer, ByVal v_lGISDataModelID As Integer, ByVal v_sGisDataModelCode As String, ByVal v_lGisBusinessTypeId As Integer, ByRef v_lOldInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            Dim sXMLDataSet, sGisBusinessTypeCode, sXMLDataSetDef As String
            Dim oSiriusLink As bSIRIUSLink.Renewals
            Dim oBom As Object
            Dim oDataSet As Object = m_oDataSet

            Const LCRenewalsClass As String = "Renewals"


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear error log
            m_sRenTaskLog = ""

            m_oDebugTimings.StartBlock()
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompletion - Start")

            m_oDebugTimings.StartBlock()


            ' Get an instance of the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:=LCRenewalsClass, r_oBOM:=oBom, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If

            m_oDebugTimings.PrintDebugMessage("bGis - RenCompletion - CreateBom")

            If Not (oBom Is Nothing) Then


                sGisBusinessTypeCode = oBom.GISBusinessTypeCode

                '        m_lReturn& = oBOM.RenCompletionBefore()
                '        If (m_lReturn& <> PMTrue) Then
                '            RenCompletion = PMFalse
                '            Exit Function
                '        End If
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompletion - oBOM.RenCompletionBefore")


                ' Load the risk (current insurer)
                m_lReturn = LoadRisk(v_sGisDataModelCode:=v_sGisDataModelCode, r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataset:=sXMLDataSet, r_lInsuranceFileCnt:=v_lOldInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process LoadRisk."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDebugTimings.PrintDebugMessage("bGis - RenCompletion - LoadRisk")


                m_lReturn = oBom.RenCompletionAfter(r_oDataSet:=oDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process oBOM.RenCompletionAfter."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If
                m_oDataSet = oDataSet

                'AK 220801 - Save the dataset to Database now
                m_lReturn = m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
                m_oDataSet.ReturnAsXML(r_sXMLDataSetDef:=sXMLDataSetDef, r_sXMLDataSet:=sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process m_oDataSet.ReturnAsXML."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If

                m_lReturn = m_oGisApplication.SaveToDB(v_sGisDataModelCode:=v_sGisDataModelCode, sXMLDataSet)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to save to database.", ACApp, ACClass, "RenCompletion-SaveToDB")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    m_sRenTaskLog = "Failed to process m_oGisApplication.SaveToDB."
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                    Return result
                End If

                m_oDebugTimings.PrintDebugMessage("bGis - RenCompletion - oBOM.RenCompletionAfter")


                oBom.Dispose()
                oBom = Nothing
            End If

            ' Sirius Link
            oSiriusLink = New bSIRIUSLink.Renewals
            'm_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oSiriusLink, v_sClassName:="bSiriusLink.Renewals", v_sCallingAppName:=ACApp, v_sUsername:=gpmfunctions.ToSafeString(m_sUsername), v_sPassword:=gpmfunctions.ToSafeString(m_sPassword), v_iUserID:=gpmfunctions.ToSafeInteger(m_iUserID), v_iSourceID:=gpmfunctions.ToSafeInteger(m_iSourceID), v_iLanguageID:=gpmfunctions.ToSafeInteger(m_iLanguageID), v_iCurrencyID:=gpmfunctions.ToSafeInteger(m_iCurrencyID), v_iLogLevel:=gpmfunctions.ToSafeInteger(m_iLogLevel), v_oDatabase:=directcast(m_oDatabase,dpmdao.database))

            '' Remove component services

            'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            '    result = gPMConstants.PMEReturnCode.PMFalse
            '    ' Log Error Message
            '    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSiriusLink.Renewals", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            '    m_sRenTaskLog = "Failed to create bSiriusLink.Renewals."
            '    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
            '    Return result
            'End If
            m_oDebugTimings.PrintDebugMessage("bGis - RenCompletion - Create SiriusLink")


            '    ' Call Update Renewal Control
            '    m_lReturn& = oSiriusLink.NewInsuranceFile( _
            ''                                    v_lInsuranceFolderCnt:=gpmfunctions.ToSafeInteger(v_lInsuranceFolderCnt), _
            ''                                    v_lInsuranceFileCnt:=gpmfunctions.ToSafeInteger(r_lRenewalInsuranceFileCnt))
            '
            '    If (m_lReturn <> PMTrue) Then
            '        RenCompletion = PMFalse
            '        Exit Function
            '    End If

            'Use the old InsuranceFileCnt to mark it as Replaced/Lapsed

            m_lReturn = oSiriusLink.RenCompleted(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_lGisSchemeId:=v_lGISSchemeID, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=gPMFunctions.ToSafeInteger(v_dtRenewalDate), v_lGisDataModelId:=v_lGISDataModelID, v_lOldInsuranceFileCnt:=v_lOldInsuranceFileCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                'AK 291001 - log the message
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call oSiriusLink.RenCompleted for " & v_lInsuranceFolderCnt & " ", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to process oSiriusLink.RenCompleted."
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)
                Return result
            End If
            m_oDebugTimings.EndBlock("bGis - RenCompletion - oSiriusLink.RenCompletedHoldingInsurer")

            ' Terminate

            oSiriusLink.Dispose()
            oSiriusLink = Nothing

            m_oDebugTimings.EndBlock("bGis - RenCompletion - Total Time")

            ' Log success
            m_sRenTaskLog = ""
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMSucceed, v_sMessage:=m_sRenTaskLog)

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            m_sRenTaskLog = "Error encountered in RenCompletion."
            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMError, v_sMessage:=m_sRenTaskLog)
            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    ' Name: UpdatePolicySchemesSel
    '
    ' Description: Update the scheme id in the gis_policy_schemes_sel table.
    '
    ' History: 24/08/01 - IJM - Created.
    '
    ' ***************************************************************** '
    Private Function UpdatePolicySchemesSel(ByRef v_lGISPolicyLinkID As Integer, ByRef v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' GIS policy link id
        lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGISPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add gis_policy_link_id Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicySchemesSel", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' GIS scheme id
        lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add gis_scheme_id Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicySchemesSel", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLAction(sSQL:=ACSchemeSelUpdSQL, sSQLName:=ACSchemeSelUpdName, bStoredProcedure:=ACSchemeSelUpdStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicySchemesSel", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyAgentsFromOldToNewInsFile
    '
    ' Description: Copy agents from old to new insurance_file_cnt
    '
    ' History: 12/11/02 - DC - Created.
    '
    ' ***************************************************************** '
    Private Function CopyAgentsFromOldToNewInsFile(ByRef v_lOldInsuranceFileCnt As Integer, ByRef v_lNewInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        ' old insurance_file_cnt
        lReturn = m_oDatabase.Parameters.Add(sName:="old_insurance_file_cnt", vValue:=CStr(v_lOldInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add old_insurance_file_cnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAgentsFromOldToNewInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' new insurance_file_cnt
        lReturn = m_oDatabase.Parameters.Add(sName:="new_insurance_file_cnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add new_insirance_file_cnt Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAgentsFromOldToNewInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Call the SQL
        lReturn = m_oDatabase.SQLAction(sSQL:=ACCopyAgentsSQL, sSQLName:=ACCopyAgentsName, bStoredProcedure:=ACCopyAgentsStored)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLAction Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyAgentsFromOldToNewInsFile", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result

        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: ListRenewals
    '
    ' Description:  Calls the bSIRRenewalsManager to get an array of
    '               renewals that meet the criteria passed.
    '
    ' History: 23/10/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function ListRenewals(ByRef r_vResultArray As Object, ByVal v_sDataModelCode As String, Optional ByVal v_sRenewalStatus As String = "", Optional ByVal v_dtDueDateStart As Integer = -1, Optional ByVal v_dtDueDateLimit As Integer = -1, Optional ByVal v_sClientCode As String = "", Optional ByVal v_sPolicyNo As String = "", Optional ByVal v_lBusinessTypeID As Byte = 0, Optional ByVal v_lSchemeID As Byte = 0, Optional ByVal v_lInsurerId As Byte = 0, Optional ByVal v_lSuspensionLevel As Byte = 0, Optional ByVal v_lOfferAlt As Byte = 0) As Integer

        Dim result As Integer = 0
        Dim oRenewals As Object

        Try

            ' Create the BOM
            m_lReturn = CreateBOM(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, v_sGisDataModelCode:=v_sDataModelCode, v_sGisBusinessTypeCode:="", v_sClassName:="Renewals", r_oBOM:=oRenewals, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                m_sRenTaskLog = "Failed to create oBOM.Renewals."
                Return result
            End If

            'Pass the call through to the Business object

            m_lReturn = oRenewals.SelectRenewalControl(r_vResultArray:=r_vResultArray, v_sRenewalStatus:=gPMFunctions.ToSafeString(v_sRenewalStatus),
                                                       v_dtDueDateStart:=gPMFunctions.ToSafeInteger(v_dtDueDateStart), v_dtDueDateLimit:=gPMFunctions.ToSafeInteger(v_dtDueDateLimit),
                                                       v_sClientCode:=gPMFunctions.ToSafeString(v_sClientCode), v_sPolicyNo:=gPMFunctions.ToSafeString(v_sPolicyNo), v_lBusinessTypeID:=gPMFunctions.ToSafeInteger(v_lBusinessTypeID),
                                                       v_lSchemeID:=gPMFunctions.ToSafeString(v_lSchemeID), v_lInsurerId:=gPMFunctions.ToSafeInteger(v_lInsurerId), v_lSuspensionLevel:=gPMFunctions.ToSafeInteger(v_lSuspensionLevel), v_lOfferAlt:=gPMFunctions.ToSafeInteger(v_lOfferAlt))

            result = m_lReturn

            'Clean up

            oRenewals.Dispose()
            oRenewals = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ListRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ListRenewals", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateLapseReason
    '
    ' Description:  Updates the Lapse Reason in the Insurance File and
    '               sets the Renewal Control to LAPSECONF.
    '
    ' History: 16/11/2001 - DD Created.
    '
    ' ***************************************************************** '
    Public Function UpdateLapseReason(ByVal v_lInsuranceFolderCnt As Object, ByVal v_lLapseReasonID As Object, ByVal v_sLapseComment As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim oLapse As Object

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the renewal action business object
            ' Create component services

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oLapse, v_sClassName:="bSIRLapseReason.Business", v_sCallingAppName:=m_sCallingAppName, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oLapse.Dispose()
                oLapse = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If



            m_lReturn = oLapse.UpdateLapseReason(v_lInsuranceFolderCnt, v_lLapseReasonID, v_sLapseComment)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                oLapse.Dispose()
                oLapse = Nothing
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Destroy object instance

            oLapse.Dispose()
            oLapse = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateLapseReason Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLapseReason", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalControl
    '
    ' Description:  Updates the Renewal Control Record
    '
    ' History: 05/11/2001 DD - Created.
    '
    ' ***************************************************************** '
    Public Function UpdateRenewalControl(ByVal v_lInsuranceFolderCnt As Integer, Optional ByVal v_vProductID As Object = Nothing, Optional ByVal v_vRenewalInsuranceFileCnt As Object = Nothing, Optional ByVal v_vRenewalStatusTypeCode As Object = Nothing, Optional ByVal v_vSuspensionLevel As Object = Nothing, Optional ByVal v_vRenewalEdiAuditId As Object = Nothing, Optional ByVal v_vRenewalGisSchemeID As Object = Nothing, Optional ByVal v_vGISSchemeID As Object = Nothing, Optional ByVal v_vRenewalDate As Object = Nothing, Optional ByVal v_vGISDataModelID As Object = Nothing, Optional ByVal v_vOldInsuranceFileCnt As Object = Nothing, Optional ByVal v_vOfferAlt As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oRenewals As Object

            ' Create the Business Object

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oRenewals, v_sClassName:="bSIRRenewalControl.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Pass the call through to the Business object
            'translating defaults to nulls













            m_lReturn = oRenewals.DirectUpdate(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=If(CStr(v_vProductID) = "", DBNull.Value, v_vProductID), v_vRenewalInsuranceFileCnt:=If(CStr(v_vRenewalInsuranceFileCnt) = "", DBNull.Value, v_vRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=If(CStr(v_vRenewalStatusTypeCode) = "", DBNull.Value, v_vRenewalStatusTypeCode), v_vSuspensionLevel:=If(CStr(v_vSuspensionLevel) = "", DBNull.Value, v_vSuspensionLevel), v_vRenewalEdiAuditId:=If(CStr(v_vRenewalEdiAuditId) = "", DBNull.Value, v_vRenewalEdiAuditId), v_vRenewalGisSchemeID:=If(CStr(v_vRenewalGisSchemeID) = "", DBNull.Value, v_vRenewalGisSchemeID), v_vGISSchemeID:=If(CStr(v_vGISSchemeID) = "", DBNull.Value, v_vGISSchemeID), v_vRenewalDate:=If(CStr(v_vRenewalDate) = "", DBNull.Value, v_vRenewalDate), v_vGISDataModelID:=If(CStr(v_vGISDataModelID) = "", DBNull.Value, v_vGISDataModelID), v_vOldInsuranceFileCnt:=If(CStr(v_vOldInsuranceFileCnt) = "", DBNull.Value, v_vOldInsuranceFileCnt), v_vOfferAlt:=If(CStr(v_vOfferAlt) = "", DBNull.Value, v_vOfferAlt))

            result = m_lReturn

            'Clean up

            oRenewals.Dispose()
            oRenewals = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateRenewalControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateRenewalControl", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: CreateTaskLog
    '
    ' Description: Add a record to the Renewal_Batch_Log table
    '
    ' History: TF311001 - Created
    '
    ' ***************************************************************** '
    Private Function CreateTaskLog(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lProcessID As Integer, ByVal v_sProcessCode As String, ByVal v_lStatus As Integer, ByVal v_sMessage As String) As Integer


        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue


        ' TF021101 - Use bSIRRenTaskLog to log progress

        ' Create component if not already created
        ' Create here to avoid creating for functions that never use it

        If m_oTaskLog Is Nothing Then

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oTaskLog, v_sClassName:="bSIRRenTaskLog.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get instance of bSIRRenTaskLog.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

        End If

        m_lReturn = m_oTaskLog.AddRenewalTaskLog(v_lInsuranceFolderCnt:=gPMFunctions.ToSafeInteger(v_lInsuranceFolderCnt), v_lProcessID:=gPMFunctions.ToSafeInteger(v_lProcessID), v_sProcessCode:=gPMFunctions.ToSafeString(v_sProcessCode), v_lStatus:=gPMFunctions.ToSafeInteger(v_lStatus), v_sMessage:=gPMFunctions.ToSafeString(v_sMessage))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process m_oTaskLog.AddRenewalTaskLog", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SaveSelectedSchemes
    '
    ' Description:
    '
    ' History: 06/11/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function SaveSelectedSchemes(ByVal v_vResultArray(,) As Object, ByVal v_lGISSchemeID As Integer, ByVal v_lPolicyLinkID As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lSelectedSchemes() As Integer
        Dim lUboundSchemes As Integer
        Dim bFound As Boolean

        'Delete any existing schemes
        m_lReturn = m_oGISSchemeBusiness.DeleteAllSelectedSchemes(v_lPolicyLinkID:=v_lPolicyLinkID)

        lUboundSchemes = v_vResultArray.GetUpperBound(1)

        ReDim lSelectedSchemes(lUboundSchemes)
        bFound = False
        For lCnt As Integer = 0 To lUboundSchemes

            lSelectedSchemes(lCnt) = CInt(v_vResultArray(GISSharedConstants.GISQEMSchID, lCnt))
            If lSelectedSchemes(lCnt) = v_lGISSchemeID Then
                bFound = True
            End If
        Next lCnt
        If Not bFound Then
            ReDim Preserve lSelectedSchemes(lUboundSchemes + 1)
            lSelectedSchemes(lUboundSchemes + 1) = v_lGISSchemeID
        End If

        'Save the selected schemes to the gis_policy_schemes_sel table
        m_lReturn = m_oGISSchemeBusiness.SaveSelectedSchemes(v_lPolicyLinkID:=v_lPolicyLinkID, r_lSelectedSchemes:=lSelectedSchemes)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oGISSchemeBusiness.SaveSelectedSchemes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SaveSelectedSchemes")
            Return result

        End If

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CallQEMToQuote
    '
    ' Description:
    '
    '
    ' ***************************************************************** '
    Private Function CallQEMToQuote(ByVal v_sQEMName As String, ByVal v_vQEMSchemeArray As Object, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByVal v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim oQEM As Object
        Dim lReturn As Integer
        Dim sXMLDataSet As String = ""
        Dim oDataSet As Object = m_oDataSet



        result = gPMConstants.PMEReturnCode.PMTrue

        lReturn = CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=oQEM, vDatabase:=m_oDatabase)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Do the Quotation and Return the Results

        lReturn = oQEM.RenQuotationBrokerLead(v_vSchemeArray:=v_vQEMSchemeArray, r_oDataSet:=oDataSet, v_lGISSchemeID:=gPMFunctions.ToSafeInteger(v_lGISSchemeID))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        m_oDataSet = oDataSet

        oQEM.Dispose()

        oQEM = Nothing

        Return result

    End Function


    ' ***************************************************************** '
    ' Name: CreateQEM
    '
    ' Description:
    '
    ' History: CTAF 230401 - Changed from Private to Friend
    '
    ' ***************************************************************** '
    Friend Function CreateQEM(ByVal v_sQEMName As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oQEM As Object) As Integer
        Return CreateQEM(v_sQEMName:=v_sQEMName, v_sGisDataModelCode:=v_sGisDataModelCode, v_sGisBusinessTypeCode:=v_sGisBusinessTypeCode, r_oQEM:=r_oQEM, vDatabase:=Nothing)
    End Function

    Friend Function CreateQEM(ByVal v_sQEMName As String, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String, ByRef r_oQEM As Object, ByRef vDatabase As Object) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue
        Try
            r_oQEM = gPMFunctions.CreateLateBoundObject(v_sQEMName)

        Catch
        End Try

        If r_oQEM Is Nothing Then
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to Create QEM Object :- " & v_sQEMName, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateQEM")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' sj 09/09/99 - pass through the real values to the business

        lReturn = r_oQEM.Initialise(gPMFunctions.ToSafeString(m_sUsername), gPMFunctions.ToSafeString(m_sPassword), gPMFunctions.ToSafeInteger(m_iUserID),
                                    gPMFunctions.ToSafeInteger(m_iSourceID), gPMFunctions.ToSafeInteger(m_iLanguageID), gPMFunctions.ToSafeInteger(m_iCurrencyID),
                                    gPMFunctions.ToSafeInteger(m_iLogLevel), gPMFunctions.ToSafeString(ACApp), vDatabase)

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        ' Initialise the Quote Engine Mapper

        lReturn = r_oQEM.InitialiseEngine(gPMFunctions.ToSafeString(v_sGisDataModelCode), gPMFunctions.ToSafeString(v_sGisBusinessTypeCode))

        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return lReturn
        End If

        Return result

Err_CreateQEM:

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateQEMFailed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateQEM", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: RenMultipleQuotationBrokerLed
    '
    ' Description:  Processes the Quotations for an array of
    '               Renewal Insurance Folders returning an array of
    '               failures or Resulting Insurance Files.
    '               This is used by iGISSellerTool to maximise
    '               performance from a web page.
    '
    ' History:  20/11/2001 DD - Created.
    '           26/11/2001 DD - Added SP call to update holding insurer flag.
    '           03/12/2001 DD - Marking Holding Ins made generic using SQL
    '
    ' ***************************************************************** '
    Public Function RenMultipleQuotationBrokerLead(ByVal v_sDataModelCode As String, ByVal v_sBusinessTypeCode As String, ByRef v_vSelectedArray As Object, ByRef r_vFailedArray(,) As Object, ByRef r_vResultArray() As Object) As Integer

        Dim result As Integer = 0
        Try

            Dim oRenewals, vControlRecord As Object
            Dim iFailures, iSuccesses As Integer
            Dim sSQL As String = ""
            'Dim lRenInsuranceFileCnt As Long

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Business Object

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oRenewals, v_sClassName:="bSIRRenewalControl.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Perform the action for each Folder
            iFailures = 0

            For Each vFolder As Object In v_vSelectedArray

                m_lReturn = oRenewals.GetRenewalControl(vFolder, vControlRecord)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'General failure
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If



                m_lReturn = RenQuotationBrokerLead(v_lInsuranceFolderCnt:=bz(CByte(vControlRecord(0, 0))), v_lPartyCnt:=bz(CByte(vControlRecord(9, 0))), v_dtRenewalDate:=CDate(vControlRecord(8, 0)), v_lRiskCodeID:=bz(CByte(vControlRecord(10, 0))), v_lGISDataModelID:=bz(CByte(vControlRecord(11, 0))), v_sGisDataModelCode:=v_sDataModelCode, v_lGISSchemeID:=bz(CByte(vControlRecord(1, 0))), v_lProductID:=bz(CByte(vControlRecord(7, 0))), r_lRenewalInsuranceFileCnt:=bz(CByte(vControlRecord(5, 0))), v_lRenewalGISSchemeID:=bz(CByte(vControlRecord(4, 0))), v_sGisBusinessTypeCode:=v_sBusinessTypeCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'Add the entry to the failure array
                    iFailures += 1
                    If Informations.IsArray(r_vFailedArray) Then
                        ReDim Preserve r_vFailedArray(1, iFailures - 1)
                    Else
                        ReDim r_vFailedArray(1, 0)
                    End If


                    r_vFailedArray(0, iFailures - 1) = vControlRecord(16, 0)

                    r_vFailedArray(1, iFailures - 1) = "Error: " & m_sRenTaskLog
                Else
                    'Add the entry to the success array
                    iSuccesses += 1
                    If Informations.IsArray(r_vResultArray) Then
                        ReDim Preserve r_vResultArray(iSuccesses - 1)
                    Else
                        ReDim r_vResultArray(0)
                    End If


                    r_vResultArray(iSuccesses - 1) = vControlRecord(5, 0)

                    'Build the SQL to Mark the Holding Insurer

                    sSQL = "UPDATE " & v_sDataModelCode & "_Quote_Binder " &
                           "SET QB.Holding_Ins_Ind = 1 " &
                           "FROM " & v_sDataModelCode & "_Quote_Binder QB " &
                           "INNER JOIN " & v_sDataModelCode & "_NP_Quote Q ON Q." & v_sDataModelCode & "_Quote_Binder_id=QB." & v_sDataModelCode & "_Quote_Binder_id " &
                           "INNER JOIN GIS_Scheme S ON S.Gis_Scheme_id=Q.Scheme_id " &
                           "INNER JOIN GIS_Policy_link GPL ON GPL.Gis_Policy_link_id=Q.Gis_Policy_link_id " &
                           "INNER JOIN GIS_Scheme S2 ON S2.GIS_Scheme_id=GPL.GIS_Scheme_id " &
                           "INNER JOIN Insurance_file F ON F.Insurance_File_Cnt=GPL.Insurance_File_Cnt " &
                           "WHERE S2.GIS_Insurer_id = s.GIS_Insurer_id " &
                           "AND F.Insurance_Folder_cnt=" & CStr(vControlRecord(0, 0))

                    m_lReturn = m_oDatabase.SQLAction(sSQL, "Mark Holding Insurer", False)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Throw New Exception()
                    End If
                End If

            Next vFolder


            oRenewals.Dispose()
            oRenewals = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMultipleQuotationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMultipleQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: SelectRenewalTaskLog
    '
    ' Description:  Returns the contents of the Renewal Task Log
    '
    ' History:  06/12/2001 DD - Created.
    '           DD, 07/12/2001: Added optional filters.
    '
    ' ***************************************************************** '
    Public Function SelectRenewalTaskLog(ByRef r_vResultArray As Object) As Integer
        Return SelectRenewalTaskLog(r_vResultArray:=r_vResultArray, v_vStartDate:=Nothing, v_vEndDate:=Nothing, v_vStatus:=Nothing, v_vPolicyNo:=Nothing)
    End Function

    Public Function SelectRenewalTaskLog(ByRef r_vResultArray As Object, ByVal v_vStartDate As Object, ByVal v_vEndDate As Object, ByVal v_vStatus As Object, ByVal v_vPolicyNo As Object) As Integer

        Dim result As Integer = 0
        Dim oTaskLog As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the Business Object

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oTaskLog, v_sClassName:="bSIRRenTaskLog.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Get the Task Log

            m_lReturn = oTaskLog.SelectRenewalTaskLog(r_vResultArray, v_vStartDate, v_vEndDate, v_vStatus, v_vPolicyNo)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Shut Down

            oTaskLog.Dispose()
            oTaskLog = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectRenewalTaskLog Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRenewalTaskLog", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenIsActiveRenewal
    '
    ' Description:  Returns the Insurance Folder and Status if the File
    '               or Reference is currently an active Renewal.
    '
    ' History: 30/01/2002 DD - Created.
    '
    ' ***************************************************************** '
    Public Function RenIsActiveRenewal(ByVal v_lInsuranceFileCnt As Object, ByVal v_sInsuranceRef As Object, ByRef r_lInsuranceFolderCnt As String, ByRef r_sStatus As String, ByRef r_sNewPolicyNo As String, ByRef r_dRenewalDate As Object, ByRef r_sInsurer As String, ByRef r_sScheme As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set up the parameters
            m_oDatabase.Parameters.Clear()


            m_oDatabase.Parameters.Add("InsuranceFileCnt", CStr(If(CDbl(v_lInsuranceFileCnt) = 0, DBNull.Value, v_lInsuranceFileCnt)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)


            m_oDatabase.Parameters.Add("InsuranceRef", CStr(If(CStr(v_sInsuranceRef) = "", DBNull.Value, v_sInsuranceRef)), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMString)

            'Run the SP

            m_lReturn = m_oDatabase.SQLSelect("spu_SirRen_IsActiveRenewal", "Is Active Renewal", True, , vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return m_lReturn
            End If

            'Determine the result
            If Not Informations.IsArray(vResultArray) Then
                r_lInsuranceFolderCnt = CStr(0)
                r_sStatus = ""
                r_sNewPolicyNo = ""

                r_dRenewalDate = ""
                r_sInsurer = ""
                r_sScheme = ""
            Else

                r_lInsuranceFolderCnt = CStr(vResultArray(0, 0))

                r_sStatus = CStr(vResultArray(1, 0))

                r_sNewPolicyNo = CStr(vResultArray(2, 0))


                r_dRenewalDate = CDate(vResultArray(3, 0))

                r_sInsurer = CStr(vResultArray(4, 0))

                r_sScheme = CStr(vResultArray(5, 0))
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenIsActiveRenewal Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenIsActiveRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    'JES 250402 - sets up risk link
    'UPGRADE_NOTE: (7001) The following declaration (CreateRiskLink) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CreateRiskLink(ByRef v_lNewInsuranceFileCnt As Integer, ByRef v_lOldInsuranceFileCnt As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'Dim sSQL As String = ""
    'Dim vData As Object
    '
    'sSQL = "INSERT INTO insurance_file_risk_link (" & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "insurance_file_cnt," & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "status_flag, " & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "original_risk_cnt)" & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "SELECT " & CStr(v_lNewInsuranceFileCnt) & "," & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "risk_cnt," & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "'U'," & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "original_risk_cnt" & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "FROM insurance_file_risk_link" & Strings.ChrW(13) & Strings.ChrW(10) &  _
    '       "WHERE insurance_file_cnt = " & CStr(v_lOldInsuranceFileCnt) & Strings.ChrW(13) & Strings.ChrW(10)
    '
    'm_oDatabase.Parameters.Clear()
    '
    'm_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="CopyInsuranceFileRiskLink", bStoredProcedure:=False)
    '
    ' Check for errors
    '
    '*********** ERROR HANDLING
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return m_lReturn
    'Else
    'Return gPMConstants.PMEReturnCode.PMTrue

    '
    'Catch 

    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMError
    '
    ' Log Error Message
    'bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Risk Link Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenIsActiveRenewal", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
    '
    'Return result
    '


    ' ***************************************************************** '
    ' Name          : CopyRiskData
    '
    ' Description   : copy all Risks attached to OldInsuranceFileCnt to NewInsuranceFileCnt
    '                 copy all GIS details attached to each risk to NewInsuranceFileCnt
    '
    ' Author        : Ram Chandrabose
    ' Created on    : 21/05/2002
    ' Edit History  :
    ' RAM20020521   : Created
    ' ***************************************************************** '
    Public Function CopyRiskData(ByVal v_lOldInsuranceFileCnt As Integer, ByVal v_lNewInsuranceFileCnt As Integer, ByRef r_lNewRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vRiskArray(,) As Object
        Dim lNewRiskCnt As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'get all risks associate with OldInsuranceFileCnt
            If GetRisk(v_lInsuranceFileCnt:=v_lOldInsuranceFileCnt, r_vResultArray:=vRiskArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'do we have any risks
            If Not Informations.IsArray(vRiskArray) Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'loop thro and copy each risk details
            ' Note : Over here we always have one Risk,
            '        In the future, there may be many risks associated with Insurance File (multiple versions)

            For lCount As Integer = 0 To vRiskArray.GetUpperBound(1)

                'copy risk to NewInsuranceFileCnt

                m_lReturn = CopyRisk(v_lNewInsuranceFileCnt:=v_lNewInsuranceFileCnt, v_vRiskDetail:=vRiskArray, v_lPosNo:=lCount, r_lRiskCnt:=lNewRiskCnt)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                ' Return the newly created RiskCnt
                r_lNewRiskCnt = lNewRiskCnt

            Next lCount

            ' Return the newly created Rick Cnt
            r_lNewRiskCnt = lNewRiskCnt

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="CopyRiskData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyRiskData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name          : GetRisk
    '
    ' Description   : get all associate risks for policy for the supplied
    '                 InsuranceFileCnt
    '
    ' Author        : Ram Chandrabose
    ' Created on    : 21/05/2002
    ' Edit History  :
    ' RAM20020521   : Created
    ' ***************************************************************** '
    Private Function GetRisk(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const ACSaaRiskStored As Boolean = True
        Const ACSaaRiskName As String = "GetAllRisks"
        Const ACSaaRiskSQL As String = "spe_Risk_saa"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        result = m_oDatabase.SQLSelect(sSQL:=ACSaaRiskSQL, sSQLName:=ACSaaRiskName, bStoredProcedure:=ACSaaRiskStored, vResultArray:=r_vResultArray, bKeepNulls:=True)

        If Informations.IsArray(r_vResultArray) Then
        Else
            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Stored procedure spe_Risk_saa( " & v_lInsuranceFileCnt & " ) returns no records", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRisk", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : CopyRisk
    '
    ' Description   : copy risk details from OldInsuranceFileCnt to NewInsuranceFileCnt
    '
    ' Author        : Ram Chandrabose
    ' Created on    : 21/05/2002
    ' Edit History  :
    ' RAM20020521   : Created
    ' ***************************************************************** '
    Private Function CopyRisk(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_vRiskDetail(,) As Object, ByVal v_lPosNo As Integer, ByRef r_lRiskCnt As Integer) As Integer

        Dim result As Integer = 0
        Const ACAddRiskStored As Boolean = True
        Const ACAddRiskName As String = "AddRisk"

        Const ACAddRiskSQL As String = "spe_Risk_add"

        Dim lRiskCnt, lOldRiskCnt, lNewRiskCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue


        lOldRiskCnt = CInt(v_vRiskDetail(0, v_lPosNo))
        lNewRiskCnt = 0

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(0), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_status_id", vValue:=CStr(v_vRiskDetail(1, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_folder_cnt", vValue:=CStr(v_vRiskDetail(2, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="accumulation_id", vValue:=CStr(v_vRiskDetail(3, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_type_id", vValue:=CStr(v_vRiskDetail(4, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="description", vValue:=CStr(v_vRiskDetail(5, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="sequence_number", vValue:=CStr(v_vRiskDetail(6, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="sum_insured_requested", vValue:=CStr(v_vRiskDetail(7, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="inception_date", vValue:=CStr(v_vRiskDetail(8, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="expiry_date", vValue:=CStr(v_vRiskDetail(9, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_not_index_linked", vValue:=CStr(v_vRiskDetail(10, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_accumulated", vValue:=CStr(v_vRiskDetail(11, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_reason_id", vValue:=CStr(v_vRiskDetail(12, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_date", vValue:=CStr(v_vRiskDetail(13, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="lapsed_description", vValue:=CStr(v_vRiskDetail(14, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="var_data_ref", vValue:=CStr(v_vRiskDetail(15, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="total_sum_insured", vValue:=CStr(v_vRiskDetail(16, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="total_annual_premium", vValue:=CStr(v_vRiskDetail(17, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="total_this_premium", vValue:=CStr(v_vRiskDetail(18, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_ri_at_risk_level", vValue:=CStr(v_vRiskDetail(19, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="is_auto_reinsured", vValue:=CStr(v_vRiskDetail(20, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_screen_id", vValue:=CStr(v_vRiskDetail(21, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_lReturn = m_oDatabase.Parameters.Add(sName:="eml_percentage", vValue:=CStr(v_vRiskDetail(21, v_lPosNo)), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDouble)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskSQL, sSQLName:=ACAddRiskName, bStoredProcedure:=ACAddRiskStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        lRiskCnt = m_oDatabase.Parameters.Item("risk_cnt").Value

        If lRiskCnt <> 0 Then
            m_lReturn = AddRiskLink(v_lNewInsuranceFileCnt, lRiskCnt, "C")

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            r_lRiskCnt = lRiskCnt

            lNewRiskCnt = lRiskCnt

        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : CopyRisk
    '
    ' Description   : Adds record to Insurance_file_risk_link.
    '
    ' Author        : Ram Chandrabose
    ' Created on    : 21/05/2002
    ' Edit History  :
    ' RAM20020521   : Created
    ' ***************************************************************** '
    Private Function AddRiskLink(ByVal v_lNewInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_sStatusFlag As String, Optional ByVal v_lOriginalRiskCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const ACAddRiskLinkStored As Boolean = True
        Const ACAddRiskLinkName As String = "AddRiskLink"

        Const ACAddRiskLinkSQL As String = "spe_insurance_file_risk_li_add"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lNewInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_cnt", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="status_flag", vValue:=v_sStatusFlag, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        If (False) Or (v_lOriginalRiskCnt.Equals(0)) Then


            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_risk_cnt", vValue:=(DBNull.Value), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else
            m_lReturn = m_oDatabase.Parameters.Add(sName:="original_risk_cnt", vValue:=CStr(v_lOriginalRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACAddRiskLinkSQL, sSQLName:=ACAddRiskLinkName, bStoredProcedure:=ACAddRiskLinkStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenTransferPolicyToStandardRenewals
    '
    ' Description:
    '
    ' History: 26/03/2001 RFC - Created.
    '
    ' ***************************************************************** '
    Public Function RenTransferPolicyToStandardRenewals(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sTransferReason As String, ByVal v_sTransferNotes As String) As Integer

        Dim result As Integer = 0
        Dim oSBOLink As bSIRIUSLink.Renewals

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            oSBOLink = New bSIRIUSLink.Renewals
            m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "RenTransferPolicyToStandardRenewals")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                Return result
            End If


            m_lReturn = oSBOLink.RenTransferPolicyToStandardRenewals(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lPartyCnt:=v_lPartyCnt, v_sTransferReason:=v_sTransferReason, v_sTransferNotes:=v_sTransferNotes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "RenTransferPolicyToStandardRenewals Failed", ACApp, ACClass, "RenTransferPolicyToStandardRenewals")
                result = gPMConstants.PMEReturnCode.PMFalse
                oSBOLink = Nothing
                Return result
            End If



            oSBOLink.Dispose()

            oSBOLink = Nothing

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenTransferPolicyToStandardRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenTransferPolicyToStandardRenewals", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

            Return result
        End Try
    End Function



    ' ***************************************************************** '
    ' Name: RenAutoConfirmRiskChangeBOLink
    '
    ' Description:
    '
    ' History: 23/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function RenAutoConfirmRiskChangeBOLink(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_sNotes As String = "") As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oSBOLink As bSIRIUSLink.Renewals
        Dim lEventCnt As Integer


        oSBOLink = New bSIRIUSLink.Renewals
        m_lReturn = oSBOLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "Failed to get instance of bSIRIUSLink.Renewals", ACApp, ACClass, "RenAutoConfirmRiskChangeBOLink")
            result = gPMConstants.PMEReturnCode.PMFalse
            oSBOLink = Nothing
            Return result
        End If

        'Create the suspend event

        m_lReturn = oSBOLink.SuspendEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSuspendReason:=PMRenewalEventTypeRiskChangeSuspension, r_lEventCnt:=lEventCnt, v_sShortSuspendReason:=PMRenewalEventTypeRiskChangeSuspensionShort, v_sNotes:=v_sNotes)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            GISSharedConstants.LogMessageFile(gPMConstants.PMELogLevel.PMLogOnError, "RenAutoConfirmRiskChangeBOLink Failed", ACApp, ACClass, "RenAutoConfirmRiskChangeBOLink")
            result = gPMConstants.PMEReturnCode.PMFalse
            oSBOLink = Nothing
            Return result
        End If

        'Update the renewal control record with the event_cnt
        m_lReturn = AttachEventToRenewalControl(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lEventCnt:=lEventCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AttachEventToRenewalControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenAutoConfirmRiskChangeBOLink")
            Return result
        End If

        'Update edi_message_sent flag on insurance_file

        m_lReturn = oSBOLink.UpdateEdiMessageSent(v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSBOLink.UpdateLastEdiMessageCountSent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenAutoConfirmRiskChangeBOLink")
            Return result
        End If

        'Make this version the current renewal version

        m_lReturn = oSBOLink.NewInsuranceFile(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lInsuranceFileCnt:=v_lInsuranceFileCnt)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="oSBOLink.NewInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenAutoConfirmRiskChangeBOLink")
            Return result
        End If


        oSBOLink.Dispose()
        oSBOLink = Nothing


        Return result

    End Function


    ' ***************************************************************** '
    ' Name: AttachEventToRenewalControl
    '
    ' Description:
    '
    ' History: 23/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function AttachEventToRenewalControl(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lEventCnt As Integer) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACRenewalSuspensionUpdateSQL As String = "spu_SIR_renewal_suspension_upd"
        Const ACRenewalSuspensionUpdateStored As Boolean = True
        Const ACRenewalSuspensionUpdateName As String = "RenewalSuspensionUpdate"

        With m_oDatabase
            .Parameters.Clear()

            ' Suspension Level
            m_lReturn = .Parameters.Add(sName:="suspension_level", vValue:=CStr(v_lEventCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Insurance Folder Cnt
            m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACRenewalSuspensionUpdateSQL, sSQLName:=ACRenewalSuspensionUpdateName, bStoredProcedure:=ACRenewalSuspensionUpdateStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End With

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: AutoConfirmRiskChangeGetData
    '
    ' Description:
    '
    ' History: 23/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (AutoConfirmRiskChangeGetData) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function AutoConfirmRiskChangeGetData(ByVal v_sAlternateReference As String, ByRef r_lPartyCnt As Integer, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lGisSchemeId As Integer, ByRef r_sGISDataModelCode As String) As Integer
    '
    'Dim result As Integer = 0
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'Const ACAutoConfirmRiskChangeSQL As String = "{call spu_SirRen_AutoConfirmRiskChange_Sel(?)}"
    'Const ACAutoConfirmRiskChangeStored As Boolean = True
    'Const ACAutoConfirmRiskChangeName As String = "AutoConfirmRiskChange"
    '
    'Const ACAPartyCnt As Integer = 0
    'Const ACAInsuranceFolderCnt As Integer = 1
    'Const ACAInsuranceFileCnt As Integer = 2
    'Const ACAGisDataModelCode As Integer = 3
    'Const ACAGisSchemeId As Integer = 4
    '
    'Dim vResultArray(,) As Object
    '
    'With m_oDatabase
    '.Parameters.Clear()
    '
    ' Alternate Reference
    'm_lReturn = .Parameters.Add(sName:="alternate_reference", vValue:=gpmfunctions.ToSafeString(v_sAlternateReference), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse

    '
    'm_lReturn = .SQLSelect(sSQL:=ACAutoConfirmRiskChangeSQL, sSQLName:=ACAutoConfirmRiskChangeName, bStoredProcedure:=ACAutoConfirmRiskChangeStored, vResultArray:=vResultArray)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    'Return gPMConstants.PMEReturnCode.PMFalse


    '
    'If Not Informations.IsArray(vResultArray) Then
    'Return gPMConstants.PMEReturnCode.PMNotFound

    '

    'r_lPartyCnt = CInt(vResultArray(ACAPartyCnt, 0))

    'r_lInsuranceFileCnt = CInt(vResultArray(ACAInsuranceFileCnt, 0))

    'r_lInsuranceFolderCnt = CInt(vResultArray(ACAInsuranceFolderCnt, 0))

    'r_lGisSchemeId = CInt(vResultArray(ACAGisSchemeId, 0))

    'r_sGISDataModelCode = CStr(vResultArray(ACAGisDataModelCode, 0))
    '
    'Return result
    '
    'Catch excep As System.Exception
    '
    '
    '
    'result = gPMConstants.PMEReturnCode.PMFalse
    '
    'bPMFunc.LogMessage(sUserName:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AutoConfirmRiskChangeGetData Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AutoConfirmRiskChangeGetData", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '


    ' ************************************************************************************************************
    '
    ' Function: UpdatePolicyLink
    '
    ' Description: Updates the given policy_link record with the new insurance_file_cnt
    '
    ' ************************************************************************************************************
    Private Function UpdatePolicyLink(ByVal v_lPolicyLinkID As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lRiskCnt As Integer, ByVal v_lGISSchemeID As Integer) As Integer

        Dim result As Integer = 0
        Dim sSQL As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue



        ' Construct the SQL
        sSQL = "UPDATE gis_policy_link SET insurance_file_cnt = {insurance_file_cnt}, risk_id = {risk_id}, gis_scheme_id = {gis_scheme_id} " &
               "WHERE gis_policy_link_id = {gis_policy_link_id}"


        ' Clear the paramaters
        m_oDatabase.Parameters.Clear()

        ' Add the new parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=CStr(v_lRiskCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGISSchemeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=sSQL, sSQLName:="UpdatePolicyLink", bStoredProcedure:=False)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            ' Log Error Message
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdatePolicyLink Failed to call SQL : " & sSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="UpdatePolicyLink", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name          : GetLookupProperties
    '
    ' Description   :
    '
    ' Author        : Steve James
    ' Created on    : 18/06/2004
    ' Edit History  :
    ' ***************************************************************** '
    Public Function GetLookupProperties(ByVal v_sGisDataModelCode As String, ByRef r_oLookupProperties As Hashtable) As Integer
        Return GetLookupProperties(v_sGisDataModelCode:=v_sGisDataModelCode, r_oLookupProperties:=r_oLookupProperties, vDatabase:=Nothing)
    End Function

    Public Function GetLookupProperties(ByVal v_sGisDataModelCode As String, ByRef r_oLookupProperties As Hashtable, ByRef vDatabase As dPMDAO.Database) As Integer


        Dim result As Integer = 0
        Const ACGetLookupPropertiesStored As Boolean = True
        Const ACGetLookupPropertiesName As String = "GetLookupProperties"

        Const ACGetLookupPropertiesSQL As String = "spu_gis_get_lookup_properties"

        Const ACObjectName As Integer = 0
        Const ACPropertyName As Integer = 1
        Const ACGisPropertyId As Integer = 2
        Const ACListType As Integer = 3

        Dim vResultArray(,) As Object
        Dim sKey As String = ""
        Dim oDatabase As dPMDAO.Database

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Informations.IsNothing(vDatabase) Then
                oDatabase = m_oDatabase
            Else
                oDatabase = vDatabase
            End If

            r_oLookupProperties = New Hashtable()

            oDatabase.Parameters.Clear()

            m_lReturn = oDatabase.Parameters.Add(sName:="gis_data_model_code", vValue:=v_sGisDataModelCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = oDatabase.SQLSelect(sSQL:=ACGetLookupPropertiesSQL, sSQLName:=ACGetLookupPropertiesName, bStoredProcedure:=ACGetLookupPropertiesStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                oDatabase = Nothing
                Return result
            End If


            For i As Integer = 0 To vResultArray.GetUpperBound(1)


                sKey = BuildLookupPropertyKey(CStr(vResultArray(ACObjectName, i)), CStr(vResultArray(ACPropertyName, i)))

                r_oLookupProperties.Add(sKey, CStr(vResultArray(ACGisPropertyId, i)).Trim())

                r_oLookupProperties.Add(sKey & "|TYPE", CStr(vResultArray(ACListType, i)).Trim())
            Next i

            oDatabase = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="GetLookupProperties Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLookupProperties", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ************************************************************************
    '
    ' Function: InitialiseListManager
    '
    ' ************************************************************************
    Private Function InitialiseListManager(ByVal v_sDataModel As String) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Get an instance of the bGISListManager object
        m_oListManager = New bGISListManager.InterfaceNoLogin

        ' Initialise the object
        lReturn = m_oListManager.Initialise()
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Load the lookups and make sure we have the right versions
        lReturn = CType(m_oListManager.CheckListVersions(v_sDataModel), gPMConstants.PMEReturnCode)
        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    Private Function BuildLookupPropertyKey(ByVal v_sObjectName As String, ByVal v_sPropertyName As String) As String

        Return v_sObjectName.Trim().ToUpper() & ":" & v_sPropertyName.Trim().ToUpper()

    End Function


    ' ***************************************************************** '
    ' Name: GetPMLookupCodeFromID
    '
    ' Description: Get a PMLookup (UDL_) Code from a given ID.
    '
    ' History: 10/11/2004 CJB - Created.
    '
    ' ***************************************************************** '
    Public Function GetPMLookupCodeFromID(ByVal v_sLookupTable As String, ByVal v_sLookupId As String, ByRef r_sLookupCode As String) As Integer

        Dim result As Integer = 0
        Try

            Dim sGetPMLookupCodeFromIDSQL As String = ""
            Const ACGetPMLookupCodeFromIDName As String = "GetPMLookupCodeFromID"
            Const ACGetPMLookupCodeFromIDStored As Boolean = False

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vResultArray(,) As Object

            ' Clear the parameters
            m_oDatabase.Parameters.Clear()

            'build the SQL
            sGetPMLookupCodeFromIDSQL = "select code from " & v_sLookupTable & " where " & v_sLookupTable & "_id" & "=" & v_sLookupId

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=sGetPMLookupCodeFromIDSQL, sSQLName:=ACGetPMLookupCodeFromIDName, bStoredProcedure:=ACGetPMLookupCodeFromIDStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to call SQL : " & sGetPMLookupCodeFromIDSQL, vApp:=ACApp, vClass:=ACClass, vMethod:="GetGetPMLookupCodeFromID")
                Return result
            End If

            ' Check our results
            If Informations.IsArray(vResultArray) Then

                r_sLookupCode = CStr(vResultArray(0, 0)).Trim()
            Else
                result = gPMConstants.PMEReturnCode.PMNotFound
                ' Didn't return anything, lets error
                GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetPMLookupCodeFromIDName & " didn't return any data.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupCodeFromID")
                Return result
            End If

            ' Clear up
            vResultArray = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=gpmfunctions.ToSafeString(m_sUsername), iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPMLookupCodeFromID Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPMLookupCodeFromID", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Class
