Option Strict Off
Option Explicit On
Imports System.Globalization
Imports SSP.Shared
'Modified by Sudhanshu Behera on 5/18/2010 6:34:38 PM refer developer guide no. 129 (guide)
<System.Runtime.InteropServices.ProgId("Renewals_NET.Renewals")>
Public NotInheritable Class Renewals
    Implements IDisposable
    ' ***************************************************************** '
    ' Class Name:   Renewals 
    '
    ' Date: 14/09/1998
    '
    ' Description: Describes the Renewals.
    '
    ' Edit History:
    ' CJB 260105 - Changed RenAutoDebit to call new CreditCardProcessing routine
    '              after posting to accs has been done. This will check if we are
    '              processing a card payment for the renewal and if so check the
    '              renewal premium is not zero and is within any min and max amount
    '              limits that may have been setup and is not zero. We will then
    '              connect to the relevant connector for authorisation and payment
    '              collection and post the allocated cashlistitem. Any critical
    '              errors will cause the renawal batch run to be aborted. All errors
    '              will cause a Work Manager task to be generated and any cashlist
    '              and cashlistitem records to be removed from the policy version.
    '              As part of this development other new functions called from
    '              CreditCardProcessing are: RenewalsCreditCardCheck,
    '              SelectCardDataForAuthorisation, AddTaskToWorkManager and
    '              DeleteCashlistItem
    '
    ' CLG 100205 Process Sheet - AON Professional Risks, Document Link
    '            Changed RenAutoDebit, removed code to print linked documents and replaced with three functions
    '            PrintLinkedDocumentsStart, PrintLinkedDocuments and PrintLinkedDocumentsEnd
    '            These are now called by PreRenSelection, RenSelection RenAutoDebit allowing linked documents of type
    '            PRERENSEL, RENSEL and RN_C to be printed respectively.
    '
    ' CJB 040405 Changed RenCombSelection to call the routines already setup for PreRenSel
    '            and Sel as that way we can pick up more records in the 2nd stage (that were
    '            already at that stage) and it also makes the code simpler and more maintainable.
    '
    ' CJB 070405 Changed RenReminder to actually print a reminder (code was never developed in BOM) but to use
    '            CG's mechanism decribed 2 issues above. Had to return AgentCnt from the rec selection too.
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
    Private Const ACClass As String = "Renewals"

    ' PUBLIC Data Members (Begin)

    ' PUBLIC Data Members (End)

    ' PRIVATE Data Members (Begin)

    ' Database Class (Private)
    Private m_oDatabase As dPMDAO.Database

    ' Close Database Flag (Private)
    Private m_bCloseDatabase As Boolean

    Private m_vOptions As Object

    ' Error Code (Private)
    Private m_lReturn As Integer

    ' Process Mode Properties
    Private m_iTask As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    ' Instance of bGIS
    Private m_oGIS As Object 'bGIS.Renewals

    ' String for direct debit. This will be compared to a ucase
    Private Const AC_DirectDebit As String = "DIRECT DEBIT"

    ' Default business type code if none is set
    Private Const AC_DefaultBusinessTypeCode As String = "GIIM"

    ' GISBusinessTypeCode
    Private m_sGISBusinessTypeCode As String = ""

    'sj 20/11/2001 - start
    Private m_oPMLockUser As bpmlock.User
    Private m_oPMLockForm As bpmlock.Form
    'sj 20/11/2001 - end

    'AK 211101
    ' Message to write to log file.
    Private m_sRenTaskLog As String = ""
    ' Object to control Task Logging
    Private m_oTaskLog As Object

    Private m_lAgentCnt As Integer

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

    ' And some more to associate with an integer value
    Private Const AC_PreRenSelectionID As Integer = 1
    Private Const AC_SelectionID As Integer = 2
    Private Const AC_Quote_BrokerID As Integer = 3
    Private Const AC_ProcessEDIID As Integer = 4
    Private Const AC_Quote_InsurerID As Integer = 5
    Private Const AC_InviteID As Integer = 6
    Private Const AC_ReminderID As Integer = 7
    Private Const AC_CompleteID As Integer = 8

    'AK 13/12/2001
    Private Const ACRegServerRenewal As String = "Renewals"
    Private Const ACRegHousekeep As String = "Housekeeping Days"

    'SJ 14/04/2004 - start
    Private m_bIsMultiCompany As Boolean

    'DC030105 PN26049
    Private m_sUnderwritingOrAgency As String = ""

    ' IsInsurerMode
    Private m_bIsInsurerMode As Boolean
    'SJ 14/04/2004 - end

    '3 more optional properties
    Private m_boBatchRun As Boolean
    Private m_boAgentDoc As Boolean
    Private m_sDefaultProduct As String = ""

    ' CJB 270404 Folgate Development -  New Branch Specific Mode property
    ' If 0 then implies all branches - i.e. no specific branch, else has the source id of the specific branch
    ' Note that this will initially be set as a command line source value passed to individual renewals batch
    ' processing exe's which will then set this property...
    Private m_iBranchSpecificMode As Integer

    ' PM Product Family
    Public ReadOnly Property PMProductFamily() As Integer
        Get
            Return gPMConstants.PMEProductFamily.pmePFSiriusSolutions
        End Get
    End Property

    Public Property GISBusinessTypeCode() As String
        Get
            Return If(m_sGISBusinessTypeCode <> "", m_sGISBusinessTypeCode, AC_DefaultBusinessTypeCode)
        End Get
        Set(ByVal Value As String)
            m_sGISBusinessTypeCode = Value.Trim()
        End Set
    End Property

    ' 3 more optional properties
    Public Property BatchRun() As Boolean
        Get
            Return m_boBatchRun
        End Get
        Set(ByVal Value As Boolean)
            m_boBatchRun = Value
        End Set
    End Property
    Public Property AgentDoc() As Boolean
        Get
            Return m_boAgentDoc
        End Get
        Set(ByVal Value As Boolean)
            m_boAgentDoc = Value
        End Set
    End Property
    Public Property DefaultProduct() As String
        Get
            Return m_sDefaultProduct
        End Get
        Set(ByVal Value As String)
            m_sDefaultProduct = Value
        End Set
    End Property

    ' CJB 270404 Folgate Development -  New Branch Specific Mode property
    ' If 0 then implies all branches - i.e. no specific branch, else has the source id of the specific branch
    ' Note that this will initially be set as a command line source value passed to individual renewals batch
    ' processing exe's which will then set this property...
    Public Property BranchSpecificMode() As Integer
        Get
            Return m_iBranchSpecificMode
        End Get
        Set(ByVal Value As Integer)
            m_iBranchSpecificMode = Value
        End Set
    End Property

    'SJ 14/04/2004 - start
    'SJ 14/04/2004 - end
    'CJB 040504 - added for lookup purposes
    Public Property IsInsurerMode() As Boolean
        Get
            Return m_bIsInsurerMode
        End Get
        Set(ByVal Value As Boolean)
            m_bIsInsurerMode = Value
        End Set
    End Property


    ' ***************************************************************** '
    '
    ' Name: CreatePolicy
    '
    ' Description: Creates a new insurance file and
    '              risk based on a passed insurance folder
    '
    ' History: 18/07/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Public Function CreatePolicy(ByVal v_lInsuranceFolderCnt As Integer, Optional ByRef v_lInsuranceFileCnt As Object = 0) As Integer

        Dim result As Integer = 0
        Try

            Dim vResultArray(,) As Object = Nothing
            Dim sGISDataModelCode As String = ""
            Dim lPartyCnt, lGisBusinessTypeId, lGISSchemeId, lInsuranceFileCnt, lNewInsuranceFolderCnt, lNewInsuranceFileCnt, lPolicyLinkId, lQuoteBinderId As Object

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CreatePolicy")

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the current live file
            If v_lInsuranceFileCnt = 0 Then
                m_lReturn = GetCurrentPolicyVersion(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_lInsuranceFileCnt:=lInsuranceFileCnt)
            End If

            ' Get the one record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectCreatePolicySQL, v_sSQLName:=ACSelectCreatePolicyName, v_bStoredProcedure:=ACSelectCreatePolicyStored, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out of here
                Return result
            End If

            ' Get the variables

            lGISSchemeId = CInt(vResultArray(ACArrayPCGisSchemeId, 0))

            lPartyCnt = CInt(vResultArray(ACArrayPCPartyCnt, 0))

            lGisBusinessTypeId = CInt(vResultArray(ACArrayPCGisBusinessTypeId, 0))

            sGISDataModelCode = CStr(vResultArray(ACArrayPCGisDataModelCode, 0)).Trim()


            m_lReturn = m_oGIS.CreatePolicy(v_lOldInsuranceFileCnt:=v_lInsuranceFileCnt, v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_sPolicyRef:="", v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGisBusinessTypeId:=ToSafeInteger(lGisBusinessTypeId), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId),
                                            r_lInsuranceFolderCnt:=lNewInsuranceFolderCnt, r_lInsuranceFileCnt:=lNewInsuranceFileCnt, r_lPolicyLinkId:=lPolicyLinkId, r_lQuoteBinderId:=lQuoteBinderId)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' We dont exit, so we can process the next item
                ' Log Error Message
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CreatePolicy")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".CreatePolicy")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CreatePolicy Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="CreatePolicy", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: PreRenSelection
    '
    ' Description: Identify the records (insurance files) that are shortly
    '              to enter the renewals process.
    '
    ' History: 05/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function PreRenSelection(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim lInsuranceFolderCnt, lPartyCnt As Integer
        Dim dtRenewalDate As Date
        Dim sGISDataModelCode, lGISDataModelID As String
        Dim lAgentCnt, lRiskCodeID, lGISSchemeId, lProductID As Integer

        ' Local constants
        Const LCArrayPartyCnt As Integer = 0
        Const LCArrayInsuranceFolderCnt As Integer = 1
        Const LCArrayRenewalDate As Integer = 2
        Const LCArrayRiskCodeID As Integer = 3
        Const LCArrayGISSchemeID As Integer = 4
        Const LCArrayProductID As Integer = 5
        'AK 011101
        Const LCArrayInsuranceFileCnt As Integer = 6
        Const LCArrayDataModelCode As Integer = 7
        Const LCArrayDataModelID As Integer = 8
        Const LCArrayAgentCnt As Integer = 9
        Dim lInsuranceFileCnt As Integer
        'sj 21/11/2001 - start
        Dim bLocked As Boolean
        'sj 21/11/2001 - end

        'added for PrintLinkedDocuments
        Dim sMessage As String = ""
        'Modified by Sudhanshu Behera on 5/18/2010 6:58:00 PM refer developer guide no. 108 (guide)
        'Dim oDocMgrWrapper As bSIRDocManagerWrapper.Interface
        Dim oDocMgrWrapper As Object = Nothing

        Dim oDocLink As New bPMBDocLink.Business
        Dim oDocTemplate As New bSIRDocTemplate.Business
        Dim oTaskInstance As New bPMWrkTaskInstanceTemp.FormClass

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'SJ 19/04/2004 - start
            m_lReturn = SelectRecordsForPreRenSelection(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=vResultArray, v_sDefaultProduct:=DefaultProduct)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                Return result
            End If
            'SJ 19/04/2004 - end

            sMessage = "PreRenSelection Failed"
            PrintLinkedDocumentsStart(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance, sMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If


            '    If (v_lInsuranceFolderCnt = 0) Then
            '
            '        ' Select the records
            '        m_lReturn& = SelectRecords(v_sSQL:=ACPreRenSelSQL, _
            ''                                   v_sSQLName:=ACPreRenSelName, _
            ''                                   v_bStoredProcedure:=ACPreRenSelStored, _
            ''                                   r_vResultArray:=vResultArray, _
            ''                                   v_bSourceIdRequired:=True, _
            ''                                   v_bInsurerModeRequired:=True)
            '
            '    If (m_lReturn& <> PMTrue) Then
            '            ' Error if nothing was found
            '            If (m_lReturn& <> PMNotFound) Then
            '                PreRenSelection = PMFalse
            '            End If
            '            ' Exit out of here
            '            Exit Function
            '        End If
            '
            '    Else
            '
            '        ' CTAF : 170601 - We're never going to come in here are we?!?!?
            '
            '        ' Get the details from the renewal_control
            '        m_lReturn& = GetDetailsFromFolder( _
            ''                        v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, _
            ''                        r_vResultArray:=vFolderDetails)
            '        If (m_lReturn& <> PMTrue) Then
            '            PreRenSelection = PMFalse
            '            Exit Function
            '        End If
            '
            '        ' Store the values in an array for processing
            '        ReDim vResultArray(0 To 3, 0)
            '
            '        vResultArray(LCArrayInsuranceFolderCnt, 0) = v_lInsuranceFolderCnt&
            '        vResultArray(LCArrayPartyCnt, 0) = vFolderDetails(10, 0)
            '        vResultArray(LCArrayRenewalDate, 0) = vFolderDetails(7, 0)
            '        vResultArray(LCArrayRiskCodeID, 0) = vFolderDetails(11, 0)
            '
            '    End If

            ' Throw each record at the gis

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Get the variables

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                lInsuranceFileCnt = CInt(vResultArray(LCArrayInsuranceFileCnt, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayDataModelCode, iLoop1)).Trim()

                lGISDataModelID = CStr(vResultArray(LCArrayDataModelID, iLoop1))

                lAgentCnt = CInt(vResultArray(LCArrayAgentCnt, iLoop1))



                'sj 21/11/2001 - start
                ' Lock the record
                bLocked = False
                If v_lInsuranceFolderCnt = 0 Then
                    'Only when called from the scheduler
                    m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenSelection", r_bLocked:=bLocked)
                End If
                If Not bLocked Then
                    'sj 21/11/2001 - end
                    ' Call the GIS

                    m_lReturn = m_oGIS.PreRenSelection(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), v_sBusinessTypeCode:=ToSafeString(GISBusinessTypeCode), v_lInsuranceFileCnt:=ToSafeInteger(lInsuranceFileCnt), v_lGISDataModelID:=ToSafeString(lGISDataModelID))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="PreRenSelection Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelection")
                        'Exit Function
                    End If

                    'sj 21/11/2001 - start
                    'Unlock the record
                    If v_lInsuranceFolderCnt = 0 Then
                        m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                    End If
                    'sj 21/11/2001 - end

                    ' print linked documents
                    PrintLinkedDocuments("PRERENSEL", "PreRenSelection", lGISSchemeId, lAgentCnt, lInsuranceFolderCnt, lInsuranceFileCnt, lPartyCnt, oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance)

                End If


            Next iLoop1

            PrintLinkedDocumentsEnd(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance)

            Return result

        Catch excep As System.Exception



            PrintLinkedDocumentsEnd(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="PreRenSelection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenCombSelection
    '
    ' Description: Process PreRenSelection and RenSelection concurrently.
    '
    ' History: TF121101 - Created (based on PreRenSelection)
    '
    ' ***************************************************************** '
    Public Function RenCombSelection() As Integer

        ' Process Pre-Renewal Selection
        Dim result As Integer = 0
        m_lReturn = PreRenSelection()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        ' Instead of continuing with the group of records already obtained that we have done pre-renewal on,
        ' query again to get any additonal policies that were at that stage anyway. This also makes the code
        ' simpler and more maintainable  PN17461

        'Process Renewal Selection
        m_lReturn = RenSelection()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        Return result


        result = gPMConstants.PMEReturnCode.PMError
        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCombSelection Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCombSelection", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
        Return result
    End Function

    ' ***************************************************************** '
    '
    ' Name: WhatIfQuote
    '
    ' Description: Perform "what-if" quote
    '
    ' History: 20/07/2001 IJM - Created.
    '
    ' ***************************************************************** '
    Public Function WhatIfQuote(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Object, ByRef r_sGisDataModelCode As Object, ByRef r_lGisSchemeId As Object, ByRef r_lNewGISPolicyLinkID As Object, ByRef r_sGisBusinessType As String) As Integer

        Dim result As Integer = 0
        Try

            Const LCArrayInsuranceFileCnt As Integer = 0
            Const LCArrayGISDataModelCode As Integer = 1
            Const LCArrayGISSchemeID As Integer = 2
            Const LCArrayGISBusinessType As Integer = 3
            Const LCArrayThisPremium As Integer = 4
            Const LCArrayNetPremium As Integer = 5
            Const LCArrayTaxAmount As Integer = 6

            Dim vResultArray(,) As Object = Nothing


            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACWhatIfQuoteSQL, v_sSQLName:=ACWhatIfQuoteName, v_bStoredProcedure:=ACWhatIfQuoteStored, r_vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingleRecord failed. ReturnCode = " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuote")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Grab the values from the array

            r_lInsuranceFileCnt = CInt(vResultArray(LCArrayInsuranceFileCnt, 0))

            r_sGisDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, 0)).Trim()

            r_lGisSchemeId = CInt(vResultArray(LCArrayGISSchemeID, 0))

            r_sGisBusinessType = CStr(vResultArray(LCArrayGISBusinessType, 0)).Trim()

            'Create the what if policy, default to use same amounts so that finance plan copies correctly later

            m_lReturn = m_oGIS.WhatIfQuote(v_lInsuranceFileCnt:=r_lInsuranceFileCnt, v_sGisDataModelCode:=r_sGisDataModelCode, v_lGISSchemeId:=r_lGisSchemeId, r_lNewGISPolicyLinkID:=r_lNewGISPolicyLinkID, v_cThisPremium:=vResultArray(LCArrayThisPremium, 0), v_cNetPremium:=vResultArray(LCArrayNetPremium, 0), v_cTaxAmount:=vResultArray(LCArrayTaxAmount, 0))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="WhatIfquote Failed for Insurance Folder" & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuote")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Need to save here!!!
            m_lReturn = GetInsFileCnt(v_vPolicyLinkID:=r_lNewGISPolicyLinkID, r_vInsuranceFileCnt:=r_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get InsuranceFileCnt for GISPolicyLinkID " & r_lNewGISPolicyLinkID, vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuote")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Add the finance plan to this quote if the original policy had one
            m_lReturn = AddPremiumFinancePlan(v_lRenewalInsuranceFileCnt:=r_lInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddPremiumFinancePlan failed where v_lRenewalInsuranceFileCnt:= " & r_lInsuranceFileCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuote")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Error in WhatIfQuote", vApp:=ACApp, vClass:=ACClass, vMethod:="WhatIfQuote", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    ' Name: GetInsFileCnt
    '
    ' Description: Get the InsuranceFileCnt from PolicyLink.
    '
    ' TF101001 - Based on bGIS.GetPolicyLink()
    ' ***************************************************************** '
    Private Function GetInsFileCnt(ByVal v_vPolicyLinkID As Object, ByRef r_vInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the Parameters
        With m_oDatabase
            .Parameters.Clear()

            ' Add the required GisPolicyLink parameter

            lReturn = .Parameters.Add(sName:="gis_policy_link_id", vValue:=CStr(v_vPolicyLinkID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'gis_policy_link_id'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Add other parameteres required by stored procedure

            'developer guide no.85
            lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no.85
            lReturn = m_oDatabase.Parameters.Add(sName:="risk_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'risk_id'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no.85
            lReturn = m_oDatabase.Parameters.Add(sName:="quote_ref", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'quote_ref'", vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Call the SQL
            lReturn = .SQLSelect(sSQL:=ACGetInsFileCntSQL, sSQLName:=ACGetInsFileCntName, bStoredProcedure:=ACGetInsFileCntStored, bKeepNulls:=True, lNumberRecords:=gPMConstants.PMAllRecords)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=ACGetInsFileCntSQL & " failed. ReturnCode = " & CStr(lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="GetInsFileCnt", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If there are No Records return NOT Found
            If m_oDatabase.Records.Count() < 1 Then
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the Record
            r_vInsuranceFileCnt = gPMFunctions.NullToLong(.Records.Item(1).Fields()("insurance_file_cnt"))
        End With

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetDetailsFromFolder
    '
    ' Description: Gets the details from the renewal control for a folder
    '
    ' History: 02/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Friend Function GetDetailsFromFolder(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray As Object) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetDetailsFromFolder"

        Dim vResultArray(,) As Object = Nothing

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".GetDetailsFromFolder")

            'Clear the parameters
            m_oDatabase.Parameters.Clear()

            'Add the parameters

            m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=Informations.FormatDateTime(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=effective_date", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurance_folder_cnt", gPMConstants.PMELogLevel.PMLogError)
            End If


            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=source_id", gPMConstants.PMELogLevel.PMLogError)
            End If


            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_mode", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurer_mode", gPMConstants.PMELogLevel.PMLogError)
            End If


            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="default_product", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=default_product", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectSingleSelectionSQL, bStoredProcedure:=ACSelectSingleSelectionStored, sSQLName:=ACSelectSingleSelectionName, vResultArray:=vResultArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.SQLSelect.Add", "sSQL:=ACSelectSingleSelectionSQL", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Check we have sone results
            If Not Informations.IsArray(vResultArray) Then
                result = gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Return the entire array


            r_vResultArray = vResultArray


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".GetDetailsFromFolder")

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".GetDetailsFromFolder")

            '        Return result

            ' This is for debugging only

            'Exit Function
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenSelection
    '
    ' Description: Records (insurances files) selected to go through
    '              renewal selection process.
    '
    ' History: 10/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenSelection(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing
        Dim iLoop1, lInsuranceFolderCnt, lPartyCnt, lRiskCodeID As Integer
        Dim dtRenewalDate As Date
        Dim sGISDataModelCode As String = ""
        Dim lAgentCnt, lGISSchemeId, lInsuranceFileCnt As Integer

        Dim vFolderDetails As Object = Nothing
        'sj 21/11/2001 - start
        Dim bLocked As Boolean
        'sj 21/11/2001 - end
        ' SET 17/03/2004 ISS10883
        Dim sBusinessTypeCode As String = ""

        ' Local constants
        Const LCArrayInsuranceFolderCnt As Integer = 0
        Const LCArrayPartyCnt As Integer = 1
        Const LCArrayRenewalDate As Integer = 2
        Const LCArrayRiskCodeID As Integer = 3
        ' SET 17/03/2004 ISS10883
        Const LCArrayGISBusinessTypeCode As Integer = 4
        'added for PrintLinkedDocuments
        Const LCArrayAgentCnt As Integer = 5
        Const LCArrayGISSchemeID As Integer = 6
        Const LCArrayRenewalInsuranceFileCnt As Integer = 7

        'added for PrintLinkedDocuments
        Dim sMessage As String = ""
        'Modified by Sudhanshu Behera on 5/18/2010 6:59:09 PM refer developer guide no. 108 (guide)
        'Dim oDocMgrWrapper As bSIRDocManagerWrapper.Interface
        Dim oDocMgrWrapper As Object = Nothing
        Dim oDocLink As New bPMBDocLink.Business
        Dim oDocTemplate As New bSIRDocTemplate.Business
        Dim oTaskInstance As New bPMWrkTaskInstanceTemp.FormClass

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 211101 - Clear error log
            m_sRenTaskLog = ""

            ' If insurance_folder_cnt is passed in, then select the values for it
            If v_lInsuranceFolderCnt = 0 Then

                'sj 24/12/2003 - Add "v_bInsurerFolderCntRequired" parameter
                ' Select the records
                ' CJB 290404 Pass source id in to allow filtering if necessary
                ' CJB 050504 Pass insurer mode param in
                m_lReturn = SelectRecords(v_sSQL:=ACRenSelSQL, v_sSQLName:=ACRenSelName, v_bStoredProcedure:=ACRenSelStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True, v_sDefaultProduct:=DefaultProduct)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    ' Error if nothing was found
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                    End If
                    ' Exit out of here
                    Return result
                End If

            Else

                ' Select the values here
                m_lReturn = GetDetailsFromFolder(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_vResultArray:=vFolderDetails)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    'JSB 17052003 - If not found, don't log just exit with correct status set
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

                If Informations.IsArray(vFolderDetails) Then
                    ' Store the values in an array for processing
                    ReDim vResultArray(7, 0)


                    vResultArray(LCArrayInsuranceFolderCnt, 0) = v_lInsuranceFolderCnt


                    vResultArray(LCArrayPartyCnt, 0) = vFolderDetails(1, 0)


                    vResultArray(LCArrayRenewalDate, 0) = vFolderDetails(2, 0)


                    vResultArray(LCArrayRiskCodeID, 0) = vFolderDetails(3, 0)


                    vResultArray(LCArrayGISBusinessTypeCode, 0) = vFolderDetails(4, 0)
                    'DC050505 PN20694 added new fields


                    vResultArray(LCArrayAgentCnt, iLoop1) = vFolderDetails(5, 0)


                    vResultArray(LCArrayGISSchemeID, iLoop1) = vFolderDetails(6, 0)


                    vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1) = vFolderDetails(7, 0)

                End If

            End If

            sMessage = "RenSelection Failed"
            PrintLinkedDocumentsStart(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance, sMessage)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New Exception()
            End If

            ' Otherwise, lets do the selection thang

            For iLoop1 = 0 To vResultArray.GetUpperBound(1)

                ' Grab the values from the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lAgentCnt = CInt(vResultArray(LCArrayAgentCnt, iLoop1))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))


                ' SET 17/03/2004 ISS10883
                Dim auxVar As Object = vResultArray(LCArrayGISBusinessTypeCode, iLoop1)


                If Not (Convert.IsDBNull(auxVar) Or Informations.IsNothing(auxVar)) Then

                    sBusinessTypeCode = CStr(vResultArray(LCArrayGISBusinessTypeCode, iLoop1)).Trim()
                End If
                If sBusinessTypeCode.Length < 1 Then
                    ' get the default value for this
                    sBusinessTypeCode = GISBusinessTypeCode
                End If

                'sj 21/11/2001 - start
                ' Lock the record
                bLocked = False
                If v_lInsuranceFolderCnt = 0 Then
                    'Only when called from the scheduler
                    m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenSelection", r_bLocked:=bLocked)
                End If
                If Not bLocked Then
                    'sj 21/11/2001 - end
                    ' Blank for now
                    sGISDataModelCode = ""

                    ' Call the scary monster

                    m_lReturn = m_oGIS.RenSelection(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), r_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_sBusinessTypeCode:=ToSafeString(sBusinessTypeCode))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelection Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelection")
                        'Exit Function
                    End If

                    'IDP Jan 2003 Added Fortis specific Scheme Switch on Renewal
                    m_lReturn = FortisRenewalCheck(lInsuranceFolderCnt)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenSelection Failed on Fortis Check for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelection")
                    End If

                    'sj 21/11/2001 - start
                    'Unlock the record
                    If v_lInsuranceFolderCnt = 0 Then
                        m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                    End If
                    'sj 21/11/2001 - end

                    ' print linked documents
                    PrintLinkedDocuments("RENSEL", "PreRenSelection", lGISSchemeId, lAgentCnt, lInsuranceFolderCnt, lInsuranceFileCnt, lPartyCnt, oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance)

                Else
                    m_sRenTaskLog = "Failed to Select : Record Locked"
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lProcessID:=AC_SelectionID, v_sProcessCode:=AC_Selection, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)

                End If
            Next iLoop1

            PrintLinkedDocumentsEnd(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance)

            Return result

        Catch excep As System.Exception



            PrintLinkedDocumentsEnd(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance)

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="RenSelection", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotationInsurerLead
    '
    ' Description: Renewal quotation is type Insurer Lead.
    '
    ' History: 20/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenQuotationInsurerLead(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const LCArrayIsReplacement As Single = 0
        Const LCArrayRenewalEDIAuditID As Single = 1
        Const LCArrayInsuranceFolderCnt As Single = 2
        Const LCArrayGISSchemeID As Single = 3
        Const LCArrayRenewalGISSchemeID As Single = 4
        Const LCArrayRenewalInsuranceFileCnt As Single = 5
        Const LCArrayProductID As Single = 6
        Const LCArrayRenewalDate As Single = 7
        Const LCArrayPartyCnt As Single = 8
        Const LCArrayRiskCodeID As Single = 9
        Const LCArrayGISDataModelID As Single = 10
        Const LCArrayGISDataModelCode As Single = 11

        Dim lRenewalEdiAuditID, lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Object
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID As Integer
        Dim sGISDataModelCode As String = ""

        'sj 21/11/2001 - start
        Dim bLocked As Boolean
        'sj 21/11/2001 - end

        'AK 290501 Intialise method-status
        result = gPMConstants.PMEReturnCode.PMTrue

        'AK 211101 - Clear error log
        m_sRenTaskLog = ""

        If v_lInsuranceFolderCnt = 0 Then

            ' Get all the records
            'sj 24/12/2003 - Add "v_bInsurerFolderCntRequired" parameter
            ' CJB 290404 Pass source id in to allow filtering if necessary
            ' CJB 050504 Pass insurer_mode parameter
            m_lReturn = SelectRecords(v_sSQL:=ACQuoteInsurerSQL, v_sSQLName:=ACQuoteInsurerName, v_bStoredProcedure:=ACQuoteInsurerStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True)

        Else

            ' Get the one record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleQuoteInsurerSQL, v_sSQLName:=ACSelectSingleQuoteInsurerName, v_bStoredProcedure:=ACSelectSingleQuoteInsurerStored, r_vResultArray:=vResultArray)

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'RWH(10/12/2003) Let's ripple actual return code back to interface
            'to enable more informative message.
            result = m_lReturn

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Selection Failed, return code = " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Go through the array

        For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)


            Dim dbNumericTemp As Double
            If Double.TryParse(CStr(vResultArray(LCArrayIsReplacement, iLoop1)), NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then

                ' Grab the insurance folder count

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                'sj 21/11/2001 - start
                ' Lock the record
                bLocked = False
                If v_lInsuranceFolderCnt = 0 Then
                    'Only when called from the scheduler
                    m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenSelection", r_bLocked:=bLocked)
                End If
                If Not bLocked Then
                    'sj 21/11/2001 - end

                    If CInt(vResultArray(LCArrayIsReplacement, iLoop1)) = 1 Then

                        ' Yes, it's a replacement
                        ' Slap this fool back into Selection
                        m_lReturn = RenSelection(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)

                    End If

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then

                        ' Get the variables

                        lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

                        lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                        lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

                        lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

                        lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                        dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                        lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                        lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                        lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                        sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()

                        ' Now let it carry on with the rest of the "quotation"

                        m_lReturn = m_oGIS.RenQuotationInsurerLead(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID),
                                                                   r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lProductID:=ToSafeInteger(lProductID), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID),
                                                                   v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="REnQuotationInsurerLead Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationInsurerLead")
                        End If

                    Else
                        ' So we dont fail on the next one...
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue

                    End If
                    'sj 21/11/2001 - start
                    'Unlock the record
                    If v_lInsuranceFolderCnt = 0 Then
                        m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                    End If
                    'sj 21/11/2001 - end

                    'AK 211101 - Log this in Task Log
                Else
                    m_sRenTaskLog = "Failed to run Quotation (EDI)  : Record Locked"
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lProcessID:=AC_Quote_InsurerID, v_sProcessCode:=AC_Quote_Insurer, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)


                End If
            End If

        Next iLoop1

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationInsurerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationInsurerLead", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenQuotationBrokerLead
    '
    ' Description: Renewal quotation is type BrokerLead.
    '
    ' History: 20/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenQuotationBrokerLead(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Integer = 0
        Const LCArrayPartyCnt As Integer = 1
        Const LCArrayRenewalDate As Integer = 2
        Const LCArrayRiskCodeID As Integer = 3
        Const LCArrayGISDataModelID As Integer = 4
        Const LCArrayGISDataModelCode As Integer = 5
        Const LCArrayGISSchemeID As Integer = 6
        Const LCArrayProductID As Integer = 7
        Const LCArrayRenInsuranceFileCnt As Integer = 8
        Const LCArrayRenGISSchemeID As Integer = 9
        Const LCArrayGISBusinessTypeCode As Integer = 10

        Dim vResultArray(,) As Object = Nothing

        Dim lInsuranceFolderCnt, lPartyCnt As Integer
        Dim dtRenewalDate As Date
        Dim lRiskCodeID, lGISSchemeId, lGISDataModelID As Integer
        Dim sGISDataModelCode As String = ""
        Dim lProductID, lRenInsuranceFileCnt, lRenGISSchemeID As Object

        'sj 21/11/2001 - start
        Dim bLocked As Boolean
        'sj 21/11/2001 - end

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 211101 - Clear error log
            m_sRenTaskLog = ""

            If v_lInsuranceFolderCnt = 0 Then

                ' Select the records
                'sj 24/12/2003 - Add "v_bInsurerFolderCntRequired" parameter
                ' CJB 290404 Pass source id in to allow filtering if necessary
                ' CJB 050504 Pass insurer mode parameter
                m_lReturn = SelectRecords(v_sSQL:=ACQuoteBrokerSQL, v_sSQLName:=ACQuoteBrokerName, v_bStoredProcedure:=ACQuoteBrokerStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True, v_sDefaultProduct:=DefaultProduct)

            Else

                ' Select the single record
                m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleQuoteBrokerSQL, v_sSQLName:=ACSelectSingleQuoteBrokerName, v_bStoredProcedure:=ACSelectSingleQuoteBrokerStored, r_vResultArray:=vResultArray)
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Iterrate through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Get the next load of info from the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                lRenInsuranceFileCnt = CInt(vResultArray(LCArrayRenInsuranceFileCnt, iLoop1))

                lRenGISSchemeID = CInt(vResultArray(LCArrayRenGISSchemeID, iLoop1))


                m_sGISBusinessTypeCode = CStr(vResultArray(LCArrayGISBusinessTypeCode, iLoop1)).Trim()
                'sj 21/11/2001 - start
                ' Lock the record
                bLocked = False
                If v_lInsuranceFolderCnt = 0 Then
                    'Only when called from the scheduler
                    m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_sMethod:="RenSelection", r_bLocked:=bLocked)
                End If
                If Not bLocked Then
                    'sj 21/11/2001 - end
                    ' Call the gis

                    m_lReturn = m_oGIS.RenQuotationBrokerLead(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID),
                                                              v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID),
                                                              r_lRenewalInsuranceFileCnt:=lRenInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="REnQuotationBrokerLead Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead")
                        result = gPMConstants.PMEReturnCode.PMFalse
                        ' Don't exit out because we want to carry on processing
                        ' as many as we can.
                    End If

                    'sj 21/11/2001 - start
                    'Unlock the record
                    If v_lInsuranceFolderCnt = 0 Then
                        m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                    End If
                    'sj 21/11/2001 - end

                    'AK 211101 - Log this in Task Log
                Else
                    m_sRenTaskLog = "Failed to Run Broker-Led Quotation : Record Locked"
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lProcessID:=AC_Quote_BrokerID, v_sProcessCode:=AC_Quote_Broker, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)


                End If
            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenQuotationBrokerLead Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenQuotationBrokerLead", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenInvitePreferredQuotes
    '
    ' Description: Renewal quotation is type BrokerLead.
    '
    ' History: 11/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitePreferredQuotes(ByVal v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Integer = 0
        Const LCArrayPartyCnt As Integer = 1
        Const LCArrayRenewalDate As Integer = 2
        Const LCArrayRiskCodeID As Integer = 3
        Const LCArrayGISDataModelID As Integer = 4
        Const LCArrayGISDataModelCode As Integer = 5
        Const LCArrayGISSchemeID As Integer = 6
        Const LCArrayProductID As Integer = 7
        Const LCArrayRenInsuranceFileCnt As Integer = 8
        Const LCArrayRenGISSchemeID As Integer = 9

        Dim vResultArray(,) As Object = Nothing

        Dim lInsuranceFolderCnt, lPartyCnt As Integer
        Dim dtRenewalDate As Date
        Dim lRiskCodeID, lGISSchemeId, lGISDataModelID As Integer
        Dim sGISDataModelCode As String = ""
        Dim lProductID, lRenInsuranceFileCnt, lRenGISSchemeID As Object


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFolderCnt = 0 Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Select the single record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSinglePerferredQuotesSQL, v_sSQLName:=ACSelectSinglePerferredQuotesName, v_bStoredProcedure:=ACSelectSinglePerferredQuotesStored, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Iterrate through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Get the next load of info from the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                lRenInsuranceFileCnt = CInt(vResultArray(LCArrayRenInsuranceFileCnt, iLoop1))

                lRenGISSchemeID = CInt(vResultArray(LCArrayRenGISSchemeID, iLoop1))

                ' Call the gis

                m_lReturn = m_oGIS.RenInvitePreferredQuotes(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID),
                                                            v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID),
                                                            r_lRenewalInsuranceFileCnt:=lRenInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotes Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitePreferredQuotes")
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Don't exit out because we want to carry on processing
                    ' as many as we can.
                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitePreferredQuotes Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitePreferredQuotes", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenConfDocsHoldingInsurer
    '
    ' Description: Renewal quotation is type BrokerLead.
    '
    ' History: 11/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenConfDocsHoldingInsurer(ByVal v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Dim sRenewalStatusCode As String = ""
        Dim lRenewalEdiAuditID, lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Object
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID As Integer
        Dim sGISDataModelCode, sInsFileTypeCode As String
        Dim lGisBusinessTypeId, lOldInsuranceFileCnt As Integer



        result = gPMConstants.PMEReturnCode.PMTrue

        If v_lInsuranceFolderCnt = 0 Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Get the one record
        m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleRenCompSQL, v_sSQLName:=ACSelectSingleRenCompName, v_bStoredProcedure:=ACSelectSingleRenCompStored, r_vResultArray:=vResultArray)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Error if nothing was found
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Exit out of here
            Return result
        End If

        ' Go through the array

        For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

            ' Grab the insurance folder count

            lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

            ' Get the variables

            lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

            lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

            sRenewalStatusCode = CStr(vResultArray(LCArrayRenewalStatusCode, iLoop1))

            lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

            lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

            lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

            dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

            lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

            lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

            lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

            sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()

            sInsFileTypeCode = CStr(vResultArray(LCArrayInsFileTypeCode, iLoop1)).Trim()

            lGisBusinessTypeId = CInt(vResultArray(LCArrayGisBusinessTypeId, iLoop1))

            lOldInsuranceFileCnt = CInt(Val(CStr(vResultArray(LCArrayOldInsuranceFileCnt, iLoop1))))

            'Check the renewal status to see whether to lapse or renew
            If sRenewalStatusCode.Trim() = ACStatusLapseConfirmed Then
                Return result
            Else
                If sRenewalStatusCode.Trim() = ACStatusConfirm Then
                    ' Differentiate between renewal, alt insurer and what-if
                    Select Case sInsFileTypeCode
                        Case ACFileTypeRenewal


                            'm_lReturn = m_oGIS.RenConfDocsHoldingInsurer(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_dtRenewalDate:=dtRenewalDate, v_lRiskCodeID:=lRiskCodeID, v_lGISDataModelID:=lGISDataModelID, v_sGisDataModelCode:=sGISDataModelCode, v_lGISSchemeId:=lGISSchemeId, v_lProductID:=lProductID, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=lRenewalGISSchemeID, v_lRenewalEdiAuditId:=lRenewalEdiAuditID, v_sGisBusinessTypeCode:=GISBusinessTypeCode)
                            m_lReturn = m_oGIS.RenConfDocsHoldingInsurer(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID),
                                                                         v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID),
                                                                         r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                        Case ACFileTypePolicy, ACFileTypeWhatIf
                            Return result
                    End Select

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                    m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                End If

            End If

        Next iLoop1

        Return result



        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenConfDocsHoldingInsurer Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenConfDocsHoldingInsurer", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewal
    '
    ' Description:
    '
    ' History: 14/06/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function ConfirmRenewal(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmRenewal")



        result = gPMConstants.PMEReturnCode.PMTrue

        'AK - 22/11/01 - Siriuslink will have to change the record status to Confirm-Pending
        '                in Auto-confirm cases

        m_lReturn = m_oGIS.ConfirmRenewal(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_lSchemeID:=ToSafeInteger(v_lSchemeID), v_lPartyCnt:=ToSafeInteger(v_lPartyCnt),
                                          v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=ToSafeString(v_sGisBusinessTypeCode), v_bIsWhatIfQ:=False, v_bAutoConfirm:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".ConfirmRenewal")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: ConfirmRenewalBrokerLed
    '
    ' Description:
    '
    ' History: TF281101 - Created from ConfirmRenewalBrokerLed.
    '
    ' ***************************************************************** '
    Private Function ConfirmRenewalBrokerLed(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lInsuranceFileCnt As Integer, ByVal v_lSchemeID As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sGisDataModelCode As String, ByVal v_sGisBusinessTypeCode As String) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".ConfirmRenewalBrokerLed")



        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oGIS.ConfirmRenewalBrokerLed(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_lInsuranceFileCnt:=ToSafeInteger(v_lInsuranceFileCnt), v_lSchemeID:=ToSafeInteger(v_lSchemeID), v_lPartyCnt:=ToSafeInteger(v_lPartyCnt),
                                                   v_sGisDataModelCode:=ToSafeString(v_sGisDataModelCode), v_sGisBusinessTypeCode:=ToSafeString(v_sGisBusinessTypeCode), v_bAutoConfirm:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".ConfirmRenewalBrokerLed")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitationEDI
    '
    ' Description: Performs an invite process to produce output for EDI
    '
    ' History: 01/04/2004 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitationEDI(Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByVal v_lWhatIfInsuranceFileCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        Const LCArrayProductID As Single = 4
        Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        Const LCArrayRiskCodeID As Single = 7
        Const LCArrayGISDataModelID As Single = 8
        'Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10
        'Const LCArrayOfferAlt As Single = 11
        'Const LCArraySchemeTypeFlags As Integer = 12
        Const LCArrayGISBusinessTypeCode As Integer = 13
        Const LCArrayAgentCnt As Integer = 14

        Dim vResultArray(,) As Object = Nothing
        Dim lInsuranceFolderCnt, lRenewalInsuranceFileCnt As Object
        Dim bLocked As Boolean
        Dim sGISDataModelCode As String = ""
        Dim lGISSchemeId, lRenewalGISSchemeID, lProductID As Integer
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_sRenTaskLog = ""

            ' We after a specific folder?
            If v_lInsuranceFolderCnt = 0 Then

                ' Select the records
                ' CJB 290404 Pass source id in to allow filtering if necessary
                ' CJB 040504 Pass InsurerMode parameter
                m_lReturn = SelectRecords(v_sSQL:=ACInvitationSQL, v_sSQLName:=ACInvitationName, v_bStoredProcedure:=ACInvitationStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True)

            Else

                ' Pick up the individual record
                m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleInvitationSQL, v_sSQLName:=ACSelectSingleInvitationName, v_bStoredProcedure:=ACSelectSingleInvitationStored, r_vResultArray:=vResultArray)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out of
                Return result
            End If

            ' Go through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Grab the values out of the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))
                If v_lWhatIfInsuranceFileCnt = 0 Then

                    lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))
                Else
                    lRenewalInsuranceFileCnt = v_lWhatIfInsuranceFileCnt
                End If

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()

                m_sGISBusinessTypeCode = CStr(vResultArray(LCArrayGISBusinessTypeCode, iLoop1)).Trim()

                m_lAgentCnt = CInt(Val(CStr(vResultArray(LCArrayAgentCnt, iLoop1))))

                ' Lock the record
                bLocked = False
                If v_lInsuranceFolderCnt = 0 Then
                    'Only when called from the scheduler
                    m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenSelection", r_bLocked:=bLocked)
                End If
                If Not bLocked Then

                    ' Invite the record

                    'm_lReturn = m_oGIS.RenInvitationEDI(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_dtRenewalDate:=dtRenewalDate, v_lRiskCodeID:=lRiskCodeID, v_lGISDataModelID:=lGISDataModelID, v_sGisDataModelCode:=sGISDataModelCode, v_lGISSchemeId:=lGISSchemeId, v_lProductID:=lProductID, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=lRenewalGISSchemeID, v_sGisBusinessTypeCode:=GISBusinessTypeCode)
                    m_lReturn = m_oGIS.RenInvitationEDI(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID),
                                                        v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="RenInvitationEDI Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitation")
                        ' Don't exit because we want to renew as many as possible
                    End If

                    ' Unlock the record
                    If v_lInsuranceFolderCnt = 0 Then
                        m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                    End If

                Else

                    ' Log a message saying we were unable to lock the record
                    m_sRenTaskLog = "Failed to Invite : Record Locked"
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)

                End If

            Next iLoop1

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitationEDI Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitationEDI", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result


            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenInvitation
    '
    ' Description: Insurance files selected for Renewal Invitation.
    '
    ' History: 03/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenInvitation(Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_bDocumentNotLinked As Boolean = False) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        Const LCArrayProductID As Single = 4
        Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        Const LCArrayRiskCodeID As Single = 7
        Const LCArrayGISDataModelID As Single = 8
        Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10
        'AK 121001
        Const LCArrayOfferAlt As Single = 11
        'sj 21/11/2001 - start
        Const LCArraySchemeTypeFlags As Integer = 12
        'sj 21/11/2001 - end
        Const LCArrayGISBusinessTypeCode As Integer = 13
        Const LCArrayAgentCnt As Integer = 14

        Dim vResultArray(,) As Object = Nothing

        Dim lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Object
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID, lRenewalEdiAuditID As Integer
        Dim sGISDataModelCode As String = ""
        'AK 121001
        Dim iOfferAlt As Integer
        'sj 21/11/2001 - start
        Dim lSchemeTypeFlags As Integer
        'sj 21/11/2001 - end

        ' CTAF 12062001
        Dim bAutoPayment As Boolean
        'sj 21/11/2001 - start
        Dim bLocked As Boolean
        'sj 21/11/2001 - end

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RenInvitation")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'AK 211101 - Clear error log
            m_sRenTaskLog = ""

            ' We after a specific folder?
            If v_lInsuranceFolderCnt = 0 Then

                ' Select the records
                'sj 24/12/2003 - Add "v_bInsurerFolderCntRequired" parameter
                ' CJB 290404 Pass source id in to allow filtering if necessary
                ' CJB 040504 Pass insurer mode parameter in
                m_lReturn = SelectRecords(v_sSQL:=ACInvitationSQL, v_sSQLName:=ACInvitationName, v_bStoredProcedure:=ACInvitationStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True, v_sDefaultProduct:=DefaultProduct)

            Else

                ' Pick up the individual record
                m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleInvitationSQL, v_sSQLName:=ACSelectSingleInvitationName, v_bStoredProcedure:=ACSelectSingleInvitationStored, r_vResultArray:=vResultArray)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out of
                Return result
            End If

            ' Go through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Grab the values out of the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

                lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()
                'AK 121001 - ofer alternative

                iOfferAlt = CInt(CStr(vResultArray(LCArrayOfferAlt, iLoop1)).Trim())
                'sj 21/11/2001 - start

                lSchemeTypeFlags = CInt(Val(CStr(vResultArray(LCArraySchemeTypeFlags, iLoop1))))
                'sj 21/11/2001 - end
                'TF300702

                m_sGISBusinessTypeCode = CStr(vResultArray(LCArrayGISBusinessTypeCode, iLoop1)).Trim()
                'sj 21/11/2001 - start

                m_lAgentCnt = CInt(Val(CStr(vResultArray(LCArrayAgentCnt, iLoop1))))

                ' Lock the record
                bLocked = False
                If v_lInsuranceFolderCnt = 0 Then
                    'Only when called from the scheduler
                    m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenSelection", r_bLocked:=bLocked)
                End If
                If Not bLocked Then
                    'sj 21/11/2001 - end
                    ' Call the GIS
                    If lRenewalEdiAuditID = 0 Then

                        ' Broker led

                        'AK 121001 - check if the current record is flagged for inviting preferred
                        '            quotations (alternative quotes), 1 - marked for alternatives

                        If iOfferAlt = 0 Then

                            'm_lReturn = m_oGIS.RenInvitationBrokerLed(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_dtRenewalDate:=dtRenewalDate, v_lRiskCodeID:=lRiskCodeID, v_lGISDataModelID:=lGISDataModelID, v_sGisDataModelCode:=sGISDataModelCode, v_lGISSchemeId:=lGISSchemeId, v_lProductID:=lProductID, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=lRenewalGISSchemeID, v_lRenewalEdiAuditId:=lRenewalEdiAuditID, v_sGisBusinessTypeCode:=GISBusinessTypeCode, v_lBatchRun:=BatchRun, v_sAgentDoc:=AgentDoc, v_lAgentCnt:=m_lAgentCnt, r_bDocumentNotLinked:=r_bDocumentNotLinked)
                            m_lReturn = m_oGIS.RenInvitationBrokerLed(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID),
                                                                      v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode), v_lBatchRun:=ToSafeBoolean(BatchRun), v_sAgentDoc:=ToSafeBoolean(AgentDoc), v_lAgentCnt:=ToSafeInteger(m_lAgentCnt), r_bDocumentNotLinked:=ToSafeBoolean(r_bDocumentNotLinked))
                        Else
                            ' Call the gis for printing preferred quotes

                            m_lReturn = m_oGIS.RenInvitePreferredQuotes(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID),
                                                                        v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitation Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitation")
                            ' Dont exit because we want to process as many as we can
                        End If
                    Else
                        ' Insurer led
                        'sj 30/10/2001 - Add iOfferAlt parameter

                        m_lReturn = m_oGIS.RenInvitationInsurerLed(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID),
                                                                   v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode), v_iOfferAlt:=ToSafeInteger(iOfferAlt))
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitation Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitation")
                            ' Dont exit because we want to process as many as we can
                        Else
                            ' sj 13/09/01 - Only do this if Invitation was sucessful
                            ' This looks like it will slow down the batch job. Seems kinda kludgy to me.
                            ' If some clever soul has time then they could merge it into the
                            ' previous sql statement ;-)

                            ' If the payment method's Direct Debit then automatically renew
                            'JSB 10/03/2003 - change following call to use lRenewalInsuranceFileCnt
                            '                 instead of lRenInsuranceFileCnt as this doesn't have a
                            '                 value set against it
                            m_lReturn = CheckAutoPayment(v_lInsuranceFileCnt:=lRenewalInsuranceFileCnt, r_bAutoPayment:=bAutoPayment, v_lSchemeTypeFlags:=lSchemeTypeFlags)

                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                result = gPMConstants.PMEReturnCode.PMError
                                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckAutoPayment failed for insurance file cnt " & lRenewalInsuranceFileCnt & " return code = " & CStr(m_lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitation")
                                Return result
                            End If

                            If bAutoPayment Then
                                'JSB 10/03/2003 - change following call to use lRenewalInsuranceFileCnt
                                '                 instead of lRenInsuranceFileCnt as this doesn't have a
                                '                 value set against it
                                ' Confirm it
                                m_lReturn = ConfirmRenewal(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lSchemeID:=lGISSchemeId, v_lPartyCnt:=lPartyCnt, v_sGisDataModelCode:=sGISDataModelCode, v_sGisBusinessTypeCode:=m_sGISBusinessTypeCode)
                                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                    result = gPMConstants.PMEReturnCode.PMError
                                    LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ConfirmRenewal failed for insurance file cnt " & lRenewalInsuranceFileCnt & " return code = " & CStr(m_lReturn), vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitation")
                                    Return result
                                End If
                            End If
                        End If

                    End If

                    'sj 21/11/2001 - start
                    'Unlock the record
                    If v_lInsuranceFolderCnt = 0 Then
                        m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                    End If
                    'sj 21/11/2001 - end
                    'AK 211101 - Log this in Task Log
                Else
                    m_sRenTaskLog = "Failed to Invite : Record Locked"
                    m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lProcessID:=AC_InviteID, v_sProcessCode:=AC_Invite, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)

                End If

            Next iLoop1

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RenInvitation")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenInvitation")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenInvitation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenInvitation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: RenReprintInvitation
    '
    ' Description: Insurance files selected for Renewal Invitation.
    '
    ' History: 13/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenReprintInvitation(Optional ByVal v_lInsuranceFolderCnt As Integer = 0, Optional ByRef r_bDocumentNotLinked As Boolean = False) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        Const LCArrayProductID As Single = 4
        Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        Const LCArrayRiskCodeID As Single = 7
        Const LCArrayGISDataModelID As Single = 8
        Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10
        'AK 121001
        Const LCArrayOfferAlt As Single = 11

        Dim vResultArray(,) As Object = Nothing

        Dim lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Object
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID, lRenewalEdiAuditID As Integer
        Dim sGISDataModelCode As String = ""
        'AK 121001
        Dim iOfferAlt As Integer

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RenReprintInvitation")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' We after a specific folder?
            'If (v_lInsuranceFolderCnt& = 0) Then

            ' Pick up the individual record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleInvitationSQL, v_sSQLName:=ACSelectSingleInvitationName, v_bStoredProcedure:=ACSelectSingleInvitationStored, r_vResultArray:=vResultArray)

            'End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out of
                Return result
            End If

            ' Go through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Grab the values out of the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

                lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()
                'AK 121001 - ofer alternative

                iOfferAlt = CInt(CStr(vResultArray(LCArrayOfferAlt, iLoop1)).Trim())

                ' Call the GIS
                If lRenewalEdiAuditID = 0 Then

                    ' Broker led

                    'AK 121001 - check if the current record is flagged for inviting preferred
                    '            quotations (alternative quotes), 1 - marked for alternatives

                    If iOfferAlt = 0 Then

                        m_lReturn = m_oGIS.RenInvitationBrokerLed(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode),
                                                                  v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode), r_bDocumentNotLinked:=ToSafeBoolean(r_bDocumentNotLinked))
                    Else
                        ' Call the gis for printing preferred quotes

                        m_lReturn = m_oGIS.RenInvitePreferredQuotes(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode),
                                                                    v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode))
                    End If

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintInvitation Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintInvitation")
                        ' Dont exit because we want to process as many as we can
                    End If
                Else
                    ' Insurer led

                    'm_lReturn = m_oGIS.RenReprintInvitationInsurerLed(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lPartyCnt:=lPartyCnt, v_dtRenewalDate:=dtRenewalDate, v_lRiskCodeID:=lRiskCodeID, v_lGISDataModelID:=lGISDataModelID, v_sGisDataModelCode:=sGISDataModelCode, v_lGISSchemeId:=lGISSchemeId, v_lProductID:=lProductID, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=lRenewalGISSchemeID, v_lRenewalEdiAuditId:=lRenewalEdiAuditID, v_sGisBusinessTypeCode:=GISBusinessTypeCode, v_iOfferAlt:=iOfferAlt)
                    m_lReturn = m_oGIS.RenReprintInvitationInsurerLed(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode),
                                                                      v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode), v_iOfferAlt:=ToSafeInteger(iOfferAlt))
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                End If
            Next iLoop1

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RenReprintInvitation")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenReprintInvitation")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintInvitation Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintInvitation", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenReSendInvitationEdiMessage
    '
    ' Description: Insurance files selected for Renewal Invitation.
    '
    ' History: 13/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenReSendInvitationEdiMessage(ByRef v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        Const LCArrayProductID As Single = 4
        Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        Const LCArrayRiskCodeID As Single = 7
        Const LCArrayGISDataModelID As Single = 8
        Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10

        'Const LCArrayOfferAlt As Single = 11

        Dim vResultArray(,) As Object = Nothing

        Dim lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Object
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID, lRenewalEdiAuditID As Integer
        Dim sGISDataModelCode As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RenReSendInvitationEdiMessage")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pick up the individual record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleInvitationSQL, v_sSQLName:=ACSelectSingleInvitationName, v_bStoredProcedure:=ACSelectSingleInvitationStored, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                Return result
            End If

            ' Go through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Grab the values out of the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

                lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

                lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

                dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

                lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

                lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()


                m_lReturn = m_oGIS.RenInvitationEDI(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode),
                                                    v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_sGisBusinessTypeCode:=ToSafeString(GISBusinessTypeCode), v_bResendMessage:=True)


            Next iLoop1

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RenReSendInvitationEdiMessage")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenReSendInvitationEdiMessage")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReSendInvitationEdiMessage Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReSendInvitationEdiMessage", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenCompletion
    '
    ' Description: Insurance files that are completed with Renewal process.
    '              to be finalized by the customer.
    '
    ' History: 04/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function RenCompletion(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Dim sRenewalStatusCode As String = ""
        Dim lRenewalEdiAuditID, lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Object
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID As Integer
        Dim sGISDataModelCode, sInsFileTypeCode As String
        Dim lGisBusinessTypeId, lOldInsuranceFileCnt As Integer

        'sj 21/11/2001 - start
        Dim bLocked As Boolean
        'sj 21/11/2001 - end
        Dim bCompHoldingInsurer As Boolean

        'AK 290501
        result = gPMConstants.PMEReturnCode.PMTrue

        'AK 211101 - Clear error log
        m_sRenTaskLog = ""

        If v_lInsuranceFolderCnt = 0 Then

            ' Get all the records
            'sj 24/12/2003 - Add "v_bInsurerFolderCntRequired" parameter
            ' CJB 290404 Pass source id in to allow filtering if necessary
            ' CJB 040504 Pass insurer mode parameter
            m_lReturn = SelectRecords(v_sSQL:=ACSelectRenCompSQL, v_sSQLName:=ACSelectRenCompName, v_bStoredProcedure:=ACSelectRenCompStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True)

        Else

            ' Get the one record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleRenCompSQL, v_sSQLName:=ACSelectSingleRenCompName, v_bStoredProcedure:=ACSelectSingleRenCompStored, r_vResultArray:=vResultArray)

        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            ' Error if nothing was found
            If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                result = gPMConstants.PMEReturnCode.PMFalse
            End If
            ' Exit out of here
            Return result
        End If

        ' Go through the array

        For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)

            ' Grab the insurance folder count

            lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

            ' Get the variables

            lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

            lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

            sRenewalStatusCode = CStr(vResultArray(LCArrayRenewalStatusCode, iLoop1))

            'Added by MKW210503 PN4276 Part of 1.8.5 to 1.8.6 catchup.
            ' ISS2364.  CMG(SJP) Some lapsed records come back with a null renewal gis
            ' scheme id which breaks at this point.  As it seems to be the case that
            ' all other records have the same renewal gis scheme id as the parent record
            ' then set it here to get such lapsed records through the batch process.

            If (CStr(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))).Length > 0 Then

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))
            Else

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))
            End If


            lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

            lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

            dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

            lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

            lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

            lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

            sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()

            sInsFileTypeCode = CStr(vResultArray(LCArrayInsFileTypeCode, iLoop1)).Trim()

            lGisBusinessTypeId = CInt(vResultArray(LCArrayGisBusinessTypeId, iLoop1))

            lOldInsuranceFileCnt = CInt(Val(CStr(vResultArray(LCArrayOldInsuranceFileCnt, iLoop1))))

            m_sGISBusinessTypeCode = CStr(vResultArray(LCArrayBusinessTypeCode, iLoop1)).Trim()
            'sj 21/11/2001 - start
            ' Lock the record
            bLocked = False
            If v_lInsuranceFolderCnt = 0 Then
                'Only when called from the scheduler
                m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenSelection", r_bLocked:=bLocked)
            End If
            If Not bLocked Then
                'sj 21/11/2001 - end

                'Check the renewal status to see whether to lapse or renew
                If sRenewalStatusCode.Trim() = ACStatusLapseConfirmed Then

                    ' Now initiate the lapse process for this Insurance folder
                    'AK 130901 - added another parameter to this function, to differentiate between lapseconf
                    '            Alt Quote - Lapse cases
                    'sj 051101 - Add OldInsuranceFileCnt parameter

                    ' SET 04/08/2004 ISS13073 - use the default business type only if it is not already set
                    If m_sGISBusinessTypeCode.Length < 1 Then
                        m_sGISBusinessTypeCode = GISBusinessTypeCode.Trim()
                    End If

                    m_lReturn = m_oGIS.RenCompLapse(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID),
                                                    r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lProductID:=ToSafeInteger(lProductID), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_sGisBusinessTypeCode:=ToSafeString(m_sGISBusinessTypeCode), v_sRenewalStatusCode:=ToSafeString(sRenewalStatusCode), v_lOldInsuranceFileCnt:=ToSafeInteger(lOldInsuranceFileCnt))

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        result = gPMConstants.PMEReturnCode.PMFalse
                        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletion Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletion")
                        ' We dont exit, so we can process the next item
                        ' Log Error Message
                    End If

                Else

                    If sRenewalStatusCode.Trim() = ACStatusConfirm Then
                        ' Differentiate between renewal, alt insurer and what-if
                        Select Case sInsFileTypeCode
                            Case ACFileTypeRenewal
                                bCompHoldingInsurer = True
                            Case ACFileTypeWhatIf


                                bCompHoldingInsurer = vResultArray(LCArrayGISSchemeID, iLoop1).Equals(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))
                            Case Else
                                bCompHoldingInsurer = False
                        End Select

                        If bCompHoldingInsurer Then

                            'm_lReturn = m_oGIS.RenCompHoldingInsurer(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lRenewalEdiAuditId:=lRenewalEdiAuditID, v_lGISSchemeId:=lGISSchemeId, v_lRenewalGISSchemeID:=lRenewalGISSchemeID, r_lRenewalInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lProductID:=lProductID, v_dtRenewalDate:=dtRenewalDate, v_lPartyCnt:=lPartyCnt, v_lRiskCodeID:=lRiskCodeID, v_lGISDataModelID:=lGISDataModelID, v_sGisDataModelCode:=sGISDataModelCode)
                            m_lReturn = m_oGIS.RenCompHoldingInsurer(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=ToSafeInteger(lRenewalInsuranceFileCnt), v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID))
                        Else

                            'Complete for the new/old insurer

                            m_lReturn = m_oGIS.RenCompletion(v_lInsuranceFolderCnt:=ToSafeInteger(lInsuranceFolderCnt), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_dtRenewalDate:=ToSafeDate(dtRenewalDate), v_lRiskCodeID:=ToSafeInteger(lRiskCodeID), v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), v_lGISSchemeId:=ToSafeInteger(lGISSchemeId), v_lProductID:=ToSafeInteger(lProductID), r_lRenewalInsuranceFileCnt:=ToSafeInteger(lRenewalInsuranceFileCnt), v_lRenewalGISSchemeID:=ToSafeInteger(lRenewalGISSchemeID), v_lGisBusinessTypeId:=ToSafeInteger(lGisBusinessTypeId), v_lOldInsuranceFileCnt:=ToSafeInteger(lOldInsuranceFileCnt))
                        End If


                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            result = gPMConstants.PMEReturnCode.PMFalse
                            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletion Failed for Insurance Folder" & lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletion")
                            ' We dont exit, so we can process the next item
                            ' Log Error Message
                        End If
                        m_lReturn = gPMConstants.PMEReturnCode.PMTrue
                    End If

                End If

                'sj 21/11/2001 - start
                'Unlock the record
                If v_lInsuranceFolderCnt = 0 Then
                    m_lReturn = UnlockRecord(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                End If
                'sj 21/11/2001 - end

                'AK 211101 - Log this in Task Log
            Else
                m_sRenTaskLog = "Failed to Complete : Record Locked"
                m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=m_sRenTaskLog)

            End If

            If m_sGISBusinessTypeCode = "GIIT" Or m_sGISBusinessTypeCode = "GIIH" Or m_sGISBusinessTypeCode = "GIIM" Then

                m_lReturn = RenHouseKeep(lInsuranceFolderCnt:=lInsuranceFolderCnt)
            End If

        Next iLoop1

        Return result



        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenCompletion")

        result = gPMConstants.PMEReturnCode.PMError

        ' Log Error Message
        LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenCompletion Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenCompletion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        Return result

    End Function



    ' ***************************************************************** '
    '
    ' Name: Initialise
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Public Function Initialise(ByVal sUsername As String, ByVal sPassword As String, ByVal iUserID As Integer, ByVal iSourceId As Integer, ByVal iLanguageID As Integer, ByVal iCurrencyID As Integer, ByVal iLogLevel As Integer, ByVal sCallingAppName As String, Optional ByVal bStandAlone As Boolean = False, Optional ByVal vDatabase As Object = Nothing) As Long





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
            m_iSourceID = iSourceId
            m_iCurrencyID = iCurrencyID
            m_iLogLevel = iLogLevel


            ' Store the variables

            ' Get an instance of component services

            ' Get an instance of bGIS

            m_oGIS = Nothing
            result = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oGIS, v_sClassName:="bGIS.Renewals", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            If result <> gPMConstants.PMEReturnCode.PMTrue Then
                Dim r_sMessage As String = "Failed to create an instance of bGIS.Renewals"
                bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=r_sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:="bGIS.Renewals", vErrNo:=CStr(Informations.Err().Number), vErrDesc:=Informations.Err().Description)
                Return result
            End If
            m_lReturn = m_oGIS.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp))
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Get a connection to the database

            m_lReturn = gPMComponentServices.CheckDatabase(m_sUsername, m_iSourceID, m_iLanguageID, v_lPMProductFamily:=PMProductFamily, r_bNewInstanceCreated:=m_bCloseDatabase, r_oCheckedDatabase:=m_oDatabase, v_vDatabase:=vDatabase)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'sj 20/11/2001 - start
            ' bPMLock (User Class)

            m_oPMLockUser = New bpmlock.User
            m_lReturn = m_oPMLockUser.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMLock.User Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'bPMLock (Form)

            m_oPMLockForm = New bpmlock.Form
            m_lReturn = m_oPMLockForm.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp, vDatabase:=m_oDatabase)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="bPMLock.Form Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            'sj 20/11/2001 - end

            m_lReturn = CheckMultiCompany()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckMultiCompany Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'DC030105 PN26049
            m_lReturn = CheckUnderwriting()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="CheckUnderwriting Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Initialise Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)


            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: Terminate
    '
    ' Description:
    '
    ' History: 04/04/2001 CTAF - Created.
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
                If m_oGIS IsNot Nothing Then
                    m_oGIS.Dispose()
                End If
                m_oGIS = Nothing
                If m_oPMLockUser IsNot Nothing Then
                    m_oPMLockUser.Dispose()
                End If
                m_oPMLockUser = Nothing
                If m_oPMLockForm IsNot Nothing Then
                    m_oPMLockForm.Dispose()
                End If
                m_oPMLockForm = Nothing
                If m_oTaskLog IsNot Nothing Then
                    m_oTaskLog.Dispose()
                End If
                m_oTaskLog = Nothing
                If m_bCloseDatabase Then
                    m_oDatabase.CloseDatabase()
                End If
                m_oDatabase = Nothing
            End If
        End If
        Me.disposedValue = True
    End Sub


    ' ***************************************************************** '
    '
    ' Name: SelectSingleRecord
    '
    ' Description: Selects a single record based on in the insurance_folder
    '
    ' History: 15/05/2001 CTAF - Created.
    '
    ' ***************************************************************** '
    Private Function SelectSingleRecord(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sSQL As String, ByVal v_sSQLName As String, ByVal v_bStoredProcedure As Boolean, ByRef r_vResultArray(,) As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SelectSingleRecord")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add the effective date
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=Informations.FormatDateTime(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'JSB 11/06/2003 -  Add the insurance folder cnt
        If v_lInsuranceFolderCnt = 0 Then
            'Pass through vbNULL if value = 0, as SPs check for NULL
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(gPMConstants.VariantType_Null), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add failed to add insurance_folder_cnt, return code = " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingleRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        Else
            'Value is not zero so pass through actual value
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMError
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.Parameters.Add failed to add insurance_folder_cnt, return code = " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingleRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

                Return result
            End If
        End If

        If v_sSQL = ACSelectSingleSelectionSQL Or v_sSQL = ACSelectSingleInvitationSQL Then

            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=source_id", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_mode", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurer_mode", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="default_product", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=default_product", gPMConstants.PMELogLevel.PMLogError)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If

        'JSB 11/06/2003 end

        ' Call the stored procedure
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=v_sSQL, sSQLName:=v_sSQLName, bStoredProcedure:=v_bStoredProcedure, vResultArray:=r_vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            'JSB 11/06/2003
            result = gPMConstants.PMEReturnCode.PMFalse
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oDatabase.SQLSelect failed, return code = " & m_lReturn, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingleRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        ' Check we have sone results
        If Not Informations.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SelectSingleRecord Failed - Record not found for insurance folder: " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="SelectSingleRecord", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SelectSingleRecord")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: GetCurrentPolicyVersion
    '
    ' Description: This was taken from bSIRIUSLink and chopped up a bit
    '
    ' History: 29/03/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function GetCurrentPolicyVersion(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_lInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const ACInsuranceFileCnt As Integer = 0
        'Const ACProductId As Integer = 1
        'Const ACGisSchemeId As Integer = 2
        'Const ACIsInsurerLead As Integer = 3
        'Const ACPartyCnt As Integer = 4
        'Const ACRenewalDate As Integer = 5
        'Const ACGISDataModelID As Integer = 6
        'Const ACGISDataModel As Integer = 7
        'Const ACGISPolicyLinkID As Integer = 8

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
            result = gPMConstants.PMEReturnCode.PMNotFound

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetCurrentPolicyVersion Failed - Record not found for insurance folder: " & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:="GetCurrentPolicyVersion", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

            Return result
        End If

        ' Grab the values

        r_lInsuranceFileCnt = CInt(vResultArray(ACInsuranceFileCnt, 0))

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SelectRecords
    '
    ' Description: Some common code used to select the records
    '              for the various stages.
    '
    ' History: 20/04/2001 CTAF - Created.
    '          24/12/2003 SJ - Add "v_bInsurerFolderCntRequired" parameter
    '          RAM20040202  : Bug fix for PN Issue 10142
    '          27/04/2004 CJB - Cater for possibility that this may be running via a batch exe that
    '                           has been restricted to a particular branch (via a command line source id)
    ' ***************************************************************** '
    Private Function SelectRecords(ByVal v_sSQL As String, ByVal v_sSQLName As String, ByVal v_bStoredProcedure As Boolean, ByRef r_vResultArray(,) As Object, Optional ByVal v_bInsurerFolderCntRequired As Boolean = False, Optional ByVal v_sDefaultProduct As String = "", Optional ByVal v_bSourceIdRequired As Boolean = False, Optional ByVal v_bInsurerModeRequired As Boolean = False) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SelectRecords")



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iInsurerMode As Integer

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add the effective date
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=Informations.FormatDateTime(DateTime.Now), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'SET 20040225 - Merged from 1.6.9 - DC211002 pass default product/scheme
        If v_sSQL = ACSelectAutoRenDebitSQL Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="default_product", vValue:=v_sDefaultProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        ElseIf Not False Then
            If (v_sSQL = ACPreRenSelSQL) Or (v_sSQL = ACRenSelSQL) Or (v_sSQL = ACQuoteBrokerSQL) Or (v_sSQL = ACInvitationSQL) Then
                m_lReturn = m_oDatabase.Parameters.Add(sName:="default_product", vValue:=v_sDefaultProduct, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If
        End If

        'sj 24/12/2003 - Start
        If v_bInsurerFolderCntRequired Then
            ' Add the folder_cnt

            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'insurance_folder_cnt'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecords")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'sj 24/12/2003 - End

        'SJ 14/04/2004 - start
        ' CJB 270404 Cater for possibility that this may be running via a batch exe that has been
        ' restricted to a particular branch (via a command line source id). If BranchSpecificMode
        ' is zero then process for all branches (as before), else process for the value of the branch
        ' found in BranchSpecificMode.
        If v_bSourceIdRequired Then
            ' Add the source_id
            m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(m_iBranchSpecificMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'source_id'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecords")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        If v_bInsurerModeRequired Then
            If m_bIsInsurerMode Then
                iInsurerMode = 1
            Else
                iInsurerMode = 0
            End If
            ' Add the insurer mode
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_mode", vValue:=CStr(iInsurerMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'insurer_mode'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecords")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
        End If
        'SJ 14/04/2004 - end

        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040202 : Set the parameter to return all records not 500.
        '               Bug fix for PN Issue 10142 - START
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' Call the stored procedure
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=v_sSQL, sSQLName:=v_sSQLName, bStoredProcedure:=v_bStoredProcedure, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        ' RAM20040202  : Bug fix for PN Issue 10142 - END
        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check we have sone results
        If Not Informations.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No records found for processing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecords", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)

        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SelectRecords")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SelectAutoRenew
    '
    ' Description: Calls spu_SIRREN_Select_AutoRenew
    '
    ' History: 13/06/2001 CTAF - Created.
    '           TF271101 - Change to work on Renewal InsFileCnt only
    '
    ' ***************************************************************** '
    Private Function SelectAutoRenew(ByVal v_lInsuranceFileCnt As Integer, ByRef r_vResultArray(,) As Object) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SelectAutoRenew")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear parameters
        m_oDatabase.Parameters.Clear()

        ' Add parameters
        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_file_cnt", vValue:=CStr(v_lInsuranceFileCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Call the procedure
        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACSelectAutoRenewSQL, sSQLName:=ACSelectAutoRenewName, bStoredProcedure:=ACSelectAutoRenewStored, vResultArray:=r_vResultArray)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SelectAutoRenew")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckAutoPayment
    '
    ' Description:
    '
    ' History: 12/06/2001 CTAF - Created.
    '           TF271101 - Changed to work on Renewal InsFileCnt only
    ' ***************************************************************** '
    Private Function CheckAutoPayment(ByVal v_lInsuranceFileCnt As Integer, ByRef r_bAutoPayment As Boolean, ByVal v_lSchemeTypeFlags As Integer) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing

        Const LCArrayPaymentMethod As Integer = 0

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".CheckAutoPayment")



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Default to not auto
        r_bAutoPayment = False

        ' TF271101 Check Payment Type against current Renewal record ...
        m_lReturn = SelectAutoRenew(v_lInsuranceFileCnt:=v_lInsuranceFileCnt, r_vResultArray:=vResultArray)

        ' Check if we have more than 1
        If Not Informations.IsArray(vResultArray) Then
            Return result
        End If

        ' Check that its direct debit payment method

        If CStr(vResultArray(LCArrayPaymentMethod, 0)).ToUpper() = AC_DirectDebit Then

            'sj 21/11/2001 - start
            'Check the scheme flags to see if this scheme supports autorenew
            m_lReturn = DecodeSchemeFlags(v_lSchemeFlags:=v_lSchemeTypeFlags, r_bAutoRenew:=r_bAutoPayment)
            'sj 21/11/2001 - end

        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".CheckAutoPayment")

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: RenReminder
    '
    ' Description: Renewals Reminder
    '
    ' History: 13/06/2001 CTAF - Created.
    '
    '          AK  - 22062001 - added Document production code
    '          CJB - 09092002 - Pass SchemeID to the RenReminder functio
    '                           for CNIC.
    '          CJB - 29042004 - Pass source id in to allow filtering if necessary
    ' ***************************************************************** '
    Public Function RenReminder(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0

        Dim vResultArray(,) As Object = Nothing
        Dim lInsFolderCnt, lRenInsFileCnt, lPartyCnt As Integer
        Dim sDataModel As String = ""
        Dim lGISSchemeId As Integer 'CJB090902
        Dim lAgentCnt As Integer

        'Modified by Sudhanshu Behera on 5/18/2010 6:59:44 PM refer developer guide no. 108 (guide)
        'Dim oDocMgrWrapper As bSIRDocManagerWrapper.Interface
        Dim oDocMgrWrapper As Object = Nothing
        Dim oDocLink As New bPMBDocLink.Business
        Dim oDocTemplate As New bSIRDocTemplate.Business
        Dim oTaskInstance As New bPMWrkTaskInstanceTemp.FormClass
        Dim sMessage As String = ""

        ' Local constants
        Const LCArrayInsuranceFolderCnt As Integer = 0
        Const LCArrayGISSchemeID As Integer = 1
        'Const LCArrayRenSchemeID As Integer = 2
        Const LCArrayRenewalInsFileCnt As Integer = 3
        'Const LCArrayProductID As Integer = 4
        'Const LCArrayRenewalDate As Integer = 5
        Const LCArrayPartyCnt As Integer = 6
        'Const LCArrayRiskCodeID As Integer = 7
        'Const LCArrayDataModelID As Integer = 8
        'Const LCArrayEDIAuditId As Integer = 9
        Const LCArrayDataModel As Integer = 10
        Const LCArrayAgentCnt As Integer = 11

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RenReminder")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If v_lInsuranceFolderCnt = 0 Then
                'sj 24/12/2003 - Add "v_bInsurerFolderCntRequired" parameter
                ' No insurance folder passed. Select all valid records
                ' CJB 290404 Pass source id in to allow filtering if necessary
                ' CJB 050504 Pass insurer mode parameter
                m_lReturn = SelectRecords(v_sSQL:=ACSelectReminderSQL, v_sSQLName:=ACSelectReminderName, v_bStoredProcedure:=ACSelectReminderStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray, v_bInsurerFolderCntRequired:=True)

            Else
                ' Insurance folder passed in
                m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleReminderSQL, v_sSQLName:=ACSelectSingleReminderName, v_bStoredProcedure:=ACSelectSingleReminderStored, r_vResultArray:=vResultArray)

            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out
                Return result
            End If

            PrintLinkedDocumentsStart(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance, sMessage) 'PN19539

            ' Iterrate through the array

            For iLoop1 As Integer = 0 To vResultArray.GetUpperBound(1)


                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

                lInsFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

                lRenInsFileCnt = CInt(vResultArray(LCArrayRenewalInsFileCnt, iLoop1))

                sDataModel = CStr(vResultArray(LCArrayDataModel, iLoop1)).Trim()

                lGISSchemeId = CInt(CStr(vResultArray(LCArrayGISSchemeID, iLoop1)).Trim()) 'CJB090902

                lAgentCnt = CInt(vResultArray(LCArrayAgentCnt, iLoop1)) 'PN19539

                PrintLinkedDocuments("REMIND", "RenReminder", lGISSchemeId, lAgentCnt, lInsFolderCnt, lRenInsFileCnt, lPartyCnt, oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance) 'PN19539

                ' Since doing PN19539 above this is no longer rqd (has no code anyway) but left in for possible
                ' future use...

                m_lReturn = m_oGIS.RenReminder(v_lInsuranceFolderCnt:=ToSafeInteger(lInsFolderCnt), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_lRenInsFileCnt:=ToSafeInteger(lRenInsFileCnt), v_sGisDataModelCode:=ToSafeString(sDataModel), v_vGISSchemeID:=ToSafeInteger(lGISSchemeId))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    ' Don't exit, so we can process as many records as possible
                End If

            Next iLoop1

            PrintLinkedDocumentsEnd(oDocLink, oDocTemplate, oDocMgrWrapper, oTaskInstance) 'PN19539

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RenReminder")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenReminder")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReminder Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReminder", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: RenReprintConfirm
    '
    ' Description: Insurance files selected for Renewal Invitation.
    '
    ' History: 13/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenReprintConfirm(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        Const LCArrayProductID As Single = 4
        Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        Const LCArrayRiskCodeID As Single = 7
        Const LCArrayGISDataModelID As Single = 8
        Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10

        Dim vResultArray(,) As Object = Nothing
        Dim iLoop1 As Integer

        Dim lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Integer
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID, lRenewalEdiAuditID As Integer
        Dim sGISDataModelCode As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RenReprintConfirm")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pick up the individual record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleInvitationSQL, v_sSQLName:=ACSelectSingleInvitationName, v_bStoredProcedure:=ACSelectSingleInvitationStored, r_vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out of
                Return result
            End If

            iLoop1 = 0

            ' Grab the values out of the array

            lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

            lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

            lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

            lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

            lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

            dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

            lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

            lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

            lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

            lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

            sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()


            m_lReturn = m_oGIS.RenReprintConfirm(v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), r_lRenewalInsuranceFileCnt:=ToSafeInteger(lRenewalInsuranceFileCnt), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_sGisBusinessTypeCode:="")


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RenReprintConfirm")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenReprintConfirm")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenReprintConfirm Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenReprintConfirm", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: RenResendEdi
    '
    ' Description: Insurance files selected for Renewal Invitation.
    '
    ' History: 13/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function RenResendEdi(Optional ByVal v_lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        Const LCArrayProductID As Single = 4
        Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        Const LCArrayRiskCodeID As Single = 7
        Const LCArrayGISDataModelID As Single = 8
        Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10

        Dim vResultArray(,) As Object = Nothing
        Dim iLoop1 As Integer

        Dim lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lProductID As Integer
        Dim dtRenewalDate As Date
        Dim lPartyCnt, lRiskCodeID, lGISDataModelID, lRenewalEdiAuditID As Integer
        Dim sGISDataModelCode As String = ""

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".RenResendEdi")

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Pick up the individual record
            m_lReturn = SelectSingleRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_sSQL:=ACSelectSingleInvitationSQL, v_sSQLName:=ACSelectSingleInvitationName, v_bStoredProcedure:=ACSelectSingleInvitationStored, r_vResultArray:=vResultArray)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Error if nothing was found
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                ' Exit out of
                Return result
            End If

            iLoop1 = 0

            ' Grab the values out of the array

            lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop1))

            lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop1))

            lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop1))

            lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop1))

            lProductID = CInt(vResultArray(LCArrayProductID, iLoop1))

            dtRenewalDate = CDate(vResultArray(LCArrayRenewalDate, iLoop1))

            lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop1))

            lRiskCodeID = CInt(vResultArray(LCArrayRiskCodeID, iLoop1))

            lGISDataModelID = CInt(vResultArray(LCArrayGISDataModelID, iLoop1))

            lRenewalEdiAuditID = CInt(vResultArray(LCArrayRenewalEDIAuditID, iLoop1))

            sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop1)).Trim()


            m_lReturn = m_oGIS.RenResendEdi(v_lGISDataModelID:=ToSafeInteger(lGISDataModelID), v_sGisDataModelCode:=ToSafeString(sGISDataModelCode), r_lRenewalInsuranceFileCnt:=ToSafeInteger(lRenewalInsuranceFileCnt), v_lRenewalEdiAuditId:=ToSafeInteger(lRenewalEdiAuditID), v_sGisBusinessTypeCode:="")


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".RenResendEdi")

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".RenResendEdi")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenResendEdi Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="RenResendEdi", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: TransferPolicyToStandardRenewals
    '
    ' Description: Insurance files selected for Renewal Invitation.
    '
    ' History: 13/09/2001 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function TransferPolicyToStandardRenewals(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lPartyCnt As Integer, ByVal v_sTransferReason As String, ByVal v_sTransferNotes As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oGIS.RenTransferPolicyToStandardRenewals(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_lPartyCnt:=ToSafeInteger(v_lPartyCnt), v_sTransferReason:=ToSafeString(v_sTransferReason), v_sTransferNotes:=ToSafeString(v_sTransferNotes))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            ' Debug message
            Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Errored in " & ACApp & "." & ACClass & ".TransferPolicyToStandardRenewals")

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="TransferPolicyToStandardRenewals Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="TransferPolicyToStandardRenewals", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: CheckSetRecordLock
    '
    ' Description:
    '
    ' History: 21/11/2001 sj - Created.
    '
    ' ***************************************************************** '
    Private Function CheckSetRecordLock(ByVal v_lInsuranceFolderCnt As Integer, ByVal v_sMethod As String, ByRef r_bLocked As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim sCurrentlyLockedBy As String = ""
        Dim sMessage As String = ""

        r_bLocked = False

        If v_lInsuranceFolderCnt = 0 Then
            Return result
        End If

        'This is called from the scheduler
        m_lReturn = LockRecord(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, r_bLocked:=r_bLocked, r_sCurrentlyLockedBy:=sCurrentlyLockedBy)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to lock record for Insurance Folder" & v_lInsuranceFolderCnt, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)
            r_bLocked = True
            Return result
        End If

        If r_bLocked Then
            sMessage = "Record for Insurance Folder " & v_lInsuranceFolderCnt &
                        " Locked By " & sCurrentlyLockedBy
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMessage, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sMethod)
            Return result
        End If


        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: LockRecord
    '
    ' Description:
    '
    ' History: 20/11/2001 sj - Created.
    '
    ' ***************************************************************** '
    Public Function LockRecord(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_bLocked As Boolean, ByRef r_sCurrentlyLockedBy As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim sCurrentlyLockedBy As String = ""

            r_bLocked = False


            m_lReturn = m_oPMLockUser.LockKey(sKeyName:=ACLockKeyInsuranceFolder, vKeyValue:=v_lInsuranceFolderCnt, iUserID:=m_iUserID, sCurrentlyLockedBy:=sCurrentlyLockedBy)
            If m_lReturn = gPMConstants.PMEReturnCode.PMFalse And sCurrentlyLockedBy.Trim() <> "" Then
                r_sCurrentlyLockedBy = sCurrentlyLockedBy
                r_bLocked = True
                result = gPMConstants.PMEReturnCode.PMTrue
            Else
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMLockUser.LockKey", vApp:=ACApp, vClass:=ACClass, vMethod:="LockRecord")

                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LockRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LockRecord", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: UnlockRecord
    '
    ' Description:
    '
    ' History: 20/11/2001 sj - Created.
    '
    ' ***************************************************************** '
    Public Function UnlockRecord(ByRef v_lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oPMLockUser.UnLockKey(sKeyName:=ACLockKeyInsuranceFolder, vKeyValue:=v_lInsuranceFolderCnt, iUserID:=m_iUserID)
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then

                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMLockUser.UnLockKey Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockRecord")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If


            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockRecord Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockRecord", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    '
    ' Name: UnlockAllRecords
    '
    ' Description:
    '
    ' History: 20/11/2001 sj - Created.
    '
    ' ***************************************************************** '
    Public Function UnlockAllRecords() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oPMLockForm.UnLockAllForUser(iUserID:=m_iUserID)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="m_oPMLockForm.UnLockAllForUser Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockAllRecords")

                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockAllRecords Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockAllRecords", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function




    'AK 211101 - added this fucntion to handle Locking-errors

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


        m_lReturn = m_oTaskLog.AddRenewalTaskLog(v_lInsuranceFolderCnt:=ToSafeInteger(v_lInsuranceFolderCnt), v_lProcessID:=ToSafeInteger(v_lProcessID), v_sProcessCode:=ToSafeString(v_sProcessCode), v_lStatus:=ToSafeInteger(v_lStatus), v_sMessage:=ToSafeString(v_sMessage))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            result = gPMConstants.PMEReturnCode.PMFalse
            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to process m_oTaskLog.AddRenewalTaskLog", vApp:=ACApp, vClass:=ACClass, vMethod:="Initialise", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description)
            Return result
        End If

        Return result

    End Function

    ' ***************************************************************** '
    ' Name: RenAutoRenewInvitedInvited
    '
    ' Description: AutoConfirm & Renew invited records that have passed Renewal date.
    '
    ' History: TF271101 - Created
    '
    ' ***************************************************************** '
    Public Function RenAutoRenewInvited() As Integer


        Dim result As Integer = 0
        Const LCArrayInsuranceFolderCnt As Single = 0
        Const LCArrayGISSchemeID As Single = 1
        Const LCArrayRenewalGISSchemeID As Single = 2
        Const LCArrayRenewalInsuranceFileCnt As Single = 3
        'Const LCArrayProductID As Single = 4
        'Const LCArrayRenewalDate As Single = 5
        Const LCArrayPartyCnt As Single = 6
        'Const LCArrayRiskCodeID As Single = 7
        'Const LCArrayGISDataModelID As Single = 8
        'Const LCArrayRenewalEDIAuditID As Single = 9
        Const LCArrayGISDataModelCode As Single = 10
        'Const LCArrayOfferAlt As Single = 11
        Const LCArraySchemeTypeFlags As Integer = 12

        Dim vResultArray(,) As Object = Nothing
        Dim bLocked, bAutoPayment As Boolean

        Dim lInsuranceFolderCnt, lGISSchemeId, lRenewalGISSchemeID, lRenewalInsuranceFileCnt, lPartyCnt As Integer
        Dim sGISDataModelCode As String = ""
        Dim lSchemeTypeFlags As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' CJB 290404 Pass source id in to allow filtering if necessary
            ' CJB 040504 Pass InsurerMode in
            ' Get all the records
            m_lReturn = SelectRecords(v_sSQL:=ACSelectAutoRenInvitedSQL, v_sSQLName:=ACSelectAutoRenInvitedName, v_bStoredProcedure:=ACSelectAutoRenInvitedStored, v_bSourceIdRequired:=True, v_bInsurerModeRequired:=True, r_vResultArray:=vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                If m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                End If
                Return result
            End If

            ' Go through the array

            For iLoop As Integer = 0 To vResultArray.GetUpperBound(1)

                ' Grab the values out of the array

                lInsuranceFolderCnt = CInt(vResultArray(LCArrayInsuranceFolderCnt, iLoop))

                lGISSchemeId = CInt(vResultArray(LCArrayGISSchemeID, iLoop))

                lRenewalGISSchemeID = CInt(vResultArray(LCArrayRenewalGISSchemeID, iLoop))

                lRenewalInsuranceFileCnt = CInt(vResultArray(LCArrayRenewalInsuranceFileCnt, iLoop))

                lPartyCnt = CInt(vResultArray(LCArrayPartyCnt, iLoop))

                sGISDataModelCode = CStr(vResultArray(LCArrayGISDataModelCode, iLoop)).Trim()

                lSchemeTypeFlags = CInt(Val(CStr(vResultArray(LCArraySchemeTypeFlags, iLoop))))

                ' Lock the record
                bLocked = False
                m_lReturn = CheckSetRecordLock(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_sMethod:="RenAutoRenewInvited", r_bLocked:=bLocked)
                If Not bLocked Then

                    m_lReturn = CheckAutoPayment(v_lInsuranceFileCnt:=lRenewalInsuranceFileCnt, r_bAutoPayment:=bAutoPayment, v_lSchemeTypeFlags:=lSchemeTypeFlags)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If

                    If bAutoPayment Then

                        ' Confirm it
                        m_lReturn = ConfirmRenewalBrokerLed(v_lInsuranceFolderCnt:=lInsuranceFolderCnt, v_lInsuranceFileCnt:=lRenewalInsuranceFileCnt, v_lSchemeID:=lGISSchemeId, v_lPartyCnt:=lPartyCnt, v_sGisDataModelCode:=sGISDataModelCode, v_sGisBusinessTypeCode:=GISBusinessTypeCode)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                        ' Complete it (Renew)
                        m_lReturn = RenCompletion(v_lInsuranceFolderCnt:=lInsuranceFolderCnt)
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            Return gPMConstants.PMEReturnCode.PMFalse
                        End If

                    End If

                End If

            Next iLoop

            Return result

        Catch excep As System.Exception




            result = gPMConstants.PMEReturnCode.PMError

            GISSharedConstants.LogMessageFile(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="RenAutoRenewInvited Failed.", vApp:=ACApp, vClass:=ACClass, vMethod:="RenAutoRenewInvited", vErrNo:=Informations.Err().Number, vErrDesc:=excep.Message)

            Return result

        End Try
    End Function

    Public Function RenHouseKeep(Optional ByVal lInsuranceFolderCnt As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "RenHouseKeep"

        Dim sHousekeepDayNum As String = ""
        Dim lHousekeepDayNum As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'If InsuranceFolderCnt has not been passed, run housekeep all
            If lInsuranceFolderCnt = 0 Then

                'Get the registry setting for Housekeep day num
                m_lReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLServer, v_sSettingName:=ACRegHousekeep, r_sSettingValue:=sHousekeepDayNum, v_sSubKey:=ACRegServerRenewal)

                Dim dbNumericTemp As Double
                If Double.TryParse(sHousekeepDayNum, NumberStyles.Number, CultureInfo.CurrentCulture.NumberFormat, dbNumericTemp) Then
                    lHousekeepDayNum = CInt(Val(sHousekeepDayNum))
                Else
                    'Default it to 60
                    lHousekeepDayNum = 60
                End If

                'Clear the parameters
                m_oDatabase.Parameters.Clear()

                'Add the parameters
                m_lReturn = m_oDatabase.Parameters.Add(sName:="DayNum", vValue:=CStr(lHousekeepDayNum), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=DayNum", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(BranchSpecificMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=source_id", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_mode", vValue:=CStr(IsInsurerMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=insurer_mode", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=user_id", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Call the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenHouseKeepAllSQL, sSQLName:=ACRenHouseKeepAllName, bStoredProcedure:=ACRenHouseKeepAllStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACRenHouseKeepAllSQL", gPMConstants.PMELogLevel.PMLogError)
                End If
            Else

                'Clear the parameters
                m_oDatabase.Parameters.Clear()

                'Add the parameters
                m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_Folder_Cnt", vValue:=CStr(lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=Insurance_Folder_Cnt", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = m_oDatabase.Parameters.Add(sName:="user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.Parameters.Add", "sName:=user_id", gPMConstants.PMELogLevel.PMLogError)
                End If

                'Call the stored procedure
                m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenHouseKeepSQL, sSQLName:=ACRenHouseKeepName, bStoredProcedure:=ACRenHouseKeepStored)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oDatabase.SQLAction", "sSQL:=ACRenHouseKeepSQL", gPMConstants.PMELogLevel.PMLogError)
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

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

    '***************************************************************** '
    'Name: FortisRenewalCheck
    '
    'Description: Designed to switch a renewal from a non-Polaris Fortis
    '       Scheme to a Polaris scheme. The SP performs a check for
    '       particular Fortis Schemes, and if found changes the prior
    '       created copied records.
    '
    'IDP Jan 2003.
    '***************************************************************** '
    Private Function FortisRenewalCheck(ByVal lInsuranceFolderCnt As Integer) As Integer

        Dim result As Integer = 0
        Dim sMessage As String = ""



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()

        ' Add the Insurance Folder Count
        m_lReturn = m_oDatabase.Parameters.Add(sName:="Insurance_Folder_Cnt", vValue:=CStr(lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "m_oDatabase.Parameters.Add failed to add effective_date, return code = " & m_lReturn
            Throw New Exception()
        End If

        ' Call the stored procedure
        m_lReturn = m_oDatabase.SQLAction(sSQL:=ACRenFortisRenewalSQL, sSQLName:=ACRenFortisRenewalName, bStoredProcedure:=ACRenFortisRenewalStored)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            sMessage = "m_oDatabase.SQLSelect failed, return code = " & m_lReturn
            Throw New Exception()
        End If

        Return result

    End Function



    Private Function CreateDocumentTask(ByRef oDocTemplate As Object, ByRef oTaskInstance As Object, ByVal lDocumentTemplateId As Integer, ByVal lInsuranceFileCnt As Integer, ByVal lPartyCnt As Integer) As Integer
        Dim result As Integer = 0


        Dim vTaskId As Object = Nothing
        Dim lInstanceID As Integer
        Dim vPartyType As Object = Nothing
        Dim lPMWrkTaskGroupID, lPMWrkTaskID As Integer
        Dim sCustomer As String = ""
        Dim dtTaskDueDate As Date
        Dim lPMUserGroupID As Integer
        Dim iUserID As Integer
        Dim sDescription As String = ""
        Dim iTaskStatus, iIsUrgent As Integer
        Dim dtDateCreated As Date
        Dim iCreatedByID As Integer
        Dim dtLastModified As Date
        Dim iModifiedByID As Integer
        Dim vPartyPolicy As Object = Nothing
        Dim iNumDays As Integer


        result = gPMConstants.PMEReturnCode.PMTrue

        'If there is a task attached to this document then get the task_id

        m_lReturn = oDocTemplate.GetTaskID(r_vTaskID:=vTaskId, m_lDocumentTemplateId:=ToSafeInteger(lDocumentTemplateId))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get task details for document_template_id " & lDocumentTemplateId, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocumentTask")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If Not Informations.IsArray(vTaskId) Then
            'No task for this document
            Return result
        End If


        If CStr(vTaskId(0, 0)) = "" Then
            'No task for this document
            Return result
        End If

        'Get customer details

        m_lReturn = oDocTemplate.GetPartyPolicy(r_vArray:=vPartyPolicy, m_lInsuranceFileCnt:=ToSafeInteger(lInsuranceFileCnt), m_lPartyCnt:=ToSafeInteger(lPartyCnt))

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get policy & policy details", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocumentTask")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the task related details

        m_lReturn = oTaskInstance.GetDetails(v_lPMWrkTaskInstanceCnt:=vTaskId(0, 0), r_lPMWrkTaskGroupID:=ToSafeInteger(lPMWrkTaskGroupID), r_lpmwrktaskid:=ToSafeInteger(lPMWrkTaskID), r_scustomer:=ToSafeString(sCustomer), r_dttaskduedate:=ToSafeDate(dtTaskDueDate), r_lpmusergroupid:=ToSafeInteger(lPMUserGroupID), r_iuserid:=ToSafeInteger(iUserID), r_sdescription:=ToSafeString(sDescription), r_itaskstatus:=ToSafeInteger(iTaskStatus), r_iisurgent:=ToSafeInteger(iIsUrgent), r_dtdatecreated:=ToSafeDate(dtDateCreated), r_icreatedbyid:=ToSafeInteger(iCreatedByID), r_dtlastmodified:=ToSafeDate(dtLastModified), r_imodifiedbyid:=ToSafeInteger(iModifiedByID))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get task details for task_id " & lPMWrkTaskID, vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocumentTask")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Get the party type

        m_lReturn = oDocTemplate.GetPartyType(r_vPartytype:=vPartyType, v_lPartyCnt:=ToSafeInteger(lPartyCnt))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get party type", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocumentTask")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'get the number of days between - TaskDueDate and DateCreated
        iNumDays = Informations.DateDiff("d", dtTaskDueDate, dtDateCreated, DayOfWeek.Sunday, FirstWeekOfYear.FirstJan1)
        dtDateCreated = DateTime.Now
        dtTaskDueDate = DateTime.Parse(Informations.FormatDateTime((dtDateCreated).AddDays(iNumDays).AddDays(CDate("23:59:59").ToOADate())))

        ' set the customer details


        sCustomer = CStr(vPartyPolicy(1)) & " - " & CStr(vPartyPolicy(2))


        m_lReturn = oTaskInstance.CreateNew(r_lPMWrkTaskInstanceCnt:=ToSafeInteger(lInstanceID), v_lPMWrkTaskGroupID:=ToSafeInteger(lPMWrkTaskGroupID), v_lPMWrkTaskID:=ToSafeInteger(lPMWrkTaskID), v_sCustomer:=ToSafeString(sCustomer), v_dtTaskDueDate:=ToSafeDate(dtTaskDueDate), v_lPMUserGroupID:=ToSafeInteger(lPMUserGroupID), v_iUserID:=ToSafeInteger(iUserID), v_sDescription:=ToSafeString(sDescription), v_iTaskStatus:=ToSafeInteger(iTaskStatus), v_iIsUrgent:=ToSafeInteger(iIsUrgent), v_dtdatecreated:=ToSafeDate(dtDateCreated), v_icreatedbyid:=ToSafeInteger(iCreatedByID), v_bIsNewInstance:=True)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create new task", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocumentTask")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        'Create the task instance keys now


        m_lReturn = oTaskInstance.CreateKeys(v_lTaskInstanceID:=ToSafeInteger(lInstanceID), v_lPartyCnt:=ToSafeInteger(lPartyCnt), v_sShortName:=CType(vPartyPolicy(1), Object), v_sPartyType:=CStr(vPartyType(0, 0)).Trim(), v_sResolvedName:=CStr(vPartyType(1, 0)).Trim(), v_lInsuranceFileCnt:=ToSafeInteger(lInsuranceFileCnt))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create task keys", vApp:=ACApp, vClass:=ACClass, vMethod:="CreateDocumentTask")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckMultiCompany
    '
    ' Description:
    '
    ' History: 13/04/2004 SJ - Created.
    '
    ' ***************************************************************** '
    Private Function CheckMultiCompany() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue
        'developer guide no.(As per VB Code)
        'Dim vValue As Byte
        Dim vValue As Object = Nothing

        m_bIsMultiCompany = False

        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option for Multi Tree Accounting", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMultiCompany")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If vValue <> 1 Then
            Return result
        End If

        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableBranchSelectAtLogon, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option for Enable Branch Select At Logon", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckMultiCompany")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        If vValue <> 1 Then
            Return result
        End If

        m_bIsMultiCompany = True

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: CheckUnderwriting
    '
    ' Description:
    '
    ' History: 02-01-06 DC - Created.
    '
    ' ***************************************************************** '
    Private Function CheckUnderwriting() As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        Dim vValue As Object = Nothing

        m_sUnderwritingOrAgency = "A"


        m_lReturn = bPMFunc.getProductOptionValue(v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iMainSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_sCallingAppName:=m_sCallingAppName, v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTUnderwriting, v_vBranch:=m_iSourceID, r_vUnderwriting:=CStr(vValue))
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get product option for Underwriting or Agency", vApp:=ACApp, vClass:=ACClass, vMethod:="CheckUnderwriting")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If


        m_sUnderwritingOrAgency = CStr(vValue)

        Return result

    End Function

    ' ***************************************************************** '
    '
    ' Name: SelectRecordsForPreRenSelection
    '
    ' Description: Select records for pre renewal selection
    '
    ' History: 19/04/2004 SJ  - Created.
    '          27/04/2004 CJB - Cater for possibility that this may be running via a batch exe that
    '                           has been restricted to a particular branch (via a command line source id)
    '
    ' ***************************************************************** '
    Private Function SelectRecordsForPreRenSelection(ByVal v_lInsuranceFolderCnt As Integer, ByRef r_vResultArray(,) As Object, ByVal v_sDefaultProduct As String) As Integer

        ' Debug message
        Dim result As Integer = 0
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Entering " & ACApp & "." & ACClass & ".SelectRecordsForPreRenSelection")



        result = gPMConstants.PMEReturnCode.PMTrue

        Dim iInsurerMode As Integer
        Dim dtEffectiveDate As Date

        ' Clear the parameters
        m_oDatabase.Parameters.Clear()


        If v_lInsuranceFolderCnt <> 0 Then
            dtEffectiveDate = DateTime.Now.AddYears(2)
        Else
            dtEffectiveDate = DateTime.Now
        End If

        ' Add the effective date
        m_lReturn = m_oDatabase.Parameters.Add(sName:="effective_date", vValue:=Informations.FormatDateTime(dtEffectiveDate), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMDate)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' CJB 270404 Cater for possibility that this may be running via a batch exe that has been
        ' restricted to a particular branch (via a command line source id). If BranchSpecificMode
        ' is zero then process for all branches (as before), else process for the value of the branch
        ' found in BranchSpecificMode.
        m_lReturn = m_oDatabase.Parameters.Add(sName:="source_id", vValue:=CStr(BranchSpecificMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'source_id'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecordsForPreRenSelection")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the insurer mode
        If m_bIsInsurerMode Then
            iInsurerMode = 1
        Else
            iInsurerMode = 0
        End If

        m_lReturn = m_oDatabase.Parameters.Add(sName:="insurer_mode", vValue:=CStr(iInsurerMode), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'insurer_mode'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecordsForPreRenSelection")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the insurance folder_cnt
        If v_lInsuranceFolderCnt <> 0 Then
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=CStr(v_lInsuranceFolderCnt), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        Else

            'developer guide no.85
            m_lReturn = m_oDatabase.Parameters.Add(sName:="insurance_folder_cnt", vValue:=DBNull.Value, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMLong)
        End If

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'insurance_folder_cnt'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecordsForPreRenSelection")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Add the Default Product
        m_lReturn = m_oDatabase.Parameters.Add(sName:="default_product", vValue:=gPMFunctions.ToSafeString(v_sDefaultProduct, ""), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to add parameter 'default_product'", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecordsForPreRenSelection")
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        m_lReturn = m_oDatabase.SQLSelect(sSQL:=ACPreRenSelSQL, sSQLName:=ACPreRenSelName, bStoredProcedure:=ACPreRenSelStored, vResultArray:=r_vResultArray, lNumberRecords:=gPMConstants.PMAllRecords)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            Return gPMConstants.PMEReturnCode.PMFalse
        End If

        ' Check we have sone results
        If Not Informations.IsArray(r_vResultArray) Then
            result = gPMConstants.PMEReturnCode.PMNotFound

            ' Log Error Message
            LogAppError(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="No records found for processing.", vApp:=ACApp, vClass:=ACClass, vMethod:="SelectRecordsForPreRenSelection")
        End If

        ' Debug message
        Debug.WriteLine(CStr(DateTime.Now.TimeOfDay.TotalSeconds) & ": Exiting " & ACApp & "." & ACClass & ".SelectRecordsForPreRenSelection")

        Return result

    End Function

    ' ********************************************************************* '
    ' Name          : RenewalsCreditCardCheck
    ' Description   : Function to see if we have a valid card we can try to
    '                 take payment from. Note that if we haven't got one then
    '                 this sp will take care of removing the cashlist and
    '                 cashlistitem records (if applicable).
    ' Depends On    : spu_ACT_Do_RenewalsCreditCardCheck
    ' Edit History  :
    ' CJB20050121   : Created
    ' ********************************************************************* '
    Private Function RenewalsCreditCardCheck(ByVal v_lRenewalInsFileCnt As Integer, ByRef r_bIsCardPaymentType As Boolean, ByRef r_bIsValidCard As Boolean) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("renewal_insurance_file_cnt", CStr(v_lRenewalInsFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            .Parameters.Add("is_card_payment_type", CStr(r_bIsCardPaymentType), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMBoolean)
            .Parameters.Add("is_valid_card", CStr(r_bIsValidCard), gPMConstants.PMEParameterDirection.PMParamOutput, gPMConstants.PMEDataType.PMBoolean)
            'developer guide no.39
            m_lReturn = .SQLAction("spu_ACT_Do_RenewalsCreditCardCheck", "Do RenewalsCreditCardCheck", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("RenewalsCreditCardCheck", "spu_ACT_Do_RenewalsCreditCardCheck failed. renewal_insurance_file_cnt:" & v_lRenewalInsFileCnt & "")
            End If

            r_bIsCardPaymentType = gPMFunctions.NullToBoolean(.Parameters.Item("is_card_payment_type").Value)
            r_bIsValidCard = gPMFunctions.NullToBoolean(.Parameters.Item("is_valid_card").Value)

        End With

        Return result
    End Function

    ' ********************************************************************* '
    ' Name          : DeleteCashlistItem
    ' Description   : Remove any cashlist & cashlistitem records linked to the
    '                 insurance_file that were copied over previously as an
    '                 error has occurred.
    ' Depends On    : spu_ACT_Update_PolicyCashListItem
    ' Edit History  :
    ' CJB20050124   : Created
    ' ********************************************************************* '
    Private Function DeleteCashlistItem(ByVal v_lInsFileCnt As Integer, ByRef v_lCashlistItemID As Object) As Integer

        Dim result As Integer = 0


        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("insurance_file_cnt", CStr(v_lInsFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)

            .Parameters.Add("cashlistitem_id", CStr(v_lCashlistItemID), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no.39
            m_lReturn = .SQLAction("spu_ACT_Update_PolicyCashListItem", "Do DeleteCashlistItem", True)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DeleteCashlistItem", "spu_ACT_Do_DeleteCashlistItem failed. insurance_file_cnt:" & v_lInsFileCnt)
            End If

        End With

        Return result
    End Function

    ' ********************************************************************* '
    ' Name          : SelectCardDataForAuthorisation
    ' Description   : Function to get card related data prior to authorisation.
    ' Depends On    : spu_ACT_Select_CardDataForAuthorisation
    ' Edit History  :
    ' CJB20050121   : Created
    ' ********************************************************************* '
    Private Function SelectCardDataForAuthorisation(ByVal v_lRenewalInsFileCnt As Integer, ByRef r_sMediaTypeIssuerConnectorCode As String, ByRef r_cAmount As Decimal, ByRef r_sCCNumber As String, ByRef r_sCCNameOnCard As String, ByRef r_sCCExpiryDate As String, ByRef r_sCCStartDate As String, ByRef r_sCCIssue As String, ByRef r_sCCPIN As String, ByRef r_sAddress1 As String, ByRef r_sPostcode As String, ByRef r_sCCCustomerFlag As String, ByRef r_lCashlistitemID As Integer, ByRef r_lCashlistID As Integer, ByRef r_sPartyShortName As String, ByRef r_sPolicyNo As String, ByRef r_sPartyType As String, ByRef r_sPartyResolvedName As String) As Integer

        Dim result As Integer = 0
        Dim vResultArray(,) As Object = Nothing



        result = gPMConstants.PMEReturnCode.PMTrue

        With m_oDatabase
            .Parameters.Clear()
            .Parameters.Add("insurance_file_cnt", CStr(v_lRenewalInsFileCnt), gPMConstants.PMEParameterDirection.PMParamInput, gPMConstants.PMEDataType.PMLong)
            'developer guide no.39
            m_lReturn = .SQLSelect("spu_ACT_Select_CardDataForAuthorisation", "Do SelectCardDataForAuthorisation", True, , vResultArray)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SelectCardDataForAuthorisation", "spu_ACT_Select_CardDataForAuthorisation failed. insurance_file_cnt:" & v_lRenewalInsFileCnt)
            End If

            ' Check we have sone results
            If Informations.IsArray(vResultArray) Then

                r_sMediaTypeIssuerConnectorCode = CStr(vResultArray(0, 0))

                r_cAmount = CDec(vResultArray(1, 0))

                r_sCCNumber = CStr(vResultArray(2, 0))

                r_sCCNameOnCard = CStr(vResultArray(3, 0))

                r_sCCExpiryDate = CStr(vResultArray(4, 0))

                r_sCCStartDate = CStr(vResultArray(5, 0))

                r_sCCIssue = CStr(vResultArray(6, 0))

                r_sCCPIN = CStr(vResultArray(7, 0))

                r_sAddress1 = CStr(vResultArray(8, 0))

                r_sPostcode = CStr(vResultArray(9, 0))

                r_sCCCustomerFlag = CStr(vResultArray(10, 0))

                r_lCashlistitemID = CInt(vResultArray(11, 0))

                r_lCashlistID = CInt(vResultArray(12, 0))

                r_sPartyShortName = CStr(vResultArray(13, 0))

                r_sPolicyNo = CStr(vResultArray(14, 0))

                r_sPartyType = CStr(vResultArray(15, 0))

                r_sPartyResolvedName = CStr(vResultArray(16, 0))
            End If

        End With

        Return result
    End Function

    ' ***************************************************************** '
    ' Name          : AddTaskToWorkManager
    ' Desc          : Adds a task to work manager. Copied from other
    '                 components. There appears to be no standard way
    '                 of doing this!
    ' Edit History  :
    ' CJB20050125   : Created
    ' ***************************************************************** '
    Public Function AddTaskToWorkManager(ByVal v_sDescription As String, ByVal v_dtTaskDueDate As Date, ByVal v_sPartyShortName As String, Optional ByVal v_lPMWrkTaskID As Integer = 0, Optional ByVal v_sTaskCode As String = "", Optional ByVal v_lPMWrkTaskGroupID As Integer = 0, Optional ByVal v_sTaskGroupCode As String = "", Optional ByVal v_iUserID As Integer = 0, Optional ByVal v_sUserGroupCode As String = "PURCLDGR", Optional ByVal v_vKeyArray As Object = Nothing, Optional ByVal v_iIsUrgent As Integer = 0, Optional ByVal v_iIsVisible As Integer = gPMConstants.PMEReturnCode.PMTrue) As Integer

        Dim result As Integer = 0
        Dim oWrkTaskInstance As Object = Nothing
        Dim sSQL As String = ""
        Dim vResultArray(,) As Object = Nothing
        Dim lUserGroupID, lPMWrkTaskInstanceCnt As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            'Check to see if we have task_id or task code
            If v_lPMWrkTaskID = 0 And v_sTaskCode = "" Then
                gPMFunctions.RaiseError("AddTaskToWorkManager", "Must supply either task group id or task group code")
            End If

            'Check to see if we have task_group_id or task group code
            If v_lPMWrkTaskGroupID = 0 And v_sTaskGroupCode = "" Then
                gPMFunctions.RaiseError("AddTaskToWorkManager", "Must supply either task id or task code")
            End If

            oWrkTaskInstance = New bPMWrkTaskInstance.TaskControl

            If oWrkTaskInstance.Initialise(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp), vDatabase:=CType(m_oDatabase, dPMDAO.Database)) <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SelectCardDataForAuthorisation", "gPMComponentServices.CreateBusinessObject failed")
            End If

            '*******************************************************
            'get user group id
            '*******************************************************
            m_oDatabase.Parameters.Clear()

            If v_sUserGroupCode <> "" Then
                sSQL = "SELECT pmuser_group_id FROM pmuser_group WHERE code = {group_code}"

                ' Add the user_id parameter
                If m_oDatabase.Parameters.Add("group_code", vValue:=v_sUserGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.Parameters.Add failed for group_code")
                End If

            Else
                sSQL = "SELECT pmuser_group_id FROM pmuser_group_user WHERE user_id = {user_id}"

                ' Add the user_id parameter
                If m_oDatabase.Parameters.Add("user_id", vValue:=CStr(m_iUserID), iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMInteger) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.Parameters.Add failed for user_id")
                End If

            End If

            If m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetGroupIDs", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.SQLSelect failed for " & sSQL)
            End If

            'do we have any data
            If Not Informations.IsArray(vResultArray) Then
                gPMFunctions.RaiseError("AddTaskToWorkManager", "No data from " & sSQL)
            End If

            ' Get the user_group_id

            lUserGroupID = CInt(vResultArray(0, 0))

            '*******************************************************
            ' Get the task_id
            '*******************************************************
            If v_sTaskCode <> "" Then
                sSQL = "SELECT pmwrk_task_id FROM PMWrk_Task WHERE code = {task_code}"

                m_oDatabase.Parameters.Clear()

                If m_oDatabase.Parameters.Add("task_code", vValue:=v_sTaskCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.Parameters.Add failed for task_code")
                End If

                If m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetTaskID", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.SQLSelect failed for " & sSQL)
                End If

                If Not Informations.IsArray(vResultArray) Then
                    gPMFunctions.RaiseError("AddTaskToWorkManager", "No data from " & sSQL)
                End If


                v_lPMWrkTaskID = CInt(vResultArray(0, 0))
            End If

            '*******************************************************
            'get task group id
            '*******************************************************
            If v_sTaskGroupCode <> "" Then
                sSQL = "SELECT pmwrk_task_group_id FROM PMWrk_Task_group WHERE code = {task_group_code}"

                m_oDatabase.Parameters.Clear()

                If m_oDatabase.Parameters.Add("task_group_code", vValue:=v_sTaskGroupCode, iDirection:=gPMConstants.PMEParameterDirection.PMParamInput, iDataType:=gPMConstants.PMEDataType.PMString) <> gPMConstants.PMEReturnCode.PMTrue Then

                    gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.Parameters.Add failed for task_group_code")
                End If

                If m_oDatabase.SQLSelect(sSQL:=sSQL, sSQLName:="GetGroupTaskID", bStoredProcedure:=False, vResultArray:=vResultArray) <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("AddTaskToWorkManager", "m_oDatabase.SQLSelect failed for " & sSQL)
                End If

                If Not Informations.IsArray(vResultArray) Then
                    gPMFunctions.RaiseError("AddTaskToWorkManager", "No data from " & sSQL)
                End If


                v_lPMWrkTaskGroupID = CInt(vResultArray(0, 0))
            End If

            '*******************************************************
            'create task
            '*******************************************************


            If oWrkTaskInstance.CreateNew(v_lPMWrkTaskGroupID:=ToSafeInteger(v_lPMWrkTaskGroupID), v_lPMWrkTaskID:=ToSafeInteger(v_lPMWrkTaskID), v_sCustomer:=ToSafeString(v_sPartyShortName), v_dtTaskDueDate:=ToSafeDate(v_dtTaskDueDate), v_lPMUserGroupID:=ToSafeInteger(lUserGroupID), v_sDescription:=ToSafeString(v_sDescription), v_iTaskStatus:=gPMConstants.PMEWrkManTaskStatus.pmeWMTSNew, v_iIsUrgent:=ToSafeInteger(v_iIsUrgent), r_lPMWrkTaskInstanceCnt:=ToSafeInteger(lPMWrkTaskInstanceCnt), v_iUserID:=ToSafeInteger(v_iUserID), v_vKeyArray:=CType(v_vKeyArray, Object)) <> gPMConstants.PMEReturnCode.PMTrue Then

                gPMFunctions.RaiseError("AddTaskToWorkManager", "oWrkTaskInstance.CreateNew failed")
            End If


            oWrkTaskInstance.Dispose()


        Catch ex As Exception
            bPMFunc.LogMessage(m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AddTaskToWorkManager Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AddTaskToWorkManager", vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
            result = gPMConstants.PMEReturnCode.PMFalse
        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: PrintLinkedDocuments
    '
    ' Description:  Prints the linked documents for a specified process type
    '
    ' History: CLG 20050209
    '
    ' ***************************************************************** '
    'Modified by Sudhanshu Behera on 5/18/2010 7:00:44 PM refer developer guide no. 108 (guide)
    'Private Function PrintLinkedDocuments(ByVal v_sProcessType As String, ByVal v_sFunctionName As String, ByVal v_lGISSchemeId As Integer, ByVal v_lAgentCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_oDocLink As Object, ByRef r_oDocTemplate As Object, ByRef r_oDocMgrWrapper As bSIRDocManagerWrapper.Interface, ByRef r_oTaskInstance As Object) As gPMConstants.PMEReturnCode
    Private Function PrintLinkedDocuments(ByVal v_sProcessType As String, ByVal v_sFunctionName As String, ByVal v_lGISSchemeId As Integer, ByVal v_lAgentCnt As Integer, ByVal v_lInsuranceFolderCnt As Integer, ByVal v_lRenewalInsuranceFileCnt As Integer, ByVal v_lPartyCnt As Integer, ByRef r_oDocLink As Object, ByRef r_oDocTemplate As Object, ByRef r_oDocMgrWrapper As Object, ByRef r_oTaskInstance As Object) As Integer


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Const LCACArrayDocumentTemplateId As Integer = 0
        Const LCACArrayDocumentTypeId As Integer = 1
        Const LCACArraySpoolDocument As Integer = 2
        Const LCACArrayAutoArchiveDocument As Integer = 3

        ' Modes for printing
        'Const ACPrintMode As Integer = 2
        Const ACPrintSilentMode As Integer = 3
        Const ACSpoolDocMode As Integer = 4
        'Const ACSpoolReportMode As Integer = 5

        Dim sMsg As String = ""
        Dim vDocumentArray(,) As Object = Nothing


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' find the documents for this scheme

            m_lReturn = r_oDocLink.GetDocTemplate(v_lSchemeID:=ToSafeInteger(v_lGISSchemeId), v_lAgentCnt:=ToSafeInteger(v_lAgentCnt), v_sProcessTypeCode:=ToSafeString(v_sProcessType), v_vDocumentArray:=CType(vDocumentArray, Object))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                sMsg = "Failed to process oDocLink.GetDocTemplate()."
                Throw New Exception(sMsg)
            End If

            If Informations.IsArray(vDocumentArray) Then
                ' documents found
                With r_oDocMgrWrapper

                    For iNum As Integer = 0 To vDocumentArray.GetUpperBound(1)
                        .PartyCnt = v_lPartyCnt
                        .InsuranceFolderCnt = v_lInsuranceFolderCnt
                        .InsuranceFileCnt = v_lRenewalInsuranceFileCnt

                        .DocumentTemplateId = CInt(vDocumentArray(LCACArrayDocumentTemplateId, iNum))

                        .DocumentTypeId = CInt(vDocumentArray(LCACArrayDocumentTypeId, iNum))

                        If CDbl(vDocumentArray(LCACArraySpoolDocument, iNum)) = 1 Then
                            .Mode = ACSpoolDocMode
                        Else
                            .Mode = ACPrintSilentMode
                        End If

                        .ArchiveDoc = CDbl(vDocumentArray(LCACArrayAutoArchiveDocument, iNum)) = 1

                        m_lReturn = .Start()
                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            sMsg = "Failed to Print Document"
                            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=sMsg)

                        End If

                        ' schedule the task(s) for this document

                        m_lReturn = CreateDocumentTask(oDocTemplate:=r_oDocTemplate, oTaskInstance:=r_oTaskInstance, lDocumentTemplateId:=CInt(vDocumentArray(LCACArrayDocumentTemplateId, iNum)), lInsuranceFileCnt:=v_lRenewalInsuranceFileCnt, lPartyCnt:=v_lPartyCnt)

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            'TODOLIST
                            'sMsg = "Failed to create a task for the document " & .DocumentTemplateId
                            m_lReturn = CreateTaskLog(v_lInsuranceFolderCnt:=v_lInsuranceFolderCnt, v_lProcessID:=AC_CompleteID, v_sProcessCode:=AC_Complete, v_lStatus:=gPMConstants.PMEReturnCode.PMFail, v_sMessage:=sMsg)

                        End If
                    Next

                End With
            End If

        Catch ex As Exception
            ' Log Error Message
            bPMFunc.LogMessage(sUsername:=m_sUsername, iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_sFunctionName, vErrNo:=Informations.Err().Number, vErrDesc:=Informations.Err().Description, excep:=ex)
        Finally
        End Try
        Return result

    End Function



    ' ***************************************************************** '
    ' Name: PrintLinkedDocumentsStart
    '
    ' Description:  Initialises the objects required by PrintLinkedDocuments
    '
    ' History: CLG 20050209
    '
    ' ***************************************************************** '
    'Modified by Sudhanshu Behera on 5/18/2010 7:01:21 PM refer developer guide no. 108 (guide)
    'Private Function PrintLinkedDocumentsStart(ByRef r_oDocLink As Object, ByRef r_oDocTemplate As Object, ByRef r_oDocMgrWrapper As bSIRDocManagerWrapper.Interface, ByRef r_oTaskInstance As Object, ByRef r_sMessage As String) As gPMConstants.PMEReturnCode
    Private Function PrintLinkedDocumentsStart(ByRef r_oDocLink As bPMBDocLink.Business, ByRef r_oDocTemplate As bSIRDocTemplate.Business, ByRef r_oDocMgrWrapper As Object, ByRef r_oTaskInstance As bPMWrkTaskInstanceTemp.FormClass, ByRef r_sMessage As String) As Integer
        'Private r_oDocLink As bPMBDocLink.Business
        'Private r_oDocTemplate As bSIRDocTemplate.Business
        'Private r_oTaskInstance As bPMWrkTaskInstanceTemp.FormClass


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            r_oDocLink = New bPMBDocLink.Business
            m_lReturn = r_oDocLink.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create instance of bPMBDocLink.Business."
                Throw New Exception(r_sMessage)
            End If

            ' can't use the same method as above because we need to
            ' use the business initialisation routine
            'Modified by Sudhanshu Behera on 5/18/2010 7:01:53 PM refer developer guide no. 108 (guide)
            'r_oDocMgrWrapper = New bSIRDocManagerWrapper.Interface()
            'r_oDocMgrWrapper = gPMComponentServices.CreateObject("bSIRDocManagerWrapper.Interface_Renamed")
            r_oDocMgrWrapper = gPMComponentServices.CreateBusinessObject(r_oObject:=m_oTaskLog, v_sClassName:="bSIRDocManagerWrapper.Interface_Renamed", v_sCallingAppName:=ACApp, v_sUsername:=m_sUsername, v_sPassword:=m_sPassword, v_iUserID:=m_iUserID, v_iSourceID:=m_iSourceID, v_iLanguageID:=m_iLanguageID, v_iCurrencyID:=m_iCurrencyID, v_iLogLevel:=m_iLogLevel, v_oDatabase:=m_oDatabase)
            m_lReturn = r_oDocMgrWrapper.InitialiseBusiness(sUserName:=ToSafeString(m_sUsername), sPassword:=ToSafeString(m_sPassword), iUserID:=ToSafeInteger(m_iUserID), iSourceID:=ToSafeInteger(m_iSourceID), iLanguageID:=ToSafeInteger(m_iLanguageID), iCurrencyID:=ToSafeInteger(m_iCurrencyID), iLogLevel:=ToSafeInteger(m_iLogLevel), sCallingAppName:=ToSafeString(ACApp))

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create instance of bSIRDocManagerWrapper.Interface."
                Throw New Exception(r_sMessage)
            End If


            r_oDocTemplate = New bSIRDocTemplate.Business
            m_lReturn = r_oDocTemplate.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create instance of bSIRDocTemplate.Business."
                Throw New Exception(r_sMessage)
            End If


            r_oTaskInstance = New bPMWrkTaskInstanceTemp.FormClass
            m_lReturn = r_oTaskInstance.Initialise(sUsername:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sMessage = "Failed to create instance of bPMWrkTaskInstanceTemp.FormClass."
                Throw New Exception(r_sMessage)
            End If

        Catch ex As Exception

            'clear down objects
            PrintLinkedDocumentsEnd(r_oDocLink, r_oDocTemplate, r_oDocMgrWrapper, r_oTaskInstance)
            'error handled in calling function

        Finally
        End Try
        Return result

    End Function

    ' ***************************************************************** '
    ' Name: PrintLinkedDocumentsEnd
    '
    ' Description:  Clears the objects required by PrintLinkedDocuments
    '
    ' History: CLG 20050209
    '
    ' ***************************************************************** '
    'Modified by Sudhanshu Behera on 5/18/2010 7:02:22 PM refer developer guide no. 108 (guide)
    'Private Function PrintLinkedDocumentsEnd(ByRef r_oDocLink As Object, ByRef r_oDocTemplate As Object, ByRef r_oDocMgrWrapper As bSIRDocManagerWrapper.Interface, ByRef r_oTaskInstance As Object) As gPMConstants.PMEReturnCode
    Private Function PrintLinkedDocumentsEnd(ByRef r_oDocLink As Object, ByRef r_oDocTemplate As Object, ByRef r_oDocMgrWrapper As Object, ByRef r_oTaskInstance As Object) As Integer


        Dim result As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMFalse



        result = gPMConstants.PMEReturnCode.PMTrue

        If Not (r_oDocLink Is Nothing) Then

            r_oDocLink.Dispose()
            r_oDocLink = Nothing
        End If

        If Not (r_oDocTemplate Is Nothing) Then

            r_oDocTemplate.Dispose()
            r_oDocTemplate = Nothing
        End If
        If Not (r_oDocMgrWrapper Is Nothing) Then
            r_oDocMgrWrapper.Dispose()
            r_oDocMgrWrapper = Nothing
        End If
        If Not (r_oTaskInstance Is Nothing) Then

            r_oTaskInstance.Dispose()
            r_oTaskInstance = Nothing
        End If

        Return result

    End Function

    Private Function AddPremiumFinancePlan(ByVal v_lRenewalInsuranceFileCnt As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddPremiumFinancePlan"

        Dim oPremiumFinance As New bSIRPremiumFinance.Business

        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            oPremiumFinance = New bSIRPremiumFinance.Business
            m_lReturn = oPremiumFinance.Initialise(sUserName:=m_sUsername, sPassword:=m_sPassword, iUserID:=m_iUserID, iSourceID:=m_iSourceID, iLanguageID:=m_iLanguageID, iCurrencyID:=m_iCurrencyID, iLogLevel:=m_iLogLevel, sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("gPMComponentServices.CreateBusinessObject", "v_sClassName:=bSirPremiumFinance.Business", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = oPremiumFinance.GenerateRenewalQuote(lRenewalInsuranceFileCnt:=v_lRenewalInsuranceFileCnt)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPremiumFinance.GenerateRenewalQuote", "lRenewalInsuranceFileCnt:=" & v_lRenewalInsuranceFileCnt, gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            bPMFunc.LogError(v_sUsername:=m_sUsername, v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Do any tidy up, e.g. Set x = Nothing here
            If Not (oPremiumFinance Is Nothing) Then

                oPremiumFinance.Dispose()
                oPremiumFinance = Nothing
            End If

        End Try

        Return result
    End Function

    Private Shared _DefaultInstance As Renewals = Nothing
    Public Shared ReadOnly Property DefaultInstance() As Renewals
        Get
            If _DefaultInstance Is Nothing Then
                _DefaultInstance = New Renewals
            End If
            Return _DefaultInstance
        End Get
    End Property
End Class
