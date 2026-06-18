Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports VB6 = Microsoft.VisualBasic.Compatibility.VB6
Imports System.IO
Imports System.Runtime.ExceptionServices
<System.Runtime.InteropServices.ProgId("PartyFunc_NET.PartyFunc")> _
Public Module PartyFunc

    Private Const ACClass As String = "NRMAFunc"

    Public Const KeyAsciiBackSpace As Integer = 8
    Public Const ksLoyaltyNumberPrefix As String = "601435"
    Public Const m_kBlankAlternativeIdentifier As String = "000000000000000"
    ' CR-43 Party_History
    Public Const kSchemaName As String = "Party History Schema"
    Public Const kSchemaNameSQl As String = "spu_Get_Party_History_Schema"
    Public Const kPartyHistoryDataModelCode As String = "PARTYHISTORYXSD"
    Private m_lReturn As Integer

    ' ***************************************************************** '
    '
    ' Name: GetHiddenOptions
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    'DC111203 -added extra option for Business Field On Client Mandatory
    'Modified by Deepak Sharma on 5/10/2010 4:22:36 PM refer developer guide no. 101(Guide)
    'Public Function GetHiddenOptions(ByVal v_lSourceId As Integer, Optional ByRef r_vIsNRMA As Boolean = False, Optional ByRef r_vValidateAlternativeIdentifier As Boolean = False, Optional ByRef r_vAONAffinity As Boolean = False, Optional ByRef r_vRestrictedInsurerAccess As Boolean = False, Optional ByRef r_vFutureDateAddressChanges As Boolean = False, Optional ByRef r_vMultiTreeAccounting As Boolean = False, Optional ByRef r_vLimitPersonalClientEditFields As Boolean = False, Optional ByRef r_vShareDisclosures As Boolean = False, Optional ByRef r_vBusinessFieldOnClientIsMandatory As Boolean = False, Optional ByRef r_vAONPRClientScreenChanges As Boolean = False) As Integer
    Public Function GetHiddenOptions(ByVal v_lSourceId As Integer, Optional ByRef r_vIsNRMA As Object = Nothing, Optional ByRef r_vValidateAlternativeIdentifier As Object = Nothing, Optional ByRef r_vAONAffinity As Object = Nothing, Optional ByRef r_vRestrictedInsurerAccess As Object = Nothing, Optional ByRef r_vFutureDateAddressChanges As Object = Nothing, Optional ByRef r_vMultiTreeAccounting As Object = Nothing, Optional ByRef r_vLimitPersonalClientEditFields As Object = Nothing, Optional ByRef r_vShareDisclosures As Object = Nothing, Optional ByRef r_vBusinessFieldOnClientIsMandatory As Object = Nothing, Optional ByRef r_vAONPRClientScreenChanges As Object = Nothing) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            'Dim vValue As Byte
            Dim vValue As Object = Nothing

            'Is NRMA

            If Not Information.IsNothing(r_vIsNRMA) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTIsNRMA, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vIsNRMA = vValue = 1
                r_vIsNRMA = vValue = "1"
            End If

            'Alternative Identifier

            If Not Information.IsNothing(r_vValidateAlternativeIdentifier) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTValidateAlternativeIdentifier, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTValidateAlternativeIdentifier, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vValidateAlternativeIdentifier = vValue = 1
                r_vValidateAlternativeIdentifier = vValue = "1"
            End If

            'sj 02/07/2002 - start
            'Is AON

            If Not Information.IsNothing(r_vAONAffinity) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAONAffinity, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTAONAffinity, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vAONAffinity = vValue = 1
                r_vAONAffinity = vValue = "1"
            End If
            'sj 02/07/2002 - end

            'sj 02/07/2002 - start
            'Restricted Insurer Access

            If Not Information.IsNothing(r_vRestrictedInsurerAccess) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTRestrictedInsurerAccess, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTRestrictedInsurerAccess, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vRestrictedInsurerAccess = vValue = 1
                r_vRestrictedInsurerAccess = vValue = "1"
            End If
            'sj 02/07/2002 - end
            '    r_vValidateAlternativeIdentifier = True
            '    r_vIsNRMA = True


            If Not Information.IsNothing(r_vFutureDateAddressChanges) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTFutureDateAddressChanges, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTFutureDateAddressChanges, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vFutureDateAddressChanges = vValue = 1
                r_vFutureDateAddressChanges = vValue = "1"
            End If

            'sj 23/09/2002 - start

            If Not Information.IsNothing(r_vMultiTreeAccounting) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vMultiTreeAccounting = vValue = 1
                r_vMultiTreeAccounting = vValue = "1"
            End If

            'sj 23/09/2002 - end
            'ED 19/11/2002 - Get SIROPTLimitPersonalClientEditFields
            'PS224

            If Not Information.IsNothing(r_vLimitPersonalClientEditFields) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTLimitPersonalClientEditFields, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTMultiTreeAccounting, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vLimitPersonalClientEditFields = vValue = 1
                r_vLimitPersonalClientEditFields = vValue = "1"
            End If
            'sj 19/11/2002 - end


            'CJR 8/1/2003 419 IAG-Handling Disclosures

            If Not Information.IsNothing(r_vShareDisclosures) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTShareDisclosures, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTShareDisclosures, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vShareDisclosures = (vValue = 1)
                r_vShareDisclosures = (vValue = "1")

            End If

            'DC111203 -added for new hidden option Business Field On Client Is Mandatory

            If Not Information.IsNothing(r_vBusinessFieldOnClientIsMandatory) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTBusinessFieldOnClientIsMandatory, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTBusinessFieldOnClientIsMandatory, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vBusinessFieldOnClientIsMandatory = vValue = 1
                r_vBusinessFieldOnClientIsMandatory = vValue = "1"
            End If

            'DJM 13/01/2004 : Copied from 1.8.5 issue 5877.

            If Not Information.IsNothing(r_vAONPRClientScreenChanges) Then
                m_lReturn = iPMFunc.getProductOptionValue(v_vOptionNumber:=gPMConstants.SIRHiddenOptions.SIROPTAONPRClientScreenChanges, v_vBranch:=CStr(v_lSourceId), r_vUnderwriting:=CStr(vValue))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="getProductOptionValue Failed for option " & gPMConstants.SIRHiddenOptions.SIROPTAONPRClientScreenChanges, vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions")
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If
                'Modified by Deepak Sharma on 5/4/2010 12:31:44 PM refer developer guide no. 142(Guide)
                'r_vAONPRClientScreenChanges = vValue = 1
                r_vAONPRClientScreenChanges = vValue = "1"
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetHiddenOptions Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetHiddenOptions", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: SetAddressHeaders
    '
    ' Description:
    '
    ' History: 13/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Function SetAddressHeaders(ByRef r_oAddresses As ListView, ByVal v_sPostCode As String, ByVal v_sAddressUsage As String) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            With r_oAddresses
                .Columns.Item(0).Text = v_sPostCode
                .Columns.Item(1).Text = v_sAddressUsage
                .Columns.Item(2).Text = "Property Name"
                .Columns.Item(3).Text = "Street/PO Box"
                .Columns.Item(4).Text = "Suberb"
                .Columns.Item(5).Text = "City"
            End With

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="SetAddressHeaders Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="SetAddressHeaders", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: GetSubBranchDetails
    '
    ' Description:
    '
    ' History: 11/06/2002 SJ - Created.
    '          VB 04/03/2005 PN19229: Trim function used for removing extra spaces
    '                                  from Sub Branch Description.
    ' ***************************************************************** '
    Public Function GetSubBranchDetails(ByRef r_oSubBranch As ComboBox, ByRef r_oBranch As ComboBox, ByRef r_oBusiness As Object, ByVal v_lSubBranchId As Integer) As Integer

        Dim result As Integer = 0
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Const ACSubBranchId As Integer = 0
            Const ACSubBranchDescription As Integer = 3

            Dim lIndex, lSourceId As Integer
            Dim vSubBranchArray(,) As Object = Nothing

            r_oSubBranch.Items.Clear()

            lIndex = r_oBranch.SelectedIndex
            If lIndex < 0 Then
                Return result
            End If

            lSourceId = VB6.GetItemData(r_oBranch, lIndex)


            m_lReturn = r_oBusiness.GetSubBranches(v_lSourceId:=lSourceId, r_vSubBranchArray:=vSubBranchArray)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If
            If Not Information.IsArray(vSubBranchArray) Then
                Return result
            End If

            For i As Integer = 0 To vSubBranchArray.GetUpperBound(1)
                'VB PN19229
                Dim r_oSubBranch_NewIndex As Integer = -1

                r_oSubBranch_NewIndex = r_oSubBranch.Items.Add(CStr(vSubBranchArray(ACSubBranchDescription, i)).Trim())

                VB6.SetItemData(r_oSubBranch, r_oSubBranch_NewIndex, CInt(vSubBranchArray(ACSubBranchId, i)))

                If CInt(vSubBranchArray(ACSubBranchId, i)) = v_lSubBranchId Then
                    r_oSubBranch.SelectedIndex = r_oSubBranch_NewIndex
                End If
            Next i

            If v_lSubBranchId = 0 Then
                r_oSubBranch.SelectedIndex = 0
            End If

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="GetSubBranchDetails Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="GetSubBranchDetails", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    ' ***************************************************************** '
    '
    ' Name: AlternativeIdentifierChange
    '
    ' Description:
    '
    ' History: 13/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Sub AlternativeIdentifierChange(ByRef r_oAlternativeIdentifier As TextBox)

        Try

            Dim sText As String = ""
            Static bNoChange As Boolean

            'Field will always be 15 characters.
            'Characters are entered from the right and the field is padded out with
            'zeros from the left
            If bNoChange Then
                bNoChange = False
                Exit Sub
            End If
            If r_oAlternativeIdentifier.Text = m_kBlankAlternativeIdentifier Then
                Exit Sub
            End If
            If Strings.Len(r_oAlternativeIdentifier.Text) > 14 Then
                sText = r_oAlternativeIdentifier.Text
                If r_oAlternativeIdentifier.Text.StartsWith("0") Then
                    Mid(sText, 1, 1) = " "
                Else
                    Mid(sText, sText.Length, 1) = " "
                End If
                bNoChange = True
                r_oAlternativeIdentifier.Text = sText.Trim()
            Else
                bNoChange = True
                r_oAlternativeIdentifier.Text = "0" & r_oAlternativeIdentifier.Text
            End If
            r_oAlternativeIdentifier.SelectionStart = Strings.Len(r_oAlternativeIdentifier.Text)

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AlternativeIdentifierChange Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AlternativeIdentifierChange", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: StopNonNumericCharacters
    '
    ' Description:
    '
    ' History: 13/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Sub StopNonNumericCharacters(ByRef r_iKeyAscii As Integer)

        Try

            If (r_iKeyAscii < 48 Or r_iKeyAscii > 57) And r_iKeyAscii <> KeyAsciiBackSpace Then
                r_iKeyAscii = 0
            End If

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="StopNonNumericCharacters Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="StopNonNumericCharacters", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub
    ' ***************************************************************** '
    '
    ' Name: LoyaltyNumberLostFocus
    '
    ' Description:
    '
    ' History: 13/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Sub LoyaltyNumberLostFocus(ByVal v_oNewLoyaltyNumber As TextBox, ByVal v_sLoyaltyNumberScript As String, ByVal v_iTask As Integer, Optional ByVal v_sOldLoyaltyNumber As String = "")

        Try

            Dim vMessage As String = ""

            If v_oNewLoyaltyNumber.Text.Trim() <> "" And v_sLoyaltyNumberScript.Trim() <> "" Then
                'This is a non mandatory field for NRMA only
                m_lReturn = ValidateLoyaltyNumber(v_vLoyaltyNumber:=v_oNewLoyaltyNumber.Text, v_sLoyaltyNumberScript:=v_sLoyaltyNumberScript, r_vMessage:=vMessage)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                        MessageBox.Show(vMessage, "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateLoyaltyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="v_oNewLoyaltyNumber_LostFocus")
                    End If
                    v_oNewLoyaltyNumber.Focus()
                    Exit Sub
                End If
            End If


            If v_iTask <> gPMConstants.PMEComponentAction.PMAdd And v_oNewLoyaltyNumber.Text.Trim() <> v_sOldLoyaltyNumber And v_sOldLoyaltyNumber <> "" Then
                vMessage = "Do Risk Loyalty Numbers Require Amendment?"
                MessageBox.Show(vMessage, "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Question)
            End If

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="LoyaltyNumberLostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="LoyaltyNumberLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try

    End Sub

    ' ***************************************************************** '
    '
    ' Name: ValidateLoyaltyNumber
    '
    ' Description:
    '
    ' History: 05/06/2002 sj - Created.
    '
    ' ***************************************************************** '
	<HandleProcessCorruptedStateExceptions>
    Public Function ValidateLoyaltyNumber(ByVal v_vLoyaltyNumber As String, ByVal v_sLoyaltyNumberScript As String, ByRef r_vMessage As String) As Integer

        Dim result As Integer = 0
        Dim oScriptControl As MSScriptControl.ScriptControl = Nothing
        Dim sMessage As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vReturn As String = ""

            'Create an instance of Windows Scripting Control
            oScriptControl = New MSScriptControl.ScriptControl()
            If oScriptControl Is Nothing Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of Script control", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLoyaltyNumber")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set the Windows Scripting Language to VB Script
            oScriptControl.Language = "VBScript"

            oScriptControl.AddCode(v_sLoyaltyNumberScript)

            ' Start Execution of the Rule
            oScriptControl.Run("ValidateLoyaltyNumber", v_vLoyaltyNumber, r_vMessage, vReturn)

            result = CInt(Conversion.Val(vReturn))

            oScriptControl = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            If oScriptControl.Error.Description <> "" Then
                sMessage = "******************************************************" & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Warning: An Error occured running script" & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Description" & Strings.Chr(9) & ": " & oScriptControl.Error.Description & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Number" & Strings.Chr(9) & Strings.Chr(9) & ": " & CStr(oScriptControl.Error.Number) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Source" & Strings.Chr(9) & Strings.Chr(9) & ": " & oScriptControl.Error.Source & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Text" & Strings.Chr(9) & Strings.Chr(9) & ": " & oScriptControl.Error.Text & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Line" & Strings.Chr(9) & Strings.Chr(9) & ": " & CStr(oScriptControl.Error.Line) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Column" & Strings.Chr(9) & Strings.Chr(9) & ": " & CStr(oScriptControl.Error.Column) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "******************************************************"

                MessageBox.Show(sMessage, "ValidateLoyaltyNumber", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateLoyaltyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateLoyaltyNumber", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result

        End Try
    End Function

    ' ***************************************************************** '
    '
    ' Name: AlternativeIdentifierLostFocus
    '
    ' Description:
    '
    ' History: 13/06/2002 SJ - Created.
    '
    ' ***************************************************************** '
    Public Sub AlternativeIdentifierLostFocus(ByVal v_oAlternativeIdentifier As TextBox, ByVal v_sAlternativeIdentifierScript As String, ByVal v_iTask As Integer)

        Try

            Dim vMessage As Object = Nothing

            If v_oAlternativeIdentifier.Text.Trim() <> "" And v_sAlternativeIdentifierScript.Trim() <> "" Then
                'This is a non mandatory field for NRMA only

                m_lReturn = ValidateAlternativeIdentifier(v_vAlternativeIdentifier:=v_oAlternativeIdentifier.Text, v_sAlternativeIdentifierScript:=v_sAlternativeIdentifierScript, r_vMessage:=CStr(vMessage))
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then

                        MessageBox.Show(CStr(vMessage), "Personal Client", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                    Else
                        iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateLoyaltyNumber Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AlternativeIdentifierLostFocus")
                    End If
                    v_oAlternativeIdentifier.Focus()
                    Exit Sub
                End If
            End If

        Catch excep As System.Exception




            ' Log Error Message
            iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="AlternativeIdentifierLostFocus Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="AlternativeIdentifierLostFocus", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)

            Exit Sub

        End Try


    End Sub

    ' ***************************************************************** '
    '
    ' Name: ValidateAlternativeIdentifier
    '
    ' Description:
    '
    ' History: 05/06/2002 sj - Created.
    '
    ' ***************************************************************** '
	<HandleProcessCorruptedStateExceptions>
    Public Function ValidateAlternativeIdentifier(ByVal v_vAlternativeIdentifier As String, ByVal v_sAlternativeIdentifierScript As String, ByRef r_vMessage As String) As Integer
        Dim result As Integer = 0
        Dim oScriptControl As MSScriptControl.ScriptControl = Nothing
        Dim sMessage As String = ""
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim vReturn As String = ""

            'Create an instance of Windows Scripting Control
            oScriptControl = New MSScriptControl.ScriptControl()
            If oScriptControl Is Nothing Then
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="Failed to create instance of Script control", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAlternativeIdentifier")
                Return gPMConstants.PMEReturnCode.PMError
            End If

            ' Set the Windows Scripting Language to VB Script
            oScriptControl.Language = "VBScript"

            oScriptControl.AddCode(v_sAlternativeIdentifierScript)

            ' Start Execution of the Rule
            oScriptControl.Run("ValidateAlternativeIdentifier", v_vAlternativeIdentifier, r_vMessage, vReturn)

            result = CInt(Conversion.Val(vReturn))

            oScriptControl = Nothing

            Return result
        Catch excep As System.Exception
            result = gPMConstants.PMEReturnCode.PMError
            If oScriptControl.Error.Description <> "" Then
                sMessage = "******************************************************" & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Warning: An Error occured running script" & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Description" & Strings.Chr(9) & ": " & oScriptControl.Error.Description & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Number" & Strings.Chr(9) & Strings.Chr(9) & ": " & CStr(oScriptControl.Error.Number) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Source" & Strings.Chr(9) & Strings.Chr(9) & ": " & oScriptControl.Error.Source & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Text" & Strings.Chr(9) & Strings.Chr(9) & ": " & oScriptControl.Error.Text & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Line" & Strings.Chr(9) & Strings.Chr(9) & ": " & CStr(oScriptControl.Error.Line) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "Column" & Strings.Chr(9) & Strings.Chr(9) & ": " & CStr(oScriptControl.Error.Column) & Strings.Chr(13) & Strings.Chr(10)
                sMessage = sMessage & "******************************************************"

                MessageBox.Show(sMessage, "ValidateAlternativeIdentifier", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Else
                ' Log Error Message
                iPMFunc.LogMessage(iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:="ValidateAlternativeIdentifier Failed", vApp:=ACApp, vClass:=ACClass, vMethod:="ValidateAlternativeIdentifier", vErrNo:=Information.Err().Number, vErrDesc:=excep.Message, excep:=excep)
            End If

            Return result

        End Try
    End Function

    '' ***************************************************************** '
    ''
    '' Name: ValidateLoyaltyNumber
    ''
    '' Description:
    ''
    '' History: 05/06/2002 sj - Created.
    ''
    '' ***************************************************************** '
    'Public Function ValidateLoyaltyNumber( _
    ''    ByVal v_sLoyaltyNumber As String, _
    ''    ByRef r_sMessage As String) As Long
    '
    '    On Error GoTo Err_ValidateLoyaltyNumber
    '
    '    ValidateLoyaltyNumber = PMTrue
    '
    '    Dim lPos As Long
    '    Dim lStep1Number As Long
    '    Dim lStep2Number As Long
    '    Dim lCheckNumber As Long
    '    Dim lFinalNumber As Long
    '    Dim lRem As Long
    '    Dim lCheckDigit As Long
    '
    '    v_sLoyaltyNumber = Trim(v_sLoyaltyNumber)
    '
    '    If Len(v_sLoyaltyNumber) <> 10 Then
    '        r_sMessage = "Invalid: Loyalty Number length Must be 10 digits"
    '        ValidateLoyaltyNumber = PMFalse
    '        Exit Function
    '    End If
    '
    '    If IsNumeric(v_sLoyaltyNumber) = False Then
    '        r_sMessage = "Invalid: Loyalty Number must be numeric"
    '        ValidateLoyaltyNumber = PMFalse
    '        Exit Function
    '    End If
    '
    '    'Add the prefix
    '    v_sLoyaltyNumber = ksLoyaltyNumberPrefix & v_sLoyaltyNumber
    '
    '    'Step 1
    '    'Sum the digits in the even numbered positions from left to right
    '    lStep1Number = 0
    '    lPos = 2
    '    While lPos < 15
    '        lStep1Number = lStep1Number + Mid(v_sLoyaltyNumber, lPos, 1)
    '        lPos = lPos + 2
    '    Wend
    '
    '    'Step 2
    '    'Multiply each odd numbered digit (left to right) by 2.
    '    'If any results are 2 digits, sum them into one.
    '    'Sum the results.
    '    lStep2Number = 0
    '    lPos = 1
    '    While lPos <= 15
    '        lCheckNumber = Mid(v_sLoyaltyNumber, lPos, 1)
    '        'Multiply by 2
    '        lCheckNumber = lCheckNumber * 2
    '        ' If result is 2 digits
    '        If lCheckNumber > 9 Then
    '            lCheckNumber = (lCheckNumber - 10) + 1
    '        End If
    '        'Sum the result
    '        lStep2Number = lStep2Number + lCheckNumber
    '        lPos = lPos + 2
    '    Wend
    '
    '    'Step 3
    '    'Add the final results of steps 1 and 2.
    '    'If this result is divisible by  10 the check digit is 0, otherwise take the
    '    'last digit of the result of step 3 and subtract it from 10 to give the
    '    'check digit
    '    lFinalNumber = lStep1Number + lStep2Number
    '    lRem = lFinalNumber Mod 10
    '    If lRem = 0 Then
    '        lCheckDigit = 0
    '    Else
    '        lCheckDigit = 10 - lRem
    '    End If
    '
    '    'Validate the check digit
    '    If lCheckDigit <> CLng(Mid(v_sLoyaltyNumber, 16, 1)) Then
    '        r_sMessage = "Invalid: Loyalty Number Check digit incorrect"
    '        ValidateLoyaltyNumber = PMFalse
    '        Exit Function
    '    End If
    '
    '    Exit Function
    '
    'Err_ValidateLoyaltyNumber:
    '
    '    ValidateLoyaltyNumber = PMError
    '
    '    ' Log Error Message
    '    LogMessage _
    ''        iType:=PMLogOnError, _
    ''        sMsg:="ValidateLoyaltyNumber Failed", _
    ''        vApp:=ACApp, _
    ''        vClass:=ACClass, _
    ''        vMethod:="ValidateLoyaltyNumber", _
    ''        vErrNo:=Err.Number, _
    ''        vErrDesc:=Err.Description
    '
    '    Exit Function
    '

    ''' <summary>
    ''' Create and Save Party History Schema File
    ''' </summary>
    ''' <param name="v_oDatabase"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function CreateAndSavePartyHistorySchema(ByVal v_oDatabase As Object) As Integer
        Dim nReturn As Integer = gPMConstants.PMEReturnCode.PMTrue
        Dim sSchemaData As String = String.Empty
        Dim sFile As String = String.Empty
        Dim oWriter As StreamWriter
        Dim sDataSetsPath As String = String.Empty
        Dim oSchemaData As Object = Nothing
        Dim kMethodName As String = "CreateAndSavePartyHistorySchema"
        Try
            nReturn = gPMFunctions.GetPMRegSetting(v_lPMERegSettingRoot:=gPMConstants.PMERegSettingRoot.pmeRSRLocalMachine, v_lPMEProductFamily:=gPMConstants.PMEProductFamily.pmePFSiriusSolutions, v_lPMERegSettingLevel:=gPMConstants.PMERegSettingLevel.pmeRSLCommon, v_sSettingName:=GISSharedConstants.GISRegDataSetPath, r_sSettingValue:=sDataSetsPath, v_sSubKey:=GISSharedConstants.GISRegSubKey)
            If sDataSetsPath.Trim() = "" Then
                bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="Failed to find Data Sets Path (" & GISSharedConstants.GISRegDataSetPath & ") Registry Setting.", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                Return gPMConstants.PMEReturnCode.PMFalse
            End If
            If Not sDataSetsPath.EndsWith("\") Then
                sDataSetsPath = sDataSetsPath & "\"
            End If
            sFile = sDataSetsPath & kPartyHistoryDataModelCode & ".XSD"

            With v_oDatabase
                .Parameters.Clear()
                nReturn = .SQLSelect(sSQL:=kSchemaNameSQl, sSQLName:=kSchemaName, bStoredProcedure:=True, lNumberRecords:=-1, vResultArray:=oSchemaData)
                If nReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogError, sMsg:="v_oDatabase.SQLSelect failed to GetSchema Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=Information.Err().Description)
                    Return nReturn
                End If
            End With

            If Not Information.IsArray(oSchemaData) Then
                nReturn = gPMConstants.PMEReturnCode.PMNotFound
            Else
                sSchemaData = oSchemaData.GetValue(0, 0).ToString()
            End If

            If Not String.IsNullOrEmpty(sSchemaData) Then
                If Directory.Exists(sDataSetsPath) Then
                    If File.Exists(sFile) Then
                        File.Delete(sFile)
                    End If
                End If
                File.Create(sFile).Dispose()
                oWriter = New StreamWriter(sFile, True)
                oWriter.WriteLine(sSchemaData)
                oWriter.Close()
            End If
            Return nReturn
        Catch excep As Exception
            bPMFunc.LogMessage(sUsername:="", iType:=gPMConstants.PMELogLevel.PMLogOnError, sMsg:=kMethodName & " Failed", vApp:=ACApp, vClass:=ACClass, vMethod:=kMethodName, vErrNo:=Information.Err().Number, vErrDesc:=excep.Message)
            Return nReturn
        Finally
            oWriter = Nothing
        End Try
    End Function


End Module