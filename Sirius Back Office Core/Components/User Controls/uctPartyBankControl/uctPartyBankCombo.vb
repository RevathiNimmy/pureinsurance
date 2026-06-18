Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.ComponentModel
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
<System.Runtime.InteropServices.ProgId("uctPartyBankCombo_NET.uctPartyBankCombo")>
Partial Public Class uctPartyBankCombo
    Inherits System.Windows.Forms.UserControl
    'developer guide no. 107
    Public g_oObjectManager As bObjectManager.ObjectManager
    Private Const ACClass As String = "uctPartyBankCombo"
    Public Event ComboChange(ByVal Sender As Object, ByVal e As ComboChangeEventArgs)
    Public Event AddPartyBankItem(ByVal Sender As Object, ByVal e As AddPartyBankItemEventArgs)
    Public Event EditPartyBankItem(ByVal Sender As Object, ByVal e As EditPartyBankItemEventArgs)


    ' objects


    Private m_oBusiness As bSIRPartyBank.Business
    Private m_lReturn As gPMConstants.PMEReturnCode


    ' generic interface details
    Private m_iTask As gPMConstants.PMEComponentAction
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sTransactionType As String = ""

    Private m_vPartyBankDetails As Object
    Private m_vPartyAccountTypes(,) As Object

    Private m_vPartyCnt As Object
    Private m_vAccountId As Object
    Private m_vBankItem() As Object
    Private m_vSelectedPaymentIDs() As Object

    Private m_bEnableAdd As Boolean
    Private m_bEnableEdit As Boolean
    Private m_vIsBank As Object
    Private m_lSelectedPaymentID As Integer
    Private m_sSysOptionPaymentEdited As String = ""
    Private m_sBankPaymentTypeCode As String = ""
    Private m_sPartyName As String = ""
    Private m_bEditAllInstalmentPlans As Boolean

    Private m_bPopulating As Boolean
    Private m_lSelectedAccountType As String


    <Browsable(True)>
    Public Property PartyBankDetails() As Object
        Get
            Return m_vPartyBankDetails
        End Get
        Set(ByVal Value As Object)


            m_vPartyBankDetails = Value
        End Set
    End Property


    <Browsable(True)>
    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)


            m_vPartyCnt = Value
        End Set
    End Property

    'UPGRADE_NOTE: (7001) The following declaration (get BankItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BankItem() As Object
    'Return VB6.CopyArray(m_vBankItem)
    'End Function


    <Browsable(True)>
    Public Property EnableAdd() As Boolean
        Get
            'EnableAdd = m_bEnableAdd
        End Get
        Set(ByVal Value As Boolean)
            'm_bEnableAdd = bNewValue
            'cmdAddPaymentType.Enabled = bNewValue
            'PropertyChanged "EnableAdd"
        End Set
    End Property

    <Browsable(True)>
    Public Property SelectedPaymentID() As Integer
        Get
            Dim result As Integer = 0
            If cboAccountType.SelectedIndex >= 0 Then
                result = VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex)
            End If
            Return result
        End Get
        Set(ByVal Value As Integer)
            m_lSelectedPaymentID = Value
            SetSelectedIndex()
        End Set
    End Property

    <Browsable(True)>
    Public Property SelectedAccountType() As String
        Get
            Dim result As String = ""
            If cboAccountType.SelectedIndex >= 0 Then
                result = cboAccountType.SelectedItem.ToString()
            End If
            Return result
        End Get
        Set(ByVal Value As String)
            m_lSelectedAccountType = Value
        End Set
    End Property

    <Browsable(False)>
    Public ReadOnly Property SelectedPartyCnt() As Integer
        Get
            Return VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex)
        End Get
    End Property


    <Browsable(True)>
    Public Property EnableEdit() As Boolean
        Get
            'EnableAdd = m_bEnableEdit
        End Get
        Set(ByVal Value As Boolean)
            'm_bEnableEdit = vNewValue
            'cmdEditPaymentType.Enabled = vNewValue
            'PropertyChanged "EnableEdit"
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property EnableCombo() As Boolean
        Set(ByVal Value As Boolean)
            EnableDisableControls(Value)
        End Set
    End Property

    <Browsable(False)>
    Public WriteOnly Property IsBank() As Object
        Set(ByVal Value As Object)


            m_vIsBank = Value
        End Set
    End Property

    <Browsable(True)>
    Public Property BankPaymentTypeCode() As String
        Get
            Return m_sBankPaymentTypeCode
        End Get
        Set(ByVal Value As String)
            m_sBankPaymentTypeCode = Value
        End Set
    End Property


    <Browsable(False)>
    Public WriteOnly Property Task() As Integer
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Private ReadOnly Property SelectedPaymentTypes() As Object
        Get
            Dim result As Object = Nothing
            If Information.IsArray(m_vPartyBankDetails) Then
                m_vSelectedPaymentIDs = Nothing

                For lPaymentCount As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)

                    If CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lPaymentCount)) = 0 Then
                        If IsArrayEmpty(m_vSelectedPaymentIDs) Then
                            ReDim m_vSelectedPaymentIDs(0)
                        Else
                            ReDim Preserve m_vSelectedPaymentIDs(m_vSelectedPaymentIDs.GetUpperBound(0) + 1)
                        End If


                        m_vSelectedPaymentIDs(m_vSelectedPaymentIDs.GetUpperBound(0)) = m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, lPaymentCount)
                    End If
                Next
                If Information.IsArray(m_vSelectedPaymentIDs) Then
                    result = VB6.CopyArray(m_vSelectedPaymentIDs)
                End If
            End If
            Return result
        End Get
    End Property


    Private Sub cboAccountType_SelectedIndexChanged(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cboAccountType.SelectedIndexChanged
        If m_sBankPaymentTypeCode = "RECPAY" Or m_sBankPaymentTypeCode = "CLM" Then
            If Not m_bPopulating Or cboAccountType.SelectedIndex >= 0 Then
                RaiseEvent ComboChange(Me, New ComboChangeEventArgs(VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex)))
            End If
        Else
            RaiseEvent ComboChange(Me, New ComboChangeEventArgs(VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex)))
        End If

    End Sub

    Private Sub cmdAddPaymentType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdAddPaymentType.Click
        If ToSafeLong(m_vPartyCnt) <> 0 Or ToSafeLong(m_vAccountId) <> 0 Then
            Initialise()
            ProcessAddBank()
        End If
    End Sub

    Private Function SearchArrayIndexOnSelItem(ByVal lSelectedItem As Integer, ByRef r_lSelectedArrayIndex As Integer, ByVal lColumnId As MainModule.ENPartyBank) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SearchArrayIndexOnSelItem"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue




            For lArrayCount As Integer = m_vPartyBankDetails.GetLowerBound(1) To m_vPartyBankDetails.GetUpperBound(1)

                If CDbl(m_vPartyBankDetails(lColumnId, lArrayCount)) = lSelectedItem Then
                    r_lSelectedArrayIndex = lArrayCount
                    Exit For
                End If
            Next


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    Public Function ProcessAddBank() As Integer
        Const kMethodName As String = "ProcessAddBank"

        Dim objPartyBank As New frmPartyBankDetails
        objPartyBank.Task = gPMConstants.PMEComponentAction.PMAdd
        If Information.IsArray(m_vPartyBankDetails) Then
            objPartyBank.SelectedPaymentTypes = SelectedPaymentTypes
        End If
        If BankPaymentTypeCode <> "" Then
            objPartyBank.BankPaymentTypeCode = BankPaymentTypeCode
        End If


        objPartyBank.SetBusiness = m_oBusiness


        objPartyBank.PartyCnt = m_vPartyCnt


        If CDbl(m_vPartyCnt) > 0 Then

            m_lReturn = m_oBusiness.GetPartyName(vPartyCnt:=m_vPartyCnt, vAccountID:=0, vPartyName:=m_sPartyName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            objPartyBank.PartyName = m_sPartyName
        ElseIf CDbl(m_vAccountId) > 0 Then

            m_lReturn = m_oBusiness.GetPartyName(vPartyCnt:=0, vAccountID:=m_vAccountId, vPartyName:=m_sPartyName)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            objPartyBank.PartyName = m_sPartyName
        End If
        objPartyBank.ShowDialog()
        If objPartyBank.Status = gPMConstants.PMEReturnCode.PMOK Then
            m_vBankItem = objPartyBank.BankItem
            m_lReturn = CType(AddArrayItem(m_vBankItem), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "AddArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(SaveAdd(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SaveAdd Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


            If CDbl(m_vPartyCnt) > 0 Then
                m_lReturn = GetPartyAccountTypes()
            End If

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyAccountTypes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = PopulateCombos()

            cboAccountType.SelectedIndex = cboAccountType.Items.Count - 1
            'Send the updated details back to the interface instead of going bakck to DB to retrieve the updated details
            RaiseEvent AddPartyBankItem(Me, New AddPartyBankItemEventArgs(m_vPartyBankDetails))
        End If
        m_vPartyBankDetails = Nothing
    End Function

    Public Function AddArrayItem(ByVal vAddedItem() As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddArrayItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim uboundBankDetails As Integer

            If Not IsArrayEmpty(m_vPartyBankDetails) Then
                If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, uboundBankDetails) <> gPMConstants.PMEReturnCode.PMNotFound Then

                    uboundBankDetails = m_vPartyBankDetails.GetUpperBound(1) + 1

                    ReDim Preserve m_vPartyBankDetails(m_vPartyBankDetails.GetUpperBound(0), uboundBankDetails)
                End If
            Else
                ReDim m_vPartyBankDetails(MainModule.ENPartyBank.uBoundPartyBank, 0)
            End If

            uboundBankDetails = m_vPartyBankDetails.GetUpperBound(1)




            m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, uboundBankDetails) = gPMConstants.PMEComponentAction.PMAdd

            m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, uboundBankDetails) = "0"

            m_lReturn = CType(SetBankDetailsItem(vBankItem:=vAddedItem, lIndex:=uboundBankDetails), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetBankDetailsItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function SetBankDetailsItem(ByVal vBankItem() As Object, ByVal lIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetBankDetailsItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, lIndex) = vBankItem(MainModule.ENPartyBank.PartyBankId)


            m_vPartyBankDetails(MainModule.ENPartyBank.IsBank, lIndex) = vBankItem(MainModule.ENPartyBank.IsBank)


            m_vPartyBankDetails(MainModule.ENPartyBank.AccountId, lIndex) = vBankItem(MainModule.ENPartyBank.AccountId)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, lIndex) = vBankItem(MainModule.ENPartyBank.BankPaymentTypeId) '(ENPMLookups.Id)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankAccountTypeId, lIndex) = vBankItem(MainModule.ENPartyBank.BankAccountTypeId) '(ENPMLookups.Id)


            m_vPartyBankDetails(MainModule.ENPartyBank.IsBank, lIndex) = vBankItem(MainModule.ENPartyBank.IsBank)


            m_vPartyBankDetails(MainModule.ENPartyBank.AccountHolderName, lIndex) = vBankItem(MainModule.ENPartyBank.AccountHolderName)


            m_vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, lIndex) = vBankItem(MainModule.ENPartyBank.AccountNumber)

            m_vPartyBankDetails(MainModule.ENPartyBank.BIC, lIndex) = vBankItem(MainModule.ENPartyBank.BIC)

            m_vPartyBankDetails(MainModule.ENPartyBank.IBAN, lIndex) = vBankItem(MainModule.ENPartyBank.IBAN)

            If CDbl(vBankItem(MainModule.ENPartyBank.IsBank)) = 1 Then


                m_vPartyBankDetails(MainModule.ENPartyBank.BankNameId, lIndex) = vBankItem(MainModule.ENPartyBank.BankNameId) '(ENPMLookups.Id)
            Else


                m_vPartyBankDetails(MainModule.ENPartyBank.BankNameId, lIndex) = vBankItem(MainModule.ENPartyBank.BankNameId)
            End If


            m_vPartyBankDetails(MainModule.ENPartyBank.BankBranch, lIndex) = vBankItem(MainModule.ENPartyBank.BankBranch)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankBranchCode, lIndex) = vBankItem(MainModule.ENPartyBank.BankBranchCode)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd1, lIndex) = vBankItem(MainModule.ENPartyBank.BankAdd1)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd2, lIndex) = vBankItem(MainModule.ENPartyBank.BankAdd2)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd3, lIndex) = vBankItem(MainModule.ENPartyBank.BankAdd3)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankTown, lIndex) = vBankItem(MainModule.ENPartyBank.BankTown)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankPCode, lIndex) = vBankItem(MainModule.ENPartyBank.BankPCode)



            m_vPartyBankDetails(MainModule.ENPartyBank.BankRegion, lIndex) = vBankItem(MainModule.ENPartyBank.BankRegion)


            m_vPartyBankDetails(MainModule.ENPartyBank.BankCountry, lIndex) = vBankItem(MainModule.ENPartyBank.BankCountry)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCNum, lIndex) = vBankItem(MainModule.ENPartyBank.CCNum)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCStartDate, lIndex) = vBankItem(MainModule.ENPartyBank.CCStartDate)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCExpiryDate, lIndex) = vBankItem(MainModule.ENPartyBank.CCExpiryDate)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCIssueNum, lIndex) = vBankItem(MainModule.ENPartyBank.CCIssueNum)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCPIN, lIndex) = vBankItem(MainModule.ENPartyBank.CCPIN)



            m_vPartyBankDetails(MainModule.ENPartyBank.CCNameOnCard, lIndex) = vBankItem(MainModule.ENPartyBank.CCNameOnCard)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCManualAuthorisationNum, lIndex) = vBankItem(MainModule.ENPartyBank.CCManualAuthorisationNum)



            m_vPartyBankDetails(MainModule.ENPartyBank.IsRegistered, lIndex) = vBankItem(MainModule.ENPartyBank.IsRegistered)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd1, lIndex) = vBankItem(MainModule.ENPartyBank.CCAdd1)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd2, lIndex) = vBankItem(MainModule.ENPartyBank.CCAdd2)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd3, lIndex) = vBankItem(MainModule.ENPartyBank.CCAdd3)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCTown, lIndex) = vBankItem(MainModule.ENPartyBank.CCTown)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCPCode, lIndex) = vBankItem(MainModule.ENPartyBank.CCPCode)


            m_vPartyBankDetails(MainModule.ENPartyBank.CCCountry, lIndex) = vBankItem(MainModule.ENPartyBank.CCCountry)


            m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lIndex) = vBankItem(MainModule.ENPartyBank.IsDeleted)

            m_vPartyBankDetails(MainModule.ENPartyBank.CCIsDefault, lIndex) = vBankItem(MainModule.ENPartyBank.CCIsDefault)
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    Private Function SetBankItem(ByRef lSelectedIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetBankItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ReDim m_vBankItem(MainModule.ENPartyBank.uBoundPartyBank)

            m_vBankItem(MainModule.ENPartyBank.BankPaymentTypeId) = m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, lSelectedIndex)(MainModule.ENPMLookups.Id) ''PN 4699

            m_vBankItem(MainModule.ENPartyBank.PartyBankId) = m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.AccountHolderName) = m_vPartyBankDetails(MainModule.ENPartyBank.AccountHolderName, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.AccountId) = m_vPartyBankDetails(MainModule.ENPartyBank.AccountId, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.AccountNumber) = m_vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BIC) = m_vPartyBankDetails(MainModule.ENPartyBank.BIC, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.IBAN) = m_vPartyBankDetails(MainModule.ENPartyBank.IBAN, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.IsBank) = m_vPartyBankDetails(MainModule.ENPartyBank.IsBank, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankNameId) = m_vPartyBankDetails(MainModule.ENPartyBank.BankNameId, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankAccountTypeId) = m_vPartyBankDetails(MainModule.ENPartyBank.BankAccountTypeId, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankBranch) = m_vPartyBankDetails(MainModule.ENPartyBank.BankBranch, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankBranchCode) = m_vPartyBankDetails(MainModule.ENPartyBank.BankBranchCode, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankAdd1) = m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd1, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankAdd2) = m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd2, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankTown) = m_vPartyBankDetails(MainModule.ENPartyBank.BankTown, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankPCode) = m_vPartyBankDetails(MainModule.ENPartyBank.BankPCode, lSelectedIndex)


            m_vBankItem(MainModule.ENPartyBank.BankRegion) = m_vPartyBankDetails(MainModule.ENPartyBank.BankRegion, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.BankCountry) = m_vPartyBankDetails(MainModule.ENPartyBank.BankCountry, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCNum) = m_vPartyBankDetails(MainModule.ENPartyBank.CCNum, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCStartDate) = m_vPartyBankDetails(MainModule.ENPartyBank.CCStartDate, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCExpiryDate) = m_vPartyBankDetails(MainModule.ENPartyBank.CCExpiryDate, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCIssueNum) = m_vPartyBankDetails(MainModule.ENPartyBank.CCIssueNum, lSelectedIndex)

            If ToSafeInteger(m_vPartyBankDetails(MainModule.ENPartyBank.IsRegistered, lSelectedIndex)) = 0 Then
                m_vBankItem(MainModule.ENPartyBank.IsRegistered) = 0

                m_vBankItem(MainModule.ENPartyBank.CCPIN) = m_vPartyBankDetails(MainModule.ENPartyBank.CCPIN, lSelectedIndex)


                m_vBankItem(MainModule.ENPartyBank.CCAdd1) = m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd1, lSelectedIndex)

                m_vBankItem(MainModule.ENPartyBank.CCAdd2) = m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd2, lSelectedIndex)

                m_vBankItem(MainModule.ENPartyBank.CCAdd3) = m_vPartyBankDetails(MainModule.ENPartyBank.CCTown, lSelectedIndex)

                m_vBankItem(MainModule.ENPartyBank.CCTown) = m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd3, lSelectedIndex)

                m_vBankItem(MainModule.ENPartyBank.CCPCode) = m_vPartyBankDetails(MainModule.ENPartyBank.CCPCode, lSelectedIndex)

                m_vBankItem(MainModule.ENPartyBank.CCCountry) = m_vPartyBankDetails(MainModule.ENPartyBank.CCCountry, lSelectedIndex)
            Else
                m_vBankItem(MainModule.ENPartyBank.IsRegistered) = 1
            End If

            m_vBankItem(MainModule.ENPartyBank.CCPIN) = m_vPartyBankDetails(MainModule.ENPartyBank.CCPIN, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCNameOnCard) = m_vPartyBankDetails(MainModule.ENPartyBank.CCNameOnCard, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCManualAuthorisationNum) = m_vPartyBankDetails(MainModule.ENPartyBank.CCManualAuthorisationNum, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.IsDeleted) = m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lSelectedIndex)

            m_vBankItem(MainModule.ENPartyBank.CCIsDefault) = m_vPartyBankDetails(MainModule.ENPartyBank.CCIsDefault, lSelectedIndex)
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Public Function SaveAdd() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SaveAdd"
        Dim nTemp As Integer

        Try
            nTemp = ToSafeLong(m_vBankItem(ENPartyBank.PartyBankId))

            ' All parameters can be retrieved from BankItem array and passed to AddPartyBank Array
            m_lReturn = m_oBusiness.AddPartyBank(r_lPartyBankId:=nTemp,
                                                 vPartyCnt:=m_vPartyCnt,
                                                 vAccountID:=m_vAccountId,
                                                 sAccHolderName:=m_vBankItem(ENPartyBank.AccountHolderName),
                                                 sAccNumber:=m_vBankItem(ENPartyBank.AccountNumber),
                                                 lBankPaymentTypeID:=m_vBankItem(ENPartyBank.BankPaymentTypeId)(ENPMLookups.Id),
                                                 sBankAccountType:=m_vBankItem(ENPartyBank.BankAccountTypeId),
                                                 lIsBank:=m_vBankItem(ENPartyBank.IsBank),
                                                 vBankNameId:=m_vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id),
                                                 sBankBranch:=m_vBankItem(ENPartyBank.BankBranch),
                                                 sBankBranchCode:=m_vBankItem(ENPartyBank.BankBranchCode),
                                                 sBankAdd1:=m_vBankItem(ENPartyBank.BankAdd1),
                                                 sBankAdd2:=m_vBankItem(ENPartyBank.BankAdd2),
                                                 sBankAdd3:=ToSafeString(m_vBankItem(ENPartyBank.BankAdd3)),
                                                 sBankTown:=m_vBankItem(ENPartyBank.BankTown),
                                                 sBankPCode:=m_vBankItem(ENPartyBank.BankPCode),
                                                 sBankRegion:=m_vBankItem(ENPartyBank.BankRegion),
                                                 sBankCountry:=m_vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Id),
                                                 sCCNum:=m_vBankItem(ENPartyBank.CCNum),
                                                 sCCStartDate:=m_vBankItem(ENPartyBank.CCStartDate),
                                                 sCCExpiryDate:=m_vBankItem(ENPartyBank.CCExpiryDate),
                                                 sCCIssueNum:=m_vBankItem(ENPartyBank.CCIssueNum),
                                                 sCCPin:=m_vBankItem(ENPartyBank.CCPIN),
                                                 lIsRegistered:=m_vBankItem(ENPartyBank.IsRegistered),
                                                 sCCAdd1:=m_vBankItem(ENPartyBank.CCAdd1),
                                                 sCCAdd2:=m_vBankItem(ENPartyBank.CCAdd2),
                                                 sCCAdd3:=m_vBankItem(ENPartyBank.CCAdd3),
                                                 sCCTown:=m_vBankItem(ENPartyBank.CCTown),
                                                 sCCPCode:=m_vBankItem(ENPartyBank.CCPCode),
                                                 sCCCountry:=m_vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Id),
                                                 sCCNameOnCard:=m_vBankItem(ENPartyBank.CCNameOnCard),
                                                 sCCManaulAuthNumber:=m_vBankItem(ENPartyBank.CCManualAuthorisationNum),
                                                 sBIC:=m_vBankItem(ENPartyBank.BIC),
                                                 sIBAN:=m_vBankItem(ENPartyBank.IBAN),
                                                 v_nIsDefault:=ToSafeInteger(m_vBankItem(ENPartyBank.CCIsDefault), 0))

            m_vBankItem(ENPartyBank.PartyBankId) = nTemp
            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "AddPartyBank Failed", PMELogLevel.PMLogError)
            End If


            ' All parameters can be retrieved from BankItem array and passed to AddPartyBankHistory Array
            m_lReturn = m_oBusiness.AddPartyBankHistory(lPartyBankId:=m_vBankItem(ENPartyBank.PartyBankId),
                                                        sActionCode:="Setup",
                                                        vPartyCnt:=m_vPartyCnt,
                                                        vAccountID:=m_vBankItem(ENPartyBank.AccountId),
                                                        sAccHolderName:=m_vBankItem(ENPartyBank.AccountHolderName),
                                                        sAccNumber:=m_vBankItem(ENPartyBank.AccountNumber),
                                                        lBankPaymentTypeID:=m_vBankItem(ENPartyBank.BankPaymentTypeId)(ENPMLookups.Id),
                                                        sBankAccountType:=m_vBankItem(ENPartyBank.BankAccountTypeId),
                                                        vBankNameId:=m_vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id),
                                                        sBankBranch:=m_vBankItem(ENPartyBank.BankBranch),
                                                        sBankBranchCode:=m_vBankItem(ENPartyBank.BankBranchCode),
                                                        sBankAdd1:=m_vBankItem(ENPartyBank.BankAdd1),
                                                        sBankAdd2:=m_vBankItem(ENPartyBank.BankAdd2),
                                                        sBankAdd3:=m_vBankItem(ENPartyBank.BankAdd3),
                                                        sBankTown:=m_vBankItem(ENPartyBank.BankTown),
                                                        sBankPCode:=m_vBankItem(ENPartyBank.BankPCode),
                                                        sBankRegion:=m_vBankItem(ENPartyBank.BankRegion),
                                                        sBankCountry:=m_vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Id),
                                                        sCCNum:=m_vBankItem(ENPartyBank.CCNum),
                                                        sCCStartDate:=m_vBankItem(ENPartyBank.CCStartDate),
                                                        sCCExpiryDate:=m_vBankItem(ENPartyBank.CCExpiryDate),
                                                        sCCIssueNum:=m_vBankItem(ENPartyBank.CCIssueNum),
                                                        sCCPin:=m_vBankItem(ENPartyBank.CCPIN),
                                                        lIsRegistered:=m_vBankItem(ENPartyBank.IsRegistered),
                                                        sCCAdd1:=m_vBankItem(ENPartyBank.CCAdd1),
                                                        sCCAdd2:=m_vBankItem(ENPartyBank.CCAdd2),
                                                        sCCAdd3:=m_vBankItem(ENPartyBank.CCAdd3),
                                                        sCCTown:=m_vBankItem(ENPartyBank.CCTown),
                                                        sCCPCode:=m_vBankItem(ENPartyBank.CCPCode),
                                                        sCCCountry:=m_vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Id),
                                                        lUserId:=ENPartyBankHistory.UserID,
                                                        sCCNameOnCard:=m_vBankItem(ENPartyBank.CCNameOnCard),
                                                        sCCManaulAuthNumber:=m_vBankItem(ENPartyBank.CCManualAuthorisationNum),
                                                        sBIC:=m_vBankItem(ENPartyBank.BIC),
                                                        sIBAN:=m_vBankItem(ENPartyBank.IBAN),
                                                        v_nIsDefault:=ToSafeInteger(m_vBankItem(ENPartyBank.CCIsDefault), 0))

            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "AddPartyBankHistory Failed", PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return nResult
    End Function

    Public Function ProcessEditBank() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessEditBank"
        Dim lSelectedArrayIndex As Integer
        Dim objPartyBank As New frmPartyBankDetails

        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(SearchArrayIndexOnSelItem(lSelectedItem:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), r_lSelectedArrayIndex:=lSelectedArrayIndex, lColumnId:=MainModule.ENPartyBank.PartyBankId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetBankItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            If BankPaymentTypeCode <> "" Then
                objPartyBank.BankPaymentTypeCode = BankPaymentTypeCode
            End If
            m_lReturn = CType(SetBankItem(lSelectedArrayIndex), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetBankItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            objPartyBank.Task = gPMConstants.PMEComponentAction.PMEdit
            objPartyBank.BankItem = m_vBankItem
            objPartyBank.SetBusiness = m_oBusiness
            objPartyBank.PartyCnt = m_vPartyCnt
            objPartyBank.ShowDialog()

            If objPartyBank.Status = gPMConstants.PMEReturnCode.PMOK Then
                m_vBankItem = objPartyBank.BankItem

                m_lReturn = CType(EditArrayItem(vEditedItem:=m_vBankItem), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "EditArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
                ' Save Details back to the database
                m_lReturn = CType(SaveEdit(), gPMConstants.PMEReturnCode)

                If objPartyBank.IsMediaTypeChanged Then
                    m_lReturn = GetPartyAccountTypes()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "GetPartyAccountTypes Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = PopulateCombos()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "PopulateCombos Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If
                End If

                'Send the updated details back to the interface instead of going back to database to retrieve the updated details
                RaiseEvent EditPartyBankItem(Me, New EditPartyBankItemEventArgs(m_vPartyBankDetails))
            End If
            m_vPartyBankDetails = Nothing

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function EditArrayItem(ByVal vEditedItem As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "EditArrayItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim lSelectedArrayIndex As Integer

            m_lReturn = CType(SearchArrayIndexOnSelItem(lSelectedItem:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), r_lSelectedArrayIndex:=lSelectedArrayIndex, lColumnId:=MainModule.ENPartyBank.PartyBankId), gPMConstants.PMEReturnCode)


            If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSelectedArrayIndex) <> gPMConstants.PMEComponentAction.PMAdd Then

                m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSelectedArrayIndex) = gPMConstants.PMEComponentAction.PMEdit
            End If


            m_lReturn = CType(SetBankDetailsItem(vBankItem:=vEditedItem, lIndex:=lSelectedArrayIndex), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetBankDetailsItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume

            '        Return result
        End Try
        Return result
    End Function

    ''' <summary>
    ''' SaveEdit
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SaveEdit() As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SaveAdd"
        Try
            m_lReturn = m_oBusiness.EditPartyBank(lPartyBankId:=m_vBankItem(ENPartyBank.PartyBankId),
                                                  vPartyCnt:=m_vPartyCnt,
                                                  vAccountID:=1,
                                                  sAccHolderName:=m_vBankItem(ENPartyBank.AccountHolderName),
                                                  sAccNumber:=m_vBankItem(ENPartyBank.AccountNumber),
                                                  lBankPaymentTypeID:=m_vBankItem(ENPartyBank.BankPaymentTypeId)(ENPMLookups.Id),
                                                  sBankAccountType:=m_vBankItem(ENPartyBank.BankAccountTypeId),
                                                  lIsBank:=m_vBankItem(ENPartyBank.IsBank),
                                                  vBankNameId:=m_vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id),
                                                  sBankBranch:=m_vBankItem(ENPartyBank.BankBranch),
                                                  sBankBranchCode:=m_vBankItem(ENPartyBank.BankBranchCode),
                                                  sBankAdd1:=m_vBankItem(ENPartyBank.BankAdd1),
                                                  sBankAdd2:=m_vBankItem(ENPartyBank.BankAdd2),
                                                  sBankAdd3:=m_vBankItem(ENPartyBank.BankAdd3),
                                                  sBankTown:=m_vBankItem(ENPartyBank.BankTown),
                                                  sBankPCode:=m_vBankItem(ENPartyBank.BankPCode),
                                                  sBankRegion:=m_vBankItem(ENPartyBank.BankRegion),
                                                  sBankCountry:=m_vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Id),
                                                  sCCNum:=m_vBankItem(ENPartyBank.CCNum),
                                                  sCCStartDate:=m_vBankItem(ENPartyBank.CCStartDate),
                                                  sCCExpiryDate:=m_vBankItem(ENPartyBank.CCExpiryDate),
                                                  sCCIssueNum:=m_vBankItem(ENPartyBank.CCIssueNum),
                                                  sCCPin:=m_vBankItem(ENPartyBank.CCPIN),
                                                  lIsRegistered:=m_vBankItem(ENPartyBank.IsRegistered),
                                                  sCCAdd1:=m_vBankItem(ENPartyBank.CCAdd1),
                                                  sCCAdd2:=m_vBankItem(ENPartyBank.CCAdd2),
                                                  sCCAdd3:=m_vBankItem(ENPartyBank.CCAdd3),
                                                  sCCTown:=m_vBankItem(ENPartyBank.CCTown),
                                                  sCCPCode:=m_vBankItem(ENPartyBank.CCPCode),
                                                  sCCCountry:=m_vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Id),
                                                  sCCNameOnCard:=m_vBankItem(ENPartyBank.CCNameOnCard),
                                                  sCCManaulAuthNumber:=m_vBankItem(ENPartyBank.CCManualAuthorisationNum),
                                                  bEditInstalmentPlans:=m_bEditAllInstalmentPlans,
                                                  sBIC:=m_vBankItem(ENPartyBank.BIC),
                                                  sIBAN:=m_vBankItem(ENPartyBank.IBAN),
                                                  v_nIsDefault:=ToSafeInteger(m_vBankItem(ENPartyBank.CCIsDefault), 0))

            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "AddPartyBank Failed", PMELogLevel.PMLogError)
            End If

            m_lReturn = m_oBusiness.AddPartyBankHistory(lPartyBankId:=m_vBankItem(ENPartyBank.PartyBankId),
                                                        sActionCode:="Amendment",
                                                        vPartyCnt:=m_vPartyCnt,
                                                        vAccountID:=m_vBankItem(ENPartyBank.AccountId),
                                                        sAccHolderName:=m_vBankItem(ENPartyBank.AccountHolderName),
                                                        sAccNumber:=m_vBankItem(ENPartyBank.AccountNumber),
                                                        lBankPaymentTypeID:=m_vBankItem(ENPartyBank.BankPaymentTypeId)(ENPMLookups.Id),
                                                        sBankAccountType:=m_vBankItem(ENPartyBank.BankAccountTypeId),
                                                        vBankNameId:=m_vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Id),
                                                        sBankBranch:=m_vBankItem(ENPartyBank.BankBranch),
                                                        sBankBranchCode:=m_vBankItem(ENPartyBank.BankBranchCode),
                                                        sBankAdd1:=m_vBankItem(ENPartyBank.BankAdd1),
                                                        sBankAdd2:=m_vBankItem(ENPartyBank.BankAdd2),
                                                        sBankAdd3:=m_vBankItem(ENPartyBank.BankAdd3),
                                                        sBankTown:=m_vBankItem(ENPartyBank.BankTown),
                                                        sBankPCode:=m_vBankItem(ENPartyBank.BankPCode),
                                                        sBankRegion:=m_vBankItem(ENPartyBank.BankRegion),
                                                        sBankCountry:=m_vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Id),
                                                        sCCNum:=m_vBankItem(ENPartyBank.CCNum),
                                                        sCCStartDate:=m_vBankItem(ENPartyBank.CCStartDate),
                                                        sCCExpiryDate:=m_vBankItem(ENPartyBank.CCExpiryDate),
                                                        sCCIssueNum:=m_vBankItem(ENPartyBank.CCIssueNum),
                                                        sCCPin:=m_vBankItem(ENPartyBank.CCPIN),
                                                        lIsRegistered:=m_vBankItem(ENPartyBank.IsRegistered),
                                                        sCCAdd1:=m_vBankItem(ENPartyBank.CCAdd1),
                                                        sCCAdd2:=m_vBankItem(ENPartyBank.CCAdd2),
                                                        sCCAdd3:=m_vBankItem(ENPartyBank.CCAdd3),
                                                        sCCTown:=m_vBankItem(ENPartyBank.CCTown),
                                                        sCCPCode:=m_vBankItem(ENPartyBank.CCPCode),
                                                        sCCCountry:=m_vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Id),
                                                        lUserId:=ENPartyBankHistory.UserID,
                                                        sCCNameOnCard:=m_vBankItem(ENPartyBank.CCNameOnCard),
                                                        sCCManaulAuthNumber:=m_vBankItem(ENPartyBank.CCManualAuthorisationNum),
                                                        sBIC:=m_vBankItem(ENPartyBank.BIC),
                                                        sIBAN:=m_vBankItem(ENPartyBank.IBAN),
                                                        v_nIsDefault:=ToSafeInteger(m_vBankItem(ENPartyBank.CCIsDefault), 0))

            If m_lReturn <> PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "AddPartyBankHistory Failed", PMELogLevel.PMLogError)
            End If

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=SaveAdd(), excep:=ex)
        Finally
        End Try
        Return nResult
    End Function

    Private Sub cmdEditPaymentType_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdEditPaymentType.Click
        Try

            Dim bIsLinked As Boolean

            If cboAccountType.SelectedIndex >= 0 Then
                If VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex) > 0 Then
                    Initialise()
                    m_lReturn = CType(ISPartyLinkedwithInstalments(bIsLinked), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError("ISPartyLinkedwithInstalments", "ISPartyLinkedwithInstalments Failed")
                    End If
                    If bIsLinked Then
                        If MessageBox.Show("This account type is being used by other instalments plans." & Strings.Chr(13) & Strings.Chr(10) &
                                           "Do you wish to continue.", "Account Linked", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                            m_bEditAllInstalmentPlans = True
                        Else
                            m_bEditAllInstalmentPlans = False
                            Exit Sub
                        End If
                    End If

                    GetPartyBankDetails()
                    ProcessEditBank()
                Else
                    MessageBox.Show("Please Select Account Type", "Account Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                End If
            End If

        Catch ex As Exception
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "cmdEditPaymentType_Click Failed", "cmdEditPaymentType_Click", Information.Err().Number, Information.Err().Description, excep:=ex)
        Finally

        End Try
        Exit Sub
    End Sub

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Create an instance of the object manager.
            g_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If g_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With g_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInstance of bClMCase.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = ProcessInterface()

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "ProcessInterface Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Case Claim links
            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetCaseClaimLink Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = PopulateScreen()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "PopulateCaseClaimList Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Public Function PopulateScreen() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "PopulateScreen"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_bPopulating = True

            If CDbl(m_vPartyCnt) > 0 Then
                m_lReturn = GetPartyAccountTypes()
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyAccountTypes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = PopulateCombos()
            m_bPopulating = False
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function

    Private Function GetPartyBankDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyBankDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            m_lReturn = m_oBusiness.GetPartyBankDetails(vPartyBankDetails:=m_vPartyBankDetails, vPartyCnt:=m_vPartyCnt, vAccountID:=m_vAccountId)
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function
    Private Function ISPartyLinkedwithInstalments(ByRef bIsLinked As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ISPartyLinkedwithInstalments"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.ISPartyBankLinkedWithInstalment(lPartyBankId:=VB6.GetItemData(cboAccountType, cboAccountType.SelectedIndex), bisLinked:=bIsLinked)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function
    Private Function GetPartyAccountTypes() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyAccountTypes"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            If m_oBusiness Is Nothing Then
                Dim temp_m_oBusiness As Object = Nothing
                m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
                m_oBusiness = temp_m_oBusiness
            End If

            m_lReturn = m_oBusiness.GetPartyAccountTypes(vPartyAccountsType:=m_vPartyAccountTypes, vPartyCnt:=m_vPartyCnt, vAccountID:=m_vAccountId, sBankPaymentTypeCode:=m_sBankPaymentTypeCode, vIsBank:=m_vIsBank)
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyAccountTypes Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            'Get Party Bank Details
            m_lReturn = GetPartyBankDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    ''' <summary>
    ''' PopulateCombos
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateCombos() As Integer
        Dim nResult As Integer = 0
        Const kMethodName As String = "PopulateCombos"
        Try

            nResult = PMEReturnCode.PMTrue

            cboAccountType.Items.Clear()
            Dim cboAccountType_NewIndex As Integer = -1
            cboAccountType_NewIndex = cboAccountType.Items.Add("(Select Account Type)")
            If ToSafeLong(m_vPartyCnt) = 0 And ToSafeLong(m_vAccountId) = 0 Then
                Return nResult
            End If
            If Information.IsArray(m_vPartyAccountTypes) Then
                For ivar As Integer = 0 To m_vPartyAccountTypes.GetUpperBound(1)
                    'Developer Guie no 153
                    cboAccountType_NewIndex = cboAccountType.Items.Add(New VB6.ListBoxItem(CStr(m_vPartyAccountTypes(1, ivar)), CInt(m_vPartyAccountTypes(0, ivar))))
                Next
                If m_vPartyAccountTypes.GetUpperBound(1) >= 0 Then
                    For iCnt As Integer = 0 To cboAccountType.Items.Count - 1
                        If VB6.GetItemData(cboAccountType, iCnt) = m_lSelectedPaymentID Then
                            cboAccountType.SelectedIndex = iCnt
                            Exit For
                        End If
                    Next iCnt
                Else
                    cboAccountType.SelectedIndex = 0
                End If
            Else
                cboAccountType.SelectedIndex = 0
            End If
            If m_iTask = gPMConstants.PMEComponentAction.PMAdd Or m_iTask = gPMConstants.PMEComponentAction.PMEdit Or m_iTask = gPMConstants.PMEComponentAction.PMView Then
                If cboAccountType.SelectedIndex = 0 Then
                    If cboAccountType.Items.Count >= 2 Then
                        cboAccountType.SelectedIndex = 1
                    Else
                        cboAccountType.SelectedIndex = 0
                    End If
                End If
            End If
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally




        End Try
        Return nResult
    End Function

    Private Function SetSelectedIndex() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetSelectedIndex"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            For iLoop As Integer = 0 To cboAccountType.Items.Count - 1
                If VB6.GetItemData(cboAccountType, iLoop) = m_lSelectedPaymentID Then
                    cboAccountType.SelectedIndex = iLoop
                    Exit For
                End If
            Next

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally






        End Try
        Return result
    End Function

    Public Function ProcessInterface() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInterface"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(GetOption(v_iOptionNumber:=5062, r_sOptionValue:=m_sSysOptionPaymentEdited), gPMConstants.PMEReturnCode)

            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, " Failed to get system Option : 5062", gPMConstants.PMELogLevel.PMLogError)
            End If

            If m_sSysOptionPaymentEdited = "1" Then
                cmdEditPaymentType.Visible = False
                cmdAddPaymentType.Visible = False
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '		Return result
            '		Resume 
            '		Return result
        End Try
        Return result
    End Function


    Private Sub UserControl_InitProperties()
        EnableAdd = True
    End Sub
    'developer guide no sloution no 1
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)
        EnableAdd = CBool(PropBag.ReadProperty("EnableAdd", True))
    End Sub
    'developer guide no sloution no 1
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)
        PropBag.WriteProperty("EnableAdd", m_bEnableAdd, True)
    End Sub

    Private Sub EnableDisableControls(ByRef bNewValue As Boolean)
        If bNewValue Then
            cboAccountType.Enabled = True
            cmdAddPaymentType.Enabled = True
            cmdEditPaymentType.Enabled = True
        Else
            'cboAccountType.Clear
            If cboAccountType.Items.Count <> 0 And m_lSelectedPaymentID = 0 Then
                cboAccountType.SelectedIndex = 0
            End If
            cboAccountType.Enabled = False
            cmdAddPaymentType.Enabled = False
            cmdEditPaymentType.Enabled = False
        End If
    End Sub
    'developer guide no. 107

    Public Function GetOption(ByVal v_iOptionNumber As Integer, ByRef r_sOptionValue As String, Optional ByRef vDatabase As Object = Nothing) As Integer
        Dim result As Integer = 0

        Try

            Dim oSystemOption As bSIROptions.Business
            Dim lReturn As Integer
            result = gPMConstants.PMEReturnCode.PMTrue

            If oSystemOption Is Nothing Then

                ' Get an instance of the business object via
                ' the public object manager.
                Dim temp_oSystemOption As Object = Nothing
                lReturn = g_oObjectManager.GetInstance(temp_oSystemOption, "bSIROptions.Business", "ClientManager")
                oSystemOption = temp_oSystemOption

                If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    Return gPMConstants.PMEReturnCode.PMFalse
                End If

            End If
            'Developer Guide No.33
            lReturn = oSystemOption.GetOption(v_iOptionNumber, r_sOptionValue)

            If lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                r_sOptionValue = "0"

            End If

            oSystemOption.Dispose()

            oSystemOption = Nothing

            Return result

        Catch excep As System.Exception



            result = gPMConstants.PMEReturnCode.PMError

            ' Log Error Message
            iPMFunc.LogMessage(gPMConstants.PMELogLevel.PMLogOnError, "GetOption Failed", ACApp, CInt(ACClass), "GetOption", Information.Err().Number, excep.Message, excep:=excep)

            Return result

        End Try
    End Function
    'end'
End Class
