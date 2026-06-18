Option Strict Off
Option Explicit On
Imports Artinsoft.VB6.Utils
Imports Microsoft.VisualBasic
Imports System
Imports System.Windows.Forms
Imports SharedFiles
Partial Friend Class frmPickDocumentTemplate
	Inherits System.Windows.Forms.Form
	'********************************************************************************
	'Created By:Arul Stephen
	'Tech Spec:TechSpec WR6ClauseGrouping.doc
	'********************************************************************************
	
	'************************************************************************************
	'Variable Declaration
	'************************************************************************************
	
	Private Const ACClass As String = "iSIRPickDocTemplate"
	
	Private m_bIsInitialised As Boolean
	Private m_vProductList As Object
	Private m_vSelectedClasues As Object
	Private m_vProductListWithBranches( ,  ) As Object
	Private m_vSelectedItems() As Object
	Private m_vResultArray As Object
	Private m_sCallingAppName As String = ""
    Private m_lStatus As gPMConstants.PMEReturnCode = gPMConstants.PMEReturnCode.PMCancel
	Private m_lErrorNumber As Integer
	Private m_lLanguageID As Integer
	Private m_lSourceID As Integer
	Private m_lProductID As Integer
	Private m_lUserId As Integer
	Private m_lRiskId As Integer
	Private m_lClauseId As Integer
	Private m_vDocumentTemplate As Object
	Private m_lNavigate As Integer
	Private m_lProcessMode As Integer
	Private m_lTask As gPMConstants.PMEComponentAction
	Private m_oBusiness As Object
	Private m_lReturn As Integer
	
	'Start Arul -Bug Fixing PN 55217
	Private m_sColumnName As String = ""
	Private m_sPropertyName As String = ""
	'End Arul -Bug Fixing PN 55217
	
	Private m_bSearchable As Boolean
	
	'************************************************************************************
	'Properties
	'************************************************************************************
	'Start(Sriram P)PN60826
	Private m_vDefaultClauses As Object
	Public Property DefaultClauses() As Object
		Get
			Return m_vDefaultClauses
		End Get
		Set(ByVal Value As Object)


			m_vDefaultClauses = Value
		End Set
    End Property

    Private m_sCoverToDate As String
    Public Property CoverToDate() As String
        Get
            Return m_sCoverToDate
        End Get
        Set(ByVal Value As String)
            m_sCoverToDate = Value
        End Set
    End Property


	'End(Sriram P)PN60826
	Public ReadOnly Property ErrorNumber() As Integer
		Get
			Return m_lErrorNumber
		End Get
	End Property
	
	Public WriteOnly Property CallingAppName() As String
		Set(ByVal Value As String)
			m_sCallingAppName = Value
		End Set
	End Property
	
	
	Public Property Status() As Integer
		Get
			Return m_lStatus
		End Get
		Set(ByVal Value As Integer)
			m_lStatus = Value
		End Set
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
	
	
	Public Property Task() As Integer
		Get
			Return m_lTask
		End Get
		Set(ByVal Value As Integer)
			m_lTask = Value
		End Set
	End Property
	
	
	Public Property SourceId() As Integer
		Get
			Return m_lSourceID
		End Get
		Set(ByVal Value As Integer)
			m_lSourceID = Value
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
	
	
	Public Property ClauseId() As Integer
		Get
			Return m_lClauseId
		End Get
		Set(ByVal Value As Integer)
			m_lClauseId = Value
		End Set
	End Property
	
	
	Public Property RiskId() As Integer
		Get
			Return m_lRiskId
		End Get
		Set(ByVal Value As Integer)
			m_lRiskId = Value
		End Set
	End Property
	
	
	Public Property DocumentTemplate() As Object
		Get
			Return m_vDocumentTemplate
		End Get
		Set(ByVal Value As Object)


			m_vDocumentTemplate = Value
		End Set
	End Property
	
	'Start Arul -Bug Fixing PN 55217
	Public Property PropertyName() As String
		Get
			Return m_sPropertyName
		End Get
		Set(ByVal Value As String)
			m_sPropertyName = Value
		End Set
	End Property
	Public Property ColumnName() As String
		Get
			Return m_sColumnName
		End Get
		Set(ByVal Value As String)
			m_sColumnName = Value
		End Set
	End Property
	'End Arul -Bug Fixing PN 55217
	
	Public WriteOnly Property Searchable() As Boolean
		Set(ByVal Value As Boolean)
			m_bSearchable = Value
		End Set
	End Property
	
	'***************************************************************************************'
	' Name          :   GetBusiness
	' Description   :   To do the real business
	' Created by    :   Arul Stephen
	'****************************************************************************************'
	
	Public Function GetBusiness() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "GetBusiness"
		Try
		
		
        result = PMEReturnCode.PMTrue
		
		m_lReturn = LoadClauses()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "LoadClauses method failed to get the clauses attached", PMELogLevel.PMLogError)
        End If
		
		m_lReturn = SetPickList()
        If m_lReturn <> PMEReturnCode.PMTrue Then
            gPMFunctions.RaiseError(kMethodName, "SetPickList failed to get the selected records ", PMELogLevel.PMLogError)
        End If
		
		
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
	
	'*********************************************************************************'
	' Name          :   cmdApply_Click
	' Description   :   To Populate the result Array
	' Created by    :   Arul Stephen
	'*********************************************************************************'
	
	Private Sub cmdApply_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdApply.Click
		
		Const kMethodName As String = "cmdApply_Click"
		Try
		
		
		
		m_lReturn = PopulateResultArray()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "populateResultArray method Failed to form  an Array", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		cmdApply.Enabled = False
		CmdOk.Enabled = True
		
		

		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

		End Try
		Exit Sub
	End Sub
	
	'*********************************************************************************'
	' Name          :   cmdCancel_Click
	' Description   :   To Terminate the process
	' Created by    :   Arul Stephen
	'*********************************************************************************'
	
	Private Sub cmdCancel_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdCancel.Click
		
		Const kMethodName As String = "cmdCancel_Click"
		Try
		
		
		m_lReturn = MessageBox.Show("Cancelling will not add or update any details." & Strings.Chr(13) & Strings.Chr(10) &  _
		            " Do you really wish to cancel?", "Clause Selection", MessageBoxButtons.YesNo, MessageBoxIcon.Information)
        If m_lReturn = MainModule.ACYestButton Then
			Status = gPMConstants.PMEReturnCode.PMCancel
			Me.Close()
		End If
		

		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

		End Try
		Exit Sub
	End Sub
	
	Private Sub cmdClear_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdClear.Click
		txtSearchCode.Text = ""
		cmdFind_Click(cmdFind, New EventArgs())
	End Sub
	
	Private Sub cmdFind_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles cmdFind.Click
		Const kMethodName As String = "cmdFind_Click"
		Try
		
		
		
		' before search adjust arrays
		m_lReturn = PopulateResultArray()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "populateResultArray method Failed to form  an Array", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' set the default list to current selection


		m_vDefaultClauses = m_vResultArray
		
		m_lReturn = GetBusiness()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed to fetch the required values from database", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		

		End Try
		Exit Sub
	End Sub
	
	'***************************************************************************************'
	' Name          :   CmdOk_Click
	' Description   :   After assigning the array to property it will terminate the process
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	
	Private Sub CmdOk_Click(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles CmdOk.Click
		Dim lCount As Integer
		
		Const kMethodName As String = "CmdOk_Click"
		Try
		
            m_vDocumentTemplate = m_vResultArray
            m_lStatus = gPMConstants.PMEReturnCode.PMFalse
            Me.Close()
		

		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

		End Try
		Exit Sub
	End Sub
	
	'***************************************************************************************'
	' Name          :   PopulateResultArray
	' Description   :   To Populate the array
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	
	Private Function PopulateResultArray() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "populateResultArray"
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Dim lRowCount As Integer
		Dim bAlreadySelected As Boolean
		Dim lPointer As Integer
		

		m_vSelectedItems = PickList.GetSelectedItems
		
		'remove all

        'developer guide no. 12
        m_vResultArray = Nothing
		
		If Not Information.IsArray(m_vSelectedItems) Then
			Return result
		End If
		
		For	Each m_vSelectedItems_item As Object In m_vSelectedItems
			lPointer = -1
			bAlreadySelected = False
			If Information.IsArray(m_vDefaultClauses) Then
				'Check presence of already chosen

				For lRowCount = m_vDefaultClauses.GetLowerBound(ACArrayRowPosition - 1) To m_vDefaultClauses.GetUpperBound(ACArrayRowPosition - 1)

                    If gPMFunctions.ToSafeString(CStr(m_vDefaultClauses(MainModule.ENSelectClause.Id, lRowCount))) = gPMFunctions.ToSafeString(CStr(m_vSelectedItems_item)) Then
                        bAlreadySelected = True
                        Exit For
                    End If
                Next lRowCount
            End If

            If Information.IsArray(m_vProductListWithBranches) Then
                lPointer = -1
                'search in current list applicable risk clauses
                For lProductCount As Integer = m_vProductListWithBranches.GetLowerBound(ACArrayRowPosition - 1) To m_vProductListWithBranches.GetUpperBound(ACArrayRowPosition - 1)
                    If m_vProductListWithBranches(MainModule.ENSelectClause.Id, lProductCount).Equals(m_vSelectedItems_item) Then
                        lPointer = lProductCount
                        Exit For
                    End If
                Next lProductCount
            End If

            If bAlreadySelected And lPointer = -1 Then
                ' if not in risk clause list
                lPointer = lRowCount
            Else
                bAlreadySelected = False
            End If

            'See if we have description to send back
            If lPointer >= 0 Then
                If Information.IsArray(m_vResultArray) Then

                    ReDim Preserve m_vResultArray(MainModule.ENSelectedClauseArray.Edited, m_vResultArray.GetUpperBound(1) + 1)
                Else
                    ReDim m_vResultArray(MainModule.ENSelectedClauseArray.Edited, 0)
                End If

                If Not bAlreadySelected Then


                    m_vResultArray(MainModule.ENSelectedClauseArray.Id, m_vResultArray.GetUpperBound(1)) = m_vProductListWithBranches(MainModule.ENSelectedClauseArray.Id, lPointer)


                    m_vResultArray(MainModule.ENSelectedClauseArray.Code, m_vResultArray.GetUpperBound(1)) = m_vProductListWithBranches(MainModule.ENSelectedClauseArray.Code, lPointer)


                    m_vResultArray(MainModule.ENSelectedClauseArray.Description, m_vResultArray.GetUpperBound(1)) = m_vProductListWithBranches(MainModule.ENSelectedClauseArray.Description, lPointer)


                    m_vResultArray(MainModule.ENSelectedClauseArray.Edited, m_vResultArray.GetUpperBound(1)) = ""
                Else



                    m_vResultArray(MainModule.ENSelectedClauseArray.Id, m_vResultArray.GetUpperBound(1)) = m_vDefaultClauses(MainModule.ENSelectedClauseArray.Id, lPointer)



                    m_vResultArray(MainModule.ENSelectedClauseArray.Code, m_vResultArray.GetUpperBound(1)) = m_vDefaultClauses(MainModule.ENSelectedClauseArray.Code, lPointer)



                    m_vResultArray(MainModule.ENSelectedClauseArray.Description, m_vResultArray.GetUpperBound(1)) = m_vDefaultClauses(MainModule.ENSelectedClauseArray.Description, lPointer)


                    If m_lClauseId = EnClauseType.RiskType > 0 Then
                        m_vResultArray(MainModule.ENSelectedClauseArray.Edited, m_vResultArray.GetUpperBound(1)) = m_vDefaultClauses(MainModule.ENSelectedClauseArray.Edited, lPointer)
                    End If
                End If

            End If
		Next m_vSelectedItems_item
		
		
		

		
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
	
	'***************************************************************************************'
	' Name          :   Form_Load
	' Description   :   To Load the page
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	

	Public Sub frmPickDocumentTemplate_Load(ByVal eventSender As Object, ByVal eventArgs As EventArgs) Handles MyBase.Load
		
		Const kMethodName As String = "Load"
		Try
		
		
		
		m_lReturn = Initialise()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "Initialise method Failed to create the instance", gPMConstants.PMELogLevel.PMLogError)
			Exit Sub
		End If
		
		m_lReturn = GetBusiness()
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetBusiness Failed to fetch the required values from database", gPMConstants.PMELogLevel.PMLogError)
			Exit Sub
		End If
		
		
		
		Catch ex As Exception
		
		' Do Not Call any functions before here or the error will be lost
		iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)
		
		' If you want to rollback a transaction or something, do it here
		
		Finally
		
		' Set the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
		
		
		End Try
	End Sub
    Private Const vbFormControlMenu As Integer = 0
	'***************************************************************************************'
	' Name          :   Form_QueryUnload
	' Description   :   To Unload
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	
	Private Sub frmPickDocumentTemplate_FormClosing(ByVal eventSender As Object, ByVal eventArgs As FormClosingEventArgs) Handles MyBase.FormClosing
		Dim Cancel As Integer = IIf(eventArgs.Cancel, 1, 0)
		Dim UnloadMode As Integer = CInt(eventArgs.CloseReason)
		
		' Forms query unload event.
		Const kMethodName As String = "Form_QueryUnload"
		Try
		
		
		
		
		'Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		m_oBusiness = Nothing
		'Reset the mouse pointer to normal.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseReset)



        If UnloadMode = vbFormControlMenu Then
            m_lStatus = gPMConstants.PMEReturnCode.PMCancel
        End If



		Catch ex As Exception

        ' Do Not Call any functions before here or the error will be lost
        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=False, excep:=ex)

        ' If you want to rollback a transaction or something, do it here

		Finally

        ' Set the mouse pointer to normal.
        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)


        eventArgs.Cancel = Cancel <> 0
		End Try
        Exit Sub
    End Sub
	
	'***************************************************************************************'
	' Name          :   GetAllClausesWithBranches
	' Description   :   To get the clauses that attached to the current branch
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	
	Private Function LoadClauses() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "LoadClauses"
		Try
		
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		If Me.ClauseId = MainModule.EnClauseType.ProductType Then

                m_lReturn = m_oBusiness.LoadClauses(v_lClauseType:=MainModule.EnClauseType.ProductType, v_lRiskType:=ACRiskTypeId, v_lProduct_id:=Me.ProductId, v_lBranch_Id:=Me.SourceId, r_vResultArray:=m_vProductListWithBranches, v_sCoverToDate:=m_sCoverToDate, _
                                                        v_sCode:=Trim(txtSearchCode.Text))
			
		ElseIf (Me.ClauseId = MainModule.EnClauseType.RiskType) Then 

			m_lReturn = m_oBusiness.LoadClauses(v_lClauseType:=MainModule.EnClauseType.RiskType, v_lRiskType:=Me.RiskId, v_lProduct_id:=ACProductTypeId, v_lBranch_Id:=Me.SourceId, r_vResultArray:=m_vProductListWithBranches, v_sPropertyName:=Me.PropertyName, v_sColumnName:=Me.ColumnName, v_sCode:=txtSearchCode.Text.Trim())
		End If
		
		If m_lReturn = gPMConstants.PMEReturnCode.PMNotFound Then
			' Do Nothing
		ElseIf m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then 
			gPMFunctions.RaiseError(kMethodName, "LoadClauses method Failed to fetch the clauses that are attached to Risk or Product", gPMConstants.PMELogLevel.PMLogError)
		End If
		

		
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
	
	'***************************************************************************************'
	' Name          :   Initialise
	' Description   :   To initilize the component "bSIRFindDocTemplate.Form"
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	
	Public Function Initialise() As Integer
		
		Dim result As Integer = 0
		Const kMethodName As String = "Initialise"
		Try
		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		' Check if already initialised
		If m_bIsInitialised Then
			Return result
		End If
		
		' Create an instance of the object manager.
        MainModule.g_oObjectManager = New bObjectManager.ObjectManager()
		
		' Call the initialise method.
        m_lReturn = MainModule.g_oObjectManager.Initialise(MainModule.ACApp)
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "g_oOBjectManager.Initialise Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		' If UserID is 0 assume that user cancelled logon
        If MainModule.g_oObjectManager.UserID = 0 Then
			' Exit application
			result = gPMConstants.PMEReturnCode.PMFalse
			Return result
		End If
		
		' Store the language ID from the object manager to the public variables,
		' to enable us to use them throughout the object.
        With MainModule.g_oObjectManager
			m_lLanguageID = .LanguageID

            m_lSourceID = .SourceID
            m_lUserId = .UserID
		End With
		
		' Set the mouse pointer to busy.
		iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseBusy)
		
		' Get an instance of the business object via the public object manager.
        Dim temp_m_oBusiness As Object = Nothing
        m_lReturn = MainModule.g_oObjectManager.GetInstance(temp_m_oBusiness, "bSIRFindDocTemplate.Form", gPMConstants.PMGetViaClientManager)
		m_oBusiness = temp_m_oBusiness
		If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
			gPMFunctions.RaiseError(kMethodName, "GetInstance of bSIRFindDocTemplate.Form Failed", gPMConstants.PMELogLevel.PMLogError)
		End If
		
		fraSearch.Visible = m_bSearchable
		' hold Initialised status
		m_bIsInitialised = True
		
		
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
	
	'***************************************************************************************'
	' Name          :   SetPickList
	' Description   :   To populate the value in the pick list
	' Created by    :   Arul Stephen
	'***************************************************************************************'
	
	Private Function SetPickList() As Integer
		
        Dim result As Integer = 0
		Const kMethodName As String = "SetPickList"
		Try

            Dim vResultArray(,) As Object
            Dim lNewRowCount As Long
            Dim lSelectedItemcount As Long
            Dim lInnerSelectedItemcount As Long
            Dim bDefaultClauses As Boolean

		
		
		
		result = gPMConstants.PMEReturnCode.PMTrue
		
		Dim Key As uctPickList.PickListKey


                        If m_lClauseId = EnClauseType.ProductType Then
                            If IsArray(m_vDefaultClauses) Then
                                If IsArray(m_vProductListWithBranches) = True Then
                    ' ReDim Preserve vResultArray(ACDocumentMaxId, 0)
                                    For lSelectedItemcount = LBound(m_vProductListWithBranches, 2) To UBound(m_vProductListWithBranches, 2)
                                        bDefaultClauses = False

                                        For lInnerSelectedItemcount = LBound(m_vDefaultClauses, 2) To UBound(m_vDefaultClauses, 2)
                                            If m_vProductListWithBranches(ENSelectClause.Code, lSelectedItemcount) = m_vDefaultClauses(ENSelectClause.Code, lInnerSelectedItemcount) Then
                                                bDefaultClauses = True
                                                Exit For
                                            End If

                                        Next lInnerSelectedItemcount
                                        If Not bDefaultClauses Then
                                            ReDim Preserve vResultArray(ACDocumentTypeEffectiveDate, lNewRowCount)
                                            vResultArray(ENSelectClause.Id, lNewRowCount) = m_vProductListWithBranches(ENSelectClause.Id, lSelectedItemcount)
                                            vResultArray(ENSelectClause.Code, lNewRowCount) = m_vProductListWithBranches(ENSelectClause.Code, lSelectedItemcount)
                                            vResultArray(ENSelectClause.Description, lNewRowCount) = m_vProductListWithBranches(ENSelectClause.Description, lSelectedItemcount)
                                            vResultArray(ACisDeleted, lNewRowCount) = m_vProductListWithBranches(ACisDeleted, lSelectedItemcount)
                                            vResultArray(ACDocumentTypeId, lNewRowCount) = m_vProductListWithBranches(ACDocumentTypeId, lSelectedItemcount)
                                            vResultArray(ACDocumentTypeCode, lNewRowCount) = m_vProductListWithBranches(ACDocumentTypeCode, lSelectedItemcount)
                                            vResultArray(ACDocumentTypeDecription, lNewRowCount) = m_vProductListWithBranches(ACDocumentTypeDecription, lSelectedItemcount)
                                            vResultArray(ACDocumentTypeEffectiveDate, lNewRowCount) = m_vProductListWithBranches(ACDocumentTypeEffectiveDate, lSelectedItemcount)
                                            lNewRowCount = lNewRowCount + 1
                                        End If

                                    Next lSelectedItemcount
                                End If

                                m_lReturn = PickList.LoadFromArray(vResultArray, m_vDefaultClauses)

                            Else
                                m_lReturn = PickList.LoadFromArray(m_vProductListWithBranches)


                            End If
                        ElseIf m_lClauseId = EnClauseType.RiskType Then

                            If IsArray(m_vDefaultClauses) Then
                                m_lReturn = PickList.LoadFromArray(m_vProductListWithBranches, m_vDefaultClauses)
                            Else
                                m_lReturn = PickList.LoadFromArray(m_vProductListWithBranches)
                            End If

                        End If

                        If m_lReturn <> gPMConstants.PMEReturnCode.PMTrue Then
                            gPMFunctions.RaiseError(kMethodName, " PickList Failed to get the selected clauses", gPMConstants.PMELogLevel.PMLogError)
                            iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)
                        End If




		Catch ex As Exception

                        ' Do Not Call any functions before here or the error will be lost
                        iPMFunc.LogError(v_sClass:=ACClass, v_sMethod:=kMethodName, r_lFunctionReturn:=Initialise(), excep:=ex)

                        ' If you want to rollback a transaction or something, do it here

		Finally

                        ' Set the mouse pointer to normal.
                        iPMFunc.SetMousePointer(gPMConstants.PMEMousePointerStatus.PMMouseNormal)

		End Try
		Return result
    End Function
	
	
	
	Private Sub PickList_Change(ByVal Sender As Object, ByVal e As EventArgs) Handles PickList.Change
		CmdOk.Enabled = False
		cmdApply.Enabled = True
	End Sub
End Class
