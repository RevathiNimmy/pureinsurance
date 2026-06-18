Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide No 129. 
'Start
Imports SharedFiles
Imports cSIRRIControls

'End
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
    'Developer Guide No.7
    Private Const vbFormCode As Integer = 0
    'Developer Guide No.69
    Public frmTreaty As frmTreaty
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As gPMConstants.PMENavigateButtonStatus
    Private m_lProcessMode As gPMConstants.PMEProcessMode
    Private m_sTransactionType As String = ""
    Private m_dtEffectiveDate As Date

    Private m_lInsuranceFileCnt As Integer
    Private m_lRiskID As Integer

    ' Status members
    Private m_sProcessStatus As New FixedLengthString(2)
    Private m_sMapStatus As New FixedLengthString(2)
    Private m_sStepStatus As New FixedLengthString(2)


    ' Declare an instance of the general interface object.
    Private m_oGeneral As iPMUReinsurance.General

    ' Declare an instance of the Business object.
    Private m_oBusiness As Object

    ' Stores the return value for the a function call.
    Private m_lReturn As Integer

    ' Array of all applicable RI Bands and active band
    Private m_vBands(,) As Object
    Private m_lActiveBand As Integer

    Private m_bDisplayScreen As Boolean

    ' Current RI Model
    Private m_lRIModelID As Integer

    Private m_sPartyType As String = ""
    Private m_iAction As Integer

    Private m_cUpperLimit As Decimal
    Private m_cLowerLimit As Decimal
    Private m_dRetained_percent As Double
    Private m_dParticipation_percent As Double
    Private m_dComm_percent As Double
    Private m_cSumInsured As Decimal
    Private m_cTotalSumInsured As Decimal
    Private m_cPremium As Decimal
    Private m_cPremiumTax As Decimal
    Private m_cCommission As Decimal
    Private m_cCommTax As Decimal
    Private m_bIsMultiActs As Boolean
    Private m_lGroupingId As Integer
    Private m_lRiArrangementId As Integer
    Private m_vDeletedRILineIds() As Object
    Private vParticipantArray As XArrayHelper
    Private m_vGroupingIDs(,) As Object
    Private m_vAdedFindRIPartyLines As Object 'PN 44646

    ' E007 Changes
    Private m_lXOLRIModelId As Long

    Private m_cGrossPremium As Decimal
    Private m_oRIVersion(,) As Object
    Private m_nRIVersion As Integer

    Private m_bIsRiskDeleted As Boolean

    Private m_nRIVersionId As Integer
    Private m_bRIPending As Boolean
    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
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

    'UPGRADE_NOTE: (7001) The following declaration (let Status) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub Status(ByVal Value As Integer)
    'm_lStatus = Value
    'End Sub
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

    Public WriteOnly Property TransactionType() As String
        Set(ByVal Value As String)
            m_sTransactionType = Value
        End Set
    End Property

    Public Property InsuranceFileCnt() As Integer
        Get
            Return m_lInsuranceFileCnt
        End Get
        Set(ByVal Value As Integer)
            m_lInsuranceFileCnt = Value
        End Set
    End Property

    Public Property RiskID() As Integer
        Get
            Return m_lRiskID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskID = Value
        End Set
    End Property


    Public Property DisplayScreen() As Boolean
        Get
            Return m_bDisplayScreen
        End Get
        Set(ByVal Value As Boolean)
            m_bDisplayScreen = Value
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

    Public Property IsRiskDeleted() As Boolean
        Get
            Return m_bIsRiskDeleted
        End Get
        Set(value As Boolean)
            m_bIsRiskDeleted = value
        End Set
    End Property

    Public Property RIVersionId() As Integer
        Get
            Return m_nRIVersionId
        End Get
        Set(value As Integer)
            m_nRIVersionId = value
        End Set
    End Property


    ' ***************************************************************** '
    '                          PUBLIC METHODS
    ' ***************************************************************** '

    ' ***************************************************************** '
    ' Updates all interface details from the business object.
    ' ***************************************************************** '
    Public Function BusinessToInterface() As Integer

        Dim nResult As Integer
        Dim nCount As Integer
        Const kMethodName As String = "BusinessToInterface"
        Try
            nResult = gPMConstants.PMEReturnCode.PMTrue
            If Information.IsArray(m_oRIVersion) Then
                nCount = m_oRIVersion.GetUpperBound(1)
                m_nRIVersionId = m_oRIVersion(0, nCount)
            End If
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
                uctORI.ReadOnly_Renamed = True
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                If Information.IsArray(m_oRIVersion) Then
                    cboRIVersionType.Items.Clear()
                    For nCount = m_oRIVersion.GetLowerBound(1) To m_oRIVersion.GetUpperBound(1)
                        Dim cboRIVersionType_NewIndex As Integer = -1
                        cboRIVersionType_NewIndex = cboRIVersionType.Items.Add(m_oRIVersion(1, nCount))
                        VB6.SetItemData(cboRIVersionType, cboRIVersionType_NewIndex, CInt(m_oRIVersion(0, nCount)))
                    Next nCount

                    ' Set default item, this will trigger population of everything else
                    cboRIVersionType.SelectedIndex = 0

                    txtEffectiveDate.Text = CDate(m_oRIVersion(2, 0))
                End If
            End If


        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
        Return nResult

    End Function

    ' ***************************************************************** '
    ' Updates all business members from the grid data.
    ' ***************************************************************** '
    Public Function DataToBusiness() As Integer

        Dim result As Integer = 0
        Dim vRILines As Object
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
                        m_vSelectedArray(iRow, ACIBrokerShortName) = vParticipantArray(iRow, ACIBrokerShortName)
                        m_vSelectedArray(iRow, ACIBrokerLongName) = vParticipantArray(iRow, ACIBrokerLongName)
                        m_vSelectedArray(iRow, ACIBrokerParticipant_percent) = vParticipantArray(iRow, ACIBrokerParticipant_percent)
                        m_vSelectedArray(iRow, ACIBrokerAssociationPartyCnt) = vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)
                        m_vSelectedArray(iRow, ACIBrokerPartyCnt) = vParticipantArray(iRow, ACIBrokerPartyCnt)
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

                ' Gaurav Changed
                ' Update the Premium Percent after
                ' it is being assigned to the Arrangements


                m_lReturn = m_oBusiness.UpdatePremiumPercent()

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

        Dim result As Integer
        Dim nReturn As Integer
        Const kMethodName As String = "GetBusiness"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get the details from the business object.

            nReturn = m_oBusiness.GetDetails()
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) And (nReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                gPMFunctions.RaiseError("m_oBusiness.GetDetails", "Failed to get details from business object")
            End If


            m_lRiArrangementId = m_oBusiness.RIArrangementID
            ' Get the applicable RI Bands

            nReturn = m_oBusiness.GetRIBands(m_vBands)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) And (nReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIBands", "Unable to retrieve ri bands")
            End If

            nReturn = m_oBusiness.GetRIVersion(m_oRIVersion)
            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) And (nReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                gPMFunctions.RaiseError("m_oBusiness.GetRIVersion", "Unable to retrieve ri versions")
            End If
            'Fetch all FAC XOL Groupings from previous transaction

            nReturn = m_oBusiness.GetGroupingIDs(m_vGroupingIDs)

            If (nReturn <> gPMConstants.PMEReturnCode.PMTrue) And (nReturn <> gPMConstants.PMEReturnCode.PMNotFound) Then
                gPMFunctions.RaiseError("m_oBusiness.GetGroupingIDs", "Unable to retrieve FAX Grouping")
            End If
        Catch ex As Exception
            result = gPMConstants.PMEReturnCode.PMFalse
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
            Return result
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
            ''Debugger.Break()
            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetStatus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetStatus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

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
            Me.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACRiskInterfaceTitle, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            ' Check for an error.
            If Strings.Len(Me.Text) = 0 Then
                gPMFunctions.RaiseError("Len(Me.Caption) = 0", "Unable to retrieve data from the resource file." & Strings.Chr(10).ToString() &
                                        "Please check the file exists and the correct captions are available")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Display the banding details for a given party.
    ' ***************************************************************** '
    Private Function DisplayRIDetails() As Integer

        Dim nResult As Integer
        Dim cSumInsured, cPremium As Decimal
        Dim vRILines As Object
        Dim iFacPremiumMethod As cSIRRIControls.RiskRIArrangement.FACPremiumEnum
        Dim cOriginalSumInsured, cOriginalPremium As Decimal
        Dim vOriginalRILines As Object
        Dim bIsextendedlimitApplied As Boolean
        Dim cExtendedLimitAmount As Decimal
        Dim oRI, oORI As cSIRRIControls.RiskRIArrangement
        Dim oIsRIRegeneration As Object
        Dim nRiskCnt As Integer
        Dim bIsDeletedRatingSection As Boolean
        Dim nReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DisplayRIDetails"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure we clear the model before we check it, if no model is
            ' found the call will not update it to zero.
            m_lRIModelID = 0
            m_lXOLRIModelId = 0
            ' Get information for the active band

            nReturn = m_oBusiness.GetBandValues(lRIBandID:=m_lActiveBand, cSumInsured:=cSumInsured, cPremium:=cPremium, vRILines:=vRILines, lRIModelID:=m_lRIModelID, iFacPremiumMethod:=iFacPremiumMethod, cOriginalSumInsured:=cOriginalSumInsured, cOriginalPremium:=cOriginalPremium, vOriginalRILines:=vOriginalRILines, bIsextendedlimitApplied:=bIsextendedlimitApplied, cExtendedLimitAmount:=cExtendedLimitAmount, lXOLRIModelId:=m_lXOLRIModelId)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("GetBandValues", "Unable to set original reinsurance model")
            End If
            ' Create new ri summary
            oRI = New cSIRRIControls.RiskRIArrangement()
            oRI.IsOriginal = False
            oRI.SumInsured = cSumInsured
            oRI.Premium = cPremium

            oRI.ExtendedLimitamount = cExtendedLimitAmount
            oRI.IsExtendedLimitApplied = bIsextendedlimitApplied
            oRI.ReinsuranceLines = vRILines
            oRI.FACPremiumMethod = iFacPremiumMethod
            oRI.RIModelId = m_lRIModelID
            oRI.XOLRIModelId = m_lXOLRIModelId
            m_cTotalSumInsured = cSumInsured
            m_cGrossPremium = cPremium
            uctSummary.RIArrangementID = m_oBusiness.RIArrangementID ' PN 78513
            uctSummary.FilterType = 1
            m_lRiArrangementId = m_oBusiness.RIArrangementID

            ' Check if we have original reinsurance
            If Information.IsArray(vOriginalRILines) Then
                oORI = New cSIRRIControls.RiskRIArrangement()
                oORI.IsOriginal = True
                oORI.SumInsured = cOriginalSumInsured
                oORI.Premium = cOriginalPremium


                oORI.ReinsuranceLines = vOriginalRILines
            End If

            ' Populate reinsurance control
            uctRI.Agency = False
            uctRI.TransactionType = m_sTransactionType
            uctRI.RIVersionId = m_nRIVersionId

            nReturn = uctRI.SetProperties(oRI, oORI)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("uctRI.SetProperties", "Unable to set new reinsurance model")
            End If

            ' If we have original reinsurance populate it
            If Not (oORI Is Nothing) Then
                nReturn = uctORI.SetProperties(oORI)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctORI.SetProperties", "Unable to set original reinsurance model")
                End If
                SSTabHelper.SetTabVisible(tabRI, 1, True)
            Else
                ' Clear original control
                nReturn = uctORI.Clear()
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctORI.Clear", "Unable to clear original reinsurance")
                End If
                SSTabHelper.SetTabVisible(tabRI, 1, False)
            End If

            ' Set command button enabled states

            'cmdAddTreaty.Enabled = (tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            cmdAddFacXOL.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            'cmdAddTreatyXOL.Enabled = (tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdEdit.Enabled = False '(tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdView.Enabled = False '(tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdDelete.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)

            'PN 38421 Defensive Coding Required to Disable Add Prop Treaty and Add Treaty XOL Buttons
            cmdAddTreaty.Enabled = False
            cmdAddTreatyXOL.Enabled = False


            If m_iTask = PMEComponentAction.PMView Then
                cmdAddFAC.Enabled = False
                cmdAddFacXOL.Enabled = False
                cmdDelete.Enabled = False
            End If

            ' Update summary control, if necessary
            If SSTabHelper.GetSelectedIndex(tabRI) = 2 Then
                If uctSummary.RIModelID <> m_lRIModelID Then
                    nReturn = uctSummary.Clear()
                    If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("uctSummary.Clear", "Unable to clear ri model summary")
                    End If

                    ' Only populate if we have a model
                    If m_lRIModelID > 0 Then
                        nReturn = uctSummary.SetProperties(m_lRIModelID, v_lXOLRIModelId:=m_lXOLRIModelId)
                        If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("uctSummary.SetProperties", "Unable to set new ri model")
                        End If
                    End If
                End If
            End If

            nReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRIRegeneration, v_vBranch:=g_iSourceID, r_vUnderwriting:=oIsRIRegeneration)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("iPMFunc.getProductOptionValue", "Unable to get RI Regeneration product option")
            End If
            nRiskCnt = m_oBusiness.RiskId

            nReturn = m_oBusiness.IsRatingSectionDeleted(nRiskCnt:=nRiskCnt, nRIBand:=m_lActiveBand, bIsDeletedRatingSection:=bIsDeletedRatingSection)
            If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("IsRatingSectionDeleted", "IsRatingSectionDeleted failed")
            End If
            If oIsRIRegeneration = "1" And Not (m_bIsRiskDeleted Or bIsDeletedRatingSection) Then
                If m_sTransactionType <> "" Then
                    If m_lRIModelID = 0 And m_lXOLRIModelId = 0 Then
                        MessageBox.Show("RI Model not available for the financial period", "Reinsurance", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        m_bRIPending = True
                        m_lStatus = gPMConstants.PMEReturnCode.PMCancel
                    End If
                End If
            End If

        Catch ex As Exception
            nResult = gPMConstants.PMEReturnCode.PMTrue
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
            Return nResult
        End Try
        Return nResult

    End Function

    ' ***************************************************************** '
    ' Process party find and sets the return values.
    ' ***************************************************************** '
    Private Function GetFacDefaults(ByVal lPartyCnt As Integer, ByRef dCommission As Double, ByRef bIsRiBroker As Boolean) As Integer
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

            lReturn = oPartyIn.GetNext(vPartyCnt:=lPartyCnt, vDefaultCommRate:=dCommission, vIsRIBroker:=bIsRiBroker)
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

            oFindParty.CallingAppName = "iPMUReinsurance2007"

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
                    'Developer Guide No. 
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
    ' Process to Delete the Selected Row from the Grid.
    ' ***************************************************************** '
    Private Function ProcessDeleteRow(Optional ByVal bManualDeleted As Boolean = True) As Integer

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
                            'Developer Guide No.188
                            If uctRI.PartyCnt = vParticipantArray(iRow1, ACIBrokerAssociationPartyCnt) Then
                                vParticipantArray.DeleteRows(iRow1)
                                Exit For
                            End If
                        Next
                    Next
                End If
            End If
            ' Add to grid
            lReturn = uctRI.DeleteRow(m_iAction, bManualDeleted)
            Select Case lReturn
                Case gPMConstants.PMEReturnCode.PMTrue
                    ' All is good
                Case gPMConstants.PMEReturnCode.PMRecordInUse
                Case Else
                    result = gPMConstants.PMEReturnCode.PMFalse
            End Select


            ' Set focus to grid
            uctRI.grdRI_Enter()


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            'Debugger.Break()
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessNewTreaty(), excep:=ex)

        Finally



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
        Dim dDefaultComm As Double
        Dim bIsRiBroker As Boolean

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessNewParty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            bIsRiBroker = False
            ' Process the party find to set the values.
            If m_sPartyType = "FAP" Then
                lReturn = CType(ProcessFindParty(sPartyName:=sPartyName, lPartyCnt:=lPartyCnt), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    Return result
                End If

                ' Check if we have valid values.
                If lPartyCnt > 0 Then
                    ' Get the default commission rate
                    ' Note: If we fail or error ignore, just fill in rate as 0 as the
                    '  problem will be logged and isn't serious enough to stop working
                    lReturn = CType(GetFacDefaults(lPartyCnt, dDefaultComm, bIsRiBroker), gPMConstants.PMEReturnCode)

                    ' Add to grid
                    lReturn = uctRI.AddFacultative(lPartyCnt, sPartyName, dDefaultComm, bIsRiBroker)
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
        Dim dCommission As Double
        Dim bIsRetained As Boolean

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessNewTreaty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'Developer Guide No. 69
            frmTreaty = New frmTreaty
            If TransactionType = "T" Then
                frmTreaty.TransactionType = "T"
            ElseIf TransactionType = "TX" Then
                frmTreaty.TransactionType = "TX"
            End If

            ' Show treaty dialog
            'Debugger.Break()
            frmTreaty.ShowDialog()

            ' Check status
            If frmTreaty.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Get and check treaty id
                lTreatyID = frmTreaty.TreatyId
                If lTreatyID > 0 Then
                    ' Get additional treaty information

                    lReturn = m_oBusiness.GetTreatyInfo(lTreatyID:=lTreatyID, sCode:=sCode, sAgreementCode:=sAgreementCode, dCommissionPercent:=dCommission, bIsRetained:=bIsRetained)
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.GetTreatyInfo", "Unable to get treaty information")
                    End If

                    ' Add to grid
                    lReturn = uctRI.AddTreaty(lTreatyID, sCode, dCommission, sAgreementCode, bIsRetained, TransactionType)
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

            vParticipantArray = New XArrayHelper()
            vParticipantArray.RedimXArray(New Integer() {-1, 4}, New Integer() {0, 0})

            ' Ensure proper display of tabs
            SSTabHelper.SetSelectedIndex(tabRI, 0)
            tabRI_SelectedIndexChanged(tabRI, New EventArgs())


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            ''Debugger.Break()
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
            uctRI.SelectedRIType = ""
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
                If uctRI.Visible Then
                    uctRI.grdRI_Enter()
                End If
                If uctORI.Visible Then
                    uctORI.grdRI_Enter()
                End If
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

    Private Sub cboRIVersionType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboRIVersionType.SelectedIndexChanged

        Dim nReturn As PMEReturnCode

        ' Check active band
        If m_nRIVersion <> VB6.GetItemData(cboRIVersionType, cboRIVersionType.SelectedIndex) Then
            ' Store current data
            ' Set new band and refresh data
            m_nRIVersion = VB6.GetItemData(cboRIVersionType, cboRIVersionType.SelectedIndex)
            txtEffectiveDate.Text = m_oRIVersion(2, cboRIVersionType.SelectedIndex)
            m_oBusiness.RIVersionId = m_nRIVersion

            nReturn = GetBusiness()
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError("DataToBusiness", "Unable to store new ri details")
            End If

            nReturn = DisplayRIDetails()
            If nReturn <> PMEReturnCode.PMTrue Then
                RaiseError("DisplayRIDetails", "Unable to display ri details")
            End If
        End If

    End Sub

    Private Sub cmdAddFAC_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFAC.Click
        ' Delegate
        m_sPartyType = "FAP"
        ProcessNewParty()
    End Sub

    Private Sub cmdAddFacXOL_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFacXOL.Click
        m_sPartyType = "FAX"
        m_iAction = 1 'Add
        ProcessNewParty()
    End Sub

    Private Sub cmdAddTreaty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTreaty.Click
        ' Delegate
        ProcessNewTreaty("T")
    End Sub

    Private Sub cmdAddTreatyXOL_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTreatyXOL.Click
        ' Delegate
        ProcessNewTreaty("TX")
    End Sub

    Private Sub cmdDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdDelete.Click
        m_iAction = 4
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

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lValid, lBand As Integer
        Dim bValid As Boolean
        Dim lReturn As Integer
        Const kMethodName As String = "cmdOK_Click"


        Try

            ' Set the interface status.
            m_lStatus = gPMConstants.PMEReturnCode.PMOK

            ' Should not be visible but check, just in case
            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                Me.Hide()
                Exit Sub
            End If

            If Not (uctRI.RowCountFAC = 0 And uctRI.RowCountTreaty = 0 And uctRI.RowCountRetained = 0) Then

                m_lReturn = uctRI.ValidateRILines(bValid)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("OK Click", "Unable to validate RI Lines")
                End If

                If Not bValid Then
                    Exit Sub
                End If

            End If

            ' Ensure edits are committed
            uctRI.FinaliseEdit()

            ' Store current data
            lReturn = DataToBusiness()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DataToBusiness", "Unable to store new ri details")
            End If

            ' Validate complete allocation
            'lReturn = m_oBusiness.ValidateBands( _
            'r_lValid:=lValid, _
            'r_lBand:=lBand)
            If Math.Round(uctRI.UnallocatedRI, 4) <> 0 Then
                lValid = 1
            End If
            If uctRI.UnallocatedPremium <> 0 Then
                lValid = 2
            End If
            Select Case True
                Case lReturn <> gPMConstants.PMEReturnCode.PMTrue
                    Interaction.MsgBox("Unable to validate bands", VariantType.Error, "Reinsurance")
                    Exit Sub
                Case lValid = 1
                    iPMFunc.SetComboBoxValue(cboRIBand, CStr(lBand))
                    MessageBox.Show("Sum insured share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                Case lValid = 2
                    iPMFunc.SetComboBoxValue(cboRIBand, CStr(lBand))
                    MessageBox.Show("Premium share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
            End Select


            If Not (m_vDeletedRILineIds Is Nothing) Then
                ' Set the Property for Deleted RI Lines at Business Level

                m_oBusiness.DeletedRIArrangementIds = VB6.CopyArray(m_vDeletedRILineIds)
            End If

            ' Process the next set of actions depending upon the interface task etc.
            lReturn = m_oGeneral.ProcessCommand()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If

            lReturn = CheckAllocationAllRIBands()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                cboRIBand_SelectedIndexChanged(cboRIBand, New EventArgs())
                Exit Sub
            End If


            m_lReturn = m_oBusiness.UpdatePremiumPercent()

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Exit Sub
            End If


            lReturn = m_oBusiness.ChangeRiskStatus()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.ChangeRiskStatus", "Unable to change risk status")
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
                'Start PN 44646
                If MessageBox.Show("Any changes to the Reinsurance model's made" & Strings.Chr(13) & Strings.Chr(10) &
                                   "in this transaction will be lost and it will revert" & Strings.Chr(13) & Strings.Chr(10) &
                                   "back to its original state.", "Cancel Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                    If Information.IsArray(m_vAdedFindRIPartyLines) Then

                        For lCount As Integer = m_vAdedFindRIPartyLines.GetLowerBound(0) To m_vAdedFindRIPartyLines.GetUpperBound(0)

                            m_lReturn = m_oBusiness.DeleteRILines(gPMFunctions.ToSafeLong(m_vAdedFindRIPartyLines(lCount)))
                        Next
                    End If
                Else
                    Exit Sub
                End If
                'END PN 44646

                ' Ensure edits are committed
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
            uctORI.Visible = (SSTabHelper.GetSelectedIndex(tabRI) = 1)
            uctSummary.Visible = (SSTabHelper.GetSelectedIndex(tabRI) = 2)

            If (uctORI.Visible) Then
                uctORI.grdRI_Enter()
            End If

            ' Set enabled states
            'cmdAddTreaty.Enabled = (tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            cmdAddFacXOL.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            'cmdAddTreatyXOL.Enabled = (tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdView.Enabled = False '(tabRI.Tab = 0) And (Not uctRI.ReadOnly)
            cmdDelete.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            cmdEdit.Enabled = False '(tabRI.Tab = 0) And (Not uctRI.ReadOnly)

            cmdAddTreaty.Enabled = False
            cmdAddTreatyXOL.Enabled = False

            If m_iTask = PMEComponentAction.PMView Then
                cmdAddFAC.Enabled = False
                cmdAddFacXOL.Enabled = False
                cmdDelete.Enabled = False
            End If

            ' Update summary control, if necessary
            If SSTabHelper.GetSelectedIndex(tabRI) = 2 Then
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



        End Try
    End Sub

    Private Sub uctRI_GotFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctRI.GotFocus
        VB6.SetDefault(cmdOK, False)
    End Sub

    Private Sub uctRI_LostFocus(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles uctRI.LostFocus
        VB6.SetDefault(cmdOK, True)
    End Sub

    Private Sub uctRI_RecalculateFacTax(ByVal Sender As Object, ByRef e As uctRiskRIControlRI2007.RecalculateFacTaxEventArgs) Handles uctRI.RecalculateFacTax

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "uctRI_RecalculateFacTax"


        Try

            ' Pass down to business object

            lReturn = m_oBusiness.CalculateFacTax(v_lArrangementLineID:=e.lArrangementLineID, v_lPartyCnt:=e.lPartyCnt, v_cPremium:=e.cPremium, v_cCommission:=e.cCommission, r_cPremiumTax:=e.cPremiumTax, r_cCommissionTax:=e.cCommTax)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CalculateFacTax", "Unable to recalculate facultative taxes")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub uctRI_ResetControls(ByVal Sender As Object, ByVal e As uctRiskRIControlRI2007.ResetControlsEventArgs) Handles uctRI.ResetControls
        Dim lReturn As Integer
        Const kMethodName As String = "uctRI_ResetControls"


        Try
            If m_sTransactionType <> "" Then
                If e.SelRIType = "FX" Or e.SelRIType = "F" Then
                    cmdView.Enabled = True
                    cmdEdit.Enabled = True
                    cmdDelete.Enabled = True
                Else
                    cmdView.Enabled = False
                    cmdEdit.Enabled = False
                    cmdDelete.Enabled = False
                End If
            Else
                cmdView.Enabled = True
                cmdEdit.Enabled = False
            End If




        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub uctRI_RecalculateTreatyTax(ByVal Sender As Object, ByRef e As uctRiskRIControlRI2007.RecalculateTreatyTaxEventArgs) Handles uctRI.RecalculateTreatyTax

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "uctRI_RecalculateTreatyTax"


        Try

            ' Pass down to business object

            lReturn = m_oBusiness.CalculateTreatyTax(v_lArrangementLineID:=e.lArrangementLineID, v_lTreatyID:=e.lTreatyID, v_cPremium:=e.cPremium, v_cCommission:=e.cCommission, r_cPremiumTax:=e.cPremiumTax, r_cCommissionTax:=e.cCommTax)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("CalculateFacTax", "Unable to recalculate treaty taxes")
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

        Finally


        End Try
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

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReinsuranceRI2007.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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
            m_oGeneral = New iPMUReinsurance.General()

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
            m_lReturn = m_oGeneral.Initialise(frmInterface:=Me, oBusiness:=m_oBusiness)
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


    Private Sub frmInterfaceRI2007_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        If (uctRI.Visible) Then
            uctRI.grdRI_Enter()
        End If

        If (uctORI.Visible) Then
            uctORI.grdRI_Enter()
        End If
    End Sub

    Public Sub frmInterfaceLoad()
        Dim bApplyReinsurance As Boolean

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

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetProcessModes", "Failed to set the process modes for the business object")
            End If

            ' Set the status for the business object.

            lReturn = m_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetStatus", "Failed to set the status for the business object")
            End If

            ' Set the business keys.

            m_oBusiness.InsuranceFileCnt = m_lInsuranceFileCnt

            m_oBusiness.RiskID = m_lRiskID


            lReturn = m_oBusiness.ApplyReinsurance(bApplyReinsurance)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.ApplyReinsurance", "Unable to check auto reinsurance flag")
            End If

            If Not bApplyReinsurance Then
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse

                lReturn = m_oBusiness.ChangeRiskStatus
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.ChangeRiskStatus", "Unable to change risk status where no reinsurance applies")
                End If

                Exit Sub
            End If

            ' Initialise user controls
            'Developer Guide No. 9
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
            lReturn = CType(IsDisplayRI(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("frmInterface.IsDisplayRI", "IsDisplayRI function fail to retrive the value")
            End If

            If m_iTask = gPMConstants.PMEComponentAction.PMView Then
                cboRIVersionType.Visible = True
                cboRIVersionType.Enabled = True
                txtEffectiveDate.Visible = True
            Else
                cboRIVersionType.Visible = False
                cboRIVersionType.Enabled = False
                txtEffectiveDate.Visible = False
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


            If m_iTask <> gPMConstants.PMEComponentAction.PMView And UnloadMode <> vbFormCode Then
                'Start PN 44646
                If MessageBox.Show("Any changes to the Reinsurance model's made" & Strings.Chr(13) & Strings.Chr(10) &
                                   "in this transaction will be lost and it will revert" & Strings.Chr(13) & Strings.Chr(10) &
                                   "back to its original state.", "Cancel Search", MessageBoxButtons.YesNo, MessageBoxIcon.Question, MessageBoxDefaultButton.Button2) = System.Windows.Forms.DialogResult.Yes Then
                    If Information.IsArray(m_vAdedFindRIPartyLines) Then

                        For lCount As Integer = m_vAdedFindRIPartyLines.GetLowerBound(0) To m_vAdedFindRIPartyLines.GetUpperBound(0)

                            m_lReturn = m_oBusiness.DeleteRILines(gPMFunctions.ToSafeLong(m_vAdedFindRIPartyLines(lCount)))
                        Next
                    End If
                Else
                    Cancel = 1
                    eventArgs.Cancel = True
                    Exit Sub
                End If
                'END PN 44646
            End If

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



        End Try
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterfaceRI2007_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        'If isInitializingComponent Then
        '    Exit Sub
        'End If

        'Try
        '    ' Resize tabs
        '    tabRI.SetBounds(VB6.TwipsToPixelsX(90), VB6.TwipsToPixelsY(570), ClientRectangle.Width - VB6.TwipsToPixelsX(180), ClientRectangle.Height - VB6.TwipsToPixelsY(1080))

        '    ' Resize tab controls
        '    'TODO
        '    'uctRI.Move(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    'uctORI.Move(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    'uctSummary.Move(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    uctRI.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    uctORI.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    uctSummary.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)


        '    ' Move command buttons
        '    cmdOK.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(2580), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdCancel.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1290), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        '    cmdAddTreaty.SetBounds(VB6.TwipsToPixelsX(120), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdAddFAC.SetBounds(VB6.TwipsToPixelsX(1785), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        '    cmdAddTreatyXOL.SetBounds(VB6.TwipsToPixelsX(3405), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdAddFacXOL.SetBounds(VB6.TwipsToPixelsX(5010), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        '    cmdView.SetBounds(VB6.TwipsToPixelsX(6615), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdEdit.SetBounds(VB6.TwipsToPixelsX(7995), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdDelete.SetBounds(VB6.TwipsToPixelsX(9360), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        'Catch exc As System.Exception
        '    NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        'End Try
    End Sub

    Public Function IsDisplayRI() As Integer

        Dim result As Integer = 0
        Dim vlsDisplayed As Boolean
        'Dim oBusiness As Object
        Dim lReturn As Integer
        Const kMethodName As String = "IsDisplayRI"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            '     m_lReturn = g_oObjectManager.GetInstance( _
            ''        oObject:=oBusiness, _
            ''        sClassName:="bSIRReinsuranceRI2007.Form", _
            ''        vInstanceManager:=PMGetViaClientManager)
            '    If m_lReturn <> PMTrue Then
            '        ' Failed to get an instance of the business object.
            '        m_lErrorNumber = PMFalse
            '        RaiseError "g_oObjectManager.GetInstance", "Unable to get instance of bSIRReinsuranceRI2007.Form"
            '        Exit Function
            '    End If



            m_lReturn = m_oBusiness.checkifdisplayedRI(iRiskType:=m_lRiskID, r_blsDisplayed:=vlsDisplayed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If gPMFunctions.ToSafeBoolean(vlsDisplayed) Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                DisplayScreen = True
            ElseIf Not gPMFunctions.ToSafeBoolean(vlsDisplayed) Then
                If gPMFunctions.ToSafeCurrency(uctRI.UnallocatedRI) = 0 Then
                    DisplayScreen = False

                    lReturn = m_oBusiness.ChangeRiskStatus()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.ChangeRiskStatus", "Unable to change risk status")
                    End If
                ElseIf gPMFunctions.ToSafeCurrency(uctRI.UnallocatedRI) <> 0 Then
                    DisplayScreen = True
                End If
            End If
            If m_bRIPending = True Then
                DisplayScreen = False
            End If

            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            Return result
        End Try
    End Function

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
        Dim sAgreementCode As String = ""

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

            oFindRIParty.InsuranceFileCnt = m_lInsuranceFileCnt

            oFindRIParty.TotalSumInsured = m_cTotalSumInsured - uctRI.TotalSumInsuredFacProp

            oFindRIParty.FACPropExists = uctRI.FACPropExists

            oFindRIParty.GrossPremium = m_cGrossPremium

            If m_sTransactionType <> "NB" And m_sTransactionType <> "REN" Then
                If Information.IsArray(m_vGroupingIDs) Then
                    For iRow = 0 To m_vGroupingIDs.GetUpperBound(1)
                        If uctRI.GroupingID = CDbl(gPMFunctions.ToSafeString(m_vGroupingIDs(0, iRow))) Then

                            oFindRIParty.AddMode = "AU"
                            Exit For
                        End If
                    Next
                End If
            End If

            ' Set the process modes.

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
                            'Developer Guide No.188
                            If ToSafeInteger(vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)) = uctRI.PartyCnt Then
                                lCount += 1
                            End If
                        Next
                        If lCount > 0 Then
                            ReDim m_vSelectedArray(lCount - 1, 4)
                            iRow1 = 0
                            For iRow = 0 To vParticipantArray.GetUpperBound(0)
                                'Developer Guide No.188
                                'start
                                If ToSafeInteger(vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)) = uctRI.PartyCnt Then

                                    m_vSelectedArray(iRow1, ACIBrokerShortName) = vParticipantArray(iRow, ACIBrokerShortName)

                                    m_vSelectedArray(iRow1, ACIBrokerLongName) = vParticipantArray(iRow, ACIBrokerLongName)

                                    m_vSelectedArray(iRow1, ACIBrokerParticipant_percent) = vParticipantArray(iRow, ACIBrokerParticipant_percent)

                                    m_vSelectedArray(iRow1, ACIBrokerAssociationPartyCnt) = vParticipantArray(iRow, ACIBrokerAssociationPartyCnt)

                                    m_vSelectedArray(iRow1, ACIBrokerPartyCnt) = vParticipantArray(iRow, ACIBrokerPartyCnt)
                                    'end
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

                m_cPremium = oFindRIParty.Premium

                m_cPremiumTax = oFindRIParty.PremiumTax

                m_cCommission = oFindRIParty.Commission

                m_dComm_percent = oFindRIParty.Comm_percent

                m_cCommTax = oFindRIParty.CommTax


                m_cSumInsured = oFindRIParty.SumInsured * (100 - oFindRIParty.Retained_Percent) / 100

                m_lGroupingId = oFindRIParty.GroupingId

                sAgreementCode = oFindRIParty.AgreementCode 'Sankar - PN 50348

                If lPartyCnt = 0 And sPartyName = "Multiple Acts" Then
                    m_bIsMultiActs = True
                End If

                If v_iTask = gPMConstants.PMEComponentAction.PMEdit Then
                    m_lReturn = ProcessDeleteRow(bManualDeleted:=False)
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

                    'Sankar - PN 50348 - Added sAgreementCode
                    lReturn = uctRI.AddFacultativeXOL(lPartyCnt:=lPartyCnt, sDescription:=sPartyName, dRetained:=m_dRetained_percent, cLowerLimit:=m_cLowerLimit, cUpperLimit:=m_cUpperLimit, cSumInsured:=m_cSumInsured, cPremium:=m_cPremium, cPremiumTax:=m_cPremiumTax, dCommPercent:=m_dComm_percent, cComm:=m_cCommission, cCommTax:=m_cCommTax, lGroupingId:=m_lGroupingId, sAgreementCode:=sAgreementCode)
                    Select Case lReturn
                        Case gPMConstants.PMEReturnCode.PMTrue
                            uctRI.IsDirty = True
                            ' All is good
                        Case gPMConstants.PMEReturnCode.PMRecordInUse
                            MessageBox.Show("'" & sPartyName & "' is already present in this arrangement", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                        Case Else
                            gPMFunctions.RaiseError("uctRI.AddFacultative", "Unable to add facultative reinsurer")
                            result = gPMConstants.PMEReturnCode.PMFalse
                    End Select
                    '            If v_iTask = PMEdit Then
                    '                uctRI.DeletedRIArragementIds = Empty
                    '            End If

                End If
            ElseIf (oFindRIParty.Status = gPMConstants.PMEReturnCode.PMOK) And m_sPartyType = "FAP" And v_iTask = gPMConstants.PMEComponentAction.PMEdit Then


                vBrokerArray = oFindRIParty.BrokerArray

                If vParticipantArray.GetUpperBound(0) > -1 Then
                    lCount = vParticipantArray.GetUpperBound(0)
                    For iRow = 0 To lCount
                        For iRow1 = 0 To vParticipantArray.GetUpperBound(0)
                            If uctRI.PartyCnt = ToSafeInteger(vParticipantArray(iRow1, ACIBrokerAssociationPartyCnt)) Then
                                vParticipantArray.DeleteRows(iRow1)
                                Exit For
                            End If
                        Next
                    Next
                End If
                If Information.IsArray(vBrokerArray) Then

                    For lCount = 0 To vBrokerArray.GetUpperBound(0)
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
                m_cPremium = 0
                m_cPremiumTax = 0
                m_cCommission = 0
                m_dComm_percent = 0
                m_cCommTax = 0
                m_cSumInsured = 0
                m_lGroupingId = 0
            End If



            m_vAdedFindRIPartyLines = oFindRIParty.AddedFindRIPartyLines 'PN 44646


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

    Private Function CheckAllocationAllRIBands() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "CheckAllocationAllRIBands"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            For lCount As Integer = 0 To cboRIBand.Items.Count - 1
                ' Set new band and refresh data
                m_lActiveBand = VB6.GetItemData(cboRIBand, lCount)

                lReturn = CType(DisplayRIDetails(), gPMConstants.PMEReturnCode)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("DisplayRIDetails", "Unable to display ri details")
                End If
                cboRIBand.SelectedIndex = lCount
                If uctRI.UnallocatedPremium > 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Premium share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                ElseIf Math.Round(uctRI.UnallocatedRI, 4) > 0 Then
                    result = gPMConstants.PMEReturnCode.PMFalse
                    MessageBox.Show("Sum insured share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Return result
                End If

            Next


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function

    Private Sub frmInterfaceRI2007_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
        Select Case e.KeyCode
            Case Keys.Escape
                cmdCancel.Focus()
                cmdCancel.PerformClick()
        End Select

    End Sub
    ''' <summary>
    ''' ProcessOKClickForSilentQuote
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ProcessOKClickForSilentQuote() As Integer
        Const kMethodName As String = "ProcessOKClickForSilentQuote"

        Dim nValid As Integer
        Dim nBand As Integer
        Dim nReturn As Integer = PMEReturnCode.PMTrue

        Try
            ' Set the interface status.
            m_lStatus = PMEReturnCode.PMOK

            ' Ensure edits are committed
            uctRI.FinaliseEdit()

            ' Store current data
            nReturn = DataToBusiness()
            If nReturn <> PMEReturnCode.PMTrue Then
                HandleErrorOnSilentQuote(sMsg:="DataToBusiness Failed",
                                         sMethod:=kMethodName)
                Return nReturn
            End If

            ' Validate complete allocation
            nReturn = m_oBusiness.ValidateBands(
                r_lValid:=nValid,
                r_lBand:=nBand)
            Select Case True
                Case nReturn <> PMEReturnCode.PMTrue
                    HandleErrorOnSilentQuote(sMsg:="bSIRReinsurance.Form.ValidateBands Failed",
                                             sMethod:=kMethodName)
                    Return nReturn
                Case nValid = 1
                    Call SetComboBoxValue(cboRIBand, nBand)
                    HandleErrorOnSilentQuote(sMsg:="Sum insured share for '" & cboRIBand.Text & "' is not 100%",
                                             sMethod:=kMethodName)
                    Return nReturn
                Case nValid = 2
                    Call SetComboBoxValue(cboRIBand, nBand)
                    HandleErrorOnSilentQuote(sMsg:="Premium share for '" & cboRIBand.Text & "' is not 100%",
                                             sMethod:=kMethodName)
                    Return nReturn
            End Select

            ' Process the next set of actions depending upon the interface task etc.
            nReturn = m_oGeneral.ProcessCommand()
            If nReturn <> PMEReturnCode.PMTrue Then
                HandleErrorOnSilentQuote(sMsg:="m_oGeneral.ProcessCommand Failed",
                        sMethod:=kMethodName)
                Return nReturn
            End If

            nReturn = m_oBusiness.ChangeRiskStatus()
            If nReturn <> PMEReturnCode.PMTrue Then
                HandleErrorOnSilentQuote(sMsg:="bSIRReinsurance.Form..ChangeRiskStatus Failed",
                        sMethod:=kMethodName)
            End If
            Return nReturn
        Catch ex As Exception
            Throw New Exception(kMethodName + " Failed", ex)
        End Try


    End Function

    ''' <summary>
    '''HandleErrorOnSilentQuote
    ''' </summary>
    ''' <param name="sMsg"></param>
    ''' <param name="sMethod"></param>
    ''' <remarks></remarks>
    Private Sub HandleErrorOnSilentQuote(ByVal sMsg As String,
                                         ByVal sMethod As String)

        Const kMethodName As String = "HandleErrorOnSilentQuote"

        Try
            iPMFunc.LogMessage(iType:=PMELogLevel.PMLogOnError, sMsg:=sMsg,
                       vApp:=ACApp, vClass:=ACClass,
                       vMethod:=sMethod, bSilent:=True)
            m_lStatus = PMEReturnCode.PMCancel
        Catch ex As Exception
            iPMFunc.LogMessage(sUsername:=g_oObjectManager.UserName,
                                           iType:=PMELogLevel.PMLogOnError,
                                           sMsg:=kMethodName + " Failed",
                                           vApp:=ACApp, vClass:=ACClass,
                                           vMethod:=kMethodName,
                                           vErrNo:=Err().Number,
                                           vErrDesc:=ex.Message, excep:=ex)
        End Try
    End Sub

    Private Sub HandleErrorOnSilentQuote(ByVal v_sMsg As String,
                                    ByVal v_vMethod As Object)

        Dim lReturn As Long
        Const kMethodName As String = "HandleErrorOnSilentQuote"

        Try
            'Try to Log the message Silently
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=v_sMsg, vApp:=ACApp, vClass:=ACClass, vMethod:=v_vMethod, bSilent:=True)
            'Set the status to PMCancel
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        Catch
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn)
        End Try
    End Sub
    'End - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enh)
End Class