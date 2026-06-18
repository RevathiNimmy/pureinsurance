Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmInterface
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name: frmInterface
    '
    ' Date: 04/03/1997
    '
    ' Description: Main interface.
    ' ***************************************************************** '
    'Developer Guide no. 50
    Dim frmTreaty As frmTreaty
    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmInterface"

    ' Object parameter members.
    Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode
    Private m_lErrorNumber As gPMConstants.PMEReturnCode

    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
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
    Private m_bOpenClaimNoTrans As Boolean

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
            '		Return result
            '		Resume 
            '		Return result
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
            '		Return result
            '		Resume 
            '		Return result
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
            '		Return result
            '		Resume 

            '		Return result
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
            '		Return result
            '		Resume 
            '		Return result
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


            m_lReturn = oPMLock.UnlockKey(sKeyName:="claim_id", vKeyValue:=v_lOriginalClaimID, iUserID:=g_oObjectManager.UserID)

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

    ' ***************************************************************** '
    ' Display the banding details for a given party.
    ' ***************************************************************** '
    Private Function DisplayRIDetails() As Integer

        Dim result As Integer = 0
        Dim cSumInsured, cReserveToDate, cPaymentToDate, cThisReserve, cThisPayment As Decimal
        Dim lRIModelID, lCatastropheCodeID, lXolClmModelID As Integer
        Dim cXolClmLimit As Decimal
        Dim lXolCatModelID As Integer
        Dim cXolCatLimit As Decimal
        Dim lXolCatReinstatements As Integer
        Dim vRILines As Object
        Dim oRI As cSIRRIControls.ClaimRIArrangement
        Dim lReturn As gPMConstants.PMEReturnCode

        Const kMethodName As String = "DisplayRIDetails"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Ensure we clear the model before we check it, if no model is
            ' found the call will not update it to zero.
            m_lRIModelID = 0

            ' Get information for the active band

            lReturn = m_oBusiness.GetBandValues(lRIBandID:=m_lActiveBand, cSumInsured:=cSumInsured, cReserveToDate:=cReserveToDate, cPaymentToDate:=cPaymentToDate, cThisReserve:=cThisReserve, cThisPayment:=cThisPayment, lRIModelID:=m_lRIModelID, lCatastropheCodeID:=lCatastropheCodeID, lXolClmModelID:=lXolClmModelID, cXolClmLimit:=cXolClmLimit, lXolCatModelID:=lXolCatModelID, cXolCatLimit:=cXolCatLimit, lXolCatReinstatements:=lXolCatReinstatements, vRILines:=vRILines)

            ' Create new ri summary
            oRI = New cSIRRIControls.ClaimRIArrangement()
            oRI.SumInsured = cSumInsured
            oRI.ReserveToDate = cReserveToDate
            oRI.PaymentToDate = cPaymentToDate
            oRI.ThisReserve = cThisReserve
            oRI.ThisPayment = cThisPayment
            oRI.CatastropheCodeID = lCatastropheCodeID
            oRI.XolClmModelID = lXolClmModelID
            oRI.XolClmLimit = cXolClmLimit
            oRI.XolCatModelID = lXolCatModelID
            oRI.XolCatLimit = cXolCatLimit
            oRI.XolCatReinstatements = lXolCatReinstatements
            oRI.ReinsuranceLines = vRILines

            ' Populate reinsurance control?
            If Information.IsArray(vRILines) Then
                lReturn = uctRI.SetProperties(oRI)
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctRI.SetProperties", "Unable to set claims reinsurance model")
                End If
                uctRI.Enabled = True
            Else
                ' Clear reinsurance control
                lReturn = uctRI.Clear()
                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError("uctRI.Clear", "Unable to clear reinsurance")
                End If
                uctRI.Enabled = False
            End If

            ' Set enabled states
            cmdAddTreaty.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR")
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR")

            ' Update summary control, if necessary
            If SSTabHelper.GetSelectedIndex(tabRI) = 1 Then
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
    Private Function ProcessFindParty(ByRef sPartyName As String, ByRef lPartyCnt As Integer) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object
        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim lReturn As Integer
        Dim temp_oFindParty As Object
        Const kMethodName As String = "ProcessFindParty"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the find party component.

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
                ' Add to grid
                lReturn = uctRI.AddFacultative(lPartyCnt, sPartyName)
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
        Dim bIsRetained As Boolean

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "ProcessNewTreaty"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            'developer guide no.50
            frmTreaty = New frmTreaty
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
                    lReturn = uctRI.AddTreaty(lTreatyID, sCode, sAgreementCode, bIsRetained)
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

            ' Decide if we should show the payment grid?
            uctRI.ShowPayments = (m_sTransactionType <> "C_CO")

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

            ' Ensure we have committed any edits in the grid
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
                    MessageBox.Show("Reserve share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
                Case lValid = 2
                    iPMFunc.SetComboBoxValue(cboRIBand, CStr(lBand))
                    MessageBox.Show("Payment share for '" & cboRIBand.Text & "' is not 100%", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Exit Sub
            End Select

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

    Private Sub tabRI_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles tabRI.SelectedIndexChanged

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "tabRI_Click"

        Try

            ' Set visible states as the tab control uses positions to hide controls which
            ' conflicts with the screen resize code
            uctRI.Visible = (SSTabHelper.GetSelectedIndex(tabRI) = 0)
            uctSummary.Visible = (SSTabHelper.GetSelectedIndex(tabRI) = 1)

            ' Set enabled states
            cmdAddTreaty.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR")
            cmdAddFAC.Enabled = (SSTabHelper.GetSelectedIndex(tabRI) = 0) And (Not uctRI.ReadOnly_Renamed) And (m_sTransactionType = "C_CO" Or m_sTransactionType = "C_CP" Or m_sTransactionType = "C_CR")

            ' Update summary control, if necessary
            If SSTabHelper.GetSelectedIndex(tabRI) = 1 Then
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
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bCLMReinsurance.Form", vInstanceManager:=gPMConstants.PMGetViaClientManager)
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

            ' Call the initialise method passing this interface
            ' and the business object as parameters.
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

    Private Sub frmInterface_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        uctRI.grdRI_Enter()
    End Sub

    Public Sub frmInterfaceLoad()

        Dim bApplyReinsurance, bDeferredRIStatus As Boolean
        Dim sMessage As String = ""

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

            'Start - Prakash - PN71185
            'Uncommented the follwoing code block since for Open/maintain claim, RI screen should not be shown
            'iCLMCheckDeferredRI component validates deffered risk for claim payment process so it can not be used in open/ maintain claim
            'Check if RI is deferred on the risk, do not process reinsurance if it is

            m_lReturn = m_oBusiness.GetClaimRiskStatus(v_lClaimId:=m_lClaimID, r_bIsDeferred:=bDeferredRIStatus)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "m_oBusiness.GetClaimRiskStatus failed - Unable to check deferred reinsurance status", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' Check deferred flag
            If bDeferredRIStatus Then
                MessageBox.Show("Risk is using deferred reinsurance model." & Strings.Chr(13) & Strings.Chr(10) & "No reinsurance allocations will be processed.", "Reinsurance", MessageBoxButtons.OK, MessageBoxIcon.Information)
                m_lErrorNumber = gPMConstants.PMEReturnCode.PMFalse
                Exit Sub
            End If
            'End - Prakash - PN71185

            ' Set the process modes for the busines object.

            lReturn = m_oBusiness.SetProcessModes(vTask:=m_iTask, vNavigate:=m_lNavigate, vProcessMode:=m_lProcessMode, vTransactionType:=m_sTransactionType, vEffectiveDate:=m_dtEffectiveDate)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetProcessModes", "Failed to set the process modes for the business object")
            End If

            'PN-61908 (By Nitesh Dwivedi as on 04-05-2010)
            uctRI.TransactionType = m_sTransactionType
            'PN-61908

            ' Set the status for the business object.

            lReturn = m_oBusiness.SetStatus(sProcessStatus:=m_sProcessStatus.Value, sMapStatus:=m_sMapStatus.Value, sStepStatus:=m_sStepStatus.Value)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oBusiness.SetStatus", "Failed to set the status for the business object")
            End If
            If m_sTransactionType = "REN" Then
                uctRI.TransactionType = "C_CV"
            End If
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
        If isInitializingComponent Then
            Exit Sub
        End If

        Try

            ' Resize tabs
            tabRI.SetBounds(VB6.TwipsToPixelsX(90), VB6.TwipsToPixelsY(570), ClientRectangle.Width - VB6.TwipsToPixelsX(180), ClientRectangle.Height - VB6.TwipsToPixelsY(1080))

            ' Resize tab controls
            'Developer Guide No. Modified, checked by R & D
            'start
            uctRI.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
            uctSummary.SetBounds(90, 390, VB6.PixelsToTwipsX(tabRI.Width) - 180, VB6.PixelsToTwipsY(tabRI.Height) - 480)
            'end
            ' Move command buttons
            cmdAddTreaty.SetBounds(VB6.TwipsToPixelsX(90), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdAddFAC.SetBounds(VB6.TwipsToPixelsX(1380), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdOK.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(2580), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)
            cmdCancel.SetBounds(ClientRectangle.Width - VB6.TwipsToPixelsX(1290), ClientRectangle.Height - VB6.TwipsToPixelsY(420), 0, 0, BoundsSpecified.X Or BoundsSpecified.Y)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub


End Class
