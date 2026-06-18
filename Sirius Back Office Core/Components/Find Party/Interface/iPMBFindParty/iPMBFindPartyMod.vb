Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 17/02/1997
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:
    ' SP130199 - Remove NavigatorV3 class an put in stub so can be called
    ' iteratively.
    ' ***************************************************************** '


    ' Main public constant for all functions
    ' to identify which application this is.
    Public Const ACApp As String = "iPMBFindParty"
    'ECK 18/5/99
    Public Const ThisApp As String = "Client Manager" ' Registry App constant.
    Public Const ThisKey As String = "Recent Files" ' Registry Key constant.

    ' Public interface constants used when
    ' retrieving data from the resource file.

    ' {* USER DEFINED CODE (Begin) *}

    ' General Icons

    ' Form

    ' Alix
    Public Const ACMultAddFormName As Integer = 312
    Public Const ACMultAddLabel As Integer = 313
    Public Const ACMultAddHouse As Integer = 314
    Public Const ACMultAddStreet As Integer = 315
    Public Const ACMultAddSuburb As Integer = 316
    Public Const ACMultAddCity As Integer = 317
    Public Const ACMultAddPostcode As Integer = 123

    Public Const ACInterfaceTitle As Integer = 100
    Public Const ACTabTitle1 As Integer = 101
    Public Const ACTabTitle2 As Integer = 102
    Public Const ACTabTitle3 As Integer = 103

    Public Const ACShortName As Integer = 104
    Public Const ACLongName As Integer = 105
    Public Const ACType As Integer = 106
    Public Const ACStatus As Integer = 107
    Public Const ACAddress1 As Integer = 108
    Public Const ACPostalCode As Integer = 109
    Public Const ACTelephone As Integer = 110
    Public Const ACInsReference As Integer = 111
    'RKS 141004 PN13238 & PN14838
    Public Const ACIncludeClosedBranches As Integer = 112
    'DM PYV
    Public Const ACInsurerType As Integer = 313

    Public Const ACListTitle1 As Integer = 120
    Public Const ACListTitle2 As Integer = 121
    Public Const ACListTitle3 As Integer = 122
    Public Const ACListTitle4 As Integer = 123
    Public Const ACListTitle5 As Integer = 124
    Public Const ACListTitleAON5 As Integer = 156
    Public Const ACListTitle6 As Integer = 125
    Public Const ACListTitle7 As Integer = 126
    Public Const ACListTitle8 As Integer = 127
    Public Const ACListTitleAddressLine2 As Integer = 155
    Public Const ACListTitleDateOfBirth As Integer = 153
    Public Const ACListTitleSwiftLink As Integer = 154
    'TF181000
    Public Const ACFindFinanceProviderTitle As Integer = 128
    'TN20000918
    Public Const ACFindReinsurerTitle As Integer = 129
    Public Const ACFindAgentTitle As Integer = 130
    Public Const ACFindConsultantTitle As Integer = 131
    Public Const ACFindAccountHandlerTitle As Integer = 132
    Public Const ACFindInsurerTitle As Integer = 133
    Public Const ACFindBrokerTitle As Integer = 134
    Public Const ACFindFeeTitle As Integer = 135
    Public Const ACFindExtraTitle As Integer = 136
    Public Const ACFindDiscountTitle As Integer = 137
    Public Const ACFindCommissionAccountTitle As Integer = 138
    Public Const ACFindIntermediaryTitle As Integer = 162

    'TF181000
    Public Const ACListFinanceProviderTitle As Integer = 139
    Public Const ACListAgentTitle As Integer = 140
    Public Const ACListConsultantTitle As Integer = 141
    Public Const ACListAccountHandlerTitle As Integer = 142
    Public Const ACListInsurerTitle As Integer = 143
    Public Const ACListBrokerTitle As Integer = 144
    Public Const ACListFeeTitle As Integer = 145
    Public Const ACListExtraTitle As Integer = 146
    Public Const ACListDiscountTitle As Integer = 147
    Public Const ACListCommissionAccountTitle As Integer = 148
    Public Const ACListIntermediaryTitle As Integer = 163

    'TN20000918
    Public Const ACListReinsurerTitle As Integer = 149

    Public Const ACFindOtherPartyTitle As Integer = 150
    Public Const ACOtherPartyCode As Integer = 151
    Public Const ACOtherPartyType As Integer = 152

    'CMG/PB 18072002
    Public Const ACFindAgentGroupTitle As Integer = 157
    Public Const ACListAgentGroupTitle As Integer = 158
    Public Const ACListBranchTitle As Integer = 159
    'DC101204
    Public Const ACListThirdPartyTitle As Integer = 160
    Public Const ACFindThirdPartyTitle As Integer = 161

    Public Const ACListTitleSurname As Integer = 164
    Public Const ACListTitleForename As Integer = 165
    Public Const ACListTitleAddressLine3 As Integer = 166
    Public Const ACListTitleAddressLine4 As Integer = 167

    ' Buttons
    Public Const ACOKButton As Integer = 200
    Public Const ACCancelButton As Integer = 201
    Public Const ACHelpButton As Integer = 202
    Public Const ACNavigateButton As Integer = 203

    Public Const ACFindNowButton As Integer = 204
    Public Const ACNewSearchButton As Integer = 205
    Public Const ACNewButton As Integer = 206
    Public Const ACEditButton As Integer = 207

    Public Const ACDeleteButton As Integer = 208
    Public Const ACUndeleteButton As Integer = 209

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303

    Public Const ACClearDetailsTitle As Integer = 304
    Public Const ACClearDetails As Integer = 305
    Public Const ACStatusSearching As Integer = 306
    Public Const ACStatusFound As Integer = 307

    Public Const ACNoAgencyAgreementTitle As Integer = 308 'TF030399
    Public Const ACNoAgencyAgreement As Integer = 309

    Public Const ACRemoveClientTitle As Integer = 310 'CJB200802
    Public Const ACRemoveClient As Integer = 311 'CJB200802
    Public Const ACAmendClientDetails As Integer = 312

    ' Menus

    'Constants for List View Column Headers
    Public Const g_kLvwColumnClientCode As Integer = 1
    Public Const g_kLvwColumnName As Integer = 2
    Public Const g_kLvwColumnSurname As Integer = 3
    Public Const g_kLvwColumnForename As Integer = 4
    Public Const g_kLvwColumnAddressLine1 As Integer = 5
    Public Const g_kLvwColumnAddressLine2 As Integer = 6
    Public Const g_kLvwColumnAddressLine3 As Integer = 7
    Public Const g_kLvwColumnAddressLine4 As Integer = 8
    Public Const g_kLvwColumnPostCode As Integer = 9
    Public Const g_kLvwColumnFileCode As Integer = 10
    Public Const g_kLvwColumnType As Integer = 11
    Public Const g_kLvwColumnStatus As Integer = 12
    Public Const g_kLvwColumnSource As Integer = 13
    Public Const g_kLvwColumnDateOfBirth As Integer = 14
    Public Const g_kLvwColumnSwiftLink As Integer = 15
    Public Const g_kLvwColumnActiveStatus As Integer = 16
    Public Const g_kLvwColumnBranch As Integer = 17


    ' Constants for the search data array indexes.
    Public Const ACIPartyCnt As Integer = 0
    Public Const ACIPartyType As Integer = 1
    Public Const ACIShortName As Integer = 2
    Public Const ACILongName As Integer = 3
    Public Const ACIAddress1 As Integer = 4
    Public Const ACIPostalCode As Integer = 5
    Public Const ACISourceID As Integer = 6
    Public Const ACIPartyID As Integer = 7
    Public Const ACITelAreaCode As Integer = 8
    Public Const ACITelNumber As Integer = 9
    Public Const ACIPartyStatus As Integer = 10

    Public Const ACIInvariantKey As Integer = 11
    Public Const ACISource As Integer = 12

    Public Const ACIResolvedName As Integer = 13
    Public Const ACIAgentType As Integer = 14

    ' - Constant for File Code
    Public Const ACIFileCode As Integer = 15

    Public Const ACIDOB As Integer = 16
    Public Const ACISwiftPartyID As Integer = 17
    Public Const ACIAddress2 As Integer = 18

    Public Const ACISourceName As Integer = 20
    Public Const ACIAgentCnt As Integer = 21
    Public Const ACIrecord_status As Integer = 22
    Public Const ACIPartyTypeCode As Integer = 23
    Public Const ACIPartyTypeCode_SpecialParty As Integer = 23
    Public Const ACIPartyDateCancelled As Integer = 24
    'TMP Added one Constant
    Public Const ACIAllowConsolidatedCommission As Integer = 25
    'sj 12/11/2002 - end
    Public Const ACIPartyDateCancelled_SpecialParty As Integer = 24

    Public Const ACIOnlineAccess As Integer = 24
    Public Const ACIIsRiBroker As Integer = 26
    Public Const ACIMax As Integer = 27

    Public Const ACISurname As Integer = 26
    Public Const ACIForename As Integer = 27
    Public Const ACIAddress3 As Integer = 28
    Public Const ACIAddress4 As Integer = 29
    Public Const ACICurrencyID As Integer = 30
    Public Const ACIPartyServiceLevelCode As Integer = 33
    Public Const ACIPartyServiceLevelDescription As Integer = 34
    'CMG/PB 19072002
    Public Const ACIBranch As Integer = 19
    Public Const ACIActiveStatus As Integer = 20
    ' CMG end

    Public Const PMSearchSirius As Integer = 0
    Public Const PMSearchPMB As Integer = 1
    Public Const PMSearchSiriusPMB As Integer = 2

    ' {* USER DEFINED CODE (End) *}

    ' Public contants used for the start
    ' and end control indexes.
    Public Const ACControlStart As Integer = 0
    Public Const ACControlEnd As Integer = 1

    ' Constant for the maxiumum search details.
    Public Const ACMaxSearchDetails As Integer = 500

    ' Constant for the miniumum search length.
    'Modified by ECK 11/05/99
    Public Const ACMinSearchLength As Integer = 1

    ' Constant for the functions to identify
    ' which class this is.
    Private Const ACClass As String = "MainModule"

    ' Public source and language ID's from the
    ' Object Manager.
    <ThreadStatic()> _
    Public g_iSourceID As Integer
    <ThreadStatic()> _
    Public g_iLanguageID As Integer
    'eck220500
    <ThreadStatic()> _
    Public g_iUserID As Integer
    ' Public instance of the object manager.
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    ' Public instance of the business object.
    <ThreadStatic()> _
    Public g_oBusiness As Object
    'eck160500
    <ThreadStatic()> _
    Public g_iRefCounter As Integer

    'CMG/PB Risk Index searching bug 315

    <ThreadStatic()> _
    Public g_oClaimBusiness As bCLMFindClaim.Business
    ' Public instance of the back office business object.

    <ThreadStatic()> _
    Public g_oBackofficelink As bBackOfficeLink.bBOLink
    'End CMG


    <ThreadStatic()> _
    Public g_oPMUser As bPMUser.Business

    Public g_sProductFamily As gPMConstants.PMEProductFamily = gPMConstants.PMEProductFamily.pmePFSiriusSolutions
    <ThreadStatic()> _
    Public g_oUserAuthorities As Object '2005 Client Security

    'Public Const ScreenHelpID As Integer = 1
    Public Const ScreenHelpID As Integer = 5001

    Public Const UWScreenHelpID As Integer = 4001


    <ThreadStatic()> _
    Public g_bGenericConnectionStatus As Boolean

    Public Const ACNowtSelected As Integer = 0
    Public Const ACNewPasswordEntered As Integer = 1
    Public Const ACPasswordOK As Integer = 2
    Public Const ACVoiceRecognised As Integer = 3
    Public Const ACTelephoneNumberRecognised As Integer = 4
    Public Const ACNoPasswordEntered As Integer = 5

    Public Const kSystemOptionClientBlacklistingInForce As Integer = 5011

    'PN38955 (RC)
    Public Const kSystemOptionEnhanceFilterscreens As Integer = 5043

    Public Const kUSLangId As Integer = 2
    Public Const kUKLangId As Integer = 1

    Sub Main_Renamed()

        Dim vKeyArray(,) As Object
        Dim sClientCode As String = String.Empty
        Dim sClientName As String = String.Empty
        Dim sAddress1 As String = String.Empty
        Dim sAddress2 As String = String.Empty
        Dim sAddress3 As String = String.Empty
        Dim sAddress4 As String = String.Empty
        Dim sPostcode As String = String.Empty
        Dim sPortfolio As String = String.Empty
        Dim sCustomerId As String = String.Empty
        'developer guide no. 88(Guide)
        Dim oFindParty As New Interface_Renamed

        Dim lError As gPMConstants.PMEReturnCode = CType(CType(oFindParty, SSP.S4I.Interfaces.ILocalInterface).Initialise(), gPMConstants.PMEReturnCode)

        oFindParty.CallingAppName = "TEST"

        lError = CType(oFindParty.SetProcessModes(vNavigate:=gPMConstants.PMENavigateButtonStatus.PMNavigateDisabled, vProcessMode:=gPMConstants.PMEProcessMode.PMProcessModeGeneric, vTransactionType:="", vEffectiveDate:=DateTime.Now), gPMConstants.PMEReturnCode)

        ReDim vKeyArray(1, 0)

        'oFindParty.SpecialParty = PMBPartyTypeConsultant

        lError = CType(oFindParty.Start(), gPMConstants.PMEReturnCode)

        If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

            lError = CType(oFindParty.GetKeys(vKeyArray), gPMConstants.PMEReturnCode)



            MessageBox.Show("SELECTED: " & CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 0)) & ", " & _
                   CStr(vKeyArray(gPMConstants.PMENavLetGetKeyColPosition.PMKeyValue, 2)), Application.ProductName)

        End If

        Dim lPartyCnt As Integer = oFindParty.PartyCnt
        Dim lInvariantKey As Integer = oFindParty.InvariantKey

        lError = CType(oFindParty.GetDetailsFromPMBArray(v_lInvariantKey:=lInvariantKey, r_sClientCode:=sClientCode, r_sClientName:=sClientName, r_sAddress1:=sAddress1, r_sAddress2:=sAddress2, r_sAddress3:=sAddress3, r_sAddress4:=sAddress4, r_sPostCode:=sPostcode, r_sPortfolio:=sPortfolio, r_sCustomerId:=sCustomerId), gPMConstants.PMEReturnCode)

        oFindParty.Dispose()


    End Sub

    'DC120204 PN9436 added for ClientManager to use FSA verification (recentfiles)
    '                   was previously in frmInterface
    Public Function FSACustomerValidate(ByVal lPartyCnt As Integer, ByVal sPartyType As String, ByVal iIsComplaint As Integer, ByRef r_bProceed As Boolean) As Integer
        ' ---------------------------------------------------------------------------
        ' PROCEDURE NAME: FSACustomerValidate
        ' PURPOSE: Validates the Customer by verifying why a Party is being
        '          queried.
        ' AUTHOR: Danny Davis
        ' DATE: 29 October 2003, 13:23:14
        ' RETURNS: PMTrue for success
        ' CHANGES:
        ' ---------------------------------------------------------------------------

        Dim result As Integer = 0
        'Dim frm As frmSecurityReason
        'developer guide no.69
        Dim frmSecReason As frmSecurityReason
        Dim frmSecQuest As frmSecurityQuestion
        Dim bQuestion, bLogged As Boolean
        Dim sUserReason, sSecurityReason As String
        Dim sPassword As String = String.Empty
        Dim vPostcode As String = ""
        Dim vValue As String = ""
        Dim lEventCnt, lError As Integer
        Dim sEnteredPassword As String = ""
        Dim iStatus As Integer


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lError = iPMFunc.getProductOptionValue(gPMConstants.SIRHiddenOptions.SIROPTEnableFSACompliance, CStr(1), vValue)
            If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            'Assume true
            r_bProceed = True

            If gPMFunctions.NullToString(vValue) = "1" And (sPartyType = PMBConst.PMBPartyTypePersonalClient Or sPartyType = PMBConst.PMBPartyTypeCorporateClient) Then

                'Get the reason code for querying the Party
                frmSecReason = New frmSecurityReason()
                'MKW080104 PN9424 Include Complaint in FSA reasons Pass setting
                'DC090103 PN now parameter passed to function
                frmSecReason.IsComplaint = iIsComplaint
                frmSecReason.ShowDialog()
                bQuestion = frmSecReason.IsQuestion
                bLogged = frmSecReason.IsLogged
                sSecurityReason = frmSecReason.SecurityReason
                frmSecReason.Close()
                frmSecReason = Nothing

                'See if the reason code requires the customer to answer a security question
                If bQuestion Then
                    'Get the information about the user

                    lError = g_oBusiness.GetFSAPartyQuestion(lPartyCnt:=lPartyCnt, sPartyType:=sPartyType, r_sPassword:=sPassword, r_vPostCode:=vPostcode)
                    If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get FSA Question information.", vApp:=ACApp, vClass:=ACClass, vMethod:="FSACustomerValidate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                    'Now ask the security question
                    'developer guide no.69
                    frmSecQuest = New frmSecurityQuestion()

                    frmSecQuest.PartyCnt = lPartyCnt

                    frmSecQuest.PartyType = sPartyType

                    frmSecQuest.Password = sPassword

                    frmSecQuest.Postcode = gPMFunctions.NullToString(vPostcode)


                    frmSecQuest.ShowDialog()

                    'Get the response

                    r_bProceed = frmSecQuest.Proceed

                    sEnteredPassword = frmSecQuest.Password

                    iStatus = frmSecQuest.Status

                    'and tidy up
                    frmSecReason.Close()
                    frmSecReason = Nothing

                    If r_bProceed Then
                        'DJM 20/01/2004 : Set event message depending on circumstances.
                        ' SET 27/05/2004 ISS11879
                        Select Case iStatus
                            Case ACNewPasswordEntered
                                sUserReason = " - The security question was set up with '" & sEnteredPassword.Trim() & "'."
                            Case ACNoPasswordEntered
                                sUserReason = " - The security question was not set up."
                            Case ACPasswordOK
                                sUserReason = " - The caller answered the security question correctly with '" & sEnteredPassword.Trim() & "'."
                            Case ACVoiceRecognised
                                sUserReason = " - The operator selected the option indicating recognition of the caller's voice."
                            Case ACTelephoneNumberRecognised
                                sUserReason = " - The operator selected the option indicating recognition of the caller's number."
                            Case Else
                                sUserReason = " - The caller did not answer the security question."
                        End Select

                        'Create the Event entry


                        lError = g_oBusiness.CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=DBNull.Value, v_vInsuranceFileCnt:=DBNull.Value, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventFSANotes, v_dtEventDate:=DateTime.Now, v_vDescription:=sSecurityReason & sUserReason)

                        If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                            ' Log Error.
                            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the Event.", vApp:=ACApp, vClass:=ACClass, vMethod:="FSACustomerValidate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                        End If
                    End If
                End If

                'See if the reason code requires the user to log the reason
                If bLogged Then
                    'Now ask the security question
                    ''developer guide no. 51
                    Dim objFrmAccessReason As frmAccessReason = New frmAccessReason()
                    objFrmAccessReason.ShowDialog()


                    sUserReason = objFrmAccessReason.Reason
                    If sUserReason <> "" Then
                        sUserReason = " - " & sUserReason
                    End If

                    'and tidy up
                    objFrmAccessReason.Close()
                    objFrmAccessReason = Nothing

                    'Create the Event entry


                    lError = g_oBusiness.CreateEvent(r_lEventCnt:=lEventCnt, v_lPartyCnt:=lPartyCnt, v_vInsuranceFolderCnt:=DBNull.Value, v_vInsuranceFileCnt:=DBNull.Value, v_vClaimCnt:=DBNull.Value, v_vDocumentCnt:=DBNull.Value, v_vOldAddressCnt:=DBNull.Value, v_vNewAddressCnt:=DBNull.Value, v_vCampaignId:=DBNull.Value, v_vDocumentTypeId:=DBNull.Value, v_vReportTypeId:=DBNull.Value, v_lEventTypeId:=PMBConst.PMBEventFSANotes, v_dtEventDate:=DateTime.Now, v_vDescription:=sSecurityReason & sUserReason)

                    If lError <> gPMConstants.PMEReturnCode.PMTrue Then
                        ' Log Error.
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create the Event.", vApp:=ACApp, vClass:=ACClass, vMethod:="FSACustomerValidate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    End If

                End If
            End If

            Return result

            '----------------------------------------------------------------------------------------
            'Only for Debugging, the code will never execute this line
            '----------------------------------------------------------------------------------------


        Catch ex As Exception
            Select Case Information.Err().Number
                Case Else
                    ' Log Error.
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="", vApp:=ACApp, vClass:=ACClass, vMethod:="FSACustomerValidate", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description, excep:=ex)

                    result = gPMConstants.PMEReturnCode.PMFalse

                    Return result
            End Select

        Finally

        End Try
        Return result
    End Function

    Public Function GetName(ByVal v_lPartyCnt As Integer, ByRef r_sPartyShortName As String, ByRef r_sPartyResolvedName As String) As Integer
        Dim result As Integer = 0
        Dim oObject As bSIRFindParty.Business
        Dim lReturn As Integer

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If Not IsNothing(g_oObjectManager) Then
                Dim temp_oObject As Object = Nothing
                lReturn = g_oObjectManager.GetInstance(temp_oObject, "bSIRFindParty.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                oObject = temp_oObject

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create an instance of bSIRFindParty.Business", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                    Return result
                End If



                lReturn = oObject.GetName(v_lPartyCnt, r_sPartyShortName)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                lReturn = oObject.GetResolvedName(v_lPartyCnt, r_sPartyResolvedName)

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If


                oObject.Dispose()

                oObject = Nothing
            End If
            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMFalse

            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetName Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetName", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
End Module
