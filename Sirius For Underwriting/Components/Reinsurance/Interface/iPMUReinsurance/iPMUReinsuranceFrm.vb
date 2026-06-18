Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide  no 129. 
'Start
Imports SharedFiles
Imports cSIRRIControls

'End
Partial Friend Class frmInterface
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
    'Developer Guide No.69
    Dim frmTreaty As frmTreaty
    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
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
    '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
    Dim m_vProductValue As String = ""
    Private m_vProductArray(,) As Object

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

                    If ToSafeCurrency(uctRI.UnallocatedRI) = 0 Then
                        cboRIBand.SelectedIndex = lCount
                        'cboRIBand.ListIndex = lCount
                    End If
                Next lCount

                ' Set default item, this will trigger population of everything else
                cboRIBand.SelectedIndex = 0
            Else
                ' Not found, empty bands and disable other controls
                cboRIBand.Items.Clear()
                uctRI.ReadOnly_Renamed = True
                uctORI.ReadOnly_Renamed = True
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
        Dim vRILines As Object

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

                ' Update the business object

                lReturn = m_oBusiness.EditUpdate(lRIBandID:=m_lActiveBand, vRILines:=vRILines)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.EditUpdate", "Failed to update business object with new reinsurance")
                End If
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
        Dim bSIRProduct, bSIRPartyFee As Object

        Dim lReturn As gPMConstants.PMEReturnCode

        '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
        Dim bIsRiskRIArrangementExist As Boolean
        'Start Arul-PN 70641

        Dim obSIRPartyFee As bSIRPartyFee.UBusiness
        Dim vRiskData As Object
        Dim crRiskSumInsured, crRiskPremium, crRISumInsured, crRIPremium As Decimal
        Const kRiskSumInsured As Integer = 16
        Const kRiskPremium As Integer = 17
        'End Arul-PN 70641

        Const kMethodName As String = "ProcessAutoRI"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim m_oProduct As bSIRProduct.Business


            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oProduct As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oProduct, "bSIRProduct.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oProduct = temp_m_oProduct
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Fail to Initialize bSIRProduct", gPMConstants.PMELogLevel.PMLogError)
            End If


            'Start-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)

            m_lReturn = m_oProduct.GetProductDetailsForPolicy(v_lInsuranceFileCnt:=m_lInsuranceFileCnt, r_vProductArray:=m_vProductArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Or Not Information.IsArray(m_vProductArray) Then
                gPMFunctions.RaiseError(kMethodName, "Unable to fetch the product value for the given" &
                                        "insurancefilecnt from the method GetProductValueFromInsuranceFileID", gPMConstants.PMELogLevel.PMLogError)
            Else
                m_vProductValue = CStr(m_vProductArray(ACRIManualPremiumAdjustment, 0))
            End If

            'Arul-PN 70641-Two parameters have been added

            m_lReturn = m_oBusiness.ChecktheExistenceofRIArrangement(v_lRiskCnt:=m_lRiskID, r_bIsRiskRIArrangementExist:=bIsRiskRIArrangementExist, r_crRISumInsured:=crRISumInsured, r_crRIPremium:=crRIPremium)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Unable to check the existence of RI Arrangement value " &
                                        "in the method ChecktheExistenceofRIArrangement", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Start-Arul-PN 70641
            Dim temp_obSIRPartyFee As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_obSIRPartyFee, "bSIRPartyFee.UBusiness", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            obSIRPartyFee = temp_obSIRPartyFee
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Failed to Initialize bSIRPartyFee", gPMConstants.PMELogLevel.PMLogError)
            End If


            m_lReturn = obSIRPartyFee.GetRiskDetails(v_lRiskCnt:=m_lRiskID, r_vResults:=vRiskData)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "The method GetRiskDetails failed to fetch the Risk details", gPMConstants.PMELogLevel.PMLogError)
            ElseIf Not gPMFunctions.IsArrayEmpty(vRiskData) Then

                crRiskSumInsured = CDec(vRiskData(kRiskSumInsured, 0))

                crRiskPremium = CDec(vRiskData(kRiskPremium, 0))
            End If
            bIsRiskRIArrangementExist = gPMFunctions.ToSafeCurrency(crRiskSumInsured) = gPMFunctions.ToSafeCurrency(crRISumInsured) And gPMFunctions.ToSafeCurrency(crRIPremium) = gPMFunctions.ToSafeCurrency(crRiskPremium) And bIsRiskRIArrangementExist
            'End-Arul-PN 70641

            If m_sTransactionType = "REN" OrElse Not (bIsRiskRIArrangementExist And m_vProductValue = "1") Then
                'End-(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)

                lReturn = m_oBusiness.CalculateRI
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("m_oBusiness.CalculateRI", "Unable to auto calculate reinsurance")
                End If
                '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)
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

        Dim result As Integer = 0
        Dim cSumInsured, cPremium As Decimal
        Dim vRILines As Object
        Dim iFacPremiumMethod As cSIRRIControls.RiskRIArrangement.FACPremiumEnum
        Dim cOriginalSumInsured, cOriginalPremium As Decimal
        Dim vOriginalRILines As Object

        Dim oRI, oORI As cSIRRIControls.RiskRIArrangement

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "DisplayRIDetails"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure we clear the model before we check it, if no model is
            ' found the call will not update it to zero.
            m_lRIModelID = 0

            ' Get information for the active band

            lReturn = m_oBusiness.GetBandValues(lRIBandID:=m_lActiveBand, cSumInsured:=cSumInsured, cPremium:=cPremium, vRILines:=vRILines, lRIModelID:=m_lRIModelID, iFacPremiumMethod:=iFacPremiumMethod, cOriginalSumInsured:=cOriginalSumInsured, cOriginalPremium:=cOriginalPremium, vOriginalRILines:=vOriginalRILines)

            ' Create new ri summary
            oRI = New cSIRRIControls.RiskRIArrangement()
            oRI.IsOriginal = False
            oRI.SumInsured = cSumInsured
            oRI.Premium = cPremium


            oRI.ReinsuranceLines = vRILines
            oRI.FACPremiumMethod = iFacPremiumMethod
            oRI.TransactionType = m_sTransactionType
            '(Arul Stephen)-(Tech Spec - HGPS001 Reinsurance Modifications)-(6.3.1.1)
            'Developer Guide No. 131
            If Not String.IsNullOrEmpty(m_vProductValue) Then
                oRI.RIManualPremiumAdjustments = CInt(m_vProductValue)
            End If
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
            lReturn = uctRI.SetProperties(oRI, oORI)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("uctRI.SetProperties", "Unable to set new reinsurance model")
            End If

            ' If we have original reinsurance populate it
            If Not (oORI Is Nothing) Then
                lReturn = uctORI.SetProperties(oORI)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctORI.SetProperties", "Unable to set original reinsurance model")
                End If
                SSTabHelper.SetTabVisible(tabRI, 1, True)
            Else
                ' Clear original control
                lReturn = uctORI.Clear()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctORI.Clear", "Unable to clear original reinsurance")
                End If
                SSTabHelper.SetTabVisible(tabRI, 1, False)
            End If

            ' Set command button enabled states

            cmdAddTreaty.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)


            If m_iTask = PMEComponentAction.PMView Then
                cmdAddTreaty.Enabled = False
                cmdAddFAC.Enabled = False
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
                        lReturn = uctSummary.SetProperties(m_lRIModelID)
                        If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError("uctSummary.SetProperties", "Unable to set new ri model")
                        End If
                    End If
                End If
            End If


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
    Private Function GetFacDefaults(ByVal lPartyCnt As Integer, ByRef dCommission As Double) As Integer
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

            lReturn = oPartyIn.GetNext(vPartyCnt:=lPartyCnt, vDefaultCommRate:=dCommission)
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

        Dim lReturn As Integer
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

            oFindParty.CallingAppName = m_sCallingAppName

            ' Set the process modes.

            lReturn = oFindParty.SetProcessModes(vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oPartyIn.SetProcessModes", "Unable to set process modes on iPMBFindParty.Interface")
            End If

            ' Set special party type and start interface

            oFindParty.SpecialParty = PMBConst.PMBPartyTypeInsurer

            lReturn = oFindParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oFindParty.Start", "Unable to start Find Party interface")
            End If

            ' Check status and return either party details of empty values

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then

                lPartyCnt = oFindParty.PartyCnt

                sPartyName = oFindParty.ShortName
            Else
                lPartyCnt = 0
                sPartyName = ""
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
        Dim dDefaultComm As Double

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessNewParty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Process the party find to set the values.
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
                lReturn = CType(GetFacDefaults(lPartyCnt, dDefaultComm), gPMConstants.PMEReturnCode)

                ' Add to grid
                lReturn = uctRI.AddFacultative(lPartyCnt, sPartyName, dDefaultComm)
                Select Case lReturn
                    Case gPMConstants.PMEReturnCode.PMTrue
                        ' All is good
                    Case gPMConstants.PMEReturnCode.PMRecordInUse
                        MessageBox.Show("'" & sPartyName & "' is already present in this arrangement", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Case Else
                        gPMFunctions.RaiseError("uctRI.AddFacultative", "Unable to add facultative reinsurer")
                End Select
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
    Private Function ProcessNewTreaty() As Integer

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
            ' Show treaty dialog
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
                    lReturn = uctRI.AddTreaty(lTreatyID, sCode, dCommission, sAgreementCode, bIsRetained)
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

            ' Ensure proper display of tabs
            SSTabHelper.SetSelectedIndex(tabRI, 0)
            tabRI_SelectedIndexChanged(tabRI, New EventArgs())


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

    Private Sub cmdAddFAC_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddFAC.Click
        ' Delegate
        ProcessNewParty()
    End Sub

    Private Sub cmdAddTreaty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddTreaty.Click
        ' Delegate
        ProcessNewTreaty()
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim lValid, lBand As Integer

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

            ' Ensure edits are committed
            uctRI.FinaliseEdit()

            ' Store current data
            lReturn = DataToBusiness()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DataToBusiness", "Unable to store new ri details")
            End If

            ' Validate complete allocation

            lReturn = m_oBusiness.ValidateBands(r_lValid:=lValid, r_lBand:=lBand)
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

            ' Process the next set of actions depending upon the interface task etc.
            lReturn = m_oGeneral.ProcessCommand()
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
            cmdAddTreaty.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed)



            If m_iTask = PMEComponentAction.PMView Then
                cmdAddTreaty.Enabled = False
                cmdAddFAC.Enabled = False
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
                        lReturn = uctSummary.SetProperties(m_lRIModelID)
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

    Private Sub uctRI_RecalculateFacTax(ByVal Sender As Object, ByRef e As uctRiskRIControl.RecalculateFacTaxEventArgs) Handles uctRI.RecalculateFacTax

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

    Private Sub uctRI_RecalculateTreatyTax(ByVal Sender As Object, ByRef e As uctRiskRIControl.RecalculateTreatyTaxEventArgs) Handles uctRI.RecalculateTreatyTax

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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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


    Private Sub frmInterface_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        uctRI.grdRI_Enter()
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
        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lErrorNumber, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        End Try
    End Sub

    Private Sub frmInterface_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
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



        End Try
    End Sub

    Private isInitializingComponent As Boolean
    Private Sub frmInterface_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        'If isInitializingComponent Then
        '    Exit Sub
        'End If

        'Try

        '    ' Resize tabs
        '    tabRI.SetBounds(VB6.TwipsToPixelsX(90), VB6.TwipsToPixelsY(570), ClientRectangle.Width - VB6.TwipsToPixelsX(180), ClientRectangle.Height - VB6.TwipsToPixelsY(1080))

        '    ' Resize tab controls
        '    'uctRI.Move(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    'uctORI.Move(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    'uctSummary.Move(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    uctRI.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    uctORI.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
        '    uctSummary.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)

        '    ' Move command buttons
        '    cmdAddTreaty.SetBounds(VB6.TwipsToPixelsX(90), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdAddFAC.SetBounds(VB6.TwipsToPixelsX(1380), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdOK.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(2580), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
        '    cmdCancel.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1290), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        'Catch exc As System.Exception
        '    NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        'End Try
    End Sub

    Public Function IsDisplayRI() As Integer
        Dim result As Integer = 0

        Dim vlsDisplayed As Boolean

        Dim oBusiness As bSIRReinsurance.Form
        Dim lReturn As Integer
        Const kMethodName As String = "IsDisplayRI"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oBusiness, "bSIRReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            oBusiness = temp_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get an instance of the business object.
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to get instance of bSIRReinsurance.Form")
                Return result
            End If



            m_lReturn = oBusiness.CheckIfDisplayedRI(IRiskType:=m_lRiskID, r_blsDisplayed:=vlsDisplayed)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                ' Failed to get the details.
                Return gPMConstants.PMEReturnCode.PMTrue
            End If

            If gPMFunctions.ToSafeBoolean(vlsDisplayed) Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                DisplayScreen = True
            ElseIf Not gPMFunctions.ToSafeBoolean(vlsDisplayed) Then
                If gPMFunctions.ToSafeCurrency(uctRI.UnallocatedRI) = 0 AndAlso gPMFunctions.ToSafeCurrency(uctRI.UnallocatedPremium) = 0 Then
                    DisplayScreen = False

                    lReturn = m_oBusiness.ChangeRiskStatus()
                    If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError("m_oBusiness.ChangeRiskStatus", "Unable to change risk status")
                    End If
                ElseIf gPMFunctions.ToSafeCurrency(uctRI.UnallocatedRI) <> 0 OrElse gPMFunctions.ToSafeCurrency(uctRI.UnallocatedPremium) <> 0 Then
                    DisplayScreen = True
                End If
            End If
            Return result

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn, excep:=ex)

            Return result
        End Try
    End Function

    Private Sub frmInterface_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles MyBase.KeyDown
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

            Exit Sub
        Catch
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=lReturn)
        End Try
    End Sub
    'End - (Jai Prakash) - (WPR60_ReRate_All_Transaction_Risks-Enh)

End Class