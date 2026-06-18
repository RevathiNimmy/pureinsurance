Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports Microsoft.VisualBasic.Compatibility.VB6
Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Imports System.Runtime.InteropServices

<System.Runtime.InteropServices.ProgId("uctPartyBankControl_NET.uctPartyBankControl")> _
Partial Public Class uctPartyBankControl
    Inherits System.Windows.Forms.UserControl
    ' issues
    ' -->if user has added and then deleted the bank item then
    '       --> Detect the selected item of an array
    '       --> Design a function to delete item from an array and place the last item of an array to that array item
    '       --> Then at the time adding next item of an array search whether last item is empty or not,
    '       --> if it is empty then don't increase its upper bound
    '

    'HIGH PRIORITY
    ' --> user details for history details                                                Done
    ' --> Update all records of pfpremium finance and history on live instalments
    ' --> Work on Un Delete                                                         Done



    ' --> Handle Delete button
    ' --> Description for Lookup fields when loaded in the list  -Done pending in history
    ' --> None enteries in non mandaory lookups


    ' --> Set List view items to ghosted when deleted and it exists                 Done
    ' --> in the database, user can also un-delete it only if their exists
    ' --> no details against this payment type

    ' Implementing RES file
    ' Resize the user control on the basis of Parent Screen
    ' Credit Card details are still inconsistent.
    ' Place icons in the listview                               - Done
    ' Generating ListView columns at run time                   - Done

    'DEEPAK_COMMENT: Replaced iPMFunc.GetResData with GetResData in the whole document



    Private Const ACClass As String = "uctPartyBankControl"

    ' objects
    Private m_oObjectManager As bObjectManager.ObjectManager

    Private m_oBusiness As bSIRPartyBank.Business
    Private m_lReturn As gPMConstants.PMEReturnCode

    ' generic interface details
    Private m_iTask As Integer
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_lNavigate As Integer
    Private m_lProcessMode As Integer
    Private m_dtEffectiveDate As Date
    Private m_sTransactionType As String = ""

    ' Array declared to keep the updated bank details
    Private m_vPartyBankDetails(,) As Object
    Private m_vBankHistory As Object
    Private m_lSelectedArrayIndexOnTag As Integer
    Private m_lListSelectedItem As Integer
    Private m_lSelectedTag As Integer
    Private m_vPartyBankHistory As Object
    'developer guide no. 17
    Private m_vSelectedPaymentIDs As Object
    Private m_vSelectedAccountIDs As Object
    ' Bank Item Added or Edited, it will be pumped into BankDetails Array
    Private m_vBankItem() As Object

    ' Variables
    Private m_vPartyCnt As Object
    'developer guide no. 101
    Private m_vAccountId As Object
    Private m_bIsInitialised As Boolean
    Private m_lWidth As Integer
    Private m_lHeight As Integer
    Private m_bReadOnly As Boolean
    Private m_sPartyName As String = ""
    Private m_bMultiSelect As Boolean
    Private m_vSelectedTags() As Object
    Private m_vSelectedRows() As Object
    ' Event that will pass the interface with the changed Bank Details
    Public Event RefreshBankDetails(ByVal Sender As Object, ByVal e As RefreshBankDetailsEventArgs)
    Private m_vOldBankItem As Object
    Private m_sUniqueId As String
    Private m_sScreenHierarchy As String

    'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
    ' Removed system option m_bIsCCAuthorisationOff and corresponding property
    ' Adding a system option m_bIsExternalCreditCardProcessing
    Private m_bIsExternalCreditCardProcessing As Boolean

    Private hScrollValue As Integer = 0

    'Win32 API declarations to preserve list view horizontal scroll position after sort
    Const LVM_FIRST As Int32 = &H1000
    Const LVM_SCROLL As Int32 = LVM_FIRST + 20
    Const SBS_HORZ As Integer = 0

    ' Control array to store the first and last
    ' text box controls for each tab.
    Private m_ctlTabFirstLast(,) As Control



    <Browsable(True)>
    Public Property ResetPreviousOne() As Boolean

    <Browsable(True)> _
    Public Property IsExternalCreditCardProcessing() As Boolean
        Get
            Return m_bIsExternalCreditCardProcessing
        End Get
        Set(ByVal Value As Boolean)
            m_bIsExternalCreditCardProcessing = Value
        End Set
    End Property
    'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
    <Browsable(True)> _
    Public Property PartyBankDetails() As Object
        Get
            Return VB6.CopyArray(m_vPartyBankDetails)
        End Get
        Set(ByVal Value As Object)
            m_vPartyBankDetails = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property PartyBankHistory() As Object
        Get
            Return m_vPartyBankHistory
        End Get
        Set(ByVal Value As Object)


            m_vPartyBankHistory = Value
        End Set
    End Property

    'UPGRADE_NOTE: (7001) The following declaration (get SelectedArrayItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SelectedArrayItem() As Integer
    '
    'End Function


    <Browsable(True)> _
       Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)
            m_vPartyCnt = Value
        End Set
    End Property


    '<Browsable(True)> _
    Public Property AccountId() As Object
        Get
            Return m_vAccountId
        End Get
        Set(ByVal Value As Object)

            m_vAccountId = Value
        End Set
    End Property

    Public Property UniqueId() As String
        Get
            Return m_sUniqueId
        End Get
        Set(ByVal Value As String)
            m_sUniqueId = Value
        End Set
    End Property

    Public Property ScreenHierarchy() As String
        Get
            Return m_sScreenHierarchy
        End Get
        Set(ByVal Value As String)
            m_sScreenHierarchy = Value
        End Set
    End Property

    'UPGRADE_NOTE: (7001) The following declaration (get BankItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function BankItem() As Object
    'Return VB6.CopyArray(m_vBankItem)
    'End Function
    <Browsable(False)> _
    Public WriteOnly Property PartyName() As String
        Set(ByVal Value As String)
            m_sPartyName = Value
        End Set
    End Property

    <Browsable(False)> _
    Public WriteOnly Property ReadOnly_Renamed() As Boolean
        Set(ByVal Value As Boolean)
            m_bReadOnly = Value
            SetupControl()
        End Set
    End Property


    Private Property ListSelectedItem() As Integer
        Get
            Return m_lListSelectedItem
        End Get
        Set(ByVal Value As Integer)
            m_lListSelectedItem = Value
        End Set
    End Property

    'UPGRADE_NOTE: (7001) The following declaration (get SelectedTag) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function SelectedTag() As Integer
    'Return m_lSelectedTag
    'End Function

    Private ReadOnly Property SelectedArrayIndexOnTag() As Integer
        Get
            Dim result As Integer = 0
            If ListSelectedItem >= 0 Then

                m_lSelectedTag = Convert.ToString(lvwBankDetailsList.Items.Item(ListSelectedItem).Tag)

                m_lReturn = CType(SearchArrayIndexOnTag(lSelectedTag:=m_lSelectedTag, lSelectedArrayIndex:=m_lSelectedArrayIndexOnTag, lColumnId:=MainModule.ENPartyBank.RowIndex), gPMConstants.PMEReturnCode)

                result = m_lSelectedArrayIndexOnTag
            End If
            Return result
        End Get
    End Property

    Private ReadOnly Property SelectedPaymentTypes() As Object
        Get
            Dim result As Object = Nothing
            Dim v_lCount As Integer
            If Information.IsArray(m_vPartyBankDetails) Then

                ReDim m_vSelectedPaymentIDs(1)
                For lPaymentCount As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)
                    m_vSelectedPaymentIDs(v_lCount) = m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, lPaymentCount)(MainModule.ENPMLookups.Id)
                    m_vSelectedPaymentIDs(v_lCount + 1) = m_vPartyBankDetails(MainModule.ENPartyBank.BankAccountTypeId, lPaymentCount)
                    ReDim Preserve m_vSelectedPaymentIDs(m_vSelectedPaymentIDs.GetUpperBound(0) + 2)
                    v_lCount += 2
                Next
                If m_vSelectedPaymentIDs.GetUpperBound(0) > 0 Then
                    ReDim Preserve m_vSelectedPaymentIDs(m_vSelectedPaymentIDs.GetUpperBound(0) - 2)
                End If
                If Information.IsArray(m_vSelectedPaymentIDs) Then
                    result = VB6.CopyArray(m_vSelectedPaymentIDs)
                End If
            End If
            Return result
        End Get
    End Property

    <DllImport("user32.dll")> _
   Private Shared Function GetScrollPos(ByVal hWnd As System.IntPtr, ByVal nBar As Integer) As Integer

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function SendMessage(ByVal hWnd As IntPtr, ByVal Msg As UInteger, ByVal wParam As Integer, ByVal lParam As Integer) As Boolean

    End Function
    <DllImport("user32.dll")> _
    Private Shared Function LockWindowUpdate(ByVal Handle As IntPtr) As Boolean

    End Function
    'Store the horizontal scroll value.
    Private Sub StoreHScrollValue()
        hScrollValue = GetScrollPos(lvwBankDetailsList.Handle, SBS_HORZ)
    End Sub
    'Recover the old scroll position
    Private Sub RecoverHorizontalScroll()
        LockWindowUpdate(lvwBankDetailsList.Handle)
        'Calculate the value the scroll needs to scroll back.
        Dim dx As Integer = hScrollValue - GetScrollPos(lvwBankDetailsList.Handle, SBS_HORZ)
        'Send the scroll message.
        Dim b As Boolean = SendMessage(lvwBankDetailsList.Handle, LVM_SCROLL, dx, 0)
        LockWindowUpdate(IntPtr.Zero)

    End Sub

    Private Sub cmdBankActiveInactive_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBankActiveInactive.Click
        If cmdBankActiveInactive.Text.ToLower() = "&inactive" Then
            m_lReturn = ProcessInActiveBank()
        ElseIf cmdBankActiveInactive.Text.ToLower() = "&make active" Then
            m_lReturn = ProcessActiveBank()
        End If
    End Sub

    Private Sub cmdBankAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBankAdd.Click
        m_lReturn = ProcessAddBank()
    End Sub

    Public Function ProcessUnDeleteBank() As Integer
        Const kMethodName As String = "ProcessDeleteBank"
        Try


            ' Validate to find that their isn't exists live policy

            Dim lSeletedArrayItem As Integer
            lSeletedArrayItem = SelectedArrayIndexOnTag
            m_lListSelectedItem = lvwBankDetailsList.FocusedItem.Index + 1 - 1

            If (m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSeletedArrayItem) = gPMConstants.PMEComponentAction.PMDelete And CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lSeletedArrayItem)) = 0) Or (m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSeletedArrayItem) <> gPMConstants.PMEComponentAction.PMDelete And CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lSeletedArrayItem)) = 0) Then

                m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSeletedArrayItem) = -1

                lvwBankDetailsList.Items(lvwBankDetailsList.FocusedItem.Index + 1).ImageKey = "saved"
            ElseIf m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSeletedArrayItem) <> gPMConstants.PMEComponentAction.PMDelete And CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lSeletedArrayItem)) = 1 Then

                ' Validate and undelete the row
                m_lReturn = CType(IsExistsPaymentType(CInt(m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, lSeletedArrayItem)(MainModule.ENPMLookups.Id))), gPMConstants.PMEReturnCode)
                If m_lReturn = gPMConstants.PMEReturnCode.PMFalse Then
                    m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lSeletedArrayItem) = gPMConstants.PMEComponentAction.PMReverse

                    lvwBankDetailsList.Items(lvwBankDetailsList.FocusedItem.Index + 1).ImageKey = "add"
                    cmdBankActiveInactive.Text = "&Inactive"
                End If

            End If


            RaiseEvent RefreshBankDetails(Me, New RefreshBankDetailsEventArgs(m_vPartyBankDetails))
            cmdBankActiveInactive.Text = "&Inactive"
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessDeleteBank(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
    End Function

    Private Function IsExistsPaymentType(ByVal lPaymentId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "IsExistsPaymentType"
        Try

            result = gPMConstants.PMEReturnCode.PMFalse

            ' Check to find that details against this payment is not already entered
            If Not IsArrayEmpty(SelectedPaymentTypes) Then
                For lCount As Integer = 0 To SelectedPaymentTypes.GetUpperBound(0)

                    If lPaymentId = CDbl(SelectedPaymentTypes(lCount)) Then
                        MessageBox.Show("Bank Details with this Payment Type already exists.", "Bank Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        result = gPMConstants.PMEReturnCode.PMTrue
                        Return result
                    End If
                Next
            End If



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    Private Sub cmdBankDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBankDelete.Click
        ProcessDeleteBank()
    End Sub

    Private Sub cmdBankEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBankEdit.Click
        m_lReturn = ProcessEditBank()
    End Sub

    Private Function SearchArrayIndexOnTag(ByVal lSelectedTag As Integer, ByRef lSelectedArrayIndex As Integer, ByVal lColumnId As MainModule.ENPartyBank) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SearchArrayIndexOnTag"


        Try



            result = gPMConstants.PMEReturnCode.PMTrue



            For lArrayCount As Integer = m_vPartyBankDetails.GetLowerBound(1) To m_vPartyBankDetails.GetUpperBound(1)
                If CDbl(m_vPartyBankDetails(lColumnId, lArrayCount)) = lSelectedTag Then
                    lSelectedArrayIndex = lArrayCount
                    Exit For
                End If
            Next



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: Initialise
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-03-2008 :
    ' ***************************************************************** '
    Public Function Initialise() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"


        Try



            Dim sValue As String = ""

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Check if already initialised
            If m_bIsInitialised Then
                Return result
            End If

            'Set m_colPaymentItems = New Collection

            ' Create an instance of the object manager.
            m_oObjectManager = New bObjectManager.ObjectManager()

            ' Call the initialise method.
            m_lReturn = m_oObjectManager.Initialise(sCallingAppName:=ACApp)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            ' If UserID is 0 assume that user cancelled logon
            If m_oObjectManager.UserID = 0 Then
                ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Return result
            End If

            ' Store the language ID from the object manager to the public variables,
            ' to enable us to use them throughout the object.
            With m_oObjectManager
                m_iLanguageID = .LanguageID
                m_iSourceID = .SourceID
                m_iUserId = .UserID
            End With

            ' Set the mouse pointer to busy.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)

            ' Get an instance of the business object via the public object manager.
            Dim temp_m_oBusiness As Object = Nothing
            m_lReturn = m_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRPartyBank.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetInstance of bClMCase.Business Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            'Start (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)
            ' Removed codes related to m_bIsCCAuthorisationOff
            m_lReturn = CType(iPMFunc.GetSystemOption(5069, sValue, 1), gPMConstants.PMEReturnCode) ' System option credit card processing method
            m_bIsExternalCreditCardProcessing = (sValue = "1") '0- Internal ; 1- External
            'End (Prakash C Varghese)-(PGR003 - V2 - Credit Card Encryption Sup v0 01.doc)

            ' hold Initialised status
            m_bIsInitialised = True


        Catch ex As Exception

            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: Load
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created: Gaurav Arora :
    ' ***************************************************************** '
    Public Function Load_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Load"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            ' Display all language specific captions.
            m_lReturn = SetInterfaceDefaults()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

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

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function

    Public Function SetInterfaceDefaults() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetInterfaceDefaults"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue

        Catch
        End Try



        m_lReturn = CType(DisplayCaptions(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = SetupListView()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Return result
    End Function

    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            cmdBankAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdBankEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            cmdBankDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabPartyBank, 0, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kTabPartyBank, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


            SSTabHelper.SetTabCaption(tabPartyBank, 1, iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kTabPartyBankHistory, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    '***************************************************************** '
    ' Name: SetupListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : Date :
    '***************************************************************** '
    Private Function SetupListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupListView"

        Dim lColWidth As Integer
        Dim sCaption As String = ""


        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(SetupBankDetailsListView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If

            m_lReturn = CType(SetupBankHistoryListView(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    ''' <summary>
    ''' SetupBankDetailsListView
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupBankDetailsListView() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SetupBankDetailsListView"
        Dim sCaption As String = ""
        Try

            If Not IsNothing(Me.ParentForm) Then
                fraBankDetails.Width = Me.ParentForm.Width - 25
                'lvwBankDetailsList.Width = fraBankDetails.Width - 25
            End If

            lvwBankDetailsList.Columns.Clear()

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwPaymentType,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexPaymentType, "", sCaption, CInt(VB6.TwipsToPixelsX(1500)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAccountType,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexAccountType, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAccHolderName,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexAccHolderName, "", sCaption, CInt(VB6.TwipsToPixelsX(3000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAccNum,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexAccNum, "", sCaption, CInt(VB6.TwipsToPixelsX(2500)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwSortCode,
                                   iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexBankBranchCode, "", sCaption, CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwBIC,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexBIC, "", sCaption, CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwIBAN,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexIBAN, "", sCaption, CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwBranch,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexBankBranch, "", sCaption, CInt(VB6.TwipsToPixelsX(1700)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBankName,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexBankName, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwExpiryDate,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCCExpiryDate, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwStartDate,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCCStartDate, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwIssueNumber,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCCIssueNumber, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwManualAuth,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCCManualAuth, "", sCaption, CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwAuthCode,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCCAuthCode, "", sCaption, CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwStreetName,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexNoStreet, "", sCaption, CInt(VB6.TwipsToPixelsX(2000)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwLocality,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexLocality, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwPostTown,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexPostTown, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwCounty,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCounty, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwPostCode,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexPostCode, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwCountry,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCountry, "", sCaption, CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)
            lvwBankDetailsList.Columns.Insert(kPtyBankColHIndexCCIsDefault, "", "Is Default", CInt(VB6.TwipsToPixelsX(1200)), HorizontalAlignment.Left, -1)
            lvwBankDetailsList.LabelEdit = False


        Catch ex As Exception

            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        Finally

        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' SetupBankHistoryListView
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetupBankHistoryListView() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SetupBankHistoryListView"
        Dim sCaption As String = ""
        Try
            If Not IsNothing(Me.ParentForm) Then
                fraBankDetails.Width = Me.ParentForm.Width - 25

                lvwBankDetailsHistory.Width = fraBankDetails.Width - 25
            End If
            lvwBankDetailsHistory.Columns.Clear()



            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwActionCode,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexActionCode, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwDate,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexDate, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)


            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwBankName,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexBankName, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwBranch,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexBranch, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwAccountName,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexAccountName, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwSortCode,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexSortCode, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwAccNum,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexAccNum, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwBIC,
                                   iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexBIC, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwIBAN,
                                   iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexIBAN, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwUser,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexUser, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwStreetName,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexStreetName, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

            sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstHisLvwPostCode,
                                               iDataType:=PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))
            lvwBankDetailsHistory.Columns.Insert(kPtyBankHisColHIndexPostCode, "", sCaption, CInt(VB6.TwipsToPixelsX(1400)), HorizontalAlignment.Left, -1)

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally

        End Try
        Return nResult
    End Function

    Public Function ProcessAddBank() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessAddBank"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            Dim objPartyBank As New frmPartyBankDetails

            ' Set the Properties
            objPartyBank.Task = gPMConstants.PMEComponentAction.PMAdd
            If Information.IsArray(m_vPartyBankDetails) Then
                objPartyBank.SelectedPaymentTypes = SelectedPaymentTypes
            End If

            If m_vPartyCnt > 0 Then

                m_lReturn = m_oBusiness.GetPartyName(vPartyCnt:=m_vPartyCnt, vAccountID:=0, vPartyName:=m_sPartyName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            ElseIf m_vAccountId > 0 Then

                m_lReturn = m_oBusiness.GetPartyName(vPartyCnt:=0, vAccountID:=m_vAccountId, vPartyName:=m_sPartyName)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetPartyName Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If
            objPartyBank.PartyName = m_sPartyName

            Dim sBankPaymentType As String = ""
            Dim sAccountType As String = ""

            CheckDefaultedTempCreditCard(sBankPaymentType, saccountType)
            objPartyBank.DefaultBankPaymentType = sBankPaymentType
            objPartyBank.DefaultAccountType = sAccountType

            objPartyBank.SetBusiness = m_oBusiness
            objPartyBank.ShowDialog()
            If objPartyBank.Status = gPMConstants.PMEReturnCode.PMOK Then
                m_vBankItem = objPartyBank.BankItem
                ResetPreviousOne = objPartyBank.ResetPreviousOne
                m_lReturn = CType(AddArrayItem(m_vBankItem), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "AddArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    m_lReturn = CType(AddBankItemToList(m_vBankItem), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "AddArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    '    PopulateBankDetails
                    RaiseEvent RefreshBankDetails(Me, New RefreshBankDetailsEventArgs(m_vPartyBankDetails))
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

    'Public Function SetupListviewDisplay(lListItem as Long, _
    '

    'End Function

    Public Function ProcessEditBank() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessEditBank"
        Try


            result = gPMConstants.PMEReturnCode.PMTrue

            Dim objPartyBank As New frmPartyBankDetails
            Dim lBankPaymentTypeId As Integer
            Dim bSamePaymentTypes, bBankAccountType, bCreditAccountType As Boolean
            Dim lCountAccType As Integer
            Dim bIsLinked As Boolean
            ' Set the Properties
            If Not Information.IsArray(m_vSelectedTags) Then
                Return result
            End If
            If lvwBankDetailsList.Items.Count > 0 Then
                m_lListSelectedItem = CInt(m_vSelectedTags(0))

                ' Block to edit different Account Type
                If Information.IsArray(m_vSelectedRows) And m_bMultiSelect And Information.IsArray(m_vPartyBankDetails) Then
                    For Each m_vSelectedRows_item As Object In m_vSelectedRows
                        If CStr(m_vSelectedRows_item) = "1" Then
                            bBankAccountType = True
                        Else
                            ' Credit Account Type
                            bCreditAccountType = True
                        End If
                    Next m_vSelectedRows_item
                    If bBankAccountType And bCreditAccountType Then
                        MessageBox.Show("Multiple Media Types selected. To Edit select a single Media Type", "Bank Details", MessageBoxButtons.OK, MessageBoxIcon.Information)
                        Return result
                    End If
                End If

                If Information.IsArray(m_vSelectedTags) Then
                    lBankPaymentTypeId = CInt(m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, CInt(m_vSelectedTags(0)))(MainModule.ENPMLookups.Id))
                    For Each m_vSelectedTags_item As Object In m_vSelectedTags
                        If lBankPaymentTypeId = CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, CInt(m_vSelectedTags_item))(MainModule.ENPMLookups.Id)) Then
                            bSamePaymentTypes = True
                        Else
                            bSamePaymentTypes = False
                            Exit For
                        End If
                    Next m_vSelectedTags_item
                End If
                If Not m_bMultiSelect Then
                    objPartyBank.AccountType = CStr(m_vPartyBankDetails(MainModule.ENPartyBank.BankAccountTypeId, CInt(m_vSelectedTags(0))))
                    If Information.IsArray(m_vPartyBankDetails) Then
                        objPartyBank.SelectedPaymentTypes = SelectedPaymentTypes
                    End If
                End If
                m_lReturn = CType(SetBankItem(m_lListSelectedItem), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "SetBankItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = CType(ISPartyLinkedwithInstalments(bIsLinked), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError("ISPartyLinkedwithInstalments", "ISPartyLinkedwithInstalments Failed")
                End If
                If bIsLinked Then
                    If MessageBox.Show("This account type is being used by other instalments plans." & Strings.Chr(13) & Strings.Chr(10) &
                                       "Do you wish to continue.", "Account Linked", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) = System.Windows.Forms.DialogResult.OK Then
                        'm_bEditAllInstalmentPlans = True
                    Else
                        'm_bEditAllInstalmentPlans = False
                        Exit Function
                    End If
                End If

                objPartyBank.Task = gPMConstants.PMEComponentAction.PMEdit
                objPartyBank.MultiSelect = m_bMultiSelect
                objPartyBank.SamePaymentTypes = bSamePaymentTypes

                objPartyBank.BankItem = m_vBankItem
                If Not m_bMultiSelect Then
                    If ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.PFLINKEXISTS, CInt(m_vSelectedTags(0)))) > 0 Or ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.CLILINKEXISTS, CInt(m_vSelectedTags(0)))) > 0 Or ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.CPLINKEXISTS, CInt(m_vSelectedTags(0)))) > 0 Then
                        objPartyBank.txtAccountType.Enabled = False
                    End If
                End If

                Dim sBankPaymentType As String = ""
                Dim sAccountType As String = ""

                CheckDefaultedTempCreditCard(sBankPaymentType, sAccountType)
                If sBankPaymentType <> CStr(m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, CInt(m_vSelectedTags(0)))(MainModule.ENPMLookups.Description)) Then
                    objPartyBank.DefaultBankPaymentType = sBankPaymentType
                    objPartyBank.DefaultAccountType = sAccountType
                End If

                objPartyBank.SetBusiness = m_oBusiness
                objPartyBank.ShowDialog()

                If objPartyBank.Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_vBankItem = objPartyBank.BankItem
                    ResetPreviousOne = objPartyBank.ResetPreviousOne
                    m_vOldBankItem = objPartyBank.OldBankItem
                    m_lReturn = CType(EditArrayItem(vEditedItem:=m_vBankItem, vOldItem:=m_vOldBankItem), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "EditArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = PopulateBankDetailsList()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "PopulateBankDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    RaiseEvent RefreshBankDetails(Me, New RefreshBankDetailsEventArgs(m_vPartyBankDetails))


                End If
                'lvwBankDetailsList.ListItems(lvwBankDetailsList.SelectedItem.Index).SmallIcon = "saved"
            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessAddBank(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

            '        Return result
            '        Resume
            '        Return result
        End Try
        Return result
    End Function




    Private Function ISPartyLinkedwithInstalments(ByRef bIsLinked As Boolean) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ISPartyLinkedwithInstalments"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue
            m_lListSelectedItem = CInt(m_vSelectedTags(0))

            m_lReturn = m_oBusiness.ISPartyBankLinkedWithInstalment(lPartyBankId:=CInt(m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, CInt(m_lListSelectedItem))), bisLinked:=bIsLinked)

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

    ''' <summary>
    ''' SetBankItem
    ''' </summary>
    ''' <param name="lSelectedIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function SetBankItem(ByRef lSelectedIndex As Integer) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SetBankItem"
        Try
            Dim nIsRegistered As Integer
            Dim nCount As Integer
            Dim bSameIsRegistered As Boolean

            ReDim m_vBankItem(ENPartyBank.uBoundPartyBank)
            If Not m_bMultiSelect Then
                m_vBankItem(ENPartyBank.BankPaymentTypeId) = m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, lSelectedIndex)(ENPMLookups.Id)
                m_vBankItem(ENPartyBank.BankAccountTypeId) = m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, lSelectedIndex)
                If CDbl(m_vPartyBankDetails(ENPartyBank.IsBank, lSelectedIndex)) = 1 Then
                    m_vBankItem(ENPartyBank.BankNameId) = m_vPartyBankDetails(ENPartyBank.BankNameId, lSelectedIndex)
                Else

                    m_vBankItem(ENPartyBank.BankNameId) = DBNull.Value
                End If
                m_vBankItem(ENPartyBank.AccountHolderName) = m_vPartyBankDetails(ENPartyBank.AccountHolderName, lSelectedIndex)
                m_vBankItem(ENPartyBank.AccountId) = m_vPartyBankDetails(ENPartyBank.AccountId, lSelectedIndex)
                m_vBankItem(ENPartyBank.AccountNumber) = m_vPartyBankDetails(ENPartyBank.AccountNumber, lSelectedIndex)
                m_vBankItem(ENPartyBank.BIC) = m_vPartyBankDetails(ENPartyBank.BIC, lSelectedIndex)
                m_vBankItem(ENPartyBank.IBAN) = m_vPartyBankDetails(ENPartyBank.IBAN, lSelectedIndex)
                m_vBankItem(ENPartyBank.IsBank) = m_vPartyBankDetails(ENPartyBank.IsBank, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankBranch) = m_vPartyBankDetails(ENPartyBank.BankBranch, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankBranchCode) = m_vPartyBankDetails(ENPartyBank.BankBranchCode, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankAdd1) = m_vPartyBankDetails(ENPartyBank.BankAdd1, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankAdd2) = m_vPartyBankDetails(ENPartyBank.BankAdd2, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankTown) = m_vPartyBankDetails(ENPartyBank.BankTown, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankPCode) = m_vPartyBankDetails(ENPartyBank.BankPCode, lSelectedIndex)

                m_vBankItem(ENPartyBank.BankRegion) = m_vPartyBankDetails(ENPartyBank.BankRegion, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankCountry) = m_vPartyBankDetails(ENPartyBank.BankCountry, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCNum) = m_vPartyBankDetails(ENPartyBank.CCNum, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCStartDate) = m_vPartyBankDetails(ENPartyBank.CCStartDate, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCExpiryDate) = m_vPartyBankDetails(ENPartyBank.CCExpiryDate, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCIssueNum) = m_vPartyBankDetails(ENPartyBank.CCIssueNum, lSelectedIndex)

                If CDbl(m_vPartyBankDetails(ENPartyBank.IsRegistered, lSelectedIndex)) = 0 Then
                    m_vBankItem(ENPartyBank.IsRegistered) = 0
                    m_vBankItem(ENPartyBank.CCAdd1) = m_vPartyBankDetails(ENPartyBank.CCAdd1, lSelectedIndex)
                    m_vBankItem(ENPartyBank.CCAdd2) = m_vPartyBankDetails(ENPartyBank.CCAdd2, lSelectedIndex)
                    m_vBankItem(ENPartyBank.CCTown) = m_vPartyBankDetails(ENPartyBank.CCTown, lSelectedIndex)
                    m_vBankItem(ENPartyBank.CCAdd3) = m_vPartyBankDetails(ENPartyBank.CCAdd3, lSelectedIndex)
                    m_vBankItem(ENPartyBank.CCPCode) = m_vPartyBankDetails(ENPartyBank.CCPCode, lSelectedIndex)
                Else
                    m_vBankItem(ENPartyBank.IsRegistered) = 1
                End If
                m_vBankItem(ENPartyBank.CCCountry) = m_vPartyBankDetails(ENPartyBank.CCCountry, lSelectedIndex)

                m_vBankItem(ENPartyBank.IsDeleted) = m_vPartyBankDetails(ENPartyBank.BankBranch, lSelectedIndex)

                m_vBankItem(ENPartyBank.CCPIN) = m_vPartyBankDetails(ENPartyBank.CCPIN, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCNameOnCard) = m_vPartyBankDetails(ENPartyBank.CCNameOnCard, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCManualAuthorisationNum) = m_vPartyBankDetails(ENPartyBank.CCManualAuthorisationNum, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCIsDefault) = m_vPartyBankDetails(ENPartyBank.CCIsDefault, lSelectedIndex)
            Else
                m_vBankItem(ENPartyBank.BankPaymentTypeId) = m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankAccountTypeId) = m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankNameId) = m_vPartyBankDetails(ENPartyBank.BankNameId, lSelectedIndex)
                m_vBankItem(ENPartyBank.AccountHolderName) = m_vPartyBankDetails(ENPartyBank.AccountHolderName, lSelectedIndex)
                m_vBankItem(ENPartyBank.IsBank) = m_vPartyBankDetails(ENPartyBank.IsBank, lSelectedIndex)
                m_vBankItem(ENPartyBank.BankCountry) = m_vPartyBankDetails(ENPartyBank.BankCountry, lSelectedIndex)
                m_vBankItem(ENPartyBank.CCCountry) = m_vPartyBankDetails(ENPartyBank.CCCountry, lSelectedIndex)

                If IsArray(m_vSelectedTags) Then
                    nIsRegistered = m_vPartyBankDetails(ENPartyBank.IsRegistered, m_vSelectedTags(0))
                    For nCount = LBound(m_vSelectedTags) To UBound(m_vSelectedTags)
                        If nIsRegistered = m_vPartyBankDetails(ENPartyBank.IsRegistered, m_vSelectedTags(nCount)) Then
                            bSameIsRegistered = True
                        Else
                            bSameIsRegistered = False
                            Exit For
                        End If
                    Next
                End If
                If bSameIsRegistered Then
                    If nIsRegistered = 1 Then
                        m_vBankItem(ENPartyBank.IsRegistered) = nIsRegistered
                    End If
                End If

            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return nResult
    End Function

    Private Function ProcessInActiveBank() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessInActiveBank"
        Try
            Dim v_lSelectedItemIndex As Integer

            If Information.IsArray(m_vPartyBankDetails) Then
                For v_lCount As Integer = 1 To lvwBankDetailsList.Items.Count

                    'developer guide no. 49 & 162
                    If lvwBankDetailsList.Items(v_lCount - 1).ImageKey <> "add" Then
                        If lvwBankDetailsList.Items.Item(v_lCount - 1).Selected Then
                            v_lSelectedItemIndex = v_lCount

                            m_lSelectedArrayIndexOnTag = Convert.ToString(lvwBankDetailsList.Items.Item(v_lCount - 1).Tag)
                            m_lReturn = CType(DeleteArrayItem(m_lSelectedArrayIndexOnTag), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "DeleteArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            'lvwBankDetailsList.ListItems(v_lCount).SmallIcon = "Inactive"
                        End If
                    End If
                Next
                cmdBankActiveInactive.Enabled = False
                cmdBankActiveInactive.Text = "&Inactive"
                RaiseEvent RefreshBankDetails(Me, New RefreshBankDetailsEventArgs(m_vPartyBankDetails))
                PopulateBankDetailsList()
                lvwBankDetailsList_ItemClick(lvwBankDetailsList.FocusedItem)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function ProcessActiveBank() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessActiveBank"
        Try
            Dim v_lSelectedItemIndex As Integer

            If Information.IsArray(m_vPartyBankDetails) Then
                For v_lCount As Integer = 1 To lvwBankDetailsList.Items.Count


                    'developer guide no. 49 & 162
                    If lvwBankDetailsList.Items(v_lCount - 1).ImageKey <> "add" Then
                        If lvwBankDetailsList.Items.Item(v_lCount - 1).Selected Then
                            v_lSelectedItemIndex = v_lCount

                            m_lSelectedArrayIndexOnTag = Convert.ToString(lvwBankDetailsList.Items.Item(v_lCount - 1).Tag)
                            m_lReturn = CType(UnDeleteArrayItem(m_lSelectedArrayIndexOnTag), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "DeleteArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                            'lvwBankDetailsList.ListItems(v_lCount).SmallIcon = "saved"
                        End If
                    End If
                Next
                cmdBankActiveInactive.Enabled = False
                cmdBankActiveInactive.Text = "&Inactive"
                RaiseEvent RefreshBankDetails(Me, New RefreshBankDetailsEventArgs(m_vPartyBankDetails))
                PopulateBankDetailsList()
                lvwBankDetailsList_ItemClick(lvwBankDetailsList.FocusedItem)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Public Function ProcessDeleteBank() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessDeleteBank"
        Try

            Dim lIsExits, v_lSelectedItemIndex As Integer
            If Information.IsArray(m_vPartyBankDetails) And Information.IsArray(m_vSelectedTags) Then
                For Each m_vSelectedTags_item As Object In m_vSelectedTags
                    ' Validate to find that their isn't exists live policy
                    'm_lReturn = IsActiveInstalmentPlanExits(lPartyCnt:=PartyCnt, _
                    'lPaymentTypeId:=m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, m_vSelectedTags(v_lCount))(ENPMLookups.Id), _
                    'lIsExists:=lIsExits)

                    m_lReturn = CType(IsPartyBankActiveTransactions(lPartyBankId:=CInt(m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, CInt(m_vSelectedTags_item))), lIsExists:=lIsExits), gPMConstants.PMEReturnCode)

                    If m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                        If lIsExits <= 0 Then
                            m_lReturn = CType(DeleteArrayItemFromDB(CInt(m_vSelectedTags_item)), gPMConstants.PMEReturnCode)
                            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                                RaiseError(kMethodName, "DeleteArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                            End If
                        End If
                    End If
                Next m_vSelectedTags_item
                PopulateBankDetailsList()
                RaiseEvent RefreshBankDetails(Me, New RefreshBankDetailsEventArgs(m_vPartyBankDetails))
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

    'UPGRADE_NOTE: (7001) The following declaration (IsActiveInstalmentPlanExits) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsActiveInstalmentPlanExits(ByVal lPartyCnt As Integer, ByVal lPaymentTypeId As Integer, ByRef lIsExists As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "IsActiveInstalmentPlanExits"
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '
    'If lPartyCnt > 0 Then

    'm_lReturn = m_oBusiness.IsActiveInstalmentPlanExits(lPartyCnt:=lPartyCnt, lPaymentTypeId:=lPaymentTypeId, lIsExists:=lIsExists)
    '
    'End If
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    '
    'Return result
    'End Function

    Public Function DeleteArrayItem(ByVal ArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteArrayItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' at the time of deletion we need to find out whether item is in database or not
            ' if it is in database then set rowstatus = PMDelete
            ' else delete the item from an array and swap the last row with this new blank row
            ' and set the status of last row to PMNotFound
            '    If m_vPartyBankDetails(ENPartyBank.RowStatus, ArrayIndex) <> PMAdd Then
            m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, ArrayIndex) = gPMConstants.PMEComponentAction.PMDelete
            m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, ArrayIndex) = 1
            '    ElseIf m_vPartyBankDetails(ENPartyBank.RowStatus, ArrayIndex) = PMAdd Then
            ''        m_lReturn = MoveArrayRow(SelectedArrayIndexOnTag, _
            ''                                    UBound(m_vPartyBankDetails, 2))
            '
            '        m_vPartyBankDetails(ENPartyBank.RowStatus, ArrayIndex) = PMNotFound
            '        m_lReturn = PopulateBankDetailsList()

            ' Delete th item from an array and swap the last row to this index
            '    End If
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

    Public Function DeleteArrayItemFromDB(ByVal ArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteArrayItemFromDB"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' at the time of deletion we need to find out whether item is in database or not
            ' if it is in database then set rowstatus = PMDelete
            ' else delete the item from an array and swap the last row with this new blank row
            ' and set the status of last row to PMNotFound
            If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, ArrayIndex) <> gPMConstants.PMEComponentAction.PMAdd Then
                m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, ArrayIndex) = gPMConstants.PMEComponentAction.PMDeleteFromDB
                '    ElseIf m_vPartyBankDetails(ENPartyBank.RowStatus, ArrayIndex) = PMAdd Then
                ''        m_lReturn = MoveArrayRow(SelectedArrayIndexOnTag, _
                ''                                    UBound(m_vPartyBankDetails, 2))
                '
                '        m_vPartyBankDetails(ENPartyBank.RowStatus, SelectedArrayIndexOnTag) = PMNotFound
                '        m_lReturn = PopulateBankDetailsList()

                ' Delete th item from an array and swap the last row to this index
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

    Public Function UnDeleteArrayItem(ByVal ArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UnDeleteArrayItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' at the time of deletion we need to find out whether item is in database or not
            ' if it is in database then set rowstatus = PMDelete
            ' else delete the item from an array and swap the last row with this new blank row
            ' and set the status of last row to PMNotFound
            '    If m_vPartyBankDetails(ENPartyBank.RowStatus, ArrayIndex) <> PMAdd Then
            m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, ArrayIndex) = gPMConstants.PMEComponentAction.PMReverse
            m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, ArrayIndex) = 0
            '    ElseIf m_vPartyBankDetails(ENPartyBank.RowStatus, ArrayIndex) = PMAdd Then
            ''        m_lReturn = MoveArrayRow(SelectedArrayIndexOnTag, _
            ''                                    UBound(m_vPartyBankDetails, 2))
            '
            '        m_vPartyBankDetails(ENPartyBank.RowStatus, SelectedArrayIndexOnTag) = PMNotFound
            '        m_lReturn = PopulateBankDetailsList()
            '
            '        ' Delete th item from an array and swap the last row to this index
            '    End If
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


    Public Function AddArrayItem(ByVal vAddedItem() As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddArrayItem"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            Dim uboundBankDetails As Integer

            ' Find whether last item is empty or not
            ' if empty then don't increase its bound and put the item their only
            ' it can be found by checking the rowstatus to PMNotFound

            If Not IsArrayEmpty(m_vPartyBankDetails) Then
                If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, uboundBankDetails) <> gPMConstants.PMEReturnCode.PMNotFound Then
                    uboundBankDetails = m_vPartyBankDetails.GetUpperBound(1) + 1
                    ReDim Preserve m_vPartyBankDetails(m_vPartyBankDetails.GetUpperBound(0), uboundBankDetails)
                End If
            Else
                ReDim m_vPartyBankDetails(MainModule.ENPartyBank.uBoundPartyBank, 0)
            End If

            If ResetPreviousOne Then
                If IsArray(m_vPartyBankDetails) Then
                    For iCount As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)
                        m_vPartyBankDetails(ENPartyBank.CCIsDefault, iCount) = 0
                    Next
                End If

                For iCount As Integer = 0 To lvwBankDetailsList.Items.Count - 1
                    If ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexCCIsDefault).Text = "1" Then
                        ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexCCIsDefault).Text = ""
                    End If
                Next
            End If

            uboundBankDetails = m_vPartyBankDetails.GetUpperBound(1)

            m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, uboundBankDetails) = gPMConstants.PMEComponentAction.PMAdd
            m_vPartyBankDetails(MainModule.ENPartyBank.RowIndex, uboundBankDetails) = uboundBankDetails '+ 1 ' Need to think
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

    ''' <summary>
    ''' Set BankDetails
    ''' </summary>
    ''' <param name="vBankItem"></param>
    ''' <param name="lIndex"></param>
    ''' <param name="vOldBankItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function SetBankDetailsItem(ByVal vBankItem() As Object, ByVal lIndex As Integer, Optional ByVal vOldBankItem As Object = Nothing) As Integer
        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "SetBankDetailsItem"
        Dim bEditBank As Boolean

        Try
            m_vPartyBankDetails(ENPartyBank.IsBank, lIndex) = vBankItem(ENPartyBank.IsBank)
            m_vPartyBankDetails(ENPartyBank.AccountId, lIndex) = AccountId
            If Not m_bMultiSelect Then
                m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, lIndex) = vBankItem(ENPartyBank.BankAccountTypeId) '(ENPMLookups.Id)
                m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, lIndex) = vBankItem(ENPartyBank.BankPaymentTypeId)
            End If
            m_vPartyBankDetails(ENPartyBank.IsBank, lIndex) = vBankItem(ENPartyBank.IsBank)
            m_vPartyBankDetails(ENPartyBank.AccountHolderName, lIndex) = vBankItem(ENPartyBank.AccountHolderName)
            m_vPartyBankDetails(ENPartyBank.AccountNumber, lIndex) = vBankItem(ENPartyBank.AccountNumber)
            If CDbl(vBankItem(ENPartyBank.IsBank)) = 1 Then
                m_vPartyBankDetails(ENPartyBank.BankNameId, lIndex) = vBankItem(ENPartyBank.BankNameId)
            Else
                m_vPartyBankDetails(ENPartyBank.BankNameId, lIndex) = DBNull.Value
            End If
            m_vPartyBankDetails(ENPartyBank.BankBranch, lIndex) = vBankItem(ENPartyBank.BankBranch)
            m_vPartyBankDetails(ENPartyBank.BankBranchCode, lIndex) = vBankItem(ENPartyBank.BankBranchCode)
            m_vPartyBankDetails(ENPartyBank.BankAdd1, lIndex) = vBankItem(ENPartyBank.BankAdd1)
            m_vPartyBankDetails(ENPartyBank.BankAdd2, lIndex) = vBankItem(ENPartyBank.BankAdd2)
            m_vPartyBankDetails(ENPartyBank.BankAdd3, lIndex) = vBankItem(ENPartyBank.BankAdd3)
            m_vPartyBankDetails(ENPartyBank.BankTown, lIndex) = vBankItem(ENPartyBank.BankTown)
            m_vPartyBankDetails(ENPartyBank.BankPCode, lIndex) = vBankItem(ENPartyBank.BankPCode)
            m_vPartyBankDetails(ENPartyBank.BankRegion, lIndex) = vBankItem(ENPartyBank.BankRegion)
            m_vPartyBankDetails(ENPartyBank.BankCountry, lIndex) = vBankItem(ENPartyBank.BankCountry)
            m_vPartyBankDetails(ENPartyBank.CCNum, lIndex) = vBankItem(ENPartyBank.CCNum)
            m_vPartyBankDetails(ENPartyBank.CCStartDate, lIndex) = vBankItem(ENPartyBank.CCStartDate)
            m_vPartyBankDetails(ENPartyBank.CCExpiryDate, lIndex) = vBankItem(ENPartyBank.CCExpiryDate)
            m_vPartyBankDetails(ENPartyBank.CCIssueNum, lIndex) = vBankItem(ENPartyBank.CCIssueNum)
            m_vPartyBankDetails(ENPartyBank.CCPIN, lIndex) = vBankItem(ENPartyBank.CCPIN)
            m_vPartyBankDetails(ENPartyBank.IsRegistered, lIndex) = vBankItem(ENPartyBank.IsRegistered)
            m_vPartyBankDetails(ENPartyBank.CCAdd1, lIndex) = vBankItem(ENPartyBank.CCAdd1)
            m_vPartyBankDetails(ENPartyBank.CCAdd2, lIndex) = vBankItem(ENPartyBank.CCAdd2)
            m_vPartyBankDetails(ENPartyBank.CCAdd3, lIndex) = vBankItem(ENPartyBank.CCAdd3)
            m_vPartyBankDetails(ENPartyBank.CCTown, lIndex) = vBankItem(ENPartyBank.CCTown)
            m_vPartyBankDetails(ENPartyBank.CCPCode, lIndex) = vBankItem(ENPartyBank.CCPCode)
            m_vPartyBankDetails(ENPartyBank.CCCountry, lIndex) = vBankItem(ENPartyBank.CCCountry)
            m_vPartyBankDetails(ENPartyBank.IsDeleted, lIndex) = vBankItem(ENPartyBank.IsDeleted)
            m_vPartyBankDetails(ENPartyBank.CCNameOnCard, lIndex) = vBankItem(ENPartyBank.CCNameOnCard)
            m_vPartyBankDetails(ENPartyBank.CCManualAuthorisationNum, lIndex) = vBankItem(ENPartyBank.CCManualAuthorisationNum)
            m_vPartyBankDetails(ENPartyBank.BIC, lIndex) = vBankItem(ENPartyBank.BIC)
            m_vPartyBankDetails(ENPartyBank.IBAN, lIndex) = vBankItem(ENPartyBank.IBAN)
            m_vPartyBankDetails(ENPartyBank.CCIsDefault, lIndex) = vBankItem(ENPartyBank.CCIsDefault)
            Return nResult
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

    Public Function EditArrayItem(ByVal vEditedItem As Object, Optional ByVal vOldItem As Object = Nothing) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "EditArrayItem"
        Dim bEditBank As Boolean
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If ResetPreviousOne Then
                If IsArray(m_vPartyBankDetails) Then
                    For iCount As Integer = 0 To m_vPartyBankDetails.GetUpperBound(1)
                        m_vPartyBankDetails(ENPartyBank.CCIsDefault, iCount) = 0
                    Next
                End If

                For iCount As Integer = 0 To lvwBankDetailsList.Items.Count - 1
                    If ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexCCIsDefault).Text = "1" Then
                        ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexCCIsDefault).Text = ""
                    End If
                Next
            End If

            Dim lSelectedArrayIndex As Integer
            If m_bMultiSelect Then
                For Each lSelectedArrayIndex In m_vSelectedTags
                    m_lReturn = CType(SetBankDetailsItem(vBankItem:=vEditedItem, lIndex:=CInt(lSelectedArrayIndex), vOldBankItem:=vOldItem), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        RaiseError(kMethodName, "SetBankDetailsItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(lSelectedArrayIndex)) <> gPMConstants.PMEComponentAction.PMAdd And m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(lSelectedArrayIndex)) <> gPMConstants.PMEComponentAction.PMDelete Then

                        m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(lSelectedArrayIndex)) = gPMConstants.PMEComponentAction.PMEdit
                    End If
                Next lSelectedArrayIndex
            Else

                lSelectedArrayIndex = m_lSelectedArrayIndexOnTag


                m_lReturn = CType(SetBankDetailsItem(vBankItem:=vEditedItem, lIndex:=CInt(lSelectedArrayIndex), vOldBankItem:=vOldItem), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "SetBankDetailsItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If


                If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(lSelectedArrayIndex)) <> gPMConstants.PMEComponentAction.PMAdd Then

                    m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(lSelectedArrayIndex)) = gPMConstants.PMEComponentAction.PMEdit
                End If
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


    Private Function PopulateScreen() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "PopulateScreen"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = CType(PopulateBankDetailsList(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function BuildArrayIndex() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "BuildArrayIndex"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Information.IsArray(m_vPartyBankDetails) Then
                For lBounds As Integer = m_vPartyBankDetails.GetLowerBound(1) To m_vPartyBankDetails.GetUpperBound(1)
                    m_vPartyBankDetails(MainModule.ENPartyBank.RowIndex, lBounds) = lBounds
                Next lBounds
            End If




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    ''' <summary>
    ''' AddBankItemToList
    ''' </summary>
    ''' <param name="vBankItem"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddBankItemToList(ByVal vBankItem() As Object) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "AddBankItemToList"

        Try
            Dim oListItem As ListViewItem
            oListItem = lvwBankDetailsList.Items.Add(CStr(vBankItem(ENPartyBank.BankPaymentTypeId)(ENPMLookups.Description)).Trim(), "add")
            If Not (Convert.IsDBNull(vBankItem(ENPartyBank.BankAccountTypeId)) Or IsNothing(vBankItem(ENPartyBank.BankAccountTypeId))) Then
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccountType).Text = CStr(vBankItem(ENPartyBank.BankAccountTypeId)).Trim()
            Else
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccountType).Text = ""
            End If

            If CDbl(vBankItem(ENPartyBank.IsBank)) = 1 Then
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccHolderName).Text = CStr(vBankItem(ENPartyBank.AccountHolderName)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = CStr(vBankItem(ENPartyBank.AccountNumber)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranchCode).Text = CStr(vBankItem(ENPartyBank.BankBranchCode)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBIC).Text = CStr(vBankItem(ENPartyBank.BIC)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexIBAN).Text = CStr(vBankItem(ENPartyBank.IBAN)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranch).Text = CStr(vBankItem(ENPartyBank.BankBranch)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankName).Text = CStr(vBankItem(ENPartyBank.BankNameId)(ENPMLookups.Description)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCExpiryDate).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCStartDate).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCIssueNumber).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCManualAuth).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCAuthCode).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexNoStreet).Text = CStr(vBankItem(ENPartyBank.BankAdd1)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexLocality).Text = CStr(vBankItem(ENPartyBank.BankAdd2)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostTown).Text = CStr(vBankItem(ENPartyBank.BankTown)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCounty).Text = CStr(vBankItem(ENPartyBank.BankRegion)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostCode).Text = CStr(vBankItem(ENPartyBank.BankPCode)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCountry).Text = CStr(vBankItem(ENPartyBank.BankCountry)(ENPMLookups.Description)).Trim()
            Else
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccHolderName).Text = CStr(vBankItem(ENPartyBank.AccountHolderName)).Trim()
                If Strings.Len(CStr(vBankItem(ENPartyBank.CCNum))) >= 4 Then
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = "**** **** **** " & CStr(vBankItem(ENPartyBank.CCNum)).Trim().Substring(CStr(vBankItem(ENPartyBank.CCNum)).Trim().Length - 4)
                Else

                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = CStr(vBankItem(ENPartyBank.CCNum)).Trim()
                End If

                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranchCode).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBIC).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexIBAN).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranch).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankName).Text = ""
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCExpiryDate).Text = CStr(vBankItem(ENPartyBank.CCExpiryDate)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCStartDate).Text = CStr(vBankItem(ENPartyBank.CCStartDate)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCIssueNumber).Text = CStr(vBankItem(ENPartyBank.CCIssueNum)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCManualAuth).Text = CStr(vBankItem(ENPartyBank.CCManualAuthorisationNum)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCAuthCode).Text = CStr(vBankItem(ENPartyBank.CCPIN)).Trim()
                If Not (vBankItem(ENPartyBank.CCAdd1) Is Nothing) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexNoStreet).Text = CStr(vBankItem(ENPartyBank.CCAdd1)).Trim()
                End If

                If Not (vBankItem(ENPartyBank.CCAdd2) Is Nothing) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexLocality).Text = CStr(vBankItem(ENPartyBank.CCAdd2)).Trim()
                End If

                If Not (vBankItem(ENPartyBank.CCTown) Is Nothing) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostTown).Text = CStr(vBankItem(ENPartyBank.CCTown)).Trim()
                End If
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCounty).Text = ""

                If Not (vBankItem(ENPartyBank.CCPCode) Is Nothing) Then
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostCode).Text = CStr(vBankItem(ENPartyBank.CCPCode)).Trim()
                End If

                If Not (vBankItem(ENPartyBank.CCCountry)) Is Nothing Then
                    If Not (vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Description) Is Nothing) Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCountry).Text = CStr(vBankItem(ENPartyBank.CCCountry)(ENPMLookups.Description)).Trim()
                    End If
                End If
            End If
            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCIsDefault).Text = CStr(vBankItem(ENPartyBank.CCIsDefault)).Trim()

            oListItem.Tag = CStr(m_vPartyBankDetails.GetUpperBound(1)) '+ 1

        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally

        End Try
        Return nResult
    End Function

    ''' <summary>
    ''' PopulateBankDetailsList
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PopulateBankDetailsList() As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "PopulateBankDetailsList"
        Try

            Dim i As Integer
            Dim oListItem As ListViewItem

            If IsArrayEmpty(m_vPartyBankDetails) Then
                Return nResult
            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwBankDetailsList.Items.Clear()

            For i = m_vPartyBankDetails.GetLowerBound(1) To m_vPartyBankDetails.GetUpperBound(1)
                If m_vPartyBankDetails(ENPartyBank.RowStatus, i) <> PMEReturnCode.PMNotFound Then

                    oListItem = lvwBankDetailsList.Items.Add(CStr(m_vPartyBankDetails(ENPartyBank.BankPaymentTypeId, i)(ENPMLookups.Description)).Trim())
                    If CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = -1 And CDbl(m_vPartyBankDetails(ENPartyBank.IsDeleted, i)) = 0 Then
                        oListItem.ImageKey = "saved"
                    ElseIf CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = -1 And CDbl(m_vPartyBankDetails(ENPartyBank.IsDeleted, i)) = 1 Then
                        oListItem.ImageKey = "Inactive"
                    ElseIf CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = 1 Then
                        oListItem.ImageKey = "add"
                    ElseIf CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = 2 Then
                        oListItem.ImageKey = "edited"
                    ElseIf CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = 11 Then
                        oListItem.ImageKey = "saved"
                    ElseIf CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = 3 Then
                        oListItem.ImageKey = "Inactive"
                    ElseIf CDbl(m_vPartyBankDetails(ENPartyBank.RowStatus, i)) = 21 Then
                        oListItem.ImageKey = "delete"
                    End If

                    If Not (Convert.IsDBNull(m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, i)) Or IsNothing(m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, i))) Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccountType).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankAccountTypeId, i)).Trim()
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccountType).Text = ""
                    End If

                    If CDbl(m_vPartyBankDetails(ENPartyBank.IsBank, i)) = 1 Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccHolderName).Text = CStr(m_vPartyBankDetails(ENPartyBank.AccountHolderName, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = CStr(m_vPartyBankDetails(ENPartyBank.AccountNumber, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranchCode).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankBranchCode, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBIC).Text = CStr(m_vPartyBankDetails(ENPartyBank.BIC, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexIBAN).Text = CStr(m_vPartyBankDetails(ENPartyBank.IBAN, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranch).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankBranch, i)).Trim()
                        If Information.IsArray(m_vPartyBankDetails(ENPartyBank.BankNameId, i)) Then
                            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankName).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankNameId, i)(ENPMLookups.Description)).Trim()
                        End If
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCExpiryDate).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCStartDate).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCIssueNumber).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCManualAuth).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCAuthCode).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexNoStreet).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankAdd1, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexLocality).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankAdd2, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostTown).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankTown, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCounty).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankRegion, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostCode).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankPCode, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCountry).Text = CStr(m_vPartyBankDetails(ENPartyBank.BankCountry, i)(ENPMLookups.Description)).Trim()
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccHolderName).Text = CStr(m_vPartyBankDetails(ENPartyBank.AccountHolderName, i)).Trim()
                        If Strings.Len(CStr(m_vPartyBankDetails(ENPartyBank.CCNum, i))) >= 4 Then
                            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = "**** **** **** " & CStr(m_vPartyBankDetails(ENPartyBank.CCNum, i)).Trim().Substring(CStr(m_vPartyBankDetails(ENPartyBank.CCNum, i)).Trim().Length - 4)
                        Else
                            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = CStr(m_vPartyBankDetails(ENPartyBank.CCNum, i)).Trim()
                        End If

                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranchCode).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBIC).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexIBAN).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankBranch).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBankName).Text = ""
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCExpiryDate).Text = CStr(m_vPartyBankDetails(ENPartyBank.CCExpiryDate, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCStartDate).Text = CStr(m_vPartyBankDetails(ENPartyBank.CCStartDate, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCIssueNumber).Text = CStr(m_vPartyBankDetails(ENPartyBank.CCIssueNum, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCManualAuth).Text = CStr(m_vPartyBankDetails(ENPartyBank.CCManualAuthorisationNum, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCAuthCode).Text = CStr(m_vPartyBankDetails(ENPartyBank.CCPIN, i)).Trim()

                        'Developer Guide No 149
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexNoStreet).Text = Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCAdd1, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexLocality).Text = Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCAdd2, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostTown).Text = Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCTown, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCounty).Text = Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCAdd3, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexPostCode).Text = Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCPCode, i)).Trim()
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCountry).Text = Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCCountry, i)(ENPMLookups.Description)).Trim()
                    End If
                    If Convert.ToString(m_vPartyBankDetails(ENPartyBank.CCIsDefault, i)).Trim() = "1" Then
                        ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexCCIsDefault).Text = "1"
                    End If
                    oListItem.Tag = CStr(i)
                End If

            Next i
            If i > 0 Then
                lvwBankDetailsList.Items.Item(0).Selected = False
            End If

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        End Try
        Return nResult
    End Function

    Public Function BusinessToInterface() As Integer
        PopulateBankDetailsList()
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (cmdOK_Click) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub cmdOK_Click()

    'Unload(Me)
    'End Sub

    Private Sub cmdBankSelectAll_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBankSelectAll.Click
        Dim vsSelectedPaymentType As String = ""
        ReDim m_vSelectedTags(0)
        With lvwBankDetailsList
            For lCount As Integer = 0 To .Items.Count - 1
                If CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, Convert.ToString(.Items.Item(lCount).Tag))) <> 3 And CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, Convert.ToString(.Items.Item(lCount).Tag))) <> 1 And CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, Convert.ToString(.Items.Item(lCount).Tag))) <> 21 Then
                    .Items.Item(lCount).Selected = True

                    m_vSelectedTags(m_vSelectedTags.GetUpperBound(0)) = Convert.ToString(.Items.Item(lCount).Tag)
                    ReDim Preserve m_vSelectedTags(m_vSelectedTags.GetUpperBound(0) + 1)
                Else
                    .Items.Item(lCount).Selected = False
                End If
            Next
        End With

        ' Block to edit different Account Type
        ReDim m_vSelectedRows(0)
        For lCount As Integer = 1 To lvwBankDetailsList.Items.Count
            If lvwBankDetailsList.Items.Item(lCount - 1).Selected Then
                m_vSelectedRows(m_vSelectedRows.GetUpperBound(0)) = m_vPartyBankDetails(MainModule.ENPartyBank.IsBank, lCount - 1)
                ReDim Preserve m_vSelectedRows(m_vSelectedRows.GetUpperBound(0) + 1)
            End If
        Next

        If m_vSelectedRows.GetUpperBound(0) > 0 Then
            ReDim Preserve m_vSelectedRows(m_vSelectedRows.GetUpperBound(0) - 1)
        End If

        If m_vSelectedTags.GetUpperBound(0) > 0 Then
            ReDim Preserve m_vSelectedTags(m_vSelectedTags.GetUpperBound(0) - 1)
        End If
        m_bMultiSelect = m_vSelectedTags.GetUpperBound(0) >= 1
    End Sub

    'PN No.-62009
    'Date.-19-Jan-2009
    Private Sub cmdNext_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs)
        Dim Index As Integer = Array.IndexOf(cmdNext, eventSender)
        Try
            ' Change to the next tab.
            If SSTabHelper.GetSelectedIndex(tabPartyBank) < SSTabHelper.GetTabCount(tabPartyBank) - 1 Then
                SSTabHelper.SetSelectedIndex(tabPartyBank, Index + 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabPartyBank) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch



            ' Error Section

            Exit Sub
        End Try

    End Sub


    'PN No.-62009
    'Date.-19-Jan-2009
    Private Sub cmdPrevious_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles _cmdPrevious_0.Click
        Dim Index As Integer = Array.IndexOf(cmdPrevious, eventSender)
        Try
            'change to previous tab
            If SSTabHelper.GetSelectedIndex(tabPartyBank) > 0 Then
                SSTabHelper.SetSelectedIndex(tabPartyBank, SSTabHelper.GetSelectedIndex(tabPartyBank) - 1)
            End If

            ' Set focus to the first control on the tab.
            If SSTabHelper.GetSelectedIndex(tabPartyBank) <= m_ctlTabFirstLast.GetUpperBound(1) Then
                m_ctlTabFirstLast(ACControlStart, Index + 1).Focus()
            End If

        Catch

            Exit Sub
        End Try
    End Sub

    Private Sub lvwBankDetailsList_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwBankDetailsList.Click
        'developer guide no. 224
        If lvwBankDetailsList.Items.Count > 0 Then
            lvwBankDetailsList_ItemClick(lvwBankDetailsList.SelectedItems(0))
        End If
    End Sub

    Private Sub lvwBankDetailsList_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwBankDetailsList.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwBankDetailsList.Columns(eventArgs.Column)
        StoreHScrollValue()
        ' Sort the data
        m_lReturn = CType(SortListView(v_iIndex:=ColumnHeader.Index + 1 - 1), gPMConstants.PMEReturnCode)
        RecoverHorizontalScroll()
    End Sub

    Private Function CheckIfDeleted(ByVal lArrayIndex As Integer, ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIfDeleted"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            If Not IsArrayEmpty(m_vPartyBankDetails) Then
                If (CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, lArrayIndex)) = 1 Or m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lArrayIndex) = gPMConstants.PMEComponentAction.PMDelete) And m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, lArrayIndex) <> gPMConstants.PMEComponentAction.PMReverse Then
                    lStatus = 1
                End If
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    Private Function GetBankHistoryByPaymentID(ByVal lPartyBankId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetHistoryByPaymentId"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            lvwBankDetailsHistory.Items.Clear()

            If Not IsArrayEmpty(m_vPartyBankHistory) Then

                For lArrayCount As Integer = m_vPartyBankHistory.GetLowerBound(1) To m_vPartyBankHistory.GetUpperBound(1)

                    If CDbl(m_vPartyBankHistory(MainModule.ENPartyBankHistory.PartyBankId, lArrayCount)) = lPartyBankId Then
                        m_lReturn = CType(AddBankItemHistoryToList(m_vPartyBankHistory, lArrayCount), gPMConstants.PMEReturnCode)
                    End If
                Next
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    ''' <summary>
    ''' AddBankItemHistoryToList
    ''' </summary>
    ''' <param name="m_vPartyBankHistory"></param>
    ''' <param name="lIndex"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function AddBankItemHistoryToList(ByVal m_vPartyBankHistory As Object, ByVal lIndex As Integer) As Integer

        Dim nResult As Integer = PMEReturnCode.PMTrue
        Const kMethodName As String = "AddBankItemHistoryToList"
        Try
            Dim oListItem As ListViewItem

            oListItem = lvwBankDetailsHistory.Items.Add(CStr(m_vPartyBankHistory(ENPartyBankHistory.ActionCode, lIndex)).Trim(), "history")
            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexDate).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.DateModified, lIndex)).Trim()

            If Not IsArrayEmpty(m_vPartyBankHistory(ENPartyBankHistory.BankNameId, lIndex)) Then
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexBankName).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BankNameId, lIndex)(ENPMLookups.Description)).Trim()
            End If

            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexBranch).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BankBranch, lIndex)).Trim()
            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexAccountName).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.AccountHolderName, lIndex)).Trim()
            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexSortCode).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BankBranchCode, lIndex)).Trim()

            If CStr(m_vPartyBankHistory(ENPartyBankHistory.CCNum, lIndex)).Trim() <> "" Then
                If CStr(m_vPartyBankHistory(ENPartyBankHistory.CCNum, lIndex)).Trim().Length >= 4 Then
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = "**** **** **** " & CStr(m_vPartyBankHistory(ENPartyBankHistory.CCNum, lIndex)).Trim().Substring(CStr(m_vPartyBankHistory(ENPartyBankHistory.CCNum, lIndex)).Trim().Length - 4)
                Else
                    ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexAccNum).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.CCNum, lIndex)).Trim()
                End If
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexBIC).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BIC, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankColHIndexIBAN).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.IBAN, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexStreetName).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.CCAdd1, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexPostCode).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.CCPCode, lIndex)).Trim()
            Else
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexBIC).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BIC, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexIBAN).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.IBAN, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexAccNum).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.AccountNumber, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexStreetName).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BankAdd1, lIndex)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexPostCode).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.BankPCode, lIndex)).Trim()
            End If
            ListViewHelper.GetListViewSubItem(oListItem, kPtyBankHisColHIndexUser).Text = CStr(m_vPartyBankHistory(ENPartyBankHistory.User, lIndex)).Trim()
        Catch ex As Exception
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)
        Finally
        End Try
        Return nResult
    End Function

    Private Sub CheckDefaultedTempCreditCard(ByRef r_sBankPaymentName As String, ByRef r_sAccountType As String)

        For iCount As Integer = 0 To lvwBankDetailsList.Items.Count - 1
            If ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexCCIsDefault).Text = "1" Then
                r_sBankPaymentName = ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexPaymentType).Text
                r_sAccountType = ListViewHelper.GetListViewSubItem(lvwBankDetailsList.Items(iCount), kPtyBankColHIndexAccountType).Text
                Exit Sub
            End If
        Next
    End Sub

    Private Sub lvwBankDetailsList_ItemClick(ByVal Item As ListViewItem)

        Dim v_lStatus, vlSelectedCount As Integer
        Dim vsSelectedPaymentType As String = ""
        m_bMultiSelect = False
        ReDim m_vSelectedTags(0)
        ReDim m_vSelectedRows(0)
        For lCount As Integer = 1 To lvwBankDetailsList.Items.Count
            If lvwBankDetailsList.Items.Item(lCount - 1).Selected Then

                m_lSelectedArrayIndexOnTag = Convert.ToString(lvwBankDetailsList.Items.Item(lCount - 1).Tag)
                If vsSelectedPaymentType = "" Then
                    vsSelectedPaymentType = CStr(m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, m_lSelectedArrayIndexOnTag)(MainModule.ENPMLookups.Id))
                End If
                If vsSelectedPaymentType = CStr(m_vPartyBankDetails(MainModule.ENPartyBank.BankPaymentTypeId, m_lSelectedArrayIndexOnTag)(MainModule.ENPMLookups.Id)) Then
                    vlSelectedCount += 1
                    m_vSelectedTags(m_vSelectedTags.GetUpperBound(0)) = m_lSelectedArrayIndexOnTag
                    ReDim Preserve m_vSelectedTags(m_vSelectedTags.GetUpperBound(0) + 1)
                Else
                    lvwBankDetailsList.Items.Item(lCount - 1).Selected = False
                End If
                m_vSelectedRows(m_vSelectedRows.GetUpperBound(0)) = m_vPartyBankDetails(MainModule.ENPartyBank.IsBank, m_lSelectedArrayIndexOnTag)
                ReDim Preserve m_vSelectedRows(m_vSelectedRows.GetUpperBound(0) + 1)
            End If
        Next
        If m_vSelectedTags.GetUpperBound(0) > 0 Then
            ReDim Preserve m_vSelectedTags(m_vSelectedTags.GetUpperBound(0) - 1)
        End If
        If m_vSelectedRows.GetUpperBound(0) > 0 Then
            ReDim Preserve m_vSelectedRows(m_vSelectedRows.GetUpperBound(0) - 1)
        End If
        m_bMultiSelect = vlSelectedCount > 1
        If vlSelectedCount > 1 Then
            cmdBankActiveInactive.Enabled = Not m_bReadOnly
            m_lReturn = CType(CheckIfDeleted(CInt(m_vSelectedTags(0)), v_lStatus), gPMConstants.PMEReturnCode)
            If Not m_bReadOnly Then
                If v_lStatus = 1 Then
                    cmdBankActiveInactive.Text = "&Make Active"
                Else
                    cmdBankActiveInactive.Text = "&Inactive"
                End If
            End If

            If Not m_bReadOnly Then
                cmdBankDelete.Enabled = ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.PFLINKEXISTS, CInt(m_vSelectedTags(0)))) = 0 And ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.CLILINKEXISTS, CInt(m_vSelectedTags(0)))) = 0 And ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.CPLINKEXISTS, CInt(m_vSelectedTags(0)))) = 0
            End If

            m_lListSelectedItem = CInt(m_vSelectedTags(0))
            m_lReturn = CType(GetBankHistoryByPaymentID(lPartyBankId:=CInt(m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, m_lListSelectedItem))), gPMConstants.PMEReturnCode) ' Need to see
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If
        Else
            If Not m_bReadOnly Then
                cmdBankDelete.Enabled = ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.PFLINKEXISTS, CInt(m_vSelectedTags(0)))) = 0 And ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.CLILINKEXISTS, CInt(m_vSelectedTags(0)))) = 0 And ToSafeLong(m_vPartyBankDetails(MainModule.ENPartyBank.CPLINKEXISTS, CInt(m_vSelectedTags(0)))) = 0
            End If
            If Not m_bReadOnly Then
                If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(m_vSelectedTags(0))) = gPMConstants.PMEComponentAction.PMDelete Or CDbl(m_vPartyBankDetails(MainModule.ENPartyBank.IsDeleted, CInt(m_vSelectedTags(0)))) = 1 Then
                    cmdBankEdit.Enabled = False
                    cmdBankActiveInactive.Enabled = True
                    cmdBankActiveInactive.Text = "&Make Active"
                Else
                    cmdBankActiveInactive.Enabled = True
                    cmdBankActiveInactive.Text = "&Inactive"
                    cmdBankEdit.Enabled = True
                End If
            End If
            If Not m_bReadOnly Then
                If m_vPartyBankDetails(MainModule.ENPartyBank.RowStatus, CInt(m_vSelectedTags(0))) = gPMConstants.PMEComponentAction.PMAdd Then
                    cmdBankDelete.Enabled = False

                    cmdBankActiveInactive.Enabled = False
                    cmdBankActiveInactive.Text = "&Inactive"
                End If
            End If
            m_lListSelectedItem = CInt(m_vSelectedTags(0))
            m_lReturn = CType(GetBankHistoryByPaymentID(lPartyBankId:=CInt(m_vPartyBankDetails(MainModule.ENPartyBank.PartyBankId, m_lListSelectedItem))), gPMConstants.PMEReturnCode) ' Need to see
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            End If
        End If

    End Sub


    Private Sub lvwBankDetailsList_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwBankDetailsList.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        '    Dim lDeleteStatus As Long
        '
        '    m_lReturn = CheckIfDeleted(SelectedArrayIndexOnTag, lDeleteStatus)
        '
        '    If m_lReturn <> PMTrue Then
        '
        '    ElseIf m_lReturn = PMTrue Then
        '        If lDeleteStatus = 1 Then
        '            cmdBankActiveInactive.Caption = "Ac&tive"
        '        Else
        '            cmdBankActiveInactive.Caption = "&Inactive"
        '        End If
        '    End If
    End Sub

    Private Sub lvwBankDetailsList_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwBankDetailsList.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        Dim x As Single = VB6.PixelsToTwipsX(eventArgs.X)
        Dim y As Single = VB6.PixelsToTwipsY(eventArgs.Y)
        '    If (lvwBankDetailsList.HitTest(x, y) Is Nothing) Then
        '        ' Nothing selected
        '    Else
        '        Dim lDeleteStatus As Long
        '
        '        m_lReturn = CheckIfDeleted(SelectedArrayIndexOnTag, lDeleteStatus)
        '        If m_lReturn <> PMTrue Then
        '
        '        ElseIf m_lReturn = PMTrue Then
        '            If lDeleteStatus = 1 Then
        '                cmdBankActiveInactive.Caption = "Ac&tive"
        '            Else
        '                cmdBankActiveInactive.Caption = "&Inactive"
        '            End If
        '        End If
        '    End If
    End Sub

    Private Sub UserControl_Initialize()
        'SetResize
    End Sub

    'UPGRADE_NOTE: (7001) The following declaration (SetResize) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Sub SetResize()
    'Try 
    '
    ' Set start dimensions
    'm_lWidth = CInt(ClientRectangle.Width)
    'm_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))
    '
    ' Search Block
    'uctAnchor.Add(lvwBankDetailsList, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
    'uctAnchor.Add(fraBankDetails, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
    '
    'uctAnchor.Add(cmdBankAdd, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft)
    'uctAnchor.Add(cmdBankEdit, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft)
    'uctAnchor.Add(cmdBankDelete, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft)
    '
    'Catch 
    'End Try
    '
    '
    '
    '
    'End Sub

    Private Function GetPartyBankDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyBankDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue



            'developer guide no. 46
            m_lReturn = m_oBusiness.GetPartyBankDetails(vPartyBankDetails:=m_vPartyBankDetails, vPartyCnt:=m_vPartyCnt, vAccountID:=m_vAccountId)
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = CType(BuildArrayIndex(), gPMConstants.PMEReturnCode)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    Private Function GetPartyBankHistory() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetPartyBankHistory"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            'developer guide no. 46(Guide)
            m_lReturn = m_oBusiness.GetPartyBankHistory(vPartyBankHistory:=m_vPartyBankHistory, vPartyCnt:=m_vPartyCnt, vAccountID:=m_vAccountId)
            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                RaiseError(kMethodName, "GetPartyBankHistory Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function
    ''' <summary>
    ''' GetBusiness
    ''' </summary>
    ''' <returns></returns>
    Public Function GetBusiness() As Integer
        Dim nResult As Integer = 0
        Const kMethodName As String = "GetBusiness"

        Try

            nResult = gPMConstants.PMEReturnCode.PMTrue

            If PartyCnt <> 0 Or AccountId <> 0 Then
                'Get Party Bank Details
                nResult = GetPartyBankDetails()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetPartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                nResult = GetPartyBankHistory()
                If nResult <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "GetPartyBankHistory Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

        Catch ex As Exception
            nResult = PMEReturnCode.PMError
            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=nResult, excep:=ex)

        End Try
        Return nResult
    End Function

    Public Function UpdatePartyBankDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdatePartyBankDetails"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If Information.IsArray(m_vPartyBankDetails) Then

                m_lReturn = m_oBusiness.UpdatePartyBankDetails(vPartyCnt:=m_vPartyCnt, vPartyBankDetails:=m_vPartyBankDetails, sUniqueId:=m_sUniqueId, sScreenHierarchy:=m_sScreenHierarchy)

                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    RaiseError(kMethodName, "UpdatePartyBankDetails Failed", gPMConstants.PMELogLevel.PMLogError)
                End If
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

    Private Function SortListView(ByVal v_iIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SortListView"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            ' Tell it that it's not sorted
            ListViewHelper.SetSortedProperty(lvwBankDetailsList, False)

            ' Set the column to sort on
            ListViewHelper.SetSortKeyProperty(lvwBankDetailsList, v_iIndex)

            ' Swap the ascending/descending around
            If ListViewHelper.GetSortOrderProperty(lvwBankDetailsList) = SortOrder.Ascending Then
                ListViewHelper.SetSortOrderProperty(lvwBankDetailsList, SortOrder.Descending)
            Else
                ListViewHelper.SetSortOrderProperty(lvwBankDetailsList, SortOrder.Ascending)
            End If

            ' Tell it that it's now sorted
            ListViewHelper.SetSortedProperty(lvwBankDetailsList, True)


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=UpdatePartyBankDetails(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function


    ' Function to find whether the row sent as input parameter is active or deleted
    ' It will find out
    'UPGRADE_NOTE: (7001) The following declaration (IsActive) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsActive() As Integer
    '
    'End Function

    Private Sub uctPartyBankControl_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize

        ' Resize the screen
        'Call uctAnchor.Resize(m_lWidth, m_lHeight, ScaleWidth, ScaleHeight)
    End Sub

    ' ***************************************************************** '
    ' Name: SetupControl
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Taken from uctPartyTax
    ' ***************************************************************** '
    Private Function SetupControl(Optional ByVal v_lMode As Integer = 0) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupControl"

        Dim lReturn As Integer

        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            '   lvwBankDetailsList.Enabled = Not m_bReadOnly
            cmdBankAdd.Enabled = Not m_bReadOnly
            cmdBankEdit.Enabled = Not m_bReadOnly
            cmdBankDelete.Enabled = Not m_bReadOnly
            cmdBankSelectAll.Enabled = Not m_bReadOnly
            '    lvwBankDetailsHistory.Enabled = Not m_bReadOnly



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Private Function IsPartyBankActiveTransactions(ByVal lPartyBankId As Integer, ByRef lIsExists As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PartyBankActiveTransactions"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            If lPartyBankId > 0 Then

                m_lReturn = m_oBusiness.ISPartyBankActiveTransactions(lPartyBankId:=lPartyBankId, lIsExists:=lIsExists)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (CheckForChangeBankDetail) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function CheckForChangeBankDetail(ByVal vBankItem As Object, ByVal lIndex As Integer, ByVal vOldBankItem() As Object, ByRef r_bIsEdit As Boolean) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "CheckForChangeBankDetail"
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.IsBank, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.IsBank)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankAccountTypeId, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankAccountTypeId)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If
    '

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.AccountHolderName, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.AccountHolderName)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.AccountNumber, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.AccountNumber)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If
    '

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankBranch, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankBranch)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankBranchCode, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankBranchCode)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd1, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankAdd1)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd2, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankAdd2)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankAdd3, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankAdd3)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankTown, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankTown)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankPCode, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankPCode)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.BankRegion, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.BankRegion)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCNum, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCNum)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCStartDate, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCStartDate)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCExpiryDate, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCExpiryDate)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCIssueNum, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCIssueNum)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCPIN, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCPIN)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.IsRegistered, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.IsRegistered)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd1, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCAdd1)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd2, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCAdd2)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCAdd3, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCAdd3)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCTown, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCTown)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCPCode, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCPCode)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCNameOnCard, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCNameOnCard)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If

    'If ToSafeString(m_vPartyBankDetails(MainModule.ENPartyBank.CCManualAuthorisationNum, lIndex)) <> CStr(vOldBankItem(MainModule.ENPartyBank.CCManualAuthorisationNum)) Then
    'r_bIsEdit = True
    'GoTo Finally_Renamed
    'End If
    '
    'GoTo Finally_Renamed
    'Catch_Renamed: '
    '
    ' DO Not Call any functions before here or the error will be lost
    'iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)
    '
    ' If you want to rollback a transaction or something, do it here
    '
    'Finally_Renamed: '
    '
    'Return result
    'Resume 
    '
    'Return result
    'End Function


    Private Sub lvwBankDetailsHistory_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvwBankDetailsHistory.Click

    End Sub
End Class
