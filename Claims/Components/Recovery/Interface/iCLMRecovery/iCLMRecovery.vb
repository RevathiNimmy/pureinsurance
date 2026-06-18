Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Module MainModule
    ' ***************************************************************** '
    ' Module Name: MainModule
    '
    ' Date: 11/08/2000
    '
    ' Description: Main module containing public variable/constants.
    '
    ' Edit History:Pandu
    ' ***************************************************************** '
    'Developer Guide no. 50
    'developer guide no. 107
    <ThreadStatic()> _
    Public m_ofrmRecovery As frmRecovery

    ' Main public constant for all functions to identify which application this is.
    Public Const ACApp As String = "iCLMSalvageRecovery"

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "MainModule"


    ' Running mode enumerator (Note: Keep in sync with AC constants)
    Public Enum RecoveryModeEnum
        RMSalvageReserve = 1
        RMSalvageReceipt = 2
        RMThirdPartyReserve = 3
        RMThirdPartyReceipt = 4
    End Enum

    ' Public source and language ID's from the Object Manager.
    Public g_iSourceID As Integer
    Public g_iLanguageID As Integer

    'Instance of Bussiness Object
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oBusiness As Object

    ' Public instance of the object manager.
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_oObjectManager As bObjectManager.ObjectManager

    'Get All Recovery Types for Salvage
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_vRecoveryTypes As Object

    ' Global variable for current running mode
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_lRecoveryMode As RecoveryModeEnum

    'S4B Claim Enhancements R&D 2005
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_sUnderwritingOrAgency As String = ""
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_bReceiptMade As Boolean
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_lCashListAccountId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_lCashListPartyId As Integer
    'developer guide no. 107
    <ThreadStatic()> _
    Public g_crCashListAmount As Decimal

    ' Payment posting
    Public Const ACAmountAgainst As String = "Amount Against "
    Public Const ACOptionPostReceiptToSuspense As Integer = 2002

    ' Coinsurance array constants
    Public Const ACCIRecoveryID As Integer = 0
    Public Const ACCIPartyCnt As Integer = 1
    Public Const ACCIDescription As Integer = 2
    Public Const ACCISharePercent As Integer = 3
    Public Const ACCIPaidToDate As Integer = 4
    Public Const ACCIIsTaxShared As Integer = 5
    Public Const ACCIThisPayment As Integer = 6
    Public Const ACCIThisPaymentLoss As Integer = 7
    Public Const ACCITaxAmount As Integer = 8
    Public Const ACCITaxAmountLoss As Integer = 9
    Public Const ACCIMAX As Integer = 9

    ' Reinsurance array constants
    Public Const ACRIRecoveryID As Integer = 0
    Public Const ACRIArrangmentLineID As Integer = 1
    Public Const ACRITreatyID As Integer = 2
    Public Const ACRIFACPartyCnt As Integer = 3
    Public Const ACRIDescription As Integer = 4
    Public Const ACRISharePercent As Integer = 5
    Public Const ACRIPaidToDate As Integer = 6
    Public Const ACRIIsTaxShared As Integer = 7
    Public Const ACRIThisPayment As Integer = 8
    Public Const ACRIThisPaymentLoss As Integer = 9
    Public Const ACRITaxAmount As Integer = 10
    Public Const ACRITaxAmountLoss As Integer = 11
    Public Const ACRIMAX As Integer = 11

    ' Transaction Type constants.
    Public Const ACTransTypeClaimOpen As String = "C_CO"
    Public Const ACTransTypeClaimPay As String = "C_CP"
    Public Const ACTransTypeClaimRevise As String = "C_CR"
    Public Const ACTransTypeClaimView As String = "C_VW"
    Public Const ACTransTypeClaimSalvage As String = "C_SA"
    Public Const ACTransTypeClaimRecovery As String = "C_RV"

    ' Constants for the search data array indexes for perils.
    Public Const ACPAPerilId As Integer = 0
    Public Const ACPAPerilTypeId As Integer = 1
    Public Const ACPAPeril As Integer = 2
    Public Const ACPADescription As Integer = 3
    Public Const ACPACurrencyID As Integer = 4
    Public Const ACPACurrency As Integer = 5
    Public Const ACPACompanyID As Integer = 6
    Public Const ACPACOBID As Integer = 7
    Public Const ACPACOBCode As Integer = 8

    ' Constants for the tax type and band array
    Public Const ACTaxTypeID As Integer = 0
    Public Const ACTaxTypeDescription As Integer = 1
    Public Const ACTaxBandID As Integer = 2
    Public Const ACTaxBandDescription As Integer = 3
    Public Const ACTaxIsValue As Integer = 4
    Public Const ACTaxRate As Integer = 5
    Public Const ACTaxTypeCode As Integer = 6


    ' Public interface constants used when retrieving data from the resource file.
    ' General
    Public Const ACOKButton As Integer = 11
    Public Const ACCancelButton As Integer = 12
    Public Const ACHelpButton As Integer = 13
    Public Const ACAddButton As Integer = 14
    Public Const ACEditButton As Integer = 15
    Public Const ACDeleteButton As Integer = 16

    ' Select Peril Form
    Public Const ACSPFormCaption As Integer = 101
    Public Const ACSPClaimNumber As Integer = 111
    Public Const ACSPColumnPeril As Integer = 120
    Public Const ACSPColumnDescription As Integer = 121

    ' Messages
    Public Const ACCancelDetailsTitle As Integer = 300
    Public Const ACCancelDetails As Integer = 301
    Public Const ACBusinessFailTitle As Integer = 302
    Public Const ACBusinessFail As Integer = 303


    ' Dialog messages
    Public Const ACDialogCloseClaimTitle As Integer = 901
    Public Const ACDialogCloseClaimDetail As Integer = 902

    Public Const ACMessageInvalidDelete As Integer = 911
    Public Const ACMessageInvalidRecoveryType As Integer = 912

    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.1)
    Public Const ACPartyTypeOther As String = "Other Party"
    Public Const ACPartyTypeClient As String = "Client"
    Public Const ACPartyTypeAgent As String = "Agent"
    Public Const ACPartyTypeInsurer As String = "Insurer"
    Public Const ACPartyTypeOtherPartyCode As String = "OT"
    Public Const ACPartyTypeInsurerCode As String = "IN"
    Public Const ACPartyTypeDefault As Integer = 4
    Public Const ACPartyTypeEmpty As Integer = 0
    Public Const ACPartyTypeDefaultCaption As String = "(None)"
    Public Const ACAnyStringEmpty As String = ""
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.1)

    Sub Main_Renamed()

    End Sub


    Public Sub DisplayMessage(ByVal MessageConstant As Integer, ByVal sTitle As String)

        Static sMessage As String = ""

        Try

            ' Get message text if not already present.

            sMessage = CStr(iPMFunc.GetResData(g_iLanguageID, MessageConstant, gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Display the status message.
            MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)

        Catch excep As System.Exception



            ' Error Section.

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display status message", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayMethod", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub


    Public Function GetUserAuthorities(ByRef r_bAllowOverride As Boolean) As Integer
        Dim result As Integer = 0
        Dim bACTUserAuthorities As Object


        Dim oUserAuthorities As bACTUserAuthorities.Business
        Dim vOverrideDate, vOverrideRate As Object

        Const kMethodName As String = "GetUserAuthorities"
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCount As Integer

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get UserAuthorities business object
            Dim temp_oUserAuthorities As Object
            lReturn = g_oObjectManager.GetInstance(temp_oUserAuthorities, "bACTUserAuthorities.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oUserAuthorities = temp_oUserAuthorities
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of 'bACTUserAuthorities.Business'")
            End If

            ' Get authority details for current user.

            lReturn = oUserAuthorities.GetDetails(vUserID:=g_oObjectManager.UserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oUserAuthorities.GetDetails", "Failed to get authority details")
            End If

            ' Get override options for current user.

            lReturn = oUserAuthorities.GetNext(vOverrideDate:=vOverrideDate, vOverrideRate:=vOverrideRate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oUserAuthorities.GetNext", "Failed to get authority details")
            End If

            ' Overrides allowed if rate = 1

            r_bAllowOverride = (CInt(vOverrideRate) = 1)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

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

    ' Unlock the claim
    Public Function UnlockClaim(ByVal v_lClaimId As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User
        Dim lOriginalClaimID As Integer
        Dim lReturn As gPMConstants.PMEReturnCode

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            lReturn = gPMConstants.PMEReturnCode.PMError

            'S4B Claim Enhancements R&D 2005

            ' Get original claim id as this is what we need to unlock

            lReturn = g_oBusiness.GetOriginalClaimID(v_lClaimId:=v_lClaimId, r_lOriginalClaimID:=lOriginalClaimID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockClaim, Failed to create instance of bPMLock.User")
            End If


            ' Get bPMLock
            Dim temp_oPMLock As Object
            lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockClaim, Failed to create instance of bPMLock.User")
            End If

            ' Unlock claim

            lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=lOriginalClaimID, iUserID:=g_oObjectManager.UserID)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Throw New System.Exception(Constants.vbObjectError.ToString() + ", UnlockClaim, Failed to unlock claim")
            End If

            oPMLock = Nothing

            Return result

        Catch excep As System.Exception



            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)


            Return lReturn

        End Try
    End Function
End Module