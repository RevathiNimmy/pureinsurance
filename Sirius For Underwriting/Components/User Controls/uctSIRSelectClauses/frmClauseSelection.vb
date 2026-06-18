Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Gui
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
'developer guide no. 129
Imports SharedFiles
Partial Friend Class frmClauseSelection
    Inherits System.Windows.Forms.Form
    '***********************************************************************************************
    'Created By     :   Arul Stephen
    'Date           :   02-Sep-2008
    'History        :   It is an New File
    '***********************************************************************************************

    '************************************************************************************************
    'Variable Declaration
    '************************************************************************************************
    Private Const ACClass As String = "uctSelectClauseControl"

    Private m_oBusiness As Object
    Private m_iLanguageID As Integer
    Private m_iSourceID As Integer
    Private m_iUserId As Integer
    Private m_bIsInitialised As Boolean
    Private m_vSelectClause(,) As Object
    Private m_vSelectedClauses(,) As Object
    Private m_vGetAllClausesAttachedToBranches(,) As Object
    Private m_vBranchList(,) As Object
    Private m_vMatchedBranches(,) As Object
    Private m_vSelectedBranchList(,) As Object
    Private m_bPropertiesNotSame As Boolean
    Private m_bDefaultClause As Boolean
    Private m_bSavedOrNot As Boolean
    Private m_iTask As Integer
    Private m_lStatus As Integer
    Private m_lReturn As Integer
    Private m_lSystemCurrencyId As Integer
    Private m_lProductID As Integer
    Private m_lRiskID As Integer
    Private m_lClauseID As Integer
    Private m_sCaption As String = ""
    Private m_sUniqueId As String = ""
    Private m_sScreenHierarchy As String = ""


    '*************************************************************************************************
    'Properties
    '*************************************************************************************************

    Public WriteOnly Property SystemCurrencyId() As Integer
        Set(ByVal Value As Integer)
            m_lSystemCurrencyId = Value
        End Set
    End Property


    Public Property Task() As Integer
        Get
            Return m_iTask
        End Get
        Set(ByVal Value As Integer)
            m_iTask = Value
        End Set
    End Property


    Public Property SelectedClauses() As Object
        Get
            Return VB6.CopyArray(m_vSelectedClauses)
        End Get
        Set(ByVal Value As Object)
            m_vSelectedClauses = Value
        End Set
    End Property


    Public Property GetAllClausesAttachedToBranches() As Object
        Get
            Return VB6.CopyArray(m_vGetAllClausesAttachedToBranches)
        End Get
        Set(ByVal Value As Object)
            m_vGetAllClausesAttachedToBranches = Value
        End Set
    End Property


    Public Property ClauseId() As Integer
        Get
            Return m_lClauseID
        End Get
        Set(ByVal Value As Integer)
            m_lClauseID = Value
        End Set
    End Property


    Public Property ProductId() As Integer
        Get
            Return m_lProductID
        End Get
        Set(ByVal Value As Integer)
            m_lProductID = Value
        End Set
    End Property
    Public Property RiskId() As Integer
        Get
            Return m_lRiskID
        End Get
        Set(ByVal Value As Integer)
            m_lRiskID = Value
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

    '*************************************************************************************************
    ' Business Started
    '*************************************************************************************************

    Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click

        Const kMethodName As String = "cmdApply_Click"
        Try



            'PN63416
            m_lReturn = Validate_Renamed()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then

                MessageBox.Show("Warning: Clause(s) have not been linked to a branch." & Strings.Chr(13) & Strings.Chr(10) & _
                            "Please check and correct configuration", ACApp, MessageBoxButtons.OK, MessageBoxIcon.Error)
                Exit Sub

            End If

            m_lReturn = DelSelectedClauses()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "DelSelectedClauses Failed to delete the clauses(s)", gPMConstants.PMELogLevel.PMLogError)
            End If
            cmdOk.Enabled = True



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)


        Finally


        End Try
        Exit Sub
    End Sub

    '**************************************************************************************************
    'This function will delete all the clauses that are not  attached with branches
    '**************************************************************************************************
    Function DelSelectedClauses() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "DelSelectedClauses"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue
            Dim bFlag As Boolean

            bFlag = False
            For lBranchCount As Integer = 0 To lvwBranches.Items.Count - 1
                If lvwBranches.Items.Item(lBranchCount).Checked Then
                    bFlag = True
                    Exit For
                End If
            Next

            If bFlag Then
                m_lReturn = UpdateSelectedClauses()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "UpdateSelectedClauses Failed to update the clauses", gPMConstants.PMELogLevel.PMLogError)
                End If

            Else

                m_lReturn = m_oBusiness.DelSelectedClausesProperties(v_lClauseType:=m_lClauseID, v_lRisk_Type_Id:=m_lRiskID, v_lProduct_Type_Id:=m_lProductID, v_vSelectedClauses:=m_vSelectClause, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)

                If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                    gPMFunctions.RaiseError(kMethodName, "There is no record to delete, DelSelectedClauses failed ", gPMConstants.PMEReturnCode.PMNotFound)
                ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "DelSelectedClauses Failed to delete the record(s)", gPMConstants.PMELogLevel.PMLogError)
                End If
            End If

            cmdApply.Enabled = False
            cmdOk.Enabled = True

        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=gPMConstants.PMEReturnCode.PMFalse, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally
        End Try
        Return result
    End Function

    Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
        Const kMethodName As String = "cmdCancel_Click"
        Try


            m_sCaption = CStr(iPMFunc.GetResData(iLangID:=m_iLanguageID, lId:=kRegKeyConstCancel, iDataType:=gPMConstants.PMEResourseFileDataType.PMResString, bResFile:=My.Resources.ResourceManager))

            m_lReturn = MessageBox.Show(m_sCaption, "Clause Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
            If m_lReturn = kYesNoButton Then
                Me.Close()
            Else
                Exit Sub
            End If



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=cmdCancel.Focused, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Exit Sub
    End Sub


    Private Sub cmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdOk.Click

        Const kMethodName As String = "cmdOk_Click"
        Try


            m_oBusiness.Dispose()

            Me.Close()



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=cmdOk.Focused, excep:=ex)

        Finally


        End Try
        Exit Sub
    End Sub


    Private Sub frmClauseSelection_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
        Const kMethodName As String = "Form_Load"
        Try



            m_vSelectClause = VB6.CopyArray(m_vSelectedClauses)
            m_vGetAllClausesAttachedToBranches = VB6.CopyArray(m_vGetAllClausesAttachedToBranches)

            m_lReturn = PopulateClausesDetailsList()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "PopulateClausesDetailsList Failed to load the Clauses", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = Initialise()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetBusiness()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed to do the main business", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception
            ' Do Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)


        Finally

            ' Set the mouse pointer to normal.
            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        End Try
        Exit Sub
    End Sub

    Public Function Initialise() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Initialise"
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
            Dim temp_m_oBusiness As Object
            m_lReturn = g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRSelectClauses.Business", vInstanceManager:=gPMConstants.PMGetViaClientManager)
            m_oBusiness = temp_m_oBusiness
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRSelectClauses.Business Failed", gPMConstants.PMELogLevel.PMLogError)
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
    '**********************************************************************************************
    'This method is used to fetch all the branches from source table and fetch the selected braches
    '**********************************************************************************************
    Private Function GetBusiness() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetBusiness"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            m_lReturn = GetBranches()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBranches Failed to fetch the branches", gPMConstants.PMELogLevel.PMLogError)
            End If

            m_lReturn = GetSelectedClauses()
            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSelectedClauses Failed to fetch the selected clauses", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function
    '**************************************************************************************************
    'This function will save all the clauses along with branches attached
    '**************************************************************************************************
    Private Function UpdateSelectedClauses() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "UpdateSelectedClauses"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim bDefaultFlag As Boolean

            ReDim m_vSelectedBranchList(MainModule.ENSelectClause.Description, kSelectClauseDefaultIndex)
            Dim lRecordCount As Integer

            For lBranchCount As Integer = 0 To lvwBranches.Items.Count - 1
                If lvwBranches.Items.Item(lBranchCount).Checked Then
                    ReDim Preserve m_vSelectedBranchList(MainModule.ENSelectClause.Description, lRecordCount)

                    m_vSelectedBranchList(MainModule.ENSelectClause.id, lRecordCount) = Convert.ToString(lvwBranches.Items.Item(lBranchCount).Tag)
                    m_vSelectedBranchList(MainModule.ENSelectClause.code, lRecordCount) = lvwBranches.Items.Item(lBranchCount).Name
                    m_vSelectedBranchList(MainModule.ENSelectClause.Description, lRecordCount) = lvwBranches.Items.Item(lBranchCount).Text
                    lRecordCount += 1
                End If
            Next


            If chkDefault.CheckState = CheckState.Checked Then
                bDefaultFlag = True
            ElseIf (chkDefault.CheckState = CheckState.Unchecked) Then
                bDefaultFlag = False
            End If


            m_lReturn = m_oBusiness.UpdateSelectedClausesProperties(v_lClauseType:=m_lClauseID, v_lRisk_Type_Id:=m_lRiskID, v_lProduct_Type_Id:=m_lProductID, v_vSelectedClauses:=m_vSelectClause, v_vBranches:=m_vSelectedBranchList, v_bDefaultClause:=bDefaultFlag, v_sUniqueId:=m_sUniqueId, v_sScreenHierarchy:=m_sScreenHierarchy)


            If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue And m_lReturn <> gPMConstants.PMEReturnCode.PMNotFound Then
                gPMFunctions.RaiseError(kMethodName, "UpdateSelectedClausesProperties Failed to update the records", gPMConstants.PMELogLevel.PMLogError)
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here
        Finally



        End Try
        Return result
    End Function
    '**************************************************************************************************
    'This function is used to get all the branches from source table
    '**************************************************************************************************
    Private Function GetBranches() As Integer
        Dim result As Integer = 0
        Const kMethodName As String = "GetBranches"
        Try

            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetAllBranches(r_vReturnValues:=m_vBranchList)

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetBranches Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then
                m_lReturn = PopulateBranchesDetailsList()
                If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                    gPMFunctions.RaiseError(kMethodName, "Populating the branches Failed", gPMConstants.PMELogLevel.PMLogError)
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

    Private Function GetSelectedClauses() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "GetSelectedClauses"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue


            m_lReturn = m_oBusiness.GetSelectedClausesProperties(v_lClauseType:=m_lClauseID, v_lRisk_Type_Id:=m_lRiskID, v_lProduct_Type_Id:=ProductId, v_vSelectedClauses:=m_vSelectClause, r_vBranches:=m_vMatchedBranches, r_bDefaultClause:=m_bDefaultClause, r_bPropertiesNotSame:=m_bPropertiesNotSame)

            If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
                ' Do Nothing
            ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                gPMFunctions.RaiseError(kMethodName, "GetSelectedClausesProperties Failed", gPMConstants.PMELogLevel.PMLogError)
            ElseIf m_lReturn = gPMConstants.PMEReturnCode.PMTrue Then


                If m_vMatchedBranches Is Nothing And m_bPropertiesNotSame Then
                    For lBranchCount As Integer = 0 To lvwBranches.Items.Count - 1
                        lvwBranches.Items.Item(lBranchCount).Checked = False
                    Next
                    If Not m_bDefaultClause Then
                        chkDefault.CheckState = CheckState.Unchecked
                    Else
                        chkDefault.CheckState = CheckState.Checked
                    End If

                    framBranch.Text = kBranchesAccessDifferentCaption
                    framBranch.Font = VB6.FontChangeItalic(framBranch.Font, True)

                Else
                    For lMatchedBranchCount As Integer = m_vMatchedBranches.GetLowerBound(kDefaultRowIndex - 1) To m_vMatchedBranches.GetUpperBound(kDefaultRowIndex - 1)
                        For lBranchCount As Integer = 0 To lvwBranches.Items.Count - 1

                            If m_vMatchedBranches(kSelectClauseDefaultIndex, lMatchedBranchCount).Equals(Convert.ToString(lvwBranches.Items.Item(lBranchCount).Tag)) Then
                                lvwBranches.Items.Item(lBranchCount).Checked = True
                            End If
                        Next
                    Next
                    If m_bDefaultClause Then
                        chkDefault.CheckState = CheckState.Checked
                    Else
                        chkDefault.CheckState = CheckState.Unchecked
                    End If

                    framBranch.Text = kBranchesAccessCaption


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
    Private Function PopulateClausesDetailsList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateClausesDetailsList"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oListItem As ListViewItem
            Dim olistsubitem As ListViewItem.ListViewSubItem


            If gPMFunctions.IsArrayEmpty(m_vSelectClause) Then
                Return result
            End If

            'Set max rows to number of addresses - though must be at least 5
            lvwSelectClause.Items.Clear()

            For lRecordCount As Integer = m_vSelectClause.GetLowerBound(kDefaultRowIndex - 1) To m_vSelectClause.GetUpperBound(kDefaultRowIndex - 1)

                If m_vSelectClause(MainModule.ENSelectClause.id, lRecordCount) <> gPMConstants.PMEReturnCode.PMNotFound Then

                    oListItem = lvwSelectClause.Items.Add(CStr(m_vSelectClause(MainModule.ENSelectClause.code, lRecordCount)).Trim())
                    ListViewHelper.GetListViewSubItem(oListItem, kSelectClauseColHIndexDescription).Text = CStr(m_vSelectClause(MainModule.ENSelectClause.Description, lRecordCount)).Trim()
                    oListItem.Tag = CStr(m_vSelectClause(MainModule.ENSelectClause.id, lRecordCount)).Trim()

                End If

            Next lRecordCount




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally




        End Try
        Return result
    End Function


    Private Function PopulateBranchesDetailsList() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "PopulateBranchesDetailsList"
        Try



            result = gPMConstants.PMEReturnCode.PMTrue

            Dim oListItem As ListViewItem
            Dim olistsubitem As ListViewItem.ListViewSubItem

            If gPMFunctions.IsArrayEmpty(m_vBranchList) Then
                Return result
            End If

            lvwBranches.Items.Clear()

            For lRowCount As Integer = m_vBranchList.GetLowerBound(kDefaultRowIndex - 1) To m_vBranchList.GetUpperBound(kDefaultRowIndex - 1)
                If m_vBranchList(MainModule.ENSelectClause.id, lRowCount) <> gPMConstants.PMEReturnCode.PMNotFound Then
                    oListItem = lvwBranches.Items.Add(CStr(m_vBranchList(MainModule.ENSelectClause.Description, lRowCount)).Trim())
                    oListItem.Tag = CStr(m_vBranchList(MainModule.ENSelectClause.id, lRowCount)).Trim()
                    'Start Girija - PN 54101
                    'oListItem.Key = Trim(m_vBranchList(ENSelectClause.code, lRowCount))
                    'End Girija - PN 54101
                End If

            Next lRowCount




        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
    'Start Girija - PN 54272
    Private isInitializingComponent As Boolean
    Private Sub frmClauseSelection_Resize(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Resize
        If isInitializingComponent Then
            Exit Sub
        End If


        Try
            '(Start)-(Arul Stephen)-(PN 54272)

            framBranch.Height = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 7.6)
            framBranch.Left = Me.Width - VB6.TwipsToPixelsX(3390)
            lvwBranches.Height = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 3.8)


            lblDefaultclause.Top = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 4.4)

            chkDefault.Top = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 4.9)
            framClause.Width = framBranch.Left - VB6.TwipsToPixelsX(100)
            framClause.Height = Me.Height - VB6.TwipsToPixelsY(425)

            lvwSelectClause.Width = framClause.Width - VB6.TwipsToPixelsX(200)
            lvwSelectClause.Height = framClause.Height - VB6.TwipsToPixelsY(300)
            lvwSelectClause.Columns.Item(0).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSelectClause.Width) / 2.5 + 1))
            lvwSelectClause.Columns.Item(1).Width = CInt(VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(lvwSelectClause.Width) / 1.5 + 1))

            cmdOk.Left = framBranch.Left
            cmdOk.Top = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 9.1)

            cmdCancel.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdOk.Left) + VB6.PixelsToTwipsX(cmdOk.Width) + VB6.PixelsToTwipsY(cmdOk.Height) / 4)
            cmdCancel.Top = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 9.1)

            cmdApply.Left = VB6.TwipsToPixelsX(VB6.PixelsToTwipsX(cmdOk.Left) + VB6.PixelsToTwipsX(cmdOk.Width) + VB6.PixelsToTwipsX(cmdCancel.Width) + VB6.PixelsToTwipsY(cmdCancel.Height) / 4 + VB6.PixelsToTwipsY(cmdOk.Height) / 4)
            cmdApply.Top = Me.Height - VB6.TwipsToPixelsY(VB6.PixelsToTwipsY(Me.Height) / 9.1)
            '(End)-(Arul Stephen)-(PN 54272)

        Catch exc As System.Exception
            NotUpgradedHelper.NotifyNotUpgradedElement("Resume in On-Error-Resume-Next Block")
        End Try
    End Sub
    'End Girija - PN 54272

    Private Sub lvwBranches_ItemCheck(ByVal eventSender As Object, ByVal eventArgs As ItemCheckEventArgs) Handles lvwBranches.ItemCheck
        Dim Item As ListViewItem = lvwBranches.Items(eventArgs.Index)
        cmdApply.Enabled = True
        cmdOk.Enabled = False
    End Sub

    Private Sub lvwSelectClause_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles lvwSelectClause.Click
        lvwSelectClause.FocusedItem.Selected = False

        'developer guide no. no solution 12
        lvwSelectClause.SelectedItems.Item(0).ForeColor = Color.White
    End Sub

    Private Sub lvwSelectClause_ItemClick(ByVal Item As ListViewItem)
        lvwSelectClause.FocusedItem.Selected = False

        'developer guide no. no solution 12
        lvwSelectClause.SelectedItems.Item(0).ForeColor = Color.White
    End Sub

    Private Function Validate_Renamed() As Integer

        Dim result As Integer = 0
        Const kMethodName As String = "Validate"
        Try



            result = gPMConstants.PMEReturnCode.PMFalse

            If chkDefault.CheckState = CheckState.Checked Then
                For lBranchCount As Integer = 0 To lvwBranches.Items.Count - 1
                    If lvwBranches.Items.Item(lBranchCount).Checked Then
                        Return gPMConstants.PMEReturnCode.PMTrue
                    End If
                Next
            Else
                result = gPMConstants.PMEReturnCode.PMTrue
            End If



        Catch ex As Exception

            ' DO Not Call any functions before here or the error will be lost
            iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=result, excep:=ex)

            ' If you want to rollback a transaction or something, do it here

        Finally



        End Try
        Return result
    End Function
End Class
