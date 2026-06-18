Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'Developer Guide No. 129
Imports SharedFiles
Partial Friend Class frmTreatyParty
    Inherits System.Windows.Forms.Form

    ' Constant for the functions to identify which class this is.
    Private Const ACClass As String = "frmTreatyParty"
    Private Const vbFormCode As Integer = 0

    ' ***************************************************************** '
    '                        PUBLIC PROPERTIES
    ' ***************************************************************** '
    Public Status As gPMConstants.PMEReturnCode

    ' Declare an instance of the Business object.
    Public Business As Object


    ' ***************************************************************** '
    '                        PRIVATE PROPERTIES
    ' ***************************************************************** '
    ' Declare an instance of the FormControl object
    Private m_oFormFields As iPMFormControl.FormFields

    ' Stores the return value for the a function call.
    Private m_lReturn As gPMConstants.PMEReturnCode

    Private m_sUnderwritingType As String = ""
    Private m_bIsRi2007Enabled As Boolean   'E016
    'E005
    Private m_vBrokerParticipantArray(,) As Object
    Private m_vBrokerParticipantArrayForDisplay(,) As Object

    ' ***************************************************************** '
    '                         PUBLIC METHODS
    ' ***************************************************************** '
    Public Function Clear(Optional ByVal dTotalShare As Double = 0) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Clear"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Clear reinsurer details
            txtReinsurer.Tag = CStr(0)
            txtReinsurer.Text = ""

            ' Assign percentages with appropriate formatting
            lReturn = m_oFormFields.FormatControl(ctlControl:=txtSharePercent, vControlValue:=100 - gPMFunctions.ToSafeDouble(dTotalShare))
            lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommPercent, vControlValue:=0)

            ' Clear tax details
            chkIsDomiciledForTax.CheckState = CheckState.Unchecked
            cboTaxGroup.ItemId = 0

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

    Public Function GetProperties(ByRef lPartyCnt As Integer, ByRef sReinsurer As String, ByRef dSharePercent As Double, ByRef dCommPercent As Double, ByRef sTaxGroup As String, ByRef bIsDomiciled As Boolean, _
    ByRef lApproved As Integer, _
    Optional ByRef vRIBrokerParticipant(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As Integer
        Const kMethodName As String = "GetProperties"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Return all detail data
            lPartyCnt = gPMFunctions.ToSafeLong(Convert.ToString(txtReinsurer.Tag))
            sReinsurer = txtReinsurer.Text

            dSharePercent = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtSharePercent))

            dCommPercent = CDbl(m_oFormFields.UnformatControl(ctlControl:=txtCommPercent))
            sTaxGroup = IIf(cboTaxGroup.ItemId = 0, "", cboTaxGroup.ItemCaption)
            bIsDomiciled = (chkIsDomiciledForTax.CheckState = CheckState.Checked)
            'Start E016
            If m_bIsRi2007Enabled Then
                If chkIsReinsurerApproved.CheckState = CheckState.Checked Then
                    lApproved = 1
                Else
                    lApproved = 0
                End If
            End If
            'End E016

            vRIBrokerParticipant = m_vBrokerParticipantArray   'E005

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)
        Finally

            '            Return result
            '            Resume
            '            Return result
        End Try
        Return result
    End Function

    Public Function SetProperties(ByVal lPartyCnt As Integer, ByVal sReinsurer As String, ByVal dSharePercent As Double, ByVal dCommPercent As Double, _
    ByVal lApproved As Integer, _
    Optional ByVal vBrokerParticipantArrayForDisplay(,) As Object = Nothing) As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetProperties"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Set all detail data
            txtReinsurer.Tag = CStr(lPartyCnt)
            txtReinsurer.Text = sReinsurer.Trim()
            lReturn = m_oFormFields.FormatControl(ctlControl:=txtSharePercent, vControlValue:=dSharePercent)
            lReturn = m_oFormFields.FormatControl(ctlControl:=txtCommPercent, vControlValue:=dCommPercent)

            'Start E016
            If m_bIsRi2007Enabled Then
                chkIsReinsurerApproved.CheckState = lApproved
            End If
            'End E016
            ' Reload tax group
            PopulatePartyTaxInfo()
            m_vBrokerParticipantArrayForDisplay = vBrokerParticipantArrayForDisplay  'E005

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
    '                         PRIVATE METHODS
    ' ***************************************************************** '
    Private Function PopulatePartyTaxInfo() As Integer

        Dim result As Integer = 0
        Dim iIsDomiciledForTax As CheckState
        Dim lTaxGroupId As Integer

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "GetTreatyTaxInfo"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Get data

            lReturn = Business.GetTreatyPartyTaxInfo(lPartyCnt:=Convert.ToString(txtReinsurer.Tag), r_iIsDomiciledForTax:=iIsDomiciledForTax, r_lTaxGroupID:=lTaxGroupId)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("Business.GetTreatyPartyTaxInfo", "Failed to get tax information for reinsurer")
            End If

            ' Show values
            chkIsDomiciledForTax.CheckState = iIsDomiciledForTax
            cboTaxGroup.ItemId = lTaxGroupId


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' Clear any existing values as they may be incorrect
            chkIsDomiciledForTax.CheckState = CheckState.Unchecked
            cboTaxGroup.ItemId = 0

        Finally




        End Try
        Return result
    End Function

    Private Function SetFieldValidation() As Integer

        Dim result As Integer = 0
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "SetFieldValidation"


        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the form control object.
            m_oFormFields = New iPMFormControl.FormFields()
            m_oFormFields.LanguageID = g_iLanguageID

            ' Add controls
            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtSharePercent, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtSharePercent")
            End If

            lReturn = m_oFormFields.AddNewFormField(ctlControl:=txtCommPercent, lFieldType:=gPMConstants.PMEDataType.PMCurrency, lFormat:=gPMConstants.PMEFormatStyle.PMFormatPercent, lMandatory:=gPMConstants.PMEMandatoryStatus.PMMandatory, lDecimalPlaces:=-5)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("m_oFormFields.AddNewFormField", "Failed to add txtCommPercent")
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally




        End Try
        Return result
    End Function

    Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Dim dShare As Double
        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Validate"


        Try

            ' Default to false, only set true if we get to the end
            result = gPMConstants.PMEReturnCode.PMFalse

            ' Standard validation
            lReturn = m_oFormFields.CheckMandatoryControls()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                Return result
            End If

            ' Check reinsurer is selected
            If gPMFunctions.ToSafeLong(Convert.ToString(txtReinsurer.Tag)) = 0 Then
                MessageBox.Show("Please select a reinsurer", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                cmdReinsurer.Focus()
                Return result
            End If

            ' Check share is valid

            dShare = CDbl(m_oFormFields.UnformatControl(txtSharePercent))
            If (dShare <= 0) Or (dShare > 100) Then
                MessageBox.Show("Share percentage must be greater than zero and less that or equal to 100%", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtSharePercent.Focus()
                Return result
            End If

            ' Check share is valid

            dShare = CDbl(m_oFormFields.UnformatControl(txtCommPercent))
            If (dShare < 0) Or (dShare > 100) Then
                MessageBox.Show("Commission percentage must be between 0% and 100%", Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                txtCommPercent.Focus()
                Return result
            End If

            ' All validation passed return True
            result = gPMConstants.PMEReturnCode.PMTrue


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        Finally



        End Try
        Return result
    End Function


    ' ***************************************************************** '
    '                             EVENTS
    ' ***************************************************************** '
    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        ' Check the user wants to close
        If MessageBox.Show("Cancelling will lose all of your current details." & _
                           Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.Yes Then
            ' Set status to cancel and close
            Status = gPMConstants.PMEReturnCode.PMCancel
            Me.Hide()
        End If
    End Sub

    Private Sub cmdOK_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOK.Click

        Dim dShare As Double
        Dim lReturn As Integer
        Const kMethodName As String = "cmdOK_Click"


        Try

            ' Validate data
            If Validate_Renamed() = gPMConstants.PMEReturnCode.PMTrue Then
                ' Set status to OK and close
                Status = gPMConstants.PMEReturnCode.PMOK
                Me.Hide()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub cmdReinsurer_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdReinsurer.Click
        Dim iPMBFindParty As Object

        Dim lTag, lControlCount As Integer
        Dim vArray As Object


        Dim oFindParty As iPMBFindParty.Interface_Renamed
        Dim vKeyArray(,) As Object

        Dim lReturn As Integer
        Const kMethodName As String = "cmdReinsurer_Click"


        Try

            ' Get an instance of the find party interface object via the public object manager.
            Dim temp_oFindParty As Object
            lReturn = g_oObjectManager.GetInstance(temp_oFindParty, sClassName:="iPMBFindParty.Interface_Renamed", vInstanceManager:=gPMConstants.PMGetLocalInterface)
            oFindParty = temp_oFindParty
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("g_oObjectManager.GetInstance", "Unable to create instance of iPMBFindParty")
            End If

            ' Set up key array
            ReDim vKeyArray(1, 0)

            vKeyArray(0, 0) = "special_party"

            vKeyArray(1, 0) = "IN"

            ' Pass array

            lReturn = oFindParty.SetKeys(vKeyArray)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oFindParty.SetKeys", "Failed to set party keys")
            End If

            ' Set properties and run

            oFindParty.NotEditable = 1

            'E005
            oFindParty.CallingAppName = "iPMUTreaty"
            oFindParty.TreatyPartiesBrokerParticipantsForDisplay = m_vBrokerParticipantArrayForDisplay
            'E005

            lReturn = oFindParty.Start()
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("oFindParty.Start", "Failed to launch find party interface")
            End If
            m_vBrokerParticipantArray = oFindParty.BrokerArray   'E005
            ' Only store details if the user clicked ok

            If oFindParty.Status = gPMConstants.PMEReturnCode.PMOK Then
                ' Store party cnt and description

                txtReinsurer.Tag = oFindParty.PartyCnt


                txtReinsurer.Text = IIf(Strings.Len(oFindParty.LongName), oFindParty.LongName, oFindParty.ShortName)

                ' Get tax information
                PopulatePartyTaxInfo()
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally

            ' If we created a party object release it.
            If Not (oFindParty Is Nothing) Then

                oFindParty.Dispose()
                oFindParty = Nothing
            End If


        End Try
    End Sub


    '	Private Sub frmTreatyParty_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load

    '		Dim lReturn As gPMConstants.PMEReturnCode
    '		Const kMethodName As String = "Form_Load"


    '		On Error GoTo Catch_Renamed

    '		' Set the mouse pointer to busy.
    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

    '		m_lReturn = CType(iPMFunc.GetSystemOption(5005, m_sUnderwritingType), gPMConstants.PMEReturnCode)

    '		' Validate fields using Forms Control
    '		lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
    '		If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
    '			gPMFunctions.RaiseError("SetFieldValidation", "Unable to set field validation")
    '		End If

    '		If m_sUnderwritingType = "1" Then
    '			cmdReinsurer.Text = "Insurer..."
    '		End If

    '		GoTo Finally_Renamed
    'Catch_Renamed: 
    '		' DO Not Call any functions before here or the error will be lost
    '		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn)

    'Finally_Renamed: 
    '		' Set the mouse pointer to normal.
    '		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

    '		Exit Sub
    '		Resume 
    '    End Sub
    Public Sub frmTreatyPartyLoad()

        Dim lReturn As gPMConstants.PMEReturnCode
        Const kMethodName As String = "Form_Load"
        Dim vValue As Object = Nothing 'E016

        Try
            Me.cboTaxGroup.FirstItem = "(none)"

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            m_lReturn = CType(iPMFunc.GetSystemOption(5005, m_sUnderwritingType), gPMConstants.PMEReturnCode)

            ' Validate fields using Forms Control
            lReturn = CType(SetFieldValidation(), gPMConstants.PMEReturnCode)
            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetFieldValidation", "Unable to set field validation")
            End If
            'Start E016
            m_lReturn = iPMFunc.getProductOptionValue( _
                v_vOptionNumber:=SIRHiddenOptions.SIROPTEnableRI2007, _
                v_vBranch:=g_iSourceID, _
                r_vUnderwriting:=vValue)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError("SetFieldValidation", "Unable to set field validation")
            End If

            If vValue = "1" Then
                chkIsReinsurerApproved.Visible = True
                m_bIsRi2007Enabled = True
            Else
                chkIsReinsurerApproved.Visible = False
                m_bIsRi2007Enabled = False
            End If
            'End E016
            If m_sUnderwritingType = "1" Then
                cmdReinsurer.Text = "Insurer..."
            End If

        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally
            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
    End Sub

    Private Sub frmTreatyParty_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
        Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
        Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)

        Dim lReturn As Integer
        Const kMethodName As String = "Form_QueryUnload"


        Try

            ' Check if the interface has been terminated by means
            ' other than pressing the command buttons.


            If UnloadMode <> vbFormCode Then
                ' Check the user wants to close
                If MessageBox.Show("Cancelling will lose all of your current details." & _
                                   Strings.Chr(13) & Strings.Chr(10) & "Do you really wish to cancel?", Text, MessageBoxButtons.YesNo) = System.Windows.Forms.DialogResult.No Then
                    ' Do not procced with the interface termination.
                    Cancel = 1
                    eventArgs.Cancel = True
                End If
            End If


        Catch ex As Exception
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=m_lReturn, excep:=ex)

        Finally


        End Try
    End Sub

    Private Sub txtCommPercent_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommPercent.Enter
        m_oFormFields.GotFocus(ctlControl:=txtCommPercent)
    End Sub
    Private Sub txtCommPercent_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtCommPercent.Leave
        m_oFormFields.LostFocus(ctlControl:=txtCommPercent)
    End Sub

    Private Sub txtSharePercent_Enter(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSharePercent.Enter
        m_oFormFields.GotFocus(ctlControl:=txtSharePercent)
    End Sub
    Private Sub txtSharePercent_Leave(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles txtSharePercent.Leave
        m_oFormFields.LostFocus(ctlControl:=txtSharePercent)
    End Sub


    Private Sub chkIsReinsurerApproved_Click(sender As Object, e As EventArgs) Handles chkIsReinsurerApproved.Click

    End Sub
End Class
