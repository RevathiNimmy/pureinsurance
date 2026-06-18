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
<System.Runtime.InteropServices.ProgId("uctBankGuarenteeControl_NET.uctBankGuarenteeControl")> _
Public Partial Class uctBankGuarenteeControl
	Inherits System.Windows.Forms.UserControl
	' issues
	' -->if user has added and then deleted the bank item then
	'       --> Detect the selected item of an array
	'       --> Design a function to delete item from an array and place the last item of an array to that array item
	'       --> Then at the time adding next item of an array search whether last item is empty or not,
	'       --> if it is empty then don't increase its upper bound
	'
	
	
	
	'------
	' HIGH PRIORITY
	' If BG is already in use then no BG details can be edited and message will be flashed.
	'       "BG already used for Transaction, cannot be edited"
	' If BG is already in use then no BG details can be deleted and message will be flashed.
	'       "BG already used for Transaction, cannot be deleted"
	'
	' Currency Conversion Work
	' Delete Guarantee
	
	
	
	
	Private Const ACClass As String = "uctBankGuaranteeControl"
	
	' objects

	Private m_oBusiness As bSIRBankGuarantee.Business
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
	Private m_lSystemCurrencyId As Integer
	' Array declared to keep the updated bank details
	Private m_vBankGuaranteeDetails As Array
	Private m_lSelectedArrayIndexOnTag As Integer
	Private m_lListSelectedItem As Integer
	Private m_lSelectedTag As Integer
	Private m_bViewMode As Boolean
	' Bank Item Added or Edited, it will be pumped into BankDetails Array
	Private m_vGuaranteeItem As Array
	
    ' Variables
    'developer guide no.101
    Private m_vPartyCnt As Object
	Private m_sPartyCode As String = ""
    Private m_sPartyName As String = ""
    'developer guide no.101
    Private m_vAccountId As Object
	Private m_bIsInitialised As Boolean
	Private m_lWidth As Integer
	Private m_lHeight As Integer
	Private m_vSelectedBranches As Object
	Private m_vSelectedProducts As Object
	Private m_lSelBgId As Integer
	Private m_lStatus As gPMConstants.PMEReturnCode
	' Event that will pass the interface with the changed Bank Details
	Public Event RefreshBGDetails(ByVal Sender As Object, ByVal e As RefreshBGDetailsEventArgs)
	
	
	<Browsable(True)> _
	Public Property PartyCode() As String
		Get
			Return m_sPartyCode
		End Get
		Set(ByVal Value As String)
			m_sPartyCode = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property PartyName() As String
		Get
			Return m_sPartyName
		End Get
		Set(ByVal Value As String)
			m_sPartyName = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property Task() As Integer
		Get
			Return m_iTask
		End Get
		Set(ByVal Value As Integer)
			m_iTask = Value
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property SystemCurrency() As Integer
		Get
			Return m_lSystemCurrencyId
		End Get
		Set(ByVal Value As Integer)
			m_lSystemCurrencyId = Value
		End Set
	End Property
	
	<Browsable(False)> _
	Public ReadOnly Property Status() As Integer
		Get
			Return m_lStatus
		End Get
	End Property
	
	
	<Browsable(True)> _
	Public Property SelBgId() As Integer
		Get
			Return m_lSelBgId
		End Get
		Set(ByVal Value As Integer)

			m_lSelBgId = CInt(Value)
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property ViewMode() As Boolean
		Get
			Return m_bViewMode
		End Get
		Set(ByVal Value As Boolean)

			m_bViewMode = Value
			
			If m_bViewMode Then
				cmdBGAdd.Visible = False
				cmdBGEdit.Visible = False
				cmdBGDelete.Visible = False
				lvwBGList.Height = VB6.TwipsToPixelsY(3500)
				fraBankDetails.Height = VB6.TwipsToPixelsY(4395) 'fraBankDetails.Height - (cmdBGAdd.Height + 100)
				MyBase.Height = VB6.TwipsToPixelsY(4380) 'fraBankDetails.Height + 30
			ElseIf Not m_bViewMode Then 
				cmdBGAdd.Visible = True
				cmdBGEdit.Visible = True
				cmdBGDelete.Visible = True
				
				lvwBGList.Height = VB6.TwipsToPixelsY(5475)
				fraBankDetails.Height = VB6.TwipsToPixelsY(6765) 'fraBankDetails.Height + (cmdBGAdd.Height + 100)
				MyBase.Height = VB6.TwipsToPixelsY(6780) 'fraBankDetails.Height + 30
			End If
		End Set
	End Property
	
	
	<Browsable(True)> _
	Public Property BankGuaranteeDetails() As Object
		Get
			Return m_vBankGuaranteeDetails
		End Get
		Set(ByVal Value As Object)
			m_vBankGuaranteeDetails = Value
		End Set
	End Property
	
	'Public Property Get PartyBankHistory() As Variant
	'    PartyBankHistory = m_vPartyBankHistory
	'End Property
	'
	'Public Property Let PartyBankHistory(ByVal value As Variant)
	'    m_vPartyBankHistory = value
	'End Property
    'developer guide no.101
    <Browsable(True)> _
    Public Property PartyCnt() As Object
        Get
            Return m_vPartyCnt
        End Get
        Set(ByVal Value As Object)
            m_vPartyCnt = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property SelectedBranches() As Object
        Get
            Return m_vSelectedBranches
        End Get
        Set(ByVal Value As Object)


            m_vSelectedBranches = Value
        End Set
    End Property


    <Browsable(True)> _
    Public Property SelectedProducts() As Object
        Get
            Return m_vSelectedProducts
        End Get
        Set(ByVal Value As Object)


            m_vSelectedProducts = Value
        End Set
    End Property

    'developer guide no.101
    <Browsable(True)> _
    Public Property AccountId() As Object
        Get
            Return m_vAccountId
        End Get
        Set(ByVal Value As Object)

            m_vAccountId = Value
        End Set
    End Property

    'UPGRADE_NOTE: (7001) The following declaration (get GuaranteeItem) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function GuaranteeItem() As Object
    'Return m_vGuaranteeItem
    'End Function


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

                m_lSelectedTag = Convert.ToString(lvwBGList.Items.Item(ListSelectedItem).Tag)

                m_lReturn = CType(SearchArrayIndexOnTag(lSelectedTag:=m_lSelectedTag, lSelectedArrayIndex:=m_lSelectedArrayIndexOnTag, lColumnId:=MainModule.ENBankGuarantee.RowIndex), gPMConstants.PMEReturnCode)

                result = m_lSelectedArrayIndexOnTag
            End If
            Return result
        End Get
    End Property


    Private Sub cmdBGAdd_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBGAdd.Click
        m_lReturn = ProcessAddBG()
    End Sub

    Private Sub cmdBGDelete_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBGDelete.Click
        If cmdBGDelete.Text.ToLower() = "&delete" Then
            m_lReturn = ProcessDeleteBG()
        ElseIf cmdBGDelete.Text.ToLower() = "&undelete" Then
            m_lReturn = ProcessUnDeleteBG()
        End If
    End Sub


    Public Function ProcessUnDeleteBG() As Integer
        Const kMethodName As String = "ProcessDeleteBG"
        Dim lSeletedArrayItem As Integer
        Try


            ' Validate to find that their isn't exists live policy


            lSeletedArrayItem = SelectedArrayIndexOnTag
            m_lListSelectedItem = lvwBGList.FocusedItem.Index + 1 - 1

            If (m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSeletedArrayItem) = gPMConstants.PMEComponentAction.PMDelete And CDbl(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsDeleted, lSeletedArrayItem)) = 0) Or (m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSeletedArrayItem) <> gPMConstants.PMEComponentAction.PMDelete And CDbl(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsDeleted, lSeletedArrayItem)) = 0) Then

                m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSeletedArrayItem) = -1

                'developer guide no. 185,49
                lvwBGList.Items.Item(lvwBGList.FocusedItem.Index).ImageKey = "saved"
            ElseIf m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSeletedArrayItem) <> gPMConstants.PMEComponentAction.PMDelete And CDbl(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsDeleted, lSeletedArrayItem)) = 1 Then

                ' Validate and undelete the row
                'm_lReturn = IsExistsPaymentType(m_vBankGuaranteeDetails(ENBankGuarantee.BankNameId, lSeletedArrayItem)(ENPMLookups.Id))
                'If m_lReturn = PMFalse Then
                m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSeletedArrayItem) = gPMConstants.PMEComponentAction.PMReverse

                'developer guide no. 185,49
                lvwBGList.Items.Item(lvwBGList.FocusedItem.Index).ImageKey = "add"
                cmdBGDelete.Text = "&Delete"
                'End If

            End If


            RaiseEvent RefreshBGDetails(Me, New RefreshBGDetailsEventArgs(m_vBankGuaranteeDetails))

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessDeleteBG(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
    End Function

    Private Sub cmdBGEdit_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdBGEdit.Click
        m_lReturn = ProcessEditBG()
    End Sub

    Private Function SearchArrayIndexOnTag(ByVal lSelectedTag As Integer, ByRef lSelectedArrayIndex As Integer, ByVal lColumnId As MainModule.ENBankGuarantee) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SearchArrayIndexOnTag"


        Try



        result = gPMConstants.PMEReturnCode.PMTrue



        For lArrayCount As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
            If CDbl(m_vBankGuaranteeDetails(lColumnId, lArrayCount)) = lSelectedTag Then
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
        Dim temp_m_oBusiness As Object = Nothing
        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Check if already initialised
        If m_bIsInitialised Then
            Return result
        End If

        'Set m_colPaymentItems = New Collection

        ' Create an instance of the object manager.
        g_oObjectManager = New bObjectManager.ObjectManager()

        ' Call the initialise method.
        m_lReturn = g_oObjectManager.Initialise(sCallingAppName:=ACApp)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' If UserID is 0 assume that user cancelled logon
        If g_oObjectManager.UserID = 0 Then
            ' Exit application
                result = gPMConstants.PMEReturnCode.PMFalse
                Exit Function
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

        m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRBankGuarantee.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
        m_oBusiness = temp_m_oBusiness
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRBankGuarantee.Business Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' hold Initialised status
        m_bIsInitialised = True


        Catch ex As Exception

        ' Do Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

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
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = GetBusiness()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetCaseClaimLink Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        m_lReturn = PopulateScreen()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateCaseClaimList Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function SetSelectedItemOnId() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetSelectedItemOnId"
        Dim SelRowIndex As gPMConstants.PMEComponentAction


        Try

       
        result = gPMConstants.PMEReturnCode.PMTrue

        For lCount As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
            If gPMFunctions.ToSafeInteger(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, lCount)) = SelBgId Then
                SelRowIndex = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowIndex, lCount)

                For lSelListItem As Integer = 1 To lvwBGList.Items.Count
                    If Convert.ToString(lvwBGList.Items.Item(lSelListItem - 1).Tag) = SelRowIndex Then
                        lvwBGList.Items.Item(lSelListItem - 1).Selected = True
                    End If
                Next
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

    ' ***************************************************************** '
    ' Name: LoadEdit
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created: Gaurav Arora :
    ' ***************************************************************** '
    Public Function LoadAdd() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadAdd"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display all language specific captions.
        m_lReturn = Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display all language specific captions.
        m_lReturn = ProcessAddBG()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: LoadEdit
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created: Gaurav Arora :
    ' ***************************************************************** '
    Public Function LoadEdit() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadEdit"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display all language specific captions.
        m_lReturn = Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        '    ' Display all language specific captions.
        m_lReturn = SetSelectedItemOnId()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display all language specific captions.
        m_lReturn = ProcessEditBG()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    ' ***************************************************************** '
    ' Name: LoadEdit
    ' Parameters: n/a
    ' Description:
    ' History:
    ' Created: Gaurav Arora :
    ' ***************************************************************** '
    Public Function LoadView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "LoadView"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        ' Display all language specific captions.
        m_lReturn = Load_Renamed()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        '    ' Display all language specific captions.
        m_lReturn = SetSelectedItemOnId()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If

        ' Display all language specific captions.
        m_lReturn = ProcessViewBG()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetInterfaceDefaults Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=LoadEdit(), excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

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
            gPMFunctions.RaiseError(kMethodName, "DisplayCaptions Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = SetupListView()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        If m_iTask = gPMConstants.PMEComponentAction.PMView Then
            SetupInViewOnlyMode()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetupListView Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
        End If

        Return result
    End Function

    Private Function ProcessViewBG() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessEditBG"
        Dim objPartyBank As New frmBankGuaranteeDetails
        Try


            result = gPMConstants.PMEReturnCode.PMTrue



            If lvwBGList.Items.Count > 0 Then
                ' m_lListSelectedItem = lvwBGList.FocusedItem.Index + 1 - 1
                m_lListSelectedItem = lvwBGList.SelectedItems(0).Index + 1 - 1
                m_lReturn = CType(SetGuaranteeItem(SelectedArrayIndexOnTag), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetBankItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                objPartyBank.Task = gPMConstants.PMEComponentAction.PMView
                objPartyBank.SystemCurrencyId = SystemCurrency
                'developer guide no. 24 (guide)
                objPartyBank.GuaranteeItem = m_vGuaranteeItem


                objPartyBank.ShowDialog()
            End If
            m_lStatus = objPartyBank.Status


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessAddBG(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (cmdBGView) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function cmdBGView() As Integer
    'm_lReturn = ProcessViewBG()
    'ProcessEditBG()
    '
    'End Function

    Private Function SetupInViewOnlyMode() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetupInViewOnlyMode"

        Try

            result = gPMConstants.PMEReturnCode.PMTrue

            cmdBGAdd.Enabled = False
            cmdBGEdit.Enabled = False
            cmdBGDelete.Enabled = False
            cmdInvoke.Enabled = False
            cmdView.Enabled = True

        Catch
        End Try

        Return result
    End Function


    Private Function DisplayCaptions() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DisplayCaptions"

        Try



        result = gPMConstants.PMEReturnCode.PMTrue


        cmdBGAdd.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kAddButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        cmdBGEdit.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kEditButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))


        cmdBGDelete.Text = CStr(iPMFunc.GetResData(iLangID:=g_iLanguageID, lId:=kDeleteButton, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))



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

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    '***************************************************************** '
    ' Name: SetupBankDetailsListView
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : Date :
    '***************************************************************** '
    Private Function SetupBankDetailsListView() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "SetupBankDetailsListView"

        Dim lColWidth As Integer
        Dim sCaption As String = ""


        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        lColWidth = CInt((VB6.PixelsToTwipsX(lvwBGList.Width) - 100) / 6)

        lvwBGList.Columns.Clear()


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBankName, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexBankName, "", sCaption, CInt(VB6.TwipsToPixelsX(2129)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBGNo, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexBGNo, "", sCaption, CInt(VB6.TwipsToPixelsX(lColWidth)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBGLimit, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexBGLimit, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwAvailableBal, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexAvailableBal, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwExpDate, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexExpdate, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)

        '    sCaption = GetResData(iLangID:=m_iLanguageID%, _
        ''                          lId:=kRegKeyConstLvwParty, _
        ''                          iDataType:=PMResString)
        '
        '    lvwBGList.ColumnHeaders.Add kBankGuaranteeColHIndexParty + 1, _
        ''                                        , _
        ''                                        sCaption, _
        ''                                        1890, _
        ''                                        lvwColumnLeft


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwProduct, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexproduct, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)


        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBranch, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexBranch, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)

        'Start - Sankar - Bank Guarantee Bug Fixing

        sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstLvwBGStatus, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

        lvwBGList.Columns.Insert(kBankGuaranteeColHIndexBGStatus, "", sCaption, CInt(VB6.TwipsToPixelsX(1890)), HorizontalAlignment.Left, -1)
        'End - Sankar - Bank Guarantee Bug Fixing


        lvwBGList.LabelEdit = False
        '
        '     add the grid lines and full row select for the Reserve List view
        '    m_lReturn = SetExtraListViewProperties(v_hWndList:=lvwCaseClaimList.hwnd, _
        ''        v_vShowGridLines:=False, v_vShowRowSelect:=True)
        '
        '    If m_lReturn <> PMTrue Then
        '        RaiseError kMethodName, "SetExtraListViewProperties  Failed", PMLogError
        '    End If
        '
        '    DisableInterface bEnabled:=False, bAllControl:=False



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function

    Public Function ProcessAddBG() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessAddBG"
        Dim objPartyBank As New frmBankGuaranteeDetails
        Try


            result = gPMConstants.PMEReturnCode.PMTrue



            ' Set the Properties
            objPartyBank.Task = gPMConstants.PMEComponentAction.PMAdd
            objPartyBank.SystemCurrencyId = SystemCurrency
            objPartyBank.ShowDialog()
            If objPartyBank.Status = gPMConstants.PMEReturnCode.PMOK Then
                m_vGuaranteeItem = objPartyBank.GuaranteeItem
                m_lReturn = CType(AddArrayItem(m_vGuaranteeItem), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                m_lReturn = CType(AddBankItemToList(m_vGuaranteeItem), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "AddArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                '    PopulateBankDetails
                RaiseEvent RefreshBGDetails(Me, New RefreshBGDetailsEventArgs(m_vBankGuaranteeDetails))
            End If
            m_lStatus = objPartyBank.Status

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    'Public Function SetupListviewDisplay(lListItem as Long, _
    '

    'End Function

    Public Function ProcessEditBG() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessEditBG"
        Dim objPartyBank As New frmBankGuaranteeDetails
        Try


            result = gPMConstants.PMEReturnCode.PMTrue



            ' Set the Properties
            'objPartyBank.Task = PMEdit
            If lvwBGList.Items.Count > 0 Then

                'start
                m_lListSelectedItem = lvwBGList.Items.IndexOf(lvwBGList.SelectedItems.Item(0))

                'developer guide no. 185,49
                lvwBGList.Items(m_lListSelectedItem).ImageKey = "edited"

                m_lReturn = CType(SetGuaranteeItem(SelectedArrayIndexOnTag), gPMConstants.PMEReturnCode)
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "SetBankItem Failed", gPMConstants.PMELogLevel.PMLogError)
                End If

                objPartyBank.Task = gPMConstants.PMEComponentAction.PMEdit
                objPartyBank.SystemCurrencyId = SystemCurrency
                'developer guide no. 24 (guide)
                objPartyBank.GuaranteeItem = m_vGuaranteeItem


                objPartyBank.ShowDialog()


                If objPartyBank.Status = gPMConstants.PMEReturnCode.PMOK Then
                    m_vGuaranteeItem = objPartyBank.GuaranteeItem

                    m_lReturn = CType(EditArrayItem(vEditedItem:=m_vGuaranteeItem), gPMConstants.PMEReturnCode)
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "EditArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If


                    m_lReturn = PopulateBankDetailsList()
                    If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                        gPMFunctions.RaiseError(kMethodName, "PopulateBankDetailsList Failed", gPMConstants.PMELogLevel.PMLogError)
                    End If

                    RaiseEvent RefreshBGDetails(Me, New RefreshBGDetailsEventArgs(m_vBankGuaranteeDetails))
                End If

                'developer guide no. 185,49
                lvwBGList.Items.Item(m_lListSelectedItem).ImageKey = "saved"
                'end
                m_lStatus = objPartyBank.Status

            End If
        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=ProcessAddBG(), excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function SetGuaranteeItem(ByRef lSelectedIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetGuaranteeItem"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        m_vGuaranteeItem = Array.CreateInstance(GetType(Object), New Integer() {MainModule.ENBankGuarantee.uBoundBankGuarantee + 1}, New Integer() {0})

        m_vGuaranteeItem(MainModule.ENBankGuarantee.AvailableBal) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.AvailableBal, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.BankBranch) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.Branches) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, lSelectedIndex)


        m_vGuaranteeItem(MainModule.ENBankGuarantee.BGCurrencyId) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGCurrencyId, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.BGId) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.BGLimit) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.BGRef) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, lSelectedIndex)

        m_vGuaranteeItem(MainModule.ENBankGuarantee.ExpiryDate) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.IsPolicyLock) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsPolicyLock, lSelectedIndex)

        m_vGuaranteeItem(MainModule.ENBankGuarantee.PartyCnt) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.PartyCnt, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.Products) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.CustodyBranchId) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.CustodyBranchId, lSelectedIndex)
        m_vGuaranteeItem(MainModule.ENBankGuarantee.IssueDate) = m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IssueDate, lSelectedIndex)

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Public Function ProcessDeleteBG() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "ProcessDeleteBG"
        Try

        'Dim lIsExits As Long
        ' Validate to find that their isn't exists live policy

        If Not (m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, SelectedArrayIndexOnTag) = gPMConstants.PMEComponentAction.PMReverse) Then
            'If m_lReturn <> PMTrue Then
            'ElseIf m_lReturn = PMTrue Then
            '    If lIsExits <= 0 Then
            m_lListSelectedItem = lvwBGList.FocusedItem.Index + 1 - 1
            m_lReturn = CType(DeleteArrayItem(SelectedArrayIndexOnTag), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DeleteArrayItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If
            '   Else
            '       MsgBox "Their is already bank setup against this Party." & vbCrLf & "Can't Delete.", vbInformation
            '       Exit Function
            '   End If
            'End If
        ElseIf m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, SelectedArrayIndexOnTag) = gPMConstants.PMEComponentAction.PMReverse Then
            m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, SelectedArrayIndexOnTag) = -1

            'developer guide no. 185,49
            lvwBGList.Items.Item(m_lListSelectedItem).ImageKey = "delete"
        End If

        RaiseEvent RefreshBGDetails(Me, New RefreshBGDetailsEventArgs(m_vBankGuaranteeDetails))

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    'UPGRADE_NOTE: (7001) The following declaration (IsBGinUse) seems to be dead code More Information: http://www.vbtonet.com/ewis/ewi7001.aspx
    'Private Function IsBGinUse(ByVal lPartyCnt As Integer, ByVal lPaymentTypeId As Integer, ByRef lIsExists As Integer) As Integer
    '
    'Dim result As Integer = 0
    'Const kMethodName As String = "IsBGinUse"
    'On Error GoTo Catch_Renamed
    '
    'result = gPMConstants.PMEReturnCode.PMTrue
    '

    'm_lReturn = m_oBusiness.IsBGinUse(lPartyCnt:=lPartyCnt, lPaymentTypeId:=lPaymentTypeId, lIsExists:=lIsExists)
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

    Public Function DeleteArrayItem(ByVal ArrayIndex As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DeleteArrayItem"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        ' at the time of deletion we need to find out whether item is in database or not
        ' if it is in database then set rowstatus = PMDelete
        ' else delete the item from an array and swap the last row with this new blank row
        ' and set the status of last row to PMNotFound
        If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, ArrayIndex) <> gPMConstants.PMEComponentAction.PMAdd Then
            m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, ArrayIndex) = gPMConstants.PMEComponentAction.PMDelete

            'developer guide no. 185,49
            lvwBGList.Items.Item(lvwBGList.FocusedItem.Index).ImageKey = "delete"
        ElseIf m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, ArrayIndex) = gPMConstants.PMEComponentAction.PMAdd Then
            '        m_lReturn = MoveArrayRow(SelectedArrayIndexOnTag, _
            ''                                    UBound(m_vBankGuaranteeDetails, 2))

            m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, SelectedArrayIndexOnTag) = gPMConstants.PMEReturnCode.PMNotFound
            m_lReturn = CType(PopulateBankDetailsList(), gPMConstants.PMEReturnCode)

            ' Delete th item from an array and swap the last row to this index
        End If
        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function AddArrayItem(ByVal vAddedItem As Array) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "AddArrayItem"

        Dim uboundBankDetails As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ' Find whether last item is empty or not
            ' if empty then don't increase its bound and put the item their only
            ' it can be found by checking the rowstatus to PMNotFound

            If Not gPMFunctions.IsArrayEmpty(m_vBankGuaranteeDetails) Then
                If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, uboundBankDetails) <> gPMConstants.PMEReturnCode.PMNotFound Then
                    uboundBankDetails = m_vBankGuaranteeDetails.GetUpperBound(1) + 1
                    m_vBankGuaranteeDetails = ArraysHelper.RedimPreserve(Of Object(,))(m_vBankGuaranteeDetails, New Integer() {m_vBankGuaranteeDetails.GetUpperBound(0) + 1, uboundBankDetails + 1}, New Integer() {0, 0})
                End If
            Else
                m_vBankGuaranteeDetails = Array.CreateInstance(GetType(Object), New Integer() {MainModule.ENBankGuarantee.uBoundBankGuarantee + 1, 1}, New Integer() {0, 0})
            End If
            uboundBankDetails = m_vBankGuaranteeDetails.GetUpperBound(1)



            m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, uboundBankDetails) = gPMConstants.PMEComponentAction.PMAdd
            m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowIndex, uboundBankDetails) = uboundBankDetails + 1 ' Need to think
            m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGId, uboundBankDetails) = "0"

            m_lReturn = CType(SetBankGuaranteeItem(vGuaranteeItem:=vAddedItem, lIndex:=uboundBankDetails), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetBankGuaranteeItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If


        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function FilterArrayForChanges(ByRef oFilteredArray(,) As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetBankGuaranteeItem"
        Dim vArray(,) As Object
        Dim lSelRowCount As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            ReDim vArray(MainModule.ENBankGuarantee.uBoundBankGuarantee, 0)
            lSelRowCount = 0

            For lRowCount As Integer = oFilteredArray.GetLowerBound(1) To oFilteredArray.GetUpperBound(1)

                If CDbl(oFilteredArray(MainModule.ENBankGuarantee.RowStatus, lRowCount)) <> -1 Then
                    For lColCount As Integer = oFilteredArray.GetLowerBound(0) To oFilteredArray.GetUpperBound(0)


                        vArray(lColCount, lSelRowCount) = oFilteredArray(lColCount, lRowCount)
                    Next
                    lSelRowCount += 1

                    ReDim Preserve vArray(vArray.GetUpperBound(0), vArray.GetUpperBound(1) + 1)
                End If
            Next



            oFilteredArray = vArray
            ReDim Preserve oFilteredArray(oFilteredArray.GetUpperBound(0), oFilteredArray.GetUpperBound(1) - 1)

            Return result

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result)

            ' If you want to rollback a transaction or something, do it here

        Catch ex As Exception

        Finally





        End Try
        Return result
    End Function

    Public Function SetBankGuaranteeItem(ByVal vGuaranteeItem() As Object, ByVal lIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SetBankGuaranteeItem"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.AvailableBal, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.AvailableBal)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankBranch, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.BankBranch)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.Branches)



        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGCurrencyId, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.BGCurrencyId)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.BGLimit)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.BGRef)


        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.ExpiryDate)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsPolicyLock, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.IsPolicyLock)


        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.PartyCnt, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.PartyCnt)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.Products)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.CustodyBranchId, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.CustodyBranchId)

        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IssueDate, lIndex) = vGuaranteeItem(MainModule.ENBankGuarantee.IssueDate)
        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, lIndex) = MainModule.BGStatus.Active

        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Public Function EditArrayItem(ByVal vEditedItem As Object) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "EditArrayItem"
        Dim lSelectedArrayIndex As Integer
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            lSelectedArrayIndex = SelectedArrayIndexOnTag

            If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSelectedArrayIndex) <> gPMConstants.PMEComponentAction.PMAdd Then
                m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lSelectedArrayIndex) = gPMConstants.PMEComponentAction.PMEdit
            End If


            m_lReturn = CType(SetBankGuaranteeItem(vGuaranteeItem:=vEditedItem, lIndex:=lSelectedArrayIndex), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "SetBankGuaranteeItem Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally

        End Try
        Return result
    End Function

    Private Function DataToInterface() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "DataToInterface"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        If Information.IsArray(m_vBankGuaranteeDetails) Then
            m_sPartyCode = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Shortname, 0)).Trim()
            m_sPartyName = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Resolved_Name, 0)).Trim()
        End If

        txtPartyCode.Text = m_sPartyCode
        txtPartyName.Text = m_sPartyName



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    Private Function PopulateScreen() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "PopulateScreen"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = DataToInterface()
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = CType(PopulateBankDetailsList(), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "PopulateScreen Failed", gPMConstants.PMELogLevel.PMLogError)
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


        If Information.IsArray(m_vBankGuaranteeDetails) Then
            For lBounds As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
                m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowIndex, lBounds) = lBounds
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

    ' ***************************************************************** '
    ' Name: PopulateBankDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-07-2007 :
    ' ***************************************************************** '
    Private Function AddBankItemToList(ByVal vGuaranteeItem As Array) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "AddBankItemToList"
        Dim oListItem As ListViewItem

        Try



        result = gPMConstants.PMEReturnCode.PMTrue





        oListItem = lvwBGList.Items.Add(CStr(vGuaranteeItem(MainModule.ENBankGuarantee.BankNameId)(MainModule.ENPMLookups.Description)).Trim(), "add")

        If CDbl(vGuaranteeItem(MainModule.ENBankGuarantee.IsDeleted)) = 0 Then

            'developer guide no. 49
            oListItem.ImageKey = "add"
        ElseIf CDbl(vGuaranteeItem(MainModule.ENBankGuarantee.IsDeleted)) = 1 Then

            'developer guide no. 49
            oListItem.ImageKey = "delete"
        End If

        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGNo).Text = CStr(vGuaranteeItem(MainModule.ENBankGuarantee.BGRef)).Trim()

        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGLimit).Text = CStr(vGuaranteeItem(MainModule.ENBankGuarantee.BGLimit)).Trim()

        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexAvailableBal).Text = CStr(vGuaranteeItem(MainModule.ENBankGuarantee.AvailableBal)).Trim()

        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexExpdate).Text = CStr(vGuaranteeItem(MainModule.ENBankGuarantee.ExpiryDate)).Trim()

        'developer guide no. 240
        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexParty).Text = Convert.ToString(vGuaranteeItem(MainModule.ENBankGuarantee.PartyCnt)).Trim()

        'Start - Sankar - Bank Guarantee Bug Fixing
        If Information.IsArray(vGuaranteeItem(MainModule.ENBankGuarantee.Branches)) Then

            If vGuaranteeItem(MainModule.ENBankGuarantee.Branches).GetUpperBound(0) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBranch).Text = "Multiple"
            Else

                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBranch).Text = CStr(vGuaranteeItem(MainModule.ENBankGuarantee.Branches)(0, MainModule.ENPMLookups.Description)).Trim()
            End If
        End If

        If Information.IsArray(vGuaranteeItem(MainModule.ENBankGuarantee.Products)) Then

            If vGuaranteeItem(MainModule.ENBankGuarantee.Products).GetUpperBound(0) > 0 Then
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexproduct).Text = "Multiple"
            Else

                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexproduct).Text = CStr(vGuaranteeItem(MainModule.ENBankGuarantee.Products)(0, MainModule.ENPMLookups.Description)).Trim()
            End If
        End If
        Dim sBGStatusDescription As String = ""

        GetBGStatusDescription(CInt(vGuaranteeItem(MainModule.ENBankGuarantee.BGStatusId)), sBGStatusDescription)
        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGStatus).Text = sBGStatusDescription
        'End - Sankar - Bank Guarantee Bug Fixing

        'oListItem.SubItems(kBankGuaranteeColHIndexproduct) = Trim$(vGuaranteeItem(ENBankGuarantee.BankPaymentTypeId, i)(ENPMLookups.Description))
        'oListItem.SubItems(kBankGuaranteeColHIndexBranch) = Trim$(vGuaranteeItem(ENBankGuarantee.BankAccountTypeId, i)(ENPMLookups.Description))

        oListItem.Tag = CStr(m_vBankGuaranteeDetails.GetUpperBound(1) + 1)



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function


    ' ***************************************************************** '
    ' Name: PopulateBankDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-07-2007 :
    ' ***************************************************************** '
    Private Function PopulateBankDetailsList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBankDetails"
        Dim oListItem As ListViewItem
        Dim sBGStatusDescription As String = ""

        Try



        result = gPMConstants.PMEReturnCode.PMTrue

     

        If gPMFunctions.IsArrayEmpty(m_vBankGuaranteeDetails) Then
            Return result
        End If

        'Set max rows to number of addresses - though must be at least 5
        lvwBGList.Items.Clear()

        For i As Integer = m_vBankGuaranteeDetails.GetLowerBound(1) To m_vBankGuaranteeDetails.GetUpperBound(1)
            If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, i) <> gPMConstants.PMEReturnCode.PMNotFound Then


                'developer guide no.49
                oListItem = lvwBGList.Items.Add(CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BankNameId, i)(MainModule.ENPMLookups.Description)).Trim(), "saved")
                'developer guide no.248
                If gPMFunctions.ToSafeDouble(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsDeleted, i)) = 0 Then

                    'developer guide no. 49
                    oListItem.ImageKey = "saved"
                ElseIf gPMFunctions.ToSafeDouble(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsDeleted, i)) = 1 Then

                    'developer guide no. 49
                    oListItem.ImageKey = "delete"
                End If
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGNo).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGRef, i)).Trim()
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGLimit).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGLimit, i)).Trim())))

                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexAvailableBal).Text = gPMFunctions.FormatField(iFormatType:=gPMConstants.PMEFormatStyle.PMFormatCurrency, vFieldValue:=CStr(gPMFunctions.ToSafeCurrency(CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.AvailableBal, i)).Trim())))
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexExpdate).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.ExpiryDate, i)).Trim()
                'developer guide no.248
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexParty).Text = Convert.ToString(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.PartyCnt, i)).Trim()
                'oListItem.SubItems(kBankGuaranteeColHIndexBranch) = Trim$(m_vBankGuaranteeDetails(ENBankGuarantee.Branches, i))
                'oListItem.SubItems(kBankGuaranteeColHIndexproduct) = Trim$(m_vBankGuaranteeDetails(ENBankGuarantee.Products, i))

                'Start - Sankar - Bank Guarantee Bug Fixing
                If Information.IsArray(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i)) Then
                    If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i).GetUpperBound(1) > 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBranch).Text = "Multiple"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBranch).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Branches, i)(MainModule.ENPMLookups.Description, 0)).Trim()
                    End If
                End If

                If Information.IsArray(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i)) Then
                    If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i).GetUpperBound(1) > 0 Then
                        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexproduct).Text = "Multiple"
                    Else
                        ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexproduct).Text = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.Products, i)(MainModule.ENPMLookups.Description, 0)).Trim()
                    End If
                End If
                'developer guide no.248
                GetBGStatusDescription(gPMFunctions.ToSafeInteger(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, i)), sBGStatusDescription)
                ListViewHelper.GetListViewSubItem(oListItem, kBankGuaranteeColHIndexBGStatus).Text = sBGStatusDescription
                'End - Sankar - Bank Guarantee Bug Fixing

                'oListItem.SubItems(kBankGuaranteeColHIndexBranch) = Trim$(m_vBankGuaranteeDetails(ENBankGuarantee.BankAccountTypeId, i)(ENPMLookups.Description,0))

                oListItem.Tag = CStr(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowIndex, i))
            End If

        Next i
        If (lvwBGList.Items.Count > 0) Then
            lvwBGList.FullRowSelect = True
            lvwBGList.Focus()
            lvwBGList.Items(0).Selected = True
            lvwBGList.Items(0).Focused = True
        End If



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

        Finally





        End Try
        Return result
    End Function

    'Start - Sankar - Bank Guarantee Bug Fixing
    Public Sub GetBGStatusDescription(ByVal iBGStatusId As Integer, ByRef sBGStatusDescription As String)
        If iBGStatusId = 1 Then
            sBGStatusDescription = kBGStatusActive
        ElseIf iBGStatusId = 2 Then
            sBGStatusDescription = kBGStatusIssued
        ElseIf iBGStatusId = 3 Then
            sBGStatusDescription = kBGStatusInvoked
        ElseIf iBGStatusId = 4 Then
            sBGStatusDescription = kBGStatusDeleted
        ElseIf iBGStatusId = 5 Then
            sBGStatusDescription = kBGStatusExpired
        End If
    End Sub
    'End - Sankar - Bank Guarantee Bug Fixing

    Public Function BusinessToInterface() As Integer
        PopulateBankDetailsList()
    End Function

    Private Sub cmdInvoke_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdInvoke.Click
        'Dim dtInvokedDate As Date = CDate(Interaction.InputBox("Enter the Invoked date for the Bank Guarantee", "Invoked Date"))
        Dim dtInvokedDate As Object = Interaction.InputBox("Enter the Invoked date for the Bank Guarantee", "Invoked Date")
        If Not Information.IsDate(dtInvokedDate) Then
            MessageBox.Show("Invalid Date specified.", Application.ProductName)
        Else
            'm_lReturn = CType(InvokeBG(dtInvokedDate:=dtInvokedDate), gPMConstants.PMEReturnCode)
            m_lReturn = CType(InvokeBG(dtInvokedDate:=CDate(dtInvokedDate)), gPMConstants.PMEReturnCode)
        End If
    End Sub

    Private Function InvokeBG(ByVal dtInvokedDate As Date) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "InvokeBG"
        Try

        Dim lSelectedTag, lSelectedArrayIndex As Integer

        result = gPMConstants.PMEReturnCode.PMTrue

        'Start - Sankar - Bank Guarantee Bug Fixing
        'Changed m_lListSelectedItem to m_lSelectedArrayIndexOnTag
        'm_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IssueDate, m_lSelectedArrayIndexOnTag) = dtInvokedDate
        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, m_lSelectedArrayIndexOnTag) = dtInvokedDate
        m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, m_lSelectedArrayIndexOnTag) = 4
        'End - Sankar - Bank Guarantee Bug Fixing


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here
        Finally





        End Try
        Return result
    End Function

    Private Sub cmdView_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdView.Click
        m_lReturn = ProcessViewBG()
    End Sub



    Private Sub lvwBGList_ColumnClick(ByVal eventSender As Object, ByVal eventArgs As ColumnClickEventArgs) Handles lvwBGList.ColumnClick
        Dim ColumnHeader As ColumnHeader = lvwBGList.Columns(eventArgs.Column)
        ' Sort the data
        m_lReturn = CType(SortListView(v_iIndex:=ColumnHeader.Index + 1 - 1), gPMConstants.PMEReturnCode)
    End Sub

    Private Sub lvwBGList_DoubleClick(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwBGList.DoubleClick
        If lvwBGList.Items.Count > 0 Then
            If m_iTask <> gPMConstants.PMEComponentAction.PMView And CDbl(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, lvwBGList.FocusedItem.Index + 1 - 1)) = 1 Then
                ' Write a code to call cmdedit click on the double click of this list
                m_lReturn = ProcessEditBG()
            Else
                ' Write a code to call cmdedit click on the double click of this list
                m_lReturn = ProcessViewBG()
            End If
        End If
    End Sub

    Private Sub lvwBGList_ItemClick(ByVal Item As ListViewItem)

        m_lListSelectedItem = lvwBGList.FocusedItem.Index + 1 - 1

        If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
            If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, SelectedArrayIndexOnTag) = MainModule.BGStatus.Active Then
                cmdBGEdit.Enabled = True
                cmdBGDelete.Enabled = True
            ElseIf m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, SelectedArrayIndexOnTag) = MainModule.BGStatus.Deleted Then
                cmdBGDelete.Enabled = True
                cmdBGEdit.Enabled = False
            Else
                cmdBGEdit.Enabled = False
                cmdBGDelete.Enabled = False
            End If

            If m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, SelectedArrayIndexOnTag) = MainModule.BGStatus.Issued Then
                cmdInvoke.Enabled = True
                cmdBGEdit.Enabled = False
                cmdBGDelete.Enabled = False
            Else
                cmdInvoke.Enabled = False
            End If
        End If
    End Sub

    Private Function CheckIfDeleted(ByVal lArrayIndex As Integer, ByRef lStatus As Integer) As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "CheckIfDeleted"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        If Not gPMFunctions.IsArrayEmpty(m_vBankGuaranteeDetails) Then
            'developer guide no.248
            If (gPMFunctions.ToSafeDouble(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.IsDeleted, lArrayIndex)) = 1 Or m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lArrayIndex) = gPMConstants.PMEComponentAction.PMDelete) And m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.RowStatus, lArrayIndex) <> gPMConstants.PMEComponentAction.PMReverse Then
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

    'Private Function GetBankHistoryByPaymentID(ByVal lPartyBankId As Long) As Long
    '    Const kMethodName As String = "GetHistoryByPaymentId"
    '    On Error GoTo Catch
    'Try:
    '    GetBankHistoryByPaymentID = PMTrue
    'Dim lArrayCount As Long
    '
    'lvwBankDetailsHistory.ListItems.Clear
    '
    'If Not IsArrayEmpty(m_vPartyBankHistory) Then
    '    For lArrayCount = LBound(m_vPartyBankHistory, 2) To UBound(m_vPartyBankHistory, 2)
    '        If m_vPartyBankHistory(ENBankGuaranteeHistory.PartyBankId, lArrayCount) = lPartyBankId Then
    '            m_lReturn = AddBankItemHistoryToList(m_vPartyBankHistory, lArrayCount)
    '        End If
    '    Next
    'End If
    '    GoTo Finally
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=GetBankHistoryByPaymentID
    '
    '    ' If you want to rollback a transaction or something, do it here
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function

    ' ***************************************************************** '
    ' Name: PopulateBankDetails
    '
    ' Parameters: n/a
    '
    ' Description:
    '
    ' History:
    '           Created : Gaurav Arora : 06-07-2007 :
    ' ***************************************************************** '
    'Private Function AddBankItemHistoryToList(ByVal m_vPartyBankHistory As Variant, _
    ''                                            ByVal lIndex As Long) As Long
    '
    'Const kMethodName As String = "AddBankItemHistoryToList"
    '    On Error GoTo Catch
    '
    'Try:
    '
    '    AddBankItemHistoryToList = PMTrue
    '
    '        Dim oListItem As ListItem
    '
    '
    '        Set oListItem = lvwBankDetailsHistory.ListItems.Add(Text:=Trim(m_vPartyBankHistory(ENBankGuaranteeHistory.ActionCode, lIndex)), SmallIcon:="history")
    '
    '        oListItem.SubItems(kPtyBankHisColHIndexDate) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.DateModified, lIndex))
    '        oListItem.SubItems(kPtyBankHisColHIndexBankName) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.BankNameId, lIndex)(ENPMLookups.Description))
    '        oListItem.SubItems(kPtyBankHisColHIndexBranch) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.BankBranch, lIndex))
    '
    '        oListItem.SubItems(kPtyBankHisColHIndexAccountName) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.AccountHolderName, lIndex))
    '        oListItem.SubItems(kPtyBankHisColHIndexSortCode) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.BankBranchCode, lIndex))
    '        oListItem.SubItems(kPtyBankHisColHIndexAccNum) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.AccountNumber, lIndex))
    '        oListItem.SubItems(kPtyBankHisColHIndexUser) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.UserID, lIndex))
    '
    '        oListItem.SubItems(kPtyBankHisColHIndexStreetName) = "Address"
    '        oListItem.SubItems(kPtyBankHisColHIndexPostCode) = Trim$(m_vPartyBankHistory(ENBankGuaranteeHistory.BankPCode, lIndex))
    '
    '    GoTo Finally
    '
    'Catch:
    '
    '     ' DO Not Call any functions before here or the error will be lost
    '     LogError _
    ''          v_sClass:=ACClass, _
    ''          v_sMethod:=kMethodName, _
    ''          r_lFunctionReturn:=AddBankItemHistoryToList
    '
    '    ' If you want to rollback a transaction or something, do it here
    '
    'Finally:
    '
    '    Exit Function
    '    Resume
    '
    'End Function

    Private Sub lvwBGList_KeyUp(ByVal eventSender As Object, ByVal eventArgs As KeyEventArgs) Handles lvwBGList.KeyUp
        Dim KeyCode As Integer = eventArgs.KeyCode
        Dim Shift As Integer = eventArgs.KeyData \ &H10000
        Dim lDeleteStatus As Integer

        m_lReturn = CType(CheckIfDeleted(SelectedArrayIndexOnTag, lDeleteStatus), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            If lDeleteStatus = 1 Then
                cmdBGDelete.Text = "&UnDelete"
            Else
                cmdBGDelete.Text = "&Delete"
            End If
        End If
    End Sub

    Private Sub lvwBGList_MouseUp(ByVal eventSender As Object, ByVal eventArgs As MouseEventArgs) Handles lvwBGList.MouseUp
        Dim Button As Integer = CInt(eventArgs.Button)
        Dim Shift As Integer = Control.ModifierKeys \ &H10000
        'developer guide no.70
        'start

        Dim x As Single = eventArgs.X
        Dim y As Single = eventArgs.Y
        'end
        Dim lDeleteStatus As Integer
        If lvwBGList.GetItemAt(x, y) Is Nothing Then
            ' Nothing selected
        Else

            m_lReturn = CType(CheckIfDeleted(SelectedArrayIndexOnTag, lDeleteStatus), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                If lDeleteStatus = 1 Then
                    cmdBGDelete.Text = "&UnDelete"
                Else
                    cmdBGDelete.Text = "&Delete"
                End If
            End If
        End If

    End Sub

    Private Sub UserControl_Initialize()
        SetResize()
    End Sub

    Private Sub SetResize()
        Try

            ' Set start dimensions
            m_lWidth = CInt(ClientRectangle.Width)
            m_lHeight = CInt(VB6.PixelsToTwipsY(ClientRectangle.Height))

            ' Search Block
            uctAnchor.Add(lvwBGList, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)
            uctAnchor.Add(fraBankDetails, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorRight)

            uctAnchor.Add(cmdBGAdd, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft)
            uctAnchor.Add(cmdBGEdit, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft)
            uctAnchor.Add(cmdBGDelete, uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorTop + uSIRCommonControls.uctAnchor.ControlAnchorEnum.AnchorLeft)

        Catch
        End Try

    End Sub

    Private Function GetSystemCurrency(ByRef m_lSystemCurrencyId As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetSystemCurrency"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue


        m_lReturn = m_oBusiness.GetSystemCurrency(r_lCurrencyId:=m_lSystemCurrencyId)
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            ' Do Nothing
        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function

    Private Function GetBankGuaranteeDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBankGuaranteeDetails"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue



        m_lReturn = m_oBusiness.GetBankGuaranteeDetails(vBankGuaranteeDetails:=m_vBankGuaranteeDetails, vPartyCnt:=m_vPartyCnt)
        If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
            ' Do Nothing
        ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "GetBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
            m_lReturn = CType(BuildArrayIndex(), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "BuildArrayIndex Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Public Function GetBusiness() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"


        Try



        result = gPMConstants.PMEReturnCode.PMTrue

        If PartyCnt <> 0 Or AccountId <> 0 Then
            'Get Party Bank Details
            m_lReturn = GetBankGuaranteeDetails()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = CType(GetSystemCurrency(m_lSystemCurrencyId:=m_lSystemCurrencyId), gPMConstants.PMEReturnCode)
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
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


    Public Function UpdateBankGuaranteeDetails() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "UpdateBankGuaranteeDetails"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        m_lReturn = CType(FilterArrayForChanges(oFilteredArray:=m_vBankGuaranteeDetails), gPMConstants.PMEReturnCode)
        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "FilterArrayForChanges Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        m_lReturn = m_oBusiness.UpdateBankGuaranteeDetails(vPartyCnt:=m_vPartyCnt, vBankGuaranteeDetails:=m_vBankGuaranteeDetails)

        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "UpdateBankGuaranteeDetails Failed", gPMConstants.PMELogLevel.PMLogError)
        End If


        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here
        Finally

        End Try
        Return result
    End Function

    Private Function SortListView(ByVal v_iIndex As Integer) As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "SortListView"
        Try

        result = gPMConstants.PMEReturnCode.PMTrue

        ' Tell it that it's not sorted
        ListViewHelper.SetSortedProperty(lvwBGList, False)

        ' Set the column to sort on
        ListViewHelper.SetSortKeyProperty(lvwBGList, v_iIndex)

        ' Swap the ascending/descending around
        If ListViewHelper.GetSortOrderProperty(lvwBGList) = SortOrder.Ascending Then
            ListViewHelper.SetSortOrderProperty(lvwBGList, SortOrder.Descending)
        Else
            ListViewHelper.SetSortOrderProperty(lvwBGList, SortOrder.Ascending)
        End If

        ' Tell it that it's now sorted
        ListViewHelper.SetSortedProperty(lvwBGList, True)



        Catch ex As Exception

        ' DO Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

        ' If you want to rollback a transaction or something, do it here
        Finally




        End Try
        Return result
    End Function


    Private Sub UserControl_InitProperties()
        m_bViewMode = False
    End Sub

    'Load property values from storage


    'developer guide no. 1 (no solution)
    Private Sub UserControl_ReadProperties(ByRef PropBag As Object)


        m_bViewMode = CBool(PropBag.ReadProperty("ViewMode", False))
    End Sub


    'Write property values to storage


    'developer guide no. 1 (no solition)
    Private Sub UserControl_WriteProperties(ByRef PropBag As Object)

        PropBag.WriteProperty("ViewMode", m_bViewMode, False)
    End Sub

    'Private Sub uctBankGuarenteeControl_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    'End Sub

    Private Sub lvwBGList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvwBGList.SelectedIndexChanged

        If Not IsNothing(lvwBGList.FocusedItem) Then
            m_lListSelectedItem = lvwBGList.FocusedItem.Index + 1 - 1
            If m_iTask <> gPMConstants.PMEComponentAction.PMView Then
                If gPMFunctions.ToSafeInteger(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, SelectedArrayIndexOnTag)) = MainModule.BGStatus.Active Then
                    cmdBGEdit.Enabled = True
                    cmdBGDelete.Enabled = True
                ElseIf gPMFunctions.ToSafeInteger(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, SelectedArrayIndexOnTag)) = MainModule.BGStatus.Deleted Then
                    cmdBGDelete.Enabled = True
                    cmdBGEdit.Enabled = False
                Else
                    cmdBGEdit.Enabled = False
                    cmdBGDelete.Enabled = False
                End If

                If gPMFunctions.ToSafeInteger(m_vBankGuaranteeDetails(MainModule.ENBankGuarantee.BGStatusId, SelectedArrayIndexOnTag)) = MainModule.BGStatus.Issued Then
                    cmdInvoke.Enabled = True
                    cmdBGEdit.Enabled = False
                    cmdBGDelete.Enabled = False
                Else
                    cmdInvoke.Enabled = False
                End If
            End If
        End If
    End Sub
End Class
