Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms

Imports SharedFiles
Partial Friend Class frmInterfaceRI2007
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 04/03/1997
    '
    ' Description: Main interface.
    ' ***************************************************************** '

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"
    'Developer guide no. 50
    Dim frmTreaty As frmTreaty
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As Integer
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lClaimID As Integer

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iCLMReinsurance.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Array of all applicable RI Bands and active band
    Private m_vBands(,) As Object
    Private m_lActiveBand As Integer

    ' Current RI Model
    Private m_lRIModelID As Integer
    Private m_sPartyType As String = ""
    Private m_iAction As Integer
    Private m_lRiArrangementId As Integer
    Private m_lRiskID As Integer
    Private m_vDeletedRILineIds() As Object
    Private vParticipantArray As XArrayHelper
    Private m_cUpperLimit As Decimal
    Private m_cLowerLimit As Decimal
    Private m_dRetained_percent As Double
    Private m_cSumInsured As Decimal
    Private m_bIsMultiActs As Boolean
    Private m_lGroupingId As Integer
    Private m_iRiArrangementVersion As Integer
    Private b_Loading As Boolean
    Private m_bOpenClaimNoTrans As Boolean
    Private m_lXOLRIModelId As Integer
    Private dIncurredToDate As Decimal
    Private m_bCanEditClaimRI As Boolean

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Private m_bRecovery As Boolean
    Private m_lRecovery As Integer

    Public WriteOnly Property Recovery() As Boolean
        Set(ByVal Value As Boolean)
            m_bRecovery = Value
        End Set
    End Property
    Public WriteOnly Property ActualRecovery() As Integer
        Set(ByVal Value As Integer)
            m_lRecovery = Value
        End Set
    End Property

    Public WriteOnly Property CallingAppName() As String
        Set(ByVal Value As String)
            m_sCallingAppName = Value
        End Set
    End Property

    Public WriteOnly Property EffectiveDate() As Date
        Set(ByVal Value As Date)
            m_dtEffectiveDate = Value
        End Set
    End Property

    Public ReadOnly Property ErrorNumber() As Integer
        Get
            Return m_lErrorNumber
        End Get
    End Property

    Public WriteOnly Property Navigate() As Integer
        Set(ByVal Value As Integer)
            m_lNavigate = Value
        End Set
    End Property

    Public WriteOnly Property ProcessMode() As Integer
        Set(ByVal Value As Integer)
            m_lProcessMode = Value
        End Set
    End Property

    Public ReadOnly Property Status() As Integer
        Get
            Return m_lStatus
        End Get
    End Property

    Public ReadOnly Property StepStatus() As String
        Get
            Return m_sStepStatus.Value
        End Get
    End Property

    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property

    Public Property TransactionType() As String
        Get
            Return m_sTransactionType
        End Get
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property ClaimID() As Integer
        Get
            Return m_lClaimID
        End Get
        Set(ByVal Value As Integer)
            m_lClaimID = Value
        End Set
    End Property

    Public Property RIArrangementID() As Integer
        Get
            Return m_lRiArrangementId
        End Get
        Set(ByVal Value As Integer)
            m_lRiArrangementId = Value
        End Set
    End Property

    Public Property IsOpenClaimNoTrans() As Boolean
        Get
            Return m_bOpenClaimNoTrans
        End Get
        Set(ByVal Value As Boolean)
            m_bOpenClaimNoTrans = Value
        End Set
    End Property

    ' ***************************************************************** '
    '                          PUBLIC METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "BusinessToInterface"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the details to the interface.
            If Information.IsArray(m_vBands) Then
                ' Populate ri band combo
                cboRIBand.Items.Clear()
                For lCount As Integer = m_vBands.GetLowerBound(1) To m_vBands.GetUpperBound(1)
                    Dim cboRIBand_NewIndex As Integer = -1
                    cboRIBand_NewIndex = cboRIBand.Items.Add(CStr(m_vBands(1, lCount)))
                    VB6.SetItemData(cboRIBand, cboRIBand_NewIndex, CInt(m_vBands(0, lCount)))
                Next lCount

                ' Set default item, this will trigger population of everything else
                cboRIBand.SelectedIndex = 0
            Else
                ' Not found, empty bands and disable other controls
                cboRIBand.Items.Clear()
                uctRI.ReadOnly_Renamed = True
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Updates all business members from the grid data.
    ' ***************************************************************** '
    Public Function DataToBusiness() As Integer

        Dim result As Integer = 0
        Dim vRILines(,) As Object
        Dim vSelectedArray(,) As Object
        Dim iRow1 As Integer
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DataToBusiness"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if we have data to update
            If uctRI.IsDirty Then
                ' Get updated ri arrangement

                lReturn = uctRI.GetProperties(vRILines)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctRI.GetProperties", "Failed to read new reinsurance lines")
                End If

                If vParticipantArray.GetUpperBound(0) > -1 Then
                    Dim m_vSelectedArray(vParticipantArray.GetUpperBound(0), 4) As Object
                    For iRow As Integer = 0 To vParticipantArray.GetUpperBound(0)
                        'Developer Guide no. 188
                        'start
                        m_vSelectedArray(iRow, ACIBrokerShortName) = vParticipantArray(iRow, ACIBrokerShortName)
                        m_vSelectedArray(iRow, ACIBrokerLongName) = vParticipantArray(iRow, ACIBrokerLongName)
                        m_vSelectedArray(iRow, ACIBrokerParticipant_percent) = vParticipantArray(iRow, ACIBrokerParticipant_percent)
                        m_vSelectedArray(iRow, ACIBrokerAssociationPartyCnt) = vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)
                        m_vSelectedArray(iRow, ACIBrokerPartyCnt) = vParticipantArray(iRow, ACIBrokerPartyCnt)
                        'end
                    Next

                    m_oBusiness.BrokerParticipantArray = m_vSelectedArray
                End If
                ' Update the business object

                lReturn = m_oBusiness.EditUpdate(lRIBandID:=m_lActiveBand, vRILines:=vRILines)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.EditUpdate", "Failed to update business object with new reinsurance")
                End If

                ' Retrieve the details related to rows deleted at Interface
                ' Level.
                m_lReturn = uctRI.GetDeletedRILines(m_vDeletedRILineIds)

            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Retrieves the details from the business object.
    ' ***************************************************************** '
    Public Function GetBusiness() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "GetBusiness"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            ' Get the details from the business object.

            lReturn = m_oBusiness.GetDetails()
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                gPMFunctions.RaiseError("m_oBusiness.GetDetails", "Failed to get details from business object")
            End If


            m_lRiArrangementId = m_oBusiness.RIArrangementID

            m_iRiArrangementVersion = m_oBusiness.RIArrangementVersion
            txtRI_version.Text = CStr(m_iRiArrangementVersion)
            ' Get the applicable RI Bands

            lReturn = m_oBusiness.GetRIBands(m_vBands)
            If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIBands", "Unable to retrieve ri bands")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' ProcessAutoRI
    ' ***************************************************************** '
    Public Function ProcessAutoRI() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessAutoRI"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Arul Stephen

            m_oBusiness.Recovery = m_bRecovery

            m_oBusiness.ActualRecovery = m_lRecovery
            'End Arul Stephen

            lReturn = m_oBusiness.CalculateRI
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.CalculateRI", "Unable to auto calculate reinsurance")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Set the Process, Map and Step status.
    ' ***************************************************************** '
    Public Function SetStatus(ByRef sProcessStatus As String, ByRef sMapStatus As String, ByRef sStepStatus As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Assign the current Status settings.
            m_sProcessStatus.Value = sProcessStatus.Trim()
            m_sMapStatus.Value = sMapStatus.Trim()
            m_sStepStatus.Value = sStepStatus.Trim()

            Return result

        Catch excep As System.Exception

            ' Error Section.

            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function


    ' ***************************************************************** '
    ' Unlock the claim
    ' ***************************************************************** '
    Public Function UnlockClaim(ByVal v_lOriginalClaimID As Integer) As Integer
        Dim result As Integer = 0
        Dim oPMLock As bPMLock.User

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'Get bPMLock
            Dim temp_oPMLock As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oPMLock, "bPMLock.User", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPMLock = temp_oPMLock

            ' Check for errors.
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to process the interface.
                result = gPMConstants.PMEReturnCode.PMFalse
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to get PMLock", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)

                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

                Return result
            End If
            m_lReturn = oPMLock.UnLockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)

            ' DD 26/7/2004 - PN13122
            ' Only error if return = PMError. If return = PMFalse, it just means
            ' the claim was not locked in the first place.
            'If (m_lReturn <> PMTrue) Then
            If m_lReturn = gPMConstants.PMEReturnCode.PMError Then
                result = gPMConstants.PMEReturnCode.PMFalse
                ' Log Error.
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to unlock claim", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockDataModel", vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return result
            End If
            oPMLock = Nothing
            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="UnlockClaim Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="UnlockClaim", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            Return result
        End Try
    End Function

    ' ***************************************************************** '
    '                         PRIVATE METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Display all language specific captions.
    ' ***************************************************************** '
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "DisplayCaptions"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            If g_bIsUnderwritingAgency Then

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskInterfaceTitleInsurer, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            Else

                Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            End If

            ' Check for an error.
            If Strings.Len(Me.Text) = 0 Then
                gPMFunctions.RaiseError("Len(Me.Caption) = 0", "Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() & _
                                        "Please check the file exists and the correct captions are available")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ''' <summary>
    ''' Display the banding details for a given party.
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function DisplayRIDetails() As Integer

        Dim nResult As Integer
        Dim cSumInsured As Decimal
        Dim cReserveToDate As Decimal
        Dim cPaymentToDate As Decimal
        Dim cThisReserve As Decimal
        Dim cThisPayment As Decimal
        Dim nCatastropheCodeID As Integer
        Dim nXolClmModelID As Integer
        Dim cXolClmLimit As Decimal
        Dim nXolCatModelID As Integer
        Dim cXolCatLimit As Decimal
        Dim nXolCatReinstatements As Integer
        Dim oRILines As Object
        Dim cRecoveredToDate As Decimal
        Dim oRI As cSIRRIControls.ClaimRIArrangement

        Const kMethodName As String = "DisplayRIDetails"

        Try

            nResult = PMEReturnCode.PMTrue

            ' Ensure we clear the model before we check it, if no model is
            ' found the call will not update it to zero.
            m_lRIModelID = 0
            m_lXOLRIModelId = 0

            ' Get information for the active band
            'Note--New parameter "cRecoveredToDate" added for the PN 58889

            nResult = m_oBusiness.GetBandValues(lRIBandID:=m_lActiveBand, cSumInsured:=cSumInsured, _
                                                cReserveToDate:=cReserveToDate, cPaymentToDate:=cPaymentToDate, _
                                                cThisReserve:=cThisReserve, cThisPayment:=cThisPayment, lRIModelID:=m_lRIModelID, _
                                                lCatastropheCodeID:=nCatastropheCodeID, lXolClmModelID:=nXolClmModelID, _
                                                cXolClmLimit:=cXolClmLimit, lXolCatModelID:=nXolCatModelID, cXolCatLimit:=cXolCatLimit, _
                                                lXolCatReinstatements:=nXolCatReinstatements, vRILines:=oRILines, cRecoveredToDate:=cRecoveredToDate, _
                                                lXOLRIModelId:=m_lXOLRIModelId, dIncurredToDate:=dIncurredToDate)

            ' Create new ri summary
            oRI = New cSIRRIControls.ClaimRIArrangement()
            oRI.SumInsured = cSumInsured
            oRI.ReserveToDate = cReserveToDate
            oRI.PaymentToDate = cPaymentToDate
            oRI.ThisReserve = cThisReserve
            oRI.ThisPayment = cThisPayment
            oRI.RecoveredToDate = cRecoveredToDate
            oRI.CatastropheCodeID = nCatastropheCodeID
            oRI.XolClmModelID = nXolClmModelID
            oRI.XolClmLimit = cXolClmLimit
            oRI.XolCatModelID = nXolCatModelID
            oRI.XolCatLimit = cXolCatLimit
            oRI.XolCatReinstatements = nXolCatReinstatements

            oRI.ReinsuranceLines = oRILines
            oRI.IncurredToDate = dIncurredToDate
            ' Populate reinsurance control?
            If IsArray(oRILines) Then

                If m_bRecovery Then
                    uctRI.Recovery = True
                End If

                nResult = uctRI.SetProperties(oRI)
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError("uctRI.SetProperties", "Unable to set claims reinsurance model")
                End If
                uctRI.Enabled = True
            Else
                ' Clear reinsurance control
                nResult = uctRI.Clear()
                If nResult <> PMEReturnCode.PMTrue Then
                    RaiseError("uctRI.Clear", "Unable to clear reinsurance")
                End If
                uctRI.Enabled = False
            End If
            EnableDisableButtons()
            uctSummary.RIArrangementID = m_oBusiness.RIArrangementID
            uctSummary.FilterType = 2

            ' Update summary control, if necessary
            If SSTabHelper.GetSelectedIndex(tabRI) = 1 Then
                If uctSummary.RIModelID <> m_lRIModelID Then
                    nResult = uctSummary.Clear()
                    If nResult <> PMEReturnCode.PMTrue Then
                        RaiseError("uctSummary.Clear", "Unable to clear ri model summary")
                    End If

                    ' Only populate if we have a model
                    If m_lRIModelID > 0 Then
                        nResult = uctSummary.SetProperties(m_lRIModelID)
                        If nResult <> PMEReturnCode.PMTrue Then
                            RaiseError("uctSummary.SetProperties", "Unable to set new ri model")
                        End If
                    End If
                End If
            End If

            Return nResult

        Catch excep As Exception

            nResult = PMEReturnCode.PMFalse
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=excep)
            Return nResult

        Finally
            oRI = Nothing
        End Try
    End Function

    Private Sub EnableDisableButtons()
        Try
            If b_Loading Then Exit Sub
            cmdAddTreaty.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            cmdAddFACXOL.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            cmdAddXOLTreaty.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            cmdView.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            cmdEdit.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            cmdDelete.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO")
            ' E005 Part1
            If Not m_bCanEditClaimRI Then
                cmdAddTreaty.Visible = False
                cmdAddFAC.Visible = False
                cmdAddXOLTreaty.Visible = False
                cmdAddFACXOL.Visible = False
                cmdEdit.Visible = False
                cmdDelete.Visible = False
            End If
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:="EnableDisbaleButtons", r_lFunctionReturn:=0, excep:=ex)
        End Try

    End Sub

    ' ***************************************************************** '
    ' Process party find and sets the return values.
    ' ***************************************************************** '
    Private Function ProcessFindParty(ByRef sPartyName As String, ByRef lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vBrokerArray(,) As Object
        Dim lReturn As Integer
        Dim iRow As Integer

        Const kMethodName As String = "ProcessFindParty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the find party component.
            Dim temp_oFindParty As Object
            lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of iPMBFindParty.Interface")
            End If

            ' Set the property values.

            oFindParty.CallingAppName = "iCLMReinsurance2007"

            ' Set the process modes.

            lReturn = oFindParty.SetProcessModes(vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.SetProcessModes", "Unable to set process modes on iPMBFindParty.Interface")
            End If

            ' Set special party type and start interface
            oFindParty.SpecialParty = PMBConst.PMBPartyTypeInsurer
            oFindParty.IsRetained = True
            oFindParty.RetainedValue = 0
            oFindParty.ReinsuranceTypeArray = "FAC"
            oFindParty.ReinsuranceTypeArray = "FAP"
            lReturn = oFindParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oFindParty.Start", "Unable to start Find Party interface")
            End If

            ' Check status and return either party details of empty values

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                lPartyCnt = oFindParty.PartyCnt
                sPartyName = oFindParty.ShortName
                vBrokerArray = oFindParty.BrokerArray
            Else
                lPartyCnt = 0
                sPartyName = ""
                vBrokerArray = Nothing
            End If

            If Information.IsArray(vBrokerArray) Then

                For lCount As Integer = 0 To vBrokerArray.GetUpperBound(0)
                    'Modied,checked at runtime
                    'vParticipantArray.AppendRows(1)
                    vParticipantArray.AppendRows()
                    iRow = vParticipantArray.GetUpperBound(0)
                    vParticipantArray(iRow, ACIBrokerShortName) = vBrokerArray(lCount, ACIBrokerShortName)
                    vParticipantArray(iRow, ACIBrokerLongName) = vBrokerArray(lCount, ACIBrokerLongName)
                    vParticipantArray(iRow, ACIBrokerParticipant_percent) = vBrokerArray(lCount, ACIBrokerParticipant_percent)
                    vParticipantArray(iRow, ACIBrokerAssociationPartyCnt) = vBrokerArray(lCount, ACIBrokerAssociationPartyCnt)
                    vParticipantArray(iRow, ACIBrokerPartyCnt) = vBrokerArray(lCount, ACIBrokerPartyCnt)
                Next
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' Terminate the component.
            If Not (oFindParty Is Nothing) Then

                oFindParty.Dispose()
            End If
            oFindParty = Nothing




        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Process to add a new party (facultative reinsurer)
    ' ***************************************************************** '
    Private Function ProcessNewParty() As Integer

        Dim result As Integer = 0
        Dim lPartyCnt As Integer
        Dim sPartyName As String = ""
        Dim bIsRiBroker As Boolean


        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessNewParty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            bIsRiBroker = False

            If m_sPartyType = "FAP" Then
                ' Process the party find to set the values.
                lReturn = CType(ProcessFindParty(sPartyName:=sPartyName, lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                ' Check if we have valid values.
                If lPartyCnt > 0 Then
                    lReturn = CType(GetFacDefaults(lPartyCnt, bIsRiBroker), gPMConstants.PMEReturnCode)
                    ' Add to grid
                    lReturn = uctRI.AddFacultative(lPartyCnt, sPartyName, bIsRiBroker)
                    Select Case lReturn
                        Case gPMConstants.PMEReturnCode.PMTrue
                            ' All is good
                        Case gPMConstants.PMEReturnCode.PMRecordInUse
                            MessageBox.Show("'" & sPartyName & "' is already present in this arrangement", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Case Else
                            gPMFunctions.RaiseError("uctRI.AddFacultative", "Unable to add facultative reinsurer")
                    End Select
                End If
            ElseIf m_sPartyType = "FAX" Then
                lReturn = CType(ProcessFindRIParty(sPartyName:=sPartyName, lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If
            End If


            ' Set focus to grid
            uctRI.Focus()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Process to add a new treaty.
    ' ***************************************************************** '
    Private Function ProcessNewTreaty(Optional ByRef TransactionType As String = "T") As Integer

        Dim result As Integer = 0
        Dim lTreatyID As Integer
        Dim sCode, sAgreementCode As String
        Dim bIsRetained As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessNewTreaty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no.50
            frmTreaty = New frmTreaty

            If TransactionType = "T" Then
                frmTreaty.TransactionType = "T"
            ElseIf TransactionType = "TX" Then
                frmTreaty.TransactionType = "TX"
            End If

            ' Show treaty dialog
            frmTreaty.ShowDialog()

            ' Check status
            If frmTreaty.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get and check treaty id
                lTreatyID = frmTreaty.TreatyId
                If lTreatyID > 0 Then
                    ' Get additional treaty information

                    lReturn = m_oBusiness.GetTreatyInfo(lTreatyID:=lTreatyID, sCode:=sCode, sAgreementCode:=sAgreementCode, bIsRetained:=bIsRetained)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.GetTreatyInfo", "Unable to get treaty information")
                    End If

                    ' Add to grid
                    lReturn = uctRI.AddTreaty(lTreatyID, sCode, sAgreementCode, bIsRetained, TransactionType)
                    Select Case lReturn
                        Case gPMConstants.PMEReturnCode.PMTrue
                            ' All is good
                        Case gPMConstants.PMEReturnCode.PMRecordInUse
                            MessageBox.Show("'" & sCode & "' is already present in this arrangement", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Case Else
                            gPMFunctions.RaiseError("uctRI.AddTreaty", "Unable to add treaty")
                    End Select
                End If
            End If

            ' Set focus to grid
            uctRI.Focus()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Sets all of the interface default values.
    ' ***************************************************************** '
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetInterfaceDefaults"
        Dim vResultArray(,) As Object

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Display all language specific captions.
            lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' If task is view only then hide ok button and relabel Cancel as close
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cmdOK.Visible = False
                cmdCancel.Text = "&Close"

                ' Also ensure ri grid is read only
                uctRI.ReadOnly_Renamed = True
            End If

            ''PN 61908 (start)
            If m_sTransactionType = "REN" Or m_sTransactionType = "C_CO" Then
                ' Decide if we should show the payment grid?

                lReturn = m_oBusiness.GetClaimTransType(vResultArray)
                If (lReturn <> gPMConstants.PMEReturnCode.PMTrue) And (lReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                    gPMFunctions.RaiseError("m_oBusiness.GetClaimTransType", "Unable to retrieve claim transaction type")
                Else
                    ''PN 62300
                    If IsOpenClaimNoTrans Then
                        uctRI.ShowPayments = True
                    Else
                        '1 Maintain Claim,2 Open Claim,3 payment(26-05-2010)
                        If gPMFunctions.ToSafeLong(vResultArray(0, 0)) = 1 Or gPMFunctions.ToSafeLong(vResultArray(0, 0)) = 2 Then
                            uctRI.ShowPayments = False
                        End If
                    End If
                    ''PN 62300
                End If
            End If
            ''PN 61908 (end)

            vParticipantArray = New XArrayHelper()
            vParticipantArray.RedimXArray(New Integer() {-1, 4}, New Integer() {0, 0})

            txtRI_version.Enabled = False
            If m_sTransactionType = "C_CO" Then
                txtRI_version.Text = CStr(1)
            End If
            b_Loading = True
            ' Ensure proper display of tabs
            SSTabHelper.SetSelectedIndex(tabRI, 0)
            tabRI_SelectedIndexChanged(tabRI, New EventArgs())
            b_Loading = False


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function
    ' ***************************************************************** '
    '                        CONTROL EVENTS
    ' ***************************************************************** '
    Private Sub cboRIBand_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRIBand.SelectedIndexChanged

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cboRIBand_Click"

        Try

            ' Check active band
            If m_lActiveBand <> VB6.GetItemData(cboRIBand, cboRIBand.SelectedIndex) Then
                ' Store current data
                lReturn = DataToBusiness()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("DataToBusiness", "Unable to store new ri details")
                End If

                ' Set new band and refresh data
                m_lActiveBand = VB6.GetItemData(cboRIBand, cboRIBand.SelectedIndex)

                lReturn = CType(DisplayRIDetails(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("DisplayRIDetails", "Unable to display ri details")
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub cmdAddFAC_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFAC.Click
        ' Delegate
        m_sPartyType = "FAP"
        ProcessNewParty()
    End Sub

    Private Sub cmdAddFACXOL_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFACXOL.Click
        m_sPartyType = "FAX"
        m_iAction = 1 'Add
        ProcessNewParty()
    End Sub

    Private Sub cmdAddTreaty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTreaty.Click
        ' Delegate
        ProcessNewTreaty("T")
    End Sub

    Private Sub cmdAddXOLTreaty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddXOLTreaty.Click
        ProcessNewTreaty("TX")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        If uctRI.SelectedRIType = "T" Or uctRI.SelectedRIType = "TX" Then
            If MessageBox.Show("You are deleting a Treaty, Please confirm you wish to Delete.", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                m_lReturn = ProcessDeleteRow()
            End If

        ElseIf uctRI.SelectedRIType = "F" Or uctRI.SelectedRIType = "FX" Then
            If MessageBox.Show("Do you really wish to Delete this Reinsurance Placement?", "Delete Confirmation", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                m_lReturn = ProcessDeleteRow()
            End If
        End If
    End Sub

    Private Sub cmdEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEdit.Click
        m_iAction = 3 'edit
        If uctRI.SelectedRIType = "FX" Then
            m_sPartyType = "FAX"
            ProcessParty()
        ElseIf uctRI.SelectedRIType = "F" Then
            If uctRI.IsRIBroker Then
                m_sPartyType = "FAP"
                ProcessParty()
            Else
                MessageBox.Show("Facultative Placement is not linked to an RI Broker.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show("Option not available for this reinsurance placement type.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub
    ' ***************************************************************** '
    ' Process to Delete the Selected Row from the Grid.
    ' ***************************************************************** '
    Private Function ProcessDeleteRow(Optional v_bOnEditFX As Boolean = False) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Dim lCount As Integer

        Const kMethodName As String = "DeleteRow"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'delete row from array
            If uctRI.IsRIBroker Then
                If vParticipantArray.GetUpperBound(0) > -1 Then
                    lCount = vParticipantArray.GetUpperBound(0)
                    For iRow As Integer = 0 To lCount
                        For iRow1 As Integer = 0 To vParticipantArray.GetUpperBound(0)
                            'Developer Guide no. 188
                            If uctRI.PartyCnt = vParticipantArray(iRow1, ACIBrokerAssociationPartyCnt) Then
                                vParticipantArray.DeleteRows(iRow1)
                                Exit For
                            End If
                        Next
                    Next
                End If
            End If
            ' Add to grid
            lReturn = uctRI.DeleteRow(m_iAction, v_bOnEditFX:=v_bOnEditFX)
            Select Case lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' All is good
                Case gPMConstants.PMEReturnCode.PMRecordInUse
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
            End Select

            ' Set focus to grid
            uctRI.Focus()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessNewTreaty(), excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lValid, lBand As Integer
        Dim bValid As Boolean
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdOK_Click"

        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Should not be visible but check, just in case
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Me.Hide()
                Exit Sub
            End If

            m_lReturn = uctRI.ValidateRILines(bValid)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("OK Click", "Unable to validate RI Lines")
            End If

            If Not bValid Then
                Exit Sub
            End If

            ' Ensure we have committed any edits in the grid
            uctRI.FinaliseEdit()

            ' Store current data
            lReturn = DataToBusiness()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DataToBusiness", "Unable to store new ri details")
            End If

            If uctRI.UnallocatedReserve <> 0 Then
                lValid = 1
            End If
            If uctRI.UnallocatedPayment <> 0 Then
                lValid = 2
            End If

            If uctRI.UnallocatedSumInsured <> 0 Then ' PN 76602
                lValid = 3
            End If

            Select Case True
                Case lReturn <> gPMConstants.PMEReturnCode.PMTrue
                    Interaction.MsgBox("Unable to validate bands", VariantType.Error, "Reinsurance")
                    Exit Sub
                Case lValid = 1
                    iPMFunc.SetComboBoxValue(cboRIBand, CStr(lBand))
                    MessageBox.Show("Reserve share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                Case lValid = 2
                    iPMFunc.SetComboBoxValue(cboRIBand, CStr(lBand))
                    MessageBox.Show("Payment share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                Case lValid = 3 'PN 76602
                    Call SetComboBoxValue(cboRIBand, lBand)
                    MsgBox("Sum Insured share for '" & cboRIBand.Text & "' is not 100%", vbExclamation, Me.Text)
                    Exit Sub
            End Select

            If Not (m_vDeletedRILineIds Is Nothing) Then
                ' Set the Property for Deleted RI Lines at Business Level

                m_oBusiness.DeletedRIArrangementIds = VB6.CopyArray(m_vDeletedRILineIds)
            End If

            ' Process the next set of actions depending upon the interface task etc.
            lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            ' Everything OK, so we can hide the interface.
            Me.Hide()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub


    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "cmdCancel_Click"


        Try

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                ' In view mode this button is simply 'close'
                m_lStatus = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            Else
                ' Ensure we have committed any edits in the grid
                uctRI.FinaliseEdit()

                ' Set the interface status.
                m_lStatus = gPMConstants.PMEReturnCode.PMCancel

                ' Process the next set of actions depending upon the interface task etc.
                lReturn = CType(m_oGeneral.ProcessCommand(), gPMConstants.PMEReturnCode)
                If lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                    ' Everything OK, so we can hide the interface.
                    Me.Hide()
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    'Private Sub cndView_Click()
    'm_iAction = 2 'View
    'End Sub

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
        m_iAction = 2 'View
        If uctRI.SelectedRIType = "FX" Then
            m_sPartyType = "FAX"
            ProcessParty()
        ElseIf uctRI.SelectedRIType = "F" Then
            If uctRI.IsRIBroker Then
                m_sPartyType = "FAP"
                ProcessParty()
            Else
                MessageBox.Show("Facultative Placement is not linked to an RI Broker.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        Else
            MessageBox.Show("Option not available for this reinsurance placement type.", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub tabRI_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabRI.SelectedIndexChanged

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "tabRI_Click"

        Try

            ' Set visible states as the tab control uses positions to hide controls which
            ' conflicts with the screen resize code
            uctRI.Visible = (SSTabHelper.GetSelectedIndex(tabRI) = 0)
            uctSummary.Visible = (SSTabHelper.GetSelectedIndex(tabRI) = 1)

            EnableDisableButtons()

            ' Update summary control, if necessary
            If SSTabHelper.GetSelectedIndex(tabRI) = 1 Then
                If uctSummary.RIModelID <> m_lRIModelID Then
                    lReturn = uctSummary.Clear()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("uctSummary.Clear", "Unable to clear ri model summary")
                    End If

                    ' Only populate if we have a model
                    If m_lRIModelID > 0 Then
                        lReturn = uctSummary.SetProperties(m_lRIModelID, v_lXOLRIModelId:=m_lXOLRIModelId)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("uctSummary.SetProperties", "Unable to set new ri model")
                        End If
                    End If
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally
            tabRIPreviousTab = tabRI.SelectedIndex
        End Try
    End Sub
    Private Function ProcessFindRIParty(ByRef sPartyName As String, ByRef lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMUFindRIParty As Object
        Dim oFindRIParty As iPMUFindRIParty.Interface_Renamed
        Dim lReturn As Integer
        Dim v_iTask As gPMConstants.PMEComponentAction
        Dim vBrokerArray(,) As Object
        Dim m_vSelectedArray(,) As Object
        Dim iRow, iRow1 As Integer
        Dim lCount As Integer

        Const kMethodName As String = "ProcessFindRIParty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case m_iAction
                Case 1
                    v_iTask = gPMConstants.PMEComponentAction.PMAdd
                Case 2
                    v_iTask = gPMConstants.PMEComponentAction.PMView
                Case 3
                    v_iTask = gPMConstants.PMEComponentAction.PMEdit
                Case Else
                    v_iTask = gPMConstants.PMEComponentAction.PMView
            End Select

            ' Create an instance of the find ri party component.
            Dim temp_oFindRIParty As Object
            lReturn = g_oObjectManager.GetInstance(temp_oFindRIParty, sClassName:="iPMUFindRIParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindRIParty = temp_oFindRIParty
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of iPMUFindRIParty.Interface")
            End If

            ' Set the property values.

            oFindRIParty.CallingAppName = ACApp
            oFindRIParty.IsFAX = m_sPartyType = "FAX"
            oFindRIParty.Ri_Arrangement_id = m_lRiArrangementId
            oFindRIParty.RiskId = m_lRiskID
            oFindRIParty.ClaimId = m_lClaimID
            oFindRIParty.TotalSumInsured = m_oBusiness.TotalSumInsured
            ' Set the process modes.
            If m_sTransactionType = "REN" Then
                m_sTransactionType = "C_CV" ' Claim View
            End If

            lReturn = oFindRIParty.SetProcessModes(vTask:=v_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.SetProcessModes", "Unable to set process modes on iPMBFindParty.Interface")
            End If

            If Not Object.Equals(uctRI.ExistingLimits, Nothing) Then
                oFindRIParty.ExistingLimits = uctRI.ExistingLimits
            End If

            If m_iAction = 2 Or m_iAction = 3 Then 'View Or Edit of FAC XOL
                If uctRI.SelectedRIType = "FX" Then
                    oFindRIParty.GroupingId = uctRI.GroupingID
                    oFindRIParty.UpperLimit = uctRI.UpperLimit
                    oFindRIParty.LowerLimit = uctRI.LowerLimit
                ElseIf uctRI.SelectedRIType = "F" Then

                    oFindRIParty.RI_Arrangement_Line_Id = uctRI.SelRIArrangementLine
                    If uctRI.SelRIArrangementLine = 0 Then

                        lCount = 0
                        For iRow = 0 To vParticipantArray.GetUpperBound(0)
                            'Developer Guide no. 188
                            'start
                            If vParticipantArray(iRow, ACIBrokerAssociationPartyCnt) = uctRI.PartyCnt Then
                                lCount += 1
                            End If
                        Next
                        If lCount > 0 Then
                            ReDim m_vSelectedArray(lCount - 1, 4)
                            iRow1 = 0
                            For iRow = 0 To vParticipantArray.GetUpperBound(0)
                                If vParticipantArray(iRow, ACIBrokerAssociationPartyCnt) = uctRI.PartyCnt Then
                                    m_vSelectedArray(iRow1, ACIBrokerShortName) = vParticipantArray(iRow, ACIBrokerShortName)
                                    m_vSelectedArray(iRow1, ACIBrokerLongName) = vParticipantArray(iRow, ACIBrokerLongName)
                                    m_vSelectedArray(iRow1, ACIBrokerParticipant_percent) = vParticipantArray(iRow, ACIBrokerParticipant_percent)
                                    m_vSelectedArray(iRow1, ACIBrokerAssociationPartyCnt) = vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)
                                    m_vSelectedArray(iRow1, ACIBrokerPartyCnt) = vParticipantArray(iRow, ACIBrokerPartyCnt)
                                    iRow1 += 1
                                End If
                            Next
                        End If
                    End If
                    oFindRIParty.PartyCnt = uctRI.PartyCnt
                    oFindRIParty.BrokerArray = m_vSelectedArray
                End If
            End If

            If m_iAction = 2 Then
                oFindRIParty.NotEditable = True
            End If

            lReturn = oFindRIParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oFindRIParty.Start", "Unable to start Find Party interface")
                Return gPMConstants.PMEReturnCode.PMFalse
            End If

            ' Check status and return either party details of empty values

            If (oFindRIParty.Status = gPMConstants.PMEReturnCode.PMOK) And m_sPartyType = "FAX" Then
                lPartyCnt = oFindRIParty.PartyCnt
                sPartyName = oFindRIParty.ShortName
                m_cUpperLimit = oFindRIParty.UpperLimit
                m_cLowerLimit = oFindRIParty.LowerLimit
                m_dRetained_percent = oFindRIParty.Retained_Percent
                m_cSumInsured = oFindRIParty.SumInsured * (100 - oFindRIParty.Retained_Percent) / 100
                m_lGroupingId = oFindRIParty.GroupingId
                If lPartyCnt = 0 And sPartyName = "Multiple Acts" Then
                    m_bIsMultiActs = True
                End If
                If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    m_lReturn = ProcessDeleteRow(v_bOnEditFX:=True)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("ProcessFindRIParty", "Failed to delete row.", gPMConstants.PMELogLevel.PMLogError)
                        Return gPMConstants.PMEReturnCode.PMFalse
                    End If
                End If
                If v_iTask = gPMConstants.PMEComponentAction.PMAdd Or v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    ' Add to grid

                    If m_lGroupingId <> 0 And m_lGroupingId <> oFindRIParty.GroupingId Then

                        m_lGroupingId = oFindRIParty.GroupingId
                    End If

                    lReturn = uctRI.AddFacultativeXOL(lPartyCnt:=lPartyCnt, sDescription:=sPartyName, dRetained:=m_dRetained_percent, cLowerLimit:=m_cLowerLimit, cUpperLimit:=m_cUpperLimit, cSumInsured:=m_cSumInsured, lGroupingId:=m_lGroupingId, sTransactionType:=m_sTransactionType)
                    Select Case lReturn
                        Case gPMConstants.PMEReturnCode.PMTrue
                            ' All is good
                        Case gPMConstants.PMEReturnCode.PMRecordInUse
                            MessageBox.Show("'" & sPartyName & "' is already present in this arrangement", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Case Else
                            gPMFunctions.RaiseError("uctRI.AddFacultative", "Unable to add facultative reinsurer")
                            result = gPMConstants.PMEReturnCode.PMFalse
                    End Select
                End If
            ElseIf (oFindRIParty.Status = gPMConstants.PMEReturnCode.PMOK) And m_sPartyType = "FAP" And v_iTask = gPMConstants.PMEComponentAction.PMEdit Then


                vBrokerArray = oFindRIParty.BrokerArray

                If vParticipantArray.GetUpperBound(0) > -1 Then
                    lCount = vParticipantArray.GetUpperBound(0)
                    For iRow = 0 To lCount
                        For iRow1 = 0 To vParticipantArray.GetUpperBound(0)
                            If uctRI.PartyCnt = vParticipantArray(iRow1, ACIBrokerAssociationPartyCnt) Then
                                vParticipantArray.DeleteRows(iRow1)
                                Exit For
                            End If
                        Next
                    Next
                End If
                If Information.IsArray(vBrokerArray) Then

                    For lCount = 0 To vBrokerArray.GetUpperBound(0)
                        'Modified,checked at runtime
                        'vParticipantArray.AppendRows(1)
                        vParticipantArray.AppendRows()
                        iRow = vParticipantArray.GetUpperBound(0)

                        vParticipantArray(iRow, ACIBrokerShortName) = vBrokerArray(lCount, ACIBrokerShortName)

                        vParticipantArray(iRow, ACIBrokerLongName) = vBrokerArray(lCount, ACIBrokerLongName)

                        vParticipantArray(iRow, ACIBrokerParticipant_percent) = vBrokerArray(lCount, ACIBrokerParticipant_percent)

                        vParticipantArray(iRow, ACIBrokerAssociationPartyCnt) = vBrokerArray(lCount, ACIBrokerAssociationPartyCnt)

                        vParticipantArray(iRow, ACIBrokerPartyCnt) = vBrokerArray(lCount, ACIBrokerPartyCnt)
                    Next
                    uctRI.IsDirty = True
                End If
            Else
                lPartyCnt = 0
                sPartyName = ""
                m_cUpperLimit = 0
                m_cLowerLimit = 0
                m_dRetained_percent = 0
                m_cSumInsured = 0
                m_lGroupingId = 0
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' Terminate the component.
            If Not (oFindRIParty Is Nothing) Then

                oFindRIParty.Dispose()
            End If
            oFindRIParty = Nothing




        End Try
        Return result
    End Function

    Private Sub uctRI_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctRI.GotFocus
        VB6.SetDefault(cmdOK, False)
    End Sub

    Private Sub uctRI_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctRI.LostFocus
        VB6.SetDefault(cmdOK, True)
    End Sub
    ' ***************************************************************** '
    '                         FORM EVENTS
    ' ***************************************************************** '
    Private Sub Form_Initialize_Renamed()

        Dim sMessage, sTitle As String

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Initialise the error number value.
            m_lErrorNumber = gPMConstants.PMEReturnCode.PMTrue

            ' Retrieve the product option to find on which type of RI processing we need to work with
            ' Get an instance of the business object via the public object manager.

            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMReinsuranceRI2007.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                ' Get description from the resource file.

                sTitle = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFailTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
                sMessage = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACBusinessFail, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

                ' Display message.
                MessageBox.Show(sMessage, sTitle, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub
            End If

            ' Create an instance of the general interface object.
            m_oGeneral = New iCLMReinsurance.General()

            ' Call the initialise method passing this interface and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(ofrmInterface:=Me, oBusiness:=m_oBusiness)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the cancelled property to true. This is done so that any interface termination
            ' will be noted as cancelled except in the event of accepting the interface.
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

        Catch excep As System.Exception


            m_lErrorNumber = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to initialise the interface object", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Initialise", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End Try

    End Sub
    Private Sub frmInterfaceRI2007_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        uctRI.grdRI_Enter()
    End Sub

    Public Sub frmInterfaceLoad()

        Dim bApplyReinsurance, bDeferredRIStatus As Boolean
        Dim sMessage As String = ""
        Dim vIsEditRI As Object
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_Load"


        Try

            ' Check if we have had an error so far.
            ' Possibly creating the business object.
            If m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse Then
                ' We have already encountered an error, so we MUST exit now.
                Exit Sub
            End If

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
            m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTEnableClaimRIEditing, v_vBranch:=g_iSourceID, r_vUnderwriting:=vIsEditRI)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetClaimRiskStatus", "Unable to check deferred reinsurance status")
            End If

            If vIsEditRI = "" Or vIsEditRI = "0" Then
                m_bCanEditClaimRI = False
            Else
                m_bCanEditClaimRI = True
            End If

            ' Check if RI is deferred on the risk, exit if it is

            m_lReturn = m_oBusiness.GetClaimRiskStatus(v_lClaimId:=m_lClaimID, r_bIsDeferred:=bDeferredRIStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.GetClaimRiskStatus", "Unable to check deferred reinsurance status")
            End If

            ' Check deferred flag
            If bDeferredRIStatus Then
                MessageBox.Show("Risk is using deferred reinsurance model." & Strings.Chr(13) & Strings.Chr(10) & "No reinsurance allocations will be processed.", "Reinsurance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate, bOpenClaimNoTrans:=m_bOpenClaimNoTrans)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetProcessModes", "Failed to set the process modes for the business object")
            End If

            ' Set the status for the business object.

            lReturn = m_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetStatus", "Failed to set the status for the business object")
            End If

            m_oBusiness.Recovery = m_bRecovery
            uctRI.TransactionType = m_sTransactionType
            ' Set the business keys.
            m_oBusiness.ClaimID = m_lClaimID
            m_oBusiness.BalanceAndCloseClaim = g_bBalanceAndCloseClaim

            lReturn = m_oBusiness.ApplyReinsurance(bApplyReinsurance)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.ApplyReinsurance", "Unable to check auto reinsurance flag")
            End If

            If Not bApplyReinsurance Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If

            ' Initialise user controls
            'developer guide no.9
            lReturn = uctSummary.Initialise()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("uctSummary.Initialise", "Unable to initialise summary control")
            End If

            ' Set the interface default values.
            lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetInterfaceDefaults", "Unable to set interface defaults")
            End If

            ' Gets the interface details to be displayed.
            lReturn = CType(m_oGeneral.GetInterfaceDetails(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oGeneral.GetInterfaceDetails", "Unable to get interface details")
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lErrorNumber, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)



        End Try
    End Sub

    Private Sub frmInterfaceRI2007_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_QueryUnload"

        Try

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Terminate the general object.
            m_oGeneral.Dispose()

            m_oGeneral = Nothing

            ' Terminate the business object

            m_oBusiness.Dispose()

            m_oBusiness = Nothing


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally
            ' Reset the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)


            eventArgs.Cancel = Cancel <> 0
        End Try
    End Sub

    Private isInitializingComponent As Boolean


    Private Sub frmInterfaceRI2007_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' Resize tabs
            tabRI.SetBounds(VB6.TwipsToPixelsX(90), VB6.TwipsToPixelsY(570), ClientRectangle.Width - VB6.TwipsToPixelsX(180), ClientRectangle.Height - VB6.TwipsToPixelsY(1080))
            ''
            ' Resize tab controls
            'Developer Guide No. Modified, checked by R & D
            uctRI.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
            uctSummary.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)

            cmdOK.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(2580), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdCancel.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1290), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

            ' Move command buttons
            cmdOK.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(2580), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdCancel.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1290), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdAddTreaty.SetBounds(VB6.TwipsToPixelsX(120), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdAddFAC.SetBounds(VB6.TwipsToPixelsX(1785), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdAddXOLTreaty.SetBounds(VB6.TwipsToPixelsX(3405), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdAddFACXOL.SetBounds(VB6.TwipsToPixelsX(5010), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdView.SetBounds(VB6.TwipsToPixelsX(6615), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdEdit.SetBounds(VB6.TwipsToPixelsX(7995), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdDelete.SetBounds(VB6.TwipsToPixelsX(9360), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try

    End Sub

    Private Function ProcessParty() As Integer

        Dim result As Integer = 0
        Dim lPartyCnt As Integer
        Dim sPartyName As String = ""
        Dim dDefaultComm As Double

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessParty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Process the party find to set the values.

            lReturn = CType(ProcessFindRIParty(sPartyName:=sPartyName, lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Set focus to grid
            uctRI.Focus()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Process party find and sets the return values.
    ' ***************************************************************** '
    Private Function GetFacDefaults(ByVal lPartyCnt As Integer, ByRef bIsRiBroker As Boolean) As Integer
        Dim result As Integer = 0
        Dim bSIRPartyIN As Object
        Dim oPartyIn As bSIRPartyIN.Business
        Dim lReturn As Integer
        Const kMethodName As String = "GetFacDefaults"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the find party component.
            Dim temp_oPartyIn As Object
            lReturn = g_oObjectManager.GetInstance(temp_oPartyIn, "bSIRPartyIN.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oPartyIn = temp_oPartyIn
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of bSIRParyIN.Business")
            End If

            ' Set the process modes.

            lReturn = oPartyIn.SetProcessModes(vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.SetProcessModes", "Unable to set process modes on bSIRPartyIN.Business")
            End If

            ' Get the insurer details

            lReturn = oPartyIn.GetDetails(vPartyCnt:=lPartyCnt)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.GetDetails", "Unable to get insurer details")
            End If

            ' Get value

            lReturn = oPartyIn.GetNext(vPartyCnt:=lPartyCnt, vIsRIBroker:=bIsRiBroker)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.GetNext", "Unable to get default commission for FAC reinsurer")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally
            ' Terminate and release the component.
            If Not (oPartyIn Is Nothing) Then

                oPartyIn.Dispose()
            End If
            oPartyIn = Nothing




        End Try
        Return result
    End Function
    'Developer Guide No. Modified as per requiremnt
    Private Sub uctRI_ResetControls(ByVal Sender As Object, ByVal e As cSIRRIControls.uctClaimRIControlRI2007.ResetControlsEventArgs) Handles uctRI.ResetControls
        Dim lReturn As Integer
        Const kMethodName As String = "uctRI_ResetControls"


        Try
            If m_sTransactionType = "C_CO" Then
                'Developer Guide No.
                If e.SelRIType = "FX" Or e.SelRIType = "F" Then
                    cmdView.Enabled = True
                    If m_bCanEditClaimRI Then
                        cmdEdit.Enabled = True
                    End If
                Else
                    cmdView.Enabled = False
                    cmdEdit.Enabled = False
                End If
            Else
                'If uctRI.ThisPayment > 0 Or uctRI.ThisReserve > 0 Then
                cmdView.Enabled = e.SelRIType = "FX" Or e.SelRIType = "F"
                'End If
            End If



        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally
        End Try
    End Sub


End Class
