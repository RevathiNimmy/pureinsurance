Option Strict Off
Option Explicit On
Imports Microsoft.VisualBasic
Imports System
Imports System.Drawing
Imports System.Windows.Forms
'Developer Guide no. 129
Imports SharedFiles
Partial Friend Class frmRecoveryReserve
    Inherits System.Windows.Forms.Form
    ' ***************************************************************** '
    ' Form Name:   frmRecoveryReserve
    ' Date:        07/04/2005
    ' Description: Allow maintenance of recovery reserves
    ' ***************************************************************** '

    Private Const ACClass As String = "frmRecoveryReserve"

    Private Const vbFormControlMenu As Integer = 0

    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode


    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    '
    ' Note:
    '       Public PerilID As Long
    ' is the SAME as:
    '       Public Property Let PerilID(ByVal RHS As Long)
    '       End Property
    '       Public Property Get PerilID() As Long
    '       End Property
    ' ***************************************************************** '
    Public PerilID As Integer
    Public ClaimCompanyID As Integer

    Public RecoveryType As String = ""
    Public RecoveryTypeID As Integer
    Public InitialReserve As Decimal
    Public RevisedReserve As Decimal
    Public ThisReserve As Decimal
    Public Status As gPMConstants.PMEReturnCode
    Public Mode As gPMConstants.PMEComponentAction
    Private m_IIsComplaint As Integer
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.6)
    Private m_sPartytype As String = ""
    Private m_lPartytypeId As Integer
    Private m_sPartyCode As String = ""
    Private m_lPartyCnt As Integer
    Private m_sPartyName As String = ""
    Private m_lAgentCnt As Integer
    Private m_sAgentCode As String = ""
    Private m_sAgentName As String = ""
    Private m_lClientCnt As Integer
    Private m_sClientName As String = ""
    Private m_sClientCode As String = ""
    Private m_bIsRecoveriesReadOnly As Boolean = False

    Public WriteOnly Property IsRecoveriesReadOnly() As Boolean
        Set(ByVal value As Boolean)
            m_bIsRecoveriesReadOnly = value
        End Set
    End Property

    Public WriteOnly Property IsComplaint() As Integer
        Set(ByVal Value As Integer)

            m_IIsComplaint = Value

        End Set
    End Property


    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.6)
    Public Property PartyCode() As String
        Get
            Return m_sPartyCode
        End Get
        Set(ByVal Value As String)
            m_sPartyCode = Value
        End Set
    End Property
    Public Property PartyType() As String
        Get
            Return m_sPartytype
        End Get
        Set(ByVal Value As String)
            m_sPartytype = Value
        End Set
    End Property
    Public Property PartyTypeID() As Integer
        Get
            Return m_lPartytypeId
        End Get
        Set(ByVal Value As Integer)
            m_lPartytypeId = Value
        End Set
    End Property
    Public Property PartyName() As String
        Get
            Return m_sPartyName
        End Get
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property
    Public Property PartyCnt() As Integer
        Get
            Return m_lPartyCnt
        End Get
        Set(ByVal Value As Integer)
            m_lPartyCnt = Value
        End Set
    End Property
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.6)

    Public ReadOnly Property TotalReserve() As Decimal
        Get
            Return InitialReserve + RevisedReserve + ThisReserve
        End Get
    End Property



    ' ***************************************************************** '
    '                         PRIVATE FUNCTIONS
    ' ***************************************************************** '

    ' Display all language specific captions.
    Private Function DisplayCaptions() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Select Case g_lRecoveryMode
                Case MainModule.RecoveryModeEnum.RMThirdPartyReserve
                    Text = "Third Party Recovery Reserve"
                Case MainModule.RecoveryModeEnum.RMSalvageReserve
                    Text = "Salvage Recovery Reserve"
                Case Else
                    ' Invalid mode for this dialog
                    result = gPMConstants.PMEReturnCode.PMFalse
            End Select


            cmdOK.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACOKButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdCancel.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=ACCancelButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to display the language captions", vApp:=ACApp, vClass:=ACClass, vMethod:="DisplayCaptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Updates the interface details from the property members.
    Private Function PropertiesToInterface() As Integer

        Dim result As Integer = 0

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set recovery type
            m_lReturn = CType(iPMFunc.SetComboBoxValue(cboRecoveryType, CStr(RecoveryTypeID)), gPMConstants.PMEReturnCode)
            'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1)
            'Set Recovery Party Type

            Dim oDetail As frmRecoveryReserve
            oDetail = New frmRecoveryReserve()
            If m_lPartytypeId = ACPartyTypeEmpty Then
                If Mode = gPMConstants.PMEComponentAction.PMEdit Then
                    cboPartyType.ItemId = ACPartyTypeEmpty
                    cmdGetParty.Enabled = False
                Else
                    cboPartyType.ItemId = ACPartyTypeDefault
                End If
            Else
                cboPartyType.ItemId = CInt(gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEDataType.PMInteger, vFieldValue:=CDbl(CStr(m_lPartytypeId).Trim())))
            End If
            'developer guide no.98
            txtPartyCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPartyCode.Trim())

            'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1)
            ' Set properties
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtInitialReserve, vControlValue:=InitialReserve)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtRevisedReserve, vControlValue:=RevisedReserve)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtThisRevision, vControlValue:=ThisReserve)
            m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTotalReserve, vControlValue:=TotalReserve)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to update the interface details", vApp:=ACApp, vClass:=ACClass, vMethod:="PropertiesToInterface", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Sets the rules for validating fields.
    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_oFormFields.Clear()

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboRecoveryType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtInitialReserve, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=IIf(Mode = gPMConstants.PMEComponentAction.PMAdd, gPMConstants.PMEMandatoryStatus.PMMandatory, gPMConstants.PMEMandatoryStatus.PMNonMandatory))
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtRevisedReserve, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtThisRevision, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency, lMandatory:=IIf(Mode = gPMConstants.PMEComponentAction.PMEdit, gPMConstants.PMEMandatoryStatus.PMMandatory, gPMConstants.PMEMandatoryStatus.PMNonMandatory))
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtTotalReserve, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatCurrency)
            'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)
            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtPartyCode, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=IIf(cboPartyType.ItemCaption.Trim() <> "(None)", gPMConstants.PMEMandatoryStatus.PMMandatory, gPMConstants.PMEMandatoryStatus.PMNonMandatory))

            m_lReturn = m_oFormFields.AddNewFormField(ctlControl:=cboPartyType, lFieldType:=gPMConstants.PMEDataType.PMString, lFormat:=gPMConstants.PMEFormatStyle.PMFormatString, lMandatory:=gPMConstants.PMEMandatoryStatus.PMNonMandatory)
            'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to SetFieldValidation", vApp:=ACApp, vClass:=ACClass, vMethod:="SetFieldValidation", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' Sets all of the interface default values.
    Private Function SetInterfaceDefaults() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"
        Dim lReturn As gPMConstants.PMEReturnCode

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            ' Center the interface.
            iPMFunc.CenterForm(Me)

            ' Clear and then populate the recovery types
            If Information.IsArray(g_vRecoveryTypes) Then
                cboRecoveryType.Items.Clear()


                For lCount As Integer = g_vRecoveryTypes.GetLowerBound(1) To g_vRecoveryTypes.GetUpperBound(1)
                    Dim cboRecoveryType_NewIndex As Integer = -1

                    cboRecoveryType_NewIndex = cboRecoveryType.Items.Add(CStr(g_vRecoveryTypes(1, lCount)))

                    VB6.SetItemData(cboRecoveryType, cboRecoveryType_NewIndex, CInt(g_vRecoveryTypes(0, lCount)))
                Next lCount
            Else
                MessageBox.Show("No recovery types defined", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                Return gPMConstants.PMEReturnCode.PMNotFound
            End If

            ' Set enabled state based on mode
            cboRecoveryType.Enabled = (Mode = gPMConstants.PMEComponentAction.PMAdd)
            ' Display all language specific captions.
            lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("DisplayCaptions", "Unable to display captions")
            End If

            ' Update the interface details with the property members.
            lReturn = CType(PropertiesToInterface(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("PropertiesToInterface", "Unable to display data")
            End If

            ' Setup form for given mode
            If Mode = gPMConstants.PMEComponentAction.PMAdd Then
                ' In add mode we only want the 'initial reserve'
                lblRevisedReserve.Visible = False
                txtRevisedReserve.Visible = False
                lblThisRevision.Visible = False
                txtThisRevision.Visible = False
                lblTotalReserve.Visible = False
                txtTotalReserve.Visible = False

                ' Reposition buttons and resize form
                cmdOK.Top = txtInitialReserve.Top + VB6.TwipsToPixelsY(510)
                cmdCancel.Top = txtInitialReserve.Top + VB6.TwipsToPixelsY(510)
                grpRecoveryReserve.Height = txtInitialReserve.Top + VB6.TwipsToPixelsY(400)
                Me.Height -= VB6.TwipsToPixelsY(1035)

                ' Set mandatory controls
                lblRecoveryType.Font = VB6.FontChangeBold(lblRecoveryType.Font, True)
                lblInitialReserve.Font = VB6.FontChangeBold(lblInitialReserve.Font, True)
            Else
                ' Lock and recolour initial reserve in edit mode
                txtInitialReserve.ReadOnly = True
                txtInitialReserve.BackColor = SystemColors.Control

                ' Set mandatory controls
                lblRecoveryType.Font = VB6.FontChangeBold(lblRecoveryType.Font, True)
                lblThisRevision.Font = VB6.FontChangeBold(lblThisRevision.Font, True)

                ' In edit mode we want all.
                lblRevisedReserve.Visible = True
                txtRevisedReserve.Visible = True
                lblThisRevision.Visible = True
                txtThisRevision.Visible = True
                lblTotalReserve.Visible = True
                txtTotalReserve.Visible = True
            End If

            grpRecoveryReserve.Enabled = IIf(m_bIsRecoveriesReadOnly, False, True)
            cmdOK.Enabled = IIf(m_bIsRecoveriesReadOnly, False, True)

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here


            ' This is for debugging only



        End Try
        Return result
    End Function




    'Private Sub cboPartyType_Change()
    '
    'End Sub


    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.4)
    Private Sub cboPartyType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboPartyType.Click
        Const kMethodName As String = "cboPartyType_Click"
        Try


            txtPartyCode.Text = ""
            If cboPartyType.ItemId = ACPartyTypeEmpty Then
                cmdGetParty.Enabled = False
                txtPartyCode.Enabled = False
                cmdOK.Enabled = True
                m_lPartyCnt = ACPartyTypeEmpty
                m_sPartyName = ACAnyStringEmpty
                m_lPartytypeId = ACPartyTypeEmpty
            Else

                If cboPartyType.ItemCaption.Trim() = ACPartyTypeAgent Or cboPartyType.ItemCaption.Trim() = ACPartyTypeClient Then

                    m_lReturn = CType(GetAttachedPartiesDetails(m_lAgentCnt, m_sAgentCode, m_sAgentName, m_lClientCnt, m_sClientCode, m_sClientName, m_ofrmRecovery.ClaimId), gPMConstants.PMEReturnCode)

                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                        gPMFunctions.RaiseError(kMethodName, "GetAttachedPartiesDetails  Failed to fetch the Client or Agent details", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lPartytypeId = cboPartyType.ItemId

                    If cboPartyType.ItemCaption.Trim() = ACPartyTypeAgent Then
                        'developer guide no.98
                        txtPartyCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sAgentCode.Trim())
                        cmdGetParty.Enabled = False
                        m_lPartyCnt = m_lAgentCnt
                        m_sPartyName = m_sAgentName

                        If m_lAgentCnt <= 0 Then
                            MessageBox.Show("There is no agent associated with the policy record. Please make another selection", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            cmdOK.Enabled = False
                        Else
                            cmdOK.Enabled = True
                        End If

                    ElseIf cboPartyType.ItemCaption.Trim() = ACPartyTypeClient Then
                        'developer guide no.98
                        txtPartyCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sClientCode.Trim())
                        cmdGetParty.Enabled = False
                        m_lPartyCnt = m_lClientCnt
                        m_sPartyName = m_sClientName

                        If m_lClientCnt <= 0 Then
                            MessageBox.Show("There is no Client associated with the policy record. Please make another selection", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                            cmdOK.Enabled = False
                        Else
                            cmdOK.Enabled = True
                        End If
                    Else
                        cmdGetParty.Enabled = True
                    End If
                Else
                    cmdGetParty.Enabled = True
                End If


            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
    End Sub
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.4)





    ' ***************************************************************** '
    '                          CONTROL EVENTS
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Status = gPMConstants.PMEReturnCode.PMCancel
        'Added as in VB formclosing is not called while Me.hide
        RemoveHandler MyBase.FormClosing, AddressOf frmRecoveryReserve_FormClosing
        Me.Hide()
    End Sub
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.5)
    'This method will fetch agent and client details based on ClaimID
    Public Function GetAttachedPartiesDetails(ByRef r_lAgentCnt As Integer, ByRef r_sAgentCode As String, ByRef r_sAgentName As String, ByRef r_lClientCnt As Integer, ByRef r_sClientCode As String, ByRef r_sClientName As String, ByVal ClaimId As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetAttachedPartiesDetails"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = g_oBusiness.GetAttachedParties(r_lAgentCnt:=m_lAgentCnt, r_sAgentCode:=r_sAgentCode, r_sAgentName:=r_sAgentName, r_lClientCnt:=r_lClientCnt, r_sClientCode:=r_sClientCode, r_sClientName:=r_sClientName, v_lClaim_Id:=ClaimId)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "g_oBusiness.GetAttachedParties method" & _
                                    "calling Failed to get the Agent and Client detials ", gPMConstants.PMELogLevel.PMLogError)
                result = gPMConstants.PMEReturnCode.PMFalse
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)


        Finally


        End Try
        Return result
    End Function
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.5)
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.3)
    Private Sub cmdGetParty_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdGetParty.Click

        Const kMethodName As String = "cmdGetParty_Click"
        Try


            If cboPartyType.ItemCaption.Trim() = ACPartyTypeOther Then

                m_lPartytypeId = cboPartyType.ItemId
                m_lReturn = CType(GetPartyInfo(v_sPartyType:=ACPartyTypeOtherPartyCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    gPMFunctions.RaiseError(kMethodName, "GetPartyInfo  Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If m_sPartyCode.Trim() = "" Then

                    MessageBox.Show("No Other Party is attached with this Claim Recovery", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cmdOK.Enabled = False
                Else
                    cmdOK.Enabled = True
                End If


            ElseIf cboPartyType.ItemCaption.Trim() = ACPartyTypeInsurer Then

                m_lPartytypeId = cboPartyType.ItemId

                m_lReturn = CType(GetPartyInfo(v_sPartyType:=ACPartyTypeInsurerCode), gPMConstants.PMEReturnCode)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                    gPMFunctions.RaiseError(kMethodName, "GetPartyInfo  Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                If m_sPartyCode.Trim() = "" Then
                    MessageBox.Show("No Insurer is attached with this Claim Recovery", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                    cmdOK.Enabled = False
                Else
                    cmdOK.Enabled = True
                End If
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
            ' Do any tidy up, e.g. Set x = Nothing here
        End Try
    End Sub
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.3)
    'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.6)
    'If PartyType is an insurer or other party then  the corresponding party info can be got
    'from here
    Public Function GetPartyInfo(ByVal v_sPartyType As String) As Integer
        Dim result As Integer = 0
        Dim iPMBFindParty As Object

        Const kMethodName As String = "GetPartyInfo"
        Try



            Dim oFindParty As iPMBFindParty.Interface_Renamed
            result = gPMConstants.PMEReturnCode.PMTrue

            Dim temp_oFindParty As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "iPMBFindParty.Interface Failed to create the Instance", gPMConstants.PMELogLevel.PMLogError)
            End If


            oFindParty.SpecialParty = v_sPartyType

            oFindParty.CallingAppName = ACApp

            oFindParty.IsComplaint = m_IIsComplaint

            oFindParty.IgnoreDPAQuestions = True

            oFindParty.NotEditable = 1

            oFindParty.EnableNewParty = True

            m_lReturn = oFindParty.Start()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMCancel Then
                gPMFunctions.RaiseError(kMethodName, "oFindParty.Start Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                'Retrieve party details

                m_lPartyCnt = oFindParty.PartyCnt

                m_sPartyName = oFindParty.LongName.Trim()

                m_sPartyCode = oFindParty.ShortName.Trim()


                oFindParty.Dispose()

                oFindParty = Nothing

                'Display Agent on form
                'developer guide no.98
                txtPartyCode.Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatString, vFieldValue:=m_sPartyCode.Trim())

            ElseIf oFindParty.Status = gPMConstants.PMEReturnCode.PMCancel Then
                result = gPMConstants.PMEReturnCode.PMCancel
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)



        Finally



        End Try
        Return result
    End Function
    'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.1.6)
    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Status = gPMConstants.PMEReturnCode.PMOK

        ' Force focus to trigger any outstanding validation
        If Not iPMFunc.ForceLostFocus(cmdOK) Then Exit Sub

        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

        ' Check mandatory controls have been entered into.
        m_lReturn = m_oFormFields.CheckMandatoryControls()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
            Exit Sub
        End If

        If Mode = gPMConstants.PMEComponentAction.PMAdd Then
            ' Get and validate the recovery type
            RecoveryType = cboRecoveryType.Text
            RecoveryTypeID = VB6.GetItemData(cboRecoveryType, cboRecoveryType.SelectedIndex)

            m_lReturn = CType(m_ofrmRecovery.CheckRecoveryTypeID(RecoveryTypeID), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                DisplayMessage(ACMessageInvalidRecoveryType, Text)
                Exit Sub
            End If


            InitialReserve = CDec(m_oFormFields.UnformatControl(txtInitialReserve))
        Else

            ThisReserve = CDec(m_oFormFields.UnformatControl(txtThisRevision))
        End If
        'Start-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.3)
        If cboPartyType.ItemCaption.Trim() = ACPartyTypeDefaultCaption Then
            m_sPartytype = ACAnyStringEmpty
            m_sPartyCode = ACAnyStringEmpty
            m_lPartyCnt = ACPartyTypeEmpty
            m_lPartytypeId = ACPartyTypeEmpty
        Else
            If txtPartyCode.Text.Trim() = ACAnyStringEmpty Then

                MessageBox.Show("No " & cboPartyType.ItemCaption.Trim() & " is attached with this Claim Recovery", Me.Text, MessageBoxButtons.OK, MessageBoxIcon.Error)
                iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                Exit Sub

            Else

                m_sPartytype = cboPartyType.ItemCaption.Trim()
                m_sPartyCode = txtPartyCode.Text.Trim()

            End If
        End If
        'End-(Arul Stephen)-(Tech Spec WR34 - Claims Recovery Party Link.doc)-(4.2.3)
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
        Me.Hide()

    End Sub



    Private Sub txtInitialReserve_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitialReserve.Enter
        m_oFormFields.GotFocus(txtInitialReserve)
    End Sub

    Private Sub txtInitialReserve_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtInitialReserve.Leave
        m_oFormFields.LostFocus(txtInitialReserve)
    End Sub

    Private Sub txtThisRevision_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisRevision.Enter
        m_oFormFields.GotFocus(txtThisRevision)
    End Sub

    Private Sub txtThisRevision_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtThisRevision.Leave
        m_oFormFields.LostFocus(txtThisRevision)

        ' Unformat the control so we can update the total value

        ThisReserve = CDec(m_oFormFields.UnformatControl(txtThisRevision))
        m_lReturn = m_oFormFields.FormatControl(ctlControl:=txtTotalReserve, vControlValue:=TotalReserve)
    End Sub


    ' ***************************************************************** '
    '                            FORM EVENTS
    ' ***************************************************************** '

    ' Loads all required details of the form

    Private Sub frmRecoveryReserve_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

        Try

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            ' Set field validation
            m_lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Status = gPMConstants.PMEReturnCode.PMError
            End If

            ' Set the interface default values.
            m_lReturn = CType(SetInterfaceDefaults(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Status = IIf(m_lReturn = gPMConstants.PMEReturnCode.PMNotFound, gPMConstants.PMEReturnCode.PMNotFound, gPMConstants.PMEReturnCode.PMError)
            End If

        Catch excep As System.Exception



            ' Log Error.
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to load interface", vApp:=ACApp, vClass:=ACClass, vMethod:="Form_Load", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub
        End Try

    End Sub

    Private Sub frmRecoveryReserve_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        If UnloadMode <> vbFormControlMenu Then
            Status = gPMConstants.PMEReturnCode.PMCancel
        End If

        ' Terminate the form control object.
        m_oFormFields.Dispose()

        ' Destroy the instance of the form control object from memory.
        m_oFormFields = Nothing
        eventArgs.Cancel = Cancel <> 0
    End Sub

End Class