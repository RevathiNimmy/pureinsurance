Option Strict Off
Option Explicit On
Imports System.Globalization
'developer guide no. 129
Imports SSP.Shared
<System.Runtime.InteropServices.ProgId("Renewals_NET.Renewals")> _
Public NotInheritable Class Renewals
    Implements IDisposable
    '
    ' History : CJB 20/01/2005 Perkins Slade Retail Logic Integration Development
    '                          Change CopyInsuranceFile to call new function - RenewalsCreditCardSetup -
    '                          to prepare the renewal policy version for possible card processing at the
    '                          debit stage in the renewals cycle.


    ' ************************************************
    ' Added to replace global variables 27/10/2003
    ' Username.
    Private m_sUserName As String = ""

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


    Private Const ACClass As String = "Renewals"
    'developer guide no 39. 
    Private Const ACSelectRenewalVersionsSQL As String = "spu_SIRRen_GetPolRen"
    Private Const ACSelectRenewalVersionsName As String = "SelectRenVersions"
    Private Const ACSelectRenewalVersionsStored As Boolean = True
    'developer guide no 39.
    Private Const ACRenMtaAtRenewalQuoteSQL As String = "spu_renewal_at_mta_quote_sel"
    Private Const ACREnMtaAtRenewalQuoteName As String = "MtaAtRenewalQuote"
    Private Const ACRenMtaAtRenewalQuoteStored As Boolean = True
    'developer guide no 39.
    Private Const ACSetExistRenToRepSQL As String = "spu_SIR_set_exist_ren_to_rep"
    Private Const ACSetExistRenToRepName As String = "SetExistRenRep"
    Private Const ACSetExistRenToRepStored As Boolean = True
    'developer guide no 39.
    Private Const ACSelectLivePolicySQL As String = "spu_SirRen_Select_Live_Policy"
    Private Const ACSelectLivePolicyName As String = "SelectLivePolicy"
    Private Const ACSelectLivePolicyStored As Boolean = True
    'developer guide no 39.
    Private Const ACUpdateInsFileDatesSQL As String = "spu_SirRen_Ins_File_Dates_Upd"
    Private Const ACUpdateInsFileDatesName As String = "UpdateInsuranceFileDates"
    Private Const ACUpdateInsFileDatesStored As Boolean = True

    'AK 220801 - To check if two insurancefiles belong to the same folder
    'developer guide no 39.
    Private Const ACCompareInsFilesSQL As String = "spu_SirRen_Compare_Ins_Files"
    Private Const ACCompareInsFilesName As String = "CompareInsFiles"
    Private Const ACCompareInsFilesStored As Boolean = True

    'Ak 250402
    'AK 250402
    'developer guide no 39.
    Private Const ACGetLatestVersionSQL As String = "spu_SIR_Get_Ren_PolVersion"
    Private Const ACGetLatestVersionName As String = "GetLatestVersion"
    Private Const ACGetLatestVersionStored As Boolean = True

    ' RAM20040204 : Declared the following Constants - Ref. PN Issue 8555
    'developer guide no 39.
    Private Const ACGetRenewalStopCodeDetailsSQL As String = "spu_GET_Renewal_Stop_Code_Details_For_Policy"
    Private Const ACGetRenewalStopCodeDetailsName As String = "GetRenewalStopCodeDetails"
    Private Const ACGetRenewalStopCodeDetailsStored As Boolean = True

    'SJ 16/04/2004 - start
    'developer guide no 39.
    Private Const ACUpdateLastEdiMessageCountSentSQL As String = "spu_SirRen_last_edi_message_count_sent_upd"
    Private Const ACUpdateLastEdiMessageCountSentName As String = "UpdateLastEdiMessageCountSent"
    Private Const ACUpdateLastEdiMessageCountSentStored As Boolean = True
    'developer guide no 39.
    Private Const ACUpdateAlternateReferenceSQL As String = "spu_SirRen_AlternateRef_upd"
    Private Const ACUpdateAlternateReferenceName As String = "AlternateReference"
    Private Const ACUpdateAlternateReferenceStored As Boolean = True
    'developer guide no 39.
    Private Const ACUpdateEdiMessageSentSQL As String = "spu_SirRen_edi_message_sent_upd"
    Private Const ACUpdateEdiMessageSentName As String = "UpdateEdiMessageSent"
    Private Const ACUpdateEdiMessageSentStored As Boolean = True
    'SJ 16/04/2004 - end

    'SJ 12/05/2004 - start
    'developer guide no 39.
    Private Const ACLapsedReasonSelSQL As String = "spu_SIRRen_Lapsed_Reason_sel"
    Private Const ACLapsedReasonSelName As String = "LapsedReasonSel"
    Private Const ACLapsedReasonSelStored As Boolean = True
    'SJ 12/05/2004 - end

    'IJM 220801 - Returned array constants
    Private Const ACInsuranceFileCnt As Integer = 0
    Private Const ACProductId As Integer = 1
    Private Const ACGisSchemeId As Integer = 2
    Private Const ACIsInsurerLead As Integer = 3
    Private Const ACPartyCnt As Integer = 4
    Private Const ACRenewalDate As Integer = 5
    Private Const ACGISDataModelID As Integer = 6
    Private Const ACGISDataModel As Integer = 7
    Private Const ACGisPolicyLinkId As Integer = 8

    Private Const ACAutoRenLapseReason As String = "RENMANAUTO"
    Private m_lReturn As Integer

    Private m_oSirEvent As bSIREvent.Business
    Private m_oLookup As bPMLookup.Business
    Private m_oRenewalControl As Object
    'Insurance Object
    Private m_oInsurance As Object 'bSIRInsuranceFile.Services

    ''AK 091101
    'Private m_oDebugTimings As New DebugTimings


    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' ***************************************************************** '
    ' Name: Initialise (Standard Method)
    '
    ' Description: Entry point for any initialisation code for this
    '              object.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByRef sUsername As String, ByRef sPassword As String, ByRef iUserID As Integer, ByRef iSourceID As Integer, ByRef iLanguageID As Integer, ByRef iCurrencyID As Integer, ByRef iLogLevel As Integer, ByRef sCallingAppName As String, Optional ByRef vDatabase As dPMDAO.Database = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            '
            ' *******************************************************************
            ' THESE MUST BE SETUP BEFORE ANY OTHER CODE IS EXECUTED
            ' Set Username and Password
            m_sUserName = sUsername
            m_sPassword = sPassword
            m_iUserID = iUserID
            m_sCallingAppName = sCallingAppName
            m_iLanguageID = iLanguageID
            m_iSourceID = iSourceID
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel



            ' Initialisation Code.
            m_oLookup = New bPMLookup.Business()

            ' Set Username and Password

            ' Set User ID

            ' Set Calling Application

            ' Set Language ID

            ' Set Source ID

            ' Set Currency ID

            ' Set Log Level

            'Create instance of policy events object

            m_oSirEvent = New bSIREvent.Business
            m_lReturn = m_oSirEvent.Initialise(sUsername:=ToSafeString(m_sUserName), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oLookup = Nothing
                Return result
            End If

            ' PM Lookup
            m_lReturn = m_oLookup.Initialise(sUsername:=ToSafeString(m_sUserName), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oLookup = Nothing
                Return result
            End If

            m_oLookup.PMLookupProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions

            ' Create instance of Renewal Control object

            m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oRenewalControl, v_sClassName:="bSIRRenewalControl.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=vDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = m_lReturn
                m_oLookup = Nothing
                Return result
            End If

            'Get the Insurance object
            'm_oInsurance = New bSIRInsuranceFile.Services
            m_oInsurance = Nothing
            If m_oInsurance Is Nothing Then
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oInsurance, v_sClassName:="bSIRInsuranceFile.Services", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bSIRInsuranceFile.Services"
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If
            m_lReturn = m_oInsurance.Initialise(sUserName:=ToSafeString(m_sUserName), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If m_oInsurance Is Nothing Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_oDatabase = vDatabase

            'SSL 09/7/10 - dereference the ComponentServices

            ''AK 091101
            '    With m_oDebugTimings
            '        .UserId = m_iUserID
            '        .SaveAsFile = True
            '        .PrintToScreen = True
            '    End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message

            TempFunc.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="InitialiseFailed", sUsername:=m_sUserName, vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

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
                If m_oSirEvent IsNot Nothing Then
                    m_oSirEvent.Dispose()
                    m_oSirEvent = Nothing
                End If
                If m_oLookup IsNot Nothing Then
                    m_oLookup.Dispose()
                    m_oLookup = Nothing
                End If
                If m_oRenewalControl IsNot Nothing Then
                    m_oRenewalControl.Dispose()
                    m_oRenewalControl = Nothing
                End If
                If m_oInsurance IsNot Nothing Then
                    m_oInsurance.Dispose()
                    m_oInsurance = Nothing
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    ' Name: PreRenSelected
    ' Description:
    ' Edit History  :
    '  28/03/2001 SJ - Created.
    '  02/11/2001 AK - Added parameter for passing InsuranceFileCnt
    '  RAM20040204   - Bug fix for PN Issue 8555.
    '                  Note :The fix involves a new stored procedure
    '                        spu_GET_Renewal_Stop_Code_Reason_For_Policy
    '  CJB20040811   - PN14031 Cater for 'Refer at Renewal' indicator being
    '                  set on the policy record and suspend if set.
    ' ***************************************************************** '
    Public Function PreRenSelected(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_lGisDataModelId As Object = Nothing, Optional ByVal v_lSuspensionLevel As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        'AK 021101 - suspension label forrecords where NCD needs to be updated by the user
        Const ACSuspendReason As String = " Claims exist for this policy, NCD needs to be updated"

        ''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040204 : Bug fix for PN Issue 8555 - START
        ''''''''''''''''''''''''''''''''''''''''''''''''''
        Dim lRenewalMethodID As Integer
        Dim sRenewalMethodDescription As String = ""
        Dim lRenewalStopCodeID As Integer
        Dim sRenewalStopCodeDescription As String = ""
        ''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040204 : Bug fix for PN Issue 8555 - END
        ''''''''''''''''''''''''''''''''''''''''''''''''''

        ' CJB 110804 PN14031
        Dim bReferAtRenewal As Boolean
        Const ACReferAtRenewalSuspendReason As String = " Refer at Renewal Indicator has been set on this policy"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeDescPreRenewalSelect)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelectedPolicyEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelected")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040204 : If the suspension reason is 0, then check if there
            '               is any renewal stop code, set in the Policy.
            '               i.e. renewal_stop_code_id in insurance_file table
            '               if it is set, fetch the description for that ID and
            '               create a suspend reason event
            '               Bug fix for PN Issue 8555  - START
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            m_lReturn = GetRenewalStopCodeDetails(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_lRenewalMethodID:=lRenewalMethodID, r_sRenewalMethodDescription:=sRenewalMethodDescription, r_lRenewalStopCodeID:=lRenewalStopCodeID, r_sRenewalStopCodeDescription:=sRenewalStopCodeDescription)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get RenewalStopCode Details.", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelected")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If lRenewalStopCodeID > 0 Then
                ' we have a renewal stop code, so create a renewal suspension event
                ' with the resaon we got from the RenewalStopCode Description

                m_lReturn = CreateSuspendEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=sRenewalStopCodeDescription, r_lEventCnt:=v_lSuspensionLevel)
            Else

                '  CJB20040811 PN14031 Cater for 'Refer at Renewal' indicator being
                '  set on the policy record and suspend if set.
                m_lReturn = GetPolicyReferAtRenewalIndicatorValue(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_bReferAtRenewal:=bReferAtRenewal)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyReferAtRenewalIndicatorValue failed with m_lReturn:" & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelected")
                    Return m_lReturn
                End If

                If bReferAtRenewal Then
                    m_lReturn = CreateSuspendEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=ACReferAtRenewalSuspendReason, r_lEventCnt:=v_lSuspensionLevel)
                Else

                    'AK 021101 - If suspension level has been passed as non zero, a suspension event
                    '            will need to be created
                    If v_lSuspensionLevel > 0 Then
                        m_lReturn = CreateSuspendEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=ACSuspendReason, r_lEventCnt:=v_lSuspensionLevel)
                    End If
                End If
            End If

            ' Check if any error
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Log Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelected Policy Suspension Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelected")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
            ' RAM20040204 : Bug fix for PN Issue 8555  - END
            ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

            'Create Renewal Control
            'AK 021101 - added InsuranceFileCnt in the parameter list
            m_lReturn = PreRenSelectedRenewalControl(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_dtRenewalDate:=v_dtRenewalDate, v_lRiskCodeID:=v_lRiskCodeID, v_lGisSchemeId:=v_lGisSchemeId, v_lProductID:=v_lProductID, v_lInsuranceFileCnt:=v_lInsuranceFileCnt, v_lSuspensionLevel:=v_lSuspensionLevel, v_lGisDataModelId:=v_lGisDataModelId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelectedRenewalControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelected")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelected Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelected", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenSelected
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          30/10/2001 Thinh Nguyen add optional param for suspension level
    ' ***************************************************************** '
    Public Function RenSelected(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, Optional ByVal v_lSuspensionLevel As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '    'AK 091101
            '    m_oDebugTimings.StartBlock
            '    m_oDebugTimings.PrintDebugMessage "bSiriusLink - RenSelectedRenewalControl - Start"

            '30/10/2001 Thinh Nguyen add suspension level
            'Update Renewal Control
            m_lReturn = RenSelectedRenewalControl(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lGisSchemeId:=v_lGisSchemeId, v_lProductID:=v_lProductID, v_lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=v_lRenewalGISSchemeID, v_dtRenewalDate:=v_dtRenewalDate, v_lGisDataModelId:=v_lGisDataModelId, v_lSuspensionLevel:=v_lSuspensionLevel)

            '    'AK 091101
            '    m_oDebugTimings.PrintDebugMessage "bSiriusLink - RenSelectedRenewalControl  "

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelectedRenewalControl Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelected")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create the Policy Event

            '    m_lReturn = RenSelectedPolicyEvent( _
            ''        v_lPartyCnt:=v_lPartyCnt, _
            ''        v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt _
            ''        )

            'AK 161001 - use this method instead
            '    'AK 091101
            '    m_oDebugTimings.StartBlock
            '    m_oDebugTimings.PrintDebugMessage "bSiriusLink - CreatePolicyEvent - Start"

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeDescRenewalSelect)

            '    'AK 091101
            '    m_oDebugTimings.PrintDebugMessage "bSiriusLink - CreatePolicyEvent  "
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelectedPolicyEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelected")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelected Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelected", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateRenPolicyVersion
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function CreateRenPolicyVersion(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_lRenewalInsuranceFileCnt As Integer, ByRef r_lProductId As Integer, ByRef r_lGisSchemeId As Integer, ByRef r_lIsInsurerLead As Integer, ByRef r_lPartyCnt As Integer, ByRef r_dtRenewalDate As Date, ByRef r_lGISDataModelID As Integer, ByRef r_sGISDataModelCode As String, ByRef r_lGISPolicyLinkID As Integer, Optional ByRef r_lInsuranceFileCnt As Integer = 0, Optional ByVal v_sInsFileType As String = ACPMInsFileTypeRenewal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set any existing renewal quotes to replaced
            m_lReturn = SetExistingRenQuotesToReplaced(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to set existing quotes to replaced", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenPolicyVersion")
                Return result
            End If

            ' Get the current live policy version
            m_lReturn = GetCurrentPolicyVersion(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_lInsuranceFileCnt:=r_lInsuranceFileCnt, r_lProductId:=r_lProductId, r_lGisSchemeId:=r_lGisSchemeId, r_lIsInsurerLead:=r_lIsInsurerLead, r_lPartyCnt:=r_lPartyCnt, r_dtRenewalDate:=r_dtRenewalDate, r_lGISDataModelID:=r_lGISDataModelID, r_sGISDataModelCode:=r_sGISDataModelCode, r_lGISPolicyLinkID:=r_lGISPolicyLinkID)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get current live policy version", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenPolicyVersion")
                Return result
            End If

            r_sGISDataModelCode = r_sGISDataModelCode.Trim()

            'Make a copy of the current live policy
            m_lReturn = CopyInsuranceFile(v_lOldPolicyKey:=r_lInsuranceFileCnt, r_lNewPolicyKey:=r_lRenewalInsuranceFileCnt, v_InsFileType:=v_sInsFileType, v_bUpdateYears:=True, v_bRenewalsMode:=True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMError

                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create renewal version of insurance file", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenPolicyVersion")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateRenPolicyVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateRenPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotedInsurerLead
    '
    ' Description:
    '
    ' History: 02/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenQuotedInsurerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_cPremium As Decimal, ByVal v_cIPT As Decimal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenQuotedInsurerLead")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update Insurance File with premium details
            With m_oInsurance


                .InsuranceFileStructure = "GEM"

                .InsuranceFileCnt = v_lRenewalInsuranceFileCnt

                .InsuranceFileID = v_lRenewalInsuranceFileCnt

                .ThisPremium = v_cPremium

                .TaxAmount = v_cIPT

                .NetPremium = v_cPremium - v_cIPT

                .IPTableAmount = v_cPremium - v_cIPT

                .GeminiPolicyStatus = 7

                m_lReturn = .UpdatePolicy
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update premium details on Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            ' Update the renewal control

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalQuoted), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vRenewalEDIAuditID:=ToSafeInteger(v_lRenewalEdiAuditId))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeDescQuoteInsurerSelect)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenQuotedInsurerLead")

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenQuotedInsurerLead")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotedInsurerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: ReplacementEDIRenMsg
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function ReplacementEDIRenMsg() As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ReplacementEDIRenMsg Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ReplacementEDIRenMsg", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotedBrokerLead
    '
    ' Description:
    '
    ' History: 27/04/2001 CTAF - Created.
    '          130901     AK   - need to update policy record with the premium details,
    '                             so do get premium and ipt in the parameter list
    '
    ' ***************************************************************** '
    Public Function RenQuotedBrokerLead(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_cPremium As Decimal, ByVal v_cIPT As Decimal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim oPF As Object 'bSIRPremiumFinance.Business
        Dim sRenewalStatusTypeCode, sRenewalEventTypeDescription As String
        Dim sLapsedReasonDesc As String = String.Empty
        Dim lLapsedReasonId As Integer

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenQuotedBrokerLead")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SJ 12/05/2004 - start
            'Check for lapse
            m_lReturn = GetLapsedReason(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lGisDataModelId:=v_lGisDataModelId, v_lGisSchemeId:=v_lGisSchemeId, r_sLapsedReasonDesc:=sLapsedReasonDesc, r_lLapsedReasonId:=lLapsedReasonId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLapsedReason Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead")
                Return result
            End If

            If sLapsedReasonDesc = "" Then
                sRenewalStatusTypeCode = PMRenewalStatusTypeRenewalQuoted
                sRenewalEventTypeDescription = PMRenewalEventTypeDescQuoteBrokerSelect
            Else
                sRenewalStatusTypeCode = PMRenewalStatusTypePolicyLapseConfirmed
                sRenewalEventTypeDescription = PMRenewalEventTypeLapseConfirmed & ": " & sLapsedReasonDesc
            End If
            'SJ 12/05/2004 - end

            'AK 130901
            ' Update Insurance File with premium details
            With m_oInsurance


                .InsuranceFileStructure = "GEM"

                .InsuranceFileCnt = v_lRenewalInsuranceFileCnt

                .InsuranceFileID = v_lRenewalInsuranceFileCnt

                .ThisPremium = v_cPremium

                .TaxAmount = v_cIPT

                .NetPremium = v_cPremium - v_cIPT

                .IPTableAmount = v_cPremium - v_cIPT

                .GeminiPolicyStatus = 7
                If lLapsedReasonId > 0 Then

                    .LapsedReasonid = lLapsedReasonId
                End If


                m_lReturn = .UpdatePolicy
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update premium details on Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With


            ' Update the renewal control
            ' TF120202 - Dont pass v_vSuspensionLevel:=0


            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(sRenewalStatusTypeCode), v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vRenewalEDIAuditID:=DBNull.Value)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=sRenewalEventTypeDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'DD 15/12/2003 Generate a Renewal Instalments Quote
            'oPF = New bSIRPremiumFinance.Business
            oPF = Nothing
            If m_oInsurance Is Nothing Then
                result = gPMComponentServices.CreateBusinessObject(r_oObject:=oPF, v_sClassName:="bSIRPremiumFinance.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
                If result <> gPMConstants.PMEReturnCode.PMTrue Then
                    Dim r_sMessage As String = "Failed to create an instance of bSIRPremiumFinance.Business"
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                    Return result
                End If
            End If


            m_lReturn = oPF.Initialise(sUsername:=ToSafeString(m_sUserName), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create bSIRPremiumFinance object.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvited", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'Generate the Instalment quote.

            m_lReturn = oPF.GenerateRenewalQuote(ToSafeInteger(v_lRenewalInsuranceFileCnt))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to generate a Renewal Instalments Quote.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvited", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            'And finish

            oPF.Dispose()
            oPF = Nothing

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenQuotedBrokerLead")

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenQuotedBrokerLead")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotedBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    ' Name: GetLapsedReason
    '
    ' Description:
    '
    ' History: 12/05/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetLapsedReason(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lGisDataModelId As Integer, ByVal v_lGisSchemeId As Integer, ByRef r_sLapsedReasonDesc As String, ByRef r_lLapsedReasonId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Const ACLRId As Integer = 0
        Const ACLRDescription As Integer = 2

        Dim vResultArray(,) As Object = Nothing
        Dim sLapsedReasonCode As String = ""

        r_sLapsedReasonDesc = ""
        r_lLapsedReasonId = 0

        ' Clear the Parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_data_model_id", vValue:=CStr(v_lGisDataModelId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_scheme_id", vValue:=CStr(v_lGisSchemeId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACLapsedReasonSelSQL, sSQLName:=ACLapsedReasonSelName, bStoredProcedure:=ACLapsedReasonSelStored, vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'Don't fail here as the output property or table may not exist
            Return result
        End If

        If Not Informations.IsArray(vResultArray) Then
            Return result
        End If


        If CStr(vResultArray(ACLRDescription, 0)).Trim() <> "" Then

            r_lLapsedReasonId = ToSafeInteger(vResultArray(ACLRId, 0))

            r_sLapsedReasonDesc = CStr(vResultArray(ACLRDescription, 0)).Trim()
        End If

        Return result

    End Function


    ' ***************************************************************** '
    '
    ' Name: RenQuotedInsurerLeadRebroke
    '
    ' Description:
    '
    ' History: 27/04/2001 CTAF - Created.
    '          130901     AK   - need to update policy record with the premium details,
    '                             so do get premium and ipt in the parameter list
    '
    ' ***************************************************************** '
    Public Function RenQuotedInsurerLeadRebroke(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_cPremium As Decimal, ByVal v_cIPT As Decimal) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenQuotedInsurerLeadRebroke")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'AK 130901
            ' Update Insurance File with premium details
            '    With m_oInsurance
            '
            '        .InsuranceFileStructure = "GEM"
            '        .InsuranceFileCnt = v_lRenewalInsuranceFileCnt
            '        .InsuranceFileID = v_lRenewalInsuranceFileCnt
            '        .ThisPremium = v_cPremium
            '        .TaxAmount = v_cIPT
            '        .NetPremium = v_cPremium - v_cIPT
            '        .IPTableAmount = v_cPremium - v_cIPT
            '        .GeminiPolicyStatus = 7
            '        m_lReturn = .UpdatePolicy
            '        If (m_lReturn& <> PMTrue) Then
            '            RenQuotedInsurerLeadRebroke = PMFalse
            '            ' Log Error Message
            '            LogMessage m_sUsername, _
            ''                iType:=PMLogError, _
            ''                sMsg:="Failed to update premium details on Insurance_File table", _
            ''                vApp:=ACApp, _
            ''                vClass:=ACClass, _
            ''                vMethod:="RenQuotedInsurerLeadRebroke", _
            ''                vErrNo:=Err.Number, _
            ''                vErrDesc:=Err.Description
            '            Exit Function
            '        End If
            '
            '    End With
            ' Update the renewal control
            '    m_lReturn& = m_oRenewalControl.DirectUpdate( _
            ''        v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, _
            ''        v_vProductID:=v_lProductID, _
            ''        v_vRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, _
            ''        v_vRenewalStatusTypeCode:=PMRenewalStatusTypeRenewalQuoted, _
            ''        v_vSuspensionLevel:=0, _
            ''        v_vRenewalGisSchemeID:=v_lRenewalGISSchemeID, _
            ''        v_vGISSchemeID:=v_lGISSchemeID, _
            ''        v_vRenewalDate:=v_dtRenewalDate, _
            ''        v_vGISDataModelID:=v_lGISDataModelID, _
            ''        v_vRenewalEDIAuditID:=Null)
            '    If (m_lReturn& <> PMTrue) Then
            '        RenQuotedInsurerLeadRebroke = PMFalse
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to update renewal control record.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="RenQuotedInsurerLeadRebroke", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeInsurerQuotationRebroke)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedInsurerLeadRebroke", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenQuotedInsurerLeadRebroke")

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenQuotedInsurerLeadRebroke")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotedInsurerLeadRebroke Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedInsurerLeadRebroke", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: SuspendEvent
    '
    ' Description:
    '
    ' History: 20/06/2001 IJM - Created.
    '          07/04/2004 CJB - Changed to cater for optional Short
    '                           Suspension reason (passed from
    '                           bSIRRenewalsManager).
    '
    ' ***************************************************************** '
    Public Function SuspendEvent(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sSuspendReason As String, Optional ByRef r_lEventCnt As Integer = 0, Optional ByRef v_sShortSuspendReason As String = "", Optional ByVal v_sNotes As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CreateSuspendEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=v_sSuspendReason, r_lEventCnt:=r_lEventCnt, v_sShortDescription:=v_sShortSuspendReason, v_sNotes:=v_sNotes)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create suspend event for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SuspendEvent")

                Return gPMConstants.PMEReturnCode.PMFalse

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SuspendEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SuspendEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvited
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    'developer guide no 17. 
    Public Function RenInvited(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_vRenewalEDIAuditID As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenInvited")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Make sure audit id is pukka
            Dim dbNumericTemp As Double
            If Not Double.TryParse(CStr(v_vRenewalEDIAuditID), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                v_vRenewalEDIAuditID = 0
            End If

            ' Update the renewal control

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalInvited), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vRenewalEDIAuditID:=ToSafeInteger(v_vRenewalEDIAuditID))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvited", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeInvited)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvited", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenInvited")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenInvited")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvited Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvited", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitedEdi
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitedEdi(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, Optional ByVal v_bResendMessage As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenInvitedEdi")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_bResendMessage Then
                'Just update the event log and exit
                m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeInvitedEdiResend)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitedEdi")
                    Return result
                End If

                Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenInvitedEdi")
                Return result
            End If

            ' Update the renewal control

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalInvited), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vRenewalEDIAuditID:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitedEdi")
                Return result
            End If

            'Increment the message sent counter and set the edi_message_sent flag on the insurance_file to 1
            m_lReturn = UpdateLastEdiMessageCountSent(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateLastEdiMessageCountSent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitedEdi")
                Return result
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeInvitedEdi)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitedEdi")
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenInvitedEdi")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenInvitedEdi")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitedEdi Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitedEdi", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: RenRevoke
    '
    ' Description:
    '
    ' History: 22/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenRevoke(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_bIsRevokeConfirm As Boolean) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sMessage As String = ""

            If v_bIsRevokeConfirm Then
                sMessage = PMRenewalEventTypeRevokeConfirm
            Else
                sMessage = PMRenewalEventTypeRevokeLapse
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lInsuranceFileCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=sMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenRevoke")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenRevoke Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenRevoke", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenReprintInvitation
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenReprintInvitation(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_vRenewalEDIAuditID As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenReprintInvitation")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeReprintInvitation)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead")
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenReprintInvitation")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenReprintInvitation")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintInvitation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintInvitation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenConfDocsHoldingInsurer
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenConfDocsHoldingInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_vRenewalEDIAuditID As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenConfDocsHoldingInsurer")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeConfDocsHoldingInsurer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenConfDocsHoldingInsurer")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenConfDocsHoldingInsurer")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenConfDocsHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenConfDocsHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenInvitePreferredQuotes
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitePreferredQuotes(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenInvitePreferredQuotes")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Update the renewal control

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalInvited), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vRenewalEDIAuditID:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitePreferredQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeInvitePreferredQuotes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenInvitePreferredQuotes")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenInvitePreferredQuotes")

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitePreferredQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetPolicyRenewalVersions
    '
    ' Description:
    '
    ' History: 21/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function GetPolicyRenewalVersions(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".GetPolicyRenewalVersions")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance folder cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectRenewalVersionsSQL, sSQLName:=ACSelectRenewalVersionsName, bStoredProcedure:=ACSelectRenewalVersionsStored, vResultArray:=r_vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(r_vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".GetPolicyRenewalVersions")

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".GetPolicyRenewalVersions")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetPolicyRenewalVersions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyRenewalVersions", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalQuote
    '
    ' Description:
    '
    ' History: 21/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenMtaAtRenewalQuote(ByVal v_lGisPolicyLinkId As Integer, ByRef r_lInsuranceFolderCnt As Integer, ByRef r_sRenewalStatusTypeCode As String, ByRef r_lRenewalEdiAuditId As Integer, ByRef r_lIsInsurerLead As Integer, ByRef r_lRenewalAtMtaDayNum As Integer, ByRef r_dtRenewalDate As Date, ByRef r_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Const ACInsuranceFolderCnt As Integer = 0
        Const ACRenewalStatusTypeCode As Integer = 1
        Const ACRenewalEdiAuditId As Integer = 2
        Const ACIsInsurerLead As Integer = 3
        Const ACRenewalAtMtaDayNum As Integer = 4
        Const ACRenewalDate As Integer = 5
        Const ACPartyCnt As Integer = 6
        Dim vResultArray(,) As Object = Nothing

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the gis_policy_link_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_lGisPolicyLinkId), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACRenMtaAtRenewalQuoteSQL, sSQLName:=ACREnMtaAtRenewalQuoteName, bStoredProcedure:=ACRenMtaAtRenewalQuoteStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If


            r_lInsuranceFolderCnt = ToSafeInteger(vResultArray(ACInsuranceFolderCnt, 0))

            r_sRenewalStatusTypeCode = CStr(vResultArray(ACRenewalStatusTypeCode, 0)).Trim()

            r_lRenewalEdiAuditId = ToSafeInteger((CStr(vResultArray(ACRenewalEdiAuditId, 0))))

            r_lIsInsurerLead = ToSafeInteger((CStr(vResultArray(ACIsInsurerLead, 0))))

            r_lRenewalAtMtaDayNum = ToSafeInteger((CStr(vResultArray(ACRenewalAtMtaDayNum, 0))))

            r_dtRenewalDate = CDate(vResultArray(ACRenewalDate, 0))

            r_lPartyCnt = ToSafeInteger((CStr(vResultArray(ACPartyCnt, 0))))

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".RenMtaAtRenewalQuote")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalQuote Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalTransact
    '
    ' Description:
    '
    ' History: 12/06/2001 sj - Created.
    '
    ' ***************************************************************** '
    Public Function RenMtaAtRenewalTransact(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_bUseXYZRule As Boolean, ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Reset the renewal Edi Audit Id to null


            m_lReturn = m_oRenewalControl.UpdateRenewalEdiAuditId(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalEDIAuditID:=DBNull.Value)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenewalControl.UpdateRenewalEdiAuditId Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Reset the renewal status to pre-renewal selected
            'AK 011101 - added another parameter in this call

            m_lReturn = m_oRenewalControl.UpdateRenewalStatusType(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_sRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypePreSelection), v_iResetFlag:=ToSafeInteger(ACResetMTA))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oRenewalControl.UpdateRenewalStatusType Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create an entry in the policy event table
            m_lReturn = RenMtaAtRenewalPolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalPolicyEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            If Not v_bUseXYZRule Then
                ' We are not going to recalculate the premium so we need to set the
                ' status on the renewal_edi_audit table to complete
                m_lReturn = UpdateRenewalEdiAuditStatus(v_lRenewalEdiAuditId:=v_lRenewalEdiAuditId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="UpdateRenewalEdiAuditStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenMtaAtRenewalTransact Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalTransact", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenewalConfirmed
    '
    ' Description:
    '
    ' History: 23/05/2001 CTAF - Created.
    '          20/09/01   AK   - added Whatif flag to avoid passing InsuranceFileCnt
    '          22/11/01   AK   - added another optional parameter to handle auto-confirm cases
    '
    ' ***************************************************************** '
    Public Function RenewalConfirmed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, Optional ByVal v_bWhatIf As Boolean = False, Optional ByVal v_bAutoConfirm As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".RenewalConfirmed")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 101201 - Do not need to handle Whatif Cases differently now, as UnConfirm process
            '            can now reset the Renewal_Control data back to original, and reset Insurenac file
            'AK 221101 - need to move the status to confirm pending in case of
            '            insurer led auto-confirm cases
            If v_bAutoConfirm Then

                'AK - 101201 - GIS_Scheme_ID still needs to point at the original scheme on the policy
                '              so do not update it

                m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalPending), v_vRenewalGisSchemeID:=ToSafeInteger(v_lSchemeID), v_vSuspensionLevel:=0)

            Else
                ' Update the control record

                'AK - 101201 - GIS_Scheme_ID still needs to point at the original scheme on the policy
                '              so do not update it

                m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalConfirmed), v_vRenewalGisSchemeID:=ToSafeInteger(v_lSchemeID), v_vSuspensionLevel:=0)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AK 22/11/01 - do not raise event in case of auto-confirm
            ' Create the event
            If Not v_bAutoConfirm Then
                m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeConfirmed, v_vInsuranceFileCnt:=v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            Else

            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".RenewalConfirmed")

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass &
                            ".RenewalConfirmed")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalConfirmed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalConfirmed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenewalConfirmedBrokerLed
    '
    ' Description:
    '
    ' History: TF281101 - Created from RenewalConfirmed.
    '
    ' ***************************************************************** '
    Public Function RenewalConfirmedBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, Optional ByVal v_bWhatIf As Boolean = False, Optional ByVal v_bAutoConfirm As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass &
                        ".RenewalConfirmedBrokerLed")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 101201 - Do not need to handle Whatif Cases differently now, as UnConfirm process
            '            can now reset the Renewal_Control data back to original, and reset Insurenac file

            If v_bAutoConfirm Then
                'AK - 101201 - GIS_Scheme_ID still needs to point at the original scheme on the policy
                '              so do not update it

                m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalConfirmed), v_vRenewalGisSchemeID:=ToSafeInteger(v_lSchemeID), v_vSuspensionLevel:=0)

            Else
                ' Update the control record

                m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalConfirmed), v_vRenewalGisSchemeID:=ToSafeInteger(v_lSchemeID), v_vSuspensionLevel:=0)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'AK 22/11/01 - do not raise event in case of auto-confirm
            ' Create the event
            If Not v_bAutoConfirm Then
                m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeConfirmed, v_vInsuranceFileCnt:=v_lInsuranceFileCnt)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass &
                            ".RenewalConfirmedBrokerLed")

            Return result

        Catch excep As System.Exception




            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass &
                            ".RenewalConfirmedBrokerLed")

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenewalConfirmedBrokerLed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenewalConfirmedBrokerLed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: NewInsuranceFile
    '
    ' Description:
    '
    ' History: 20/05/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Public Function NewInsuranceFile(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".NewInsuranceFile")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the control record

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".NewInsuranceFile")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="NewInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="NewInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: LapseConfirmed
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          29/05/2001 SSL - Implemented (same as created by CTAF for RenewalConfirmed)
    ' ***************************************************************** '
    Public Function LapseConfirmed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".LapseConfirmed")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Update the control record

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypePolicyLapseConfirmed), v_vRenewalGisSchemeID:=ToSafeInteger(v_lSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lSchemeID), v_vSuspensionLevel:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeLapseConfirmed, v_vInsuranceFileCnt:=v_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".LapseConfirmed")

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LapseConfirmed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LapseConfirmed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompLapsed
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '          02/11/2001 SJ - Add v_lOldInsuranceFileCnt parameter
    ' ***************************************************************** '
    Public Function RenCompLapsed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lOldInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RenCompLapsed"

        Dim lLapsedReasonId As Integer
        Dim sLapsedDescription As String = ""
        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Clear Parameters
            m_oDatabase.Parameters.Clear()

            'Add parameters
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lRenewalInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurance_file_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLapsedReasonSQL, sSQLName:=ACGetLapsedReasonName, bStoredProcedure:=ACGetLapsedReasonStored, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect", "sSQL:=ACGetLapsedReasonSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            'If we did not get any results then return a status of not found but do not error
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
                Return result
            End If

            'Retrieve the values
            Dim auxVar As Object = vResultArray(0, 0)


            If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                Dim dbNumericTemp As Double
                If Double.TryParse(CStr(vResultArray(0, 0)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                    lLapsedReasonId = ToSafeInteger(vResultArray(0, 0))
                    sLapsedDescription = gPMFunctions.ToSafeString(vResultArray(1, 0))
                End If
            End If

            If lLapsedReasonId = 0 Then
                'No lapsed reason so get the system default
                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="lapsed_reason", v_sCode:=ACAutoRenLapseReason, v_dtEffectiveDate:=DateTime.Now, r_lID:=lLapsedReasonId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oLookup.GetEffectiveIDFromCode", "v_sTableName:=lapsed_reason; v_scode:=RENMANAUTO", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            'Update the current live policy to show that it has been lapsed.
            m_lReturn = UpdateInsuranceFileAsLapsed(v_lInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lLapsedReasonId:=lLapsedReasonId, v_sLapsedDescription:=sLapsedDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateInsuranceFileAsLapsed", "v_lInsuranceFileCnt:=" & v_lOldInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Update the renewal version in case it has changed.
            m_lReturn = UpdateInsuranceFileAsLapsed(v_lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_lLapsedReasonId:=lLapsedReasonId, v_sLapsedDescription:=sLapsedDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("UpdateInsuranceFileAsLapsed", "v_lInsuranceFileCnt:=" & v_lRenewalInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Update the renewal control record

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypePolicyLapsed), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vRenewalEDIAuditID:=ToSafeInteger(v_lRenewalEdiAuditId))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oRenewalControl.DirectUpdate", "v_lInsuranceFolderCnt:=" & v_lInsuranceFolderCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            'Create an event to show what we have done.
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeCompletionLapse)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CreatePolicyEvent", "v_lInsuranceFolderCnt:=" & v_lInsuranceFolderCnt, gPMConstants.PMELogLevel.PMLogError)
            End If

            Return result
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUserName, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here

            '        Return result

            ' This is for debugging only
            '        Resume

            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenCompLapsedAlternateInsurer
    '
    ' Description:
    '
    ' History: 09/08/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompLapsedAlternateInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, ByVal v_lRenewalEdiAuditId As Integer, ByVal v_lOldInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim lLapsedReasonId As Integer
        Dim sLapsedDescription As String = ""
        Dim vResultArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'eck 090703 PN4760

            m_oDatabase.Parameters.Clear()

            ' Add the folder parameter
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lRenewalInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLapsedReasonSQL, sSQLName:=ACGetLapsedReasonName, bStoredProcedure:=ACGetLapsedReasonStored, vResultArray:=vResultArray)

            ' Check the SQL executed properly
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check we got some results
            If Not Informations.IsArray(vResultArray) Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Grab the values

            lLapsedReasonId = ToSafeInteger(vResultArray(0, 0))

            sLapsedDescription = CStr(vResultArray(1, 0))

            'eck 090703 PN4760

            'sj 12/08/2004 - Start
            'PN14041
            If lLapsedReasonId = 0 Then
                'No lapsed reason so get the system default
                m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="lapsed_reason", v_sCode:=ACAutoRenLapseReason, v_dtEffectiveDate:=DateTime.Now, r_lID:=lLapsedReasonId)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Unable to get default lapse reason", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapsed")
                End If
            End If
            'sj 12/08/2004 - End

            'sj 02/11/2001 - start
            m_lReturn = UpdateInsuranceFileAsLapsed(v_lInsuranceFileCnt:=v_lOldInsuranceFileCnt, v_lLapsedReasonId:=lLapsedReasonId, v_sLapsedDescription:=sLapsedDescription)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update insurance file as lapsed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapsed")
                Return result
            End If
            'sj 02/11/2001 - end

            ' Update the renewal control record

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeCompletedAlternateInsurer), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Update the renewal control record
            '    m_lReturn& = m_oRenewalControl.DirectUpdate( _
            ''        v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, _
            ''        v_vProductID:=v_lProductID, _
            ''        v_vRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, _
            ''        v_vRenewalStatusTypeCode:=PMRenewalStatusTypePolicyLapsed, _
            ''        v_vSuspensionLevel:=0, _
            ''        v_vRenewalGisSchemeID:=v_lRenewalGISSchemeID, _
            ''        v_vGISSchemeID:=v_lGISSchemeID, _
            ''        v_vRenewalDate:=v_dtRenewalDate, _
            ''        v_vGISDataModelID:=v_lGISDataModelID, _
            ''        v_vRenewalEDIAuditID:=v_lRenewalEDIAuditID)
            '    If (m_lReturn& <> PMTrue) Then
            '        RenCompLapsedAlternateInsurer = PMFalse
            '        ' Log Error Message
            '        LogMessage m_sUsername, _
            ''            iType:=PMLogError, _
            ''            sMsg:="Failed to update renewal control record.", _
            ''            vApp:=ACApp, _
            ''            vClass:=ACClass, _
            ''            vMethod:="RenQuotedBrokerLead", _
            ''            vErrNo:=Err.Number, _
            ''            vErrDesc:=Err.Description
            '        Exit Function
            '    End If

            ' Create the event
            ' AK 020701 - Changed the Event type from 'RENEWAL' to 'RENPOLCHG'
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeAlternateLapse)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapsedAlternateInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompLapsedAlternateInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapsedAlternateInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompletedHoldingInsurer
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompletedHoldingInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Integer, ByVal v_lGisDataModelId As Integer, Optional ByVal v_lOldInsuranceFileCnt As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            Dim sOldStatus As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            'Set status of policy live
            With m_oInsurance


                .InsuranceFileStructure = "GEM"

                .InsuranceFileCnt = v_lRenewalInsuranceFileCnt

                .InsuranceFileID = v_lRenewalInsuranceFileCnt

                .InsuranceFileStatus = ""

                .InsuranceFileType = "POLICY"

                .GeminiPolicyStatus = 40


                m_lReturn = .UpdatePolicy
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update status and type on Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletedHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            'AK 170502 - Update the Current Policy status according to the chosen scheme
            If v_lOldInsuranceFileCnt <> 0 Then
                If v_lGisSchemeId <> v_lRenewalGISSchemeID Then
                    sOldStatus = "REP"
                Else
                    sOldStatus = "LAP"
                End If
                With m_oInsurance


                    .InsuranceFileStructure = "GEM"

                    .InsuranceFileCnt = v_lOldInsuranceFileCnt
                    '.InsuranceFileID = v_lOldInsuranceFileCnt

                    .InsuranceFileStatus = sOldStatus
                    If sOldStatus = "REP" Then

                        .LapsedReason = "LREN"

                        .LapsedDate = DateTime.Today
                    End If


                    m_lReturn = .UpdatePolicy
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update status on old Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompleted", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                        Return result
                    End If

                End With

            End If


            ' Update the renewal control record

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypePolicyRenewed), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeInteger(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotedBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' Create the event
            ' AK 020701 - Changed the Event type from 'RENEWAL' to 'RENPOLCHG'
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeCompletionHoldingInsurer)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompLapsed", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletedHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletedHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompletedAlternateInsurer
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompletedAlternateInsurer(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Integer, ByVal v_lGisDataModelId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'TODO: 8.10 Lapse old policy and make renewal quotation live
            'TODO: bSirEvents.RenCompletedAlternateInsurer
            'TODO: 8.20 Post to accounts
            'TODO: bSirRenewalControl.Application.Rencompleted

            'Code below is copied from RenCompletedHoldingInsurer

            'Set status of policy live
            With m_oInsurance


                .InsuranceFileStructure = "GEM"

                .InsuranceFileCnt = v_lRenewalInsuranceFileCnt

                .InsuranceFileID = v_lRenewalInsuranceFileCnt

                .InsuranceFileStatus = ""

                .InsuranceFileType = "POLICY"

                .GeminiPolicyStatus = 40


                m_lReturn = .UpdatePolicy
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update status and type on Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletedHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletedAlternateInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletedAlternateInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PreRenSelectedPolicyEvent
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (PreRenSelectedPolicyEvent) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function PreRenSelectedPolicyEvent(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer) As Integer
    'Dim result As Integer = 0
    'Dim ACApp As String="bSiriusLink"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeDescPreRenewalSelect)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create policy event for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelectedPolicyEvent")
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
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
    'bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelectedPolicyEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelectedPolicyEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function

    ' ***************************************************************** '
    '
    ' Name: RenSelectedPolicyEvent
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    'UPGRADE_NOTE: (7001) The following declaration (RenSelectedPolicyEvent) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function RenSelectedPolicyEvent(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer) As Integer
    'Dim result As Integer = 0
    'Dim ACApp As String="bSiriusLink"
    '
    'Try 
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'm_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeDescRenewalSelect)
    '
    'If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '
    'bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create policy event for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelectedPolicyEvent")
    '
    'Return gPMConstants.PMEReturnCode.PMFalse
    '
    'End If
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
    'bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelectedPolicyEvent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelectedPolicyEvent", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)
    '
    'Return result
    '
    'End Try
    'End Function
    ' ***************************************************************** '
    '
    ' Name: RenMtaAtRenewalPolicyEvent
    '
    ' Description:
    '
    ' History: 28/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function RenMtaAtRenewalPolicyEvent(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeMtaAtRenewal)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create policy event for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalPolicyEvent")

            Return gPMConstants.PMEReturnCode.PMFalse

        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreatePolicyEvent
    '
    ' Description:
    '
    ' History: 20/03/2001 SJ - Created.
    '          16/05/2001 CTAF - Changed insfilecnt to variant
    '
    ' ***************************************************************** '
    Private Function CreatePolicyEvent(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sEventTypeCode As String, ByVal v_sDescription As String, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_vEventCnt As Object = Nothing) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim lEventTypeId As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' ***************************************************************** '
        ' Get the event_type table id
        ' ***************************************************************** '
        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="event_type", v_sCode:=v_sEventTypeCode, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=lEventTypeId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get event type id for " & v_sEventTypeCode, vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicyEvent")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add the event
        'SJ 21/04/2004 - start

        If Not Informations.IsNothing(r_vEventCnt) Then

            m_lReturn = m_oSirEvent.DirectAdd(vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_lInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vEventType:=lEventTypeId, vDescription:=v_sDescription, vEventCnt:=r_vEventCnt)
        Else
            'SJ 21/04/2004 - end

            m_lReturn = m_oSirEvent.DirectAdd(vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_lInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vEventType:=lEventTypeId, vDescription:=v_sDescription)
        End If
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create policy event for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicyEvent")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateSuspendEvent
    '
    ' Description:
    '
    ' History: 20/03/2001 SJ - Created.
    '          16/05/2001 CTAF - Changed insfilecnt to variant
    '          07/04/2004 CJB  - Changed to cater for new short description
    '
    ' ***************************************************************** '
    Private Function CreateSuspendEvent(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sEventTypeCode As String, ByVal v_sDescription As String, Optional ByVal v_vInsuranceFileCnt As Object = Nothing, Optional ByRef r_lEventCnt As Integer = 0, Optional ByVal v_sShortDescription As String = "", Optional ByVal v_sNotes As String = "") As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim lEventTypeId As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' ***************************************************************** '
        ' Get the event_type table id
        ' ***************************************************************** '
        m_lReturn = m_oLookup.GetEffectiveIDFromCode(v_sTableName:="event_type", v_sCode:=v_sEventTypeCode, v_dtEffectiveDate:=DateTime.Now.AddDays(1), r_lID:=lEventTypeId)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get event type id for " & v_sEventTypeCode, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSuspendEvent")

            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Add the event

        m_lReturn = m_oSirEvent.DirectAdd(vEventCnt:=r_lEventCnt, vPartyCnt:=v_lPartyCnt, vInsuranceFolderCnt:=v_lInsuranceFolderCnt, vInsuranceFileCnt:=v_vInsuranceFileCnt, vEventType:=lEventTypeId, vDescription:=v_sDescription, vShortDescription:=v_sShortDescription)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMError

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create suspend event for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSuspendEvent")
            Return result
        End If

        'SJ 30/04/2004 - Start
        If v_sNotes <> "" Then
            'Add some public notes
            m_lReturn = CreateFreeFormText(v_lEventCnt:=r_lEventCnt, v_sTransferNotes:=v_sNotes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFreeFormText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateSuspendEvent")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'SJ 30/04/2004 - End

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PreRenSelectedRenewalControl
    '
    ' Description:
    '
    ' History: 19/03/2001 sj - Created.
    '
    ' ***************************************************************** '
    'AK 021101 - added another parameter to mark live InsuranceFileCnt
    Private Function PreRenSelectedRenewalControl(ByVal v_lPartyCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lRiskCodeID As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lInsuranceFileCnt As Integer, Optional ByVal v_lGisDataModelId As Object = Nothing, Optional ByVal v_lSuspensionLevel As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oRenewalControl.DirectAdd(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypePreSelection), v_vSuspensionLevel:=ToSafeInteger(v_lSuspensionLevel), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vPartyCnt:=ToSafeInteger(v_lPartyCnt), v_vRiskCodeID:=ToSafeInteger(v_lRiskCodeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalGisSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vGISDataModelID:=CType(v_lGisDataModelId, Object))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create renewal control file for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelectedRenewalControl")

            Return result
        End If

        'AK 021101 - will need to update InsuranceFileCnt in the renewal control as well
        '            in order to create log file for batch jobs

        m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vOldInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to Update renewal control file for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelectedRenewalControl")

            Return result
        End If



        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RenSelectedRenewalControl
    '
    ' Description:
    '
    ' History: 19/03/2001 sj - Created.
    '          30/10/2001 Thinh Nguyen add optional param for suspension level
    ' ***************************************************************** '
    Private Function RenSelectedRenewalControl(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Date, ByVal v_lGisDataModelId As Integer, Optional ByVal v_lSuspensionLevel As Integer = 0) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim vResultArray(,) As Object = Nothing
        Dim lOldInsuranceFileCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Retrieve the current live policy for updating renewal control record
        m_oDatabase.Parameters.Clear()

        ' Add the folder parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectLivePolicySQL, sSQLName:=ACSelectLivePolicyName, bStoredProcedure:=ACSelectLivePolicyStored, vResultArray:=vResultArray)

        ' Check the SQL executed properly
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check we got some results
        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' Grab the values

        lOldInsuranceFileCnt = ToSafeInteger(vResultArray(ACInsuranceFileCnt, 0))

        '30/10/2001 Thinh Nguyen instead of hard coded zero use v_lSuspensionLevel
        ' CTAF 170401 - Update the renewal_status_type too

        m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypeRenewalSelected), v_vSuspensionLevel:=ToSafeInteger(v_lSuspensionLevel), v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeDate(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId), v_vOldInsuranceFileCnt:=ToSafeInteger(lOldInsuranceFileCnt))

        'Function DirectUpdate(v_lInsuranceFolderCnt As Long, [v_vProductID], [v_vRenewalInsuranceFileCnt], [v_vRenewalStatusTypeCode], [v_vSuspensionLevel], [v_vIsInsurerLead], [v_vRenewalGisSchemeID], [v_vGisSchemeId], [v_vRenewalDate]) As Long

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            result = gPMConstants.PMEReturnCode.PMFalse

            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update renewal control file for " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelectedRenewalControl")
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SetExistingRenQuotesToReplaced
    '
    ' Description:
    '
    ' History: 29/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function SetExistingRenQuotesToReplaced(ByRef v_lInsuranceFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim lRecordsAffected As Integer

        ' Clear the Parameters
        m_oDatabase.Parameters.Clear()

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACSetExistRenToRepSQL, sSQLName:=ACSetExistRenToRepName, bStoredProcedure:=ACSetExistRenToRepStored, lRecordsAffected:=lRecordsAffected)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCurrentPolicyVersion
    '
    ' Description:
    '
    ' History: 29/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetCurrentPolicyVersion(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer, ByRef r_lProductId As Integer, ByRef r_lGisSchemeId As Integer, ByRef r_lIsInsurerLead As Integer, ByRef r_lPartyCnt As Integer, ByRef r_dtRenewalDate As Date, ByRef r_lGISDataModelID As Integer, ByRef r_sGISDataModelCode As String, ByRef r_lGISPolicyLinkID As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Parameters
        m_oDatabase.Parameters.Clear()

        ' Add the folder parameter
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Execute the SQL
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectLivePolicySQL, sSQLName:=ACSelectLivePolicyName, bStoredProcedure:=ACSelectLivePolicyStored, vResultArray:=vResultArray)

        ' Check the SQL executed properly
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check we got some results
        If Not Informations.IsArray(vResultArray) Then
            Return gPMConstants.PMEReturnCode.PMNotFound
        End If

        ' Grab the values

        r_lInsuranceFileCnt = ToSafeInteger(vResultArray(ACInsuranceFileCnt, 0))

        r_lProductId = ToSafeInteger(vResultArray(ACProductId, 0))

        r_lGisSchemeId = ToSafeInteger(vResultArray(ACGisSchemeId, 0))

        r_lIsInsurerLead = ToSafeInteger((CStr(vResultArray(ACIsInsurerLead, 0))))

        r_lPartyCnt = ToSafeInteger(vResultArray(ACPartyCnt, 0))

        r_dtRenewalDate = CDate(vResultArray(ACRenewalDate, 0))
        ' CTAF 180401

        r_lGISDataModelID = ToSafeInteger(vResultArray(ACGISDataModelID, 0))
        ' CTAF 190401

        r_sGISDataModelCode = CStr(vResultArray(ACGISDataModel, 0))
        ' CTAF 180501

        r_lGISPolicyLinkID = ToSafeInteger(vResultArray(ACGisPolicyLinkId, 0))

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: CopyInsuranceFile
    '
    ' Description: Creates a new version of the Insurance File
    '
    ' ***************************************************************** '
    Public Function CopyInsuranceFile(ByVal v_lOldPolicyKey As Integer, ByRef r_lNewPolicyKey As Integer, Optional ByRef v_InsFileType As String = ACPMInsFileTypeRenewal, Optional ByRef v_bUpdateYears As Boolean = False, Optional ByVal v_cThisPremium As Decimal = 0, Optional ByVal v_cNetPremium As Decimal = 0, Optional ByVal v_cTaxAmount As Decimal = 0, Optional ByVal v_bRenewalsMode As Boolean = False) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim lPolicyVersion As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMFalse


            m_oInsurance.InsuranceFileStructure = "GEM"

            m_oInsurance.InsuranceFileCnt = v_lOldPolicyKey
            ' First get the one we're after copying
            'm_lReturn = m_oInsurance.GetDetails

            ' Set the insurance file id to original id

            m_oInsurance.InsuranceFileID = v_lOldPolicyKey

            ' Set the status to under renewal

            m_oInsurance.InsuranceFileStatus = "REN"

            ' set type to renewal

            m_oInsurance.InsuranceFileType = v_InsFileType

            'AK 250402 - get renewal version number

            m_lReturn = GetLastVersion(v_lInsFileCnt:=v_lOldPolicyKey, r_lVersion:=lPolicyVersion)
            ' Update the policy version
            ''''    lPolicyVersion = m_oInsurance.PolicyVersion
            ''''    m_oInsurance.PolicyVersion = lPolicyVersion + 1
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 'it will never happen but in case....
                lPolicyVersion = 2
            End If

            m_oInsurance.PolicyVersion = lPolicyVersion

            'Set all the premiums to zero

            m_oInsurance.ThisPremium = 0

            m_oInsurance.NetPremium = 0

            m_oInsurance.CommissionAmount = 0

            m_oInsurance.IPTableAmount = 0

            m_oInsurance.TaxAmount = 0

            'SJ 30/04/2004 -Start

            m_oInsurance.ThisPremium = v_cThisPremium

            m_oInsurance.NetPremium = v_cNetPremium

            m_oInsurance.IPTableAmount = v_cNetPremium

            m_oInsurance.TaxAmount = v_cTaxAmount
            'SJ 30/04/2004 -End


            m_oInsurance.LastTransDescription = ""

            Select Case v_InsFileType
                Case ACPMInsFileTypeRenewal


                    m_oInsurance.EventDescription = "Created renewal policy version for policy " & m_oInsurance.InsuranceRef
                Case ACPMInsFileTypeWhatIf


                    m_oInsurance.EventDescription = "Created what if renewal policy version for policy " & m_oInsurance.InsuranceRef
            End Select

            ' Then copy it

            m_lReturn = m_oInsurance.CreatePolicy()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Return the code for the freshly created insurance file record

            r_lNewPolicyKey = m_oInsurance.InsuranceFileCnt

            ' Run SP to update dates!
            If v_bUpdateYears Then
                m_lReturn = UpdateInsFileDates(v_lInsuranceFileCnt:=r_lNewPolicyKey)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            ' Prepare the renewal policy version for possible card processing at the
            ' debit stage in the renewals cycle. Note that had to add new optional
            ' v_bRenewalsMode parameter on CopyInsuranceFile as we only want to do this
            ' if running renewals (this is a Renewals class but there is code in bGIS
            ' to call this function for What If quotes too...)
            If v_bRenewalsMode Then
                m_lReturn = RenewalsCreditCardSetup(v_lOldInsFileCnt:=v_lOldPolicyKey, v_lRenewalInsFileCnt:=r_lNewPolicyKey)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If


            Return gPMConstants.PMEReturnCode.PMTrue

        Catch excep As System.Exception



            ' Error Section.
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CopyInsuranceFile Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CopyInsuranceFile", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UpdateInsFileDates
    '
    ' Description: Moves the dates for an insurance file forward one
    '              year
    '
    ' History: 20/08/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateInsFileDates(ByVal v_lInsuranceFileCnt As Integer) As gPMConstants.PMEReturnCode
        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lInsuranceFileCnt = 0 Then
            ' Do Nothing
            Return result
        End If

        ' Clear the parameters collection
        m_oDatabase.Parameters.Clear()

        ' Add the insurance folder cnt
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the SQL
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACUpdateInsFileDatesSQL, sSQLName:=ACUpdateInsFileDatesName, bStoredProcedure:=ACUpdateInsFileDatesStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCompleted
    '
    ' Description:
    '
    ' History: 22/08/01 AK - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompleted(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_lGisSchemeId As Integer, ByVal v_lProductID As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lRenewalGISSchemeID As Integer, ByVal v_dtRenewalDate As Integer, ByVal v_lGisDataModelId As Integer, ByVal v_lOldInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim bResult As Boolean
        Dim sOldStatus As String = ""

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 170502 - Update the Current Policy status according to the chosen scheme
            If v_lOldInsuranceFileCnt <> 0 Then

                ' CJB 280704 PN13668 Start : Set the policy status of the old policy version after a renewal to be
                ' Replaced in all circumstances (not Lapsed)

                '        If v_lGisSchemeId <> v_lRenewalGISSchemeID Then
                '            sOldStatus = "REP"
                '        Else
                '                bResult = True  'CJR 16/9/02 - Ensure renewal control record updates.
                '            sOldStatus = "LAP"
                '        End If
                sOldStatus = "REP"

                If v_lGisSchemeId = v_lRenewalGISSchemeID Then
                    bResult = True
                End If
                ' CJB 280704 PN13668 End


                With m_oInsurance


                    .InsuranceFileStructure = "GEM"

                    .InsuranceFileCnt = v_lOldInsuranceFileCnt
                    '.InsuranceFileID = v_lOldInsuranceFileCnt

                    .InsuranceFileStatus = sOldStatus
                    If sOldStatus = "REP" Then

                        .LapsedReason = "LREN"

                        .LapsedDate = DateTime.Today
                    End If


                    m_lReturn = .UpdatePolicy
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Log Error Message
                        bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update status on old Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompleted")
                        Return result
                    End If

                End With

            End If

            'Set status of NewPolicy as Live
            With m_oInsurance


                .InsuranceFileStructure = "GEM"

                .InsuranceFileCnt = v_lRenewalInsuranceFileCnt

                .InsuranceFileID = v_lRenewalInsuranceFileCnt



                .InsuranceFileStatus = Nothing


                m_lReturn = .UpdatePolicy
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Log Error Message
                    bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update status on Insurance_File table", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompleted")
                    Return result
                End If

            End With

            'AK 220801 - mark it differently for Holding/Alternative Insurer

            ' Update the renewal control record

            m_lReturn = m_oRenewalControl.DirectUpdate(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_vProductID:=ToSafeInteger(v_lProductID), v_vRenewalInsuranceFileCnt:=ToSafeInteger(v_lRenewalInsuranceFileCnt), v_vRenewalStatusTypeCode:=ToSafeString(PMRenewalStatusTypePolicyRenewed), v_vSuspensionLevel:=0, v_vRenewalGisSchemeID:=ToSafeInteger(v_lRenewalGISSchemeID), v_vGISSchemeID:=ToSafeInteger(v_lGisSchemeId), v_vRenewalDate:=ToSafeInteger(v_dtRenewalDate), v_vGISDataModelID:=ToSafeInteger(v_lGisDataModelId))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to update renewal control record.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompleted")
                Return result
            End If

            ' Create the event

            'AK 220801 - check if it has been renewed to holding/alternative insurer
            If bResult Then

                ' AK 020701 - Changed the Event type from 'RENEWAL' to 'RENPOLCHG'
                m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeCompletionHoldingInsurer)
            Else
                m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_vInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, v_sEventTypeCode:=PMRenewalPolicyChange, v_sDescription:=PMRenewalEventTypeCompletionAlternateInsurer)
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to create event.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompleted")
                Return result
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompleted Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompleted", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CompInsFiles
    '
    ' Description: Function to check if two insurance files belong to the same folder
    '              to decide whether the renewal has progressed for the holding insurer
    '              or an alternative one
    '
    '              Returns True for Holding and False for Alternative insurer
    '
    ' History: 22/08/01 AK - Created.
    '
    ' ***************************************************************** '

    Public Function CompInsFiles(ByVal v_lOldInsFileCnt As Integer, ByVal v_lNewInsFileCnt As Integer, ByRef r_bResult As Boolean) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Dim vResultArray(,) As Object = Nothing

        result = gPMConstants.PMEReturnCode.PMTrue
        Try

            m_oDatabase.Parameters.Clear()

            ' Add the Old Insurance Folder Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="OldInsurance_File_cnt", vValue:=CStr(v_lOldInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the New Insurance Folder Cnt parameter (INPUT)
            m_lReturn = m_oDatabase.Parameters.Add(sName:="NewInsurance_File_cnt", vValue:=CStr(v_lNewInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Execute SQL Statement
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACCompareInsFilesSQL, sSQLName:=ACCompareInsFilesName, bStoredProcedure:=ACCompareInsFilesStored, vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompInsFiles SQLSelect Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompInsFiles", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return gPMConstants.PMEReturnCode.PMFalse
            Else

                If Informations.IsArray(vResultArray) Then

                    r_bResult = CBool(vResultArray(0, 0))
                Else
                    r_bResult = 1 ' If something makes this fail, assume holding insurer
                End If

            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CompInsFiles Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CompInsFiles", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateRenewalEdiAuditStatus
    '
    ' Description:
    '
    ' History: 25/09/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateRenewalEdiAuditStatus(ByVal v_lRenewalEdiAuditId As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oRenEdiAudit As Object = Nothing


        'Create instance of policy events object

        m_lReturn = gPMComponentServices.CreateBusinessObject(r_oObject:=oRenEdiAudit, v_sClassName:="bRenEDIAudit.Business", v_sCallingAppName:=CStr(ACApp), v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return m_lReturn
        End If


        m_lReturn = oRenEdiAudit.UpdateRenewalEdiStatus(v_lRenewalEdiAuditId:=ToSafeInteger(v_lRenewalEdiAuditId), v_vRenewalEdiStatus:=ToSafeInteger(PMRenewalComplete))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            oRenEdiAudit = Nothing
            Return result
        End If


        oRenEdiAudit.Dispose()

        oRenEdiAudit = Nothing

        Return result

    End Function
    ' ***************************************************************** '
    '
    ' Name: UpdateInsuranceFileAsLapsed
    '
    ' Description:
    '
    ' History: 02/11/2001 sj - Created.
    ' ECK 090703 PN4760 Passed lapsed details
    ' ***************************************************************** '
    Private Function UpdateInsuranceFileAsLapsed(ByVal v_lInsuranceFileCnt As Integer, ByVal v_lLapsedReasonId As Integer, ByVal v_sLapsedDescription As String) As Integer

        Dim result As Integer = 0

        result = gPMConstants.PMEReturnCode.PMTrue

        'Setting this loads the policy version into the object

        m_oInsurance.InsuranceFileCnt = v_lInsuranceFileCnt

        'Set the status to lapsed and update the lapsed details.

        m_oInsurance.InsuranceFileStatus = "LAP"

        m_oInsurance.LapsedReasonid = v_lLapsedReasonId

        m_oInsurance.LapsedDescription = v_sLapsedDescription

        m_oInsurance.LapsedDate = DateTime.Now


        m_lReturn = m_oInsurance.UpdatePolicy
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError("m_oInsurance.UpdatePolicy", "InsuranceFileCnt = " & v_lInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Do any tidy up, e.g. Set x = Nothing here
        Return result
        ' This is for debugging only
    End Function


    'AK 250402 gets last version number for a given policy
    Public Function GetLastVersion(ByRef v_lInsFileCnt As Integer, ByRef r_lVersion As Integer) As Integer
        Dim result As Integer = 0
        Dim vData(,) As Object = Nothing
        Dim sSQL As String = ""

        result = gPMConstants.PMEReturnCode.PMTrue

        Try

            ' Clear the parameters collection
            m_oDatabase.Parameters.Clear()

            ' Add the insurance folder cnt
            m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_File_Cnt", vValue:=CStr(v_lInsFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACGetLatestVersionSQL, sSQLName:=ACGetLatestVersionName, bStoredProcedure:=ACGetLatestVersionStored, vResultArray:=vData)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
            Else
                If Informations.IsArray(vData) Then
                    result = gPMConstants.PMEReturnCode.PMTrue
                    'send back value

                    r_lVersion = ToSafeInteger(vData(0, 0))
                Else
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
            Return result

        Catch excep As System.Exception

            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetLastVersion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetLastVersion", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ********************************************************************* '
    ' Name          : GetRenewalStopCodeDetails
    ' Description   : Function to get the Renewal Method, Renewal Stop Code
    '                   for a supplied insurance_file_cnt
    ' Notes         : This function is created in releated to fix ISS8555
    ' Depends On    : spu_GET_Renewal_Stop_Code_Details_For_Policy
    ' Edit History  :
    ' RAM20040204   : Created
    ' ********************************************************************* '
    Private Function GetRenewalStopCodeDetails(ByVal v_lInsuranceFileCnt As Integer, ByRef r_lRenewalMethodID As Integer, ByRef r_sRenewalMethodDescription As String, ByRef r_lRenewalStopCodeID As Integer, ByRef r_sRenewalStopCodeDescription As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".GetRenewalStopCodeDetails")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' check if we have a valid insurance file cnt
        If v_lInsuranceFileCnt <= 0 Then

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid insurance_file_cnt : " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalStopCodeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' We have a valid Insurance File Cnt

        With m_oDatabase

            ' Clear the parameters collection
            .Parameters.Clear()

            ' Set the Input parameter

            ' Add the insurance_file_cnt
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Set the output parameters

            ' Add RenewalMethodID
            m_lReturn = .Parameters.Add(sName:="RenewalMethodID", vValue:=CStr(r_lRenewalMethodID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the RenewalMethodDescription
            m_lReturn = .Parameters.Add(sName:="RenewalMethodDescription", vValue:=r_sRenewalMethodDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the RenewalStopCodeID
            m_lReturn = .Parameters.Add(sName:="RenewalStopCodeID", vValue:=CStr(r_lRenewalStopCodeID), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the RenewalStopCodeDescription
            m_lReturn = .Parameters.Add(sName:="RenewalStopCodeDescription", vValue:=r_sRenewalStopCodeDescription, iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            m_lReturn = .SQLAction(sSQL:=ACGetRenewalStopCodeDetailsSQL, sSQLName:=ACGetRenewalStopCodeDetailsName, bStoredProcedure:=ACGetRenewalStopCodeDetailsStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                result = gPMConstants.PMEReturnCode.PMFalse

                ' Log Error Message
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Renewal Stop Code Details.", vApp:=ACApp, vClass:=ACClass, vMethod:="GetRenewalStopCodeDetails", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return result
            End If

            ' We got the resutls. Get all the details from the output parameters
            r_lRenewalMethodID = gPMFunctions.NullToLong(.Parameters.Item("RenewalMethodID").Value)
            r_sRenewalMethodDescription = gPMFunctions.NullToString(.Parameters.Item("RenewalMethodDescription").Value)
            r_lRenewalStopCodeID = gPMFunctions.NullToLong(.Parameters.Item("RenewalStopCodeID").Value)
            r_sRenewalStopCodeDescription = gPMFunctions.NullToString(.Parameters.Item("RenewalStopCodeDescription").Value)

        End With

        ' Debug message

        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".GetRenewalStopCodeDetails")

        Return result

    End Function

    ' ********************************************************************* '
    ' Name          : UpdateLastEdiMessageCountSent
    ' Description   : Function to update last edi message count sent
    ' Edit History  :
    ' SJ 16042004   : Created
    ' ********************************************************************* '
    Private Function UpdateLastEdiMessageCountSent(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".UpdateLastEdiMessageCountSent")



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            ' Add the insurance_file_cnt
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACUpdateLastEdiMessageCountSentSQL, sSQLName:=ACUpdateLastEdiMessageCountSentName, bStoredProcedure:=ACUpdateLastEdiMessageCountSentStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update last edi message count sent", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateLastEdiMessageCountSent")
                Return result
            End If

        End With


        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".UpdateLastEdiMessageCountSent")

        Return result

    End Function

    ' ********************************************************************* '
    ' Name          : UpdateEdiMessageSent
    ' Description   : Function to update edi_message_sent flag on insurance_file
    ' Edit History  :
    ' SJ 16042004   : Created
    ' ********************************************************************* '
    Public Function UpdateEdiMessageSent(ByVal v_lInsuranceFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With m_oDatabase

                .Parameters.Clear()

                ' Add the insurance_file_cnt
                m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                m_lReturn = .SQLAction(sSQL:=ACUpdateEdiMessageSentSQL, sSQLName:=ACUpdateEdiMessageSentName, bStoredProcedure:=ACUpdateEdiMessageSentStored)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update last edi message count sent", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateEdiMessageSent")
                    Return result
                End If

            End With

            Return result

        Catch excep As System.Exception



            ' Debug message

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & CStr(ACApp) & "." & ACClass & ".UpdateEdiMessageSent")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateEdiMessageSent Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenMtaAtRenewalQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenTransferPolicyToStandardRenewals
    '
    ' Description:
    '
    ' History: 22/04/2004 SJ - Created.
    ' ***************************************************************** '
    Public Function RenTransferPolicyToStandardRenewals(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sTransferReason As String, ByVal v_sTransferNotes As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vEventCnt As Object = Nothing

            ' Create the event
            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeTransferPolicyToStandardRenewals & " - " & v_sTransferReason, r_vEventCnt:=vEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Policy Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenTransferPolicyToStandardRenewals")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create a public note

            m_lReturn = CreateFreeFormText(v_lEventCnt:=ToSafeInteger((CStr(vEventCnt))), v_sTransferNotes:=v_sTransferNotes)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFreeFormText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenTransferPolicyToStandardRenewals")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Set the alternate reference to null
            m_lReturn = UpdateAlternateReference(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UpdateAlternateReference Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenTransferPolicyToStandardRenewals")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenTransferPolicyToStandardRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenTransferPolicyToStandardRenewals", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenLapseConfirmed
    '
    ' Description:
    '
    ' History: 22/04/2004 SJ - Created.
    ' ***************************************************************** '
    Public Function RenLapseConfirmed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sLapseReasonDesc As String, ByVal v_sLapseComment As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vEventCnt As Object = Nothing

            m_lReturn = CreatePolicyEvent(v_lPartyCnt:=v_lPartyCnt, v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sEventTypeCode:=PMRenewalEventType, v_sDescription:=PMRenewalEventTypeLapseConfirmed & " - " & v_sLapseReasonDesc, r_vEventCnt:=vEventCnt)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Create Policy Event Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenLapseConfirmed")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Create a public note

            m_lReturn = CreateFreeFormText(v_lEventCnt:=ToSafeInteger((CStr(vEventCnt))), v_sTransferNotes:=v_sLapseComment)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFreeFormText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenLapseConfirmed")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Update the renewal status type

            m_lReturn = m_oRenewalControl.UpdateRenewalStatusType(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_sRenewalStatusTypeCode:=ToSafeString(ACStatusLapseConfirmed), v_iResetFlag:=0)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreateFreeFormText Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenLapseConfirmed")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenLapseConfirmed Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenLapseConfirmed", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: CreateFreeFormText
    '
    ' Description:
    '
    ' History: 21/04/2004 SJ - Created.
    ' ***************************************************************** '
    Private Function CreateFreeFormText(ByVal v_lEventCnt As Integer, ByVal v_sTransferNotes As String) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim oFreeFormText As Object
        Dim sTextLine, sLineBreak As String
        Dim vTextArray() As Object

        'oFreeFormText = CreateLateBoundObject("bSIRFreeFormText.Business")

        oFreeFormText = Nothing
        result = gPMComponentServices.CreateBusinessObject(r_oObject:=oFreeFormText, v_sClassName:="bSIRFreeFormText.Business", v_sCallingAppName:=ACApp, v_sUsername:=m_sUserName, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
        If result <> gPMConstants.PMEReturnCode.PMTrue Then
            Dim r_sMessage As String = "Failed to create an instance of bSIRInsuranceFile.Business"
            bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="ReprocessClaim", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
            Return result
        End If
        m_lReturn = oFreeFormText.Initialise(sUserName:=ToSafeString(m_sUserName), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = m_lReturn
            oFreeFormText = Nothing
            Return result
        End If

        oFreeFormText.EntityName = "Event"
        oFreeFormText.TextType = "Public"


        vTextArray = v_sTransferNotes.Split(CChar(Strings.ChrW(13) & Strings.ChrW(10)))

        If Not Informations.IsArray(vTextArray) Then
            Return result
        End If

        sLineBreak = Strings.ChrW(13).ToString() & Strings.ChrW(10).ToString()
        sTextLine = "[" & m_sUserName.ToUpper() & " " & (DateTime.Now).ToString & "]" &
                    sLineBreak
        'Add the first line
        m_lReturn = oFreeFormText.EditAdd(lRow:=1, vInsuranceFileCnt:=ToSafeInteger(v_lEventCnt), vTextLine:=ToSafeString(sTextLine))


        For lRow As Integer = 2 To vTextArray.GetUpperBound(0) + 2

            m_lReturn = oFreeFormText.EditAdd(lRow:=ToSafeInteger(lRow), vInsuranceFileCnt:=ToSafeInteger(v_lEventCnt), vTextLine:=CStr(vTextArray(lRow - 2)) & sLineBreak)
        Next lRow

        m_lReturn = oFreeFormText.Update()

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: UpdateAlternateReference
    '
    ' Description:
    '
    ' History: 21/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function UpdateAlternateReference(ByVal v_lInsuranceFolderCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase

            .Parameters.Clear()

            ' Add the insurance_file_cnt
            m_lReturn = .Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACUpdateAlternateReferenceSQL, sSQLName:=ACUpdateAlternateReferenceName, bStoredProcedure:=ACUpdateAlternateReferenceStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                bPMFunc.LogMessage(sUsername:=m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update alternate reference", vApp:=ACApp, vClass:=ACClass, vMethod:="UpdateAlternateReference")
                Return result
            End If

        End With

        Return result

    End Function

    ' ********************************************************************* '
    ' Name          : GetPolicyReferAtRenewalIndicatorValue
    ' Description   : Function to get the Refer at Renewal indicator value
    '                   for a supplied insurance_file_cnt
    ' Notes         : This function is created in releated to fix PN14031
    ' Depends On    : spu_Get_Policy_Refer_At_Renewal_Indicator_Value
    ' Edit History  :
    ' CJB20040811   : Created
    ' ********************************************************************* '
    Private Function GetPolicyReferAtRenewalIndicatorValue(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bReferAtRenewal As Boolean) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"


        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & CStr(ACApp) & "." & ACClass & ".GetPolicyReferAtRenewalIndicatorValue")



        Dim iReferAtRenewal As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        ' check if we have a valid insurance file cnt
        If v_lInsuranceFileCnt <= 0 Then
            bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Invalid insurance_file_cnt : " & v_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyReferAtRenewalIndicatorValue", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        With m_oDatabase
            .Parameters.Clear()

            ' Add the insurance_file_cnt input parameter
            m_lReturn = .Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter insurance_file_cnt. lReturn:" & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyReferAtRenewalIndicatorValue", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Add the ReferAtRenewal o/p parameter
            m_lReturn = .Parameters.Add(sName:="ReferAtRenewal", vValue:=CStr(iReferAtRenewal), iDirection:=gPMConstants.PMEParameterDirection.PMParamOutput, iDataType:=gPMConstants.PMEDataType.PMInteger)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter ReferAtRenewal. lReturn:" & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyReferAtRenewalIndicatorValue", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            m_lReturn = .SQLAction(sSQL:=ACGetPolicyReferAtRenewalIndicatorValueSQL, sSQLName:=ACGetPolicyReferAtRenewalIndicatorValueName, bStoredProcedure:=ACGetPolicyReferAtRenewalIndicatorValueStored)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUserName, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to fetch Refer at Renewal Details for insurance file:" & v_lInsuranceFileCnt & ", lReturn:" & CStr(m_lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="GetPolicyReferAtRenewalIndicatorValue", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' We got the resutls. Get the detail value the output parameter
            r_bReferAtRenewal = gPMFunctions.NullToLong(.Parameters.Item("ReferAtRenewal").Value) = 1

        End With


        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & CStr(ACApp) & "." & ACClass & ".GetPolicyReferAtRenewalIndicatorValue")

        Return result

    End Function

    ' ********************************************************************* '
    ' Name          : RenewalsCreditCardSetup
    ' Description   : Function to prepare the renewal policy version for possible
    '                 card processing at the debit stage in the renewals cycle.
    ' Depends On    : spu_ACT_Do_RenewalsCreditCardSetup
    ' Edit History  :
    ' CJB20050120   : Created
    ' ********************************************************************* '
    Private Function RenewalsCreditCardSetup(ByVal v_lOldInsFileCnt As Integer, ByVal v_lRenewalInsFileCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim ACApp As String = "bSiriusLink"



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("old_insurance_file_cnt", CStr(v_lOldInsFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("renewal_insurance_file_cnt", CStr(v_lRenewalInsFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no 39. 
            m_lReturn = .SQLSelect("spu_ACT_Do_RenewalsCreditCardSetup", "Do RenewalsCreditCardSetup", True)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(m_lReturn.ToString() + ", RenewalsCreditCardSetup, " + "spu_ACT_Do_RenewalsCreditCardSetup failed. old_insurance_file_cnt:" & v_lOldInsFileCnt & ", v_lRenewalInsFileCnt:" & CStr(v_lRenewalInsFileCnt))
            End If
        End With

        Return result
    End Function
End Class
